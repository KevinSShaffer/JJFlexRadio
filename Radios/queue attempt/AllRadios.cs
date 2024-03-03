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
        public virtual bool Split { get; set; }
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

        /// <summary>
        /// Vox on/off
        /// </summary>
        public virtual bool Vox { get; set; }

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
                // contLock.WaitOne(); don't think we need this.
                rv = cont;
                // contLock.ReleaseMutex();
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

        public enum MemoryTypes
        {
            Normal,
            Range
        }
        public class MemoryData
        {
            internal bool complete;
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
            public MemoryData()
            {
                complete = Present = false;
                Frequency = new ulong[2];
                Mode = new Modes[2];
                DataMode = new DataModes[2];
            }
        }
        /// <summary>
        /// serialize access to memories collection.
        /// </summary>
        protected Mutex MemoryLock;
        /// <summary>
        /// Memory collection
        /// </summary>
        protected Collection<MemoryData> Memories;
        /// <summary>
        /// Current memory channel number
        /// </summary>
        public virtual int CurrentMemoryChannel { get; set; }
        /// <summary>
        /// Get the specified memory
        /// </summary>
        /// <param name="memno">memory number</param>
        /// <returns>The MemoryData object or null.</returns>
        public MemoryData GetMemoryByNumber(int memno)
        {
            return GetMemoryByNumber(memno,true);
        }
        /// <summary>
        /// Get the memory for this number.
        /// </summary>
        /// <param name="num">number</param>
        /// <param name="mustBeComplete">True if must be complete</param>
        /// <returns>the MemoryData object or null.</returns>
        protected MemoryData GetMemoryByNumber(int num, bool mustBeComplete)
        {
            MemoryData rv = null;
            MemoryLock.WaitOne();
            foreach (MemoryData m in Memories)
            {
                if (mustBeComplete && !m.complete) continue;
                if (m.Number == num)
                {
                    rv = m;
                    break;
                }
            }
            MemoryLock.ReleaseMutex();
            return rv;
        }
        /// <summary>
        /// Expose memory info.
        /// </summary>
        /// <param name="memno">memory number</param>
        /// <returns>True if memory found in the collection.</returns>
        protected bool ActuateMemory(int memno)
        {
            bool rv = false;
            Debug.Write("ActuateMemory:");
            MemoryLock.WaitOne();
            MemoryData m = GetMemoryByNumber(memno, true);
            if (m != null)
            {
                rv = true;
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
            }
            MemoryLock.ReleaseMutex();
            return rv;
        }

        /// <summary>
        /// thread-safe queue.
        /// </summary>
        private Queue TheQ;
        private Mutex QLock;
        private Thread QThread;

        public AllRadios()
        {
        }

        public HandRtn InterruptHandler;
        /// <summary>
        /// Port interrupt handler
        /// </summary>
        /// <param name="o">the raw data</param>
        protected virtual void IHandler(object o)
        {
            // Stub, handled by the rigs.
            // This should enqueue commands.
        }

        /// <summary>
        /// Q handler thread.
        /// Handles elements queued by IHandler.
        /// </summary>
        private void QHandler()
        {
            const int shortHoldoff = 50;
            //const int longHoldoff = 100;
            int holdoff = shortHoldoff;
            while (true)
            {
                QLock.WaitOne();
                int ct = TheQ.Count;
                if (ct == 0)
                {
                    QLock.ReleaseMutex();
                    Thread.Sleep(holdoff);
                }
                else
                {
                    while (ct-- > 0)
                    {
                        //if (ct > 2) Debug.WriteLine("Queue count " + ct.ToString());
                        object o = TheQ.Dequeue();
                        QLock.ReleaseMutex();
                        CommandHandler(o);
                        if (ct > 0) QLock.WaitOne();
                    }
                    // ct is 0, lock is released.
                }
            }
        }
        /// <summary>
        /// Enqueue 
        /// </summary>
        /// <param name="o">object to enqueue</param>
        protected void Enq(object o)
        {
            QLock.WaitOne();
            TheQ.Enqueue(o);
            QLock.ReleaseMutex();
        }
        /// <summary>
        /// Handle commands/directives from the rig.
        /// </summary>
        /// <param name="o">command object</param>
        protected virtual void CommandHandler(object o)
        {
            // provided by the rigs.
        }

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
        /// Receives rig output when ContinuousMonitoring is off.
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
            pwr = false; // Assume power is off for now.
            contLock = new Mutex();
            MemoryLock = new Mutex();
            cont = rptRaw = false;
            InterruptHandler = IHandler;
            RIT = new RITData();
            XIT = new RITData();
            Memories = new Collection<MemoryData>();
            //Q = new Queue();
            //TheQ = Queue.Synchronized(Q);
            TheQ = new Queue();
            QLock = new Mutex();
            QThread = new Thread(new ThreadStart(QHandler));
            QThread.Start();
            Thread.Sleep(0);
            ContinuousMonitoring = true;
            return true;
        }

        /// <summary>
        /// Close.
        /// Call after inherritor's close has finished.
        /// </summary>
        public virtual void close()
        {
            ContinuousMonitoring = false;
            if ((QThread != null) && QThread.IsAlive) QThread.Abort();
        }
    }

    /// <summary>
    /// Used to select the radio by model number
    /// </summary>
    public static class RadioSelection
    {
        // supported radio ids
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
        public static RigElement[] RigTable = { new RigElement(idKenwoodTS930, "TS930", new KenwoodTS930()),
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
