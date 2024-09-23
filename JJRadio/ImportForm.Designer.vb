<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ImportForm
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
        Me.FromLabel = New System.Windows.Forms.Label()
        Me.FromName = New System.Windows.Forms.TextBox()
        Me.ToLabel = New System.Windows.Forms.Label()
        Me.ToName = New System.Windows.Forms.TextBox()
        Me.ImportingLabel = New System.Windows.Forms.Label()
        Me.OkButton = New System.Windows.Forms.Button()
        Me.CnclButton = New System.Windows.Forms.Button()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.SuspendLayout()
        '
        'FromLabel
        '
        Me.FromLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.FromLabel.AutoSize = True
        Me.FromLabel.Location = New System.Drawing.Point(23, 20)
        Me.FromLabel.Name = "FromLabel"
        Me.FromLabel.Size = New System.Drawing.Size(62, 13)
        Me.FromLabel.TabIndex = 0
        Me.FromLabel.Text = "Import from "
        '
        'FromName
        '
        Me.FromName.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.FromName.Location = New System.Drawing.Point(85, 20)
        Me.FromName.Name = "FromName"
        Me.FromName.ReadOnly = True
        Me.FromName.Size = New System.Drawing.Size(100, 20)
        Me.FromName.TabIndex = 1
        '
        'ToLabel
        '
        Me.ToLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.ToLabel.AutoSize = True
        Me.ToLabel.Location = New System.Drawing.Point(34, 40)
        Me.ToLabel.Name = "ToLabel"
        Me.ToLabel.Size = New System.Drawing.Size(51, 13)
        Me.ToLabel.TabIndex = 2
        Me.ToLabel.Text = "Import to "
        '
        'ToName
        '
        Me.ToName.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.ToName.Location = New System.Drawing.Point(85, 40)
        Me.ToName.Name = "ToName"
        Me.ToName.ReadOnly = True
        Me.ToName.Size = New System.Drawing.Size(100, 20)
        Me.ToName.TabIndex = 3
        '
        'ImportingLabel
        '
        Me.ImportingLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.ImportingLabel.AutoSize = True
        Me.ImportingLabel.Enabled = False
        Me.ImportingLabel.Location = New System.Drawing.Point(8, 90)
        Me.ImportingLabel.Name = "ImportingLabel"
        Me.ImportingLabel.Size = New System.Drawing.Size(62, 13)
        Me.ImportingLabel.TabIndex = 4
        Me.ImportingLabel.Text = "Importing ..."
        Me.ImportingLabel.Visible = False
        '
        'OkButton
        '
        Me.OkButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.OkButton.Location = New System.Drawing.Point(8, 170)
        Me.OkButton.Name = "OkButton"
        Me.OkButton.Size = New System.Drawing.Size(75, 23)
        Me.OkButton.TabIndex = 10
        Me.OkButton.Text = "Ok"
        Me.OkButton.UseVisualStyleBackColor = True
        '
        'CnclButton
        '
        Me.CnclButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.CnclButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CnclButton.Location = New System.Drawing.Point(40, 170)
        Me.CnclButton.Name = "CnclButton"
        Me.CnclButton.Size = New System.Drawing.Size(75, 23)
        Me.CnclButton.TabIndex = 11
        Me.CnclButton.Text = "Cancel"
        Me.CnclButton.UseVisualStyleBackColor = True
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'ImportForm
        '
        Me.AcceptButton = Me.OkButton
        Me.AccessibleRole = System.Windows.Forms.AccessibleRole.Window
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.CnclButton
        Me.ClientSize = New System.Drawing.Size(284, 164)
        Me.Controls.Add(Me.CnclButton)
        Me.Controls.Add(Me.OkButton)
        Me.Controls.Add(Me.ImportingLabel)
        Me.Controls.Add(Me.ToName)
        Me.Controls.Add(Me.ToLabel)
        Me.Controls.Add(Me.FromName)
        Me.Controls.Add(Me.FromLabel)
        Me.Name = "ImportForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Import the log"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents FromLabel As System.Windows.Forms.Label
    Friend WithEvents FromName As System.Windows.Forms.TextBox
    Friend WithEvents ToLabel As System.Windows.Forms.Label
    Friend WithEvents ToName As System.Windows.Forms.TextBox
    Friend WithEvents ImportingLabel As System.Windows.Forms.Label
    Friend WithEvents OkButton As System.Windows.Forms.Button
    Friend WithEvents CnclButton As System.Windows.Forms.Button
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
End Class
