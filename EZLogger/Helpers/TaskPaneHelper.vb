Namespace Helpers

    Public Module TaskPaneHelper

        Private _currentTaskPane As ReportWizardPanel

        ''' <summary>
        ''' Stores the active ReportWizardPanel so views and handlers can reference it later.
        ''' </summary>
        Public Sub SetTaskPane(panel As ReportWizardPanel)
            _currentTaskPane = panel
        End Sub

        ''' <summary>
        ''' Gets the current active ReportWizardPanel.
        ''' </summary>
        Public Function GetTaskPane() As ReportWizardPanel
            Return _currentTaskPane
        End Function

        ''' <summary>
        ''' Clears the reference to the task pane (optional).
        ''' </summary>
        Public Sub ClearTaskPane()
            _currentTaskPane = Nothing
        End Sub

    End Module

End Namespace
