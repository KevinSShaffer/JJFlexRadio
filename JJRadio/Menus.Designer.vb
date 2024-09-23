<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Menus
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
        Me.MenuListBox = New System.Windows.Forms.ListBox()
        Me.OKButton = New System.Windows.Forms.Button()
        Me.CnclButton = New System.Windows.Forms.Button()
        Me.BankCombo = New System.Windows.Forms.ComboBox()
        Me.BankLabel = New System.Windows.Forms.Label()
        Me.ValueBox = New System.Windows.Forms.GroupBox()
        Me.SuspendLayout()
        '
        'MenuListBox
        '
        Me.MenuListBox.AccessibleName = "menu"
        Me.MenuListBox.AccessibleRole = System.Windows.Forms.AccessibleRole.List
        Me.MenuListBox.FormattingEnabled = True
        Me.MenuListBox.Location = New System.Drawing.Point(0, 50)
        Me.MenuListBox.Name = "MenuListBox"
        Me.MenuListBox.Size = New System.Drawing.Size(300, 290)
        Me.MenuListBox.TabIndex = 10
        '
        'OKButton
        '
        Me.OKButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.OKButton.Location = New System.Drawing.Point(100, 530)
        Me.OKButton.Name = "OKButton"
        Me.OKButton.Size = New System.Drawing.Size(75, 23)
        Me.OKButton.TabIndex = 100
        Me.OKButton.Text = "OK"
        Me.OKButton.UseVisualStyleBackColor = True
        '
        'CnclButton
        '
        Me.CnclButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.CnclButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CnclButton.Location = New System.Drawing.Point(200, 530)
        Me.CnclButton.Name = "CnclButton"
        Me.CnclButton.Size = New System.Drawing.Size(75, 23)
        Me.CnclButton.TabIndex = 101
        Me.CnclButton.Text = "Cancel"
        Me.CnclButton.UseVisualStyleBackColor = True
        '
        'BankCombo
        '
        Me.BankCombo.AccessibleName = "menu bank"
        Me.BankCombo.AccessibleRole = System.Windows.Forms.AccessibleRole.ComboBox
        Me.BankCombo.FormattingEnabled = True
        Me.BankCombo.Location = New System.Drawing.Point(70, 20)
        Me.BankCombo.Name = "BankCombo"
        Me.BankCombo.Size = New System.Drawing.Size(30, 21)
        Me.BankCombo.TabIndex = 6
        '
        'BankLabel
        '
        Me.BankLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.BankLabel.AutoSize = True
        Me.BankLabel.Location = New System.Drawing.Point(3, 20)
        Me.BankLabel.Name = "BankLabel"
        Me.BankLabel.Size = New System.Drawing.Size(67, 13)
        Me.BankLabel.TabIndex = 5
        Me.BankLabel.Text = "Menu bank: "
        '
        'ValueBox
        '
        Me.ValueBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping
        Me.ValueBox.Location = New System.Drawing.Point(8, 28)
        Me.ValueBox.Name = "ValueBox"
        Me.ValueBox.Size = New System.Drawing.Size(220, 100)
        Me.ValueBox.TabIndex = 20
        Me.ValueBox.TabStop = False
        Me.ValueBox.Text = "Menu value"
        '
        'Menus
        '
        Me.AcceptButton = Me.OKButton
        Me.AccessibleRole = System.Windows.Forms.AccessibleRole.Window
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.CnclButton
        Me.ClientSize = New System.Drawing.Size(584, 564)
        Me.Controls.Add(Me.ValueBox)
        Me.Controls.Add(Me.BankLabel)
        Me.Controls.Add(Me.BankCombo)
        Me.Controls.Add(Me.CnclButton)
        Me.Controls.Add(Me.OKButton)
        Me.Controls.Add(Me.MenuListBox)
        Me.Name = "Menus"
        Me.Text = "Menus"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuListBox As System.Windows.Forms.ListBox
    Friend WithEvents OKButton As System.Windows.Forms.Button
    Friend WithEvents CnclButton As System.Windows.Forms.Button
    Friend WithEvents BankCombo As System.Windows.Forms.ComboBox
    Friend WithEvents BankLabel As System.Windows.Forms.Label
    Friend WithEvents ValueBox As System.Windows.Forms.GroupBox
End Class
