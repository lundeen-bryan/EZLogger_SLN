' Namespace=EZLogger/Helpers
' Filename=AlertHelper.vb
' !See Label ProcedureFooter for notes
Option Explicit

Imports EZLogger.Helpers
Imports Microsoft.Office.Interop.Word

Namespace Helpers

    Public Module AlertHelper

        ''' <summary>
        ''' Adds patient and county alerts for the given document into the TaskList (Tasks.xml).
        ''' </summary>
        ''' <param name="doc">The Word document to pull Patient Number and County from.</param>
        Public Sub AddAlertsToTaskList(doc As Document)
        	Dim functionThatCalls As String = "AlertHelper.AddAlertsToTaskList"
            If doc Is Nothing Then Exit Sub

            Try
                ' Step 1: Get patient number and county from document
                Dim patientNumber As String = DocumentPropertyHelper.GetPropertyValue(doc, "Patient Number")
                Dim county As String = DocumentPropertyHelper.GetPropertyValue(doc, "County")

                ' Step 2: Load global config path
                Dim globalConfigPath As String = ConfigHelper.GetGlobalConfigPath()
                If String.IsNullOrWhiteSpace(globalConfigPath) Then Exit Sub

                ' Step 3: Load patient and county alerts
                Dim patientAlerts = ConfigHelper.GetPatientAlerts(globalConfigPath)
                Dim countyAlerts = ConfigHelper.GetCountyAlerts(globalConfigPath)

                ' Step 4: Initialize TaskListHandler
                Dim taskHandler As New TaskListHandler()

                ' Step 5: Add patient alert if exists
                If Not String.IsNullOrWhiteSpace(patientNumber) Then
                    Dim key = patientNumber.Trim().ToUpper()
                    If patientAlerts.ContainsKey(key) Then
                        Dim alertText As String = $"Patient Alert for {patientNumber}: {patientAlerts(key)}"
                        alertText = alertText.Replace(vbCrLf, "; ") ' Reformat line breaks
                        taskHandler.AddTaskFromReport(alertText)
                    End If
                End If

                ' Step 6: Add county alert if exists
                If Not String.IsNullOrWhiteSpace(county) Then
                    Dim key = county.Trim().ToUpper()
                    If countyAlerts.ContainsKey(key) Then
                        Dim alertText As String = $"County Alert for {county}: {countyAlerts(key)}"
                        alertText = alertText.Replace(vbCrLf, "; ") ' Reformat line breaks
                        taskHandler.AddTaskFromReport(alertText)
                    End If
                End If

            Catch ex As Exception
                Dim errNum As String = ex.HResult.ToString()
                Dim errMsg As String = CStr(ex.Message)
                Dim recommendation As String = "There was a problem trying to add an alert to the task list. Please see help file."

                ErrorHelper.HandleError(functionThatCalls, errNum, errMsg, recommendation)
            End Try
        End Sub

        ''' <summary>
        ''' Searches for a County alert and displayes to user
        ''' </summary>
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

        ''' <summary>
        ''' Shows an alert for the current patient to user
        ''' </summary>
        ''' <remarks>Based on the patient number in the global_config.json</remarks>
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

' Footer:
''===========================================================================================
'' Procedure: ......... AlertHelper.vb
'' Description: ....... Gets alerts from global_config.json
'' Created: ........... 2025-05-12
'' Updated: ........... 2025-05-12
'' Installs to: ....... EZLogger/Helpers
'' Contact Author: .... lundeen-bryan
'' Copyright:  ........ n/a ©2025. All rights reserved.
'' Notes: ............. _
 '(1) notes_here
''===========================================================================================