namespace MsgLib
{
    partial class DefaultUserControl
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
            this.MessageText = new System.Windows.Forms.Label();
            this.OKButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // MessageText
            // 
            this.MessageText.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.MessageText.AutoSize = true;
            this.MessageText.Location = new System.Drawing.Point(0, 80);
            this.MessageText.Name = "MessageText";
            this.MessageText.Size = new System.Drawing.Size(0, 13);
            this.MessageText.TabIndex = 0;
            // 
            // OKButton
            // 
            this.OKButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.OKButton.Location = new System.Drawing.Point(100, 170);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 90;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // DefaultUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.MessageText);
            this.Name = "DefaultUserControl";
            this.Size = new System.Drawing.Size(300, 200);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label MessageText;
        private System.Windows.Forms.Button OKButton;
    }
}
