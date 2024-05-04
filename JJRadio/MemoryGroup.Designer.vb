<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MemoryGroup
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.GroupsCheckBox = New System.Windows.Forms.CheckedListBox()
        Me.MembersBox = New System.Windows.Forms.ListBox()
        Me.GroupsListBox = New System.Windows.Forms.ListBox()
        Me.SuspendLayout()
        '
        'GroupsLabel
        '
        Me.GroupsLabel.AutoSize = True
        Me.GroupsLabel.Location = New System.Drawing.Point(20, 0)
        Me.GroupsLabel.Name = "GroupsLabel"
        Me.GroupsLabel.Size = New System.Drawing.Size(41, 13)
        Me.GroupsLabel.TabIndex = 10
        Me.GroupsLabel.Text = "Groups"
        '
        'MembersLabel
        '
        Me.MembersLabel.AutoSize = True
        Me.MembersLabel.Location = New System.Drawing.Point(320, 0)
        Me.MembersLabel.Name = "MembersLabel"
        Me.MembersLabel.Size = New System.Drawing.Size(50, 13)
        Me.MembersLabel.TabIndex = 50
        Me.MembersLabel.Text = "Members"
        '
        'GroupsCheckBox
        '
        Me.GroupsCheckBox.AccessibleName = "groups"
        Me.GroupsCheckBox.AccessibleRole = System.Windows.Forms.AccessibleRole.List
        Me.GroupsCheckBox.FormattingEnabled = True
        Me.GroupsCheckBox.Location = New System.Drawing.Point(0, 20)
        Me.GroupsCheckBox.Name = "GroupsCheckBox"
        Me.GroupsCheckBox.Size = New System.Drawing.Size(300, 244)
        Me.GroupsCheckBox.TabIndex = 11
        '
        'MembersBox
        '
        Me.MembersBox.AccessibleName = "Members"
        Me.MembersBox.AccessibleRole = System.Windows.Forms.AccessibleRole.List
        Me.MembersBox.FormattingEnabled = True
        Me.MembersBox.Location = New System.Drawing.Point(300, 20)
        Me.MembersBox.Name = "MembersBox"
        Me.MembersBox.Size = New System.Drawing.Size(300, 238)
        Me.MembersBox.TabIndex = 51
        '
        'GroupsListBox
        '
        Me.GroupsListBox.AccessibleName = "groups"
        Me.GroupsListBox.AccessibleRole = System.Windows.Forms.AccessibleRole.List
        Me.GroupsListBox.Enabled = False
        Me.GroupsListBox.FormattingEnabled = True
        Me.GroupsListBox.Location = New System.Drawing.Point(0, 20)
        Me.GroupsListBox.Name = "GroupsListBox"
        Me.GroupsListBox.Size = New System.Drawing.Size(300, 238)
        Me.GroupsListBox.TabIndex = 11
        Me.GroupsListBox.Visible = False
        '
        'MemoryGroup
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.GroupsListBox)
        Me.Controls.Add(Me.GroupsLabel)
        Me.Controls.Add(Me.MembersLabel)
        Me.Controls.Add(Me.GroupsCheckBox)
        Me.Controls.Add(Me.MembersBox)
        Me.Name = "MemoryGroup"
        Me.Size = New System.Drawing.Size(600, 280)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupsLabel As System.Windows.Forms.Label
    Friend WithEvents MembersLabel As System.Windows.Forms.Label
    Friend WithEvents GroupsCheckBox As System.Windows.Forms.CheckedListBox
    Friend WithEvents MembersBox As System.Windows.Forms.ListBox
    Friend WithEvents GroupsListBox As System.Windows.Forms.ListBox

End Class
