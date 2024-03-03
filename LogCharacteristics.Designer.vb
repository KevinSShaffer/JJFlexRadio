<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LogCharacteristics
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
        Me.BrowseButton = New System.Windows.Forms.Button()
        Me.DupLabel = New System.Windows.Forms.Label()
        Me.DupList = New System.Windows.Forms.ListBox()
        Me.OkButton = New System.Windows.Forms.Button()
        Me.CnclButton = New System.Windows.Forms.Button()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.FirstSerialLabel = New System.Windows.Forms.Label()
        Me.FirstSerialBox = New System.Windows.Forms.TextBox()
        Me.FormLabel = New System.Windows.Forms.Label()
        Me.FormList = New System.Windows.Forms.ListBox()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LookupLabel = New System.Windows.Forms.Label()
        Me.LookupList = New System.Windows.Forms.ListBox()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'NameLabel
        '
        Me.NameLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.NameLabel.AutoSize = True
        Me.NameLabel.Location = New System.Drawing.Point(9, 20)
        Me.NameLabel.Name = "NameLabel"
        Me.NameLabel.Size = New System.Drawing.Size(41, 13)
        Me.NameLabel.TabIndex = 10
        Me.NameLabel.Text = "Name: "
        '
        'NameBox
        '
        Me.NameBox.AccessibleName = "file name"
        Me.NameBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.NameBox.Location = New System.Drawing.Point(50, 20)
        Me.NameBox.Name = "NameBox"
        Me.NameBox.Size = New System.Drawing.Size(300, 20)
        Me.NameBox.TabIndex = 11
        '
        'BrowseButton
        '
        Me.BrowseButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.BrowseButton.Location = New System.Drawing.Point(375, 20)
        Me.BrowseButton.Name = "BrowseButton"
        Me.BrowseButton.Size = New System.Drawing.Size(75, 23)
        Me.BrowseButton.TabIndex = 12
        Me.BrowseButton.Text = "Browse"
        Me.BrowseButton.UseVisualStyleBackColor = True
        '
        'DupLabel
        '
        Me.DupLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.DupLabel.AutoSize = True
        Me.DupLabel.Location = New System.Drawing.Point(19, 100)
        Me.DupLabel.Name = "DupLabel"
        Me.DupLabel.Size = New System.Drawing.Size(81, 13)
        Me.DupLabel.TabIndex = 20
        Me.DupLabel.Text = "Dup Checking: "
        '
        'DupList
        '
        Me.DupList.AccessibleName = "dup checking"
        Me.DupList.AccessibleRole = System.Windows.Forms.AccessibleRole.List
        Me.DupList.FormattingEnabled = True
        Me.DupList.Location = New System.Drawing.Point(100, 100)
        Me.DupList.Name = "DupList"
        Me.DupList.Size = New System.Drawing.Size(80, 95)
        Me.DupList.TabIndex = 21
        '
        'OkButton
        '
        Me.OkButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.OkButton.Location = New System.Drawing.Point(20, 280)
        Me.OkButton.Name = "OkButton"
        Me.OkButton.Size = New System.Drawing.Size(75, 23)
        Me.OkButton.TabIndex = 900
        Me.OkButton.Text = "Ok"
        Me.OkButton.UseVisualStyleBackColor = True
        '
        'CnclButton
        '
        Me.CnclButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.CnclButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CnclButton.Location = New System.Drawing.Point(250, 280)
        Me.CnclButton.Name = "CnclButton"
        Me.CnclButton.Size = New System.Drawing.Size(75, 23)
        Me.CnclButton.TabIndex = 910
        Me.CnclButton.Text = "Cancel"
        Me.CnclButton.UseVisualStyleBackColor = True
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'FirstSerialLabel
        '
        Me.FirstSerialLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.FirstSerialLabel.AutoSize = True
        Me.FirstSerialLabel.Location = New System.Drawing.Point(22, 190)
        Me.FirstSerialLabel.Name = "FirstSerialLabel"
        Me.FirstSerialLabel.Size = New System.Drawing.Size(78, 13)
        Me.FirstSerialLabel.TabIndex = 30
        Me.FirstSerialLabel.Text = "Starting Serial: "
        '
        'FirstSerialBox
        '
        Me.FirstSerialBox.AccessibleName = "first serial"
        Me.FirstSerialBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.FirstSerialBox.Location = New System.Drawing.Point(100, 190)
        Me.FirstSerialBox.Name = "FirstSerialBox"
        Me.FirstSerialBox.Size = New System.Drawing.Size(50, 20)
        Me.FirstSerialBox.TabIndex = 31
        '
        'FormLabel
        '
        Me.FormLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.FormLabel.AutoSize = True
        Me.FormLabel.Location = New System.Drawing.Point(33, 50)
        Me.FormLabel.Name = "FormLabel"
        Me.FormLabel.Size = New System.Drawing.Size(67, 13)
        Me.FormLabel.TabIndex = 15
        Me.FormLabel.Text = "Form Name: "
        '
        'FormList
        '
        Me.FormList.AccessibleName = "Form list"
        Me.FormList.AccessibleRole = System.Windows.Forms.AccessibleRole.List
        Me.FormList.FormattingEnabled = True
        Me.FormList.Location = New System.Drawing.Point(100, 50)
        Me.FormList.Name = "FormList"
        Me.FormList.Size = New System.Drawing.Size(200, 43)
        Me.FormList.TabIndex = 16
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(484, 24)
        Me.MenuStrip1.TabIndex = 911
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileMenuItem
        '
        Me.FileMenuItem.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuPopup
        Me.FileMenuItem.Name = "FileMenuItem"
        Me.FileMenuItem.Size = New System.Drawing.Size(42, 20)
        Me.FileMenuItem.Text = "Files"
        '
        'LookupLabel
        '
        Me.LookupLabel.AutoSize = True
        Me.LookupLabel.Location = New System.Drawing.Point(7, 220)
        Me.LookupLabel.Name = "LookupLabel"
        Me.LookupLabel.Size = New System.Drawing.Size(93, 13)
        Me.LookupLabel.TabIndex = 40
        Me.LookupLabel.Text = "HamQTH lookup: "
        '
        'LookupList
        '
        Me.LookupList.AccessibleName = "hamQTH lookup"
        Me.LookupList.AccessibleRole = System.Windows.Forms.AccessibleRole.List
        Me.LookupList.FormattingEnabled = True
        Me.LookupList.Location = New System.Drawing.Point(100, 220)
        Me.LookupList.Name = "LookupList"
        Me.LookupList.Size = New System.Drawing.Size(30, 43)
        Me.LookupList.TabIndex = 41
        '
        'LogCharacteristics
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.CnclButton
        Me.ClientSize = New System.Drawing.Size(484, 322)
        Me.Controls.Add(Me.LookupList)
        Me.Controls.Add(Me.LookupLabel)
        Me.Controls.Add(Me.FormList)
        Me.Controls.Add(Me.FormLabel)
        Me.Controls.Add(Me.FirstSerialBox)
        Me.Controls.Add(Me.FirstSerialLabel)
        Me.Controls.Add(Me.CnclButton)
        Me.Controls.Add(Me.OkButton)
        Me.Controls.Add(Me.DupList)
        Me.Controls.Add(Me.DupLabel)
        Me.Controls.Add(Me.BrowseButton)
        Me.Controls.Add(Me.NameBox)
        Me.Controls.Add(Me.NameLabel)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "LogCharacteristics"
        Me.Text = "Log Characteristics"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents NameLabel As System.Windows.Forms.Label
    Friend WithEvents NameBox As System.Windows.Forms.TextBox
    Friend WithEvents BrowseButton As System.Windows.Forms.Button
    Friend WithEvents DupLabel As System.Windows.Forms.Label
    Friend WithEvents DupList As System.Windows.Forms.ListBox
    Friend WithEvents OkButton As System.Windows.Forms.Button
    Friend WithEvents CnclButton As System.Windows.Forms.Button
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents FirstSerialLabel As System.Windows.Forms.Label
    Friend WithEvents FirstSerialBox As System.Windows.Forms.TextBox
    Friend WithEvents FormLabel As System.Windows.Forms.Label
    Friend WithEvents FormList As System.Windows.Forms.ListBox
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FileMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LookupLabel As System.Windows.Forms.Label
    Friend WithEvents LookupList As System.Windows.Forms.ListBox
End Class
