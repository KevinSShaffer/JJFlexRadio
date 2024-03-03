<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CWMessageUpdate
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
        Me.KeysLabel = New System.Windows.Forms.Label()
        Me.KeysList = New System.Windows.Forms.ListBox()
        Me.UpdateButton = New System.Windows.Forms.Button()
        Me.DeleteButton = New System.Windows.Forms.Button()
        Me.CnclButton = New System.Windows.Forms.Button()
        Me.AddButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'KeysLabel
        '
        Me.KeysLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.KeysLabel.AutoSize = True
        Me.KeysLabel.Location = New System.Drawing.Point(40, 20)
        Me.KeysLabel.Name = "KeysLabel"
        Me.KeysLabel.Size = New System.Drawing.Size(30, 13)
        Me.KeysLabel.TabIndex = 10
        Me.KeysLabel.Text = "Keys"
        '
        'KeysList
        '
        Me.KeysList.AccessibleName = "Keys"
        Me.KeysList.AccessibleRole = System.Windows.Forms.AccessibleRole.List
        Me.KeysList.FormattingEnabled = True
        Me.KeysList.Location = New System.Drawing.Point(8, 50)
        Me.KeysList.Name = "KeysList"
        Me.KeysList.Size = New System.Drawing.Size(200, 147)
        Me.KeysList.TabIndex = 11
        '
        'UpdateButton
        '
        Me.UpdateButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.UpdateButton.Location = New System.Drawing.Point(80, 250)
        Me.UpdateButton.Name = "UpdateButton"
        Me.UpdateButton.Size = New System.Drawing.Size(75, 23)
        Me.UpdateButton.TabIndex = 91
        Me.UpdateButton.Text = "Update"
        Me.UpdateButton.UseVisualStyleBackColor = True
        '
        'DeleteButton
        '
        Me.DeleteButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.DeleteButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.DeleteButton.Location = New System.Drawing.Point(140, 250)
        Me.DeleteButton.Name = "DeleteButton"
        Me.DeleteButton.Size = New System.Drawing.Size(75, 23)
        Me.DeleteButton.TabIndex = 92
        Me.DeleteButton.Text = "Delete"
        Me.DeleteButton.UseVisualStyleBackColor = True
        '
        'CnclButton
        '
        Me.CnclButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.CnclButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CnclButton.Location = New System.Drawing.Point(220, 250)
        Me.CnclButton.Name = "CnclButton"
        Me.CnclButton.Size = New System.Drawing.Size(75, 23)
        Me.CnclButton.TabIndex = 94
        Me.CnclButton.Text = "Cancel"
        Me.CnclButton.UseVisualStyleBackColor = True
        '
        'AddButton
        '
        Me.AddButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.AddButton.Location = New System.Drawing.Point(0, 250)
        Me.AddButton.Name = "AddButton"
        Me.AddButton.Size = New System.Drawing.Size(75, 23)
        Me.AddButton.TabIndex = 90
        Me.AddButton.Text = "Add"
        Me.AddButton.UseVisualStyleBackColor = True
        '
        'CWMessageUpdate
        '
        Me.AcceptButton = Me.UpdateButton
        Me.AccessibleRole = System.Windows.Forms.AccessibleRole.Window
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.CnclButton
        Me.ClientSize = New System.Drawing.Size(334, 312)
        Me.Controls.Add(Me.AddButton)
        Me.Controls.Add(Me.CnclButton)
        Me.Controls.Add(Me.DeleteButton)
        Me.Controls.Add(Me.UpdateButton)
        Me.Controls.Add(Me.KeysList)
        Me.Controls.Add(Me.KeysLabel)
        Me.Name = "CWMessageUpdate"
        Me.Text = "CW Messages"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents KeysLabel As System.Windows.Forms.Label
    Friend WithEvents KeysList As System.Windows.Forms.ListBox
    Friend WithEvents UpdateButton As System.Windows.Forms.Button
    Friend WithEvents DeleteButton As System.Windows.Forms.Button
    Friend WithEvents CnclButton As System.Windows.Forms.Button
    Friend WithEvents AddButton As System.Windows.Forms.Button
End Class
