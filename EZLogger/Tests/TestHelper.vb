Imports System.Text.Json
Imports MessageBox = System.Windows.MessageBox
Imports EZLogger.Helpers
Imports System.IO
Imports System.Data
Imports EZLogger.Handlers
Imports System.Windows

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
            Dim connectionString As String = DatabaseHelper.GetConnectionString()
            If String.IsNullOrEmpty(connectionString) Then
                MsgBoxHelper.Show("Database not found.")
                Exit Sub
            End If

            Using conn As New SQLite.SQLiteConnection(connectionString)
                conn.Open()

                Dim keepTrying As Boolean = True

                While keepTrying
                    Dim sql As String = "
                    SELECT patient_number, classification
                    FROM EZL
                    WHERE TRIM(patient_number) <> ''
                    ORDER BY RANDOM()
                    LIMIT 1"
                    Using cmd As New SQLite.SQLiteCommand(sql, conn)
                        Using reader = cmd.ExecuteReader()
                            If reader.Read() Then
                                Dim patientNumber As String = FormatPatientNumber(reader("patient_number").ToString())
                                Dim classification As String = reader("classification").ToString()

                                ' Build and show confirmation prompt
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
                                                                  ' Try again
                                                                  keepTrying = True
                                                              End If
                                                          End Sub)

                                ' Wait for the dialog to return
                                Exit While ' MsgBox is async, so prevent loop continuation here
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
