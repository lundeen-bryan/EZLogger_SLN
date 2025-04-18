Imports System.Windows
Imports System.Windows.Forms
Imports EZLogger.Helpers
Imports Microsoft.Office.Interop.Word
Imports MessageBox = System.Windows.MessageBox

Namespace Handlers

    ''' <summary>
    ''' Provides logic for save-related operations in EZLogger.
    ''' </summary>
    Public Class SaveFileHandler

        ''' <summary>
        ''' Displays a message box as a placeholder for save file logic.
        ''' </summary>
        Public Sub ShowSaveMessage()
            Dim host As New SaveFileHost()
            host.TopMost = True
            FormPositionHelper.MoveFormToTopLeftOfAllScreens(host, 10, 10)
            host.Show()
        End Sub
        Public Sub HandleCloseClick(form As Form)
            If form IsNot Nothing Then form.Close()
        End Sub

        ''' <summary>
        ''' Loads Word document properties and populates controls in SaveFileView.
        ''' </summary>
        Public Sub HandleSearchPatientIdClick(view As SaveFileView)
            Try
                Dim GetProp = Function(name As String) DocumentPropertyHelper.GetPropertyValue(name)

                view.TxtPatientId.Text = GetProp("Patient Number")
                view.ReportTypeCbo.SelectedItem = GetProp("Report Type")

                Dim dateStr = GetProp("Report Date")
                If Date.TryParse(dateStr, Nothing) Then
                    view.ReportDatePicker.SelectedDate = Date.Parse(dateStr)
                Else
                    view.ReportDatePicker.SelectedDate = Nothing
                End If

                view.LblPatientName.Content = GetProp("Patient Name")
                view.LblProgram.Content = GetProp("Program")
                view.LblUnit.Content = GetProp("Unit")
                view.LblClassification.Content = GetProp("Classification")

            Catch ex As Exception
                MessageBox.Show("Failed to load document properties: " & ex.Message,
                                    "Error", MessageBoxButton.OK, MessageBoxImage.Error)
            End Try
        End Sub
    End Class

End Namespace
