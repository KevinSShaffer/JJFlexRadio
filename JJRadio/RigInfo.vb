Imports System.Collections
Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.IO.Ports
Imports JJRadio.RigData
Imports JJTrace

Public Class RigInfo
    ''' <summary> collection of existing rigs </summary>
    Friend RigList As List(Of rig)
    ''' <summary>
    ''' Rig we're working on, Nothing for new rigs 
    ''' Passed back on an update.
    ''' </summary>
    Friend theRig As rig
    Private wasActive As Boolean
    Private UpdateFlag As Boolean
    Const dupNameMsg As String = "A rig with this name already exists."
    Const mustSelectMsg As String = "You must select a "
    Const nameMsg As String = "name."
    Const modelMsg As String = "model."
    Const comportMsg As String = "COM port."
    Const baudrateMsg As String = "baud rate."
    Const parityMsg As String = "parity."
    Const databitsMsg As String = "data bits value."
    Const stopbitsMsg As String = "stop bits value."
    Const handshakeMsg As String = "handshake value."
    Const networkRadioMsg As String = "network radio"

    Private Sub setupBox(ByVal items As Object, ByVal o As Object)
        If o.GetType.Name = "ComboBox" Then
            Dim box As ComboBox = o
            box.DataSource = items
            box.DisplayMember = "Name"
            box.ValueMember = "id"
            box.SelectedIndex = -1
        Else
            Dim box As ListBox = o
            box.DataSource = items
            box.DisplayMember = "Name"
            box.ValueMember = "id"
            box.ClearSelected()
        End If
    End Sub

    Private Sub clear()
        For Each o As Object In Controls
            Select Case o.GetType.Name
                Case "ListBox"
                    Dim box As ListBox = o
                    box.ClearSelected()
                Case "ComboBox"
                    Dim box As ComboBox = o
                    box.Text = ""
                    box.SelectedIndex = -1
                Case "TextBox"
                    o.Text = ""
                Case "CheckBox"
                    o.Checked =
                        ((Rigs Is Nothing) OrElse (RigList.Count = 0))
            End Select
        Next
        NameBox.Focus()
    End Sub

    Private Sub setupButtons()
        UpdateButton.Visible = UpdateFlag
        UpdateButton.Enabled = UpdateFlag
        AddAnotherButton.Enabled = (Not UpdateFlag)
        AddAnotherButton.Visible = (Not UpdateFlag)
    End Sub

    Private Sub RigInfo_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        DialogResult = DialogResult.None
        OKOnCancel = Windows.Forms.DialogResult.Cancel
        wasActive = False
        setupBox(Models, ModelList)
        setupBox(BaudRates, BaudRateCombo)
        PortNameList.Items.Clear()
        For Each p As String In SerialPort.GetPortNames
            PortNameList.Items.Add(p)
        Next
        setupBox(Parities, ParityList)
        setupBox(DataBits, DataBitsList)
        setupBox(StopBitses, StopBitsList)
        setupBox(HandShakes, HandShakeList)
        netInfoList = New ArrayList

        If theRig Is Nothing Then
            ' It's an add.
            UpdateFlag = False
            clear()
        Else
            ' It's an update.  Setup the screen fields.
            UpdateFlag = True
            showRigInfo(theRig)
            ' Can't change the default flag on an update if it's set.
            DefaultBox.Enabled = (Not theRig.DefaultFlag)
        End If
        setupButtons()
    End Sub

    Private Sub setSelected(ByVal id As Integer, ByVal lst As RigData.listData(),
                            ByVal o As Object)
        If o.GetType.Name = "ComboBox" Then
            o.SelectedIndex = -1
            o.Text = CStr(id)
        Else
            Dim i As Integer
            For i = 0 To lst.Length - 1
                If lst(i).id = id Then
                    Exit For
                End If
            Next
            If i = lst.Length Then
                i = -1
            End If
            o.SelectedIndex = i
        End If
    End Sub

    ' Only called on an update.
    Private Sub showRigInfo(item As rig)
        NameBox.Text = item.Name
        setSelected(item.model, RigData.Models, ModelList) ' sets netFlag.
        If netFlag Then
            ' nothing at this point.
        Else
            For i As Integer = 0 To PortNameList.Items.Count - 1
                If PortNameList.Items(i) = item.PortName Then
                    PortNameList.SelectedIndex = i
                    Exit For
                End If
            Next
            showRigGenericCom(item)
        End If
        DefaultBox.Checked = theRig.DefaultFlag
        AllowRemoteBox.Checked = theRig.AllowRemote
    End Sub

    Private Sub showRigGenericCom(item As rig)
        ' Should only be called for Com port devices.
        setSelected(item.parity, RigData.Parities, ParityList)
        setSelected(item.baudRate, RigData.BaudRates, BaudRateCombo)
        setSelected(item.dataBits, RigData.DataBits, DataBitsList)
        setSelected(item.stopBits, RigData.StopBitses, StopBitsList)
        setSelected(item.handShake, RigData.HandShakes, HandShakeList)
    End Sub

    Private Sub setupRig(ByVal r As rig, oldRig As rig)
        r.Name = NameBox.Text
        r.model = ModelList.SelectedValue
        If netFlag Then
            ' It looks like a Flex.
            If UpdateFlag Then
                ' Get from the old rig.
                r.DiscoveredInfo = oldRig.DiscoveredInfo
            Else
                ' Get from discovered info.
                r.DiscoveredInfo = netInfoList(DiscoveredListbox.SelectedIndex)
            End If
        Else
            r.PortName = PortNameList.Items(PortNameList.SelectedIndex)
            r.baudRate = BaudRateCombo.Text
            r.parity = ParityList.SelectedValue
            r.dataBits = DataBitsList.SelectedValue
            r.stopBits = StopBitsList.SelectedValue
            r.handShake = HandShakeList.SelectedValue
        End If
        r.DefaultFlag = DefaultBox.Checked
        r.AllowRemote = AllowRemoteBox.Checked
    End Sub

    Private Function add() As Boolean
        Dim rv As Boolean = checkRig()
        If rv Then
            Dim r As New rig
            setupRig(r, Nothing)
            RigList.Add(r)
        End If
        Return rv
    End Function

    Private Function checkRig() As Boolean
        ' com ports must specify all items.
        ' The name must be unique.
        Dim rv As Boolean
        Try
            checkField(NameBox, nameMsg)
            checkField(ModelList, modelMsg)
            If netFlag Then
                ' network rig
                If (Not UpdateFlag) AndAlso (DiscoveredListbox.SelectedIndex = -1) Then
                    DiscoveredListbox.Focus()
                    Throw New Exception(mustSelectMsg & networkRadioMsg)
                End If
            Else
                checkField(PortNameList, comportMsg)
                checkField(BaudRateCombo, baudrateMsg)
                checkField(ParityList, parityMsg)
                checkField(DataBitsList, databitsMsg)
                checkField(StopBitsList, stopbitsMsg)
                checkField(HandShakeList, handshakeMsg)
            End If
            rv = True
        Catch ex As Exception
            Tracing.TraceLine("checkRig:" & ex.Message, TraceLevel.Error)
            MsgBox(ex.Message)
            rv = False
        End Try
        If rv And (Not UpdateFlag) Then
            For Each r As rig In RigList
                If r.Name = NameBox.Text Then
                    MsgBox(dupNameMsg)
                    NameBox.Focus()
                    rv = False
                    Exit For
                End If
            Next
        End If
        Return rv
    End Function
    Private Sub checkField(ByVal o As Object, ByVal msg As String)
        Dim err As Boolean
        Select Case o.GetType.Name
            Case "TextBox"
                err = (o.Text = "")
            Case "ComboBox"
                err = (o.Text = "")
            Case "ListBox"
                err = (o.SelectedIndex = -1)
        End Select
        If err Then
            o.focus()
            Throw New Exception(mustSelectMsg & msg)
        End If
    End Sub

    Private Sub AddAnotherButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddAnotherButton.Click
        If add() Then
            OKOnCancel = Windows.Forms.DialogResult.OK ' cancel now returns OK.
            clear()
        End If
    End Sub

    Private Sub UpdateButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UpdateButton.Click
        Dim rv As Boolean = checkRig()
        If rv Then
            ' Pass back new rig.
            Dim newRig = New rig
            setupRig(newRig, theRig)
            theRig = newRig
            DialogResult = DialogResult.OK
        End If
    End Sub

    Private OKOnCancel As DialogResult
    Private Sub CnclButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CnclButton.Click
        DialogResult = OKOnCancel ' ok if at least one rig added.
    End Sub

    Private Sub RigInfo_Activated(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Activated
        If Not wasActive Then
            ' form just loaded.
            wasActive = True
            NameBox.Focus()
        End If
    End Sub

    Private Sub ModelList_Leave(sender As System.Object, e As System.EventArgs) Handles ModelList.Leave
        Dim id As Integer = ModelList.SelectedIndex
        If (id >= 0) And (theRig Is Nothing) Then
            ' This is an add, and a model was selected, setup com defaults.
            Dim rigComDefaults As Radios.RadioSelection.ComDefaults =
                Radios.RadioSelection.RigTable(id).ComDefaults
            Dim defaultCom As New rig(rigComDefaults)
            If (defaultCom IsNot Nothing) And
               ((rigComDefaults IsNot Nothing) AndAlso (rigComDefaults.ComType <> Radios.RadioSelection.ComType.network)) Then
                showRigGenericCom(defaultCom)
            End If
        End If
    End Sub

    Private netFlag As Boolean ' Set for a network radio (FlexLike).
    Private Sub ModelList_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ModelList.SelectedIndexChanged
        ' This sets netFlag.
        If ModelList.SelectedIndex = -1 Then
            netFlag = False
            Return
        End If
        ' We do all this to enable/disable the right entry fields.
        ' For a network radio, only the discovered list is enabled.
        Dim dflt As Radios.RadioSelection.ComDefaults =
            Radios.RadioSelection.RigTable(ModelList.SelectedIndex).ComDefaults
        Dim portFlag, baudFlag, comFlag As Boolean
        If dflt Is Nothing Then
            ' Generic comport device
            netFlag = False
            portFlag = True
            baudFlag = True
            comFlag = True
        Else
            netFlag = (dflt.ComType = Radios.RadioSelection.ComType.network)
            portFlag = (Not netFlag)
            If Not netFlag Then
                baudFlag = dflt.ExposeBaud
                comFlag = dflt.ExposeCom
            End If
        End If
        SuspendLayout()
        If UpdateFlag Then
            ' We don't discover network rigs on an update.
            DiscoveredLabel.Enabled = False
            DiscoveredLabel.Visible = False
            DiscoveredListbox.Enabled = False
            DiscoveredListbox.Visible = False
        Else
            DiscoveredLabel.Enabled = netFlag
            DiscoveredLabel.Visible = netFlag
            DiscoveredListbox.Enabled = netFlag
            DiscoveredListbox.Visible = netFlag
        End If
        PortNameLabel.Enabled = portFlag
        PortNameLabel.Visible = portFlag
        PortNameList.Enabled = portFlag
        PortNameList.Visible = portFlag
        BaudRateLabel.Enabled = baudFlag
        BaudRateCombo.Enabled = baudFlag
        BaudRateLabel.Visible = baudFlag
        BaudRateCombo.Visible = baudFlag
        ParityLabel.Enabled = comFlag
        ParityLabel.Visible = comFlag
        ParityList.Enabled = comFlag
        ParityList.Visible = comFlag
        DataBitsLabel.Enabled = comFlag
        DataBitsLabel.Visible = comFlag
        DataBitsList.Enabled = comFlag
        DataBitsList.Visible = comFlag
        StopBitsLabel.Enabled = comFlag
        StopBitsLabel.Visible = comFlag
        StopBitsList.Enabled = comFlag
        StopBitsList.Visible = comFlag
        HandShakeLabel.Enabled = comFlag
        HandShakeLabel.Visible = comFlag
        HandShakeList.Enabled = comFlag
        HandShakeList.Visible = comFlag
        ResumeLayout()
    End Sub

    Private Sub DiscoveredListbox_Enter(sender As System.Object, e As System.EventArgs) Handles DiscoveredListbox.Enter
        ' Discover network radios.
        RemoveHandler Radios.AllRadios.RadioDiscoveredEvent, AddressOf radioFound
        AddHandler Radios.AllRadios.RadioDiscoveredEvent, AddressOf radioFound
        Radios.AllRadios.DiscoverRadios()
    End Sub

    Private Sub DiscoveredListbox_Leave(sender As Object, e As EventArgs) Handles DiscoveredListbox.Leave
        ' Leave if something is selected.
        If (DiscoveredListbox.SelectedIndex <> -1) Then
            Return
        End If

        Dim info = Radios.AllRadios.ManualNetworkRadioInfo()
        If info IsNot Nothing Then
            listAdd(info)
            DiscoveredListbox.SelectedIndex = 0 ' indicate selected
        End If
    End Sub

    Private netInfoList As ArrayList
    Private Delegate Sub listAddDel(info As Radios.AllRadios.RadioDiscoveredEventArgs)
    Private listAdd As listAddDel =
        Sub(info As Radios.AllRadios.RadioDiscoveredEventArgs)
            netInfoList.Add(info)
            DiscoveredListbox.Items.Add(info.Name & " - " & info.Model & " " & info.Serial)
        End Sub
    Private Sub radioFound(info As Radios.AllRadios.RadioDiscoveredEventArgs)
        Tracing.TraceLine("radioFound info:" & info.Name, TraceLevel.Info)
        Try
            DiscoveredListbox.Invoke(listAdd, New Object() {info})
        Catch ex As Exception
            Tracing.TraceLine("radioFound exception:" + ex.Message, TraceLevel.Error)
        End Try
    End Sub
End Class