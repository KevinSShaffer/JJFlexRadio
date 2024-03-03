Imports System.IO
Imports System.Windows.Forms
Imports Ionic.Zip

Friend Class ExportSetup
    Private Const openDialogTitle As String = "Setup info archive"
    Private Const mustHaveFile As String = "You must specify an output file."
    Private Const infoGathered As String = "Setup gathered."

    Friend Shared Sub ExportSetup()
        Dim openDialog = New OpenFileDialog()
        openDialog.AddExtension = True
        openDialog.CheckFileExists = False
        openDialog.DefaultExt = "zip"
        openDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        openDialog.Title = openDialogTitle
        If openDialog.ShowDialog() <> DialogResult.OK Then
            openDialog.Dispose()
            MessageBox.Show(mustHaveFile, ErrorHdr, MessageBoxButtons.OK)
            Return
        End If

        File.Delete(openDialog.FileName)
        Using archive As ZipFile = New ZipFile(openDialog.FileName)
            ' get application data
            archive.AddSelectedFiles("name != *trace*.txt", BaseConfigDir, ProgramName, True)
            'archive.AddDirectory(BaseConfigDir, ProgramName)

            archive.Save()
        End Using
        MessageBox.Show(infoGathered, MessageHdr, MessageBoxButtons.OK)
        openDialog.Dispose()
    End Sub
End Class
