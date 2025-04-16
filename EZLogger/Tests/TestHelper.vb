Imports System.Text.Json
Imports MessageBox = System.Windows.MessageBox
Imports EZLogger.Helpers
Imports System.IO

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

End Module
