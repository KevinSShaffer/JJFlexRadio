namespace MsgLib
{
    partial class MessageForm
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
            this.DontshowBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // DontshowBox
            // 
            this.DontshowBox.AccessibleRole = System.Windows.Forms.AccessibleRole.CheckButton;
            this.DontshowBox.AutoSize = true;
            this.DontshowBox.Location = new System.Drawing.Point(8, 210);
            this.DontshowBox.Name = "DontshowBox";
            this.DontshowBox.Size = new System.Drawing.Size(172, 17);
            this.DontshowBox.TabIndex = 90;
            this.DontshowBox.Text = "Don\'t show this message again";
            this.DontshowBox.UseVisualStyleBackColor = true;
            // 
            // MessageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.DontshowBox);
            this.Name = "MessageForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Message Form";
            this.Activated += new System.EventHandler(this.MessageForm_Activated);
            this.Load += new System.EventHandler(this.MessageForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox DontshowBox;
    }
}