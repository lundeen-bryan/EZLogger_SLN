Public Class ThisAddIn

    Private myTaskPane As Microsoft.Office.Tools.CustomTaskPane
    Public Property ReportWizardTaskPaneContainer As ReportWizardTaskPaneContainer

    Public ReadOnly Property ReportWizardTaskPane As Microsoft.Office.Tools.CustomTaskPane
        Get
            Return myTaskPane
        End Get
    End Property

    Private Sub ThisAddIn_Startup() Handles Me.Startup
        ' Create an instance of your container UserControl (TaskPaneContainer)
        Dim myTaskPaneContainer As New ReportWizardTaskPaneContainer()

        ' Set the dock style for the ElementHost inside the container
        myTaskPaneContainer.ElementHost1.Dock = System.Windows.Forms.DockStyle.Fill

        ' Create an instance of your ReportWizardPanel (WPF control)
        Dim myReportWizardPanel As New ReportWizardPanel()

        ' Set the ReportWizardPanel as the child of the ElementHost
        myTaskPaneContainer.ElementHost1.Child = myReportWizardPanel

        ' Add the container to the CustomTaskPanes collection with a title
        myTaskPane = Me.CustomTaskPanes.Add(myTaskPaneContainer, "EZ Logger Report Wizard")

        ' Set the initial size of the task pane
        myTaskPane.Width = 440

        ' Initially, keep the panel hidden
        myTaskPane.Visible = False
    End Sub

    Private Sub ThisAddIn_Shutdown() Handles Me.Shutdown
        ' Any necessary cleanup can be done here
    End Sub

    Protected Overrides Function CreateRibbonExtensibilityObject() As Office.IRibbonExtensibility
        Return New EZLoggerRibbonXml()
    End Function

End Class
