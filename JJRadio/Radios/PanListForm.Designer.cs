namespace Radios
{
    partial class PanListForm
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
            this.CnclButton = new System.Windows.Forms.Button();
            this.RangeListLabel = new System.Windows.Forms.Label();
            this.RangeList = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // CnclButton
            // 
            this.CnclButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CnclButton.Location = new System.Drawing.Point(220, 100);
            this.CnclButton.Name = "CnclButton";
            this.CnclButton.Size = new System.Drawing.Size(75, 23);
            this.CnclButton.TabIndex = 99;
            this.CnclButton.Text = "Cancel";
            this.CnclButton.UseVisualStyleBackColor = true;
            this.CnclButton.Click += new System.EventHandler(this.CnclButton_Click);
            // 
            // RangeListLabel
            // 
            this.RangeListLabel.AutoSize = true;
            this.RangeListLabel.Location = new System.Drawing.Point(0, 20);
            this.RangeListLabel.Name = "RangeListLabel";
            this.RangeListLabel.Size = new System.Drawing.Size(84, 13);
            this.RangeListLabel.TabIndex = 10;
            this.RangeListLabel.Text = "Pertinent ranges";
            // 
            // RangeList
            // 
            this.RangeList.AccessibleRole = System.Windows.Forms.AccessibleRole.List;
            this.RangeList.FormattingEnabled = true;
            this.RangeList.Location = new System.Drawing.Point(0, 40);
            this.RangeList.Name = "RangeList";
            this.RangeList.Size = new System.Drawing.Size(200, 147);
            this.RangeList.TabIndex = 11;
            this.RangeList.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.RangeList_KeyPress);
            // 
            // PanListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CnclButton;
            this.ClientSize = new System.Drawing.Size(284, 162);
            this.Controls.Add(this.RangeList);
            this.Controls.Add(this.RangeListLabel);
            this.Controls.Add(this.CnclButton);
            this.Name = "PanListForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ranges";
            this.Load += new System.EventHandler(this.PanListForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CnclButton;
        private System.Windows.Forms.Label RangeListLabel;
        private System.Windows.Forms.ListBox RangeList;
    }
}