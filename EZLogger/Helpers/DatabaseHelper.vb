Imports System.Data.SQLite
Imports System.IO
Imports System.Windows.Forms
Imports EZLogger.Helpers
Imports EZLogger.Models

Public Module DatabaseHelper

    ''' <summary>
    ''' Retrieves a single patient record matching the given patient number.
    ''' </summary>
    ''' <param name="patientNumber">The patient number to search for.</param>
    ''' <returns>A PatientCls object if found; otherwise, Nothing.</returns>
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

                Dim query As String = "SELECT * FROM EZL WHERE patient_number = @patientNumber LIMIT 1"

                Using cmd As New SQLiteCommand(query, conn)
                    cmd.Parameters.AddWithValue("@patientNumber", patientNumber)

                    Using reader As SQLiteDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            Dim patient As New PatientCls With {
                            .PatientNumber = reader("patient_number").ToString(),
                            .CommitmentDate = reader("commitment_date").ToString(),
                            .AdmissionDate = reader("admission_date").ToString(),
                            .Expiration = reader("expiration").ToString(),
                            .DOB = reader("dob").ToString(),
                            .FullName = reader("fullname").ToString(),
                            .LName = reader("lname").ToString(),
                            .FName = reader("fname").ToString(),
                            .MName = reader("mname").ToString(),
                            .BedStatus = reader("bed_status").ToString(),
                            .P = reader("p").ToString(),
                            .U = reader("u").ToString(),
                            .Classification = reader("class").ToString(),
                            .County = reader("county").ToString(),
                            .Language = reader("language").ToString(),
                            .AssignedTo = reader("assigned_to").ToString(),
                            .RevokeDate = reader("revoke_date").ToString(),
                            .CourtNumbers = reader("court_numbers").ToString(),
                            .Department = reader("department").ToString()
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
