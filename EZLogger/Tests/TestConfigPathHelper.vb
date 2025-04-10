Imports System.IO
Imports EZLogger.Helpers

Namespace Tests  ' 👈 This matches the folder name

    Public Module TestConfigPathHelper

        Public Sub RunAllTests()
            TestLocalUserConfigPath()
            TestOneDriveOrFallbackPath()
        End Sub

        Private Sub TestLocalUserConfigPath()
            Dim path = ConfigPathHelper.GetLocalUserConfigPath(True)
            LogTestResult("LocalUserConfigPath", path)
        End Sub

        Private Sub TestOneDriveOrFallbackPath()
            Dim path = ConfigPathHelper.LoadOneDriveOrFallbackPath()
            LogTestResult("OneDriveOrFallbackPath", path)
        End Sub

        Private Sub LogTestResult(testName As String, result As String)
            Dim downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads")
            Dim logFile = Path.Combine(downloadsPath, "EZLoggerTestLog.txt")
            Using writer As StreamWriter = File.AppendText(logFile)
                writer.WriteLine($"{DateTime.Now}: {testName} -> {result}")
            End Using
        End Sub

    End Module

End Namespace
