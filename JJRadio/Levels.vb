Imports System.Threading
Imports JJTrace

''' <summary>
''' provides peak holding for a level
''' </summary>
Friend Class Levels
    ''' <summary>
    ''' function delegate that returns the level.
    ''' </summary>
    ''' <returns>Integer level</returns>
    Public Delegate Function fld() As Integer
    Private userValue As fld
    Private peakFlag As Boolean
    Private peakValue As Integer
    ''' <summary>
    ''' Control whether the level peak or raw value is returned.
    ''' </summary>
    ''' <value>true to provide the peak value</value>
    ''' <returns>true if peak provided</returns>
    Public Property Peak As Boolean
        Get
            Return peakFlag
        End Get
        Set(value As Boolean)
            Tracing.TraceLine("Levels.Peak:" & value.ToString, TraceLevel.Info)
            peakFlag = value
            If value Then
                stop_tmr = False
                peakThread = New Thread(AddressOf peakProc)
                peakThread.Name = "peakThread"
                'peakThread.Priority = ThreadPriority.AboveNormal
                peakThread.Start()
            Else
                stop_tmr = True
                Thread.Sleep(baseInterval * 2)
            End If
        End Set
    End Property
    Const baseInterval As Integer = 50 ' . .2 seconds
    Const defaultTicks As Integer = 500 ' .5 second default
    Private cycleTicks As Integer ' ticks per cycle
    Private WithEvents tmr As Timer
    ''' <summary>
    ''' time interval over which the peak is evaluated.
    ''' Zero turns peak evaluation off, peak=false.
    ''' </summary>
    ''' <value>millisecond time</value>
    ''' <returns>millisecond time</returns>
    Public Property Cycle As Integer
        Get
            Return cycleTicks * baseInterval
        End Get
        Set(value As Integer)
            Tracing.TraceLine("Levels.Cycle:" & value, TraceLevel.Info)
            If value = 0 Then
                ' turn off peak
                Peak = False
            Else
                ' Round up cycles to nearest base interval
                cycleTicks = (value + baseInterval - 1) / baseInterval
            End If
        End Set
    End Property
    ''' <summary>
    ''' Provide the function to retrieve the level.
    ''' </summary>
    ''' <param name="f">delegate</param>
    Public Sub New(f As fld)
        Tracing.TraceLine("Levels.new", TraceLevel.Info)
        userValue = f
        peakFlag = False
        tmr = Nothing
        Cycle = defaultTicks
    End Sub

    ''' <summary>
    ''' (ReadOnly) The level, or raw peak level.
    ''' </summary>
    ''' <returns>Integer level</returns>
    Public ReadOnly Property Value As Integer
        Get
            If Peak Then
                Return peakValue
            Else
                Return userValue()
            End If
        End Get
    End Property

    Private peakThread As Thread
    Private stop_tmr As Boolean
    Private Sub peakProc()
        Tracing.TraceLine("peakProc", TraceLevel.Info)
        If tmr Is Nothing Then
            ' start timer
            Dim inst As New TimerCallback(AddressOf tmr_tick)
            tmr = New Timer(inst, Nothing, 0, baseInterval)
        End If

        ' Just await timer ticks
        While Not stop_tmr
            Thread.Sleep(baseInterval)
        End While
        Tracing.TraceLine("peakProc stop_tmr", TraceLevel.Info)

        If tmr IsNot Nothing Then
            ' stop timer
            tmr.Dispose()
            tmr = Nothing
        End If
    End Sub

    Private myPeak As Integer = 0
    Private myTicks As Integer = 0
    Private Sub tmr_tick()
        Dim v As Integer
        Try
            v = userValue() ' Read the real value
        Catch ex As Exception
            Tracing.TraceLine("tmr_tick exception:" & ex.Message, TraceLevel.Error)
            v = 0
        End Try
        If v > myPeak Then
            myPeak = v
        End If
        If myTicks = cycleTicks Then
            myTicks = 0
            peakValue = myPeak
            myPeak = 0
        Else
            myTicks += 1
        End If
    End Sub

    Public Sub ResetPeak()
        myPeak = 0
        peakValue = 0
    End Sub
End Class
