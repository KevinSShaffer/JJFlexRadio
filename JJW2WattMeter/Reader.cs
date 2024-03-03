using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using JJTrace;

namespace JJW2WattMeter
{
    internal class Reader
    {
        private W2 theMeter;
        private Thread readThread;
        private const int defaultReadInterval = 100; // 100ms
        private int readInterval;

        /// <summary>
        /// Meter reader.
        /// </summary>
        /// <param name="meter">meter object</param>
        /// <param name="interval">(optional) interread interval</param>
        public Reader(W2 meter, int interval=defaultReadInterval)
        {
            theMeter = meter;
            readInterval = interval;

            readThread = new Thread(readProc);
            readThread.Name = "W2 reader";
            readThread.Start();
        }

        private bool closing = false;
        private void readProc()
        {
            // Make sure the meter's power setting is set.
            while (!theMeter.MeterPowerSetting(theMeter.ConfigInfo.Info.PowerType) & !closing)
            {
                Thread.Sleep(readInterval);
            }

            if (closing)
            {
                Tracing.TraceLine("readProc:meter was off", TraceLevel.Info);
            }

            try
            {
                while (!closing)
                {
                    if (theMeter.IsUseable)
                    {
                        theMeter.ForwardPower = theMeter.serial.Read("f");
                        theMeter.SWR = theMeter.serial.Read("s");
                    }

                    Thread.Sleep(readInterval);
                }
            }
            catch (ThreadInterruptedException)
            {
                Tracing.TraceLine("readProc stopping", TraceLevel.Info);
            }
            catch(Exception ex)
            {
                Tracing.TraceLine("readProc exception:" + ex.Message, TraceLevel.Error);
            }
            Tracing.TraceLine("readProc:exiting", TraceLevel.Info);
        }

        /// <summary>
                /// Close - stop readThread.
        /// </summary>
        internal void Close()
        {
            closing = true;
            try
            {
                readThread.Join();
            }
            catch (Exception ex)
            {
                Tracing.TraceLine("W2 reader closing exception:" + ex.Message, TraceLevel.Error);
            }
        }
    }
}
