<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RigInfo
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
        Me.ModelLabel = New System.Windows.Forms.Label()
        Me.PortNameLabel = New System.Windows.Forms.Label()
        Me.BaudRateLabel = New System.Windows.Forms.Label()
        Me.BaudRateCombo = New System.Windows.Forms.ComboBox()
        Me.DataBitsLabel = New System.Windows.Forms.Label()
        Me.DataBitsList = New System.Windows.Forms.ListBox()
        Me.StopBitsLabel = New System.Windows.Forms.Label()
        Me.StopBitsList = New System.Windows.Forms.ListBox()
        Me.ParityLabel = New System.Windows.Forms.Label()
        Me.ParityList = New System.Windows.Forms.ListBox()
        Me.HandShakeLabel = New System.Windows.Forms.Label()
        Me.HandShakeList = New System.Windows.Forms.ListBox()
        Me.AddAnotherButton = New System.Windows.Forms.Button()
        Me.CnclButton = New System.Windows.Forms.Button()
        Me.PortNameList = New System.Windows.Forms.ListBox()
        Me.ModelList = New System.Windows.Forms.ListBox()
        Me.DefaultBox = New System.Windows.Forms.CheckBox()
        Me.UpdateButton = New System.Windows.Forms.Button()
        Me.DiscoveredLabel = New System.Windows.Forms.Label()
        Me.DiscoveredListbox = New System.Windows.Forms.ListBox()
        Me.AllowRemoteBox = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'NameLabel
        '
        Me.NameLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.NameLabel.AutoSize = True
        Me.NameLabel.Location = New System.Drawing.Point(36, 25)
        Me.NameLabel.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.NameLabel.Name = "NameLabel"
        Me.NameLabel.Size = New System.Drawing.Size(53, 17)
        Me.NameLabel.TabIndex = 10
        Me.NameLabel.Text = "Name: "
        '
        'NameBox
        '
        Me.NameBox.AccessibleName = "Rig name"
        Me.NameBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.NameBox.Location = New System.Drawing.Point(91, 25)
        Me.NameBox.Margin = New System.Windows.Forms.Padding(4)
        Me.NameBox.Name = "NameBox"
        Me.NameBox.Size = New System.Drawing.Size(199, 22)
        Me.NameBox.TabIndex = 11
        '
        'ModelLabel
        '
        Me.ModelLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.ModelLabel.AutoSize = True
        Me.ModelLabel.Location = New System.Drawing.Point(35, 68)
        Me.ModelLabel.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.ModelLabel.Name = "ModelLabel"
        Me.ModelLabel.Size = New System.Drawing.Size(54, 17)
        Me.ModelLabel.TabIndex = 20
        Me.ModelLabel.Text = "Model: "
        '
        'PortNameLabel
        '
        Me.PortNameLabel.AccessibleName = "Port name"
        Me.PortNameLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.ComboBox
        Me.PortNameLabel.AutoSize = True
        Me.PortNameLabel.Location = New System.Drawing.Point(265, 68)
        Me.PortNameLabel.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.PortNameLabel.Name = "PortNameLabel"
        Me.PortNameLabel.Size = New System.Drawing.Size(81, 17)
        Me.PortNameLabel.TabIndex = 30
        Me.PortNameLabel.Text = "Port name: "
        '
        'BaudRateLabel
        '
        Me.BaudRateLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.BaudRateLabel.AutoSize = True
        Me.BaudRateLabel.Location = New System.Drawing.Point(5, 111)
        Me.BaudRateLabel.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.BaudRateLabel.Name = "BaudRateLabel"
        Me.BaudRateLabel.Size = New System.Drawing.Size(83, 17)
        Me.BaudRateLabel.TabIndex = 40
        Me.BaudRateLabel.Text = "Baud Rate: "
        '
        'BaudRateCombo
        '
        Me.BaudRateCombo.AccessibleName = "Baud rate"
        Me.BaudRateCombo.AccessibleRole = System.Windows.Forms.AccessibleRole.ComboBox
        Me.BaudRateCombo.FormattingEnabled = True
        Me.BaudRateCombo.Location = New System.Drawing.Point(91, 111)
        Me.BaudRateCombo.Margin = New System.Windows.Forms.Padding(4)
        Me.BaudRateCombo.Name = "BaudRateCombo"
        Me.BaudRateCombo.Size = New System.Drawing.Size(79, 24)
        Me.BaudRateCombo.TabIndex = 41
        '
        'DataBitsLabel
        '
        Me.DataBitsLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.DataBitsLabel.AutoSize = True
        Me.DataBitsLabel.Location = New System.Drawing.Point(17, 154)
        Me.DataBitsLabel.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.DataBitsLabel.Name = "DataBitsLabel"
        Me.DataBitsLabel.Size = New System.Drawing.Size(72, 17)
        Me.DataBitsLabel.TabIndex = 60
        Me.DataBitsLabel.Text = "Data bits: "
        '
        'DataBitsList
        '
        Me.DataBitsList.AccessibleName = "Data bits"
        Me.DataBitsList.AccessibleRole = System.Windows.Forms.AccessibleRole.List
        Me.DataBitsList.FormattingEnabled = True
        Me.DataBitsList.ItemHeight = 16
        Me.DataBitsList.Location = New System.Drawing.Point(91, 154)
        Me.DataBitsList.Margin = New System.Windows.Forms.Padding(4)
        Me.DataBitsList.Name = "DataBitsList"
        Me.DataBitsList.Size = New System.Drawing.Size(12, 20)
        Me.DataBitsList.TabIndex = 61
        '
        'StopBitsLabel
        '
        Me.StopBitsLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.StopBitsLabel.AutoSize = True
        Me.StopBitsLabel.Location = New System.Drawing.Point(275, 154)
        Me.StopBitsLabel.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.StopBitsLabel.Name = "StopBitsLabel"
        Me.StopBitsLabel.Size = New System.Drawing.Size(71, 17)
        Me.StopBitsLabel.TabIndex = 70
        Me.StopBitsLabel.Text = "Stop bits: "
        '
        'StopBitsList
        '
        Me.StopBitsList.AccessibleName = "Stop bits"
        Me.StopBitsList.AccessibleRole = System.Windows.Forms.AccessibleRole.List
        Me.StopBitsList.FormattingEnabled = True
        Me.StopBitsList.ItemHeight = 16
        Me.StopBitsList.Location = New System.Drawing.Point(347, 154)
        Me.StopBitsList.Margin = New System.Windows.Forms.Padding(4)
        Me.StopBitsList.Name = "StopBitsList"
        Me.StopBitsList.Size = New System.Drawing.Size(12, 20)
        Me.StopBitsList.TabIndex = 71
        '
        'ParityLabel
        '
        Me.ParityLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.ParityLabel.AutoSize = True
        Me.ParityLabel.Location = New System.Drawing.Point(295, 111)
        Me.ParityLabel.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.ParityLabel.Name = "ParityLabel"
        Me.ParityLabel.Size = New System.Drawing.Size(52, 17)
        Me.ParityLabel.TabIndex = 50
        Me.ParityLabel.Text = "Parity: "
        '
        'ParityList
        '
        Me.ParityList.AccessibleName = "Parity"
        Me.ParityList.AccessibleRole = System.Windows.Forms.AccessibleRole.List
        Me.ParityList.FormattingEnabled = True
        Me.ParityList.ItemHeight = 16
        Me.ParityList.Location = New System.Drawing.Point(347, 111)
        Me.ParityList.Margin = New System.Windows.Forms.Padding(4)
        Me.ParityList.Name = "ParityList"
        Me.ParityList.Size = New System.Drawing.Size(65, 36)
        Me.ParityList.TabIndex = 51
        '
        'HandShakeLabel
        '
        Me.HandShakeLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.Text
        Me.HandShakeLabel.AutoSize = True
        Me.HandShakeLabel.Location = New System.Drawing.Point(0, 197)
        Me.HandShakeLabel.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.HandShakeLabel.Name = "HandShakeLabel"
        Me.HandShakeLabel.Size = New System.Drawing.Size(88, 17)
        Me.HandShakeLabel.TabIndex = 80
        Me.HandShakeLabel.Text = "Handshake: "
        '
        'HandShakeList
        '
        Me.HandShakeList.AccessibleName = "Handshake"
        Me.HandShakeList.AccessibleRole = System.Windows.Forms.AccessibleRole.List
        Me.HandShakeList.FormattingEnabled = True
        Me.HandShakeList.ItemHeight = 16
        Me.HandShakeList.Location = New System.Drawing.Point(91, 197)
        Me.HandShakeList.Margin = New System.Windows.Forms.Padding(4)
        Me.HandShakeList.Name = "HandShakeList"
        Me.HandShakeList.Size = New System.Drawing.Size(159, 36)
        Me.HandShakeList.TabIndex = 81
        '
        'AddAnotherButton
        '
        Me.AddAnotherButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.AddAnotherButton.AutoSize = True
        Me.AddAnotherButton.Location = New System.Drawing.Point(0, 308)
        Me.AddAnotherButton.Margin = New System.Windows.Forms.Padding(4)
        Me.AddAnotherButton.Name = "AddAnotherButton"
        Me.AddAnotherButton.Size = New System.Drawing.Size(100, 28)
        Me.AddAnotherButton.TabIndex = 970
        Me.AddAnotherButton.Text = "Add"
        Me.AddAnotherButton.UseVisualStyleBackColor = True
        '
        'CnclButton
        '
        Me.CnclButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.CnclButton.AutoSize = True
        Me.CnclButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CnclButton.Location = New System.Drawing.Point(133, 308)
        Me.CnclButton.Margin = New System.Windows.Forms.Padding(4)
        Me.CnclButton.Name = "CnclButton"
        Me.CnclButton.Size = New System.Drawing.Size(100, 28)
        Me.CnclButton.TabIndex = 990
        Me.CnclButton.Text = "Done"
        Me.CnclButton.UseVisualStyleBackColor = True
        '
        'PortNameList
        '
        Me.PortNameList.AccessibleName = "Port name"
        Me.PortNameList.AccessibleRole = System.Windows.Forms.AccessibleRole.List
        Me.PortNameList.FormattingEnabled = True
        Me.PortNameList.ItemHeight = 16
        Me.PortNameList.Location = New System.Drawing.Point(347, 68)
        Me.PortNameList.Margin = New System.Windows.Forms.Padding(4)
        Me.PortNameList.Name = "PortNameList"
        Me.PortNameList.Size = New System.Drawing.Size(65, 36)
        Me.PortNameList.TabIndex = 31
        '
        'ModelList
        '
        Me.ModelList.AccessibleName = "Rig model"
        Me.ModelList.AccessibleRole = System.Windows.Forms.AccessibleRole.List
        Me.ModelList.FormattingEnabled = True
        Me.ModelList.ItemHeight = 16
        Me.ModelList.Location = New System.Drawing.Point(91, 68)
        Me.ModelList.Margin = New System.Windows.Forms.Padding(4)
        Me.ModelList.Name = "ModelList"
        Me.ModelList.Size = New System.Drawing.Size(132, 36)
        Me.ModelList.TabIndex = 21
        '
        'DefaultBox
        '
        Me.DefaultBox.AccessibleName = "default"
        Me.DefaultBox.AccessibleRole = System.Windows.Forms.AccessibleRole.CheckButton
        Me.DefaultBox.AutoSize = True
        Me.DefaultBox.Location = New System.Drawing.Point(0, 271)
        Me.DefaultBox.Margin = New System.Windows.Forms.Padding(4)
        Me.DefaultBox.Name = "DefaultBox"
        Me.DefaultBox.Size = New System.Drawing.Size(75, 21)
        Me.DefaultBox.TabIndex = 200
        Me.DefaultBox.Text = "Default"
        Me.DefaultBox.UseVisualStyleBackColor = True
        '
        'UpdateButton
        '
        Me.UpdateButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.UpdateButton.AutoSize = True
        Me.UpdateButton.Enabled = False
        Me.UpdateButton.Location = New System.Drawing.Point(0, 308)
        Me.UpdateButton.Margin = New System.Windows.Forms.Padding(4)
        Me.UpdateButton.Name = "UpdateButton"
        Me.UpdateButton.Size = New System.Drawing.Size(100, 28)
        Me.UpdateButton.TabIndex = 970
        Me.UpdateButton.Text = "Update"
        Me.UpdateButton.UseVisualStyleBackColor = True
        Me.UpdateButton.Visible = False
        '
        'DiscoveredLabel
        '
        Me.DiscoveredLabel.AutoSize = True
        Me.DiscoveredLabel.Location = New System.Drawing.Point(200, 111)
        Me.DiscoveredLabel.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.DiscoveredLabel.Name = "DiscoveredLabel"
        Me.DiscoveredLabel.Size = New System.Drawing.Size(122, 17)
        Me.DiscoveredLabel.TabIndex = 40
        Me.DiscoveredLabel.Text = "Discovered radios"
        '
        'DiscoveredListbox
        '
        Me.DiscoveredListbox.AccessibleName = "discovered"
        Me.DiscoveredListbox.AccessibleRole = System.Windows.Forms.AccessibleRole.List
        Me.DiscoveredListbox.FormattingEnabled = True
        Me.DiscoveredListbox.ItemHeight = 16
        Me.DiscoveredListbox.Location = New System.Drawing.Point(5, 135)
        Me.DiscoveredListbox.Margin = New System.Windows.Forms.Padding(4)
        Me.DiscoveredListbox.Name = "DiscoveredListbox"
        Me.DiscoveredListbox.Size = New System.Drawing.Size(465, 84)
        Me.DiscoveredListbox.TabIndex = 41
        '
        'AllowRemoteBox
        '
        Me.AllowRemoteBox.AccessibleRole = System.Windows.Forms.AccessibleRole.CheckButton
        Me.AllowRemoteBox.AutoSize = True
        Me.AllowRemoteBox.Location = New System.Drawing.Point(8, 279)
        Me.AllowRemoteBox.Name = "AllowRemoteBox"
        Me.AllowRemoteBox.Size = New System.Drawing.Size(110, 21)
        Me.AllowRemoteBox.TabIndex = 210
        Me.AllowRemoteBox.Text = "Allow remote"
        Me.AllowRemoteBox.UseVisualStyleBackColor = True
        '
        'RigInfo
        '
        Me.AccessibleRole = System.Windows.Forms.AccessibleRole.Window
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.CnclButton
        Me.ClientSize = New System.Drawing.Size(512, 325)
        Me.Controls.Add(Me.AllowRemoteBox)
        Me.Controls.Add(Me.DiscoveredListbox)
        Me.Controls.Add(Me.DiscoveredLabel)
        Me.Controls.Add(Me.UpdateButton)
        Me.Controls.Add(Me.DefaultBox)
        Me.Controls.Add(Me.ModelList)
        Me.Controls.Add(Me.PortNameList)
        Me.Controls.Add(Me.CnclButton)
        Me.Controls.Add(Me.AddAnotherButton)
        Me.Controls.Add(Me.HandShakeList)
        Me.Controls.Add(Me.HandShakeLabel)
        Me.Controls.Add(Me.ParityList)
        Me.Controls.Add(Me.ParityLabel)
        Me.Controls.Add(Me.StopBitsList)
        Me.Controls.Add(Me.StopBitsLabel)
        Me.Controls.Add(Me.DataBitsList)
        Me.Controls.Add(Me.DataBitsLabel)
        Me.Controls.Add(Me.BaudRateCombo)
        Me.Controls.Add(Me.BaudRateLabel)
        Me.Controls.Add(Me.PortNameLabel)
        Me.Controls.Add(Me.ModelLabel)
        Me.Controls.Add(Me.NameBox)
        Me.Controls.Add(Me.NameLabel)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "RigInfo"
        Me.Text = "Rig Information"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents NameLabel As System.Windows.Forms.Label
    Friend WithEvents NameBox As System.Windows.Forms.TextBox
    Friend WithEvents ModelLabel As System.Windows.Forms.Label
    Friend WithEvents PortNameLabel As System.Windows.Forms.Label
    Friend WithEvents BaudRateLabel As System.Windows.Forms.Label
    Friend WithEvents BaudRateCombo As System.Windows.Forms.ComboBox
    Friend WithEvents DataBitsLabel As System.Windows.Forms.Label
    Friend WithEvents DataBitsList As System.Windows.Forms.ListBox
    Friend WithEvents StopBitsLabel As System.Windows.Forms.Label
    Friend WithEvents StopBitsList As System.Windows.Forms.ListBox
    Friend WithEvents ParityLabel As System.Windows.Forms.Label
    Friend WithEvents ParityList As System.Windows.Forms.ListBox
    Friend WithEvents HandShakeLabel As System.Windows.Forms.Label
    Friend WithEvents HandShakeList As System.Windows.Forms.ListBox
    Friend WithEvents AddAnotherButton As System.Windows.Forms.Button
    Friend WithEvents CnclButton As System.Windows.Forms.Button
    Friend WithEvents PortNameList As System.Windows.Forms.ListBox
    Friend WithEvents ModelList As System.Windows.Forms.ListBox
    Friend WithEvents DefaultBox As System.Windows.Forms.CheckBox
    Friend WithEvents UpdateButton As System.Windows.Forms.Button
    Friend WithEvents DiscoveredLabel As System.Windows.Forms.Label
    Friend WithEvents DiscoveredListbox As System.Windows.Forms.ListBox
    Friend WithEvents AllowRemoteBox As CheckBox
End Class
