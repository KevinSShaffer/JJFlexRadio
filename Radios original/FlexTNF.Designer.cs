namespace Radios
{
    partial class FlexTNF
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
            this.TNFListLabel = new System.Windows.Forms.Label();
            this.TNFList = new System.Windows.Forms.ListBox();
            this.DepthBox = new RadioBoxes.NumberBox();
            this.WidthBox = new RadioBoxes.NumberBox();
            this.PermanentBox = new RadioBoxes.Combo();
            this.AddButton = new System.Windows.Forms.Button();
            this.DoneButton = new System.Windows.Forms.Button();
            this.RemoveButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TNFListLabel
            // 
            this.TNFListLabel.AutoSize = true;
            this.TNFListLabel.Location = new System.Drawing.Point(20, 20);
            this.TNFListLabel.Name = "TNFListLabel";
            this.TNFListLabel.Size = new System.Drawing.Size(34, 13);
            this.TNFListLabel.TabIndex = 10;
            this.TNFListLabel.Text = "Filters";
            // 
            // TNFList
            // 
            this.TNFList.AccessibleName = "filters";
            this.TNFList.AccessibleRole = System.Windows.Forms.AccessibleRole.List;
            this.TNFList.FormattingEnabled = true;
            this.TNFList.Location = new System.Drawing.Point(0, 40);
            this.TNFList.Name = "TNFList";
            this.TNFList.Size = new System.Drawing.Size(100, 95);
            this.TNFList.TabIndex = 11;
            this.TNFList.SelectedIndexChanged += new System.EventHandler(this.TNFList_SelectedIndexChanged);
            // 
            // DepthBox
            // 
            this.DepthBox.Header = "Depth";
            this.DepthBox.HighValue = 0;
            this.DepthBox.Increment = 0;
            this.DepthBox.Location = new System.Drawing.Point(120, 80);
            this.DepthBox.LowValue = 0;
            this.DepthBox.Name = "DepthBox";
            this.DepthBox.Size = new System.Drawing.Size(50, 36);
            this.DepthBox.TabIndex = 120;
            this.DepthBox.Tag = "Depth";
            this.DepthBox.UpdateDisplayFunction = null;
            this.DepthBox.UpdateRigFunction = null;
            // 
            // WidthBox
            // 
            this.WidthBox.Header = "Width";
            this.WidthBox.HighValue = 0;
            this.WidthBox.Increment = 0;
            this.WidthBox.Location = new System.Drawing.Point(120, 40);
            this.WidthBox.LowValue = 0;
            this.WidthBox.Name = "WidthBox";
            this.WidthBox.Size = new System.Drawing.Size(100, 36);
            this.WidthBox.TabIndex = 110;
            this.WidthBox.Tag = "Width";
            this.WidthBox.UpdateDisplayFunction = null;
            this.WidthBox.UpdateRigFunction = null;
            // 
            // PermanentBox
            // 
            this.PermanentBox.BoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PermanentBox.ExpandedSize = new System.Drawing.Size(50, 36);
            this.PermanentBox.Header = "Perm";
            this.PermanentBox.Location = new System.Drawing.Point(120, 120);
            this.PermanentBox.Name = "PermanentBox";
            this.PermanentBox.ReadOnly = false;
            this.PermanentBox.Size = new System.Drawing.Size(50, 36);
            this.PermanentBox.SmallSize = new System.Drawing.Size(50, 36);
            this.PermanentBox.TabIndex = 130;
            this.PermanentBox.Tag = "Permanent";
            this.PermanentBox.TheList = null;
            this.PermanentBox.UpdateDisplayFunction = null;
            this.PermanentBox.UpdateRigFunction = null;
            // 
            // AddButton
            // 
            this.AddButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.AddButton.AutoSize = true;
            this.AddButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.AddButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.AddButton.Location = new System.Drawing.Point(0, 220);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(36, 23);
            this.AddButton.TabIndex = 200;
            this.AddButton.Text = "Add";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // DoneButton
            // 
            this.DoneButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.DoneButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.DoneButton.Location = new System.Drawing.Point(120, 220);
            this.DoneButton.Name = "DoneButton";
            this.DoneButton.Size = new System.Drawing.Size(75, 23);
            this.DoneButton.TabIndex = 290;
            this.DoneButton.Text = "Done";
            this.DoneButton.UseVisualStyleBackColor = true;
            this.DoneButton.Click += new System.EventHandler(this.DoneButton_Click);
            // 
            // RemoveButton
            // 
            this.RemoveButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.RemoveButton.AutoSize = true;
            this.RemoveButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.RemoveButton.Location = new System.Drawing.Point(50, 220);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(57, 23);
            this.RemoveButton.TabIndex = 210;
            this.RemoveButton.Text = "Remove";
            this.RemoveButton.UseVisualStyleBackColor = true;
            this.RemoveButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // FlexTNF
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.DoneButton;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.RemoveButton);
            this.Controls.Add(this.DoneButton);
            this.Controls.Add(this.AddButton);
            this.Controls.Add(this.PermanentBox);
            this.Controls.Add(this.WidthBox);
            this.Controls.Add(this.DepthBox);
            this.Controls.Add(this.TNFList);
            this.Controls.Add(this.TNFListLabel);
            this.Name = "FlexTNF";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tracking Notch Filters";
            this.Activated += new System.EventHandler(this.FlexTNF_Activated);
            this.Load += new System.EventHandler(this.FlexTNF_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label TNFListLabel;
        private System.Windows.Forms.ListBox TNFList;
        private RadioBoxes.NumberBox DepthBox;
        private RadioBoxes.NumberBox WidthBox;
        private RadioBoxes.Combo PermanentBox;
        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.Button DoneButton;
        private System.Windows.Forms.Button RemoveButton;
    }
}