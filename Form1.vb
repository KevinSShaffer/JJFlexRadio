﻿#Const LeaveBootTraceOn = 2
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
    Const notConnected As String = "The radio didn't connect."
    Const nowDisconnected As String = "The radio disconnected"
    Const noRadioSelected As String = "No radio was selected."
    Const antennaTuneButtonBaseText As String = "Ant Tune"
    Const memorizedText As String = "Memorized"
    Private ReadOnly Property antennaTuneButtonText As String
        Get
            Dim rv = antennaTuneButtonBaseText
            If ((RigControl IsNot Nothing) And
                RigControl.FlexTunerUsingMemoryNow) Then
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
    Dim WithEvents thePollTimer As System.Windows.Forms.Timer
    Private Property PollTimer As Boolean
        Get
            If thePollTimer IsNot Nothing Then
                Return thePollTimer.Enabled
            Else
                Return False
            End If
        End Get
        Set(value As Boolean)
            Tracing.TraceLine("PollTimer:" & value.ToString, TraceLevel.Info)
            If value Then
                thePollTimer = New System.Windows.Forms.Timer(components)
                AddHandler thePollTimer.Tick, AddressOf PollTimer_Tick
                thePollTimer.Interval = pollTimerInterval
                thePollTimer.Start()
            Else
                If thePollTimer IsNot Nothing Then
                    thePollTimer.Stop()
                    thePollTimer.Dispose()
                End If
            End If
        End Set
    End Property

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
            MessageBox.Show(ex.Message, ExceptionHdr, MessageBoxButtons.OK)
            appSettings = Nothing
        End Try
        Return appSettings
    End Function
#End If

    ' Status line
    Private statusFields() As MainBox.Field =
    {New MainBox.Field("Power", 3, "pwr:", "  ", Nothing),
     New MainBox.Field("Memories", 5, "memories:", "  ", Nothing),
     New MainBox.Field("Scan", 7, "scan:", "  ", Nothing),
     New MainBox.Field("Knob", 7, "Knob:", "  ", Nothing),
     New MainBox.Field("LogFile", 20, "log:", "", Nothing)
    }
    Private Sub statusSetup()
        StatusBox.Populate(statusFields)
        StatusBox.Clear()
        StatusBox.Write("Scan", OffWord)
    End Sub

    Private WithEvents testMenuItem As System.Windows.Forms.ToolStripMenuItem
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        statusSetup() ' setup the status line.

        ' Create main objects.
        GetConfigInfo()

        ' set the station name.
        StationName = getStationName()
        Tracing.TraceLine("StationName:" & StationName, TraceLevel.Info)

        ' Get program name
        Dim pgmName = StationName
        If ProgramInstance > 1 Then
            pgmName &= ProgramInstance.ToString
        End If
        Me.Text &= " " & pgmName
        Me.Refresh()

#If LeaveBootTraceOn >= 1 Then
        ' debug build, add the Test menu item.
        Me.testMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.testMenuItem.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuItem
        Me.testMenuItem.Name = "testFunction"
        Me.testMenuItem.Size = New System.Drawing.Size(275, 26)
        Me.testMenuItem.Text = "&Test function"
        AddHandler Me.testMenuItem.Click, AddressOf testMenuItem_Click
        Me.ActionsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.testMenuItem})
#End If

        ' Add handlers to handle any future configuration changes.
        AddHandler Operators.ConfigEvent, AddressOf operatorChanged

        WriteText = AddressOf iWriteText
        WriteTextX = AddressOf iWriteTextX

        ProgramDirectory = Directory.GetCurrentDirectory
        Tracing.TraceLine("Form1 load:" & ProgramDirectory, TraceLevel.Info)

        ' Turn off the screen saver.
        onExitScreenSaver = setScreenSaver(False)

        openTheRadio(True) ' initial call

        FreqOut.BringToFront()
    End Sub

    ''' <summary>
    ''' Open the radio.
    ''' This will terminate the program if user elects to abort.
    ''' </summary>
    ''' <returns>True on success</returns>
    Friend Function openTheRadio(initialCall As Boolean) As Boolean
        Try
            Dim rv As Boolean
            OpenParms = New FlexBase.OpenParms()
            OpenParms.ProgramName = ProgramName
            OpenParms.CWTextReceiver = AddressOf Commands.DisplayDecodedText
            ' Frequency formatters
            OpenParms.FormatFreqForRadio = AddressOf UlongFreq
            OpenParms.FormatFreq = AddressOf FormatFreqUlong
            OpenParms.GotoHome = AddressOf gotoHome
            OpenParms.ConfigDirectory = BaseConfigDir & "\Radios"
            OpenParms.AudioDevicesFile = AudioDevicesFile
            OpenParms.GetOperatorName = AddressOf currentOperatorName
            OpenParms.StationName = StationName
            OpenParms.BrailleCells = CurrentOp.BrailleDisplaySize
            OpenParms.License = CurrentOp.License
            OpenParms.Profiles = CurrentOp.Profiles

            ' Note this creates a new rigControl object.
            selectorThread = New Thread(AddressOf selectorProc)
            selectorThread.Name = "selectorThread"
            selectorThread.SetApartmentState(ApartmentState.STA)
            selectorThread.Start(initialCall)
            selectorThread.Join()
            Me.Activate()
            rv = (radioSelected = DialogResult.OK)

            If rv Then
                ' add handlers for RigControl events.
                AddHandler RigControl.PowerStatus, AddressOf powerStatusHandler
                AddHandler RigControl.NoSliceError, AddressOf noSliceErrorHandler

                Tracing.TraceLine("OpenTheRadio:rig is starting", TraceLevel.Info)
                rv = RigControl.Start()
                If Not rv Then
                    radioSelected = DialogResult.Abort
                End If
            End If

            If rv Then

                setupBoxes()
                ' Rig dependent menu items.
                ' disable window controls initially.
                enableDisableWindowControls(False)

                ' Start polling for changes
                PollTimer = True
            Else
                Tracing.TraceLine("OpenTheRadio:rig's open failed", TraceLevel.Error)
                If radioSelected = DialogResult.Abort Then
                    ' radio couldn't start
                    CloseTheRadio()
                ElseIf radioSelected = DialogResult.No Then
                    MessageBox.Show(notConnected, ErrorHdr, MessageBoxButtons.OK)
                Else
                    'MessageBox.Show(noRadioSelected, MessageHdr, MessageBoxButtons.OK)
                    ' No radio was desired.  Perhaps turn off tracing.
#If LeaveBootTraceOn = 0 Then
                turnTracingOff()
#End If
                End If
            End If
            Return rv
        Catch ex As Exception
            Tracing.TraceLine("openTheRadio exception:" & ex.Message & Environment.NewLine & ex.StackTrace, TraceLevel.Error)
            Return False
        End Try
    End Function

    Private radioSelected As DialogResult
    Private Sub selectorProc(o As Object)
        Dim initialCall = CType(o, Boolean)
        ' new rig
        Dim selector As RigSelector = New RigSelector(initialCall, OpenParms, Me)
        Dim theForm As Form = CType(selector, Form)
        RigControl = New FlexBase(OpenParms)
        radioSelected = theForm.ShowDialog()
        If radioSelected <> DialogResult.OK Then
            'enableDisableWindowControls(False)
            RigControl.Dispose()
            RigControl = Nothing
        End If
        theForm.Dispose()
    End Sub

    Friend Sub CloseTheRadio()
        Tracing.TraceLine("CloseTheRadio", TraceLevel.Info)
        clearMainWindow()
        StopKnob()
        PollTimer = False
        If SMeter IsNot Nothing Then
            SMeter.Peak = False
        End If
        If RigControl IsNot Nothing Then
            If RigControl.RigFields IsNot Nothing Then
                If RigControl.RigFields.RigControl IsNot Nothing Then
                    If enableDisableControls IsNot Nothing Then
                        enableDisableControls.Remove(RigControl.RigFields.RigControl)
                    End If
                    Controls.Remove(RigControl.RigFields.RigControl)
                End If
                RemoveHandler RigControl.RigFields.RigControl.KeyDown, AddressOf doCommand_KeyDown
            End If
            ' We need to turn power off explicitly here, not via interrupt.
            Power = False
            RemoveHandler RigControl.PowerStatus, AddressOf powerStatusHandler
            RigControl.Dispose()
            RigControl = Nothing
        End If
    End Sub

    Private Sub PollTimer_Tick(sender As System.Object, e As System.EventArgs)
        UpdateStatus()
    End Sub

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
        If p.fromRig Then
            ' show transmit or virtual receive frequency.
            Dim freq As ULong
            If RigControl.Transmit Then
                freq = RigControl.TXFrequency
            Else
                freq = RXFrequency
            End If
            FreqOut.Write(p.ID, FormatFreq(freq))
        Else
            ' Supported keys: up and down arrow, D, U, A - D, =, space, S, T, V, X, K, minus, plus, and digits 0-9.
            ' You may not change the frequency here if transmitting.
            If (p.k <> xmitKey) And RigControl.Transmit Then
                Tracing.TraceLine("AdjustFreq:can't change while transmitting", TraceLevel.Error)
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
                        ' display and send to radio.  Use virtual receive frequency.
                        RXFrequency = longFreq
                    End If
                Case Keys.K
                    ' Round to nearest khz.
                    Dim freq As Double = CType(RigControl.Frequency, Double)
                    freq = 1000 * Math.Round(freq / 1000)
                    RXFrequency = CType(freq, ULong)
                Case Keys.S, Keys.T
                    ' Treat "s" and "t" as they are with split.
                    adjustSplit(p)
                Case Keys.V
                    ' vox on/off
                    RigControl.Vox = RigControl.ToggleOffOn(RigControl.Vox)
                Case xmitKey
                    ' transmit on/off
                    toggleTransmit()
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
                            ' Note we don't use the virtual receive freq here.
                            If SplitVFOs Then
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
        If p.fromRig Then
            If RigControl.MyCaps.HasCap(RigCaps.Caps.RITGet) Then
                FreqOut.Write(p.ID, setRIT(RigControl.RIT, False))
            End If
        Else
            AdjustRITXIT(p, RigControl.RIT)
        End If
    End Sub

    Private Sub adjustXit(p As fieldFuncParm)
        If p.fromRig Then
            If RigControl.MyCaps.HasCap(RigCaps.Caps.TXITGet) Then
                FreqOut.Write("XIT", setRIT(RigControl.XIT, True))
            End If
        Else
            AdjustRITXIT(p, RigControl.XIT)
        End If
    End Sub

    Private Sub AdjustRITXIT(ByVal p As fieldFuncParm, fld As FlexBase.RITData)
        Dim rv As FlexBase.RITData = New FlexBase.RITData(fld)
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
                If p.ID = "RIT" Then
                    Dim dat = New FlexBase.RITData(rv)
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
            If (p.ID = "RIT") Then
                RigControl.RIT = rv
            Else
                RigControl.XIT = rv
            End If
        End If
    End Sub

    Private Sub adjustSMeter(p As fieldFuncParm)
        If p.fromRig Then
            FreqOut.Write(p.ID, formatMeter(SMeter.Value))
        End If
    End Sub

    ''' <summary>
    ''' toggle split or show XMit frequency if in split mode.
    ''' </summary>
    ''' <param name="p"></param>
    ''' <remarks>toggle with space or up/down. show XMit freq with "t".</remarks>
    Private Sub adjustSplit(ByVal p As fieldFuncParm)
        If p.fromRig Then
            If ShowXMITFrequency Then
                FreqOut.Write(p.ID, "T")
            ElseIf SplitVFOs Then
                FreqOut.Write(p.ID, "S")
            Else
                FreqOut.Write(p.ID, " ")
            End If
        Else
            Dim oldTX As Integer = RigControl.TXVFO
            p.rv = True
            Select Case p.k
                Case Keys.Up, Keys.Down, Keys.Space
                    ' If showXmitFrequency, turn it off, otherwise toggle split.
                    If ShowXMITFrequency Then
                        ShowXMITFrequency = False
                    Else
                        SplitVFOs = Not SplitVFOs
                    End If
                Case Keys.S
                    ' If showXmitFreq, turn it off, else toggle split.
                    If ShowXMITFrequency Then
                        ShowXMITFrequency = False
                    Else
                        SplitVFOs = Not SplitVFOs
                    End If
                Case Keys.T
                    ' if split, toggle showXmitFreq.
                    ' otherwise turn on split and showXmitFreq.
                    If SplitVFOs Then
                        ShowXMITFrequency = Not ShowXMITFrequency
                    Else
                        SplitVFOs = True
                        ShowXMITFrequency = True
                    End If
                Case Else
                    p.rv = False
            End Select
            If p.rv Then
                If SplitVFOs Then
                    ' enable the TXVFO's audio
                    RigControl.SetVFOAudio(RigControl.TXVFO, True)
                Else
                    ' disable the oldTX VFO's audio if not the RXVFO.
                    If RigControl.ValidVFO(oldTX) And (oldTX <> RigControl.RXVFO) Then
                        RigControl.SetVFOAudio(oldTX, False)
                    End If
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' toggle the vox
    ''' </summary>
    ''' <param name="p"></param>
    ''' <remarks>toggle with space or up/down</remarks>
    Private Sub adjustVox(ByVal p As fieldFuncParm)
        If p.fromRig Then
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
                RigControl.Vox = FlexBase.OffOnValues.on
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
        If p.fromRig Then
            If MemoryMode Then
                If RigControl.CurrentMemoryChannel = -1 Then
                    MemoryMode = False
                    FreqOut.Write(p.ID, vfoLetter(RigControl.CurVFO))
                Else
                    FreqOut.Write(p.ID, "m")
                End If
            Else
                FreqOut.Write(p.ID, vfoLetter(RigControl.CurVFO))
            End If
        Else
            ' keyboard:
            ' Make sure not transmitting.
            If RigControl.Transmit Then
                Tracing.TraceLine("adjustVFO:can't change vfo while transmitting", TraceLevel.Error)
                Return
            End If
            Dim oldVal As Integer = RigControl.RXVFO
            p.rv = True
            Select Case p.k
                Case Keys.Up
                    Dim splt As Boolean = SplitVFOs
                    Dim v2 As Integer
                    v2 = RigControl.NextVFO(oldVal)
                    Tracing.TraceLine("dbgRX:" & oldVal & " " & v2)
                    RigControl.RXVFO = v2
                    While RigControl.RXVFO <> v2
                        Thread.Sleep(10)
                    End While
                    If Not splt And RigControl.CanTransmit Then
                        Tracing.TraceLine("dbgTX:" & RigControl.RXVFO)
                        RigControl.TXVFO = RigControl.RXVFO
                    End If
                    changeSliceAudio(oldVal, RigControl.RXVFO)
                Case Keys.Down
                    Dim splt As Boolean = SplitVFOs
                    RigControl.RXVFO = RigControl.PriorVFO(RigControl.RXVFO)
                    If Not splt And RigControl.CanTransmit Then
                        RigControl.TXVFO = RigControl.RXVFO
                    End If
                    changeSliceAudio(oldVal, RigControl.RXVFO)
                Case Keys.M
                    If RigControl.CurrentMemoryChannel <> -1 Then
                        MemoryMode = (Not MemoryMode)
                        If MemoryMode Then
                            RigControl.SelectMemory()
                        End If
                    Else
                        MemoryMode = False
                    End If
                Case Keys.V
                    ' just turn off memory mode.
                    MemoryMode = False
                Case Else
                    ' check for a digit
                    If (p.k >= Keys.D0) And (p.k <= Keys.D9) Then
                        Dim v As Integer = p.k - Keys.D0
                        If RigControl.ValidVFO(v) Then
                            Dim splt As Boolean = SplitVFOs
                            RigControl.RXVFO = v
                            If Not splt And RigControl.CanTransmit Then
                                RigControl.TXVFO = RigControl.RXVFO
                            End If
                            changeSliceAudio(oldVal, RigControl.RXVFO)
                        End If
                    Else
                        p.rv = False
                    End If
            End Select
        End If
    End Sub

    Private Sub adjustMem(ByVal p As fieldFuncParm)
        If p.fromRig Then
            FreqOut.Write(p.ID, RigControl.CurrentMemoryChannel.ToString)
        Else
            p.rv = False
            Dim incr As Integer
            If p.k = Keys.Up Then
                incr = 1
            ElseIf p.k = Keys.Down Then
                incr = -1
            Else
                Return
            End If
            p.rv = True
            Dim val As Integer = RigControl.CurrentMemoryChannel + incr
            If val >= RigControl.NumberOfMemories Then
                val = 0
            ElseIf val < 0 Then
                val = RigControl.NumberOfMemories - 1
            End If
            RigControl.CurrentMemoryChannel = val
            RigControl.SelectMemory()
        End If
    End Sub

    Private Sub adjustOffset(p As fieldFuncParm)
        If p.fromRig Then
            FreqOut.Write("Offset", formatOffset(RigControl.OffsetDirection))
        Else
            Select Case p.k
                Case Keys.Oemplus
                    RigControl.OffsetDirection = FlexBase.OffsetDirections.plus
                    p.rv = True
                Case Keys.OemMinus
                    RigControl.OffsetDirection = FlexBase.OffsetDirections.minus
                    p.rv = True
                Case Keys.Space, Keys.Down
                    ' cycle the value
                    Dim n As Integer = [Enum].GetValues(GetType(FlexBase.OffsetDirections)).Length
                    Dim val As FlexBase.OffsetDirections = RigControl.OffsetDirection
                    RigControl.OffsetDirection = CType(((val + 1) Mod n), FlexBase.OffsetDirections)
                    p.rv = True
                Case Keys.Up
                    ' cycle the value up
                    Dim n As Integer = [Enum].GetValues(GetType(FlexBase.OffsetDirections)).Length
                    Dim val As FlexBase.OffsetDirections = RigControl.OffsetDirection
                    If val = 0 Then
                        val = n - 1
                    Else
                        val -= 1
                    End If
                    RigControl.OffsetDirection = CType(val, FlexBase.OffsetDirections)
                    p.rv = True
            End Select
        End If
    End Sub

    Private Const soundChar As Char = "s"
    Private Const muteChar As Char = "m"
    Private Const transmitChar As Char = "\"
    Private Sub adjustRigField(p As fieldFuncParm)
        ' p.ID is the VFO or slice.
        Dim vfo As Integer = System.Int32.Parse(p.ID)
        Dim state = RigControl.SliceState(vfo)

        If p.fromRig Then
            Dim theChar As Char
            Select Case state
                Case FlexBase.SliceStates.mine
                    If RigControl.Transmit And (vfo = RigControl.TXVFO) Then
                        theChar = transmitChar
                    Else
                        If RigControl.GetVFOAudio(vfo) Then
                            theChar = soundChar
                        Else
                            theChar = muteChar
                        End If
                        ' uppercase for the transmit VFO.
                        If vfo = RigControl.TXVFO Then
                            theChar = Char.ToUpper(theChar)
                        End If
                    End If
                Case FlexBase.SliceStates.available
                    theChar = "."
                Case Else
                    theChar = "-"
            End Select
            FreqOut.Write(p.ID, theChar)

        Else
            ' from the user
            ' must be without modifiers.
            If (p.k And Keys.Modifiers) <> 0 Then
                Return
            End If
            p.rv = True
            Select Case p.k
                Case Keys.D0, Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7
                    Dim k = System.Int32.Parse(p.k) - 48
                    RigControl.CopyVFO(p.ID, k)
                Case Keys.OemPeriod
                    If state = FlexBase.SliceStates.available Then
                        RigControl.NewSlice()
                    ElseIf state = FlexBase.SliceStates.mine Then
                        RigControl.RemoveSlice(vfo)
                    End If
                Case Keys.A
                    If state = FlexBase.SliceStates.mine Then
                        ' active slice (RXVFO)
                        RigControl.RXVFO = vfo
                    End If
                Case Keys.T
                    If (state = FlexBase.SliceStates.mine) And RigControl.CanTransmit Then
                        ' transmit slice (TXVFO)
                        RigControl.TXVFO = vfo
                    End If
                Case Keys.X
                    ' transceive
                    If state = FlexBase.SliceStates.mine Then
                        RigControl.RXVFO = vfo
                        If RigControl.CanTransmit Then
                            RigControl.TXVFO = vfo
                        End If
                    End If
                Case Keys.L
                    If state = FlexBase.SliceStates.mine Then
                        ' pan left side
                        RigControl.SetVFOPan(vfo, FlexBase.MinPan)
                    End If
                Case Keys.C
                    If state = FlexBase.SliceStates.mine Then
                        ' pan center
                        Dim val = ((FlexBase.MaxPan - FlexBase.MinPan) / 2) + FlexBase.MinPan
                        RigControl.SetVFOPan(vfo, val)
                    End If
                Case Keys.R
                    If state = FlexBase.SliceStates.mine Then
                        ' pan right side
                        RigControl.SetVFOPan(vfo, FlexBase.MaxPan)
                    End If
                Case Keys.PageUp
                    If state = FlexBase.SliceStates.mine Then
                        ' pan left
                        Dim val = RigControl.GetVFOPan(vfo) - 10
                        RigControl.SetVFOPan(vfo, val)
                    End If
                Case Keys.PageDown
                    If state = FlexBase.SliceStates.mine Then
                        ' pan right
                        Dim val = RigControl.GetVFOPan(vfo) + 10
                        RigControl.SetVFOPan(vfo, val)
                    End If
                Case Keys.Up
                    If state = FlexBase.SliceStates.mine Then
                        ' volume up
                        Dim val = RigControl.GetVFOGain(vfo) + 10
                        RigControl.SetVFOGain(vfo, val)
                    End If
                Case Keys.Down
                    If state = FlexBase.SliceStates.mine Then
                        ' volume down
                        Dim val = RigControl.GetVFOGain(vfo) - 10
                        RigControl.SetVFOGain(vfo, val)
                    End If
                Case Keys.M, Keys.S, Keys.Space
                    If state = FlexBase.SliceStates.mine Then
                        ' setup future states array, used because state changes are queued.
                        Dim futures(RigControl.MyNumSlices - 1) As Boolean
                        For i As Integer = 0 To RigControl.MyNumSlices - 1
                            futures(i) = RigControl.GetVFOAudio(i)
                        Next

                        Dim k As Keys = p.k
                        ' space is a toggle.
                        If k = Keys.Space Then
                            If RigControl.GetVFOAudio(vfo) Then
                                k = Keys.M ' mute
                            Else
                                k = Keys.S ' sound
                            End If
                        End If
                        If k = Keys.M Then
                            ' mute
                            RigControl.SetVFOAudio(vfo, False)
                            futures(vfo) = False
                        Else
                            ' sound
                            RigControl.SetVFOAudio(vfo, True)
                            futures(vfo) = True
                        End If
                        ' If only one slice is sounding, make it the active slice.
                        Dim active As Integer = -1
                        For i As Integer = 0 To RigControl.MyNumSlices - 1
                            If futures(i) Then
                                If active = -1 Then
                                    ' only sounding slice so far.
                                    active = i
                                Else
                                    ' multiple sounding slices
                                    active = -1
                                    Exit For
                                End If
                            End If
                        Next
                        If active <> -1 Then
                            ' Only one sounding slice.
                            Dim wasSplit = SplitVFOs
                            RigControl.RXVFO = active
                            If Not wasSplit And RigControl.CanTransmit Then
                                RigControl.TXVFO = active
                            End If
                        End If
                    End If
                Case Else
                    ' unhandled
                    p.rv = False
            End Select
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
    Private usingMem As Boolean = False
    ' Update the freqOut display.
    ' Write it to the screen if anything actually changed.
    Private Sub showFrequency()
        Tracing.TraceLine("showFrequency", TraceLevel.Verbose)
        If MemoryMode And (Not usingMem) Then
            ' Now using a memory.
            currentFields = memFreqFields
            FreqOut.Populate(memFreqFields)
            usingMem = True
        ElseIf (Not MemoryMode) And usingMem Then
            ' Now it's a vfo.
            currentFields = vfoFreqFields
            FreqOut.Populate(vfoFreqFields)
            usingMem = False
        End If
        ' Otherwise there was no change.
        For id As Integer = 0 To currentFields.Length - 1
            ' This sets parm.FromRig.
            Dim parm = New fieldFuncParm(currentFields(id).Key)
            FreqOut.Function(parm.ID, parm)
        Next
        If FreqOut.Changed Then
            writeFreq()
        End If
    End Sub
    Private Function setRIT(ByVal rit As FlexBase.RITData, ByVal xit As Boolean) As String
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
    ' Actually returns the number's string
    Private Function vfoLetter(ByVal v As Integer) As String
        Return v.ToString
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
            ' Receive
            If RigControl.SmeterInDBM Then
                rv = val
            Else
                ' s-units
                If val > 9 Then
                    rv = "+" & (val - 9).ToString()
                Else
                    rv = val.ToString()
                End If
            End If
        End If
            Return rv
    End Function

    Private Function formatOffset(offset As FlexBase.OffsetDirections) As Char
        Static outChars As Char() = {" ", "-", "+", "e"}
        Dim rv As Char
        Try
            If (RigControl.Mode IsNot Nothing) AndAlso (RigControl.Mode.ToString = "FM") Then
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
    Friend Sub UpdateStatus()
        Tracing.TraceLine("updateStatus", TraceLevel.Verbose)
        If Ending Then
            Return
        End If
        If (RigControl Is Nothing) OrElse Not RigControl.IsConnected Then
            'powerNowOff()
            Return
        End If

        Try
            ' don't assume power on initially.
            If Power Then
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
                   RigControl.FlexTunerOn And
                   (RigControl.FlexTunerType = FlexBase.FlexTunerTypes.manual) Then
                    Dim SWRtext = OpenParms.GetSWRText()
                    If SWRtext <> oldSWR Then
                        oldSWR = SWRtext
                        setButtonText(AntennaTuneButton, oldSWR)
                    End If
                End If

#If 0 Then
                ' See if any data to display.
                Tracing.TraceLine("UpdateStatus:doing receive data", TraceLevel.Verbose)
                If RigControl.CanReceiveData Then
                    WriteText(WindowIDs.ReceiveDataOut, RigControl.DataReceived, False)
                End If
#End If

            Else
                ' power is off
            End If

            ' Update the status.
            If StatusBox.Changed Then
                StatusBox.Display()
            End If
        Catch ex As Exception
            If Not Power Then
                Tracing.TraceLine("updateStatus:power is off", TraceLevel.Error)
            Else
                Tracing.ErrMessageTrace(ex, True, True)
                'Tracing.TraceLine("updateStatus:" & ex.Message, TraceLevel.Error)
                powerNowOff()
            End If
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
        Tracing.TraceLine("knobOnOffHandler:" & e.Status.ToString, TraceLevel.Info)
        Dim onOffText As String = ""
        Select Case e.Status
            Case FlexKnob.KnobStatus_t.fullOn
                onOffText = OnWord
            Case FlexKnob.KnobStatus_t.off
                onOffText = OffWord
            Case FlexKnob.KnobStatus_t.locked
                onOffText = LockedWord
        End Select
        StatusBox.Write("Knob", onOffText)
    End Sub

    Private Sub powerStatusHandler(sender As Object, status As Boolean)
        If status Then
            powerNowOn()
        Else
            powerNowOff()
        End If
    End Sub

    Private Sub powerNowOn()
        Tracing.TraceLine("Form1 powerNowOn", TraceLevel.Error)
        TextOut.PerformGenericFunction(FreqOut,
            Sub()
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
#If LeaveBootTraceOn = 0 Then
                turnTracingOff()
#End If
                SetupOperationsMenu()
                setupFreqout()
            End Sub)
        'Tracing.TraceLine("rig caps:" & RigControl.MyCaps.ToString(), TraceLevel.Info)
        invokeConfigVariableControls()
        enableDisableWindowControls(True)
        AddHandler FlexKnob.KnobOnOffEvent, AddressOf knobOnOffHandler
        SetupKnob()
        StatusBox.Write("Power", OnWord)
        StatusBox.Write("Memories", RigControl.NumberOfMemories.ToString)
        'knobOnOffHandler(Me, RigControl.KnobStatus)
        AddHandler RigControl.TransmitChange, AddressOf transmitChangeProc
        AddHandler RigControl.FlexAntTunerStartStop, AddressOf FlexAntTuneStartStopHandler
        AddHandler RigControl.ConnectedEvent, AddressOf connectedEventHandler
        Power = True
    End Sub

    Friend Sub powerNowOff()
        Tracing.TraceLine("Form1 powerNowOff", TraceLevel.Info)
        RemoveHandler FlexKnob.KnobOnOffEvent, AddressOf knobOnOffHandler
        If RigControl IsNot Nothing Then
            RemoveHandler RigControl.TransmitChange, AddressOf transmitChangeProc
            RemoveHandler RigControl.FlexAntTunerStartStop, AddressOf FlexAntTuneStartStopHandler
            RemoveHandler RigControl.ConnectedEvent, AddressOf connectedEventHandler
        End If
        Power = False
        If Not Ending Then
            clearMainWindow()
            StatusBox.Write("Power", OffWord)
            enableDisableWindowControls(False)
        End If
    End Sub

    Private Sub turnTracingOff()
        If BootTrace Then
            Tracing.TraceLine("Boot tracing off")
            Tracing.On = False
            BootTrace = False
        End If
    End Sub

    Private Sub connectedEventHandler(sender As Object, e As FlexBase.ConnectedArg)
        Tracing.TraceLine("connectedEventHandler:" & Power.ToString & " " & e.Connected.ToString, TraceLevel.Info)
        If Power And Not e.Connected Then
            powerNowOff()
            MessageBox.Show(nowDisconnected, ErrorHdr, MessageBoxButtons.OK)
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
            'remoteLan = False
            If Commands IsNot Nothing Then
                Commands.ClusterShutdown()
            End If
            CloseTheRadio()
            If W2WattMeter IsNot Nothing Then
                W2WattMeter.Dispose()
            End If
            setScreenSaver(onExitScreenSaver)
            Tracing.TraceLine("exit:screen saver set:" & onExitScreenSaver.ToString, TraceLevel.Info)
        Catch ex As Exception
            Tracing.TraceLine("Form1 FileExitToolstripMenuItem:" & ex.Message, TraceLevel.Error)
        End Try
        Tracing.TraceLine("End.")
        Tracing.On = False
        Me.Dispose()
        Environment.Exit(0)
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

        If RigControl IsNot Nothing Then
            RigControl.OperatorChangeHandler()
        End If
    End Sub

    Private selectorThread As Thread
    Private Sub SelectRigMenuItem_Click(sender As Object, e As EventArgs) Handles SelectRigMenuItem.Click
        Tracing.TraceLine("SelectRigMenuItem_Click", TraceLevel.Info)
        Try
            If RigControl IsNot Nothing Then
                CloseTheRadio()
            End If
            openTheRadio(False) ' a subsequent open
        Catch ex As Exception
            Tracing.TraceLine("SelectRigMenuItem_Click:exception " & ex.Message, TraceLevel.Error)
        End Try
    End Sub

    Private Delegate Sub cvcdel()
    Private Sub invokeConfigVariableControls()
        Tracing.TraceLine("invokeConfigVariableControls", TraceLevel.Info)
        Dim cvcRtn As cvcdel = AddressOf configVariableControls
        ' TransmitButton is always present.
        If TransmitButton.InvokeRequired Then
            TransmitButton.Invoke(cvcRtn)
        Else
            cvcRtn()
        End If
        'Tracing.TraceLine("invokeConfigVariableControls:done", TraceLevel.Info)
    End Sub
    Private Sub rigCapsChanged(arg As FlexBase.CapsChangeArg)
        Tracing.TraceLine("rigCapsChanged:" + arg.NewCaps.ToString(), TraceLevel.Info)
        invokeConfigVariableControls()
        enableDisableWindowControls(True)
    End Sub

    Private Function doCommand(ByVal e As System.Windows.Forms.KeyEventArgs) As Boolean
        Dim rv As Boolean = Commands.DoCommand(e.KeyData)
        Return rv
    End Function

    Public Sub DisplayHelp()
        DisplayHelp(ShowHelpTypes.standard)
    End Sub

    Private Sub DisplayHelp(helpType As ShowHelpTypes)
        Dim helper = New ShowHelp(helpType)
        helper.ShowDialog()
        helper.Dispose()
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

    Friend Sub doCommand_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ModeControl.BoxKeydown, AntennaTuneButton.KeyDown, TXTuneControl.BoxKeydown, StatusBox.BoxKeydown, ReceivedTextBox.KeyDown, TransmitButton.KeyDown
        e.SuppressKeyPress = doCommand(e)
        Tracing.TraceLine("doCommand_KeyDown: DoCommand returned " & e.SuppressKeyPress.ToString, TraceLevel.Info)
    End Sub

    Private Sub LogCharacteristicsMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LogCharacteristicsMenuItem.Click
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
            ProgramDirectory & "\JJFlexRadioReadme.htm"
        System.Diagnostics.Process.Start(fn)
    End Sub

    Private Sub HelpKeysItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HelpKeysItem.Click
        DisplayHelp(ShowHelpTypes.standard)
    End Sub

    Private Sub HelpKeysAlphaItem_Click(sender As Object, e As EventArgs) Handles HelpKeysAlphaItem.Click
        DisplayHelp(ShowHelpTypes.alphabetic)
    End Sub

    Private Sub HelpKeysGroupItem_Click(sender As Object, e As EventArgs) Handles HelpKeysGroupItem.Click
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

    ' FreqBox handling.  Also see setupFreqout().
    Private Const meterSize As Integer = 4 ' field size
    Private vfoFreqFields() As MainBox.Field
    ' the fixed stuff, (i.e.) freq, RIT, etc.
    Private vfoFields() As MainBox.Field =
        {New MainBox.Field("SMeter", meterSize, "", "", AddressOf adjustSMeter),
         New MainBox.Field("Split", 1, "", "", AddressOf adjustSplit),
         New MainBox.Field("VOX", 1, "", "", AddressOf adjustVox),
         New MainBox.Field("VFO", 1, "", "", AddressOf adjustVFO),
         New MainBox.Field("Freq", 12, "", "", AddressOf AdjustFreq),
         New MainBox.Field("Offset", 1, "", "", AddressOf adjustOffset),
         New MainBox.Field("RIT", 5, "", "", AddressOf adjustRit),
         New MainBox.Field("XIT", 5, " ", "", AddressOf adjustXit)}
    Private memFreqFields() As MainBox.Field
    ' the fixed stuff, (i.e.) freq, RIT, etc.
    Private memFields() As MainBox.Field =
        {New MainBox.Field("SMeter", 4, "", "", AddressOf adjustSMeter),
         New MainBox.Field("Split", 1, "", "", AddressOf adjustSplit),
         New MainBox.Field("VOX", 1, "", "", AddressOf adjustVox),
         New MainBox.Field("VFO", 1, "", "", AddressOf adjustVFO),
         New MainBox.Field("Memory", 3, "", "", AddressOf adjustMem),
         New MainBox.Field("Freq", 12, "", "", AddressOf AdjustFreq),
         New MainBox.Field("Offset", 1, "", "", AddressOf adjustOffset),
         New MainBox.Field("RIT", 5, " ", "", AddressOf adjustRit),
         New MainBox.Field("XIT", 5, " ", "", AddressOf adjustXit)}
    Private Const freqID As String = "Freq"
    ' Passed to field functions.
    Class fieldFuncParm
        Public ID As String
        Public fromRig As Boolean = False
        Public k As Keys
        Public rv As Boolean
        Public Sub New(i As String, ByVal key As Keys, ByVal ret As Boolean)
            ID = i
            k = key
            rv = ret
        End Sub
        Public Sub New(i As String)
            ID = i
            fromRig = True
        End Sub
    End Class

    ' Return the id of the freqOut field we're in, or -1 if not.
    Private Function freqoutField() As Integer
        Dim rv As Integer = -1
        Dim pos As Integer = FreqOut.SelectionStart
        For i As Integer = 0 To FreqOut.NumberOfFields - 1
            If (pos >= FreqOut.Position(i)) And (pos < FreqOut.Position(i) + FreqOut.Length(i)) Then
                rv = i
                Exit For
            End If
        Next
        Return rv
    End Function
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
        If (RigControl IsNot Nothing) AndAlso Power Then
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

        ' These levels' values are shown as the peak over a time period.
        'SMeter = New Levels(Function() RigControl.SMeter)
        SMeter = New Levels(AddressOf getSMeter)
        'SMeter.Cycle = 2000
        SMeter.Peak = True

        combos = New List(Of Combo)
        enableDisableControls = New List(Of Control)

        ModeControl.Visible = True
        ModeControl.Clear()
        ModeControl.Enabled = True
        modeList = New ArrayList
        For Each Val As String In RigCaps.ModeTable
            modeList.Add(Val)
        Next
        ModeControl.TheList = modeList
        ModeControl.UpdateDisplayFunction = Function() RigControl.Mode
        ModeControl.UpdateRigFunction =
            Sub(v As String)
                If Not Power Then
                    Tracing.TraceLine("mode:no power", TraceLevel.Error)
                    Return
                End If
                RigControl.Mode = v
            End Sub
        combos.Add(ModeControl)
        enableDisableControls.Add(ModeControl)

        ' Add other controls to the enable/disable collection.
        enableDisableControls.Add(SentTextBox)

        ' Add any config variable controls.
        invokeConfigVariableControls()
    End Sub

    Private Sub configVariableControls()
        Tracing.TraceLine("configVariableControls", TraceLevel.Info)
        enableDisableControls.Remove(TransmitButton)
        'TransmitButton.Enabled = False
        TransmitButton.Visible = True
        If RigControl.MyCaps.HasCap(RigCaps.Caps.ManualTransmit) Then
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
        If RigControl.MyCaps.HasCap(RigCaps.Caps.ATSet) Then
            ' auto tuner
            Tracing.TraceLine("configVariableControls:autoTuner", TraceLevel.Info)
            TXTuneList = New ArrayList
            TXTuneList.Add(New TrueFalseElement(False))
            TXTuneList.Add(New TrueFalseElement(True))
            TXTuneControl.TheList = TXTuneList
            TXTuneControl.UpdateDisplayFunction =
                Function()
                    Return (RigControl.FlexTunerType = FlexBase.FlexTunerTypes.auto)
                End Function
            TXTuneControl.UpdateRigFunction =
                Sub(v As Boolean)
                    If Not Power Then
                        Tracing.TraceLine("TXTune:no power", TraceLevel.Error)
                        Return
                    End If
                    Dim val = FlexBase.FlexTunerTypes.manual
                    If (v And RigControl.MyCaps.HasCap(RigCaps.Caps.ATGet)) Then
                        val = FlexBase.FlexTunerTypes.auto
                    End If
                    RigControl.FlexTunerType = val
                End Sub
            combos.Add(TXTuneControl)
            enableDisableControls.Add(TXTuneControl)
            TXTuneControl.Clear()
            'TXTuneControl.Enabled = True
            'TXTuneControl.Visible = True

            'AntennaTuneButton.Enabled = True
            'AntennaTuneButton.Visible = True
            enableDisableControls.Add(AntennaTuneButton)
        ElseIf RigControl.MyCaps.HasCap(RigCaps.Caps.ManualATSet) Then
            Tracing.TraceLine("configVariableControls:manual tuner", TraceLevel.Info)
            'AntennaTuneButton.Enabled = True
            'AntennaTuneButton.Visible = True
            enableDisableControls.Add(AntennaTuneButton)
        End If
        Tracing.TraceLine("configVariableControls:done", TraceLevel.Info)
    End Sub

    Private Sub setupFreqout()
        ' setup the main frequency box fields.
        ' Note the slices are on the left side, named "0" through "n".
        Dim VFOs As Integer = RigControl.TotalNumSlices
        ReDim vfoFreqFields(VFOs + vfoFields.Length - 1)
        ReDim memFreqFields(VFOs + memFields.Length - 1)
        For i As Integer = 0 To VFOs - 1
            Dim name As String = i.ToString() ' name is really an index
            vfoFreqFields(i) = New MainBox.Field(name, 1, "", "", AddressOf adjustRigField)
            memFreqFields(i) = New MainBox.Field(name, 1, "", "", AddressOf adjustRigField)
        Next
        ' Setup the fixed stuff, (i.e.) freq, RIT, etc.
        For i As Integer = VFOs To VFOs + vfoFields.Length - 1
            vfoFreqFields(i) = vfoFields(i - VFOs)
        Next
        For i As Integer = VFOs To VFOs + memFields.Length - 1
            memFreqFields(i) = memFields(i - VFOs)
        Next

        FreqOut.Populate(vfoFreqFields)
        ' default current FreqOut display fields
        currentFields = vfoFreqFields
    End Sub

    Private Sub FreqOut_BoxKeydown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles FreqOut.BoxKeydown
        If Not Power Then
            Tracing.TraceLine("freqout:no power", TraceLevel.Error)
            Return
        End If

        Dim fld As MainBox.Field = FreqOut.PositionToField(FreqOut.SelectionStart)
        If (RigControl IsNot Nothing) And (fld IsNot Nothing) And
            Not (e.Alt Or e.Control) Then
            ' Get the field's key.
            Dim fldKey As String = fld.Key
            ' execute the field's function if there is one.
            ' Note that freqOut.Function returns true if there's a function.
            ' The function sets parm.rv if it processed the key.
            Dim parm As New fieldFuncParm(fldKey, e.KeyData, False)
            FreqOut.Function(fldKey, parm)
            e.SuppressKeyPress = parm.rv
        End If
        ' if not already handled...
        If Not e.SuppressKeyPress Then
            FreqoutKeyHandler(e)
        End If
        If Not e.SuppressKeyPress Then
            e.SuppressKeyPress = doCommand(e)
        End If
    End Sub

    Private Sub TransmitButton_Click(sender As System.Object, e As System.EventArgs) Handles TransmitButton.Click
        Tracing.TraceLine("TransmitButton_Click", TraceLevel.Info)
        toggleTransmit()
    End Sub
    Private Sub toggleTransmit()
        If Not Power Then
            Tracing.TraceLine("toggleTransmit:no power", TraceLevel.Error)
            Return
        End If

        Tracing.TraceLine("toggling transmit:" & RigControl.Transmit.ToString, TraceLevel.Info)
        RigControl.Transmit = Not RigControl.Transmit
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

    Private Sub AntennaTuneButton_Enter(sender As Object, e As EventArgs) Handles AntennaTuneButton.Enter
        If Not Power Then
            Tracing.TraceLine("antennaTune:no power", TraceLevel.Error)
            Return
        End If
        setButtonText(AntennaTuneButton, antennaTuneButtonText)
    End Sub

    Private Sub AntennaTuneButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AntennaTuneButton.Click
        If Not Power Then
            Tracing.TraceLine("antennaTune:no power", TraceLevel.Error)
            Return
        End If
        oldSWR = ""
        ' just toggle the tuner on/off.
        RigControl.FlexTunerOn = Not RigControl.FlexTunerOn
    End Sub

    Private Sub AntennaTuneButton_Leave(sender As Object, e As EventArgs) Handles AntennaTuneButton.Leave
        If Not Power Then
            Return
        End If
        setButtonText(AntennaTuneButton, antennaTuneButtonText)
    End Sub

    Private Sub FlexAntTuneStartStopHandler(e As FlexBase.FlexAntTunerArg)
        TextOut.PerformGenericFunction(AntennaTuneButton,
            Sub()
                If RigControl.FlexTunerType = FlexBase.FlexTunerTypes.manual Then
                    If (e.Status = "OK") Then
                        setButtonText(AntennaTuneButton, e.SWR)
                    End If
                Else
                    setButtonText(AntennaTuneButton, e.Status)
        End If
                'AntennaTuneButton.Focus()
            End Sub)
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
        If RigControl Is Nothing Then
            Return
        End If

        If isFunction(e) Then
            ' not just a character to send.
            ' check for clipboard functions
            If e.Control Then
                Select Case e.KeyCode
                    Case Keys.B
                        ' start/stop buffering
                        RigControl.CWBuffering = Not RigControl.CWBuffering
                        e.SuppressKeyPress = True
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

    Private cwBuf As String = ""
    Private Sub SentTextBox_KeyPress(sender As System.Object, e As System.Windows.Forms.KeyPressEventArgs) Handles SentTextBox.KeyPress
        If Not Power Then
            Tracing.TraceLine("SentTextBox:no power", TraceLevel.Error)
            Return
        End If

        Tracing.TraceLine("SentTextBox_KeyPress:" & AscW(e.KeyChar), TraceLevel.Info)
        If DirectCW Then
            RigControl.SendCW(e.KeyChar)
            Return
        End If
        ' check for backspace
        If (e.KeyChar = ChrW(8)) And (cwBuf.Length <> 0) Then
            cwBuf = cwBuf.Substring(0, cwBuf.Length - 1)
            Return
        End If
        cwBuf &= e.KeyChar
        If Char.IsWhiteSpace(e.KeyChar) Then
            RigControl.SendCW(cwBuf)
            cwBuf = ""
        End If
    End Sub

    Private Sub ShowBandsMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles ShowBandsMenuItem.Click
        ShowBands.ShowDialog()
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
        If (RigControl IsNot Nothing) AndAlso (RigControl.RigFields IsNot Nothing) AndAlso (RigControl.RigFields.ScreenFields IsNot Nothing) Then
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
            If (RigControl Is Nothing) OrElse Not Power Then
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

    Friend Sub gotoHome()
        TextOut.PerformGenericFunction(Me,
            Sub()
                Me.BringToFront()
                FreqOut.Focus()
            End Sub)
    End Sub

    Private Function currentOperatorName() As String
        Return CurrentOp.UserBasename
    End Function

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

    ' For testing when Debugger is attached
    Private Sub testMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        RigControl.TestRoutine()
    End Sub

    Private Function getControlsOfInterest() As List(Of Control)
        Dim rv As List(Of Control) = New List(Of Control)
        For Each ctl As Control In Me.Controls
            rv.Add(ctl)
        Next
        rv.AddRange(RigControl.RigFields.ScreenFields)
        Return rv
    End Function

    Private Sub StationNamesMenuItem_Click(sender As Object, e As EventArgs) Handles StationNamesMenuItem.Click
        ShowStationNames.ShowDialog()
    End Sub

    Private Sub noSliceErrorHandler(sender As Object, msg As String)
        MessageBox.Show(msg, ErrorHdr, MessageBoxButtons.OK)
        CloseTheRadio()
    End Sub

    Private Sub LocalPTTMenuItem_Click(sender As Object, e As EventArgs) Handles LocalPTTMenuItem.Click
        If RigControl IsNot Nothing Then
            RigControl.LocalPTT = True
        End If
    End Sub

    Private Sub ProfilesMenuItem_Click(sender As Object, e As EventArgs) Handles ProfilesMenuItem.Click
        Dim profile = New Profile
        Dim theForm = CType(profile, Form)
        theForm.ShowDialog()
        profile.Dispose()
    End Sub

    Private Sub ExportOperatorMenuItem_Click(sender As Object, e As EventArgs) Handles ExportSetupMenuItem.Click
        ExportSetup.ExportSetup()
    End Sub
End Class
