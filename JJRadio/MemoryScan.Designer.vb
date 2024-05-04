<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MemoryScan
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
        Me.SpeedLabel = New System.Windows.Forms.Label()
        Me.SpeedBox = New System.Windows.Forms.TextBox()
        Me.StartButton = New System.Windows.Forms.Button()
        Me.CnclButton = New System.Windows.Forms.Button()
        Me.ManageButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'SpeedLabel
        '
        Me.SpeedLabel.AutoSize = True
        Me.SpeedLabel.Location = New System.Drawing.Point(8, 350)
        Me.SpeedLabel.Name = "SpeedLabel"
        Me.SpeedLabel.Size = New System.Drawing.Size(82, 13)
        Me.SpeedLabel.TabIndex = 100
        Me.SpeedLabel.Text = "Speed (.1 sec): "
        '
        'SpeedBox
        '
        Me.SpeedBox.AccessibleName = "speed (.1 sec)"
        Me.SpeedBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.SpeedBox.Location = New System.Drawing.Point(82, 350)
        Me.SpeedBox.Name = "SpeedBox"
        Me.SpeedBox.Size = New System.Drawing.Size(30, 20)
        Me.SpeedBox.TabIndex = 101
        '
        'StartButton
        '
        Me.StartButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.StartButton.Location = New System.Drawing.Point(300, 350)
        Me.StartButton.Name = "StartButton"
        Me.StartButton.Size = New System.Drawing.Size(75, 23)
        Me.StartButton.TabIndex = 150
        Me.StartButton.Text = "Start scan"
        Me.StartButton.UseVisualStyleBackColor = True
        '
        'CnclButton
        '
        Me.CnclButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.CnclButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CnclButton.Location = New System.Drawing.Point(300, 420)
        Me.CnclButton.Name = "CnclButton"
        Me.CnclButton.Size = New System.Drawing.Size(75, 23)
        Me.CnclButton.TabIndex = 990
        Me.CnclButton.Text = "Cancel"
        Me.CnclButton.UseVisualStyleBackColor = True
        '
        'ManageButton
        '
        Me.ManageButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.ManageButton.Location = New System.Drawing.Point(8, 420)
        Me.ManageButton.Name = "ManageButton"
        Me.ManageButton.Size = New System.Drawing.Size(75, 23)
        Me.ManageButton.TabIndex = 900
        Me.ManageButton.Text = "Manage groups"
        Me.ManageButton.UseVisualStyleBackColor = True
        '
        'MemoryScan
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.CnclButton
        Me.ClientSize = New System.Drawing.Size(584, 462)
        Me.Controls.Add(Me.ManageButton)
        Me.Controls.Add(Me.CnclButton)
        Me.Controls.Add(Me.StartButton)
        Me.Controls.Add(Me.SpeedBox)
        Me.Controls.Add(Me.SpeedLabel)
        Me.Name = "MemoryScan"
        Me.Text = "Memory Scan"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents SpeedLabel As System.Windows.Forms.Label
    Friend WithEvents SpeedBox As System.Windows.Forms.TextBox
    Friend WithEvents StartButton As System.Windows.Forms.Button
    Friend WithEvents CnclButton As System.Windows.Forms.Button
    Friend WithEvents ManageButton As System.Windows.Forms.Button
End Class
