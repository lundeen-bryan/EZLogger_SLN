# NOTES ABOUT HELPER FUNCTIONS IN THIS PROJECT

This document provides reference documentation for the shared helper modules used across the EZLogger project. Each helper module encapsulates reusable functionality to streamline UI behavior, file access, clipboard handling, and Word document automation.

---

## ClipboardHelper

The `ClipboardHelper` module provides utility functions for safely interacting with the Windows clipboard. It simplifies copying and retrieving text and gracefully handles invalid input or clipboard access exceptions.

### `CopyText(text As String) As Boolean`

Copies a string of text to the system clipboard.

**Parameters:**
- `text`: The string you want to copy.

**Returns:**
- `True` if the text was successfully copied.
- `False` if the input is empty/null or if clipboard access fails.

**Example:**
```vbnet
Dim success As Boolean = ClipboardHelper.CopyText("Case ID: 12345")
If success Then
    MessageBox.Show("Copied to clipboard.")
Else
    MessageBox.Show("Failed to copy.")
End If
