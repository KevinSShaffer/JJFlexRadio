Imports System.Diagnostics
Imports System.IO
Imports JJLogLib
Imports JJTrace

Friend Class LogStats
    Private Const noLog As String = "There is no active log file."

    Friend Function ShowLogStats() As Boolean
        If (ContactLog Is Nothing) OrElse (ContactLog.Name = vbNullString) OrElse (Not File.Exists(ContactLog.Name)) Then
            MsgBox(noLog)
            Return False
        End If
        Dim session = New LogSession(ContactLog)
        If Not session.Start() Then
            Tracing.TraceLine("ShowLogStats couldn't start session", TraceLevel.Error)
            Return False
        End If

        If (session.ShowStats IsNot Nothing) Then
            session.ShowStats()
        End If

        session.EndSession()
        Return True
    End Function
End Class
