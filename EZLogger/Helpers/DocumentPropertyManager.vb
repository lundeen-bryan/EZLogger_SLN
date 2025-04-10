Imports Microsoft.Office.Interop.Word
Imports Office = Microsoft.Office.Core
Imports System.Windows.Forms
Imports EZLogger.Helpers
Imports Microsoft.Office.Core
Imports System.Diagnostics
Imports EZLogger.ViewModels
Imports EZLogger.Models

Namespace Helpers

    Public Class DocumentPropertyManager

        ''' <summary>
        ''' Writes a single custom document property to the active Word document.
        ''' </summary>
        Public Shared Sub WriteCustomProperty(propertyName As String, value As String)
            Try
                If String.IsNullOrWhiteSpace(propertyName) OrElse String.IsNullOrWhiteSpace(value) Then Exit Sub

                Dim doc As Document = Globals.ThisAddIn.Application.ActiveDocument
                Dim props As Office.DocumentProperties = CType(doc.CustomDocumentProperties, Office.DocumentProperties)

                If props.Cast(Of Office.DocumentProperty).Any(Function(p) p.Name = propertyName) Then
                    props(propertyName).Value = value
                Else
                    props.Add(propertyName, False, Office.MsoDocProperties.msoPropertyTypeString, value)
                End If

            Catch ex As Exception
                MessageBox.Show($"Error writing property '{propertyName}': {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        ''' <summary>
        ''' Writes selected patient details as custom document properties to the active Word document.
        ''' </summary>
        Public Shared Sub WriteDataToDocProperties(patient As PatientCls)
            Try
                WriteCustomProperty("Patient Number", patient.PatientNumber)
                WriteCustomProperty("Patient Name", patient.FullName)
                WriteCustomProperty("Firstname", patient.FName)
                WriteCustomProperty("Lastname", patient.LName)
                WriteCustomProperty("Program", patient.P)
                WriteCustomProperty("Unit", patient.U)
                WriteCustomProperty("Classification", patient.Classification)
                WriteCustomProperty("County", patient.County)
                WriteCustomProperty("DOB", patient.DOB)

                ' Calculate age using helper
                Dim age As String = AgeHelper.CalculateAge(Date.Parse(patient.DOB)).ToString()
                WriteCustomProperty("Age", age)

                WriteCustomProperty("Commitment", patient.CommitmentDate)
                WriteCustomProperty("Admission", patient.AdmissionDate)
                WriteCustomProperty("Expiration", patient.Expiration)
                WriteCustomProperty("Assigned To", patient.AssignedTo)
                WriteCustomProperty("Court Number", patient.CourtNumbers)

                ' Also write the current user info (e.g. "Processed By")
                SenderHelper.WriteProcessedBy()

                MessageBox.Show("Document properties have been updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Catch ex As Exception
                MessageBox.Show("Error writing document properties: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        ''' <summary>
        ''' Clears both built-in and custom document properties.
        ''' </summary>
        Public Shared Sub ClearAllProperties(Optional justPrint As Boolean = False)
            Try
                Dim doc As Document = Globals.ThisAddIn.Application.ActiveDocument

                ' Built-in properties
                For Each prop As DocumentProperty In doc.BuiltInDocumentProperties
                    Try
                        If justPrint Then
                            Debug.Print("BuiltIn: " & prop.Name)
                        Else
                            prop.Value = ""
                            prop.Delete()
                        End If
                    Catch
                        ' Skip read-only or non-removable properties
                    End Try
                Next

                ' Custom properties
                For Each prop As DocumentProperty In doc.CustomDocumentProperties
                    Try
                        If justPrint Then
                            Debug.Print("Custom: " & prop.Name)
                        Else
                            prop.Value = ""
                            prop.Delete()
                        End If
                    Catch
                        ' Skip if error
                    End Try
                Next

                ' Break mail merge connection
                doc.MailMerge.MainDocumentType = WdMailMergeMainDocType.wdNotAMergeDocument

                ' Save document
                doc.Save()

            Catch ex As Exception
                MessageBox.Show("Error clearing document properties: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        ''' <summary>
        ''' Returns the count of custom document properties in the active document.
        ''' </summary>
        Public Shared Function CountCustomProperties() As Integer
            Dim doc As Document = Globals.ThisAddIn.Application.ActiveDocument
            Return doc.CustomDocumentProperties.Count
        End Function

        ''' <summary>
        ''' Loads patient values into the provided ViewModel.
        ''' </summary>
        Public Shared Sub LoadDataIntoViewModel(vm As MainVM, patient As PatientCls)
            vm.PatientNumber = patient.PatientNumber
            vm.FullName = patient.FullName
            vm.FName = patient.FName
            vm.LName = patient.LName
            vm.Program = patient.P
            vm.Unit = patient.U
            vm.Classification = patient.Classification
            vm.County = patient.County
            vm.DOB = patient.DOB
            vm.CommitmentDate = patient.CommitmentDate
            vm.AdmissionDate = patient.AdmissionDate
            vm.Expiration = patient.Expiration
            vm.AssignedTo = patient.AssignedTo
            vm.CourtNumbers = patient.CourtNumbers

            ' Calculate age
            Dim age As String = AgeHelper.CalculateAge(Date.Parse(patient.DOB)).ToString()
            vm.Age = age
        End Sub

        Public Shared Function ReadCustomProperty(propertyName As String) As String
            Try
                Dim doc As Document = Globals.ThisAddIn.Application.ActiveDocument
                Dim props As Office.DocumentProperties = CType(doc.CustomDocumentProperties, Office.DocumentProperties)

                If props.Cast(Of Office.DocumentProperty).Any(Function(p) p.Name = propertyName) Then
                    Return props(propertyName).Value.ToString()
                End If
            Catch ex As Exception
                MessageBox.Show($"Error reading property '{propertyName}': {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

            Return String.Empty
        End Function

    End Class

End Namespace
