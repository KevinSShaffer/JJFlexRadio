namespace JJArClusterLib
{
    partial class ClusterForm
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
            this.OutputBox = new System.Windows.Forms.TextBox();
            this.BeepButton = new System.Windows.Forms.Button();
            this.CommandLabel = new System.Windows.Forms.Label();
            this.CommandBox = new System.Windows.Forms.TextBox();
            this.TrackButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // OutputBox
            // 
            this.OutputBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.OutputBox.Location = new System.Drawing.Point(0, 20);
            this.OutputBox.Multiline = true;
            this.OutputBox.Name = "OutputBox";
            this.OutputBox.ReadOnly = true;
            this.OutputBox.Size = new System.Drawing.Size(620, 440);
            this.OutputBox.TabIndex = 10;
            this.OutputBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OutputBox_KeyDown);
            // 
            // BeepButton
            // 
            this.BeepButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.BeepButton.Location = new System.Drawing.Point(0, 500);
            this.BeepButton.Name = "BeepButton";
            this.BeepButton.Size = new System.Drawing.Size(75, 23);
            this.BeepButton.TabIndex = 85;
            this.BeepButton.Text = "Beep On";
            this.BeepButton.UseVisualStyleBackColor = true;
            this.BeepButton.Click += new System.EventHandler(this.BeepButton_Click);
            // 
            // CommandLabel
            // 
            this.CommandLabel.AutoSize = true;
            this.CommandLabel.Location = new System.Drawing.Point(0, 470);
            this.CommandLabel.Name = "CommandLabel";
            this.CommandLabel.Size = new System.Drawing.Size(66, 13);
            this.CommandLabel.TabIndex = 50;
            this.CommandLabel.Text = "Telnet cmd: ";
            // 
            // CommandBox
            // 
            this.CommandBox.AcceptsReturn = true;
            this.CommandBox.AccessibleName = "command";
            this.CommandBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.CommandBox.Location = new System.Drawing.Point(66, 470);
            this.CommandBox.Multiline = true;
            this.CommandBox.Name = "CommandBox";
            this.CommandBox.Size = new System.Drawing.Size(400, 20);
            this.CommandBox.TabIndex = 51;
            this.CommandBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CommandBox_KeyDown);
            // 
            // TrackButton
            // 
            this.TrackButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.TrackButton.Location = new System.Drawing.Point(100, 500);
            this.TrackButton.Name = "TrackButton";
            this.TrackButton.Size = new System.Drawing.Size(102, 23);
            this.TrackButton.TabIndex = 90;
            this.TrackButton.Text = "Track last post off";
            this.TrackButton.UseVisualStyleBackColor = true;
            this.TrackButton.Click += new System.EventHandler(this.TrackButton_Click);
            // 
            // ClusterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 562);
            this.Controls.Add(this.TrackButton);
            this.Controls.Add(this.CommandBox);
            this.Controls.Add(this.CommandLabel);
            this.Controls.Add(this.BeepButton);
            this.Controls.Add(this.OutputBox);
            this.Name = "ClusterForm";
            this.Text = "Cluster Form";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClusterForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ClusterForm_FormClosed);
            this.Load += new System.EventHandler(this.ClusterForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox OutputBox;
        private System.Windows.Forms.Button BeepButton;
        private System.Windows.Forms.Label CommandLabel;
        private System.Windows.Forms.TextBox CommandBox;
        private System.Windows.Forms.Button TrackButton;
    }
}