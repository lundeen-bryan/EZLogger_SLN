Imports System.Windows.Forms.Integration
Imports Microsoft.Office.Interop.Word

Public Class ThisAddIn

    Private myTaskPane As Microsoft.Office.Tools.CustomTaskPane
    Private myCoverPagePane As Microsoft.Office.Tools.CustomTaskPane

    Public ReadOnly Property ReportWizardTaskPane As Microsoft.Office.Tools.CustomTaskPane
        Get
            Return myTaskPane
        End Get
    End Property

    Public ReadOnly Property CoverWizardTaskPane As Microsoft.Office.Tools.CustomTaskPane
        Get
            Return myCoverPagePane
        End Get
    End Property

    Private Sub ThisAddIn_Startup() Handles Me.Startup
        ' Create an instance of your container UserControl (TaskPaneContainer)
        Dim myTaskPaneContainer As New ReportWizardTaskPaneContainer()
        Dim myCoverPageContainer As New CoverPageWizardPaneContainer()

        ' Set the dock style for the ElementHost inside the container
        myTaskPaneContainer.ElementHost1.Dock = System.Windows.Forms.DockStyle.Fill
        myCoverPageContainer.ElementHost1.Dock = System.Windows.Forms.DockStyle.Fill

        ' Create an instance of your ReportWizardPanel (WPF control)
        Dim myReportWizardPanel As New ReportWizardPanel()
        Dim myCoverPageWizardPanel As New CoverPageWizardPane()

        ' Set the ReportWizardPanel as the child of the ElementHost
        myTaskPaneContainer.ElementHost1.Child = myReportWizardPanel
        myCoverPageContainer.ElementHost1.Child = myCoverPageWizardPanel

        ' Add the container to the CustomTaskPanes collection with a title
        myTaskPane = Me.CustomTaskPanes.Add(myTaskPaneContainer, "Report Wizard")
        myCoverPagePane = Me.CustomTaskPanes.Add(myCoverPageContainer, "Cover Page Wizard")

        ' Initially, keep the panel hidden
        myTaskPane.Visible = False
        myCoverPagePane.Visible = False
    End Sub

    Private Sub ThisAddIn_Shutdown() Handles Me.Shutdown
        ' Any necessary cleanup can be done here
    End Sub

End Class
