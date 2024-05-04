<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FreqInput
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
        Me.FreqLabel = New System.Windows.Forms.Label()
        Me.OKButton = New System.Windows.Forms.Button()
        Me.CnclButton = New System.Windows.Forms.Button()
        Me.InfoLabel = New System.Windows.Forms.Label()
        Me.FreqBox = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'FreqLabel
        '
        Me.FreqLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.FreqLabel.AutoSize = True
        Me.FreqLabel.Location = New System.Drawing.Point(3, 20)
        Me.FreqLabel.Name = "FreqLabel"
        Me.FreqLabel.Size = New System.Drawing.Size(34, 13)
        Me.FreqLabel.TabIndex = 10
        Me.FreqLabel.Text = "Freq: "
        '
        'OKButton
        '
        Me.OKButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.OKButton.AutoSize = True
        Me.OKButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.OKButton.Location = New System.Drawing.Point(8, 80)
        Me.OKButton.Name = "OKButton"
        Me.OKButton.Size = New System.Drawing.Size(75, 23)
        Me.OKButton.TabIndex = 900
        Me.OKButton.Text = "OK"
        Me.OKButton.UseVisualStyleBackColor = True
        '
        'CnclButton
        '
        Me.CnclButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.CnclButton.AutoSize = True
        Me.CnclButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CnclButton.Location = New System.Drawing.Point(90, 80)
        Me.CnclButton.Name = "CnclButton"
        Me.CnclButton.Size = New System.Drawing.Size(75, 23)
        Me.CnclButton.TabIndex = 910
        Me.CnclButton.Text = "Cancel"
        Me.CnclButton.UseVisualStyleBackColor = True
        '
        'InfoLabel
        '
        Me.InfoLabel.AutoSize = True
        Me.InfoLabel.Location = New System.Drawing.Point(0, 50)
        Me.InfoLabel.Name = "InfoLabel"
        Me.InfoLabel.Size = New System.Drawing.Size(231, 13)
        Me.InfoLabel.TabIndex = 20
        Me.InfoLabel.Text = "Allowed forms:  MHZ.KHZ.HZ, MHZ.KHZ, KHZ"
        '
        'FreqBox
        '
        Me.FreqBox.AccessibleName = "Frequency"
        Me.FreqBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.FreqBox.Location = New System.Drawing.Point(37, 20)
        Me.FreqBox.Name = "FreqBox"
        Me.FreqBox.Size = New System.Drawing.Size(100, 20)
        Me.FreqBox.TabIndex = 11
        '
        'FreqInput
        '
        Me.AcceptButton = Me.OKButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.CnclButton
        Me.ClientSize = New System.Drawing.Size(284, 112)
        Me.Controls.Add(Me.FreqBox)
        Me.Controls.Add(Me.InfoLabel)
        Me.Controls.Add(Me.CnclButton)
        Me.Controls.Add(Me.OKButton)
        Me.Controls.Add(Me.FreqLabel)
        Me.Name = "FreqInput"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Frequency Input"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents FreqLabel As System.Windows.Forms.Label
    Friend WithEvents OKButton As System.Windows.Forms.Button
    Friend WithEvents CnclButton As System.Windows.Forms.Button
    Friend WithEvents InfoLabel As System.Windows.Forms.Label
    Friend WithEvents FreqBox As System.Windows.Forms.TextBox
End Class
