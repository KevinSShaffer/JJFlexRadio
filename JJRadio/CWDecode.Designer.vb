<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CWDecode
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
        Me.ConstrainButton = New System.Windows.Forms.Button()
        Me.ValueLabel = New System.Windows.Forms.Label()
        Me.DoneButton = New System.Windows.Forms.Button()
        Me.ValueBox = New System.Windows.Forms.TextBox()
        Me.ChangeButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'ConstrainButton
        '
        Me.ConstrainButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.ConstrainButton.AutoSize = True
        Me.ConstrainButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.ConstrainButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ConstrainButton.Location = New System.Drawing.Point(8, 20)
        Me.ConstrainButton.Name = "ConstrainButton"
        Me.ConstrainButton.Size = New System.Drawing.Size(6, 6)
        Me.ConstrainButton.TabIndex = 10
        Me.ConstrainButton.UseVisualStyleBackColor = True
        '
        'ValueLabel
        '
        Me.ValueLabel.AutoSize = True
        Me.ValueLabel.Location = New System.Drawing.Point(100, 20)
        Me.ValueLabel.Name = "ValueLabel"
        Me.ValueLabel.Size = New System.Drawing.Size(40, 13)
        Me.ValueLabel.TabIndex = 20
        Me.ValueLabel.Text = "Value: "
        '
        'DoneButton
        '
        Me.DoneButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.DoneButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.DoneButton.Location = New System.Drawing.Point(100, 40)
        Me.DoneButton.Name = "DoneButton"
        Me.DoneButton.Size = New System.Drawing.Size(75, 23)
        Me.DoneButton.TabIndex = 99
        Me.DoneButton.Text = "Cancel"
        Me.DoneButton.UseVisualStyleBackColor = True
        '
        'ValueBox
        '
        Me.ValueBox.AccessibleName = "value"
        Me.ValueBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.ValueBox.Location = New System.Drawing.Point(140, 20)
        Me.ValueBox.Name = "ValueBox"
        Me.ValueBox.Size = New System.Drawing.Size(50, 20)
        Me.ValueBox.TabIndex = 21
        '
        'ChangeButton
        '
        Me.ChangeButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.ChangeButton.Location = New System.Drawing.Point(8, 40)
        Me.ChangeButton.Name = "ChangeButton"
        Me.ChangeButton.Size = New System.Drawing.Size(75, 23)
        Me.ChangeButton.TabIndex = 90
        Me.ChangeButton.Text = "Change"
        Me.ChangeButton.UseVisualStyleBackColor = True
        '
        'CWDecode
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.DoneButton
        Me.ClientSize = New System.Drawing.Size(284, 262)
        Me.Controls.Add(Me.ChangeButton)
        Me.Controls.Add(Me.ValueBox)
        Me.Controls.Add(Me.DoneButton)
        Me.Controls.Add(Me.ValueLabel)
        Me.Controls.Add(Me.ConstrainButton)
        Me.Name = "CWDecode"
        Me.Text = "CW Decode Display"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ConstrainButton As System.Windows.Forms.Button
    Friend WithEvents ValueLabel As System.Windows.Forms.Label
    Friend WithEvents DoneButton As System.Windows.Forms.Button
    Friend WithEvents ValueBox As System.Windows.Forms.TextBox
    Friend WithEvents ChangeButton As System.Windows.Forms.Button
End Class
