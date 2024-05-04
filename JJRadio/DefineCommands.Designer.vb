<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DefineCommands
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
        Me.components = New System.ComponentModel.Container()
        Me.ValueBox = New System.Windows.Forms.TextBox()
        Me.OKButton = New System.Windows.Forms.Button()
        Me.CnclButton = New System.Windows.Forms.Button()
        Me.CommandsLabel = New System.Windows.Forms.Label()
        Me.ValueLabel = New System.Windows.Forms.Label()
        Me.PressKeyLabel = New System.Windows.Forms.Label()
        Me.CommandsListBox = New System.Windows.Forms.CheckedListBox()
        Me.DupItemsLabel = New System.Windows.Forms.Label()
        Me.CheckTimer = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'ValueBox
        '
        Me.ValueBox.AccessibleName = "press key to change"
        Me.ValueBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.ValueBox.Location = New System.Drawing.Point(320, 80)
        Me.ValueBox.Name = "ValueBox"
        Me.ValueBox.Size = New System.Drawing.Size(50, 20)
        Me.ValueBox.TabIndex = 11
        '
        'OKButton
        '
        Me.OKButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.OKButton.Location = New System.Drawing.Point(8, 300)
        Me.OKButton.Name = "OKButton"
        Me.OKButton.Size = New System.Drawing.Size(75, 23)
        Me.OKButton.TabIndex = 90
        Me.OKButton.Text = "OK"
        Me.OKButton.UseVisualStyleBackColor = True
        '
        'CnclButton
        '
        Me.CnclButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.CnclButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CnclButton.Location = New System.Drawing.Point(100, 300)
        Me.CnclButton.Name = "CnclButton"
        Me.CnclButton.Size = New System.Drawing.Size(75, 23)
        Me.CnclButton.TabIndex = 91
        Me.CnclButton.Text = "Cancel"
        Me.CnclButton.UseVisualStyleBackColor = True
        '
        'CommandsLabel
        '
        Me.CommandsLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.CommandsLabel.AutoSize = True
        Me.CommandsLabel.Location = New System.Drawing.Point(0, 60)
        Me.CommandsLabel.Name = "CommandsLabel"
        Me.CommandsLabel.Size = New System.Drawing.Size(59, 13)
        Me.CommandsLabel.TabIndex = 5
        Me.CommandsLabel.Text = "Commands"
        '
        'ValueLabel
        '
        Me.ValueLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.ValueLabel.AutoSize = True
        Me.ValueLabel.Location = New System.Drawing.Point(320, 60)
        Me.ValueLabel.Name = "ValueLabel"
        Me.ValueLabel.Size = New System.Drawing.Size(34, 13)
        Me.ValueLabel.TabIndex = 6
        Me.ValueLabel.Text = "Value"
        '
        'PressKeyLabel
        '
        Me.PressKeyLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.PressKeyLabel.AutoSize = True
        Me.PressKeyLabel.Location = New System.Drawing.Point(100, 40)
        Me.PressKeyLabel.Name = "PressKeyLabel"
        Me.PressKeyLabel.Size = New System.Drawing.Size(0, 13)
        Me.PressKeyLabel.TabIndex = 3
        '
        'CommandsListBox
        '
        Me.CommandsListBox.AccessibleName = ""
        Me.CommandsListBox.AccessibleRole = System.Windows.Forms.AccessibleRole.List
        Me.CommandsListBox.ForeColor = System.Drawing.SystemColors.WindowText
        Me.CommandsListBox.FormattingEnabled = True
        Me.CommandsListBox.Location = New System.Drawing.Point(0, 80)
        Me.CommandsListBox.Name = "CommandsListBox"
        Me.CommandsListBox.Size = New System.Drawing.Size(300, 184)
        Me.CommandsListBox.TabIndex = 10
        '
        'DupItemsLabel
        '
        Me.DupItemsLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.DupItemsLabel.AutoSize = True
        Me.DupItemsLabel.Location = New System.Drawing.Point(100, 20)
        Me.DupItemsLabel.Name = "DupItemsLabel"
        Me.DupItemsLabel.Size = New System.Drawing.Size(142, 13)
        Me.DupItemsLabel.TabIndex = 2
        Me.DupItemsLabel.Text = "Duplicate items are checked"
        '
        'CheckTimer
        '
        '
        'DefineCommands
        '
        Me.AcceptButton = Me.OKButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.CnclButton
        Me.ClientSize = New System.Drawing.Size(384, 464)
        Me.Controls.Add(Me.DupItemsLabel)
        Me.Controls.Add(Me.CommandsListBox)
        Me.Controls.Add(Me.PressKeyLabel)
        Me.Controls.Add(Me.ValueLabel)
        Me.Controls.Add(Me.CommandsLabel)
        Me.Controls.Add(Me.CnclButton)
        Me.Controls.Add(Me.OKButton)
        Me.Controls.Add(Me.ValueBox)
        Me.Name = "DefineCommands"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Define Keys"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ValueBox As System.Windows.Forms.TextBox
    Friend WithEvents OKButton As System.Windows.Forms.Button
    Friend WithEvents CnclButton As System.Windows.Forms.Button
    Friend WithEvents CommandsLabel As System.Windows.Forms.Label
    Friend WithEvents ValueLabel As System.Windows.Forms.Label
    Friend WithEvents PressKeyLabel As System.Windows.Forms.Label
    Friend WithEvents CommandsListBox As System.Windows.Forms.CheckedListBox
    Friend WithEvents DupItemsLabel As System.Windows.Forms.Label
    Friend WithEvents CheckTimer As System.Windows.Forms.Timer
End Class
