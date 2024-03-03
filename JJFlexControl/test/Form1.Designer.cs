namespace test
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
            this.OutputBox = new System.Windows.Forms.TextBox();
            this.RunButton = new System.Windows.Forms.Button();
            this.SelectPortButton = new System.Windows.Forms.Button();
            this.MapFunctionsButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // OutputBox
            // 
            this.OutputBox.AcceptsReturn = true;
            this.OutputBox.AccessibleName = "output";
            this.OutputBox.Location = new System.Drawing.Point(0, 20);
            this.OutputBox.Multiline = true;
            this.OutputBox.Name = "OutputBox";
            this.OutputBox.Size = new System.Drawing.Size(250, 170);
            this.OutputBox.TabIndex = 10;
            // 
            // RunButton
            // 
            this.RunButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.RunButton.Location = new System.Drawing.Point(0, 200);
            this.RunButton.Name = "RunButton";
            this.RunButton.Size = new System.Drawing.Size(75, 23);
            this.RunButton.TabIndex = 90;
            this.RunButton.Text = "Run";
            this.RunButton.UseVisualStyleBackColor = true;
            this.RunButton.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // SelectPortButton
            // 
            this.SelectPortButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.SelectPortButton.Location = new System.Drawing.Point(100, 200);
            this.SelectPortButton.Name = "SelectPortButton";
            this.SelectPortButton.Size = new System.Drawing.Size(75, 23);
            this.SelectPortButton.TabIndex = 93;
            this.SelectPortButton.Text = "Select port";
            this.SelectPortButton.UseVisualStyleBackColor = true;
            this.SelectPortButton.Click += new System.EventHandler(this.SelectPortButton_Click);
            // 
            // MapFunctionsButton
            // 
            this.MapFunctionsButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.MapFunctionsButton.Location = new System.Drawing.Point(200, 200);
            this.MapFunctionsButton.Name = "MapFunctionsButton";
            this.MapFunctionsButton.Size = new System.Drawing.Size(75, 23);
            this.MapFunctionsButton.TabIndex = 95;
            this.MapFunctionsButton.Text = "Map Functions";
            this.MapFunctionsButton.UseVisualStyleBackColor = true;
            this.MapFunctionsButton.Click += new System.EventHandler(this.MapFunctionsButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Controls.Add(this.MapFunctionsButton);
            this.Controls.Add(this.SelectPortButton);
            this.Controls.Add(this.RunButton);
            this.Controls.Add(this.OutputBox);
            this.Name = "Form1";
            this.Text = "Test JJFlexControl";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox OutputBox;
        private System.Windows.Forms.Button RunButton;
        private System.Windows.Forms.Button SelectPortButton;
        private System.Windows.Forms.Button MapFunctionsButton;
    }
}

