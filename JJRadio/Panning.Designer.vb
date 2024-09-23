<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Panning
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
        Me.DoneButton = New System.Windows.Forms.Button()
        Me.PanBox = New System.Windows.Forms.TextBox()
        Me.StartButton = New System.Windows.Forms.Button()
        Me.LowLabel = New System.Windows.Forms.Label()
        Me.LowBox = New System.Windows.Forms.TextBox()
        Me.HighLabel = New System.Windows.Forms.Label()
        Me.HighBox = New System.Windows.Forms.TextBox()
        Me.IncrementLabel = New System.Windows.Forms.Label()
        Me.IncrementBox = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'DoneButton
        '
        Me.DoneButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.DoneButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.DoneButton.Location = New System.Drawing.Point(200, 225)
        Me.DoneButton.Name = "DoneButton"
        Me.DoneButton.Size = New System.Drawing.Size(75, 23)
        Me.DoneButton.TabIndex = 900
        Me.DoneButton.Text = "Done"
        Me.DoneButton.UseVisualStyleBackColor = True
        '
        'PanBox
        '
        Me.PanBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.PanBox.Location = New System.Drawing.Point(0, 140)
        Me.PanBox.Name = "PanBox"
        Me.PanBox.Size = New System.Drawing.Size(300, 20)
        Me.PanBox.TabIndex = 500
        '
        'StartButton
        '
        Me.StartButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.StartButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.StartButton.Location = New System.Drawing.Point(200, 200)
        Me.StartButton.Name = "StartButton"
        Me.StartButton.Size = New System.Drawing.Size(75, 23)
        Me.StartButton.TabIndex = 800
        Me.StartButton.Text = "Start"
        Me.StartButton.UseVisualStyleBackColor = True
        '
        'LowLabel
        '
        Me.LowLabel.AutoSize = True
        Me.LowLabel.Location = New System.Drawing.Point(0, 40)
        Me.LowLabel.Name = "LowLabel"
        Me.LowLabel.Size = New System.Drawing.Size(66, 13)
        Me.LowLabel.TabIndex = 100
        Me.LowLabel.Text = "Start (KHZ): "
        '
        'LowBox
        '
        Me.LowBox.AccessibleName = "start"
        Me.LowBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.LowBox.Location = New System.Drawing.Point(66, 40)
        Me.LowBox.Name = "LowBox"
        Me.LowBox.Size = New System.Drawing.Size(50, 20)
        Me.LowBox.TabIndex = 101
        '
        'HighLabel
        '
        Me.HighLabel.AutoSize = True
        Me.HighLabel.Location = New System.Drawing.Point(133, 40)
        Me.HighLabel.Name = "HighLabel"
        Me.HighLabel.Size = New System.Drawing.Size(63, 13)
        Me.HighLabel.TabIndex = 110
        Me.HighLabel.Text = "End (KHZ): "
        '
        'HighBox
        '
        Me.HighBox.AccessibleName = "high"
        Me.HighBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.HighBox.Location = New System.Drawing.Point(196, 40)
        Me.HighBox.Name = "HighBox"
        Me.HighBox.Size = New System.Drawing.Size(50, 20)
        Me.HighBox.TabIndex = 111
        '
        'IncrementLabel
        '
        Me.IncrementLabel.AutoSize = True
        Me.IncrementLabel.Location = New System.Drawing.Point(260, 40)
        Me.IncrementLabel.Name = "IncrementLabel"
        Me.IncrementLabel.Size = New System.Drawing.Size(91, 13)
        Me.IncrementLabel.TabIndex = 120
        Me.IncrementLabel.Text = "Increment (KHZ): "
        '
        'IncrementBox
        '
        Me.IncrementBox.AccessibleName = "increment"
        Me.IncrementBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.IncrementBox.Location = New System.Drawing.Point(351, 40)
        Me.IncrementBox.Name = "IncrementBox"
        Me.IncrementBox.Size = New System.Drawing.Size(50, 20)
        Me.IncrementBox.TabIndex = 121
        '
        'Panning
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.DoneButton
        Me.ClientSize = New System.Drawing.Size(484, 262)
        Me.Controls.Add(Me.IncrementBox)
        Me.Controls.Add(Me.IncrementLabel)
        Me.Controls.Add(Me.HighBox)
        Me.Controls.Add(Me.HighLabel)
        Me.Controls.Add(Me.LowBox)
        Me.Controls.Add(Me.LowLabel)
        Me.Controls.Add(Me.StartButton)
        Me.Controls.Add(Me.PanBox)
        Me.Controls.Add(Me.DoneButton)
        Me.Name = "Panning"
        Me.Text = "Band Panner"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents DoneButton As System.Windows.Forms.Button
    Friend WithEvents PanBox As System.Windows.Forms.TextBox
    Friend WithEvents StartButton As System.Windows.Forms.Button
    Friend WithEvents LowLabel As System.Windows.Forms.Label
    Friend WithEvents LowBox As System.Windows.Forms.TextBox
    Friend WithEvents HighLabel As System.Windows.Forms.Label
    Friend WithEvents HighBox As System.Windows.Forms.TextBox
    Friend WithEvents IncrementLabel As System.Windows.Forms.Label
    Friend WithEvents IncrementBox As System.Windows.Forms.TextBox
End Class
