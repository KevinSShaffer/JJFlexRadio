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
        Me.ContinueButton = New System.Windows.Forms.Button()
        Me.RigBox = New System.Windows.Forms.TextBox()
        Me.NumberBox = New RadioBoxes.NumberBox()
        Me.Combo1 = New RadioBoxes.Combo()
        Me.TheBox = New RadioBoxes.MainBox()
        Me.testBoxOut = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'ContinueButton
        '
        Me.ContinueButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.ContinueButton.Location = New System.Drawing.Point(8, 350)
        Me.ContinueButton.Name = "ContinueButton"
        Me.ContinueButton.Size = New System.Drawing.Size(75, 23)
        Me.ContinueButton.TabIndex = 90
        Me.ContinueButton.Text = "Continue"
        Me.ContinueButton.UseVisualStyleBackColor = True
        '
        'RigBox
        '
        Me.RigBox.AccessibleName = "rig data"
        Me.RigBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.RigBox.Location = New System.Drawing.Point(0, 200)
        Me.RigBox.Name = "RigBox"
        Me.RigBox.Size = New System.Drawing.Size(300, 20)
        Me.RigBox.TabIndex = 50
        '
        'NumberBox
        '
        Me.NumberBox.AccessibleName = "number box"
        Me.NumberBox.Header = "testBox"
        Me.NumberBox.HighValue = 190
        Me.NumberBox.Increment = 10
        Me.NumberBox.Location = New System.Drawing.Point(0, 220)
        Me.NumberBox.LowValue = 0
        Me.NumberBox.Name = "NumberBox"
        Me.NumberBox.Size = New System.Drawing.Size(50, 35)
        Me.NumberBox.TabIndex = 60
        Me.NumberBox.UpdateDisplayFunction = Nothing
        Me.NumberBox.UpdateRigFunction = Nothing
        '
        'Combo1
        '
        Me.Combo1.AccessibleName = "rig stuff"
        Me.Combo1.AccessibleRole = System.Windows.Forms.AccessibleRole.ComboBox
        Me.Combo1.BoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Combo1.ExpandedSize = New System.Drawing.Size(0, 0)
        Me.Combo1.Header = ""
        Me.Combo1.Location = New System.Drawing.Point(0, 30)
        Me.Combo1.Name = "Combo1"
        Me.Combo1.Size = New System.Drawing.Size(100, 150)
        Me.Combo1.SmallSize = New System.Drawing.Size(0, 0)
        Me.Combo1.TabIndex = 10
        Me.Combo1.TheList = Nothing
        Me.Combo1.UpdateDisplayFunction = Nothing
        Me.Combo1.UpdateRigFunction = Nothing
        '
        'TheBox
        '
        Me.TheBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.TheBox.Location = New System.Drawing.Point(0, 0)
        Me.TheBox.Name = "TheBox"
        Me.TheBox.SelectionLength = 0
        Me.TheBox.SelectionStart = 0
        Me.TheBox.Size = New System.Drawing.Size(300, 20)
        Me.TheBox.TabIndex = 0
        '
        'testBoxOut
        '
        Me.testBoxOut.AccessibleName = "test box output"
        Me.testBoxOut.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.testBoxOut.Location = New System.Drawing.Point(100, 235)
        Me.testBoxOut.Name = "testBoxOut"
        Me.testBoxOut.Size = New System.Drawing.Size(50, 20)
        Me.testBoxOut.TabIndex = 61
        '
        'Form1
        '
        Me.AcceptButton = Me.ContinueButton
        Me.AccessibleRole = System.Windows.Forms.AccessibleRole.Window
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(584, 362)
        Me.Controls.Add(Me.testBoxOut)
        Me.Controls.Add(Me.NumberBox)
        Me.Controls.Add(Me.Combo1)
        Me.Controls.Add(Me.RigBox)
        Me.Controls.Add(Me.ContinueButton)
        Me.Controls.Add(Me.TheBox)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TheBox As RadioBoxes.MainBox
    Friend WithEvents ContinueButton As System.Windows.Forms.Button
    Friend WithEvents RigBox As System.Windows.Forms.TextBox
    Friend WithEvents Combo1 As RadioBoxes.Combo
    Friend WithEvents NumberBox As RadioBoxes.NumberBox
    Friend WithEvents testBoxOut As System.Windows.Forms.TextBox

End Class
