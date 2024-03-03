using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MsgLib
{
    public class CalledUserControl: UserControl
    {
        public MessageForm Form;
        public OptionalMessageElement Message;

        public CalledUserControl() { }
    }

    /// <summary>
    /// Contains the optional message and supporting info.
    /// </summary>
    public class OptionalMessageElement
    {
        /// <summary>
        /// Required message tag.
        /// </summary>
        public string Tag;
        /// <summary>
        /// Message dialogue title
        /// </summary>
        public string Title;
        /// <summary>
        /// Required Message text.
        /// </summary>
        public string Text;
        /// <summary>
        /// Result sent to the config routine.
        /// </summary>
        public DialogResult Result;
        /// <summary>
        /// True if this is to be ignored in future.
        /// </summary>
        public bool Ignore;
        /// <summary>
        /// The user control to use, or null for the default.
        /// </summary>
        public CalledUserControl Control;
        /// <summary>
        /// optional Application-specific data.
        /// </summary>
        public object[] ApplicationData;
        /// <summary>
        /// optional message and supporting info.
        /// </summary>
        /// <param name="text">the message</param>
        /// <param name="tag">the message's tag</param>
        public OptionalMessageElement(string text, string tag)
        {
            Tag = tag;
            Text = text;
        }
        /// <summary>
        /// optional message and supporting info.
        /// </summary>
        /// <param name="tag">message tag</param>
        /// <param name="rslt">result</param>
        /// <param name="ign">ignore flag</param>
        public OptionalMessageElement(string tag, DialogResult rslt, bool ign)
        {
            Tag = tag;
            Result = rslt;
            Ignore = ign;
        }
    }

    /// <summary>
    /// Put up a message with the option not to show again.
    /// </summary>
    public class OptionalMessage
    {
        public delegate bool ConfigResultDel(OptionalMessageElement[] items);
        /// <summary>
        /// User-supplied routine to save the result if to be ignored in future.
        /// </summary>
        public static ConfigResultDel ConfigResult = null;

        public delegate OptionalMessageElement[] ConfigRetrieveDel();
        /// <summary>
        /// Retrieve any configured message ignores.
        /// </summary>
        public static ConfigRetrieveDel ConfigRetrieve;
        private static OptionalMessageElement[] configInfo;

        /// <summary>
        /// Called by the main program to perform setup.
        /// </summary>
        /// <param name="rtn">routine to save an ignore request</param>
        /// <param name="rtrvRtn">routine to retrieve config data</param>
        public static void Setup(ConfigResultDel rtn, ConfigRetrieveDel rtrvRtn)
        {
            ConfigResult = rtn;
            ConfigRetrieve = rtrvRtn;

            configInfo = ConfigRetrieve();
        }

        /// <summary>
        /// Show an optional message
        /// </summary>
        /// <param name="msg">the message string</param>
        /// <param name="title">the title string</param>
        /// <param name="tag">the message's tag string</param>
        public static void Show(string msg, string title, string tag)
        {
            OptionalMessageElement el = new OptionalMessageElement(msg, tag);
            if (!string.IsNullOrEmpty(title)) el.Title = title;
            el.Control = new DefaultUserControl(el);
            Show(el);
        }

        /// <summary>
        /// Show an optional message
        /// </summary>
        /// <param name="el">an OptionalMessageElement object</param>
        public static void Show(OptionalMessageElement el)
        {
            MessageForm form = new MessageForm(el);
            el.Result = form.ShowDialog();
        }

        /// <summary>
        /// Clear the ignore states
        /// </summary>
        public static void Clear()
        {
            if (configInfo == null) return;
            foreach (OptionalMessageElement el in configInfo)
            {
                el.Ignore = false;
            }
            ConfigResult(configInfo);
        }

        internal static OptionalMessageElement FindItem(string tag)
        {
            if (configInfo == null) return null;
            foreach (OptionalMessageElement el in configInfo)
            {
                if ((el != null) && (el.Tag == tag)) return el;
            }
            return null;
        }

        internal static void WriteResult(OptionalMessageElement item)
        {
            OptionalMessageElement el = FindItem(item.Tag);
            if (el == null)
            {
                // Add to the array
                if (configInfo == null)
                {
                    configInfo = new OptionalMessageElement[1];
                    configInfo[0] = item;
                }
                else
                {
                    OptionalMessageElement[] newInfo = new OptionalMessageElement[configInfo.Length + 1];
                    newInfo[0] = item;
                    Array.ConstrainedCopy(configInfo, 0, newInfo, 1, configInfo.Length);
                    configInfo = newInfo;
                }
            }
            else
            {
                // replace
                configInfo[Array.IndexOf(configInfo, el)] = item;
            }

            ConfigResult(configInfo);
        }
    }
}
