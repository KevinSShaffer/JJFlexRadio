namespace Radios
{
    partial class K3Menus
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
            this.DoneButton = new System.Windows.Forms.Button();
            this.ItemLabel = new System.Windows.Forms.Label();
            this.MenuList = new System.Windows.Forms.ListBox();
            this.ValueLabel = new System.Windows.Forms.Label();
            this.ValueBox = new System.Windows.Forms.TextBox();
            this.DescriptionBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // DoneButton
            // 
            this.DoneButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.DoneButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.DoneButton.Location = new System.Drawing.Point(300, 280);
            this.DoneButton.Name = "DoneButton";
            this.DoneButton.Size = new System.Drawing.Size(75, 23);
            this.DoneButton.TabIndex = 999;
            this.DoneButton.Text = "Done";
            this.DoneButton.UseVisualStyleBackColor = true;
            this.DoneButton.Click += new System.EventHandler(this.DoneButton_Click);
            // 
            // ItemLabel
            // 
            this.ItemLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.ItemLabel.AutoSize = true;
            this.ItemLabel.Location = new System.Drawing.Point(0, 0);
            this.ItemLabel.Name = "ItemLabel";
            this.ItemLabel.Size = new System.Drawing.Size(27, 13);
            this.ItemLabel.TabIndex = 10;
            this.ItemLabel.Text = "Item";
            // 
            // MenuList
            // 
            this.MenuList.AccessibleName = "menus";
            this.MenuList.AccessibleRole = System.Windows.Forms.AccessibleRole.List;
            this.MenuList.FormattingEnabled = true;
            this.MenuList.Location = new System.Drawing.Point(0, 20);
            this.MenuList.Name = "MenuList";
            this.MenuList.Size = new System.Drawing.Size(120, 199);
            this.MenuList.TabIndex = 11;
            this.MenuList.SelectedIndexChanged += new System.EventHandler(this.MenuList_SelectedIndexChanged);
            this.MenuList.Enter += new System.EventHandler(this.MenuList_Enter);
            // 
            // ValueLabel
            // 
            this.ValueLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.ValueLabel.AutoSize = true;
            this.ValueLabel.Location = new System.Drawing.Point(150, 0);
            this.ValueLabel.Name = "ValueLabel";
            this.ValueLabel.Size = new System.Drawing.Size(34, 13);
            this.ValueLabel.TabIndex = 100;
            this.ValueLabel.Text = "Value";
            // 
            // ValueBox
            // 
            this.ValueBox.AccessibleName = "value";
            this.ValueBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.ValueBox.Location = new System.Drawing.Point(150, 20);
            this.ValueBox.Name = "ValueBox";
            this.ValueBox.ReadOnly = true;
            this.ValueBox.Size = new System.Drawing.Size(100, 20);
            this.ValueBox.TabIndex = 101;
            // 
            // DescriptionBox
            // 
            this.DescriptionBox.AcceptsReturn = true;
            this.DescriptionBox.AccessibleName = "description";
            this.DescriptionBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.DescriptionBox.Location = new System.Drawing.Point(150, 60);
            this.DescriptionBox.Multiline = true;
            this.DescriptionBox.Name = "DescriptionBox";
            this.DescriptionBox.Size = new System.Drawing.Size(400, 140);
            this.DescriptionBox.TabIndex = 200;
            this.DescriptionBox.Enter += new System.EventHandler(this.DescriptionBox_Enter);
            // 
            // K3Menus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.DoneButton;
            this.ClientSize = new System.Drawing.Size(584, 302);
            this.Controls.Add(this.DescriptionBox);
            this.Controls.Add(this.ValueBox);
            this.Controls.Add(this.ValueLabel);
            this.Controls.Add(this.MenuList);
            this.Controls.Add(this.ItemLabel);
            this.Controls.Add(this.DoneButton);
            this.Name = "K3Menus";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "K3 Menus";
            this.Activated += new System.EventHandler(this.K3Menus_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.K3Menus_FormClosing);
            this.Load += new System.EventHandler(this.K3Menus_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button DoneButton;
        private System.Windows.Forms.Label ItemLabel;
        private System.Windows.Forms.ListBox MenuList;
        private System.Windows.Forms.Label ValueLabel;
        private System.Windows.Forms.TextBox ValueBox;
        private System.Windows.Forms.TextBox DescriptionBox;
    }
}