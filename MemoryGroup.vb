Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.Diagnostics
Imports System.IO
Imports System.Xml.Serialization
Imports JJTrace
Imports Radios

''' <summary>
''' Memory group UserControl.
''' </summary>
''' <remarks>
''' Externalized items:
''' allGroups and userGroups - lists of all and user groups.
''' groupFile - dictionary mapping group name to group for all groups.
''' ManageUserGroups - must be true if managing user groups.
''' SelectedGroup - the selected group
''' Setup - sets up this control and group lists.
''' AddUserGroup - Adds a user group.
''' UpdateUserGroup - updates a user group.
''' </remarks>
Public Class MemoryGroup
    Private Const mustBeLoaded As String = "Memories must be loaded."
    ''' <summary>
    ''' The listBox used.
    ''' Depends upon whether we're managing or scanning.
    ''' </summary>
    ''' <returns>a ListBox type, either the checked or regular list.</returns>
    Friend ReadOnly Property GroupsBox As ListBox
        Get
            Dim rv As ListBox = GroupsCheckBox
            If ManageUserGroups Then
                rv = GroupsListBox
            End If
            Return rv
        End Get
    End Property

    Friend allGroups As List(Of FlexBase.ScanGroup)
    Friend groupFile As Dictionary(Of String, FlexBase.ScanGroup)

    Private ReadOnly Property fileName As String
        Get
            Return BaseConfigDir & "\" & PersonalData.UniqueOpName(CurrentOp) & "_" &
                CurrentRig.ModelName & "_groups.xml"
        End Get
    End Property

    Friend userGroups As List(Of FlexBase.ScanGroup)
    Friend ManageUserGroups As Boolean
    Friend SelectedGroup As FlexBase.ScanGroup
    ''' <summary>
    ''' The group list to display.
    '''  depends upon managing or scanning.
    ''' </summary>
    ''' <returns>list of ScanGroup</returns>
    Friend ReadOnly Property displayList As List(Of FlexBase.ScanGroup)
        Get
            Dim rv As List(Of FlexBase.ScanGroup) = allGroups
            If ManageUserGroups Then
                rv = userGroups
            End If
            Return rv
        End Get
    End Property

    ' Get userGroups from the file.
    Private Function fromFile() As Boolean
        Dim fs As Stream
        Try
            fs = File.Open(fileName, FileMode.Open)
        Catch nf As FileNotFoundException
            Return True ' ok if no file
        Catch ex As Exception
            MsgBox(ex.Message)
            Return False
        End Try

        ' Get the groups.
        Dim rv As Boolean = True
        Dim xs As New XmlSerializer(GetType(List(Of FlexBase.ScanGroup)))
        Try
            userGroups = xs.Deserialize(fs)
        Catch ex As Exception
            MsgBox(ex.Message)
            rv = False
        Finally
            fs.Dispose()
        End Try
        Return rv
    End Function
    ' Write userGroups to the file.
    Private Function toFile() As Boolean
        Dim fs As Stream
        Try
            fs = File.Open(fileName, FileMode.Create)
        Catch ex As Exception
            MsgBox(ex.Message)
            Return False
        End Try

        ' Write the groups.
        Dim rv As Boolean = True
        Try
            Dim xs As New XmlSerializer(GetType(List(Of FlexBase.ScanGroup)))
            Dim extGroups = New List(Of FlexBase.ScanGroup)
            extGroups.AddRange(userGroups)
            xs.Serialize(fs, extGroups)
        Catch ex As Exception
            MsgBox(ex.Message)
            rv = False
        Finally
            fs.Dispose()
        End Try
        Return rv
    End Function

    Private Function compareNames(g1 As FlexBase.ScanGroup, g2 As FlexBase.ScanGroup)
        Dim x As String = g1.Name
        Dim y As String = g2.Name
        Dim minLen As Integer = Math.Min(x.Length, y.Length)
        Dim xs As String = x.Substring(0, minLen)
        Dim ys As String = y.Substring(0, minLen)
        Dim rv As Integer = xs.CompareTo(ys)
        If rv = 0 Then
            rv = x.Length.CompareTo(y.Length)
        End If
        Return rv
    End Function

    ''' <summary>
    ''' Setup the GroupFile dictionary and the display, GroupsBox.
    ''' </summary>
    Private Sub setupLists()
        ' Setup the group dictionary with reserved and user groups.
        groupFile = New Dictionary(Of String, FlexBase.ScanGroup)
        For Each g As FlexBase.ScanGroup In allGroups
            groupFile.Add(g.Name, g)
        Next
        ' Setup the GroupsBox display from the provided list.
        GroupsBox.SuspendLayout()
        Dim saveID As Integer = GroupsBox.SelectedIndex
        GroupsBox.Items.Clear()
        displayList.Sort(AddressOf compareNames)
        For Each g As FlexBase.ScanGroup In displayList
            GroupsBox.Items.Add(g.Name)
        Next
        If saveID < GroupsBox.Items.Count Then
            GroupsBox.SelectedIndex = saveID
        End If
        GroupsBox.ResumeLayout()
    End Sub

    ''' <summary>
    ''' Setup the MemoryGroup control.
    ''' </summary>
    ''' <returns>true on success</returns>
    Friend Function Setup() As Boolean
        Tracing.TraceLine("MemoryGroup Setup:" & ManageUserGroups, TraceLevel.Info)
        ' Use the checked box or the list box
        If ManageUserGroups Then
            GroupsCheckBox.Enabled = False
            GroupsCheckBox.Visible = False
            GroupsListBox.Enabled = True
            GroupsListBox.Visible = True
        Else
            GroupsCheckBox.Enabled = True
            GroupsCheckBox.Visible = True
            GroupsListBox.Enabled = False
            GroupsListBox.Visible = False
        End If

        If (RigControl Is Nothing) OrElse (RigControl.NumberOfMemories = 0) Then
            MsgBox(mustBeLoaded)
            Return False
        End If
        allGroups = New List(Of FlexBase.ScanGroup)
        allGroups.AddRange(RigControl.GetReservedGroups)
        ' Get user groups from the file.
        If Not fromFile() Then
            Return False ' function failed.
        End If
        ' Add any user groups to allGroups.
        If userGroups Is Nothing Then
            userGroups = New List(Of FlexBase.ScanGroup)
        Else
            allGroups.AddRange(userGroups)
        End If

        ' setup the lists.
        setupLists()

        Tracing.TraceLine("MemoryGroup Setup:" & allGroups.Count & " " & userGroups.Count, TraceLevel.Info)
        Return True
    End Function

    Private Sub GroupsBox_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles GroupsCheckBox.SelectedIndexChanged, GroupsListBox.SelectedIndexChanged
        If GroupsBox.SelectedIndex = -1 Then
            Return
        End If
        SelectedGroup = groupFile(GroupsBox.SelectedItem)

        ' show the group's members
        MembersBox.Items.Clear()
        For Each name As String In displayList(GroupsBox.SelectedIndex).Members
            MembersBox.Items.Add(name)
        Next
    End Sub

    ''' <summary>
    ''' Add the specified user group.
    ''' </summary>
    ''' <param name="group">The ScanGroup to add.</param>
    Friend Sub AddUserGroup(group As FlexBase.ScanGroup)
        Tracing.TraceLine("AddUserGroup:" & group.Name & " " & group.Members.Count, TraceLevel.Info)
        allGroups.Add(group)
        userGroups.Add(group)
        setupLists()
        toFile()
    End Sub

    ''' <summary>
    ''' Update the specified user group.
    ''' </summary>
    ''' <param name="oldGroup">the original ScanGroup</param>
    ''' <param name="group">the updated ScanGroup</param>
    Friend Sub UpdateUserGroup(oldGroup As FlexBase.ScanGroup, group As FlexBase.ScanGroup)
        Tracing.TraceLine("UpdateUserGroup:" & oldGroup.Name & " " & oldGroup.Members.Count & " " & group.Name & " " & group.Members.Count, TraceLevel.Info)
        ' First remove the original group.
        Dim g As FlexBase.ScanGroup = groupFile(oldGroup.Name)
        allGroups.Remove(g)
        userGroups.Remove(g)
        groupFile.Remove(g.Name)
        AddUserGroup(group)
    End Sub

    Friend Sub DeleteUserGroup(group As FlexBase.ScanGroup)
        Tracing.TraceLine("DeleteUserGroup:" & group.Name, TraceLevel.Info)
        allGroups.Remove(group)
        userGroups.Remove(group)
        groupFile.Remove(group.Name)
        setupLists()
        toFile()
    End Sub
End Class
