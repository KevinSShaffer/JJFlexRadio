using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using JJTrace;
using RadioBoxes;
using Radios;

namespace Radios
{
    public partial class TS2000Memories : Form
    {
        internal const string MessageTitle = "Message";
        internal const string memoriesNotLoaded = "The memories are still being loaded.";
        const string notSelected = "No memory is selected.";
        const string notEmptyText = "Do you want to overwrite it?";
        const string notEmptyM1 = "Memory ";
        const string notEmptyM2 = " is not empty.";
        internal const string IncludeEmpty = "Include empty memories";
        internal const string OnlyUsed = "Only used memories";
        const string numericFrequency = "Specify a valid frequency (mmm.kkk)";

        private AllRadios rig;
        private bool wasSetup = false;
        private bool wasActive;

        private bool allMemories;
        private class memoryItem
        {
            private AllRadios.MemoryData mem;
            private AllRadios rig;
            public memoryItem(AllRadios r, AllRadios.MemoryData m)
            {
                rig = r;
                mem = m;
            }
            public string Display
            {
                get { return mem.DisplayName; }
            }
            public AllRadios.MemoryData Value
            {
                get { return mem; }
            }
        }
        private ArrayList theList;
        private AllRadios.MemoryData selectedMemory;

        private Collection<Control> combos;

        private class modeElement
        {
            private Radios.AllRadios.ModeValue val;
            public string Display
            {
                get { return val.ToString(); }
            }
            public Radios.AllRadios.ModeValue RigItem
            {
                get { return val; }
            }
            public modeElement(Radios.AllRadios.ModeValue v)
            {
                val = v;
            }
        }
        private ArrayList modeList, tModeList;

        private ArrayList toneCTCSSList;
        private ArrayList toneFreqList, CTCSSFreqList;

        internal class directionElement
        {
            private Radios.AllRadios.OffsetDirections val;
            public string Display
            {
                get { return val.ToString(); }
            }
            public Radios.AllRadios.OffsetDirections RigItem
            {
                get { return val; }
            }
            public directionElement(Radios.AllRadios.OffsetDirections v)
            {
                val = v;
            }
        }
        private ArrayList directionList;
        private static directionElement[] directionItems =
        {new directionElement(AllRadios.OffsetDirections.off),
         new directionElement(AllRadios.OffsetDirections.plus),
         new directionElement(AllRadios.OffsetDirections.minus),
         new directionElement(AllRadios.OffsetDirections.allTypes)
        };

        private ArrayList OffsetFreqList, StepSizeSSBList, StepSizeAMFMList, GroupList;

        // true/false lists
        private ArrayList splitList, reverseList;

        private ArrayList lockoutList;

        private void setupBoxes()
        {
            // Initial state is to only show used memories
            EmptyNonemptyButton.Text = IncludeEmpty;
            allMemories = false;

            // Setup the mode change stuff.
            modeChange = new modeChangeClass(this);

            theList = new ArrayList();
            for (int i = 0; i <= rig.NumberOfMemories - 1; i++)
            {
                AllRadios.MemoryData m = rig.Memories.mems[i];
                if (m.Present)
                {
                    theList.Add(new memoryItem(rig, m));
                }
            }
            combos = new Collection<Control>();

            modeList = new ArrayList();
            tModeList = new ArrayList();
            foreach (AllRadios.ModeValue m in Kenwood.myModeTable)
            {
                if (m.ToString() != "none")
                {
                    modeList.Add(new modeElement(m));
                    tModeList.Add(new modeElement(m));
                }
            }
            ModeControl.TheList = modeList;
            ModeControl.UpdateDisplayFunction =
                () =>
                {
                    modeChange.enableDisable(selectedMemory.Mode[0]);
                    return selectedMemory.Mode[0];
                };
            ModeControl.UpdateRigFunction =
                (object v) =>
                {
                    modeChange.enableDisable((Radios.AllRadios.ModeValue)v);
                    selectedMemory.Mode[0] = (Radios.AllRadios.ModeValue)v;
                };
            combos.Add(ModeControl);

            TModeControl.TheList = tModeList;
            TModeControl.UpdateDisplayFunction = () => selectedMemory.Mode[1];
            TModeControl.UpdateRigFunction =
                (object v) => selectedMemory.Mode[1] = (Radios.AllRadios.ModeValue)v;
            combos.Add(TModeControl);

            splitList = new ArrayList();
            splitList.Add(new TS2000Filters.trueFalseElement(false));
            splitList.Add(new TS2000Filters.trueFalseElement(true));
            SplitControl.TheList = splitList;
            SplitControl.UpdateDisplayFunction = () => selectedMemory.Split;
            SplitControl.UpdateRigFunction =
                (object v) => this.splitChanged((bool)v);
            combos.Add(SplitControl);

            reverseList = new ArrayList();
            reverseList.Add(new TS2000Filters.trueFalseElement(false));
            reverseList.Add(new TS2000Filters.trueFalseElement(true));
            ReverseControl.TheList = reverseList;
            ReverseControl.UpdateDisplayFunction = () => selectedMemory.Reverse;
            ReverseControl.UpdateRigFunction =
                (object v) => selectedMemory.Reverse = (bool)v;
            combos.Add(ReverseControl);

            directionList = new ArrayList();
            foreach (directionElement e in directionItems)
            {
                directionList.Add(e);
            }
            OffsetDirectionControl.TheList = directionList;
            OffsetDirectionControl.UpdateDisplayFunction = () => selectedMemory.OffsetDirection;
            OffsetDirectionControl.UpdateRigFunction =
                (object v) => selectedMemory.OffsetDirection = (AllRadios.OffsetDirections)v;
            combos.Add(OffsetDirectionControl);

            // repeater offset
            OffsetFreqList = new ArrayList();
            for (int i = rig.MinOffsetFrequency; i <= rig.MaxOffsetFrequency; i += rig.OffsetFrequencyStep)
            {
                OffsetFreqList.Add(new TS2000Filters.numericElement(i));
            }
            OffsetFrequencyControl.TheList = OffsetFreqList;
            OffsetFrequencyControl.UpdateDisplayFunction = () => selectedMemory.OffsetFrequency / 1000;
            OffsetFrequencyControl.UpdateRigFunction =
                (object v) => selectedMemory.OffsetFrequency = (int)v * 1000;
            combos.Add(OffsetFrequencyControl);

            // Step sizes
            StepSizeSSBList = new ArrayList();
            foreach (float f in KenwoodTS2000.stepSizesSSB)
            {
                StepSizeSSBList.Add(new TS2000Filters.stepElement(f));
            }
            StepSizeSSBCWFSKControl.TheList = StepSizeSSBList;
            StepSizeSSBCWFSKControl.UpdateDisplayFunction =
                () => { return KenwoodTS2000.stepIDToSize(selectedMemory.StepSize, selectedMemory.Mode[0]); };
            StepSizeSSBCWFSKControl.UpdateRigFunction =
                (object v) => { selectedMemory.StepSize = KenwoodTS2000.stepSizeToID((float)v, selectedMemory.Mode[0]); };
            combos.Add(StepSizeSSBCWFSKControl);

            StepSizeAMFMList = new ArrayList();
            foreach (float f in KenwoodTS2000.stepSizesAMFM)
            {
                StepSizeAMFMList.Add(new TS2000Filters.stepElement(f));
            }
            StepSizeAMFMControl.TheList = StepSizeAMFMList;
            StepSizeAMFMControl.UpdateDisplayFunction =
                () => { return KenwoodTS2000.stepIDToSize(selectedMemory.StepSize, selectedMemory.Mode[0]); };
            StepSizeAMFMControl.UpdateRigFunction =
                (object v) => { selectedMemory.StepSize = KenwoodTS2000.stepSizeToID((float)v, selectedMemory.Mode[0]); };
            combos.Add(StepSizeAMFMControl);

            // Memory group
            GroupList = new ArrayList();
            for (int i = KenwoodTS2000.minMemoryGroupID; i <= KenwoodTS2000.maxMemoryGroupID; i++)
            {
                GroupList.Add(new TS2000Filters.numericElement(i));
            }
            MemoryGroupControl.TheList = GroupList;
            MemoryGroupControl.UpdateDisplayFunction = () => selectedMemory.GroupID;
            MemoryGroupControl.UpdateRigFunction =
                (object v) => selectedMemory.GroupID = (int)v;
            combos.Add(MemoryGroupControl);

            // FM tone or CTCSS mode.
            toneCTCSSList = new ArrayList();
            foreach (AllRadios.ToneCTCSSValue t in rig.FMToneModes)
            {
                toneCTCSSList.Add(new TS2000Filters.toneCTCSSElement(t));
            }
            ToneCTCSSControl.TheList = toneCTCSSList;
            ToneCTCSSControl.UpdateDisplayFunction = () => selectedMemory.ToneCTCSS;
            ToneCTCSSControl.UpdateRigFunction =
                (object v) => selectedMemory.ToneCTCSS = (Radios.AllRadios.ToneCTCSSValue)v;
            combos.Add(ToneCTCSSControl);

            // FM tone frequency
            toneFreqList = new ArrayList();
            foreach (float f in rig.ToneFrequencyTable)
            {
                toneFreqList.Add(new TS2000Filters.toneCTCSSFreqElement(f));
            }
            ToneFrequencyControl.TheList = toneFreqList;
            ToneFrequencyControl.UpdateDisplayFunction = () => selectedMemory.ToneFrequency;
            ToneFrequencyControl.UpdateRigFunction =
                (object v) => selectedMemory.ToneFrequency = (float)v;
            combos.Add(ToneFrequencyControl);

            // FM CTCSS frequency
            CTCSSFreqList = new ArrayList();
            foreach (float f in rig.ToneFrequencyTable)
            {
                CTCSSFreqList.Add(new TS2000Filters.toneCTCSSFreqElement(f));
            }
            CTSSFrequencyControl.TheList = CTCSSFreqList;
            CTSSFrequencyControl.UpdateDisplayFunction = () => selectedMemory.CTSSFrequency;
            CTSSFrequencyControl.UpdateRigFunction =
                (object v) => selectedMemory.CTSSFrequency = (float)v;
            combos.Add(CTSSFrequencyControl);

            lockoutList = new ArrayList();
            lockoutList.Add(new TS2000Filters.trueFalseElement(false));
            lockoutList.Add(new TS2000Filters.trueFalseElement(true));
            LockoutControl.TheList = lockoutList;
            LockoutControl.UpdateDisplayFunction = () => selectedMemory.Lockout;
            LockoutControl.UpdateRigFunction =
                (object v) => selectedMemory.Lockout = (bool)v;
            combos.Add(LockoutControl);
        }

        public TS2000Memories(AllRadios r)
        {
            InitializeComponent();
            rig = r;
        }

        private void TS2000Memories_Load(object sender, EventArgs e)
        {
            Tracing.TraceLine("TS2000Memories_Load",TraceLevel.Info);
            wasActive = false;
            DialogResult = System.Windows.Forms.DialogResult.Abort;
            if (!rig.MemoriesLoaded)
            {
                MessageBox.Show(memoriesNotLoaded, MessageTitle);
                return;
            }
            DialogResult = System.Windows.Forms.DialogResult.None;
            if (!wasSetup)
            {
                setupBoxes();
                wasSetup = true;
            }
            //MemoryListBox.DataSource = null;
            Tracing.TraceLine("TS2000Memories_Load end", TraceLevel.Info);
        }

        private void refreshMemoryList()
        {
            // no channel specified
            refreshMemoryList(-1);
        }
        private void refreshMemoryList(int channel)
        {
            Tracing.TraceLine("refreshMemoryList:" + channel.ToString());
            int id = -1;
            if (channel >= 0)
            {
                AllRadios.MemoryData m;
                for (int i = 0; i < theList.Count; i++)
                {
                    m = ((memoryItem)theList[i]).Value;
                    if (m.Number == channel)
                    {
                        // The current memory channel is reported automatically if changed.
                        if (m.Number != rig.CurrentMemoryChannel)
                        {
                            // Fetch the memory from the rig.
                            theList[i] = new memoryItem(rig, rig.Memories[m.Number]);
                        }
                        id = i;
                        break;
                    }
                }
            }
            // We do all this to ensure we're showing the latest values.
            MemoryListBox.SuspendLayout();
            MemoryListBox.DataSource = null;
            MemoryListBox.DisplayMember = "Display";
            MemoryListBox.ValueMember = "Value";
            MemoryListBox.DataSource = theList;
            if (id != -1) MemoryListBox.SelectedIndex = id;
            MemoryListBox.ResumeLayout();
            Tracing.TraceLine("refreshMemoryList end",TraceLevel.Info);
        }

        private void MemoryListBox_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            Tracing.TraceLine("MemoryListBox_SelectedIndexChanged:" + MemoryListBox.SelectedIndex.ToString(), TraceLevel.Info);
            selectedMemory = (AllRadios.MemoryData)MemoryListBox.SelectedValue;
            if (selectedMemory != null)
            {
                FrequencyTextBox.Text = rig.Callouts.FormatFreq(selectedMemory.Frequency[0]);
                TFrequencyTextBox.Text = rig.Callouts.FormatFreq(selectedMemory.Frequency[1]);
                NameTextBox.Text = selectedMemory.Name;
                if (!selectedMemory.Present)
                {
                    // Setup defaults.
                    ModeControl.SetSelectedIndex(0);
                    TModeControl.SetSelectedIndex(0);
                    SplitControl.SetSelectedIndex(0);
                    ReverseControl.SetSelectedIndex(0);
                    OffsetDirectionControl.SetSelectedIndex(0);
                    OffsetFrequencyControl.SetSelectedIndex(0);
                    ToneCTCSSControl.SetSelectedIndex(0);
                    ToneFrequencyControl.SetSelectedIndex(0);
                    CTSSFrequencyControl.SetSelectedIndex(0);
                    StepSizeAMFMControl.SetSelectedIndex(0);
                    StepSizeSSBCWFSKControl.SetSelectedIndex(0);
                    MemoryGroupControl.SetSelectedIndex(0);
                    LockoutControl.SetSelectedIndex(0);
                }
                else
                {
                    // Set from the memory.
                    foreach (Combo c in combos)
                    {
                        c.UpdateDisplay(true);
                    }
                }
                // enable/disable fields conditionally.
                conditionalFields();
                // The split field isn't always applicable.
                SplitControl.Visible = (selectedMemory.Type != AllRadios.MemoryTypes.Range);
                SplitControl.Enabled = (selectedMemory.Type != AllRadios.MemoryTypes.Range);
            }
        }

        private void conditionalFields()
        {
            // enable/disable fields used if split or range.
            bool splitOrRange = selectedMemory.Split | (selectedMemory.Type == AllRadios.MemoryTypes.Range);
            TFrequencyTextBox.Visible = splitOrRange;
            TFrequencyTextBox.Enabled = splitOrRange;
            TModeControl.Enabled = splitOrRange;
            TModeControl.Visible = splitOrRange;
            // Disable SetFromVFO if memory mode, or rig in split mode and we don't know the xmit mode.
            SetFromVFOButton.Enabled = !(rig.MemoryMode | rig.CallChannel.Active | (rig.Split & (rig.TXMode == null)));
        }

        private delegate void rtn(Control c);
        private rtn enab = enable;
        private rtn disab = disable;
        private static void enable(Control c)
        {
            if (!c.Enabled)
            {
                c.Enabled = true;
                c.Visible = true;
                c.BringToFront();
            }
        }
        private static void disable(Control c)
        {
            if (c.Enabled)
            {
                c.Enabled = false;
                c.Visible = false;
                c.SendToBack();
            }
        }

        private class modeChangeClass
        {
            // A mode's controls are enabled when that mode is selected.
            private class controlsClass
            {
                public Control[] controls;
                public controlsClass(Control[] controlArray)
                {
                    controls = controlArray;
                }
            }
            private controlsClass[] modeControls;
            private TS2000Memories parent;
            public modeChangeClass(TS2000Memories p)
            {
                parent = p;
                modeControls = new controlsClass[Kenwood.myModeTable.Length];

                // setup the mode to combobox mapping.
                modeControls[(int)Kenwood.modes.fm] = new controlsClass(
                    new Control[] {
                        parent.OffsetDirectionControl, parent.OffsetFrequencyControl,
                        parent.ReverseControl, parent.ToneCTCSSControl,
                        parent.ToneFrequencyControl, parent.CTSSFrequencyControl,
                        parent.StepSizeAMFMControl,
                    });
                modeControls[(int)Kenwood.modes.lsb] = new controlsClass(
                    new Control[] {
                        parent.StepSizeSSBCWFSKControl,
                    });
                modeControls[(int)Kenwood.modes.usb] = new controlsClass(
                    new Control[] {
                        parent.StepSizeSSBCWFSKControl,
                    });
                modeControls[(int)Kenwood.modes.cw] = new controlsClass(
                    new Control[] {
                        parent.StepSizeSSBCWFSKControl,
                    });
                modeControls[(int)Kenwood.modes.cwr] = new controlsClass(
                    new Control[] {
                        parent.StepSizeSSBCWFSKControl,
                    });
                modeControls[(int)Kenwood.modes.fsk] = new controlsClass(
                    new Control[] {
                        parent.StepSizeSSBCWFSKControl,
                    });
                modeControls[(int)Kenwood.modes.fskr] = new controlsClass(
                    new Control[] {
                        parent.StepSizeSSBCWFSKControl,
                    });
                modeControls[(int)Kenwood.modes.am] = new controlsClass(
                    new Control[] {
                        parent.StepSizeAMFMControl,
                    });
            }

            private AllRadios.ModeValue oldMode = Kenwood.myModeTable[0];
            public void enableDisable(Kenwood.ModeValue mode)
            {
                // Just quit if the mode hasn't changed.
                if (mode == oldMode) return;
                oldMode = mode;
                int mod = mode.id;
                // quit if no controls for this mode.
                if (modeControls[mod] == null) return;
                // enables holds the controls to be enabled.
                Control[] enables = new Control[modeControls[mod].controls.Length];
                for (int i = 0; i < enables.Length; i++)
                {
                    // We need to quit if no more controls for this mode.
                    if (modeControls[mod].controls[i] == null) break;
                    enables[i] = modeControls[mod].controls[i];
                }
                parent.SuspendLayout();
                for (int i = 0; i < modeControls.Length; i++)
                {
                    if (modeControls[i] == null) continue;
                    for (int j = 0; j < modeControls[i].controls.Length; j++)
                    {
                        Control c = modeControls[i].controls[j];
                        if (c == null) break;
                        if (Array.IndexOf(enables, c) >= 0)
                        {
                            // enable
                            if (parent.InvokeRequired)
                            {
                                parent.Invoke(parent.enab, new object[] { c });
                            }
                            else
                            {
                                parent.enab(c);
                            }
                        }
                        else
                        {
                            // disable
                            if (parent.InvokeRequired)
                            {
                                parent.Invoke(parent.disab, new object[] { c });
                            }
                            else
                            {
                                parent.disab(c);
                            }
                        }
                    }
                }
                parent.ResumeLayout();
            }
        }
        private modeChangeClass modeChange;

        private void splitChanged(bool val)
        {
            selectedMemory.Split = val;
            conditionalFields();
        }

        private void ChangeButton_Click(System.Object sender, System.EventArgs e)
        {
            Tracing.TraceLine("ChangeButton", TraceLevel.Info);
            DialogResult = System.Windows.Forms.DialogResult.None;
            AllRadios.MemoryData m = selectedMemory;
            if (m == null)
            {
                MessageBox.Show(notSelected, MessageTitle);
                return;
            }
            // Convert kkk.hhh to m.kkk.hhh
            string str = FrequencyTextBox.Text;
            if (str.Split(new char[] { '.' }, StringSplitOptions.None).Length == 2) str = "0." + str;
            m.Frequency[0] = rig.Callouts.FormatFreqForRadio(str);
            str = TFrequencyTextBox.Text;
            if (str.Split(new char[] { '.' }, StringSplitOptions.None).Length == 2) str = "0." + str;
            m.Frequency[1] = rig.Callouts.FormatFreqForRadio(str);
            // check the frequencies
            bool err = false;
            if (m.Frequency[0] == 0)
            {
                MessageBox.Show(numericFrequency, MessageTitle);
                FrequencyTextBox.Focus();
                err = true;
            }
            if (TFrequencyTextBox.Enabled && (m.Frequency[1] == 0))
            {
                MessageBox.Show(numericFrequency, MessageTitle);
                TFrequencyTextBox.Focus();
                err = true;
            }
            if (!err)
            {
                m.Present = true;
                m.Name = NameTextBox.Text;
                rig.Memories[m.Number] = m;
                refreshMemoryList(m.Number);
                MemoryListBox.Focus();
            }
        }

        private void SetFromVFOButton_Click(System.Object sender, System.EventArgs e)
        {
            Tracing.TraceLine("SetFromVFOButton", TraceLevel.Info);
            DialogResult = System.Windows.Forms.DialogResult.None;
            AllRadios.MemoryData m = selectedMemory;
            if (m != null)
            {
                if (m.Present)
                {
                    if (MessageBox.Show(notEmptyText, 
                        notEmptyM1 + m.Number.ToString() + notEmptyM2,
                        MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return;
                    }
                }
                else
                {
                    m.Present = true;
                }
                m.Frequency[0] = rig.RXFrequency;
                m.Mode[0] = rig.RXMode;
                if ((rig.OffsetDirection == AllRadios.OffsetDirections.off) &&
                   ((rig.RXFrequency != rig.TXFrequency) ||
                    (rig.RXMode != rig.TXMode)))
                {
                    m.Split = true;
                    m.Frequency[1] = rig.TXFrequency;
                    m.Mode[1] = rig.TXMode;
                }
                else
                {
                    m.Split = false;
                }
                m.ToneCTCSS = rig.ToneCTCSS;
                m.ToneFrequency = rig.ToneFrequency;
                m.CTSSFrequency = rig.CTSSFrequency;
                m.OffsetFrequency = rig.OffsetFrequency;
                m.OffsetDirection = rig.OffsetDirection;
                m.Reverse = false;
                m.DCS = rig.DCS;
                m.StepSize = KenwoodTS2000.stepSizeToID(rig.StepSize, m.Mode[0]);
                m.Lockout = false;
                m.Name = " ";
                // m.type should already be setup.
                rig.Memories[m.Number] = m;
                refreshMemoryList(m.Number);
                MemoryListBox.Focus();
            }
        }

        private void DoneButton_Click(System.Object sender, System.EventArgs e)
        {
            Tracing.TraceLine("DoneButton", TraceLevel.Info);
            //MemoryListBox.DataSource = null;
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void DeleteButton_Click(System.Object sender, System.EventArgs e)
        {
            Tracing.TraceLine("DeleteButton", TraceLevel.Info);
            DialogResult = System.Windows.Forms.DialogResult.None;
            if (selectedMemory != null)
            {
                selectedMemory.Present = false;
                rig.Memories[selectedMemory.Number] = selectedMemory;
                refreshMemoryList(selectedMemory.Number);
                MemoryListBox.Focus();
            }
        }

        private void Memories_Activated(System.Object sender, System.EventArgs e)
        {
            Tracing.TraceLine("Memories_Activated", TraceLevel.Info);
            if ((DialogResult == System.Windows.Forms.DialogResult.None) & (!wasActive))
            {
#if zero
                if (!wasSetup)
                {
                    setupBoxes();
                    wasSetup = true;
                }
#endif
                refreshMemoryList(rig.CurrentMemoryChannel);
                MemoryListBox.Focus();
                wasActive = true;
            }
        }

        private void MemoryListBox_KeyPress(System.Object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            Tracing.TraceLine("MemoryListBox_KeyPress", TraceLevel.Verbose);
            if ((selectedMemory != null) && (e.KeyChar == (char)Keys.Return))
            {
                Tracing.TraceLine("MemoryListBox:going to " + selectedMemory.Number.ToString(), TraceLevel.Info);
                e.Handled = true;
                if (!(rig.MemoryMode &&
                    (rig.CurrentMemoryChannel == selectedMemory.Number)))
                {
                    rig.CurrentMemoryChannel = selectedMemory.Number;
                    rig.MemoryMode = true;
                }
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }

        internal void Done()
        {
            Tracing.TraceLine("done", TraceLevel.Info);
            wasSetup = false;
            wasActive = false;
            rig.MemoriesLoaded = false;
        }

        private void EmptyNonemptyButton_Click(object sender, EventArgs e)
        {
            Tracing.TraceLine("EmptyNonemptyButton", TraceLevel.Info);
            allMemories = (!allMemories);
            EmptyNonemptyButton.Text = (allMemories) ? OnlyUsed : IncludeEmpty;
            theList = new ArrayList();
            for (int i = 0; i <= rig.NumberOfMemories - 1; i++)
            {
                AllRadios.MemoryData m = rig.Memories[i];
                if (allMemories || (m.Present))
                {
                    theList.Add(new memoryItem(rig, m));
                }
            }
            refreshMemoryList();
            MemoryListBox.Focus();
        }
    }
}
