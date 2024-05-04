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
            /// Provide field parameters
            /// </summary>
            /// <param name="ln">field length</param>
            /// <param name="ld">left-side delimiter</param>
            /// <param name="rd">right-side delimiter</param>
            public Field(int ln, string ld, string rd)
            {
                setup(ln, ld, rd, null);
            }
            /// <summary>
            /// provide field parameters
            /// </summary>
            /// <param name="ln">field length</param>
            /// <param name="ld">left-side delimiter</param>
            /// <param name="rd">right-side delimiter</param>
            /// <param name="h">field handler delegate</param>
            public Field(int ln, string ld, string rd, HandlerDel h)
            {
                setup(ln, ld, rd, h);
            }
            private void setup(int ln, string ld, string rd, HandlerDel h)
            {
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
        /// Populate the box with fields
        /// </summary>
        /// <param name="flds">array of Field structures</param>
        public void Populate(Field[] flds)
        {
            fields = new Field[flds.Length];
            int lastPos = 0;
            for (int i=0; i<flds.Length; i++)
            {
                fields[i] = new Field(flds[i].Length,flds[i].LeftDelim,flds[i].RightDelim,flds[i].Handler);
                fields[i].pos = lastPos + flds[i].LeftDelim.Length;
                lastPos += flds[i].LeftDelim.Length + flds[i].Length + flds[i].RightDelim.Length;
            }
            Display(); // This will also allow the cursor position to be set.
            Changed = false;
        }

        /// <summary>
        /// Read the field's value.
        /// </summary>
        /// <param name="id">field ID</param>
        /// <returns>the field's text</returns>
        public string Read(int id)
        {
            string rv;
            try { rv = fields[id].Text; }
            catch { rv = ""; }
            return rv;
        }

        /// <summary>
        /// Write the specified field.
        /// The text is right-justified.
        /// </summary>
        /// <param name="id">field's ID</param>
        /// <param name="txt">Text to write</param>
        public void Write(int id, string txt)
        {
            try
            {
                string newText;
                int ln = txt.Length;
                int padLen = fields[id].Length - ln;
                if (padLen > 0)
                    newText = blanks.Substring(0, padLen) + txt.Substring(0, ln);
                else if (padLen < 0)
                    newText = txt.Substring(-padLen);
                else newText = txt;
                if (newText != fields[id].Text)
                {
                    Changed = true;
                    fields[id].Text = newText;
                }        
            }
            catch { }
        }

        /// <summary>
        /// field's position
        /// </summary>
        /// <param name="id">field id</param>
        /// <returns>offset within the box</returns>
        public int Position(int id)
        {
            try { return fields[id].pos; }
            catch { return -1; }
        }

        /// <summary>
        /// field's length
        /// </summary>
        /// <param name="id">field id</param>
        /// <returns>length</returns>
        public int Length(int id)
        {
            try { return fields[id].Length; }
            catch { return 0; }
        }

        /// <summary>
        /// execute the field's function
        /// </summary>
        /// <param name="id">field id</param>
        /// <param name="o">function input</param>
        /// <returns>true if function is present</returns>
        public bool Function(int id, object o)
        {
            HandlerDel h;
            bool rv;
            try { h = fields[id].Handler; }
            catch { return false; }
            rv = (h != null);
            if (rv) h(o);
            return rv;
        }

        /// <summary>
        /// return true if field is empty
        /// </summary>
        /// <param name="id">of the field</param>
        /// <returns>true if empty</returns>
        public bool IsEmpty(int id)
        {
            return (Length(id) == 0);
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
            for (int i = 0; i < fields.Length; i++) Write(i, "");
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
            Field[] f = { new Field(0, "", "") };
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
