using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RadioBoxes
{
    public partial class ChekBox : UserControl
    {
        private CheckBox[] CBs;
        private Label CurrentValueBox;
        private string[] bitNames;
        private int[] bitVals;
        private int nBits;
        private int flags = 0;
        public void Setup(Type T, object o)
        {
            bitNames = T.GetEnumNames();
            bitVals = (int[])T.GetEnumValues();
            nBits = bitVals.Length;
            int wt = Box.Width - 2 * Box.Padding.All;
            int ht = 20;
            SuspendLayout();
            Size = Box.Size = SmallSize;
            // Setup the current value box
            CurrentValueBox = new Label();
            CurrentValueBox.Size = new Size(wt, CurrentValueBox.Height);
            CurrentValueBox.Location = new Point(0, ht);
            Box.Controls.Add(CurrentValueBox);
            // Setup the check boxes.
            CBs = new CheckBox[nBits];
            int i;
            for (i = 0; i < nBits; i++)
            {
                flags |= bitVals[i];
                CBs[i] = new CheckBox();
                CBs[i].Text = bitNames[i];
                CBs[i].Location = new Point(0, ht);
                ht += CBs[i].Height + 2;
                CBs[i].Size = new Size(CBs[i].Height, wt);
                CBs[i].TabStop = false;
                CBs[i].Enabled = false;
                CBs[i].Visible = false;
                Box.Controls.Add(CBs[i]);
            }
            // for testing
            int msk = flags;
            int bts = (int)o;
            bool sep = false;
            CurrentValueBox.Text = "";
            // Check items.
            for (i = 0; msk != 0; bts >>= 1, msk >>= 1, i++)
            {
                if ((bts & 0x1) != 0)
                {
                    CBs[i].Checked = true;
                    if (sep) CurrentValueBox.Text += "+";
                    else sep = true;
                    CurrentValueBox.Text += CBs[i].Text;
                }
                else CBs[i].Checked = false;
            }
            ResumeLayout();
        }

        /// <summary>
        /// group box heading
        /// </summary>
        public string Heading
        {
            get { return Box.Text; }
            set { Box.Text = value; }
        }
        /// <summary>
        /// Size when doesn't have the focus
        /// </summary>
        public Size SmallSize { get; set; }
        /// <summary>
        /// Size when has the focus.
        /// </summary>
        public Size ExpandedSize { get; set; }

        public ChekBox()
        {
            InitializeComponent();
        }

        private void ChekBox_Load(object sender, EventArgs e)
        {
            Size = Box.Size = SmallSize;
            Box.AccessibleName = AccessibleName;
        }

        private void Box_Enter(object sender, EventArgs e)
        {
            entering(true);
        }

        private void Box_Leave(object sender, EventArgs e)
        {
            entering(false);
        }

        private void entering(bool ent)
        {
            SuspendLayout();
            //CurrentValueBox.Enabled = !ent;
            CurrentValueBox.Visible = !ent;
            for (int i = 0; i < nBits; i++)
            {
                CBs[i].Enabled = ent;
                CBs[i].Visible = ent;
            }
            Size = Box.Size = (ent) ? ExpandedSize : SmallSize;
            ResumeLayout();
        }
#if zero

        public delegate object UpdateDisplayDel();
        public UpdateDisplayDel UpdateDisplayFunction { get; set; }
        public delegate void UpdateRigDel(object o);
        public UpdateRigDel UpdateRigFunction { get; set; }

        private object oldVal;
        delegate void d1();
        /// <summary>
        /// Called to update the data from the rig.
        /// The UpdateDisplayFunction must be setup.
        /// </summary>
        public void UpdateDisplay()
        {
            object newVal = UpdateDisplayFunction();
            if (newVal != oldVal)
            {
                d1 d = () =>
                    {
                        if (Focus)
                        {
                            Int32 i = (Int32)newVal;
                            for (int j=0; j<32; j++)
                            {
                                if ((j&0x1) != 0) 
                            }
                        }
                        else
                        {
                        }
                    };
                if (InvokeRequired) Invoke(d);
                else d();
                oldVal = newVal;
            }
        }

        /// <summary>
        /// Size when doesn't have the focus.
        /// </summary>
        public Size SmallSize { get; set; }
        /// <summary>
        /// Size when it gets the focus.
        /// </summary>
        public Size ExpandedSize { get; set; }
        public ComboBoxStyle BoxStyle
        {
            get { return Box.DropDownStyle; }
            set { Box.DropDownStyle = value; }
        }

        public Combo()
        {
            InitializeComponent();
        }

        private void Combo_SizeChanged(object sender, EventArgs e)
        {
            try { 
                //Box.Size = new Size(Size.Width, Size.Height - 15);
                Box.SuspendLayout();
                Box.Height = Height - 15;
                Box.Width = Width;
                Box.ResumeLayout();
            }
            catch { Box.Visible = false; }
        }

        private void Box_Enter(object sender, EventArgs e)
        {
            Box.SelectedIndexChanged += new EventHandler(Box_SelectedIndexChanged);
            Size = ExpandedSize;
            BringToFront();
        }

        private void Box_Leave(object sender, EventArgs e)
        {
            Box.SelectedIndexChanged -= Box_SelectedIndexChanged;
            Size = SmallSize;
        }

        private bool userEntry = false;
        private void Box_KeyDown(object sender, KeyEventArgs e)
        {
            userEntry = true;
        }

        private void Box_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Box.SelectedIndex != -1)
            {
                // Only update the rig if change was from the user.
                if (userEntry)
                {
                    userEntry = false;
                    UpdateRigFunction(Box.SelectedValue);
                }
            }
        }

        private void Combo_Load(object sender, EventArgs e)
        {
            // Transfer some properties to the real ComboBox.
            Box.AccessibleRole = AccessibleRole;
            Box.AccessibleName = AccessibleName;
        }
#endif
    }
}
