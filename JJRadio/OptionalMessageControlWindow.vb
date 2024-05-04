Imports System.Xml.Serialization

Public Class OptionalMessageControlWindow
    Public Class OptionalMessageElement
        Public Tag As String
        Public IntResult As Integer
        <XmlIgnore()> Public Property result As Windows.Forms.DialogResult
            Get
                Return IntResult
            End Get
            Set(value As Windows.Forms.DialogResult)
                IntResult = value
            End Set
        End Property
        Public Ignore As Boolean
        Public Sub New()
        End Sub
        Public Sub New(t As String, rslt As Windows.Forms.DialogResult, i As Boolean)
            Tag = t
            IntResult = rslt
            Ignore = i
        End Sub
    End Class

    Friend Class ResultEventArg
        Inherits System.EventArgs
        Public Result As Windows.Forms.DialogResult
        Public Sub New(r As Windows.Forms.DialogResult)
            Result = r
        End Sub
    End Class
    Friend Event ResultEvent(arg As ResultEventArg)

    Friend Property Title As String
        Get
            Return Text
        End Get
        Set(value As String)
            Text = value
        End Set
    End Property

    Friend Message As String = Nothing

    Friend MessageControl As UserControl

    Friend MessageTag As String = Nothing

    Private theControl As Control

    Private Sub OptionalMessageControlWindow_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        If ((Message Is Nothing) And (MessageControl Is Nothing) Or
            (Message IsNot Nothing) And (MessageControl IsNot Nothing)) Then
            DialogResult = Windows.Forms.DialogResult.Abort
            Return
        End If

        If MessageTag = vbNullString Then
            DontshowBox.Enabled = False
            DontshowBox.Visible = False
        End If

        If Message IsNot Nothing Then
            Dim optionalMsg = New OptionalMessageControlDefault
            theControl = optionalMsg
            optionalMsg.Message = Message
        Else
            theControl = MessageControl
        End If
        AddHandler ResultEvent, AddressOf messageKeyEventHandler
        theControl.Location = New Point(0, 0)
        Controls.Add(theControl)
    End Sub

    Friend Sub messageKeyEventHandler(e As ResultEventArg)
        If DontshowBox.Checked Then
            Operators.UpdateOptionalMessageTags(CurrentOp, New OptionalMessageElement(Tag, e.Result, True))
        End If

        Controls.Remove(theControl)
        theControl.Dispose()
        DialogResult = e.Result
    End Sub

    Friend Sub RaiseResultEvent(result As Windows.Forms.DialogResult)
        RaiseEvent ResultEvent(New ResultEventArg(result))
    End Sub
End Class