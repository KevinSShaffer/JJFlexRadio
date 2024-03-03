//#define debugFlag

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace Radios
{
    /// <summary>
    /// Provide template functions for radio queries and actions.
    /// </summary>
    public class AllRadios
    {
        /// <summary>
        /// This rig's capabilities
        /// </summary>
        protected RigCaps myCaps; // See RigCaps.cs

        // region - general properties and methods
#region general properties
        /// <summary>
        /// Internal vfos
        /// </summary>
        protected RigCaps.VFOs rxV, txV;
        /// <summary>
        /// receive VFO in use
        /// </summary>
        public virtual RigCaps.VFOs RXVFO
        {
            get { return rxV; }
            set { rxV = value; }
        }
        /// <summary>
        /// transmit VFO
        /// </summary>
        public virtual RigCaps.VFOs TXVFO
        {
            get { return txV; }
            set { txV = value; }
        }
        /// <summary>
        /// return the next VFO in the list.
        /// If it's a memory, VFOs.None, just return VFOs.None.
        /// </summary>
        /// <param name="v">Starting VFO</param>
        /// <returns>Next VFO</returns>
        public virtual RigCaps.VFOs NextVFO(RigCaps.VFOs v)
        {
            RigCaps.VFOs rv;
            if (v == RigCaps.VFOs.None) rv = v;
            else
            {
                int n = Enum.GetNames(typeof(RigCaps.VFOs)).Length;
                int i = (int)v;
                // Note there are n-1 real VFOs.
                rv = (RigCaps.VFOs)((i + 1) % (n-1));
            }
            return rv;
        }

        /// <summary>
        /// copy a VFO.
        /// </summary>
        /// <param name="inv">VFO to copy</param>
        /// <param name="outv">Target VFO</param>
        public virtual void CopyVFO(RigCaps.VFOs inv, RigCaps.VFOs outv)
        {
            // Provided by the rigs.  It would be possible to provide such a function here.
        }

        /// <summary>
        /// Internal transmit indicator
        /// </summary>
        protected bool XMit;
        /// <summary>
        /// True if transmitting
        /// </summary>
        public virtual bool Transmit
        {
            get { return XMit; }
            set { XMit = value; }
        }
        /// <summary>
        /// True if split operation
        /// </summary>
        public virtual bool Split
        {
            get
            {
                bool rv = false;
                if (RXVFO == RigCaps.VFOs.None)
                {
                    // Using a memory
                    try
                    {
                        MemoryData m = Memories[CurrentMemoryChannel];
                        rv = (m.complete && m.Present && m.Split);
                    }
                    catch (Exception ex) { DBGWL("split:" + ex.Message); }
                }
                else
                {
                    // using VFOs.
                    rv = (RXVFO != TXVFO);
                }
                return rv;
            }
            // The "set" must be provided by the rigs.
            set { }
        }
        /// <summary>
        /// Get the current VFO in use.
        /// </summary>
        public RigCaps.VFOs CurVFO
        {
            get
            {
                return (Transmit) ? TXVFO : RXVFO;
            }
            set
            {
                if (Transmit)
                {
                    TXVFO = value;
                }
                else
                {
                    RXVFO = value;
                }
            }
        }
        // TFSetOn is set if showing xmit frequency when split.
        protected bool TFSetOn;
        /// <summary>
        /// Show the transmit frequency while split.
        /// </summary>
        public virtual bool SplitShowXmitFrequency
        {
            get { return TFSetOn; }
            // Set provided by the rigs if function exists.
            set { }
        }
        /// <summary>
        /// Internal frequencies
        /// </summary>
        protected ulong RXFreq, TXFreq;
        /// <summary>
        /// receive frequency
        /// </summary>
        public virtual ulong RXFrequency
        {
            get { return RXFreq; }
            set { RXFreq = value; }
        }
        /// <summary>
        /// transmit frequency
        /// </summary>
        public virtual ulong TXFrequency
        {
            get { return TXFreq; }
            set { TXFreq = value; }
        }
        /// <summary>
        /// current frequency
        /// </summary>
        public ulong Frequency
        {
            get
            {
                if (Transmit) return TXFrequency;
                else return RXFrequency;
            }
            set
            {
                if (Transmit) TXFrequency = value;
                else RXFrequency = value;
            }
        }
        protected int SMtr;
        /// <summary>
        /// (readOnly) SMeter value
        /// </summary>
        public int SMeter {
            get { return SMtr; }
        }
        public class RITData
        {
            public bool Active;
            public int Value; // may be negative
            public RITData()
            {
                Active = false;
                Value = 0;
            }
        }
        /// <summary>
        /// RIT, Active and Value.
        /// </summary>
        public virtual RITData RIT { get; set; }
        /// <summary>
        /// XIT, Active and Value.
        /// </summary>
        public virtual RITData XIT { get; set; }

        protected bool pwr;
        /// <summary>
        /// power on/off
        /// </summary>
        public virtual bool Power
        {
            get { return pwr; }
            set { pwr = value; }
        }
        /// <summary>
        /// Method to initiate a power on check.
        /// </summary>
        public virtual void PowerCheck()
        {
            // handled by the rigs.
        }

        protected bool vx;
        /// <summary>
        /// Vox on/off
        /// </summary>
        public virtual bool Vox
        {
            get { return vx; }
            // Set must be provided by the rigs.
            set { }
        }

        /// <summary>
        /// Operating modes
        /// </summary>
        public enum Modes
        {
            None = -1,
            lsb = 0,
            usb,
            cw,
            cwr,
            am,
            fsk,
            fskr,
            fm,
        }

        protected Modes rxMd, txMd;
        /// <summary>
        /// Receive operating mode
        /// </summary>
        public virtual Modes RXMode
        {
            get { return rxMd; }
            set { rxMd = value; }
        }
        /// <summary>
        ///  Transmit operating mode.
        /// </summary>
        public virtual Modes TXMode
        {
            get { return txMd; }
            set { txMd = value; }
        }
        /// <summary>
        /// Current mode
        /// </summary>
        public Modes Mode
        {
            get
            {
                if (Transmit) return TXMode;
                else return RXMode;
            }
            set
            {
                if (Transmit) TXMode = value;
                else RXMode = value;
            }
        }
        /// <summary>
        /// Data mode values
        /// </summary>
        public enum DataModes
        {
            off,
            on
        }
        protected DataModes rxDM, txDM;
        /// <summary>
        /// Receive Data mode
        /// </summary>
        public virtual DataModes RXDataMode
        {
            get { return rxDM; }
            set { rxDM = value; }            
        }
        /// <summary>
        /// Transmit data mode
        /// </summary>
        public virtual DataModes TXDataMode
        {
            get { return txDM; }
            set { txDM = value; }
        }
        /// <summary>
        /// Current Data mode
        /// </summary>
        public DataModes DataMode
        {
            get
            {
                if (Transmit) return TXDataMode;
                else return RXDataMode;
            }
            set
            {
                if (Transmit) TXDataMode = value;
                else RXDataMode = value;
            }
        }
        /// <summary>
        /// Tone/CTSS values
        /// </summary>
        public enum ToneCTSSValue
        {
            Off = 0,
            Tone,
            CTSS,
            CrossTone
        }
        /// <summary>
        /// Internal tone/CTSS value
        /// </summary>
        protected ToneCTSSValue toneCT;
        /// <summary>
        /// Tone/CTSS
        /// </summary>
        public virtual ToneCTSSValue ToneCTSS
        {
            get { return toneCT; }
            set { toneCT = value; }
        }
        /// <summary>
        /// Array of tone frequencies
        /// </summary>
        public static float[] ToneFrequencyTable =
        {
            67.0F, 69.3F, 71.9F, 74.4F, 77.0F, 79.7F, 82.5F, 85.4F, 88.5F, 91.5F,
            94.8F, 97.4F, 100.0F, 103.5F, 107.2F, 110.9F, 114.8F, 118.8F, 123.0F,
            127.3F, 131.8F, 136.5F, 141.3F, 146.2F, 151.4F, 156.7F, 162.2F, 167.9F,
            173.8F, 179.9F, 186.2F, 192.8F, 203.5F, 206.5F, 210.7F, 218.1F, 225.7F,
            229.1F, 233.6F, 241.8F, 250.3F, 254.1F, 1750F
        };
        /// <summary>
        /// Internal tone frequency
        /// </summary>
        protected float toneFreq;
        /// <summary>
        /// Tone frequency
        /// </summary>
        public virtual float ToneFrequency
        {
            get { return toneFreq; }
            set { toneFreq = value; }
        }
        /// <summary>
        /// Internal CTSS frequency.
        /// </summary>
        protected float CTSSFreq;
        /// <summary>
        /// CTSS frequency
        /// </summary>
        public virtual float CTSSFrequency
        {
            get { return CTSSFreq; }
            set { CTSSFreq = value; }
        }
        /// <summary>
        /// FM modes (normal and narrow)
        /// </summary>
        public enum FMModes
        {
            Normal,
            Narrow
        }
        /// <summary>
        /// Internal FM mode.
        /// </summary>
        protected FMModes fmMd;
        public virtual FMModes FMMode
        {
            get { return fmMd; }
            set { fmMd = value; }
        }

        private bool cont;
        // contLock makes changing the cont and toggling the state atomic.
        private Mutex contLock;
        /// <summary>
        /// Continuous monitoring on/off.
        /// </summary>
        public bool ContinuousMonitoring 
        {
            get
            {
                bool rv;
                contLock.WaitOne();
                rv = cont;
                contLock.ReleaseMutex();
                return rv;
            }
            set
            {
                contLock.WaitOne();
                if (value != cont)
                {
                    cont = value;
                    ToggleContinuous();
                }
                contLock.ReleaseMutex();
            }
        }
        /// <summary>
        /// called when continuous operation is changed.
        /// </summary>
        protected virtual void ToggleContinuous()
        {
            // Place holder, handled by the rigs.
            // The cont flag is already toggled.
        }

        protected bool rptRaw;
        public virtual bool ReportRaw
        {
            get { return rptRaw; }
            set { rptRaw = value; }
        }

        protected int ksp;
        /// <summary>
        /// Minimum keyer speed
        /// </summary>
        public virtual int MinSpeed { get { return 4; } }
        /// <summary>
        /// Maximum keyer speed.
        /// </summary>
        public virtual int MaxSpeed { get { return 60; } }
        /// <summary>
        /// Keyer speed
        /// </summary>
        public virtual int KeyerSpeed
        {
            get { return ksp; }
            set { ksp = value; }
        }

        /// <summary>
        /// Transmit antennas
        /// </summary>
        public virtual int TXAnts { get { return 2; } }
        /// <summary>
        /// Receive antennas
        /// </summary>
        public virtual int RXAnts { get { return 2; } }
        protected int txAnt;
        /// <summary>
        /// Which main antenna (0 through n)
        /// </summary>
        public virtual int TXAntenna
        {
            get { return txAnt; }
            set { txAnt = value; }
        }
        protected bool rxAnt;
        /// <summary>
        /// Auxiliary receive antenna
        /// </summary>
        public virtual bool RXAntenna
        {
            get { return rxAnt; }
            set { rxAnt = value; }
        }
        protected bool drvA;
        /// <summary>
        /// true if set to drive an amp.
        /// </summary>
        public virtual bool DriveAmp
        {
            get { return drvA; }
            set { drvA = value; }
        }

        /// <summary>
        /// bitwise antenna tuner values
        /// </summary>
        [Flags]
        public enum AntTunerVals
        {
            rx=0x01,
            tx=0x02,
            tune=0x04
        }
        protected AntTunerVals atuner;
        /// <summary>
        /// Antenna tuner value
        /// </summary>
        public virtual AntTunerVals AntennaTuner
        {
            get { return atuner; }
            set { atuner = value; }
        }

        /// <summary>
        /// RF attenuator values
        /// </summary>
        public enum RFAttenuatorVals
        {
            off,
            on
        }
        protected RFAttenuatorVals RFAtt;
        /// <summary>
        /// RF attenuator
        /// </summary>
        public virtual RFAttenuatorVals RFAttenuator
        {
            get { return RFAtt; }
            set { RFAtt = value; }
        }

        /// <summary>
        /// PreAmp values
        /// </summary>
        public enum PreAmpVals
        {
            off,
            on
        }
        protected PreAmpVals preA;
        /// <summary>
        /// PreAmp setting
        /// </summary>
        public virtual PreAmpVals PreAmp
        {
            get { return preA; }
            set { preA = value; }
        }

        /// <summary>
        /// Break-in delay maximum in milliseconds
        /// </summary>
        public virtual int BreakinDelayMax { get { return 1000; } }
        /// <summary>
        /// Break-in delay increment
        /// </summary>
        public int BreakinDelayIncrement { get { return 50; } }
        protected int bDelay;
        /// <summary>
        /// Break-in delay in milliseconds
        /// </summary>
        public virtual int BreakinDelay 
        {
            get { return bDelay; }
            set { bDelay = value; }
        }

        /// <summary>
        /// Vox delay maximum in milliseconds
        /// </summary>
        public virtual int VoxDelayMax { get { return 3000; } }
        /// <summary>
        /// Vox delay increment in milliseconds.
        /// </summary>
        public virtual int VoxDelayIncrement { get { return 150; } }
        protected int vDelay;
        /// <summary>
        /// Vox delay in milliseconds
        /// </summary>
        public virtual int VoxDelay
        {
            get { return vDelay; }
            set { vDelay = value; }
        }

        /// <summary>
        /// Vox gain maximum value
        /// </summary>
        public virtual int VoxGainMax { get { return 9; } }
        protected int vGain;
        /// <summary>
        /// Vox gain
        /// </summary>
        public virtual int VoxGain
        {
            get { return vGain; }
            set { vGain = value; }
        }

        /// <summary>
        /// Mic gain maximum value
        /// </summary>
        public virtual int MicGainMax { get { return 100; } }
        /// <summary>
        /// Mic gain increment
        /// </summary>
        public virtual int MicGainIncrement { get { return 1; } }
        protected int mGain;
        /// <summary>
        /// Mic gain.
        /// </summary>
        public virtual int MicGain
        {
            get { return mGain; }
            set { mGain = value; }
        }

        public virtual int CarrierLevelMax { get { return 100; } }
        public virtual int CarrierLevelIncrement { get { return 1; } }
        protected int cLevel;
        public virtual int CarrierLevel
        {
            get { return cLevel; }
            set { cLevel = value; }
        }

        /// <summary>
        /// speech processor states
        /// </summary>
        public enum ProcessorStates
        {
            off,
            on
        }
        protected ProcessorStates pState;
        /// <summary>
        /// speech processor state
        /// </summary>
        public virtual ProcessorStates ProcessorState
        {
            get { return pState; }
            set { pState = value; }
        }
        protected int pInLevel, pOutLevel;
        /// <summary>
        /// Processor input level maximum
        /// </summary>
        public virtual int ProcessorInputLevelMax { get { return 100; } }
        /// <summary>
        /// speech processor input level increment
        /// </summary>
        public virtual int ProcessorInputLevelIncrement { get { return 1; } }
        /// <summary>
        /// speech processor input level
        /// </summary>
        public virtual int ProcessorInputLevel
        {
            get { return pInLevel; }
            set { pInLevel = value; }
        }
        /// <summary>
        /// speech processor output maximum
        /// </summary>
        public virtual int ProcessorOutputLevelMax { get { return 100; } }
        /// <summary>
        /// speech processor output level increment
        /// </summary>
        public virtual int ProcessorOutputLevelIncrement { get { return 1; } }
        /// <summary>
        /// speech processor output level
        /// </summary>
        public virtual int ProcessorOutputLevel
        {
            get { return pOutLevel; }
            set { pOutLevel = value; }
        }

        /// <summary>
        /// (overloaded) Send a CW character.
        /// </summary>
        /// <param name="c">character to send</param>
        /// <returns>true if sent</returns>
        public virtual bool SendCW(char c)
        {
            // Rigs must override.
            return false;
        }
        /// <summary>
        /// (overloaded) Send a CW string
        /// </summary>
        /// <param name="str">string</param>
        /// <returns>true on success</returns>
        public virtual bool SendCW(string str)
        {
            // May be overridden by the rigs.
            foreach (char c in str)
            {
                if (!SendCW(c)) return false;
            }
            return true;
        }
        /// <summary>
        /// Halt the sending of CW.
        /// </summary>
        public virtual void StopCW()
        {
            // provided by the rigs.
        }
        /// <summary>
        /// Get the CW message size.
        /// </summary>
        public virtual int CWMessageSize
        {
            get;
            protected set;
        }

        internal delegate int filterGetDel(int id);
        internal delegate void filterSetDel(int id, int Val);
        public class ReceiverFilter
        {
            public string Name { get; set; }
            public int Length { get; set; }
            protected int id;
            internal filterGetDel getRtn;
            internal filterSetDel setRtn;
            public int Value
            {
                get { return (getRtn != null) ? getRtn(id) : 0; }
                set
                {
                    if (setRtn != null) setRtn(id, value);
                }
            }
            internal ReceiverFilter(int i, string n, int l,
                filterGetDel g, filterSetDel s)
            {
                id = i;
                Name = n;
                Length = l;
                getRtn = g;
                setRtn = s;
            }
        }
        public ReceiverFilter[] Filters;
#endregion

        // region - memory stuff.
#region memory stuff
        /// <summary>
        /// Memory type enumeration
        /// </summary>
        public enum MemoryTypes
        {
            Normal,
            Range,
            MemorySwitch,
            PowerOn
        }
        internal delegate void memLoadDel(MemoryData m);
        internal delegate bool memGetDel(MemoryData m);
        internal delegate void memSetDel(MemoryData m);
        internal memLoadDel LoadRtn;
        internal memGetDel GetRtn;
        internal memSetDel SetRtn;
        /// <summary>
        /// a memory data item.
        /// </summary>
        public class MemoryData
        {
            internal Mutex myLock;
            private byte cmplt;
            internal bool complete
            {
                get { return (Thread.VolatileRead(ref cmplt) == 1) ? true : false; }
                set
                {
                    byte b = (byte)((value) ? 1 : 0);
                    Thread.VolatileWrite(ref cmplt, b);
                }
            }
            //internal int tag;
            public int Number;
            public string Name;
            public bool Present;
            public MemoryTypes Type;
            public bool Split;
            public ulong[] Frequency;
            public Modes[] Mode;
            public DataModes[] DataMode;
            public ToneCTSSValue ToneCTSS;
            public float ToneFrequency;
            public float CTSSFrequency;
            public FMModes FMMode;
            public bool Lockout;
            internal object parent;
            internal MemoryData()
            {
                myLock = new Mutex();
                Frequency = new ulong[2];
                Mode = new Modes[2];
                DataMode = new DataModes[2];
            }
            internal MemoryData(object p)
            {
                myLock = new Mutex();
                complete = Present = false;
                Frequency = new ulong[2];
                Mode = new Modes[2];
                DataMode = new DataModes[2];
                parent = p;
            }
            /// <summary>
            /// Make a copy
            /// </summary>
            /// <returns>a copy of this object</returns>
            public MemoryData Copy()
            {
                MemoryData m = new MemoryData();
                for (int i = 0; i < Frequency.Length; i++)
                {
                    m.Frequency[i] = Frequency[i];
                    m.Mode[i] = Mode[i];
                    m.DataMode[i] = DataMode[i];
                }
                m.complete = complete;
                m.CTSSFrequency = CTSSFrequency;
                m.FMMode = FMMode;
                m.Lockout = Lockout;
                m.Name = Name;
                m.Number = Number;
                m.Present = Present;
                m.Split = Split;
                m.ToneCTSS = ToneCTSS;
                m.ToneFrequency = ToneFrequency;
                m.Type = Type;
                return m;
            }
        }
        /// <summary>
        /// The group of memories for this rig.
        /// </summary>
        public class MemoryGroup
        {
            internal MemoryData[] mems;
            internal MemoryGroup(int totalMemories, object p)
            {
                mems = new MemoryData[totalMemories];
                for (int i = 0; i < totalMemories; i++)
                {
                    mems[i] = new MemoryData(p);
                    mems[i].Number = i;
                }
            }
            /// <summary>
            /// Get the memory for this id
            /// </summary>
            /// <param name="id">index</param>
            /// <returns>corresponding MemoryData object</returns>
            public MemoryData this[int id]
            {
                get
                {
                    // Get the memory from the radio if necessary.
                    // Return a copy of the memory.
                    MemoryData rv = null;
                    MemoryData m = mems[id];
                    AllRadios g = (AllRadios)m.parent;
                    if (g.GetRtn != null)
                    {
                        m.myLock.WaitOne();
                        if (g.GetRtn(m))
                        {
                            rv = m.Copy();
                        }
                        m.myLock.ReleaseMutex();
                    }
                    return rv;
                }
                set
                {
                    MemoryData m = mems[id];
                    AllRadios g = (AllRadios)m.parent;
                    if (g.SetRtn != null)
                    {
                        // We don't need to lock this now.
                        g.SetRtn(value);
                    }
                }
            }
        }
        public MemoryGroup Memories;
        /// <summary>
        /// (readonly) number of memories, provided by the rigs.
        /// </summary>
        public virtual int NumberOfMemories
        {
            get { return Memories.mems.Length; }
        }
        protected int memChannel;
        /// <summary>
        /// Current memory channel number
        /// </summary>
        public virtual int CurrentMemoryChannel
        {
            get { return memChannel; }
            set { memChannel = value; }
        }
        // oldVFO is the VFO to return to when memory mode is turned off.
        protected RigCaps.VFOs oldVFO = RigCaps.VFOs.VFOA;
        /// <summary>
        /// Memory mode, true or false.
        /// </summary>
        public virtual bool MemoryMode
        {
            get { return (CurVFO == RigCaps.VFOs.None); }
            // Set provided by the rigs.
            set { }
        }
        /// <summary>
        /// Memory has been selected, keep us up to date.
        /// </summary>
        /// <param name="m">memoryData object</param>
        internal void ActuateMemory(MemoryData m)
        {
            //DBGWL("ActuateMemory " + m.Number.ToString());
            m.myLock.WaitOne();
            RXFreq = m.Frequency[0];
            TXFreq = m.Frequency[1];
            rxMd = m.Mode[0];
            txMd = m.Mode[1];
            rxDM = m.DataMode[0];
            txDM = m.DataMode[1];
            toneCT = m.ToneCTSS;
            toneFreq = m.ToneFrequency;
            CTSSFreq = m.CTSSFrequency;
            fmMd = m.FMMode;
            m.myLock.ReleaseMutex();
        }
        /// <summary>
        /// Set the specified vfo from the specified memory.
        /// </summary>
        /// <param name="n">memory number</param>
        /// <param name="vfo">vfo to set
        /// If VFO is RigCaps.VFOs.None, the last vfo is used.
        /// </param>
        /// <returns>true if it will be set.</returns>
        public bool MemoryToVFO(int n, RigCaps.VFOs vfo)
        {
            MemoryData m;
            RigCaps.VFOs newVFO = vfo;
            try
            {
                //DBGWL("MemoryToVFO:" + n.ToString() + " " + vfo.ToString());
                m = Memories[n];
                // The memory must be present (not empty).
                if (!m.Present) return false;
                // Either go to the last vfo or the specified vfo.
                if (newVFO == RigCaps.VFOs.None)
                {
                    // go to the last real VFO.
                    newVFO = oldVFO;
                    MemoryMode = false;
                }
                else
                {
                    CurVFO = newVFO;
                    oldVFO = newVFO;
                }
            }
            catch { return false; }
            // Use a thread to await the VFO change and then set the VFO.
            Thread chgThread = new Thread(new ParameterizedThreadStart(waitForVFOChange));
            chgThread.Start(new waitForVFOChangeParm(m, newVFO));
            return true;
        }
        // Used by MemoryToVFO.
        private class waitForVFOChangeParm
        {
            public MemoryData m;
            public RigCaps.VFOs vfo;
            public waitForVFOChangeParm(MemoryData mp, RigCaps.VFOs vp)
            {
                m = mp;
                vfo = vp;
            }
        }
        private void waitForVFOChange(object o)
        {
            waitForVFOChangeParm p = (waitForVFOChangeParm)o;
            // Wait for the curVFO to really change.
            int sanity = 40;
            while ((CurVFO != p.vfo) && (sanity-- > 0))
            {
                Thread.Sleep(25);
            }
            if (CurVFO != p.vfo) return;
            p.m.myLock.WaitOne();
            RXFrequency = p.m.Frequency[0];
            TXFrequency = p.m.Frequency[1];
            RXMode = p.m.Mode[0];
            RXDataMode = p.m.DataMode[0];
            ToneCTSS = p.m.ToneCTSS;
            ToneFrequency = p.m.ToneFrequency;
            CTSSFrequency = p.m.CTSSFrequency;
            //fmMd = p.m.FMMode;
            p.m.myLock.ReleaseMutex();
        }
#endregion

        // region - menu stuff
        #region menu stuff
        /// <summary>
        /// menu item type
        /// </summary>
        public enum MenuTypes
        {
            /// <summary>
            /// 0=Off,1=On, no descriptor
            /// </summary>
            OnOff,
            /// <summary>
            /// number range, takes a NumberRangeDescriptor
            /// </summary>
            NumberRange,
            /// <summary>
            /// number range where 0 is off, takes an int for highest value.
            /// </summary>
            NumberRangeOff0,
            /// <summary>
            /// Enumerated items, takes an EnumeratedDescriptor.
            /// </summary>
            Enumerated,
            /// <summary>
            /// string of text, takes a string.
            /// </summary>
            Text
        }
        /// <summary>
        /// Description and associated value.
        /// If Value is null, use relative position between Low and High.
        /// </summary>
        public class EnumAndValue
        {
            public string Description;
            public MenuTypes Type;
            public object Value;
            public EnumAndValue() { }
            /// <summary>
            /// EnumAndValue object with a numeric value and type of Enumerated.
            /// </summary>
            /// <param name="desc">description</param>
            /// <param name="v">integral value</param>
            public EnumAndValue(string desc, int v)
            {
                Description = desc;
                Value = v;
                Type = MenuTypes.Enumerated;
            }
            /// <summary>
            /// EnumAndValue object with a specified type and value.
            /// </summary>
            /// <param name="desc">description</param>
            /// <param name="t">value type</param>
            /// <param name="v">value</param>
            public EnumAndValue(string desc, MenuTypes t, object v)
            {
                Description = desc;
                Type = t;
                Value = v;
            }
        }
        internal delegate object getDel(MenuDescriptor md);
        internal delegate void setDel(MenuDescriptor md, object v);
        /// <summary>
        /// menu descriptor class
        /// </summary>
        public class MenuDescriptor
        {
            private byte cmplt;
            /// <summary>
            /// Internal:  true if the menu has been read in.
            /// </summary>
            internal bool Complete
            {
                get { return (Thread.VolatileRead(ref cmplt) != 0); }
                set { Thread.VolatileWrite(ref cmplt, (byte)((value)?1:0)); }
            }
            /// <summary>
            /// menu number
            /// </summary>
            public int Number;
            /// <summary>
            /// type enumeration, see MenuTypes
            /// </summary>
            public MenuTypes Type;
            /// <summary>
            /// text description
            /// </summary>
            public string Description;
            /// <summary>
            /// low value
            /// </summary>
            public int Low;
            /// <summary>
            /// high value
            /// </summary>
            public int High;
            /// <summary>
            /// enumerated descriptors
            /// </summary>
            public EnumAndValue[] Enumerants;
            /// <summary>
            /// Routine to get the value.
            /// This field must be dynamically initialized.
            /// </summary>
            internal getDel getRtn;
            /// <summary>
            /// routine to set the value.
            /// This field must be dynamically initialized.
            /// </summary>
            internal setDel setRtn;
            internal object val;
            /// <summary>
            /// menu's current value.
            /// This may need to await a command response.
            /// </summary>
            public object Value
            {
                get { return getRtn(this); }
                set
                {
                    // This actually does cause val to be set.
                    setRtn(this, value);
                }
            }
            internal MenuDescriptor(int n, string desc,
                MenuTypes typ, int l, int h)
            {
                // There is no Enumerants data.
                Complete = false;
                Number = n;
                Description = desc;
                Type = typ;
                Low = l;
                High = h;
                Enumerants = null;
            }
            internal MenuDescriptor(int n, string desc,
                MenuTypes typ, int l, int h, string[] e)
            {
                // If only strings are passed, the enumerants' values start at Low.
                Complete = false;
                Number = n;
                Description = desc;
                Type = typ;
                Low = l;
                High = h;
                Enumerants = new EnumAndValue[e.Length];
                for (int i=0; i<e.Length; i++)
                {
                    Enumerants[i] = new EnumAndValue(e[i], i+Low);
                }
            }
            internal MenuDescriptor(int n, string desc,
                MenuTypes typ, int l, int h, EnumAndValue[] e)
            {
                // Here the entire enumerants array is passed.
                Complete = false;
                Number = n;
                Description = desc;
                Type = typ;
                Low = l;
                High = h;
                Enumerants = e;
            }
        }
        /// <summary>
        /// Menu array
        /// </summary>
        public MenuDescriptor[] Menus;
        /// <summary>
        /// (ReadOnly) number of menus
        /// </summary>
        public virtual int NumberOfMenus
        {
            get { return Menus.Length; }
        }
#endregion

        // region - events
#region events
        /// <summary>
        /// Events that report "Complete".
        /// </summary>
        public enum CompleteEvents
        {
            memories,
            menus
        }
        /// <summary>
        /// Complete event arguments
        /// </summary>
        public class CompleteEventArgs
        {
            public CompleteEvents TheEvent;
            public DateTime TheTime;
            internal CompleteEventArgs(CompleteEvents ev)
            {
                TheEvent = ev;
                TheTime = DateTime.Now;
            }
        }
        public delegate void CompleteHandler(object sender, CompleteEventArgs e);
        /// <summary>
        /// Complete event
        /// </summary>
        public event CompleteHandler CompleteEvent;
        /// <summary>
        /// raise the complete event for this event.
        /// </summary>
        /// <param name="ev">event from CompleteEvents</param>
        protected void raiseComplete(CompleteEvents ev)
        {
            if (CompleteEvent != null)
            {
                CompleteEvent(this, new CompleteEventArgs(ev));
            }
        }
#endregion

        public AllRadios()
        {
            IsOpen = false;
#if debugFlag
            DBGTicks = DateTime.Now.Ticks;
            wasWriteLine = true;
#endif
        }

        public HandRtn InterruptHandler;
        /// <summary>
        /// Port interrupt handler
        /// </summary>
        /// <param name="o">the raw data</param>
        protected virtual void IHandler(object o)
        {
            // Stub, handled by the rigs.
        }

#if zero
        /// <summary>
        /// return number of milliseconds to wait for n characters.
        /// </summary>
        /// <param name="nChars">number of characters</param>
        /// <returns>milliseconds</returns>
        protected int waitMS(int nChars)
        {
            return nChars * 1000 / Callouts.Speed;
        }
#endif

        /// <summary>
        /// Internal routine to get radio's memories.
        /// </summary>
        protected virtual void GetMemories()
        {
            // provided by the rigs.
        }

        /// <summary>
        /// routine to send data to the rig.
        /// </summary>
        /// <param name="str">data to send</param>
        /// <returns>True if sent</returns>
        public delegate bool SendRtn(string str);
        /// <summary>
        /// Receives direct rig output
        /// </summary>
        /// <param name="o">data from the rig</param>
        public delegate void DDRcvr(object o);
        /// <summary>
        /// Process returned data
        /// </summary>
        /// <param name="o">the data</param>
        public delegate void HandRtn(object o);
        /// <summary>
        /// Callout vector
        /// </summary>
        public class OpenParms
        {
            /// <summary>
            /// Send commands to the radio
            /// </summary>
            public SendRtn SendRoutine { get; set; }
            /// <summary>
            /// routine to receive data when ContinuousMonitoring is off.
            /// </summary>
            public DDRcvr DirectDataReceiver { get; set; }
            /// <summary>
            /// Communications speed in CPS (not Baud).
            /// </summary>
            public int Speed { get; set; }
            internal void safeSend(string str)
            {
                try { SendRoutine(str); }
                catch { }
            }
            internal void safeReceiver(object o)
            {
                try { DirectDataReceiver(o); }
                catch { }
            }
        }
        /// <summary>
        /// Callout vector provided at open().
        /// </summary>
        public OpenParms Callouts;

        /// <summary>
        /// Open the radio.
        /// The communication port must be open.
        /// Turns on Continuous Monitoring.
        /// </summary>
        /// <param name="p">Callout vector, see the OpenParms class.</param>
        /// <returns>True on success</returns>
        public virtual bool Open(OpenParms p)
        {
            Callouts = p;
            // If speed is not set, assume 1000 CPS, around 9600 baud.
            if (Callouts.Speed == 0) Callouts.Speed = 1000;
            pwr = false; // Assume power is off for now.
            contLock = new Mutex();
            cont = rptRaw = false;
            InterruptHandler = IHandler;
            RIT = new RITData();
            XIT = new RITData();
            ContinuousMonitoring = true;
            IsOpen = true;
            return true;
        }

        /// <summary>
        /// Close.
        /// Call after inherritor's close has finished.
        /// </summary>
        public virtual void close()
        {
            ContinuousMonitoring = false;
            IsOpen = false;
        }

        /// <summary>
        /// True if open.
        /// </summary>
        public bool IsOpen
        {
            get;
            protected set;
        }

#if debugFlag
        long DBGTicks;
        bool wasWriteLine;
        protected void DBGW(string s)
        {
            string hdr;
            if (wasWriteLine)
            {
                long ticks = (DateTime.Now.Ticks - DBGTicks) / 10000;
                hdr = ticks.ToString();
            }
            else hdr = "";
            Debug.Write(hdr + " " + s);
            wasWriteLine = false;
        }
        protected void DBGWL(string s)
        {
            string hdr;
            if (wasWriteLine)
            {
                long ticks = (DateTime.Now.Ticks - DBGTicks) / 10000;
                hdr = ticks.ToString();
            }
            else hdr = "";
            Debug.WriteLine(hdr + " " + s);
            wasWriteLine = true;
        }
#else
        protected void DBGW(string s) {}
        protected void DBGWL(string s) {}
#endif
    }

    /// <summary>
    /// Used to select the radio by model number
    /// </summary>
    public static class RadioSelection
    {
        // supported radio ids
        const int idNone = 0;
        const int idKenwood = 200;
        const int idKenwoodTS2000 = idKenwood+14;
        const int idKenwoodTS590 = idKenwood+31;
        const int idKenwoodTS930 = idKenwood+99;

        /// <summary>
        /// The rig names and ids.
        /// </summary>
        public class RigElement
        {
            public int id;
            public string name;
            internal object rad;
            public RigElement(int num, string nam, AllRadios r)
            {
                id = num;
                name = nam;
                rad = r;
            }
        }
        /// <summary>
        /// Array of supported rigs.
        /// </summary>
        public static RigElement[] RigTable =
            {new RigElement(idNone, "None", new AllRadios()),
             new RigElement(idKenwoodTS930, "TS930", new KenwoodTS930()),
             new RigElement(idKenwoodTS590, "TS590", new KenwoodTS590()),
             new RigElement(idKenwoodTS2000, "TS2000", new KenwoodTS590()) };
        /// <summary>
        /// Get an object for the specified rig.
        /// </summary>
        /// <param name="id">model number</param>
        /// <returns>The AllRadios object for the rig.</returns>
        public static AllRadios GetRig(int id)
        {
            foreach (RigElement t in RigTable)
            {
                if (id == t.id)
                {
                    return (AllRadios)t.rad;
                }
            }
            return null;
        }
    }
}