' Namespace=EZLogger/Helpers
' Filename=GetEarly90DayHelper.vb
' !See Label Footer for notes

Imports Microsoft.Office.Interop.Word
Imports EZLogger.Helpers
Imports System.Data
Imports System.Data.SqlClient
Imports System.Windows
Imports MessageBox = System.Windows.MessageBox

Module GetEarly90DayHelper

    ''' <summary>
    ''' This Sub does not create a new PatientCls it takes an existing patient obj and ADDS
    ''' (or updates) the Early 90-Day fields.
    ''' </summary>
    ''' <param name="patient"></param>
    Public Sub PopulateEarly90DayInfo(patient As PatientCls)
        Dim connStr As String = ConfigHelper.GetGlobalConfigValue("database", "connectionString")
        Dim dtEarly As New System.Data.DataTable()
        '^--hold results from the stored procedure in a DataTable
        Dim row As DataRow = Nothing
        Dim doc As Document = DocumentHelper.GetActiveWordDocument()
        If doc Is Nothing Then Exit Sub


        If patient Is Nothing Then Exit Sub

        If String.IsNullOrWhiteSpace(patient.PatientNumber) Then Exit Sub

        If patient.Classification <> "PC1370" Then Exit Sub
        '^--We should only need to run this on 1370 pts

        If String.IsNullOrWhiteSpace(connStr) Then
            MessageBox.Show("SQL Server connection string not found in global_config.json.", "Missing Config", MessageBoxButton.OK, MessageBoxImage.Error)
            Exit Sub
        End If

        Try
            Using conn As New SqlConnection(connStr)
                conn.Open()

                Using cmd As New SqlCommand("uspGetEarly90DayInfo", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.AddWithValue("@ptnum", patient.PatientNumber)

                    Using daEarly As New SqlDataAdapter(cmd)
                        daEarly.Fill(dtEarly)

                        If dtEarly.Rows.Count > 0 Then
                            row = dtEarly.Rows(0)

                            ' If the value is = 1 then we can write properties to the document
                            If Not IsDBNull(row("Early")) Then
                                patient.EarlyNinetyDay = Convert.ToInt32(row("Early"))
                                'Early = 1 if true (int)
                            Else
                                patient.EarlyNinetyDay = 0
                            End If

                            Dim earlyValue As String = patient.EarlyNinetyDay.ToString()
                            DocumentPropertyHelper.WriteCustomProperty(doc, "Early90Day", earlyValue)

                            Dim completionDateValue As String = ""
                            If earlyValue = "1" Then
                                patient.Completion90 = Convert.ToDateTime(row("Completed90"))
                                completionDateValue = patient.Completion90.Value.ToString("MM/dd/yyyy")
                                DocumentPropertyHelper.WriteCustomProperty(doc, "Early90Date", completionDateValue)
                            End If
                        End If
                    End Using
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("SQL Server error while retrieving Early 90-Day data: " & ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error)
        End Try

    End Sub

End Module
