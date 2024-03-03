using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Radios
{
    public partial class PanListForm : Form
    {
        private Collection<PanRanges.PanRange> theRanges;
        public PanRanges.PanRange SelectedRange = null;

        public PanListForm(Collection<PanRanges.PanRange> ranges)
        {
            InitializeComponent();

            theRanges = ranges;
            foreach (PanRanges.PanRange r in ranges)
            {
                RangeList.Items.Add(r.ToString());
            }
            if (ranges.Count > 0) RangeList.SelectedIndex = 0;
        }

        private void PanListForm_Load(object sender, EventArgs e)
        {
        }

        private void RangeList_KeyPress(object sender, KeyPressEventArgs e)
        {
            DialogResult = DialogResult.None;
            if ((e.KeyChar == '\r') && (RangeList.SelectedIndex != -1))
            {
                SelectedRange = theRanges[RangeList.SelectedIndex];
                DialogResult = DialogResult.OK;
            }
        }

        private void CnclButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
