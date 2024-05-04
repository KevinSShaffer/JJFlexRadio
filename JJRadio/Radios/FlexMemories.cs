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
using Flex.Smoothlake.FlexLib;
using JJTrace;
using RadioBoxes;

namespace Radios
{
    public partial class FlexMemories : Form
    {
        private const string warning = "Warning";
        private const string dupName =
            "Warning:  Duplicate name\n\r" +
            "Do you want to change it?";
        private bool wasSetup;
        private bool wasActive;
        private Flex6300 rig;
        private Radio theRadio { get { return rig.theRadio; } }

        private delegate Memory getMemDel(ListBox m);
        private static getMemDel getMem = (ListBox m) =>
        { return (Memory)m.SelectedValue; };
        private Memory selectedMemory
        {
            get
            {
                if (MemoryList.InvokeRequired)
                {
                    return (Memory)MemoryList.Invoke(getMem, new object[] { MemoryList });
                }
                else return getMem(MemoryList);
            }
        }
        private Collection<Combo> combos;
        private Collection<NumberBox> numberBoxes;
        private Collection<TextBox> textBoxes;
        // tag field contains this function.
        private delegate void getBoxDel();

        private class modeElement
        {
            private string val;
            public string Display
            {
                get { return val; }
            }
            public string RigItem
            {
                get { return val; }
            }
            public modeElement(string v)
            {
                val = v;
            }
        }
        private ArrayList modeList;
        private ArrayList toneModeList, toneFrequencyList;
        private ArrayList SquelchList, offsetDirectionList;

        public FlexMemories(Flex6300 r)
        {
            InitializeComponent();  

            rig = r;
            combos = new Collection<Combo>();
            numberBoxes = new Collection<NumberBox>();
            textBoxes = new Collection<TextBox>();

            // Mode box.
            modeList = new ArrayList();
            foreach (string m in Flex.modeDictionary.Keys)
            {
                if (m != "none") modeList.Add(new modeElement(m.ToLower()));
            }
            ModeControl.TheList = modeList;
            ModeControl.UpdateDisplayFunction =
                () =>
                {
                    modeChange.enableDisable(rig.getMode(selectedMemory.Mode));
                    return selectedMemory.Mode.ToLower();
                };
            ModeControl.UpdateRigFunction =
                (object v) =>
                {
                    string m = ((string)v).ToUpper();
                    modeChange.enableDisable(rig.getMode(m));
                    selectedMemory.Mode = m;
                };
            combos.Add(ModeControl);

            // Filters
            FilterLowControl.LowValue = Flex6300Filters.filterLowMinimum;
            FilterLowControl.HighValue = Flex6300Filters.filterHighMinimum;
            FilterLowControl.Increment = Flex6300Filters.filterLowIncrement;
            FilterLowControl.UpdateDisplayFunction =
                () => { return selectedMemory.RXFilterLow; };
            FilterLowControl.UpdateRigFunction =
                (int v) => { selectedMemory.RXFilterLow = v; };
            numberBoxes.Add(FilterLowControl);

            FilterHighControl.LowValue = Flex6300Filters.filterLowMinimum;
            FilterHighControl.HighValue = Flex6300Filters.filterHighMinimum;
            FilterHighControl.Increment = Flex6300Filters.filterLowIncrement;
            FilterHighControl.UpdateDisplayFunction =
                () => { return selectedMemory.RXFilterHigh; };
            FilterHighControl.UpdateRigFunction =
                (int v) => { selectedMemory.RXFilterHigh= v; };
            numberBoxes.Add(FilterHighControl);

            // Power
            PowerControl.LowValue = Flex6300.XmitPowerMin;
            PowerControl.HighValue = Flex6300.XmitPowerMax;
            PowerControl.Increment = Flex6300.XmitPowerIncrement;
            PowerControl.UpdateDisplayFunction =
                () => { return selectedMemory.RFPower; };
            PowerControl.UpdateRigFunction =
                (int v) => { selectedMemory.RFPower = v; };
            numberBoxes.Add(PowerControl);

            // FM tone or CTCSS mode.
            toneModeList = new ArrayList();
            foreach (AllRadios.ToneCTCSSValue t in rig.FMToneModes)
            {
                toneModeList.Add(new TS2000Filters.toneCTCSSElement(t));
            }
            ToneModeControl.TheList = toneModeList;
            ToneModeControl.UpdateDisplayFunction =
                () => { return rig.ToneModeToToneCTCSS(selectedMemory.ToneMode); };
            ToneModeControl.UpdateRigFunction =
                (object v) => { selectedMemory.ToneMode = rig.ToneCTCSSToToneMode((AllRadios.ToneCTCSSValue)v); };
            combos.Add(ToneModeControl);

            // FM tone frequency
            toneFrequencyList = new ArrayList();
            foreach (float f in rig.ToneFrequencyTable)
            {
                toneFrequencyList.Add(new TS2000Filters.toneCTCSSFreqElement(f));
            }
            ToneFrequencyControl.TheList = toneFrequencyList;
            ToneFrequencyControl.UpdateDisplayFunction =
                () => { return rig.ToneValueToFloat(selectedMemory.ToneValue); };
            ToneFrequencyControl.UpdateRigFunction =
                (object v) => { selectedMemory.ToneValue = rig.FloatToToneValue((float)v); };
            combos.Add(ToneFrequencyControl);

            // Squelch controls
            SquelchList = new ArrayList();
            SquelchList.Add(new TS590Filters.offOnElement(AllRadios.OffOnValues.off));
            SquelchList.Add(new TS590Filters.offOnElement(AllRadios.OffOnValues.on));
            SquelchControl.TheList = SquelchList;
            SquelchControl.UpdateDisplayFunction =
                () => { return (selectedMemory.SquelchOn)? AllRadios.OffOnValues.on:AllRadios.OffOnValues.off; };
            SquelchControl.UpdateRigFunction =
                (object v) =>
                { selectedMemory.SquelchOn = ((AllRadios.OffOnValues)v == AllRadios.OffOnValues.on) ? true : false; };
            combos.Add(SquelchControl);

            SquelchLevelControl.LowValue = Flex6300.SquelchLevelMin;
            SquelchLevelControl.HighValue = Flex6300.SquelchLevelMax;
            SquelchLevelControl.Increment = Flex6300.SquelchLevelIncrement;
            SquelchLevelControl.UpdateDisplayFunction =
                () => { return selectedMemory.SquelchLevel; };
            SquelchLevelControl.UpdateRigFunction =
                (int v) => { selectedMemory.SquelchLevel = v; };
            numberBoxes.Add(SquelchLevelControl);

            // Offset direction and offset
            offsetDirectionList = new ArrayList();
            offsetDirectionList.Add(new TS2000Memories.directionElement(AllRadios.OffsetDirections.off));
            offsetDirectionList.Add(new TS2000Memories.directionElement(AllRadios.OffsetDirections.minus));
            offsetDirectionList.Add(new TS2000Memories.directionElement(AllRadios.OffsetDirections.plus));
            OffsetDirectionControl.TheList = offsetDirectionList;
            OffsetDirectionControl.UpdateDisplayFunction =
                () => { return rig.FlexOffsetDirectionToOffsetDirection(selectedMemory.OffsetDirection); };
            OffsetDirectionControl.UpdateRigFunction =
                (object v) => { selectedMemory.OffsetDirection = rig.OffsetDirectionToFlexOffsetDirection((AllRadios.OffsetDirections)v); };
            combos.Add(OffsetDirectionControl);

            OffsetControl.LowValue = Flex.offsetMin;
            OffsetControl.HighValue = Flex.offsetMax;
            OffsetControl.Increment = Flex.offsetIncrement;
            OffsetControl.UpdateDisplayFunction =
                () => { return (int)(selectedMemory.RepeaterOffset * 1e3); };
            OffsetControl.UpdateRigFunction =
                (int v) => { selectedMemory.RepeaterOffset = (v / 1e3); };
            numberBoxes.Add(OffsetControl);

            // Text boxes
            NameBox.Tag = (getBoxDel)(() => { NameBox.Text = selectedMemory.Name; });
            textBoxes.Add(NameBox);
            OwnerBox.Tag = (getBoxDel)(() => { OwnerBox.Text = selectedMemory.Owner; });
            textBoxes.Add(OwnerBox);
            GroupBox.Tag = (getBoxDel)(() => { GroupBox.Text = selectedMemory.Group; });
            textBoxes.Add(GroupBox);

            modeChange = new Flex6300Filters.modeChangeClass(this, buildModeChange(), null);
        }

        private void FlexMemories_Load(object sender, EventArgs e)
        {
            if (theRadio == null)
            {
                DialogResult = DialogResult.Abort;
                return;
            }

            wasActive = false;
            DialogResult = DialogResult.None;

            // One-time setup
            if (!wasSetup)
            {
                wasSetup = true;
                theRadio.MemoryAdded += memoryAdded;
                theRadio.MemoryRemoved += memoryRemoved;
                refreshMemoryListFunc = refreshMemoryListProc;
            }

            refreshMemoryList(rig.CurrentMemoryChannel);
        }

        private void FlexMemories_Activated(object sender, EventArgs e)
        {
            if (!wasActive)
            {
                wasActive = true;
                MemoryList.Focus();
            }
        }

        private string listName(Memory m)
        {
            return (string.IsNullOrEmpty(m.Name)) ? m.Freq.ToString("F6") : m.Name;
        }

        private delegate void refreshMemoryListDel(List<Flex.memoryElement> sortedMemories, Memory newMem);
        private refreshMemoryListDel refreshMemoryListFunc;
        // (overloaded) May be called from an interrupt handler.
        private void refreshMemoryList(int memno)
        {
            if ((rig.Memories == null) || (rig.Memories.mems == null) || (rig.Memories.mems.Length <= memno)) return;
            refreshMemoryList((Memory)rig.Memories.mems[memno].ExternalMemory);
        }
        // (overloaded) May be called from an interrupt handler.
        private void refreshMemoryList(Memory memToLocate)
        {
            List<Flex.memoryElement> sortedMemories = rig.SortElements();
            if (MemoryList.InvokeRequired) MemoryList.Invoke(refreshMemoryListFunc, new object[] { sortedMemories, memToLocate });
            else refreshMemoryListFunc(sortedMemories, memToLocate);
        }
        private void refreshMemoryListProc(List<Flex.memoryElement> sortedMemories, Memory MemToLocate)
        {
            MemoryList.SuspendLayout();
            noSelectedAction = true;
            MemoryList.DataSource = null;
            MemoryList.DataSource = sortedMemories;
            MemoryList.DisplayMember = "Display";
            MemoryList.ValueMember = "Value";
            noSelectedAction = false;
            MemoryList.ResumeLayout();
            if (MemToLocate != null)
            {
                for (int id = 0; id < sortedMemories.Count; id++)
                {
                    if (sortedMemories[id].Value == MemToLocate)
                    {
                        MemoryList.SelectedIndex = id;
                        break;
                    }
                }
            }
            else MemoryList.SelectedIndex = -1;
        }

        private void showMemory(Memory m)
        {
            FreqBox.Text = rig.Callouts.FormatFreq(rig.LibFreqtoLong(m.Freq));
            foreach (Combo c in combos)
            {
                if (c.Enabled) c.UpdateDisplay(true);
            }

            foreach (NumberBox n in numberBoxes)
            {
                if (n.Enabled) n.UpdateDisplay(true);
            }

            foreach (TextBox tb in textBoxes)
            {
                ((getBoxDel)tb.Tag)();
            }
        }

        private bool addRemoveFlag;
        private void memoryRemoved(Memory mem)
        {
            Tracing.TraceLine("memoryRemoved:" + mem.ToString(), TraceLevel.Info);
            addRemoveFlag = true;
        }

        private Memory addedMem;
        private void memoryAdded(Memory mem)
        {
            Tracing.TraceLine("memoryAdded:" + mem.ToString(), TraceLevel.Info);
            addedMem = mem;
            addRemoveFlag = true;
        }

        private Flex6300Filters.modeChangeClass modeChange;

        private Dictionary<string, Flex6300Filters.modeChangeClass.controlsClass> buildModeChange()
        {
            Dictionary<string, Flex6300Filters.modeChangeClass.controlsClass> rv = new Dictionary<string, Flex6300Filters.modeChangeClass.controlsClass>();

            // setup the mode to combobox mapping.
            rv.Add("fm", new Flex6300Filters.modeChangeClass.controlsClass(
                new Control[] {
                    ToneModeControl, ToneFrequencyControl,
                    SquelchControl, SquelchLevelControl,
                    OffsetDirectionControl, OffsetControl,
                    }));
            rv.Add("digl", new Flex6300Filters.modeChangeClass.controlsClass(
                new Control[] {
                    }));
            rv.Add("digu", new Flex6300Filters.modeChangeClass.controlsClass(
                new Control[] {
                    }));
            rv.Add("nfm", new Flex6300Filters.modeChangeClass.controlsClass(
                new Control[] {
                    SquelchControl, SquelchLevelControl,
                    }));
            rv.Add("dfm", new Flex6300Filters.modeChangeClass.controlsClass(
                new Control[] {
                    SquelchControl, SquelchLevelControl,
                    }));

            return rv;
        }

        private bool noSelectedAction;
        private void MemoryList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (noSelectedAction) return;
            if (selectedMemory != null)
            {
                showMemory(selectedMemory);
            }
        }

        private void MemoryList_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (selectedMemory == null) return;
            if (e.KeyChar == '\r')
            {
                rig.MemoryMode = true;
                rig.CurrentMemoryChannel = MemoryList.SelectedIndex;
                rig.MemoryMode = false;
                e.Handled = true;
                DialogResult = DialogResult.OK;
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            addRemoveFlag = false;
            Memory m = new Memory(theRadio);
            rig.q.Enqueue((Flex.FunctionDel)(() => { m.RequestMemoryFromRadio(); }));
            AllRadios.await(() => { return addRemoveFlag; }, 1000);
            rig.RefreshMemories();
            refreshMemoryList(addedMem);
            MemoryList.Focus();
            DialogResult = DialogResult.None;
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.None;
            if (selectedMemory != null)
            {
                Tracing.TraceLine("DeleteButton_Click", TraceLevel.Info);
                addRemoveFlag = false;
                Memory mem = selectedMemory;
                rig.q.Enqueue((Flex.FunctionDel)(() => { mem.Remove(); }));
                AllRadios.await(() => { return addRemoveFlag; }, 1000);
                rig.RefreshMemories();
                refreshMemoryList(null);
            }
            MemoryList.Focus();
        }

        private void DoneButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void FreqBox_Leave(object sender, EventArgs e)
        {
            if (selectedMemory == null) return;
            // Set the memory's frequency.
            double freq = rig.LongFreqToLibFreq(rig.Callouts.FormatFreqForRadio(FreqBox.Text));
            if ((freq != 0) && (selectedMemory.Freq != freq))
            {
                selectedMemory.Freq = freq;
                refreshMemoryList(selectedMemory);
                rig.RefreshMemories();
            }
        }

        private void FreqBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                FreqBox_Leave(sender, new EventArgs());
                e.Handled = true;
            }
        }

        // Called on leave
        private void GroupBox_Change(object sender, EventArgs e)
        {
            if (selectedMemory != null)
            {
                if (selectedMemory.Group != GroupBox.Text)
                {
                    selectedMemory.Group = GroupBox.Text;
                    refreshMemoryList(selectedMemory);
                    rig.RefreshMemories();
                }
            }
        }

        private void GroupBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                GroupBox_Change(sender, new EventArgs());
                MemoryList.Focus();
            }
        }

        private void OwnerBox_Change(object sender, EventArgs e)
        {
            if (selectedMemory != null)
            {
                if (selectedMemory.Owner != OwnerBox.Text)
                {
                    selectedMemory.Owner = OwnerBox.Text;
                }
            }
        }

        private void OwnerBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r') OwnerBox_Change(sender, new EventArgs());
        }

        private void NameBox_Change(object sender, EventArgs e)
        {
            if (selectedMemory != null)
            {
                if (selectedMemory.Name != NameBox.Text)
                {
                    foreach (Memory m in theRadio.MemoryList)
                    {
                        if (m.Name == NameBox.Text)
                        {
                            if (MessageBox.Show(dupName, warning, MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                NameBox.Focus();
                                return;
                            }
                        }
                    }
                    selectedMemory.Name = NameBox.Text;
                    refreshMemoryList(selectedMemory);
                    rig.RefreshMemories();
                }
            }
        }

        private void NameBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                NameBox_Change(NameBox, new EventArgs());
                MemoryList.Focus();
            }
        }
    }
}
