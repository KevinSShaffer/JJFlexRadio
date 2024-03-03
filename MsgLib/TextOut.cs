using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using JJTrace;

namespace MsgLib
{
    public static class TextOut
    {
        private delegate void showTextDel(Control tb, string str, bool nlFlag, bool clearFlag);
        private static showTextDel showText = (Control tb, string str, bool nlFlag, bool clearFlag) =>
        {
            if (clearFlag) tb.Text = str;
            else tb.Text += str;
            if (nlFlag) tb.Text += Environment.NewLine;
            //tb.Focus();
        };

        /// <summary>
        /// Display text to a control.
        /// Optionally uses invoke if required.
        /// </summary>
        /// <param name="tb">the control</param>
        /// <param name="str">string to output</param>
        /// <param name="nlFlag">add a new line, default is true</param>
        /// <param name="clearFlag">clear the text before writing, default is false</param>
        public static void DisplayText(Control tb, string str, bool nlFlag, bool clearFlag)
        {
            if (tb.InvokeRequired) tb.Invoke(showText, new object[] { tb, str, nlFlag, clearFlag });
            else showText(tb, str, nlFlag, clearFlag);
        }

        public static void DisplayText(Control tb, string str, bool nlFlag)
        {
            DisplayText(tb, str, nlFlag, false);
        }

        public static void DisplayText(Control tb, string str)
        {
            DisplayText(tb, str, true, false);
        }

        public delegate void GenericFunctionDel();
        /// <summary>
        /// Perform the specified function on specified control, invoke if necessary.
        /// </summary>
        /// <param name="ctl">a Control</param>
        /// <param name="func">a void function with no parameters</param>
        public static void PerformGenericFunction(Control ctl, GenericFunctionDel func)
        {
            try
            {
                if (ctl.InvokeRequired) ctl.Invoke(func);
                else func();
            }
            catch (Exception ex)
            {
                Tracing.TraceLine("PerformGenericFunction error:" + ex.Message, System.Diagnostics.TraceLevel.Error);
            }
        }
    }
}
