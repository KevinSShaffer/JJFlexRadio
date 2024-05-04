Imports RadioBoxes
Imports System.Collections
Imports System.Threading
Imports JJTrace

Public Class Form1
    Const meterID As Integer = 0
    Const splitID As Integer = 1
    Const vfoID As Integer = 2
    Const freqID As Integer = 3
    Dim mainFields() As MainBox.Field = _
        {New MainBox.Field(4, "", " "), _
         New MainBox.Field(1, "", ""), _
         New MainBox.Field(1, "", ""), _
         New MainBox.Field(11, "", " ")}

    Class boxTest
        Public val As Single
        Public ReadOnly Property Display As String
            Get
                Return val.ToString("f1")
            End Get
        End Property
        Public ReadOnly Property RigItem As Single
            Get
                Return val
            End Get
        End Property
        Public Sub New(v As Single)
            val = v
        End Sub
    End Class
    Private RigStuff() As boxTest = _
        {New boxTest(14.0), New boxTest(14.1)}
    Private rigList As New ArrayList

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Tracing.TraceFile = "tester_trace.txt"
        Tracing.On = True
        TheBox.Populate(mainFields)
        rigList.Add(RigStuff(0))
        rigList.Add(RigStuff(1))
        Combo1.TheList = rigList
        Combo1.UpdateDisplayFunction = Function() CSng(RigBox.Text)
        Combo1.UpdateRigFunction = _
            Sub(v As Single) RigBox.Text = v.ToString("f1")
        RigBox.Text = "14.1" ' Seed some data.
        Combo1.UpdateDisplay()

        TheBox.SelectionStart = 10
        TheBox.Write(meterID, CStr(TheBox.SelectionStart))
        TheBox.Write(splitID, "")
        TheBox.Write(vfoID, "A")
        TheBox.Write((freqID), CStr(14.02505))
        If TheBox.Changed Then
            TheBox.Display()
        End If

        NumberBox.LowValue = 0
        NumberBox.HighValue = 190
        NumberBox.Increment = 10
        NumberBox.UpdateDisplayFunction = Function() CInt(testBoxOut.Text)
        NumberBox.UpdateRigFunction =
            Sub(v As Integer) testBoxOut.Text = CStr(v)
        testBoxOut.Text = "100"
        NumberBox.UpdateDisplay()
    End Sub

    Private Sub ContinueButton_Click(sender As System.Object, e As System.EventArgs) Handles ContinueButton.Click
        Dim t As New Thread(AddressOf threadProc)
        t.Start()
    End Sub
    Private Sub threadProc()
        TheBox.SelectionStart = 9
        TheBox.Write(meterID, CStr(TheBox.SelectionStart))
        TheBox.Write(splitID, "S")
        TheBox.Write(vfoID, "B")
        TheBox.Write((freqID), CStr(7.2))
        If TheBox.Changed Then
            TheBox.Display()
        Else
            TheBox.Clear()
            TheBox.Write(freqID, "no change")
            TheBox.Display()
        End If
    End Sub

    Private Sub RigBox_Leave(sender As System.Object, e As System.EventArgs) Handles RigBox.Leave
        Combo1.UpdateDisplay()
    End Sub

    Private Sub testBoxOut_Leave(sender As System.Object, e As System.EventArgs) Handles testBoxOut.Leave
        NumberBox.UpdateDisplay()
    End Sub
End Class