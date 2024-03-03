using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using Flex.Smoothlake.FlexLib;

namespace Radios
{
    public partial class FlexInfo : Form
    {
        private bool wasActive;
        private FlexBase rig;
        private Radio theRadio
        {
            get { return (rig != null) ? rig.theRadio : null; }
        }

        private class displayElement
        {
            private ScreensaverMode val;
            public string Display { get { return val.ToString(); } }
            public ScreensaverMode RigItem { get { return val; } }
            public displayElement(int v)
            {
                val = (ScreensaverMode)v;
            }
        }
        private ArrayList displayList;

        public FlexInfo(FlexBase r)
        {
            InitializeComponent();

            rig = r;

            // What to display on front panel.'
            displayList = new ArrayList();
            foreach (int v in Enum.GetValues(typeof(ScreensaverMode)))
            {
                displayList.Add(new displayElement(v));
            }
            DisplayControl.TheList = displayList;
            DisplayControl.UpdateDisplayFunction =
                () => { return theRadio.Screensaver; };
            DisplayControl.UpdateRigFunction =
                (object v) => { theRadio.Screensaver = (ScreensaverMode)v; };
        }

        private void FlexInfo_Load(object sender, EventArgs e)
        {
            if (rig == null)
            {
                DialogResult = DialogResult.Abort;
                return;
            }

            wasActive = false;

            showValues();
        }

        private void showValues()
        {
            ModelBox.Text = theRadio.Model;
            VersionBox.Text =
                ((theRadio.Version & 0x00ff000000000000) / 0x0001000000000000).ToString() + '.' +
                ((theRadio.Version & 0x0000ff0000000000) / 0x0000010000000000).ToString() + '.' +
                ((theRadio.Version & 0x000000ff00000000) / 0x0000000100000000).ToString();
                //((theRadio.Version & 0x00000000ffffffff)).ToString();
            SerialBox.Text = theRadio.Serial;
            CallBox.Text = theRadio.Callsign;
            NameBox.Text = theRadio.Nickname;
            IPBox.Text = theRadio.IP.ToString();
            DisplayControl.UpdateDisplay();
        }

        private void FlexInfo_Activated(object sender, EventArgs e)
        {
            if (!wasActive)
            {
                wasActive = true;
                ModelBox.Focus();
            }
        }

        private void DoneButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void setCall(object sender, EventArgs e)
        {
            if (CallBox.Text != theRadio.Callsign) theRadio.Callsign = CallBox.Text;
        }

        private void CallBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r') setCall(CallBox, new EventArgs());
        }

        private void setName(object sender, EventArgs e)
        {
            if (NameBox.Text != theRadio.Nickname) theRadio.Nickname = NameBox.Text;
        }

        private void NameBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r') setName(CallBox, new EventArgs());
        }
    }
}
