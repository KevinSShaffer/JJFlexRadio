Imports System.IO
Imports System.Windows.Forms
Imports Ionic.Zip
Imports JJTrace

Friend Class DebugInfo
    Private Const openDialogTitle As String = "Debug info archive"
    Private Const mustHaveFile As String = "You must specify a debug file."
    Private Const infoGathered As String = "Debug info gathered."

    Friend Shared Sub GetDebugInfo()
        Dim openDialog = New OpenFileDialog()
        openDialog.AddExtension = True
        openDialog.CheckFileExists = False
        openDialog.DefaultExt = "zip"
        openDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        openDialog.Title = openDialogTitle
        If openDialog.ShowDialog() <> DialogResult.OK Then
            MsgBox(mustHaveFile)
            Return
        End If

        If Tracing.On Then
            Tracing.TraceLine("GetDebugInfo:Tracing turned off")
            Tracing.On = False
        End If

        File.Delete(openDialog.FileName)
        Using archive As ZipFile = New ZipFile(openDialog.FileName)
            archive.AddDirectory(BaseConfigDir, ConfigDirBaseName)

            If LastUserTraceFile <> vbNullString Then
                archive.AddFile(LastUserTraceFile)
            End If

            archive.Save()
        End Using
        MsgBox(infoGathered)
    End Sub
End Class
