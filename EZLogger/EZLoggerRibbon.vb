Imports EZLogger.Handlers
Imports EZLogger.Helpers
Imports Microsoft.Office.Tools.Ribbon
Imports System.Windows
Imports System.Windows.Forms
Imports MessageBox = System.Windows.MessageBox
Imports EZLogger.Controls
Imports System.Threading.Tasks
Public Class EZLoggerRibbon

    ' This event fires when the Ribbon is loaded.
    Private Sub EZLoggerRibbon_Load(ByVal sender As System.Object, ByVal e As RibbonUIEventArgs) Handles MyBase.Load
        'TBD
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

    Private Sub DeleteDocPropsBtn_Click(sender As Object, e As RibbonControlEventArgs) Handles DeleteDocPropsBtn.Click
        Try
            DocumentPropertyHelper.DeleteAllCustomProperties()
            MessageBox.Show("All custom document properties have been cleared.", "Success", MessageBoxButton.OK, MessageBoxImage.Information)
        Catch ex As Exception
            MessageBox.Show("Failed to clear properties: " & ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error)
        End Try
    End Sub

    Private Sub BtnTestFolder_Click(sender As Object, e As RibbonControlEventArgs) Handles BtnTestFolder.Click
        Dim handler As New ConfigViewHandler()
        handler.HandleTestFolderPickerClick()
    End Sub

    Private Async Sub LookupHlvBtn_Click(sender As Object, e As RibbonControlEventArgs) Handles LookupHlvBtn.Click
        Dim patientNumber As String = "219891-9"
        Dim provider As String = Nothing

        ' Just show the spinner — SpinnerHost handles everything internally
        Using hostForm As New BusyHost()
            hostForm.Show()

            Await Task.Delay(100) ' Give UI time to finish rendering

            Try
                ' Run the Excel work on a background thread
                provider = Await Task.Run(Function()
                                              Return ExcelHelper.GetProviderFromHLV(patientNumber)
                                          End Function)
            Finally
                hostForm.Close()
            End Try
        End Using

        If Not String.IsNullOrWhiteSpace(provider) Then
            MsgBoxHelper.Show($"Provider for patient {patientNumber}:{vbCrLf}{provider}")
        Else
            MsgBoxHelper.Show($"No provider found for patient number: {patientNumber}")
        End If
    End Sub
End Class

