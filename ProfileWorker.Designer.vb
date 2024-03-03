<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ProfileWorker
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
        Me.NameLabel = New System.Windows.Forms.Label()
        Me.NameBox = New System.Windows.Forms.TextBox()
        Me.DefaultBox = New System.Windows.Forms.CheckBox()
        Me.OKButton = New System.Windows.Forms.Button()
        Me.CnclButton = New System.Windows.Forms.Button()
        Me.TypeBox = New System.Windows.Forms.ComboBox()
        Me.SuspendLayout()
        '
        'NameLabel
        '
        Me.NameLabel.AccessibleName = ""
        Me.NameLabel.AutoSize = True
        Me.NameLabel.Location = New System.Drawing.Point(0, 20)
        Me.NameLabel.Name = "NameLabel"
        Me.NameLabel.Size = New System.Drawing.Size(41, 13)
        Me.NameLabel.TabIndex = 10
        Me.NameLabel.Text = "Name: "
        '
        'NameBox
        '
        Me.NameBox.AccessibleName = "name"
        Me.NameBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.NameBox.Location = New System.Drawing.Point(41, 20)
        Me.NameBox.Name = "NameBox"
        Me.NameBox.Size = New System.Drawing.Size(200, 20)
        Me.NameBox.TabIndex = 11
        '
        'DefaultBox
        '
        Me.DefaultBox.AccessibleName = "default"
        Me.DefaultBox.AccessibleRole = System.Windows.Forms.AccessibleRole.CheckButton
        Me.DefaultBox.AutoSize = True
        Me.DefaultBox.Location = New System.Drawing.Point(150, 50)
        Me.DefaultBox.Name = "DefaultBox"
        Me.DefaultBox.Size = New System.Drawing.Size(60, 17)
        Me.DefaultBox.TabIndex = 30
        Me.DefaultBox.Text = "Default"
        Me.DefaultBox.UseVisualStyleBackColor = True
        '
        'OKButton
        '
        Me.OKButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.OKButton.Location = New System.Drawing.Point(8, 200)
        Me.OKButton.Name = "OKButton"
        Me.OKButton.Size = New System.Drawing.Size(75, 23)
        Me.OKButton.TabIndex = 90
        Me.OKButton.Text = "Ok"
        Me.OKButton.UseVisualStyleBackColor = True
        '
        'CnclButton
        '
        Me.CnclButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.CnclButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CnclButton.Location = New System.Drawing.Point(100, 200)
        Me.CnclButton.Name = "CnclButton"
        Me.CnclButton.Size = New System.Drawing.Size(75, 23)
        Me.CnclButton.TabIndex = 98
        Me.CnclButton.Text = "Cancel"
        Me.CnclButton.UseVisualStyleBackColor = True
        '
        'TypeBox
        '
        Me.TypeBox.AccessibleName = "type"
        Me.TypeBox.AccessibleRole = System.Windows.Forms.AccessibleRole.ComboBox
        Me.TypeBox.FormattingEnabled = True
        Me.TypeBox.Location = New System.Drawing.Point(8, 50)
        Me.TypeBox.Name = "TypeBox"
        Me.TypeBox.Size = New System.Drawing.Size(121, 21)
        Me.TypeBox.TabIndex = 20
        '
        'ProfileWorker
        '
        Me.AcceptButton = Me.OKButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.CnclButton
        Me.ClientSize = New System.Drawing.Size(284, 211)
        Me.Controls.Add(Me.TypeBox)
        Me.Controls.Add(Me.CnclButton)
        Me.Controls.Add(Me.OKButton)
        Me.Controls.Add(Me.DefaultBox)
        Me.Controls.Add(Me.NameBox)
        Me.Controls.Add(Me.NameLabel)
        Me.Name = "ProfileWorker"
        Me.Text = "Add or Update"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents NameLabel As Label
    Friend WithEvents NameBox As TextBox
    Friend WithEvents DefaultBox As CheckBox
    Friend WithEvents OKButton As Button
    Friend WithEvents CnclButton As Button
    Friend WithEvents TypeBox As ComboBox
End Class
