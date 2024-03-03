<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ExportForm
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
        Me.ExportFromLabel = New System.Windows.Forms.Label()
        Me.ExportToLabel = New System.Windows.Forms.Label()
        Me.OkButton = New System.Windows.Forms.Button()
        Me.CnclButton = New System.Windows.Forms.Button()
        Me.FromName = New System.Windows.Forms.TextBox()
        Me.ToName = New System.Windows.Forms.TextBox()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.ExportingLabel = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'ExportFromLabel
        '
        Me.ExportFromLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.ExportFromLabel.AutoSize = True
        Me.ExportFromLabel.Location = New System.Drawing.Point(8, 20)
        Me.ExportFromLabel.Name = "ExportFromLabel"
        Me.ExportFromLabel.Size = New System.Drawing.Size(77, 13)
        Me.ExportFromLabel.TabIndex = 0
        Me.ExportFromLabel.Text = "Exporting from "
        '
        'ExportToLabel
        '
        Me.ExportToLabel.AutoSize = True
        Me.ExportToLabel.Location = New System.Drawing.Point(33, 40)
        Me.ExportToLabel.Name = "ExportToLabel"
        Me.ExportToLabel.Size = New System.Drawing.Size(52, 13)
        Me.ExportToLabel.TabIndex = 2
        Me.ExportToLabel.Text = "Export to "
        '
        'OkButton
        '
        Me.OkButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.OkButton.AutoSize = True
        Me.OkButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.OkButton.Location = New System.Drawing.Point(8, 170)
        Me.OkButton.Name = "OkButton"
        Me.OkButton.Size = New System.Drawing.Size(31, 23)
        Me.OkButton.TabIndex = 10
        Me.OkButton.Text = "Ok"
        Me.OkButton.UseVisualStyleBackColor = True
        '
        'CnclButton
        '
        Me.CnclButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.CnclButton.AutoSize = True
        Me.CnclButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.CnclButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CnclButton.Location = New System.Drawing.Point(40, 170)
        Me.CnclButton.Name = "CnclButton"
        Me.CnclButton.Size = New System.Drawing.Size(50, 23)
        Me.CnclButton.TabIndex = 11
        Me.CnclButton.Text = "Cancel"
        Me.CnclButton.UseVisualStyleBackColor = True
        '
        'FromName
        '
        Me.FromName.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.FromName.Location = New System.Drawing.Point(85, 20)
        Me.FromName.Name = "FromName"
        Me.FromName.ReadOnly = True
        Me.FromName.Size = New System.Drawing.Size(200, 20)
        Me.FromName.TabIndex = 1
        '
        'ToName
        '
        Me.ToName.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.ToName.Location = New System.Drawing.Point(85, 40)
        Me.ToName.Name = "ToName"
        Me.ToName.ReadOnly = True
        Me.ToName.Size = New System.Drawing.Size(200, 20)
        Me.ToName.TabIndex = 3
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'ExportingLabel
        '
        Me.ExportingLabel.AccessibleName = "Exporting ..."
        Me.ExportingLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.ExportingLabel.AutoSize = True
        Me.ExportingLabel.Enabled = False
        Me.ExportingLabel.Location = New System.Drawing.Point(8, 90)
        Me.ExportingLabel.Name = "ExportingLabel"
        Me.ExportingLabel.Size = New System.Drawing.Size(63, 13)
        Me.ExportingLabel.TabIndex = 4
        Me.ExportingLabel.Text = "Exporting ..."
        Me.ExportingLabel.Visible = False
        '
        'ExportForm
        '
        Me.AcceptButton = Me.OkButton
        Me.AccessibleRole = System.Windows.Forms.AccessibleRole.Window
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.CnclButton
        Me.ClientSize = New System.Drawing.Size(284, 164)
        Me.Controls.Add(Me.ExportingLabel)
        Me.Controls.Add(Me.ToName)
        Me.Controls.Add(Me.FromName)
        Me.Controls.Add(Me.CnclButton)
        Me.Controls.Add(Me.OkButton)
        Me.Controls.Add(Me.ExportToLabel)
        Me.Controls.Add(Me.ExportFromLabel)
        Me.Name = "ExportForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Export the Log"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ExportFromLabel As System.Windows.Forms.Label
    Friend WithEvents ExportToLabel As System.Windows.Forms.Label
    Friend WithEvents OkButton As System.Windows.Forms.Button
    Friend WithEvents CnclButton As System.Windows.Forms.Button
    Friend WithEvents FromName As System.Windows.Forms.TextBox
    Friend WithEvents ToName As System.Windows.Forms.TextBox
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents ExportingLabel As System.Windows.Forms.Label
End Class
