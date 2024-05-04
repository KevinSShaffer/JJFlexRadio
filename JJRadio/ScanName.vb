Public Class ScanName
    Friend sName As String

    Private Sub ScanName_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        sName = ""
        DialogResult = DialogResult.None
    End Sub

    Private Sub OKButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OKButton.Click
        sName = ScanNameBox.Text
        If sName <> "" Then
            DialogResult = DialogResult.OK
        End If
    End Sub

    Private Sub CnclButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CnclButton.Click
        DialogResult = DialogResult.Cancel
    End Sub
End Class