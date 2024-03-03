namespace JJFlexControl
{
    partial class SetupKeysAndActions
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
            this.KeysLabel = new System.Windows.Forms.Label();
            this.KeysListBox = new System.Windows.Forms.ListBox();
            this.DoneButton = new System.Windows.Forms.Button();
            this.CnclButton = new System.Windows.Forms.Button();
            this.ActionsBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // KeysLabel
            // 
            this.KeysLabel.AccessibleName = "";
            this.KeysLabel.AutoSize = true;
            this.KeysLabel.Location = new System.Drawing.Point(0, 20);
            this.KeysLabel.Name = "KeysLabel";
            this.KeysLabel.Size = new System.Drawing.Size(90, 17);
            this.KeysLabel.TabIndex = 10;
            this.KeysLabel.Text = "Defined keys";
            // 
            // KeysListBox
            // 
            this.KeysListBox.AccessibleName = "defined keys";
            this.KeysListBox.AccessibleRole = System.Windows.Forms.AccessibleRole.List;
            this.KeysListBox.FormattingEnabled = true;
            this.KeysListBox.ItemHeight = 16;
            this.KeysListBox.Location = new System.Drawing.Point(4, 40);
            this.KeysListBox.Name = "KeysListBox";
            this.KeysListBox.Size = new System.Drawing.Size(300, 292);
            this.KeysListBox.TabIndex = 11;
            this.KeysListBox.SelectedIndexChanged += new System.EventHandler(this.KeysListBox_SelectedIndexChanged);
            this.KeysListBox.Enter += new System.EventHandler(this.KeysListBox_Enter);
            this.KeysListBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeysListBox_KeyDown);
            this.KeysListBox.Leave += new System.EventHandler(this.KeysListBox_Leave);
            // 
            // DoneButton
            // 
            this.DoneButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.DoneButton.Location = new System.Drawing.Point(130, 500);
            this.DoneButton.Name = "DoneButton";
            this.DoneButton.Size = new System.Drawing.Size(75, 23);
            this.DoneButton.TabIndex = 90;
            this.DoneButton.Text = "Done";
            this.DoneButton.UseVisualStyleBackColor = true;
            this.DoneButton.Click += new System.EventHandler(this.DoneButton_Click);
            // 
            // CnclButton
            // 
            this.CnclButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.CnclButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CnclButton.Location = new System.Drawing.Point(270, 500);
            this.CnclButton.Name = "CnclButton";
            this.CnclButton.Size = new System.Drawing.Size(75, 23);
            this.CnclButton.TabIndex = 95;
            this.CnclButton.Text = "Cancel";
            this.CnclButton.UseVisualStyleBackColor = true;
            this.CnclButton.Click += new System.EventHandler(this.CnclButton_Click);
            // 
            // ActionsBox
            // 
            this.ActionsBox.AccessibleRole = System.Windows.Forms.AccessibleRole.List;
            this.ActionsBox.Enabled = false;
            this.ActionsBox.FormattingEnabled = true;
            this.ActionsBox.ItemHeight = 16;
            this.ActionsBox.Location = new System.Drawing.Point(0, 0);
            this.ActionsBox.Name = "ActionsBox";
            this.ActionsBox.Size = new System.Drawing.Size(300, 84);
            this.ActionsBox.TabIndex = 50;
            this.ActionsBox.Visible = false;
            // 
            // SetupKeysAndActions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CnclButton;
            this.ClientSize = new System.Drawing.Size(382, 553);
            this.Controls.Add(this.ActionsBox);
            this.Controls.Add(this.CnclButton);
            this.Controls.Add(this.DoneButton);
            this.Controls.Add(this.KeysListBox);
            this.Controls.Add(this.KeysLabel);
            this.Name = "SetupKeysAndActions";
            this.Text = "Setup Keys And Actions";
            this.Load += new System.EventHandler(this.SetupKeysAndActions_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label KeysLabel;
        private System.Windows.Forms.ListBox KeysListBox;
        private System.Windows.Forms.Button DoneButton;
        private System.Windows.Forms.Button CnclButton;
        private System.Windows.Forms.ListBox ActionsBox;
    }
}