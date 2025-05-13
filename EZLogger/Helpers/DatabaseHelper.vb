Imports EZLogger.Helpers
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.IO
Imports System.Threading
Imports System.Windows
Imports System.Windows.Forms
Imports MessageBox = System.Windows.MessageBox

Public Module DatabaseHelper

    ''' <summary>
    ''' Builds an SQL INSERT command string for the EZL_PRC table using the provided data.
    ''' </summary>
    ''' <param name="prcData">A dictionary containing column names as keys and their corresponding values.</param>
    ''' <returns>
    ''' A string representing the SQL INSERT command with placeholders for parameterized values.
    ''' Example: "INSERT INTO EZL_PRC (Column1, Column2) VALUES (@Column1, @Column2);"
    ''' </returns>
    ''' <remarks>
    ''' This function dynamically generates the SQL command based on the keys in the provided dictionary.
    ''' Ensure that the keys in the dictionary match the column names in the EZL_PRC table.
    ''' </remarks>
    Private Function BuildInsertCommand(prcData As Dictionary(Of String, Object)) As String
        Dim columns As String = String.Join(",", prcData.Keys)
        Dim parameters As String = String.Join(",", prcData.Keys.Select(Function(k) "@" & k))
        Return $"INSERT INTO EZL_PRC ({columns}) VALUES ({parameters});"
    End Function

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
    ''' Determines the appropriate SQL data type (SqlDbType) for a given column name.
    ''' </summary>
    ''' <param name="columnName">The name of the column for which the SQL data type is required.</param>
    ''' <returns>
    ''' A SqlDbType value corresponding to the column name. Defaults to SqlDbType.NVarChar
    ''' if the column name does not match any predefined cases.
    ''' </returns>
    Private Function GetSqlDbTypeForColumn(columnName As String) As SqlDbType
        Select Case columnName
            Case "DueDate", "ReportDate", "Created", "Admission", "Expiration", "Dob", "TCAR"
                Return SqlDbType.Date
            Case "DueDateOffset", "TcarOffset", "Pages", "Age"
                Return SqlDbType.Int
            Case Else
                Return SqlDbType.NVarChar
        End Select
    End Function

    ''' <summary>
    ''' Inserts a new record into the EZL_PRC table.
    ''' </summary>
    ''' <param name="prcData">A dictionary of column-value pairs to insert.</param>
    Public Sub InsertPrcTable(prcData As Dictionary(Of String, Object))
        Dim wordDoc As Word.Document = GetActiveWordDocument()
        InitializeVsto(wordDoc)

        If prcData Is Nothing Then
            Throw New ArgumentNullException(NameOf(prcData), "The prcData parameter cannot be null.")
        End If

        If prcData.Count = 0 Then
            Throw New ArgumentException("The prcData parameter cannot be an empty dictionary.", NameOf(prcData))
        End If

        Dim connectionString As String = ConfigHelper.GetGlobalConfigValue("database", "connectionString")
        If String.IsNullOrWhiteSpace(connectionString) Then
            MessageBox.Show("SQL Server connection string not found in global_config.json.", "Missing Config", MessageBoxButton.OK, MessageBoxImage.Error)
            Exit Sub
        End If

        Dim sql As String = BuildInsertCommand(prcData)
        Dim insertSuccess As Boolean = TryExecuteInsert(connectionString, sql, prcData)

        If insertSuccess Then
            MsgBoxHelper.Show("Report successfully logged to the PRC table.")
        Else
            MessageBox.Show("Failed to save processed report data to the PRC table after retrying.", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error)
        End If
    End Sub

    ''' <summary>
    ''' Normalizes raw prcData values into correct types (Date, Int, Bit, etc.)
    ''' </summary>
    ''' <param name="input">Raw dictionary of property values from document</param>
    ''' <returns>A cleaned and type-safe dictionary ready for SQL insertion</returns>
    Public Function NormalizePrcData(input As Dictionary(Of String, Object)) As Dictionary(Of String, Object)
        Dim normalized As New Dictionary(Of String, Object)()

        For Each kvp In input
            Dim key As String = kvp.Key
            Dim rawValue As Object = kvp.Value
            Dim normalizedValue As Object

            ' Handle null/empty first
            If rawValue Is Nothing OrElse String.IsNullOrWhiteSpace(rawValue.ToString()) Then
                normalizedValue = DBNull.Value

            Else
                Select Case key
                    Case "DueDate", "ReportDate", "Created", "Admission", "Expiration", "Dob", "Commitment", "TCAR"
                        ' Try to parse date
                        Dim dt As DateTime
                        If DateTime.TryParse(rawValue.ToString(), dt) Then
                            normalizedValue = dt.Date
                        Else
                            normalizedValue = DBNull.Value
                        End If

                    Case "DueDateOffset", "TcarOffset", "Pages", "Age"
                        Dim i As Integer
                        If Integer.TryParse(rawValue.ToString(), i) Then
                            normalizedValue = i
                        Else
                            normalizedValue = DBNull.Value
                        End If

                    Case "Malingering", "IMO", "MinuteOrder", "JBCT"
                        ' Accept "Y"/"N" or true/false or blank
                        Dim str = rawValue.ToString().Trim().ToUpper()
                        If str = "Y" OrElse str = "YES" OrElse str = "TRUE" Then
                            normalizedValue = True
                        ElseIf str = "N" OrElse str = "NO" OrElse str = "FALSE" Then
                            normalizedValue = False
                        Else
                            normalizedValue = DBNull.Value
                        End If

                    Case Else
                        ' Treat everything else as string
                        normalizedValue = rawValue.ToString()
                End Select
            End If

            normalized(key) = normalizedValue
        Next

        Return normalized
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

    ''' <summary>
    ''' Attempts to execute an SQL INSERT command with the provided connection string, SQL query, and data.
    ''' Retries the operation up to two times in case of failure.
    ''' </summary>
    ''' <param name="connectionString">The connection string to the SQL Server database.</param>
    ''' <param name="sql">The SQL INSERT command to execute.</param>
    ''' <param name="prcData">A dictionary containing column names as keys and their corresponding values to insert.</param>
    ''' <returns>
    ''' True if the SQL INSERT command is successfully executed; otherwise, False.
    ''' </returns>
    ''' <remarks>
    ''' This function handles exceptions during the SQL execution and logs detailed debug information
    ''' in case of failure. It retries the operation once before returning False.
    ''' </remarks>
    Private Function TryExecuteInsert(connectionString As String, sql As String, prcData As Dictionary(Of String, Object)) As Boolean
        For attempt As Integer = 1 To 2
            Try
                Using conn As New SqlConnection(connectionString)
                    conn.Open()
                    Using cmd As New SqlCommand(sql, conn)
                        For Each kvp In prcData
                            Dim param As SqlParameter = cmd.Parameters.Add("@" & kvp.Key, GetSqlDbTypeForColumn(kvp.Key))
                            If kvp.Value Is Nothing OrElse TypeOf kvp.Value Is DBNull Then
                                param.Value = DBNull.Value
                            Else
                                param.Value = kvp.Value
                            End If
                        Next
                        cmd.ExecuteNonQuery()
                    End Using
                End Using
                Return True

            Catch ex As Exception
                Dim debugInfo As String = $"Attempt {attempt} failed: {ex.Message}" & vbCrLf &
                                      $"SQL: {sql}" & vbCrLf &
                                      $"Parameters:" & vbCrLf &
                                      String.Join(vbCrLf, prcData.Select(Function(kvp) $"{kvp.Key} = {kvp.Value}"))

                ErrorHelper.HandleError("DatabaseHelper.TryExecuteInsert", ex.HResult.ToString(), debugInfo,
                                    "SQL insert failed. Confirm patient information is complete and retry.")

#If DEBUG Then
                MessageBox.Show(debugInfo, "SQL Insert Debug", MessageBoxButton.OK, MessageBoxImage.Warning)
#End If
                Thread.Sleep(100)
            End Try
        Next
        Return False
    End Function

End Module
