﻿Imports System.Collections
Imports System.Collections.Generic
Imports System.Collections.ObjectModel
'Imports System.Collections.Specialized
'Imports System.Configuration
Imports System.Diagnostics
Imports System.IO
Imports System.IO.Ports
Imports System.Reflection
Imports System.Threading
Imports System.Xml.Serialization
Imports adif
Imports JJCountriesDB
Imports JJLogLib
Imports JJPortaudio
Imports JJTrace
Imports JJW2WattMeter
Imports MsgLib
Imports Radios

Module globals
    Public Const CopyRight As String = "Copyright 2013 by J.J. Shaffer"
    Friend Const OnWord As String = "On"
    Friend Const OffWord As String = "Off"
    Friend Const NoneWord As String = "none"
    Friend Const Loaded As String = "loaded"
    Friend Const Loading As String = "loading"
    Friend Const Running As String = "running"
    Friend Const Paused As String = "paused"
    Friend Const msgMemoriesLoaded As String = "Memories are all loaded."
    Friend Const msgMenusLoaded As String = "Menus are all loaded."
    Friend Const mustHaveLog As String = "A log file must be defined."
    Friend Const NotSupportedForThisRig As String = "This function is not supported on this radio."
    Friend Const NoLongerSupported As String = "This function is no longer supported."
    Friend Const RequiresBrailleDisplay As String = "This function requires a braille display."
    Friend Const NotValidHost As String = "Hostname must be host or host:port."
    Friend Const NoAudioDevice As String = "No output audio device is configured."

#If 0 Then
    Friend AppSettings As AppSettingsSection
    Friend Function GetConfigValue(key As String) As String
        Dim rv As String
        If (AppSettings IsNot Nothing) AndAlso (AppSettings.Settings(key) IsNot Nothing) Then
            rv = AppSettings.Settings(key).Value
        Else
            rv = vbNullString
        End If
        Return rv
    End Function
#End If

    Friend BootTrace As Boolean
    Friend ProgramDirectory As String ' This program's directory.
    Friend UsingSimulator As Boolean
    Friend Commands As KeyCommands
    Friend RadioPort As ComPort
    Friend ContactLog As LogClass
    Friend LookupStation As StationLookup = Nothing

    Friend Dups As LogDupChecking
    ''' <summary>
    ''' dup checking type
    ''' </summary>
    Friend ReadOnly Property DupType As LogDupChecking.DupTypes
        Get
            If Dups Is Nothing Then
                Return LogDupChecking.DupTypes.none
            Else
                Return Dups.dupType
            End If
        End Get
    End Property
    ''' <summary>
    ''' True if dup checking
    ''' </summary>
    Friend ReadOnly Property isDupChecking As Boolean
        Get
            Return DupType <> LogDupChecking.DupTypes.none
        End Get
    End Property

    Friend FindDialog As Boolean = False

    Friend CWText As CWMessages
    Enum WindowIDs
        ReceiveDataOut
        SendDataOut
    End Enum
    Delegate Sub WrtTxt(ByVal TextboxID As WindowIDs, ByVal text As String, ByVal clearFlag As Boolean)
    Friend WriteText As WrtTxt
    Delegate Sub WrtTxtX(ByVal tbid As WindowIDs, ByVal s As String, _
                         ByVal cur As Integer, ByVal c As Boolean)
    Friend WriteTextX As WrtTxtX
    Delegate Sub tbrtn(ByVal tbid As WindowIDs)
    ''' <summary>
    ''' True if ending the program.
    ''' Access with volatile read and write.
    ''' </summary>
    Friend Ending As Boolean = False

    ''' <summary>
    ''' SMeter raw and calibrated values.
    ''' </summary>
    Friend SMeter As Levels

    ' region - Config data stuff.
#Region "config"
    Friend myAssembly As Assembly
    Friend myAssemblyName As AssemblyName
    Friend myVersion As Version
    ''' <summary>
    ''' configuration event types
    ''' </summary>
    Friend Enum ConfigEvents
        OperatorChanged
        RigChanged
    End Enum
    ''' <summary>
    ''' type of the config event argument.
    ''' </summary>
    Friend Class ConfigArgs
        Inherits EventArgs
        Public TheEvent As ConfigEvents
        Public TheData As Object
        ''' <summary>
        ''' define a config event
        ''' </summary>
        ''' <param name="e">the event from ConfigEvents</param>
        ''' <param name="d">Event dependent data</param>
        Public Sub New(ByVal e As ConfigEvents, ByVal d As Object)
            TheEvent = e
            TheData = d
        End Sub
    End Class

    Friend Const ConfigDirBaseName = "JJRadio"
    Const reqOpMsgTitle As String = "You must define a default operator."
    Const reqOpMsg As String = _
        "If you do not define a default operator, the program will exit." & vbCrLf & _
        "Do you wish to define one?"
    Const noDefaultRigTitle As String = "No default rig"
    Const noDefaultRig As String = _
        "You must define a default rig." & vbCrLf & _
        "Do you wish to define one?"
    Friend MenusLoaded As Boolean
    Friend MemoriesLoaded As Boolean
    Friend BaseConfigDir As String
    Friend LastUserTraceFile As String ' Last user-started trace file (see DebugInfo)
    Friend WithEvents Operators As PersonalData = Nothing
    Friend WithEvents Knob As FlexKnob = Nothing
    Friend Rigs As RigData = Nothing
    ''' <summary>
    ''' (ReadOnly) the current operator
    ''' </summary>
    Friend ReadOnly Property CurrentOp As PersonalData.personal_v1
        Get
            Return Operators.CurrentItem
        End Get
    End Property
    ''' <summary>
    ''' ID of the current operator
    ''' </summary>
    Friend Property CurrentOpID As Integer
        Get
            Return Operators.CurrentID
        End Get
        Set(ByVal value As Integer)
            Operators.CurrentID = value
        End Set
    End Property
    Friend CurrentRig As RigData.rig = Nothing
    Friend CurrentRigID As Integer
    Friend WithEvents RigControl As Radios.AllRadios
    Friend KeepCheckingOpen As Boolean = False
    ''' <summary>
    ''' Current rig's open parameters.
    ''' </summary>
    Friend OpenParms As AllRadios.OpenParms

    Friend Const HamqthLookupID As String = "JJRadio"
    Friend Const HamqthLookupPassword As String = "JJRadio"

    ''' <summary>
    ''' W2 wattmeter.
    ''' </summary>
    Friend W2WattMeter As W2
    Friend W2ConfigFile As String
    Private W2ConfigFileBasename As String = "w2.xml"

    Friend Sub GetConfigInfo()
        myAssembly = Assembly.GetEntryAssembly
        myAssemblyName = myAssembly.GetName
        myVersion = myAssemblyName.Version
        BaseConfigDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) &
            "\" & ConfigDirBaseName
        Try
            If Not Directory.Exists(BaseConfigDir) Then
                ' show welcome screen.
                If Welcome.ShowDialog() <> DialogResult.OK Then
                    End
                End If
                Directory.CreateDirectory(BaseConfigDir)
            End If
        Catch ex As Exception
            Tracing.ErrMessageTrace(ex)
            Exit Sub
        End Try

        BootTrace = (Not Debugger.IsAttached)
        'BootTrace = True
        Dim bootLevel As TraceLevel = TraceLevel.Info
        If BootTrace Then
            Tracing.TraceFile = BaseConfigDir & "\JJRadioBootTrace.txt"
            Tracing.TheSwitch.Level = bootLevel
            Tracing.On = True
            Tracing.TraceLine("Boot Tracing on, " & myAssembly.Location & " " & myVersion.ToString() & " " & Date.Now & " level=" & bootLevel.ToString)
        End If

        Tracing.TraceLine("GetConfigInfo:" & BaseConfigDir, TraceLevel.Info)
        ' Audio device selection file name.
        AudioDevicesFile = BaseConfigDir & "\" & audioDevicesBasename

        ' Load keyboard command config data.
        Commands = New KeyCommands
        Form1.SetupOperationsMenu()

        ' Load operator and rig data.
        Operators = New PersonalData(BaseConfigDir)
        ' There must be a default operator!
        While CurrentOp Is Nothing
            SetCurrentOp(Operators.TheDefault, Operators.DefaultID)
            If CurrentOp Is Nothing Then
                If MessageBox.Show(reqOpMsg, reqOpMsgTitle, MessageBoxButtons.YesNo) <> DialogResult.Yes Then
                    End
                End If
                Lister.TheList = Operators
                Lister.ShowDialog()
            End If
        End While

        ' setup log file
        ConfigContactLog()

        Rigs = New RigData(BaseConfigDir)
        ' Start with the default rig.
        SetCurrentRig(Rigs.TheDefault, Rigs.DefaultID)
        If RigControl Is Nothing Then
            RigControl = New Radios.NullRig
        End If
#If 0 Then
        While CurrentRig Is Nothing
            SetCurrentRig(Rigs.TheDefault, Rigs.DefaultID)
            If CurrentRig Is Nothing Then
                If MessageBox.Show(noDefaultRig, noDefaultRigTitle, MessageBoxButtons.YesNo) <> DialogResult.Yes Then
                    End
                End If
                Lister.TheList = Rigs
                Lister.ShowDialog()
            End If
        End While
#End If

        ' Check for W2 watt meter.
        W2ConfigFile = BaseConfigDir & "\" & W2ConfigFileBasename
        ConfigW2(True) ' only setup if already configured.
    End Sub

    Friend Sub SetCurrentOp(ByVal op As PersonalData.personal_v1, _
                            ByVal id As Integer)
        If op IsNot Nothing Then
            Tracing.TraceLine("SetCurrentOp(" & op.fullName & "," & id.ToString & ")", TraceLevel.Info)
        Else
            Tracing.TraceLine("SetCurrentOp:no operator", TraceLevel.Error)
            id = -1
        End If
        CurrentOpID = id
        If id = -1 Then
            Return
        End If

        ' Initialize the optional message processor
        OptionalMessage.Setup(AddressOf Operators.UpdateOptionalMessages, AddressOf Operators.RetrieveOptionalMessages)
        ' Update the key dictionaries.
        Commands.UpdateCWText()
        ' Setup macros
        MacroItems.Items(MacroItems.MacroIDS.myCallSign).Acquire = _
            Function()
                Return op.callSign
            End Function
        MacroItems.Items(MacroItems.MacroIDS.myName).Acquire = _
            Function()
                Return op.handl
            End Function
        MacroItems.Items(MacroItems.MacroIDS.myQTH).Acquire = _
            Function()
                Return op.qth
            End Function
    End Sub

    Friend Sub SetCurrentRig(ByVal r As RigData.rig, ByVal id As Integer)
        If r Is Nothing Then
            Tracing.TraceLine("SetCurrentRig:no rig", TraceLevel.Error)
        Else
            Tracing.TraceLine("SetCurrentRig(" & r.ModelName & "," & id.ToString & ")", TraceLevel.Info)
        End If
        KeepCheckingOpen = False ' stop any continuous open trials.
        ' Close the current RigControl if needed.
        If (RigControl IsNot Nothing) AndAlso RigControl.IsOpen Then
            Form1.CloseTheRadio()
        End If

        CurrentRig = r
        CurrentRigID = id
        If CurrentRig IsNot Nothing Then
            RigControl = Radios.RadioSelection.GetRig(CurrentRig.model)
        Else
            RigControl = Nothing
        End If

        If RigControl Is Nothing Then
            CurrentRig = Nothing
            Return
        End If

        ' setup screen boxes
        Form1.setupBoxes()

        If RadioPort IsNot Nothing Then
            ' a port was already open.
            RadioPort.Close()
            Form1.openTheRadio()
        Else
            ' The radio is opened in form1.
        End If
    End Sub

    ' FlexControl knob stuff
    Private knobThread As Thread = Nothing
    Friend Sub SetupKnob()
        knobThread = New Thread(AddressOf knobThreadProc)
        knobThread.Name = "knob thread"
        knobThread.Start()
    End Sub

    Friend Sub StopKnob()
        If knobThread IsNot Nothing Then
            knobThread.Interrupt()
            Try
                If knobThread.IsAlive Then
                    knobThread.Join()
                End If
            Catch ex As Exception
                ' ignore
            End Try
        End If
    End Sub

    Private Sub knobThreadProc()
        Try
            ' setup the knob and let it run
            Knob = New FlexKnob
            Thread.Sleep(Timeout.Infinite)
        Catch ex As ThreadInterruptedException
            ' done with the knob
            If Knob IsNot Nothing Then
                Knob.Dispose()
                Knob = Nothing
            End If
        End Try
    End Sub

    Friend Sub ConfigContactLog()
#If 0 Then
        Logs.NewLog(CurrentOp.HamqthID, CurrentOp.HamqthPassword)
#End If
        Logs.NewLog(HamqthLookupID, HamqthLookupPassword)
        ContactLog = New LogClass(CurrentOp.LogFile)
        logSetup()
        ' Setup macros
        MacroItems.Items(MacroItems.MacroIDS.callSign).Acquire =
            Function()
                Return LogEntry.getFieldTextValue(AdifTags.ADIF_Call)
            End Function
        MacroItems.Items(MacroItems.MacroIDS.name).Acquire =
            Function()
                Return LogEntry.getFieldTextValue(AdifTags.ADIF_Name)
            End Function
        MacroItems.Items(MacroItems.MacroIDS.QTH).Acquire =
            Function()
                Return LogEntry.getFieldTextValue(AdifTags.ADIF_QTH)
            End Function
        MacroItems.Items(MacroItems.MacroIDS.myRST).Acquire =
            Function()
                Return LogEntry.getFieldTextValue(AdifTags.ADIF_MyRST)
            End Function
        MacroItems.Items(MacroItems.MacroIDS.RST).Acquire =
            Function()
                Return LogEntry.getFieldTextValue(AdifTags.ADIF_HisRST)
            End Function
        MacroItems.Items(MacroItems.MacroIDS.mySerial).Acquire =
            Function()
                Return LogEntry.getFieldTextValue(AdifTags.ADIF_SentSerial)
            End Function
    End Sub

    ''' <summary>
    ''' start dup checking
    ''' </summary>
    Private Sub logSetup()
        ' Can't do this if there's no log.
        Form1.StatusBox.Write(Form1.statLogID, " ")
        If (ContactLog.Name = vbNullString) Or (Not File.Exists(ContactLog.Name)) Then
            Return
        End If
        Dim session = New LogSession(ContactLog)
        If Not session.Start() Then
            Tracing.TraceLine("startDupChecking couldn't start session", TraceLevel.Error)
            Return
        End If
        Form1.StatusBox.Write(Form1.statLogID, LogCharacteristics.TrimmedFilename(ContactLog.Name, 20))
        ' Set the keys from the log form.
        Dim defs = New Collection(Of KeyCommands.KeyDefType)
        For Each fld As LogField In session.FormData.Fields.Values
            If fld.KeyName <> vbNullString Then
                ' First use the name to get the id.
                fld.KeyID = KeyCommands.getKeyFromTypename(fld.KeyName)
                ' Get the entry to set in my keyTable.
                Dim t As KeyCommands.keyTbl = Commands.lookup(CType(fld.KeyID, KeyCommands.CommandValues))
                If (t IsNot Nothing) Then
                    defs.Add(t.key)
                End If
            End If
        Next
        ' Add any keys for use when logging.
        For Each ktbl As KeyCommands.keyTbl In Commands.KeyTable
            If ktbl.UseWhenLogging Then
                defs.Add(ktbl.key)
            End If
        Next
        Commands.SetValues(defs.ToArray, KeyCommands.KeyTypes.log, False)

        ' Setup dup checking and other log calculations and fixup.
        Dim dupCheck As LogDupChecking.DupTypes
        dupCheck = CType(CInt(session.GetHeaderFieldText(AdifTags.HDR_DupCheck)), LogDupChecking.DupTypes)
        Dups = Nothing
        Tracing.TraceLine("startDupChecking:" & dupCheck.ToString, TraceLevel.Info)
        If dupCheck <> LogDupChecking.DupTypes.none Then
            Dups = New LogDupChecking(dupCheck)
        End If

        Dim countriesdb As CountriesDB = Nothing
        ' For each log record...
        While (Not session.EOF) AndAlso session.NextRecord()
            Dim needUpdate As Boolean = False ' set if need to update the record.
            If session.NeedFrequencyFix Then
                ' Fix bogus frequencies.
                Dim item As LogFieldElement
                item = session.getField(AdifTags.ADIF_RXFreq, False, session.FieldDictionary)
                If (item IsNot Nothing) AndAlso (item.Data <> vbNullString) Then
                    item.Data = fixFreq(item.Data)
                End If
                item = session.getField(AdifTags.ADIF_TXFreq, False, session.FieldDictionary)
                If (item IsNot Nothing) AndAlso (item.Data <> vbNullString) Then
                    item.Data = fixFreq(item.Data)
                End If
                needUpdate = True
            End If

            ' maintain dup checking
            If dupCheck <> LogDupChecking.DupTypes.none Then
                Dim key As New LogDupChecking.keyElement(session, DupType)
                Dups.AddToDictionary(key)
            End If

            ' See if need to update the DXCC info.
            If session.FormData.NeedCountryInfo Then
                Dim callItem As LogFieldElement = session.getField(AdifTags.ADIF_Call, False, session.FieldDictionary)
                If (callItem IsNot Nothing) AndAlso (callItem.Data <> vbNullString) Then
                    Dim dxccItem As LogFieldElement = session.getField(AdifTags.ADIF_DXCC, False, session.FieldDictionary)
                    If (dxccItem IsNot Nothing) AndAlso (dxccItem.Data = vbNullString) Then
                        ' no DXCC info.
                        If countriesdb Is Nothing Then
                            countriesdb = New CountriesDB
                        End If
                        Dim rec As Record = countriesdb.LookupByCall(callItem.Data)
                        If rec IsNot Nothing Then
                            dxccItem.Data = rec.CountryID
                            needUpdate = True
                        End If
                    End If
                End If
            End If

            ' Perform any housekeeping such as score calculation.
            If session.FormData.WriteEntry IsNot Nothing Then
                session.FormData.WriteEntry(session.FieldDictionary, Nothing)
            End If

            If needUpdate Then
                session.Update()
            End If
        End While

        session.EndSession()
    End Sub
    ' Remove consequtive periods
    Private Function fixFreq(inFreq As String) As String
        Dim rv As String = vbNullString
        Dim wasPeriod As Boolean = False
        For i As Integer = 0 To inFreq.Length - 1
            If inFreq(i) = "." Then
                If wasPeriod Then
                    Continue For
                Else
                    wasPeriod = True
                End If
            Else
                wasPeriod = False
            End If
            rv &= inFreq(i)
        Next
        Return rv
    End Function

    Friend Sub ConfigW2(suppressDialog As Boolean)
        Tracing.TraceLine("ConfigW2:" & suppressDialog, TraceLevel.Info)
        If W2WattMeter IsNot Nothing Then
            W2WattMeter.Dispose()
        End If
        W2WattMeter = New W2(W2ConfigFile)
        If suppressDialog Then
            ' Called from GetConfigInfo().
            ' Only setup if already configured.
            If W2WattMeter.IsConfigured Then
                W2WattMeter.Setup() ' no config dialogue
            End If
        Else
            ' User wants to configure.
            W2WattMeter.Setup(True)
        End If
    End Sub
    ''' <summary>
    ''' Configure W2 wattmeter.
    ''' </summary>
    Friend Sub ConfigW2()
        ConfigW2(False)
    End Sub

    ''' <summary>
    ''' Validatte a path or file name
    ''' </summary>
    ''' <param name="name"></param>
    ''' <returns>true if good</returns>
    Friend Function IsValidFileNameOrPath(ByVal name As String) As Boolean
        ' Determines if the name is empty or all white space.
        If (name = vbNullString) OrElse (name.Trim = vbNullString) Then
            Return False
        End If

        ' Determines if there are bad characters in the name. 
        For Each badChar As Char In System.IO.Path.GetInvalidPathChars
            If InStr(name, badChar) > 0 Then
                Return False
            End If
        Next

        ' The name passes basic validation. 
        Return True
    End Function

    Friend Function NotOnFlex() As Boolean
        Dim rv As Boolean = False
        If (CurrentRig IsNot Nothing) AndAlso CurrentRig.FlexLike Then
            Tracing.TraceLine("NotOnFlex", TraceLevel.Error)
            MsgBox(NotSupportedForThisRig)
            rv = True
        End If
        Return rv
    End Function
#End Region

    ' region - Scan stuff
#Region "scan"
    Friend SavedScans As SavedScanData
    Friend Enum scans
        none
        linear
        memory
    End Enum
    Friend scanstate As scans = scans.none
    Friend ReadOnly Property ScanInProcess As Boolean
        Get
            Return Form1.ScanTmr.Enabled
        End Get
    End Property
    Friend MemoryGroupControl As MemoryGroup
    Friend speechStatus, autoModeStatus As Boolean
    Friend modeStatus As AllRadios.ModeValue
    ' Note the scan timer and statusline are defined in Form1.
    Friend ReadOnly Property scanTimer
        Get
            Return Form1.ScanTmr
        End Get
    End Property
    Friend ReadOnly Property StatusBox As RadioBoxes.MainBox
        Get
            Return Form1.StatusBox
        End Get
    End Property
    Friend ReadOnly Property StatScanID As Integer
        Get
            Return Form1.statScanID
        End Get
    End Property
#End Region

    Friend Const MHZSIZE As Integer = 5
    Friend Const KHZSIZE As Integer = 6
    Friend Const FREQSIZE As Integer = MHZSIZE + KHZSIZE
    Friend Const SMETERSIZE As Integer = 4
    Friend Const RITOFFSETSIZE As Integer = 4 ' 4 digits

    Private Function iFormatFreq(ByVal str As String) As String
        ' Format the frequency for display.
        If str.Length <> FREQSIZE OrElse Not IsNumeric(str) Then
            Return Nothing
        End If
        Dim mhzi As Integer = CInt(str.Substring(0, MHZSIZE))
        ' note that CStr(mhzi) removes leading zeros.
        Dim khz As String = str.Substring(MHZSIZE, KHZSIZE)
        str = khz.Insert(3, ".")
        Return CStr(mhzi) & "." & str
    End Function
    ''' <summary>
    ''' (Overloaded) format the frequency for display
    ''' </summary>
    ''' <param name="IFText">Text from the IF command</param>
    ''' <returns>displayable frequency</returns>
    Friend Function FormatFreq(ByVal IFText As String) As String
        ' Format from "IF" data, or just a frequency.
        Dim freq, rit As String
        Dim vfo As String = ""
        Dim split As String = ""
        Dim i As Integer = 0
        Try
            freq = iFormatFreq(IFText.Substring(i, FREQSIZE))
        Catch ex As Exception
            Tracing.TraceLine("FormatFreq bogus string:" & IFText & " " & ex.Message, TraceLevel.Error)
            Return ""
        End Try
        ' Get RIT offset
        i += 16
        If i >= IFText.Length Then
            Return freq
        End If
        Try
            rit = IFText(i)
            If Not ((rit = " ") OrElse (rit = "+") OrElse (rit = "-")) Then
                ' bogus IF packet
                Return freq
            End If
            If rit = " " Then
                rit = "+"
            End If
            i += 1
            rit &= IFText.Substring(i, RITOFFSETSIZE)
            If ((IFText.Substring(i + RITOFFSETSIZE, 1) = "1") Or _
                (IFText.Substring(i + RITOFFSETSIZE + 1, 1) = "1")) Then
                ' RIT/XIT enabled
                freq &= rit
                If (IFText.Substring(i + RITOFFSETSIZE + 1, 1) = "1") Then
                    ' Xit
                    freq &= "x"
                End If
            End If
            i += RITOFFSETSIZE + 2 + 1
            ' Get the VFO.
            If IFText(i + 4) = "0" Then
                vfo = "A"
            Else
                vfo = "B"
            End If
            If (IFText.Substring(i + 6, 1) = "1") Then
                ' split
                split = "S"
            End If
        Catch ex As Exception
            ' Can happen if just the frequency is passed or the data is bogus.
            ' Return the frequency so far.
            Tracing.TraceLine("FormatFreq exception:" & IFText & " " & ex.Message, TraceLevel.Error)
        End Try
        ' Note that split and vfo are empty if not applicable.
        Return split & vfo & freq
    End Function
    ''' <summary>
    ''' (Overloaded) format the frequency for display
    ''' </summary>
    ''' <param name="freq">64-bit frequency</param>
    ''' <returns>displayable frequency</returns>
    Friend Function FormatFreq(ByVal freq As ULong) As String
        Return FormatFreqUlong(freq)
    End Function
    Friend Function FormatFreqUlong(ByVal freq As ULong) As String
        Dim rv As String = ""
        Dim str As String
        Try
            str = freq.ToString
        Catch ex As Exception
            Tracing.TraceLine("FormatFreq bogus value:" & ex.Message, TraceLevel.Error)
            Return rv ' will be ""
        End Try
        Dim len As Integer = str.Length
        If len > 6 Then
            rv = str.Substring(0, len - 6) & "."c & str.Substring(len - 6, 3) & _
                "."c & str.Substring(len - 3)
        ElseIf len > 3 Then
            rv = str.Substring(0, len - 3) & "."c & str.Substring(len - 3)
        Else
            rv = str
        End If
        Return rv
    End Function

    ''' <summary>
    ''' get numeric frequency string
    ''' </summary>
    ''' <param name="str">string containing frequency as mm.kkk.hhh </param>
    ''' <returns>int64 value</returns>
    Friend Function FreqInt64(ByVal str As String) As ULong
        Dim str2 As String = "0"
        For Each c As Char In str
            If IsNumeric(c) Then
                str2 &= c
            End If
        Next
        Return CLng(str2)
    End Function
    ''' <summary>
    ''' get numeric frequency string
    ''' </summary>
    ''' <param name="str">frequency string</param>
    ''' <returns>numeric frequency as a double </returns>
    Friend Function FreqDouble(ByVal str As String) As Double
        Dim str2 As String = ""
        Dim decSW As Boolean = False
        For Each c As Char In str
            If IsNumeric(c) Then
                str2 &= c
            ElseIf c = "."c Then
                If Not decSW Then
                    decSW = True
                    str2 &= c
                End If
            End If
        Next
        Return CDbl(str2)
    End Function

    Friend Function FormatSMeter(ByVal str As String) As String
        Return str
    End Function

    Friend Const DupEntryMsg As String = " is already on file."
    Friend Const BadFreqMSG As String = " must be of the form mhz.khz.hz, mhz.khz, or khz."
    Friend Function FormatFreqForRadio(ByVal str As String) As String
        ' Return 11-digit freq or nothing.
        Dim s() As String
        Dim st As String = ""
        Dim i As Integer = 0
        Dim err As Boolean = (str Is Nothing)
        If Not err Then
            Dim sep() As Char = {"."c}
            s = str.Split(sep, 3, StringSplitOptions.None)
            For Each st In s
                If st = "" OrElse Not IsNumeric(st) Then
                    err = True
                End If
                i += 1
            Next
            If (i = 3) AndAlso (s(2).IndexOf("."c) > -1) Then
                err = True
            End If
            If Not err Then
                ' They're all numeric.
                Select Case i
                    Case 1
                        ' just khz
                        st = s(0) & "000" ' hz = 0
                        If st.Length > FREQSIZE Then
                            err = True
                        End If
                    Case 2
                        ' khz, s(1), must be 1 to KHZSIZE (6) digits.  Pad to 3 if less.
                        st = s(1)
                        For i = st.Length + 1 To KHZSIZE
                            st &= "0"
                        Next
                        If st.Length > KHZSIZE OrElse s(0).Length > MHZSIZE Then
                            err = True
                        Else
                            st = st.Insert(0, s(0))
                        End If
                    Case 3
                        If s(0).Length > MHZSIZE OrElse s(1).Length <> 3 OrElse s(2).Length > 3 Then
                            err = True
                        Else
                            st = s(1) & s(2)
                            ' May need to expand this to KHZSIZE digits.
                            For i = st.Length + 1 To KHZSIZE
                                st &= "0"
                            Next
                            st = st.Insert(0, s(0))
                        End If
                End Select
                If Not err Then
                    ' pad with leading zeros.
                    For i = 1 To FREQSIZE - st.Length
                        st = st.Insert(0, "0")
                    Next
                End If
            End If
        End If
        If err Then
            Return Nothing
        Else
            Return st
        End If
    End Function
    ''' <summary>
    ''' convert a numeric frequency to a string for the radio
    ''' </summary>
    ''' <param name="intFreq">long integer frequency</param>
    ''' <returns>the number</returns>
    Friend Function FormatUlongFreqForRadio(ByVal intFreq As ULong) As String
        Dim str As String = CStr(intFreq)
        ' Needs to be 11 digits, pad on left with 0's.
        Dim pad As String = ""
        For i As Integer = str.Length To 11 - 1
            pad &= "0"
        Next
        Return pad & str
    End Function
    Friend Function UlongFreq(ByVal str As String) As ULong
        If str Is Nothing Then
            Return 0
        End If
        Dim rv As ULong
        Try
            rv = CULng(FormatFreqForRadio(str))
        Catch ex As Exception
            Tracing.TraceLine("ulongFreq error:" & str, TraceLevel.Error)
            rv = 0
        End Try
        Return rv
    End Function

    Friend Delegate Function awaitFuncDel() As Boolean
    Friend Function Await(func As awaitFuncDel, ms As Integer)
        Return Await(func, ms, 25)
    End Function
    Friend Function Await(func As awaitFuncDel, ms As Integer, waitMS As Integer)
        Dim iterations As Integer = ms / waitMS
        Dim rv As Boolean = func()
        While (Not rv) And (iterations > 0)
            Thread.Sleep(waitMS)
            iterations -= 1
            rv = func()
        End While
        Return rv
    End Function

    ''' <summary>
    ''' (overloaded) Return true if hostname is valid.
    ''' It may be host colon port.
    ''' </summary>
    ''' <param name="host">the entire hostname</param>
    ''' <param name="name">returned hostname</param>
    ''' <param name="port">returned integer port (default is 23)</param>
    ''' <returns>true on success</returns>
    Friend Function IsValidHostname(host As String, ByRef name As String, ByRef port As Integer) As Boolean
        Dim rv As Boolean = (host <> vbNullString)
        name = host
        port = 23 ' default to telnet port#
        If Not rv Then
            Return rv
        End If

        Dim id As Integer = host.IndexOf(":") + 1
        If id > 0 Then
            If ((id < 2) Or (id >= host.Length)) OrElse _
               Not System.Int32.TryParse(host.Substring(id), port) Then
                rv = False
            Else
                name = host.Substring(0, id - 1)
            End If
        End If
        Return rv
    End Function
    Friend Function IsValidHostname(host As String) As Boolean
        Dim dummy1 As String = vbNullString
        Dim dummy2 As Integer = 0
        Return IsValidHostname(host, dummy1, dummy2)
    End Function

    ''' <summary>
    ''' If the value isn't empty, set the field to it.
    ''' Select all the text in the fld.
    ''' </summary>
    ''' <param name="fld">the screen field</param>
    ''' <param name="val">the string value</param>
    Friend Sub SelectFieldText(ByVal fld As TextBox, ByVal val As String)
        If val <> vbNullString Then
            fld.Text = val
        End If
        fld.SelectionStart = 0
        fld.SelectionLength = fld.Text.Length
    End Sub

    ''' <summary>
    ''' get the descriptive string for this key
    ''' </summary>
    ''' <param name="k">the key</param>
    ''' <returns>the string</returns>
    ''' <remarks></remarks>
    Friend Function KeyString(ByVal k As Keys) As String
        Dim str As String
        Dim n As String = k.ToString
        Dim id As Integer = n.IndexOf(", ")
        If id > -1 Then
            ' Reformat the string.
            str = n.Substring(id + 2) & "-"
            str &= n.Substring(0, id)
        Else
            str = n
        End If
        Return str
    End Function

    ' Region remote audio
#Region "remote audio"
    Private Const audioDevicesBasename As String = "audioDevices.xml"
    Friend AudioDevicesFile As String
    Friend InputAudioDevice, OutputAudioDevice As JJPortaudio.Devices.Device

    Friend Sub GetNewAudioDevices()
        Dim dev = New JJPortaudio.Devices(AudioDevicesFile)
        dev.Setup()
        InputAudioDevice = dev.getNewDevice(JJPortaudio.Devices.DeviceTypes.input)
        OutputAudioDevice = dev.getNewDevice(JJPortaudio.Devices.DeviceTypes.output)
    End Sub

    Private _remoteLan As Boolean
    ''' <summary>
    ''' Remote audio for a local rig.
    ''' </summary>
    Friend Property remoteLan As Boolean
        Get
            Return _remoteLan
        End Get
        Set(value As Boolean)
            If RigControl Is Nothing Then
                Return
            End If
            If _remoteLan <> value Then
                RigControl.LANAudio = value
                _remoteLan = value
            End If
        End Set
    End Property
#End Region

    ' region - internal errors
#Region "internal errors"
    ''' <summary>
    ''' Show an internal error
    ''' </summary>
    ''' <param name="num">internal error number</param>
    Friend Sub ShowInternalError(num As Integer)
        Dim text As String = InternalError & num
        Tracing.TraceLine("InternalError error:" & text, TraceLevel.Error)
        MessageBox.Show(text, "JJRadio error", MessageBoxButtons.OK)
    End Sub

    ' Internal errors.
    Friend Const InternalError As String = "Internal error #"
    Friend Const MSReplace As Integer = 1 ' MemoryScan replace.
    Friend Const MSRemove As Integer = 2 ' MemoryScan remove
    Friend Const ScanReplace As Integer = 3 ' Scan replace.
    Friend Const ScanRemove As Integer = 4 ' Scan remove
    Friend Const MSReplaceAdd As Integer = 5 ' MemoryScan replace add.
    Friend Const ScanReplaceAdd As Integer = 6 ' Scan replace add.
    Friend Const MySplitERR As Integer = 7 ' LogEntry, escape at end of string
    Friend Const LogFldMismatch1 As Integer = 8 ' LogEntry.ShowEntries, field mismatch
    Friend Const LogFldMismatch2 As Integer = 9 ' LogEntry.Read, field mismatch
    Friend Const LogVersionErr As Integer = 10 ' bad data version.
    Friend Const ImportHangup As Integer = 11 ' Excessive looping in import().
    'Friend Const NoReadB4Update As Integer = 12
    Friend Const NoSession As Integer = 13 ' No log sessions are established.
    Friend Const MenuMalfunction As Integer = 14 ' the menu should be setup
    Friend Const LogHeaderVersionError As Integer = 15 ' bad log header version
    Friend Const BandProblem As Integer = 16 ' can't get a known band's data.
    Friend Const NoRigError As Integer = 17 ' no rig defined
    Friend Const DupValueError As Integer = 18 ' adding a duplicate CommandValue.
    Friend Const BadMessageIDError As Integer = 19 ' bad cw message id when sending.
    Friend Const DupNotFoundError As Integer = 20 ' dup key element not found.
    Friend Const SessionADIFNotFound As Integer = 21 ' required session.FieldDictionary item not found.
    Friend Const BadCommandID As Integer = 22 ' invalid CommandID.
#End Region
End Module
