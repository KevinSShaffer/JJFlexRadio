//#define DoOpusHere
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using JJPortaudio;
using JJTrace;
using POpusCodec;

namespace testAudioHelper
{
    public partial class Form1 : Form
    {
        private string cfgFile;
        private JJPortaudio.Devices dev;
        private JJPortaudio.Devices.Device outDevice, inDevice;

        public Form1()
        {
            InitializeComponent();

            string cfgDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\TestAudioHelper";
            Directory.CreateDirectory(cfgDir);
            cfgFile = cfgDir + "\\cfg.xml";

            Tracing.TheSwitch.Level = TraceLevel.Info;
            if (Debugger.IsAttached)
            {
                //Tracing.ToConsole = true;
            }
            else
            {
                string trcFile = cfgFile.Substring(0, cfgFile.LastIndexOf('\\')) + "\\audioTrace.txt";
                Tracing.TraceFile = trcFile;
            }
            Tracing.On = true;
            Tracing.TraceLine("Audio Tracing on " + DateTime.Now.ToString() + " level=" + Tracing.TheSwitch.Level.ToString());
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                dev = new Devices(cfgFile);
                dev.Setup();
                outDevice = dev.GetConfiguredDevice(Devices.DeviceTypes.output, true);
                inDevice = dev.GetConfiguredDevice(Devices.DeviceTypes.input, true);
                Audio.Initialize(inDevice, outDevice);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
                DialogResult = DialogResult.Abort;
                return;
            }

            DialogResult = DialogResult.None;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Audio.Terminate();
            Tracing.On = false;
        }

        private Thread startThread(ThreadStart proc, string name)
        {
            Thread rv = new Thread(proc);
            rv.Name = name;
            rv.Start();
            Thread.Sleep(0);
            return rv;
        }

        private void sendStopCommand(string pipeName, bool useOpus, BinaryReader br)
        {
#if zero
            sendAudioCommand(AdminDirectives.stopChannel, 0, pipeName);
            try
            {
                short len;
                while ((len = br.ReadInt16()) != -1)
                {
                    if (useOpus)
                    {
                        byte[] buf = new byte[len];
                        br.Read(buf, 0, len);
                    }
                    else
                    {
                        while (len-- > 0) br.ReadSingle();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageTextBox.Text += "flush:" + ex.Message;
            }
#endif
        }

        private const string wavFile = @"c:\users\jjs\dropbox\documents\JJPortAudioTest\audfile.dat";
        private const int wavBufSZ = 4800;
        // This button doesn't allow multiple channels to play.
        private void WavButton_Click(object sender, EventArgs e)
        {
            Thread t = startThread(wavProc, "wavProc");
            t.Join();
        }
        private void wavProc()
        {
            #region copy
            JJAudioStream audStream = new JJAudioStream();
            if (!audStream.OpenAudio(Devices.DeviceTypes.output, 24000))
            {
                MessageBox.Show("open failed", "error", MessageBoxButtons.OK);
                return;
            }

            // Audio file.
            using (Stream inStream = File.Open(wavFile, FileMode.Open))
            {
                using (BinaryReader br = new BinaryReader(inStream))
                {
                    float[] buf = new float[wavBufSZ];
                    int offset = 0;
                    try
                    {
                        if (!audStream.StartAudio()) throw new Exception("audio device didn't start");
                        while (true)
                        {
                            buf[offset++] = br.ReadSingle(); // exit the while on EOS.
                            if (offset == wavBufSZ)
                            {
                                audStream.Write(buf);
                                offset = 0;
                            }
                        }
                    }
                    catch (EndOfStreamException)
                    {
                        // if unfinished buffer
                        if (offset > 0)
                        {
                            audStream.Write(buf);
                        }
                        audStream.Flush();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
                    }
                }
            }
            audStream.StopAudio();
            audStream.Close();
            #endregion
        }

        private const string opusFile = @"c:\users\jjs\dropbox\documents\JJPortAudioTest\opusout.dat";
        //private const string opusFile = @"C:\Users\jjs\AppData\Local\Temp\tmpcab8.tmp";
        private void OpusButton_Click(object sender, EventArgs e)
        {
            Thread t = startThread(opusProc, "opusProc");
            t.Join();
        }
        private void opusProc()
        {
            JJAudioStream audioStream = new JJAudioStream();
            audioStream.OpenOpus(Devices.DeviceTypes.output, opusInputRate);

            // opus input file.
            // Each record must be of the form short length, byte[] opus data.
            using (Stream inStream = File.Open(opusFile, FileMode.Open))
            {
                using (BinaryReader br = new BinaryReader(inStream))
                {
                    try
                    {
                        if (!audioStream.StartAudio()) throw new Exception("opus stream didn't start");
                        while (true)
                        {
                            short len = br.ReadInt16();
                            byte[] buf = new byte[len];
                            br.Read(buf, 0, len);
                            audioStream.WriteOpus(buf);
                        }
                    }
                    catch (EndOfStreamException)
                    {
                        // len -1 signals end-of-stream
                        audioStream.Flush();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
                    }
                    audioStream.StopAudio();
                }
            }
            audioStream.Close();
        }

        private const int inputRepeats = 1;
        #region Input
        private void InputButton_Click(object sender, EventArgs e)
        {
            Thread t = startThread(inputProc, "inputProc");
            t.Join();
        }
        private BinaryWriter wavWriter;
        private int byteCount = 0;
        private bool inputDone = false;
        private void inputProc()
        {
            bool keepGoing = true;
            MessageTextBox.Text = "";
            string tempName = Path.GetTempFileName();
            JJAudioStream inStream = new JJAudioStream();
            inStream.OpenAudio(Devices.DeviceTypes.input, 24000, writeWav);

            int rpt = 0;
            do
            {
                // temporary stream
                using (Stream tempStream = File.Open(tempName,
                    (File.Exists(tempName)) ? FileMode.Append : FileMode.Create))
                {
                    using (wavWriter = new BinaryWriter(tempStream))
                    {
                        inputDone = false;
                        try
                        {
                            // Start input
                            if (!inStream.StartAudio()) throw new Exception("audio stream didn't start");
                            Thread.Sleep(5000);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace, "Error", MessageBoxButtons.OK);
                            keepGoing = false;
                        }
                        // end input
                        inStream.StopAudio();
                        while (!inputDone) Thread.Sleep(25);
                        //MessageTextBox.Text += "audio floats out:" + byteCount + "\r\n";
                    }
                }
                if (inputRepeats > 0)
                {
                    Console.Beep();
                    if (rpt < inputRepeats)
                    {
                        Thread.Sleep(2000);
                        Console.Beep();
                    }
                }
            } while (rpt++ < inputRepeats);
            inStream.Close();

            if (!keepGoing) goto inWavDone;

            JJAudioStream outStream = new JJAudioStream();
            outStream.OpenAudio(Devices.DeviceTypes.output, 24000);
            // read from temp file
            using (Stream tempStream = File.Open(tempName, FileMode.Open))
            {
                using (BinaryReader br = new BinaryReader(tempStream))
                {
                    byteCount = 0;
                    float[] buf = new float[1024];
                    int offset = 0;
                    try
                    {
                        // Start output
                        if (!outStream.StartAudio()) throw new Exception("output stream didn't start");
                        while (true)
                        {
                            buf[offset] = br.ReadSingle(); // could get EOF
                            offset++;
                            if (offset == buf.Length)
                            {
                                outStream.Write(buf);
                                byteCount += buf.Length;
                                offset = 0;
                            }
                        }
                    }
                    catch (EndOfStreamException)
                    {
                        if (offset > 0)
                        {
                            byteCount += offset;
                            while (offset < buf.Length) buf[offset++] = 0f;
                            outStream.Write(buf);
                        }
                        outStream.Flush();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace, "Error", MessageBoxButtons.OK);
                    }
                    // end output
                    outStream.StopAudio();
                    outStream.Close();
                    //.Text += "audio floats out:" + byteCount + "\r\n";
                }
            }

            inWavDone:
            //int i = 1;
            File.Delete(tempName);
        }
        private void writeWav(float[] data)
        {
            int len = data.Length;
            inputDone = (len == 0);
            if (!inputDone)
            {
                for (int i = 0; i < len; i++)
                {
                    wavWriter.Write(data[i]);
                }
                byteCount += len;
            }
        }
        #endregion

        private const uint opusInputRate = 48000;
        private void OpusInputButton_Click(object sender, EventArgs e)
        {
            Thread t = startThread(inputOpusProc, "inputOpusProc");
            t.Join();
        }
        private BinaryWriter opusWriter;
        private void inputOpusProc()
        {
            bool keepGoing = true;
            string tempName = Path.GetTempFileName();
            JJAudioStream inStream = new JJAudioStream();
            inStream.OpenOpus(Devices.DeviceTypes.input, opusInputRate, writeOpus);

            int rpt = 0;
            do
            {
                // temporary stream
                using (Stream tempStream = File.Open(tempName,
                    (File.Exists(tempName)) ? FileMode.Append : FileMode.Create))
                {
                    using (opusWriter = new BinaryWriter(tempStream))
                    {
                        inputDone = false;
                        try
                        {
                            // Start input
                            if (!inStream.StartAudio()) throw new Exception("audio stream didn't start");
                            Thread.Sleep(5000);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace, "Error", MessageBoxButtons.OK);
                            keepGoing = false;
                        }
                        // end input
                        inStream.StopAudio();
                        while (!inputDone) Thread.Sleep(25);
                        //MessageTextBox.Text += "audio floats out:" + byteCount + "\r\n";
                    }
                }
                if (inputRepeats > 0)
                {
                    Console.Beep();
                    if (rpt < inputRepeats)
                    {
                        Thread.Sleep(2000);
                        Console.Beep();
                    }
                }
            } while (rpt++ < inputRepeats);
            inStream.Close();

            if (!keepGoing) goto inWavDone;

            JJAudioStream outStream = new JJAudioStream();
            outStream.OpenOpus(Devices.DeviceTypes.output, opusInputRate);
            // read from temp file
            using (Stream tempStream = File.Open(tempName, FileMode.Open))
            {
                using (BinaryReader br = new BinaryReader(tempStream))
                {
                    try
                    {
                        // Start output
                        if (!outStream.StartAudio()) throw new Exception("output stream didn't start");
                        while (true)
                        {
                            short len = br.ReadInt16();
                            byte[] buf = new byte[len];
                            br.Read(buf, 0, len);
                            outStream.WriteOpus(buf);
                        }
                    }
                    catch (EndOfStreamException)
                    {
                        outStream.Flush();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace, "Error", MessageBoxButtons.OK);
                    }
                    // end output
                    outStream.StopAudio();
                    outStream.Close();
                }
            }

            inWavDone:
            //int i = 1;
            File.Delete(tempName);
        }

        private void writeOpus(byte[] data)
        {
            short len = (short)data.Length;
            inputDone = (len == 0);
            if (!inputDone)
            {
                opusWriter.Write(len);
                opusWriter.Write(data, 0, len);
            }
        }

        private void MorseInputButton_Click(object sender, EventArgs e)
        {
            MorseForm form = new MorseForm();
            form.ShowDialog();
            form.Dispose();
        }
    }
}
