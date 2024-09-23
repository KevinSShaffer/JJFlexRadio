<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OptionalMessageControlDefault
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
        Me.TheMessage = New System.Windows.Forms.Label()
        Me.OKButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'TheMessage
        '
        Me.TheMessage.AccessibleName = "message"
        Me.TheMessage.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.TheMessage.AutoSize = True
        Me.TheMessage.Location = New System.Drawing.Point(10, 75)
        Me.TheMessage.Name = "TheMessage"
        Me.TheMessage.Size = New System.Drawing.Size(39, 13)
        Me.TheMessage.TabIndex = 10
        Me.TheMessage.Text = "Label1"
        '
        'OKButton
        '
        Me.OKButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.OKButton.Location = New System.Drawing.Point(200, 150)
        Me.OKButton.Name = "OKButton"
        Me.OKButton.Size = New System.Drawing.Size(75, 23)
        Me.OKButton.TabIndex = 90
        Me.OKButton.Text = "OK"
        Me.OKButton.UseVisualStyleBackColor = True
        '
        'OptionalMessageControlDefault
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.OKButton)
        Me.Controls.Add(Me.TheMessage)
        Me.Name = "OptionalMessageControlDefault"
        Me.Size = New System.Drawing.Size(600, 200)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TheMessage As System.Windows.Forms.Label
    Friend WithEvents OKButton As System.Windows.Forms.Button

End Class
