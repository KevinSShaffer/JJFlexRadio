namespace Radios
{
    partial class TS2000Filters
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
            this.FilterControl = new RadioBoxes.Combo();
            this.CWOffsetControl = new RadioBoxes.Combo();
            this.CWWidthControl = new RadioBoxes.Combo();
            this.SSBFMFMDLowControl = new RadioBoxes.Combo();
            this.SSBFMFMDHighControl = new RadioBoxes.Combo();
            this.FSKWidthControl = new RadioBoxes.Combo();
            this.SSBDOffsetControl = new RadioBoxes.Combo();
            this.SSBDWidthControl = new RadioBoxes.Combo();
            this.AMLowControl = new RadioBoxes.Combo();
            this.AMHighControl = new RadioBoxes.Combo();
            this.SWRBox = new System.Windows.Forms.TextBox();
            this.SWRLabel = new System.Windows.Forms.Label();
            this.CompLabel = new System.Windows.Forms.Label();
            this.CompBox = new System.Windows.Forms.TextBox();
            this.ALCLabel = new System.Windows.Forms.Label();
            this.ALCBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // FilterControl
            // 
            this.FilterControl.AccessibleName = "filter";
            this.FilterControl.AccessibleRole = System.Windows.Forms.AccessibleRole.ComboBox;
            this.FilterControl.BoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FilterControl.ExpandedSize = new System.Drawing.Size(50, 80);
            this.FilterControl.Header = "filter";
            this.FilterControl.Location = new System.Drawing.Point(0, 0);
            this.FilterControl.Name = "FilterControl";
            this.FilterControl.Size = new System.Drawing.Size(50, 36);
            this.FilterControl.SmallSize = new System.Drawing.Size(50, 36);
            this.FilterControl.TabIndex = 0;
            this.FilterControl.TheList = null;
            this.FilterControl.UpdateDisplayFunction = null;
            this.FilterControl.UpdateRigFunction = null;
            this.FilterControl.BoxKeydown += new System.Windows.Forms.KeyEventHandler(this.BoxKeydownDefault);
            // 
            // CWOffsetControl
            // 
            this.CWOffsetControl.AccessibleName = "offset";
            this.CWOffsetControl.AccessibleRole = System.Windows.Forms.AccessibleRole.ComboBox;
            this.CWOffsetControl.BoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CWOffsetControl.Enabled = false;
            this.CWOffsetControl.ExpandedSize = new System.Drawing.Size(50, 100);
            this.CWOffsetControl.Header = "offset";
            this.CWOffsetControl.Location = new System.Drawing.Point(140, 0);
            this.CWOffsetControl.Name = "CWOffsetControl";
            this.CWOffsetControl.Size = new System.Drawing.Size(50, 36);
            this.CWOffsetControl.SmallSize = new System.Drawing.Size(50, 36);
            this.CWOffsetControl.TabIndex = 20;
            this.CWOffsetControl.TheList = null;
            this.CWOffsetControl.UpdateDisplayFunction = null;
            this.CWOffsetControl.UpdateRigFunction = null;
            this.CWOffsetControl.Visible = false;
            this.CWOffsetControl.BoxKeydown += new System.Windows.Forms.KeyEventHandler(this.BoxKeydownDefault);
            // 
            // CWWidthControl
            // 
            this.CWWidthControl.AccessibleName = "width";
            this.CWWidthControl.AccessibleRole = System.Windows.Forms.AccessibleRole.ComboBox;
            this.CWWidthControl.BoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CWWidthControl.Enabled = false;
            this.CWWidthControl.ExpandedSize = new System.Drawing.Size(50, 100);
            this.CWWidthControl.Header = "width";
            this.CWWidthControl.Location = new System.Drawing.Point(70, 0);
            this.CWWidthControl.Name = "CWWidthControl";
            this.CWWidthControl.Size = new System.Drawing.Size(50, 36);
            this.CWWidthControl.SmallSize = new System.Drawing.Size(50, 36);
            this.CWWidthControl.TabIndex = 10;
            this.CWWidthControl.TheList = null;
            this.CWWidthControl.UpdateDisplayFunction = null;
            this.CWWidthControl.UpdateRigFunction = null;
            this.CWWidthControl.Visible = false;
            this.CWWidthControl.BoxKeydown += new System.Windows.Forms.KeyEventHandler(this.BoxKeydownDefault);
            // 
            // SSBFMFMDLowControl
            // 
            this.SSBFMFMDLowControl.AccessibleName = "low";
            this.SSBFMFMDLowControl.AccessibleRole = System.Windows.Forms.AccessibleRole.ComboBox;
            this.SSBFMFMDLowControl.BoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SSBFMFMDLowControl.Enabled = false;
            this.SSBFMFMDLowControl.ExpandedSize = new System.Drawing.Size(50, 100);
            this.SSBFMFMDLowControl.Header = "low";
            this.SSBFMFMDLowControl.Location = new System.Drawing.Point(70, 0);
            this.SSBFMFMDLowControl.Name = "SSBFMFMDLowControl";
            this.SSBFMFMDLowControl.Size = new System.Drawing.Size(50, 36);
            this.SSBFMFMDLowControl.SmallSize = new System.Drawing.Size(50, 36);
            this.SSBFMFMDLowControl.TabIndex = 10;
            this.SSBFMFMDLowControl.TheList = null;
            this.SSBFMFMDLowControl.UpdateDisplayFunction = null;
            this.SSBFMFMDLowControl.UpdateRigFunction = null;
            this.SSBFMFMDLowControl.Visible = false;
            this.SSBFMFMDLowControl.BoxKeydown += new System.Windows.Forms.KeyEventHandler(this.BoxKeydownDefault);
            // 
            // SSBFMFMDHighControl
            // 
            this.SSBFMFMDHighControl.AccessibleName = "high";
            this.SSBFMFMDHighControl.AccessibleRole = System.Windows.Forms.AccessibleRole.ComboBox;
            this.SSBFMFMDHighControl.BoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SSBFMFMDHighControl.Enabled = false;
            this.SSBFMFMDHighControl.ExpandedSize = new System.Drawing.Size(50, 100);
            this.SSBFMFMDHighControl.Header = "high";
            this.SSBFMFMDHighControl.Location = new System.Drawing.Point(140, 0);
            this.SSBFMFMDHighControl.Name = "SSBFMFMDHighControl";
            this.SSBFMFMDHighControl.Size = new System.Drawing.Size(50, 36);
            this.SSBFMFMDHighControl.SmallSize = new System.Drawing.Size(50, 36);
            this.SSBFMFMDHighControl.TabIndex = 20;
            this.SSBFMFMDHighControl.TheList = null;
            this.SSBFMFMDHighControl.UpdateDisplayFunction = null;
            this.SSBFMFMDHighControl.UpdateRigFunction = null;
            this.SSBFMFMDHighControl.Visible = false;
            this.SSBFMFMDHighControl.BoxKeydown += new System.Windows.Forms.KeyEventHandler(this.BoxKeydownDefault);
            // 
            // FSKWidthControl
            // 
            this.FSKWidthControl.AccessibleName = "width";
            this.FSKWidthControl.AccessibleRole = System.Windows.Forms.AccessibleRole.ComboBox;
            this.FSKWidthControl.BoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FSKWidthControl.Enabled = false;
            this.FSKWidthControl.ExpandedSize = new System.Drawing.Size(50, 100);
            this.FSKWidthControl.Header = "width";
            this.FSKWidthControl.Location = new System.Drawing.Point(140, 0);
            this.FSKWidthControl.Name = "FSKWidthControl";
            this.FSKWidthControl.Size = new System.Drawing.Size(50, 36);
            this.FSKWidthControl.SmallSize = new System.Drawing.Size(50, 36);
            this.FSKWidthControl.TabIndex = 20;
            this.FSKWidthControl.TheList = null;
            this.FSKWidthControl.UpdateDisplayFunction = null;
            this.FSKWidthControl.UpdateRigFunction = null;
            this.FSKWidthControl.Visible = false;
            this.FSKWidthControl.BoxKeydown += new System.Windows.Forms.KeyEventHandler(this.BoxKeydownDefault);
            // 
            // SSBDOffsetControl
            // 
            this.SSBDOffsetControl.AccessibleName = "shift";
            this.SSBDOffsetControl.AccessibleRole = System.Windows.Forms.AccessibleRole.ComboBox;
            this.SSBDOffsetControl.BoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SSBDOffsetControl.Enabled = false;
            this.SSBDOffsetControl.ExpandedSize = new System.Drawing.Size(50, 100);
            this.SSBDOffsetControl.Header = "shift";
            this.SSBDOffsetControl.Location = new System.Drawing.Point(70, 0);
            this.SSBDOffsetControl.Name = "SSBDOffsetControl";
            this.SSBDOffsetControl.Size = new System.Drawing.Size(50, 36);
            this.SSBDOffsetControl.SmallSize = new System.Drawing.Size(50, 36);
            this.SSBDOffsetControl.TabIndex = 10;
            this.SSBDOffsetControl.TheList = null;
            this.SSBDOffsetControl.UpdateDisplayFunction = null;
            this.SSBDOffsetControl.UpdateRigFunction = null;
            this.SSBDOffsetControl.Visible = false;
            this.SSBDOffsetControl.BoxKeydown += new System.Windows.Forms.KeyEventHandler(this.BoxKeydownDefault);
            // 
            // SSBDWidthControl
            // 
            this.SSBDWidthControl.AccessibleName = "width";
            this.SSBDWidthControl.AccessibleRole = System.Windows.Forms.AccessibleRole.ComboBox;
            this.SSBDWidthControl.BoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SSBDWidthControl.Enabled = false;
            this.SSBDWidthControl.ExpandedSize = new System.Drawing.Size(50, 100);
            this.SSBDWidthControl.Header = "width";
            this.SSBDWidthControl.Location = new System.Drawing.Point(140, 0);
            this.SSBDWidthControl.Name = "SSBDWidthControl";
            this.SSBDWidthControl.Size = new System.Drawing.Size(50, 36);
            this.SSBDWidthControl.SmallSize = new System.Drawing.Size(50, 36);
            this.SSBDWidthControl.TabIndex = 20;
            this.SSBDWidthControl.TheList = null;
            this.SSBDWidthControl.UpdateDisplayFunction = null;
            this.SSBDWidthControl.UpdateRigFunction = null;
            this.SSBDWidthControl.Visible = false;
            this.SSBDWidthControl.BoxKeydown += new System.Windows.Forms.KeyEventHandler(this.BoxKeydownDefault);
            // 
            // AMLowControl
            // 
            this.AMLowControl.AccessibleName = "low";
            this.AMLowControl.AccessibleRole = System.Windows.Forms.AccessibleRole.ComboBox;
            this.AMLowControl.BoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AMLowControl.Enabled = false;
            this.AMLowControl.ExpandedSize = new System.Drawing.Size(50, 100);
            this.AMLowControl.Header = "low";
            this.AMLowControl.Location = new System.Drawing.Point(70, 0);
            this.AMLowControl.Name = "AMLowControl";
            this.AMLowControl.Size = new System.Drawing.Size(50, 36);
            this.AMLowControl.SmallSize = new System.Drawing.Size(50, 36);
            this.AMLowControl.TabIndex = 10;
            this.AMLowControl.TheList = null;
            this.AMLowControl.UpdateDisplayFunction = null;
            this.AMLowControl.UpdateRigFunction = null;
            this.AMLowControl.Visible = false;
            this.AMLowControl.BoxKeydown += new System.Windows.Forms.KeyEventHandler(this.BoxKeydownDefault);
            // 
            // AMHighControl
            // 
            this.AMHighControl.AccessibleName = "high";
            this.AMHighControl.AccessibleRole = System.Windows.Forms.AccessibleRole.ComboBox;
            this.AMHighControl.BoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AMHighControl.Enabled = false;
            this.AMHighControl.ExpandedSize = new System.Drawing.Size(50, 100);
            this.AMHighControl.Header = "high";
            this.AMHighControl.Location = new System.Drawing.Point(140, 0);
            this.AMHighControl.Name = "AMHighControl";
            this.AMHighControl.Size = new System.Drawing.Size(50, 36);
            this.AMHighControl.SmallSize = new System.Drawing.Size(50, 36);
            this.AMHighControl.TabIndex = 20;
            this.AMHighControl.TheList = null;
            this.AMHighControl.UpdateDisplayFunction = null;
            this.AMHighControl.UpdateRigFunction = null;
            this.AMHighControl.Visible = false;
            this.AMHighControl.BoxKeydown += new System.Windows.Forms.KeyEventHandler(this.BoxKeydownDefault);
            // 
            // SWRBox
            // 
            this.SWRBox.AccessibleName = "SWR";
            this.SWRBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.SWRBox.Location = new System.Drawing.Point(210, 16);
            this.SWRBox.Name = "SWRBox";
            this.SWRBox.ReadOnly = true;
            this.SWRBox.Size = new System.Drawing.Size(50, 20);
            this.SWRBox.TabIndex = 31;
            this.SWRBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BoxKeydownDefault);
            // 
            // SWRLabel
            // 
            this.SWRLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.SWRLabel.Location = new System.Drawing.Point(210, 0);
            this.SWRLabel.Name = "SWRLabel";
            this.SWRLabel.Size = new System.Drawing.Size(50, 16);
            this.SWRLabel.TabIndex = 30;
            this.SWRLabel.Text = "SWR";
            // 
            // CompLabel
            // 
            this.CompLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.CompLabel.Location = new System.Drawing.Point(280, 0);
            this.CompLabel.Name = "CompLabel";
            this.CompLabel.Size = new System.Drawing.Size(50, 16);
            this.CompLabel.TabIndex = 40;
            this.CompLabel.Text = "Comp";
            // 
            // CompBox
            // 
            this.CompBox.AccessibleName = "compression";
            this.CompBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.CompBox.Location = new System.Drawing.Point(280, 16);
            this.CompBox.Name = "CompBox";
            this.CompBox.ReadOnly = true;
            this.CompBox.Size = new System.Drawing.Size(50, 20);
            this.CompBox.TabIndex = 41;
            this.CompBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BoxKeydownDefault);
            // 
            // ALCLabel
            // 
            this.ALCLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.ALCLabel.Location = new System.Drawing.Point(350, 0);
            this.ALCLabel.Name = "ALCLabel";
            this.ALCLabel.Size = new System.Drawing.Size(50, 16);
            this.ALCLabel.TabIndex = 50;
            this.ALCLabel.Text = "ALC";
            // 
            // ALCBox
            // 
            this.ALCBox.AccessibleName = "ALC";
            this.ALCBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.ALCBox.Location = new System.Drawing.Point(350, 16);
            this.ALCBox.Name = "ALCBox";
            this.ALCBox.ReadOnly = true;
            this.ALCBox.Size = new System.Drawing.Size(50, 20);
            this.ALCBox.TabIndex = 51;
            this.ALCBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BoxKeydownDefault);
            // 
            // TS2000Filters
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ALCBox);
            this.Controls.Add(this.ALCLabel);
            this.Controls.Add(this.CompBox);
            this.Controls.Add(this.CompLabel);
            this.Controls.Add(this.SWRLabel);
            this.Controls.Add(this.SWRBox);
            this.Controls.Add(this.AMHighControl);
            this.Controls.Add(this.AMLowControl);
            this.Controls.Add(this.SSBDWidthControl);
            this.Controls.Add(this.SSBDOffsetControl);
            this.Controls.Add(this.FSKWidthControl);
            this.Controls.Add(this.SSBFMFMDHighControl);
            this.Controls.Add(this.SSBFMFMDLowControl);
            this.Controls.Add(this.CWWidthControl);
            this.Controls.Add(this.CWOffsetControl);
            this.Controls.Add(this.FilterControl);
            this.Name = "TS2000Filters";
            this.Size = new System.Drawing.Size(500, 40);
            this.Load += new System.EventHandler(this.Filters_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RadioBoxes.Combo FilterControl;
        private RadioBoxes.Combo CWOffsetControl;
        private RadioBoxes.Combo CWWidthControl;
        private RadioBoxes.Combo SSBFMFMDLowControl;
        private RadioBoxes.Combo SSBFMFMDHighControl;
        private RadioBoxes.Combo FSKWidthControl;
        private RadioBoxes.Combo SSBDOffsetControl;
        private RadioBoxes.Combo SSBDWidthControl;
        private RadioBoxes.Combo AMLowControl;
        private RadioBoxes.Combo AMHighControl;
        private System.Windows.Forms.TextBox SWRBox;
        private System.Windows.Forms.Label SWRLabel;
        private System.Windows.Forms.Label CompLabel;
        private System.Windows.Forms.TextBox CompBox;
        private System.Windows.Forms.Label ALCLabel;
        private System.Windows.Forms.TextBox ALCBox;
    }
}
