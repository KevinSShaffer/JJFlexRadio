using System;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace JJTrace
{
    public static partial class Tracing
    {
        /// <summary>
        /// (Overloaded) Trace and report an error.
        /// The trace includes the stack trace.
        /// </summary>
        /// <param name="ex">The exception</param>
        public static void ErrMessageTrace(Exception ex)
        {
            ErrMessageTrace(ex, false, true);
        }
        /// <summary>
        /// (Overloaded) Trace and report an error.
        /// The trace includes the stack trace.
        /// </summary>
        /// <param name="ex">The exception</param>
        /// <param name="inner">true to include the inner exception</param>
        public static void ErrMessageTrace(Exception ex, bool inner)
        {
            ErrMessageTrace(ex, inner, true);
        }
        /// <summary>
        /// (Overloaded) Trace and optionally report an error.
        /// The trace includes the stack trace.
        /// </summary>
        /// <param name="ex">the exception</param>
        /// <param name="inner">true to report inner exception</param>
        /// <param name="msg">true to produce an error message dialogue</param>
        public static void ErrMessageTrace(Exception ex, bool inner, bool msg)
        {
            string str = ex.Message + "\r\n";
            if (inner & (ex.InnerException != null)) str += ex.InnerException.Message + "\r\n";
            string str2 = str += ex.StackTrace;
            TraceLine("Exception:" + str2, TraceLevel.Error);
            if (msg) MessageBox.Show(str, "Exception", MessageBoxButtons.OK);
        }
    }
}
