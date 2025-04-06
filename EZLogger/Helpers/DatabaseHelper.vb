Imports System.Data.SQLite

' Simple model to hold patient data
Public Class Patient
    Public Property PatientNumber As String
    Public Property FullName As String
    Public Property ClassName As String
    Public Property County As String
    Public Property BedStatus As String
End Class

' Database helper class to read from SQLite
Public Class DatabaseHelper
    Private ReadOnly _dbPath As String

    Public Sub New(dbPath As String)
        _dbPath = dbPath
    End Sub

    Public Function GetAllPatients() As List(Of Patient)
        Dim patients As New List(Of Patient)()

        Using conn As New SQLiteConnection($"Data Source={_dbPath};Version=3;")
            conn.Open()

            Dim query As String = "
                SELECT patient_number, fullname, class, county, bed_status
                FROM EZL
                ORDER BY fullname
            "

            Using cmd As New SQLiteCommand(query, conn)
                Using reader As SQLiteDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim p As New Patient With {
                            .PatientNumber = reader("patient_number").ToString(),
                            .FullName = reader("fullname").ToString(),
                            .ClassName = reader("class").ToString(),
                            .County = reader("county").ToString(),
                            .BedStatus = reader("bed_status").ToString()
                        }
                        patients.Add(p)
                    End While
                End Using
            End Using
        End Using

        Return patients
    End Function
End Class
