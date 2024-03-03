<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class scan
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
        Me.StartFreq = New System.Windows.Forms.TextBox()
        Me.EndFreq = New System.Windows.Forms.TextBox()
        Me.StartfreqLabel = New System.Windows.Forms.Label()
        Me.EndfreqLabel = New System.Windows.Forms.Label()
        Me.IncrementLabel = New System.Windows.Forms.Label()
        Me.Increment = New System.Windows.Forms.TextBox()
        Me.SpeedLabel = New System.Windows.Forms.Label()
        Me.Speed = New System.Windows.Forms.TextBox()
        Me.StartButton = New System.Windows.Forms.Button()
        Me.CnclButton = New System.Windows.Forms.Button()
        Me.ClearButton = New System.Windows.Forms.Button()
        Me.UseSavedButton = New System.Windows.Forms.Button()
        Me.SaveButton = New System.Windows.Forms.Button()
        Me.ReplaceButton = New System.Windows.Forms.Button()
        Me.RemoveButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'StartFreq
        '
        Me.StartFreq.AccessibleName = "start frequency"
        Me.StartFreq.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.StartFreq.Location = New System.Drawing.Point(0, 34)
        Me.StartFreq.Name = "StartFreq"
        Me.StartFreq.Size = New System.Drawing.Size(100, 20)
        Me.StartFreq.TabIndex = 0
        '
        'EndFreq
        '
        Me.EndFreq.AccessibleName = "ending frequency"
        Me.EndFreq.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.EndFreq.Location = New System.Drawing.Point(172, 34)
        Me.EndFreq.Name = "EndFreq"
        Me.EndFreq.Size = New System.Drawing.Size(100, 20)
        Me.EndFreq.TabIndex = 1
        '
        'StartfreqLabel
        '
        Me.StartfreqLabel.AutoSize = True
        Me.StartfreqLabel.Location = New System.Drawing.Point(16, 20)
        Me.StartfreqLabel.Name = "StartfreqLabel"
        Me.StartfreqLabel.Size = New System.Drawing.Size(93, 13)
        Me.StartfreqLabel.TabIndex = 0
        Me.StartfreqLabel.Text = "Starting frequency"
        '
        'EndfreqLabel
        '
        Me.EndfreqLabel.AutoSize = True
        Me.EndfreqLabel.Location = New System.Drawing.Point(178, 20)
        Me.EndfreqLabel.Name = "EndfreqLabel"
        Me.EndfreqLabel.Size = New System.Drawing.Size(90, 13)
        Me.EndfreqLabel.TabIndex = 1
        Me.EndfreqLabel.Text = "Ending frequency"
        '
        'IncrementLabel
        '
        Me.IncrementLabel.AccessibleName = ""
        Me.IncrementLabel.AutoSize = True
        Me.IncrementLabel.Location = New System.Drawing.Point(9, 60)
        Me.IncrementLabel.Name = "IncrementLabel"
        Me.IncrementLabel.Size = New System.Drawing.Size(91, 13)
        Me.IncrementLabel.TabIndex = 2
        Me.IncrementLabel.Text = "Increment (KHZ): "
        '
        'Increment
        '
        Me.Increment.AccessibleName = "increment in KHZ"
        Me.Increment.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.Increment.Location = New System.Drawing.Point(100, 60)
        Me.Increment.Name = "Increment"
        Me.Increment.Size = New System.Drawing.Size(25, 20)
        Me.Increment.TabIndex = 2
        '
        'SpeedLabel
        '
        Me.SpeedLabel.AutoSize = True
        Me.SpeedLabel.Location = New System.Drawing.Point(175, 60)
        Me.SpeedLabel.Name = "SpeedLabel"
        Me.SpeedLabel.Size = New System.Drawing.Size(82, 13)
        Me.SpeedLabel.TabIndex = 3
        Me.SpeedLabel.Text = "Speed (.1 sec): "
        '
        'Speed
        '
        Me.Speed.AccessibleName = "speed in tenth of a second"
        Me.Speed.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.Speed.Location = New System.Drawing.Point(257, 60)
        Me.Speed.Name = "Speed"
        Me.Speed.Size = New System.Drawing.Size(25, 20)
        Me.Speed.TabIndex = 3
        '
        'StartButton
        '
        Me.StartButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.StartButton.Location = New System.Drawing.Point(6, 86)
        Me.StartButton.Name = "StartButton"
        Me.StartButton.Size = New System.Drawing.Size(75, 23)
        Me.StartButton.TabIndex = 4
        Me.StartButton.Text = "Start scan"
        Me.StartButton.UseVisualStyleBackColor = True
        '
        'CnclButton
        '
        Me.CnclButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.CnclButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CnclButton.Location = New System.Drawing.Point(178, 86)
        Me.CnclButton.Name = "CnclButton"
        Me.CnclButton.Size = New System.Drawing.Size(75, 23)
        Me.CnclButton.TabIndex = 5
        Me.CnclButton.Text = "Cancel"
        Me.CnclButton.UseVisualStyleBackColor = True
        '
        'ClearButton
        '
        Me.ClearButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.ClearButton.Location = New System.Drawing.Point(6, 175)
        Me.ClearButton.Name = "ClearButton"
        Me.ClearButton.Size = New System.Drawing.Size(75, 23)
        Me.ClearButton.TabIndex = 10
        Me.ClearButton.Text = "&Clear"
        Me.ClearButton.UseVisualStyleBackColor = True
        '
        'UseSavedButton
        '
        Me.UseSavedButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.UseSavedButton.AutoSize = True
        Me.UseSavedButton.Location = New System.Drawing.Point(6, 115)
        Me.UseSavedButton.Name = "UseSavedButton"
        Me.UseSavedButton.Size = New System.Drawing.Size(96, 23)
        Me.UseSavedButton.TabIndex = 6
        Me.UseSavedButton.Text = "Use saved Scan"
        Me.UseSavedButton.UseVisualStyleBackColor = True
        '
        'SaveButton
        '
        Me.SaveButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.SaveButton.AutoSize = True
        Me.SaveButton.Location = New System.Drawing.Point(104, 115)
        Me.SaveButton.Name = "SaveButton"
        Me.SaveButton.Size = New System.Drawing.Size(87, 23)
        Me.SaveButton.TabIndex = 7
        Me.SaveButton.Text = "Save this scan"
        Me.SaveButton.UseVisualStyleBackColor = True
        '
        'ReplaceButton
        '
        Me.ReplaceButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.ReplaceButton.AutoSize = True
        Me.ReplaceButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.ReplaceButton.Enabled = False
        Me.ReplaceButton.Location = New System.Drawing.Point(6, 144)
        Me.ReplaceButton.Name = "ReplaceButton"
        Me.ReplaceButton.Size = New System.Drawing.Size(57, 23)
        Me.ReplaceButton.TabIndex = 8
        Me.ReplaceButton.Text = "Replace"
        Me.ReplaceButton.UseVisualStyleBackColor = True
        '
        'RemoveButton
        '
        Me.RemoveButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.RemoveButton.AutoSize = True
        Me.RemoveButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.RemoveButton.Enabled = False
        Me.RemoveButton.Location = New System.Drawing.Point(69, 144)
        Me.RemoveButton.Name = "RemoveButton"
        Me.RemoveButton.Size = New System.Drawing.Size(57, 23)
        Me.RemoveButton.TabIndex = 9
        Me.RemoveButton.Text = "Remove"
        Me.RemoveButton.UseVisualStyleBackColor = True
        '
        'scan
        '
        Me.AcceptButton = Me.StartButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.CnclButton
        Me.ClientSize = New System.Drawing.Size(284, 184)
        Me.Controls.Add(Me.RemoveButton)
        Me.Controls.Add(Me.ReplaceButton)
        Me.Controls.Add(Me.SaveButton)
        Me.Controls.Add(Me.UseSavedButton)
        Me.Controls.Add(Me.ClearButton)
        Me.Controls.Add(Me.CnclButton)
        Me.Controls.Add(Me.StartButton)
        Me.Controls.Add(Me.Speed)
        Me.Controls.Add(Me.SpeedLabel)
        Me.Controls.Add(Me.Increment)
        Me.Controls.Add(Me.IncrementLabel)
        Me.Controls.Add(Me.EndfreqLabel)
        Me.Controls.Add(Me.StartfreqLabel)
        Me.Controls.Add(Me.EndFreq)
        Me.Controls.Add(Me.StartFreq)
        Me.Name = "scan"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "scan"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents StartFreq As System.Windows.Forms.TextBox
    Friend WithEvents EndFreq As System.Windows.Forms.TextBox
    Friend WithEvents StartfreqLabel As System.Windows.Forms.Label
    Friend WithEvents EndfreqLabel As System.Windows.Forms.Label
    Friend WithEvents IncrementLabel As System.Windows.Forms.Label
    Friend WithEvents Increment As System.Windows.Forms.TextBox
    Friend WithEvents SpeedLabel As System.Windows.Forms.Label
    Friend WithEvents Speed As System.Windows.Forms.TextBox
    Friend WithEvents StartButton As System.Windows.Forms.Button
    Friend WithEvents CnclButton As System.Windows.Forms.Button
    Friend WithEvents ClearButton As System.Windows.Forms.Button
    Friend WithEvents UseSavedButton As System.Windows.Forms.Button
    Friend WithEvents SaveButton As System.Windows.Forms.Button
    Friend WithEvents ReplaceButton As System.Windows.Forms.Button
    Friend WithEvents RemoveButton As System.Windows.Forms.Button
End Class
