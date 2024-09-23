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

namespace Tester
{
    public partial class Form1 : Form
    {
        // Directives sent in admin pipe.
        private const string doWav = "wav";
        private const string doOpus = "opus";
        private const string doCWMon = "cwmon";
        private const string endWav = "ewav";
        private const string endOpus = "eopus";
        private const string endCWMon = "ecwmon";
        private const string allDone = "done";

        private AnonymousPipeServerStream adminPipe;
        private BinaryWriter adminWriter;
        private Process audioClient;
        private string cfgFile;
        private Thread testThread;
        private JJPortaudio.Devices dev;
        private JJPortaudio.Devices.Device outDevice;
        private uint channel = 0;
        private string pipeName(string prefix, uint suffix)
        {
            return prefix + suffix.ToString();
        }

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
                adminWriter.Flush();
                adminPipe.WaitForPipeDrain();
                dev = new Devices(cfgFile);
                dev.Setup();
                outDevice = dev.GetConfiguredDevice(Devices.DeviceTypes.output, true);
            }
            catch(Exception ex)
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
                adminWriter.Write(allDone);
                adminWriter.Flush();
                audioClient.WaitForExit(1000);
                if (!audioClient.HasExited) audioClient.Kill();
                adminWriter.Dispose();
                adminPipe.Dispose();
                audioClient.Dispose();
            }
            catch { }
        }

        private const string wavFile = @"c:\users\jjs\documents\audfile.dat";
        private void WavButton_Click(object sender, EventArgs e)
        {
            string name = pipeName(doWav, channel++);
            using (Stream inStream = File.Open(wavFile, FileMode.Open))
            {
                using (BinaryReader br = new BinaryReader(inStream))
                {
                    using (NamedPipeClientStream pipe =
                        new NamedPipeClientStream(".", name, PipeDirection.Out))
                    {
                        using (BinaryWriter bw = new BinaryWriter(pipe))
                        {
                            adminWriter.Write(doWav);
                            adminWriter.Write((uint)24000);
                            adminWriter.Write(name);
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
        }

        private const string opusFile = @"c:\users\jjs\documents\opusTest2.opus";
        private void OpusButton_Click(object sender, EventArgs e)
        {
            string name = pipeName(doOpus, channel++);
            using (Stream inStream = File.Open(opusFile, FileMode.Open))
            {
                using (BinaryReader br = new BinaryReader(inStream))
                {
                    using (NamedPipeClientStream pipe =
                        new NamedPipeClientStream(".", name, PipeDirection.Out))
                    {
                        using (BinaryWriter bw = new BinaryWriter(pipe))
                        {
                            adminWriter.Write(doOpus);
                            adminWriter.Write((uint)48000);
                            adminWriter.Write(name);
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
        }

        private static string morseText = "hello world.";
        private void CWMonButton_Click(object sender, EventArgs e)
        {
            string name = pipeName(doCWMon, channel++);
            using (NamedPipeClientStream pipe =
                new NamedPipeClientStream(".", name, PipeDirection.Out))
            {
                using (BinaryWriter bw = new BinaryWriter(pipe))
                {
                    adminWriter.Write(doCWMon);
                    adminWriter.Write((uint)48000);
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
    }
}
