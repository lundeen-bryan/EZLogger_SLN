# Hosting a WPF **UserControl** in a Modal Windows Form
*(VB.NET VSTO Word Add‑in pattern)*

> **Purpose** – Provide a rock‑solid recipe for displaying rich WPF content in a modal popup window (WinForms `Form`) inside Microsoft Word.

---

## Why / When to use this pattern
| Typical use‑case | Benefit over a task pane |
| --- | --- |
| **About / Help** dialog | Users expect a centered, dismissible window that blocks other actions. |
| **Settings wizard** | Force the user to finish configuration before returning to the document. |
| **Small data‑entry form** | Minimises screen real‑estate and guarantees focus. |
| **Critical confirmation** | Prevents accidental clicks in Word until a choice is made. |

---

## Preconditions
Before coding, make sure the project structure and naming scheme are pinned down.

```
Solution EZLogger_SLN
└── EZLogger (Word VSTO Add‑in – VB.NET | .NET 4.8)
    ├── Views\AboutControl.xaml            ' WPF UserControl (UI only)
    ├── HostForms\AboutWindow.vb           ' WinForms Form hosting the control
    ├── Ribbon\EZLoggerRibbon.vb           ' Ribbon with "About" button
    ├── ThisAddIn.vb                       ' VSTO startup object
    └── Helpers\WindowWrapper.vb           ' Optional helper for true modality
```

### Naming conventions
* **Form** ⇒ `<DialogName>Window` (e.g. `AboutWindow`).
  *Rationale – conveys it is a window, not business logic.*
* **WPF control** ⇒ `<DialogName>Control` (e.g. `AboutControl`).
  *Easier to locate when there are many controls.*
* **Ribbon button ID / handler** ⇒ `<DialogName>Button`.
  *Keeps handler discovery intuitive.*

### References required
* `WindowsFormsIntegration` – supplies `ElementHost`.
* `PresentationCore`, `PresentationFramework`, `WindowsBase` – auto‑added by the WPF template.

---

## Step‑by‑step implementation

### 1  Create the WPF **UserControl**
1. **Solution Explorer** ▸ right‑click project ▸ **Add ▸ New Item…**.
2. Select **WPF User Control** – name **`AboutControl.xaml`**.
3. Edit XAML:
```xml
<UserControl x:Class="EZLogger.Views.AboutControl"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             MinWidth="300" MinHeight="150" Background="White">
    <Grid Margin="12">
        <StackPanel>
            <TextBlock Text="EZLogger Add‑in" FontSize="18" FontWeight="Bold"/>
            <TextBlock Text="Version 1.0.0" Margin="0,8,0,0"/>
            <Button x:Name="BtnOk" Content="OK" Width="80" HorizontalAlignment="Right" Margin="0,16,0,0"/>
        </StackPanel>
    </Grid>
</UserControl>
```

### 2  Create the WinForms **Form** that hosts the control
1. Right‑click the **HostForms** folder ▸ **Add ▸ Windows Form…**.
2. Name the file **`AboutWindow.vb`**.
3. Design view ➜ drag **ElementHost** from Toolbox onto the form, set:
   * **Name** = `ElementHost1`
   * **Dock** = **Fill**

### 3  Code behind the **ElementHost form**
```vbnet
' AboutWindow.vb – WinForms wrapper that hosts the WPF control
Imports System.Windows.Forms
Imports System.Windows.Forms.Integration
Imports EZLogger.Views   ' adjust namespace if yours differs

Public Class AboutWindow

    ' Executed once when the WinForm is first shown.
    Private Sub AboutWindow_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ' 1️⃣  Instantiate the WPF view you want to host
        Dim view As New AboutControl()

        ' 2️⃣  Embed the WPF view into the ElementHost
        ElementHost1.Child = view

        ' ---- Form appearance -------------------------------------------
        Me.ClientSize = New Drawing.Size(900, 500)   ' overall window size
        Me.Text = "About EZLogger"                   ' window caption
        Me.MinimizeBox = False
        Me.MaximizeBox = False
        Me.ShowIcon = False
        Me.FormBorderStyle = FormBorderStyle.FixedSingle
        Me.StartPosition = FormStartPosition.CenterScreen
        ' -----------------------------------------------------------------

        ' ---- Optional manual ElementHost sizing -------------------------
        ElementHost1.Width = Me.ClientSize.Width - 40
        ElementHost1.Height = Me.ClientSize.Height - 40
        ElementHost1.Location = New Drawing.Point(20, 20)
        ' -----------------------------------------------------------------

    End Sub

End Class
```

### 4  Add Ribbon button logic  Add Ribbon button logic
```vbnet
' EZLoggerRibbon.vb – toggles modal window.
Imports Microsoft.Office.Tools.Ribbon

Public Class EZLoggerRibbon
    Private Sub AboutButton_Click(
        sender As Object, e As RibbonControlEventArgs) _
        Handles AboutButton.Click

        ' Wrap Word's window handle so our dialog is truly modal to Word
        Dim owner = New WindowWrapper(Globals.ThisAddIn.Application.Hwnd)

        Using dlg As New AboutWindow()
            dlg.StartPosition = FormStartPosition.CenterParent
            dlg.ShowDialog(owner)   ' blocks Word until dismissed
        End Using                 ' ensures disposal
    End Sub
End Class
```
`WindowWrapper` is a tiny helper implementing `IWin32Window`; full source is included in **Helpers\WindowWrapper.vb**.

### 5  Run & test
1. **F5** – Word launches with the add‑in.
2. Click **About** on the EZLogger tab.
3. Window appears centered; Word is blocked until **OK** is clicked.
4. Re‑open the dialog to verify it instantiates fresh each time and disposes properly.

---

## Good vs. Bad practices

| Concern | ✅ Good | ❌ Bad |
| --- | --- | --- |
| Window ownership | `ShowDialog(owner)` with `WindowWrapper`. | `ShowDialog()` with no owner – dialog may hide behind Word. |
| Form lifetime | `Using dlg As New AboutWindow()` – disposes resources. | Keep a global `AboutWindow` instance – leaks memory & events. |
| Separation of concerns | WPF logic stays inside `AboutControl`; form only hosts UI. | Mixing business logic directly in the WinForms form. |
| Event unsubscription | Use lambda that closes the form; auto‑unsubscribed on dispose. | AddHandler without RemoveHandler when using a static control instance. |

---

## Common pitfalls

* **Keyboard shortcuts not working** – call `System.Windows.Input.InputMethod.SetIsInputMethodEnabled(ctrl, True)` if input seems dead.
* **High‑DPI blurriness** – avoid hard‑coded pixel sizes; test at 150 % zoom.
* **Lost focus after closing** – explicitly `owner.Activate()` in the `FormClosed` event if needed.

---

## Testing checklist

- [ ] Dialog opens & centers every time.
- [ ] Word ribbon is inaccessible while dialog is open.
- [ ] Keyboard input functions inside WPF control.
- [ ] Memory usage returns to baseline after closing (check with Diagnostic Tools).

---

<!-- @nested-tags:wpf-modal-hosting, vsto-modal -->

