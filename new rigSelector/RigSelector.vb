Imports System.Diagnostics
Imports System.IO
Imports System.Threading
Imports System.Xml.Serialization
Imports Flex.Smoothlake.FlexLib
Imports JJTrace
Imports MsgLib
Imports Radios

Public Class RigSelector
    Private Const mustSelect As String = "You must select a radio."
    Private Const connectOnText As String = "Auto connect On"
    Private Const connectOffText As String = "Auto connect Off"
    Private Class radio_t
        Public Rig As FlexBase.RigData
        Public Property AutoConnect As Boolean
        Public Property LowBW As Boolean
        Public ReadOnly Property Display As String
            Get
                Dim lbw As String = ""
                If LowBW Then
                    lbw = "LowBW_"
                End If
                Return lbw & Rig.Name & " " & Rig.ModelName & " " & Rig.Serial
            End Get
        End Property
        Public ReadOnly Property Value As radio_t
            Get
                Return Me
            End Get
        End Property
        Public Sub New(r As FlexBase.RigData)
            Rig = r
        End Sub
    End Class
    Private RadiosList As List(Of radio_t) = New List(Of radio_t)()

    ' autoConnect config data
    Public Class AutoConnectData
        Public Desired As Boolean
        Public Serial As String = ""
        Public LowBW As Boolean
    End Class
    Private autoConnectItem As AutoConnectData
    ' autoConnect config file name.
    Private ReadOnly Property autoConnectFileName As String
        Get
            Return BaseConfigDir & "\" & PersonalData.UniqueOpName(CurrentOp) & "_" &
                "autoConnect.xml"
        End Get
    End Property

    ''' <summary>
    ''' set if initial bringup
    ''' Set externally before showDialog.
    ''' </summary>
    Public Property initialBringup As Boolean
    ''' <summary>
    ''' Radio config parameters.
    ''' Set externally before showDialog.
    ''' </summary>
    Public Callouts As FlexBase.OpenParms

    Private Sub RigSelector_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Tracing.TraceLine("RigSelector_Load:" & initialBringup.ToString(), TraceLevel.Info)
        DialogResult = DialogResult.None

        ' get autoConnect config info.
        Dim fs As Stream = Nothing
        autoConnectItem = New AutoConnectData ' an empty item
        Try
            fs = File.Open(autoConnectFileName, FileMode.Open)
            ' Just skip if the file wasn't found.  Use the empty item.
            Dim xs = New XmlSerializer(GetType(AutoConnectData))
            autoConnectItem = xs.Deserialize(fs)
            Tracing.TraceLine("RigSelector_Load:autoConnect " & autoConnectItem.Serial, TraceLevel.Info)
        Catch nf As FileNotFoundException
            ' no action
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            If fs IsNot Nothing Then
                fs.Dispose()
            End If
        End Try

        ' Start the autoConnect timer if appropriate.
        If initialBringup And
           (autoConnectItem.Serial <> "") And autoConnectItem.Desired Then
            AutoConnectTimer.Enabled = True
        Else
            AutoConnectTimer.Enabled = False
        End If

        RigControl = New FlexBase(Callouts)
        AddHandler FlexBase.RadioFound, AddressOf radioFoundHandler
        AddHandler RigControl.NoSliceError, AddressOf noSliceErrorHandler

        ' Show any local radios.
        RigControl.LocalRadios()
    End Sub

    Private connectThread As Thread = Nothing
    Private Sub radioFoundHandler(sender As Object, e As FlexBase.RigData)
        Tracing.TraceLine("radioFoundHandler:" & e.Serial, TraceLevel.Info)
        Dim radio = New radio_t(e)
        If (autoConnectItem.Serial = radio.Rig.Serial) Then
            ' matches the autoConnect item
            radio.AutoConnect = autoConnectItem.Desired
            radio.LowBW = autoConnectItem.LowBW
        End If
        SyncLock RadiosList
            RadiosList.Add(radio)
        End SyncLock
        redisplayRadiosBox()
    End Sub
    Private Sub connectThreadProc(o As Object)
        Dim radio As radio_t = CType(o, radio_t)
        Tracing.TraceLine("connectThreadProc:" & radio.Rig.Serial, TraceLevel.Info)

        ' This will exit this form.
        CurrentRig = radio.Rig
        If RigControl.Connect(CurrentRig.Serial, radio.LowBW) Then
            DialogResult = DialogResult.OK
        Else
            DialogResult = DialogResult.No
        End If
        Me.Close()
    End Sub

    Private Sub redisplayRadiosBox()
        TextOut.PerformGenericFunction(RadiosBox,
            Sub()
                RadiosBox.DataSource = Nothing
                RadiosBox.DisplayMember = "Display"
                RadiosBox.ValueMember = "Value"
                RadiosBox.DataSource = RadiosList
            End Sub)
    End Sub

    Private Sub RadiosBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RadiosBox.SelectedIndexChanged
        If RadiosBox.SelectedIndex = -1 Then
            Return
        End If

        setupRadiosBoxContext()
    End Sub

    Private Sub setupRadiosBoxContext()
        Dim radio As radio_t = CType(RadiosBox.SelectedValue, radio_t)
        ' setup for autoConnect on/off
        If radio.AutoConnect Then
            RadiosBoxAutoConnectMenuItem.Text = connectOffText
        Else
            RadiosBoxAutoConnectMenuItem.Text = connectOnText
        End If
    End Sub

    Private Sub RemoteButton_Click(sender As Object, e As EventArgs) Handles RemoteButton.Click
        RigControl.RemoteRadios()
        RadiosBox.Focus()
    End Sub

    Private Sub LoginButton_Click(sender As Object, e As EventArgs) Handles LoginButton.Click
        RigControl.ClearWebCache()
        RigControl.RemoteRadios()
        RadiosBox.Focus()
    End Sub

    Private Sub AutoConnectTimer_Tick(sender As Object, e As EventArgs) Handles AutoConnectTimer.Tick
        SyncLock RadiosList
            For i As Integer = 0 To RadiosList.Count - 1
                Dim radio As radio_t = RadiosList(i)
                Dim id = i ' to avoid warning
                If radio.Rig.Serial = autoConnectItem.Serial Then
                    Tracing.TraceLine("AutoConnectTimer_Tick:autoConnect " & radio.Rig.Serial, TraceLevel.Info)
                    TextOut.PerformGenericFunction(ConnectButton,
                        Sub()
                            ' select radio and simulate an Enter.
                            RadiosBox.SelectedIndex = id
                            ConnectButton.Focus()
                            SendKeys.Send("{ENTER}")
                        End Sub)
                End If
            Next
        End SyncLock
    End Sub

    Private Sub ConnectButton_Click(sender As Object, e As EventArgs) Handles ConnectButton.Click
        If RadiosBox.SelectedIndex = -1 Then
            MsgBox(mustSelect)
            RadiosBox.Focus()
            Return
        End If

        ' This will exit this form.
        Dim radio As radio_t = CType(RadiosBox.SelectedValue, radio_t)
        Tracing.TraceLine("ConnectButton_Click:" & radio.Rig.Serial & " " & radio.LowBW.ToString(), TraceLevel.Info)
        CurrentRig = radio.Rig
        If RigControl.Connect(CurrentRig.Serial, radio.LowBW) Then
            DialogResult = DialogResult.OK
        Else
            DialogResult = DialogResult.No
        End If
    End Sub

    Private Sub LowBWConnectButton_Click(sender As Object, e As EventArgs) Handles LowBWConnectButton.Click
        If RadiosBox.SelectedIndex = -1 Then
            MsgBox(mustSelect)
            RadiosBox.Focus()
            Return
        End If
        Dim radio As radio_t = CType(RadiosBox.SelectedValue, radio_t)
        radio.LowBW = Not radio.LowBW
        If (autoConnectItem.Serial = radio.Rig.Serial) Then
            autoConnectItem.LowBW = radio.LowBW
        End If
        redisplayRadiosBox()
        RadiosBox.Focus()
    End Sub

    Private Sub CnclButton_Click(sender As Object, e As EventArgs) Handles CnclButton.Click
        Tracing.TraceLine("RigSelector.CnclButton_Click", TraceLevel.Info)
        If RigControl IsNot Nothing Then
            RigControl.Dispose()
            RigControl = Nothing
        End If
        DialogResult = DialogResult.Cancel
    End Sub

    Private Sub RadiosBox_Enter(sender As Object, e As EventArgs) Handles RadiosBox.Enter
        Me.AcceptButton = ConnectButton
    End Sub

    Private Sub RadiosBox_Leave(sender As Object, e As EventArgs) Handles RadiosBox.Leave
        Me.AcceptButton = Nothing
    End Sub

    Private Sub ConnectMenuItem_Click(sender As Object, e As EventArgs) Handles ConnectMenuItem.Click
        ConnectButton_Click(RadiosBoxContextMenuStrip, Nothing)
    End Sub

    Private Sub RadiosBoxLowBWMenuItem_Click(sender As Object, e As EventArgs) Handles RadiosBoxLowBWMenuItem.Click
        LowBWConnectButton_Click(RadiosBoxLowBWMenuItem, Nothing)
    End Sub

    Private Sub RadiosBoxAutoConnectMenuItem_Click(sender As Object, e As EventArgs) Handles RadiosBoxAutoConnectMenuItem.Click
        If RadiosBox.SelectedIndex = -1 Then
            MsgBox(mustSelect)
            RadiosBox.Focus()
            Return
        End If

        Dim radio As radio_t = CType(RadiosBox.SelectedValue, radio_t)
        Tracing.TraceLine("RadiosBoxAutoConnectMenuItem_Click:" & radio.Rig.Serial, TraceLevel.Info)
        ' setup the item
        ' Toggle if same item, otherwise new item.
        If (autoConnectItem.Serial = radio.Rig.Serial) Then
            autoConnectItem.Desired = Not autoConnectItem.Desired
        Else
            autoConnectItem.Desired = True
            autoConnectItem.Serial = radio.Rig.Serial
            autoConnectItem.LowBW = radio.LowBW
        End If
        radio.AutoConnect = autoConnectItem.Desired
        setupRadiosBoxContext()

        ' save autoConnect config
        Dim fs As Stream = Nothing
        Try
            fs = File.Open(autoConnectFileName, FileMode.Create)
            Dim xs = New XmlSerializer(GetType(AutoConnectData))
            xs.Serialize(fs, autoConnectItem)
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            If fs IsNot Nothing Then
                fs.Dispose()
            End If
        End Try
    End Sub

    Private Sub RigSelector_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Tracing.TraceLine("RigSelector_FormClosing", TraceLevel.Info)
        AutoConnectTimer.Dispose()
        RemoveHandler FlexBase.RadioFound, AddressOf radioFoundHandler
    End Sub

    Private Sub noSliceErrorHandler(sender As Object, msg As String)
        MsgBox(msg)
        DialogResult = DialogResult.Abort
        Close()
    End Sub
End Class