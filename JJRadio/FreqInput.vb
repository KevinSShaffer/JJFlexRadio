Friend Class FreqInput
    Friend Buffer As String = ""

    Private Sub FreqInput_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        FreqBox.Text = ""
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OKButton.Click
        DialogResult = Nothing
        Dim str As String
        str = FormatFreqForRadio(FreqBox.Text)
        If str Is Nothing Then
            MsgBox("Frequency" & BadFreqMSG)
        Else
            DialogResult = DialogResult.OK
            Buffer = str
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CnclButton.Click
        DialogResult = DialogResult.Cancel
    End Sub
End Class