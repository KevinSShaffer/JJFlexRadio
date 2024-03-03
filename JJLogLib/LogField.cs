using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JJLogLib
{
    /// <summary>
    /// Define a log field
    /// </summary>
    public class LogField
    {
        /// <summary>
        /// field attributes: display, log
        /// </summary>
        [Flags]
        public enum fieldAttributes
        {
            none = 0,
            display = 1,
            log = 2
        }
        public fieldAttributes Attribute;
        public bool IsDisplayed { get { return ((int)(Attribute & fieldAttributes.display) != 0); } }
        public bool IsLogged { get { return ((int)(Attribute & fieldAttributes.log) != 0); } }
        /// <summary>
        /// Special processing when retrieving data.
        /// </summary>
        /// <param name="fld">the LogField structure</param>
        /// <param name="text">The field's text.
        /// If null, the field has no control item.</param>
        /// <returns>the string to return</returns>
        internal delegate string getDel(LogField fld, string text);
        /// <summary>
        /// Special processing when setting an item.
        /// The routine must set the field's text, fld.control.text, on the form.
        /// </summary>
        /// <param name="fld">the LogField of the field.</param>
        /// <param name="text">the text to set</param>
        internal delegate void setDel(LogField fld, string text);
        internal getDel GetProcessor = null;
        internal setDel setProcessor = null;
        /// <summary>
        /// Item's text
        /// </summary>
        public string Item
        {
            get
            {
                string rv = (control != null)? control.Text: null;
                if (GetProcessor != null) rv = GetProcessor(this, rv);
                return rv;
            }
            set
            {
                if (control != null)
                {
                    if (setProcessor != null) setProcessor(this, value);
                    else control.Text = value;
                }
            }
        }
        /// <summary>
        /// ADIF Tag
        /// </summary>
        public string ADIFTag;
        /// <summary>
        /// integer key ID from KeyCommands.CommandValues
        /// </summary>
        public int KeyID;
        /// <summary>
        /// KeyCommands.CommandValue for the key.
        /// </summary>
        public string KeyName;
        /// <summary>
        /// the form field
        /// </summary>
        public Control control;
        internal string originalContent;

        internal LogField(string tag, Control c)
        {
            setup(tag, c, fieldAttributes.display | fieldAttributes.log,
                null, null, null);
        }
        internal LogField(string tag, Control c, fieldAttributes attr)
        {
            setup(tag, c, attr, null, null, null);
        }
        internal LogField(string tag, Control c, string n)
        {
            setup(tag, c, fieldAttributes.display | fieldAttributes.log, n,
                null, null);
        }
        internal LogField(string tag, Control c, fieldAttributes attr, string n)
        {
            setup(tag, c, attr, n, null, null);
        }
        /// <summary>
        /// Define a log field.
        /// </summary>
        /// <param name="tag">ADIF tag</param>
        /// <param name="c">the control</param>
        /// <param name="attr">attributes</param>
        /// <param name="n">key name or null (see KeyCommands.vb)</param>
        /// <param name="g">routine to call to get the value, or null</param>
        /// <param name="s">routine to call to set the value, or null</param>
        internal LogField(string tag, Control c, fieldAttributes attr, string n,
            getDel g, setDel s)
        {
            setup(tag, c, attr, n, g, s);
        }

        private void setup(string tag, Control c, fieldAttributes attr,
            string keyname, getDel g, setDel s)
        {
            ADIFTag = tag;
            control = c;
            Attribute = attr;
            KeyName = keyname;
            GetProcessor = g;
            setProcessor = s;
            Item = "";
        }
    }
}
