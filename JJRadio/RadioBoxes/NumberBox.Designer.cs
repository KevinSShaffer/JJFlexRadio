namespace RadioBoxes
{
    partial class NumberBox
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
            this.Heading = new System.Windows.Forms.Label();
            this.Box = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Heading
            // 
            this.Heading.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.Heading.AutoSize = true;
            this.Heading.Location = new System.Drawing.Point(0, 0);
            this.Heading.Name = "Heading";
            this.Heading.Size = new System.Drawing.Size(0, 13);
            this.Heading.TabIndex = 0;
            // 
            // Box
            // 
            this.Box.AcceptsReturn = true;
            this.Box.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.Box.Location = new System.Drawing.Point(0, 15);
            this.Box.Multiline = true;
            this.Box.Name = "Box";
            this.Box.Size = new System.Drawing.Size(100, 20);
            this.Box.TabIndex = 1;
            this.Box.Enter += new System.EventHandler(this.Box_Enter);
            this.Box.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Box_KeyDown);
            this.Box.Leave += new System.EventHandler(this.Box_Leave);
            // 
            // NumberBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Box);
            this.Controls.Add(this.Heading);
            this.Name = "NumberBox";
            this.SizeChanged += new System.EventHandler(this.NumberBox_SizeChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Heading;
        private System.Windows.Forms.TextBox Box;
    }
}
