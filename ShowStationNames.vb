Imports radios

Public Class ShowStationNames
    Private stationList = New List(Of String)

    Private Sub ShowStationNames_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If RigControl Is Nothing Then
            DialogResult = DialogResult.Cancel
            Return
        End If
        DialogResult = DialogResult.None

        stationList = RigControl.Stations
        StationsList.DataSource = stationList
    End Sub

    Private Sub DoneButton_Click(sender As Object, e As EventArgs) Handles DoneButton.Click
        DialogResult = DialogResult.OK
    End Sub
End Class