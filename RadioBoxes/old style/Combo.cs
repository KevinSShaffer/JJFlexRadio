using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RadioBoxes
{
    public partial class Combo : UserControl
    {
        public string Header
        {
            get { return Heading.Text; }
            set { Heading.Text = value; }
        }

        private ArrayList lst;
        /// <summary>
        /// ArrayList of items.
        /// Each item must have a Display and RigItem property for the
        /// DisplayMember and ValueMember DataSource items.
        /// </summary>
        public ArrayList TheList
        {
            get { return lst; }
            set
            {
                lst = value;
                Box.DataSource = lst;
                Box.DisplayMember = "Display";
                Box.ValueMember = "RigItem";
            }
        }

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
                    if (!ContainsFocus) Box.Text = newVal.ToString();
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

        private void Box_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Box.SelectedIndex != -1)
            {
                UpdateRigFunction(Box.SelectedValue);
            }
        }

        private void Combo_Load(object sender, EventArgs e)
        {
            // Transfer some properties to the real ComboBox.
            Box.AccessibleRole = AccessibleRole;
            //AccessibleRole = AccessibleRole.Default;
            Box.AccessibleName = AccessibleName;
            //AccessibleName = "";
        }
    }
}
