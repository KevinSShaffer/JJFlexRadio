Friend Class DisplayBuilder
    ' The SMeter is kept at SMeterSize.
    Dim lastFreq As String
    Dim lastSMeter As String
    Dim lastVFO, lastSplit, lastVox, lastRIT, LastXIT, lastMode As String
    Dim nullFreq As String
    Dim nullSMeter As String
    Dim changeFlag As Boolean
    Friend Sub New()
        nullFreq = ""
        nullSMeter = ""
        For i As Integer = 0 To SMETERSIZE - 1
            nullSMeter &= "0"
        Next
        ClearFrequency()
        ClearSMeter()
    End Sub
    Friend ReadOnly Property text() As String
        Get
            Return lastSMeter & " " & lastSplit & lastVox & lastVFO & lastFreq & lastRIT & LastXIT & " " & lastMode
        End Get
    End Property

    ''' <summary>
    ''' Get offset to the numeric frequency (left side).
    ''' </summary>
    ''' <returns>integer offset</returns>
    Friend ReadOnly Property FreqLeft As Integer
        Get
            Return lastSMeter.Length + 1 + lastVFO.Length + lastSplit.Length + lastVox.Length
        End Get
    End Property
    ''' <summary>
    ''' Get the change flag, then reset it.
    ''' </summary>
    ''' <returns>True if changed</returns>
    Friend ReadOnly Property Changed() As Boolean
        Get
            Dim f As Boolean = changeFlag
            changeFlag = False
            Return f
        End Get
    End Property
    ''' <summary>
    ''' frequency
    ''' </summary>
    ''' <value>displayable frequency string</value>
    ''' <returns>displayable frequency string</returns>
    Friend Property Frequency As String
        Get
            Return lastFreq
        End Get
        Set(value As String)
            If value <> lastFreq Then
                lastFreq = value
                changeFlag = True
            End If
        End Set
    End Property
    ''' <summary>
    ''' SMeter
    ''' </summary>
    ''' <value>SMeter string, any length</value>
    ''' <returns>SMeter string</returns>
    Friend Property SMeter As String
        Get
            Return lastSMeter
        End Get
        Set(value As String)
            Dim i As Integer = value.Length - SMETERSIZE
            ' Ensure it's SMeterSize.
            If i > 0 Then
                ' too long.  remove leftmost chars.
                value = value.Substring(i)
            ElseIf i < 0 Then
                ' too short.  Pad on the left.
                value = value.Insert(0, nullSMeter.Substring(0, -i))
            End If
            If value <> lastSMeter Then
                lastSMeter = value
                changeFlag = True
            End If
        End Set
    End Property
    ''' <summary>
    ''' VFO
    ''' </summary>
    ''' <value>VFO string, may be numeric string</value>
    ''' <returns>VFO string</returns>
    Friend Property VFO As String
        Get
            Return lastVFO
        End Get
        Set(value As String)
            ' If it's a numeric value, it can be either "A" or "B" currently.
            If IsNumeric(value) Then
                Select Case value
                    Case "0"
                        value = "A"
                    Case "1"
                        value = "B"
                    Case Else
                        value = ""
                End Select
            End If
            If value <> lastVFO Then
                lastVFO = value
                changeFlag = True
            End If
        End Set
    End Property
    ''' <summary>
    ''' Split
    ''' </summary>
    ''' <value>displayable split string</value>
    ''' <returns>displayable split string</returns>
    Friend Property Split As String
        Get
            Return lastSplit
        End Get
        Set(value As String)
            If value <> lastSplit Then
                lastSplit = value
                changeFlag = True
            End If
        End Set
    End Property
    Friend Property Vox As String
        Get
            Return lastVox
        End Get
        Set(value As String)
            If value <> lastVox Then
                lastVox = value
                changeFlag = True
            End If
        End Set
    End Property
    ''' <summary>
    ''' RIT
    ''' </summary>
    ''' <value>displayable RIT string</value>
    ''' <returns>displayable RIT string</returns>
    Friend Property RIT As String
        Get
            Return lastRIT
        End Get
        Set(value As String)
            If value <> lastRIT Then
                lastRIT = value
                changeFlag = True
            End If
        End Set
    End Property
    ''' <summary>
    ''' XIT
    ''' </summary>
    ''' <value>displayable XIT</value>
    ''' <returns>Displayable XIT</returns>
    Friend Property XIT As String
        Get
            Return lastXIT
        End Get
        Set(value As String)
            If value <> lastXIT Then
                lastXIT = value
                changeFlag = True
            End If
        End Set
    End Property
    Friend Property Mode As String
        Get
            Return lastMode
        End Get
        Set(value As String)
            If lastMode <> value Then
                lastMode = value
                changeFlag = True
            End If
        End Set
    End Property

    ''' <summary>
    ''' Clear the frequency area, includes VFO, split, vox and RIT.
    ''' </summary>
    Friend Sub ClearFrequency()
        lastFreq = nullFreq
        lastVFO = ""
        lastSplit = ""
        lastVox = ""
        lastRIT = ""
        lastxit = ""
        changeFlag = True
    End Sub
    ''' <summary>
    ''' Clear the SMeter
    ''' </summary>
    Friend Sub ClearSMeter()
        lastSMeter = nullSMeter
        changeFlag = True
    End Sub
    ''' <summary>
    ''' Clear the main display
    ''' </summary>
    Friend Sub Clear()
        ClearFrequency()
        ClearSMeter()
    End Sub
End Class
