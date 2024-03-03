using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
[Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
partial class Memories : System.Windows.Forms.Form
{

	//Form overrides dispose to clean up the component list.
	[System.Diagnostics.DebuggerNonUserCode()]
	protected override void Dispose(bool disposing)
	{
		try {
			if (disposing && components != null) {
				components.Dispose();
			}
		} finally {
			base.Dispose(disposing);
		}
	}

	//Required by the Windows Form Designer

	private System.ComponentModel.IContainer components;
	//NOTE: The following procedure is required by the Windows Form Designer
	//It can be modified using the Windows Form Designer.  
	//Do not modify it using the code editor.
	[System.Diagnostics.DebuggerStepThrough()]
	private void InitializeComponent()
	{
		this.LabelMemoryList = new System.Windows.Forms.Label();
		this.MemoryListBox = new System.Windows.Forms.ListBox();
		this.ChangeButton = new System.Windows.Forms.Button();
		this.DoneButton = new System.Windows.Forms.Button();
		this.FrequencyTextBox = new System.Windows.Forms.TextBox();
		this.LabelReceiveFreq = new System.Windows.Forms.Label();
		this.LabelTransmitFreq = new System.Windows.Forms.Label();
		this.TFrequencyTextBox = new System.Windows.Forms.TextBox();
		this.SetFromVFOButton = new System.Windows.Forms.Button();
		this.ModeControl = new RadioBoxes.Combo();
		this.DataModeControl = new RadioBoxes.Combo();
		this.SplitControl = new RadioBoxes.Combo();
		this.TModeControl = new RadioBoxes.Combo();
		this.TDataModeControl = new RadioBoxes.Combo();
		this.ToneCTCSSControl = new RadioBoxes.Combo();
		this.ToneFrequencyControl = new RadioBoxes.Combo();
		this.CTSSFrequencyControl = new RadioBoxes.Combo();
		this.FMModeControl = new RadioBoxes.Combo();
		this.LockoutControl = new RadioBoxes.Combo();
		this.LabelName = new System.Windows.Forms.Label();
		this.NameTextBox = new System.Windows.Forms.TextBox();
		this.DeleteButton = new System.Windows.Forms.Button();
		this.SuspendLayout();
		//
		//LabelMemoryList
		//
		this.LabelMemoryList.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
		this.LabelMemoryList.AutoSize = true;
		this.LabelMemoryList.Location = new System.Drawing.Point(0, 0);
		this.LabelMemoryList.Name = "LabelMemoryList";
		this.LabelMemoryList.Size = new System.Drawing.Size(63, 13);
		this.LabelMemoryList.TabIndex = 0;
		this.LabelMemoryList.Text = "Memory List";
		//
		//MemoryListBox
		//
		this.MemoryListBox.AccessibleName = "memories";
		this.MemoryListBox.AccessibleRole = System.Windows.Forms.AccessibleRole.List;
		this.MemoryListBox.FormattingEnabled = true;
		this.MemoryListBox.Location = new System.Drawing.Point(0, 20);
		this.MemoryListBox.Name = "MemoryListBox";
		this.MemoryListBox.Size = new System.Drawing.Size(120, 199);
		this.MemoryListBox.TabIndex = 1;
		//
		//ChangeButton
		//
		this.ChangeButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
		this.ChangeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.ChangeButton.Location = new System.Drawing.Point(8, 240);
		this.ChangeButton.Name = "ChangeButton";
		this.ChangeButton.Size = new System.Drawing.Size(75, 23);
		this.ChangeButton.TabIndex = 900;
		this.ChangeButton.Text = "Change";
		this.ChangeButton.UseVisualStyleBackColor = true;
		//
		//DoneButton
		//
		this.DoneButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
		this.DoneButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.DoneButton.Location = new System.Drawing.Point(400, 240);
		this.DoneButton.Name = "DoneButton";
		this.DoneButton.Size = new System.Drawing.Size(75, 23);
		this.DoneButton.TabIndex = 990;
		this.DoneButton.Text = "Done";
		this.DoneButton.UseVisualStyleBackColor = true;
		//
		//FrequencyTextBox
		//
		this.FrequencyTextBox.AccessibleDescription = "";
		this.FrequencyTextBox.AccessibleName = "frequency";
		this.FrequencyTextBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
		this.FrequencyTextBox.Location = new System.Drawing.Point(200, 40);
		this.FrequencyTextBox.Name = "FrequencyTextBox";
		this.FrequencyTextBox.Size = new System.Drawing.Size(100, 20);
		this.FrequencyTextBox.TabIndex = 11;
		//
		//LabelReceiveFreq
		//
		this.LabelReceiveFreq.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
		this.LabelReceiveFreq.AutoSize = true;
		this.LabelReceiveFreq.Location = new System.Drawing.Point(200, 25);
		this.LabelReceiveFreq.Name = "LabelReceiveFreq";
		this.LabelReceiveFreq.Size = new System.Drawing.Size(71, 13);
		this.LabelReceiveFreq.TabIndex = 10;
		this.LabelReceiveFreq.Text = "Receive freq.";
		//
		//LabelTransmitFreq
		//
		this.LabelTransmitFreq.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
		this.LabelTransmitFreq.AutoSize = true;
		this.LabelTransmitFreq.Location = new System.Drawing.Point(200, 65);
		this.LabelTransmitFreq.Name = "LabelTransmitFreq";
		this.LabelTransmitFreq.Size = new System.Drawing.Size(71, 13);
		this.LabelTransmitFreq.TabIndex = 100;
		this.LabelTransmitFreq.Text = "Transmit freq.";
		//
		//TFrequencyTextBox
		//
		this.TFrequencyTextBox.AccessibleName = "transmit frequency";
		this.TFrequencyTextBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
		this.TFrequencyTextBox.Location = new System.Drawing.Point(200, 80);
		this.TFrequencyTextBox.Name = "TFrequencyTextBox";
		this.TFrequencyTextBox.Size = new System.Drawing.Size(100, 20);
		this.TFrequencyTextBox.TabIndex = 101;
		//
		//SetFromVFOButton
		//
		this.SetFromVFOButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
		this.SetFromVFOButton.Location = new System.Drawing.Point(200, 240);
		this.SetFromVFOButton.Name = "SetFromVFOButton";
		this.SetFromVFOButton.Size = new System.Drawing.Size(75, 23);
		this.SetFromVFOButton.TabIndex = 910;
		this.SetFromVFOButton.Text = "Set from VFO";
		this.SetFromVFOButton.UseVisualStyleBackColor = true;
		//
		//ModeControl
		//
		this.ModeControl.AccessibleName = "operating mode";
		this.ModeControl.AccessibleRole = System.Windows.Forms.AccessibleRole.ComboBox;
		this.ModeControl.BoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.ModeControl.ExpandedSize = new System.Drawing.Size(50, 100);
		this.ModeControl.Header = "Mode";
		this.ModeControl.Location = new System.Drawing.Point(320, 24);
		this.ModeControl.Name = "ModeControl";
		this.ModeControl.Size = new System.Drawing.Size(50, 36);
		this.ModeControl.SmallSize = new System.Drawing.Size(50, 36);
		this.ModeControl.TabIndex = 20;
		this.ModeControl.TheList = null;
		this.ModeControl.UpdateDisplayFunction = null;
		this.ModeControl.UpdateRigFunction = null;
		//
		//DataModeControl
		//
		this.DataModeControl.AccessibleName = "data mode";
		this.DataModeControl.AccessibleRole = System.Windows.Forms.AccessibleRole.ComboBox;
		this.DataModeControl.BoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.DataModeControl.ExpandedSize = new System.Drawing.Size(50, 100);
		this.DataModeControl.Header = "DMode";
		this.DataModeControl.Location = new System.Drawing.Point(390, 24);
		this.DataModeControl.Name = "DataModeControl";
		this.DataModeControl.Size = new System.Drawing.Size(50, 36);
		this.DataModeControl.SmallSize = new System.Drawing.Size(50, 36);
		this.DataModeControl.TabIndex = 30;
		this.DataModeControl.TheList = null;
		this.DataModeControl.UpdateDisplayFunction = null;
		this.DataModeControl.UpdateRigFunction = null;
		//
		//SplitControl
		//
		this.SplitControl.AccessibleName = "transceive or split";
		this.SplitControl.AccessibleRole = System.Windows.Forms.AccessibleRole.ComboBox;
		this.SplitControl.BoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.SplitControl.ExpandedSize = new System.Drawing.Size(60, 80);
		this.SplitControl.Header = "split";
		this.SplitControl.Location = new System.Drawing.Point(460, 24);
		this.SplitControl.Name = "SplitControl";
		this.SplitControl.Size = new System.Drawing.Size(60, 36);
		this.SplitControl.SmallSize = new System.Drawing.Size(60, 36);
		this.SplitControl.TabIndex = 50;
		this.SplitControl.TheList = null;
		this.SplitControl.UpdateDisplayFunction = null;
		this.SplitControl.UpdateRigFunction = null;
		//
		//TModeControl
		//
		this.TModeControl.AccessibleName = "transmit mode";
		this.TModeControl.AccessibleRole = System.Windows.Forms.AccessibleRole.ComboBox;
		this.TModeControl.BoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.TModeControl.ExpandedSize = new System.Drawing.Size(50, 100);
		this.TModeControl.Header = "TMode";
		this.TModeControl.Location = new System.Drawing.Point(320, 64);
		this.TModeControl.Name = "TModeControl";
		this.TModeControl.Size = new System.Drawing.Size(50, 36);
		this.TModeControl.SmallSize = new System.Drawing.Size(50, 36);
		this.TModeControl.TabIndex = 110;
		this.TModeControl.TheList = null;
		this.TModeControl.UpdateDisplayFunction = null;
		this.TModeControl.UpdateRigFunction = null;
		//
		//TDataModeControl
		//
		this.TDataModeControl.AccessibleName = "transmit datamode";
		this.TDataModeControl.AccessibleRole = System.Windows.Forms.AccessibleRole.ComboBox;
		this.TDataModeControl.BoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.TDataModeControl.ExpandedSize = new System.Drawing.Size(50, 80);
		this.TDataModeControl.Header = "TDMode";
		this.TDataModeControl.Location = new System.Drawing.Point(390, 64);
		this.TDataModeControl.Name = "TDataModeControl";
		this.TDataModeControl.Size = new System.Drawing.Size(50, 36);
		this.TDataModeControl.SmallSize = new System.Drawing.Size(50, 36);
		this.TDataModeControl.TabIndex = 120;
		this.TDataModeControl.TheList = null;
		this.TDataModeControl.UpdateDisplayFunction = null;
		this.TDataModeControl.UpdateRigFunction = null;
		//
		//ToneCTCSSControl
		//
		this.ToneCTCSSControl.AccessibleName = "tone/CTSS";
		this.ToneCTCSSControl.AccessibleRole = System.Windows.Forms.AccessibleRole.ComboBox;
		this.ToneCTCSSControl.BoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.ToneCTCSSControl.ExpandedSize = new System.Drawing.Size(70, 100);
		this.ToneCTCSSControl.Header = "Tone/CTSS";
		this.ToneCTCSSControl.Location = new System.Drawing.Point(200, 104);
		this.ToneCTCSSControl.Name = "ToneCTCSSControl";
		this.ToneCTCSSControl.Size = new System.Drawing.Size(70, 36);
		this.ToneCTCSSControl.SmallSize = new System.Drawing.Size(70, 36);
		this.ToneCTCSSControl.TabIndex = 200;
		this.ToneCTCSSControl.TheList = null;
		this.ToneCTCSSControl.UpdateDisplayFunction = null;
		this.ToneCTCSSControl.UpdateRigFunction = null;
		//
		//ToneFrequencyControl
		//
		this.ToneFrequencyControl.AccessibleName = "tone frequency";
		this.ToneFrequencyControl.AccessibleRole = System.Windows.Forms.AccessibleRole.ComboBox;
		this.ToneFrequencyControl.BoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.ToneFrequencyControl.ExpandedSize = new System.Drawing.Size(80, 100);
		this.ToneFrequencyControl.Header = "Tone Freq";
		this.ToneFrequencyControl.Location = new System.Drawing.Point(290, 104);
		this.ToneFrequencyControl.Name = "ToneFrequencyControl";
		this.ToneFrequencyControl.Size = new System.Drawing.Size(80, 36);
		this.ToneFrequencyControl.SmallSize = new System.Drawing.Size(80, 36);
		this.ToneFrequencyControl.TabIndex = 210;
		this.ToneFrequencyControl.TheList = null;
		this.ToneFrequencyControl.UpdateDisplayFunction = null;
		this.ToneFrequencyControl.UpdateRigFunction = null;
		//
		//CTSSFrequencyControl
		//
		this.CTSSFrequencyControl.AccessibleName = "CTSS frequency";
		this.CTSSFrequencyControl.AccessibleRole = System.Windows.Forms.AccessibleRole.ComboBox;
		this.CTSSFrequencyControl.BoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.CTSSFrequencyControl.ExpandedSize = new System.Drawing.Size(80, 100);
		this.CTSSFrequencyControl.Header = "CTSS Freq";
		this.CTSSFrequencyControl.Location = new System.Drawing.Point(390, 104);
		this.CTSSFrequencyControl.Name = "CTSSFrequencyControl";
		this.CTSSFrequencyControl.Size = new System.Drawing.Size(80, 36);
		this.CTSSFrequencyControl.SmallSize = new System.Drawing.Size(80, 36);
		this.CTSSFrequencyControl.TabIndex = 220;
		this.CTSSFrequencyControl.TheList = null;
		this.CTSSFrequencyControl.UpdateDisplayFunction = null;
		this.CTSSFrequencyControl.UpdateRigFunction = null;
		//
		//FMModeControl
		//
		this.FMModeControl.AccessibleName = "FM mode";
		this.FMModeControl.AccessibleRole = System.Windows.Forms.AccessibleRole.ComboBox;
		this.FMModeControl.BoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.FMModeControl.ExpandedSize = new System.Drawing.Size(70, 80);
		this.FMModeControl.Header = "FM Mode";
		this.FMModeControl.Location = new System.Drawing.Point(200, 144);
		this.FMModeControl.Name = "FMModeControl";
		this.FMModeControl.Size = new System.Drawing.Size(70, 36);
		this.FMModeControl.SmallSize = new System.Drawing.Size(70, 36);
		this.FMModeControl.TabIndex = 300;
		this.FMModeControl.TheList = null;
		this.FMModeControl.UpdateDisplayFunction = null;
		this.FMModeControl.UpdateRigFunction = null;
		//
		//LockoutControl
		//
		this.LockoutControl.AccessibleName = "lockout";
		this.LockoutControl.AccessibleRole = System.Windows.Forms.AccessibleRole.ComboBox;
		this.LockoutControl.BoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.LockoutControl.ExpandedSize = new System.Drawing.Size(70, 80);
		this.LockoutControl.Header = "Lockout";
		this.LockoutControl.Location = new System.Drawing.Point(290, 144);
		this.LockoutControl.Name = "LockoutControl";
		this.LockoutControl.Size = new System.Drawing.Size(70, 36);
		this.LockoutControl.SmallSize = new System.Drawing.Size(70, 36);
		this.LockoutControl.TabIndex = 310;
		this.LockoutControl.TheList = null;
		this.LockoutControl.UpdateDisplayFunction = null;
		this.LockoutControl.UpdateRigFunction = null;
		//
		//LabelName
		//
		this.LabelName.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
		this.LabelName.AutoSize = true;
		this.LabelName.Location = new System.Drawing.Point(200, 185);
		this.LabelName.Name = "LabelName";
		this.LabelName.Size = new System.Drawing.Size(38, 13);
		this.LabelName.TabIndex = 400;
		this.LabelName.Text = "Name:";
		//
		//NameTextBox
		//
		this.NameTextBox.AccessibleName = "memory name";
		this.NameTextBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
		this.NameTextBox.Location = new System.Drawing.Point(250, 185);
		this.NameTextBox.Name = "NameTextBox";
		this.NameTextBox.Size = new System.Drawing.Size(100, 20);
		this.NameTextBox.TabIndex = 401;
		//
		//DeleteButton
		//
		this.DeleteButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
		this.DeleteButton.Location = new System.Drawing.Point(300, 240);
		this.DeleteButton.Name = "DeleteButton";
		this.DeleteButton.Size = new System.Drawing.Size(75, 23);
		this.DeleteButton.TabIndex = 920;
		this.DeleteButton.Text = "Delete";
		this.DeleteButton.UseVisualStyleBackColor = true;
		//
		//Memories
		//
		this.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.CancelButton = this.DoneButton;
		this.ClientSize = new System.Drawing.Size(584, 264);
		this.Controls.Add(this.DeleteButton);
		this.Controls.Add(this.NameTextBox);
		this.Controls.Add(this.LabelName);
		this.Controls.Add(this.LockoutControl);
		this.Controls.Add(this.FMModeControl);
		this.Controls.Add(this.CTSSFrequencyControl);
		this.Controls.Add(this.ToneFrequencyControl);
		this.Controls.Add(this.ToneCTCSSControl);
		this.Controls.Add(this.TDataModeControl);
		this.Controls.Add(this.TModeControl);
		this.Controls.Add(this.SplitControl);
		this.Controls.Add(this.DataModeControl);
		this.Controls.Add(this.ModeControl);
		this.Controls.Add(this.SetFromVFOButton);
		this.Controls.Add(this.TFrequencyTextBox);
		this.Controls.Add(this.LabelTransmitFreq);
		this.Controls.Add(this.LabelReceiveFreq);
		this.Controls.Add(this.FrequencyTextBox);
		this.Controls.Add(this.DoneButton);
		this.Controls.Add(this.ChangeButton);
		this.Controls.Add(this.MemoryListBox);
		this.Controls.Add(this.LabelMemoryList);
		this.Name = "Memories";
		this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Memories";
		this.ResumeLayout(false);
		this.PerformLayout();

	}
	internal System.Windows.Forms.Label LabelMemoryList;
	internal System.Windows.Forms.ListBox MemoryListBox;
	internal System.Windows.Forms.Button ChangeButton;
	internal System.Windows.Forms.Button DoneButton;
	internal System.Windows.Forms.TextBox FrequencyTextBox;
	internal System.Windows.Forms.Label LabelReceiveFreq;
	internal System.Windows.Forms.Label LabelTransmitFreq;
	internal System.Windows.Forms.TextBox TFrequencyTextBox;
	internal System.Windows.Forms.Button SetFromVFOButton;
	internal RadioBoxes.Combo ModeControl;
	internal RadioBoxes.Combo DataModeControl;
	internal RadioBoxes.Combo SplitControl;
	internal RadioBoxes.Combo TModeControl;
	internal RadioBoxes.Combo TDataModeControl;
	internal RadioBoxes.Combo ToneCTCSSControl;
	internal RadioBoxes.Combo ToneFrequencyControl;
	internal RadioBoxes.Combo CTSSFrequencyControl;
	internal RadioBoxes.Combo FMModeControl;
	internal RadioBoxes.Combo LockoutControl;
	internal System.Windows.Forms.Label LabelName;
	internal System.Windows.Forms.TextBox NameTextBox;
	internal System.Windows.Forms.Button DeleteButton;
	public Memories()
	{
		InitializeComponent();
	}
}
