﻿Imports System.IO
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
            openDialog.Dispose()
            MessageBox.Show(mustHaveFile, ErrorHdr, MessageBoxButtons.OK)
            Return
        End If

        If Tracing.On Then
            Tracing.TraceLine("GetDebugInfo:Tracing turned off")
            Tracing.On = False
        End If

        File.Delete(openDialog.FileName)
        Using archive As ZipFile = New ZipFile(openDialog.FileName)
            'archive.CompressionMethod = CompressionMethod.None
            ' get application data
            archive.AddDirectory(BaseConfigDir, ProgramName)

            ' get the program
            archive.AddDirectory(".", "program")

            Dim tempFileName = My.Computer.FileSystem.GetTempFileName
            Try
                File.Move(tempFileName, tempFileName & ".txt")
                tempFileName = tempFileName & ".txt"
            Catch
                ' won't rename
            End Try
            If RigControl IsNot Nothing Then
                ' get rig info.
                Using sw = New StreamWriter(tempFileName)
                    Dim infoList = RigControl.RigInfo
                    For Each txt As String In infoList
                        'MsgBox(txt)
                        sw.WriteLine(txt)
                    Next
                End Using
                archive.AddFile(tempFileName, "riginfo")
            End If

            If LastUserTraceFile <> vbNullString Then
                archive.AddFile(LastUserTraceFile, "")
            End If

            archive.Save()
            File.Delete(tempFileName)
        End Using
        MessageBox.Show(infoGathered, MessageHdr, MessageBoxButtons.OK)
        openDialog.Dispose()
    End Sub
End Class
