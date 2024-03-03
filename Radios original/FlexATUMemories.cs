//#define DebugUsingStatus
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
using Flex.Smoothlake.FlexLib;
using JJTrace;
using RadioBoxes;

namespace Radios
{
    public partial class FlexATUMemories : Form
    {
        private FlexBase rig;
        private Radio theRadio;
        private ArrayList enableList;

        public FlexATUMemories(FlexBase r)
        {
            InitializeComponent();

            rig = r;
            theRadio = rig.theRadio;

            // Enable memories
            enableList = new ArrayList();
            enableList.Add(new Flex6300Filters.offOnElement(FlexBase.OffOnValues.off));
            enableList.Add(new Flex6300Filters.offOnElement(FlexBase.OffOnValues.on));
            EnableControl.TheList = enableList;
            EnableControl.UpdateDisplayFunction =
                () =>
                {
                    return (FlexBase.OffOnValues)((theRadio.ATUMemoriesEnabled)? FlexBase.OffOnValues.on: FlexBase.OffOnValues.off);
                };
            EnableControl.UpdateRigFunction =
                (object v) =>
                {
                    theRadio.ATUMemoriesEnabled = ((FlexBase.OffOnValues)v == FlexBase.OffOnValues.on);
                };

#if !DebugUsingStatus
            textBox1.Enabled = false;
            textBox1.Visible = false;
#endif
        }

        private void FlexATUMemories_Load(object sender, EventArgs e)
        {
            EnableControl.UpdateDisplay(true);
            if (textBox1.Enabled)
            {
                textBox1.Text = theRadio.ATUUsingMemory.ToString();
            }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            theRadio.ATUClearMemories();
        }
    }
}
