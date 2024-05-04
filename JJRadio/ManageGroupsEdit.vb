Imports System.Collections
Imports System.Collections.Generic
Imports System.Diagnostics
Imports JJTrace
Imports Radios

Public Class ManageGroupsEdit
    Private Const mustHaveName As String = "The group must have a unique name."
    Private Const mustHaveMembers As String = "The group must have at least one member."
    Private sortedMemories As List(Of AllRadios.MemoryData)

    Friend oldGroup As AllRadios.ScanGroup
    Friend Group As AllRadios.ScanGroup
    Private ReadOnly Property updateFlag As Boolean
        Get
            Return Group IsNot Nothing
        End Get
    End Property
    Friend groupsControl As MemoryGroup

    Private Function compareNames(m1 As AllRadios.MemoryData, m2 As AllRadios.MemoryData)
        Dim x As String = m1.DisplayName
        Dim y As String = m2.DisplayName
        Dim minLen As Integer = Math.Min(x.Length, y.Length)
        Dim xs As String = x.Substring(0, minLen)
        Dim ys As String = y.Substring(0, minLen)
        Dim rv As Integer = xs.CompareTo(ys)
        If rv = 0 Then
            rv = x.Length.CompareTo(y.Length)
        End If
        Return rv
    End Function

    Private Sub ManageGroupsEdit_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        SuspendLayout()
        ' Sort memories by display name.
        ' Use only present (not empty) memories.
        sortedMemories = New List(Of AllRadios.MemoryData)
        For i As Integer = 0 To RigControl.NumberOfMemories - 1
            If RigControl.Memories(i).Present Then
                sortedMemories.Add(RigControl.Memories(i))
            End If
        Next
        sortedMemories.Sort(AddressOf compareNames)

        Dim groupID As Integer = 0
        For i As Integer = 0 To sortedMemories.Count - 1
            Dim m As AllRadios.MemoryData = sortedMemories(i)
            ' Add to the members list
            MembersBox.Items.Add(m.DisplayName)
            ' Check items from the group on an update.
            If updateFlag Then
                If (groupID < Group.Members.Count) AndAlso _
                   (Group.Members(groupID).DisplayName = m.DisplayName) Then
                    MembersBox.SetItemChecked(i, True)
                    ' Select the first checked item.
                    If groupID = 0 Then
                        MembersBox.SelectedIndex = i
                    End If
                    groupID += 1
                End If
            End If
        Next
        If updateFlag Then
            NameBox.Text = Group.Name
            oldGroup = Group
        End If
        ResumeLayout()

        DialogResult = Windows.Forms.DialogResult.None
    End Sub

    Private Sub OkButton_Click(sender As System.Object, e As System.EventArgs) Handles OkButton.Click
        If (NameBox.Text = "") OrElse _
            ((Not updateFlag) AndAlso groupsControl.groupFile.Keys.Contains(NameBox.Text)) Then
            MsgBox(mustHaveName)
            NameBox.Focus()
            Return
        End If
        If MembersBox.CheckedItems.Count = 0 Then
            MsgBox(mustHaveMembers)
            MembersBox.Focus()
            Return
        End If

        Dim items = New List(Of AllRadios.MemoryData)
        For Each id As Integer In MembersBox.CheckedIndices
            items.Add(sortedMemories(id))
        Next
        Group = New AllRadios.ScanGroup(NameBox.Text, RigControl.Memories.Bank, items)
        DialogResult = Windows.Forms.DialogResult.OK
    End Sub
End Class