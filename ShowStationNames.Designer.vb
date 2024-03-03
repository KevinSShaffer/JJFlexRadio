<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ShowStationNames
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.StationsList = New System.Windows.Forms.ListBox()
        Me.StationsLabel = New System.Windows.Forms.Label()
        Me.DoneButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'StationsList
        '
        Me.StationsList.AccessibleName = "Stations"
        Me.StationsList.AccessibleRole = System.Windows.Forms.AccessibleRole.List
        Me.StationsList.FormattingEnabled = True
        Me.StationsList.ItemHeight = 16
        Me.StationsList.Location = New System.Drawing.Point(0, 40)
        Me.StationsList.Name = "StationsList"
        Me.StationsList.Size = New System.Drawing.Size(300, 52)
        Me.StationsList.TabIndex = 11
        '
        'StationsLabel
        '
        Me.StationsLabel.AutoSize = True
        Me.StationsLabel.Location = New System.Drawing.Point(50, 20)
        Me.StationsLabel.Name = "StationsLabel"
        Me.StationsLabel.Size = New System.Drawing.Size(98, 17)
        Me.StationsLabel.TabIndex = 10
        Me.StationsLabel.Text = "Station names"
        '
        'DoneButton
        '
        Me.DoneButton.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.DoneButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.DoneButton.Location = New System.Drawing.Point(8, 48)
        Me.DoneButton.Name = "DoneButton"
        Me.DoneButton.Size = New System.Drawing.Size(75, 23)
        Me.DoneButton.TabIndex = 12
        Me.DoneButton.Text = "Done"
        Me.DoneButton.UseVisualStyleBackColor = True
        '
        'ShowStationNames
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.DoneButton
        Me.ClientSize = New System.Drawing.Size(382, 253)
        Me.Controls.Add(Me.DoneButton)
        Me.Controls.Add(Me.StationsLabel)
        Me.Controls.Add(Me.StationsList)
        Me.Name = "ShowStationNames"
        Me.Text = "Show connected stations"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents StationsList As ListBox
    Friend WithEvents StationsLabel As Label
    Friend WithEvents DoneButton As Button
End Class
