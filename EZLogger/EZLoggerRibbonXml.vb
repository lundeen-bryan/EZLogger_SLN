Imports EZLogger.Helpers
Imports System.Drawing
Imports System.Windows.Forms


'TODO:  Follow these steps to enable the Ribbon (XML) item:

'1: Copy the following code block into the ThisAddin, ThisWorkbook, or ThisDocument class.

'Protected Overrides Function CreateRibbonExtensibilityObject() As Microsoft.Office.Core.IRibbonExtensibility
'    Return New EZLoggerRibbonXml()
'End Function

'2. Create callback methods in the "Ribbon Callbacks" region of this class to handle user
'   actions, such as clicking a button. Note: if you have exported this Ribbon from the
'   Ribbon designer, move your code from the event handlers to the callback methods and
'   modify the code to work with the Ribbon extensibility (RibbonX) programming model.

'3. Assign attributes to the control tags in the Ribbon XML file to identify the appropriate callback methods in your code.

'For more information, see the Ribbon XML documentation in the Visual Studio Tools for Office Help.

<Runtime.InteropServices.ComVisible(True)>
Public Class EZLoggerRibbonXml
    Implements Office.IRibbonExtensibility

    Private ribbon As Office.IRibbonUI

    Public Sub New()
    End Sub

    Public Function GetCustomUI(ByVal ribbonID As String) As String Implements Office.IRibbonExtensibility.GetCustomUI
        Return GetResourceText("EZLogger.EZLoggerRibbonXml.xml")
    End Function

    Public Sub SettingsButton_Click(control As Office.IRibbonControl)
        'Keep
        Dim configHost As New ConfigHost()
        configHost.ShowDialog()
    End Sub

    Public Sub ReportWizardButton_Click(control As Office.IRibbonControl)

        ' Toggle visibility
        Dim pane = Globals.ThisAddIn.ReportWizardTaskPane

        If pane IsNot Nothing Then
            ' Assign the UserControl if it hasn't been assigned yet
            If Globals.ThisAddIn.ReportWizardTaskPaneContainer Is Nothing Then
                Dim container = TryCast(pane.Control, ReportWizardTaskPaneContainer)
                If container IsNot Nothing Then
                    Globals.ThisAddIn.ReportWizardTaskPaneContainer = container
                End If
            End If

            pane.Visible = Not pane.Visible
        End If

    End Sub

    Public Sub PatientInfoBtn_Click(control As Office.IRibbonControl)
        Dim ptinfo As New PatientInfoHost()
        ptinfo.Show()
    End Sub

    Public Sub AboutButton_Click(control As Office.IRibbonControl)
        Try
            ' Simulate an error (e.g., null reference or bad cast)
            Dim fakeObject As Object = Nothing
            Dim willFail As String = fakeObject.ToString() ' This will throw

        Catch ex As Exception
            Dim errNum As String = ex.HResult.ToString()
            Dim errMsg As String = "Simulated test failure in AboutBtn_Click"
            Dim recommendation As String = "This is just a test. No action needed."

            ErrorHelper.HandleError("Ribbon.AboutBtn_Click", errNum, errMsg, recommendation)
        End Try
        'Keep
        Dim aboutHost As New AboutHost()
        aboutHost.ShowDialog()
    End Sub

    Public Sub EmailButton_Click(control As Office.IRibbonControl)
        Try
            Dim emailHost As New SendEmailHost()
            emailHost.Show()
        Catch ex As Exception
            MsgBox("Failed to open Email window: " & ex.Message)
        End Try
    End Sub

    Public Sub TypoButton_Click(control As Office.IRibbonControl)
        'Keep
    End Sub

    Public Sub CloseDocBtn_Click(control As Office.IRibbonControl)
        DocumentHelper.CloseActiveDocument()
    End Sub

    Public Sub OpenTaskList_Click(control As Microsoft.Office.Core.IRibbonControl)
        Try
            Dim frm As New TaskListHost()
            frm.Show()
        Catch ex As Exception
            ' Ideally use your custom error handler here
            System.Windows.Forms.MessageBox.Show("Error opening Task List: " & ex.Message)
        End Try
    End Sub

    Public Sub RandomPatientNumberButton_Click(control As Microsoft.Office.Core.IRibbonControl)
        ' Part of the test group
        TestHelper.PromptRandomPatientNumberForTest()
    End Sub
    'RemoveMailMerge_Click
    Public Sub RemoveMailMerge_Click(control As Microsoft.Office.Core.IRibbonControl)
        ' Part of the test group
        MailMergeHelper.CleanMailMergeDocument(Globals.ThisAddIn.Application.ActiveDocument)
    End Sub

    Public Sub ExportPdfButton_Click(control As Microsoft.Office.Core.IRibbonControl)
        Try
            ' Get base file name (without extension) first
            Dim baseFileName As String = IO.Path.GetFileNameWithoutExtension(Globals.ThisAddIn.Application.ActiveDocument.Name)

            ' Prompt user with SaveFileDialog
            Using dialog As New SaveFileDialog()
                dialog.Title = "Save PDF As"
                dialog.Filter = "PDF files (*.pdf)|*.pdf"
                dialog.FileName = baseFileName
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)

                If dialog.ShowDialog() = DialogResult.OK Then
                    Dim selectedPath As String = dialog.FileName

                    ' Separate the folder and filename without extension
                    Dim destinationFolder As String = IO.Path.GetDirectoryName(selectedPath)
                    Dim fileNameWithoutExtension As String = IO.Path.GetFileNameWithoutExtension(selectedPath)

                    ' Call Export
                    ExportPdfHelper.ExportActiveDocumentToPdf(destinationFolder, fileNameWithoutExtension)

                    MsgBoxHelper.Show($"Exported to PDF successfully." & vbCrLf & $"File: {selectedPath}")
                Else
                    ' User canceled
                    Exit Sub
                End If
            End Using

        Catch ex As Exception
            MsgBoxHelper.Show("PDF export failed: " & ex.Message)
        End Try
    End Sub



#Region "Ribbon Callbacks"
    'Create callback methods here. For more information about adding callback methods, visit https://go.microsoft.com/fwlink/?LinkID=271226
    Public Sub Ribbon_Load(ByVal ribbonUI As Office.IRibbonUI)
        Me.ribbon = ribbonUI
    End Sub

    Private HiddenButtons As HashSet(Of String) = New HashSet(Of String) From {
        "HelpButton",
        "TypoButton"
    }

    Public Function GetBtnVisibility(control As Office.IRibbonControl) As Boolean
        Return Not HiddenButtons.Contains(control.Id)
    End Function

    Public Function GetConvertButtonImage(control As Office.IRibbonControl) As stdole.IPictureDisp
        ' Load the pdf1.png from project Resources
        Dim bmp As Bitmap = My.Resources.pdf1

        ' Convert it to IPictureDisp
        Return ConvertImageToPictureDisp(bmp)

    End Function

    Public Function GetTypoButtonImage(control As Office.IRibbonControl) As stdole.IPictureDisp
        ' Load the typo.png from project Resources
        Dim bmp As Bitmap = My.Resources.typo

        ' Convert it to IPictureDisp
        Return ConvertImageToPictureDisp(bmp)

    End Function


    Private Function ConvertImageToPictureDisp(image As Image) As stdole.IPictureDisp
        Return CType(AxHostWrapper.GetIPictureDispFromPicture(image), stdole.IPictureDisp)
    End Function

    Private Class AxHostWrapper
        Inherits System.Windows.Forms.AxHost
        Private Sub New()
            MyBase.New(String.Empty)
        End Sub
        Public Shared Function GetIPictureDispFromPicture(image As Image) As Object
            Return AxHost.GetIPictureDispFromPicture(image)
        End Function
    End Class


#End Region

#Region "Helpers"

    Private Shared Function GetResourceText(ByVal resourceName As String) As String
        Dim asm As Reflection.Assembly = Reflection.Assembly.GetExecutingAssembly()
        Dim resourceNames() As String = asm.GetManifestResourceNames()
        For i As Integer = 0 To resourceNames.Length - 1
            If String.Compare(resourceName, resourceNames(i), StringComparison.OrdinalIgnoreCase) = 0 Then
                Using resourceReader As IO.StreamReader = New IO.StreamReader(asm.GetManifestResourceStream(resourceNames(i)))
                    If resourceReader IsNot Nothing Then
                        Return resourceReader.ReadToEnd()
                    End If
                End Using
            End If
        Next
        Return Nothing
    End Function

#End Region

End Class
