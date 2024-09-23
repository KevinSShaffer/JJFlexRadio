#Const LeaveBootTraceOn = 0
#Const PollTimerThread = 0
Imports System.Collections
Imports System.Collections.Specialized
Imports System.Configuration
Imports System.Diagnostics
Imports System.Drawing
Imports System.IO
Imports System.Math
Imports System.Threading
Imports JJLogIO
Imports JJLogLib
Imports JJTrace
Imports JJW2WattMeter
Imports MsgLib
Imports RadioBoxes
Imports Radios

Public Class Form1
    Const antennaTuneButtonBaseText As String = "Ant Tune"
    Const memorizedText As String = "Memorized"
    Private ReadOnly Property antennaTuneButtonText As String
        Get
            Dim rv = antennaTuneButtonBaseText
            If ((CurrentRig IsNot Nothing) And
                ((RigControl IsNot Nothing) AndAlso RigControl.Power)) AndAlso
                    (CurrentRig.FlexLike And RigControl.FlexTunerUsingMemoryNow) Then
                rv &= " mem"
            End If
            Return rv
        End Get
    End Property
    Private Function TBIDToTB(ByVal tbid As WindowIDs) As TextBox
        Dim tb As TextBox = Nothing
        Select Case tbid
            Case WindowIDs.ReceiveDataOut
                tb = ReceivedTextBox
            Case WindowIDs.SendDataOut
                tb = SentTextBox
        End Select
        Return tb
    End Function

    Dim onExitScreenSaver As Boolean
    Const pollTimerInterval As Integer = 100 ' .1 seconds
#If PollTimerThread = 1 Then
    Dim WithEvents thePollTimer As Timer = Nothing
    private Property PollTimer As Boolean
        Get
            Return (thePollTimer IsNot Nothing)
        End Get
        Set(value As Boolean)
            Tracing.TraceLine("PollTimer:" & value.ToString, TraceLevel.Info)
            If value Then
                Dim inst As New TimerCallback(AddressOf pollTimer_Tick)
                thePollTimer = New Timer(inst, Nothing, 0, pollTimerInterval)
            Else
                If thePollTimer IsNot Nothing Then
                    thePollTimer.Dispose()
                    thePollTimer = Nothing
                End If
            End If
        End Set
    End Property
#Else
    Dim WithEvents thePollTimer As System.Windows.Forms.Timer
    Private Property PollTimer As Boolean
        Get
            Return thePollTimer.Enabled
        End Get
        Set(value As Boolean)
            Tracing.TraceLine("PollTimer:" & value.ToString, TraceLevel.Info)
            If value Then
                thePollTimer = New System.Windows.Forms.Timer(components)
                AddHandler thePollTimer.Tick, AddressOf PollTimer_Tick
                thePollTimer.Interval = pollTimerInterval
                thePollTimer.Start()
            Else
                thePollTimer.Stop()
                thePollTimer.Dispose()
            End If
        End Set
    End Property
#End If

#If 0 Then
    Private Function getappSettings() As AppSettingsSection
        Dim appSettings As AppSettingsSection
        Try
            Dim config As Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
            appSettings = CType(config.GetSection("appSettings"), AppSettingsSection)
            If appSettings.Settings.Count = 0 Then
                appSettings = Nothing
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
            appSettings = Nothing
        End Try
        Return appSettings
    End Function
#End If

    ' Status line
    Private statusFields() As MainBox.Field =
    {New MainBox.Field(3, "pwr:", "  ", AddressOf adjustPower),
     New MainBox.Field(7, "mems:", "  ", Nothing),
     New MainBox.Field(7, "scan:", "  ", Nothing),
     New MainBox.Field(3, "Knob:", "  ", Nothing),
     New MainBox.Field(20, "log:", "", Nothing)
    }
    Friend Const statPowerID As Integer = 0
    Friend Const statMemoryID As Integer = 1
    Friend Const statScanID As Integer = 2
    Friend Const statKnobID As Integer = 3
    Friend Const statLogID As Integer = 4
    Private Sub statusSetup()
        StatusBox.Populate(statusFields)
        StatusBox.Clear()
        StatusBox.Write(statPowerID, OffWord)
        StatusBox.Write(statScanID, OffWord)
        StatusBox.Write(statKnobID, OffWord)
    End Sub

    Private Sub adjustPower(ByVal p As fieldFuncParm)
        p.rv = (p.k = Keys.Space)
        If p.rv Then
            If RigControl IsNot Nothing Then
                RigControl.Power = Not RigControl.Power
            End If
        End If
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        statusSetup() ' setup the status line.

        ' config-dependent menu items
        CWDecodeMenuItem.Enabled = False

        ' Create main objects.
        GetConfigInfo()
        RadioPort = New ComPort

        ' Add handlers to handle any future configuration changes.
        AddHandler Operators.ConfigEvent, AddressOf operatorChanged

        WriteText = AddressOf iWriteText
        WriteTextX = AddressOf iWriteTextX

        buttonTextQ = Queue.Synchronized(New Queue)
        buttonTextThread = New Thread(AddressOf buttonTextThreadProc)
        buttonTextThread.Start()

        ProgramDirectory = Directory.GetCurrentDirectory
        Tracing.TraceLine("Form1 load:" & ProgramDirectory, TraceLevel.Info)

#If 0 Then
        ' See if using the simulator (-s).
        UsingSimulator = (GetConfigValue("Simulator") = "yes")
        ' We don't load the simulator unless necessary.
        If UsingSimulator Then
            setupSimulator()
        End If
#End If

        ' Turn off the screen saver.
        onExitScreenSaver = setScreenSaver(False)

        openTheRadio()

        currentFields = vfoFreqFields

        FreqOut.BringToFront()
    End Sub

    ''' <summary>
    ''' Open the radio.
    ''' This will terminate the program if user elects to abort.
    ''' </summary>
    ''' <returns>True on success</returns>
    Friend Function openTheRadio() As Boolean
        Dim rv As Boolean = ((CurrentRig IsNot Nothing) AndAlso (RigControl IsNot Nothing))
        If Not rv Then
            Return rv
        End If
        OpenParms = New AllRadios.OpenParms()
        If (Not UsingSimulator) Then
            ' Open the Com port if not FlexLike.
            If Not CurrentRig.FlexLike Then
                Dim result As DialogResult = RadioPort.Open
                Tracing.TraceLine("openTheRadio:port open returned " & result.ToString, TraceLevel.Error)
                Select Case result
                    Case Windows.Forms.DialogResult.Abort
                        End
                    Case Windows.Forms.DialogResult.Ignore
                        ' Keep trying if we can't open the port.
                        KeepCheckingOpen = True ' Tell oneSecond timer routine to try the open.
                        OneSecond.Enabled = True
                        rv = False
                    Case Else
                        ' open worked
                End Select
            End If
        Else
            Tracing.TraceLine("openTheRadio:simulator", TraceLevel.Error)
        End If
        If rv Then
            ' add handlers for RigControl events.
            AddHandler RigControl.CompleteEvent, AddressOf CompleteEvent_Handler
            AddHandler RigControl.CapsChangeEvent, AddressOf rigCapsChanged
            AddHandler RigControl.TransmitChange, AddressOf transmitChangeProc
            'If UsingSimulator Then
            'OpenParms = simOpenParms
            'Else
            OpenParms.SendRoutine = AddressOf RadioPort.send
            OpenParms.SendBytesRoutine = AddressOf RadioPort.sendBytes
            OpenParms.DirectDataReceiver = AddressOf Commands.HandleDirect
            OpenParms.CWTextReceiver = AddressOf DisplayDecodedText
            OpenParms.NetworkRadio = CurrentRig.DiscoveredInfo
            'End If
            ' Frequency formatters
            OpenParms.FormatFreqForRadio = AddressOf UlongFreq
            OpenParms.FormatFreq = AddressOf FormatFreqUlong
            OpenParms.GotoHome = AddressOf gotoHome
            OpenParms.ConfigDirectory = BaseConfigDir & "\Radios"
            OpenParms.AudioDevicesFile = AudioDevicesFile
            OpenParms.GetOperatorName = AddressOf currentOperatorName
            OpenParms.BrailleCells = CurrentOp.BrailleDisplaySize
            OpenParms.License = CurrentOp.License
            OpenParms.AllowRemote = CurrentRig.AllowRemote
            rv = RigControl.Open(OpenParms)
            If rv Then
                Tracing.TraceLine("OpenTheRadio:rig is open", TraceLevel.Info)
                Escapes.Escapes.HexOnly = OpenParms.RawIO
                ' setup filter box
                If RigControl.RigFields IsNot Nothing Then
                    SuspendLayout()
                    RigControl.RigFields.RigControl.Location = RigFieldsBox.Location
                    RigControl.RigFields.RigControl.Size = RigFieldsBox.Size
                    RigControl.RigFields.RigControl.TabIndex = RigFieldsBox.TabIndex
                    AddHandler RigControl.RigFields.RigControl.KeyDown, AddressOf doCommand_KeyDown
                    Controls.Add(RigControl.RigFields.RigControl)
                    enableDisableControls.Add(RigControl.RigFields.RigControl)
                    ResumeLayout()
                End If
                ' Rig dependent menu items.
                CWDecodeMenuItem.Enabled = RigControl.myCaps.HasCap(RigCaps.Caps.CWDecode)
                ' disable window controls initially.
                enableDisableWindowControls(False)
                ' Start watching the radio.
                RigControl.Heartbeat = True
                ' Start polling for changes
                PollTimer = True
            Else : Tracing.TraceLine("OpenTheRadio:rig's open failed", TraceLevel.Error)
            End If
        End If
        Return rv
    End Function

    Friend Sub CloseTheRadio()
        Tracing.TraceLine("CloseTheRadio", TraceLevel.Info)
        StopKnob()
        RigControl.Heartbeat = False
        PollTimer = False
        SMeter.Peak = False
        If RigControl IsNot Nothing Then
            If RigControl.RigFields IsNot Nothing Then
                RemoveHandler RigControl.RigFields.RigControl.KeyDown, AddressOf doCommand_KeyDown
                Controls.Remove(RigControl.RigFields.RigControl)
            End If
            RigControl.close()
        End If
        ' Memories and menus must be reloaded.
        MemoriesLoaded = False
        StatusBox.Write(statMemoryID, " ")
        Menus.Done()
        powerWasOn = False
    End Sub

#If PollTimerThread = 1 Then
    Private Sub pollTimer_Tick()
        If thePollTimer IsNot Nothing Then
            UpdateStatus()
        Else
            Tracing.TraceLine("pollTimer_Tick:thePollTimer is stopped", TraceLevel.Info)
        End If
    End Sub
#Else
    Private Sub PollTimer_Tick(sender As System.Object, e As System.EventArgs)
        UpdateStatus()
    End Sub
#End If

    Const initialFreqPos As Integer = -3
    Friend firstFreqDisplay As Boolean = True
    Private Function getFreqPos() As Integer
        Dim rv As Integer
        Try
            ' Check for all blanks
            If Not FreqOut.IsEmpty(freqID) Then
                ' Get current freq
                Dim str As String = FreqOut.Read(freqID)
                ' Set freqPos.
                Dim cursor As Integer = FreqOut.SelectionStart - FreqOut.Position(freqID)
                If str(cursor) = "."c Then
                    cursor -= 1
                End If
                rv = 0
                While cursor < str.Length - 1
                    If (str(cursor) <> "."c) Then
                        rv -= 1
                    End If
                    cursor += 1
                End While
            Else
                ' frequency is empty.
                rv = 1
            End If
        Catch ex As Exception
            rv = 1
            Tracing.TraceLine("getFreqPos exception:" & ex.Message, TraceLevel.Error)
        End Try
        Return rv
    End Function

    Const xmitKey = Keys.X
    Private Sub AdjustFreq(ByVal p As fieldFuncParm)
        If p.FromRig Then
            FreqOut.Write(p.ID, FormatFreq(RigControl.Frequency))
        Else
            ' Supported keys: up and down arrow, D, U, A - D, =, space, S, T, V, X, K, minus, plus, and digits 0-9.
            ' You may not change the frequency here if transmitting.
            If (p.k <> xmitKey) And RigControl.Transmit Then
                Return
            End If

            Dim factor As Int64
            Dim longFreq As ULong
            Select Case p.k
                Case Keys.Up, Keys.Down, Keys.U, Keys.D
                    p.rv = True
                    getFreqAndFactor(longFreq, factor)
                    If (longFreq <> 0) And (factor > 0) Then
                        ' Get new value.
                        Select Case p.k
                            Case Keys.Up, Keys.U
                                longFreq += factor
                            Case Keys.Down, Keys.D
                                If factor < longFreq Then
                                    longFreq -= factor
                                End If
                        End Select
                        ' display and send to radio.
                        RigControl.Frequency = longFreq
                    End If
                Case Keys.K
                    ' Round to nearest khz.
                    Dim freq As Double = CType(RigControl.Frequency, Double)
                    freq = 1000 * Math.Round(freq / 1000)
                    RigControl.Frequency = CType(freq, ULong)
                Case Keys.S, Keys.T
                    ' Treat "s" and "t" as they are with split.
                    adjustSplit(p)
                Case Keys.V
                    ' vox on/off
                    RigControl.Vox = RigControl.ToggleOffOn(RigControl.Vox)
                Case xmitKey
                    ' transmit on/off
                    toggleTransmit()
                Case Keys.A, Keys.B, Keys.C, Keys.D, Keys.Oemplus
                    ' Adjust the VFO.
                    adjustVFO(p)
                Case Keys.OemMinus, Keys.Subtract
                    freqShiftFactor = -1
                Case (Keys.Oemplus Or Keys.Shift), Keys.Add
                    freqShiftFactor = 1
                Case Else
                    ' check for a digit
                    Dim baseNum As Integer = 0
                    If ((p.k >= Keys.D0) And (p.k <= Keys.D9)) Then
                        baseNum = Keys.D0
                    ElseIf ((p.k >= Keys.NumPad0) And (p.k <= Keys.NumPad9)) Then
                        baseNum = Keys.NumPad0
                    End If
                    If baseNum <> 0 Then
                        p.rv = True
                        getFreqAndFactor(longFreq, factor)
                        If (longFreq <> 0) And (factor > 0) Then
                            ' Get new value.
                            Dim num As Integer = p.k - baseNum
                            num *= freqShiftFactor * factor
                            longFreq += num
                            ' display and send to radio.
                            ' We display first here to help screen readers.
                            'FreqOut.Write(freqID, FormatFreq(longFreq))
                            'FreqOut.Display()
                            If RigControl.Split Then
                                RigControl.CopyVFO(RigControl.CurVFO, RigControl.NextVFO(RigControl.CurVFO))
                                RigControl.TXFrequency = longFreq
                            Else
                                RigControl.RXFrequency = longFreq
                            End If
                        End If
                    End If
            End Select
        End If
    End Sub

    ' This is 1 for plus, -1 for minus.
    Private freqShiftFactor As Integer = 1
    ' Used in adjustFreq above.
    Private Sub getFreqAndFactor(ByRef freq As ULong, ByRef fact As Int64)
        freq = 0
        fact = -1
        Dim fp As Integer = getFreqPos()
        If fp <= 0 Then
            freq = FreqInt64(FreqOut.Read(freqID))
            If freq <> 0 Then
                fact = 10 ^ (System.Math.Abs(fp))
            End If
        End If
    End Sub

    Private Sub adjustRit(p As fieldFuncParm)
        If p.FromRig Then
            If RigControl.myCaps.HasCap(RigCaps.Caps.RITGet) Then
                FreqOut.Write(p.ID, setRIT(RigControl.RIT, False))
            End If
        Else
            AdjustRITXIT(p, RigControl.RIT)
        End If
    End Sub

    Private Sub adjustXit(p As fieldFuncParm)
        If p.FromRig Then
            If RigControl.myCaps.HasCap(RigCaps.Caps.TXITGet) Then
                FreqOut.Write(xitID, setRIT(RigControl.XIT, True))
            End If
        Else
            AdjustRITXIT(p, RigControl.XIT)
        End If
    End Sub

    Private Sub AdjustRITXIT(ByVal p As fieldFuncParm, fld As AllRadios.RITData)
        Dim rv As AllRadios.RITData = New AllRadios.RITData(fld)
        ' Get cursor position within field.
        Dim cursor As Integer = FreqOut.SelectionStart - FreqOut.Position(p.ID)
        If (cursor < 0) Or (cursor > 4) Then
            ' bogus position
            Tracing.TraceLine("adjustRITXIT:bad position " & cursor.ToString, TraceLevel.Error)
            Return
        End If
        Dim fact As Integer
        ' get multiplication factor, 0 is first position, the sign.
        If cursor = 0 Then
            fact = 0
        Else
            fact = Math.Pow(10, 4 - cursor)
        End If
        p.rv = True
        Select Case p.k
            Case Keys.Space
                rv.Active = Not rv.Active
            Case Keys.Z, Keys.C
                ' Clear
                rv.Value = 0
            Case Keys.Up, Keys.U
                rv.Value += fact
            Case Keys.Down, Keys.D
                rv.Value -= fact
            Case Keys.Oemplus
                ' If RIT, turn on XIT, set XIT to RIT, and clear RIT.
                If p.ID = ritID Then
                    Dim dat = New AllRadios.RITData(rv)
                    RigControl.XIT = dat
                    rv.Value = 0
                End If
            Case Keys.OemMinus, Keys.Subtract
                freqShiftFactor = -1
            Case (Keys.Oemplus Or Keys.Shift), Keys.Add
                freqShiftFactor = 1
            Case Keys.V
                ' vox on/off
                RigControl.Vox = RigControl.ToggleOffOn(RigControl.Vox)
            Case xmitKey
                ' transmit on/off
                toggleTransmit()
            Case Keys.A, Keys.B, Keys.C, Keys.D
                ' Adjust the VFO.
                adjustVFO(p)
            Case Else
                ' Check for numeric keys
                Dim baseNum As Integer = 0
                If ((p.k >= Keys.D0) And (p.k <= Keys.D9)) Then
                    baseNum = Keys.D0
                ElseIf ((p.k >= Keys.NumPad0) And (p.k <= Keys.NumPad9)) Then
                    baseNum = Keys.NumPad0
                End If
                If baseNum <> 0 Then
                    rv.Value += freqShiftFactor * fact * (p.k - baseNum)
                Else
                    p.rv = False
                End If
        End Select
        If p.rv Then
            If (p.ID = ritID) Then
                RigControl.RIT = rv
            Else
                RigControl.XIT = rv
            End If
        End If
    End Sub

    Private Sub adjustSMeter(p As fieldFuncParm)
        If p.FromRig Then
            FreqOut.Write(p.ID, formatMeter(SMeter.Value))
        End If
    End Sub

    ''' <summary>
    ''' toggle split or show XMit frequency if in split mode.
    ''' </summary>
    ''' <param name="p"></param>
    ''' <remarks>toggle with space or up/down. show XMit freq with "t".</remarks>
    Private Sub adjustSplit(ByVal p As fieldFuncParm)
        If p.FromRig Then
            If RigControl.SplitShowXmitFrequency Then
                FreqOut.Write(p.ID, "T")
            ElseIf RigControl.Split Then
                FreqOut.Write(p.ID, "S")
            Else
                FreqOut.Write(p.ID, " ")
            End If
        Else
            p.rv = True
            Select Case p.k
                Case Keys.Up, Keys.Down, Keys.Space
                    ' If showXmitFrequency, turn it off, otherwise toggle split.
                    If RigControl.SplitShowXmitFrequency Then
                        RigControl.SplitShowXmitFrequency = False
                    Else
                        RigControl.Split = Not RigControl.Split
                    End If
                Case Keys.S
                    ' If showXmitFreq, turn it off, else toggle split.
                    If RigControl.SplitShowXmitFrequency Then
                        RigControl.SplitShowXmitFrequency = False
                    Else
                        RigControl.Split = Not RigControl.Split
                    End If
                Case Keys.T
                    ' if split, toggle showXmitFreq.
                    ' otherwise turn on split and showXmitFreq.
                    If RigControl.Split Then
                        RigControl.SplitShowXmitFrequency = Not RigControl.SplitShowXmitFrequency
                    Else
                        RigControl.Split = True
                        If Await(Function() RigControl.Split, 500) Then
                            RigControl.SplitShowXmitFrequency = True
                        End If
                    End If
                Case Else
                    p.rv = False
            End Select
        End If
    End Sub

    ''' <summary>
    ''' toggle the vox
    ''' </summary>
    ''' <param name="p"></param>
    ''' <remarks>toggle with space or up/down</remarks>
    Private Sub adjustVox(ByVal p As fieldFuncParm)
        If p.FromRig Then
            If RigControl.Vox Then
                FreqOut.Write(p.ID, "V")
            Else
                FreqOut.Write(p.ID, " ")
            End If
        Else
            If (p.k = Keys.Up) Or (p.k = Keys.Down) Or (p.k = Keys.Space) Then
                p.rv = True
                RigControl.Vox = RigControl.ToggleOffOn(RigControl.Vox)
            ElseIf p.k = Keys.V Then
                p.rv = True
                RigControl.Vox = AllRadios.OffOnValues.on
            End If
        End If
    End Sub
    ''' <summary>
    ''' adjust the VFO according to the key pressed.
    ''' </summary>
    ''' <param name="p"></param>
    ''' <remarks>
    ''' The keys are:  up/down/space - go to the next VFO.
    '''   m - toggle memory mode.
    '''   a or b - go to VFO a or b.
    '''   v - set the VFO to the current memory and switch to the VFO.
    '''   = - Set the next VFO to the current VFO, but doesn't switch VFOs.
    ''' </remarks>
    Private Sub adjustVFO(ByVal p As fieldFuncParm)
        If p.FromRig Then
            FreqOut.Write(p.ID, vfoLetter(RigControl.CurVFO))
        Else
            ' Make sure a vfo is showing.
            If FreqOut.IsEmpty(p.ID) Then
                Return
            End If
            Select Case p.k
                Case Keys.Up, Keys.Down, Keys.Space
                    p.rv = True
                    RigControl.CurVFO = RigControl.NextVFO(RigControl.CurVFO)
                Case Keys.A
                    p.rv = True
                    RigControl.CurVFO = RigCaps.VFOs.VFOA
                Case Keys.B
                    ' There's a B if more than 2 VFOs values.
                    If [Enum].GetValues(GetType(RigCaps.VFOs)).Length > 2 Then
                        p.rv = True
                        RigControl.CurVFO = RigCaps.VFOs.VFOB
                    End If
                Case Keys.M
                    p.rv = True
                    RigControl.MemoryMode = (Not RigControl.MemoryMode)
                Case Keys.V
                    ' Set VFO to current memory.
                    ' Note that if curVFO is None, it uses the last used real VFO.
                    RigControl.MemoryToVFO(RigControl.CurrentMemoryChannel,
                                           RigControl.CurVFO)
                Case Keys.Oemplus
                    If Not RigControl.MemoryMode Then
                        Dim vfo As RigCaps.VFOs = RigControl.NextVFO(RigControl.CurVFO)
                        If vfo <> RigControl.CurVFO Then
                            RigControl.CopyVFO(RigControl.CurVFO, vfo)
                        End If
                    End If
            End Select
        End If
    End Sub
    Private Sub adjustMem(ByVal p As fieldFuncParm)
        If p.FromRig Then
            FreqOut.Write(memNoID, RigControl.CurrentMemoryNumber.ToString)
        Else
            ' Don't do this until the memories are loaded.
            If Not MemoriesLoaded Then
                Return
            End If
            Dim incr As Integer
            If p.k = Keys.Up Then
                incr = 1
            ElseIf p.k = Keys.Down Then
                incr = -1
            Else
                Return
            End If
            Dim val As Integer = RigControl.CurrentMemoryChannel
            Dim sanity As Integer = RigControl.NumberOfMemories
            While sanity > 0
                val += incr
                If val < 0 Then
                    val = RigControl.NumberOfMemories - 1
                ElseIf val = RigControl.NumberOfMemories Then
                    val = 0
                End If
                If RigControl.Memories(val).Present Then
                    Exit While
                End If
                sanity -= 1
            End While
            If sanity > 0 Then
                p.rv = True
                RigControl.CurrentMemoryChannel = val
            End If
        End If
    End Sub

    Private Sub adjustOffset(p As fieldFuncParm)
        If p.FromRig Then
            FreqOut.Write(offsetID, formatOffset(RigControl.OffsetDirection))
        Else
            Select Case p.k
                Case Keys.Oemplus
                    RigControl.OffsetDirection = AllRadios.OffsetDirections.plus
                    p.rv = True
                Case Keys.OemMinus
                    RigControl.OffsetDirection = AllRadios.OffsetDirections.minus
                    p.rv = True
                Case Keys.Space, Keys.Down
                    ' cycle the value
                    Dim n As Integer = [Enum].GetValues(GetType(AllRadios.OffsetDirections)).Length
                    Dim val As AllRadios.OffsetDirections = RigControl.OffsetDirection
                    RigControl.OffsetDirection = CType(((val + 1) Mod n), AllRadios.OffsetDirections)
                    p.rv = True
                Case Keys.Up
                    ' cycle the value up
                    Dim n As Integer = [Enum].GetValues(GetType(AllRadios.OffsetDirections)).Length
                    Dim val As AllRadios.OffsetDirections = RigControl.OffsetDirection
                    If val = 0 Then
                        val = n - 1
                    Else
                        val -= 1
                    End If
                    RigControl.OffsetDirection = CType(val, AllRadios.OffsetDirections)
                    p.rv = True
            End Select
        End If
    End Sub

    Private Sub adjustRigField(p As fieldFuncParm)
        If p.FromRig Then
            If (p.ID = rigField1ID) And (OpenParms.RigField1 IsNot Nothing) Then
                FreqOut.Write(p.ID, OpenParms.RigField1.value)
            End If
            If (p.ID = rigField2ID) And (OpenParms.RigField2 IsNot Nothing) Then
                FreqOut.Write(p.ID, OpenParms.RigField2.value)
            End If
        Else
            ' must be without modifiers.
            If (p.k And Keys.Modifiers) <> 0 Then
                Return
            End If
            ' Fixup p.k to a lower case letter.
            If ((p.k >= Keys.A) And (p.k <= Keys.Z)) And Not (p.k And Keys.Shift) Then
                p.k += 32
            End If
            Dim fld As AllRadios.RigDependent
            If p.ID = rigField1ID Then
                fld = OpenParms.RigField1
            Else
                fld = OpenParms.RigField2
            End If
            If fld Is Nothing Then
                Return
            End If
            Dim rv As Char
            If (p.k = Keys.Space) Then
                ' Toggle on or off.
                p.rv = True
                fld.Active = (Not fld.Active)
                rv = fld.value
            Else
                ' not an active toggle, field must be active.
                If Not fld.Active Then
                    ' p.rv = True
                    rv = fld.value
                Else
                    ' field is active.
                    Dim id As Integer = -1
                    ' The value must always be one of the first two members.
                    Select Case fld.value
                        Case fld.Members(0)
                            id = 0
                        Case fld.Members(1)
                            id = 1
                        Case Else
                            Tracing.TraceLine("adjustRigfield:bad fld.value" & fld.value.ToString, TraceLevel.Error)
                            'p.rv = True
                            ' Just bail out of here.
                            Return
                    End Select
                    rv = fld.value
                    ' If p.k = up or down arrow, use one of the first two values.
                    Select Case p.k
                        Case Keys.Up, Keys.Down
                            id = (id + 1) Mod 2
                            p.rv = True
                            rv = fld.Members(id)
                        Case Else
                            Dim n As Integer = fld.Members.Length - 1
                            id = -1
                            For i As Integer = 2 To n
                                If CType(ChrW(p.k), Char) = fld.Members(i) Then
                                    id = i
                                    rv = fld.Members(i)
                                    p.rv = True
                                    Exit For
                                End If
                            Next
                            If id = -1 Then
                                ' We don't handle this.
                                Return
                            End If
                    End Select
                End If
            End If
            If p.ID = rigField1ID Then
                OpenParms.RigField1.value = rv
            Else
                OpenParms.RigField2.value = rv
            End If
        End If
    End Sub

    ''' <summary>
    ''' Default key handler for the Freqout box.
    ''' Sets e.SuppressKeyPress if handles the key.
    ''' </summary>
    ''' <param name="e">KeyEventArgs</param>
    Private Sub FreqoutKeyHandler(e As KeyEventArgs)
    End Sub

    ' Display the freqOut field.
    ' Also keep track of our position within the frequency value.
    ' If the actual frequency changed, select it for a brief period.
    Private Sub writeFreq()
        Dim str As String = FreqOut.Read(freqID)
        Dim i As Integer
        Dim realFreq As Boolean = (str.IndexOf("."c) >= 0)
        If firstFreqDisplay And realFreq Then
            firstFreqDisplay = False
            Dim fp = initialFreqPos
            ' Set the cursor
            i = str.Length - 1
            While fp < 0
                i -= 1
                If str(i) = "."c Then
                    i -= 1
                End If
                fp += 1
            End While
            FreqOut.SelectionStart = i + FreqOut.Position(freqID)
        End If
        FreqOut.Display()
    End Sub

    Private currentFields() As MainBox.Field
    ' Update the freqOut display.
    ' Write it to the screen if anything actually changed.
    Private Sub showFrequency()
        Tracing.TraceLine("showFrequency", TraceLevel.Verbose)
        If RigControl.Power Then
            If RigControl.MemoryMode And (Not usingMem) Then
                ' Now using a memory.
                currentFields = memFreqFields
                FreqOut.Populate(memFreqFields)
                usingMem = True
            ElseIf (Not RigControl.MemoryMode) And usingMem Then
                ' Now it's a vfo.
                currentFields = vfoFreqFields
                FreqOut.Populate(vfoFreqFields)
                usingMem = False
            End If
            ' Otherwise there was no change.
            For id As Integer = 0 To currentFields.Length - 1
                ' This sets parm.FromRig.
                Dim parm = New fieldFuncParm(id)
                FreqOut.Function(id, parm)
            Next
        End If
        If FreqOut.Changed Then
            writeFreq()
        End If
    End Sub
    Private Function setRIT(ByVal rit As AllRadios.RITData, ByVal xit As Boolean) As String
        Dim rv As String
        If rit.Active Then
            If rit.Value < 0 Then
                rv = "-"
            Else
                rv = "+"
            End If
            rv &= Abs(rit.Value).ToString("d4")
        Else
            ' not active
            If xit Then
                rv = " xxxx"
            Else
                rv = " rrrr"
            End If
        End If
        Return rv
    End Function
    Private Function vfoLetter(ByVal v As RigCaps.VFOs) As String
        Dim rv As String
        If RigControl.Ismemorymode(v) Then
            rv = "M"
        Else
            Select Case v
                Case RigCaps.VFOs.VFOA
                    rv = "A"
                Case RigCaps.VFOs.VFOB
                    rv = "B"
                Case Else
                    rv = CStr(v)
            End Select
        End If
        Return rv
    End Function

    Private Function formatMeter(val As Integer) As String
        If (RigControl Is Nothing) Then
            Return " "
        End If
        ' If not transmitting, may be S9 plus.
        Dim rv As String
        If RigControl.Transmit Then
            ' Note that we read the swr directly.
            If (W2WattMeter IsNot Nothing) AndAlso W2WattMeter.ShowSWR Then
                Dim rv2 As String = W2WattMeter.SWR()
                If rv2.Length > meterSize Then
                    rv = rv2.Substring(0, meterSize)
                Else
                    rv = rv2
                End If
            Else
                rv = val.ToString()
            End If
        Else
            If val > 9 Then
                rv = "+" & (val - 9).ToString()
            Else
                rv = val.ToString()
            End If
        End If
        Return rv
    End Function

    Private Function formatOffset(offset As AllRadios.OffsetDirections) As Char
        Static outChars As Char() = {" ", "+", "-", "e"}
        Dim rv As Char
        Try
            If (RigControl.Mode IsNot Nothing) AndAlso (RigControl.Mode.ToString = "fm") Then
                rv = outChars(offset)
            Else
                rv = " "
            End If
        Catch ex As Exception
            Tracing.TraceLine("formatOffset error:" & offset, TraceLevel.Error)
            rv = " "
        End Try
        Return rv
    End Function

    Private oldSWR As String = ""
    Private powerWasOn As Boolean = False
    Friend Sub UpdateStatus()
        Tracing.TraceLine("updateStatus", TraceLevel.Verbose)
        If Ending Then
            Return
        End If

        Try
            ' don't assume power on initially.
            If RigControl.Power Then
                If Not powerWasOn Then
                    ' the power was off.
                    powerNowOn()
                End If
                showFrequency()

                Tracing.TraceLine("UpdateStatus:doing combos", TraceLevel.Verbose)
                For Each c As Combo In combos
                    If c.Enabled Then
                        c.UpdateDisplay()
                    End If
                Next

                ' Update the rig-dependent fields.
                If (RigControl.RigFields IsNot Nothing) AndAlso RigControl.RigFields.RigControl.Enabled Then
                    Tracing.TraceLine("UpdateStatus:doing RigFields", TraceLevel.Verbose)
                    RigControl.RigFields.RigUpdate()
                End If

                ' See if Flex is tuning.
                If (OpenParms.GetSWRText IsNot Nothing) AndAlso
                   CurrentRig.FlexLike And RigControl.FlexTunerOn And
                   (RigControl.FlexTunerType = AllRadios.FlexTunerTypes.manual) Then
                    Dim SWRtext = OpenParms.GetSWRText()
                    If SWRtext <> oldSWR Then
                        oldSWR = SWRtext
                        setButtonText(AntennaTuneButton, oldSWR)
                    End If
                End If

                ' See if any data to display.
                Tracing.TraceLine("UpdateStatus:doing receive data", TraceLevel.Verbose)
                If RigControl.CanReceiveData Then
                    WriteText(WindowIDs.ReceiveDataOut, RigControl.DataReceived, False)
                End If

                powerWasOn = True
            Else
                ' power is off
                If powerWasOn Then
                    powerNowOff()
                End If
                powerWasOn = False
            End If

            ' Update the status.
            If StatusBox.Changed Then
                StatusBox.Display()
            End If
        Catch ex As Exception
            Tracing.ErrMessageTrace(ex, True)
        End Try
        Tracing.TraceLine("updateStatus:done", TraceLevel.Verbose)
    End Sub

    ' enable or disable a control.
    Private Delegate Sub enableDisableBoxDel(box As Control, value As Boolean)
    Private Shared enableDisableFunc As enableDisableBoxDel =
        Sub(box As Control, value As Boolean)
            box.Enabled = value
        End Sub
    Private Sub enableDisableBox(box As Control, value As Boolean)
        If box.InvokeRequired Then
            box.Invoke(enableDisableFunc, New Object() {box, value})
        Else
            enableDisableFunc(box, value)
        End If
    End Sub
    Private Sub enableDisableWindowControls(value As Boolean)
        For Each c As Control In enableDisableControls
            enableDisableBox(c, value)
        Next
    End Sub

    Private Sub knobOnOffHandler(sender As Object, e As FlexKnob.KnobOnOffArgs)
        Dim onOffText As String
        If e.Status Then
            onOffText = OnWord
        Else
            onOffText = OffWord
        End If
        StatusBox.Write(statKnobID, onOffText)
    End Sub

    Private Sub powerNowOn()
        Tracing.TraceLine("Form1 powerNowOn", TraceLevel.Error)
        Tracing.TraceLine("rig caps:" & RigControl.myCaps.ToString(), TraceLevel.Info)
        invokeConfigVariableControls()
        enableDisableWindowControls(True)
        AddHandler FlexKnob.KnobOnOffEvent, AddressOf knobOnOffHandler
        SetupKnob()
        StatusBox.Write(statPowerID, OnWord)
    End Sub

    Private Sub powerNowOff()
        Tracing.TraceLine("Form1 powerNowOff", TraceLevel.Info)
        If Not Ending Then
            clearMainWindow()
            enableDisableWindowControls(False)
            StatusBox.Write(statPowerID, OffWord)
        End If
    End Sub

    Private Sub FileExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FileExitToolStripMenuItem.Click
        Tracing.TraceLine("Form1 FileExitToolstripMenuItem", TraceLevel.Info)
        Ending = True
        ' Ask user to write an entry if necessary.
        ' If false is returned, they want to cancel the exit.
        ' Note that if no write is necessary, we'll exit w/o asking.
        If Not LogEntry.optionalWrite Then
            Return
        End If
        Try
            LogEntry.Close()
            Logs.Done()
            If LookupStation IsNot Nothing Then
                LookupStation.Finished()
            End If
            remoteLan = False
            If Commands IsNot Nothing Then
                Commands.ClusterShutdown()
            End If
            CloseTheRadio()
            If W2WattMeter IsNot Nothing Then
                W2WattMeter.Dispose()
            End If
            'If UsingSimulator Then
            'simClose()
            'Else
            RadioPort.Close()
            'End If
            setScreenSaver(onExitScreenSaver)
            Tracing.TraceLine("exit:screen saver set:" & onExitScreenSaver.ToString, TraceLevel.Info)
        Catch ex As Exception
            Tracing.TraceLine("Form1 FileExitToolstripMenuItem:" & ex.Message, TraceLevel.Error)
        End Try
        Try
            If buttonTextThread.IsAlive Then
                buttonTextThread.Abort()
            End If
        Catch ex As Exception
            ' nothing to do, thread is dead.
        End Try
        Tracing.TraceLine("End.")
        Tracing.On = False
        Me.Dispose()
        End
    End Sub

    Private Sub W2ConfigToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles W2ConfigToolStripMenuItem.Click
        ConfigW2()
    End Sub

    Private Sub ListOperatorsMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListOperatorsMenuItem.Click
        ' Cleanup any outstanding log activity beforehand.
        'If Not ContactLog.Cleanup() Then
        'Return
        'End If
        Lister.TheList = Operators
        Lister.ShowDialog()
    End Sub
    ' Handles Operators.ConfigEvent
    Private Sub operatorChanged(ByVal sender As Object, ByVal e As ConfigArgs)
        If CurrentOp IsNot Nothing Then
            While (ContactLog IsNot Nothing) AndAlso (Not ContactLog.Cleanup)
            End While
            ConfigContactLog()
        End If
    End Sub

    Private Sub ListRigsMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListRigsMenuItem.Click
        Lister.TheList = Rigs
        Lister.ShowDialog()
    End Sub

    Private Delegate Sub cvcdel()
    Private Sub invokeConfigVariableControls()
        Dim cvcRtn As cvcdel = AddressOf configVariableControls
        If TransmitButton.InvokeRequired Then
            Me.Invoke(cvcRtn)
        Else cvcRtn()
        End If
    End Sub
    Private Sub rigCapsChanged(arg As AllRadios.CapsChangeArg)
        Tracing.TraceLine("rigCapsChanged:" + arg.NewCaps.ToString(), TraceLevel.Info)
        invokeConfigVariableControls()
    End Sub

    Private Function doCommand(ByVal e As System.Windows.Forms.KeyEventArgs) As Boolean
        Dim rv As Boolean = Commands.DoCommand(e.KeyData)
        Return rv
    End Function

    Public Sub DisplayHelp()
        DisplayHelp(ShowHelpTypes.standard)
    End Sub

    Public Sub DisplayHelp(helpType As ShowHelpTypes)
        Dim helper = New ShowHelp(helpType)
        helper.ShowDialog()
        helper.Dispose()
    End Sub

    Private Sub oneSecond_tick(ByVal s As System.Object, ByVal e As EventArgs) Handles OneSecond.Tick
        Tracing.TraceLine("Form1  oneSecond_tick", TraceLevel.Info)
        If KeepCheckingOpen Then
            KeepCheckingOpen = False
            OneSecond.Stop()
            RadioPort.Interactive = False ' don't prompt upon failure
            If openTheRadio() AndAlso RadioPort.IsOpen Then
                ' the port opened.
                RadioPort.Interactive = True ' next time, prompt if doesn't open.
            Else
                ' Keep trying to open the port.
                KeepCheckingOpen = True
                OneSecond.Start()
            End If
        End If
    End Sub

    Private Sub clearMainWindow()
        FreqOut.Clear()
        For Each o As Object In Me.Controls
            Select Case o.GetType.Name
                Case "TextBox"
                    Dim t As TextBox = o
                    If t.InvokeRequired Then
                        t.Invoke(Sub() t.Text = "")
                    Else
                        t.Text = ""
                    End If
                Case "ListBox"
                    Dim l As ListBox = o
                    If l.InvokeRequired Then
                        l.Invoke(Sub() l.SelectedIndex = -1)
                    Else
                        l.SelectedIndex = -1
                    End If
                Case "ComboBox"
                    Dim cb As ComboBox = o
                    If cb.InvokeRequired Then
                        cb.Invoke(Sub()
                                      cb.Text = ""
                                      cb.SelectedIndex = -1
                                  End Sub)
                    Else
                        cb.Text = ""
                        cb.SelectedIndex = -1
                    End If
            End Select
        Next
    End Sub

    Friend Sub doCommand_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ModeControl.BoxKeydown, AntennaTuneButton.KeyDown, TXTuneControl.BoxKeydown, ReceivedTextBox.KeyDown, TransmitButton.KeyDown
        e.SuppressKeyPress = doCommand(e)
        Tracing.TraceLine("doCommand_KeyDown: DoCommand returned " & e.SuppressKeyPress.ToString, TraceLevel.Info)
    End Sub

    Private Sub ToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LogOpenMenuItem.Click
        ' set log file characteristics.
        Commands.getLogFileName()
    End Sub

    Delegate Sub setText(ByVal tb As TextBox, ByVal s As String,
                         ByVal cursor As Integer, ByVal cl As Boolean)
    Private Sub toTextbox(ByVal tb As TextBox, ByVal s As String,
                          ByVal cursor As Integer, ByVal cl As Boolean)
        Dim txt As String
        If cl Then
            txt = s
        Else
            txt = tb.Text & s
        End If
        ' Preserve cursor position if passed cursor is -1.
        If cursor = -1 Then
            cursor = tb.SelectionStart
        ElseIf cursor < 0 Then
            Dim maxLen As Integer = -cursor
            If txt.Length > maxLen Then
                txt = txt.Substring(txt.Length - maxLen)
            End If
            cursor = txt.Length
        ElseIf cursor = 0 Then
            ' Set cursor to end of text.
            cursor = txt.Length
        End If
        tb.Text = txt
        tb.SelectionStart = cursor
        tb.ScrollToCaret()
    End Sub
    ''' <summary>
    ''' Write text to a main window textBox.
    ''' </summary>
    ''' <param name="tbid"></param>
    ''' <param name="s"></param>
    ''' <param name="cl"></param>
    ''' <remarks></remarks>
    Private Sub iWriteText(ByVal tbid As WindowIDs, ByVal s As String, ByVal cl As Boolean)
        iWriteTextX(tbid, s, -1, cl)
    End Sub
    Private Sub iWriteTextX(ByVal tbid As WindowIDs, ByVal s As String,
                           ByVal cursor As Integer, ByVal cl As Boolean)
        If Not Ending Then
            Dim tb As TextBox = TBIDToTB(tbid)
            Try
                If tb.InvokeRequired Then
                    Dim rtn As New setText(AddressOf toTextbox)
                    tb.Parent.Invoke(rtn, New Object() {tb, s, cursor, cl})
                Else
                    toTextbox(tb, s, cursor, cl)
                End If
            Catch ex As Exception
                Tracing.ErrMessageTrace(ex)
            End Try
        End If
    End Sub

    Private Sub ImportMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ImportMenuItem.Click
        ImportForm.ShowDialog()
    End Sub

    Private Sub ExportMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExportMenuItem.Click
        ExportForm.ShowDialog()
    End Sub

    Private Sub ScreenSaverMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ScreenSaverMenuItem.Click
        ' Toggle the screen saver.
        'onExitScreenSaver = Not ScreenSaver.GetScreenSaverActive()
        'ScreenSaver.SetScreenSaverActive(onExitScreenSaver)
        ScreenSaver.SetScreenSaverActive(Not ScreenSaver.GetScreenSaverActive())
    End Sub
    ''' <summary>
    ''' set the screen saver on or off
    ''' </summary>
    ''' <param name="val">true to set it on</param>
    ''' <returns>original value</returns>
    ''' <remarks></remarks>
    Private Function setScreenSaver(ByVal val As Boolean) As Boolean
        Dim orig As Boolean = ScreenSaver.GetScreenSaverActive
        ScreenSaver.SetScreenSaverActive(val)
        Return orig
    End Function

    Private Sub ChangeKeysMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChangeKeysMenuItem.Click
        DefineCommands.ShowDialog()
    End Sub

    Private Sub RestoreKeyMappingMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RestoreKeyMappingMenuItem.Click
        Commands.keyTableToDefault(True)
    End Sub

    Private Sub HelpPageItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HelpPageItem.Click
        Dim fn As String =
            ProgramDirectory & "\JJRadioReadme.htm"
        System.Diagnostics.Process.Start(fn)
    End Sub

    Private Sub HelpKeysItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HelpKeysItem.Click
        DisplayHelp(ShowHelpTypes.standard)
    End Sub

    Private Sub HelpKeysalphabetic_Click(sender As Object, e As EventArgs) Handles HelpKeysalphabetic.Click
        DisplayHelp(ShowHelpTypes.alphabetic)
    End Sub

    Private Sub HelpKeysByFunction_Click(sender As Object, e As EventArgs) Handles HelpKeysByFunction.Click
        DisplayHelp(ShowHelpTypes.byGroup)
    End Sub

    Private Sub HelpAboutItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HelpAboutItem.Click
        AboutProgram.ShowDialog()
    End Sub

    ''' <summary>
    ''' ComboBoxes - used to call methods.
    ''' </summary>
    Private combos As List(Of Combo)
    ''' <summary>
    ''' Controls to be enabled/disabled according to the power status.
    ''' </summary>
    Private enableDisableControls As List(Of Control)

    ' FreqOut fields
    Const rigField1ID As Integer = 0
    Const rigField2ID As Integer = rigField1ID + 1
    Const rigField3ID As Integer = rigField2ID + 1
    Const rigField4ID As Integer = rigField3ID + 1
    Const meterID As Integer = rigField4ID + 1
    Const meterSize As Integer = 4 ' field size
    Const splitID As Integer = meterID + 1
    Const voxID As Integer = splitID + 1
    Const vfoID As Integer = voxID + 1
    Const vfoFreqID As Integer = vfoID + 1
    Const vfoMemNoId As Integer = -1
    Const memMemNoID As Integer = vfoID + 1
    Const memFreqID As Integer = memMemNoID + 1
    Private usingMem As Boolean
    Private ReadOnly Property memNoID As Integer
        Get
            If usingMem Then
                Return memMemNoID
            Else
                Return vfoMemNoId
            End If
        End Get
    End Property
    Private ReadOnly Property freqID As Integer
        Get
            If usingMem Then
                Return memFreqID
            Else
                Return vfoFreqID
            End If
        End Get
    End Property
    Private ReadOnly Property offsetID As Integer
        Get
            Return freqID + 1
        End Get
    End Property
    Private ReadOnly Property ritID As Integer
        Get
            Return freqID + 2
        End Get
    End Property
    Private ReadOnly Property xitID As Integer
        Get
            Return freqID + 3
        End Get
    End Property
    Private vfoFreqFields() As MainBox.Field =
        {New MainBox.Field(1, "", "", AddressOf adjustRigField),
         New MainBox.Field(1, "", "", AddressOf adjustRigField),
         New MainBox.Field(1, "", "", Nothing),
         New MainBox.Field(1, "", "", Nothing),
         New MainBox.Field(meterSize, "", "", AddressOf adjustSMeter),
         New MainBox.Field(1, "", "", AddressOf adjustSplit),
         New MainBox.Field(1, "", "", AddressOf adjustVox),
         New MainBox.Field(1, "", "", AddressOf adjustVFO),
         New MainBox.Field(12, "", "", AddressOf AdjustFreq),
         New MainBox.Field(1, "", "", AddressOf adjustOffset),
         New MainBox.Field(5, "", "", AddressOf adjustRit),
         New MainBox.Field(5, " ", "", AddressOf adjustXit)}
    Private memFreqFields() As MainBox.Field =
        {New MainBox.Field(1, "", "", AddressOf adjustRigField),
         New MainBox.Field(1, "", "", AddressOf adjustRigField),
         New MainBox.Field(1, "", "", Nothing),
         New MainBox.Field(1, "", "", Nothing),
         New MainBox.Field(4, "", "", AddressOf adjustSMeter),
         New MainBox.Field(1, "", "", AddressOf adjustSplit),
         New MainBox.Field(1, "", "", AddressOf adjustVox),
         New MainBox.Field(1, "", "", AddressOf adjustVFO),
         New MainBox.Field(3, "", "", AddressOf adjustMem),
         New MainBox.Field(12, "", "", AddressOf AdjustFreq),
         New MainBox.Field(1, "", "", AddressOf adjustOffset),
         New MainBox.Field(5, " ", "", AddressOf adjustRit),
         New MainBox.Field(5, " ", "", AddressOf adjustXit)}
    ' Passed to field functions.
    Class fieldFuncParm
        Public ID As Integer
        Public FromRig As Boolean = False ' true if from rig, not keyboard
        Public k As Keys
        Public rv As Boolean
        Public Sub New(i As Integer, ByVal key As Keys, ByVal ret As Boolean)
            ID = i
            k = key
            rv = ret
        End Sub
        Public Sub New(i As Integer)
            ID = i
            FromRig = True
        End Sub
    End Class

    ' Return the id of the MainBox field we're in, or -1 if not.
    Private Function mainBoxField(fld As MainBox) As Integer
        Dim rv As Integer = -1
        Dim pos As Integer = fld.SelectionStart
        For i As Integer = 0 To fld.NumberOfFields - 1
            If (pos >= fld.Position(i)) And (pos < fld.Position(i) + fld.Length(i)) Then
                rv = i
                Exit For
            End If
        Next
        Return rv
    End Function

    Friend Class modeElement
        Private val As AllRadios.ModeValue
        Public ReadOnly Property Display As String
            Get
                Return val.ToString
            End Get
        End Property
        Public ReadOnly Property RigItem As AllRadios.ModeValue
            Get
                Return val
            End Get
        End Property
        Public Sub New(ByVal v As AllRadios.ModeValue)
            val = v
        End Sub
    End Class
    Friend modeList As ArrayList

    Friend Class TrueFalseElement
        Private val As Boolean
        Public ReadOnly Property Display
            Get
                Return val.ToString
            End Get
        End Property
        Public ReadOnly Property RigItem
            Get
                Return val
            End Get
        End Property
        Public Sub New(ByVal a As Boolean)
            val = a
        End Sub
    End Class
    Private TXTuneList As ArrayList

    ' Callback for peak level.
    Private Function getSMeter() As Integer
        Dim rv As Integer = 0
        If (RigControl IsNot Nothing) AndAlso RigControl.Power Then
            If RigControl.Transmit Then
                If (W2WattMeter IsNot Nothing) AndAlso W2WattMeter.IsUseable Then
                    ' Don't read meter here if showing SWR.
                    If Not W2WattMeter.ShowSWR Then
                        rv = CType(W2WattMeter.ForwardPower, Integer)
                    End If
                Else
                    rv = CType(RigControl.SMeter, Integer)
                End If
            Else
                rv = CType(RigControl.SMeter, Integer)
            End If
        End If
        Return rv
    End Function

    ' Called when Transmit changes.
    Private Sub transmitChangeProc(sender As Object, transmit As Boolean)
        SMeter.ResetPeak()
    End Sub

    Friend Sub setupBoxes()
        Tracing.TraceLine("setupBoxes", TraceLevel.Info)
        If RigControl Is Nothing Then
            Tracing.TraceLine("SetupBoxes:no rig", TraceLevel.Error)
            Return
        End If
        clearMainWindow()
        ' set the main frequency box fields.
        FreqOut.Populate(vfoFreqFields)

        ' These levels' values are shown as the peak over a time period.
        'SMeter = New Levels(Function() RigControl.SMeter)
        SMeter = New Levels(AddressOf getSMeter)
        'SMeter.Cycle = 2000
        SMeter.Peak = True

        combos = New List(Of Combo)
        enableDisableControls = New List(Of Control)

        ModeControl.Visible = True
        If RigControl.ModeTable IsNot Nothing Then
            ModeControl.Clear()
            'ModeControl.Enabled = True
            'ModeControl.Visible = True
            modeList = New ArrayList
            For Each Val As AllRadios.ModeValue In RigControl.ModeTable
                If Val.ToString <> "none" Then
                    modeList.Add(New modeElement(Val))
                End If
            Next
            ModeControl.TheList = modeList
            ModeControl.UpdateDisplayFunction = Function() RigControl.Mode
            ModeControl.UpdateRigFunction =
                Sub(v As AllRadios.ModeValue) RigControl.Mode = v
            combos.Add(ModeControl)
            enableDisableControls.Add(ModeControl)
        Else
            'ModeControl.Enabled = False
            'ModeControl.Visible = False
        End If

        ' Add other controls to the enable/disable collection.
        enableDisableControls.Add(SentTextBox)

        ' Add any config variable controls.
        configVariableControls()
    End Sub

    Private Sub configVariableControls()
        enableDisableControls.Remove(TransmitButton)
        'TransmitButton.Enabled = False
        TransmitButton.Visible = True
        If RigControl.myCaps.HasCap(RigCaps.Caps.ManualTransmit) Then
            'TransmitButton.Enabled = True
            'TransmitButton.Visible = True
            enableDisableControls.Add(TransmitButton)
        End If

        setButtonText(AntennaTuneButton, antennaTuneButtonText)
        'TXTuneControl.Enabled = False
        TXTuneControl.Visible = True
        'AntennaTuneButton.Enabled = False
        AntennaTuneButton.Visible = True
        combos.Remove(TXTuneControl)
        enableDisableControls.Remove(TXTuneControl)
        enableDisableControls.Remove(AntennaTuneButton)
        If RigControl.myCaps.HasCap(RigCaps.Caps.ATSet) Then
            ' auto tuner
            TXTuneList = New ArrayList
            TXTuneList.Add(New TrueFalseElement(False))
            TXTuneList.Add(New TrueFalseElement(True))
            TXTuneControl.TheList = TXTuneList
            TXTuneControl.UpdateDisplayFunction =
                Function()
                    If CurrentRig.FlexLike Then
                        Return (RigControl.FlexTunerType = AllRadios.FlexTunerTypes.auto)
                    Else
                        Return (RigControl.AntennaTuner <> 0)
                    End If
                End Function
            TXTuneControl.UpdateRigFunction =
                Sub(v As Boolean)
                    If CurrentRig.FlexLike Then
                        Dim val = AllRadios.FlexTunerTypes.manual
                        If (v And RigControl.myCaps.HasCap(RigCaps.Caps.ATGet)) Then
                            val = AllRadios.FlexTunerTypes.auto
                        End If
                        RigControl.FlexTunerType = val
                    Else
                        ' not a Flex
                        If v Then
                            RigControl.AntennaTuner = RigControl.AntennaTuner Or AllRadios.AntTunerVals.tx
                        Else
                            RigControl.AntennaTuner = RigControl.AntennaTuner And AllRadios.AntTunerVals.rx
                        End If
                    End If
                End Sub
            combos.Add(TXTuneControl)
            enableDisableControls.Add(TXTuneControl)
            TXTuneControl.Clear()
            'TXTuneControl.Enabled = True
            'TXTuneControl.Visible = True

            'AntennaTuneButton.Enabled = True
            'AntennaTuneButton.Visible = True
            enableDisableControls.Add(AntennaTuneButton)
        ElseIf RigControl.myCaps.HasCap(RigCaps.Caps.ManualATSet) Then
            'AntennaTuneButton.Enabled = True
            'AntennaTuneButton.Visible = True
            enableDisableControls.Add(AntennaTuneButton)
        End If
    End Sub

    Private Sub mainBox_Keydown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles FreqOut.BoxKeydown, StatusBox.BoxKeydown
        Dim box As MainBox = CType(sender, MainBox)
        Dim fldID As Integer
        fldID = mainBoxField(box)
        If (fldID <> -1) And
            Not (e.Alt Or e.Control) Then
            ' execute the field's function if there is one.
            ' Note that box.Function returns true if there's a function.
            ' The function sets parm.rv if it processed the key.
            Dim parm As New fieldFuncParm(fldID, e.KeyData, False)
            box.Function(fldID, parm)
            e.SuppressKeyPress = parm.rv
        End If
        ' if not already handled...
        If Not e.SuppressKeyPress And (box.Name = "FreqOut") Then
            FreqoutKeyHandler(e)
        End If
        If Not e.SuppressKeyPress Then
            e.SuppressKeyPress = doCommand(e)
        End If
    End Sub

    Private Sub CompleteEvent_Handler(ByVal sender As Object, ByVal e As AllRadios.CompleteEventArgs)
        Select Case e.TheEvent
            Case AllRadios.CompleteEvents.memoriesStart
                StatusBox.Write(statMemoryID, Loading)
                MemoriesLoaded = False
            Case AllRadios.CompleteEvents.memories
                StatusBox.Write(statMemoryID, Loaded)
                MemoriesLoaded = True
#If LeaveBootTraceOn = 0 Then
                If BootTrace Then
                    Tracing.TraceLine("Boot tracing off")
                    Tracing.On = False
                    BootTrace = False
                End If
#End If
            Case AllRadios.CompleteEvents.menus
                'WriteText(WindowIDs.StatusOut, msgMenusLoaded, True)
                MenusLoaded = True
        End Select
    End Sub

    Private Sub TransmitButton_Click(sender As System.Object, e As System.EventArgs) Handles TransmitButton.Click
        Tracing.TraceLine("TransmitButton_Click", TraceLevel.Info)
        toggleTransmit()
    End Sub
    Private Sub toggleTransmit()
        Tracing.TraceLine("toggling transmit:" & RigControl.Transmit.ToString, TraceLevel.Info)
        RigControl.Transmit = Not RigControl.Transmit
    End Sub

    Private buttonTextQ As Queue
    Private Class buttonTextQElement
        Public Item As Button
        Public Text As String
        Public Sub New(i As Button, t As String)
            Item = i
            Text = t
        End Sub
    End Class
    Private buttonTextThread As Thread
    Private Sub buttonTextThreadProc()
        Try
            While Not Ending
                While buttonTextQ.Count > 0
                    Dim el As buttonTextQElement = buttonTextQ.Dequeue()
                    setButtonText(el.Item, el.Text)
                End While
                Thread.Sleep(pollTimerInterval)
            End While
        Catch ex As ThreadAbortException
            Tracing.TraceLine("buttonTextThreadProc aborted", TraceLevel.Error)
        End Try
    End Sub

    Delegate Sub SetButtonTextDel()
    Private Sub setButtonText(b As Button, text As String)
        Dim setText As SetButtonTextDel = Sub()
                                              b.Text = text
                                              b.AccessibleName = text
                                          End Sub
        If b.InvokeRequired Then
            b.Invoke(setText)
        Else
            setText()
        End If
    End Sub

    Dim oldQText As String = ""
    Private Sub qButtonText(b As Button, text As String)
        If oldQText = text Then
            Return
        End If
        oldQText = text
        buttonTextQ.Enqueue(New buttonTextQElement(b, text))
    End Sub

    ' Antenna tuner simplification properties
    Private ReadOnly Property tunerRX As Boolean
        Get
            Return ((RigControl.AntennaTuner And AllRadios.AntTunerVals.rx) <> 0)
        End Get
    End Property
    Private ReadOnly Property tunerTX As Boolean
        Get
            Return ((RigControl.AntennaTuner And AllRadios.AntTunerVals.tx) <> 0)
        End Get
    End Property
    Private ReadOnly Property tunerTune As Boolean
        Get
            Return ((RigControl.AntennaTuner And AllRadios.AntTunerVals.tune) <> 0)
        End Get
    End Property

    Private Sub AntennaTuneButton_Enter(sender As Object, e As EventArgs) Handles AntennaTuneButton.Enter
        setButtonText(AntennaTuneButton, antennaTuneButtonText)
        If CurrentRig.FlexLike Then
            AddHandler RigControl.FlexAntTunerStartStop, AddressOf FlexAntTuneStartStopHandler
        End If
    End Sub

    Private Sub AntennaTuneButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AntennaTuneButton.Click
        oldSWR = ""
        ' If a Flex, just toggle the tuner on/off.
        If CurrentRig.FlexLike Then
            RigControl.FlexTunerOn = Not RigControl.FlexTunerOn
        Else
            ' Note that if the rig has a manual tuner, and we're not using its autotuner, this is a toggle,
            ' otherwise we just turn tuning on.
            Dim value As AllRadios.AntTunerVals = AllRadios.AntTunerVals.tune
            If RigControl.myCaps.HasCap(RigCaps.Caps.ManualATGet) And
           (Not tunerTX) And tunerTune Then
                value = 0 ' turn off manual tuner.
            End If
            RigControl.AntennaTuner = value
        End If
    End Sub

    Private Sub AntennaTuneButton_Leave(sender As Object, e As EventArgs) Handles AntennaTuneButton.Leave
        setButtonText(AntennaTuneButton, antennaTuneButtonText)
        If CurrentRig.FlexLike Then
            RemoveHandler RigControl.FlexAntTunerStartStop, AddressOf FlexAntTuneStartStopHandler
        End If
    End Sub

    Private Sub FlexAntTuneStartStopHandler(e As AllRadios.FlexAntTunerArg)
        If RigControl.FlexTunerType = AllRadios.FlexTunerTypes.manual Then
            setButtonText(AntennaTuneButton, e.SWR)
        Else
            setButtonText(AntennaTuneButton, e.Status)
        End If
    End Sub

    ' Public iSize As SizeF
    Private Sub Form1_Layout(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LayoutEventArgs) Handles MyBase.Layout
#If 0 Then
        If e.AffectedControl.Name = "Form1" Then
            Select Case e.AffectedProperty
                Case "Visible"
                    iSize = New SizeF(Width, Height)
                Case "Bounds"
                    Dim sz As Size = e.AffectedControl.Size
                    Dim sc = New SizeF(sz.Width / iSize.Width, sz.Height / iSize.Height)
                    Scale(sc)
            End Select
        End If
#End If
    End Sub

    Private Sub Form1_Activated(sender As System.Object, e As System.EventArgs) Handles MyBase.Activated
        BringToFront()
    End Sub

    Private Sub CWMessageUpdateMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles CWMessageUpdateMenuItem.Click
        CWMessageUpdate.ShowDialog()
    End Sub

    Private Function isFunction(e As KeyEventArgs) As Boolean
        Dim rv As Boolean
        rv = (e.Control Or e.Alt Or
              ((e.KeyCode >= Keys.F1) And (e.KeyCode <= Keys.F24)))
        Return rv
    End Function
    Private Sub SentTextBox_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles SentTextBox.KeyDown
        If isFunction(e) Then
            ' not just a character to send.
            ' check for clipboard functions
            If e.Control Then
                Select Case e.KeyCode
                    Case Keys.C
                        ' copy to clipboard
                        Dim tb As TextBox = sender
                        Try
                            Clipboard.SetText(tb.Text.Substring(tb.SelectionStart, tb.SelectionLength))
                        Catch ex As Exception
                            Tracing.TraceLine("SentTextBox_KeyDown exception:" & ex.Message, TraceLevel.Error)
                        End Try
                        e.SuppressKeyPress = True
                    Case Keys.V
                        ' paste from clipboard
                        Dim str As String
                        str = Clipboard.GetText
                        RigControl.SendCW(str)
                        WriteTextX(WindowIDs.SendDataOut, str, 0, False)
                        e.SuppressKeyPress = True
                    Case Else
                        e.SuppressKeyPress = doCommand(e)
                End Select
            Else
                ' See if it's some other command.
                e.SuppressKeyPress = doCommand(e)
            End If
        Else
            ' send this character in the keyPress routine.
        End If
    End Sub

    Private cwBuf As String
    Private Sub SentTextBox_KeyPress(sender As System.Object, e As System.Windows.Forms.KeyPressEventArgs) Handles SentTextBox.KeyPress
        Tracing.TraceLine("SentTextBox_KeyPress:" & AscW(e.KeyChar), TraceLevel.Info)
        'If OpenParms.DirectSend Then
        'RigControl.SendCW(e.KeyChar)
        'Return
        'End If
        Select Case e.KeyChar
            Case ChrW(Keys.Back)
                If cwBuf.Length > 1 Then
                    cwBuf = cwBuf.Substring(0, cwBuf.Length - 1)
                ElseIf cwBuf.Length = 1 Then
                    cwBuf = ""
                End If
            Case Else
                cwBuf &= e.KeyChar
        End Select
        If Char.IsWhiteSpace(e.KeyChar) Then
            RigControl.SendCW(cwBuf)
            cwBuf = ""
        End If
    End Sub

    Private Sub PanMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles PanMenuItem.Click
        Commands.startPanning()
    End Sub

    Private Sub ShowBandsMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ShowBandsMenuItem.Click
        ShowBands.ShowDialog()
    End Sub

    Private rBoxPos As Integer
    Private Sub ReceivedTextBox_Enter(sender As Object, e As EventArgs) Handles ReceivedTextBox.Enter
        If ReceivedTextBox.SelectionStart < 0 Then
            rBoxPos = 0
        Else
            rBoxPos = ReceivedTextBox.SelectionStart
        End If
    End Sub

    Friend Sub DisplayDecodedText(text As String)
        Dim disposition As Integer = 0 ' default is to concatinate text.
        ' See if constraining text.
        If CurrentOp.ConstrainedDecode Then
            disposition = -CurrentOp.CWDecodeCells
        End If
        WriteTextX(WindowIDs.ReceiveDataOut, text, disposition, False)
    End Sub

    Friend Sub DontScrollText(text As String)
        Dim tb As TextBox = ReceivedTextBox
        'tb.SuspendLayout()
        tb.SelectionLength = 0
        Dim pos As Integer = tb.SelectionStart
        'Dim line As Integer = tb.GetLineFromCharIndex(pos)
        tb.SelectionStart = tb.Text.Length
        tb.Text = tb.Text & text
        tb.SelectionStart = pos
        tb.ScrollToCaret()
        'tb.ResumeLayout()
    End Sub

#If 0 Then
    Private Sub ReceiveTextBox_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs)
        ' Allow clipboard stuff
        ' ctrl-C copies all text to the clipboard.
        ' ctrl-X cuts all text.
        If e.Control And (ReceivedTextBox.Text.Length > 0) Then
            If e.KeyCode = Keys.C Then
                copyReceivedText()
                e.SuppressKeyPress = True
            ElseIf (e.KeyCode = Keys.X) Then
                copyReceivedText()
                ReceivedTextBox.Text = ""
                e.SuppressKeyPress = True
            Else
                doCommand_KeyDown(sender, e)
            End If
        Else
            doCommand_KeyDown(sender, e)
        End If
    End Sub
    Private Sub copyReceivedText()
        ReceivedTextBox.Enabled = False
        ReceivedTextBox.SelectAll()
        If ReceivedTextBox.SelectedText IsNot Nothing AndAlso _
           ReceivedTextBox.SelectedText.Length > 0 Then
            Clipboard.SetText(ReceivedTextBox.SelectedText)
        End If
        ReceivedTextBox.Enabled = True
        ReceivedTextBox.Focus()
    End Sub
#End If

    Private Sub setupScreenFields()
        ScreenFieldsMenu.DropDownItems.Clear()
        If (RigControl.RigFields IsNot Nothing) AndAlso (RigControl.RigFields.ScreenFields IsNot Nothing) Then
            For Each ctl As Control In RigControl.RigFields.ScreenFields
                If ctl.Enabled Then
                    Dim item = New ToolStripMenuItem
                    item.Tag = ctl
                    item.AccessibleRole = Windows.Forms.AccessibleRole.MenuItem
                    'item.Size = New Size(227, 22)
                    item.AutoSize = True
                    item.Text = CStr(ctl.Tag)
                    AddHandler item.Click, AddressOf ScreenField_Click
                    ScreenFieldsMenu.DropDownItems.Add(item)
                End If
            Next
        End If
    End Sub

    Private Sub ScreenField_Click(sender As Object, e As EventArgs)
        Dim item As Control = sender.tag
        item.Focus()
    End Sub

    Private Sub ScreenFieldsMenu_DropDownOpening(sender As System.Object, e As System.EventArgs) Handles ScreenFieldsMenu.DropDownOpening
        Tracing.TraceLine("ScreenFieldsMenu_DropDownOpening", TraceLevel.Info)
        setupScreenFields()
    End Sub

    Private Function compareNames(m1 As KeyCommands.keyTbl, m2 As KeyCommands.keyTbl)
        ' null items sort last
        Dim x As String = m1.menuText
        Dim y As String = m2.menuText
        If x = vbNullString Then
            If y = vbNullString Then
                Return 0
            Else
                Return 1
            End If
        ElseIf y = vbNullString Then
            Return -1
        End If
        Dim minLen As Integer = Math.Min(x.Length, y.Length)
        Dim xs As String = x.Substring(0, minLen)
        Dim ys As String = y.Substring(0, minLen)
        Dim rv As Integer = xs.CompareTo(ys)
        If rv = 0 Then
            rv = x.Length.CompareTo(y.Length)
        End If
        Return rv
    End Function

    ''' <summary>
    ''' Setup the operations menu
    ''' </summary>
    ''' <remarks>
    ''' This is called from KeyCommands when the command table is initialized from the config data.
    ''' </remarks>
    Friend Sub SetupOperationsMenu()
        If Commands Is Nothing Then
            Return
        End If
        OperationsMenuItem.DropDownItems.Clear()
        Dim sortedTable = New List(Of KeyCommands.keyTbl)
        sortedTable.AddRange(Commands.KeyTable)
        sortedTable.Sort(AddressOf compareNames)
        For Each keyItem As KeyCommands.keyTbl In sortedTable
            If keyItem.menuText = vbNullString Then
                Exit For
            End If
            Dim item = New ToolStripMenuItem
            item.Tag = keyItem
            item.AccessibleRole = Windows.Forms.AccessibleRole.MenuItem
            item.AutoSize = True
            item.Text = keyItem.menuText
            AddHandler item.Click, AddressOf OperationsMenuItem_Click
            OperationsMenuItem.DropDownItems.Add(item)
        Next
    End Sub

    Private Sub OperationsMenuItem_Click(sender As Object, e As EventArgs)
        Dim item As KeyCommands.keyTbl = sender.tag
        Commands.CommandId = item.key.id
        Try
            item.rtn()
        Catch ex As Exception
            If (RigControl Is Nothing) OrElse Not RigControl.Power Then
                Tracing.TraceLine("OperationsMenuItem_Click:no rig setup", TraceLevel.Error)
            Else
                Tracing.TraceLine("OperationsMenuItem_Click:", TraceLevel.Error)
                Tracing.ErrMessageTrace(ex)
            End If
        End Try
    End Sub

    Private traceFile As String = ""
    Private Sub TraceMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles TraceMenuItem.Click
        TraceAdmin.ShowDialog()
    End Sub

    Private Sub Form1_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If Not Ending Then
            FileExitToolStripMenuItem_Click(sender, e)
        End If
    End Sub

    Private Sub gotoHome()
        TextOut.PerformGenericFunction(FreqOut,
            Sub()
                FreqOut.Focus()
            End Sub)
    End Sub

    Private Function currentOperatorName() As String
        Return CurrentOp.UserBasename
    End Function

    Private Sub CWDecodeMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles CWDecodeMenuItem.Click
        CWDecode.ShowDialog()
    End Sub

    Private Sub ClearOptionalMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ClearOptionalMenuItem.Click
        OptionalMessage.Clear()
    End Sub

    Private Sub ScanTimer_Tick(sender As System.Object, e As System.EventArgs) Handles ScanTmr.Tick
        If scanstate = scans.linear Then
            scan.ScanTimer_Tick(sender, e)
        Else
            MemoryScan.ScanTimer_Tick(sender, e)
        End If
    End Sub

    Private Sub LOTWMergeMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles LOTWMergeMenuItem.Click
        Dim lotwForm As Form = New LOTWMerge
        lotwForm.ShowDialog()
        lotwForm.Dispose()
    End Sub

    Private Sub FlexKnobMenuItem_Click(sender As Object, e As EventArgs) Handles FlexKnobMenuItem.Click
        If Knob IsNot Nothing Then
            Knob.Config()
        End If
    End Sub
End Class
