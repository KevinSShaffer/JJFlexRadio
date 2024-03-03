using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using JJTrace;

namespace RadioBoxes
{
    public partial class Combo : UserControl
    {
        public string Header
        {
            get { return Heading.Text; }
            set
            {
                Heading.Text = value;
                // If no Tag, use the header.
                if ((Tag == null) || ((string)Tag == "")) Tag = value;
            }
        }

        /// <summary>
        /// true if box is readOnly.
        /// </summary>
        public bool ReadOnly { get; set; }

        private ArrayList lst;
        /// <summary>
        /// ArrayList of items.
        /// Each item must have a Display and RigItem property for the
        /// DisplayMember and ValueMember DataSource items.
        /// The Display member should just be the "ToString" property
        /// of the value's type.
        /// </summary>
        public ArrayList TheList
        {
            get { return lst; }
            set
            {
                lst = value;
                //Box.DataSource = null;
                //Box.Items.Clear();
                Box.DataSource = lst;
                Box.DisplayMember = "Display";
                Box.ValueMember = "RigItem";
            }
        }

        public delegate object UpdateDisplayDel();
        public UpdateDisplayDel UpdateDisplayFunction { get; set; }
        public delegate int BoxIndexDel(object o);
        public BoxIndexDel BoxIndexFunction;
        public delegate void UpdateRigDel(object o);
        public UpdateRigDel UpdateRigFunction { get; set; }
        public delegate void UpdateRigByIndexDel(int id);
        public UpdateRigByIndexDel UpdateRigByIndexFunction;

        private object oldVal;
        delegate void d1();
        /// <summary>
        /// Called to update the data from the rig.
        /// The UpdateDisplayFunction must be setup.
        /// </summary>
        public void UpdateDisplay(bool forceFlag)
        {
            object newVal = UpdateDisplayFunction();
            if (newVal == null) return;
            if (forceFlag || (oldVal == null) || !newVal.Equals(oldVal))
            {
                Tracing.TraceLine("updateDisplay update:" + Box.Parent.Name + " " + newVal.ToString() + 
                    ((oldVal != null)? " " + oldVal.ToString(): ""), TraceLevel.Verbose);
                d1 d = () =>
                {
                    if (BoxIndexFunction == null) Box.Text = newVal.ToString();
                    else Box.SelectedIndex = BoxIndexFunction(newVal);
                };
                if (InvokeRequired) Invoke(d);
                else d();
                oldVal = newVal;
            }
        }
        public void UpdateDisplay()
        {
            UpdateDisplay(false);
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

        public event KeyEventHandler BoxKeydown;
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
            Size = ExpandedSize;
            BringToFront();
        }

        private void Box_Leave(object sender, EventArgs e)
        {
            Size = SmallSize;
        }

        private bool userEntry = false;
        private int savedIndex;
        private void Box_KeyDown(object sender, KeyEventArgs e)
        {
            if (!Box.Focused) return;
            bool altCtrlShift = (e.Alt || e.Control || e.Shift);
            bool ctlOnly = ((e.KeyCode == Keys.ControlKey) || (e.KeyCode == Keys.ShiftKey) || (e.KeyCode == Keys.Menu));
            // Save the item if only a control key.
            if (ctlOnly) savedIndex = Box.SelectedIndex;
            // See if selection key or key to pass on.
            if (!ReadOnly && 
                !altCtrlShift &&
                ((e.KeyData >= Keys.A) && (e.KeyData <= Keys.Z)  ||
                 (e.KeyData >= Keys.D0) && (e.KeyData <= Keys.D9) ||
                 arrowKey(e.KeyData)))
            {
                // alphabetic, numeric or arrow key with no control, alt, or shift, must be list selection.
                userEntry = true;
            }
            else
            {
                // Give the caller a chance to handle this one.
                if (BoxKeydown != null) BoxKeydown(this, e);
                // Restore the item if a control sequence.
                if (altCtrlShift && !ctlOnly && (savedIndex != -1)) Box.SelectedIndex = savedIndex;
            }
        }
        private bool arrowKey(Keys k)
        {
            return ((k == Keys.Up) || (k == Keys.Down) ||
                (k == Keys.Left) || (k == Keys.Right) ||
                (k == Keys.Home) || (k == Keys.End) ||
                (k == Keys.PageUp) || (k == Keys.PageDown));
        }

        /// <summary>
        /// Set the box's selectedIndex.
        /// Force a SelectedIndexChange even if the indecies are already equal.
        /// </summary>
        /// <param name="id">item's index</param>
        public void SetSelectedIndex(int id)
        {
            userEntry = true;
            // Force the change even if equal.
            if (Box.SelectedIndex == id) Box.SelectedIndex = -1;
            Box.SelectedIndex = id;
        }

        private void Box_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Box.SelectedIndex != -1)
            {
                // Only update the rig if change was from the user.
                if (userEntry)
                {
                    userEntry = false;
                    if (UpdateRigFunction != null)
                    {
                        UpdateRigFunction(Box.SelectedValue);
                    }
                    else if (UpdateRigByIndexFunction != null)
                    {
                        UpdateRigByIndexFunction(Box.SelectedIndex);
                    }
                }
            }
        }

        private void Combo_Load(object sender, EventArgs e)
        {
            // Transfer some properties to the real ComboBox.
            Box.AccessibleRole = AccessibleRole;
            //Box.SelectedIndexChanged += new EventHandler(Box_SelectedIndexChanged);
        }

        /// <summary>
        /// Used to clear any old display.
        /// </summary>
        public void Clear()
        {
            oldVal = null;
        }    
    }
}
