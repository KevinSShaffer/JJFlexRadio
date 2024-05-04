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

namespace Radios
{
    /// <summary>
    /// Show current memory's name or number, names first.
    /// Allow memory to be set from the VFO(s).
    /// Currently selected memory is moved to VFO(s) upon enter.
    /// </summary>
    public partial class ic9100memories : Form
    {
        private const string change = "Change";
        private const string add = "Add";
        private static string[] rangeText =
        {   "hf",
            "2m",
            "440",
            "1296"
        };

        private bool wasActive;
        private Icom9100 rig;
        private bool allMemories;

        private class memoryElement
        {
            public int ID;
            private Icom9100 rig;
            public AllRadios.MemoryData Mem
            {
                get { return rig.Memories.mems[ID];}
            }
            public int Number
            {
                get { return Mem.Number; }
            }
            public string Display
            {
                get { return Mem.DisplayName; }
            }
            public memoryElement(Icom9100 r, int i)
            {
                ID = i;
                rig = r;
            }
        }
        private ArrayList displayedMemories;
        private AllRadios.MemoryData selectedItem
        {
            get { return (AllRadios.MemoryData)((MemoriesListbox.SelectedIndex == -1) ? null : MemoriesListbox.SelectedValue); }
        }

        private int[] oldMemNumbers;
        private int memNumber
        {
            get { return oldMemNumbers[(int)rig.IC9100MemoryRange]; }
            set
            {
                oldMemNumbers[(int)rig.IC9100MemoryRange] = value;
            }
        }

        public ic9100memories(Icom9100 r)
        {
            InitializeComponent();
            Tracing.TraceLine("ic9100memories", TraceLevel.Info);
            rig = r;
        }

        private bool wasSetup = false;
        private void setup()
        {
            oldMemNumbers = new int[rig.IC9100Memories.Length];
            DisplayModeButton.Text = TS2000Memories.IncludeEmpty;
            allMemories = false;

            // Setup the band box.
            for (int i=0; i < rig.IC9100Memories.Length; i++)
            {
                BandBox.Items.Add(rangeText[i]);
            }
        }

        private void ic9100memories_Load(object sender, EventArgs e)
        {
            Tracing.TraceLine("ic9100memories_Load", TraceLevel.Info);
            if (!rig.MemoriesLoaded)
            {
                MessageBox.Show(TS2000Memories.memoriesNotLoaded, TS2000Memories.MessageTitle);
                DialogResult = DialogResult.Abort;
                return;
            }
            DialogResult = DialogResult.None;
            wasActive = false;

            if (!wasSetup)
            {
                setup();
                wasSetup = true;
            }

            // Set the band.
            runBandBoxSelect = false;
            BandBox.SelectedIndex = (int)rig.IC9100MemoryRange;
            runBandBoxSelect = true;

            // Set memory number.
            memNumber = rig.CurrentMemoryNumber;

            showMemories();
        }

        private void showMemories()
        {
            runSelect = false; // Don't run the SelectedIndex_changed code.
            MemoriesListbox.DataSource = null;
            displayedMemories = new ArrayList();
            int memid = -1;
            for (int i = 0; i < Icom.TotalMemories; i++)
            {
                if (allMemories || rig.Memories.mems[i].Present)
                {
                    displayedMemories.Add(new memoryElement(rig, i));
                    if (rig.Memories.mems[i].Number == memNumber) memid = displayedMemories.Count - 1;
                }
            }

            MemoriesListbox.DisplayMember = "Display";
            MemoriesListbox.ValueMember = "Mem";
            MemoriesListbox.DataSource = displayedMemories;

            // Use the old index if ok.
            MemoriesListbox.SelectedIndex = -1;
            runSelect = true; // run it now.
            if ((memid >= 0) & (memid < displayedMemories.Count))
            {
                MemoriesListbox.SelectedIndex = memid;
            }
            else if (displayedMemories.Count > 0) MemoriesListbox.SelectedIndex = 0;
            else MemoriesListbox.SelectedIndex = -1;
        }

        private bool runBandBoxSelect;
        private void BandBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!runBandBoxSelect || (BandBox.SelectedIndex == -1)) return;
            Tracing.TraceLine("BandBox index:" + BandBox.SelectedIndex.ToString(), TraceLevel.Info);

            rig.Memories = rig.IC9100Memories[BandBox.SelectedIndex];
            rig.CurrentMemoryChannel = rig.IC9100MemoryChannels[BandBox.SelectedIndex];
            showMemories();
        }

        private bool runSelect;
        private void MemoriesListbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!runSelect | (selectedItem == null)) return;
            memNumber = selectedItem.Number;
            bool userMem = (selectedItem.Type == AllRadios.MemoryTypes.Normal);
            NameLabel.Enabled = userMem;
            NameLabel.Visible = userMem;
            NameBox.Enabled = userMem;
            NameBox.Visible = userMem;
            DeleteButton.Enabled = userMem;
            DeleteButton.Visible = userMem;
            if (selectedItem.Present)
            {
                AddChangeButton.Text = change;
                if (NameBox.Enabled) NameBox.Text = selectedItem.Name;
            }
            else
            {
                // empty
                AddChangeButton.Text = add;
                if (NameBox.Enabled) NameBox.Text = "";
            }
        }

        private void DisplayModeButton_Click(object sender, EventArgs e)
        {
            Tracing.TraceLine("DisplayModeButton_Click:" + allMemories.ToString(), TraceLevel.Info);
            allMemories = !allMemories;
            DisplayModeButton.Text = (allMemories) ? TS2000Memories.OnlyUsed : TS2000Memories.IncludeEmpty;
            DialogResult = DialogResult.None;
            showMemories();
            MemoriesListbox.Focus();
        }

        private void MemoriesListbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (selectedItem == null) return;
            DialogResult = DialogResult.None;
            if (e.KeyChar == '\r')
            {
                Tracing.TraceLine("go to memory:" + selectedItem.Number.ToString(), TraceLevel.Info);
                if (rig.MemToVFO(selectedItem)) DialogResult=DialogResult.OK;
            }
        }

        private void AddChangeButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.None;
            if (selectedItem == null) return;
            Tracing.TraceLine("writing memory:" + selectedItem.Number.ToString(), TraceLevel.Info);
            selectedItem.Name = NameBox.Text.Trim();
            rig.VFOToMem(selectedItem);
            showMemories();
            MemoriesListbox.Focus();
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (selectedItem == null) return;
            DialogResult = DialogResult.None;
            Tracing.TraceLine("deleting memory:" + selectedItem.Number.ToString(), TraceLevel.Info);
            rig.MemDelete(selectedItem);
            showMemories();
            MemoriesListbox.Focus();
        }

        private void ic9100memories_Activated(object sender, EventArgs e)
        {
            if (!wasActive)
            {
                MemoriesListbox.Focus();
                wasActive = true;
            }
        }
    }
}
