Imports System.Data.SqlClient
Imports EZLogger.Helpers

Public Module SqlServerTestHandler

    ''' <summary>
    ''' Test connection and query against SQL Server.
    ''' </summary>
    Public Sub RunBasicSqlTest()

        ' Change this to match your local SQL Server instance
        Dim connStr As String = ConfigHelper.GetGlobalConfigValue("database", "connectionString")
        If String.IsNullOrWhiteSpace(connStr) Then
            MsgBoxHelper.Show("SQL Server connection string not found in global_config.json.")
            Exit Sub
        End If

        Try
            Using conn As New SqlConnection(connStr)
                conn.Open()
                System.Diagnostics.Debug.WriteLine("✅ Connected to SQL Server!")

                Dim sql As String = "SELECT TOP 5 PatientNumber, Name FROM EZL"
                Using cmd As New SqlCommand(sql, conn)
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim pn As String = reader("PatientNumber").ToString()
                            Dim name As String = reader("Name").ToString()
                            System.Diagnostics.Debug.WriteLine($"- {pn}: {name}")
                        End While
                    End Using
                End Using
            End Using

        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("❌ Error: " & ex.Message)
        End Try

    End Sub

End Module
