namespace RadioBoxes
{
    partial class MainBox
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
            this.Box = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Box
            // 
            this.Box.AccessibleName = "";
            this.Box.AccessibleRole = System.Windows.Forms.AccessibleRole.Document;
            this.Box.Location = new System.Drawing.Point(0, 0);
            this.Box.Margin = new System.Windows.Forms.Padding(4);
            this.Box.Name = "Box";
            this.Box.ReadOnly = true;
            this.Box.Size = new System.Drawing.Size(399, 22);
            this.Box.TabIndex = 0;
            this.Box.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Box_KeyDown);
            // 
            // MainBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Box);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainBox";
            this.Size = new System.Drawing.Size(200, 185);
            this.Resize += new System.EventHandler(this.MainBox_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Box;
    }
}
