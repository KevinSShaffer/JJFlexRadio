namespace Radios
{
    partial class ic9100memories
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
            this.MemoriesLabel = new System.Windows.Forms.Label();
            this.MemoriesListbox = new System.Windows.Forms.ListBox();
            this.DisplayModeButton = new System.Windows.Forms.Button();
            this.NameLabel = new System.Windows.Forms.Label();
            this.NameBox = new System.Windows.Forms.TextBox();
            this.AddChangeButton = new System.Windows.Forms.Button();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.DoneButton = new System.Windows.Forms.Button();
            this.BandLabel = new System.Windows.Forms.Label();
            this.BandBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // MemoriesLabel
            // 
            this.MemoriesLabel.AutoSize = true;
            this.MemoriesLabel.Location = new System.Drawing.Point(8, 45);
            this.MemoriesLabel.Name = "MemoriesLabel";
            this.MemoriesLabel.Size = new System.Drawing.Size(52, 13);
            this.MemoriesLabel.TabIndex = 20;
            this.MemoriesLabel.Text = "Memories";
            // 
            // MemoriesListbox
            // 
            this.MemoriesListbox.FormattingEnabled = true;
            this.MemoriesListbox.Location = new System.Drawing.Point(8, 60);
            this.MemoriesListbox.Name = "MemoriesListbox";
            this.MemoriesListbox.Size = new System.Drawing.Size(200, 95);
            this.MemoriesListbox.TabIndex = 21;
            this.MemoriesListbox.SelectedIndexChanged += new System.EventHandler(this.MemoriesListbox_SelectedIndexChanged);
            this.MemoriesListbox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MemoriesListbox_KeyPress);
            // 
            // DisplayModeButton
            // 
            this.DisplayModeButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.DisplayModeButton.AutoSize = true;
            this.DisplayModeButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.DisplayModeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.DisplayModeButton.Location = new System.Drawing.Point(100, 20);
            this.DisplayModeButton.Name = "DisplayModeButton";
            this.DisplayModeButton.Size = new System.Drawing.Size(6, 6);
            this.DisplayModeButton.TabIndex = 10;
            this.DisplayModeButton.UseVisualStyleBackColor = true;
            this.DisplayModeButton.Click += new System.EventHandler(this.DisplayModeButton_Click);
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.Location = new System.Drawing.Point(8, 140);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(39, 13);
            this.NameLabel.TabIndex = 30;
            this.NameLabel.Text = "name: ";
            // 
            // NameBox
            // 
            this.NameBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.NameBox.Location = new System.Drawing.Point(47, 140);
            this.NameBox.Name = "NameBox";
            this.NameBox.Size = new System.Drawing.Size(100, 20);
            this.NameBox.TabIndex = 31;
            // 
            // AddChangeButton
            // 
            this.AddChangeButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.AddChangeButton.AutoSize = true;
            this.AddChangeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.AddChangeButton.Location = new System.Drawing.Point(8, 220);
            this.AddChangeButton.Name = "AddChangeButton";
            this.AddChangeButton.Size = new System.Drawing.Size(6, 6);
            this.AddChangeButton.TabIndex = 900;
            this.AddChangeButton.UseVisualStyleBackColor = true;
            this.AddChangeButton.Click += new System.EventHandler(this.AddChangeButton_Click);
            // 
            // DeleteButton
            // 
            this.DeleteButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.DeleteButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.DeleteButton.Location = new System.Drawing.Point(108, 220);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(75, 23);
            this.DeleteButton.TabIndex = 910;
            this.DeleteButton.Text = "Delete";
            this.DeleteButton.UseVisualStyleBackColor = true;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // DoneButton
            // 
            this.DoneButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.DoneButton.Location = new System.Drawing.Point(208, 220);
            this.DoneButton.Name = "DoneButton";
            this.DoneButton.Size = new System.Drawing.Size(75, 23);
            this.DoneButton.TabIndex = 920;
            this.DoneButton.Text = "Done";
            this.DoneButton.UseVisualStyleBackColor = true;
            // 
            // BandLabel
            // 
            this.BandLabel.AutoSize = true;
            this.BandLabel.Location = new System.Drawing.Point(8, 25);
            this.BandLabel.Name = "BandLabel";
            this.BandLabel.Size = new System.Drawing.Size(38, 13);
            this.BandLabel.TabIndex = 5;
            this.BandLabel.Text = "Band: ";
            // 
            // BandBox
            // 
            this.BandBox.AccessibleRole = System.Windows.Forms.AccessibleRole.ComboBox;
            this.BandBox.FormattingEnabled = true;
            this.BandBox.Location = new System.Drawing.Point(46, 20);
            this.BandBox.Name = "BandBox";
            this.BandBox.Size = new System.Drawing.Size(50, 21);
            this.BandBox.TabIndex = 6;
            this.BandBox.SelectedIndexChanged += new System.EventHandler(this.BandBox_SelectedIndexChanged);
            // 
            // ic9100memories
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.DoneButton;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.BandBox);
            this.Controls.Add(this.BandLabel);
            this.Controls.Add(this.DoneButton);
            this.Controls.Add(this.DeleteButton);
            this.Controls.Add(this.AddChangeButton);
            this.Controls.Add(this.NameBox);
            this.Controls.Add(this.NameLabel);
            this.Controls.Add(this.DisplayModeButton);
            this.Controls.Add(this.MemoriesListbox);
            this.Controls.Add(this.MemoriesLabel);
            this.Name = "ic9100memories";
            this.Text = "Memories";
            this.Activated += new System.EventHandler(this.ic9100memories_Activated);
            this.Load += new System.EventHandler(this.ic9100memories_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label MemoriesLabel;
        private System.Windows.Forms.ListBox MemoriesListbox;
        private System.Windows.Forms.Button DisplayModeButton;
        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.TextBox NameBox;
        private System.Windows.Forms.Button AddChangeButton;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.Button DoneButton;
        private System.Windows.Forms.Label BandLabel;
        private System.Windows.Forms.ComboBox BandBox;
    }
}