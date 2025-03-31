Imports System.Windows.Forms.Integration
Imports Microsoft.Office.Interop.Word

Public Class ThisAddIn

    Private myTaskPane As Microsoft.Office.Tools.CustomTaskPane
    Private myCoverPagePane As Microsoft.Office.Tools.CustomTaskPane
    Private myDueDateTaskPane As Microsoft.Office.Tools.CustomTaskPane
    Private myOpinionTaskPane As Microsoft.Office.Tools.CustomTaskPane

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

    Public ReadOnly Property DueDateFormPane As Microsoft.Office.Tools.CustomTaskPane
        Get
            Return myDueDateTaskPane
        End Get
    End Property

    Public ReadOnly Property OpinionView As Microsoft.Office.Tools.CustomTaskPane
        Get
            Return myOpinionTaskPane
        End Get
    End Property

    Private Sub ThisAddIn_Startup() Handles Me.Startup
        ' Create an instance of your container UserControl (TaskPaneContainer)
        Dim myTaskPaneContainer As New ReportWizardTaskPaneContainer()
        Dim myCoverPageContainer As New CoverPageWizardPaneContainer()
        Dim myDueDatePaneContainer As New DueDatePaneContainer()
        Dim myOpinionPaneContainer As New OpinionHost()

        ' Set the dock style for the ElementHost inside the container
        myTaskPaneContainer.ElementHost1.Dock = System.Windows.Forms.DockStyle.Fill
        myCoverPageContainer.ElementHost1.Dock = System.Windows.Forms.DockStyle.Fill
        myDueDatePaneContainer.ElementHost1.Dock = System.Windows.Forms.DockStyle.Fill
        myOpinionPaneContainer.ElementHost1.Dock = System.Windows.Forms.DockStyle.Fill

        ' Create an instance of your ReportWizardPanel (WPF control)
        Dim myReportWizardPanel As New ReportWizardPanel()
        Dim myCoverPageWizardPanel As New CoverPageWizardPane()
        Dim myDueDatePane As New DueDateFormPane()
        Dim myOpinionPane As New OpinionView()

        ' Set the ReportWizardPanel as the child of the ElementHost
        myTaskPaneContainer.ElementHost1.Child = myReportWizardPanel
        myCoverPageContainer.ElementHost1.Child = myCoverPageWizardPanel
        myDueDatePaneContainer.ElementHost1.Child = myDueDatePane
        myOpinionPaneContainer.ElementHost1.Child = myOpinionPane

        ' Add the container to the CustomTaskPanes collection with a title
        myTaskPane = Me.CustomTaskPanes.Add(myTaskPaneContainer, "Report Wizard")
        myCoverPagePane = Me.CustomTaskPanes.Add(myCoverPageContainer, "Cover Page Wizard")
        myDueDateTaskPane  = Me.CustomTaskPanes.Add(myDueDatePaneContainer, "Due Date Calculator")
        myOpinionTaskPane = Me.CustomTaskPanes.Add(myOpinionPaneContainer, "Evaluator's Opinion")

        ' Initially, keep the panel hidden
        myTaskPane.Visible = False
        myCoverPagePane.Visible = False
        myDueDateTaskPane.Visible = False
        myOpinionTaskPane.Visible = False
    End Sub

    Private Sub ThisAddIn_Shutdown() Handles Me.Shutdown
        ' Any necessary cleanup can be done here
    End Sub

End Class
