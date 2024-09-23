Imports Radios
Imports SimIO

Partial Class Form1
    ' The simulator is lazily loaded so it doesn't have to ship.
    Dim lazySim As Lazy(Of SimulatorInterface)
    Dim WithEvents theSimulator As SimulatorInterface
    Dim simOpenParms As AllRadios.OpenParms

    Private Sub setupSimulator()
        lazySim = New Lazy(Of SimulatorInterface)
        theSimulator = lazySim.Value
        AddHandler theSimulator.DataReadyEvent, AddressOf simDataReceived
        AddHandler theSimulator.BrokenPipe, AddressOf simBrokenPipe
        simOpenParms = New AllRadios.OpenParms
        simOpenParms.SendRoutine = AddressOf simSend
        simOpenParms.DirectDataReceiver = AddressOf Commands.HandleDirect

        ' Tell the simulator which rig we're using.
        If CurrentRig IsNot Nothing Then
            simSend("RIG" & CurrentRig.model.ToString & "$")
        End If
    End Sub

    Private Sub simDataReceived(sender As Object, e As SimulatorInterface.DataReadyEventArg)
        Dim str As String = theSimulator.Read
        Try
            RigControl.InterruptHandler(str)
        Catch ex As Exception
            ' Ignore it.
        End Try
    End Sub

    Private Function simSend(str As String)
        theSimulator.Write(str)
        Return True
    End Function

    Private Sub simBrokenPipe(sender As Object, e As SimulatorInterface.DataReadyEventArg)
        MsgBox("Broken simulator pipe")
        theSimulator.Stop()
        End
    End Sub

    Private Sub simClose()
        theSimulator.Stop()
    End Sub
End Class
