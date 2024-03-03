<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ReverseBeacon
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
        Me.CallLabel = New System.Windows.Forms.Label()
        Me.CallBox = New System.Windows.Forms.TextBox()
        Me.OkButton = New System.Windows.Forms.Button()
        Me.CnclButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'CallLabel
        '
        Me.CallLabel.AutoSize = True
        Me.CallLabel.Location = New System.Drawing.Point(8, 20)
        Me.CallLabel.Name = "CallLabel"
        Me.CallLabel.Size = New System.Drawing.Size(30, 13)
        Me.CallLabel.TabIndex = 10
        Me.CallLabel.Text = "Call: "
        '
        'CallBox
        '
        Me.CallBox.AccessibleName = "call"
        Me.CallBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.CallBox.Location = New System.Drawing.Point(50, 20)
        Me.CallBox.Name = "CallBox"
        Me.CallBox.Size = New System.Drawing.Size(100, 20)
        Me.CallBox.TabIndex = 11
        '
        'OkButton
        '
        Me.OkButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.OkButton.Location = New System.Drawing.Point(8, 60)
        Me.OkButton.Name = "OkButton"
        Me.OkButton.Size = New System.Drawing.Size(75, 23)
        Me.OkButton.TabIndex = 90
        Me.OkButton.Text = "OK"
        Me.OkButton.UseVisualStyleBackColor = True
        '
        'CnclButton
        '
        Me.CnclButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.CnclButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CnclButton.Location = New System.Drawing.Point(100, 60)
        Me.CnclButton.Name = "CnclButton"
        Me.CnclButton.Size = New System.Drawing.Size(75, 23)
        Me.CnclButton.TabIndex = 99
        Me.CnclButton.Text = "Cancel"
        Me.CnclButton.UseVisualStyleBackColor = True
        '
        'ReverseBeacon
        '
        Me.AcceptButton = Me.OkButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.CnclButton
        Me.ClientSize = New System.Drawing.Size(184, 102)
        Me.Controls.Add(Me.CnclButton)
        Me.Controls.Add(Me.OkButton)
        Me.Controls.Add(Me.CallBox)
        Me.Controls.Add(Me.CallLabel)
        Me.Name = "ReverseBeacon"
        Me.Text = "Reverse Beacon"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents CallLabel As System.Windows.Forms.Label
    Friend WithEvents CallBox As System.Windows.Forms.TextBox
    Friend WithEvents OkButton As System.Windows.Forms.Button
    Friend WithEvents CnclButton As System.Windows.Forms.Button
End Class
