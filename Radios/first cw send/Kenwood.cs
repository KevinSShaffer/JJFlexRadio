//#define MemoryDebug
#define GetMemoriez
//#define GetMenuz
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace Radios
{
    /// <summary>
    /// Kenwood superclass
    /// </summary>
    public partial class Kenwood : AllRadios
    {
        // Region - Commands to send to Kenwood rigs.
        #region kenwood commands
        protected const string kcmdAC = "AC"; // Sets or reads the internal antenna tuner status.
        protected const string kcmdAG = "AG"; // Sets or reads the AF gain.
        protected const string kcmdAI = "AI"; // Sets or reads the Auto Information (AI) function ON/ OFF.
        protected const string kcmdAN = "AN"; // Selects the antenna connector ANT1/ ANT2.
        protected const string kcmdAS = "AS"; // Sets or reads the Auto Mode function parameters.
        protected const string kcmdBC = "BC"; // Sets or reads the Beat Cancel function status.
        protected const string kcmdBD = "BD"; // Sets a frequency band.
        protected const string kcmdBP = "BP"; // Adjusts the Notch Frequency of the Manual Notch Filter.
        protected const string kcmdBU = "BU"; // Sets a frequency band.
        protected const string kcmdBY = "BY"; // Reads the busy signal status.
        protected const string kcmdCA = "CA"; // Sets and reads the CW TUNE function status.
        protected const string kcmdCG = "CG"; // Sets and reads the Carrier Level.
        protected const string kcmdCH = "CH"; // Operate the MULTI/CH encoder.
        protected const string kcmdCN = "CN"; // Sets and reads the CTCSS frequency.
        protected const string kcmdCT = "CT"; // Sets and reads the CTCSS function status.
        protected const string kcmdDA = "DA"; // Sets and reads the DATA mode.
        protected const string kcmdDN = "DN"; // Emulates the microphone DWN and UP keys.
        protected const string kcmdEM = "EM"; // Sets the Emergency communication frequency mode.
        protected const string kcmdEX = "EX"; // Sets or reads the Menu.
        protected const string kcmdFA = "FA"; // Sets or reads the VFO A frequency.
        protected const string kcmdFB = "FB"; // Sets or reads the VFO B frequency.
        protected const string kcmdFL = "FL"; // Sets and reads the IF filter.
        protected const string kcmdFR = "FR"; // Selects or reads the VFO or Memory channel.
        protected const string kcmdFS = "FS"; // Sets and reads the Fine Tuning function status.
        protected const string kcmdFT = "FT"; // Selects or reads the VFO or Memory channel.
        protected const string kcmdFV = "FV"; // Verifies the Firmware version.
        protected const string kcmdFW = "FW"; // Sets or reads the DSP filtering bandwidth.
        protected const string kcmdGC = "GC"; // Sets or reads the AGC.
        protected const string kcmdGT = "GT"; // Sets or reads the AGC time constant.
        protected const string kcmdID = "ID"; // Reads the transceiver ID number.
        protected const string kcmdIF = "IF"; // Reads the transceiver status. I
        protected const string kcmdIS = "IS"; // Sets and reads the DSP Filter Shift.
        protected const string kcmdKS = "KS"; // Sets and reads the Keying speed.
        protected const string kcmdKY = "KY"; // Converts the entered characters into morse code while keying.
        protected const string kcmdLK = "LK"; // Sets and reads the Lock status.
        protected const string kcmdLM = "LM"; // Sets and reads the VGS-1 electric keyer recording status.
        protected const string kcmdMC = "MC"; // Sets and reads the Memory Channel number.
        protected const string kcmdMD = "MD"; // Sets and reads the operating mode status.
        protected const string kcmdMF = "MF"; // Sets and reads Menu A or B.
        protected const string kcmdMG = "MG"; // Sets and reads the microphone gain.
        protected const string kcmdML = "ML"; // Sets and reads the TX Monitor function output level.
        protected const string kcmdMR = "MR"; // Reads the Memory channel data.
        protected const string kcmdMW = "MW"; // Sets the Memory channel data.
        protected const string kcmdNB = "NB"; // Sets and reads the Noise Blanker function status.
        protected const string kcmdNL = "NL"; // Sets and reads the Noise Blanker level.
        protected const string kcmdNR = "NR"; // Sets and reads the Noise Reduction function status.
        protected const string kcmdNT = "NT"; // Sets and reads the Notch Filter status.
        protected const string kcmdPA = "PA"; // Sets and reads the Pre-amplifier function status.
        protected const string kcmdPB = "PB"; // Sets and reads the voice and CW message playback status.
        protected const string kcmdPC = "PC"; // Sets and reads the output power.
        protected const string kcmdPL = "PL"; // Sets and reads the Speech Processor input/output level.
        protected const string kcmdPR = "PR"; // Sets and reads the Speech Processor function ON/ OFF.
        protected const string kcmdPS = "PS"; // Sets and reads the Power ON/ OFF status.
        protected const string kcmdQD = "QD"; // Deletes the Quick Memory.
        protected const string kcmdQI = "QI"; // Stores the settings in the Quick Memory.
        protected const string kcmdQR = "QR"; // Sets and reads the Quick Memory channel data.
        protected const string kcmdRA = "RA"; // Sets and reads the RF Attenuator status.
        protected const string kcmdRC = "RC"; // Clears the RIT/XIT frequency.
        protected const string kcmdRD = "RD"; // Sets and reads the RIT/XIT frequency Up/ Down. Also sets and reads the scan speed in Scan mode.
        protected const string kcmdRG = "RG"; // Sets and reads the RF Gain status.
        protected const string kcmdRL = "RL"; // Sets and reads the Noise Reduction Level.
        protected const string kcmdRM = "RM"; // Sets and reads the Meter function.
        protected const string kcmdRS = "RS"; // enter/leave menu mode
        protected const string kcmdRT = "RT"; // Sets and reads the RIT function status.
        protected const string kcmdRU = "RU"; // Sets and reads the RIT/XIT frequency Up/ Down. Also sets and reads the scan speed in Scan mode.
        protected const string kcmdRX = "RX"; // Sets the receiver function status.
        protected const string kcmdSC = "SC"; // Sets and reads the Scan function status.
        protected const string kcmdSD = "SD"; // Sets and reads the CW break-in time delay.
        protected const string kcmdSH = "SH"; // Sets and reads the slope tune bandwidth high setting.
        protected const string kcmdSL = "SL"; // Sets and reads the slope tune bandwidth low setting.
        protected const string kcmdSM = "SM"; // Reads the SMeter.
        // "SM" command varies according to the model.
        protected virtual string SMHdr
        {
            get { return kcmdSM; }
        }
        protected const string kcmdSQ = "SQ"; // Sets and reads the squelch value.
        protected const string kcmdSR = "SR"; // Resets the transceiver.
        protected const string kcmdSS = "SS"; // Sets and reads the Program Slow Scan frequency.
        protected const string kcmdSU = "SU"; // Sets and reads the Scan group.
        protected const string kcmdSV = "SV"; // Performs the Memory Transfer function.
        protected const string kcmdTN = "TN"; // Sets and reads the Tone frequency.
        protected const string kcmdTO = "TO"; // Sets and reads the Tone status.
        protected const string kcmdTS = "TS"; // Sets and reads the TF-Set status.
        protected const string kcmdTX = "TX"; // Sets the transmission mode.
        protected const string kcmdUP = "UP"; // Emulates the microphone DWN and UP keys.
        protected const string kcmdUR = "UR"; // Sets and reads the RX equalizer.
        protected const string kcmdUT = "UT"; // Sets and reads the TX equalizer.
        protected const string kcmdVD = "VD"; // Sets and reads the VOX Delay time.
        protected const string kcmdVG = "VG"; // Sets and reads the VOX Gain.
        protected const string kcmdVR = "VR"; // Sets the Voice synthesis generation function.
        protected const string kcmdVS0 = "VS0"; // Sets and reads the Visual Scan start/ stop/ pause status.
        protected const string kcmdVS1 = "VS1"; //  Sets the Visual Scan center frequency.
        protected const string kcmdVS2 = "VS2"; //  Sets the Visual Scan span.
        protected const string kcmdVS3 = "VS3"; //  Reads the Visual Scan upper/ lower/center frequency, and span.
        protected const string kcmdVS4 = "VS4"; //  Reads the Visual Scan sweep frequency and signal level.
        protected const string kcmdVV = "VV"; // Performs the VFO copy (A=B) function.
        protected const string kcmdVX = "VX"; // Sets and reads the VOX and Break-in function status.
        protected const string kcmdXI = "XI"; // Reads the transmit frequency and mode.
        protected const string kcmdXO = "XO"; // Sets and reads the offset direction and frequency for the transverter mode.
        protected const string kcmdXT = "XT"; // Sets and reads the XIT function status.
        #endregion

        protected static string BldCmd(string cmd)
        {
            return cmd + ";";
        }

        /// <summary>
        /// Power status.
        /// If setting, you must subsequently test to see if it got set.
        /// </summary>
        public override bool Power
        {
            get { return base.Power; }
            set
            {
                // Attempt to set the power status; might not work.
                //DBGWL("Power set " + value.ToString());
                if (value) Callouts.safeSend(BldCmd(kcmdPS+"1"));
                else Callouts.safeSend(BldCmd(kcmdPS+"0"));
            }
        }
        protected uint powerCount;
        protected const int PowerCountThreshhold = 2;
        protected Mutex powerLock; // must be short term!
        public override void PowerCheck()
        {
            // Just quit if continuous monitoring is off.
            if (!ContinuousMonitoring) return;
            //DBGW("PowerCheck:count=" + powerCount.ToString());
            base.PowerCheck();
            powerLock.WaitOne();
            // See if we've received a response to the last requests.
            if (powerCount >= PowerCountThreshhold)
            {
                // looks like power is off.
                if (pwr)
                {
                    // Was on.
                    pwr = false;
                    //DBGW(" turn off");
                }
            }
            else
            {
                // power is on.
                if (!pwr)
                {
                    // it wasn't.
                    pwr = true;
                    //DBGW(" was off, turn on");
                    Callouts.safeSend(BldCmd(kcmdAI + "2"));
                    rigStat(false);
                }
            }
            powerCount++;
            powerLock.ReleaseMutex();
            Callouts.safeSend(BldCmd(kcmdPS));
            //DBGWL("");
        }

        public override bool Transmit
        {
            get { return XMit; }
            set
            {
                Callouts.safeSend(kcmdTX);
            }
        }
        public override RigCaps.VFOs RXVFO
        {
            get
            {
                return base.RXVFO;
            }
            set
            {
                string val;
                if (value == RigCaps.VFOs.None) val = "2";
                else val = ((int)value).ToString();
                Callouts.safeSend(BldCmd(kcmdFR + val));
            }
        }
        public override RigCaps.VFOs TXVFO
        {
            get
            {
                return base.TXVFO;
            }
            set
            {
                string val;
                if (value == RigCaps.VFOs.None) val = "2";
                else val = ((int)value).ToString();
                Callouts.safeSend(BldCmd(kcmdFT + val));
            }
        }
        public override void CopyVFO(RigCaps.VFOs inv, RigCaps.VFOs outv)
        {
            Callouts.safeSend(BldCmd(kcmdVV));
        }
        public override bool SplitShowXmitFrequency
        {
            get
            {
                return base.SplitShowXmitFrequency;
            }
            set
            {
                if (!Transmit)
                {
                    string val = (TFSetOn) ? "0" : "1";
                    Callouts.safeSend(BldCmd(kcmdTS + val));
                }
            }
        }
        public override ulong RXFrequency
        {
            get { return (TFSetOn) ? TXFreq : RXFreq; }
            set
            {
                string str = "F" + VFOToLetter(RXVFO) + UFreqToString(value);
                Callouts.safeSend(BldCmd(str));
            }
        }
        public override ulong TXFrequency
        {
            get { return TXFreq; }
            set
            {
                string str = "F" + VFOToLetter(TXVFO) + UFreqToString(value);
                Callouts.safeSend(BldCmd(str));
            }
        }
        public override bool Split
        {
            get
            {
                return base.Split;
            }
            set
            {
                if (RXVFO != RigCaps.VFOs.None)
                {
                    // Using VFOs.
                    int v = (value) ? (((int)RXVFO + 1) % 2) : (int)RXVFO;
                    Callouts.safeSend(BldCmd(kcmdFT + v.ToString()));
                }
                // Else using a memory, can't set it.
            }
        }

        public override int CurrentMemoryChannel
        {
            get
            {
                return base.CurrentMemoryChannel;
            }
            set
            {
                string val = value.ToString("d3");
                Callouts.safeSend(BldCmd(kcmdMC + val));
            }
        }
        public override bool MemoryMode
        {
            get
            {
                return base.MemoryMode;
            }
            set
            {
                if (value)
                {
                    if (CurVFO != RigCaps.VFOs.None)
                    {
                        oldVFO = CurVFO;
                        Callouts.safeSend(BldCmd(kcmdFR + "2"));
                    }
                    // else already in memory mode.
                }
                else
                {
                    if (CurVFO == RigCaps.VFOs.None)
                    {
                        Callouts.safeSend(BldCmd(kcmdFR + ((int)oldVFO).ToString()));
                    }
                    // else already using a VFO.
                }
            }
        }

        public override Modes RXMode
        {
            get
            {
                return base.RXMode;
            }
            set
            {
                char c = KMode(value);
                Callouts.safeSend(BldCmd(kcmdMD + c.ToString()));
            }
        }
        public override Modes TXMode
        {
            get
            {
                return base.TXMode;
            }
            set
            {
                char c = KMode(value);
                Callouts.safeSend(BldCmd(kcmdMD + c.ToString()));
            }
        }

        public override DataModes RXDataMode
        {
            get
            {
                return base.RXDataMode;
            }
            set
            {
                char c = kDataMode(value);
                Callouts.safeSend(BldCmd(kcmdDA + c.ToString()));
            }
        }
        public override DataModes TXDataMode
        {
            get
            {
                return base.TXDataMode;
            }
            set
            {
                char c = kDataMode(value);
                Callouts.safeSend(BldCmd(kcmdDA + c.ToString()));
            }
        }

        private ToneCTSSValue itoneCT; // Internal setting, see contCT and contTO
        private enum CTTOStates
        {
            none,
            CTRcvd,
            TORcvd
        }
        private CTTOStates CTTOState;
        /// <summary>
        /// Tone/CTSS setting
        /// </summary>
        public override ToneCTSSValue ToneCTSS
        {
            get
            {
                return base.ToneCTSS;
            }
            set
            {
                switch (value)
                {
                    case ToneCTSSValue.Off:
                        Callouts.safeSend(BldCmd(kcmdTO + "0"));
                        break;
                    case ToneCTSSValue.Tone:
                        Callouts.safeSend(BldCmd(kcmdTO + "1"));
                        break;
                    case ToneCTSSValue.CTSS:
                        Callouts.safeSend(BldCmd(kcmdCT + "1"));
                        break;
                    case ToneCTSSValue.CrossTone:
                        Callouts.safeSend(BldCmd(kcmdCT + "2"));
                        break;
                }
            }
        }
        /// <summary>
        /// Tone frequency
        /// </summary>
        public override float ToneFrequency
        {
            get
            {
                return base.ToneFrequency;
            }
            set
            {
                string s = kToneCTSSFreq(value);
                Callouts.safeSend(BldCmd(kcmdTN + s));
            }
        }
        /// <summary>
        /// CTSS frequency
        /// </summary>
        public override float CTSSFrequency
        {
            get
            {
                return base.CTSSFrequency;
            }
            set
            {
                string s = kToneCTSSFreq(value);
                Callouts.safeSend(BldCmd(kcmdCN + s));
            }
        }

        public override int KeyerSpeed
        {
            get
            {
                return base.KeyerSpeed;
            }
            set
            {
                Callouts.safeSend(BldCmd(kcmdKS + value.ToString("d3")));
            }
        }

        public override int TXAntenna
        {
            get
            {
                return base.TXAntenna;
            }
            set
            {
                // format is ANx99
                string val = (value+1).ToString("d1") + "99";
                Callouts.safeSend(BldCmd(kcmdAN + val));
            }
        }
        public override bool RXAntenna
        {
            get
            {
                return base.RXAntenna;
            }
            set
            {
                // format is AN9x9
                string val = (value) ? "919" : "909";
                Callouts.safeSend(BldCmd(kcmdAN + val));
            }
        }
        public override bool DriveAmp
        {
            get
            {
                return base.DriveAmp;
            }
            set
            {
                // format is AN99x
                string val = (value) ? "991" : "990";
                Callouts.safeSend(BldCmd(kcmdAN + val));
            }
        }

        public override AntTunerVals AntennaTuner
        {
            get
            {
                return base.AntennaTuner;
            }
            set
            {
                string val;
                // if tune is set, other values are "1".
                if ((value & AntTunerVals.tune) != 0) val = "111";
                else
                {
                    val = ((value & AntTunerVals.rx) != 0) ? "1" : "0";
                    val += ((value & AntTunerVals.tx) != 0) ? "10" : "00";
                }
                Callouts.safeSend(BldCmd(kcmdAC + val));
            }
        }

        public override RFAttenuatorVals RFAttenuator
        {
            get
            {
                return base.RFAttenuator;
            }
            set
            {
                // Format for setting is RAp1p1 (no p2).
                string val = (value == RFAttenuatorVals.on) ? "01" : "00";
                Callouts.safeSend(BldCmd(kcmdRA + val));
            }
        }

        public override PreAmpVals PreAmp
        {
            get
            {
                return base.PreAmp;
            }
            set
            {
                // format is PAp1 (p2 only appears on a query)
                string val = (value == PreAmpVals.on) ? "1" : "0";
                Callouts.safeSend(BldCmd(kcmdPA + val));
            }
        }

        public override int BreakinDelay
        {
            get
            {
                return base.BreakinDelay;
            }
            set
            {
                string val = value.ToString("d4");
                Callouts.safeSend(BldCmd(kcmdSD + val));
            }
        }

        /// <summary>
        /// VOX on/off.
        /// </summary>
        public override bool Vox
        {
            get
            {
                return base.Vox;
            }
            set
            {
                string val = (value) ? "1" : "0";
                Callouts.safeSend(BldCmd(kcmdVX + val));
            }
        }
        /// <summary>
        /// VOX delay
        /// </summary>
        public override int VoxDelay
        {
            get
            {
                return base.VoxDelay;
            }
            set
            {
                string val = value.ToString("d4");
                Callouts.safeSend(BldCmd(kcmdVD + val));
            }
        }

        /// <summary>
        /// VOX gain
        /// </summary>
        public override int VoxGain
        {
            get
            {
                return base.VoxGain;
            }
            set
            {
                string val = value.ToString("d3");
                Callouts.safeSend(BldCmd(kcmdVG + val));
            }
        }

        public override int MicGain
        {
            get
            {
                return base.MicGain;
            }
            set
            {
                string val = value.ToString("d3");
                Callouts.safeSend(BldCmd(kcmdMG + val));
            }
        }

        public override int CarrierLevel
        {
            get
            {
                return base.CarrierLevel;
            }
            set
            {
                string val = value.ToString("d3");
                Callouts.safeSend(BldCmd(kcmdCG + val));
            }
        }

        public override ProcessorStates ProcessorState
        {
            get
            {
                return base.ProcessorState;
            }
            set
            {
                Callouts.safeSend(BldCmd(kcmdPR + ((value == ProcessorStates.on) ? "1" : "0")));
            }
        }
        public override int ProcessorInputLevel
        {
            get
            {
                return base.ProcessorInputLevel;
            }
            set
            {
                string val = value.ToString("d3") + pOutLevel.ToString("d3");
                Callouts.safeSend(BldCmd(kcmdPL + val));
            }
        }
        public override int ProcessorOutputLevel
        {
            get
            {
                return base.ProcessorOutputLevel;
            }
            set
            {
                string val = pInLevel.ToString("d3") + value.ToString("d3");
                Callouts.safeSend(BldCmd(kcmdPL + val));
            }
        }

        private class cwSend
        {
            Kenwood parent;
            // See kcmdKY
            char[] buf;
            const int bufLen = 28;
            public const int MessageSize = bufLen - 4;
            const int initialCursorPos = 3; // zero relative
            public int Cursor = initialCursorPos;
            public cwSend(Kenwood p)
            {
                parent = p;
                parent.CWMessageSize = MessageSize;
                buf = new char[bufLen];
                // See kcmdKY
                buf[0] = 'K'; buf[1] = 'Y'; buf[2] = ' ';
                for (int i = initialCursorPos; i < bufLen - 1; i++) buf[i] = ' ';
                buf[bufLen - 1] = ';';
            }
            /// <summary>
            /// Add a character to the buffer.
            /// </summary>
            /// <param name="c">character</param>
            /// <returns>true if added, false if full.</returns>
            public bool Add(char c)
            {
                bool rv = (Cursor < bufLen-1); // keep ';' at end
                if (rv) buf[Cursor++] = c;
                return rv;
            }
            public void Send()
            {
                parent.Callouts.safeSend(new string(buf));
                // clear the buffer.
                while (Cursor > initialCursorPos)
                {
                    buf[--Cursor] = ' ';
                }
            }
        }
        private cwSend cwXMit;
        public override bool SendCW(char c)
        {
            bool rv = cwXMit.Add(c);
            if (!rv)
            {
                // buffer is full.  Send, then add.
                cwXMit.Send();
                cwXMit.Add(c);
            }
            // Always send if it's a space.
            if (c == ' ') cwXMit.Send();
            return true;
        }
        public override bool SendCW(string str)
        {
            bool rv = true;
            foreach (char c in str)
            {
                rv = SendCW(c);
                if (!rv) break;
            }
            return rv;
        }

        public Kenwood()
        {
            memThread = null;
            LoadRtn = loadMem;
            GetRtn = getMem;
            SetRtn = setMem;
            Memories = new MemoryGroup(totalMemories, this);
            powerLock = new Mutex();
            powerCount = PowerCountThreshhold;
            CTTOState = CTTOStates.none;
            setupResponseActions();
            cwXMit = new cwSend(this);
        }

        /// <summary>
        /// Open the radio
        /// </summary>
        /// <returns>True on success </returns>
        public override bool Open(OpenParms p)
        {
            // the com port should be open.
            bool rv = base.Open(p);
            // Start the radio output processor.
            pool = new bufData();
            poolThread = new Thread(new ThreadStart(poolHandler));
            poolThread.Start();
            IsOpen = rv;
            return rv;
        }

        public override void close()
        {
            // Careful if we're acquiring memories!
            if ((memThread != null) && memThread.IsAlive)
            {
                try
                {
                    memThread.Abort();
                }
                catch { }
            }
            if ((MRThread != null) && MRThread.IsAlive)
            {
                try { MRThread.Abort(); }
                catch { }
            }
            // Careful if we're acquiring menus!
            if ((menuThread != null) && menuThread.IsAlive)
            {
                try
                {
                    menuThread.Abort();
                }
                catch { }
            }
            if ((poolThread != null) && poolThread.IsAlive)
            {
                try { poolThread.Abort(); }
                catch { }
            }
            base.close(); // resets IsOpen.
        }

        private static string[] statCommands =
        {
            kcmdAC,
            kcmdAN,
            kcmdCG,
            kcmdCN,
            kcmdCT,
            kcmdDA,
            kcmdIF,
            kcmdKS,
            kcmdMG,
            kcmdPA,
            kcmdPC,
            kcmdPL,
            kcmdPR,
            kcmdRA,
            kcmdTN,
            kcmdTO,
            kcmdVD,
            kcmdVG,
            kcmdVX,
            kcmdXI
        };
        private byte powerOnForRigstat;
        private void rigStat(bool ckPower)
        {
            // Get the radio's status.
            //DBGWL("rigStat");
            if (ckPower)
            {
                Thread.VolatileWrite(ref powerOnForRigstat, 0);
                Callouts.safeSend(BldCmd(kcmdPS));
                // await a response up to .5 seconds.
                int sanity = 20;
                while ((Thread.VolatileRead(ref powerOnForRigstat) == 0) && (sanity-- > 0))
                {
                    Thread.Sleep(25);
                }
                if (Thread.VolatileRead(ref powerOnForRigstat) == 0)
                {
                    // Quit here.  Note that rigstat is called again from PowerCheck().
                    //DBGWL("rigstat:power not on");
                    return;
                }
            }
            Callouts.safeSend(BldCmd(SMHdr)); // because it's a property.
            foreach (string cmd in statCommands)
            {
                Callouts.safeSend(BldCmd(cmd));
            }
            GetMemories();
            getMenus();
        }
        protected override void ToggleContinuous()
        {
            try
            {
                base.ToggleContinuous();
                if (ContinuousMonitoring)
                {
                    // start continuous output.
                    Callouts.safeSend(BldCmd(kcmdAI+"2"));
                    // get status
                    rigStat(true);
                }
                else
                {
                    // If now off, stop continuous output.
                    Callouts.safeSend(BldCmd(kcmdAI+"0"));
                }
            }
            catch { }
        }

        private const int totalMemories = 110;
        /// <summary>
        /// Load the specified memory from the radio.
        /// </summary>
        /// <param name="m">the memory object.  The number must be set.</param>
        protected virtual void loadMem(MemoryData m)
        {
            Callouts.safeSend(BldCmd(kcmdMR + "0" + m.Number.ToString("d3")));
        }
        /// <summary>
        /// Get the data into the specified memory.
        /// </summary>
        /// <param name="m">memory object in the memories group, the number must be set.</param>
        /// <returns>true if gotten successfully.</returns>
        protected bool getMem(MemoryData m)
        {
            //DBGWL("getMem:start, locking" + m.Number.ToString());
            int sanity = 10;
            bool loadIt = true;
            bool rv;
            m.myLock.WaitOne();
            while (!m.complete && (sanity-- > 0))
            {
                // load once only.
                if (loadIt)
                {
                    loadMem(m);
                    loadIt = false;
                }
                // await response, up to .5 seconds, see sanity.
                m.myLock.ReleaseMutex();
                Thread.Sleep(50);
                m.myLock.WaitOne();
            }
            rv = m.complete;
            m.myLock.ReleaseMutex();
            //DBGWL("getMem:returning " + rv.ToString());
            return rv;
        }
        /// <summary>
        /// Set the radio's memory.
        /// </summary>
        /// <param name="m">a memoryData object</param>
        protected void setMem(MemoryData m)
        {
            // Fix the name if too long.
            if ((m.Name != null) && (m.Name.Length > 8))
                m.Name = m.Name.Substring(0, 8);
            // Get the memory object to lock.
            MemoryData mg = Memories[m.Number];
            string memnoStr = m.Number.ToString("d3");
            mg.myLock.WaitOne();
            // if the present flag isn't set, it's a deletion.
            if (!m.Present)
            {
                m.Frequency[0] = 0;
                m.Mode[0] = Modes.None;
                m.DataMode[0] = DataModes.off;
                m.Split = false;
                m.Type = MemoryTypes.Normal;
            }
            // Send the MW0.
            string mem0str = kcmdMW + "0" + memnoStr + setFreqToneData(m, 0) +
                ((int)m.ToneCTSS).ToString("d1") +
                kToneCTSSFreq(m.ToneFrequency) +
                kToneCTSSFreq(m.CTSSFrequency) +
                "000" + "0" + "0" + "000000000" +
                ((int)m.FMMode).ToString("d2") +
                ((m.Lockout) ? '1' : '0') +
                m.Name;
            DBGWL("smw0:sending " + mem0str);
            Callouts.safeSend(BldCmd(mem0str));
            // See if need to send an MW1.
            if (m.Split || (m.Type == MemoryTypes.Range))
            {
                string mem1str = kcmdMW + "1" + memnoStr + setFreqToneData(m, 1) +
                    "0000000000000000000000" + m.Name;
                DBGWL("smw1:sending " + mem1str);
                Callouts.safeSend(BldCmd(mem1str));
            }
            mg.myLock.ReleaseMutex();
            // Finally, get the updated memory from the rig.
            Callouts.safeSend(BldCmd(kcmdMR + "0" + memnoStr));
        }
        private string setFreqToneData(MemoryData m, int id)
        {
            string rv = m.Frequency[id].ToString("d11") +
                KMode(m.Mode[id]) + ((int)m.DataMode[id]).ToString("d1");
            return rv;
        }

        private Thread memThread;
        private byte memThreadRunning = 0;
        protected override void GetMemories()
        {
            base.GetMemories();
#if GetMemoriez
            if (Thread.VolatileRead(ref memThreadRunning) == 0)
            {
                Thread.VolatileWrite(ref memThreadRunning, 1);
                memThread = new Thread(new ThreadStart(CollectMemories));
                memThread.Start();
            }
#endif
        }
        protected virtual void CollectMemories()
        {
            //DBGWL("collectMem:started");
            for (int i = 0; i < totalMemories; i++)
            {
                // Just quit if ContinuousMonitoring gets turned off.
                if (!ContinuousMonitoring)
                {
                    //DBGWL("MRThread:break");
                    break;
                }
                MemoryData m = Memories.mems[i];
                // It's not a big deal if we request a memory that's being requested already.
                if (!m.complete)
                {
                    // Get the memory's first part.
                    loadMem(m);                    
                    // Wait up to .5 seconds or until it's complete.
                    int sanity = 10;
                    while (!m.complete && (sanity-- > 0))
                    {
                        Thread.Sleep(50);
                    }
                }
            }
            // Report complete.
            raiseComplete(CompleteEvents.memories);

#if MemoryDebug
            foreach (MemoryData m in Memories)
            {
                debugMemoryData(m);
            }
#endif

            // Indicate thread complete.
            Thread.VolatileWrite(ref memThreadRunning, 0);
        }
        private void debugMemoryData(MemoryData m)
        {
#if MemoryDebug
          //DBGW("memory " + m.Number.ToString());
            if (!m.Present)
            {
              //DBGWL(" empty");
            }
            else
            {
              //DBGW(" type=" + m.Type.ToString() + " split=" + m.Split.ToString());
              //DBGW(" freq=" + m.Frequency[0].ToString() + " " + m.Frequency[1].ToString());
              //DBGW(" mode=" + m.Mode[0].ToString() + " " + m.Mode[1].ToString());
              //DBGW(" DataMode=" + m.DataMode[0].ToString() + " " + m.DataMode[1].ToString());
              //DBGW(" ToneCTSS=" + m.ToneCTSS.ToString());
              //DBGW(" name=" + m.Name);
              //DBGWL("");
            }
#endif
        }

        private Thread menuThread;
        protected virtual void getMenus()
        {
#if GetMenuz
            // Not all rigs have menus.
            if (Menus != null)
            {
                menuThread = new Thread(new ThreadStart(collectMenus));
                menuThread.Start();
            }
#endif
        }
        private void collectMenus()
        {
            for (int i = 0; i < NumberOfMenus; i++)
            {
                // Just quit if continuous monitoring is off.
                if (!ContinuousMonitoring) return;
                // getrtn reads the menu and returns it's value, which we don't need.
                Menus[i].getRtn(Menus[i]);
#if zero
                Callouts.safeSend(BldCmd(kcmdEX + i.ToString("d3") + "0000"));
                int sanity = 10;
                // Wait until menu is read in.
                while (!Menus[i].Complete && (sanity-- > 0))
                {
                    waitMS(8);
                }
#endif
            }
            // Report complete.
            raiseComplete(CompleteEvents.menus);
        }
        public void SetMenu(int id, object val)
        {
        }

        protected const string ZPad = "00000000000";
        protected string UFreqToString(ulong u)
        {
            string str = u.ToString();
            if (str.Length >= 11) return str;
            return ZPad.Substring(0, 11 - str.Length) + str;
        }

        protected char VFOToLetter(RigCaps.VFOs v)
        {
            switch (v)
            {
                case RigCaps.VFOs.VFOA: return 'A';
                case RigCaps.VFOs.VFOB: return 'B';
                default: return ' ';
            }
        }
        protected RigCaps.VFOs letterToVFO(char c)
        {
            RigCaps.VFOs rv;
            switch (c)
            {
                case 'A': rv = RigCaps.VFOs.VFOA; break;
                case 'B': rv = RigCaps.VFOs.VFOB; break;
                default: rv = RigCaps.VFOs.None; break;
            }
            return rv;
        }

        protected void contFR(string cmd)
        {
            int i;
            try
            {
                // Receive VFO.
                System.Int32.TryParse(cmd.Substring(2, 1), out i);
                rxV = (i < 2)?(RigCaps.VFOs)i:RigCaps.VFOs.None;
                Callouts.safeSend(BldCmd(kcmdIF));
            }
            catch { }
        }
        protected void contFT(string cmd)
        {
            int i;
            System.Int32.TryParse(cmd.Substring(2, 1), out i);
            txV = (i < 2)?(RigCaps.VFOs)i: RigCaps.VFOs.None;
            if (TXVFO != RigCaps.VFOs.None)
            {
                // It's a VFO, not memory.
                // If a memory, we'd have gotten an FR also, which issues
                // an "IF" in any case.
                // The "IF" will issue the "XI".
            }
        }

        protected virtual void contTS(string str)
        {
            try
            {
                // We won't set this if the split flag isn't on.
                // This can happen if we're using the XIT.
                TFSetOn = (Split && (str[2] == '1')) ? true : false;
            }
            catch { }
        }

        protected virtual void contFreqA(string str)
        {
            //DBGWL("contFreqA:" + str);
            try
            {
                freqVFO(RigCaps.VFOs.VFOA, str.Substring(2, 11));
            }
            catch { }
        }
        protected virtual void contFreqB(string str)
        {
            //DBGW("contFreqB:");
            try
            {
                freqVFO(RigCaps.VFOs.VFOB, str.Substring(2, 11));
            }
            catch { }
            //DBGWL("");
        }
        protected virtual void contXI(string str)
        {
            //DBGW("contXI:");
            try
            {
                int ofst=2;
                if (!System.UInt64.TryParse(str.Substring(ofst, 11), out TXFreq)) return;
                ofst += 11;
                //DBGW("freq=" + TXFreq.ToString());
                txMd = getMode(str[ofst++]);
                txDM = getDataMode(str[ofst++]);
            }
            catch
            {
                //DBGW("XI try failed");
            }
          //DBGWL("");
        }
        protected virtual void freqVFO(RigCaps.VFOs v, string f)
        {
            ulong u;
            if (System.UInt64.TryParse(f, out u))
            {
                //DBGWL("freqVFO:v=" + v.ToString() + " freq=" + u.ToString() + " txvfo=" + TXVFO.ToString() + "rxvfo=" + RXVFO.ToString());
                if (v == TXVFO) TXFreq = u;
                if (v == RXVFO) RXFreq = u;
            }
        }
        protected virtual void contSM(string str)
        {
            //DBGWL("contsm:" + str);
            try
            {
                int i;
                int pos = SMHdr.Length; // This can vary depending on the model.
                System.Int32.TryParse(str.Substring(pos, 4), out i);
                SMtr = i;
            }
            catch { }
        }

        protected void contPS(string cmd)
        {
            //DBGW("contPS:");
            try
            {
                bool cmdOut = (cmd[2] == '1');
                if (cmdOut)
                {
                    // power is on.
                    powerLock.WaitOne(); // short term!
                    powerCount = 0;
                    powerLock.ReleaseMutex();
                    Thread.VolatileWrite(ref powerOnForRigstat, 1);
                    //DBGW(" tell powerCheck and RigStat it's on");
                }
            }
            catch { }
            //DBGWL("");
        }

        protected virtual void contIF(string str)
        {
            //DBGWL("contIF:" + str);
            //DBGW("contif:");
            int ofst = 2; // frequency offset
            try
            {
                ulong freq;
                int i;
                RigCaps.VFOs ovfo; // other VFO.
                bool mem; // memory mode
                string wkstr = str.Substring(ofst, 11);
                //DBGW(wkstr + " ");
                if (!System.UInt64.TryParse(wkstr, out freq)) return;
                ofst += 11 + 5;
                string rs = str.Substring(ofst, 5);
                RIT.Value = stringToRIT(rs);
                XIT.Value = stringToRIT(rs);
                //DBGW("RIT=" + RIT.Value.ToString());
                ofst += 5;
                RIT.Active = (str.Substring(ofst++, 1) == "1");
                XIT.Active = (str.Substring(ofst++, 1) == "1");
                //DBGW(" ra=" + RIT.Active.ToString() + " xa=" + XIT.Active.ToString());
                memChannel = getMemoryChannel(str.Substring(ofst, 3));
                ofst += 3; // mem channel
                //DBGW(" memchan=" + CurrentMemoryChannel.ToString());
                XMit = (str.Substring(ofst++, 1) == "1");
                //DBGW(" xmit=" + XMit.ToString());
                if (XMit) TXFreq = freq;
                else RXFreq = freq;
                Modes md = getMode(str[ofst++]);
                //DBGW(" mode=" + md.ToString());
                if (XMit) txMd = md;
                else rxMd = md;
                i = str[ofst++] - '0';
                if ((i >= 0) && (i <= 2))
                {
                    // For a VFO:
                    if (i < 2)
                    {
                        mem = false;
                        // set current VFO
                        ovfo = (RigCaps.VFOs)((i + 1) % 2); // the other VFO.
                        if (XMit) txV = (RigCaps.VFOs)i;
                        else rxV = (RigCaps.VFOs)i;
                    }
                    else
                    {
                        // Memory mode.
                        mem = true;
                        ovfo = rxV = txV = RigCaps.VFOs.None;
                    }
                }
                else return;
                ofst += 1; // Scan status.
                bool Splt = (str.Substring(ofst++, 1) == "1");
                //DBGW(" split=" + Split.ToString());
                // Set the other VFO/freq.
                if (Splt)
                {
                    if (!mem)
                    {
                        // Split VFOs.
                        if (XMit) rxV = (RigCaps.VFOs)ovfo;
                        else txV = (RigCaps.VFOs)ovfo;
                    } // else other vfo already set to None.
                    // Get xmit frequency.
                    if (!XMit) Callouts.safeSend(BldCmd(kcmdXI));
                }
                else
                {
                    // symplex
                    if (XMit)
                    {
                        rxV = TXVFO;
                        RXFreq = TXFreq;
                    }
                    else
                    {
                        txV = RXVFO;
                        TXFreq = RXFreq;
                    }
                }
                //DBGW(" rxfreq=" + RXFreq.ToString());
                //DBGW(" txfreq=" + TXFreq.ToString());
                toneCT = getToneCTSS(str[ofst++]);
                float tcf = ToneCTSSToFreq(str.Substring(ofst, 2));
                if ((toneCT == ToneCTSSValue.Off) || (toneCT == ToneCTSSValue.Tone))
                    toneFreq = tcf;
                else if (toneCT == ToneCTSSValue.CTSS)
                    CTSSFreq = tcf;
                else{
                    // cross tone
                    if (XMit) toneFreq =tcf;
                    else CTSSFreq = tcf;
                }
                ofst += 2;
                //DBGW(" ToneCTSS=" + toneCT.ToString() + " tonectssFreq=" + tcf.ToString());
                //DBGWL("");
            }
            catch { DBGWL("contIF Exception!"); }
        }
        private int stringToRIT(string str)
        {
            int i;
            if (System.Int32.TryParse(str.Substring(1),out i))
            {
                return (str[0] == '-') ? -i : i;
            }
            else return 0;
        }
        protected virtual void contRXTX(string str)
        {
            try
            {
                XMit = (str.Substring(0, 2) == "TX");
            }
            catch { }
        }
        Thread memGetThread;
        protected virtual void contMC(string str)
        {
            //DBGW("contMC:");
            try
            {
                memChannel = getMemoryChannel(str.Substring(2, 3));
                //DBGWL(" channel=" + CurrentMemoryChannel.ToString());
                if (RXVFO == RigCaps.VFOs.None)
                {
                    // We're in memory mode.
                    // However, the memory might not be in our collection yet!
                    // Use a thread to collect it, and get off the interrupt level.
                    memGetThread = new Thread(new ParameterizedThreadStart(getThisMemory));
                    int ch = CurrentMemoryChannel;
                    memGetThread.Start(ch);
                }
            }
            catch { }
        }
        /// <summary>
        /// Get a memory number from this string.
        /// </summary>
        /// <param name="str">memory string</param>
        /// <returns>memory number</returns>
        protected virtual int getMemoryChannel(string str)
        {
            // The memory channel might be " nn".
            try
            {
                char c = (str[0] == ' ') ? '0': str[0];
                string s = c.ToString() + str.Substring(1);
                int i = 0;
                System.Int32.TryParse(s, out i);
                return i;
            }
            catch { return 0; }
        }
        /// <summary>
        /// Request the current memory from the rig.
        /// </summary>
        private void getThisMemory(object o)
        {
            MemoryData m = Memories.mems[(int)o];
            if (getMem(m)) ActuateMemory(m);
        }
        protected virtual void contRIT(string str)
        {
            try
            {
                RIT.Active = (str.Substring(2, 1) == "1") ? true : false;
            }
            catch { }
        }
        protected virtual void contXIT(string str)
        {
            try
            {
                XIT.Active = (str.Substring(2, 1) == "1") ? true : false;
            }
            catch { }
        }
        protected virtual void contMD(string str)
        {
            try
            {
                Modes m = getMode(str[2]);
                if (Transmit) txMd = m;
                else rxMd = m;
            }
            catch { }
        }

        private Modes[] modeTable =
            {
                Modes.None, Modes.lsb, Modes.usb, Modes.cw,
                Modes.fm, Modes.am, Modes.fsk, Modes.cwr, Modes.None,
                Modes.fskr
            };
        /// <summary>
        /// Kenwood mode character to internal mode.
        /// </summary>
        /// <param name="c">ASCII character</param>
        /// <returns>modes value</returns>
        protected virtual Modes getMode(char c)
        {
            c -= '0';
            return ((c>=0) && (c<modeTable.Length)) ? modeTable[c] : Modes.None;
        }
        /// <summary>
        /// Get mode character to send to the rig.
        /// </summary>
        /// <param name="m">modes value</param>
        /// <returns>mode character</returns>
        protected virtual char KMode(Modes m)
        {
            int i;
            for (i = 0; ((i < modeTable.Length) && (m != modeTable[i])); i++) { }
            return (char)(i + '0');
        }

        protected virtual void contDA(string str)
        {
            try
            {
                DataModes dm = getDataMode(str[2]);
                if (Transmit) txDM = dm;
                else rxDM = dm;
            }
            catch { }
        }
        protected virtual DataModes getDataMode(char c)
        {
            return (c == '1') ? DataModes.on : DataModes.off;
        }
        protected char kDataMode(DataModes d)
        {
            return (char)((int)d + '0');
        }

        ToneCTSSValue[] toneCTSSTable =
            {
                ToneCTSSValue.Off, ToneCTSSValue.Tone,
                ToneCTSSValue.CTSS, ToneCTSSValue.CrossTone
            };
        protected virtual ToneCTSSValue getToneCTSS(char c)
        {
            c -= '0';
            return ((c >= 0) && (c < toneCTSSTable.Length)) ? 
                toneCTSSTable[c] : ToneCTSSValue.Off;
        }
        protected virtual char kToneCTSS(ToneCTSSValue t)
        {
            int i;
            for (i = 0; (i < toneCTSSTable.Length) && (t != toneCTSSTable[i]); i++) { }
            return (i<toneCTSSTable.Length)? (char)(i+'0'): ' ';
        }

        private Thread MRThread;
        protected virtual void contMR(string str)
        {
            // We have to perform this under a thread because of the locking.
            MRThread = new Thread(new ParameterizedThreadStart(MRThreadProc));
            MRThread.Start(str);
        }
        protected virtual void MRThreadProc(object o)
        {
            string str = o.ToString();
            DBGWL("MRThread:" + str);
            // if rxtx and frequency are 0, all done, not present.
            // frequency, mode, and dataMode come from rxtx 0 and 1.
            // If rxtx is 0, send out the MR1 command.
            // otherwise indicate complete.

            MemoryData m = null;
            try
            {
                int ofst = 2;
                int rxtx;
                string memNoString;
                rxtx = str[ofst++] - '0';
                char c = str[ofst++];
                if (c == ' ') c = '0';
                memNoString = c.ToString() + str.Substring(ofst, 2);
                ofst += 2;
                //DBGW(" rxtx=" + rxtx.ToString() + " memNo=" + memNoString);
                int memNo;
                if (!System.Int32.TryParse(memNoString, out memNo)) goto memDone;
                m = Memories.mems[memNo];
                //DBGWL("MRThread:locking " + memNoString);
                m.myLock.WaitOne();
                //DBGWL("MRThread got " + memNoString);
                if (rxtx == 0)
                {
                    m.Type = (c == '0') ? MemoryTypes.Normal : MemoryTypes.Range;
                    m.Number = memNo;
                }
                string wkstr;
                wkstr = str.Substring(ofst, 11);
                if (!System.UInt64.TryParse(wkstr, out m.Frequency[rxtx])) goto memDone;
                ofst += 11;
                //DBGW(" freq=" + m.Frequency[rxtx].ToString());
                if ((rxtx == 0) && (m.Frequency[0] != 0)) m.Present = true;
                if (m.Present)
                {
                    m.Mode[rxtx] = getMode(str[ofst++]);
                    m.DataMode[rxtx] = getDataMode(str[ofst++]);
                    //DBGW(" mode=" + m.Mode[rxtx].ToString() + " dataMode=" + m.DataMode[rxtx].ToString());
                    if (rxtx == 0)
                    {
                        m.ToneCTSS = getToneCTSS(str[ofst++]);
                        m.ToneFrequency = ToneCTSSToFreq(str.Substring(ofst, 2));
                        ofst += 2;
                        m.CTSSFrequency = ToneCTSSToFreq(str.Substring(ofst, 2));
                        ofst += 2;
                        ofst += 3; // p10
                        ofst += 1; // p11
                        ofst += 1; // p12
                        ofst += 9; // p13
                        int i;
                        if (!System.Int32.TryParse(str.Substring(ofst, 2), out i)) goto memDone;
                        m.FMMode = getFMMode(i);
                        ofst += 2;
                        m.Lockout = (str[ofst++] == '1');
                        if (ofst < str.Length)
                        {
                            m.Name = str.Substring(ofst);
                            //DBGW(" name=" + m.Name);
                        }

                        // Get the other part of this memory.
                        Callouts.safeSend(BldCmd(kcmdMR + "1" + memNoString));
                    }
                    else
                    {
                        // This memory is complete now.
                        m.Split = ((m.Frequency[0] != m.Frequency[1]) ||
                            (m.Mode[0] != m.Mode[1]) ||
                            (m.DataMode[0] != m.DataMode[1]));
                        m.complete = true;
                    }
                }
                else
                {
                    // memory is empty but complete.
                    m.complete = true;
                }
            }
            catch (Exception ex)
            {
                DBGWL("MRThread exception" + ex.Message);
            }
        memDone:
            if (m != null)
            {
                //DBGWL("MRThread releasing " + m.Number.ToString());
                m.myLock.ReleaseMutex();
            }
        }
        protected virtual void contVox(string str)
        {
            try
            {
                vx = (str[2] == '1');
            }
            catch { }
        }
        protected virtual void contCT(string str)
        {
            try
            {
                switch (str[2])
                {
                    case '0':
                        // only set if TO not received.
                        if (CTTOState == CTTOStates.none) itoneCT = ToneCTSSValue.Off;
                        break;
                    case '1': itoneCT = ToneCTSSValue.CTSS; break;
                    case '2': itoneCT = ToneCTSSValue.CrossTone; break;
                }
                if (CTTOState == CTTOStates.TORcvd)
                {
                    // The pair is complete.
                    toneCT = itoneCT; // make the change.
                    CTTOState = CTTOStates.none;
                }
                else CTTOState = CTTOStates.CTRcvd; // preceeded the TO.
                //DBGWL("contCT:itoneCT=" + itoneCT.ToString() + " CTTOState=" + CTTOState.ToString());
            }
            catch { }
        }
        protected virtual void contCN(string str)
        {
            try
            {
                CTSSFreq = ToneCTSSToFreq(str.Substring(2, 2));
            }
            catch { }
        }
        protected virtual void contTO(string str)
        {
            try
            {
                switch (str[2])
                {
                    case '0':
                        // Only set if no CT.
                        if (CTTOState == CTTOStates.none) itoneCT = ToneCTSSValue.Off;
                        break;
                    case '1':
                        itoneCT = ToneCTSSValue.Tone;
                        break;
                }
                if (CTTOState == CTTOStates.CTRcvd)
                {
                    // The pair is complete.
                    toneCT = itoneCT;
                    CTTOState = CTTOStates.none;
                }
                else CTTOState = CTTOStates.TORcvd;
                //DBGWL("contTO:itoneCT=" + itoneCT.ToString() + " CTTOState=" + CTTOState.ToString());
            }
            catch { }
        }
        protected virtual void contTN(string str)
        {
            try
            {
                toneFreq = ToneCTSSToFreq(str.Substring(2, 2));
            }
            catch { }
        }
        /// <summary>
        /// Get corresponding Tone/CTSS frequency
        /// </summary>
        /// <param name="str">2-character string</param>
        /// <returns>frequency as a float</returns>
        protected virtual float ToneCTSSToFreq(string str)
        {
            try
            {
                int t = 0;
                System.Int32.TryParse(str, out t);
                return ToneFrequencyTable[t];
            }
            catch { return 0; }
        }
        /// <summary>
        /// Convert the frequency to a 2-digit string for a command.
        /// </summary>
        /// <param name="f">tone frequency as a float</param>
        /// <returns>2-character string</returns>
        protected virtual string kToneCTSSFreq(float f)
        {
            int i;
            for (i = 0; (i < ToneFrequencyTable.Length) && (f != ToneFrequencyTable[i]); i++) { }
            return (i < ToneFrequencyTable.Length) ? i.ToString("d2") : "00";
        }

        FMModes[] FMModeTable = { FMModes.Normal, FMModes.Narrow };
        protected virtual FMModes getFMMode(int i)
        {
            try
            {
                return FMModeTable[i];
            }
            catch { return FMModes.Normal; }
        }

        protected virtual void contKS(string str)
        {
            try
            {
                int i;
                if (System.Int32.TryParse(str.Substring(2), out i))
                    ksp = i;
            }
            catch { }
        }
        protected virtual void contEX(string str)
        {
            // implemented by specific models for now.
        }
        protected virtual void contAN(string str)
        {
            try
            {
                txAnt = (str[2] == '2') ? 1 : 0; // value is 1-based.
                rxAnt = (str[3] == '1') ? true : false;
                drvA = (str[4] == '1') ? true : false;
            }
            catch { }
        }
        protected virtual void contAC(string str)
        {
            try
            {
                atuner = ((str[2] == '1') ? AntTunerVals.rx : 0) |
                    ((str[3] == '1') ? AntTunerVals.tx : 0) |
                    ((str[4] == '1') ? AntTunerVals.tune : 0);
            }
            catch { }
        }
        protected virtual void contRA(string str)
        {
            try
            {
                // format is RAp1p1p2p2 where p2 is 00.
                RFAtt = (str[3] == '1') ? RFAttenuatorVals.on : RFAttenuatorVals.off;
            }
            catch { }
        }

        protected virtual void contPA(string str)
        {
            try
            {
                // format is PAp1p2 (p2 always 0)
                preA = (str[2] == '1') ? PreAmpVals.on : PreAmpVals.off;
            }
            catch { }
        }

        protected virtual void contSD(string str)
        {
            try { System.Int32.TryParse(str.Substring(2), out bDelay); }
            catch { }
        }

        protected virtual void contVD(string str)
        {
            try { System.Int32.TryParse(str.Substring(2), out vDelay); }
            catch { }
        }

        protected virtual void contVG(string str)
        {
            try { System.Int32.TryParse(str.Substring(2), out vGain); }
            catch { }
        }

        protected virtual void contMG(string str)
        {
            try { System.Int32.TryParse(str.Substring(2),out mGain); }
            catch { }
        }

        protected virtual void contCG(string str)
        {
            try { System.Int32.TryParse(str.Substring(2),out cLevel); }
            catch { }
        }

        protected virtual void contPL(string str)
        {
            try
            {
                System.Int32.TryParse(str.Substring(2, 3), out pInLevel);
                System.Int32.TryParse(str.Substring(5, 3), out pOutLevel);
            }
            catch { }
        }
        protected virtual void contPR(string str)
        {
            try
            {
                pState = (str[2] == '1') ? ProcessorStates.on :
                  ProcessorStates.off;
            }
            catch { }
        }
    }
}
