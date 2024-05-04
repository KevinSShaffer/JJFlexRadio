<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ManageGroupsEdit
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
        Me.NameLabel = New System.Windows.Forms.Label()
        Me.NameBox = New System.Windows.Forms.TextBox()
        Me.MembersLabel = New System.Windows.Forms.Label()
        Me.MembersBox = New System.Windows.Forms.CheckedListBox()
        Me.OkButton = New System.Windows.Forms.Button()
        Me.CnclButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'NameLabel
        '
        Me.NameLabel.AutoSize = True
        Me.NameLabel.Location = New System.Drawing.Point(0, 20)
        Me.NameLabel.Name = "NameLabel"
        Me.NameLabel.Size = New System.Drawing.Size(71, 13)
        Me.NameLabel.TabIndex = 10
        Me.NameLabel.Text = "Group name: "
        '
        'NameBox
        '
        Me.NameBox.AccessibleName = "group name"
        Me.NameBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.NameBox.Location = New System.Drawing.Point(71, 20)
        Me.NameBox.Name = "NameBox"
        Me.NameBox.Size = New System.Drawing.Size(200, 20)
        Me.NameBox.TabIndex = 11
        '
        'MembersLabel
        '
        Me.MembersLabel.AutoSize = True
        Me.MembersLabel.Location = New System.Drawing.Point(0, 45)
        Me.MembersLabel.Name = "MembersLabel"
        Me.MembersLabel.Size = New System.Drawing.Size(84, 13)
        Me.MembersLabel.TabIndex = 20
        Me.MembersLabel.Text = "Group members:"
        '
        'MembersBox
        '
        Me.MembersBox.AccessibleName = "members"
        Me.MembersBox.AccessibleRole = System.Windows.Forms.AccessibleRole.List
        Me.MembersBox.FormattingEnabled = True
        Me.MembersBox.Location = New System.Drawing.Point(8, 60)
        Me.MembersBox.Name = "MembersBox"
        Me.MembersBox.Size = New System.Drawing.Size(250, 334)
        Me.MembersBox.TabIndex = 21
        '
        'OkButton
        '
        Me.OkButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.OkButton.Location = New System.Drawing.Point(8, 420)
        Me.OkButton.Name = "OkButton"
        Me.OkButton.Size = New System.Drawing.Size(75, 23)
        Me.OkButton.TabIndex = 90
        Me.OkButton.Text = "Ok"
        Me.OkButton.UseVisualStyleBackColor = True
        '
        'CnclButton
        '
        Me.CnclButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.CnclButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CnclButton.Location = New System.Drawing.Point(100, 420)
        Me.CnclButton.Name = "CnclButton"
        Me.CnclButton.Size = New System.Drawing.Size(75, 23)
        Me.CnclButton.TabIndex = 95
        Me.CnclButton.Text = "Cancel"
        Me.CnclButton.UseVisualStyleBackColor = True
        '
        'ManageGroupsEdit
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.CnclButton
        Me.ClientSize = New System.Drawing.Size(284, 462)
        Me.Controls.Add(Me.CnclButton)
        Me.Controls.Add(Me.OkButton)
        Me.Controls.Add(Me.MembersBox)
        Me.Controls.Add(Me.MembersLabel)
        Me.Controls.Add(Me.NameBox)
        Me.Controls.Add(Me.NameLabel)
        Me.Name = "ManageGroupsEdit"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents NameLabel As System.Windows.Forms.Label
    Friend WithEvents NameBox As System.Windows.Forms.TextBox
    Friend WithEvents MembersLabel As System.Windows.Forms.Label
    Friend WithEvents MembersBox As System.Windows.Forms.CheckedListBox
    Friend WithEvents OkButton As System.Windows.Forms.Button
    Friend WithEvents CnclButton As System.Windows.Forms.Button
End Class
