#define dontSkipMemoryDebug
#define GetMemoriez
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
    public class Kenwood : AllRadios
    {
        // Commands to send to Kenwood rigs.
        const string kcmdAC = "AC"; // Sets or reads the internal antenna tuner status.
        const string kcmdAG = "AG"; // Sets or reads the AF gain.
        const string kcmdAI = "AI"; // Sets or reads the Auto Information (AI) function ON/ OFF.
        const string kcmdAN = "AN"; // Selects the antenna connector ANT1/ ANT2.
        const string kcmdAS = "AS"; // Sets or reads the Auto Mode function parameters.
        const string kcmdBC = "BC"; // Sets or reads the Beat Cancel function status.
        const string kcmdBD = "BD"; // Sets a frequency band.
        const string kcmdBP = "BP"; // Adjusts the Notch Frequency of the Manual Notch Filter.
        const string kcmdBU = "BU"; // Sets a frequency band.
        const string kcmdBY = "BY"; // Reads the busy signal status.
        const string kcmdCA = "CA"; // Sets and reads the CW TUNE function status.
        const string kcmdCG = "CG"; // Sets and reads the Carrier Level.
        const string kcmdCH = "CH"; // Operate the MULTI/CH encoder.
        const string kcmdCN = "CN"; // Sets and reads the CTCSS frequency.
        const string kcmdCT = "CT"; // Sets and reads the CTCSS function status.
        const string kcmdDA = "DA"; // Sets and reads the DATA mode.
        const string kcmdDN = "DN"; // Emulates the microphone DWN and UP keys.
        const string kcmdEM = "EM"; // Sets the Emergency communication frequency mode.
        const string kcmdEX = "EX"; // Sets or reads the Menu.
        const string kcmdFA = "FA"; // Sets or reads the VFO A frequency.
        const string kcmdFB = "FB"; // Sets or reads the VFO B frequency.
        const string kcmdFL = "FL"; // Sets and reads the IF filter.
        const string kcmdFR = "FR"; // Selects or reads the VFO or Memory channel.
        const string kcmdFS = "FS"; // Sets and reads the Fine Tuning function status.
        const string kcmdFT = "FT"; // Selects or reads the VFO or Memory channel.
        const string kcmdFV = "FV"; // Verifies the Firmware version.
        const string kcmdFW = "FW"; // Sets or reads the DSP filtering bandwidth.
        const string kcmdGC = "GC"; // Sets or reads the AGC.
        const string kcmdGT = "GT"; // Sets or reads the AGC time constant.
        const string kcmdID = "ID"; // Reads the transceiver ID number.
        const string kcmdIF = "IF"; // Reads the transceiver status. I
        const string kcmdIS = "IS"; // Sets and reads the DSP Filter Shift.
        const string kcmdKS = "KS"; // Sets and reads the Keying speed.
        const string kcmdKY = "KY"; // Converts the entered characters into morse code while keying.
        const string kcmdLK = "LK"; // Sets and reads the Lock status.
        const string kcmdLM = "LM"; // Sets and reads the VGS-1 electric keyer recording status.
        const string kcmdMC = "MC"; // Sets and reads the Memory Channel number.
        const string kcmdMD = "MD"; // Sets and reads the operating mode status.
        const string kcmdMF = "MF"; // Sets and reads Menu A or B.
        const string kcmdMG = "MG"; // Sets and reads the microphone gain.
        const string kcmdML = "ML"; // Sets and reads the TX Monitor function output level.
        const string kcmdMR = "MR"; // Reads the Memory channel data.
        const string kcmdMW = "MW"; // Sets the Memory channel data.
        const string kcmdNB = "NB"; // Sets and reads the Noise Blanker function status.
        const string kcmdNL = "NL"; // Sets and reads the Noise Blanker level.
        const string kcmdNR = "NR"; // Sets and reads the Noise Reduction function status.
        const string kcmdNT = "NT"; // Sets and reads the Notch Filter status.
        const string kcmdPA = "PA"; // Sets and reads the Pre-amplifier function status.
        const string kcmdPB = "PB"; // Sets and reads the voice and CW message playback status.
        const string kcmdPC = "PC"; // Sets and reads the output power.
        const string kcmdPL = "PL"; // Sets and reads the Speech Processor input/output level.
        const string kcmdPR = "PR"; // Sets and reads the Speech Processor function ON/ OFF.
        const string kcmdPS = "PS"; // Sets and reads the Power ON/ OFF status.
        const string kcmdQD = "QD"; // Deletes the Quick Memory.
        const string kcmdQI = "QI"; // Stores the settings in the Quick Memory.
        const string kcmdQR = "QR"; // Sets and reads the Quick Memory channel data.
        const string kcmdRA = "RA"; // Sets and reads the RF Attenuator status.
        const string kcmdRC = "RC"; // Clears the RIT/XIT frequency.
        const string kcmdRD = "RD"; // Sets and reads the RIT/XIT frequency Up/ Down. Also sets and reads the scan speed in Scan mode.
        const string kcmdRG = "RG"; // Sets and reads the RF Gain status.
        const string kcmdRL = "RL"; // Sets and reads the Noise Reduction Level.
        const string kcmdRM = "RM"; // Sets and reads the Meter function.
        const string kcmdRT = "RT"; // Sets and reads the RIT function status.
        const string kcmdRU = "RU"; // Sets and reads the RIT/XIT frequency Up/ Down. Also sets and reads the scan speed in Scan mode.
        const string kcmdRX = "RX"; // Sets the receiver function status.
        const string kcmdSC = "SC"; // Sets and reads the Scan function status.
        const string kcmdSD = "SD"; // Sets and reads the CW break-in time delay.
        const string kcmdSH = "SH"; // Sets and reads the slope tune bandwidth high setting.
        const string kcmdSL = "SL"; // Sets and reads the slope tune bandwidth low setting.
        const string kcmdSM = "SM"; // Reads the SMeter.
        // "SM" command varies according to the model.
        protected virtual string SMHdr
        {
            get { return kcmdSM; }
        }
        const string kcmdSQ = "SQ"; // Sets and reads the squelch value.
        const string kcmdSR = "SR"; // Resets the transceiver.
        const string kcmdSS = "SS"; // Sets and reads the Program Slow Scan frequency.
        const string kcmdSU = "SU"; // Sets and reads the Scan group.
        const string kcmdSV = "SV"; // Performs the Memory Transfer function.
        const string kcmdTN = "TN"; // Sets and reads the Tone frequency.
        const string kcmdTO = "TO"; // Sets and reads the Tone status.
        const string kcmdTS = "TS"; // Sets and reads the TF-Set status.
        const string kcmdTX = "TX"; // Sets the transmission mode.
        const string kcmdUP = "UP"; // Emulates the microphone DWN and UP keys.
        const string kcmdUR = "UR"; // Sets and reads the RX equalizer.
        const string kcmdUT = "UT"; // Sets and reads the TX equalizer.
        const string kcmdVD = "VD"; // Sets and reads the VOX Delay time.
        const string kcmdVG = "VG"; // Sets and reads the VOX Gain.
        const string kcmdVR = "VR"; // Sets the Voice synthesis generation function.
        const string kcmdVS0 = "VS0"; // Sets and reads the Visual Scan start/ stop/ pause status.
        const string kcmdVS1 = "VS1"; //  Sets the Visual Scan center frequency.
        const string kcmdVS2 = "VS2"; //  Sets the Visual Scan span.
        const string kcmdVS3 = "VS3"; //  Reads the Visual Scan upper/ lower/center frequency, and span.
        const string kcmdVS4 = "VS4"; //  Reads the Visual Scan sweep frequency and signal level.
        const string kcmdVV = "VV"; // Performs the VFO copy (A=B) function.
        const string kcmdVX = "VX"; // Sets and reads the VOX and Break-in function status.
        const string kcmdXI = "XI"; // Reads the transmit frequency and mode.
        const string kcmdXO = "XO"; // Sets and reads the offset direction and frequency for the transverter mode.
        const string kcmdXT = "XT"; // Sets and reads the XIT function status.

        protected string BldCmd(string cmd)
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
                Debug.WriteLine("Power set");
                if (value) Callouts.SendRoutine(BldCmd(kcmdPS+"1"));
                else Callouts.SendRoutine(BldCmd(kcmdPS+"0"));
            }
        }
        protected int powerCount;
        protected const int PowerCountThreshhold = 2;
        protected Mutex powerLock; // must be short term!
        public override void PowerCheck()
        {
            Debug.Write("PowerCheck:count=" + powerCount.ToString());
            bool sendAI = false;
            base.PowerCheck();
            powerLock.WaitOne();
            // See if we've received a response to the last requests.
            if (powerCount >= PowerCountThreshhold)
            {
                // looks like power is off.
                if (pwr)
                {
                    pwr = false;
                    Debug.Write(" turn off");
                }
                else
                {
                    // Power already off.  Hold the count here.
                    powerCount--;
                }
            }
            else
            {
                // power is on.
                if (!pwr)
                {
                    // it wasn't.
                    pwr = true;
                    sendAI = ContinuousMonitoring; // Turn cont output back on?
                    Debug.Write(" was off, turn on");
                }
            }
            powerCount++;
            powerLock.ReleaseMutex();
            if (sendAI)
            {
                Callouts.SendRoutine(BldCmd(kcmdAI + "2"));
                rigStat(false);
                Debug.Write(" send AI and stat");
            }
            Callouts.SendRoutine(BldCmd(kcmdPS));
            Debug.WriteLine("");
        }

        public override bool Transmit
        {
            get { return XMit; }
            set
            {
                Callouts.SendRoutine(kcmdTX);
            }
        }
        public override ulong RXFrequency
        {
            get { return RXFreq; }
            set
            {
                string str = "F" + VFOToLetter(RXVFO) + UFreqToString(value);
                Callouts.SendRoutine(BldCmd(str));
            }
        }
        public override ulong TXFrequency
        {
            get { return TXFreq; }
            set
            {
                string str = "F" + VFOToLetter(TXVFO) + UFreqToString(value);
                Callouts.SendRoutine(BldCmd(str));
            }
        }
        protected bool SplitFlag;
        // If the TF-Set function is on, we know we're split.
        protected bool Splt
        {
            get { return SplitFlag; }
            set
            {
                if (TFSetOn) SplitFlag = true;
                else SplitFlag = value;
            }
        }
        public override bool Split
        {
            get
            {
                return Splt;
            }
            set
            {
                if (RXVFO != RigCaps.VFOs.None)
                {
                    int v = (value) ? (((int)RXVFO + 1) % 2) : (int)RXVFO;
                    Callouts.SendRoutine(BldCmd(kcmdFT + v.ToString()));
                    Splt = value;
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
                Callouts.SendRoutine(BldCmd(kcmdMD + c.ToString()));
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
                Callouts.SendRoutine(BldCmd(kcmdMD + c.ToString()));
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
                Callouts.SendRoutine(BldCmd(kcmdDA + c.ToString()));
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
                Callouts.SendRoutine(BldCmd(kcmdDA + c.ToString()));
            }
        }

        public Kenwood()
        {
            cmdBuffer = "";
            memThread = null;
            SplitFlag = false;
            TFSetOn = false;
            collectHoldup = 0;
            powerLock = new Mutex();
            powerCount = PowerCountThreshhold;
        }

        /// <summary>
        /// Open the radio
        /// </summary>
        /// <returns>True on success </returns>
        public override bool Open(OpenParms p)
        {
            // the com port should be open.
            bool rv = base.Open(p);
            return rv;
        }

        public override void close()
        {
            base.close();
            // Careful if we're acquiring memories!
            if ((memThread != null) && memThread.IsAlive)
            {
                memThread.Abort();
                Memories.Clear();
            }
        }

        private void rigStat(bool ckPower)
        {
            // Get the radio's status.
            Debug.WriteLine("rigStat");
            if (ckPower) Callouts.SendRoutine(BldCmd(kcmdPS));
            Callouts.SendRoutine(BldCmd(kcmdIF));
            Callouts.SendRoutine(BldCmd(SMHdr));
            Callouts.SendRoutine(BldCmd(kcmdXI));
            Callouts.SendRoutine(BldCmd(kcmdVX));
            if (Memories.Count == 0) GetMemories();
        }
        protected override void ToggleContinuous()
        {
            try
            {
                base.ToggleContinuous();
                if (ContinuousMonitoring)
                {
                    // start continuous output.
                    Callouts.SendRoutine(BldCmd(kcmdAI+"2"));
                    // get status
                    rigStat(true);
                }
                else
                {
                    // If now off, stop continuous output.
                    Callouts.SendRoutine(BldCmd(kcmdAI+"0"));
                }
            }
            catch { }
        }

        const int totalMemories = 110;
        private Thread memThread;
        protected override void GetMemories()
        {
            base.GetMemories();
#if GetMemoriez
            memThread = new Thread(new ThreadStart(CollectMemories));
            memThread.Start();
#endif
        }
        private byte collectHoldup;
        protected virtual void CollectMemories()
        {
            Debug.Write("CollectMemories");
            for (int i = 0; i < totalMemories; i++)
            {
                // If holdup, sit here until it's off.
                while (Thread.VolatileRead(ref collectHoldup) != 0) { Thread.Sleep(50); }
                // Get the memory's first part.
                Callouts.SendRoutine(BldCmd(kcmdMR+"0" + i.ToString("d3")));
                Thread.Sleep(250);
            }

            // Finally, make sure power is still on, and invalidate memories if not.
            // We don't need to wait here, because if we lost power, we
            // won't get the responses from the rig anyway.
            if (!Power)
            {
                Debug.Write("power off");
                MemoryLock.WaitOne();
                Memories.Clear();
                MemoryLock.ReleaseMutex();
            }
            Debug.WriteLine("");

#if dontSkipMemoryDebug
            Thread.Sleep(5000);
            foreach (MemoryData m in Memories)
            {
                debugMemoryData(m);
            }
#endif
        }
        private void debugMemoryData(MemoryData m)
        {
#if dontSkipMemoryDebug
            Debug.Write("memory " + m.Number.ToString());
            if (!m.Present)
            {
                Debug.WriteLine(" empty");
            }
            else
            {
                Debug.Write(" type=" + m.Type.ToString() + " split=" + m.Split.ToString());
                Debug.Write(" freq=" + m.Frequency[0].ToString() + " " + m.Frequency[1].ToString());
                Debug.Write(" mode=" + m.Mode[0].ToString() + " " + m.Mode[1].ToString());
                Debug.Write(" DataMode=" + m.DataMode[0].ToString() + " " + m.DataMode[1].ToString());
                Debug.Write(" ToneCTSS=" + m.ToneCTSS.ToString());
                Debug.Write(" name=" + m.Name);
                Debug.WriteLine("");
            }
#endif
        }

        private string cmdBuffer;
        protected override void IHandler(object o)
        {
            Debug.Write("Interrupt:" + o.ToString());
            base.IHandler(o);
            if (ContinuousMonitoring)
            {
                // handle the output here.
                string str;
                try
                {
                    str = (string)o;
                    for (int i = 0; i < str.Length; i++)
                    {
                        char c = str[i];
                        switch (c)
                        {
                            case '?': continue;
                            case ';':
                                // cmd delimiter, handle cmd.
                                if (cmdBuffer.Length >= 2)
                                {
                                    string cmd = cmdBuffer; // Make a copy.
                                    Enq(cmd);
                                }
                                cmdBuffer = ""; // clear the buffer
                                break;
                            default:
                                // buffer this.
                                cmdBuffer += c;
                                break;
                        }
                    }
                }
                catch { }
            }
            if (!ContinuousMonitoring || ReportRaw)
            {
                // main program handles/receives output.
                Callouts.DirectDataReceiver(o);
            }
            Debug.WriteLine("");
        }

        protected override void CommandHandler(object o)
        {
            // We're running under the QThread.
            base.CommandHandler(o);
            string cmdBuf = o.ToString();
            Debug.WriteLine("CommandHandler: " + cmdBuf);
            switch (cmdBuf.Substring(0, 2))
            {
                case kcmdFA: contFreqA(cmdBuf); break;
                case kcmdFB: contFreqB(cmdBuf); break;
                case kcmdIF: contIF(cmdBuf); break;
                case kcmdSM: contSM(cmdBuf); break;
                case kcmdFR: contFR(cmdBuf); break;
                case kcmdFT: contFT(cmdBuf); break;
                case kcmdTS: contTS(cmdBuf); break;
                case kcmdRX: contRXTX(cmdBuf); break;
                case kcmdTX: contRXTX(cmdBuf); break;
                case kcmdXI: contXI(cmdBuf); break;
                case kcmdMC: contMC(cmdBuf); break;
                case kcmdRT: contRIT(cmdBuf); break;
                case kcmdXT: contXIT(cmdBuf); break;
                case kcmdMD: contMD(cmdBuf); break;
                case kcmdDA: contDA(cmdBuf); break;
                case kcmdMR: contMR(cmdBuf); break;
                case kcmdVX: contVox(cmdBuf); break;
                case kcmdCN: contCN(cmdBuf); break;
                case kcmdTN: contTN(cmdBuf); break;
                case kcmdPS: contPS(cmdBuf); break;
                default:
                    // the PS command might start with junk.
                    if ((cmdBuf.Length > 3) &&
                        (cmdBuf.Substring(cmdBuf.Length - 3, 2) == kcmdPS))
                    {
                        contPS(cmdBuf.Substring(cmdBuf.Length - 3));
                    }
                    break;
            }
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
                Callouts.SendRoutine(BldCmd(kcmdIF));
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
                Splt = (RXVFO != TXVFO);
                // The "IF" will issue the "XI".
            }
        }

        protected bool TFSetOn;
        protected virtual void contTS(string str)
        {
            try
            {
                // We won't set this if the split flag isn't on.
                // This can happen if we're using the XIT.
                TFSetOn = (SplitFlag && (str[2] == '1')) ? true : false;
            }
            catch { }
        }

        protected virtual void contFreqA(string str)
        {
            Debug.Write("contFreqA:");
            try
            {
                freqVFO(RigCaps.VFOs.VFOA, str.Substring(2, 11));
            }
            catch { }
            Debug.WriteLine("");
        }
        protected virtual void contFreqB(string str)
        {
            try
            {
                freqVFO(RigCaps.VFOs.VFOB, str.Substring(2, 11));
            }
            catch { }
        }
        protected virtual void contXI(string str)
        {
            Debug.Write("contXI:");
            try
            {
                int ofst=2;
                if (!System.UInt64.TryParse(str.Substring(ofst, 11), out TXFreq)) return;
                ofst += 11;
                Debug.Write("freq=" + TXFreq.ToString());
                txMd = getMode(str[ofst++]);
                txDM = getDataMode(str[ofst++]);
            }
            catch { Debug.Write("XI try failed"); }
            Debug.WriteLine("");
        }
        protected virtual void freqVFO(RigCaps.VFOs v, string f)
        {
            ulong u;
            if (System.UInt64.TryParse(f, out u))
            {
                Debug.Write("v=" + v.ToString() + " freq=" + u.ToString() + " txvfo=" + TXVFO.ToString() + "rxvfo=" + RXVFO.ToString());
                if (v == TXVFO) TXFreq = u;
                if (v == RXVFO) RXFreq = u;
            }
        }
        protected virtual void contSM(string str)
        {
            Debug.WriteLine("kenwood contsm:" + str);
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
            Debug.Write("contPS:");
            try
            {
                bool cmdOut = (cmd[2] == '1');
                if (cmdOut)
                {
                    // power is on.
                    powerLock.WaitOne(); // short term!
                    powerCount = 0;
                    powerLock.ReleaseMutex();
                    Debug.Write(" tell powerCheck it's on");
                }
#if zero
                pwr = (cmd.Substring(2, 1) == "1");
                Debug.Write((pwr) ? "power on" : "power off");
                // If continuous, make sure the rig is sending data.
                if (pwr && ContinuousMonitoring)
                {
                    // start continuous output.
                    Callouts.SendRoutine(BldCmd(kcmdAI+"2"));
                    rigStat(false); // Don't check power!
                    // Get memories if necessary.
                    if (Memories.Count == 0) GetMemories();
                }
#endif
            }
            catch { }
            Debug.WriteLine("");
        }

        protected virtual void contIF(string str)
        {
            Debug.Write("contIF:");
            int ofst = 2; // frequency offset
            try
            {
                ulong freq;
                int i;
                RigCaps.VFOs ovfo; // other VFO.
                bool mem; // memory mode
                if (!System.UInt64.TryParse(str.Substring(ofst, 11), out freq)) return;
                ofst += 11 + 5;
                string rs = str.Substring(ofst, 5);
                RIT.Value = stringToRIT(rs);
                XIT.Value = stringToRIT(rs);
                Debug.Write("RIT=" + RIT.Value.ToString());
                ofst += 5;
                RIT.Active = (str.Substring(ofst++, 1) == "1");
                XIT.Active = (str.Substring(ofst++, 1) == "1");
                Debug.Write(" ra=" + RIT.Active.ToString() + " xa=" + XIT.Active.ToString());
                CurrentMemoryChannel=getMemoryChannel(str.Substring(ofst, 3));
                ofst += 3; // mem channel
                Debug.Write(" memchan=" + CurrentMemoryChannel.ToString());
                XMit = (str.Substring(ofst++, 1) == "1");
                Debug.Write(" xmit=" + XMit.ToString());
                if (XMit) TXFreq = freq;
                else RXFreq = freq;
                Modes md = getMode(str[ofst++]);
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
                Splt = (str.Substring(ofst++, 1) == "1");
                Debug.Write(" split=" + Split.ToString());
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
                    if (!XMit) Callouts.SendRoutine(BldCmd(kcmdXI));
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
                Debug.Write(" rxfreq=" + RXFreq.ToString());
                Debug.Write(" txfreq=" + TXFreq.ToString());
                ToneCTSS = getToneCTSS(str[ofst++]);
                float tcf = ToneCTSSToFreq(str.Substring(ofst, 2));
                if ((ToneCTSS == ToneCTSSValue.Off) || (ToneCTSS == ToneCTSSValue.Tone))
                    ToneFrequency = tcf;
                else if (ToneCTSS == ToneCTSSValue.CTSS)
                    CTSSFrequency = tcf;
                else{
                    // cross tone
                    if (XMit) ToneFrequency=tcf;
                    else CTSSFrequency=tcf;
                }
                ofst += 2;
                Debug.Write(" ToneCTSS=" + ToneCTSS.ToString() + " tonectssFreq=" + tcf.ToString());
            }
            catch { Debug.Write("Exception!"); }
            Debug.WriteLine("");
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
        protected virtual void contMC(string str)
        {
            Debug.Write("contMC:");
            try
            {
                CurrentMemoryChannel = getMemoryChannel(str.Substring(2, 3));
                Debug.Write(" channel=" + CurrentMemoryChannel.ToString());
                if (RXVFO == RigCaps.VFOs.None)
                {
                    // We're in memory mode.
                    // However, the memory might not be in our collection yet!
                    // Use a thread to collect it, and get off the interrupt level.
                    Thread memGetThread;
                    memGetThread = new Thread(new ThreadStart(getThisMemory));
                    memGetThread.Start();
                }
            }
            catch { }
            Debug.WriteLine("");
        }
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
        protected virtual void getThisMemory()
        {
            bool askSW = true;
            int sanity = 5;
            while (!ActuateMemory(CurrentMemoryChannel) && (sanity-- != 0))
            {
                // Request the memory once, then wait for it to appear.
                if (askSW)
                {
                    Debug.Write(" not in collection");
                    askSW = false;
                    // Stop collecting other memories for now.
                    Thread.VolatileWrite(ref collectHoldup, 1);
                    Callouts.SendRoutine(BldCmd(kcmdMR + "0" + CurrentMemoryChannel.ToString("d3")));
                }
                Thread.Sleep(200);
            }
            // Resume collecting again.
            Thread.VolatileWrite(ref collectHoldup, 0);
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
            return ((c - '0') == '1') ? DataModes.on : DataModes.off;
        }
        protected char kDataMode(DataModes d)
        {
            return (char)((int)d + '0');
        }

        protected virtual ToneCTSSValue getToneCTSS(char c)
        {
            ToneCTSSValue[] tbl =
            {
                ToneCTSSValue.Off, ToneCTSSValue.Tone,
                ToneCTSSValue.CTSS, ToneCTSSValue.CrossTone
            };
            c -= '0';
            return ((c >= 0) && (c < tbl.Length)) ? tbl[c] : ToneCTSSValue.Off;
        }
        protected virtual void contMR(string str)
        {
            // If the entry isn't found:
                // error if rxtx is 1.
                // otherwise create new entry.
            // if frequency is 0:
                // indicate not present
                // otherwise indicate present.
            // frequency, mode, and dataMode come from rxtx 0 and 1.
            // Finally, if the entry wasn't found, add it.
            // If rxtx is 0, send out the MR1 command.
            // otherwise indicate complete.

            Debug.Write("contMR:");
            MemoryLock.WaitOne();
            try
            {
                bool exists;
                int ofst = 2;
                int rxtx;
                MemoryData m;
                string memNoString;
                rxtx = str[ofst++] - '0';
                char c = str[ofst++];
                if (c == ' ') c = '0';
                memNoString = c.ToString() + str.Substring(ofst, 2);
                ofst += 2;
                Debug.Write(" rxtx=" + rxtx.ToString() + " memNo=" + memNoString);
                int memNo;
                if (!System.Int32.TryParse(memNoString, out memNo)) goto memDone;
                exists = ((m = GetMemoryByNumber(memNo, false)) != null);
                Debug.Write(" exists=" + exists.ToString());
                if (!exists)
                {
                    if (rxtx == 1)
                    {
                        Debug.WriteLine(" wasn't found.");
                        goto memDone;
                    }
                    m = new MemoryData();
                }
                if (rxtx == 0)
                {
                    m.Type = (c == '0') ? MemoryTypes.Normal : MemoryTypes.Range;
                    m.Number = memNo;
                }
                string wkstr;
                wkstr = str.Substring(ofst, 11);
                if (!System.UInt64.TryParse(wkstr, out m.Frequency[rxtx])) goto memDone;
                ofst += 11;
                Debug.Write(" freq=" + m.Frequency[rxtx].ToString());
                if ((rxtx == 0) && (m.Frequency[0] != 0)) m.Present = true;
                if (m.Present)
                {
                    m.Mode[rxtx] = getMode(str[ofst++]);
                    m.DataMode[rxtx] = getDataMode(str[ofst++]);
                    Debug.Write(" mode=" + m.Mode[rxtx].ToString() + " dataMode=" + m.DataMode[rxtx].ToString());
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
                            Debug.Write(" name=" + m.Name);
                        }

                        // Get the other part of this memory.
                        Callouts.SendRoutine(BldCmd(kcmdMR + "1" + memNoString));
                    }
                    else
                    {
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
                if (!exists)
                {
                    // Note we've already bailed out if !Exists and rxtx is 1.
                    Memories.Add(m);
                    // Note however it's not complete.
                    Debug.Write(" added.");
                }
            }
            catch
            {
                Debug.Write(" try/catch error.");
            }
        memDone: 
            MemoryLock.ReleaseMutex();
            Debug.WriteLine("");
        }
        protected virtual void contVox(string str)
        {
            try
            {
                Vox = (str[2] == '1');
            }
            catch { }
        }
        protected virtual void contCN(string str)
        {
            try
            {
                CTSSFrequency = ToneCTSSToFreq(str.Substring(2, 2));
            }
            catch { }
        }
        protected virtual void contTN(string str)
        {
            try
            {
                ToneFrequency = ToneCTSSToFreq(str.Substring(2, 2));
            }
            catch { }
        }
        float[] ToneTable =
        {
            67.0F, 69.3F, 71.9F, 74.4F, 77.0F, 79.7F, 82.5F, 85.4F, 88.5F, 91.5F,
            94.8F, 97.4F, 100.0F, 103.5F, 107.2F, 110.9F, 114.8F, 118.8F, 123.0F,
            127.3F, 131.8F, 136.5F, 141.3F, 146.2F, 151.4F, 156.7F, 162.2F, 167.9F,
            173.8F, 179.9F, 186.2F, 192.8F, 203.5F, 206.5F, 210.7F, 218.1F, 225.7F,
            229.1F, 233.6F, 241.8F, 250.3F, 254.1F, 1750F
        };
        protected virtual float ToneCTSSToFreq(string str)
        {
            try
            {
                int t = 0;
                System.Int32.TryParse(str, out t);
                return ToneTable[t];
            }
            catch { return 0; }
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
    }
}
