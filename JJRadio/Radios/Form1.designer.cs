namespace Tester
{
    partial class Form1
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
            this.WavButton = new System.Windows.Forms.Button();
            this.OpusButton = new System.Windows.Forms.Button();
            this.CWMonButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // WavButton
            // 
            this.WavButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.WavButton.AutoSize = true;
            this.WavButton.Location = new System.Drawing.Point(0, 20);
            this.WavButton.Name = "WavButton";
            this.WavButton.Size = new System.Drawing.Size(75, 27);
            this.WavButton.TabIndex = 10;
            this.WavButton.Text = "Test wav";
            this.WavButton.UseVisualStyleBackColor = true;
            this.WavButton.Click += new System.EventHandler(this.WavButton_Click);
            // 
            // OpusButton
            // 
            this.OpusButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.OpusButton.AutoSize = true;
            this.OpusButton.Location = new System.Drawing.Point(150, 20);
            this.OpusButton.Name = "OpusButton";
            this.OpusButton.Size = new System.Drawing.Size(81, 27);
            this.OpusButton.TabIndex = 11;
            this.OpusButton.Text = "Test opus";
            this.OpusButton.UseVisualStyleBackColor = true;
            this.OpusButton.Click += new System.EventHandler(this.OpusButton_Click);
            // 
            // CWMonButton
            // 
            this.CWMonButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.CWMonButton.AutoSize = true;
            this.CWMonButton.Location = new System.Drawing.Point(300, 20);
            this.CWMonButton.Name = "CWMonButton";
            this.CWMonButton.Size = new System.Drawing.Size(99, 27);
            this.CWMonButton.TabIndex = 12;
            this.CWMonButton.Text = "Test CWMon";
            this.CWMonButton.UseVisualStyleBackColor = true;
            this.CWMonButton.Click += new System.EventHandler(this.CWMonButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1006, 253);
            this.Controls.Add(this.CWMonButton);
            this.Controls.Add(this.OpusButton);
            this.Controls.Add(this.WavButton);
            this.Name = "Form1";
            this.Text = "Test AudioHelper";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button WavButton;
        private System.Windows.Forms.Button OpusButton;
        private System.Windows.Forms.Button CWMonButton;
    }
}

