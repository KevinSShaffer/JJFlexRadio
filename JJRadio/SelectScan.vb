Friend Class SelectScan
    Friend ItemIndex As Integer ' selected item's index

    Private Sub SelectScan_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ItemIndex = -1
        DialogResult = DialogResult.None
        ' Load any configured scans if not loaded.
        If SavedScans Is Nothing Then
            scan.loadScanData()
        End If
        ' SavedScans must be setup.
        If (SavedScans Is Nothing) OrElse (SavedScans.Count = 0) Then
            MsgBox("No scans were saved.")
            DialogResult = DialogResult.Abort
            Return
        End If
        NameListBox.Items.Clear()
        NameListBox.Items.AddRange(SavedScans.GetNames())
        NameListBox.SelectedIndex = -1
    End Sub

    Private Sub OkButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OkButton.Click
        If NameListBox.SelectedIndex < 0 Then
            MsgBox("You must select an item.")
            DialogResult = DialogResult.None
        Else
            ItemIndex = NameListBox.SelectedIndex
            DialogResult = DialogResult.OK
        End If
    End Sub

    Private Sub CnclButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CnclButton.Click
        DialogResult = DialogResult.Cancel
    End Sub
End Class