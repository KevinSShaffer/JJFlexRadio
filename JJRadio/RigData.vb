Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.IO
Imports System.IO.Ports
Imports System.IO.Directory
Imports System.Xml.Serialization
Imports Radios
Imports JJTrace

Public Class RigData
    Const subDir As String = "Rigs"
    Private Shared rigsDir As String
    Public Class rig
        Public fileName As String
        <XmlIgnore()> Public ReadOnly Property pathName As String
            Get
                Return rigsDir & "\" & fileName
            End Get
        End Property
        Public Name As String
        Public model As Integer
        Public ReadOnly Property ModelName As String
            Get
                For Each ld As listData In RigData.Models
                    If model = ld.id Then
                        Return ld.Name
                    End If
                Next
                Return ""
            End Get
        End Property
        ''' <summary> display value for a list </summary>
        ''' <returns>string to display</returns>
        <XmlIgnore()> Public ReadOnly Property Display As String
            Get
                Return Name & " - " & ModelName
            End Get
        End Property
        Public DefaultFlag As Boolean
        Public DiscoveredInfo As Radios.AllRadios.RadioDiscoveredEventArgs
        <XmlIgnore()> Public ReadOnly Property FlexLike As Boolean
            Get
                Return (DiscoveredInfo IsNot Nothing)
            End Get
        End Property
        Public baudRate As Integer
        Public dataBits As Integer
        Public parity As Parity
        Public PortName As String
        Public stopBits As Integer
        Public handShake As Handshake
        Public InhibitRemote As Boolean
        ''' <summary>
        ''' Allow remote login/access via internet
        ''' </summary>
        <XmlIgnore> Public Property AllowRemote As Boolean
            Get
                Return Not InhibitRemote
            End Get
            Set(value As Boolean)
                InhibitRemote = Not value
            End Set
        End Property
        ''' <summary>
        ''' new rig instance
        ''' </summary>
        Public Sub New()
        End Sub
        ''' <summary>
        ''' new rig instance
        ''' </summary>
        ''' <param name="defaults">is a radioSelection.ComDefaults item (can be Nothing)</param>
        Friend Sub New(defaults As Radios.RadioSelection.ComDefaults)
            If defaults IsNot Nothing Then
                baudRate = defaults.Baud
                parity = defaults.Parity
                dataBits = defaults.DataBits
                stopBits = defaults.StopBits
                handShake = defaults.Handshake
            End If
        End Sub
    End Class
    Public Class listData
        Dim myName As String
        Dim myid As Integer
        Public Sub New(ByVal n As String, ByVal i As Integer)
            myName = n
            myid = i
        End Sub
        Public ReadOnly Property Name As String
            Get
                Return myName
            End Get
        End Property
        Public ReadOnly Property id As Integer
            Get
                Return myid
            End Get
        End Property
    End Class
    Public Shared Models(RadioSelection.RigTable.Length - 1) As listData
    Public Shared BaudRates As listData() = _
        {New listData("2400", 2400), New listData("4800", 4800), _
         New listData("9600", 9600), New listData("19200", 19200), _
         New listData("38400", 38400), New listData("57600", 57600), _
         New listData("115200", 115200)}
    Public Shared Parities As listData() = _
        {New listData("None", Parity.None), New listData("Odd", Parity.Odd), _
         New listData("Even", Parity.Even), _
         New listData("Mark", Parity.Mark), _
         New listData("Space", Parity.Space)}
    Public Shared DataBits As listData() = _
        {New listData("7", 7), New listData("8", 8)}
    Public Shared StopBitses As listData() = _
        {New listData("None", StopBits.None), New listData("1", StopBits.One), _
         New listData("2", StopBits.Two), _
         New listData("1.5", StopBits.OnePointFive)}
    Public Shared HandShakes As listData() = _
        {New listData("None", Handshake.None), _
         New listData("Xon/Xoff", Handshake.XOnXOff), _
         New listData("HW-flow-control", Handshake.RequestToSend), _
         New listData("RTS-Xon/Xoff", Handshake.RequestToSendXOnXOff)}

    Dim dfltID As Integer
    Public Property DefaultID As Integer
        Get
            Return dfltID
        End Get
        Set(ByVal value As Integer)
            ' Reset any prior default.
            If dfltID <> -1 Then
                rigs(dfltID).DefaultFlag = False
                write(rigs(dfltID))
            End If
            dfltID = value
        End Set
    End Property
    ''' <summary>
    ''' The default rig
    ''' </summary>
    ''' <returns>rig object</returns>
    Public ReadOnly Property TheDefault As rig
        Get
            If dfltID = -1 Then
                Return Nothing
            Else
                Return rigs(dfltID)
            End If
        End Get
    End Property
    Public rigs As List(Of rig)
    Default Public ReadOnly Property Items(ByVal id As Integer) As rig
        Get
            Return rigs(id)
        End Get
    End Property
    Public ReadOnly Property Length As Integer
        Get
            Return rigs.Count
        End Get
    End Property

    Private Sub setupModels()
        For i As Integer = 0 To Models.Length - 1
            Models(i) = New listData(RadioSelection.RigTable(i).name, _
                                     RadioSelection.RigTable(i).id)
        Next
    End Sub

    Public Sub New()
        ' (overloaded)
        Tracing.TraceLine("id:new rigdata()", TraceLevel.Info)
        setupModels()
        dfltID = -1
        rigs = New List(Of rig)
    End Sub

    Public Sub New(ByVal baseDir As String)
        ' (overloaded) get configured rigs.
        ' Setup the Models list if needed.
        Tracing.TraceLine("id:new rigdata " & baseDir, TraceLevel.Info)
        setupModels()
        dfltID = -1
        rigsDir = baseDir & "\" & subDir
        rigs = New List(Of rig)
        If Not Exists(rigsDir) Then
            Try
                CreateDirectory(rigsDir)
            Catch ex As Exception
                Tracing.ErrMessageTrace(ex)
                Exit Sub
            End Try
        End If
        Dim rigFiles As String() = GetFiles(rigsDir, "*.xml")
        If rigFiles.Length = 0 Then
            ' Get initial rigs.
            Add()
        Else
            ' Get configured rigs.
            For Each fn As String In rigFiles
                Dim cfgFile As Stream
                Try
                    cfgFile = File.Open(fn, FileMode.Open)
                Catch ex As Exception
                    Tracing.ErrMessageTrace(ex)
                    Exit Sub
                End Try
                Try
                    Dim xs As New XmlSerializer(GetType(rig))
                    Dim r As rig
                    r = xs.Deserialize(cfgFile)
                    rigs.Add(r)
                    If r.DefaultFlag Then
                        DefaultID = rigs.Count - 1
                    End If
                Catch ex As Exception
                    Tracing.ErrMessageTrace(ex)
                    cfgFile.Close()
                    Exit Sub
                End Try
                cfgFile.Close()
            Next
        End If
    End Sub
    ''' <summary>
    ''' Add new rigs
    ''' </summary>
    ''' <returns>id of first rig item added or -1</returns>
    Public Function Add() As Integer
        RigInfo.RigList = rigs ' existing rigs if any
        Dim nExisting As Integer = rigs.Count
        RigInfo.theRig = Nothing
        Dim rv As Integer = -1
        If RigInfo.ShowDialog = DialogResult.OK Then
            rv = nExisting ' first added
            For i As Integer = nExisting To RigInfo.RigList.Count - 1
                rigs(i).fileName = rigs(i).Name & ".xml"
                If rigs(i).DefaultFlag Then
                    DefaultID = i
                End If
                write(rigs(i))
            Next
        End If
        Return rv
    End Function

    Public Function Update(ByVal id As Integer) As Boolean
        RigInfo.RigList = rigs ' existing rigs
        RigInfo.theRig = rigs(id)
        Dim rv As Boolean = (RigInfo.ShowDialog = DialogResult.OK)
        If rv Then
            Dim theRig As rig = RigInfo.theRig
            ' Note theRig won't be the same as the original rigInfo.theRig.
            theRig.fileName = theRig.Name & ".xml"
            ' See if the filename has changed.
            If rigs(id).fileName <> theRig.fileName Then
                File.Delete(rigs(id).pathName)
            End If
            rigs.RemoveAt(id)
            rigs.Insert(id, theRig)
            ' Reset the default if changed.
            ' Note we don't allow the default flag to be turned off.
            ' we only allow for this one becoming the default.
            If (dfltID <> id) And theRig.DefaultFlag Then
                DefaultID = id
            End If
            write(theRig)
            ' Reconfigure if changed the current rig.
            If id = CurrentRigID Then
                SetCurrentRig(rigs(id), id)
            End If
        End If
        Return rv
    End Function

    Public Function RemoveAt(ByVal id As Integer) As Boolean
        File.Delete(rigs(id).pathName)
        rigs.RemoveAt(id)
        If rigs.Count = 0 Then
            dfltID = -1
        ElseIf dfltID = id Then
            ' We removed the default; use the first one.
            dfltID = 0
            rigs(0).DefaultFlag = True
            write(rigs(0))
        ElseIf dfltID > id Then
            ' Allow for the deletion.
            dfltID = dfltID - 1
        End If
        Return True
    End Function

    Private Sub write(ByVal r As rig)
        Dim rigFile As Stream
        Try
            rigFile = File.Open(r.pathName, FileMode.Create)
        Catch ex As Exception
            Tracing.ErrMessageTrace(ex)
            Exit Sub
        End Try
        Try
            Dim xs As New XmlSerializer(GetType(rig))
            xs.Serialize(rigFile, r)
        Catch ex As Exception
            Tracing.ErrMessageTrace(ex)
            rigFile.Close()
        End Try
        rigFile.Close()
    End Sub
End Class
