' Imports System.Data.SQLite
' Imports System.Diagnostics
' Imports System.Windows.Forms
' Imports EZLogger.Helpers

' Namespace Handlers

'     Public Class PatientDatabaseHandler

'         Public Shared Function GetPatientByNumber(patientNumber As String) As PatientCls
'             ' TODO check if this Handler is called by anything or if it's defunct - DatabaseHelper might be better 04/12-BL
'             If String.IsNullOrWhiteSpace(patientNumber) Then
'                 Return Nothing
'             End If

'             Try
'                 Dim dbPath As String = GetDatabasePath()
'                 Dim connectionString As String = $"Data Source={dbPath};Version=3;"

'                 Using conn As New SQLiteConnection(connectionString)
'                     conn.Open()

'                     Dim query As String = "
'                 SELECT
'                     e.patient_number,
'                     e.full_name,
'                     e.county,
'                     MAX(ist.early_ninety_day) AS early_ninety_day
'                 FROM EZL e
'                 LEFT JOIN EZL_IST ist ON e.patient_number = ist.patient_number
'                 WHERE e.patient_number = @patientNumber
'                 GROUP BY e.patient_number, e.full_name, e.county
'             "

'                     Using cmd As New SQLiteCommand(query, conn)
'                         cmd.Parameters.AddWithValue("@patientNumber", patientNumber)

'                         Using reader As SQLiteDataReader = cmd.ExecuteReader()
'                             If reader.Read() Then
'                                 Dim patient As New PatientCls With {
'                             .PatientNumber = reader("patient_number").ToString(),
'                             .FullName = reader("full_name").ToString(),
'                             .County = reader("county").ToString()
'                         }

'                                 If Not IsDBNull(reader("early_ninety_day")) Then
'                                     patient.EarlyNinetyDay = Convert.ToInt32(reader("early_ninety_day"))
'                                 Else
'                                     patient.EarlyNinetyDay = 0
'                                 End If

'                                 ' Logging for debug
'                                 LogHelper.LogDebugInfo("Retrieved patient: " & patient.PatientNumber & ", Early90Day: " & patient.EarlyNinetyDay)

'                                 Return patient
'                             Else
'                                 LogHelper.LogDebugInfo("No patient found with number: " & patientNumber)
'                             End If
'                         End Using
'                     End Using
'                 End Using

'             Catch ex As Exception
'                 LogHelper.LogDebugInfo("Error in GetPatientByNumber: " & ex.Message)
'                 Windows.MessageBox.Show("Error retrieving patient data: " & ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
'             End Try

'             Return Nothing
'         End Function

'         Private Shared Function GetDatabasePath() As String
'             ' TODO: Replace with JSON config reader later
'             Return "C:\Users\lunde\repos\cs\ezlogger\EZLogger_SLN\data\ezlogger.db"
'         End Function

'     End Class

' End Namespace
