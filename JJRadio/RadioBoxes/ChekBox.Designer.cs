namespace RadioBoxes
{
    partial class ChekBox
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Box = new System.Windows.Forms.GroupBox();
            this.SuspendLayout();
            // 
            // Box
            // 
            this.Box.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.Box.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Box.Location = new System.Drawing.Point(0, 0);
            this.Box.Name = "Box";
            this.Box.Size = new System.Drawing.Size(6, 5);
            this.Box.TabIndex = 5;
            this.Box.TabStop = false;
            this.Box.Enter += new System.EventHandler(this.Box_Enter);
            this.Box.Leave += new System.EventHandler(this.Box_Leave);
            // 
            // ChekBox
            // 
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.Box);
            this.Name = "ChekBox";
            this.Size = new System.Drawing.Size(9, 8);
            this.Load += new System.EventHandler(this.ChekBox_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox Box;
    }
}
