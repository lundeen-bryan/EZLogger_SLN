' Namespace=EZLogger/Handlers
' Filename=ShortcutHandler.vb
' !See Label Footer for notes

Imports System.Windows.Forms
Imports System

Public Class ShortcutHandler

    Private ReadOnly _hostForm As Form
    Private ReadOnly _shortcuts As New Dictionary(Of (Keys, Keys), Action)

    ''' <summary>
    ''' Initializes the ShortcutHandler for a specific WinForms host form.
    ''' </summary>
    ''' <param name="hostForm">The form to monitor for key presses (must have KeyPreview=True)</param>
    Public Sub New(hostForm As Form)
        _hostForm = hostForm
        AddHandler _hostForm.KeyDown, AddressOf OnKeyDown
    End Sub

    ''' <summary>
    ''' Registers a keyboard shortcut with an associated action.
    ''' </summary>
    Public Sub RegisterShortcut(key As Keys, modifier As Keys, action As Action)
        _shortcuts((key, modifier)) = action
    End Sub

    ''' <summary>
    ''' Unregisters a previously registered shortcut.
    ''' </summary>
    Public Sub UnregisterShortcut(key As Keys, modifier As Keys)
        _shortcuts.Remove((key, modifier))
    End Sub

    ''' <summary>
    ''' Clears all registered shortcuts for this form.
    ''' </summary>
    Public Sub ClearShortcuts()
        _shortcuts.Clear()
    End Sub

    ''' <summary>
    ''' Handles the KeyDown event and triggers matching shortcuts.
    ''' </summary>
    Private Sub OnKeyDown(sender As Object, e As KeyEventArgs)
        Dim keyCombo = (e.KeyCode, e.Modifiers)
        If _shortcuts.ContainsKey(keyCombo) Then
            e.Handled = True
            _shortcuts(keyCombo).Invoke()
        End If
    End Sub

    ''' <summary>
    ''' Call this if the form is closing and you want to remove the handler.
    ''' </summary>
    Public Sub Dispose()
        RemoveHandler _hostForm.KeyDown, AddressOf OnKeyDown
    End Sub

End Class

' Footer:
''===========================================================================================
'' Filename: .......... ShortcutHandler.vb
'' Description: ....... Handles keyboard shortcuts (like Alt+S) for a specific host form.
'' Created: ........... 2025-05-02
'' Updated: ........... 2025-05-02
'' Installs to: ....... EZLogger/Handlers
'' Compatibility: ..... VSTO, WPF
'' Contact Author: .... lundeen-bryan
'' Copyright:  ........ ©2025. All rights reserved.
'' Notes: ............. _
' (1) Must be used with KeyPreview=True on the host form.
''===========================================================================================