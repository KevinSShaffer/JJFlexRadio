Imports System.IO
Imports System.Windows.Forms
Imports Ionic.Zip

Friend Class ImportSetup
    Private Const openDialogTitle As String = "Setup info archive"
    Private Const mustHaveFile As String = "You must specify an input file."
    Private Const infoGathered As String = "Setup imported."

    Friend Shared Function ImportSetup() As Boolean
        Dim rv As Boolean = False
        Dim openDialog = New OpenFileDialog()
        openDialog.AddExtension = True
        openDialog.CheckFileExists = True
        openDialog.DefaultExt = "zip"
        openDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        openDialog.Title = openDialogTitle
        If openDialog.ShowDialog() <> DialogResult.OK Then
            openDialog.Dispose()
            MessageBox.Show(mustHaveFile, ErrorHdr, MessageBoxButtons.OK)
            Return rv
        End If
        Dim outDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)

        Dim archive As ZipFile
        Try
            archive = New ZipFile(openDialog.FileName)
            ' get application data
            archive.ExtractSelectedEntries("type = F", Nothing, outDir, ExtractExistingFileAction.OverwriteSilently)
            rv = True
        Catch ex As Exception
            MessageBox.Show(ex.Message, ExceptionHdr, MessageBoxButtons.OK)
        End Try
        If rv Then
            MessageBox.Show(infoGathered, MessageHdr, MessageBoxButtons.OK)
        End If
        openDialog.Dispose()
        Return rv
    End Function
End Class
