namespace JJW2WattMeter
{
    partial class ConfigForm
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
            this.PortLabel = new System.Windows.Forms.Label();
            this.PortList = new System.Windows.Forms.ListBox();
            this.ConfigureButton = new System.Windows.Forms.Button();
            this.CnclButton = new System.Windows.Forms.Button();
            this.UsageLabel = new System.Windows.Forms.Label();
            this.UsageList = new System.Windows.Forms.ListBox();
            this.PowerTypeLabel = new System.Windows.Forms.Label();
            this.PowerTypeListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // PortLabel
            // 
            this.PortLabel.AutoSize = true;
            this.PortLabel.Location = new System.Drawing.Point(0, 20);
            this.PortLabel.Name = "PortLabel";
            this.PortLabel.Size = new System.Drawing.Size(74, 17);
            this.PortLabel.TabIndex = 10;
            this.PortLabel.Text = "Com Port: ";
            // 
            // PortList
            // 
            this.PortList.AccessibleName = "com port";
            this.PortList.AccessibleRole = System.Windows.Forms.AccessibleRole.List;
            this.PortList.FormattingEnabled = true;
            this.PortList.ItemHeight = 16;
            this.PortList.Location = new System.Drawing.Point(47, 20);
            this.PortList.Name = "PortList";
            this.PortList.Size = new System.Drawing.Size(120, 84);
            this.PortList.TabIndex = 11;
            // 
            // ConfigureButton
            // 
            this.ConfigureButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.ConfigureButton.Location = new System.Drawing.Point(220, 8);
            this.ConfigureButton.Name = "ConfigureButton";
            this.ConfigureButton.Size = new System.Drawing.Size(75, 23);
            this.ConfigureButton.TabIndex = 90;
            this.ConfigureButton.Text = "Configure";
            this.ConfigureButton.UseVisualStyleBackColor = true;
            this.ConfigureButton.Click += new System.EventHandler(this.ConfigureButton_Click);
            // 
            // CnclButton
            // 
            this.CnclButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.CnclButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CnclButton.Location = new System.Drawing.Point(220, 200);
            this.CnclButton.Name = "CnclButton";
            this.CnclButton.Size = new System.Drawing.Size(75, 23);
            this.CnclButton.TabIndex = 95;
            this.CnclButton.Text = "Cancel";
            this.CnclButton.UseVisualStyleBackColor = true;
            this.CnclButton.Click += new System.EventHandler(this.CnclButton_Click);
            // 
            // UsageLabel
            // 
            this.UsageLabel.AutoSize = true;
            this.UsageLabel.Location = new System.Drawing.Point(0, 120);
            this.UsageLabel.Name = "UsageLabel";
            this.UsageLabel.Size = new System.Drawing.Size(57, 17);
            this.UsageLabel.TabIndex = 20;
            this.UsageLabel.Text = "Usage: ";
            // 
            // UsageList
            // 
            this.UsageList.AccessibleName = "usage";
            this.UsageList.AccessibleRole = System.Windows.Forms.AccessibleRole.List;
            this.UsageList.FormattingEnabled = true;
            this.UsageList.ItemHeight = 16;
            this.UsageList.Location = new System.Drawing.Point(57, 120);
            this.UsageList.Name = "UsageList";
            this.UsageList.Size = new System.Drawing.Size(200, 36);
            this.UsageList.TabIndex = 21;
            // 
            // PowerTypeLabel
            // 
            this.PowerTypeLabel.AutoSize = true;
            this.PowerTypeLabel.Location = new System.Drawing.Point(0, 150);
            this.PowerTypeLabel.Name = "PowerTypeLabel";
            this.PowerTypeLabel.Size = new System.Drawing.Size(91, 17);
            this.PowerTypeLabel.TabIndex = 30;
            this.PowerTypeLabel.Text = "Power Type: ";
            // 
            // PowerTypeListBox
            // 
            this.PowerTypeListBox.AccessibleName = "power type";
            this.PowerTypeListBox.AccessibleRole = System.Windows.Forms.AccessibleRole.List;
            this.PowerTypeListBox.FormattingEnabled = true;
            this.PowerTypeListBox.ItemHeight = 16;
            this.PowerTypeListBox.Location = new System.Drawing.Point(91, 150);
            this.PowerTypeListBox.Name = "PowerTypeListBox";
            this.PowerTypeListBox.Size = new System.Drawing.Size(120, 36);
            this.PowerTypeListBox.TabIndex = 31;
            // 
            // ConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CnclButton;
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Controls.Add(this.PowerTypeListBox);
            this.Controls.Add(this.PowerTypeLabel);
            this.Controls.Add(this.UsageList);
            this.Controls.Add(this.UsageLabel);
            this.Controls.Add(this.CnclButton);
            this.Controls.Add(this.ConfigureButton);
            this.Controls.Add(this.PortList);
            this.Controls.Add(this.PortLabel);
            this.Name = "ConfigForm";
            this.Text = "W2 Setup";
            this.Load += new System.EventHandler(this.ConfigForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label PortLabel;
        private System.Windows.Forms.ListBox PortList;
        private System.Windows.Forms.Button ConfigureButton;
        private System.Windows.Forms.Button CnclButton;
        private System.Windows.Forms.Label UsageLabel;
        private System.Windows.Forms.ListBox UsageList;
        private System.Windows.Forms.Label PowerTypeLabel;
        private System.Windows.Forms.ListBox PowerTypeListBox;
    }
}