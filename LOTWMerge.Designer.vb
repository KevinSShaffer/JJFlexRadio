<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LOTWMerge
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
        Me.LOTWFileLabel = New System.Windows.Forms.Label()
        Me.LOTWFileBox = New System.Windows.Forms.TextBox()
        Me.OKButton = New System.Windows.Forms.Button()
        Me.CnclButton = New System.Windows.Forms.Button()
        Me.LOTWFileDialog = New System.Windows.Forms.OpenFileDialog()
        Me.LogLabel = New System.Windows.Forms.Label()
        Me.LogBox = New System.Windows.Forms.TextBox()
        Me.ProgressLabel = New System.Windows.Forms.Label()
        Me.ProgressBox = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'LOTWFileLabel
        '
        Me.LOTWFileLabel.AutoSize = True
        Me.LOTWFileLabel.Location = New System.Drawing.Point(0, 20)
        Me.LOTWFileLabel.Name = "LOTWFileLabel"
        Me.LOTWFileLabel.Size = New System.Drawing.Size(61, 13)
        Me.LOTWFileLabel.TabIndex = 10
        Me.LOTWFileLabel.Text = "LOTW file: "
        '
        'LOTWFileBox
        '
        Me.LOTWFileBox.AccessibleName = "LOTW file"
        Me.LOTWFileBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.LOTWFileBox.Location = New System.Drawing.Point(61, 20)
        Me.LOTWFileBox.Name = "LOTWFileBox"
        Me.LOTWFileBox.ReadOnly = True
        Me.LOTWFileBox.Size = New System.Drawing.Size(300, 20)
        Me.LOTWFileBox.TabIndex = 11
        '
        'OKButton
        '
        Me.OKButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.OKButton.Location = New System.Drawing.Point(8, 110)
        Me.OKButton.Name = "OKButton"
        Me.OKButton.Size = New System.Drawing.Size(75, 23)
        Me.OKButton.TabIndex = 90
        Me.OKButton.Text = "OK"
        Me.OKButton.UseVisualStyleBackColor = True
        '
        'CnclButton
        '
        Me.CnclButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.CnclButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CnclButton.Location = New System.Drawing.Point(100, 110)
        Me.CnclButton.Name = "CnclButton"
        Me.CnclButton.Size = New System.Drawing.Size(75, 23)
        Me.CnclButton.TabIndex = 99
        Me.CnclButton.Text = "Cancel"
        Me.CnclButton.UseVisualStyleBackColor = True
        '
        'LOTWFileDialog
        '
        Me.LOTWFileDialog.DefaultExt = "adi"
        Me.LOTWFileDialog.Title = "LOTW file"
        '
        'LogLabel
        '
        Me.LogLabel.AutoSize = True
        Me.LogLabel.Location = New System.Drawing.Point(0, 50)
        Me.LogLabel.Name = "LogLabel"
        Me.LogLabel.Size = New System.Drawing.Size(47, 13)
        Me.LogLabel.TabIndex = 20
        Me.LogLabel.Text = "Log file: "
        '
        'LogBox
        '
        Me.LogBox.AccessibleName = "log file"
        Me.LogBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.LogBox.Location = New System.Drawing.Point(61, 50)
        Me.LogBox.Name = "LogBox"
        Me.LogBox.ReadOnly = True
        Me.LogBox.Size = New System.Drawing.Size(300, 20)
        Me.LogBox.TabIndex = 21
        '
        'ProgressLabel
        '
        Me.ProgressLabel.AutoSize = True
        Me.ProgressLabel.Location = New System.Drawing.Point(0, 80)
        Me.ProgressLabel.Name = "ProgressLabel"
        Me.ProgressLabel.Size = New System.Drawing.Size(54, 13)
        Me.ProgressLabel.TabIndex = 30
        Me.ProgressLabel.Text = "Progress: "
        '
        'ProgressBox
        '
        Me.ProgressBox.AccessibleName = "progress"
        Me.ProgressBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.ProgressBox.Location = New System.Drawing.Point(61, 80)
        Me.ProgressBox.Name = "ProgressBox"
        Me.ProgressBox.ReadOnly = True
        Me.ProgressBox.Size = New System.Drawing.Size(300, 20)
        Me.ProgressBox.TabIndex = 31
        '
        'LOTWMerge
        '
        Me.AcceptButton = Me.OKButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.CnclButton
        Me.ClientSize = New System.Drawing.Size(384, 142)
        Me.Controls.Add(Me.ProgressBox)
        Me.Controls.Add(Me.ProgressLabel)
        Me.Controls.Add(Me.LogBox)
        Me.Controls.Add(Me.LogLabel)
        Me.Controls.Add(Me.CnclButton)
        Me.Controls.Add(Me.OKButton)
        Me.Controls.Add(Me.LOTWFileBox)
        Me.Controls.Add(Me.LOTWFileLabel)
        Me.Name = "LOTWMerge"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "LOTW Merge"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents LOTWFileLabel As System.Windows.Forms.Label
    Friend WithEvents LOTWFileBox As System.Windows.Forms.TextBox
    Friend WithEvents OKButton As System.Windows.Forms.Button
    Friend WithEvents CnclButton As System.Windows.Forms.Button
    Friend WithEvents LOTWFileDialog As System.Windows.Forms.OpenFileDialog
    Friend WithEvents LogLabel As System.Windows.Forms.Label
    Friend WithEvents LogBox As System.Windows.Forms.TextBox
    Friend WithEvents ProgressLabel As System.Windows.Forms.Label
    Friend WithEvents ProgressBox As System.Windows.Forms.TextBox
End Class
