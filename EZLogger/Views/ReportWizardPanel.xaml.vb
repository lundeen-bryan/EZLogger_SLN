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
                Dim customMsgBox As New CustomMsgBox()
                customMsgBox.StartPosition = FormStartPosition.CenterScreen
                CustomMsgBox.Show()
                CustomMsgBox.TextBoxMessageToUser.Text = "EZ Logger should search the footer to find the napa ID number and show it to the user for confirmation."
            Case "_B"
                Dim confirmMatch As New ConfirmPatientMatch()
                confirmMatch.StartPosition = FormStartPosition.CenterScreen
                confirmMatch.Show()
            Case "_C"
                Globals.ThisAddIn.DueDateFormPane.Visible = Not Globals.ThisAddIn.DueDateFormPane.Visible
            Case "_D"
                ' This section is for looking up the patient in the TCAR Log
                Dim tcarLogTodo As New CustomMsgBox()
                tcarLogTodo.StartPosition = FormStartPosition.CenterScreen
                tcarLogTodo.Show()
                tcarLogTodo.TextBoxMessageToUser.Text = "TODO: code a database connection to the TCAR Log"
            Case "_E"
                ' Custom logic for button E click
                Dim ConrepTodo As New CustomMsgBox()
                ConrepTodo.StartPosition = FormStartPosition.CenterScreen
                ConrepTodo.Show()
                ConrepTodo.TextBoxMessageToUser.Text = "TODO: code a connection to a database that shows who is the CONREP for the patient"
            Case "_F"
                ' Show the Choose report opinion task panel
                Globals.ThisAddIn.OpinionControl.Visible = Not Globals.ThisAddIn.OpinionControl.Visible
            Case "_G"
                ' Custom logic for button G click
            Case "_H"
                Dim approvedForm As New ApprovedByHost()
                approvedForm.StartPosition = FormStartPosition.CenterScreen
                approvedForm.Show()
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