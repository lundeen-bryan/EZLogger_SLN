**Title: Building a Custom Resizable MessageBox in EZLogger (VSTO + WPF + WinForms) (DEFUNCT See MsgBoxHelper article)**

**Background:**
Our forensic reporting tool, EZLogger, is built as a VSTO add-in that primarily uses WinForms to host WPF UserControls. Due to the constraints of the VSTO platform, we cannot use WPF `Window` objects directly. However, we wanted to create a custom MessageBox that supports:

- A modern, readable terminal-style UI (lime green Consolas on black)
- Auto-sizing based on text content
- Flexible button configurations (OK, Yes/No, etc.)
- Centralized logic for handling user choices

**Solution Overview:**
To implement this, we built a reusable system composed of three layers:

1. A **WPF UserControl** (`CustomMsgBoxControl`) for the UI
2. A **WinForms Host Form** (`CustomMsgBoxHost`) that embeds the WPF control via `ElementHost`
3. A **Handler Class** (`CustomMsgBoxHandler`) that manages message configuration and result handling

This approach respects the architectural limitations of VSTO while leveraging WPF's layout flexibility.

**Step-by-Step Implementation:**

---

**1. The UI Layer: `CustomMsgBoxControl`**

We created a WPF `UserControl` that contains:
- A `TextBlock` for the message text, using `TextWrapping="Wrap"`, `MaxWidth=500`, and a monospace `Consolas` font in lime green.
- A `StackPanel` with buttons (`Yes`, `No`, `OK`) that are dynamically shown or hidden.

The control raises a `ButtonClicked` event when a button is pressed.

**2. The Host Layer: `CustomMsgBoxHost` (WinForms)**

Since we cannot display a WPF `Window` in VSTO, we created a WinForms `Form` to host the WPF control.

Key implementation details:
- The control is measured with `Measure()` and `Arrange()` to calculate the required width and height.
- The host form's size is set based on the measured `DesiredSize` with extra padding to account for borders and margins.
- An `ElementHost` wraps the WPF control and is added to the form.

**3. The Logic Layer: `CustomMsgBoxHandler`**

This class centralizes the logic for showing the message box and collecting the result. It receives a `MessageBoxConfig` object which defines:
- The message to display
- Which buttons should be shown

The handler constructs the control, wraps it in the host form, and wires up the result.

**Optional Feature: 72-Character Text Wrapping**

To support terminal-style formatting, we optionally pre-wrap the message text to 72 characters per line. A helper function splits words and inserts line breaks at appropriate intervals. This preserves clean layout and ensures readability within the set `MaxWidth`.

**How to Use the Custom MessageBox Throughout EZLogger**

To reuse this MessageBox anywhere in the app, simply call the `CustomMsgBoxHandler.Show()` method with a configuration object. For example, when the user clicks the "Search" button on a form, and you want to show the report expiration date:

```vbnet
Private Sub BtnSearch_Click(sender As Object, e As EventArgs) Handles BtnSearch.Click
    Dim expirationDate As Date = GetExpirationDateForCurrentRecord()
    Dim config As New MessageBoxConfig With {
        .Message = $"The report expires on: {expirationDate:MMMM dd, yyyy}",
        .ShowOk = True
    }

    CustomMsgBoxHandler.Show(config)
End Sub
```

You can adjust the config to show Yes/No buttons, change the message, or handle the result:

```vbnet
Dim config As New MessageBoxConfig With {
    .Message = "Do you want to archive this report?",
    .ShowYes = True,
    .ShowNo = True
}

Dim result = CustomMsgBoxHandler.Show(config)
If result = CustomMsgBoxResult.Yes Then
    ArchiveReport()
End If
```

**Benefits:**
- **Reusability:** The handler and control can be used in any context across EZLogger.
- **Maintainability:** Central logic for message handling means easier updates.
- **Consistency:** The UI is standardized and follows a clean, readable style.
- **Flexibility:** Supports a variety of interaction styles without needing external libraries or pop-up frameworks.

**Conclusion:**
This custom MessageBox approach bridges the gap between the limitations of VSTO (no native WPF windows) and our need for modern, flexible UI components. By splitting responsibilities cleanly between UI, hosting, and logic layers, we've created a robust, scalable solution that integrates seamlessly into the EZLogger application.

<!-- @nested-tags:msgbox -->