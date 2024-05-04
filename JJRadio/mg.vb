Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.Diagnostics
Imports JJTrace
Imports Radios

Public Class MemoryGroups
    Private Const mustBeLoaded As String = "Memories must be loaded."
    Private Const mustBeGroups As String = "Memory groups must be defined"
    Private allGroups As AllRadios.ScanGroup()

    Public Worker As UserControl

    Private Sub MemoryGroups_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        ' Ensure we have memories and groups.
        DialogResult = Windows.Forms.DialogResult.Abort
        If (Not RigControl.MemoriesLoaded) Or (RigControl.NumberOfMemories = 0) Then
            MsgBox(mustBeLoaded)
            Return
        End If
        If ReservedMemoryGroups.Groups Is Nothing Then
            MsgBox(mustBeGroups)
            Return
        End If
        DialogResult = Windows.Forms.DialogResult.None

        ' Setup the Worker.
        Worker.Location = PlaceHolder.Location
        Controls.Add(Worker)

        ' Currently only have reserved groups.
        allGroups = ReservedMemoryGroups.Groups

        ' See if need to setup the list.
        Dim setup As Boolean = False
        If (GroupsBox.Items.Count > 0) AndAlso _
           (GroupsBox.Items.Count = ReservedMemoryGroups.Groups.Length) Then
            For i As Integer = 0 To GroupsBox.Items.Count
                If GroupsBox.Items(i) = ReservedMemoryGroups.Groups(i).Name Then
                    setup = True
                    Exit For
                End If
            Next
        Else
            setup = True
        End If
        If Not setup Then
            Return
        End If

        GroupsBox.Items.Clear()
        For i As Integer = 0 To ReservedMemoryGroups.Groups.Length - 1
            GroupsBox.Items.Add(ReservedMemoryGroups.Groups(i).Name)
        Next
    End Sub

    Private Sub GroupsBox_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles GroupsBox.SelectedIndexChanged
        If GroupsBox.SelectedIndex = -1 Then
            Return
        End If
        MembersBox.Items.Clear()
        For Each mem As AllRadios.MemoryData In allGroups(GroupsBox.SelectedIndex).Members
            MembersBox.Items.Add(mem.DisplayName)
        Next
    End Sub

    Private Sub MemoryGroups_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Controls.Remove(Worker)
    End Sub
End Class