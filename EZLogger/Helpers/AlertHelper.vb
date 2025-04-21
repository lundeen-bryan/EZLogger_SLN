Imports EZLogger.Helpers

Namespace Helpers

    Public Module AlertHelper

        Public Sub ShowCountyAlertIfExists(county As String)
            If String.IsNullOrWhiteSpace(county) Then Exit Sub

            Dim globalPath As String = ConfigHelper.GetGlobalConfigPath()
            If String.IsNullOrWhiteSpace(globalPath) Then Exit Sub

            Dim alerts = ConfigHelper.GetCountyAlerts(globalPath)
            Dim key = county.Trim().ToUpper()

            If alerts.ContainsKey(key) Then
                MsgBox("County Alert for " & county & ":" & vbCrLf & vbCrLf & alerts(key), MsgBoxStyle.Information, "EZLogger Alert")
            End If
        End Sub

        Public Sub ShowPatientAlertIfExists(patientNumber As String)
            If String.IsNullOrWhiteSpace(patientNumber) Then Exit Sub

            Dim globalPath As String = ConfigHelper.GetGlobalConfigPath()
            If String.IsNullOrWhiteSpace(globalPath) Then Exit Sub

            Dim alerts = ConfigHelper.GetPatientAlerts(globalPath)
            Dim key = patientNumber.Trim().ToUpper()

            If alerts.ContainsKey(key) Then
                MsgBox("Patient Alert for " & patientNumber & ":" & vbCrLf & vbCrLf & alerts(key), MsgBoxStyle.Information, "EZLogger Alert")
            End If
        End Sub

    End Module

End Namespace
