''' <summary>
''' Macro Items
''' </summary>
Public Class MacroItems
    ''' <summary> A macro item </summary>
    Public Class MacroItemType
        ''' <summary>
        ''' Function to get the data
        ''' </summary>
        ''' <returns>data string</returns>
        Public Delegate Function AcquireDel() As String
        ''' <summary> Acquire the data </summary>
        Public Acquire As AcquireDel
        ''' <summary> Macro ID character. </summary>
        Public MacroID As Char
        Public Sub New(c As Char)
            MacroID = c
        End Sub
    End Class
    ''' <summary> Macro ID enumeration </summary>
    Public Enum MacroIDS
        myCallSign
        callSign
        myName
        name
        myQTH
        QTH
        myRST
        RST
        mySerial
        serial
    End Enum
    Private Shared macroChars As Char() = _
    {"C", "c", "N", "n", "Q", "q", "R", "r", "S", "s"}
    ''' <summary> Array of available macros. </summary>
    Public Shared Items As MacroItemType() = _
    {New MacroItemType(macroChars(MacroIDS.myCallSign)), _
      New MacroItemType(macroChars(MacroIDS.callSign)), _
      New MacroItemType(macroChars(MacroIDS.myName)), _
      New MacroItemType(macroChars(MacroIDS.name)), _
      New MacroItemType(macroChars(MacroIDS.myQTH)), _
      New MacroItemType(macroChars(MacroIDS.QTH)), _
      New MacroItemType(macroChars(MacroIDS.myRST)), _
      New MacroItemType(macroChars(MacroIDS.RST)), _
      New MacroItemType(macroChars(MacroIDS.mySerial)), _
      New MacroItemType(macroChars(MacroIDS.serial)) _
    }

    ''' <summary>
    ''' Expand the string which may contain macros.
    ''' </summary>
    ''' <param name="str">the input string</param>
    ''' <returns>expanded string</returns>
    Public Shared Function Expand(str As String) As String
        Dim rv As String = ""
        Dim len As Integer = str.Length - 1
        For i As Integer = 0 To len
            Dim c As Char = str(i)
            Dim s As String = ""
            If i < len Then
                ' at least 1 character left.
                Select Case c
                    Case "\"
                        ' escape
                        i += 1
                        rv &= str(i)
                    Case "&"
                        i += 1
                        Dim j As Integer = Array.IndexOf(macroChars, str(i))
                        If j >= 0 Then
                            rv &= Items(j).Acquire()
                        End If
                    Case Else
                        rv &= c
                End Select
            Else
                rv &= c
            End If
        Next
        Return rv
    End Function
End Class
