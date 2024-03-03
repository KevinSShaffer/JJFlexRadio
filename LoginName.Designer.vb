<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LoginName

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
        Me.LoginLabel = New System.Windows.Forms.Label()
        Me.LoginBox = New System.Windows.Forms.TextBox()
        Me.OkButton = New System.Windows.Forms.Button()
        Me.CnclButton = New System.Windows.Forms.Button()
        Me.AddressLabel = New System.Windows.Forms.Label()
        Me.AddressBox = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'LoginLabel
        '
        Me.LoginLabel.AutoSize = True
        Me.LoginLabel.Location = New System.Drawing.Point(17, 50)
        Me.LoginLabel.Name = "LoginLabel"
        Me.LoginLabel.Size = New System.Drawing.Size(68, 13)
        Me.LoginLabel.TabIndex = 20
        Me.LoginLabel.Text = "Login name: "
        '
        'LoginBox
        '
        Me.LoginBox.AccessibleName = "login name"
        Me.LoginBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.LoginBox.Location = New System.Drawing.Point(85, 50)
        Me.LoginBox.Name = "LoginBox"
        Me.LoginBox.Size = New System.Drawing.Size(100, 20)
        Me.LoginBox.TabIndex = 21
        '
        'OkButton
        '
        Me.OkButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.OkButton.Location = New System.Drawing.Point(8, 80)
        Me.OkButton.Name = "OkButton"
        Me.OkButton.Size = New System.Drawing.Size(75, 23)
        Me.OkButton.TabIndex = 90
        Me.OkButton.Text = "OK"
        Me.OkButton.UseVisualStyleBackColor = True
        '
        'CnclButton
        '
        Me.CnclButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.CnclButton.Location = New System.Drawing.Point(108, 80)
        Me.CnclButton.Name = "CnclButton"
        Me.CnclButton.Size = New System.Drawing.Size(75, 23)
        Me.CnclButton.TabIndex = 99
        Me.CnclButton.Text = "Cancel"
        Me.CnclButton.UseVisualStyleBackColor = True
        '
        'AddressLabel
        '
        Me.AddressLabel.AutoSize = True
        Me.AddressLabel.Location = New System.Drawing.Point(0, 20)
        Me.AddressLabel.Name = "AddressLabel"
        Me.AddressLabel.Size = New System.Drawing.Size(85, 13)
        Me.AddressLabel.TabIndex = 10
        Me.AddressLabel.Text = "Cluster address: "
        '
        'AddressBox
        '
        Me.AddressBox.AccessibleName = "cluster address"
        Me.AddressBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.AddressBox.Location = New System.Drawing.Point(85, 20)
        Me.AddressBox.Name = "AddressBox"
        Me.AddressBox.Size = New System.Drawing.Size(100, 20)
        Me.AddressBox.TabIndex = 11
        '
        'LoginName
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.AddressBox)
        Me.Controls.Add(Me.AddressLabel)
        Me.Controls.Add(Me.CnclButton)
        Me.Controls.Add(Me.OkButton)
        Me.Controls.Add(Me.LoginBox)
        Me.Controls.Add(Me.LoginLabel)
        Me.Name = "LoginName"
        Me.Size = New System.Drawing.Size(300, 200)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents LoginLabel As System.Windows.Forms.Label
    Friend WithEvents LoginBox As System.Windows.Forms.TextBox
    Friend WithEvents OkButton As System.Windows.Forms.Button
    Friend WithEvents CnclButton As System.Windows.Forms.Button
    Friend WithEvents AddressLabel As System.Windows.Forms.Label
    Friend WithEvents AddressBox As System.Windows.Forms.TextBox

End Class
