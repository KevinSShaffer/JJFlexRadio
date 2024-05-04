<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MemoryGroups
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
        Me.GroupsLabel = New System.Windows.Forms.Label()
        Me.MembersLabel = New System.Windows.Forms.Label()
        Me.GroupsBox = New System.Windows.Forms.CheckedListBox()
        Me.MembersBox = New System.Windows.Forms.ListBox()
        Me.PlaceHolder = New System.Windows.Forms.GroupBox()
        Me.CnclButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'GroupsLabel
        '
        Me.GroupsLabel.AutoSize = True
        Me.GroupsLabel.Location = New System.Drawing.Point(20, 20)
        Me.GroupsLabel.Name = "GroupsLabel"
        Me.GroupsLabel.Size = New System.Drawing.Size(41, 13)
        Me.GroupsLabel.TabIndex = 10
        Me.GroupsLabel.Text = "Groups"
        '
        'MembersLabel
        '
        Me.MembersLabel.AutoSize = True
        Me.MembersLabel.Location = New System.Drawing.Point(320, 20)
        Me.MembersLabel.Name = "MembersLabel"
        Me.MembersLabel.Size = New System.Drawing.Size(50, 13)
        Me.MembersLabel.TabIndex = 50
        Me.MembersLabel.Text = "Members"
        '
        'GroupsBox
        '
        Me.GroupsBox.AccessibleName = "groups"
        Me.GroupsBox.AccessibleRole = System.Windows.Forms.AccessibleRole.List
        Me.GroupsBox.FormattingEnabled = True
        Me.GroupsBox.Location = New System.Drawing.Point(0, 40)
        Me.GroupsBox.Name = "GroupsBox"
        Me.GroupsBox.Size = New System.Drawing.Size(300, 244)
        Me.GroupsBox.TabIndex = 11
        '
        'MembersBox
        '
        Me.MembersBox.AccessibleName = "Members"
        Me.MembersBox.AccessibleRole = System.Windows.Forms.AccessibleRole.List
        Me.MembersBox.FormattingEnabled = True
        Me.MembersBox.Location = New System.Drawing.Point(300, 40)
        Me.MembersBox.Name = "MembersBox"
        Me.MembersBox.Size = New System.Drawing.Size(300, 238)
        Me.MembersBox.TabIndex = 51
        '
        'PlaceHolder
        '
        Me.PlaceHolder.Enabled = False
        Me.PlaceHolder.Location = New System.Drawing.Point(0, 300)
        Me.PlaceHolder.Name = "PlaceHolder"
        Me.PlaceHolder.Size = New System.Drawing.Size(600, 200)
        Me.PlaceHolder.TabIndex = 100
        Me.PlaceHolder.TabStop = False
        Me.PlaceHolder.Visible = False
        '
        'CnclButton
        '
        Me.CnclButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.CnclButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CnclButton.Location = New System.Drawing.Point(8, 520)
        Me.CnclButton.Name = "CnclButton"
        Me.CnclButton.Size = New System.Drawing.Size(75, 23)
        Me.CnclButton.TabIndex = 900
        Me.CnclButton.Text = "Cancel"
        Me.CnclButton.UseVisualStyleBackColor = True
        '
        'MemoryGroups
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.CnclButton
        Me.ClientSize = New System.Drawing.Size(584, 562)
        Me.Controls.Add(Me.CnclButton)
        Me.Controls.Add(Me.PlaceHolder)
        Me.Controls.Add(Me.MembersBox)
        Me.Controls.Add(Me.GroupsBox)
        Me.Controls.Add(Me.MembersLabel)
        Me.Controls.Add(Me.GroupsLabel)
        Me.Name = "MemoryGroups"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupsLabel As System.Windows.Forms.Label
    Friend WithEvents MembersLabel As System.Windows.Forms.Label
    Friend WithEvents GroupsBox As System.Windows.Forms.CheckedListBox
    Friend WithEvents MembersBox As System.Windows.Forms.ListBox
    Friend WithEvents PlaceHolder As System.Windows.Forms.GroupBox
    Friend WithEvents CnclButton As System.Windows.Forms.Button
End Class
