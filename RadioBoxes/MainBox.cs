using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace RadioBoxes
{
    /// <summary>
    /// Main window box
    /// </summary>
    public partial class MainBox : UserControl
    {
        /// <summary>
        /// RadioBoxes.dll version
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

        /// <summary>
        /// Field attribute class
        /// </summary>
        public class Field
        {
            internal string Text;
            /// <summary>
            /// Field's key.
            /// </summary>
            public string Key;
            /// <summary>
            /// field length
            /// </summary>
            public int Length;
            /// <summary>
            /// left-side delimiter
            /// </summary>
            public string LeftDelim, RightDelim;
            /// <summary>
            /// field handler
            /// </summary>
            public HandlerDel Handler;
            internal int pos;
            public Field() { }
            /// <summary>
            /// provide field parameters
            /// </summary>
            /// <param name="key">key, used to locate this field</param>
            /// <param name="ln">field length</param>
            /// <param name="ld">left-side delimiter</param>
            /// <param name="rd">right-side delimiter</param>
            /// <param name="h">field handler delegate</param>
            public Field(string key, int ln, string ld, string rd, HandlerDel h)
            {
                Key = key;
                const int maxSize = 20;
                Length = (ln < maxSize) ? ln : maxSize;
                LeftDelim = ld;
                RightDelim = rd;
                Handler = h;
                Text = blanks.Substring(0, Length);
            }
        }
        private Field[] fields;
        private const string blanks = "                    ";
        /// <summary>
        /// field handler delegate
        /// </summary>
        /// <param name="o">input object</param>
        public delegate void HandlerDel(object o);

        /// <summary>
        /// Field dictionary for locating fields.
        /// </summary>
        public Dictionary<string, Field> FieldDictionary;

        /// <summary>
        /// number of fields
        /// </summary>
        public int NumberOfFields
        {
            get { return fields.Length; }
        }

        /// <summary>
        /// True if a field's value changed.
        /// </summary>
        public bool Changed
        {
            get;
            protected set;
        }

        /// <summary>
        /// Populate the box with fields, and build FieldDictionary.
        /// </summary>
        /// <param name="flds">array of Field structures</param>
        public void Populate(Field[] flds)
        {
            fields = new Field[flds.Length];
            FieldDictionary = new Dictionary<string, Field>();
            int lastPos = 0;
            for (int i=0; i<flds.Length; i++)
            {
                fields[i] = new Field(flds[i].Key, flds[i].Length,flds[i].LeftDelim,flds[i].RightDelim,flds[i].Handler);
                fields[i].pos = lastPos + flds[i].LeftDelim.Length;
                lastPos += flds[i].LeftDelim.Length + flds[i].Length + flds[i].RightDelim.Length;
                // This will fail for a duplicate key.
                FieldDictionary.Add(fields[i].Key, fields[i]);
            }
            Display(); // This will also allow the cursor position to be set.
            Changed = false;
        }

        /// <summary>
        /// Read the field's value.
        /// </summary>
        /// <param name="key">field key in FieldDictionary</param>
        /// <returns>the field's text</returns>
        public string Read(string key)
        {
            string rv;
            try { rv = FieldDictionary[key].Text; }
            catch { rv = ""; }
            return rv;
        }

        /// <summary>
        /// Write the specified field.
        /// The text is right-justified.
        /// </summary>
        /// <param name="key">field key in FieldDictionary</param>
        /// <param name="txt">Text to write</param>
        public void Write(string key, string txt)
        {
            try
            {
                Field fld = FieldDictionary[key];
                string newText;
                int ln = txt.Length;
                int padLen = fld.Length - ln;
                if (padLen > 0)
                    newText = blanks.Substring(0, padLen) + txt.Substring(0, ln);
                else if (padLen < 0)
                    newText = txt.Substring(-padLen);
                else newText = txt;
                if (newText != fld.Text)
                {
                    Changed = true;
                    fld.Text = newText;
                }        
            }
            catch { }
        }

        /// <summary>
        /// field's position
        /// </summary>
        /// <param name="key">field key in FieldDictionary</param>
        /// <returns>offset within the box</returns>
        public int Position(string key)
        {
            try { return FieldDictionary[key].pos; }
            catch { return -1; }
        }

        /// <summary>
        /// get the field given the position
        /// </summary>
        /// <param name="pos">position</param>
        public Field PositionToField(int pos)
        {
            try
            {
                foreach(Field fld in fields)
                {
                    if ((pos >= fld.pos) & (pos < fld.pos + fld.Length))
                    {
                        return fld;
                    }
                }
                return null;
            }
            catch { return null; }
        }

        /// <summary>
        /// field's length
        /// </summary>
        /// <param name="key">field key in FieldDictionary</param>
        /// <returns>length</returns>
        public int Length(string key)
        {
            try { return FieldDictionary[key].Length; }
            catch { return 0; }
        }

        /// <summary>
        /// execute the field's function
        /// </summary>
        /// <param name="key">field key in FieldDictionary</param>
        /// <param name="o">function input</param>
        /// <returns>true if function is present</returns>
        public bool Function(string key, object o)
        {
            HandlerDel h;
            bool rv;
            try { h = FieldDictionary[key].Handler; }
            catch { return false; }
            rv = (h != null);
            if (rv) h(o);
            return rv;
        }

        /// <summary>
        /// return true if field is empty
        /// </summary>
        /// <param name="key">field key in FieldDictionary</param>
        /// <returns>true if empty</returns>
        public bool IsEmpty(string key)
        {
            return (Length(key) == 0);
        }

        /// <summary>
        /// Get the entire text.
        /// This resets the Changed flag.
        /// </summary>
        public override string Text
        {
            get
            {
                Changed = false;
                string rv = "";
                //if (fields == null) return rv;
                foreach (Field f in fields)
                {
                    rv += f.LeftDelim + f.Text + f.RightDelim;
                }
                return rv;
            }
        }

        /// <summary>
        /// Display the text.
        /// This resets the Changed flag.
        /// </summary>
        public void Display()
        {
            string txt = Text; // resets the Changed flag.
            if (this.InvokeRequired)
            {
                dispRtn d = new dispRtn(disp);
                this.Invoke(d, new object[] { txt });
            }
            else disp(txt);
        }
        private delegate void dispRtn(string txt);
        private void disp(string txt)
        {
            int s = Box.SelectionStart;
            int l = Box.SelectionLength;
            Box.Text = txt;
            Box.SelectionStart = s;
            Box.SelectionLength = l;
            //Box.DeselectAll();
        }

        /// <summary>
        /// Clear and display the box.
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < fields.Length; i++) Write(fields[i].Key, "");
            Display();
        }

        protected delegate int intGet();
        protected delegate void intSet(int v);
        /// <summary>
        /// SelectionStart
        /// </summary>
        public int SelectionStart
        {
            get
            {
                int rv;
                intGet d = () => Box.SelectionStart;
                if (InvokeRequired)
                {
                    rv = (int)Invoke(d);
                }
                else rv = d();
                return rv;
            }
            set
            {
                intSet d = x => { Box.SelectionStart = x; };
                if (InvokeRequired)
                {
                    Invoke(d, new object[] { value });
                }
                else d(value);
            }
        }

        /// <summary>
        /// SelectionLength
        /// </summary>
        public int SelectionLength
        {
            get
            {
                int rv;
                intGet d = () => Box.SelectionLength;
                if (InvokeRequired)
                {
                    rv = (int)Invoke(d);
                }
                else rv = d();
                return rv;
            }
            set
            {
                intSet d = x => { Box.SelectionLength = x; };
                if (InvokeRequired)
                {
                    Invoke(d, new object[] { value });
                }
                else d(value);
            }
        }

        public event KeyEventHandler BoxKeydown;
        public MainBox()
        {
            InitializeComponent();
            Box.AccessibleDescription = AccessibleDescription;
            Box.AccessibleName = AccessibleName;
            Field[] f = { new Field("dummy", 0, "", "", null) };
            Populate(f);
        }

        private void MainBox_Resize(object sender, EventArgs e)
        {
            Box.Size = this.Size;
        }

        private void Box_KeyDown(object sender, KeyEventArgs e)
        {
            if (BoxKeydown != null)
            {
                BoxKeydown(this, e);
            }
        }
    }
}
