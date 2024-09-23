Imports System.Diagnostics
Imports JJTrace

Public Class MemoryScan
    Public Sub StopScan()
        Tracing.TraceLine("Memory StopScan", TraceLevel.Info)
    End Sub

    Private Sub StartButton_Click(sender As System.Object, e As System.EventArgs) Handles StartButton.Click
        If Not (IsNumeric(SpeedBox.Text) AndAlso (SpeedBox.Text > "0") AndAlso (SpeedBox.Text <= "600")) Then
            MsgBox(SpeedError)
            dialogr()
        End If
    End Sub
End Class
