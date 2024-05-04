Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports JJTrace
Imports JJRadio.KeyCommands

Public Class DefineCommands
    Const dupTitle As String = "Duplicate key Definitions"
    Const dupMessage As String = "Continue with duplicate key definitions?"
    Const dupAccessibleName As String = "Duplicate items are checked"
    Const normAccessibleName As String = "Key and function list"
    Dim wasActive As Boolean ' true if active when activated
    Dim theKeys As keyTbl()
    Dim myCheck As Boolean()
    Dim commandChanges, messageChanges As Boolean ' true if changes were made
    ' Used for dup checking
    Class dupElement
        Public id As Integer
        Public data As KeyDefType
    End Class

    Private Sub setupList()
        theKeys = Commands.CurrentKeys
        ReDim myCheck(theKeys.Length - 1)
        CommandsListBox.SuspendLayout()
        CommandsListBox.Items.Clear()
        For i As Integer = 0 To theKeys.Length - 1
            CommandsListBox.Items.Add(KeyString(theKeys(i).key.key) & " - " & theKeys(i).helpText)
            myCheck(i) = False
        Next
        CommandsListBox.ResumeLayout()
        CommandsListBox.SelectedIndex = -1
    End Sub

    Private Function dupCheck() As Boolean
        Dim rv As Boolean = False
        ' First uncheck everything.
        For i As Integer = 0 To myCheck.Length - 1
            myCheck(i) = False
        Next

        Dim dupDict = New Dictionary(Of Keys, dupElement)
        For i As Integer = 0 To theKeys.Length - 1
            If theKeys(i).key.key <> Keys.None Then
                Dim dupItem As dupElement = Nothing
                If dupDict.TryGetValue(theKeys(i).key.key, dupItem) Then
                    checkItem(i, True)
                    checkItem(dupItem.id, True)
                    rv = True
                Else
                    dupItem = New dupElement
                    dupItem.id = i
                    dupItem.data = theKeys(i).key
                    dupDict.Add(dupItem.data.key, dupItem)
                End If
            End If
        Next
        If rv Then
            CommandsListBox.AccessibleName = dupAccessibleName
        Else
            CommandsListBox.AccessibleName = normAccessibleName
        End If
        ' Short delay to wait for the accessible name text.
        Threading.Thread.Sleep(10)
        Return rv
    End Function

    ''' <summary>
    ''' (overloaded) go to the first checked item
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub gotoFirstChecked()
        For i As Integer = 0 To myCheck.Length - 1
            If myCheck(i) Then
                CommandsListBox.SelectedIndex = i
                Exit For
            End If
        Next
        CommandsListBox.Focus()
    End Sub

    ''' <summary>
    ''' (overloaded) go to the first checked item
    ''' </summary>
    ''' <param name="k">item with this key</param>
    ''' <remarks></remarks>
    Private Sub gotoFirstChecked(ByVal k As Keys)
        For i As Integer = 0 To theKeys.Length - 1
            If theKeys(i).key.key = k Then
                CommandsListBox.SelectedIndex = i
                Exit For
            End If
        Next
        CommandsListBox.Focus()
    End Sub

    Private Sub DefineKeys_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        DialogResult = DialogResult.None
        setupList()
        wasActive = False ' not active yet
        commandChanges = False
        messageChanges = False
        CheckTimer.Enabled = True
        If dupCheck() Then
            Tracing.TraceLine("DefineKeys_load:duplicate items", TraceLevel.Warning)
            gotoFirstChecked()
        End If
    End Sub

    Private Sub OKButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OKButton.Click
        If commandChanges Or messageChanges Then
            ' Check for dups
            If dupCheck() Then
                DialogResult = MessageBox.Show(dupMessage, dupTitle, MessageBoxButtons.YesNo)
                If DialogResult <> DialogResult.Yes Then
                    DialogResult = DialogResult.None
                    gotoFirstChecked()
                Else
                    Tracing.TraceLine("DefineKeys Ok:exiting with duplicates", TraceLevel.Error)
                    DialogResult = DialogResult.OK
                End If
            Else
                DialogResult = DialogResult.OK
            End If
            If DialogResult = DialogResult.OK Then
                ' changes were made, so set values.
                Dim cmdDefs = New Collection(Of KeyDefType)
                Dim CWDefs = New Collection(Of KeyDefType)
                For Each item As KeyCommands.keyTbl In theKeys
                    If (item.KeyType = KeyTypes.Command) Or (item.KeyType = KeyTypes.log) Then
                        cmdDefs.Add(item.key)
                    Else
                        CWDefs.Add(item.key)
                    End If
                Next
                If commandChanges Then
                    Commands.SetValues(cmdDefs.ToArray, KeyCommands.KeyTypes.allKeys, True)
                    If Not messageChanges Then
                        ' must update messages now in any case.
                        Commands.UpdateCWText(CWDefs.ToArray)
                    End If
                End If
                If messageChanges Then
                    ' Update dictionaries and the keys in CWText.
                    Commands.UpdateCWText(CWDefs.ToArray)
                    ' Update the operator's info.
                    CWText.UpdateOperator()
                End If
            End If
        Else
            ' no change was made
            DialogResult = DialogResult.OK
        End If
    End Sub

    Private Sub CnclButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CnclButton.Click
        DialogResult = DialogResult.Cancel
    End Sub

    Private Sub CommandsListBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CommandsListBox.SelectedIndexChanged
        If CommandsListBox.SelectedIndex < 0 Then
            ValueBox.Enabled = False
            Return
        End If
        ValueBox.Text = KeyString(theKeys(CommandsListBox.SelectedIndex).key.key)
        ValueBox.Enabled = True
        CommandsListBox.Refresh()
    End Sub

    Private Sub ValueBox_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ValueBox.KeyDown
        If (e.KeyCode = Keys.Tab) Or (e.KeyCode = Keys.Menu) Or _
           (e.KeyCode = Keys.ShiftKey) Or (e.KeyCode = Keys.ControlKey) Then
            Return
        End If
        Dim id As Integer = CommandsListBox.SelectedIndex
        If id < 0 Then
            Return
        End If
        e.SuppressKeyPress = True
        Dim k As Keys = (e.KeyCode Or e.Modifiers)
        If k <> theKeys(id).key.key Then
            Tracing.TraceLine("defineKeys ValueBox_KeyDown:from " & theKeys(id).key.key.ToString & " to " & k.ToString, TraceLevel.Info)
            If (theKeys(id).KeyType = KeyTypes.Command) Or (theKeys(id).KeyType = KeyTypes.log) Then
                commandChanges = True
            Else
                messageChanges = True
            End If
            If k = Keys.Delete Then
                k = Keys.None
            End If
            theKeys(id).key.key = k
            Dim str As String = KeyString(k)
            CommandsListBox.Items(id) = str & " - " & theKeys(id).helpText
            ValueBox.Text = str
            dupCheck()
        End If
        CommandsListBox.Focus()
    End Sub

    Private Sub ValueBox_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ValueBox.Enter
        PressKeyLabel.Text = "Press desired key to change"
    End Sub

    Private Sub ValueBox_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ValueBox.Leave
        PressKeyLabel.Text = ""
    End Sub

    Private Sub DefineKeys_Activated(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Activated
        If Not wasActive Then
            wasActive = True
            CommandsListBox.Focus()
        End If
    End Sub

    Private Sub CommandsListBox_ItemCheck(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles CommandsListBox.ItemCheck
    End Sub

    Private Sub CheckTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckTimer.Tick
        ' Prevent the user from checking an item.
        ' Only check programatically.
        For i As Integer = 0 To myCheck.Length - 1
            CommandsListBox.SetItemChecked(i, myCheck(i))
        Next
    End Sub

    Private Sub checkItem(ByVal id As Integer, ByVal ck As Boolean)
        myCheck(id) = ck
    End Sub
End Class