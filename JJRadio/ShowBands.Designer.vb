<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ShowBands
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.BandLabel = New System.Windows.Forms.Label()
        Me.LicenseLabel = New System.Windows.Forms.Label()
        Me.ModeLabel = New System.Windows.Forms.Label()
        Me.BandBox = New System.Windows.Forms.ListBox()
        Me.LicenseBox = New System.Windows.Forms.ListBox()
        Me.ModeBox = New System.Windows.Forms.ListBox()
        Me.ResultLabel = New System.Windows.Forms.Label()
        Me.ResultBox = New System.Windows.Forms.TextBox()
        Me.ShowButton = New System.Windows.Forms.Button()
        Me.CnclButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'BandLabel
        '
        Me.BandLabel.AutoSize = True
        Me.BandLabel.Location = New System.Drawing.Point(0, 20)
        Me.BandLabel.Name = "BandLabel"
        Me.BandLabel.Size = New System.Drawing.Size(32, 13)
        Me.BandLabel.TabIndex = 10
        Me.BandLabel.Text = "Band"
        '
        'LicenseLabel
        '
        Me.LicenseLabel.AutoSize = True
        Me.LicenseLabel.Location = New System.Drawing.Point(70, 20)
        Me.LicenseLabel.Name = "LicenseLabel"
        Me.LicenseLabel.Size = New System.Drawing.Size(44, 13)
        Me.LicenseLabel.TabIndex = 20
        Me.LicenseLabel.Text = "License"
        '
        'ModeLabel
        '
        Me.ModeLabel.AutoSize = True
        Me.ModeLabel.Location = New System.Drawing.Point(200, 20)
        Me.ModeLabel.Name = "ModeLabel"
        Me.ModeLabel.Size = New System.Drawing.Size(34, 13)
        Me.ModeLabel.TabIndex = 30
        Me.ModeLabel.Text = "Mode"
        '
        'BandBox
        '
        Me.BandBox.AccessibleName = "band"
        Me.BandBox.AccessibleRole = System.Windows.Forms.AccessibleRole.List
        Me.BandBox.FormattingEnabled = True
        Me.BandBox.Location = New System.Drawing.Point(0, 35)
        Me.BandBox.Name = "BandBox"
        Me.BandBox.Size = New System.Drawing.Size(50, 95)
        Me.BandBox.TabIndex = 11
        '
        'LicenseBox
        '
        Me.LicenseBox.AccessibleName = "license"
        Me.LicenseBox.AccessibleRole = System.Windows.Forms.AccessibleRole.List
        Me.LicenseBox.FormattingEnabled = True
        Me.LicenseBox.Location = New System.Drawing.Point(70, 35)
        Me.LicenseBox.Name = "LicenseBox"
        Me.LicenseBox.Size = New System.Drawing.Size(120, 95)
        Me.LicenseBox.TabIndex = 21
        '
        'ModeBox
        '
        Me.ModeBox.AccessibleName = "mode"
        Me.ModeBox.AccessibleRole = System.Windows.Forms.AccessibleRole.List
        Me.ModeBox.FormattingEnabled = True
        Me.ModeBox.Location = New System.Drawing.Point(200, 35)
        Me.ModeBox.Name = "ModeBox"
        Me.ModeBox.Size = New System.Drawing.Size(50, 95)
        Me.ModeBox.TabIndex = 31
        '
        'ResultLabel
        '
        Me.ResultLabel.AutoSize = True
        Me.ResultLabel.Location = New System.Drawing.Point(70, 135)
        Me.ResultLabel.Name = "ResultLabel"
        Me.ResultLabel.Size = New System.Drawing.Size(144, 13)
        Me.ResultLabel.TabIndex = 100
        Me.ResultLabel.Text = "band frequencies and modes"
        '
        'ResultBox
        '
        Me.ResultBox.AccessibleName = "result"
        Me.ResultBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.ResultBox.Location = New System.Drawing.Point(0, 150)
        Me.ResultBox.Multiline = True
        Me.ResultBox.Name = "ResultBox"
        Me.ResultBox.Size = New System.Drawing.Size(300, 100)
        Me.ResultBox.TabIndex = 101
        '
        'ShowButton
        '
        Me.ShowButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.ShowButton.Location = New System.Drawing.Point(8, 320)
        Me.ShowButton.Name = "ShowButton"
        Me.ShowButton.Size = New System.Drawing.Size(75, 23)
        Me.ShowButton.TabIndex = 900
        Me.ShowButton.Text = "Show"
        Me.ShowButton.UseVisualStyleBackColor = True
        '
        'CnclButton
        '
        Me.CnclButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.CnclButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CnclButton.Location = New System.Drawing.Point(200, 320)
        Me.CnclButton.Name = "CnclButton"
        Me.CnclButton.Size = New System.Drawing.Size(75, 23)
        Me.CnclButton.TabIndex = 910
        Me.CnclButton.Text = "Cancel"
        Me.CnclButton.UseVisualStyleBackColor = True
        '
        'ShowBands
        '
        Me.AcceptButton = Me.ShowButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.CnclButton
        Me.ClientSize = New System.Drawing.Size(384, 462)
        Me.Controls.Add(Me.CnclButton)
        Me.Controls.Add(Me.ShowButton)
        Me.Controls.Add(Me.ResultBox)
        Me.Controls.Add(Me.ResultLabel)
        Me.Controls.Add(Me.ModeBox)
        Me.Controls.Add(Me.LicenseBox)
        Me.Controls.Add(Me.BandBox)
        Me.Controls.Add(Me.ModeLabel)
        Me.Controls.Add(Me.LicenseLabel)
        Me.Controls.Add(Me.BandLabel)
        Me.Name = "ShowBands"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Band Frequencies"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents BandLabel As System.Windows.Forms.Label
    Friend WithEvents LicenseLabel As System.Windows.Forms.Label
    Friend WithEvents ModeLabel As System.Windows.Forms.Label
    Friend WithEvents BandBox As System.Windows.Forms.ListBox
    Friend WithEvents LicenseBox As System.Windows.Forms.ListBox
    Friend WithEvents ModeBox As System.Windows.Forms.ListBox
    Friend WithEvents ResultLabel As System.Windows.Forms.Label
    Friend WithEvents ResultBox As System.Windows.Forms.TextBox
    Friend WithEvents ShowButton As System.Windows.Forms.Button
    Friend WithEvents CnclButton As System.Windows.Forms.Button
End Class
