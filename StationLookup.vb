Imports HamQTHLookup
Imports JJCountriesDB
Imports JJTrace
Imports MsgLib

Public Class StationLookup
    Private lookup As CallbookLookup = Nothing
    Private countriesdb As CountriesDB = Nothing

    Private Sub StationLookup_Load(sender As Object, e As EventArgs) Handles MyBase.Load
#If 0 Then
        If (CurrentOp.HamqthID <> vbNullString And CurrentOp.HamqthPassword <> vbNullString) Then
            lookup = New CallbookLookup(CurrentOp.HamqthID, CurrentOp.HamqthPassword)
            AddHandler lookup.CallsignSearchEvent, AddressOf lookupHandler
        End if
#End If
        lookup = New CallbookLookup(HamqthLookupID, HamqthLookupPassword)
        AddHandler lookup.CallsignSearchEvent, AddressOf lookupHandler
        countriesdb = New CountriesDB()
        wasActive = False
        DialogResult = DialogResult.None
    End Sub

    Public Sub Finished()
        If lookup IsNot Nothing Then
            lookup.Finished()
            LookupStation.Dispose()
        End If
    End Sub

    Private Sub LookupButton_Click(sender As Object, e As EventArgs) Handles LookupButton.Click
        If CallsignBox.Text = vbNullString Then
            CallsignBox.Focus()
            Return
        End If

        ' If the field has a blank, " ", in the tag, clear it.
        For Each c As Control In Controls
            If (c.Tag IsNot Nothing) And (c.Tag = " ") Then
                CType(c, TextBox).Text = ""
            End If
        Next

        If lookup IsNot Nothing Then
            NameBox.Focus()
            lookup.LookupCall(CallsignBox.Text)
        Else
            CountryBox.Focus()
        End If

        Dim rec As Record = countriesdb.LookupByCall(CallsignBox.Text)
        If rec IsNot Nothing Then
            CountryBox.Text = rec.Country
            If rec.CountryID <> vbNullString Then
                CountryBox.Text += " (" & rec.CountryID & ")"
            End If
            LatlongBox.Text = rec.Latitude & "/" & rec.Longitude
            CQBox.Text = rec.CQZone
            ITUBox.Text = rec.ITUZone
            GMTBox.Text = rec.TimeZone
        End If
    End Sub

    Private Sub lookupHandler(item As CallbookLookup.HamQTH)
        If item IsNot Nothing Then
            TextOut.DisplayText(NameBox, item.search.nick, False, True)
            TextOut.DisplayText(QTHBox, item.search.qth, False, True)
            TextOut.DisplayText(QTHBox, " (" + item.search.grid + ")", False, False)
            TextOut.DisplayText(StateBox, item.search.State, False, True)
        End If
    End Sub

    Private Sub DoneButton_Click(sender As Object, e As EventArgs) Handles DoneButton.Click
        DialogResult = DialogResult.OK
    End Sub

    Private Sub Box_Enter(sender As Object, e As EventArgs) Handles CallsignBox.Enter, CountryBox.Enter, LatlongBox.Enter, QTHBox.Enter, StateBox.Enter, CQBox.Enter, ITUBox.Enter, GMTBox.Enter, NameBox.Enter
        Dim tb As TextBox = CType(sender, TextBox)
        If tb.Text <> vbNullString Then
            tb.SelectionStart = 0
            tb.SelectionLength = tb.Text.Length
        End If
    End Sub

    Private wasActive As Boolean
    Private Sub StationLookup_Activated(sender As Object, e As EventArgs) Handles MyBase.Activated
        If Not wasActive Then
            wasActive = True
            CallsignBox.Focus()
        End If
    End Sub
End Class