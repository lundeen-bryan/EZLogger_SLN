Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Forms

Partial Public Class ReportWizardPanel
    Inherits Controls.UserControl

    Public Sub New()
        InitializeComponent()

        ' Assign event handlers to TaskStepControl instances
        AddHandler TaskStepControlA.TaskButtonClick, AddressOf TaskStepControl_ButtonClick
        AddHandler TaskStepControlB.TaskButtonClick, AddressOf TaskStepControl_ButtonClick
        AddHandler TaskStepControlC.TaskButtonClick, AddressOf TaskStepControl_ButtonClick
        AddHandler TaskStepControlD.TaskButtonClick, AddressOf TaskStepControl_ButtonClick
        AddHandler TaskStepControlE.TaskButtonClick, AddressOf TaskStepControl_ButtonClick
        AddHandler TaskStepControlF.TaskButtonClick, AddressOf TaskStepControl_ButtonClick
        AddHandler TaskStepControlG.TaskButtonClick, AddressOf TaskStepControl_ButtonClick
        AddHandler TaskStepControlH.TaskButtonClick, AddressOf TaskStepControl_ButtonClick
        AddHandler TaskStepControlI.TaskButtonClick, AddressOf TaskStepControl_ButtonClick
        AddHandler TaskStepControlJ.TaskButtonClick, AddressOf TaskStepControl_ButtonClick

        ' Add Loaded event handler for TaskStepControlH
        AddHandler TaskStepControlH.Loaded, AddressOf TaskStepControl_Loaded
    End Sub

    ' Custom event handler for TaskStepControl
    Private Sub TaskStepControl_ButtonClick(sender As Object, e As RoutedEventArgs)
        Dim taskControl As TaskStepControl = CType(sender, TaskStepControl)

        ' Implement your custom logic here based on the ButtonContent
        Select Case taskControl.ButtonContent
            Case "_A"
                ' Custom logic for button A click
            Case "_B"
                ' Custom logic for button B click
            Case "_C"
                ' Custom logic for button C click
            Case "_D"
                ' Custom logic for button D click
            Case "_E"
                ' Custom logic for button E click
            Case "_F"
                ' Custom logic for button F click
            Case "_G"
                ' Custom logic for button G click
            Case "_H"
                Dim approvedForm As New ApprovedByHost()
                approvedForm.StartPosition = FormStartPosition.CenterScreen
                approvedForm.Show()
                ' Custom logic for button H click
            Case "_I"
                ' Custom logic for button I click
            Case "_J"
                ' Custom logic for button J click
            Case Else
                ' Custom logic for unknown button click
        End Select
    End Sub

    ' Loaded event handler for TaskStepControlH
    Private Sub TaskStepControl_Loaded(sender As Object, e As RoutedEventArgs)
        ' Implement your custom logic here for when TaskStepControlH is loaded
    End Sub
End Class