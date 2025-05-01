Imports System.Text.Json
Imports MessageBox = System.Windows.MessageBox
Imports EZLogger.Helpers
Imports System.IO
Imports System.Data
Imports EZLogger.Handlers
Imports System.Windows
Imports System.Data.SqlClient


Module TestHelper
    Public Sub Test_UpdateLocalConfigWithGlobalPath()
        ' Arrange: use a test path (fake file path for now)
        Dim fakeGlobalPath As String = "C:\FakeSharePoint\EZLogger_Databases\global_config.json"

        ' Act: update the local config file
        UpdateLocalConfigWithGlobalPath(fakeGlobalPath)

        ' Assert: read the local config and verify the key was saved
        Dim localConfigPath As String = GetLocalConfigPath()

        If Not File.Exists(localConfigPath) Then
            MessageBox.Show("Test failed: local config file does not exist.")
            Exit Sub
        End If

        Dim json As String = File.ReadAllText(localConfigPath)
        Dim doc = JsonDocument.Parse(json)

        Dim root = doc.RootElement
        Dim spFilepath As JsonElement

        If root.TryGetProperty("sp_filepath", spFilepath) Then
            Dim globalPathElement As JsonElement

            If spFilepath.TryGetProperty("global_config_file", globalPathElement) Then
                Dim result = globalPathElement.GetString()

                If result = fakeGlobalPath Then
                    MessageBox.Show("✅ Test passed: global_config_file saved correctly.")
                Else
                    MessageBox.Show("❌ Test failed: global_config_file value is incorrect." & vbCrLf & "Expected: " & fakeGlobalPath & vbCrLf & "Found: " & result)
                End If
            Else
                MessageBox.Show("❌ Test failed: 'global_config_file' key missing in sp_filepath.")
            End If
        Else
            MessageBox.Show("❌ Test failed: 'sp_filepath' section missing.")
        End If
    End Sub

    ''' <summary>
    ''' Prompts the user with a random patient number and classification.
    ''' If the user accepts, the number is copied to clipboard.
    ''' </summary>
    Public Sub PromptRandomPatientNumberForTest()
        Try
            Dim connStr As String = "Server=LEN-MINI;Database=CoRTReport24;Trusted_Connection=True;"

            Using conn As New SqlConnection(connStr)
                conn.Open()

                Dim keepTrying As Boolean = True

                While keepTrying
                    Dim sql As String = "
                    SELECT TOP 1 PatientNumber, Classification
                    FROM EZL
                    WHERE LTRIM(RTRIM(PatientNumber)) <> ''
                    ORDER BY NEWID()
                "

                    Using cmd As New SqlCommand(sql, conn)
                        Using reader = cmd.ExecuteReader()
                            If reader.Read() Then
                                Dim patientNumber As String = FormatPatientNumber(reader("PatientNumber").ToString())
                                Dim classification As String = reader("classification").ToString()

                                Dim msg As String = $"Patient Number: {patientNumber}" & vbCrLf &
                                                $"Classification: {classification}" & vbCrLf & vbCrLf &
                                                "Use this record for testing?"

                                Dim config As New MessageBoxConfig With {
                                .Message = msg,
                                .ShowYes = True,
                                .ShowNo = True
                            }

                                MsgBoxHelper.Show(config, Sub(result)
                                                              If result = CustomMsgBoxResult.Yes Then
                                                                  Clipboard.SetText(patientNumber)
                                                                  MsgBoxHelper.Show($"Copied '{patientNumber}' to clipboard.")
                                                                  keepTrying = False
                                                              Else
                                                                  keepTrying = True
                                                              End If
                                                          End Sub)

                                Exit While ' prevent loop continuation until dialog returns
                            Else
                                MsgBoxHelper.Show("No patient records found.")
                                keepTrying = False
                            End If
                        End Using
                    End Using
                End While
            End Using

        Catch ex As Exception
            MsgBoxHelper.Show("Test failed: " & ex.Message)
        End Try
    End Sub


End Module
