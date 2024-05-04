<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MemoryScan
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.SuspendLayout()
        '
        'SpeedLabel
        '
        Me.SpeedLabel.AutoSize = True
        Me.SpeedLabel.Location = New System.Drawing.Point(8, 50)
        Me.SpeedLabel.Name = "SpeedLabel"
        Me.SpeedLabel.Size = New System.Drawing.Size(82, 13)
        Me.SpeedLabel.TabIndex = 10
        Me.SpeedLabel.Text = "Speed (.1 sec): "
        '
        'SpeedBox
        '
        Me.SpeedBox.AccessibleName = "speed .1 khz"
        Me.SpeedBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.SpeedBox.Location = New System.Drawing.Point(90, 50)
        Me.SpeedBox.Name = "SpeedBox"
        Me.SpeedBox.Size = New System.Drawing.Size(30, 20)
        Me.SpeedBox.TabIndex = 11
        '
        'StartButton
        '
        Me.StartButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.StartButton.Location = New System.Drawing.Point(200, 47)
        Me.StartButton.Name = "StartButton"
        Me.StartButton.Size = New System.Drawing.Size(75, 23)
        Me.StartButton.TabIndex = 50
        Me.StartButton.Text = "Start scan"
        Me.StartButton.UseVisualStyleBackColor = True
        '
        'MemoryScan
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.StartButton)
        Me.Controls.Add(Me.SpeedBox)
        Me.Controls.Add(Me.SpeedLabel)
        Me.Name = "MemoryScan"
        Me.Size = New System.Drawing.Size(600, 200)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents SpeedLabel As System.Windows.Forms.Label
    Friend WithEvents SpeedBox As System.Windows.Forms.TextBox
    Friend WithEvents StartButton As System.Windows.Forms.Button

End Class
