namespace JJFlexControl
{
    partial class ComInfo
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
            this.ComPortLabel = new System.Windows.Forms.Label();
            this.ComPortList = new System.Windows.Forms.ListBox();
            this.SelectButton = new System.Windows.Forms.Button();
            this.CnclButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ComPortLabel
            // 
            this.ComPortLabel.AutoSize = true;
            this.ComPortLabel.Location = new System.Drawing.Point(0, 20);
            this.ComPortLabel.Name = "ComPortLabel";
            this.ComPortLabel.Size = new System.Drawing.Size(73, 17);
            this.ComPortLabel.TabIndex = 10;
            this.ComPortLabel.Text = "Com Ports";
            // 
            // ComPortList
            // 
            this.ComPortList.AccessibleName = "com ports";
            this.ComPortList.AccessibleRole = System.Windows.Forms.AccessibleRole.List;
            this.ComPortList.FormattingEnabled = true;
            this.ComPortList.ItemHeight = 16;
            this.ComPortList.Location = new System.Drawing.Point(4, 40);
            this.ComPortList.Name = "ComPortList";
            this.ComPortList.Size = new System.Drawing.Size(120, 84);
            this.ComPortList.TabIndex = 11;
            // 
            // SelectButton
            // 
            this.SelectButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.SelectButton.Location = new System.Drawing.Point(0, 200);
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
            this.CnclButton.Location = new System.Drawing.Point(100, 200);
            this.CnclButton.Name = "CnclButton";
            this.CnclButton.Size = new System.Drawing.Size(75, 23);
            this.CnclButton.TabIndex = 95;
            this.CnclButton.Text = "Cancel";
            this.CnclButton.UseVisualStyleBackColor = true;
            this.CnclButton.Click += new System.EventHandler(this.CnclButton_Click);
            // 
            // ComInfo
            // 
            this.AcceptButton = this.SelectButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CnclButton;
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Controls.Add(this.CnclButton);
            this.Controls.Add(this.SelectButton);
            this.Controls.Add(this.ComPortList);
            this.Controls.Add(this.ComPortLabel);
            this.Name = "ComInfo";
            this.Text = "Knob\'s Port Info";
            this.Activated += new System.EventHandler(this.ComInfo_Activated);
            this.Load += new System.EventHandler(this.ComInfo_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ComPortLabel;
        private System.Windows.Forms.ListBox ComPortList;
        private System.Windows.Forms.Button SelectButton;
        private System.Windows.Forms.Button CnclButton;
    }
}