# EZLogger: Using MsgBoxHelper

## Overview

The `MsgBoxHelper` is a simplified, centralized utility for displaying custom-styled message boxes in the EZLogger application. It wraps a WPF-based message box (`CustomMsgBox`) in a modeless (non-blocking) form by default, allowing users to interact with Microsoft Word while the dialog is visible.

This helper replaces the more complex and scattered `CustomMsgBoxHandler` pattern, consolidating logic into a single module that is easier to use, maintain, and extend.

---

## Why MsgBoxHelper?

Traditional `MessageBox.Show()` calls block the UI thread, which can be frustrating for VSTO users who need to interact with Word documents. The `MsgBoxHelper`:

- Displays dialogs without blocking the user interface
- Supports Yes/No/OK buttons
- Uses styled WPF controls inside a WinForms host
- Can be extended or positioned relative to forms

---

## Basic Usage Examples

### 1. Show a simple message (fire-and-forget)

```vbnet
MsgBoxHelper.Show("Operation completed successfully.")
```

This shows a message box with an OK button and no callback.

---

### 2. Ask for confirmation and handle the result

```vbnet
MsgBoxHelper.Show("Are you sure you want to delete this record?", Sub(result)
    If result = CustomMsgBoxResult.Yes Then
        ' Proceed with delete
    Else
        ' Cancel action
    End If
End Sub)
```

The message box remains modeless and allows Word interaction while waiting for a response.

---

### 3. Use full configuration (Yes/No buttons)

```vbnet
Dim config As New MessageBoxConfig With {
    .Message = "Do you want to overwrite the file?",
    .ShowYes = True,
    .ShowNo = True
}

MsgBoxHelper.Show(config, Sub(result)
    If result = CustomMsgBoxResult.Yes Then
        ' Overwrite logic
    End If
End Sub)
```

---

### 4. Positioning the box relative to a parent form

```vbnet
MsgBoxHelper.Show(config, onResult:=Sub(result)
    ' handle result
End Sub, ownerForm:=Me)
```

This positions the message box near the calling WinForms host (like a WPF ElementHost).

---

## Best Practices

- Always use `MsgBoxHelper` instead of `MessageBox.Show()` for consistency.
- Use `MessageBoxConfig` to customize text and buttons.
- Prefer callbacks over synchronous return values when using modeless dialogs.
- Use `Clipboard.SetText(...)` if you want to copy data from the message.

---

## Future Improvements

This pattern can be extended with:

- Timeout-based auto-close logic
- Support for multiple styles/themes
- Additional button types (e.g., Retry, Cancel)

---

## Conclusion

`MsgBoxHelper` makes it easier and safer to display dialogs in EZLogger's VSTO Word environment. Its default modeless design and flexible API allow developers to write more interactive, responsive tools without managing complex WPF/WinForms integrations manually.



