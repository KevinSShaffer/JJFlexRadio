<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TraceAdmin
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
        Me.LevelListBox = New System.Windows.Forms.ListBox()
        Me.ToggleButton = New System.Windows.Forms.Button()
        Me.CnclButton = New System.Windows.Forms.Button()
        Me.FileNameLabel = New System.Windows.Forms.Label()
        Me.FileNameBox = New System.Windows.Forms.TextBox()
        Me.BrowseButton = New System.Windows.Forms.Button()
        Me.OpenFileDialog = New System.Windows.Forms.OpenFileDialog()
        Me.SuspendLayout()
        '
        'LevelListBox
        '
        Me.LevelListBox.AccessibleName = "trace level"
        Me.LevelListBox.AccessibleRole = System.Windows.Forms.AccessibleRole.List
        Me.LevelListBox.FormattingEnabled = True
        Me.LevelListBox.Items.AddRange(New Object() {"Off", "Error", "Warning", "Info", "Verbose"})
        Me.LevelListBox.Location = New System.Drawing.Point(8, 50)
        Me.LevelListBox.Name = "LevelListBox"
        Me.LevelListBox.Size = New System.Drawing.Size(120, 95)
        Me.LevelListBox.TabIndex = 30
        '
        'ToggleButton
        '
        Me.ToggleButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.ToggleButton.AutoSize = True
        Me.ToggleButton.Location = New System.Drawing.Point(0, 220)
        Me.ToggleButton.Name = "ToggleButton"
        Me.ToggleButton.Size = New System.Drawing.Size(75, 23)
        Me.ToggleButton.TabIndex = 90
        Me.ToggleButton.UseVisualStyleBackColor = True
        '
        'CnclButton
        '
        Me.CnclButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.CnclButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CnclButton.Location = New System.Drawing.Point(100, 220)
        Me.CnclButton.Name = "CnclButton"
        Me.CnclButton.Size = New System.Drawing.Size(75, 23)
        Me.CnclButton.TabIndex = 91
        Me.CnclButton.Text = "Cancel"
        Me.CnclButton.UseVisualStyleBackColor = True
        '
        'FileNameLabel
        '
        Me.FileNameLabel.AutoSize = True
        Me.FileNameLabel.Location = New System.Drawing.Point(8, 20)
        Me.FileNameLabel.Name = "FileNameLabel"
        Me.FileNameLabel.Size = New System.Drawing.Size(60, 13)
        Me.FileNameLabel.TabIndex = 10
        Me.FileNameLabel.Text = "File Name: "
        '
        'FileNameBox
        '
        Me.FileNameBox.AccessibleName = "file name"
        Me.FileNameBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.FileNameBox.Location = New System.Drawing.Point(68, 20)
        Me.FileNameBox.Name = "FileNameBox"
        Me.FileNameBox.Size = New System.Drawing.Size(150, 20)
        Me.FileNameBox.TabIndex = 11
        '
        'BrowseButton
        '
        Me.BrowseButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.BrowseButton.AutoSize = True
        Me.BrowseButton.Location = New System.Drawing.Point(220, 20)
        Me.BrowseButton.Name = "BrowseButton"
        Me.BrowseButton.Size = New System.Drawing.Size(75, 23)
        Me.BrowseButton.TabIndex = 15
        Me.BrowseButton.Text = "Browse"
        Me.BrowseButton.UseVisualStyleBackColor = True
        '
        'OpenFileDialog
        '
        Me.OpenFileDialog.CheckFileExists = False
        Me.OpenFileDialog.DefaultExt = "txt"
        Me.OpenFileDialog.Filter = "text file (*.txt)|*.txt"
        Me.OpenFileDialog.Title = "Trace File"
        '
        'TraceAdmin
        '
        Me.AcceptButton = Me.ToggleButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.CnclButton
        Me.ClientSize = New System.Drawing.Size(284, 262)
        Me.Controls.Add(Me.BrowseButton)
        Me.Controls.Add(Me.FileNameBox)
        Me.Controls.Add(Me.FileNameLabel)
        Me.Controls.Add(Me.CnclButton)
        Me.Controls.Add(Me.ToggleButton)
        Me.Controls.Add(Me.LevelListBox)
        Me.Name = "TraceAdmin"
        Me.Text = "Select Trace Level"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents LevelListBox As System.Windows.Forms.ListBox
    Friend WithEvents ToggleButton As System.Windows.Forms.Button
    Friend WithEvents CnclButton As System.Windows.Forms.Button
    Friend WithEvents FileNameLabel As System.Windows.Forms.Label
    Friend WithEvents FileNameBox As System.Windows.Forms.TextBox
    Friend WithEvents BrowseButton As System.Windows.Forms.Button
    Friend WithEvents OpenFileDialog As System.Windows.Forms.OpenFileDialog
End Class
