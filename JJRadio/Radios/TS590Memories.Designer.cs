using RadioBoxes;

namespace Radios
{
    partial class TS590Memories
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.MemoryListLabel = new System.Windows.Forms.Label();
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
            this.FMWidthControl = new RadioBoxes.Combo();
            this.LockoutControl = new RadioBoxes.Combo();
            this.LabelName = new System.Windows.Forms.Label();
            this.NameTextBox = new System.Windows.Forms.TextBox();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.EmptyNonemptyButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // MemoryListLabel
            // 
            this.MemoryListLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.MemoryListLabel.AutoSize = true;
            this.MemoryListLabel.Location = new System.Drawing.Point(0, 25);
            this.MemoryListLabel.Name = "MemoryListLabel";
            this.MemoryListLabel.Size = new System.Drawing.Size(59, 13);
            this.MemoryListLabel.TabIndex = 5;
            this.MemoryListLabel.Text = "Memory list";
            // 
            // MemoryListBox
            // 
            this.MemoryListBox.AccessibleName = "memories";
            this.MemoryListBox.AccessibleRole = System.Windows.Forms.AccessibleRole.List;
            this.MemoryListBox.FormattingEnabled = true;
            this.MemoryListBox.Location = new System.Drawing.Point(0, 45);
            this.MemoryListBox.Name = "MemoryListBox";
            this.MemoryListBox.Size = new System.Drawing.Size(120, 199);
            this.MemoryListBox.TabIndex = 6;
            this.MemoryListBox.SelectedIndexChanged += new System.EventHandler(this.MemoryListBox_SelectedIndexChanged);
            this.MemoryListBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MemoryListBox_KeyPress);
            // 
            // ChangeButton
            // 
            this.ChangeButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.ChangeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ChangeButton.Location = new System.Drawing.Point(8, 265);
            this.ChangeButton.Name = "ChangeButton";
            this.ChangeButton.Size = new System.Drawing.Size(75, 23);
            this.ChangeButton.TabIndex = 900;
            this.ChangeButton.Text = "Change";
            this.ChangeButton.UseVisualStyleBackColor = true;
            this.ChangeButton.Click += new System.EventHandler(this.ChangeButton_Click);
            // 
            // DoneButton
            // 
            this.DoneButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.DoneButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.DoneButton.Location = new System.Drawing.Point(400, 265);
            this.DoneButton.Name = "DoneButton";
            this.DoneButton.Size = new System.Drawing.Size(75, 23);
            this.DoneButton.TabIndex = 990;
            this.DoneButton.Text = "Done";
            this.DoneButton.UseVisualStyleBackColor = true;
            this.DoneButton.Click += new System.EventHandler(this.DoneButton_Click);
            // 
            // FrequencyTextBox
            // 
            this.FrequencyTextBox.AccessibleDescription = "";
            this.FrequencyTextBox.AccessibleName = "frequency";
            this.FrequencyTextBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.FrequencyTextBox.Location = new System.Drawing.Point(200, 65);
            this.FrequencyTextBox.Name = "FrequencyTextBox";
            this.FrequencyTextBox.Size = new System.Drawing.Size(100, 20);
            this.FrequencyTextBox.TabIndex = 11;
            // 
            // LabelReceiveFreq
            // 
            this.LabelReceiveFreq.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.LabelReceiveFreq.AutoSize = true;
            this.LabelReceiveFreq.Location = new System.Drawing.Point(200, 50);
            this.LabelReceiveFreq.Name = "LabelReceiveFreq";
            this.LabelReceiveFreq.Size = new System.Drawing.Size(71, 13);
            this.LabelReceiveFreq.TabIndex = 10;
            this.LabelReceiveFreq.Text = "Receive freq.";
            // 
            // LabelTransmitFreq
            // 
            this.LabelTransmitFreq.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.LabelTransmitFreq.AutoSize = true;
            this.LabelTransmitFreq.Location = new System.Drawing.Point(200, 90);
            this.LabelTransmitFreq.Name = "LabelTransmitFreq";
            this.LabelTransmitFreq.Size = new System.Drawing.Size(71, 13);
            this.LabelTransmitFreq.TabIndex = 100;
            this.LabelTransmitFreq.Text = "Transmit freq.";
            // 
            // TFrequencyTextBox
            // 
            this.TFrequencyTextBox.AccessibleName = "transmit frequency";
            this.TFrequencyTextBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.TFrequencyTextBox.Location = new System.Drawing.Point(200, 105);
            this.TFrequencyTextBox.Name = "TFrequencyTextBox";
            this.TFrequencyTextBox.Size = new System.Drawing.Size(100, 20);
            this.TFrequencyTextBox.TabIndex = 101;
            // 
            // SetFromVFOButton
            // 
            this.SetFromVFOButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.SetFromVFOButton.Location = new System.Drawing.Point(200, 265);
            this.SetFromVFOButton.Name = "SetFromVFOButton";
            this.SetFromVFOButton.Size = new System.Drawing.Size(75, 23);
            this.SetFromVFOButton.TabIndex = 910;
            this.SetFromVFOButton.Text = "Set from VFO";
            this.SetFromVFOButton.UseVisualStyleBackColor = true;
            this.SetFromVFOButton.Click += new System.EventHandler(this.SetFromVFOButton_Click);
            // 
            // ModeControl
            // 
            this.ModeControl.BoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ModeControl.ExpandedSize = new System.Drawing.Size(50, 100);
            this.ModeControl.Header = "Mode";
            this.ModeControl.Location = new System.Drawing.Point(320, 49);
            this.ModeControl.Name = "ModeControl";
            this.ModeControl.ReadOnly = false;
            this.ModeControl.Size = new System.Drawing.Size(50, 36);
            this.ModeControl.SmallSize = new System.Drawing.Size(50, 36);
            this.ModeControl.TabIndex = 20;
            this.ModeControl.Tag = "Mode";
            this.ModeControl.TheList = null;
            this.ModeControl.UpdateDisplayFunction = null;
            this.ModeControl.UpdateRigFunction = null;
            // 
            // DataModeControl
            // 
            this.DataModeControl.BoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DataModeControl.ExpandedSize = new System.Drawing.Size(50, 100);
            this.DataModeControl.Header = "DMode";
            this.DataModeControl.Location = new System.Drawing.Point(390, 49);
            this.DataModeControl.Name = "DataModeControl";
            this.DataModeControl.ReadOnly = false;
            this.DataModeControl.Size = new System.Drawing.Size(50, 36);
            this.DataModeControl.SmallSize = new System.Drawing.Size(50, 36);
            this.DataModeControl.TabIndex = 30;
            this.DataModeControl.Tag = "DMode";
            this.DataModeControl.TheList = null;
            this.DataModeControl.UpdateDisplayFunction = null;
            this.DataModeControl.UpdateRigFunction = null;
            // 
            // SplitControl
            // 
            this.SplitControl.BoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SplitControl.ExpandedSize = new System.Drawing.Size(60, 80);
            this.SplitControl.Header = "split";
            this.SplitControl.Location = new System.Drawing.Point(460, 49);
            this.SplitControl.Name = "SplitControl";
            this.SplitControl.ReadOnly = false;
            this.SplitControl.Size = new System.Drawing.Size(60, 36);
            this.SplitControl.SmallSize = new System.Drawing.Size(60, 36);
            this.SplitControl.TabIndex = 50;
            this.SplitControl.Tag = "split";
            this.SplitControl.TheList = null;
            this.SplitControl.UpdateDisplayFunction = null;
            this.SplitControl.UpdateRigFunction = null;
            // 
            // TModeControl
            // 
            this.TModeControl.BoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TModeControl.ExpandedSize = new System.Drawing.Size(50, 100);
            this.TModeControl.Header = "TMode";
            this.TModeControl.Location = new System.Drawing.Point(320, 89);
            this.TModeControl.Name = "TModeControl";
            this.TModeControl.ReadOnly = false;
            this.TModeControl.Size = new System.Drawing.Size(50, 36);
            this.TModeControl.SmallSize = new System.Drawing.Size(50, 36);
            this.TModeControl.TabIndex = 110;
            this.TModeControl.Tag = "TMode";
            this.TModeControl.TheList = null;
            this.TModeControl.UpdateDisplayFunction = null;
            this.TModeControl.UpdateRigFunction = null;
            // 
            // TDataModeControl
            // 
            this.TDataModeControl.BoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TDataModeControl.ExpandedSize = new System.Drawing.Size(50, 80);
            this.TDataModeControl.Header = "TDMode";
            this.TDataModeControl.Location = new System.Drawing.Point(390, 89);
            this.TDataModeControl.Name = "TDataModeControl";
            this.TDataModeControl.ReadOnly = false;
            this.TDataModeControl.Size = new System.Drawing.Size(50, 36);
            this.TDataModeControl.SmallSize = new System.Drawing.Size(50, 36);
            this.TDataModeControl.TabIndex = 120;
            this.TDataModeControl.Tag = "TDMode";
            this.TDataModeControl.TheList = null;
            this.TDataModeControl.UpdateDisplayFunction = null;
            this.TDataModeControl.UpdateRigFunction = null;
            // 
            // ToneCTCSSControl
            // 
            this.ToneCTCSSControl.BoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ToneCTCSSControl.ExpandedSize = new System.Drawing.Size(70, 100);
            this.ToneCTCSSControl.Header = "Tone/CTSS";
            this.ToneCTCSSControl.Location = new System.Drawing.Point(200, 129);
            this.ToneCTCSSControl.Name = "ToneCTCSSControl";
            this.ToneCTCSSControl.ReadOnly = false;
            this.ToneCTCSSControl.Size = new System.Drawing.Size(70, 36);
            this.ToneCTCSSControl.SmallSize = new System.Drawing.Size(70, 36);
            this.ToneCTCSSControl.TabIndex = 200;
            this.ToneCTCSSControl.Tag = "Tone/CTSS";
            this.ToneCTCSSControl.TheList = null;
            this.ToneCTCSSControl.UpdateDisplayFunction = null;
            this.ToneCTCSSControl.UpdateRigFunction = null;
            // 
            // ToneFrequencyControl
            // 
            this.ToneFrequencyControl.BoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ToneFrequencyControl.ExpandedSize = new System.Drawing.Size(80, 100);
            this.ToneFrequencyControl.Header = "Tone Freq";
            this.ToneFrequencyControl.Location = new System.Drawing.Point(290, 129);
            this.ToneFrequencyControl.Name = "ToneFrequencyControl";
            this.ToneFrequencyControl.ReadOnly = false;
            this.ToneFrequencyControl.Size = new System.Drawing.Size(80, 36);
            this.ToneFrequencyControl.SmallSize = new System.Drawing.Size(80, 36);
            this.ToneFrequencyControl.TabIndex = 210;
            this.ToneFrequencyControl.Tag = "Tone Freq";
            this.ToneFrequencyControl.TheList = null;
            this.ToneFrequencyControl.UpdateDisplayFunction = null;
            this.ToneFrequencyControl.UpdateRigFunction = null;
            // 
            // CTSSFrequencyControl
            // 
            this.CTSSFrequencyControl.BoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CTSSFrequencyControl.ExpandedSize = new System.Drawing.Size(80, 100);
            this.CTSSFrequencyControl.Header = "CTSS Freq";
            this.CTSSFrequencyControl.Location = new System.Drawing.Point(390, 129);
            this.CTSSFrequencyControl.Name = "CTSSFrequencyControl";
            this.CTSSFrequencyControl.ReadOnly = false;
            this.CTSSFrequencyControl.Size = new System.Drawing.Size(80, 36);
            this.CTSSFrequencyControl.SmallSize = new System.Drawing.Size(80, 36);
            this.CTSSFrequencyControl.TabIndex = 220;
            this.CTSSFrequencyControl.Tag = "CTSS Freq";
            this.CTSSFrequencyControl.TheList = null;
            this.CTSSFrequencyControl.UpdateDisplayFunction = null;
            this.CTSSFrequencyControl.UpdateRigFunction = null;
            // 
            // FMWidthControl
            // 
            this.FMWidthControl.BoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FMWidthControl.ExpandedSize = new System.Drawing.Size(70, 80);
            this.FMWidthControl.Header = "FM Mode";
            this.FMWidthControl.Location = new System.Drawing.Point(200, 169);
            this.FMWidthControl.Name = "FMWidthControl";
            this.FMWidthControl.ReadOnly = false;
            this.FMWidthControl.Size = new System.Drawing.Size(70, 36);
            this.FMWidthControl.SmallSize = new System.Drawing.Size(70, 36);
            this.FMWidthControl.TabIndex = 300;
            this.FMWidthControl.Tag = "FM Mode";
            this.FMWidthControl.TheList = null;
            this.FMWidthControl.UpdateDisplayFunction = null;
            this.FMWidthControl.UpdateRigFunction = null;
            // 
            // LockoutControl
            // 
            this.LockoutControl.BoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LockoutControl.ExpandedSize = new System.Drawing.Size(70, 80);
            this.LockoutControl.Header = "Lockout";
            this.LockoutControl.Location = new System.Drawing.Point(290, 169);
            this.LockoutControl.Name = "LockoutControl";
            this.LockoutControl.ReadOnly = false;
            this.LockoutControl.Size = new System.Drawing.Size(70, 36);
            this.LockoutControl.SmallSize = new System.Drawing.Size(70, 36);
            this.LockoutControl.TabIndex = 310;
            this.LockoutControl.Tag = "Lockout";
            this.LockoutControl.TheList = null;
            this.LockoutControl.UpdateDisplayFunction = null;
            this.LockoutControl.UpdateRigFunction = null;
            // 
            // LabelName
            // 
            this.LabelName.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.LabelName.AutoSize = true;
            this.LabelName.Location = new System.Drawing.Point(200, 210);
            this.LabelName.Name = "LabelName";
            this.LabelName.Size = new System.Drawing.Size(38, 13);
            this.LabelName.TabIndex = 400;
            this.LabelName.Text = "Name:";
            // 
            // NameTextBox
            // 
            this.NameTextBox.AccessibleName = "memory name";
            this.NameTextBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.NameTextBox.Location = new System.Drawing.Point(250, 210);
            this.NameTextBox.Name = "NameTextBox";
            this.NameTextBox.Size = new System.Drawing.Size(100, 20);
            this.NameTextBox.TabIndex = 401;
            // 
            // DeleteButton
            // 
            this.DeleteButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.DeleteButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.DeleteButton.Location = new System.Drawing.Point(300, 265);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(75, 23);
            this.DeleteButton.TabIndex = 920;
            this.DeleteButton.Text = "Delete";
            this.DeleteButton.UseVisualStyleBackColor = true;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // EmptyNonemptyButton
            // 
            this.EmptyNonemptyButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.EmptyNonemptyButton.AutoSize = true;
            this.EmptyNonemptyButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.EmptyNonemptyButton.Location = new System.Drawing.Point(8, 0);
            this.EmptyNonemptyButton.Name = "EmptyNonemptyButton";
            this.EmptyNonemptyButton.Size = new System.Drawing.Size(6, 6);
            this.EmptyNonemptyButton.TabIndex = 2;
            this.EmptyNonemptyButton.UseVisualStyleBackColor = true;
            this.EmptyNonemptyButton.Click += new System.EventHandler(this.EmptyNonemptyButton_Click);
            // 
            // TS590Memories
            // 
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.DoneButton;
            this.ClientSize = new System.Drawing.Size(584, 292);
            this.Controls.Add(this.EmptyNonemptyButton);
            this.Controls.Add(this.MemoryListLabel);
            this.Controls.Add(this.DeleteButton);
            this.Controls.Add(this.NameTextBox);
            this.Controls.Add(this.LabelName);
            this.Controls.Add(this.LockoutControl);
            this.Controls.Add(this.FMWidthControl);
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
            this.Name = "TS590Memories";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TS590 Memories";
            this.Activated += new System.EventHandler(this.Memories_Activated);
            this.Load += new System.EventHandler(this.TS590Memories_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label MemoryListLabel;
        private System.Windows.Forms.ListBox MemoryListBox;
        private System.Windows.Forms.Button ChangeButton;
        private System.Windows.Forms.Button DoneButton;
        private System.Windows.Forms.TextBox FrequencyTextBox;
        private System.Windows.Forms.Label LabelReceiveFreq;
        private System.Windows.Forms.Label LabelTransmitFreq;
        private System.Windows.Forms.TextBox TFrequencyTextBox;
        private System.Windows.Forms.Button SetFromVFOButton;
        private RadioBoxes.Combo ModeControl;
        private RadioBoxes.Combo DataModeControl;
        private RadioBoxes.Combo SplitControl;
        private RadioBoxes.Combo TModeControl;
        private RadioBoxes.Combo TDataModeControl;
        private RadioBoxes.Combo ToneCTCSSControl;
        private RadioBoxes.Combo ToneFrequencyControl;
        private RadioBoxes.Combo CTSSFrequencyControl;
        private RadioBoxes.Combo FMWidthControl;
        private RadioBoxes.Combo LockoutControl;
        private System.Windows.Forms.Label LabelName;
        private System.Windows.Forms.TextBox NameTextBox;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.Button EmptyNonemptyButton;
    }
}