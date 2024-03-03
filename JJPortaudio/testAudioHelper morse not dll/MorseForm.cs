using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using JJPortaudio;

namespace testAudioHelper
{
    public partial class MorseForm : Form
    {
        private Thread testThread;
        private BlockingCollection<char> characterQ;
        private Morse morse;

        public MorseForm()
        {
            InitializeComponent();
        }

        private void MorseForm_Load(object sender, EventArgs e)
        {
            characterQ = new BlockingCollection<char>();
            FreqBox.Text = "700";
            SpeedBox.Text = "20";
            morse = new Morse();

            testThread = new Thread(testProc);
            testThread.Name = "MorseTestThread";
            testThread.Start();
        }

        private void testProc()
        {
            uint val = 0;
            if (System.UInt32.TryParse(FreqBox.Text, out val))
            {
                morse.Frequency = val;
            }
            if (System.UInt32.TryParse(SpeedBox.Text, out val))
            {
                morse.Speed = val;
            }

            morse.Start();

            while (!characterQ.IsAddingCompleted)
            {
                try
                {
                    morse.Send(characterQ.Take());
                }
                catch (InvalidOperationException) { }
            }
        }

        private void MorseForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            characterQ.CompleteAdding();
            try
            {
                if (testThread != null) testThread.Join();
            }
            catch { }
            characterQ.Dispose();
            morse.Close();
        }

        private void FreqBox_Leave(object sender, EventArgs e)
        {
            uint val = 0;
            if (System.UInt32.TryParse(FreqBox.Text, out val))
            {
                morse.Frequency = val;
            }
        }

        private void SpeedBox_Leave(object sender, EventArgs e)
        {
            uint val = 0;
            if (System.UInt32.TryParse(SpeedBox.Text, out val))
            {
                morse.Speed = val;
            }
        }

        private void CodeBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            characterQ.Add(e.KeyChar);
        }
    }
}
