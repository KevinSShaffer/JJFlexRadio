using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading;
using JJTrace;

namespace Radios
{
    /// <summary>
    /// Elecraft super-class
    /// </summary>
    class Elecraft : AllRadios
    {
        // Region - control commands
        #region commands
        protected const string ecmdDSP1 = "!"; // direct DSP
        protected const string ecmdDSP2 = "@"; // direct DSP
        protected const string ecmdAG = "AG"; // audio gain
        protected const string ecmdAI = "AI"; // auto info
        protected const string ecmdAN = "AN"; // antenna
        protected const string ecmdAP = "AP"; // audio peaking
        protected const string ecmdBG = "BG"; // bar graph
        protected const string ecmdBN = "BN"; // band number
        protected const string ecmdBR = "BR"; // baud rate
        protected const string ecmdBW = "BW"; // filter bandwidth
        protected const string ecmdCP = "CP"; // speach compression
        protected const string ecmdCW = "CW"; // sidetone pitch
        protected const string ecmdDB = "DB"; // VFO B display
        protected const string ecmdDL = "DL"; // DSP debug
        protected const string ecmdDN = "DN"; // move down
        protected const string ecmdDS = "DS"; // VFO A and Basic Icon Read
        protected const string ecmdDT = "DT"; // DATA Sub-Mode
        protected const string ecmdDV = "DV"; // Diversity Mode
        protected const string ecmdEL = "EL"; // ** (Error Logging; SET only; KX3 only) 
        protected const string ecmdES = "ES"; // (ESSB Mode; GET/SET)
        protected const string ecmdFA = "FA"; // (VFO A Frequency; GET/SET)
        protected const string ecmdFB = "FB"; // (VFO B Frequency; GET/SET)
        protected const string ecmdFI = "FI"; // * (I.F. Center Frequency; GET only)
        protected const string ecmdFR = "FR"; // (RX VFO Assignment [K2 only] and SPLIT Cancel; GET/SET)
        protected const string ecmdFT = "FT"; // (TX VFO Assignment and optional SPLIT Enable; GET/SET)
        protected const string ecmdFW = "FW"; // $ (Filter Bandwidth and Number; GET/SET)
        protected const string ecmdGT = "GT"; // (AGC Time Constant; GET/SET)
        protected const string ecmdIC = "IC"; // (Misc. Icons and Status; GET only)
        protected const string ecmdID = "ID"; // (Transceiver Identifier; GET only)
        protected const string ecmdIF = "IF"; // (Transceiver Information; GET only)
        protected const string ecmdIO = "IO"; // (KX3, Internal Use Only)
        protected const string ecmdIS = "IS"; // (I.F. Shift; GET/SET)
        protected const string ecmdK2 = "K2"; // * (Command Mode; GET/SET)
        protected const string ecmdK3 = "K3"; // * (Command Mode; GET/SET)
        protected const string ecmdKS = "KS"; // (Keyer Speed; GET/SET)
        protected const string ecmdKY = "KY"; // (CW or CW-to-DATA Keying from Text; GET/SET)
        protected const string ecmdLK = "LK"; // $ (VFO Lock; GET/SET)
        protected const string ecmdLN = "LN"; // * (Link VFOs; GET/SET) 
        protected const string ecmdMC = "MC"; // (Memory Channel; GET/SET)
        protected const string ecmdMD = "MD"; // $ (Operating Mode; GET/SET)
        protected const string ecmdMG = "MG"; // (Mic Gain; GET/SET)
        protected const string ecmdML = "ML"; // (Monitor Level; GET/SET)
        protected const string ecmdMN = "MN"; // (Menu Selection; GET/SET; K3 and KX3 variants)
        protected const string ecmdMP = "MP"; // (8-bit Direct Menu Parameter Access; GET/SET)
        protected const string ecmdMQ = "MQ"; // (16-bit Direct Menu Parameter Access; GET/SET; KX3 Only)
        // $ (Noise Blanker On/Off; GET/SET)
        protected string ecmdNB { get { return "NB" + RCVRSuffix; } }
        protected const string ecmdNL = "NL"; // $ (DSP and IF Noise Blanker Level; GET/SET)
        protected const string ecmdOM = "OM"; // (Option Module Query; GET Only; K3 and KX3 variants)
        // $ (Receive Preamp Control; GET/SET)
        protected string ecmdPA { get { return "PA" + RCVRSuffix; } }
        protected const string ecmdPC = "PC"; // (Requested Power Output Level; GET/SET)
        protected const string ecmdPO = "PO"; // ** (Actual Power Output Level; GET only; KX3 only)
        protected const string ecmdPS = "PS"; // (Transceiver Power Status; GET/SET)
        // $ (Receive Attenuator Control; GET/SET)
        protected string ecmdRA { get { return "RA" + RCVRSuffix; } }
        protected const string ecmdRC = "RC"; // (RIT Clear; SET only)
        protected const string ecmdRD = "RD"; // (RIT Offset Down One Unit; SET only)
        protected const string ecmdRG = "RG"; // $ (RF Gain; GET/SET)
        protected const string ecmdRO = "RO"; // (RIT/XIT Offset, Absolute; GET/SET)
        protected const string ecmdRT = "RT"; // (RIT Control; GET/SET)
        protected const string ecmdRU = "RU"; // (RIT Offset Up One Unit; SET only)
        protected const string ecmdRV = "RV"; // (Firmware Revisions; GET only)
        protected const string ecmdRX = "RX"; // (Receive Mode; SET only)
        protected const string ecmdSB = "SB"; // * (Sub Receiver or Dual Watch On/Off)
        protected const string ecmdSD = "SD"; // (QSK Delay, GET only)
        // $ (S-meter Read; GET only)
        protected string ecmdSM { get { return "SM" + RCVRSuffix; } }
        protected const string ecmdSMH = "SMH"; // (High-resolution S-meter Read; GET only; K3 only at present)
        protected const string ecmdSP = "SP"; // (Special Functions)
        protected const string ecmdSQ = "SQ"; // $ (Squelch Level; GET/SET)
        protected const string ecmdSWH = "SWH"; // (Switch hold Emulation; SET only; K3 and KX3 variants)
        protected const string ecmdSWT = "SWT"; // (Switch tap Emulation; SET only; K3 and KX3 variants)
        protected const string ecmdTB = "TB"; // (Received Text Read/Transmit Text Count; GET only)
        protected const string ecmdTE = "TE"; // (Transmit EQ; SET only)
        protected const string ecmdTQ = "TQ"; // (Transmit Query; GET only)
        protected const string ecmdTT = "TT"; // (Text to Terminal; SET only)
        protected const string ecmdTX = "TX"; // (Transmit Mode; SET only)
        protected const string ecmdUP = "UP"; // (Move VFO A, or Menu Entry/Parameter Up; SET only)
        protected const string ecmdUPB = "UPB"; // (Move VFO B, or Menu Entry/Parameter Up; SET only)
        protected const string ecmdVX = "VX"; // (VOX State; GET only)
        protected const string ecmdXF = "XF"; // $ (XFIL Number; GET only)
        protected const string ecmdXT = "XT"; // (XIT Control; GET/SET)
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
                Tracing.TraceLine("Power set " + value.ToString(), TraceLevel.Info);
                if (value) Callouts.safeSend(BldCmd(ecmdPS + "1"));
                else Callouts.safeSend(BldCmd(ecmdPS + "0"));
            }
        }
        protected override void PowerCheck()
        {
            // does nothing, see rigWatcher below.
        }

        private System.Timers.Timer rigWatchTimer;
        internal const int rigWatchInterval = 250;
        /// <summary>
        /// This timer handler periodically watches rig values
        /// </summary>
        private void rigWatcher(object s, ElapsedEventArgs e)
        {
            Callouts.safeSend(BldCmd(ecmdDS)); // icon status
            Callouts.safeSend(BldCmd(ecmdIC)); // icon status
            Callouts.safeSend(BldCmd(ecmdSM));
            Callouts.safeSend(BldCmd(ecmdDB)); // VFO B display
            Callouts.safeSend(BldCmd(ecmdTB));
            if (b_BSet) Callouts.safeSend(BldCmd(ecmdFB));
        }

        public override bool Transmit
        {
            get { return base.Transmit; }
            set
            {
                base.Transmit = value;
                if (value) Callouts.safeSend(BldCmd(ecmdTX));
                else Callouts.safeSend(BldCmd(ecmdRX));
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
                if (IsMemoryMode(value)) val = "2";
                else val = ((int)value).ToString();
                Callouts.safeSend(BldCmd(ecmdFR + val));
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
                if (IsMemoryMode(value)) val = "2";
                else val = ((int)value).ToString();
                Callouts.safeSend(BldCmd(ecmdFT + val));
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
                if (!IsMemoryMode(RXVFO))
                {
                    // Using VFOs.
                    char c = (value) ? '1' : '0';
                    Callouts.safeSend(BldCmd(ecmdFT + c));
                    // Get VFO B freq if split being set.
                    if (value) Callouts.safeSend(BldCmd(ecmdFB));
                }
            }
        }

        public override void CopyVFO(RigCaps.VFOs inv, RigCaps.VFOs outv)
        {
            if (inv == CurVFO)
            {
                Tracing.TraceLine("CopyVFO:" + inv.ToString() + " to " + outv.ToString(), TraceLevel.Info);
                // This will always be a->b
                Callouts.safeSend(BldCmd(ecmdSWT + "13"));
            }
            else Tracing.TraceLine("CopyVFO:inv must be the current VFO", TraceLevel.Error);
        }

        public override bool SplitShowXmitFrequency
        {
            get { return b_BSet; }
            set
            {
                bool oldVal = b_BSet;
                TFSetOn = b_BSet = value;
                if (oldVal != value) Callouts.safeSend(BldCmd(ecmdSWH + "11")); // toggle
                if (value) Callouts.safeSend(BldCmd(ecmdFB));
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

        public override ulong RXFrequency
        {
            get { return (b_BSet) ? myVFOFreq[(int)RigCaps.VFOs.VFOB] : myVFOFreq[(int)RigCaps.VFOs.VFOA]; }
            set
            {
                RigCaps.VFOs v = (b_BSet) ? RigCaps.VFOs.VFOB : RigCaps.VFOs.VFOA;
                string str = "F" + VFOToLetter(v) + UFreqToString(value);
                Callouts.safeSend(BldCmd(str));
                _RXFrequency = value;
            }
        }
        public override ulong TXFrequency
        {
            get { return _TXFrequency; }
            set
            {
                string str = "F" + VFOToLetter(TXVFO) + UFreqToString(value);
                Callouts.safeSend(BldCmd(str));
            }
        }

        /// <summary>
        /// mode indecies which must match ModeTable
        /// </summary>
        internal enum modes
        {none, lsb, usb, cw, cwr, am, fm, data, datar}
        /// <summary>
        /// my mode table
        /// </summary>
        internal static ModeValue[] myModeTable =
            {
                new ModeValue(0, '0',"none"),
                new ModeValue(1, '1', "lsb"),
                new ModeValue(2, '2', "usb"),
                new ModeValue(3, '3', "cw"),
                new ModeValue(4, '7', "cwr"),
                new ModeValue(5, '5', "am"),
                new ModeValue(6, '4', "fm"),
                new ModeValue(7, '6', "data"),
                new ModeValue(8, '9', "datar"),
                new ModeValue(9, '8', "none"),
            };
        /// <summary>
        /// mode character to internal mode.
        /// </summary>
        /// <param name="c">ASCII character</param>
        /// <returns>modes value</returns>
        protected virtual ModeValue getMode(char c)
        {
            Tracing.TraceLine("getMode:" + c.ToString(), TraceLevel.Info);
            ModeValue rv = ModeTable[0]; // Use none if invalid
            ModeValue cm = new ModeValue(c);
            try
            {
                foreach (ModeValue m in myModeTable)
                {
                    if (m == cm)
                    {
                        rv = m; // must return m, not cm.
                        break;
                    }
                }
            }
            catch (Exception ex)
            { Tracing.TraceLine("getMode:" + ex.Message, TraceLevel.Error); }
            return rv;
        }
        /// <summary>
        /// Get mode character to send to the rig.
        /// </summary>
        /// <param name="m">modeValue item</param>
        /// <returns>mode character</returns>
        protected virtual char KMode(ModeValue m)
        {
            Tracing.TraceLine("KMode:" + m.ToString(), TraceLevel.Info);
            char rv = '0';
            foreach (ModeValue mv in myModeTable)
            {
                if (m == mv)
                {
                    rv = mv.value;
                    break;
                }
            }
            return rv;
        }

        protected override ModeValue _RXMode
        {
            get
            {
                return base._RXMode;
            }
            set
            {
                base._RXMode = value;
                if ((value == myModeTable[(int)modes.cw]) ||
                    (value == myModeTable[(int)modes.cwr]))
                {
                    CanReceiveData = true;
                }
                else
                {
                    CanReceiveData = false;
                }
            }
        }
        public override ModeValue RXMode
        {
            get
            {
                return base.RXMode;
            }
            set
            {
                char c = KMode(value);
                Callouts.safeSend(BldCmd(ecmdMD + c.ToString()));
            }
        }
        public override ModeValue TXMode
        {
            get
            {
                return base.TXMode;
            }
            set
            {
                char c = KMode(value);
                Callouts.safeSend(BldCmd(ecmdMD + c.ToString()));
            }
        }

        public virtual OffOnValues PreAmp
        {
            get
            {
                return (b_PreAmp) ? OffOnValues.on : OffOnValues.off;
            }
            set
            {
                string val = (value == OffOnValues.on) ? "1" : "0";
                Callouts.safeSend(BldCmd(ecmdPA + val));
                b_PreAmp = (value == OffOnValues.on) ? true : false;
            }
        }

        public override OffOnValues RFAttenuator
        {
            get
            {
                return (b_ATT) ? OffOnValues.on : OffOnValues.off;
            }
            set
            {
                Callouts.safeSend(BldCmd(ecmdRA + (string)((value == OffOnValues.on) ? "01" : "00")));
                b_ATT = (value == OffOnValues.on) ? true : false;
            }
        }

        public override OffOnValues NoiseBlanker
        {
            get
            {
                return (b_NB) ? OffOnValues.on : OffOnValues.off;
            }
            set
            {
                string str = (value == OffOnValues.on) ? "1" : "0";
                Callouts.safeSend(BldCmd("NB" + str));
            }
        }
        internal OffOnValues NoiseBlankerSub
        {
            get { return (b_SubRXNB) ? OffOnValues.on : OffOnValues.off; }
            set
            {
                // The subRX must be on.
                if (b_SubRX)
                {
                    string str = (value == OffOnValues.on) ? "$1" : "$0";
                    Callouts.safeSend(BldCmd("NB" + str));
                }
            }
        }

        public override OffOnValues NoiseReduction
        {
            get
            {
                return (b_NR) ? OffOnValues.on : OffOnValues.off;
            }
            set
            {
                if (((value == OffOnValues.on) ? true : false) != b_NR)
                {
                    Callouts.safeSend(BldCmd(ecmdSWT + "34")); // toggle
                }
            }
        }
        internal OffOnValues NoiseReductionSub
        {
            get { return (b_SubRXNR) ? OffOnValues.on : OffOnValues.off; }
            set
            {
                // The subRX must be on.
                // If BSet isn't on, it's turned on, then off.
                if (b_SubRX)
                {
                    if (((value == OffOnValues.on) ? true : false) != b_SubRXNR)
                    {
                        bool saveBSet = b_BSet;
                        if (!b_BSet) Callouts.safeSend(BldCmd(ecmdSWH + "11")); // toggle
                        Callouts.safeSend(BldCmd(ecmdSWT + "34"));
                        if (!saveBSet) Callouts.safeSend(BldCmd(ecmdSWH + "11")); // toggle
                    }
                }
            }
        }

        private const AntTunerVals ATUStateMask = (AntTunerVals.rx | AntTunerVals.tx);
        public override AntTunerVals AntennaTuner
        {
            get
            {
                AntTunerVals rv = (b_ATU) ? ATUStateMask : (AntTunerVals)0;
                return rv;
            }
            set
            {
                if ((value & AntTunerVals.tune) != 0) Callouts.safeSend(BldCmd(ecmdSWT + "19"));
                else
                {
                    // if b_ATU, set to false if any state bit is false.
                    // if !b_ATU, set to true if any bit is true.
                    bool oldVal = b_ATU;
                    if (b_ATU)
                    {
                        b_ATU = ((value & ATUStateMask) == ATUStateMask);
                    }
                    else
                    {
                        b_ATU = ((value & ATUStateMask) != 0);
                    }
                    if (oldVal != b_ATU) Callouts.safeSend(BldCmd(ecmdSWH + "19")); // toggle
                }
            }
        }

        public override int TXAntenna
        {
            get
            {
                return (b_ANT2) ? 1 : 0;
            }
            set
            {
                Callouts.safeSend(BldCmd(ecmdAN + (string)((value == 0) ? "1" : "2")));
                b_ANT2 = (value == 1) ? true : false;
            }
        }

        public override bool RXAntenna
        {
            get
            {
                return b_RXAnt;
            }
            set
            {
                bool oldVal = b_RXAnt;
                b_RXAnt = value;
                if (oldVal != b_RXAnt) Callouts.safeSend(BldCmd(ecmdSWT + "25")); // toggle
            }
        }

        private int _KeyerSpeed;
        internal int KeyerSpeed
        {
            get
            {
                return _KeyerSpeed;
            }
            set
            {
                Callouts.safeSend(BldCmd(ecmdKS + value.ToString("d3")));
            }
        }

        public override OffOnValues AutoNotch
        {
            get
            {
                return (b_NTCH) ? OffOnValues.on : OffOnValues.off;
            }
            set
            {
                if (((value == OffOnValues.on) ? true : false) != b_NTCH)
                {
                    Callouts.safeSend(BldCmd(ecmdSWT + "32")); // toggle
                }
            }
        }

        public override OffOnValues ManualNotch
        {
            get
            {
                return (b_ManNTCH) ? OffOnValues.on : OffOnValues.off;
            }
            set
            {
                if (((value == OffOnValues.on) ? true : false) != b_ManNTCH)
                {
                    Callouts.safeSend(BldCmd(ecmdSWH + "32")); // toggle
                }
            }
        }

        public override int AGC
        {
            get
            {
                return base.AGC;
            }
            set
            {
                string str =
                    (_AGC <= 6) ? "002" :
                    (_AGC <= 12) ? "003" : "004";
                Callouts.safeSend(BldCmd(ecmdGT + str));
            }
        }

        /// <summary>
        /// Used by panning
        /// </summary>
        public override void UpdateMeter()
        {
            Callouts.safeSend(BldCmd(ecmdSM));
        }

        internal const int noMenu = 255;
        private int menuNo;
        internal int menuNumber
        {
            get { return menuNo; }
            set
            {
                menuNo = value;
                Callouts.safeSend(BldCmd(ecmdMN + value.ToString("d3")));
            }
        }

        internal bool subAntMain
        {
            get { return b_SubRXAntMain; }
            set
            {
                if (value != b_SubRXAntMain)
                {
                    Callouts.safeSend(BldCmd(ecmdSWH + "25")); // toggle
                }
            }
        }

        internal enum elecraftOffsetDirections
        {
            off,
            minus,
            plus
        }
        internal elecraftOffsetDirections fmOffset
        {
            get
            {
                elecraftOffsetDirections rv = elecraftOffsetDirections.off;
                if (b_OffsetMinus) rv = elecraftOffsetDirections.minus;
                else if (b_OffsetPlus) rv = elecraftOffsetDirections.plus;
                return rv;
            }
            set
            {
                int dest = (int)value; // destination
                int current = (int)fmOffset;
                while (current != dest)
                {
                    current = (current + 1) % Enum.GetNames(typeof(elecraftOffsetDirections)).Length;
                    Callouts.safeSend(BldCmd(ecmdSWH + "17"));
                }
            }
        }

        internal bool VFOsLinked
        {
            get { return b_VFOLink; }
            set
            {
                b_VFOLink = value;
                Callouts.safeSend(BldCmd(ecmdLN + (string)((b_VFOLink)? "1": "0")));
            }
        }

        internal bool diversity
        {
            get { return b_Diversity; }
            set
            {
                b_Diversity = value;
                Callouts.safeSend(BldCmd(ecmdDV + (string)((b_Diversity) ? "1" : "0")));
            }
        }

        private int _AudioGainSub;
        public override int AudioGain
        {
            get
            {
                return (controlingSub) ? _AudioGainSub : _AudioGain;
            }
            set
            {
                if (value < 0) value = 0;
                if (value > 250) value = 250;
                string cmd = ecmdRG;
                if (controlingSub)
                {
                    cmd += '$';
                    _AudioGainSub = value;
                }
                else _AudioGain = value;
                Callouts.safeSend(BldCmd(cmd + value.ToString("d3")));
            }
        }

        private int _LineoutGainSub;
        public override int LineoutGain
        {
            get
            {
                return (controlingSub) ? _LineoutGainSub : _LineoutGain;
            }
            set
            {
                if (value < 0) value = 0;
                if (value > 255) value = 255;
                string cmd = ecmdAG;
                if (controlingSub)
                {
                    cmd += '$';
                    _LineoutGainSub = value;
                }
                else _LineoutGain = value;
                Callouts.safeSend(BldCmd(cmd + value.ToString("d3")));
            }
        }

        private KenwoodIhandler.ResponseItem[] myResponseActions =
        {
            new KenwoodIhandler.ResponseItem(ecmdAG),
            new KenwoodIhandler.ResponseItem(ecmdAN),
            new KenwoodIhandler.ResponseItem(ecmdDB),
            new KenwoodIhandler.ResponseItem(ecmdDS),
            new KenwoodIhandler.ResponseItem(ecmdDV),
            new KenwoodIhandler.ResponseItem(ecmdFA),
            new KenwoodIhandler.ResponseItem(ecmdFB),
            new KenwoodIhandler.ResponseItem(ecmdFT),
            new KenwoodIhandler.ResponseItem(ecmdGT),
            new KenwoodIhandler.ResponseItem(ecmdIC),
            new KenwoodIhandler.ResponseItem(ecmdIF),
            new KenwoodIhandler.ResponseItem(ecmdK3),
            new KenwoodIhandler.ResponseItem(ecmdKS),
            new KenwoodIhandler.ResponseItem(ecmdLN),
            new KenwoodIhandler.ResponseItem(ecmdMD),
            new KenwoodIhandler.ResponseItem(ecmdMN),
            new KenwoodIhandler.ResponseItem("NB"),
            new KenwoodIhandler.ResponseItem("PA"),
            new KenwoodIhandler.ResponseItem(ecmdPS),
            new KenwoodIhandler.ResponseItem("RA"),
            new KenwoodIhandler.ResponseItem(ecmdRG),
            new KenwoodIhandler.ResponseItem(ecmdSB),
            new KenwoodIhandler.ResponseItem("SM"),
            new KenwoodIhandler.ResponseItem(ecmdTB),
        };

        private KenwoodIhandler rigHandler;
        private void setupResponseActions()
        {
            Tracing.TraceLine("setupResponseActions", TraceLevel.Info);
            foreach (KenwoodIhandler.ResponseItem r in myResponseActions)
            {
                switch (r.hdr)
                {
                    case ecmdAG: r.handler = contAG; break;
                    case ecmdAN: r.handler = contAN; break;
                    case ecmdDB: r.handler = contDB; break;
                    case ecmdDS: r.handler = contDS; break;
                    case ecmdDV: r.handler = contDV; break;
                    case ecmdFA: r.handler = contFreqA; break;
                    case ecmdFB: r.handler = contFreqB; break;
                    case ecmdFT: r.handler = contFT; ; break;
                    case ecmdGT: r.handler = contGT; ; break;
                    case ecmdIC: r.handler = contIC; break;
                    case ecmdIF: r.handler = contIF; break;
                    case ecmdK3: r.handler = contK3; break;
                    case ecmdKS: r.handler = contKS; break;
                    case ecmdLN: r.handler = contLN; break;
                    case ecmdMD: r.handler = contMD; break;
                    case ecmdMN: r.handler = contMN; break;
                    case "NB": r.handler = contNB; break;
                    case "PA": r.handler = contPA; break;
                    case ecmdPS: r.handler = contPS; ; break;
                    case "RA": r.handler = contRA; break;
                    case ecmdRG: r.handler = contRG; break;
                    case ecmdSB: r.handler = contSB; ; break;
                    case "SM": r.handler = contSM; ; break;
                    case ecmdTB: r.handler = contTB; ; break;
                    default:
                        throw new Exception("no handler for " + r.hdr);
                }
            }

            // Setup the interrupt handler.
            rigHandler = new KenwoodIhandler((AllRadios)this, myResponseActions);
        }

        public Elecraft()
        {
            Tracing.TraceLine("Elecraft constructor", TraceLevel.Info);
            ModeTable = myModeTable;
            setupResponseActions();
        }

        /// <summary>
        /// Open the radio
        /// </summary>
        /// <returns>True on success </returns>
        public override bool Open(OpenParms p)
        {
            Tracing.TraceLine("Elecraft Open", TraceLevel.Info);
            // the com port should be open.
            bool rv = base.Open(p);
            if (rv)
            {
                // Start the radio output processor.
                try { rigHandler.Start(); }
                catch (Exception ex)
                { Tracing.TraceLine("Elecraft open:" + ex.Message, TraceLevel.Error); }
                // setup sub-receiver field.
                // Note that the sub receiver is toggled with m or s, so no separate active status.
                p.RigField1 = new RigDependent(subRCVRChars, 1, subGet, subSet,
                    () => true, (bool val) => { });
                p.RigField2 = null; // unused
                // Start the rig watcher timer.
                rigWatchTimer = new System.Timers.Timer();
                rigWatchTimer.Interval = rigWatchInterval;
                rigWatchTimer.AutoReset = true;
                rigWatchTimer.Elapsed += new ElapsedEventHandler(rigWatcher);
                rigWatchTimer.Enabled = true;
            }
            IsOpen = rv;
            return rv;
        }

        public override void close()
        {
            Tracing.TraceLine("Elecraft close", TraceLevel.Info);
            rigHandler.Stop();
            try
            {
                rigWatchTimer.Enabled = false;
                Thread.Sleep(rigWatchInterval);
                rigWatchTimer.Dispose();
                takeDownSending();
            }
            catch (Exception ex)
            {
                Tracing.TraceLine("Elecraft close timer exception:" + ex.Message, TraceLevel.Error);
            }
            try
            {
                if (statThread.IsAlive) statThread.Abort();
            }
            catch (Exception ex)
            {
                Tracing.TraceLine("Elecraft close statThread exception:" + ex.Message, TraceLevel.Error);
            }

            // Turn off continuous rig output.
            Callouts.safeSend(BldCmd(ecmdAI + '0'));

            base.close();
        }

        protected static string[] statCommands;
        private Thread statThread;
        /// <summary>
        /// Start the rig's continuous output and get rig status.
        /// This only runs if both power is on and continuous monitoring is on.
        /// The rig operations are run in a separate thread.
        /// </summary>
        /// <param name="ckPower">true to check power here</param>
        /// <param name="initial">true for initial call at device startup</param>
        protected override void getRigStatus(bool ckPower, bool initial)
        {
            Tracing.TraceLine("getRigStatus:" + ckPower.ToString() + ' ' + initial.ToString(), TraceLevel.Info);
            // Get the radio's status.
            if (ckPower)
            {
                if (!powerOnCheck())
                {
                    // Quit here.  Note that we're called again if power comes back on.
                    Tracing.TraceLine("getRigstat:power not on", TraceLevel.Info);
                    return;
                }
            }
            statThread = new Thread(new ThreadStart(rigStatHelper));
            try { statThread.Start(); }
            catch (Exception ex)
            { Tracing.TraceLine("GetRigStat:" + ex.Message, TraceLevel.Error); }
            Thread.Sleep(0);
        }
        private int rsHelperRunning = 0;
        /// <summary>
        /// separate thread to get the rig's status at power-on or program startup.
        /// </summary>
        private void rigStatHelper()
        {
            Tracing.TraceLine("rigStatHelper", TraceLevel.Info);
            try
            {
                // Quit if already running.
                if (Interlocked.CompareExchange(ref rsHelperRunning, 1, 0) == 1)
                {
                    Tracing.TraceLine("rigStatHelper:already running", TraceLevel.Info);
                    return;
                }
                // rsHelperRunning is now 1.

                // Turn on continuous rig output.
                Callouts.safeSend(BldCmd(ecmdAI + "2"));

                foreach (string cmd in statCommands)
                {
                    Callouts.safeSend(BldCmd(cmd));
                    // wait a bit
                    Thread.Sleep(50);
                }
                //GetMemories();
            }
            catch (ThreadAbortException) { Tracing.TraceLine("rigStatHelper abort", TraceLevel.Error); }
            catch (Exception ex)
            {
                Tracing.TraceLine("rigStatHelper:" + ex.Message, TraceLevel.Error);
            }
            finally { rsHelperRunning = 0; }
        }

        // Region - Memories
        #region memories
        #endregion

        // region - sub-receiver
        #region subRcvr
        private enum subRCVRvals { main, sub }
        private static char[] subRCVRChars = { 'm', 's' };
        private subRCVRvals subRCVRVal;
        /// <summary>
        /// Send commands for a RCVR change.
        /// </summary>
        private void RCVRChange()
        {
            Callouts.safeSend(BldCmd(ecmdSM));
            Callouts.safeSend(BldCmd(ecmdPA));
            Callouts.safeSend(BldCmd(ecmdRA));
        }
        private subRCVRvals toggleRCVR()
        {
            subRCVRvals rv = subRCVRVal;
            subRCVRVal = (subRCVRVal == subRCVRvals.main) ?
                subRCVRvals.sub : subRCVRvals.main;
            Callouts.safeSend(BldCmd(ecmdSWT + "48")); // toggle
            RCVRChange();
            return rv;
        }
        private subRCVRvals subRCVR
        {
            get { return subRCVRVal; }
            set
            {
                if (value != subRCVRVal)
                {
                    toggleRCVR();                    
                }
            }
        }
        /// <summary>
        /// true if controling sub-receiver
        /// </summary>
        internal bool controlingSub
        {
            get
            { return subRCVRVal == subRCVRvals.sub; }
        }
        /// <summary>
        /// receiver prefix for rig commands
        /// </summary>
        internal string RCVRSuffix
        { get { return (controlingSub) ? "$" : ""; } }

        protected virtual void contAG(string str)
        {
            if (str[2] == '$')
            {
                _LineoutGainSub = System.Int32.Parse(str.Substring(3));
            }
            else
            {
                _LineoutGain = System.Int32.Parse(str.Substring(2));
            }
        }

        protected virtual void contSM(string str)
        {
            int pos = (str[2] == '$') ? 3 : 2;
            _SMeter = System.Int32.Parse(str.Substring(pos));
            // For panning.
            raiseRigOutput(RigCaps.Caps.SMGet, (ulong)_SMeter);
        }

        protected virtual void contNB(string str)
        {
            if (str[2] == '$') b_SubRXNB = (str[3] == '1');
            b_NB = (str[2] == '1');
        }

        protected virtual void contPA(string str)
        {
            int pos = (str[2] == '$') ? 3 : 2;
            b_PreAmp = (str[pos] == '1');
        }

        protected virtual void contRA(string str)
        {
            int pos = (str[2] == '$') ? 3 : 2;
            // format is RA[$]p1p1
            b_ATT = (str[pos + 1] == '1') ? true : false;
            _RFAttenuator = (b_ATT) ? OffOnValues.on : OffOnValues.off;
        }

        protected virtual void contRG(string str)
        {
            if (str[2] == '$')
            {
                _AudioGainSub = System.Int32.Parse(str.Substring(3));
            }
            else
            {
                _AudioGain = System.Int32.Parse(str.Substring(2));
            }
        }
        
        private char subGet(int id)
        {
            // id is unused
            char rv = subRCVRChars[(int)subRCVRVal];
            return rv;
        }
        private void subSet(char c, int id)
        {
            // id is unused
            subRCVRvals val = (c == 's') ? subRCVRvals.sub : subRCVRvals.main;
            subRCVR = val;
        }
        #endregion

        // region - rig command handlers
        #region rigcmds
        /// <summary>
        /// display character conversion table.
        /// </summary>
        /// <remarks>
        /// 0x04 and 0x05 refer to eq levels 4 and 5.
        /// If bit 7 on, means decimal point to the left.
        /// </remarks>
        private byte[] DisplayCharacterTable =
        {
            0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f,
            0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f,
            0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x2a, 0x2b, 0x2c, 0x20, 0x2e, 0x2f,
            0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x6c, 0x3d, 0x20, 0x3f,
            0x20, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d, 0x4e, 0x4f,
            0x50, 0x51, 0x52, 0x53, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5a, 0x20, 0x04, 0x04, 0x05, 0x5f,
            0x60, 0x61, 0x62, 0x63, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69, 0x6a, 0x6b, 0x6c, 0x6d, 0x6e, 0x6f,
            0x70, 0x71, 0x72, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0x79, 0x7a, 0x7b, 0x7c, 0x7d, 0x7e, 0x7f,
        };
        internal string DisplayCharacterConvert(byte b)
        {
            string rv = "";
            if (b >= 0x80)
            {
                rv = ".";
                b -= 0x80;
            }
            char c = (char)DisplayCharacterTable[b];
            rv += c;
            return rv;
        }
        internal string DisplayCharacterConvert(string str)
        {
            string rv = "";
            foreach (char c in str)
            {
                rv += DisplayCharacterConvert((byte)c);
            }
            return rv;
        }

        protected virtual void contPS(string str)
        {
            // currently does nothing.
        }

        protected virtual void contFreqA(string str)
        {
            freqVFO(RigCaps.VFOs.VFOA, str.Substring(2, 11));
        }

        protected virtual void contFreqB(string str)
        {
            freqVFO(RigCaps.VFOs.VFOB, str.Substring(2, 11));
        }

        protected ulong[] myVFOFreq = new ulong[2];
        protected virtual void freqVFO(RigCaps.VFOs v, string f)
        {
            Tracing.TraceLine("freqVFO:v=" + v.ToString() + " freq=" + f +
                " txvfo=" + TXVFO.ToString() + " rxvfo=" + RXVFO.ToString(),
                TraceLevel.Info);
            ulong u;
            if (System.UInt64.TryParse(f, out u))
            {
                myVFOFreq[(int)v] = u;
                if (v == TXVFO) _TXFrequency = u;
                if (v == RXVFO) _RXFrequency = u;
                // For panning
                raiseRigOutput(RigCaps.Caps.FrGet, u);
            }
            else Tracing.TraceLine("freqVFO:error" + f, TraceLevel.Error);
        }

        protected virtual void contIF(string str)
        {
            int ofst = 2; // frequency offset
            ulong freq;
            int i;
            RigCaps.VFOs ovfo; // other VFO.
            string wkstr = str.Substring(ofst, 11);
            freq = System.UInt64.Parse(wkstr);
            ofst += 11 + 5;
            string rs = str.Substring(ofst, 5);
            RIT.Value = stringToRIT(rs);
            XIT.Value = stringToRIT(rs);
            ofst += 5;
            RIT.Active = (str.Substring(ofst++, 1) == "1");
            XIT.Active = (str.Substring(ofst++, 1) == "1");
            ofst += 3; // mem channel
            Transmit = (str.Substring(ofst++, 1) == "1");
            if (Transmit) _TXFrequency = freq;
            else _RXFrequency = freq;
            ModeValue md = getMode(str[ofst++]);
            if (Transmit) _TXMode = md;
            else _RXMode = md;
            i = str[ofst++] - '0';
            if ((i >= 0) && (i < 2))
            {
                // For a VFO:
                // set current VFO
                ovfo = (RigCaps.VFOs)((i + 1) % 2); // the other VFO.
                if (Transmit) _TXVFO = (RigCaps.VFOs)i;
                else _RXVFO = (RigCaps.VFOs)i;
            }
            else throw new Exception("contif:Invalid mode");
            ofst += 1; // Scan status.
            bool Splt = (str.Substring(ofst++, 1) == "1");
            // Set the other VFO/freq.
            if (Splt)
            {
                // Split VFOs.
                if (Transmit) _RXVFO = (RigCaps.VFOs)ovfo;
                else _TXVFO = (RigCaps.VFOs)ovfo;
            }
            else
            {
                // symplex
                if (Transmit)
                {
                    _RXVFO = TXVFO;
                    _RXFrequency = _TXFrequency;
                    _RXMode = _TXMode;
                }
                else
                {
                    _TXVFO = RXVFO;
                    _TXFrequency = _RXFrequency;
                    _TXMode = _RXMode;
                }
            }
        }
        private int stringToRIT(string str)
        {
            Tracing.TraceLine("stringToRIT:" + str, TraceLevel.Info);
            int i;
            if (System.Int32.TryParse(str.Substring(1), out i))
            {
                return (str[0] == '-') ? -i : i;
            }
            else
            {
                Tracing.TraceLine("stringToRIT:error" + str, TraceLevel.Error);
                return 0;
            }
        }

        protected virtual void contMD(string str)
        {
            ModeValue m = getMode(str[2]);
            if (!Split) _RXMode = _TXMode = m;
            else
            {
                if (Transmit) _TXMode = m;
                else _RXMode = m;
            }
        }

        protected virtual void contAN(string str)
        {
            b_ANT2 = (str[2] == '1') ? true : false;
        }

        private string VFOBDisp;
        /// <summary>
        /// (readOnly) VFO B display value
        /// </summary>
        internal string VFOBDisplay
        {
            get { return VFOBDisp; }
        }

        protected virtual void contDB(string str)
        {
            Tracing.TraceLine("contDB:" + Escapes.Escapes.Decode(str.Substring(2)), TraceLevel.Info);
            VFOBDisp = str.Substring(2);
        }

        private bool BitIsSet(byte[] bytes, int id, byte mask)
        {
            return ((bytes[id] & mask) != 0);
        }
        private bool BitSet(byte[] bytes, int id, byte mask, bool val)
        {
            bool rv = BitIsSet(bytes, id, mask);
            if (val) bytes[id] |= mask;
            else bytes[id] &= (byte)((byte)0xff ^ mask);
            return rv;
        }

        private byte[] DSBytes = new byte[2];
        internal bool b_NB {
            get { return BitIsSet(DSBytes, 0, 0x40); }
            set { BitSet(DSBytes, 0, 0x40, value); }
        }
        internal bool b_ANT2 {
            get { return BitIsSet(DSBytes, 0, 0x20); }
            set { BitSet(DSBytes, 0, 0x20, value); }
        }
        internal bool b_PreAmp
        {
            get { return BitIsSet(DSBytes, 0, 0x10); }
            set { BitSet(DSBytes, 0, 0x10, value); }
        }
        internal bool b_ATT {
            get { return BitIsSet(DSBytes, 0, 0x08); }
            set { BitSet(DSBytes, 0, 0x08, value); }
        }
        internal bool b_VFOASel
        {
            get { return BitIsSet(DSBytes, 0, 0x04); } // 0 for K3
            set { BitSet(DSBytes, 0, 0x04, value); }
        }
        internal bool b_RIT {
            get { return BitIsSet(DSBytes, 0, 0x02); }
            set { BitSet(DSBytes, 0, 0x02, value); }
        }
        internal bool b_XIT {
            get { return BitIsSet(DSBytes, 0, 0x01); }
            set { BitSet(DSBytes, 0, 0x01, value); }
        }

        // subRX handled through IC.
        internal bool b_RXAnt {
            get { return BitIsSet(DSBytes, 1, 0x20); }
            set { BitSet(DSBytes, 1, 0x20, value); }
        }
        internal bool b_ATU {
            get { return BitIsSet(DSBytes, 1, 0x10); }
            set { BitSet(DSBytes, 1, 0x10, value); }
        }
        internal bool b_CWT {
            get { return BitIsSet(DSBytes, 1, 0x08); }
            set { BitSet(DSBytes, 1, 0x08, value); }
        }
        internal bool b_NR {
            get { return BitIsSet(DSBytes, 1, 0x04); }
            set { BitSet(DSBytes, 1, 0x04, value); }
        }
        internal bool b_NTCH {
            get { return BitIsSet(DSBytes, 1, 0x02); }
            set { BitSet(DSBytes, 1, 0x02, value); }
        }
        internal bool b_ManNTCH {
            get { return BitIsSet(DSBytes, 1, 0x01); }
            set { BitSet(DSBytes, 1, 0x01, value); }
        }

        private string VFOADisp = "        ";
        private const string VFOADispOnValue = "     ON ";
        /// <summary>
        /// VFO A display
        /// </summary>
        internal string VFOADisplay
        {
            get { return VFOADisp; }
        }
        internal bool VFOADispOn
        {
            get { return (VFOADisp == VFOADispOnValue); }
        }

        protected void contDS(string str)
        {
            Tracing.TraceLine("contDS:" + Escapes.Escapes.Decode(str.Substring(10)), TraceLevel.Info);
            VFOADisp = DisplayCharacterConvert(str.Substring(2, 8));
            for (int i = 0; i < DSBytes.Length; i++) DSBytes[i] = (byte)str[i + 10];
        }

        private byte[] ICBytes = new byte[5];
        internal bool b_BSet
        {
            get { return BitIsSet(ICBytes, 0, 0x40); }
            set { BitSet(ICBytes, 0, 0x40, value); }
        }
        internal bool b_TXTest {
            get { return BitIsSet(ICBytes, 0, 0x20); }
            set { BitSet(ICBytes, 0, 0x20, value); }
        }
        internal bool b_VFOLink {
            get { return BitIsSet(ICBytes, 1, 0x40); }
            set { BitSet(ICBytes, 1, 0x40, value); }
        }
        internal bool b_VFOBandsIndependent
        {
            get { return BitIsSet(ICBytes, 1, 0x20); }
            set { BitSet(ICBytes, 1, 0x20, value); }
        }
        internal bool b_Diversity
        {
            get { return BitIsSet(ICBytes, 1, 0x10); }
            set { BitSet(ICBytes, 1, 0x10, value); }
        }
        internal bool b_SubRXAntMain
        {
            get { return BitIsSet(ICBytes, 1, 0x08); }
            set { BitSet(ICBytes, 1, 0x08, value); }
        }
        internal bool b_SubRXNB
        {
            get { return BitIsSet(ICBytes, 1, 0x02); }
            set { BitSet(ICBytes, 1, 0x02, value); }
        }
        internal bool b_SubRX
        { 
            get { return BitIsSet(ICBytes, 1, 0x01); }
            set { BitSet(ICBytes, 1, 0x01, value); }            
        }
        internal bool b_CWQSK
        {
            get { return BitIsSet(ICBytes, 2, 0x40); }
            set { BitSet(ICBytes, 2, 0x40, value); }
        }
        internal bool b_CWVox
        {
            get { return BitIsSet(ICBytes, 2, 0x10); }
            set { BitSet(ICBytes, 2, 0x10, value); }            
        }
        internal bool b_PhoneVox {
            get { return BitIsSet(ICBytes, 3, 0x40); }
            set { BitSet(ICBytes, 3, 0x40, value); }            
        }
        internal bool b_PLTone {
            get { return BitIsSet(ICBytes, 3, 0x04); }
            set { BitSet(ICBytes, 3, 0x04, value); }            
        }
        internal bool b_OffsetPlus {
            get { return BitIsSet(ICBytes, 3, 0x02); }
            set { BitSet(ICBytes, 3, 0x02, value); }            
        }
        internal bool b_OffsetMinus {
            get { return BitIsSet(ICBytes, 3, 0x01); }
            set { BitSet(ICBytes, 3, 0x01, value); }            
        }
        internal bool b_MainSQ
        {
            get { return BitIsSet(ICBytes, 4, 0x10); }
            set { BitSet(ICBytes, 4, 0x10, value); }
        }
        internal bool b_SubSQ
        {
            get { return BitIsSet(ICBytes, 4, 0x08); }
            set { BitSet(ICBytes, 4, 0x08, value); }
        }
        internal bool b_SubRXNR
        {
            get { return BitIsSet(ICBytes, 4, 0x04); }
            set { BitSet(ICBytes, 4, 0x04, value); }
        }

        protected virtual void contIC(string str)
        {
            Tracing.TraceLine("contIC:" + Escapes.Escapes.Decode(str.Substring(2)), TraceLevel.Info);
            for (int i = 0; i < ICBytes.Length; i++) ICBytes[i] = (byte)str[i + 2];
        }

        protected void contSB(string str)
        {
            b_SubRX = (str[2] == '1');
            subRCVRVal = (b_SubRX) ? subRCVRvals.sub : subRCVRvals.main;
        }

        protected void contK3(string str)
        {
        }

        protected void contFT(string str)
        {
            _TXVFO = (str[2] == '1') ? RigCaps.VFOs.VFOB : RigCaps.VFOs.VFOA;
        }

        protected virtual void contKS(string str)
        {
            _KeyerSpeed = System.Int32.Parse(str.Substring(2));
        }

        // region - cw sending
        #region cwSend
        private Thread sendThread = null;
        private Queue sendQ;
        private bool sendSetup { get { return (sendThread != null); } }
        private void setupSending()
        {
            Tracing.TraceLine("setupSending:", TraceLevel.Info);
            sendQ = Queue.Synchronized(new Queue());
            sendThread = new Thread(new ThreadStart(sender));
            try { sendThread.Start(); }
            catch (Exception ex)
            { Tracing.TraceLine("setupSending:" + ex.Message, TraceLevel.Error); }
            Thread.Sleep(0);
        }
        private void takeDownSending()
        {
            Tracing.TraceLine("takeDownSending", TraceLevel.Info);
            try
            {
                if (sendThread.IsAlive) sendThread.Abort();
                sendThread = null;
            }
            catch (Exception ex)
            {
                Tracing.TraceLine("takeDownSending:" + ex.Message, TraceLevel.Error);
            }
        }
        
        public override bool SendCW(char c)
        {
            bool rv = true;
            if ((Mode == myModeTable[(int)modes.cw]) ||
                (Mode == myModeTable[(int)modes.cwr]))
            {
                if (!sendSetup) setupSending();
                sendQ.Enqueue(c);
            }
            else rv = false;
            return rv;
        }

        private void sender()
        {
            Tracing.TraceLine("cw sender thread started", TraceLevel.Info);
            try
            {
                while (true)
                {
                    if ((TXRemainingChars == 0) && (sendQ.Count > 0))
                    {
                        char c = (char)sendQ.Dequeue();
                        // We don't support sending @ sign.
                        if (c != '@')
                        {
                            Callouts.safeSend(BldCmd(ecmdKY + " " + c.ToString()));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Tracing.TraceLine("sender:ct=" + sendQ.Count.ToString() + " " + ex.Message, TraceLevel.Error);
            }
        }

        public override void StopCW()
        {
            Tracing.TraceLine("StopCW", TraceLevel.Info);
            bool rv = sendSetup;
            if (rv)
            {
                Callouts.safeSend(BldCmd(ecmdKY + " @"));
                sendQ.Clear();
                Tracing.TraceLine("StopCW q cleared", TraceLevel.Info);
#if zero
                try
                {
                    sendThread.Interrupt();
                    Tracing.TraceLine("StopCW:int sent", TraceLevel.Info);
                }
                catch (Exception ex)
                { Tracing.TraceLine("StopCW:" + ex.Message, TraceLevel.Error); }
#endif
            }
            else Tracing.TraceLine("StopCW:error", TraceLevel.Error);
        }
        #endregion

        private int TXRemainingChars = 0;
        //private int RXChars = 0;
        protected virtual void contTB(string str)
        {
            TXRemainingChars = (int)(str[2] - '0');
            if (CanReceiveData)
            {
                RXCharBuffer = str.Substring(5);
#if zero
                    int n = System.Int32.Parse(str.Substring(3, 2));
                    if ((RXChars + n) <= RXCharBufferSize) RXCharBuffer += str.Substring(5);
                    else
                    {
                        if (n == RXCharBufferSize) RXCharBuffer = str.Substring(5);
                        else RXCharBuffer = RXCharBuffer.Substring(n) + str.Substring(5);
                    }
#endif
            }
        }

        protected virtual void contMN(string str)
        {
        }

        protected virtual void contDV(string str)
        {
            b_Diversity = (str[2] == '1') ? true : false;
        }

        protected virtual void contLN(string str)
        {
            b_VFOLink = (str[2] == '1') ? true : false;
        }

        protected virtual void contGT(string str)
        {
            int val = System.Int32.Parse(str.Substring(2));
            _AGC = (val == 0) ? 0 : (val == 2) ? 1 :
                (val == 3) ? 10 : 20;
        }
        #endregion
    }
}
