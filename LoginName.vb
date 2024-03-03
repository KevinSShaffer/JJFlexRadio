Imports MsgLib

Public Class LoginName
    Inherits CalledUserControl
    Private Const mustHaveAddress As String = "You must specify a cluster address."
    Private Const mustHaveName As String = "You must specify a login name."

    Public Sub New(msg As OptionalMessageElement)
        InitializeComponent()

        Message = msg
        AddressBox.Text = CurrentOp.ClusterHostname
        LoginBox.Text = CurrentOp.ClusterLoginName
    End Sub

    Private Sub LoginName_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Form.AcceptButton = OkButton
        Form.CancelButton = CnclButton
    End Sub

    Private Sub OkButton_Click(sender As System.Object, e As System.EventArgs) Handles OkButton.Click
        If Not IsValidHostname(AddressBox.Text.Trim) Then
            MsgBox(mustHaveAddress)
            AddressBox.Focus()
            Return
        End If
        If LoginBox.Text.Trim = vbNullString Then
            MsgBox(mustHaveName)
            LoginBox.Focus()
            Return
        End If

        ReDim Message.ApplicationData(1)
        Message.ApplicationData(0) = AddressBox.Text.Trim()
        Message.ApplicationData(1) = LoginBox.Text.Trim()

        ' Only update operator info if not setup.
        If CurrentOp.ClusterLoginName = vbNullString Then
            CurrentOp.ClusterHostname = Message.ApplicationData(0)
            CurrentOp.ClusterLoginName = Message.ApplicationData(1)
        End If

        Message.Result = DialogResult.OK
        Form.SendMessageEvent(Message.Result)
    End Sub

    Private Sub CnclButton_Click(sender As System.Object, e As System.EventArgs) Handles CnclButton.Click
        Message.Result = DialogResult.Cancel
        Form.SendMessageEvent(Message.Result)
    End Sub
End Class
