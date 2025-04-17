# Adding Button Functionality in a WPF **UserControl** Hosted in a WinForms **ElementHost**

> **Goal** – Keep all button‑handling logic in a dedicated *Handler* class while the WinForms host stays a thin container.

---

## When you’d choose this pattern
| Scenario | Why it fits |
| --- | --- |
| Simple modal prompts (Approve / Reject) | You want the UX polish of WPF but need the dialog to behave like a WinForms modal window under Word. |
| Small data‑entry pop‑ups | Easy to embed validation logic in a WPF control while Word remains blocked. |
| Decoupled testing | Handlers testable without Word or WinForms – pure VB classes. |

---

## Preconditions & project layout
```
Solution EZLogger_SLN
└── EZLogger (Word VSTO Add‑in – VB.NET | .NET 4.8)
    ├── Views\ApprovedByView.xaml          ' WPF UserControl (UI)
    ├── HostForms\ApprovedByHost.vb        ' WinForms Form (ElementHost only)
    ├── Handlers\ApprovedByHandler.vb      ' Contains button‑click logic
    ├── Ribbon\EZLoggerRibbon.vb           ' Ribbon button triggers popup
    └── Helpers\WindowWrapper.vb           ' Optional owner wrapper
```
*No code‑behind in **ApprovedByHost** beyond hooking up the Handler; all business logic resides in **ApprovedByHandler***.

---

## Step‑by‑step implementation

### 1  Create the WPF **UserControl** (`ApprovedByView.xaml`)
```xml
<UserControl x:Class="EZLogger.Views.ApprovedByView"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             MinWidth="300" MinHeight="150">
    <Grid>
        <Button x:Name="BtnApprove"
                Content="Approve"
                Width="100" Height="30"
                HorizontalAlignment="Center"
                VerticalAlignment="Center"/>
    </Grid>
</UserControl>
```
> **Tip –** We declare **no** click handler here; the Handler wires it at runtime.

### 2  Create the WinForms **Host Form** (`ApprovedByHost.vb`)
```vbnet
Imports System.Windows.Forms
Imports System.Windows.Forms.Integration
Imports EZLogger.Views
Imports EZLogger.Handlers

Public Class ApprovedByHost
    Inherits Form

    Private ReadOnly _handler As ApprovedByHandler   ' business logic holder
    Private ReadOnly _view As ApprovedByView         ' WPF UI
    Private ReadOnly _elementHost As New ElementHost() With {.Dock = DockStyle.Fill}

    Public Sub New()
        ' 1️⃣  Instantiate UI and handler
        _view = New ApprovedByView()
        _handler = New ApprovedByHandler(_view, Me)   ' pass view + host if needed

        ' 2️⃣  Embed WPF view
        _elementHost.Child = _view
        Controls.Add(_elementHost)

        ' 3️⃣  Window appearance – pure shell duties
        Text = "Approved By"
        ClientSize = New Drawing.Size(380, 180)
        StartPosition = FormStartPosition.CenterScreen
        MinimizeBox = False : MaximizeBox = False : ShowIcon = False
    End Sub
End Class
```
The host *only* holds the UI and wires the handler – no business code.

### 3  Implement the **Handler** (`ApprovedByHandler.vb`)
```vbnet
Imports System.Windows.Forms
Imports EZLogger.Views

' Encapsulates all logic triggered by the Approve button.
Public Class ApprovedByHandler
    Private ReadOnly _view As ApprovedByView
    Private ReadOnly _host As Form

    Public Sub New(view As ApprovedByView, host As Form)
        _view = view  :  _host = host

        ' Subscribe to the button click once.
        AddHandler _view.BtnApprove.Click, AddressOf Approve_Click
    End Sub

    ' 👉  All business rules live here.
    Private Sub Approve_Click(sender As Object, e As EventArgs)
        MessageBox.Show(_host, "Approved!", "EZLogger", MessageBoxButtons.OK, MessageBoxIcon.Information)
        _host.Close()   ' Close the modal dialog after action.
    End Sub
End Class
```
> **Good** – Handler owns the business logic and can be unit‑tested.
> **Bad** – Putting `MessageBox.Show` directly in the WPF click event.

### 4  Launch from the Ribbon
```vbnet
' EZLoggerRibbon.vb – minimal example
Imports Microsoft.Office.Tools.Ribbon

Public Class EZLoggerRibbon
    Private Sub BtnApprovedBy_Click(sender As Object, e As RibbonControlEventArgs) Handles BtnApprovedBy.Click
        Dim owner = New WindowWrapper(Globals.ThisAddIn.Application.Hwnd)
        Using dlg As New HostForms.ApprovedByHost()
            dlg.ShowDialog(owner)  ' modal over Word
        End Using
    End Sub
End Class
```

---

## Good vs. Bad patterns
| Aspect | ✅ Good | ❌ Bad |
| --- | --- | --- |
| Separation of concerns | Logic in `ApprovedByHandler`. | Logic in `ApprovedByView` code‑behind. |
| Handler instantiation | One handler per dialog instance. | Static handler – accumulates event subscriptions. |
| Disposal | `Using dlg … End Using` ensures dialog disposed. | Keep a global dialog instance – leaks memory. |
| Modal owner | `ShowDialog(owner)` with `WindowWrapper`. | `Show()` – user can click behind dialog. |

---

## Common pitfalls & fixes
* **Keyboard shortcuts dead** – call `EnableModelessKeyboardInterop(_view)` if needed.
* **Focus lost on closing** – in `FormClosed`, call `owner.Activate()`.
* **High‑DPI clipping** – don’t hard‑code pixel sizes; use dynamic layout.
* **Event leaks** – always `RemoveHandler` in Handler’s `Dispose` if you re‑use views.

---

## Testing checklist
- [ ] Ribbon button opens dialog centered.
- [ ] Word UI blocked while dialog open.
- [ ] Clicking **Approve** shows confirmation then closes dialog.
- [ ] Dialog can be reopened repeatedly without memory growth.

---

<!-- @nested-tags:wpf-button-hosting, elementhost-pattern -->

