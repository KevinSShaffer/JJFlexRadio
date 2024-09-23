Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.Xml.Serialization

''' <summary> saved scans object </summary>
Public Class SavedScanData
    <XmlIgnore()> Public Shared ReadOnly Property pathName As String
        Get
            Return BaseConfigDir & "\" & "Scans.xml"
        End Get
    End Property
    ''' <summary> types, linear and memory </summary>
    Public Enum ScanTypes
        linear
        memory
    End Enum
    ''' <summary> object for a saved scan </summary>
    Public Class ScanData
        Public Type As ScanTypes
        Public name As String
        Public memList As String()
        Public speed As Integer

        Public Sub New()
            ' (overloaded) create with no data.
        End Sub
#If 0 Then
        ''' <summary> overloaded </summary>
        ''' <param name="nam">memory scan's name</param>
        ''' <param name="itms">memoryScan.scanMemories</param>
        ''' <param name="sp">integer speed</param>
        Public Sub New(ByVal nam As String, ByVal itms As MemoryScan.ScanMemories, ByVal sp As Integer)
            ' (overloaded) create from the memory scan checkedListBox.
            Type = SavedScanData.ScanTypes.memory
            name = nam
            ReDim memList(itms.ToArray.Length - 1)
            For i As Integer = 0 To itms.ToArray.Length - 1
                memList(i) = itms(i)
            Next
            speed = sp
        End Sub
#End If
        Enum linearFrequencies
            startf = 0
            endf = 1
            increment = 2
        End Enum
        ''' <summary> overloaded </summary>
        ''' <param name="nam">linear scan's name</param>
        ''' <param name="l">string - low value</param>
        ''' <param name="h">string - high value</param>
        ''' <param name="i">string - increment in khz</param>
        ''' <param name="sp">integer speed</param>
        Public Sub New(ByVal nam As String, ByVal l As String, ByVal h As String, ByVal i As String, ByVal sp As Integer)
            ' (overloaded) Create from a linear scan.
            Type = ScanTypes.linear
            name = nam
            ReDim memList(2)
            memList(linearFrequencies.startf) = l
            memList(linearFrequencies.endf) = h
            memList(linearFrequencies.increment) = i
            speed = sp
        End Sub

        Public Property StartFrequency() As String
            Get
                If Type = ScanTypes.linear Then
                    Return memList(linearFrequencies.startf)
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal value As String)
                If Type = ScanTypes.linear Then
                    memList(linearFrequencies.startf) = value
                End If
            End Set
        End Property
        Public Property EndFrequency() As String
            Get
                If Type = ScanTypes.linear Then
                    Return memList(linearFrequencies.endf)
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal value As String)
                If Type = ScanTypes.linear Then
                    memList(linearFrequencies.endf) = value
                End If
            End Set
        End Property
        Public Property Increment() As String
            Get
                If Type = ScanTypes.linear Then
                    Return memList(linearFrequencies.increment)
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal value As String)
                If Type = ScanTypes.linear Then
                    memList(linearFrequencies.increment) = value
                End If
            End Set
        End Property
    End Class
    ''' <summary> scan data </summary>
    Public dat As List(Of ScanData)
    Public Sub New()
        dat = New List(Of ScanData)
    End Sub
    Public ReadOnly Property Count() As Integer
        Get
            Return dat.Count
        End Get
    End Property
    Public Property Item(ByVal i As Integer) As ScanData
        Get
            Return dat(i)
        End Get
        Set(ByVal value As ScanData)
            dat(i) = value
        End Set
    End Property
    ''' <summary> Add a scan </summary>
    ''' <param name="val">scanData object to add</param>
    ''' <returns>true if added, false if duplicate</returns>
    Public Function Add(ByVal val As ScanData) As Boolean
        ' Keep items in alphabetic order by name.
        Dim i As Integer = 0
        For Each msd As ScanData In dat
            If msd.name > val.name Then
                Exit For
            ElseIf msd.name = val.name Then
                Return False
            End If
            i += 1
        Next
        dat.Insert(i, val)
        Return True
    End Function
    Public Function IndexOf(ByVal name As String) As Integer
        Dim id As Integer = -1
        If (name Is Nothing) OrElse (name = "") Then
            Return id
        End If
        For i As Integer = 0 To Count - 1
            If Item(i).name = name Then
                id = i
                Exit For
            End If
        Next
        ' Note that if count = 0, id is -1.
        Return id
    End Function
    ''' <summary> overloaded, remove </summary>
    ''' <param name="id">scan id</param>
    ''' <returns>true if removed</returns>
    Public Function Remove(ByVal id As Integer) As Boolean
        ' (overloaded)
        If (id >= 0) AndAlso (id < Count) Then
            dat.RemoveAt(id)
            Return True
        Else
            Return False
        End If
    End Function
    ''' <summary> overloaded, remove </summary>
    ''' <param name="name">scan's name</param>
    ''' <returns>true if removed</returns>
    Public Function Remove(ByVal name As String) As Boolean
        ' (overloaded)
        Return Remove(IndexOf(name))
    End Function
    ''' <summary> get saved scans' names </summary>
    ''' <returns>string array of names</returns>
    Public Function GetNames() As String()
        ' Return the scans' names.
        If Count = 0 Then
            Return Nothing
        End If
        Dim rStr(Count - 1) As String
        For i As Integer = 0 To Count - 1
            rStr(i) = Item(i).name
        Next
        Return rStr
    End Function
End Class
