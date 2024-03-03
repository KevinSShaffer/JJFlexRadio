<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CWMessageAdd
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
        Me.KeyLabel = New System.Windows.Forms.Label()
        Me.KeyTextBox = New System.Windows.Forms.TextBox()
        Me.MessageLabel = New System.Windows.Forms.Label()
        Me.MessageTextBox = New System.Windows.Forms.TextBox()
        Me.OkButton = New System.Windows.Forms.Button()
        Me.CnclButton = New System.Windows.Forms.Button()
        Me.LabelLabel = New System.Windows.Forms.Label()
        Me.LabelTextBox = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'KeyLabel
        '
        Me.KeyLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.KeyLabel.AutoSize = True
        Me.KeyLabel.Location = New System.Drawing.Point(29, 50)
        Me.KeyLabel.Name = "KeyLabel"
        Me.KeyLabel.Size = New System.Drawing.Size(31, 13)
        Me.KeyLabel.TabIndex = 10
        Me.KeyLabel.Text = "Key: "
        '
        'KeyTextBox
        '
        Me.KeyTextBox.AccessibleName = "key"
        Me.KeyTextBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.KeyTextBox.Location = New System.Drawing.Point(60, 50)
        Me.KeyTextBox.Name = "KeyTextBox"
        Me.KeyTextBox.Size = New System.Drawing.Size(200, 20)
        Me.KeyTextBox.TabIndex = 11
        '
        'MessageLabel
        '
        Me.MessageLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.MessageLabel.AutoSize = True
        Me.MessageLabel.Location = New System.Drawing.Point(4, 80)
        Me.MessageLabel.Name = "MessageLabel"
        Me.MessageLabel.Size = New System.Drawing.Size(56, 13)
        Me.MessageLabel.TabIndex = 20
        Me.MessageLabel.Text = "Message: "
        '
        'MessageTextBox
        '
        Me.MessageTextBox.AccessibleName = "text to send"
        Me.MessageTextBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.MessageTextBox.Location = New System.Drawing.Point(60, 80)
        Me.MessageTextBox.Name = "MessageTextBox"
        Me.MessageTextBox.Size = New System.Drawing.Size(400, 20)
        Me.MessageTextBox.TabIndex = 21
        '
        'OkButton
        '
        Me.OkButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.OkButton.Location = New System.Drawing.Point(50, 120)
        Me.OkButton.Name = "OkButton"
        Me.OkButton.Size = New System.Drawing.Size(75, 23)
        Me.OkButton.TabIndex = 90
        Me.OkButton.Text = "Ok"
        Me.OkButton.UseVisualStyleBackColor = True
        '
        'CnclButton
        '
        Me.CnclButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.CnclButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CnclButton.Location = New System.Drawing.Point(300, 120)
        Me.CnclButton.Name = "CnclButton"
        Me.CnclButton.Size = New System.Drawing.Size(75, 23)
        Me.CnclButton.TabIndex = 95
        Me.CnclButton.Text = "Cancel"
        Me.CnclButton.UseVisualStyleBackColor = True
        '
        'LabelLabel
        '
        Me.LabelLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.LabelLabel.AutoSize = True
        Me.LabelLabel.Location = New System.Drawing.Point(21, 20)
        Me.LabelLabel.Name = "LabelLabel"
        Me.LabelLabel.Size = New System.Drawing.Size(39, 13)
        Me.LabelLabel.TabIndex = 5
        Me.LabelLabel.Text = "Label: "
        '
        'LabelTextBox
        '
        Me.LabelTextBox.AccessibleName = "Message label"
        Me.LabelTextBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.LabelTextBox.Location = New System.Drawing.Point(60, 20)
        Me.LabelTextBox.Name = "LabelTextBox"
        Me.LabelTextBox.Size = New System.Drawing.Size(200, 20)
        Me.LabelTextBox.TabIndex = 6
        '
        'CWMessageAdd
        '
        Me.AcceptButton = Me.OkButton
        Me.AccessibleRole = System.Windows.Forms.AccessibleRole.Window
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.CnclButton
        Me.ClientSize = New System.Drawing.Size(584, 162)
        Me.Controls.Add(Me.LabelTextBox)
        Me.Controls.Add(Me.LabelLabel)
        Me.Controls.Add(Me.CnclButton)
        Me.Controls.Add(Me.OkButton)
        Me.Controls.Add(Me.MessageTextBox)
        Me.Controls.Add(Me.MessageLabel)
        Me.Controls.Add(Me.KeyTextBox)
        Me.Controls.Add(Me.KeyLabel)
        Me.Name = "CWMessageAdd"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents KeyLabel As System.Windows.Forms.Label
    Friend WithEvents KeyTextBox As System.Windows.Forms.TextBox
    Friend WithEvents MessageLabel As System.Windows.Forms.Label
    Friend WithEvents MessageTextBox As System.Windows.Forms.TextBox
    Friend WithEvents OkButton As System.Windows.Forms.Button
    Friend WithEvents CnclButton As System.Windows.Forms.Button
    Friend WithEvents LabelLabel As System.Windows.Forms.Label
    Friend WithEvents LabelTextBox As System.Windows.Forms.TextBox
End Class
