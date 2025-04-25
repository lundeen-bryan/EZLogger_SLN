# Use Async/Await with a Custom Progress Bar in EZLogger

## Overview

This article explains how I used `Async/Await` in VB.NET to support a background Excel lookup while displaying a custom animated progress indicator in my EZLogger project. The feature allows the program to look up a "CONREP" provider from an Excel file using a patient number, and write the result to the custom document properties of the active Word document.

## Why I Needed Async/Await

During development, I discovered that calling `ExcelHelper.GetProviderFromHLV(patientNumber)` directly would freeze the UI and stop the animation in my `BusyControl`. This happened because Excel interop operations block the UI thread.

To solve this, I:

- Moved the Excel lookup to a background thread using `Tasks.Task.Run`
- Used `Await` to keep the method responsive
- Used `Task.Delay` to allow the UI time to render before starting the background task

## What I Built

To implement this feature, I created the following components:

- `BusyControl.xaml`: A WPF UserControl with an indeterminate `ProgressBar` and lime green "Working..." text
- `BusyHost.vb`: A WinForms form using `ElementHost` to display the `BusyControl`
- `ShowBtnEMessage`: An async method inside `ReportWizardHandler.vb` to run the Excel lookup and update Word properties
- `ExcelHelper.vb`: Handles reading the HLV Excel workbook
- `DocumentPropertyHelper.vb`: Handles writing the result to Word custom properties

---

## Core Function: ShowBtnEMessage

```vbnet
Imports Tasks = System.Threading.Tasks

Public Async Sub ShowBtnEMessage(patientNumber As String)
    Dim provider As String = Nothing
    Dim busyForm As New BusyHost()
    busyForm.Show()

    Await Tasks.Task.Delay(100) ' Allow spinner UI to fully render

    Try
        provider = Await Tasks.Task.Run(Function()
            Return ExcelHelper.GetProviderFromHLV(patientNumber)
        End Function)
    Finally
        busyForm.Close()
    End Try

    If Not String.IsNullOrWhiteSpace(provider) Then
        DocumentPropertyHelper.WriteCustomProperty(
            Globals.ThisAddIn.Application.ActiveDocument,
            "CONREP",
            provider
        )
        MsgBoxHelper.Show($"Provider found and saved to CONREP: {provider}")
    Else
        MsgBoxHelper.Show($"No provider found for patient number: {patientNumber}")
    End If
End Sub
```

## Why I Used Task.Delay(100)

This small delay ensures that the `BusyControl` renders and begins animating before Excel work begins. Without it, the form appears but remains frozen during processing.

## Why I Used Task.Run

`ExcelHelper.GetProviderFromHLV` performs blocking Excel interop. By using `Task.Run`, I ensured that the long-running task executes on a background thread, allowing the UI to remain responsive.

---

## Trigger: Btn_E_Click

The process starts in `Btn_E_Click` within `ReportWizardPanel.xaml.vb`. Here's how I connected the trigger to the async logic:

```vbnet
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
```

---

## Summary

This pattern allows my EZLogger UI to remain responsive while running Excel lookups in the background. It uses `Async/Await` in VB.NET, together with a custom progress form, to create a smoother user experience.

## Reuse

I plan to reuse this pattern in future tasks like:

- Exporting to PDF
- Long-running merges
- SharePoint uploads
- Excel reads or SQL queries

It’s now my go-to structure for any operation that should not block the UI.
