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
        Me.components = New System.ComponentModel.Container()
        Me.ReceivedTextBox = New System.Windows.Forms.TextBox()
        Me.SentTextBox = New System.Windows.Forms.TextBox()
        Me.ReceiveLabel = New System.Windows.Forms.Label()
        Me.SendLabel = New System.Windows.Forms.Label()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.ActionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ListOperatorsMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ListRigsMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.W2ConfigToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LogOpenMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ShowBandsMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PanMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ImportMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LOTWMergeMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.CWMessageUpdateMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChangeKeysMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RestoreKeyMappingMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CWDecodeMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ClearOptionalMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.ScreenSaverMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FileExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ScreenFieldsMenu = New System.Windows.Forms.ToolStripMenuItem()
        Me.ScreenFieldsDefaultItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OperationsMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpPageItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpKeysItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpKeysalphabetic = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpKeysByFunction = New System.Windows.Forms.ToolStripMenuItem()
        Me.TraceMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpAboutItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OneSecond = New System.Windows.Forms.Timer(Me.components)
        Me.ScanTmr = New System.Windows.Forms.Timer(Me.components)
        Me.AntennaTuneButton = New System.Windows.Forms.Button()
        Me.RigFieldsBox = New System.Windows.Forms.TextBox()
        Me.TraceOpenFileDialog = New System.Windows.Forms.OpenFileDialog()
        Me.TransmitButton = New System.Windows.Forms.Button()
        Me.StatusBox = New RadioBoxes.MainBox()
        Me.TXTuneControl = New RadioBoxes.Combo()
        Me.ModeControl = New RadioBoxes.Combo()
        Me.FreqOut = New RadioBoxes.MainBox()
        Me.FlexKnobMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ReceivedTextBox
        '
        Me.ReceivedTextBox.AcceptsReturn = True
        Me.ReceivedTextBox.AccessibleName = ""
        Me.ReceivedTextBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Document
        Me.ReceivedTextBox.Cursor = System.Windows.Forms.Cursors.Default
        Me.ReceivedTextBox.Location = New System.Drawing.Point(0, 554)
        Me.ReceivedTextBox.Margin = New System.Windows.Forms.Padding(4)
        Me.ReceivedTextBox.Multiline = True
        Me.ReceivedTextBox.Name = "ReceivedTextBox"
        Me.ReceivedTextBox.Size = New System.Drawing.Size(799, 48)
        Me.ReceivedTextBox.TabIndex = 10001
        '
        'SentTextBox
        '
        Me.SentTextBox.AccessibleName = ""
        Me.SentTextBox.AccessibleRole = System.Windows.Forms.AccessibleRole.None
        Me.SentTextBox.AllowDrop = True
        Me.SentTextBox.Location = New System.Drawing.Point(0, 640)
        Me.SentTextBox.Margin = New System.Windows.Forms.Padding(4)
        Me.SentTextBox.Multiline = True
        Me.SentTextBox.Name = "SentTextBox"
        Me.SentTextBox.Size = New System.Drawing.Size(799, 48)
        Me.SentTextBox.TabIndex = 10011
        '
        'ReceiveLabel
        '
        Me.ReceiveLabel.AutoSize = True
        Me.ReceiveLabel.Location = New System.Drawing.Point(11, 534)
        Me.ReceiveLabel.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.ReceiveLabel.Name = "ReceiveLabel"
        Me.ReceiveLabel.Size = New System.Drawing.Size(98, 17)
        Me.ReceiveLabel.TabIndex = 10000
        Me.ReceiveLabel.Text = "Received Text"
        '
        'SendLabel
        '
        Me.SendLabel.AutoSize = True
        Me.SendLabel.Location = New System.Drawing.Point(11, 620)
        Me.SendLabel.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.SendLabel.Name = "SendLabel"
        Me.SendLabel.Size = New System.Drawing.Size(91, 17)
        Me.SendLabel.TabIndex = 10010
        Me.SendLabel.Text = "Sending Text"
        '
        'MenuStrip1
        '
        Me.MenuStrip1.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ActionsToolStripMenuItem, Me.ScreenFieldsMenu, Me.OperationsMenuItem, Me.HelpToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Padding = New System.Windows.Forms.Padding(8, 2, 0, 2)
        Me.MenuStrip1.Size = New System.Drawing.Size(912, 28)
        Me.MenuStrip1.TabIndex = 0
        '
        'ActionsToolStripMenuItem
        '
        Me.ActionsToolStripMenuItem.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuPopup
        Me.ActionsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ListOperatorsMenuItem, Me.ListRigsMenuItem, Me.FlexKnobMenuItem, Me.W2ConfigToolStripMenuItem, Me.LogOpenMenuItem, Me.ShowBandsMenuItem, Me.PanMenuItem, Me.ToolStripSeparator1, Me.ImportMenuItem, Me.ExportMenuItem, Me.LOTWMergeMenuItem, Me.ToolStripSeparator2, Me.CWMessageUpdateMenuItem, Me.ChangeKeysMenuItem, Me.RestoreKeyMappingMenuItem, Me.CWDecodeMenuItem, Me.ClearOptionalMenuItem, Me.ToolStripSeparator3, Me.ScreenSaverMenuItem, Me.FileExitToolStripMenuItem})
        Me.ActionsToolStripMenuItem.Name = "ActionsToolStripMenuItem"
        Me.ActionsToolStripMenuItem.Size = New System.Drawing.Size(70, 24)
        Me.ActionsToolStripMenuItem.Text = "Actions"
        '
        'ListOperatorsMenuItem
        '
        Me.ListOperatorsMenuItem.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem
        Me.ListOperatorsMenuItem.Name = "ListOperatorsMenuItem"
        Me.ListOperatorsMenuItem.Size = New System.Drawing.Size(275, 26)
        Me.ListOperatorsMenuItem.Text = "List &Operators"
        '
        'ListRigsMenuItem
        '
        Me.ListRigsMenuItem.Name = "ListRigsMenuItem"
        Me.ListRigsMenuItem.Size = New System.Drawing.Size(275, 26)
        Me.ListRigsMenuItem.Text = "List &Rigs"
        '
        'W2ConfigToolStripMenuItem
        '
        Me.W2ConfigToolStripMenuItem.Name = "W2ConfigToolStripMenuItem"
        Me.W2ConfigToolStripMenuItem.Size = New System.Drawing.Size(275, 26)
        Me.W2ConfigToolStripMenuItem.Text = "&W2 Wattmeter"
        '
        'LogOpenMenuItem
        '
        Me.LogOpenMenuItem.Name = "LogOpenMenuItem"
        Me.LogOpenMenuItem.Size = New System.Drawing.Size(275, 26)
        Me.LogOpenMenuItem.Text = "&Logfile Characteristics"
        '
        'ShowBandsMenuItem
        '
        Me.ShowBandsMenuItem.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem
        Me.ShowBandsMenuItem.Name = "ShowBandsMenuItem"
        Me.ShowBandsMenuItem.Size = New System.Drawing.Size(275, 26)
        Me.ShowBandsMenuItem.Text = "Show &Bands and Frequencies"
        '
        'PanMenuItem
        '
        Me.PanMenuItem.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem
        Me.PanMenuItem.Name = "PanMenuItem"
        Me.PanMenuItem.Size = New System.Drawing.Size(275, 26)
        Me.PanMenuItem.Text = "Band &Pan"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(217, 6)
        '
        'ImportMenuItem
        '
        Me.ImportMenuItem.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem
        Me.ImportMenuItem.Name = "ImportMenuItem"
        Me.ImportMenuItem.Size = New System.Drawing.Size(275, 26)
        Me.ImportMenuItem.Text = "&Import Log"
        '
        'ExportMenuItem
        '
        Me.ExportMenuItem.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem
        Me.ExportMenuItem.Name = "ExportMenuItem"
        Me.ExportMenuItem.Size = New System.Drawing.Size(275, 26)
        Me.ExportMenuItem.Text = "&Export Log"
        '
        'LOTWMergeMenuItem
        '
        Me.LOTWMergeMenuItem.AccessibleName = "LOTW merge"
        Me.LOTWMergeMenuItem.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem
        Me.LOTWMergeMenuItem.Name = "LOTWMergeMenuItem"
        Me.LOTWMergeMenuItem.Size = New System.Drawing.Size(275, 26)
        Me.LOTWMergeMenuItem.Text = "LOTW &merge"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.AccessibleRole = System.Windows.Forms.AccessibleRole.Separator
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(217, 6)
        '
        'CWMessageUpdateMenuItem
        '
        Me.CWMessageUpdateMenuItem.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem
        Me.CWMessageUpdateMenuItem.Name = "CWMessageUpdateMenuItem"
        Me.CWMessageUpdateMenuItem.Size = New System.Drawing.Size(275, 26)
        Me.CWMessageUpdateMenuItem.Text = "Manage &CW Messages"
        '
        'ChangeKeysMenuItem
        '
        Me.ChangeKeysMenuItem.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem
        Me.ChangeKeysMenuItem.Name = "ChangeKeysMenuItem"
        Me.ChangeKeysMenuItem.Size = New System.Drawing.Size(275, 26)
        Me.ChangeKeysMenuItem.Text = "Change &key mapping"
        '
        'RestoreKeyMappingMenuItem
        '
        Me.RestoreKeyMappingMenuItem.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem
        Me.RestoreKeyMappingMenuItem.Name = "RestoreKeyMappingMenuItem"
        Me.RestoreKeyMappingMenuItem.Size = New System.Drawing.Size(275, 26)
        Me.RestoreKeyMappingMenuItem.Text = "Restore default key mapping"
        '
        'CWDecodeMenuItem
        '
        Me.CWDecodeMenuItem.AccessibleName = "CW decode"
        Me.CWDecodeMenuItem.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem
        Me.CWDecodeMenuItem.Name = "CWDecodeMenuItem"
        Me.CWDecodeMenuItem.Size = New System.Drawing.Size(275, 26)
        Me.CWDecodeMenuItem.Text = "CW &decode"
        '
        'ClearOptionalMenuItem
        '
        Me.ClearOptionalMenuItem.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem
        Me.ClearOptionalMenuItem.Name = "ClearOptionalMenuItem"
        Me.ClearOptionalMenuItem.Size = New System.Drawing.Size(275, 26)
        Me.ClearOptionalMenuItem.Text = "Show all &Messages"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(217, 6)
        '
        'ScreenSaverMenuItem
        '
        Me.ScreenSaverMenuItem.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem
        Me.ScreenSaverMenuItem.Name = "ScreenSaverMenuItem"
        Me.ScreenSaverMenuItem.Size = New System.Drawing.Size(275, 26)
        Me.ScreenSaverMenuItem.Text = "Toggle Screen saver"
        '
        'FileExitToolStripMenuItem
        '
        Me.FileExitToolStripMenuItem.Name = "FileExitToolStripMenuItem"
        Me.FileExitToolStripMenuItem.ShortcutKeys = CType((System.Windows.Forms.Keys.Alt Or System.Windows.Forms.Keys.F4), System.Windows.Forms.Keys)
        Me.FileExitToolStripMenuItem.Size = New System.Drawing.Size(275, 26)
        Me.FileExitToolStripMenuItem.Text = "E&xit"
        '
        'ScreenFieldsMenu
        '
        Me.ScreenFieldsMenu.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuPopup
        Me.ScreenFieldsMenu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ScreenFieldsDefaultItem})
        Me.ScreenFieldsMenu.Name = "ScreenFieldsMenu"
        Me.ScreenFieldsMenu.Size = New System.Drawing.Size(103, 24)
        Me.ScreenFieldsMenu.Text = "Screen&Fields"
        '
        'ScreenFieldsDefaultItem
        '
        Me.ScreenFieldsDefaultItem.Name = "ScreenFieldsDefaultItem"
        Me.ScreenFieldsDefaultItem.Size = New System.Drawing.Size(126, 26)
        Me.ScreenFieldsDefaultItem.Text = "empty"
        '
        'OperationsMenuItem
        '
        Me.OperationsMenuItem.AccessibleName = "operations"
        Me.OperationsMenuItem.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuPopup
        Me.OperationsMenuItem.Name = "OperationsMenuItem"
        Me.OperationsMenuItem.Size = New System.Drawing.Size(94, 24)
        Me.OperationsMenuItem.Text = "Operations"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.HelpPageItem, Me.HelpKeysItem, Me.HelpKeysalphabetic, Me.HelpKeysByFunction, Me.TraceMenuItem, Me.HelpAboutItem})
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(53, 24)
        Me.HelpToolStripMenuItem.Text = "Help"
        '
        'HelpPageItem
        '
        Me.HelpPageItem.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem
        Me.HelpPageItem.Name = "HelpPageItem"
        Me.HelpPageItem.Size = New System.Drawing.Size(332, 26)
        Me.HelpPageItem.Text = "Help page"
        '
        'HelpKeysItem
        '
        Me.HelpKeysItem.Name = "HelpKeysItem"
        Me.HelpKeysItem.ShortcutKeyDisplayString = "F1"
        Me.HelpKeysItem.ShortcutKeys = System.Windows.Forms.Keys.F1
        Me.HelpKeysItem.Size = New System.Drawing.Size(332, 26)
        Me.HelpKeysItem.Text = "Key Assignments"
        '
        'HelpKeysalphabetic
        '
        Me.HelpKeysalphabetic.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem
        Me.HelpKeysalphabetic.Name = "HelpKeysalphabetic"
        Me.HelpKeysalphabetic.ShortcutKeyDisplayString = ""
        Me.HelpKeysalphabetic.Size = New System.Drawing.Size(332, 26)
        Me.HelpKeysalphabetic.Text = "Key Assignment sorted alphabetically"
        '
        'HelpKeysByFunction
        '
        Me.HelpKeysByFunction.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem
        Me.HelpKeysByFunction.Name = "HelpKeysByFunction"
        Me.HelpKeysByFunction.ShortcutKeyDisplayString = ""
        Me.HelpKeysByFunction.Size = New System.Drawing.Size(332, 26)
        Me.HelpKeysByFunction.Text = "Key Assignments by function"
        '
        'TraceMenuItem
        '
        Me.TraceMenuItem.Name = "TraceMenuItem"
        Me.TraceMenuItem.Size = New System.Drawing.Size(332, 26)
        Me.TraceMenuItem.Text = "&Tracing"
        '
        'HelpAboutItem
        '
        Me.HelpAboutItem.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem
        Me.HelpAboutItem.Name = "HelpAboutItem"
        Me.HelpAboutItem.ShortcutKeyDisplayString = ""
        Me.HelpAboutItem.Size = New System.Drawing.Size(332, 26)
        Me.HelpAboutItem.Text = "About"
        '
        'OneSecond
        '
        Me.OneSecond.Interval = 1000
        '
        'ScanTmr
        '
        '
        'AntennaTuneButton
        '
        Me.AntennaTuneButton.AccessibleName = ""
        Me.AntennaTuneButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.AntennaTuneButton.AutoSize = True
        Me.AntennaTuneButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.AntennaTuneButton.Enabled = False
        Me.AntennaTuneButton.Location = New System.Drawing.Point(533, 49)
        Me.AntennaTuneButton.Margin = New System.Windows.Forms.Padding(4)
        Me.AntennaTuneButton.Name = "AntennaTuneButton"
        Me.AntennaTuneButton.Size = New System.Drawing.Size(6, 6)
        Me.AntennaTuneButton.TabIndex = 40
        Me.AntennaTuneButton.UseVisualStyleBackColor = True
        Me.AntennaTuneButton.Visible = False
        '
        'RigFieldsBox
        '
        Me.RigFieldsBox.Enabled = False
        Me.RigFieldsBox.Location = New System.Drawing.Point(0, 129)
        Me.RigFieldsBox.Margin = New System.Windows.Forms.Padding(4)
        Me.RigFieldsBox.Multiline = True
        Me.RigFieldsBox.Name = "RigFieldsBox"
        Me.RigFieldsBox.Size = New System.Drawing.Size(932, 399)
        Me.RigFieldsBox.TabIndex = 100
        Me.RigFieldsBox.Visible = False
        '
        'TraceOpenFileDialog
        '
        Me.TraceOpenFileDialog.CheckFileExists = False
        Me.TraceOpenFileDialog.DefaultExt = "txt"
        Me.TraceOpenFileDialog.FileName = "DebugTrace"
        Me.TraceOpenFileDialog.Filter = "text file (*.txt)|*.txt"
        Me.TraceOpenFileDialog.Title = "File to receive the trace"
        '
        'TransmitButton
        '
        Me.TransmitButton.AutoSize = True
        Me.TransmitButton.Location = New System.Drawing.Point(667, 49)
        Me.TransmitButton.Margin = New System.Windows.Forms.Padding(4)
        Me.TransmitButton.Name = "TransmitButton"
        Me.TransmitButton.Size = New System.Drawing.Size(100, 28)
        Me.TransmitButton.TabIndex = 50
        Me.TransmitButton.Text = "Transmit"
        Me.TransmitButton.UseVisualStyleBackColor = True
        '
        'StatusBox
        '
        Me.StatusBox.AccessibleDescription = "status"
        Me.StatusBox.AccessibleName = "status"
        Me.StatusBox.Location = New System.Drawing.Point(11, 704)
        Me.StatusBox.Margin = New System.Windows.Forms.Padding(5)
        Me.StatusBox.Name = "StatusBox"
        Me.StatusBox.SelectionLength = 0
        Me.StatusBox.SelectionStart = 0
        Me.StatusBox.Size = New System.Drawing.Size(667, 25)
        Me.StatusBox.TabIndex = 10021
        '
        'TXTuneControl
        '
        Me.TXTuneControl.AccessibleName = ""
        Me.TXTuneControl.BoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.TXTuneControl.Enabled = False
        Me.TXTuneControl.ExpandedSize = New System.Drawing.Size(80, 60)
        Me.TXTuneControl.Header = "Tuner"
        Me.TXTuneControl.Location = New System.Drawing.Point(400, 31)
        Me.TXTuneControl.Margin = New System.Windows.Forms.Padding(5)
        Me.TXTuneControl.Name = "TXTuneControl"
        Me.TXTuneControl.ReadOnly = False
        Me.TXTuneControl.Size = New System.Drawing.Size(107, 44)
        Me.TXTuneControl.SmallSize = New System.Drawing.Size(80, 36)
        Me.TXTuneControl.TabIndex = 30
        Me.TXTuneControl.Tag = "Tuner"
        Me.TXTuneControl.TheList = Nothing
        Me.TXTuneControl.UpdateDisplayFunction = Nothing
        Me.TXTuneControl.UpdateRigFunction = Nothing
        Me.TXTuneControl.Visible = False
        '
        'ModeControl
        '
        Me.ModeControl.AccessibleName = ""
        Me.ModeControl.BoxStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ModeControl.ExpandedSize = New System.Drawing.Size(50, 80)
        Me.ModeControl.Header = "Mode"
        Me.ModeControl.Location = New System.Drawing.Point(287, 31)
        Me.ModeControl.Margin = New System.Windows.Forms.Padding(5)
        Me.ModeControl.Name = "ModeControl"
        Me.ModeControl.ReadOnly = False
        Me.ModeControl.Size = New System.Drawing.Size(67, 44)
        Me.ModeControl.SmallSize = New System.Drawing.Size(50, 36)
        Me.ModeControl.TabIndex = 20
        Me.ModeControl.Tag = "Mode"
        Me.ModeControl.TheList = Nothing
        Me.ModeControl.UpdateDisplayFunction = Nothing
        Me.ModeControl.UpdateRigFunction = Nothing
        '
        'FreqOut
        '
        Me.FreqOut.AccessibleRole = System.Windows.Forms.AccessibleRole.None
        Me.FreqOut.Location = New System.Drawing.Point(0, 49)
        Me.FreqOut.Margin = New System.Windows.Forms.Padding(5)
        Me.FreqOut.Name = "FreqOut"
        Me.FreqOut.SelectionLength = 0
        Me.FreqOut.SelectionStart = 0
        Me.FreqOut.Size = New System.Drawing.Size(267, 25)
        Me.FreqOut.TabIndex = 0
        '
        'FlexKnobMenuItem
        '
        Me.FlexKnobMenuItem.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem
        Me.FlexKnobMenuItem.Name = "FlexKnobMenuItem"
        Me.FlexKnobMenuItem.Size = New System.Drawing.Size(275, 26)
        Me.FlexKnobMenuItem.Text = "&Flex Knob Config"
        '
        'Form1
        '
        Me.AccessibleRole = System.Windows.Forms.AccessibleRole.Window
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(912, 743)
        Me.Controls.Add(Me.TransmitButton)
        Me.Controls.Add(Me.StatusBox)
        Me.Controls.Add(Me.TXTuneControl)
        Me.Controls.Add(Me.RigFieldsBox)
        Me.Controls.Add(Me.AntennaTuneButton)
        Me.Controls.Add(Me.ModeControl)
        Me.Controls.Add(Me.FreqOut)
        Me.Controls.Add(Me.SendLabel)
        Me.Controls.Add(Me.ReceiveLabel)
        Me.Controls.Add(Me.SentTextBox)
        Me.Controls.Add(Me.ReceivedTextBox)
        Me.Controls.Add(Me.MenuStrip1)
        Me.ForeColor = System.Drawing.SystemColors.ControlText
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "J J Radio"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ReceivedTextBox As System.Windows.Forms.TextBox
    Friend WithEvents SentTextBox As System.Windows.Forms.TextBox
    Friend WithEvents ReceiveLabel As System.Windows.Forms.Label
    Friend WithEvents SendLabel As System.Windows.Forms.Label
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents ActionsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ListRigsMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FileExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents W2ConfigToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OneSecond As System.Windows.Forms.Timer
    Friend WithEvents LogOpenMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ScanTmr As System.Windows.Forms.Timer
    Friend WithEvents ImportMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExportMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ScreenSaverMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ChangeKeysMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RestoreKeyMappingMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ListOperatorsMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpPageItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpKeysItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpAboutItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FreqOut As RadioBoxes.MainBox
    Friend WithEvents ModeControl As RadioBoxes.Combo
    Friend WithEvents AntennaTuneButton As System.Windows.Forms.Button
    Friend WithEvents RigFieldsBox As System.Windows.Forms.TextBox
    Friend WithEvents CWMessageUpdateMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TXTuneControl As RadioBoxes.Combo
    Friend WithEvents PanMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ShowBandsMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ScreenFieldsMenu As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ScreenFieldsDefaultItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TraceOpenFileDialog As System.Windows.Forms.OpenFileDialog
    Friend WithEvents TraceMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents StatusBox As RadioBoxes.MainBox
    Friend WithEvents TransmitButton As System.Windows.Forms.Button
    Friend WithEvents CWDecodeMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ClearOptionalMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OperationsMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LOTWMergeMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpKeysalphabetic As ToolStripMenuItem
    Friend WithEvents HelpKeysByFunction As ToolStripMenuItem
    Friend WithEvents FlexKnobMenuItem As ToolStripMenuItem
End Class
