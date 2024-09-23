Imports System.Windows.Forms
Imports JJTrace

Public Class CWDecode
    Private Const mustBeNumeric As String = "The value must be numeric and greater than 0."
    Private Const constrainText As String = "Constrain"
    Private Const fillBoxText As String = "Fill the box"
    Private constrain As Boolean = False
    Private wasActive As Boolean
    Private constrainValue As Integer = 0
    Private Sub setConstrainText()
        constrainValue = CurrentOp.CWDecodeCells
        If constrainValue = 0 Then
            constrainValue = CurrentOp.BrailleDisplaySize
        End If
        ValueBox.Text = constrainValue
    End Sub

    Private Sub setConstrain(val As Boolean)
        constrain = val
        If val Then
            ConstrainButton.Text = fillBoxText
            setConstrainText()
        Else
            ConstrainButton.Text = constrainText
        End If
        ValueLabel.Enabled = constrain
        ValueBox.Enabled = constrain
        ChangeButton.Enabled = constrain
    End Sub

    Private Sub CWDecode_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        DialogResult = DialogResult.None
        wasActive = False
        setConstrain(CurrentOp.ConstrainedDecode)
    End Sub

    Private Sub ConstrainButton_Click(sender As System.Object, e As System.EventArgs) Handles ConstrainButton.Click
        setConstrain(Not constrain) ' toggle
        DialogResult = Windows.Forms.DialogResult.None
    End Sub

    Private Sub ChangeButton_Click(sender As System.Object, e As System.EventArgs) Handles ChangeButton.Click
        Dim val As String = ValueBox.Text
        If IsNumeric(val) AndAlso (val > 0) Then
            constrainValue = val
            ' Change user's value
            Operators.UpdateCWDecode(CurrentOp, constrain, val)
            DialogResult = Windows.Forms.DialogResult.OK
        Else
            MsgBox(mustBeNumeric)
            DialogResult = Windows.Forms.DialogResult.None
        End If
    End Sub

    Private Sub DoneButton_Click(sender As System.Object, e As System.EventArgs) Handles DoneButton.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
    End Sub

    Private Sub CWDecode_Activated(sender As System.Object, e As System.EventArgs) Handles MyBase.Activated
        If Not wasActive Then
            wasActive = True
            ConstrainButton.Focus()
        End If
    End Sub
End Class