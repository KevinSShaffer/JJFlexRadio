<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OptionalMessageControlWindow
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
        Me.DontshowBox = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'DontshowBox
        '
        Me.DontshowBox.AccessibleRole = System.Windows.Forms.AccessibleRole.CheckButton
        Me.DontshowBox.AutoSize = True
        Me.DontshowBox.Location = New System.Drawing.Point(8, 210)
        Me.DontshowBox.Name = "DontshowBox"
        Me.DontshowBox.Size = New System.Drawing.Size(172, 17)
        Me.DontshowBox.TabIndex = 90
        Me.DontshowBox.Text = "Don't show this message again"
        Me.DontshowBox.UseVisualStyleBackColor = True
        '
        'OptionalMessageControlWindow
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(584, 262)
        Me.Controls.Add(Me.DontshowBox)
        Me.Name = "OptionalMessageControlWindow"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents DontshowBox As System.Windows.Forms.CheckBox
End Class
