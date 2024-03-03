namespace JJPortaudio
{
    partial class devList
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
            this.DevListLabel = new System.Windows.Forms.Label();
            this.DevListBox = new System.Windows.Forms.ListBox();
            this.SelectButton = new System.Windows.Forms.Button();
            this.CnclButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // DevListLabel
            // 
            this.DevListLabel.AutoSize = true;
            this.DevListLabel.Location = new System.Drawing.Point(0, 20);
            this.DevListLabel.Name = "DevListLabel";
            this.DevListLabel.Size = new System.Drawing.Size(72, 17);
            this.DevListLabel.TabIndex = 10;
            this.DevListLabel.Text = "Device list";
            // 
            // DevListBox
            // 
            this.DevListBox.AccessibleName = "device list";
            this.DevListBox.AccessibleRole = System.Windows.Forms.AccessibleRole.List;
            this.DevListBox.FormattingEnabled = true;
            this.DevListBox.ItemHeight = 16;
            this.DevListBox.Location = new System.Drawing.Point(0, 40);
            this.DevListBox.Name = "DevListBox";
            this.DevListBox.Size = new System.Drawing.Size(750, 148);
            this.DevListBox.TabIndex = 11;
            // 
            // SelectButton
            // 
            this.SelectButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.SelectButton.Location = new System.Drawing.Point(8, 200);
            this.SelectButton.Name = "SelectButton";
            this.SelectButton.Size = new System.Drawing.Size(75, 23);
            this.SelectButton.TabIndex = 90;
            this.SelectButton.Text = "Select";
            this.SelectButton.UseVisualStyleBackColor = true;
            this.SelectButton.Click += new System.EventHandler(this.SelectButton_Click);
            // 
            // CnclButton
            // 
            this.CnclButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.CnclButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CnclButton.Location = new System.Drawing.Point(500, 200);
            this.CnclButton.Name = "CnclButton";
            this.CnclButton.Size = new System.Drawing.Size(75, 23);
            this.CnclButton.TabIndex = 99;
            this.CnclButton.Text = "Cancel";
            this.CnclButton.UseVisualStyleBackColor = true;
            this.CnclButton.Click += new System.EventHandler(this.CnclButton_Click);
            // 
            // devList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CnclButton;
            this.ClientSize = new System.Drawing.Size(782, 253);
            this.Controls.Add(this.CnclButton);
            this.Controls.Add(this.SelectButton);
            this.Controls.Add(this.DevListBox);
            this.Controls.Add(this.DevListLabel);
            this.Name = "devList";
            this.Text = "Device selection";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.devList_FormClosing);
            this.Load += new System.EventHandler(this.devList_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label DevListLabel;
        private System.Windows.Forms.ListBox DevListBox;
        private System.Windows.Forms.Button SelectButton;
        private System.Windows.Forms.Button CnclButton;
    }
}