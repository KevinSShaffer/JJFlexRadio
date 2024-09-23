using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using JJTrace;

namespace Radios
{
    class peakTimer
    {
        private int period; // the timer's period
        private Timer periodTimer;
        private const int defaultInterval = 500; // .5 seconds.
        private bool hardStop; // Used when disposing.
        public delegate void PeriodDoneDel();
        private PeriodDoneDel periodDone;

        public peakTimer(int interval, PeriodDoneDel done)
        {
            period = (interval == 0) ? defaultInterval : interval;
            periodDone = done;
            periodTimer = new Timer(timesUp);
            hardStop = false;
            periodTimer.Change(0, period); // starts the timer
        }

        private void timesUp(object state)
        {
            if (hardStop) return;
            // Call the passed delegate.
            periodDone();
        }

        public void Finished()
        {
            WaitHandle hand = new AutoResetEvent(false);
            hardStop = true;
            periodTimer.Dispose(hand);
            hand.WaitOne(period + (period / 2));
            hand.Dispose();
        }
    }

    class IndexedPeakType
    {
        /// <summary>
        /// Get the value from the rig.
        /// </summary>
        /// <returns>an integer value</returns>
        public delegate int ValueDel();
        private ValueDel fromRig;

        private int peakValue = 0;
        private peakTimer timer;

        public IndexedPeakType(ValueDel rtn, int interval)
        {
            fromRig = rtn;
            peakValue = 0;
            timer = new peakTimer(interval, periodDone);
        }

        public void Finished()
        {
            timer.Finished();
        }

        private void periodDone()
        {
            peakValue = 0;
        }

        public int Read()
        {
            int v = fromRig();
            if (v > peakValue) peakValue = v;
            return peakValue;
        }
    }

    class FloatPeakType
    {
        /// <summary>
        /// Get the value from the rig.
        /// </summary>
        /// <returns>a float value</returns>
        public delegate float ValueDel();
        private ValueDel fromRig;

        private float peakValue = 0;
        private float peakValueReset;
        private peakTimer timer;

        public FloatPeakType(ValueDel rtn, int interval, float minVal)
        {
            fromRig = rtn;
            peakValue = peakValueReset = minVal;
            timer = new peakTimer(interval, periodDone);
        }

        public void Finished()
        {
            timer.Finished();
        }

        private void periodDone()
        {
            peakValue = peakValueReset;
        }

        public float Read()
        {
            float v = fromRig();
            if (v > peakValue) peakValue = v;
            return peakValue;
        }
    }
}
