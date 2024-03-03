namespace testAudioHelper
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
            this.InputButton = new System.Windows.Forms.Button();
            this.OpusInputButton = new System.Windows.Forms.Button();
            this.MessageTextBox = new System.Windows.Forms.TextBox();
            this.MorseInputButton = new System.Windows.Forms.Button();
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
            // InputButton
            // 
            this.InputButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.InputButton.AutoSize = true;
            this.InputButton.Location = new System.Drawing.Point(450, 20);
            this.InputButton.Name = "InputButton";
            this.InputButton.Size = new System.Drawing.Size(76, 27);
            this.InputButton.TabIndex = 20;
            this.InputButton.Text = "test input";
            this.InputButton.UseVisualStyleBackColor = true;
            this.InputButton.Click += new System.EventHandler(this.InputButton_Click);
            // 
            // OpusInputButton
            // 
            this.OpusInputButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.OpusInputButton.AutoSize = true;
            this.OpusInputButton.Location = new System.Drawing.Point(600, 20);
            this.OpusInputButton.Name = "OpusInputButton";
            this.OpusInputButton.Size = new System.Drawing.Size(87, 27);
            this.OpusInputButton.TabIndex = 21;
            this.OpusInputButton.Text = "Opus input";
            this.OpusInputButton.UseVisualStyleBackColor = true;
            this.OpusInputButton.Click += new System.EventHandler(this.OpusInputButton_Click);
            // 
            // MessageTextBox
            // 
            this.MessageTextBox.AcceptsReturn = true;
            this.MessageTextBox.Location = new System.Drawing.Point(0, 100);
            this.MessageTextBox.Multiline = true;
            this.MessageTextBox.Name = "MessageTextBox";
            this.MessageTextBox.Size = new System.Drawing.Size(900, 100);
            this.MessageTextBox.TabIndex = 100;
            // 
            // MorseInputButton
            // 
            this.MorseInputButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.MorseInputButton.AutoSize = true;
            this.MorseInputButton.Location = new System.Drawing.Point(0, 50);
            this.MorseInputButton.Name = "MorseInputButton";
            this.MorseInputButton.Size = new System.Drawing.Size(92, 27);
            this.MorseInputButton.TabIndex = 50;
            this.MorseInputButton.Text = "Morse input";
            this.MorseInputButton.UseVisualStyleBackColor = true;
            this.MorseInputButton.Click += new System.EventHandler(this.MorseInputButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1006, 253);
            this.Controls.Add(this.MorseInputButton);
            this.Controls.Add(this.MessageTextBox);
            this.Controls.Add(this.OpusInputButton);
            this.Controls.Add(this.InputButton);
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
        private System.Windows.Forms.Button InputButton;
        private System.Windows.Forms.Button OpusInputButton;
        private System.Windows.Forms.TextBox MessageTextBox;
        private System.Windows.Forms.Button MorseInputButton;
    }
}

