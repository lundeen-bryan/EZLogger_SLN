Imports System.Data.SQLite
Imports System.Windows.Forms
Imports EZLogger.Models ' Adjust if your Patient class is in a different namespace

Namespace Handlers

    Public Class PatientDatabaseHandler

        Public Shared Function GetPatientByNumber(patientNumber As String) As PatientCls
            If String.IsNullOrWhiteSpace(patientNumber) Then
                Return Nothing
            End If

            Try
                Dim dbPath As String = GetDatabasePath()
                Dim connectionString As String = $"Data Source={dbPath};Version=3;"

                Using conn As New SQLiteConnection(connectionString)
                    conn.Open()

                    Dim query As String = "SELECT * FROM EZL WHERE patient_number = @patientNumber"

                    Using cmd As New SQLiteCommand(query, conn)
                        cmd.Parameters.AddWithValue("@patientNumber", patientNumber)

                        Using reader As SQLiteDataReader = cmd.ExecuteReader()
                            If reader.Read() Then
                                Dim patient As New PatientCls With {
                                    .PatientNumber = reader("patient_number").ToString(),
                                    .FullName = reader("full_name").ToString(),
                                    .County = reader("county").ToString()
                                }
                                Return patient
                            End If
                        End Using
                    End Using
                End Using

            Catch ex As Exception
                Windows.MessageBox.Show("Error retrieving patient data: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

            Return Nothing
        End Function

        Private Shared Function GetDatabasePath() As String
            ' TODO: Replace with JSON config reader later
            Return "C:\Users\lunde\repos\cs\ezlogger\EZLogger_SLN\data\ezlogger.db"
        End Function

    End Class

End Namespace
