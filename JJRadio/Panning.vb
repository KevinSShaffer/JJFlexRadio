Imports HamBands
Imports JJTrace
Imports Radios
Imports System.Threading

Public Class Panning
    Const badPanData As String = "the value must be entered in KHZ or MHZ.KHZ"
    Const panHighTooLow As String = "The high value must be greater than the low."

    Private wasPanning As Boolean = False
    Private wasSpeech, wasAutoMode As Boolean
    Private oldMode As AllRadios.ModeValue
    Private focusBox As TextBox = LowBox
    Private rigFrequency, oldLow, oldHigh As ULong
    Private currentBand As Bands.BandItem

    Private cells As Integer = 40
    Private hzPerCell As Single
    Private panLow As ULong = 0
    Private panHigh As ULong = 0
    Private panBand As Bands.BandNames
    Private _BrailleCells As Integer = 0
    Friend Property BrailleCells As Integer
        Get
            Return _BrailleCells
        End Get
        Set(value As Integer)
            If _BrailleCells <> value Then
                _BrailleCells = value
                PanBox.Size = New Size(_BrailleCells * 7, 20)
                cellInfo = New accumulator(_BrailleCells)
                panBoxClear()
            End If
        End Set
    End Property

    Private Const passes As Integer = 2
    Private Class accumulator
        Private lines(,) As Integer
        Private id As Integer = 0
        Private outputChars() As Char = New Char() _
            {"a", "b", "l", "p", "q", "="}
        Private Sub clearLines()
            Dim cells As Integer = lines.GetLength(1)
            For c As Integer = 0 To cells - 1
                For p As Integer = 0 To passes - 1
                    lines(p, c) = -1
                Next
            Next
        End Sub
        Public Sub New(cells As Integer)
            lines = New Integer(passes - 1, cells - 1) {}
            clearLines()
            id = 0
        End Sub
        Public Sub newPass()
            id = (id + 1) Mod passes
        End Sub
        Public Sub setCell(c As Integer, i As Integer)
            Tracing.TraceLine("pan setCell:" & id.ToString & " " & i.ToString & " " & c.ToString, TraceLevel.Verbose)
            lines(id, i) = c
        End Sub
        Public Overrides Function ToString() As String
            Dim cells As Integer = lines.GetLength(1)
            Dim rvi(cells - 1) As Integer
            Dim rv(cells - 1) As Char
            Dim floor As Integer = Integer.MaxValue
            Dim maxValue As Integer = 0
            For i As Integer = 0 To cells - 1
                ' get the max value over the passes.
                Dim value As Integer = -1
                For p As Integer = 0 To passes - 1
                    If lines(p, i) > value Then
                        value = lines(p, i)
                    End If
                Next
                rvi(i) = value
                ' If cell was used:
                If value <> -1 Then
                    ' Get the max value and noise floor.
                    If value < floor Then
                        floor = value
                    End If
                    If value > maxValue Then
                        maxValue = value
                    End If
                End If
            Next
            ' Get the output characters
            Dim valuePerChar As Single = (maxValue - floor) / outputChars.Length
            If valuePerChar < 1 Then
                valuePerChar = 1
            End If
            'Tracing.TraceLine("tostring:" & floor.ToString & " " & maxValue.ToString & " " & valuePerChar.ToString)
            For i As Integer = 0 To cells - 1
                If rvi(i) = -1 Then
                    rvi(i) = floor
                End If
                Dim id As Integer = CInt((rvi(i) - floor) / valuePerChar)
                If id = outputChars.Length Then
                    id = outputChars.Length - 1
                End If
                rv(i) = outputChars(id)
                'Tracing.TraceLine("  " & rvi(i).ToString & " " & rv(i))
            Next
            clearLines()
            Return CStr(rv)
        End Function
    End Class
    Private cellInfo As accumulator

    Private Delegate Function tbWriteRtn(tb As TextBox, txt As String) As Integer
    Private Function tbWrite(tb As TextBox, txt As String) As Integer
        Dim id As Integer = tb.SelectionStart
        tb.Text = txt
        Return id
    End Function
    Private Sub panBoxWrite(txt As String)
        Dim id As Integer
        If PanBox.InvokeRequired Then
            Dim rtn = New tbWriteRtn(AddressOf tbWrite)
            id = CInt(PanBox.Invoke(rtn, New Object() {PanBox, txt}))
        Else
            id = tbWrite(PanBox, txt)
        End If
        PanBox.SelectionStart = id
    End Sub
    Private Sub panBoxClear()
        Dim txt(cells - 1) As Char
        For i As Integer = 0 To cells - 1
            txt(i) = " "
        Next
        panBoxWrite(txt)
        PanBox.SelectionLength = 0
        PanBox.SelectionStart = 0
    End Sub

    Private Function toKHZString(frequency As ULong)
        Return CStr(CULng(frequency / 1000))
    End Function

    Private Function checkKHZField(tb As TextBox, ByRef frequency As ULong) As Boolean
        Dim str As String = FormatFreqForRadio(tb.Text)
        Dim rv As Boolean = (str IsNot Nothing)
        If rv Then
            frequency = CULng(str)
        Else
            frequency = 0
            tb.Focus()
        End If
        'Tracing.TraceLine("check:" & tb.Text & " " & str & " " & frequency.ToString())
        Return (rv)
    End Function

    Private Sub selectBoxData()
        Dim boxes() As TextBox = New TextBox() _
            {LowBox, HighBox, IncrementBox}
        For Each box As TextBox In boxes
            box.SelectionStart = 0
            box.SelectionLength = box.Text.Length
        Next
    End Sub

    Private Sub Panning_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Tracing.TraceLine("Panning_load", TraceLevel.Info)
        Static firstTime As Boolean = True
        DialogResult = Windows.Forms.DialogResult.None
        cells = CurrentOp.BrailleDisplaySize
        rigFrequency = RigControl.RXFrequency
        currentBand = Bands.Query(RigControl.RXFrequency)
        If firstTime Then
            AddHandler RigControl.PanEvent, AddressOf panHandler
            AddHandler RigControl.PanException, AddressOf panErrorHandler
            firstTime = False
        Else
            ' been here before
            ' resume panning if frequency is in range.
            If wasPanning And _
               ((rigFrequency >= oldLow) And (rigFrequency <= oldHigh)) Then
                selectBoxData()
                prePan()
                RigControl.PanStart()
                focusBox = PanBox
                Return
            End If
        End If
        ' Either first time here or rig frequency has changed.
        wasPanning = False
        focusBox = LowBox
        panBoxClear()
        If currentBand Is Nothing Then
            ' use current rig settings
            panLow = RigControl.RXFrequency
            panHigh = panLow + (cells * 1000) ' convert to khz
            panBand = Bands.BandNames.NumBands
        Else
            ' use current band
            panLow = currentBand.Low
            panHigh = currentBand.High
            panBand = currentBand.Band
        End If
        LowBox.Text = toKHZString(panLow)
        HighBox.Text = toKHZString(panHigh)
        IncrementBox.Text = 1
        selectBoxData()
    End Sub

    Private passCount As Integer = 0
    Private Sub panHandler(sender As Object, e As AllRadios.PanEventArg)
        Tracing.TraceLine("vb panHandler:" & e.Frequency.ToString & " " &
                          e.Meter.ToString & " " & passCount.ToString, TraceLevel.Info)
        If e.Frequency = panLow Then
            If passCount = passes Then
                panBoxWrite(cellInfo.ToString)
                passCount = 0
            End If
            passCount += 1
            cellInfo.newPass()
        End If
        Dim id As Integer = (e.Frequency - panLow) / hzPerCell
        If id >= cells Then
            id = cells - 1
        End If
        cellInfo.setCell(e.Meter, id)
    End Sub

    Private Sub panErrorHandler(sender As Object, e As AllRadios.PanExceptionArg)
        MsgBox(e.ex.Message)
        wasPanning = False
        DialogResult = Windows.Forms.DialogResult.Abort
    End Sub

    Private Sub StartButton_Click(sender As System.Object, e As System.EventArgs) Handles StartButton.Click
        Tracing.TraceLine("startButton:" & LowBox.Text & " " & HighBox.Text &
                          " " & IncrementBox.Text, TraceLevel.Info)
        ' Check entered data.
        Dim increment As ULong = 0
        If checkKHZField(LowBox, panLow) AndAlso _
           checkKHZField(HighBox, panHigh) AndAlso _
           checkKHZField(IncrementBox, increment) Then
            If panLow < panHigh Then
                BrailleCells = cells
                RigControl.PanLow = panLow
                RigControl.PanHigh = panHigh
                hzPerCell = (panHigh - panLow) / cells
                RigControl.PanIncrement = increment
                passCount = 0
                prePan()
                RigControl.PanStart()
                wasPanning = True
                oldLow = panLow
                oldHigh = panHigh
                PanBox.Focus()
                DialogResult = Windows.Forms.DialogResult.None
            Else
                MsgBox(panHighTooLow)
                HighBox.Focus()
            End If
        Else
            ' invalid data
            MsgBox(badPanData)
        End If
    End Sub
    Private Sub prePan()
        ' Don't want speech or auto mode on here!
        wasSpeech = RigControl.RigSpeech
        If wasSpeech Then
            RigControl.RigSpeech = False
        End If
        wasAutoMode = RigControl.AutoMode
        If wasAutoMode Then
            RigControl.AutoMode = False
            oldMode = RigControl.Mode
        End If
    End Sub

    Private Sub DoneButton_Click(sender As System.Object, e As System.EventArgs) Handles DoneButton.Click
        Tracing.TraceLine("panDoneButton", TraceLevel.Info)
        RigControl.PanStop()
        ' perhaps re-enable speech and auto mode
        reenable(rigFrequency)
        DialogResult = Windows.Forms.DialogResult.Cancel
        ' Note we don't change wasPanning.
    End Sub

    Private Sub PanBox_KeyPress(sender As System.Object, e As System.Windows.Forms.KeyPressEventArgs) Handles PanBox.KeyPress
        If e.KeyChar = " "c Then
            Tracing.TraceLine("PanBox_KeyPress:" & PanBox.SelectionStart.ToString, TraceLevel.Info)
            gotoFrequency(PanBox.SelectionStart)
        End If
    End Sub

    Private Sub PanBox_MouseClick_1(sender As System.Object, e As System.Windows.Forms.MouseEventArgs) Handles PanBox.MouseClick
        Tracing.TraceLine("panMouseClick:" & PanBox.SelectionStart.ToString, TraceLevel.Info)
        gotoFrequency(PanBox.SelectionStart)
    End Sub

    Private Sub gotoFrequency(id As Integer)
        Dim freq As ULong = RigControl.PanLow + id * hzPerCell
        Tracing.TraceLine("panGotoFrequency:" & id.ToString & " " &
                          freq.ToString, TraceLevel.Info)
        RigControl.PanStop()
        ' perhaps reenable speech and auto mode
        reenable(freq)
        DialogResult = Windows.Forms.DialogResult.OK
        ' Note we leave wasPanning set.
    End Sub

    Private Sub reenable(frequency As ULong)
        If wasAutoMode Then
            RigControl.AutoMode = wasAutoMode
        End If
        If wasSpeech Then
            RigControl.RigSpeech = wasSpeech
        End If
        RigControl.RXFrequency = frequency
        If wasAutoMode Then
            Thread.Sleep(250) ' wait a bit
            RigControl.Mode = oldMode
        End If
    End Sub

    Private Sub Panning_Activated(sender As System.Object, e As System.EventArgs) Handles MyBase.Activated
        If focusBox IsNot Nothing Then
            focusBox.Focus()
            focusBox = Nothing
        End If
    End Sub
End Class