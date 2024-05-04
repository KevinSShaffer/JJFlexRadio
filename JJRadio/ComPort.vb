Imports System.Diagnostics
Imports System.IO
Imports System.IO.Ports
Imports System.Threading
Imports System.Threading.Timer
Imports JJTrace

Friend Class ComPort
    Shared WithEvents Radio As SerialPort
    Dim useCTSHolding As Boolean
    Private serialWriteTimeout As Integer = 500 ' .5 second write timeout

    ' Prompt the user by default.
    Friend Property Interactive As Boolean

    Friend ReadOnly Property IsOpen As Boolean
        Get
            Return (Radio IsNot Nothing) AndAlso Radio.IsOpen
        End Get
    End Property

    Friend Sub New()
        Interactive = True ' prompt if port not available
    End Sub

    ''' <summary>
    ''' Open the serial port.
    ''' </summary>
    ''' <returns>
    ''' abort - user wants to abort
    ''' ignore - ignore the error and keep trying the open.
    ''' ok - success
    ''' </returns>
    Function Open() As DialogResult
        Tracing.TraceLine("ComPort open", TraceLevel.Info)
        If CurrentRig Is Nothing Then
            Return DialogResult.OK
        End If
tryOpen:
        Radio = New SerialPort
        Try
            Radio.BaudRate = CurrentRig.baudRate
            Radio.DataBits = CurrentRig.dataBits
            Radio.Handshake = CurrentRig.handShake
            Radio.Parity = CurrentRig.parity
            Radio.PortName = CurrentRig.PortName
            Radio.StopBits = CurrentRig.stopBits
            Radio.Encoding = System.Text.Encoding.UTF8
            Tracing.TraceLine("comportOpen:" & CurrentRig.PortName & " " & _
                CurrentRig.baudRate.ToString & " " & _
                CurrentRig.dataBits.ToString & " " & _
                CurrentRig.parity.ToString & " " & _
                CurrentRig.stopBits.ToString & " " & _
                CurrentRig.handShake.ToString)
            Radio.WriteTimeout = serialWriteTimeout
            Radio.Open()
            ' Check CTSHolding if HW handshaking and CTSHolding is set.
            useCTSHolding = ((Radio.Handshake = Handshake.RequestToSend) And _
                             Radio.CtsHolding)
            ' Now establish interrupt handler.
            ReDim readBytes(Radio.ReadBufferSize) ' See ComDataReceived
            AddHandler Radio.DataReceived, AddressOf ComDataReceived
        Catch ex As Exception
            If Radio IsNot Nothing Then
                Radio.Dispose()
                Radio = Nothing
            End If
            If Not Interactive Then
                ' Open retry, don't report this.
                Tracing.TraceLine("ComPort open:" & ex.Message, TraceLevel.Info)
                Return DialogResult.Ignore
            End If
            Dim result As DialogResult = MessageBox.Show("Port " & CurrentRig.PortName & " didn't open." & vbCrLf & ex.Message, "Port Open Error", MessageBoxButtons.AbortRetryIgnore)
            Tracing.TraceLine("ComPort open:" & result.ToString & " " & ex.Message, TraceLevel.Error)
            If result = DialogResult.Retry Then
                GoTo tryOpen
            Else
                ' return abort or ignore.
                Return result
            End If
        End Try
        Return DialogResult.OK
    End Function

    Sub Close()
        Tracing.TraceLine("comPort close", TraceLevel.Info)
        If (Radio Is Nothing) OrElse (Not Radio.IsOpen) Then
            Tracing.TraceLine("comPort close:wasn't open", TraceLevel.Error)
            Return
        End If
        Dim closeThread = New Thread(AddressOf closeProc)
        closeThread.Start()
        Tracing.TraceLine("close:waiting for closeThread", TraceLevel.Info)
        Dim sanity As Integer = 40
        While closeThread.IsAlive And (sanity > 0)
            sanity -= 1
            Thread.Sleep(25)
        End While
        If sanity = 0 Then
            Tracing.TraceLine("The close didn't terminate", TraceLevel.Error)
#If 0 Then
            Try
                Tracing.TraceLine("close:aborting close thread", TraceLevel.Error)
                closeThread.Abort()
                Tracing.TraceLine("close:closeThread aborted", TraceLevel.Error)
            Catch ex As Exception
                Tracing.TraceLine("close:" & ex.Message, TraceLevel.Error)
            End Try
#End If
        Else
            Tracing.TraceLine("close:closeThread done", TraceLevel.Info)
        End If
    End Sub
    Private Sub closeProc()
        Try
            Tracing.TraceLine("comPort close:discarding buffers", TraceLevel.Error)
            Radio.DiscardInBuffer()
            Radio.DiscardOutBuffer()
            Tracing.TraceLine("comPort close:closing the port", TraceLevel.Error)
            Radio.Dispose()
            Tracing.TraceLine("  port closed", TraceLevel.Error)
        Catch ex As Exception
            Tracing.TraceLine("closeThread:" & ex.Message, TraceLevel.Error)
        End Try
    End Sub

    Private Sub FlushRadioOutput()
        Tracing.TraceLine("ComPort:FlushRadioOutput", TraceLevel.Info)
        ' Set the flush flag and wait at least a second for interrupts to drain.
        ' blow out after 3 seconds.
        Dim sanity As Integer = 3
        Thread.VolatileWrite(flushFlag, True)
        Do
            Thread.Sleep(1000)
            sanity -= 1
        Loop While Radio.BytesToRead And (sanity > 0)
        Thread.VolatileWrite(flushFlag, False)
    End Sub

    'Dim CTSHoldingOffCount As UInteger
    Function send(ByVal str As String) As Boolean
        Tracing.TraceLine("Send:" & str, TraceLevel.Info)
        Dim rv As Boolean
        Dim len As Integer = str.Length
        Try
            If (Not useCTSHolding) OrElse Radio.CtsHolding Then
                'CTSHoldingOffCount = 0
                Radio.Write(str)
                rv = True
            Else
                'CTSHoldingOffCount += 1
                'Tracing.TraceLine("Send:CTSHoldingOffCount = " & CTSHoldingOffCount.ToString)
                rv = False
            End If
        Catch exto As TimeoutException
            'MsgBox(exto.Message)
            Tracing.TraceLine("comPort send:time out", TraceLevel.Error)
            rv = False
        Catch ex As Exception
            Tracing.TraceLine("comPort send exception:" & ex.Message, TraceLevel.Error)
            rv = False
        End Try
        Return rv
    End Function

    Function sendBytes(ByVal bytes As Byte()) As Boolean
        Tracing.TraceLine("SendBytes:" & Escapes.Escapes.Decode(bytes), TraceLevel.Info)
        Dim rv As Boolean
        Dim len As Integer = bytes.Length
        Try
            Radio.Write(bytes, 0, len)
        Catch exto As TimeoutException
            'MsgBox(exto.Message)
            Tracing.TraceLine("comPort send:time out", TraceLevel.Error)
            rv = False
        Catch ex As Exception
            Tracing.TraceLine("comPort send exception:" & ex.Message, TraceLevel.Error)
            rv = False
        End Try
        Return rv
    End Function

    Dim flushFlag As Boolean = False
    Dim readBytes As Byte()
    Private Sub ComDataReceived(ByVal sender As SerialPort, ByVal e As SerialDataReceivedEventArgs)
        Dim len As Integer
        Try
            len = Radio.BytesToRead
            Radio.Read(readBytes, 0, len)
        Catch ex As Exception
            ' ignore.  Probably a race condition.
            Tracing.TraceLine("comPort ComDataReceived:" & ex.Message, TraceLevel.Error)
            Return
        End Try
        If Thread.VolatileRead(flushFlag) Then
            Tracing.TraceLine("comPort ComDataReceived:flush", TraceLevel.Info)
            Return
        End If
        Try
            If OpenParms.RawIO Then
                RigControl.IBytesHandler(readBytes, len)
            Else
                Dim str As String = ""
                For i As Integer = 0 To len - 1
                    If readBytes(i) <> 0 Then
                        str &= ChrW(readBytes(i))
                    End If
                Next
                If str <> "" Then
                    RigControl.InterruptHandler(str)
                End If
            End If
        Catch ex As Exception
            ' Ignore.  RigControl likely is closed.
            Tracing.TraceLine("comPort ComDataReceived:" & ex.Message)
        End Try
    End Sub
End Class
