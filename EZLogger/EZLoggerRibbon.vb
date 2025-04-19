Imports Microsoft.Office.Tools.Ribbon
Imports System.Windows.Forms
Imports EZLogger.Helpers
Imports HostForms
Public Class EZLoggerRibbon

    ' This event fires when the Ribbon is loaded.
    Private Sub EZLoggerRibbon_Load(ByVal sender As System.Object, ByVal e As RibbonUIEventArgs) Handles MyBase.Load
        ' Clear any existing items from the DatabaseMenu control.
        DatabaseMenu.Items.Clear()

        ' Create the "Minute Orders" menu item.
        Dim item1 As Microsoft.Office.Tools.Ribbon.RibbonButton = Me.Factory.CreateRibbonButton()
        item1.Label = "Minute Orders"
        item1.Tag = "minuteOrders"
        AddHandler item1.Click, AddressOf DatabaseMenuItem_Click
        DatabaseMenu.Items.Add(item1)

        ' Create the "TCARs" menu item.
        Dim item2 As Microsoft.Office.Tools.Ribbon.RibbonButton = Me.Factory.CreateRibbonButton()
        item2.Label = "TCARs"
        item2.Tag = "tcars"
        AddHandler item2.Click, AddressOf DatabaseMenuItem_Click
        DatabaseMenu.Items.Add(item2)

        ' Create the "CONREP" menu item.
        Dim item3 As Microsoft.Office.Tools.Ribbon.RibbonButton = Me.Factory.CreateRibbonButton()
        item3.Label = "CONREP"
        item3.Tag = "conrep"
        AddHandler item3.Click, AddressOf DatabaseMenuItem_Click
        DatabaseMenu.Items.Add(item3)

        ' Create the "Notifications" menu item.
        Dim item4 As Microsoft.Office.Tools.Ribbon.RibbonButton = Me.Factory.CreateRibbonButton()
        item4.Label = "Notifications"
        item4.Tag = "notifications"
        AddHandler item4.Click, AddressOf DatabaseMenuItem_Click
        DatabaseMenu.Items.Add(item4)
    End Sub

    ' This button toggles the Report Wizard Task Pane.
    Private Sub ReportWizardButton_Click(sender As Object, e As RibbonControlEventArgs) Handles ReportWizardButton.Click
        Globals.ThisAddIn.ReportWizardTaskPane.Visible = Not Globals.ThisAddIn.ReportWizardTaskPane.Visible
    End Sub

    ' This button toggles the Report Wizard Task Pane.
    Private Sub DatabaseMenuItem_Click(sender As Object, e As RibbonControlEventArgs)
        Dim button As Microsoft.Office.Tools.Ribbon.RibbonButton = CType(sender, Microsoft.Office.Tools.Ribbon.RibbonButton)
        Dim tag As String = button.Tag.ToString()

        Select Case tag
            Case "minuteOrders"
                MessageBox.Show("Minute Orders selected")
            Case "tcars"
                MessageBox.Show("TCARs selected")
            Case "conrep"
                MessageBox.Show("CONREP selected")
            Case "notifications"
                MessageBox.Show("Notifications selected")
            Case Else
                MessageBox.Show("Unknown selection")
        End Select
    End Sub

    Private Sub Button1_Click(sender As Object, e As RibbonControlEventArgs) Handles Button1.Click
        Dim ptinfo As New PatientInfoHost()
        ptinfo.Show()
    End Sub

    Private Sub Button2_Click(sender As Object, e As RibbonControlEventArgs) Handles Button2.Click
        Dim lorem As String = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. " &
                          "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. " &
                          "Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. " &
                          "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."

        MsgBoxHelper.Show(lorem, Sub(result)
                                     If result = CustomMsgBoxResult.OK Then
                                         MsgBox("User acknowledged the long message.")
                                     End If
                                 End Sub)
    End Sub
    Private Sub SettingsButton_Click(sender As Object, e As RibbonControlEventArgs) Handles SettingsButton.Click
        Dim configHost As New ConfigHost()
        configHost.ShowDialog()
    End Sub

    Private Sub BtnCloseDoc_Click(sender As Object, e As RibbonControlEventArgs) Handles BtnCloseDoc.Click
        DocumentHelper.CloseActiveDocument(showPrompt:=False)
    End Sub

    Private Sub AboutButton_Click(sender As Object, e As RibbonControlEventArgs) Handles AboutButton.Click
        Dim aboutHost As New AboutHost()
        aboutHost.ShowDialog()
    End Sub

    Private Sub Button3_Click(sender As Object, e As RibbonControlEventArgs) Handles Button3.Click
        TestHelper.Test_UpdateLocalConfigWithGlobalPath()
    End Sub

    Private Sub RandomPatientNumberButton_Click(sender As Object, e As RibbonControlEventArgs) Handles RandomPatientNumberButton.Click
        TestHelper.PromptRandomPatientNumberForTest()
    End Sub
End Class

