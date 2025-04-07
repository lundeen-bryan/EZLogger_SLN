Imports System.Data.SQLite
Imports System.IO
Imports System.Windows.Forms
Imports EZLogger.Helpers
Imports EZLogger.Models ' Adjust if PatientCls is in a different folder or namespace

Public Module DatabaseHelper

    Public Function GetPatientByNumber(patientNumber As String) As PatientCls
        If String.IsNullOrWhiteSpace(patientNumber) Then
            Return Nothing
        End If

        Try
            Dim dbPath As String = ConfigPathHelper.GetDatabasePath()
            If String.IsNullOrWhiteSpace(dbPath) OrElse Not File.Exists(dbPath) Then
                MessageBox.Show("SQLite database path not found or file does not exist.", "Config Error")
                Return Nothing
            End If
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
                                .FullName = reader("fullname").ToString(),
                                .County = reader("county").ToString()
                            }
                            Return patient
                        End If
                    End Using
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("Error retrieving patient data: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Return Nothing
    End Function

End Module
