Imports System.Data.SQLite
Imports System.IO
Imports System.Windows
Imports System.Windows.Forms
Imports EZLogger.Helpers
Imports MessageBox = System.Windows.MessageBox

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
            Dim dbPath As String = PathHelper.GetDatabasePath()
            If String.IsNullOrWhiteSpace(dbPath) OrElse Not File.Exists(dbPath) Then
                MessageBox.Show("SQLite database path not found or file does not exist.", "Config Error")
                Return Nothing
            End If

            Dim connectionString As String = $"Data Source={dbPath};Version=3;"
            Using conn As New SQLiteConnection(connectionString)
                conn.Open()

                ' Join EZL and EZL_IST, aggregate early_ninety_day
                Dim query As String = "
                SELECT
                    e.*,
                    MAX(ist.early_ninety_day) AS early_ninety_day
                FROM EZL e
                LEFT JOIN EZL_IST ist ON e.patient_number = ist.patient_number
                WHERE e.patient_number = @patientNumber
                GROUP BY e.patient_number
                LIMIT 1
            "

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
                                .Department = reader("department").ToString(),
                                .EarlyNinetyDay = If(IsDBNull(reader("early_ninety_day")), 0, Convert.ToInt32(reader("early_ninety_day")))
                            }

                            'LogHelper.LogDebugInfo("DBHelper found patient " & patient.PatientNumber & " with Early90Day = " & patient.EarlyNinetyDay)
                            Return patient
                        Else
                            'LogHelper.LogDebugInfo("DBHelper could not find patient: " & patientNumber)
                        End If
                    End Using
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("Error retrieving patient data: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            ' TODO: decide if I want to log this event and others in this module
            'LogHelper.LogDebugInfo("Error in DBHelper.GetPatientByNumber: " & ex.Message)
        End Try

        Return Nothing
    End Function

    ''' <summary>
    ''' Builds and returns a valid SQLite connection string based on the configured database path.
    ''' </summary>
    ''' <returns>A SQLite connection string if the path is valid; otherwise, an empty string.</returns>
    Public Function GetConnectionString() As String
        Dim dbPath As String = PathHelper.GetDatabasePath()

        If String.IsNullOrWhiteSpace(dbPath) OrElse Not File.Exists(dbPath) Then
            MessageBox.Show("SQLite database path not found or file does not exist.", "Config Error")
            Return String.Empty
        End If

        Return $"Data Source={dbPath};Version=3;"
    End Function

    ''' <summary>
    ''' Inserts a new record into the EZL_PRC table.
    ''' </summary>
    ''' <param name="prcData">A dictionary of column-value pairs to insert.</param>
    Public Sub InsertPrcTable(prcData As Dictionary(Of String, Object))
        If prcData Is Nothing OrElse prcData.Count = 0 Then Exit Sub

        Dim dbPath As String = PathHelper.GetDatabasePath()
        Dim connectionString As String = $"Data Source={dbPath};Version=3;"
        Dim insertSuccess As Boolean = False

        Dim columns As String = String.Join(",", prcData.Keys)
        Dim parameters As String = String.Join(",", prcData.Keys.Select(Function(k) "@" & k))
        Dim sql As String = $"INSERT INTO EZL_PRC ({columns}) VALUES ({parameters});"

        ' Attempt insert, retry once if needed
        For attempt As Integer = 1 To 2
            Try
                Using conn As New SQLiteConnection(connectionString)
                    conn.Open()

                    Using cmd As New SQLiteCommand(sql, conn)
                        ' Add parameters
                        For Each kvp In prcData
                            cmd.Parameters.AddWithValue("@" & kvp.Key, If(kvp.Value IsNot Nothing, kvp.Value, DBNull.Value))
                        Next

                        cmd.ExecuteNonQuery()
                    End Using

                    insertSuccess = True
                    Exit For ' Exit loop if success
                End Using

            Catch ex As Exception
                LogHelper.LogError("DatabaseHelper.InsertPrcTable (Attempt " & attempt & ")", ex.Message)
                System.Threading.Thread.Sleep(100) ' Small delay before retry
            End Try
        Next

        ' If insert still failed, show popup
        If Not insertSuccess Then
            MessageBox.Show("Failed to save processed report data to EZL_PRC table after retrying.", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error)
        End If
    End Sub

End Module
