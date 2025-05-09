Imports EZLogger.Helpers
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.IO
Imports System.Windows
Imports System.Windows.Forms
Imports System.Threading
Imports MessageBox = System.Windows.MessageBox


Public Module DatabaseHelper

    ''' <summary>
    ''' Converts a SQL value to a short date string (MM/dd/yyyy) if it's a valid date.
    ''' Returns an empty string if the value is DBNull or not a date.
    ''' </summary>
    Private Function FormatDate(value As Object) As String
        If value IsNot DBNull.Value Then
            Dim dt As DateTime
            If DateTime.TryParse(value.ToString(), dt) Then
                Return dt.ToString("MM/dd/yyyy")
            End If
        End If
        Return ""
    End Function

    ''' <summary>
    ''' Retrieves the CourtNumber for a given patient using the uspEZL_CTN stored procedure.
    ''' </summary>
    ''' <param name="patientNumber">The patient number to search for.</param>
    ''' <returns>The CourtNumber string if found; otherwise, an empty string.</returns>
    Public Function GetCourtNumberByPatientNumber(patientNumber As String) As String
        If String.IsNullOrWhiteSpace(patientNumber) Then Return String.Empty

        Dim connStr As String = ConfigHelper.GetGlobalConfigValue("database", "connectionString")
        If String.IsNullOrWhiteSpace(connStr) Then
            MessageBox.Show("SQL Server connection string not found in global_config.json.", "Missing Config", MessageBoxButton.OK, MessageBoxImage.Error)
            Return String.Empty
        End If

        Try
            Using conn As New SqlConnection(connStr)
                conn.Open()

                Using cmd As New SqlCommand("uspEZL_CTN", conn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.AddWithValue("@PatientNumber", patientNumber)

                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            ' Safe null check; will return empty string if DBNull
                            Return If(reader("CourtNumber") IsNot DBNull.Value, reader("CourtNumber").ToString(), "")
                        End If
                    End Using
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("SQL Server error while retrieving Court Number: " & ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error)
        End Try

        Return String.Empty
    End Function


    ''' <summary>
    ''' Retrieves a single patient record matching the given patient number.
    ''' </summary>
    ''' <param name="patientNumber">The patient number to search for.</param>
    ''' <returns>A PatientCls object if found; otherwise, Nothing.</returns>
    Public Function GetPatientByNumber(patientNumber As String) As PatientCls
        If String.IsNullOrWhiteSpace(patientNumber) Then Return Nothing

        Dim connStr As String = ConfigHelper.GetGlobalConfigValue("database", "connectionString")
        If String.IsNullOrWhiteSpace(connStr) Then
            MessageBox.Show("SQL Server connection string not found in global_config.json.", "Missing Config", MessageBoxButton.OK, MessageBoxImage.Error)
            Exit Function
        End If

        Try
            Using conn As New SqlConnection(connStr)
                conn.Open()

                Dim query As String = "
                SELECT
                    PatientNumber,
                    FirstPatientNumber,
                    CommitmentDate,
                    AdmissionDate,
                    Expiration,
                    Dob,
                    PatientName,
                    Lname,
                    Fname,
                    Mname,
                    BedStatus,  -- renamed from location
                    Program,
                    Unit,
                    Classification,
                    County,
                    Language,
                    Psychiatrist,
                    Evaluator,
                    Sex
                    -- TODO: Add early_ninety_day later if/when EZL_IST table is migrated
                FROM EZL
                WHERE PatientNumber = @patientNumber;
            "

                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@patientNumber", patientNumber)

                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        '^--Reader pulls the following data from PatientCls
                        If reader.Read() Then
                            Dim patient As New PatientCls With {
                            .PatientNumber = reader("PatientNumber").ToString(),
                            .FirstPatientNumber = reader("FirstPatientNumber").ToString(),
                            .CommitmentDate = FormatDate(reader("CommitmentDate")),
                            .AdmissionDate = FormatDate(reader("AdmissionDate")),
                            .Expiration = FormatDate(reader("Expiration")),
                            .DOB = FormatDate(reader("Dob")),
                            .PatientName = reader("PatientName").ToString(),
                            .LName = reader("Lname").ToString(),
                            .FName = reader("Fname").ToString(),
                            .MName = reader("Mname").ToString(),
                            .BedStatus = reader("BedStatus").ToString(),
                            .Program = reader("Program").ToString(),
                            .Unit = reader("Unit").ToString(),
                            .Classification = reader("Classification").ToString(),
                            .County = reader("County").ToString(),
                            .Language = reader("Language").ToString(),
                            .Psychiatrist = reader("Psychiatrist").ToString(),
                            .Evaluator = reader("Evaluator").ToString(),
                            .Sex = reader("Sex").ToString(),
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
    ''' Inserts a new record into the EZL_PRC table and updates the "PrcInserted" property on the given Word document.
    ''' </summary>
    ''' <param name="prcData">A dictionary of column-value pairs to insert.</param>
    ''' <param name="doc">The active Word document to update with the PrcInserted property.</param>
    Public Sub InsertPrcTable(prcData As Dictionary(Of String, Object), doc As Document)

        If prcData Is Nothing OrElse prcData.Count = 0 Then Exit Sub
        If doc Is Nothing Then Exit Sub

        ' Step 1: Check if already inserted
        Dim existingProp As String = DocumentPropertyHelper.GetPropertyValue("PrcInserted")

        If Not String.IsNullOrEmpty(existingProp) AndAlso existingProp.Trim().ToLower() = "true" Then
            MessageBox.Show("This report has already been logged in the PRC table.", "Already Inserted", MessageBoxButton.OK, MessageBoxImage.Warning)
            Exit Sub
        End If

        ' Step 2: Get connection string
        Dim connectionString As String = ConfigHelper.GetGlobalConfigValue("database", "connectionString")
        If String.IsNullOrWhiteSpace(connectionString) Then
            MessageBox.Show("SQL Server connection string not found in global_config.json.", "Missing Config", MessageBoxButton.OK, MessageBoxImage.Error)
            Exit Sub
        End If

        ' Step 3: Build SQL insert
        Dim insertSuccess As Boolean = False
        Dim columns As String = String.Join(",", prcData.Keys)
        Dim parameters As String = String.Join(",", prcData.Keys.Select(Function(k) "@" & k))
        Dim sql As String = $"INSERT INTO EZL_PRC ({columns}) VALUES ({parameters});"

        ' Step 4: Try insert with one retry
        For attempt As Integer = 1 To 2
            Try
                Using conn As New SqlConnection(connectionString)
                    conn.Open()
                    Using cmd As New SqlCommand(sql, conn)
                        For Each kvp In prcData
                            cmd.Parameters.AddWithValue("@" & kvp.Key, If(kvp.Value IsNot Nothing, kvp.Value, DBNull.Value))
                        Next
                        cmd.ExecuteNonQuery()
                    End Using
                    insertSuccess = True
                    Exit For
                End Using

            Catch ex As Exception
                Dim debugInfo As String = $"Attempt {attempt} failed: {ex.Message}" & vbCrLf &
                                          $"SQL: {sql}" & vbCrLf &
                                          $"Parameters:" & vbCrLf &
                                          String.Join(vbCrLf, prcData.Select(Function(kvp) $"{kvp.Key} = {kvp.Value}"))

                ErrorHelper.HandleError("DatabaseHelper.InsertPrcTable", ex.HResult.ToString(), debugInfo,
                                        "Please confirm the patient number from the report to make sure it matches a patient in ForensicInfo.")

#If DEBUG Then
                MessageBox.Show(debugInfo, "SQL Insert Debug", MessageBoxButton.OK, MessageBoxImage.Warning)
#End If
                Thread.Sleep(100)
            End Try
        Next

        ' Step 5: After successful insert, write doc property and notify user
        If insertSuccess Then
            DocumentPropertyHelper.WriteCustomProperty(doc, "PrcInserted", "true")
            MessageBox.Show("Report successfully logged to the PRC table.", "Success", MessageBoxButton.OK, MessageBoxImage.Information)
        Else
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
