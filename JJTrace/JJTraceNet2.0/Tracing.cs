using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace JJTrace
{
    public static class Tracing
    {
        private static long beginTicks;
        private static string theFile = "";

        private static bool wasOn = false;
        /// <summary>
        /// True if tracing is on.
        /// </summary>
        public static bool On
        {
            get { return wasOn; }
            set
            {
                if (wasOn) Trace.Close();
                wasOn = value;
            }
        }

        /// <summary>
        /// the trace switch
        /// </summary>
        public static TraceSwitch TheSwitch { get; set; }

        /// <summary>
        /// the trace file
        /// </summary>
        public static string TraceFile
        {
            get { return theFile; }
            set
            {
                if (theFile != "") Trace.Listeners.Remove(theFile);
                theFile = value;
                TextWriterTraceListener l = new TextWriterTraceListener(File.Create(theFile));
                Trace.Listeners.Add(l);
                Trace.AutoFlush = true;
            }
        }

        static Tracing()
        {
            TheSwitch = new TraceSwitch("TraceSwitch", "from .config file");
            beginTicks = DateTime.Now.Ticks;
        }

        /// <summary>
        /// Unconditionally trace a line.
        /// </summary>
        /// <param name="str">string to trace</param>
        public static void TraceLine(string str)
        {
            if (!On) return;
            long tks = (DateTime.Now.Ticks - beginTicks)/10000;
            Trace.WriteLine(tks.ToString() + " " + str);
        }
        /// <summary>
        /// Conditionally trace a line for this level.
        /// </summary>
        /// <param name="str">string to trace</param>
        /// <param name="lvl">level at which to trace.</param>
        public static void TraceLine(string str, TraceLevel lvl)
        {
            if (!On) return;
            long tks = (DateTime.Now.Ticks - beginTicks) / 10000;
            if (TheSwitch.Level >= lvl)
            {
                Trace.WriteLine(tks.ToString() + " " + str);
            }
        }
    }
}
