Imports System.Data
Imports System.Windows.Controls
Imports EZLogger.Helpers
Imports Microsoft.Office.Interop.Word

Public Module TCARListHandler

    ''' <summary>
    ''' Retrieves all active TCAR records from the database.
    ''' </summary>
    ''' <returns>A list of <see cref="TCARRecord"/> objects representing active TCAR records.</returns>
    ''' <remarks>
    ''' This function connects to the SQLite database, executes a query to fetch records
    ''' where the "active" field is set to 1, and maps the results to a list of TCARRecord objects.
    ''' If an error occurs during database access, an error message is displayed to the user.
    ''' </remarks>
    Public Function LoadAllActive() As List(Of TCARRecord)
        Dim results As New List(Of TCARRecord)
        Dim connStr As String = $"Data Source={ConfigPathHelper.GetDatabasePath()}"

        Try
            Using conn As New SQLite.SQLiteConnection(connStr)
                conn.Open()

                Dim query As String = "
                SELECT casenum, patient_name, subdate, opID
                FROM tcar_list
                WHERE active = 1;
            "

                Using cmd As New SQLite.SQLiteCommand(query, conn)
                    Using reader As SQLite.SQLiteDataReader = cmd.ExecuteReader()
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
    Public Sub HandleTCARSelect(grid As DataGrid)
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

            MsgBoxHelper.Show("TCAR referral details recorded successfully.")

        Catch ex As Exception
            MsgBoxHelper.Show("Unable to access the active Word document. " & ex.Message)
        End Try
    End Sub

End Module
