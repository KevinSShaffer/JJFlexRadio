using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using JJTrace;

namespace Radios
{
    public partial class AllRadios
    {
        protected bool panning = false;
        /// <summary>
        /// Pan frequency/meter event
        /// </summary>
        public class PanEventArg
        {
            /// <summary>
            /// frequency in HZ
            /// </summary>
            public ulong Frequency;
            /// <summary>
            /// Raw meter value
            /// </summary>
            public int Meter;
            internal PanEventArg(ulong f, int m)
            {
                Frequency = f;
                Meter = m;
            }
        }
        public delegate void PanEventRoutine(object sender, PanEventArg e);
        /// <summary>
        /// Pan Event handler
        /// </summary>
        public event PanEventRoutine PanEvent;
        protected void raisePanEvent(ulong frequency, int meter)
        {
            bool raiseIt = (PanEvent != null);
            Tracing.TraceLine("raisePanEvent:" + raiseIt.ToString() +
                " " + frequency.ToString() + " " + meter.ToString(), TraceLevel.Info);
            if (raiseIt)
            {
                PanEvent(this, new PanEventArg(frequency, meter));
            }
        }

        /// <summary>
        /// Pan exception event
        /// </summary>
        public class PanExceptionArg
        {
            /// <summary>
            /// exception
            /// </summary>
            public Exception ex;
            internal PanExceptionArg(Exception e)
            {
                ex = e;
            }
        }
        public delegate void PanExceptionRoutine(object sender, PanExceptionArg e);
        /// <summary>
        /// Pan exception handler
        /// </summary>
        public event PanExceptionRoutine PanException;
        protected void raisePanException(Exception e)
        {
            bool raiseIt = (PanException != null);
            Tracing.TraceLine("raisePanException:" + raiseIt.ToString() +
                " " + e, TraceLevel.Info);
            if (raiseIt)
            {
                PanException(this, new PanExceptionArg(e));
            }
        }

        /// <summary>
        /// (readOnly) True if can do panning.
        /// </summary>
        public bool PanCapable
        {
            get { return myCaps.HasCap(RigCaps.Caps.Pan); }
        }

        private Thread panThread;
        private int panRestoreAGC;
        public void PanStart()
        {
            Tracing.TraceLine("PanStart: " + panFrequency.Low.ToString() + " " +
                panFrequency.High.ToString() + " " + panFrequency.Increment.ToString(),
                TraceLevel.Info);
            if (!PanCapable)
            {
                Tracing.TraceLine("PanStart:not capable", TraceLevel.Error);
                return;
            }
            panThread = new Thread(panner);
            panThread.Name = "panThread";
            try { panThread.Start(); }
            catch (Exception ex)
            { Tracing.TraceLine("PanStart:" + ex.Message, TraceLevel.Error); }
            Thread.Sleep(0);
        }

        public void PanStop()
        {
            Tracing.TraceLine("PanStop", TraceLevel.Info);
            try
            {
                if ((panThread != null) && panThread.IsAlive) panThread.Abort();
            }
            catch (Exception ex)
            {
                Tracing.TraceLine("PanStop exception:" + ex.Message, TraceLevel.Error);
            }
        }

        /// <summary>
        /// Set panning low frequency.
        /// </summary>
        public ulong PanLow
        {
            get { return panFrequency.Low; }
            set
            {
                if (panning) PanStop();
                panFrequency.Low = value;
            }
        }
        /// <summary>
        /// Set panning high frequency.
        /// </summary>
        public ulong PanHigh
        {
            get { return panFrequency.High; }
            set
            {
                if (panning) PanStop();
                panFrequency.High = value;
            }
        }
        /// <summary>
        /// Set panning increment in HZ.
        /// </summary>
        public ulong PanIncrement
        {
            get { return panFrequency.Increment; }
            set
            {
                if (panning) PanStop();
                panFrequency.Increment = value;
            }
        }

        protected class rigOutputArg
        {
            public RigCaps.Caps Capability;
            public ulong Value;
            public rigOutputArg(RigCaps.Caps c, ulong o)
            {
                Capability = c;
                Value = o;
            }
        }
        protected delegate void RigOutputHandler(object sender, rigOutputArg e);
        protected event RigOutputHandler rigOutput;
        protected void raiseRigOutput(RigCaps.Caps capability, ulong value)
        {
            bool raiseIt = (rigOutput != null);
            Tracing.TraceLine("raiseRigOutput:" + raiseIt.ToString() +
                " " + capability.ToString() + " " + value.ToString(), TraceLevel.Info);
            if (raiseIt)
            {
                rigOutput(this, new rigOutputArg(capability, value));
            }
        }

        private enum panStates : byte
        {
            off, freq, sMeter, done
        }
        private byte panState = (byte)panStates.off;
        private static class panFrequency
        {
            public static ulong Value;
            public static ulong Low, High;
            public static ulong Increment;
            public static void Next()
            {
                Value += Increment;
                if (Value > High) Value = Low;
            }
        }

        private void rigPanHandler(object sender, rigOutputArg e)
        {
            Tracing.TraceLine("rigPanHandler:" + panState.ToString() + " " +
                e.Capability.ToString() + " " + e.Value.ToString(),
                TraceLevel.Info);
            // Ignore if off.
            if ((panStates)Thread.VolatileRead(ref panState) == panStates.off) return;
            switch (e.Capability)
            {
                case RigCaps.Caps.FrGet:
                    if ((panState == (byte)panStates.freq) &&
                        (e.Value == panFrequency.Value))
                    {
                        Thread.VolatileWrite(ref panState, (byte)panStates.sMeter);
                    }
                    break;
                case RigCaps.Caps.SMGet:
                    if (panState == (byte)panStates.sMeter)
                    {
                        panState = (byte)panStates.done;
                    }
                    break;
            }
        }

        /// <summary>
        /// The panning thread.
        /// </summary>
        private void panner()
        {
            Tracing.TraceLine("panner", TraceLevel.Info);
            Exception noFreqChange = new Exception("The frequency didn't change during panning");
            try
            {
                // Set AGC to the lowest value.
                panRestoreAGC = AGC;
                AGC = AGCFast;
                panning = true;
                // First send a frequency.
                // Upon receipt, request meter reading.
                Thread.VolatileWrite(ref panState, (byte)panStates.off);
                panFrequency.Value = panFrequency.Low;
                rigOutput += rigPanHandler;
                while (true)
                {
                    int sanity;
                    if (RXFrequency == panFrequency.Value)
                    {
                        Thread.VolatileWrite(ref panState, (byte)panStates.sMeter);
                    }
                    else
                    {
                        // Set the frequency
                        panState = (byte)panStates.freq;
                        RXFrequency = panFrequency.Value;
                        // panState should go to at least panStates.sMeter.
                        // Don't wait more than .5 seconds.
                        sanity = 500 / 5;
                        // First await the frequency change.
                        while ((sanity-- > 0) &&
                            (Thread.VolatileRead(ref panState) < (byte)panStates.sMeter))
                        {
                            Thread.Sleep(5);
                        }
                    }
                    if (Thread.VolatileRead(ref panState) < (byte)panStates.sMeter)
                    {
                        throw noFreqChange;
                    }
                    // Now give the AGC some time.
                    Thread.Sleep(AGCMinTime);
                    // Get the SMeter reading.
                    // However, panState might have been set to done.
                    Thread.VolatileWrite(ref panState, (byte)panStates.sMeter);
                    UpdateMeter();
                    sanity = 500 / 5;
                    while ((sanity-- > 0) &&
                        ((panStates)Thread.VolatileRead(ref panState) != panStates.done))
                    {
                        Thread.Sleep(5);
                    }
                    panState = (byte)panStates.off;
                    raisePanEvent(panFrequency.Value, RawSMeter);
                    panFrequency.Next();
                }
            }
            catch (Exception ex)
            {
                rigOutput -= rigPanHandler;
                AGC = panRestoreAGC;
                panning = false;
                if (ex.Equals(noFreqChange))
                {
                    Tracing.TraceLine("Panning exception:" + ex.Message, TraceLevel.Error);
                    raisePanException(ex);
                }
                else
                {
                    // likely an abort.
                    Tracing.TraceLine("panner interrupt:" + ex.Message, TraceLevel.Error);
                }
            }
        }
    }
}
