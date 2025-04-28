Imports System.Drawing
Imports System.Reflection
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

<Runtime.InteropServices.ComVisible(True)> _
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
        'Keep this is the main entry point mouse button
        Globals.ThisAddIn.ReportWizardTaskPane.Visible = Not Globals.ThisAddIn.ReportWizardTaskPane.Visible
    End Sub

    Public Sub PatientInfoBtn_Click(control As Office.IRibbonControl)
        Dim ptinfo As New PatientInfoHost()
        ptinfo.Show()
    End Sub

    Public Sub AboutButton_Click(control As Office.IRibbonControl)
        'Keep
        Dim aboutHost As New AboutHost()
        aboutHost.ShowDialog()
    End Sub

    Public Sub EmailButton_Click(control As Office.IRibbonControl)
        'Keep
    End Sub

    Public Sub TypoButton_Click(control As Office.IRibbonControl)
        'Keep
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



#Region "Ribbon Callbacks"
    'Create callback methods here. For more information about adding callback methods, visit https://go.microsoft.com/fwlink/?LinkID=271226
    Public Sub Ribbon_Load(ByVal ribbonUI As Office.IRibbonUI)
        Me.ribbon = ribbonUI
    End Sub

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
