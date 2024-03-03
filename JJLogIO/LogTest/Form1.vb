Imports System.IO
Imports JJLogIO

Public Class Form1
    ' Default logfile.
    Dim logFile As String = "myfile.JRL"
    Dim Log As LogIO
    Dim position As Int64

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        With OpenDialog
            .CheckFileExists = False
            .CheckPathExists = True
            .DefaultExt = "JRL"
            .AddExtension = True
            .InitialDirectory = Directory.GetCurrentDirectory
            .FileName = logFile
            If .ShowDialog <> DialogResult.OK Then
                End
            End If
            logFile = .FileName
        End With
        Try
            Log = New LogIO(logFile)
            position = Log.SeekToFirst
        Catch ex As Exception
            MsgBox(ex.Message)
            End
        End Try
        If ScreenSaver.GetScreenSaverActive Then
            TextBox1.Text = "screen saver active"
            ScreenSaver.SetScreenSaverActive(False)
        Else
            TextBox1.Text = "screen saver inactive"
            ScreenSaver.SetScreenSaverActive(True)
        End If
    End Sub

    Private Sub ReadButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReadButton.Click
        If Log.EOF Then
            If Log.Empty Then
                MsgBox("The log is empty.")
                Return
            End If
            MsgBox("At the last record; going to the first one.")
            Try
                Log.SeekToFirst()
            Catch ex As Exception
                MsgBox(ex.Message)
                Return
            End Try
        End If
        ' Record the position for later.
        position = Log.Position
        Try
            TextBox1.Text = Log.Read()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        tbFocus()
    End Sub

    Private Sub AddButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddButton.Click
        Try
            Log.Append(TextBox1.Text)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        ' We're positioned at the just-added record.
        position = Log.Position
        tbFocus()
    End Sub

    Private Sub FindButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FindButton.Click
        Dim err As Boolean = False
        Dim oldPosition As Int64 = position
        Log.SeekToFirst()
        If Log.EOF Then
            MsgBox("The file is empty.")
            err = True
        ElseIf TextBox1.Text = "" Then
            MsgBox("No argument.")
            err = True
        End If
        Dim str As String
        While Not (Log.EOF Or err)
            str = ""
            Try
                str = Log.Read
            Catch ex As Exception
                MsgBox(ex.Message)
                err = True
            End Try
            If str.Contains(TextBox1.Text) Then
                TextBox1.Text = str
                Exit While
            Else
                ' Position will be set to the found record.
                position = Log.Position
            End If
        End While
        If Log.EOF AndAlso Not err Then
            MsgBox(TextBox1.Text & " wasn't found.")
            err = True
        End If
        If err Then
            position = oldPosition
        End If
        tbFocus()
    End Sub

    Private Sub UpdateButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UpdateButton.Click
        ' Seek to the last position.
        Try
            Log.SeekToPosition(position)
            Log.Update(TextBox1.Text)
            position = Log.Position
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        tbFocus()
    End Sub

    Private Sub tbFocus()
        TextBox1.SelectionStart = 0
        TextBox1.SelectionLength = TextBox1.Text.Length
        TextBox1.Focus()
    End Sub

    Private Sub ReadPrevButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReadPrevButton.Click
        If position = Log.FirstRecord Then
            MsgBox("Already at the first record.")
            Return
        End If
        Log.SeekToPosition(position)
        Try
            Log.SeekToPrevious()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        ReadButton_Click(sender, e)
        ' Return to the just-read position.
        Try
            Log.SeekToPosition(position)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub DeleteButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteButton.Click
        If Log.Empty Then
            MsgBox("There are no records.")
            Return
        End If
        Try
            Log.SeekToPosition(position)
            Log.Delete()
        Catch ex As Exception
            MsgBox(ex.Message)
            Return
        End Try
        If Log.Empty Then
            TextBox1.Text = ""
            TextBox1.Focus()
        Else
            ReadButton_Click(sender, e)
        End If
    End Sub

    Private Sub Form1_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If Log.IsOpen Then
            Log.Close()
        End If
    End Sub
End Class
