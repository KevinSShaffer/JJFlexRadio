using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace JJTrace
{
    public static partial class Tracing
    {
        /// <summary>
        /// JJTrace.dll version
        /// </summary>
        public static Version Version
        {
            get
            {
                Assembly asm = Assembly.GetExecutingAssembly();
                AssemblyName asmName = asm.GetName();
                return asmName.Version;
            }
        }

        private static long beginTicks;
        private static string theFile = "";

        private static bool _on = false;
        /// <summary>
        /// True if tracing is on.
        /// </summary>
        public static bool On
        {
            get { return _on; }
            set
            {
                if (_on != value)
                {
                    if (_on) Trace.Close();
                    else Trace.AutoFlush = true;
                    if (value & (theFile == "")) return;
                    _on = value;
                }
            }
        }

        /// <summary>
        /// the trace switch
        /// </summary>
        public static TraceSwitch TheSwitch { get; set; }

        private static TextWriterTraceListener listener = null;
        /// <summary>
        /// the trace file
        /// </summary>
        public static string TraceFile
        {
            get { return theFile; }
            set
            {
                if ((value == null) || (value == "") || _on) return;
                if (listener != null) Trace.Listeners.Remove(listener);
                theFile = value;
                listener = new TextWriterTraceListener(File.Create(theFile));
                Trace.Listeners.Add(listener);
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
                if (Debugger.IsAttached) Debug.WriteLine(tks.ToString() + " " + str);
                else Trace.WriteLine(tks.ToString() + " " + str);
            }
        }
    }
}
