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
    public partial class TS590Memories : Form
    {
        const string MessageTitle = "Message";
        const string memoriesNotLoaded = "The memories are still being loaded.";
        const string notSelected = "No memory is selected.";
        const string notEmptyText = "Do you want to overwrite it?";
        const string notEmptyM1 = "Memory ";
        const string notEmptyM2 = " is not empty.";
        const string includeEmpty = "Include empty memories";
        const string OnlyUsed = "Only used memories";
        const string numericFrequency = "Specify a valid frequency (mmm.kkk)";

        private KenwoodTS590 rig;
        private bool wasSetup;
        private bool wasActive;

        private bool allMemories;
        private class memoryItem
        {
            private AllRadios.MemoryData mem;
            private KenwoodTS590 rig;
            public memoryItem(int i, KenwoodTS590 r)
            {
                rig = r;
                mem = rig.Memories[i];
            }
            public string Display
            {
                get
                {
                    string str = mem.Number.ToString("d3") + " ";
                    if (mem.Present)
                    {
                        if (!string.IsNullOrEmpty(mem.Name))
                        {
                            str += mem.Name;
                        }
                        else
                        {
                            str += rig.Callouts.FormatFreq(mem.Frequency[0]);
                        }
                    }
                    else
                    {
                        str += "empty";
                    }
                    return str;
                }
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

        private static TS590Filters.dataModeElement[] dataModeItems =
        {
            new TS590Filters.dataModeElement(AllRadios.DataModes.off),
            new TS590Filters.dataModeElement(AllRadios.DataModes.on)
        };
        private ArrayList dataModeList, tDataModeList;

        private ArrayList toneCTCSSList;
        private ArrayList toneFreqList, CTCSSFreqList;

        private ArrayList FMWidthList;

        // true/false lists
        private ArrayList splitList;

        private ArrayList lockoutList;

        private void setupBoxes()
        {
            // Initial state is to only show used memories
            EmptyNonemptyButton.Text = includeEmpty;
            allMemories = false;

            // Setup the mode change stuff.
            modeChange = new modeChangeClass(this);

            theList = new ArrayList();
            for (int i = 0; i <= rig.NumberOfMemories - 1; i++)
            {
                if (rig.Memories[i].Present)
                {
                    theList.Add(new memoryItem(i, rig));
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

            dataModeList = new ArrayList();
            tDataModeList = new ArrayList();
            foreach (TS590Filters.dataModeElement e in dataModeItems)
            {
                dataModeList.Add(e);
                tDataModeList.Add(e);
            }
            DataModeControl.TheList = dataModeList;
            DataModeControl.TheList = dataModeList;
            DataModeControl.UpdateDisplayFunction = () => selectedMemory.DataMode[0];
            DataModeControl.UpdateRigFunction = 
                (object v) => selectedMemory.DataMode[0] = (Radios.AllRadios.DataModes)v;
            combos.Add(DataModeControl);

            TModeControl.TheList = tModeList;
            TModeControl.UpdateDisplayFunction = () => selectedMemory.Mode[1];
            TModeControl.UpdateRigFunction =
                (object v) => selectedMemory.Mode[1] = (Radios.AllRadios.ModeValue)v;
            combos.Add(TModeControl);

            TDataModeControl.TheList = tDataModeList; // setup above
            TDataModeControl.UpdateDisplayFunction = () => selectedMemory.DataMode[1];
            TDataModeControl.UpdateRigFunction =
                (object v) => selectedMemory.DataMode[1] = (Radios.AllRadios.DataModes)v;
            combos.Add(TDataModeControl);

            splitList = new ArrayList();
            splitList.Add(new TS590Filters.trueFalseElement(false));
            splitList.Add(new TS590Filters.trueFalseElement(true));
            SplitControl.TheList = splitList;
            SplitControl.UpdateDisplayFunction = () => selectedMemory.Split;
            SplitControl.UpdateRigFunction =
                (object v) => this.splitChanged((bool)v);
            combos.Add(SplitControl);

            // FM tone or CTCSS mode.
            toneCTCSSList = new ArrayList();
            foreach (AllRadios.ToneCTCSSValue t in rig.FMToneModes)
            {
                toneCTCSSList.Add(new TS590Filters.toneCTCSSElement(t));
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
                toneFreqList.Add(new TS590Filters.toneCTCSSFreqElement(f));
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
                CTCSSFreqList.Add(new TS590Filters.toneCTCSSFreqElement(f));
            }
            CTSSFrequencyControl.TheList = CTCSSFreqList;
            CTSSFrequencyControl.UpdateDisplayFunction = () => selectedMemory.CTSSFrequency;
            CTSSFrequencyControl.UpdateRigFunction =
                (object v) => selectedMemory.CTSSFrequency = (float)v;
            combos.Add(CTSSFrequencyControl);

            FMWidthList = new ArrayList();
            FMWidthList.Add(new TS590Filters.FMWidthElement(TS590Filters.fmModes590.Wide));
            FMWidthList.Add(new TS590Filters.FMWidthElement(TS590Filters.fmModes590.Naro));
            FMWidthControl.TheList = FMWidthList;
            FMWidthControl.UpdateDisplayFunction =
                () => { return (TS590Filters.fmModes590)selectedMemory.FMMode; };
            FMWidthControl.UpdateRigFunction =
                (object v) => selectedMemory.FMMode = (int)v;
            combos.Add(FMWidthControl);

            lockoutList = new ArrayList();
            lockoutList.Add(new TS590Filters.trueFalseElement(false));
            lockoutList.Add(new TS590Filters.trueFalseElement(true));
            LockoutControl.TheList = lockoutList;
            LockoutControl.UpdateDisplayFunction = () => selectedMemory.Lockout;
            LockoutControl.UpdateRigFunction =
                (object v) => selectedMemory.Lockout = (bool)v;
            combos.Add(LockoutControl);
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
            private TS590Memories parent;
            public modeChangeClass(TS590Memories p)
            {
                parent = p;
                modeControls = new controlsClass[Kenwood.myModeTable.Length];

                // setup the mode to combobox mapping.
                modeControls[(int)Kenwood.modes.fm] = new controlsClass(
                    new Control[] {
                        parent.ToneCTCSSControl, parent.FMWidthControl,
                        parent.ToneFrequencyControl, parent.CTSSFrequencyControl,
                    });
            }

            private AllRadios.ModeValue oldMode = Kenwood.myModeTable[0];
            public void enableDisable(Kenwood.ModeValue mode)
            {
                // Just quit if the mode hasn't changed.
                if (mode == oldMode) return;
                oldMode = mode;
                int mod = mode.id;
                // enables holds the controls to be enabled.
                int len = ((modeControls[mod] != null) && (modeControls[mod].controls != null))?
                    modeControls[mod].controls.Length : 0;
                Control[] enables = new Control[len];
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

        public TS590Memories(KenwoodTS590 r)
        {
            InitializeComponent();
            rig = r;
        }

        private void TS590Memories_Load(object sender, EventArgs e)
        {
            if (!rig.MemoriesLoaded)
            {
                MessageBox.Show(memoriesNotLoaded, MessageTitle);
                DialogResult = System.Windows.Forms.DialogResult.Abort;
                return;
            }
            DialogResult = System.Windows.Forms.DialogResult.None;
            wasActive = false;
            if (!wasSetup)
            {
                setupBoxes();
                wasSetup = true;
            }
            MemoryListBox.DataSource = null;
        }

        private void MemoryListBox_Enter(System.Object sender, System.EventArgs e)
        {
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
                        // Fetch the memory from the rig.
                        theList[i] = new memoryItem(m.Number, rig);
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
            Tracing.TraceLine("refreshMemoryList end", TraceLevel.Info);
        }

        private void MemoryListBox_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            selectedMemory = (AllRadios.MemoryData)MemoryListBox.SelectedValue;
            if (selectedMemory != null)
            {
                FrequencyTextBox.Text = rig.Callouts.FormatFreq(selectedMemory.Frequency[0]);
                TFrequencyTextBox.Text = rig.Callouts.FormatFreq(selectedMemory.Frequency[1]);
                NameTextBox.Text = selectedMemory.Name;
                if (!selectedMemory.Present)
                {
                    // Set defaults for these.
                    ModeControl.SetSelectedIndex(0);
                    TModeControl.SetSelectedIndex(0);
                    DataModeControl.SetSelectedIndex(0);
                    TDataModeControl.SetSelectedIndex(0);
                    SplitControl.SetSelectedIndex(0);
                    ToneCTCSSControl.SetSelectedIndex(0);
                    ToneFrequencyControl.SetSelectedIndex(0);
                    CTSSFrequencyControl.SetSelectedIndex(0);
                    FMWidthControl.SetSelectedIndex(0);
                    LockoutControl.SetSelectedIndex(0);
                }
                else
                {
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
            TDataModeControl.Enabled = splitOrRange;
            TDataModeControl.Visible = splitOrRange;
            SetFromVFOButton.Enabled = !rig.MemoryMode;
        }

        private void splitChanged(bool val)
        {
            selectedMemory.Split = val;
            conditionalFields();
        }

        private void ChangeButton_Click(System.Object sender, System.EventArgs e)
        {
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
                m.DataMode[0] = rig.RXDataMode;
                if ((rig.RXFrequency != rig.TXFrequency) |
                    (rig.RXMode != rig.TXMode) |
                    (rig.RXDataMode != rig.TXDataMode))
                {
                    m.Split = true;
                    m.Frequency[1] = rig.TXFrequency;
                    m.Mode[1] = rig.TXMode;
                    m.DataMode[1] = rig.TXDataMode;
                }
                else
                {
                    m.Split = false;
                }
                m.ToneCTCSS = rig.ToneCTCSS;
                m.ToneFrequency = rig.ToneFrequency;
                m.CTSSFrequency = rig.CTSSFrequency;
                if (m.Mode[0].ToString() == "fm") m.FMMode = rig.filterWidth;
                m.Lockout = false;
                m.Name = "";
                // m.type should already be setup.
                rig.Memories[m.Number] = m;
                refreshMemoryList(m.Number);
                MemoryListBox.Focus();
            }
        }

        private void DoneButton_Click(System.Object sender, System.EventArgs e)
        {
            MemoryListBox.DataSource = null;
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void DeleteButton_Click(System.Object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.None;
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
            if ((DialogResult == System.Windows.Forms.DialogResult.None) & (!wasActive))
            {
                refreshMemoryList(rig.CurrentMemoryChannel);
                MemoryListBox.Focus();
                wasActive = true;
            }
        }

        private void MemoryListBox_KeyPress(System.Object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if ((selectedMemory != null) && (e.KeyChar == (char)Keys.Return))
            {
                Tracing.TraceLine("MemoryListBox:going to " + selectedMemory.Number.ToString(), TraceLevel.Info);
                e.Handled = true;
#if zero
                // We need these girations to update a currently selected memory.
                if (rig.MemoryMode &&
                    (rig.CurrentMemoryChannel == selectedMemory.Number))
                {
                    rig.MemoryMode = false;
                    // Wait up to .5 seconds for it to really change.
                    int sanity = 100;
                    while (rig.MemoryMode && (sanity-- > 0)) { Thread.Sleep(5); }
                }
                if (!(rig.MemoryMode &&
                    (rig.CurrentMemoryChannel == selectedMemory.Number)))
                {
                    rig.CurrentMemoryChannel = selectedMemory.Number;
                    rig.MemoryMode = true;
                }
#endif
                DialogResult = System.Windows.Forms.DialogResult.OK;
                if (rig.MemoryMode & (rig.CurrentMemoryChannel == selectedMemory.Number))
                {
                    // Force the rig to use new settings.
                    rig.MemoryMode = false;
                    AllRadios.await(() => { return rig.MemoryMode == false; }, 1000);
                }
                rig.CurrentMemoryChannel = selectedMemory.Number;
                rig.MemoryMode = true;
            }
        }

        internal void Done()
        {
            wasSetup = false;
            wasActive = false;
            rig.MemoriesLoaded = false;
        }

        private void EmptyNonemptyButton_Click(object sender, EventArgs e)
        {
            allMemories = (!allMemories);
            EmptyNonemptyButton.Text = (allMemories) ? OnlyUsed : includeEmpty;
            theList = new ArrayList();
            for (int i = 0; i <= rig.NumberOfMemories - 1; i++)
            {
                if (allMemories || (rig.Memories[i].Present))
                {
                    theList.Add(new memoryItem(i, rig));
                }
            }
            refreshMemoryList();
            MemoryListBox.Focus();
        }
    }
}
