namespace JJArClusterLib
{
    partial class Command
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
            this.CommandLabel = new System.Windows.Forms.Label();
            this.CommandBox = new System.Windows.Forms.TextBox();
            this.OkButton = new System.Windows.Forms.Button();
            this.CnclButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CommandLabel
            // 
            this.CommandLabel.AutoSize = true;
            this.CommandLabel.Location = new System.Drawing.Point(0, 20);
            this.CommandLabel.Name = "CommandLabel";
            this.CommandLabel.Size = new System.Drawing.Size(60, 13);
            this.CommandLabel.TabIndex = 10;
            this.CommandLabel.Text = "Command: ";
            // 
            // CommandBox
            // 
            this.CommandBox.AccessibleName = "command";
            this.CommandBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.CommandBox.Location = new System.Drawing.Point(60, 20);
            this.CommandBox.Name = "CommandBox";
            this.CommandBox.Size = new System.Drawing.Size(400, 20);
            this.CommandBox.TabIndex = 11;
            // 
            // OkButton
            // 
            this.OkButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.OkButton.Location = new System.Drawing.Point(8, 40);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(75, 23);
            this.OkButton.TabIndex = 90;
            this.OkButton.Text = "OK";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // CnclButton
            // 
            this.CnclButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.CnclButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CnclButton.Location = new System.Drawing.Point(400, 40);
            this.CnclButton.Name = "CnclButton";
            this.CnclButton.Size = new System.Drawing.Size(75, 23);
            this.CnclButton.TabIndex = 99;
            this.CnclButton.Text = "Cancel";
            this.CnclButton.UseVisualStyleBackColor = true;
            this.CnclButton.Click += new System.EventHandler(this.CnclButton_Click);
            // 
            // Command
            // 
            this.AcceptButton = this.OkButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CnclButton;
            this.ClientSize = new System.Drawing.Size(484, 62);
            this.Controls.Add(this.CnclButton);
            this.Controls.Add(this.OkButton);
            this.Controls.Add(this.CommandBox);
            this.Controls.Add(this.CommandLabel);
            this.Name = "Command";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Command entry";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label CommandLabel;
        private System.Windows.Forms.TextBox CommandBox;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Button CnclButton;
    }
}