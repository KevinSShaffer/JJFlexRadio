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
using AudioHelper;
using JJPortaudio;
using JJTrace;
using POpusCodec;

namespace testAudioHelper
{
    public partial class Form1 : Form
    {
        private AnonymousPipeServerStream adminPipe;
        private BinaryWriter adminWriter;
        private Process audioClient;
        private string cfgFile;
        private JJPortaudio.Devices dev;
        private JJPortaudio.Devices.Device outDevice, inDevice;
        private uint channel = 0;

        public Form1()
        {
            InitializeComponent();

            adminPipe = new AnonymousPipeServerStream(PipeDirection.Out, HandleInheritability.Inheritable);
            adminWriter = new BinaryWriter(adminPipe);
            audioClient = new Process();
            audioClient.StartInfo.FileName = "AudioHelper.exe";
            audioClient.StartInfo.UseShellExecute = false;
            string cfgDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\TestAudioHelper";
            Directory.CreateDirectory(cfgDir);
            cfgFile = cfgDir + "\\cfg.xml";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                audioClient.StartInfo.Arguments = adminPipe.GetClientHandleAsString() + ' ' + cfgFile;
                audioClient.Start();
                adminPipe.DisposeLocalCopyOfClientHandle();
                adminWriter.Write(4f);
                adminWriter.Write(7f);
                adminWriter.Write(4f / 7f);
                adminPipe.WaitForPipeDrain();

                dev = new Devices(cfgFile);
                dev.Setup();
                outDevice = dev.GetConfiguredDevice(Devices.DeviceTypes.output, true);
                inDevice = dev.GetConfiguredDevice(Devices.DeviceTypes.input, true);
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
            try
            {
                adminWriter.Write((byte)AdminDirectives.allDone);
                audioClient.WaitForExit(1000);
                if (!audioClient.HasExited) audioClient.Kill();
                adminWriter.Dispose();
                adminPipe.Dispose();
                audioClient.Dispose();
            }
            catch { }
        }

        private bool sendAudioCommand(AudioHelper.AdminDirectives cmd,uint rate,string name)
        {
            bool rv = true;
            try
            {
                adminWriter.Write((byte)cmd);
                adminWriter.Write(rate);
                adminWriter.Write(name);
            }
            catch(Exception ex)
            {
                MessageTextBox.Text += "sendAudioCommand exception:" + ex.Message;
                rv = false;
            }
            return rv;
        }

        private const string wavFile = @"c:\users\jjs\documents\audfile.dat";
        private void WavButton_Click(object sender, EventArgs e)
        {
            #region copy
            string name = "outwav" + (channel++).ToString();
            sendAudioCommand(AdminDirectives.openOutWav, 24000, name);
            // Audio file.
            using (Stream inStream = File.Open(wavFile, FileMode.Open))
            {
                using (BinaryReader br = new BinaryReader(inStream))
                {
                    // Audio pipe
                    using (NamedPipeClientStream pipe =
                        new NamedPipeClientStream(".", name, PipeDirection.Out))
                    {
                        using (BinaryWriter bw = new BinaryWriter(pipe))
                        {
                            sendAudioCommand(AdminDirectives.startOutWav, 24000, name);
                            try
                            {
                                pipe.Connect();
                                while (true)
                                {
                                    bw.Write(br.ReadSingle());
                                }
                            }
                            catch (EndOfStreamException)
                            {
                                pipe.Flush();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
                            }
                            pipe.WaitForPipeDrain();
                        }
                    }
                }
            }
            sendAudioCommand(AdminDirectives.closeChannel, 24000, name);
#endregion
        }

        private const string opusFile = @"c:\users\jjs\documents\tmp\opusout.dat";
        //private const string opusFile = @"C:\Users\jjs\AppData\Local\Temp\tmpcab8.tmp";
        private void OpusButton_Click(object sender, EventArgs e)
        {
            string name = "outopus" + (channel++).ToString();
            sendAudioCommand(AdminDirectives.openOutOpus, 48000, name);
            // opus input file.
            // Each record must be of the form short length, byte[] opus data.
            using (Stream inStream = File.Open(opusFile, FileMode.Open))
            {
                using (BinaryReader br = new BinaryReader(inStream))
                {
                    // output pipe
                    using (NamedPipeClientStream pipe =
                        new NamedPipeClientStream(".", name, PipeDirection.Out))
                    {
                        sendAudioCommand(AdminDirectives.startOutOpus, 48000, name);
                        try
                        {
                            pipe.Connect();
                            while (true)
                            {
                                ushort len = br.ReadUInt16();
                                byte[] buf = new byte[len];
                                br.Read(buf, 0, len);
                                pipe.WriteByte((byte)(len % 256));
                                pipe.WriteByte((byte)(len / 256));
                                pipe.Write(buf, 0, len);
                            }
                        }
                        catch (EndOfStreamException)
                        {
                            pipe.Flush();
                            pipe.WaitForPipeDrain();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
                        }
                    }
                }
            }
            sendAudioCommand(AdminDirectives.closeChannel, 48000, name);
        }

        private static string morseText = "hello world.";
        private void CWMonButton_Click(object sender, EventArgs e)
        {
            string name = "cwmon" + (channel++).ToString();
            using (NamedPipeClientStream pipe =
                new NamedPipeClientStream(".", name, PipeDirection.Out))
            {
                using (BinaryWriter bw = new BinaryWriter(pipe))
                {
                    adminWriter.Write((byte)AdminDirectives.doCWMon);
                    adminWriter.Write((uint)3000);
                    adminWriter.Write(name);
                    try
                    {
                        pipe.Connect();
                        foreach(char c in morseText)
                        {
                            bw.Write((int)20);
                            bw.Write(c);
                        }
                        pipe.Flush();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
                    }
                }
            }
        }

        private void InputButton_Click(object sender, EventArgs e)
        {
            #region copy
            bool keepGoing = true;
            MessageTextBox.Text = "";
            string inName = "inwav" + (channel++).ToString();
            string outName = "outPipe";
            string tempName = Path.GetTempFileName();
            sendAudioCommand(AdminDirectives.openInWav, 24000, inName);
            // pipe for reading input
            using (NamedPipeClientStream inPipe =
                new NamedPipeClientStream(".", inName, PipeDirection.In))
            {
                using (BinaryReader br = new BinaryReader(inPipe))
                {
                    // temporary stream
                    using (Stream tempStream = File.Open(tempName, FileMode.Create))
                    {
                        using (BinaryWriter bw = new BinaryWriter(tempStream))
                        {
                            // Start input
                            sendAudioCommand(AdminDirectives.startInWav, 24000, inName);
                            int byteCount = 0;
                            try
                            {
                                inPipe.Connect();
                                DateTime stoptime = DateTime.Now + new TimeSpan(10000 * 1000 * 5);
                                do
                                {
                                    bw.Write(br.ReadSingle());
                                    byteCount++;
                                } while (DateTime.Now < stoptime);
                            }
                            catch (EndOfStreamException) { }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace, "Error", MessageBoxButtons.OK);
                                keepGoing = false;
                            }
                            // end input
                            sendAudioCommand(AdminDirectives.closeChannel, 24000, inName);
                            MessageTextBox.Text += "audio floats out:" + byteCount + "\r\n";
                        }
                    }
                }
            }

            if (!keepGoing) goto inWavDone;

            sendAudioCommand(AdminDirectives.openOutWav, 24000, outName);
            // pipe for sending audio
            using (NamedPipeClientStream outPipe = new NamedPipeClientStream(".", outName, PipeDirection.Out))
            {
                using (BinaryWriter bw = new BinaryWriter(outPipe))
                {
                    // read from temp file
                    using (Stream tempStream = File.Open(tempName, FileMode.Open))
                    {
                        using (BinaryReader br = new BinaryReader(tempStream))
                        {
                            // Start output
                            sendAudioCommand(AdminDirectives.startOutWav, 24000, outName);
                            int byteCount = 0;
                            try
                            {
                                outPipe.Connect();
                                while (true)
                                {
                                    bw.Write(br.ReadSingle());
                                    byteCount++;
                                }
                            }
                            catch (EndOfStreamException) { }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace, "Error", MessageBoxButtons.OK);
                            }
                            // end output
                            sendAudioCommand(AdminDirectives.flushChannel, 24000, outName);
                            sendAudioCommand(AdminDirectives.closeChannel, 24000, outName);
                            MessageTextBox.Text += "audio floats out:" + byteCount + "\r\n";
                        }
                    }
                }
            }

            inWavDone:
            //int i = 1;
            File.Delete(tempName);
            #endregion
        }

#if zero
        private void OpusInput2Button_Click(object sender, EventArgs e)
        {
            string name = "outopus"+(channel++).ToString();
            using (Stream inStream = File.Open(wavFile, FileMode.Open))
            {
                using (BinaryReader br = new BinaryReader(inStream))
                {
                    using (NamedPipeClientStream pipe =
                        new NamedPipeClientStream(".", name, PipeDirection.Out))
                    {
                        using (BinaryWriter bw = new BinaryWriter(pipe))
                        {
                            adminWriter.Write((byte)AdminDirectives.outOpus);
                            adminWriter.Write((uint)24000);
                            adminWriter.Write(name);
                            OpusEncoder encoder = new OpusEncoder(POpusCodec.Enums.SamplingRate.Sampling24000, POpusCodec.Enums.Channels.Stereo);
                            encoder.MaxBandwidth = POpusCodec.Enums.Bandwidth.SuperWideband;
                            encoder.EncoderDelay = POpusCodec.Enums.Delay.Delay10ms;
                            float[] buf = new float[encoder.FrameSizePerChannel * 2];
                            try
                            {
                                pipe.Connect();
                                while (true)
                                {
                                    for (int i = 0; i < buf.Length; i++) buf[i] = br.ReadSingle();
                                    byte[] opusBuf = encoder.Encode(buf);
                                    bw.Write((ushort)opusBuf.Length);
                                    bw.Write(opusBuf, 0, opusBuf.Length);
                                }
                            }
                            catch (EndOfStreamException)
                            {
                                pipe.Flush();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
                            }
                            pipe.WaitForPipeDrain();
                        }
                    }
                }
            }
        }
#endif

        private const uint opusInputRate = 48000;
#if DoOpusHere
        private const POpusCodec.Enums.SamplingRate pOpusRate = POpusCodec.Enums.SamplingRate.Sampling48000;
        private void OpusInputButton_Click(object sender, EventArgs e)
        {
            bool keepGoing = true;
            MessageTextBox.Text = "";
            string inName = "inopus"+(channel++).ToString();
            string outName = "outPipe";
            string tempName = Path.GetTempFileName();

            //OpusDecoder decoder = new OpusDecoder(pOpusRate, POpusCodec.Enums.Channels.Stereo);
            OpusEncoder encoder = new OpusEncoder(pOpusRate, POpusCodec.Enums.Channels.Stereo);
            encoder.MaxBandwidth = POpusCodec.Enums.Bandwidth.SuperWideband;
            encoder.EncoderDelay = POpusCodec.Enums.Delay.Delay10ms;
            int encodeBufSZ = encoder.FrameSizePerChannel * 2;

            // pipe for reading input
            using (NamedPipeClientStream inPipe =
                new NamedPipeClientStream(".", inName, PipeDirection.In))
            {
                using (BinaryReader br = new BinaryReader(inPipe))
                {
                    using (Stream tempStream = File.Open(tempName, FileMode.Create))
                    {
                        using (BinaryWriter bw = new BinaryWriter(tempStream))
                        {
                            // Start input
                            adminWriter.Write((byte)AdminDirectives.inWav);
                            adminWriter.Write((uint)opusInputRate);
                            adminWriter.Write(inName);
                            int recCount = 0;
                            int byteCount = 0;
                            try
                            {
                                inPipe.Connect();
                                DateTime stoptime = DateTime.Now + new TimeSpan(10000 * 1000 * 5);
                                do
                                {
                                    float[] buf = new float[encodeBufSZ];
                                    for (int id = 0; id < encodeBufSZ; id++) buf[id] = br.ReadSingle();
                                    byte[] opusBuf = encoder.Encode(buf);
                                    ushort len = (ushort)opusBuf.Length;
                                    bw.Write(len);
                                    bw.Write(opusBuf, 0, len);
                                    recCount++;
                                    byteCount += (int)len;
                                } while (DateTime.Now < stoptime);
                            }
                            catch (EndOfStreamException) { }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace, "Error", MessageBoxButtons.OK);
                                keepGoing = false;
                            }
                            // end input
                            adminWriter.Write((byte)AdminDirectives.endChannel);
                            adminWriter.Write((uint)opusInputRate);
                            adminWriter.Write(inName);
                            MessageTextBox.Text += "records in:" + recCount + "\r\n";
                            MessageTextBox.Text += "opus bytes out:" + byteCount + "\r\n";
                        }
                    }
                }
            }

            if (!keepGoing) goto inOpusDone;

            // pipe for sending audio
            using (NamedPipeClientStream outPipe = new NamedPipeClientStream(".", outName, PipeDirection.Out))
            {
                using (BinaryWriter bw = new BinaryWriter(outPipe))
                {
                    using (Stream tempStream = File.Open(tempName, FileMode.Open))
                    {
                        using (BinaryReader br = new BinaryReader(tempStream))
                        {
                            // Start output
                            adminWriter.Write((byte)AdminDirectives.outOpus);
                            adminWriter.Write((uint)opusInputRate);
                            adminWriter.Write(outName);
                            int recCount = 0;
                            int byteCount = 0;
                            try
                            {
                                outPipe.Connect();
                                while (true)
                                {
                                    short len = br.ReadInt16();
                                    if (len == -1) throw new EndOfStreamException();
                                    byte[] buf = new byte[len];
                                    br.Read(buf, 0, len);
                                    bw.Write(len);
                                    bw.Write(buf, 0, len);
                                    recCount++;
                                    byteCount += len;
                                }
                            }
                            catch (EndOfStreamException) { }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace, "Error", MessageBoxButtons.OK);
                            }
                            // end output
                            adminWriter.Write((byte)AdminDirectives.flushChannel);
                            adminWriter.Write((uint)opusInputRate);
                            adminWriter.Write(outName);
                            adminWriter.Write((byte)AdminDirectives.endChannel);
                            adminWriter.Write((uint)opusInputRate);
                            adminWriter.Write(outName);
                            MessageTextBox.Text += "records out:" + recCount + "\r\n";
                            MessageTextBox.Text += "opus bytes out:" + byteCount + "\r\n";
                        }
                    }
                }
            }

            inOpusDone:
            int i = 1;
            //File.Delete(tempName);
        }
#else
        private void OpusInputButton_Click(object sender, EventArgs e)
        {
            bool keepGoing = true;
            MessageTextBox.Text = "";
            string inName = "inopus" + (channel++).ToString();
            string outName = "outPipe" + (channel++).ToString();
            string tempName = Path.GetTempFileName();
            sendAudioCommand(AdminDirectives.openInOpus, opusInputRate, inName);
            // pipe for reading input
            using (NamedPipeClientStream inPipe =
                new NamedPipeClientStream(".", inName, PipeDirection.In))
            {
                using (BinaryReader br = new BinaryReader(inPipe))
                {
                    // temporary stream
                    using (Stream tempStream = File.Open(tempName, FileMode.Create))
                    {
                        using (BinaryWriter bw = new BinaryWriter(tempStream))
                        {
                            // Start input
                            sendAudioCommand(AdminDirectives.startInOpus, opusInputRate, inName);
                            int recCount = 0;
                            int byteCount = 0;
                            try
                            {
                                inPipe.Connect();
                                DateTime stoptime = DateTime.Now + new TimeSpan(10000 * 1000 * 5);
                                do
                                {
                                    ushort len = br.ReadUInt16();
                                    byte[] buf = new byte[len];
                                    br.Read(buf, 0, len);
                                    bw.Write(len);
                                    bw.Write(buf, 0, len);
                                    recCount++;
                                    byteCount += (int)len;
                                } while (DateTime.Now < stoptime);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace, "Error", MessageBoxButtons.OK);
                                keepGoing = false;
                            }
                            // end input
                            sendAudioCommand(AdminDirectives.closeChannel, opusInputRate, inName);
                            MessageTextBox.Text += "records in:" + recCount + "\r\n";
                            MessageTextBox.Text += "audio bytes out:" + byteCount + "\r\n";
                        }
                    }
                }
            }

            if (!keepGoing) goto inOpusDone;

            sendAudioCommand(AdminDirectives.openOutOpus, opusInputRate, outName);
            // pipe for sending audio
            using (NamedPipeClientStream outPipe = new NamedPipeClientStream(".", outName, PipeDirection.Out))
            {
                using (BinaryWriter bw = new BinaryWriter(outPipe))
                {
                    // temp file
                    using (Stream tempStream = File.Open(tempName, FileMode.Open))
                    {
                        using (BinaryReader br = new BinaryReader(tempStream))
                        {
                            // Start output
                            sendAudioCommand(AdminDirectives.startOutOpus, opusInputRate, outName);
                            int recCount = 0;
                            int byteCount = 0;
                            try
                            {
                                outPipe.Connect();
                                while (true)
                                {
                                    short len = br.ReadInt16();
                                    if (len == -1) throw new EndOfStreamException();
                                    byte[] buf = new byte[len];
                                    br.Read(buf, 0, len);
                                    bw.Write(len);
                                    bw.Write(buf, 0, len);
                                    recCount++;
                                    byteCount += len;
                                }
                            }
                            catch (EndOfStreamException) { }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message + "\r\n" + ex.StackTrace, "Error", MessageBoxButtons.OK);
                            }
                            // end output
                            sendAudioCommand(AdminDirectives.flushChannel, opusInputRate, outName);
                            sendAudioCommand(AdminDirectives.closeChannel, opusInputRate, outName);
                            MessageTextBox.Text += "records out:" + recCount + "\r\n";
                            MessageTextBox.Text += "audio bytes out:" + byteCount + "\r\n";
                        }
                    }
                }
            }

            inOpusDone:
            //int i = 1;
            File.Delete(tempName);
        }
#endif
    }
}
