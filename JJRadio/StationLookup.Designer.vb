<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class StationLookup
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
        Me.CallsignLabel = New System.Windows.Forms.Label()
        Me.CallsignBox = New System.Windows.Forms.TextBox()
        Me.LookupButton = New System.Windows.Forms.Button()
        Me.QTHLabel = New System.Windows.Forms.Label()
        Me.QTHBox = New System.Windows.Forms.TextBox()
        Me.StateLabel = New System.Windows.Forms.Label()
        Me.StateBox = New System.Windows.Forms.TextBox()
        Me.CountryLabel = New System.Windows.Forms.Label()
        Me.CountryBox = New System.Windows.Forms.TextBox()
        Me.DoneButton = New System.Windows.Forms.Button()
        Me.LatlongLabel = New System.Windows.Forms.Label()
        Me.LatlongBox = New System.Windows.Forms.TextBox()
        Me.CQLabel = New System.Windows.Forms.Label()
        Me.CQBox = New System.Windows.Forms.TextBox()
        Me.ITULabel = New System.Windows.Forms.Label()
        Me.ITUBox = New System.Windows.Forms.TextBox()
        Me.GMTLabel = New System.Windows.Forms.Label()
        Me.GMTBox = New System.Windows.Forms.TextBox()
        Me.NameLabel = New System.Windows.Forms.Label()
        Me.NameBox = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'CallsignLabel
        '
        Me.CallsignLabel.AutoSize = True
        Me.CallsignLabel.Location = New System.Drawing.Point(79, 20)
        Me.CallsignLabel.Name = "CallsignLabel"
        Me.CallsignLabel.Size = New System.Drawing.Size(69, 17)
        Me.CallsignLabel.TabIndex = 10
        Me.CallsignLabel.Text = "Call sign: "
        '
        'CallsignBox
        '
        Me.CallsignBox.AccessibleName = "call sign"
        Me.CallsignBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.CallsignBox.Location = New System.Drawing.Point(148, 20)
        Me.CallsignBox.Name = "CallsignBox"
        Me.CallsignBox.Size = New System.Drawing.Size(100, 22)
        Me.CallsignBox.TabIndex = 11
        '
        'LookupButton
        '
        Me.LookupButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.LookupButton.AutoSize = True
        Me.LookupButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.LookupButton.Location = New System.Drawing.Point(248, 20)
        Me.LookupButton.Name = "LookupButton"
        Me.LookupButton.Size = New System.Drawing.Size(65, 27)
        Me.LookupButton.TabIndex = 15
        Me.LookupButton.Text = "Lookup"
        Me.LookupButton.UseVisualStyleBackColor = True
        '
        'QTHLabel
        '
        Me.QTHLabel.AutoSize = True
        Me.QTHLabel.Location = New System.Drawing.Point(165, 100)
        Me.QTHLabel.Name = "QTHLabel"
        Me.QTHLabel.Size = New System.Drawing.Size(46, 17)
        Me.QTHLabel.TabIndex = 102
        Me.QTHLabel.Text = "QTH: "
        '
        'QTHBox
        '
        Me.QTHBox.AccessibleName = "qth"
        Me.QTHBox.Location = New System.Drawing.Point(211, 100)
        Me.QTHBox.Name = "QTHBox"
        Me.QTHBox.ReadOnly = True
        Me.QTHBox.Size = New System.Drawing.Size(265, 22)
        Me.QTHBox.TabIndex = 103
        Me.QTHBox.Tag = " "
        '
        'StateLabel
        '
        Me.StateLabel.AutoSize = True
        Me.StateLabel.Location = New System.Drawing.Point(411, 100)
        Me.StateLabel.Name = "StateLabel"
        Me.StateLabel.Size = New System.Drawing.Size(49, 17)
        Me.StateLabel.TabIndex = 105
        Me.StateLabel.Text = "State: "
        '
        'StateBox
        '
        Me.StateBox.AccessibleName = "state"
        Me.StateBox.Location = New System.Drawing.Point(460, 100)
        Me.StateBox.Name = "StateBox"
        Me.StateBox.ReadOnly = True
        Me.StateBox.Size = New System.Drawing.Size(40, 22)
        Me.StateBox.TabIndex = 106
        Me.StateBox.Tag = " "
        '
        'CountryLabel
        '
        Me.CountryLabel.AutoSize = True
        Me.CountryLabel.Location = New System.Drawing.Point(73, 140)
        Me.CountryLabel.Name = "CountryLabel"
        Me.CountryLabel.Size = New System.Drawing.Size(65, 17)
        Me.CountryLabel.TabIndex = 140
        Me.CountryLabel.Text = "Country: "
        '
        'CountryBox
        '
        Me.CountryBox.AccessibleName = "country"
        Me.CountryBox.Location = New System.Drawing.Point(138, 140)
        Me.CountryBox.Name = "CountryBox"
        Me.CountryBox.ReadOnly = True
        Me.CountryBox.Size = New System.Drawing.Size(300, 22)
        Me.CountryBox.TabIndex = 141
        Me.CountryBox.Tag = " "
        '
        'DoneButton
        '
        Me.DoneButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.DoneButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.DoneButton.Location = New System.Drawing.Point(216, 200)
        Me.DoneButton.Name = "DoneButton"
        Me.DoneButton.Size = New System.Drawing.Size(75, 23)
        Me.DoneButton.TabIndex = 200
        Me.DoneButton.Text = "Done"
        Me.DoneButton.UseVisualStyleBackColor = True
        '
        'LatlongLabel
        '
        Me.LatlongLabel.AutoSize = True
        Me.LatlongLabel.Location = New System.Drawing.Point(62, 170)
        Me.LatlongLabel.Name = "LatlongLabel"
        Me.LatlongLabel.Size = New System.Drawing.Size(62, 17)
        Me.LatlongLabel.TabIndex = 170
        Me.LatlongLabel.Text = "lat/long: "
        '
        'LatlongBox
        '
        Me.LatlongBox.AccessibleName = "lat/long"
        Me.LatlongBox.Location = New System.Drawing.Point(122, 170)
        Me.LatlongBox.Name = "LatlongBox"
        Me.LatlongBox.ReadOnly = True
        Me.LatlongBox.Size = New System.Drawing.Size(100, 22)
        Me.LatlongBox.TabIndex = 171
        Me.LatlongBox.Tag = " "
        '
        'CQLabel
        '
        Me.CQLabel.AutoSize = True
        Me.CQLabel.Location = New System.Drawing.Point(222, 170)
        Me.CQLabel.Name = "CQLabel"
        Me.CQLabel.Size = New System.Drawing.Size(36, 17)
        Me.CQLabel.TabIndex = 174
        Me.CQLabel.Text = "CQ: "
        '
        'CQBox
        '
        Me.CQBox.AccessibleName = "cq zone"
        Me.CQBox.Location = New System.Drawing.Point(256, 170)
        Me.CQBox.Name = "CQBox"
        Me.CQBox.ReadOnly = True
        Me.CQBox.Size = New System.Drawing.Size(20, 22)
        Me.CQBox.TabIndex = 175
        Me.CQBox.Tag = " "
        '
        'ITULabel
        '
        Me.ITULabel.AutoSize = True
        Me.ITULabel.Location = New System.Drawing.Point(276, 170)
        Me.ITULabel.Name = "ITULabel"
        Me.ITULabel.Size = New System.Drawing.Size(38, 17)
        Me.ITULabel.TabIndex = 176
        Me.ITULabel.Text = "ITU: "
        '
        'ITUBox
        '
        Me.ITUBox.AccessibleName = "ITU zone"
        Me.ITUBox.Location = New System.Drawing.Point(314, 170)
        Me.ITUBox.Name = "ITUBox"
        Me.ITUBox.ReadOnly = True
        Me.ITUBox.Size = New System.Drawing.Size(20, 22)
        Me.ITUBox.TabIndex = 177
        Me.ITUBox.Tag = " "
        '
        'GMTLabel
        '
        Me.GMTLabel.AutoSize = True
        Me.GMTLabel.Location = New System.Drawing.Point(334, 170)
        Me.GMTLabel.Name = "GMTLabel"
        Me.GMTLabel.Size = New System.Drawing.Size(85, 17)
        Me.GMTLabel.TabIndex = 178
        Me.GMTLabel.Text = "GMTOffset: "
        '
        'GMTBox
        '
        Me.GMTBox.AccessibleName = "gmt offset"
        Me.GMTBox.Location = New System.Drawing.Point(419, 170)
        Me.GMTBox.Name = "GMTBox"
        Me.GMTBox.ReadOnly = True
        Me.GMTBox.Size = New System.Drawing.Size(30, 22)
        Me.GMTBox.TabIndex = 179
        Me.GMTBox.Tag = " "
        '
        'NameLabel
        '
        Me.NameLabel.AutoSize = True
        Me.NameLabel.Location = New System.Drawing.Point(12, 100)
        Me.NameLabel.Name = "NameLabel"
        Me.NameLabel.Size = New System.Drawing.Size(53, 17)
        Me.NameLabel.TabIndex = 100
        Me.NameLabel.Text = "Name: "
        '
        'NameBox
        '
        Me.NameBox.AccessibleName = "name"
        Me.NameBox.Location = New System.Drawing.Point(65, 100)
        Me.NameBox.Name = "NameBox"
        Me.NameBox.Size = New System.Drawing.Size(100, 22)
        Me.NameBox.TabIndex = 101
        Me.NameBox.Tag = " "
        '
        'StationLookup
        '
        Me.AcceptButton = Me.LookupButton
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.DoneButton
        Me.ClientSize = New System.Drawing.Size(494, 253)
        Me.Controls.Add(Me.NameBox)
        Me.Controls.Add(Me.NameLabel)
        Me.Controls.Add(Me.GMTBox)
        Me.Controls.Add(Me.GMTLabel)
        Me.Controls.Add(Me.ITUBox)
        Me.Controls.Add(Me.ITULabel)
        Me.Controls.Add(Me.CQBox)
        Me.Controls.Add(Me.CQLabel)
        Me.Controls.Add(Me.LatlongBox)
        Me.Controls.Add(Me.LatlongLabel)
        Me.Controls.Add(Me.DoneButton)
        Me.Controls.Add(Me.CountryBox)
        Me.Controls.Add(Me.CountryLabel)
        Me.Controls.Add(Me.StateBox)
        Me.Controls.Add(Me.StateLabel)
        Me.Controls.Add(Me.QTHBox)
        Me.Controls.Add(Me.QTHLabel)
        Me.Controls.Add(Me.LookupButton)
        Me.Controls.Add(Me.CallsignBox)
        Me.Controls.Add(Me.CallsignLabel)
        Me.Name = "StationLookup"
        Me.Text = "Station Lookup"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents CallsignLabel As Label
    Friend WithEvents CallsignBox As TextBox
    Friend WithEvents LookupButton As Button
    Friend WithEvents QTHLabel As Label
    Friend WithEvents QTHBox As TextBox
    Friend WithEvents StateLabel As Label
    Friend WithEvents StateBox As TextBox
    Friend WithEvents CountryLabel As Label
    Friend WithEvents CountryBox As TextBox
    Friend WithEvents DoneButton As Button
    Friend WithEvents LatlongLabel As Label
    Friend WithEvents LatlongBox As TextBox
    Friend WithEvents CQLabel As Label
    Friend WithEvents CQBox As TextBox
    Friend WithEvents ITULabel As Label
    Friend WithEvents ITUBox As TextBox
    Friend WithEvents GMTLabel As Label
    Friend WithEvents GMTBox As TextBox
    Friend WithEvents NameLabel As Label
    Friend WithEvents NameBox As TextBox
End Class
