using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Radios;
using RadioBoxes;
using System.Threading;
using JJTrace;


public class TS590Memories
{
#region "r"
	const string memoriesNotLoaded = "The memories are still being loaded.";
	const string notSelected = "No memory is selected.";
	const string notEmptyText = "Do you want to overwrite it?";
	const string notEmptyM1 = "Memory ";
	const string notEmptyM2 = " is not empty.";

	const string useDelete = "Use the delete button to delete a memory.";
	private bool wasSetup;
	private bool wasActive;
	private class memoryItem
	{
		private AllRadios.MemoryData mem;
		public memoryItem(int i)
		{
			mem = RigControl.Memories(i);
		}
		public string Display {
			get {
				string str = mem.Number.ToString("d3") + " ";
				if (mem.Present) {
					if (!string.IsNullOrEmpty(mem.Name)) {
						str += mem.Name;
					} else {
						str += FormatFreq(mem.Frequency(0));
					}
				} else {
					str += "empty";
				}
				return str;
			}
		}
		public AllRadios.MemoryData Value {
			get { return mem; }
		}
	}
	private ArrayList theList;
	private AllRadios.MemoryData selectedMemory;

	private Collection combos;
#endregion
	private void Memories_Load(System.Object sender, System.EventArgs e)
	{
#region "r1"
		if (!MemoriesLoaded) {
			Interaction.MsgBox(memoriesNotLoaded);
			DialogResult = System.Windows.Forms.DialogResult.Abort;
			return;
		}
		DialogResult = System.Windows.Forms.DialogResult.None;
		wasActive = false;
		if (!wasSetup) {
			setupBoxes();
			wasSetup = true;
		}
		MemoryListBox.DataSource = null;
#endregion
	}
#region "r2"
	private class FMModeElement
	{
		private AllRadios.FMModes val;
		public string Display {
			get { return val.ToString; }
		}
		public AllRadios.FMModes RigItem {
			get { return val; }
		}
		public FMModeElement(AllRadios.FMModes v)
		{
			val = v;
		}
	}
	private FMModeElement[] FMModeItems = {
		new FMModeElement(AllRadios.FMModes.Normal),
		new FMModeElement(AllRadios.FMModes.Narrow)
	};

	private ArrayList FMModeList;
	// true/false lists
	private ArrayList splitList;

	private ArrayList lockoutList;
	private void setupBoxes()
	{
		theList = new ArrayList();
		for (int i = 0; i <= RigControl.NumberOfMemories - 1; i++) {
			theList.Add(new memoryItem(i));
		}

		combos = new Collection();
		ModeControl.TheList = Form1.modeList;
		ModeControl.UpdateDisplayFunction = () => selectedMemory.Mode(0);
		ModeControl.UpdateRigFunction = (Radios.AllRadios.Modes v) => selectedMemory.Mode(0) == v;
		combos.Add(ModeControl);

		DataModeControl.TheList = Form1.dataModeList;
		DataModeControl.UpdateDisplayFunction = () => selectedMemory.DataMode(0);
		DataModeControl.UpdateRigFunction = (Radios.AllRadios.DataModes v) => selectedMemory.DataMode(0) == v;
		combos.Add(DataModeControl);

		TModeControl.TheList = Form1.modeList;
		TModeControl.UpdateDisplayFunction = () => selectedMemory.Mode(1);
		TModeControl.UpdateRigFunction = (Radios.AllRadios.Modes v) => selectedMemory.Mode(1) == v;
		combos.Add(TModeControl);

		TDataModeControl.TheList = Form1.dataModeList;
		TDataModeControl.UpdateDisplayFunction = () => selectedMemory.DataMode(1);
		TDataModeControl.UpdateRigFunction = (Radios.AllRadios.DataModes v) => selectedMemory.DataMode(1) == v;
		combos.Add(TDataModeControl);

		splitList = new ArrayList();
		splitList.Add(new Form1.TrueFalseElement(false));
		splitList.Add(new Form1.TrueFalseElement(true));
		SplitControl.TheList = splitList;
		SplitControl.UpdateDisplayFunction = () => selectedMemory.Split;
		SplitControl.UpdateRigFunction = (bool v) => this.splitChanged(v);
		combos.Add(SplitControl);

		ToneCTCSSControl.TheList = Form1.ToneCTCSSList;
		ToneCTCSSControl.UpdateDisplayFunction = () => selectedMemory.ToneCTCSS;
		ToneCTCSSControl.UpdateRigFunction = (Radios.AllRadios.ToneCTCSSValue v) => selectedMemory.ToneCTCSS == v;
		combos.Add(ToneCTCSSControl);

		ToneFrequencyControl.TheList = Form1.toneFreqList;
		ToneFrequencyControl.UpdateDisplayFunction = () => selectedMemory.ToneFrequency;
		ToneFrequencyControl.UpdateRigFunction = (float v) => selectedMemory.ToneFrequency == v;
		combos.Add(ToneFrequencyControl);

		CTSSFrequencyControl.TheList = Form1.CTSSFreqList;
		CTSSFrequencyControl.UpdateDisplayFunction = () => selectedMemory.CTSSFrequency;
		CTSSFrequencyControl.UpdateRigFunction = (float v) => selectedMemory.CTSSFrequency == v;
		combos.Add(CTSSFrequencyControl);

		FMModeList = new ArrayList();
		foreach (FMModeElement f in FMModeItems) {
			FMModeList.Add(f);
		}
		FMModeControl.TheList = FMModeList;
		FMModeControl.UpdateDisplayFunction = () => selectedMemory.FMMode;
		FMModeControl.UpdateRigFunction = (AllRadios.FMModes v) => selectedMemory.FMMode == v;
		combos.Add(FMModeControl);

		lockoutList = new ArrayList();
		lockoutList.Add(new Form1.TrueFalseElement(false));
		lockoutList.Add(new Form1.TrueFalseElement(true));
		LockoutControl.TheList = lockoutList;
		LockoutControl.UpdateDisplayFunction = () => selectedMemory.Lockout;
		LockoutControl.UpdateRigFunction = (bool v) => selectedMemory.Lockout == v;
		combos.Add(LockoutControl);
	}
#endregion
#region "r3"
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
		// We do all this to ensure we're showing the latest values.
		MemoryListBox.SuspendLayout();
		MemoryListBox.DataSource = null;
		MemoryListBox.DisplayMember = "Display";
		MemoryListBox.ValueMember = "Value";
		MemoryListBox.DataSource = theList;
		if (channel >= 0) {
			MemoryListBox.SelectedIndex = channel;
		}
		MemoryListBox.ResumeLayout();
	}

	private void MemoryListBox_SelectedIndexChanged(System.Object sender, System.EventArgs e)
	{
		selectedMemory = MemoryListBox.SelectedValue;
		if (selectedMemory != null) {
			FrequencyTextBox.Text = FormatFreq(selectedMemory.Frequency(0));
			TFrequencyTextBox.Text = FormatFreq(selectedMemory.Frequency(1));
			NameTextBox.Text = selectedMemory.Name;
			foreach (Combo c in combos) {
				c.UpdateDisplay();
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
		if (m == null) {
			Interaction.MsgBox(notSelected);
			return;
		}
		m.Frequency(0) = FormatFreqForRadio(FrequencyTextBox.Text);
		m.Frequency(1) = FormatFreqForRadio(TFrequencyTextBox.Text);
		m.Name = NameTextBox.Text;
		// Make sure m.present is set if there's a frequency.
		if (m.Frequency(0) != 0) {
			m.Present = true;
			RigControl.Memories(m.Number) = m;
			refreshMemoryList();
			MemoryListBox.Focus();
		} else {
			Interaction.MsgBox(useDelete);
		}
	}

	private void SetFromVFOButton_Click(System.Object sender, System.EventArgs e)
	{
		DialogResult = System.Windows.Forms.DialogResult.None;
		AllRadios.MemoryData m = selectedMemory;
		if (m != null) {
			if (m.Present) {
				YesNo.Title = notEmptyM1 + m.Number.ToString + notEmptyM2;
				YesNo.Text = notEmptyText;
				if (YesNo.ShowDialog != DialogResult.Yes) {
					return;
				}
			} else {
				m.Present = true;
			}
			m.Frequency(0) = RigControl.RXFrequency;
			m.Mode(0) = RigControl.RXMode;
			m.DataMode(0) = RigControl.RXDataMode;
			if ((RigControl.RXFrequency != RigControl.TXFrequency) | (RigControl.RXMode != RigControl.TXMode) | (RigControl.RXDataMode != RigControl.TXDataMode)) {
				m.Split = true;
				m.Frequency(1) = RigControl.TXFrequency;
				m.Mode(1) = RigControl.TXMode;
				m.DataMode(1) = RigControl.TXDataMode;
			} else {
				m.Split = false;
			}
			m.ToneCTCSS = RigControl.ToneCTCSS;
			m.ToneFrequency = RigControl.ToneFrequency;
			m.CTSSFrequency = RigControl.CTSSFrequency;
			//m.FMMode=RigControl.fmmode
			m.Lockout = false;
			// m.type should already be setup.
			RigControl.Memories(m.Number) = m;
			refreshMemoryList();
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
		if (selectedMemory != null) {
			selectedMemory.Present = false;
			RigControl.Memories(selectedMemory.Number) = selectedMemory;
			refreshMemoryList();
			MemoryListBox.Focus();
		}
	}

	private void Memories_Activated(System.Object sender, System.EventArgs e)
	{
		if ((DialogResult == System.Windows.Forms.DialogResult.None) & (!wasActive)) {
			refreshMemoryList(RigControl.CurrentMemoryChannel);
			MemoryListBox.Focus();
			wasActive = true;
		}
	}

	private void MemoryListBox_KeyPress(System.Object sender, System.Windows.Forms.KeyPressEventArgs e)
	{
		if ((selectedMemory != null) && (e.KeyChar == Strings.ChrW(Keys.Return))) {
			e.Handled = true;
			RigControl.CurrentMemoryChannel = selectedMemory.Number;
			RigControl.MemoryMode = true;
			DialogResult = System.Windows.Forms.DialogResult.OK;
		}
	}

	internal void Done()
	{
		wasSetup = false;
		wasActive = false;
		MemoriesLoaded = false;
	}
#endregion
	public Memories()
	{
		Activated += Memories_Activated;
		Load += Memories_Load;
	}
}
