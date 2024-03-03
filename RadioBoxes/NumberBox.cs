using System;
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
    public partial class NumberBox : UserControl
    {
        public string Header
        {
            get { return Heading.Text; }
            set
            {
                Heading.Text = value;
                // If no Tag, use the header.
                if (Tag == null) Tag = Header;
            }
        }

        /// <summary>
        /// low limit
        /// </summary>
        public int LowValue { get; set; }
        /// <summary>
        /// high limit, set to less than LowLimit if unlimited.
        /// </summary>
        public int HighValue { get; set; }
        /// <summary>
        /// Increment/decrement value for arrow keys
        /// </summary>
        public int Increment { get; set; }
        public bool ReadOnly { get; set; }

        public delegate int UpdateDisplayDel();
        public UpdateDisplayDel UpdateDisplayFunction { get; set; }
        public delegate void UpdateRigDel(int val);
        public UpdateRigDel UpdateRigFunction { get; set; }

        private int oldVal;
        delegate void d1();
        /// <summary>
        /// Called to update the data from the rig.
        /// The UpdateDisplayFunction must be setup.
        /// </summary>
        public void UpdateDisplay(bool forceFlag)
        {
            // quit if user is entering data.
            if (directEntry) return;
            int newVal = UpdateDisplayFunction();
            if (forceFlag || (oldVal != newVal))
            {
                Tracing.TraceLine("updateDisplay update:" + Box.Parent.Name + " " + newVal.ToString() +
                    " " + oldVal.ToString(), TraceLevel.Verbose);
                d1 d = () =>
                {
                    Box.Text = newVal.ToString();
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

        public event KeyEventHandler BoxKeydown;

        public NumberBox()
        {
            InitializeComponent();

            // setup the size.
            NumberBox_SizeChanged(null, null);
            // Set oldval outside the range so first value is displayed.
            oldVal = LowValue - 1;
        }

        private void NumberBox_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                Heading.Width = Box.Width = Width;
                Box.Height = Height - 15;
            }
            catch //(Exception ex)
            {
            }
        }

        private bool directEntry;
        private void Box_KeyDown(object sender, KeyEventArgs e)
        {
#if zero
            Tracing.TraceLine(e.KeyData.ToString() + " " + ((int)e.KeyData).ToString());
#else
            int testVal = ((int)Keys.Alt | (int)Keys.Control | (int)Keys.Shift);
            // See if key to pass on.
            if ((((int)e.KeyData & testVal) != 0) ||
                ((e.KeyData >= Keys.F1) && (e.KeyData <= Keys.F24)) ||
                (e.KeyData == Keys.Escape))
            {
                // Give the caller a chance to handle this one.
                if (BoxKeydown != null) BoxKeydown(this, e);
                return;
            }
            if (!ReadOnly)
            {
                int value;
                if (!System.Int32.TryParse(Box.Text, out value)) return;
                switch (e.KeyData)
                {
                    case Keys.Up:
                        value += Increment;
                        if ((HighValue > LowValue) && (value > HighValue)) value = HighValue;
                        updateBoxAndRig(value, e);
                        break;
                    case Keys.Down:
                        value -= Increment;
                        if (value < LowValue) value = LowValue;
                        updateBoxAndRig(value, e);
                        break;
                    case Keys.Home:
                        value = LowValue;
                        updateBoxAndRig(value, e);
                        break;
                    case Keys.End:
                        if (HighValue > LowValue) value = HighValue;
                        updateBoxAndRig(value, e);
                        break;
                    case Keys.Return:
                        updateBoxAndRig(value, e);
                        directEntry = false;
                        break;
                    default:
                        directEntry = true;
                        break;
                }
            }
            else e.SuppressKeyPress = true;
#endif
        }
        private void updateBoxAndRig(int value, KeyEventArgs e)
        {
            Box.Text = value.ToString();
            Box.SelectAll();
            UpdateRigFunction(value);
            e.SuppressKeyPress = true;
        }

        private void Box_Enter(object sender, EventArgs e)
        {
            Box.SelectAll();
        }

        private void Box_Leave(object sender, EventArgs e)
        {
            directEntry = false;
        }
    }
}
