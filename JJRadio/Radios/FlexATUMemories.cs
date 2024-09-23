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
        private Flex rig;
        private Radio theRadio;
        private ArrayList enableList;

        public FlexATUMemories(Flex r)
        {
            InitializeComponent();

            rig = r;
            theRadio = rig.theRadio;

            // Enable memories
            enableList = new ArrayList();
            enableList.Add(new TS590Filters.offOnElement(AllRadios.OffOnValues.off));
            enableList.Add(new TS590Filters.offOnElement(AllRadios.OffOnValues.on));
            EnableControl.TheList = enableList;
            EnableControl.UpdateDisplayFunction =
                () =>
                {
                    return (AllRadios.OffOnValues)((theRadio.ATUMemoriesEnabled)?AllRadios.OffOnValues.on:AllRadios.OffOnValues.off);
                };
            EnableControl.UpdateRigFunction =
                (object v) =>
                {
                    theRadio.ATUMemoriesEnabled = ((AllRadios.OffOnValues)v == AllRadios.OffOnValues.on);
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
