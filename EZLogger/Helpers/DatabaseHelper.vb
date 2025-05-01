Imports System.Data.SqlClient
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
        If String.IsNullOrWhiteSpace(patientNumber) Then Return Nothing

        Dim connStr As String = "Server=LEN-MINI;Database=CoRTReport24;Trusted_Connection=True;"

        Try
            Using conn As New SqlConnection(connStr)
                conn.Open()

                Dim query As String = "
                SELECT
                    PatientNumber,
                    CommitmentDate,
                    AdmissionDate,
                    Expiration,
                    Dob,
                    PatientName,
                    Lname,
                    Fname,
                    Mname,
                    DischargeStatus,  -- renamed from location
                    Program,
                    Unit,
                    Classification,
                    County,
                    Language,
                    Psychiatrist,
                    Evaluator
                    -- TODO: Add early_ninety_day later if/when EZL_IST table is migrated
                FROM EZL
                WHERE PatientNumber = @patientNumber;
            "

                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@patientNumber", patientNumber)

                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            Dim patient As New PatientCls With {
                            .PatientNumber = reader("PatientNumber").ToString(),
                            .CommitmentDate = reader("CommitmentDate").ToString(),
                            .AdmissionDate = reader("AdmissionDate").ToString(),
                            .Expiration = reader("Expiration").ToString(),
                            .DOB = reader("Dob").ToString(),
                            .PatientName = reader("PatientName").ToString(),
                            .LName = reader("Lname").ToString(),
                            .FName = reader("Fname").ToString(),
                            .MName = reader("Mname").ToString(),
                            .Location = reader("DischargeStatus").ToString(),
                            .Program = reader("Program").ToString(),
                            .Unit = reader("Unit").ToString(),
                            .Classification = reader("Classification").ToString(),
                            .County = reader("County").ToString(),
                            .Language = reader("Language").ToString(),
                            .Psychiatrist = reader("Psychiatrist").ToString(),
                            .Evaluator = reader("Evaluator").ToString(),
                            .EarlyNinetyDay = 0 ' placeholder; EZL_IST not yet implemented
                        }
                            Return patient
                        End If
                    End Using
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("SQL Server error: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Return Nothing
    End Function

    ''' <summary>
    ''' Builds and returns a valid SQL connection string based on the configured database path.
    ''' </summary>
    ''' <returns>A SQL connection string if the path is valid; otherwise, an empty string.</returns>
    Public Function GetConnectionString() As String
        Dim dbPath As String = PathHelper.GetDatabasePath()

        If String.IsNullOrWhiteSpace(dbPath) OrElse Not File.Exists(dbPath) Then
            MessageBox.Show("SQL database path not found or file does not exist.", "Config Error")
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
                Using conn As New SqlConnection(connectionString)
                    conn.Open()

                    Using cmd As New SqlCommand(sql, conn)
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

    ''' <summary>
    ''' Formats an 8-digit raw patient number (e.g. "41234567") to display as "123456-7".
    ''' </summary>
    Public Function FormatPatientNumber(rawNumber As String) As String
        If String.IsNullOrWhiteSpace(rawNumber) OrElse rawNumber.Length <> 8 Then
            Return rawNumber
        End If

        Dim body As String = rawNumber.Substring(1, 6)
        Dim checkDigit As String = rawNumber.Substring(7, 1)
        Return $"{body}-{checkDigit}"
    End Function

    ''' <summary>
    ''' Converts a user-friendly patient number (e.g. "123456-7") back into the raw 8-digit format ("41234567").
    ''' </summary>
    Public Function ReverseFormatPatientNumber(formattedNumber As String) As String
        If String.IsNullOrWhiteSpace(formattedNumber) Then Return formattedNumber

        Dim parts = formattedNumber.Split("-"c)
        If parts.Length <> 2 Then Return formattedNumber

        Dim sixDigits = parts(0)
        Dim lastDigit = parts(1)

        If sixDigits.Length <> 6 OrElse lastDigit.Length <> 1 Then
            Return formattedNumber ' invalid format
        End If

        Return "4" & sixDigits & lastDigit
    End Function

End Module
