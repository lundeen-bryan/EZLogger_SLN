Imports System.Windows
Imports EZLogger.Helpers
Imports EZLogger.Handlers
Imports UserControl = System.Windows.Controls.UserControl

Partial Public Class ReportWizardPanel
    Inherits UserControl

    Private ReadOnly _handler As ReportWizardHandler

    Public Sub New()
        InitializeComponent()
        _handler = New ReportWizardHandler()
        WireUpButtons()
    End Sub

    Private Sub WireUpButtons()
        AddHandler Btn_A.Click, AddressOf Btn_A_Click
        AddHandler Btn_B.Click, AddressOf Btn_B_Click
        AddHandler Btn_C.Click, AddressOf Btn_C_Click
        AddHandler Btn_D.Click, AddressOf Btn_D_Click
        AddHandler Btn_E.Click, AddressOf Btn_E_Click
        AddHandler Btn_F.Click, AddressOf Btn_F_Click
        AddHandler Btn_G.Click, AddressOf Btn_G_Click
        AddHandler Btn_H.Click, AddressOf Btn_H_Click
        AddHandler Btn_I.Click, AddressOf Btn_I_Click
        AddHandler Btn_J.Click, AddressOf Btn_J_Click
        AddHandler Btn_K.Click, AddressOf Btn_K_Click
        AddHandler Btn_L.Click, AddressOf Btn_L_Click
    End Sub

    Private Sub ReportWizardPanel_Loaded(sender As Object, e As RoutedEventArgs)
    End Sub
    Private Sub Btn_A_Click(sender As Object, e As RoutedEventArgs)
        ' Searches footer for the patient number and populates the text box
        _handler.ShowBtnAMessage(Me)
    End Sub

    Private Sub Btn_B_Click(sender As Object, e As RoutedEventArgs)
        ' Pull data from database and save as doc properties
        _handler.ShowBtnBMessage(TextBoxPatientNumber.Text, Me)
        TimerHelper.DisableTemporarily(Btn_B, 2000) ' Disable for 2 seconds
    End Sub

    Private Sub Btn_C_Click(sender As Object, e As RoutedEventArgs)
        ' Set Task Pane Helper
        TaskPaneHelper.SetTaskPane(Me)
        _handler.ShowBtnCMessage()
        TimerHelper.DisableTemporarily(Btn_C, 2000) ' Disable for 2 seconds
    End Sub

    Private Sub Btn_D_Click(sender As Object, e As RoutedEventArgs)
        ' Set Task Pane Helper
        TaskPaneHelper.SetTaskPane(Me)
        _handler.ShowBtnDMessage()
        TimerHelper.DisableTemporarily(Btn_D, 2000) ' Disable for 2 seconds
    End Sub

    Private Sub Btn_E_Click(sender As Object, e As RoutedEventArgs)
        TaskPaneHelper.SetTaskPane(Me)

        Dim patientNumber As String = TextBoxPatientNumber.Text?.Trim()

        If String.IsNullOrWhiteSpace(patientNumber) Then
            MsgBoxHelper.Show("No patient number found. Please return to Step A to complete this information.")
            Exit Sub
        End If

        _handler.ShowBtnEMessage(patientNumber)

        TimerHelper.DisableTemporarily(Btn_E, 2000)
    End Sub

    Private Sub Btn_F_Click(sender As Object, e As RoutedEventArgs)
        TaskPaneHelper.SetTaskPane(Me)
        ' Should open the opinion form
        _handler.ShowBtnFMessage()
        TimerHelper.DisableTemporarily(Btn_F, 2000) ' Disable for 2 seconds
    End Sub

    Private Sub Btn_G_Click(sender As Object, e As RoutedEventArgs)
        TaskPaneHelper.SetTaskPane(Me)
        _handler.ShowBtnGMessage()
        TimerHelper.DisableTemporarily(Btn_G, 2000) ' Disable for 2 seconds
    End Sub

    Private Sub Btn_H_Click(sender As Object, e As RoutedEventArgs)
        TaskPaneHelper.SetTaskPane(Me)
        _handler.ShowBtnHMessage()
        TimerHelper.DisableTemporarily(Btn_H, 2000) ' Disable for 2 seconds
    End Sub

    Private Sub Btn_I_Click(sender As Object, e As RoutedEventArgs)
        TaskPaneHelper.SetTaskPane(Me)
        _handler.ShowBtnIMessage()
        TimerHelper.DisableTemporarily(Btn_I, 2000) ' Disable for 2 seconds
    End Sub

    Private Sub Btn_J_Click(sender As Object, e As RoutedEventArgs)
        TaskPaneHelper.SetTaskPane(Me)
        _handler.ShowBtnJMessage()
        TimerHelper.DisableTemporarily(Btn_J, 2000) ' Disable for 2 seconds
    End Sub

    Private Sub Btn_K_Click(sender As Object, e As RoutedEventArgs)
        TaskPaneHelper.SetTaskPane(Me)
        _handler.ShowBtnKMessage()
        TimerHelper.DisableTemporarily(Btn_K, 2000) ' Disable for 2 seconds
    End Sub

    Private Sub Btn_L_Click(sender As Object, e As RoutedEventArgs)
        TaskPaneHelper.SetTaskPane(Me)
        _handler.ShowBtnLMessage()
        TimerHelper.DisableTemporarily(Btn_F, 2000) ' Disable for 2 seconds
    End Sub

    Public Sub MarkCheckboxAsDone(stepID As String)
        Select Case stepID
            Case "Btn_C"
                Btn_C_Checkbox.IsChecked = True
            Case "Btn_D"
                Btn_D_Checkbox.IsChecked = True
            Case "Btn_E"
                Btn_E_Checkbox.IsChecked = True
            Case "Btn_F"
                Btn_F_Checkbox.IsChecked = True
            Case "Btn_G"
                Btn_G_Checkbox.IsChecked = True
            Case "Btn_H"
                Btn_H_Checkbox.IsChecked = True
            Case "Btn_I"
                Btn_I_Checkbox.IsChecked = True
            Case "Btn_J"
                Btn_J_Checkbox.IsChecked = True
            Case "Btn_K"
                Btn_K_Checkbox.IsChecked = True
            Case "Btn_L"
                Btn_L_Checkbox.IsChecked = True
        End Select
    End Sub


End Class
