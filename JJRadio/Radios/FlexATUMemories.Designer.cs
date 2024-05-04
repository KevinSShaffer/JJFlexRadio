namespace Radios
{
    partial class FlexATUMemories
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
            this.EnableControl = new RadioBoxes.Combo();
            this.DoneButton = new System.Windows.Forms.Button();
            this.ClearButton = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // EnableControl
            // 
            this.EnableControl.BoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.EnableControl.ExpandedSize = new System.Drawing.Size(80, 60);
            this.EnableControl.Header = "Enabled";
            this.EnableControl.Location = new System.Drawing.Point(0, 20);
            this.EnableControl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.EnableControl.Name = "EnableControl";
            this.EnableControl.ReadOnly = false;
            this.EnableControl.Size = new System.Drawing.Size(80, 60);
            this.EnableControl.SmallSize = new System.Drawing.Size(80, 36);
            this.EnableControl.TabIndex = 10;
            this.EnableControl.Tag = "Enabled";
            this.EnableControl.TheList = null;
            this.EnableControl.UpdateDisplayFunction = null;
            this.EnableControl.UpdateRigFunction = null;
            // 
            // DoneButton
            // 
            this.DoneButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.DoneButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.DoneButton.Location = new System.Drawing.Point(120, 100);
            this.DoneButton.Name = "DoneButton";
            this.DoneButton.Size = new System.Drawing.Size(75, 23);
            this.DoneButton.TabIndex = 95;
            this.DoneButton.Text = "Done";
            this.DoneButton.UseVisualStyleBackColor = true;
            // 
            // ClearButton
            // 
            this.ClearButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.ClearButton.AutoSize = true;
            this.ClearButton.Location = new System.Drawing.Point(100, 20);
            this.ClearButton.Name = "ClearButton";
            this.ClearButton.Size = new System.Drawing.Size(116, 27);
            this.ClearButton.TabIndex = 20;
            this.ClearButton.Text = "Clear memories";
            this.ClearButton.UseVisualStyleBackColor = true;
            this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(8, 60);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 22);
            this.textBox1.TabIndex = 30;
            // 
            // FlexATUMemories
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.DoneButton;
            this.ClientSize = new System.Drawing.Size(282, 153);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.ClearButton);
            this.Controls.Add(this.DoneButton);
            this.Controls.Add(this.EnableControl);
            this.Name = "FlexATUMemories";
            this.Text = "ATU Memories";
            this.Load += new System.EventHandler(this.FlexATUMemories_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RadioBoxes.Combo EnableControl;
        private System.Windows.Forms.Button DoneButton;
        private System.Windows.Forms.Button ClearButton;
        private System.Windows.Forms.TextBox textBox1;
    }
}