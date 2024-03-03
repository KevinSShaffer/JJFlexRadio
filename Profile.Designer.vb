<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Profile
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
        Me.ProfilesLabel = New System.Windows.Forms.Label()
        Me.ProfilesListbox = New System.Windows.Forms.ListBox()
        Me.SelectButton = New System.Windows.Forms.Button()
        Me.CnclButton = New System.Windows.Forms.Button()
        Me.AddButton = New System.Windows.Forms.Button()
        Me.UpdateButton = New System.Windows.Forms.Button()
        Me.DeleteButton = New System.Windows.Forms.Button()
        Me.SaveButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'ProfilesLabel
        '
        Me.ProfilesLabel.AutoSize = True
        Me.ProfilesLabel.Location = New System.Drawing.Point(0, 20)
        Me.ProfilesLabel.Name = "ProfilesLabel"
        Me.ProfilesLabel.Size = New System.Drawing.Size(41, 13)
        Me.ProfilesLabel.TabIndex = 10
        Me.ProfilesLabel.Text = "Profiles"
        '
        'ProfilesListbox
        '
        Me.ProfilesListbox.AccessibleName = "profiles"
        Me.ProfilesListbox.AccessibleRole = System.Windows.Forms.AccessibleRole.List
        Me.ProfilesListbox.FormattingEnabled = True
        Me.ProfilesListbox.Location = New System.Drawing.Point(0, 35)
        Me.ProfilesListbox.Name = "ProfilesListbox"
        Me.ProfilesListbox.Size = New System.Drawing.Size(250, 95)
        Me.ProfilesListbox.TabIndex = 11
        '
        'SelectButton
        '
        Me.SelectButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.SelectButton.Location = New System.Drawing.Point(0, 180)
        Me.SelectButton.Name = "SelectButton"
        Me.SelectButton.Size = New System.Drawing.Size(75, 23)
        Me.SelectButton.TabIndex = 60
        Me.SelectButton.Text = "Select"
        Me.SelectButton.UseVisualStyleBackColor = True
        '
        'CnclButton
        '
        Me.CnclButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.CnclButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CnclButton.Location = New System.Drawing.Point(100, 210)
        Me.CnclButton.Name = "CnclButton"
        Me.CnclButton.Size = New System.Drawing.Size(75, 23)
        Me.CnclButton.TabIndex = 95
        Me.CnclButton.Text = "Cancel"
        Me.CnclButton.UseVisualStyleBackColor = True
        '
        'AddButton
        '
        Me.AddButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.AddButton.Location = New System.Drawing.Point(0, 150)
        Me.AddButton.Name = "AddButton"
        Me.AddButton.Size = New System.Drawing.Size(75, 23)
        Me.AddButton.TabIndex = 50
        Me.AddButton.Text = "Add"
        Me.AddButton.UseVisualStyleBackColor = True
        '
        'UpdateButton
        '
        Me.UpdateButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.UpdateButton.Location = New System.Drawing.Point(100, 150)
        Me.UpdateButton.Name = "UpdateButton"
        Me.UpdateButton.Size = New System.Drawing.Size(75, 23)
        Me.UpdateButton.TabIndex = 55
        Me.UpdateButton.Text = "Update"
        Me.UpdateButton.UseVisualStyleBackColor = True
        '
        'DeleteButton
        '
        Me.DeleteButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.DeleteButton.Location = New System.Drawing.Point(200, 150)
        Me.DeleteButton.Name = "DeleteButton"
        Me.DeleteButton.Size = New System.Drawing.Size(75, 23)
        Me.DeleteButton.TabIndex = 58
        Me.DeleteButton.Text = "Delete"
        Me.DeleteButton.UseVisualStyleBackColor = True
        '
        'SaveButton
        '
        Me.SaveButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.SaveButton.Enabled = False
        Me.SaveButton.Location = New System.Drawing.Point(100, 180)
        Me.SaveButton.Name = "SaveButton"
        Me.SaveButton.Size = New System.Drawing.Size(75, 23)
        Me.SaveButton.TabIndex = 65
        Me.SaveButton.Text = "Save"
        Me.SaveButton.UseVisualStyleBackColor = True
        Me.SaveButton.Visible = False
        '
        'Profile
        '
        Me.AcceptButton = Me.SelectButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.CnclButton
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.SaveButton)
        Me.Controls.Add(Me.DeleteButton)
        Me.Controls.Add(Me.UpdateButton)
        Me.Controls.Add(Me.AddButton)
        Me.Controls.Add(Me.CnclButton)
        Me.Controls.Add(Me.SelectButton)
        Me.Controls.Add(Me.ProfilesListbox)
        Me.Controls.Add(Me.ProfilesLabel)
        Me.Name = "Profile"
        Me.Text = "Profile"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ProfilesLabel As Label
    Friend WithEvents ProfilesListbox As ListBox
    Friend WithEvents SelectButton As Button
    Friend WithEvents CnclButton As Button
    Friend WithEvents AddButton As Button
    Friend WithEvents UpdateButton As Button
    Friend WithEvents DeleteButton As Button
    Friend WithEvents SaveButton As Button
End Class
