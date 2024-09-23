Imports JJRadio.OptionalMessageControlWindow

Public Class OptionalMessageControlDefault
    Friend WriteOnly Property Message As String
        Set(value As String)
            TheMessage.Text = value
        End Set
    End Property

    Private Sub OKButton_Click(sender As System.Object, e As System.EventArgs) Handles OKButton.Click
        'RaiseResultEvent(Windows.Forms.DialogResult.OK)
    End Sub
End Class
