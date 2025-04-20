Imports System.Windows
Imports EZLogger.Helpers
Imports EZLogger.Handlers

Partial Public Class ReportWizardPanel
    Inherits Controls.UserControl

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
    End Sub

    Private Sub Btn_C_Click(sender As Object, e As RoutedEventArgs)
        _handler.ShowBtnCMessage()
    End Sub

    Private Sub Btn_D_Click(sender As Object, e As RoutedEventArgs)
        _handler.ShowBtnDMessage()
    End Sub

    Private Sub Btn_E_Click(sender As Object, e As RoutedEventArgs)
        _handler.ShowBtnEMessage()
    End Sub

    Private Sub Btn_F_Click(sender As Object, e As RoutedEventArgs)
        ' Should open the opinion form
        _handler.ShowBtnFMessage()
    End Sub

    Private Sub Btn_G_Click(sender As Object, e As RoutedEventArgs)
        _handler.ShowBtnGMessage()
    End Sub

    Private Sub Btn_H_Click(sender As Object, e As RoutedEventArgs)
        _handler.ShowBtnHMessage()
    End Sub

    Private Sub Btn_I_Click(sender As Object, e As RoutedEventArgs)
        _handler.ShowBtnIMessage()
    End Sub

    Private Sub Btn_J_Click(sender As Object, e As RoutedEventArgs)
        _handler.ShowBtnJMessage()
    End Sub

    Private Sub Btn_K_Click(sender As Object, e As RoutedEventArgs)
        _handler.ShowBtnKMessage()
    End Sub

    Private Sub Btn_L_Click(sender As Object, e As RoutedEventArgs)
        _handler.ShowBtnLMessage()
    End Sub

End Class
