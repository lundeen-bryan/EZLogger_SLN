# Creating a VB.NET **Word Task Pane** App with WPF

> **Audience** – VB.NET developers who want a reproducible, production‑ready pattern for embedding a rich WPF UI inside a Microsoft Word custom task pane using VSTO.

---

## Why build a WPF task pane?
| Scenario | Benefit |
| --- | --- |
| Guided data‑entry wizard | Keep users focused in a single pane while they complete multi‑step workflows. |
| Document metadata dashboard | Surface custom properties (e.g. patient info) beside the active document for immediate edits. |
| Workflow shortcuts | Offer one‑click PDF export, database logging, or compliance checks directly in Word. |
| Context‑sensitive help | Display help content that changes with cursor position or document type. |

---

## Preconditions
Make sure every item below is ready **before** you write code:
1. **Matching Office bitness** – Build **x86** for 32‑bit Office, **x64** for 64‑bit Office.
2. **Visual Studio 2022** with the **Office / SharePoint development** workload installed.
3. **Office PIAs** – _Microsoft.Office.Interop.Word_ ≥ 15.0 referenced automatically.
4. **VSTO Runtime** – Must match the Office major version on every machine that will load the add‑in.
5. **.NET Framework 4.8** (or at least 4.7.2) as the target framework.
6. **WindowsFormsIntegration.dll** reference for the `ElementHost` control.
7. **Code‑signing certificate** for deployment (self‑signed is fine for testing).

> **Pitfall #1** – A 32‑/64‑bit mismatch is the #1 reason a task pane loads on your machine but not on a colleague’s.

---

## Step‑by‑Step Project Setup
Follow every click so nothing gets missed.

### 1  Create the VSTO project
1. **File ▸ New ▸ Project**.
2. Select **Word VSTO Add‑in (VB.NET)** ➜ **Next**.
3. Configure:
   * **Project Name:** `EZLogger`
   * **Location:** `C:\Users\<you>\repos\cs\ezlogger`
     *(Replace `<you>` with your user folder)*
   * **Solution Name:** `EZLogger_SLN`
   * **Framework:** **.NET Framework 4.8**
4. Click **Create** ➜ build (**Ctrl+Shift+B**) to verify a clean compile.

### 2  Add the WPF User Control (`ReportWizardPanel`)
1. **Solution Explorer** – right‑click the project ▸ **Add ▸ New Item**.
2. Choose **WPF User Control**.
3. Name it **ReportWizardPanel.xaml** ➜ **Add**.
4. Open the XAML designer and design your panel (labels, text boxes, buttons, etc.).

### 3  Add a Ribbon (`EZLoggerRibbon`)
1. Right‑click the project ▸ **Add ▸ New Item**.
2. Select **Ribbon (Visual Designer)**.
3. Name it **EZLoggerRibbon** ➜ **Add**.
4. In the designer:
   1. Add a **Group** – name it **WizardGroup**.
   2. Add a **Button** inside the group – name it **ReportWizardButton** and set **Label** to _Report Wizard_.

### 4  Create the WinForms container (`ReportWizardTaskPaneContainer`)
1. Right‑click the project ▸ **Add ▸ New Item**.
2. Select **User Control** (Windows Forms).
3. Name it **ReportWizardTaskPaneContainer.vb** ➜ **Add**.
4. Open the designer and drag an **ElementHost** from the Toolbox onto the control.
5. In **Properties** set **Name** = `ElementHost1`, **Dock** = **Fill**.

### 5  Host the WPF control in the task pane (`ThisAddIn.vb`)
Replace the template code with:
```vbnet
' ThisAddIn.vb – created by VSTO when the project was generated.
Imports System.Windows.Forms.Integration

Public Class ThisAddIn
    ' A single cached instance of the custom task pane.
    Private _taskPane As Microsoft.Office.Tools.CustomTaskPane

    ' Executes when Word loads the add‑in.
    Private Sub ThisAddIn_Startup() Handles Me.Startup
        ' 1️⃣  Create the WinForms container (ElementHost lives inside)
        Dim container As New ReportWizardTaskPaneContainer()

        ' 2️⃣  Inject the WPF UserControl into ElementHost.Child
        container.ElementHost1.Child = New ReportWizardPanel()

        ' 3️⃣  Register the container as a custom task pane titled "Report Wizard"
        _taskPane = CustomTaskPanes.Add(container, "Report Wizard")

        ' 4️⃣  Hide by default – the Ribbon button toggles visibility
        _taskPane.Visible = False
        _taskPane.Width = 350 ' Set an initial width (optional)
    End Sub

    ' Clean‑up logic if required (usually empty)
    Private Sub ThisAddIn_Shutdown() Handles Me.Shutdown
    End Sub

    ' Expose the pane to Ribbon code.
    Public ReadOnly Property ReportWizardTaskPane As Microsoft.Office.Tools.CustomTaskPane
        Get : Return _taskPane : End Get
    End Property
End Class
```
Each numbered comment explains precisely what the line does.

### 6  Add ScrollViewer to the WPF panel (prevents clipping)
```xml
<!-- ReportWizardPanel.xaml -->
<UserControl ... MinWidth="300">
    <!--  Wrap panel in ScrollViewer so users can scroll on small screens  -->
    <ScrollViewer VerticalScrollBarVisibility="Auto">
        <StackPanel Margin="12">
            <TextBlock Text="Patient Number:" />
            <TextBox x:Name="TxtPatientNumber" />
            <Button x:Name="BtnLookup" Content="Lookup" Margin="0,12,0,0" />
        </StackPanel>
    </ScrollViewer>
</UserControl>
```
> **Good** – Users with 1366 × 768 screens can scroll.
>
> **Bad** – No `ScrollViewer`; lower controls disappear off‑screen.

### 7  Toggle the pane from the Ribbon
```vbnet
' EZLoggerRibbon.vb – generated by the Ribbon Designer.
Public Class EZLoggerRibbon
    ' Click handler wired up automatically by the Designer.
    Private Sub ReportWizardButton_Click(
        sender As Object, e As RibbonControlEventArgs) _
        Handles ReportWizardButton.Click

        Dim pane = Globals.ThisAddIn.ReportWizardTaskPane ' Get cached pane
        pane.Visible = Not pane.Visible                  ' Toggle visibility

        If pane.Visible Then                             ' UX nicety
            pane.Window.Focus()                          ' Return focus to pane
        End If
    End Sub
End Class
```

### 8  Run & test
1. Press **F5** or **▶ Start**. Word launches with the add‑in loaded.
2. Locate the custom tab containing **Report Wizard** and click the button.
3. The pane appears – click again to hide.
4. Resize Word’s window to ensure the `ScrollViewer` activates when needed.

### 9  Commit your work
Use Git:
```bash
git add .
git commit -m "Add Report Wizard task pane with toggle and ScrollViewer"
```

---

## Good vs. Bad Implementation Patterns
| Concern | ✅ Good | ❌ Bad |
| --- | --- | --- |
| Pane lifecycle | Cache one pane in `Startup`; toggle `Visible`. | Call `CustomTaskPanes.Add` every click (memory leak). |
| Async I/O | `Await Task.Run(AddressOf HeavyQuery)` inside `BtnLookup_Click`. | Block UI with long‑running database call. |
| DPI | Use dynamic sizing (`SizeToContent`). | Hardcode widths – text truncates at 150 % DPI. |
| Error handling | Wrap UI handlers in `Try…Catch` and log to `Trace`. | Let exceptions bubble – Word swallows some silently. |
| Focus | Call `pane.Window.Focus()` after long operations. | Ignore – user wonders where the caret went. |

---

## Common Pitfalls
* **ElementHost keyboard issues** – Call `System.Windows.Input.InputMethod.SetIsInputMethodEnabled` if keys stop working.
* **Ribbon icons look blurry** – Provide both 16 × 16 and 32 × 32 PNGs.
* **Mixed bitness deployment** – Provide separate installers or compile twice.
* **`NullReferenceException` at startup** – Ensure `container.ElementHost1` exists **before** assigning its `Child`.
* **Unreleased references** – Detach event handlers in `ReportWizardPanel.Unloaded`.

---

## Next steps
* Replace placeholder `MsgBox` calls with your custom `CustomMsgBox` helper.
* Add a ViewModel if you migrate toward MVVM later.
* Automate installer creation with _VSTO Installer Maker_.

---

<!-- @nested-tags:task-pane, wpf-task-pane, vsto-word-wpf -->

