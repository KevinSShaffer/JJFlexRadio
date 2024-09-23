Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports adif
Imports JJLogLib
Imports JJTrace

Public Class LogDupChecking
    ''' <summary>
    ''' duplicate types
    ''' </summary>
    Public Enum DupTypes
        none
        justCall
        callAndBand
        callAndBandAndMode
    End Enum

    Public Class keyElement
        Public Value As String
        Public ID As String
        Public Sub New(session As LogSession, typ As DupTypes)
            ' Set the dictionary key according to the type.
            Select Case typ
                Case DupTypes.justCall
                    Value = session.GetFieldText(AdifTags.ADIF_Call)
                Case DupTypes.callAndBand
                    Value = session.GetFieldText(AdifTags.ADIF_Call) &
                        session.GetFieldText(AdifTags.ADIF_Band)
                Case DupTypes.callAndBandAndMode
                    Value = session.GetFieldText(AdifTags.ADIF_Call) &
                        session.GetFieldText(AdifTags.ADIF_Band) &
                        session.GetFieldText(AdifTags.ADIF_Mode)
            End Select
            Value = Value.ToLower
            ID = New String(session.GetFieldText(AdifTags.ADIF_SentSerial))
        End Sub
    End Class

    Private Class dupCheckElement
        Public Key As String
        Public count As Integer
        Public recordIDs As List(Of Integer)
        Public Sub New()
            recordIDs = New List(Of Integer)
        End Sub
        Public Sub New(k As keyElement)
            count = 1
            Key = k.Value
            recordIDs = New List(Of Integer)
            Try
                Dim i As Integer = k.ID
                recordIDs.Add(i)
            Catch ex As Exception
                Tracing.TraceLine("new DupCheckElement exception:" & k.ID & " " & ex.Message, TraceLevel.Error)
            End Try
        End Sub
    End Class

    Private dups As Dictionary(Of String, dupCheckElement)

    ''' <summary>
    ''' dup checking type
    ''' </summary>
    Public dupType As DupTypes

    ''' <summary>
    ''' new dup checking
    ''' </summary>
    ''' <param name="typ">the dup type</param>
    Public Sub New(typ As DupTypes)
        Tracing.TraceLine("LogDupChecking:" & typ.ToString, TraceLevel.Info)
        dups = New Dictionary(Of String, dupCheckElement)
        dupType = typ
    End Sub

    Public Sub AddToDictionary(key As keyElement)
        Dim el As dupCheckElement = Nothing
        If dups.TryGetValue(key.Value, el) Then
            Dim i As Integer = key.ID
            ' only add if unique.
            If el.recordIDs.IndexOf(i) = -1 Then
                el.count += 1
                el.recordIDs.Add(i)
            End If
        Else
            el = New dupCheckElement(key)
            dups.Add(key.Value, el)
        End If
    End Sub

    Public Sub RemoveFromDictionary(key As keyElement)
        Dim el As dupCheckElement = Nothing
        If dups.TryGetValue(key.Value, el) Then
            Dim i As Integer = el.recordIDs.IndexOf(key.ID)
            If i <> -1 Then
                If el.count = 1 Then
                    dups.Remove(key.Value)
                Else
                    el.count -= 1
                    el.recordIDs.RemoveAt(i)
                End If
            End If
        End If
    End Sub

    Public Function DupTest(key As keyElement) As Integer
        Dim el As dupCheckElement = Nothing
        Dim rv As Integer = 0
        If dups.TryGetValue(key.Value, el) Then
            rv = el.count
        End If
        Return rv
    End Function
End Class
