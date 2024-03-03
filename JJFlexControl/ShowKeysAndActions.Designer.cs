namespace JJFlexControl
{
    partial class ShowKeysAndActions
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
            this.DefinedKeysLabel = new System.Windows.Forms.Label();
            this.DefinedKeysList = new System.Windows.Forms.ListBox();
            this.UpdateButton = new System.Windows.Forms.Button();
            this.DoneButton = new System.Windows.Forms.Button();
            this.ActionLabel = new System.Windows.Forms.Label();
            this.ActionBox = new System.Windows.Forms.TextBox();
            this.ValueLabel = new System.Windows.Forms.Label();
            this.ValueBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // DefinedKeysLabel
            // 
            this.DefinedKeysLabel.AutoSize = true;
            this.DefinedKeysLabel.Location = new System.Drawing.Point(10, 20);
            this.DefinedKeysLabel.Name = "DefinedKeysLabel";
            this.DefinedKeysLabel.Size = new System.Drawing.Size(92, 17);
            this.DefinedKeysLabel.TabIndex = 10;
            this.DefinedKeysLabel.Text = "Defined Keys";
            // 
            // DefinedKeysList
            // 
            this.DefinedKeysList.AccessibleRole = System.Windows.Forms.AccessibleRole.List;
            this.DefinedKeysList.FormattingEnabled = true;
            this.DefinedKeysList.ItemHeight = 16;
            this.DefinedKeysList.Location = new System.Drawing.Point(4, 40);
            this.DefinedKeysList.Name = "DefinedKeysList";
            this.DefinedKeysList.Size = new System.Drawing.Size(1000, 244);
            this.DefinedKeysList.TabIndex = 11;
            this.DefinedKeysList.SelectedIndexChanged += new System.EventHandler(this.DefinedKeysList_SelectedIndexChanged);
            // 
            // UpdateButton
            // 
            this.UpdateButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.UpdateButton.Location = new System.Drawing.Point(300, 400);
            this.UpdateButton.Name = "UpdateButton";
            this.UpdateButton.Size = new System.Drawing.Size(75, 23);
            this.UpdateButton.TabIndex = 90;
            this.UpdateButton.Text = "Update";
            this.UpdateButton.UseVisualStyleBackColor = true;
            this.UpdateButton.Click += new System.EventHandler(this.UpdateButton_Click);
            // 
            // DoneButton
            // 
            this.DoneButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.DoneButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.DoneButton.Location = new System.Drawing.Point(700, 400);
            this.DoneButton.Name = "DoneButton";
            this.DoneButton.Size = new System.Drawing.Size(75, 23);
            this.DoneButton.TabIndex = 95;
            this.DoneButton.Text = "Done";
            this.DoneButton.UseVisualStyleBackColor = true;
            this.DoneButton.Click += new System.EventHandler(this.DoneButton_Click);
            // 
            // ActionLabel
            // 
            this.ActionLabel.AutoSize = true;
            this.ActionLabel.Location = new System.Drawing.Point(0, 300);
            this.ActionLabel.Name = "ActionLabel";
            this.ActionLabel.Size = new System.Drawing.Size(55, 17);
            this.ActionLabel.TabIndex = 20;
            this.ActionLabel.Text = "Action: ";
            // 
            // ActionBox
            // 
            this.ActionBox.AccessibleDescription = "";
            this.ActionBox.AccessibleName = "action";
            this.ActionBox.Location = new System.Drawing.Point(55, 300);
            this.ActionBox.Name = "ActionBox";
            this.ActionBox.ReadOnly = true;
            this.ActionBox.Size = new System.Drawing.Size(940, 22);
            this.ActionBox.TabIndex = 21;
            // 
            // ValueLabel
            // 
            this.ValueLabel.AutoSize = true;
            this.ValueLabel.Location = new System.Drawing.Point(0, 330);
            this.ValueLabel.Name = "ValueLabel";
            this.ValueLabel.Size = new System.Drawing.Size(103, 17);
            this.ValueLabel.TabIndex = 30;
            this.ValueLabel.Text = "Current Value: ";
            // 
            // ValueBox
            // 
            this.ValueBox.AccessibleName = "value";
            this.ValueBox.Location = new System.Drawing.Point(103, 330);
            this.ValueBox.Name = "ValueBox";
            this.ValueBox.ReadOnly = true;
            this.ValueBox.Size = new System.Drawing.Size(897, 22);
            this.ValueBox.TabIndex = 31;
            // 
            // ShowKeysAndActions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.DoneButton;
            this.ClientSize = new System.Drawing.Size(1006, 453);
            this.Controls.Add(this.ValueBox);
            this.Controls.Add(this.ValueLabel);
            this.Controls.Add(this.ActionBox);
            this.Controls.Add(this.ActionLabel);
            this.Controls.Add(this.DoneButton);
            this.Controls.Add(this.UpdateButton);
            this.Controls.Add(this.DefinedKeysList);
            this.Controls.Add(this.DefinedKeysLabel);
            this.Name = "ShowKeysAndActions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Current Keys";
            this.Activated += new System.EventHandler(this.ShowKeysAndActions_Activated);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label DefinedKeysLabel;
        private System.Windows.Forms.ListBox DefinedKeysList;
        private System.Windows.Forms.Button UpdateButton;
        private System.Windows.Forms.Button DoneButton;
        private System.Windows.Forms.Label ActionLabel;
        private System.Windows.Forms.TextBox ActionBox;
        private System.Windows.Forms.Label ValueLabel;
        private System.Windows.Forms.TextBox ValueBox;
    }
}