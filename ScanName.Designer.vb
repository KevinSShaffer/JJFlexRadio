<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ScanName
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
        Me.ScanNameLabel = New System.Windows.Forms.Label()
        Me.ScanNameBox = New System.Windows.Forms.TextBox()
        Me.OKButton = New System.Windows.Forms.Button()
        Me.CnclButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'ScanNameLabel
        '
        Me.ScanNameLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.ScanNameLabel.AutoSize = True
        Me.ScanNameLabel.Location = New System.Drawing.Point(8, 20)
        Me.ScanNameLabel.Name = "ScanNameLabel"
        Me.ScanNameLabel.Size = New System.Drawing.Size(67, 13)
        Me.ScanNameLabel.TabIndex = 0
        Me.ScanNameLabel.Text = "Scan name: "
        '
        'ScanNameBox
        '
        Me.ScanNameBox.AccessibleName = "Scan name"
        Me.ScanNameBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.ScanNameBox.Location = New System.Drawing.Point(75, 20)
        Me.ScanNameBox.Name = "ScanNameBox"
        Me.ScanNameBox.Size = New System.Drawing.Size(100, 20)
        Me.ScanNameBox.TabIndex = 1
        '
        'OKButton
        '
        Me.OKButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.OKButton.AutoSize = True
        Me.OKButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.OKButton.Location = New System.Drawing.Point(8, 50)
        Me.OKButton.Name = "OKButton"
        Me.OKButton.Size = New System.Drawing.Size(32, 23)
        Me.OKButton.TabIndex = 2
        Me.OKButton.Text = "OK"
        Me.OKButton.UseVisualStyleBackColor = True
        '
        'CnclButton
        '
        Me.CnclButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.CnclButton.AutoSize = True
        Me.CnclButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.CnclButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CnclButton.Location = New System.Drawing.Point(75, 50)
        Me.CnclButton.Name = "CnclButton"
        Me.CnclButton.Size = New System.Drawing.Size(50, 23)
        Me.CnclButton.TabIndex = 3
        Me.CnclButton.Text = "Cancel"
        Me.CnclButton.UseVisualStyleBackColor = True
        '
        'ScanName
        '
        Me.AcceptButton = Me.OKButton
        Me.AccessibleRole = System.Windows.Forms.AccessibleRole.Window
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.CnclButton
        Me.ClientSize = New System.Drawing.Size(234, 64)
        Me.Controls.Add(Me.CnclButton)
        Me.Controls.Add(Me.OKButton)
        Me.Controls.Add(Me.ScanNameBox)
        Me.Controls.Add(Me.ScanNameLabel)
        Me.Name = "ScanName"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Name this scan"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ScanNameLabel As System.Windows.Forms.Label
    Friend WithEvents ScanNameBox As System.Windows.Forms.TextBox
    Friend WithEvents OKButton As System.Windows.Forms.Button
    Friend WithEvents CnclButton As System.Windows.Forms.Button
End Class
