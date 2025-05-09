﻿Imports System.Windows.Forms
Imports EZLogger.Helpers
Imports Haley.Abstractions
Imports Haley.Models
Imports Microsoft.Office.Core
Imports Microsoft.Office.Interop.Word

''' <summary>
''' Helper for updating SharePoint (SP) ContentTypeProperties from Word custom document properties.
''' </summary>
Public Module SpHelper

    ''' <summary>
    ''' Updates SharePoint metadata fields based on matching custom document properties.
    ''' </summary>
    ''' <param name="doc">The Word document to update.</param>
    Public Sub UpdateMetadata(ByVal doc As Microsoft.Office.Interop.Word.Document)
        Try
            If doc Is Nothing Then
                Throw New Exception("Word Document is not available on OneDrive yet. Please wait for OneDrive to sync the file.")
            End If

            ' Map custom property names to SharePoint field names
            Dim customToSharePointMap As New Dictionary(Of String, String) From {
                {"Court Number", "Court Number"},
                {"Charges", "Charges"},
                {"Patient Number", "NAID"},
                {"Patient Name", "Patient Name"},
                {"Program", "Program"},
                {"Unit", "Unit"},
                {"Classification", "Classification"},
                {"County", "County"},
                {"Expiration", "Expiration"},
                {"Age", "Age"},
                {"DOB", "DOB"},
                {"Commitment", "Commitment"},
                {"Gender", "Gender (on record)"},
                {"Admission", "Admission"},
                {"Assigned To", "AssignedTo"},
                {"Unique ID", "Unique ID"},
                {"Evaluator", "Evaluator"},
                {"Approved By", "Approved By"},
                {"Report Date", "Report Date"},
                {"Due Date", "Due Date"},
                {"Days Since Due", "Days Since Due Date (or before due)"},
                {"Rush Status", "Rush Status"},
                {"Processed By", "Processed By"},
                {"Report Type", "Report Type"},
                {"Pages", "Pages"},
                {"Report Cycle", "Report Cycle"}
            }

            ' Only update if in SharePoint location
            If doc.Path = String.Empty OrElse Not doc.Path.StartsWith("https://", StringComparison.OrdinalIgnoreCase) Then
                Exit Sub
            End If

            ' Update SharePoint fields from matching custom properties
            For Each customProp As DocumentProperty In doc.CustomDocumentProperties
                If customToSharePointMap.ContainsKey(customProp.Name) Then
                    Dim targetName As String = customToSharePointMap(customProp.Name)

                    For Each spProp As MetaProperty In doc.ContentTypeProperties
                        If spProp.Name = targetName AndAlso Not String.IsNullOrWhiteSpace(customProp.Value.ToString()) Then
                            If targetName = "Approved By" Then
                                spProp.Value = "Dr. " & StrConv(customProp.Value.ToString(), VbStrConv.ProperCase)
                            Else
                                spProp.Value = customProp.Value.ToString()
                            End If
                            Exit For
                        End If
                    Next
                End If
            Next

        Catch ex As Exception
            Dim errNum As String = ex.HResult.ToString()
            Dim errMsg As String = CStr(ex.Message)
            Dim recommendation As String = "Please ensure the file has synced with OneDrive and try again."

            ErrorHelper.HandleError("SpHelper.UpdateMetadata", errNum, errMsg, recommendation)
        End Try
    End Sub

End Module
