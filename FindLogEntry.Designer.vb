<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FindLogEntry
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
        Me.ItemListLabel = New System.Windows.Forms.Label()
        Me.ItemList = New System.Windows.Forms.ListBox()
        Me.DoneButton = New System.Windows.Forms.Button()
        Me.CheckQTimer = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'ItemListLabel
        '
        Me.ItemListLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.ItemListLabel.AutoSize = True
        Me.ItemListLabel.Location = New System.Drawing.Point(8, 20)
        Me.ItemListLabel.Name = "ItemListLabel"
        Me.ItemListLabel.Size = New System.Drawing.Size(65, 13)
        Me.ItemListLabel.TabIndex = 0
        Me.ItemListLabel.Text = "Found Items"
        '
        'ItemList
        '
        Me.ItemList.AccessibleName = "Found items"
        Me.ItemList.AccessibleRole = System.Windows.Forms.AccessibleRole.List
        Me.ItemList.FormattingEnabled = True
        Me.ItemList.Location = New System.Drawing.Point(8, 40)
        Me.ItemList.Name = "ItemList"
        Me.ItemList.Size = New System.Drawing.Size(260, 199)
        Me.ItemList.TabIndex = 1
        '
        'DoneButton
        '
        Me.DoneButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.DoneButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.DoneButton.Location = New System.Drawing.Point(8, 250)
        Me.DoneButton.Name = "DoneButton"
        Me.DoneButton.Size = New System.Drawing.Size(75, 23)
        Me.DoneButton.TabIndex = 2
        Me.DoneButton.Text = "Done"
        Me.DoneButton.UseVisualStyleBackColor = True
        '
        'CheckQTimer
        '
        Me.CheckQTimer.Interval = 250
        '
        'FindLogEntry
        '
        Me.AccessibleRole = System.Windows.Forms.AccessibleRole.Window
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.DoneButton
        Me.ClientSize = New System.Drawing.Size(284, 264)
        Me.Controls.Add(Me.DoneButton)
        Me.Controls.Add(Me.ItemList)
        Me.Controls.Add(Me.ItemListLabel)
        Me.Name = "FindLogEntry"
        Me.Text = "Find Log Entries"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ItemListLabel As System.Windows.Forms.Label
    Friend WithEvents ItemList As System.Windows.Forms.ListBox
    Friend WithEvents DoneButton As System.Windows.Forms.Button
    Friend WithEvents CheckQTimer As System.Windows.Forms.Timer
End Class
