namespace Radios
{
    partial class FlexMemories
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
            this.MemoryList = new System.Windows.Forms.ListBox();
            this.AddButton = new System.Windows.Forms.Button();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.DoneButton = new System.Windows.Forms.Button();
            this.FreqLabel = new System.Windows.Forms.Label();
            this.FreqBox = new System.Windows.Forms.TextBox();
            this.FilterHighControl = new RadioBoxes.NumberBox();
            this.FilterLowControl = new RadioBoxes.NumberBox();
            this.ModeControl = new RadioBoxes.Combo();
            this.SquelchControl = new RadioBoxes.Combo();
            this.SquelchLevelControl = new RadioBoxes.NumberBox();
            this.PowerControl = new RadioBoxes.NumberBox();
            this.ToneModeControl = new RadioBoxes.Combo();
            this.ToneFrequencyControl = new RadioBoxes.Combo();
            this.OffsetDirectionControl = new RadioBoxes.Combo();
            this.OffsetControl = new RadioBoxes.NumberBox();
            this.NameLabel = new System.Windows.Forms.Label();
            this.NameBox = new System.Windows.Forms.TextBox();
            this.OwnerLabel = new System.Windows.Forms.Label();
            this.OwnerBox = new System.Windows.Forms.TextBox();
            this.GroupLabel = new System.Windows.Forms.Label();
            this.GroupBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // MemoryListLabel
            // 
            this.MemoryListLabel.AutoSize = true;
            this.MemoryListLabel.Location = new System.Drawing.Point(24, 20);
            this.MemoryListLabel.Name = "MemoryListLabel";
            this.MemoryListLabel.Size = new System.Drawing.Size(52, 13);
            this.MemoryListLabel.TabIndex = 10;
            this.MemoryListLabel.Text = "Memories";
            // 
            // MemoryList
            // 
            this.MemoryList.FormattingEnabled = true;
            this.MemoryList.Location = new System.Drawing.Point(0, 40);
            this.MemoryList.Name = "MemoryList";
            this.MemoryList.Size = new System.Drawing.Size(120, 199);
            this.MemoryList.TabIndex = 11;
            this.MemoryList.SelectedIndexChanged += new System.EventHandler(this.MemoryList_SelectedIndexChanged);
            this.MemoryList.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MemoryList_KeyPress);
            // 
            // AddButton
            // 
            this.AddButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.AddButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.AddButton.Location = new System.Drawing.Point(0, 305);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(75, 23);
            this.AddButton.TabIndex = 900;
            this.AddButton.Text = "Add";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // DeleteButton
            // 
            this.DeleteButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.DeleteButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.DeleteButton.Location = new System.Drawing.Point(100, 305);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(75, 23);
            this.DeleteButton.TabIndex = 920;
            this.DeleteButton.Text = "Delete";
            this.DeleteButton.UseVisualStyleBackColor = true;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // DoneButton
            // 
            this.DoneButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.DoneButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.DoneButton.Location = new System.Drawing.Point(200, 305);
            this.DoneButton.Name = "DoneButton";
            this.DoneButton.Size = new System.Drawing.Size(75, 23);
            this.DoneButton.TabIndex = 930;
            this.DoneButton.Text = "Done";
            this.DoneButton.UseVisualStyleBackColor = true;
            this.DoneButton.Click += new System.EventHandler(this.DoneButton_Click);
            // 
            // FreqLabel
            // 
            this.FreqLabel.AutoSize = true;
            this.FreqLabel.Location = new System.Drawing.Point(200, 45);
            this.FreqLabel.Name = "FreqLabel";
            this.FreqLabel.Size = new System.Drawing.Size(57, 13);
            this.FreqLabel.TabIndex = 100;
            this.FreqLabel.Text = "Frequency";
            // 
            // FreqBox
            // 
            this.FreqBox.AcceptsReturn = true;
            this.FreqBox.Location = new System.Drawing.Point(200, 60);
            this.FreqBox.Name = "FreqBox";
            this.FreqBox.Size = new System.Drawing.Size(100, 20);
            this.FreqBox.TabIndex = 101;
            this.FreqBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FreqBox_KeyPress);
            this.FreqBox.Leave += new System.EventHandler(this.FreqBox_Leave);
            // 
            // FilterHighControl
            // 
            this.FilterHighControl.Header = "High";
            this.FilterHighControl.HighValue = 0;
            this.FilterHighControl.Increment = 0;
            this.FilterHighControl.Location = new System.Drawing.Point(270, 90);
            this.FilterHighControl.LowValue = 0;
            this.FilterHighControl.Name = "FilterHighControl";
            this.FilterHighControl.ReadOnly = false;
            this.FilterHighControl.Size = new System.Drawing.Size(50, 36);
            this.FilterHighControl.TabIndex = 210;
            this.FilterHighControl.Tag = "High";
            this.FilterHighControl.UpdateDisplayFunction = null;
            this.FilterHighControl.UpdateRigFunction = null;
            // 
            // FilterLowControl
            // 
            this.FilterLowControl.Header = "Low";
            this.FilterLowControl.HighValue = 0;
            this.FilterLowControl.Increment = 0;
            this.FilterLowControl.Location = new System.Drawing.Point(200, 90);
            this.FilterLowControl.LowValue = 0;
            this.FilterLowControl.Name = "FilterLowControl";
            this.FilterLowControl.ReadOnly = false;
            this.FilterLowControl.Size = new System.Drawing.Size(50, 36);
            this.FilterLowControl.TabIndex = 200;
            this.FilterLowControl.Tag = "Low";
            this.FilterLowControl.UpdateDisplayFunction = null;
            this.FilterLowControl.UpdateRigFunction = null;
            // 
            // ModeControl
            // 
            this.ModeControl.BoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ModeControl.ExpandedSize = new System.Drawing.Size(50, 80);
            this.ModeControl.Header = "Mode";
            this.ModeControl.Location = new System.Drawing.Point(340, 44);
            this.ModeControl.Name = "ModeControl";
            this.ModeControl.ReadOnly = false;
            this.ModeControl.Size = new System.Drawing.Size(50, 36);
            this.ModeControl.SmallSize = new System.Drawing.Size(50, 36);
            this.ModeControl.TabIndex = 110;
            this.ModeControl.Tag = "Mode";
            this.ModeControl.TheList = null;
            this.ModeControl.UpdateDisplayFunction = null;
            this.ModeControl.UpdateRigFunction = null;
            // 
            // SquelchControl
            // 
            this.SquelchControl.BoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SquelchControl.ExpandedSize = new System.Drawing.Size(50, 56);
            this.SquelchControl.Header = "Squelch";
            this.SquelchControl.Location = new System.Drawing.Point(200, 170);
            this.SquelchControl.Name = "SquelchControl";
            this.SquelchControl.ReadOnly = false;
            this.SquelchControl.Size = new System.Drawing.Size(50, 36);
            this.SquelchControl.SmallSize = new System.Drawing.Size(50, 36);
            this.SquelchControl.TabIndex = 400;
            this.SquelchControl.Tag = "Squelch";
            this.SquelchControl.TheList = null;
            this.SquelchControl.UpdateDisplayFunction = null;
            this.SquelchControl.UpdateRigFunction = null;
            // 
            // SquelchLevelControl
            // 
            this.SquelchLevelControl.Header = "Sq level";
            this.SquelchLevelControl.HighValue = 0;
            this.SquelchLevelControl.Increment = 0;
            this.SquelchLevelControl.Location = new System.Drawing.Point(270, 170);
            this.SquelchLevelControl.LowValue = 0;
            this.SquelchLevelControl.Name = "SquelchLevelControl";
            this.SquelchLevelControl.ReadOnly = false;
            this.SquelchLevelControl.Size = new System.Drawing.Size(50, 36);
            this.SquelchLevelControl.TabIndex = 410;
            this.SquelchLevelControl.Tag = "Sq level";
            this.SquelchLevelControl.UpdateDisplayFunction = null;
            this.SquelchLevelControl.UpdateRigFunction = null;
            // 
            // PowerControl
            // 
            this.PowerControl.Header = "Power";
            this.PowerControl.HighValue = 0;
            this.PowerControl.Increment = 0;
            this.PowerControl.Location = new System.Drawing.Point(340, 90);
            this.PowerControl.LowValue = 0;
            this.PowerControl.Name = "PowerControl";
            this.PowerControl.ReadOnly = false;
            this.PowerControl.Size = new System.Drawing.Size(50, 36);
            this.PowerControl.TabIndex = 220;
            this.PowerControl.Tag = "Power";
            this.PowerControl.UpdateDisplayFunction = null;
            this.PowerControl.UpdateRigFunction = null;
            // 
            // ToneModeControl
            // 
            this.ToneModeControl.BoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ToneModeControl.ExpandedSize = new System.Drawing.Size(60, 56);
            this.ToneModeControl.Header = "ToneMode";
            this.ToneModeControl.Location = new System.Drawing.Point(200, 130);
            this.ToneModeControl.Name = "ToneModeControl";
            this.ToneModeControl.ReadOnly = false;
            this.ToneModeControl.Size = new System.Drawing.Size(60, 36);
            this.ToneModeControl.SmallSize = new System.Drawing.Size(60, 36);
            this.ToneModeControl.TabIndex = 300;
            this.ToneModeControl.Tag = "ToneMode";
            this.ToneModeControl.TheList = null;
            this.ToneModeControl.UpdateDisplayFunction = null;
            this.ToneModeControl.UpdateRigFunction = null;
            // 
            // ToneFrequencyControl
            // 
            this.ToneFrequencyControl.BoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ToneFrequencyControl.ExpandedSize = new System.Drawing.Size(50, 86);
            this.ToneFrequencyControl.Header = "ToneFreq";
            this.ToneFrequencyControl.Location = new System.Drawing.Point(270, 130);
            this.ToneFrequencyControl.Name = "ToneFrequencyControl";
            this.ToneFrequencyControl.ReadOnly = false;
            this.ToneFrequencyControl.Size = new System.Drawing.Size(50, 36);
            this.ToneFrequencyControl.SmallSize = new System.Drawing.Size(50, 36);
            this.ToneFrequencyControl.TabIndex = 310;
            this.ToneFrequencyControl.Tag = "ToneFreq";
            this.ToneFrequencyControl.TheList = null;
            this.ToneFrequencyControl.UpdateDisplayFunction = null;
            this.ToneFrequencyControl.UpdateRigFunction = null;
            // 
            // OffsetDirectionControl
            // 
            this.OffsetDirectionControl.BoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.OffsetDirectionControl.ExpandedSize = new System.Drawing.Size(50, 56);
            this.OffsetDirectionControl.Header = "OffDir";
            this.OffsetDirectionControl.Location = new System.Drawing.Point(340, 170);
            this.OffsetDirectionControl.Name = "OffsetDirectionControl";
            this.OffsetDirectionControl.ReadOnly = false;
            this.OffsetDirectionControl.Size = new System.Drawing.Size(50, 36);
            this.OffsetDirectionControl.SmallSize = new System.Drawing.Size(50, 36);
            this.OffsetDirectionControl.TabIndex = 420;
            this.OffsetDirectionControl.Tag = "OffDir";
            this.OffsetDirectionControl.TheList = null;
            this.OffsetDirectionControl.UpdateDisplayFunction = null;
            this.OffsetDirectionControl.UpdateRigFunction = null;
            // 
            // OffsetControl
            // 
            this.OffsetControl.Header = "Offset";
            this.OffsetControl.HighValue = 0;
            this.OffsetControl.Increment = 0;
            this.OffsetControl.Location = new System.Drawing.Point(410, 170);
            this.OffsetControl.LowValue = 0;
            this.OffsetControl.Name = "OffsetControl";
            this.OffsetControl.ReadOnly = false;
            this.OffsetControl.Size = new System.Drawing.Size(50, 36);
            this.OffsetControl.TabIndex = 430;
            this.OffsetControl.Tag = "Offset";
            this.OffsetControl.UpdateDisplayFunction = null;
            this.OffsetControl.UpdateRigFunction = null;
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.Location = new System.Drawing.Point(200, 220);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(41, 13);
            this.NameLabel.TabIndex = 500;
            this.NameLabel.Text = "Name: ";
            // 
            // NameBox
            // 
            this.NameBox.AccessibleName = "Name";
            this.NameBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.NameBox.Location = new System.Drawing.Point(241, 220);
            this.NameBox.Name = "NameBox";
            this.NameBox.Size = new System.Drawing.Size(70, 20);
            this.NameBox.TabIndex = 501;
            this.NameBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.NameBox_KeyPress);
            this.NameBox.Leave += new System.EventHandler(this.NameBox_Change);
            // 
            // OwnerLabel
            // 
            this.OwnerLabel.AutoSize = true;
            this.OwnerLabel.Location = new System.Drawing.Point(330, 220);
            this.OwnerLabel.Name = "OwnerLabel";
            this.OwnerLabel.Size = new System.Drawing.Size(44, 13);
            this.OwnerLabel.TabIndex = 510;
            this.OwnerLabel.Text = "Owner: ";
            // 
            // OwnerBox
            // 
            this.OwnerBox.AccessibleName = "Owner";
            this.OwnerBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.OwnerBox.Location = new System.Drawing.Point(374, 220);
            this.OwnerBox.Name = "OwnerBox";
            this.OwnerBox.Size = new System.Drawing.Size(70, 20);
            this.OwnerBox.TabIndex = 511;
            this.OwnerBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OwnerBox_KeyPress);
            this.OwnerBox.Leave += new System.EventHandler(this.OwnerBox_Change);
            // 
            // GroupLabel
            // 
            this.GroupLabel.AutoSize = true;
            this.GroupLabel.Location = new System.Drawing.Point(460, 220);
            this.GroupLabel.Name = "GroupLabel";
            this.GroupLabel.Size = new System.Drawing.Size(42, 13);
            this.GroupLabel.TabIndex = 520;
            this.GroupLabel.Text = "Group: ";
            // 
            // GroupBox
            // 
            this.GroupBox.AccessibleName = "Group";
            this.GroupBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.GroupBox.Location = new System.Drawing.Point(502, 220);
            this.GroupBox.Name = "GroupBox";
            this.GroupBox.Size = new System.Drawing.Size(70, 20);
            this.GroupBox.TabIndex = 521;
            this.GroupBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.GroupBox_KeyPress);
            this.GroupBox.Leave += new System.EventHandler(this.GroupBox_Change);
            // 
            // FlexMemories
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.DoneButton;
            this.ClientSize = new System.Drawing.Size(584, 332);
            this.Controls.Add(this.GroupBox);
            this.Controls.Add(this.GroupLabel);
            this.Controls.Add(this.OwnerBox);
            this.Controls.Add(this.OwnerLabel);
            this.Controls.Add(this.NameBox);
            this.Controls.Add(this.NameLabel);
            this.Controls.Add(this.OffsetControl);
            this.Controls.Add(this.OffsetDirectionControl);
            this.Controls.Add(this.ToneFrequencyControl);
            this.Controls.Add(this.ToneModeControl);
            this.Controls.Add(this.PowerControl);
            this.Controls.Add(this.SquelchLevelControl);
            this.Controls.Add(this.SquelchControl);
            this.Controls.Add(this.FilterHighControl);
            this.Controls.Add(this.FilterLowControl);
            this.Controls.Add(this.ModeControl);
            this.Controls.Add(this.FreqBox);
            this.Controls.Add(this.FreqLabel);
            this.Controls.Add(this.DoneButton);
            this.Controls.Add(this.DeleteButton);
            this.Controls.Add(this.AddButton);
            this.Controls.Add(this.MemoryList);
            this.Controls.Add(this.MemoryListLabel);
            this.Name = "FlexMemories";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Flex Memories";
            this.Activated += new System.EventHandler(this.FlexMemories_Activated);
            this.Load += new System.EventHandler(this.FlexMemories_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label MemoryListLabel;
        private System.Windows.Forms.ListBox MemoryList;
        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.Button DoneButton;
        private System.Windows.Forms.Label FreqLabel;
        private System.Windows.Forms.TextBox FreqBox;
        private RadioBoxes.Combo ModeControl;
        private RadioBoxes.NumberBox FilterLowControl;
        private RadioBoxes.NumberBox FilterHighControl;
        private RadioBoxes.Combo SquelchControl;
        private RadioBoxes.NumberBox SquelchLevelControl;
        private RadioBoxes.NumberBox PowerControl;
        private RadioBoxes.Combo ToneModeControl;
        private RadioBoxes.Combo ToneFrequencyControl;
        private RadioBoxes.Combo OffsetDirectionControl;
        private RadioBoxes.NumberBox OffsetControl;
        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.TextBox NameBox;
        private System.Windows.Forms.Label OwnerLabel;
        private System.Windows.Forms.TextBox OwnerBox;
        private System.Windows.Forms.Label GroupLabel;
        private System.Windows.Forms.TextBox GroupBox;
    }
}