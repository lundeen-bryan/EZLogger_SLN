# Hosting a WPF UserControl in a Modal Windows Form (VB.NET VSTO Add-in)

---

## About This Article

This article provides a step-by-step guide for VB.NET developers building VSTO Add-ins for Microsoft Word. It details how to host a WPF `UserControl` inside a Windows Form using the `ElementHost` control, and how to invoke the form modally from a Ribbon button. This approach allows developers to use modern WPF controls in a familiar Windows Forms environment.

---

## Applies To

- Microsoft Office Word
- VSTO Add-ins
- VB.NET
- .NET Framework (Windows-only)

---

## Prerequisites

- Visual Studio with VB.NET support
- A VSTO Word Add-in project
- Basic knowledge of WPF and Windows Forms
- Reference to `WindowsFormsIntegration.dll`

---

## Summary

You can integrate WPF UI into VSTO Add-ins by hosting a `UserControl` within a Windows Form using the `ElementHost` control. This allows for a modal-like user interface, offering a modern WPF-based experience with the simplicity of Windows Forms.

---

## Key Components

| Type           | Name           | Description                                          |
|----------------|----------------|------------------------------------------------------|
| WPF Control    | `UserControl1.xaml` | WPF user interface element to be hosted             |
| Windows Form   | `Form1.vb`     | Hosts the WPF control using `ElementHost`           |
| Ribbon Button  | `AboutButton`  | Triggers the display of the modal form              |
| Integration    | `ElementHost`  | Hosts the WPF control within the Windows Form       |

---

## Code Examples

### Hosting WPF Control in Windows Form

Create a WinForm as the host, add an Element host in the Winform.

```vbnet
Imports System.Windows.Forms.Integration

Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim myControl As New UserControl1()
        ElementHost1.Dock = DockStyle.Fill
        ElementHost1.Child = myControl
    End Sub
End Class
```

### Triggering the Modal Form from Ribbon

```vbnet
Private Sub AboutButton_Click(sender As Object, e As RibbonControlEventArgs) Handles AboutButton.Click
    Dim aboutForm As New Form1()
    aboutForm.StartPosition = FormStartPosition.CenterScreen
    aboutForm.ShowDialog() ' Use Show() for non-blocking behavior
End Sub
```

---

## Execution Flow

1. Add a WPF UserControl to your project and design it (e.g., add a Label).
2. Add a Windows Form to the project and place an `ElementHost` on it.
3. In the Form’s Load event, instantiate and embed the UserControl into the `ElementHost`.
4. Add a Ribbon button and handle its click event to display the form.
5. Run the project and test the modal window.

---

## Sample Input / Output

**Expected Behavior:**

- When the Ribbon button is clicked, the form opens and displays the WPF control.
- If `ShowDialog()` is used, interaction with Word is blocked until the form is closed.

---

## Use Case

**Scenario**:
An add-in developer wants to present an "About This Add-in" UI. Instead of creating a plain Windows Form, they use a WPF UserControl to provide modern styling. The control is hosted in a modal form and invoked via a Ribbon button.

---

## Related Features or Dependencies

- `System.Windows.Forms`
- `System.Windows.Forms.Integration`
- VSTO Ribbon customization
- .NET Framework for Windows

---

## User Story

> As an **Office Add-in developer**, I want to use WPF UI in my modal forms so that I can create a more modern and visually appealing experience for my users.

---

## Troubleshooting

- Make sure `WindowsFormsIntegration.dll` is referenced.
- Ensure `ElementHost` is properly docked.
- Use `ShowDialog()` if Word interaction should be blocked.
- Verify all renames (e.g., `Form1` to `AboutWindow`) are reflected in code.

---

## Diagram

```
[ Ribbon Button Clicked ]
           ↓
[ Form1 Opens ]
           ↓
[ ElementHost Loads WPF UserControl ]
           ↓
[ Modal Window Displays WPF UI ]
```

---

## Development Status

- [x] WPF UserControl added and styled
- [x] Windows Form hosts WPF control via ElementHost
- [x] Ribbon integration working
- [x] Modal display logic functional

---

## See Also

*(No additional links specified)*

<!-- @nested-tags:wpf-user-control -->