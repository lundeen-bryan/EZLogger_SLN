# How to Share the Active Task Pane (ReportWizardPanel) with WPF Views in a VSTO Add-in

## About This Article

This article explains how to share a reference to the active task pane (`ReportWizardPanel`) from anywhere within your VSTO Word Add-in, particularly when working with WPF views like `ReportTypeView.xaml`. This pattern allows you to update checkboxes or UI state in the task pane from child views without needing to pass the panel reference through every constructor. You will learn how to implement a shared helper (`TaskPaneHelper`), how to integrate it with your view logic, and how to scale it for additional steps.

## Applies To

- Visual Studio 2022
- VB.NET (Windows Desktop)
- VSTO Add-ins for Microsoft Word
- .NET Framework 4.8

## Prerequisites

- Familiarity with VB.NET syntax and object-oriented programming
- Experience using ElementHost to embed WPF views in WinForms
- A multi-step task pane layout using a `UserControl` named `ReportWizardPanel`
- WPF views like `ReportTypeView` that interact with the panel

## In This Article

- Problem Overview
- Introducing `TaskPaneHelper`
- Setting the Active Task Pane
- Marking Checkboxes from a View
- Closing the View Cleanly
- Extending to Multiple Steps
- Summary

---

## Problem Overview

In multi-step task pane workflows, each view (e.g., `ReportTypeView`) may need to signal that its step is complete by checking a corresponding checkbox on the main panel (`ReportWizardPanel`). Passing the panel through multiple constructors becomes unwieldy, especially when multiple buttons or forms can launch the same view.

---

## Introducing `TaskPaneHelper`

To simplify the reference-sharing process, I created a module called `TaskPaneHelper`.

```vbnet
Namespace Helpers

    Public Module TaskPaneHelper

        Private _currentTaskPane As ReportWizardPanel

        Public Sub SetTaskPane(panel As ReportWizardPanel)
            _currentTaskPane = panel
        End Sub

        Public Function GetTaskPane() As ReportWizardPanel
            Return _currentTaskPane
        End Function

        Public Sub ClearTaskPane()
            _currentTaskPane = Nothing
        End Sub

    End Module

End Namespace
```

This allows you to store the panel globally when a view is launched, and retrieve it later from anywhere.

---

## Setting the Active Task Pane

Inside your task pane code-behind (`ReportWizardPanel.xaml.vb`), when a button is clicked (e.g., `Btn_C_Click`), store the panel reference:

```vbnet
Private Sub Btn_C_Click(sender As Object, e As RoutedEventArgs)
    TaskPaneHelper.SetTaskPane(Me)
    _handler.ShowBtnCMessage(Me)
    TimerHelper.DisableTemporarily(Btn_C, 2000)
End Sub
```

---

## Marking Checkboxes from a View

Add a method to your `ReportWizardPanel` to mark the appropriate checkbox:

```vbnet
Public Sub MarkCheckboxAsDone(stepId As String)
    Select Case stepId
        Case "Btn_C"
            Btn_C_Checkbox.IsChecked = True
        Case "Btn_D"
            Btn_D_Checkbox.IsChecked = True
        ' Add more as needed
    End Select
End Sub
```

Then, in your WPF view (e.g., `ReportTypeView.xaml.vb`):

```vbnet
Private Sub DoneBtn_Click(sender As Object, e As RoutedEventArgs)
    Dim panel = TaskPaneHelper.GetTaskPane()
    panel?.MarkCheckboxAsDone("Btn_C")

    _handler.HandleCloseClick(_hostForm)
End Sub
```

This ensures only the appropriate checkbox is checked when the user clicks "Done."

---

## Closing the View Cleanly

To maintain a clean user experience, the handler closes the form when the step is complete:

```vbnet
Public Sub HandleCloseClick(form As Form)
    If form IsNot Nothing Then
        form.Close()
    End If
End Sub
```

---

## Extending to Multiple Steps

You can reuse `MarkCheckboxAsDone` with different IDs for other steps:

```vbnet
panel?.MarkCheckboxAsDone("Btn_D")
panel?.MarkCheckboxAsDone("Btn_E")
```

Or replace the string ID with an `Enum` for stronger typing.

---

## Summary

By introducing a `TaskPaneHelper` module to store and retrieve the current `ReportWizardPanel`, you can reduce constructor clutter and make your views more modular. This approach allows any WPF view to easily mark a step as complete and close itself cleanly, while keeping your code maintainable and scalable as your wizard grows.

---

## See Also