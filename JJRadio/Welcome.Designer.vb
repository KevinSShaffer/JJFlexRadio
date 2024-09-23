<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Welcome
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
        Me.WelcomeBox = New System.Windows.Forms.TextBox()
        Me.DocButton = New System.Windows.Forms.Button()
        Me.ConfigButton = New System.Windows.Forms.Button()
        Me.QuitButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'WelcomeBox
        '
        Me.WelcomeBox.AccessibleName = "Welcome to JJRadio"
        Me.WelcomeBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.WelcomeBox.Location = New System.Drawing.Point(0, 20)
        Me.WelcomeBox.Multiline = True
        Me.WelcomeBox.Name = "WelcomeBox"
        Me.WelcomeBox.ReadOnly = True
        Me.WelcomeBox.Size = New System.Drawing.Size(550, 420)
        Me.WelcomeBox.TabIndex = 0
        '
        'DocButton
        '
        Me.DocButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.DocButton.AutoSize = True
        Me.DocButton.Location = New System.Drawing.Point(0, 450)
        Me.DocButton.Name = "DocButton"
        Me.DocButton.Size = New System.Drawing.Size(177, 23)
        Me.DocButton.TabIndex = 90
        Me.DocButton.Text = "Read the JJRadio Documentation"
        Me.DocButton.UseVisualStyleBackColor = True
        '
        'ConfigButton
        '
        Me.ConfigButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.ConfigButton.AutoSize = True
        Me.ConfigButton.Location = New System.Drawing.Point(0, 480)
        Me.ConfigButton.Name = "ConfigButton"
        Me.ConfigButton.Size = New System.Drawing.Size(103, 23)
        Me.ConfigButton.TabIndex = 92
        Me.ConfigButton.Text = "Configure JJRadio"
        Me.ConfigButton.UseVisualStyleBackColor = True
        '
        'QuitButton
        '
        Me.QuitButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.QuitButton.AutoSize = True
        Me.QuitButton.Location = New System.Drawing.Point(0, 510)
        Me.QuitButton.Name = "QuitButton"
        Me.QuitButton.Size = New System.Drawing.Size(75, 23)
        Me.QuitButton.TabIndex = 99
        Me.QuitButton.Text = "Exit JJRadio"
        Me.QuitButton.UseVisualStyleBackColor = True
        '
        'Welcome
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(584, 562)
        Me.Controls.Add(Me.QuitButton)
        Me.Controls.Add(Me.ConfigButton)
        Me.Controls.Add(Me.DocButton)
        Me.Controls.Add(Me.WelcomeBox)
        Me.Name = "Welcome"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Welcome to JJRadio"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents WelcomeBox As System.Windows.Forms.TextBox
    Friend WithEvents DocButton As System.Windows.Forms.Button
    Friend WithEvents ConfigButton As System.Windows.Forms.Button
    Friend WithEvents QuitButton As System.Windows.Forms.Button
End Class
