<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.ReadButton = New System.Windows.Forms.Button()
        Me.AddButton = New System.Windows.Forms.Button()
        Me.FindButton = New System.Windows.Forms.Button()
        Me.OpenDialog = New System.Windows.Forms.OpenFileDialog()
        Me.UpdateButton = New System.Windows.Forms.Button()
        Me.ReadPrevButton = New System.Windows.Forms.Button()
        Me.DeleteButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(0, 20)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(100, 20)
        Me.TextBox1.TabIndex = 0
        '
        'ReadButton
        '
        Me.ReadButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.ReadButton.AutoSize = True
        Me.ReadButton.Location = New System.Drawing.Point(8, 100)
        Me.ReadButton.Name = "ReadButton"
        Me.ReadButton.Size = New System.Drawing.Size(75, 23)
        Me.ReadButton.TabIndex = 1
        Me.ReadButton.Text = "&Read"
        Me.ReadButton.UseVisualStyleBackColor = True
        '
        'AddButton
        '
        Me.AddButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.AddButton.AutoSize = True
        Me.AddButton.Location = New System.Drawing.Point(8, 130)
        Me.AddButton.Name = "AddButton"
        Me.AddButton.Size = New System.Drawing.Size(75, 23)
        Me.AddButton.TabIndex = 3
        Me.AddButton.Text = "&Add"
        Me.AddButton.UseVisualStyleBackColor = True
        '
        'FindButton
        '
        Me.FindButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.FindButton.AutoSize = True
        Me.FindButton.Location = New System.Drawing.Point(108, 100)
        Me.FindButton.Name = "FindButton"
        Me.FindButton.Size = New System.Drawing.Size(75, 23)
        Me.FindButton.TabIndex = 2
        Me.FindButton.Text = "&Find"
        Me.FindButton.UseVisualStyleBackColor = True
        '
        'OpenDialog
        '
        Me.OpenDialog.FileName = "OpenFileDialog1"
        '
        'UpdateButton
        '
        Me.UpdateButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.UpdateButton.AutoSize = True
        Me.UpdateButton.Location = New System.Drawing.Point(108, 130)
        Me.UpdateButton.Name = "UpdateButton"
        Me.UpdateButton.Size = New System.Drawing.Size(75, 23)
        Me.UpdateButton.TabIndex = 4
        Me.UpdateButton.Text = "&Update"
        Me.UpdateButton.UseVisualStyleBackColor = True
        '
        'ReadPrevButton
        '
        Me.ReadPrevButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.ReadPrevButton.AutoSize = True
        Me.ReadPrevButton.Location = New System.Drawing.Point(8, 160)
        Me.ReadPrevButton.Name = "ReadPrevButton"
        Me.ReadPrevButton.Size = New System.Drawing.Size(87, 23)
        Me.ReadPrevButton.TabIndex = 5
        Me.ReadPrevButton.Text = "Read &Previous"
        Me.ReadPrevButton.UseVisualStyleBackColor = True
        '
        'DeleteButton
        '
        Me.DeleteButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.DeleteButton.AutoSize = True
        Me.DeleteButton.Location = New System.Drawing.Point(108, 160)
        Me.DeleteButton.Name = "DeleteButton"
        Me.DeleteButton.Size = New System.Drawing.Size(75, 23)
        Me.DeleteButton.TabIndex = 6
        Me.DeleteButton.Text = "&Delete"
        Me.DeleteButton.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 264)
        Me.Controls.Add(Me.DeleteButton)
        Me.Controls.Add(Me.ReadPrevButton)
        Me.Controls.Add(Me.UpdateButton)
        Me.Controls.Add(Me.FindButton)
        Me.Controls.Add(Me.AddButton)
        Me.Controls.Add(Me.ReadButton)
        Me.Controls.Add(Me.TextBox1)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents ReadButton As System.Windows.Forms.Button
    Friend WithEvents AddButton As System.Windows.Forms.Button
    Friend WithEvents FindButton As System.Windows.Forms.Button
    Friend WithEvents OpenDialog As System.Windows.Forms.OpenFileDialog
    Friend WithEvents UpdateButton As System.Windows.Forms.Button
    Friend WithEvents ReadPrevButton As System.Windows.Forms.Button
    Friend WithEvents DeleteButton As System.Windows.Forms.Button

End Class
