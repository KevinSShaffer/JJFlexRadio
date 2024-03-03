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
    public partial class InfoBox : UserControl
    {
        public string Header
        {
            get { return BoxLabel.Text; }
            set
            {
                BoxLabel.Text = value;
                // If no Tag, use the header.
                if ((Tag == null) || ((string)Tag == "")) Tag = value;
            }
        }

        /// <summary>
        /// true if box is readOnly.
        /// </summary>
        public bool ReadOnly
        {
            get { return Box.ReadOnly; }
            set
            {
                Box.ReadOnly = value;
            }
        }

        public delegate string UpdateDisplayDel();
        public UpdateDisplayDel UpdateDisplayFunction { get; set; }
        public delegate void UpdateRigDel(string txt);
        public UpdateRigDel UpdateRigFunction { get; set; }

        private string oldVal = null;
        delegate void d1();
        /// <summary>
        /// Called to update the data from the rig.
        /// </summary>
        public void UpdateDisplay(bool forceFlag)
        {
            if (UpdateDisplayFunction == null) return;
            string newVal = UpdateDisplayFunction();
            if (newVal == null) return;
            if (forceFlag || (oldVal == null) || (newVal != oldVal))
            {
                Tracing.TraceLine("updateDisplay update:" + Box.Parent.Name +
                    " " + newVal +
                    ((oldVal != null) ? " " + oldVal : ""), TraceLevel.Verbose);
                d1 d = () =>
                {
                    Box.Text = newVal;
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

        public override string Text
        {
            get
            {
                return Box.Text;
            }
            set
            {
                Box.Text = value;
            }
        }

        public event KeyEventHandler BoxKeydown;

        public InfoBox()
        {
            InitializeComponent();
        }

        private void Box_Enter(object sender, EventArgs e)
        {
            BringToFront();
        }

        //private bool userEntry = false;
        private void Box_KeyDown(object sender, KeyEventArgs e)
        {
            if (BoxKeydown != null) BoxKeydown(this, e);
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
