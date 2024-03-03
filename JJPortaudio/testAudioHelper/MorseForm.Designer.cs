namespace testAudioHelper
{
    partial class MorseForm
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
            this.FreqLabel = new System.Windows.Forms.Label();
            this.FreqBox = new System.Windows.Forms.TextBox();
            this.SpeedLabel = new System.Windows.Forms.Label();
            this.SpeedBox = new System.Windows.Forms.TextBox();
            this.CodeBox = new System.Windows.Forms.TextBox();
            this.VolumeLabel = new System.Windows.Forms.Label();
            this.VolumeUpDown = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.VolumeUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // FreqLabel
            // 
            this.FreqLabel.AutoSize = true;
            this.FreqLabel.Location = new System.Drawing.Point(0, 20);
            this.FreqLabel.Name = "FreqLabel";
            this.FreqLabel.Size = new System.Drawing.Size(83, 17);
            this.FreqLabel.TabIndex = 10;
            this.FreqLabel.Text = "Frequency: ";
            // 
            // FreqBox
            // 
            this.FreqBox.AccessibleName = "frequency";
            this.FreqBox.Location = new System.Drawing.Point(83, 20);
            this.FreqBox.Name = "FreqBox";
            this.FreqBox.Size = new System.Drawing.Size(30, 22);
            this.FreqBox.TabIndex = 11;
            this.FreqBox.Leave += new System.EventHandler(this.FreqBox_Leave);
            // 
            // SpeedLabel
            // 
            this.SpeedLabel.AutoSize = true;
            this.SpeedLabel.Location = new System.Drawing.Point(150, 20);
            this.SpeedLabel.Name = "SpeedLabel";
            this.SpeedLabel.Size = new System.Drawing.Size(57, 17);
            this.SpeedLabel.TabIndex = 20;
            this.SpeedLabel.Text = "Speed: ";
            // 
            // SpeedBox
            // 
            this.SpeedBox.AccessibleName = "speed";
            this.SpeedBox.Location = new System.Drawing.Point(207, 20);
            this.SpeedBox.Name = "SpeedBox";
            this.SpeedBox.Size = new System.Drawing.Size(30, 22);
            this.SpeedBox.TabIndex = 21;
            this.SpeedBox.Leave += new System.EventHandler(this.SpeedBox_Leave);
            // 
            // CodeBox
            // 
            this.CodeBox.AccessibleName = "text";
            this.CodeBox.Location = new System.Drawing.Point(0, 50);
            this.CodeBox.Multiline = true;
            this.CodeBox.Name = "CodeBox";
            this.CodeBox.Size = new System.Drawing.Size(250, 110);
            this.CodeBox.TabIndex = 80;
            this.CodeBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CodeBox_KeyPress);
            // 
            // VolumeLabel
            // 
            this.VolumeLabel.AutoSize = true;
            this.VolumeLabel.Location = new System.Drawing.Point(300, 20);
            this.VolumeLabel.Name = "VolumeLabel";
            this.VolumeLabel.Size = new System.Drawing.Size(63, 17);
            this.VolumeLabel.TabIndex = 30;
            this.VolumeLabel.Text = "Volume: ";
            // 
            // VolumeUpDown
            // 
            this.VolumeUpDown.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.VolumeUpDown.Location = new System.Drawing.Point(363, 20);
            this.VolumeUpDown.Name = "VolumeUpDown";
            this.VolumeUpDown.Size = new System.Drawing.Size(40, 22);
            this.VolumeUpDown.TabIndex = 31;
            this.VolumeUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.VolumeUpDown.ValueChanged += new System.EventHandler(this.VolumeUpDown_ValueChanged);
            // 
            // MorseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 253);
            this.Controls.Add(this.VolumeUpDown);
            this.Controls.Add(this.VolumeLabel);
            this.Controls.Add(this.CodeBox);
            this.Controls.Add(this.SpeedBox);
            this.Controls.Add(this.SpeedLabel);
            this.Controls.Add(this.FreqBox);
            this.Controls.Add(this.FreqLabel);
            this.Name = "MorseForm";
            this.Text = "Send Morse";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MorseForm_FormClosing);
            this.Load += new System.EventHandler(this.MorseForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.VolumeUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label FreqLabel;
        private System.Windows.Forms.TextBox FreqBox;
        private System.Windows.Forms.Label SpeedLabel;
        private System.Windows.Forms.TextBox SpeedBox;
        private System.Windows.Forms.TextBox CodeBox;
        private System.Windows.Forms.Label VolumeLabel;
        private System.Windows.Forms.NumericUpDown VolumeUpDown;
    }
}