' Namespace=EZLogger/Handlers
' Filename=TCARListHandler.vb
' !See Label Footer for notes

Imports EZLogger.Helpers
Imports System.Data.SqlClient
Imports System.Windows.Forms
Imports Haley.Utils
Imports Microsoft.Office.Interop.Word
Imports System.Collections.ObjectModel

Namespace Handlers
    Public Class TcarListHandler

        ''' <summary>
        ''' Represents the live collection of task items currently loaded in the Task List panel. This collection is UI-bound and updates dynamically
        ''' </summary>
        Public ReadOnly Property Tasks As ObservableCollection(Of TaskItem)

        ''' <summary>
        ''' Retrieves all active TCAR records from the database.
        ''' </summary>
        ''' <returns>A list of <see cref="TCARRecord"/> objects representing active TCAR records.</returns>
        ''' <remarks>
        ''' This function connects to the SQL database, executes a query to fetch records
        ''' where the "active" field is set to 1, and maps the results to a list of TCARRecord objects.
        ''' If an error occurs during database access, an error message is displayed to the user.
        ''' </remarks>
        Public Function LoadAllActive() As List(Of TCARRecord)
            Dim results As New List(Of TCARRecord)
            Dim connStr As String = ConfigHelper.GetGlobalConfigValue("database", "connectionString")
            If String.IsNullOrWhiteSpace(connStr) Then
                MsgBoxHelper.Show("SQL Server connection string not found in global_config.json.")
                Exit Function
            End If

            Try
                Using conn As New SqlConnection(connStr)
                    conn.Open()

                    Dim query As String = "
                        SELECT casenum, patient_name, subdate, opID
                        FROM tcar_list
                        WHERE active = 1
                        ORDER BY subdate DESC
                    "

                    Using cmd As New SqlCommand(query, conn)
                        Using reader As SqlDataReader = cmd.ExecuteReader()
                            While reader.Read()
                                results.Add(New TCARRecord With {
                                    .Casenum = reader("casenum").ToString(),
                                    .PatientName = reader("patient_name").ToString(),
                                    .Subdate = reader("subdate").ToString(),
                                    .OpID = Convert.ToInt32(reader("opID"))
                                })
                            End While
                        End Using
                    End Using
                End Using
            Catch ex As Exception
                MsgBoxHelper.Show("Error loading TCAR records: " & ex.Message)
            End Try

            Return results
        End Function

        ''' <summary>
        ''' Called when the user presses the Select button in TCARListView.
        ''' If a row is selected, logs TCAR details to Word document custom properties.
        ''' </summary>
        ''' <param name="grid">The DataGrid displaying TCAR records.</param>
        Public Sub HandleTcarSelect(grid As System.Windows.Controls.DataGrid)

            Dim selected As TCARRecord = TryCast(grid.SelectedItem, TCARRecord)

            If selected Is Nothing Then
                MsgBoxHelper.Show("Please select a patient from the list if they were found. Otherwise, press the close button.")
                Return
            End If

            Try
                Dim doc As Document = Globals.ThisAddIn.Application.ActiveDocument

                ' Write TCAR Referral Date
                DocumentPropertyHelper.WriteCustomProperty(doc, "TCAR Referral Date", selected.Subdate)

                ' Write Days Since TCAR
                Dim daysSince As Integer = (DateTime.Now - DateTime.Parse(selected.Subdate)).Days
                DocumentPropertyHelper.WriteCustomProperty(doc, "Days Since TCAR", daysSince.ToString())

                ' ✅ Add task to TaskList
                Dim message As String = $"{selected.PatientName.ToUpper()} found on TCAR List"
                Dim taskHandler As New TaskListHandler()
                taskHandler.AddTaskFromReport(message)

                MsgBoxHelper.Show("TCAR referral details recorded successfully.")

            Catch ex As Exception
                MsgBoxHelper.Show("Unable to access the active Word document. " & ex.Message)
            End Try

        End Sub

        ''' <summary>
        ''' Saves the current list of tasks to a task list
        ''' </summary>
        ''' <remarks>
        ''' This method converts the ObservableCollection of Tasks to a List and uses the TasksIO utility to save them.
        ''' It should be called whenever changes to the task list need to be persisted.
        ''' </remarks>
        Public Sub Save()
            TasksIO.SaveTasks(Tasks.ToList())
        End Sub

        ''' <summary>
        ''' Closes TCARListView
        ''' </summary>
        ''' <remarks>clean modern approach using hostForm?.Close()</remarks>
        Public Sub HandleCloseClick(hostForm As Form)
            hostForm?.Close()
        End Sub

    End Class
End Namespace

' Footer:
''===========================================================================================
'' Filename: .......... TCARListHandler.vb
'' Description: ....... Handles adding, updating, displaying the TCAR List
'' Created: ........... 2025-05-02
'' Updated: ........... 2025-05-02
'' Installs to: ....... EZLogger/Handlers
'' Compatibility: ..... VSTO, WPF
'' Contact Author: .... lundeen-bryan
'' Copyright:  ........ ©2025. All rights reserved.
'' Notes: ............. _
' (1) notes_here
''===========================================================================================