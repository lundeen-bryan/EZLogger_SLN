# Task Pane Layout Settings for `ReportWizardTaskPaneContainer` and `ElementHost1`

## `ReportWizardTaskPaneContainer` Settings:
- **Purpose**: `ReportWizardTaskPaneContainer` is a WinForms `UserControl` that contains the `ElementHost`, which hosts the WPF control (`ReportWizardPanel1`).

### Key Properties:
1. **`Size`**: Set the `Size` property of the `ReportWizardTaskPaneContainer` to ensure it has the desired default width and height when the Task Pane opens.
   - **Width**: `800` (or the desired width of the Task Pane)
   - **Height**: `600` (or the desired height of the Task Pane)

2. **`AutoSize`**: Set this property to **`False`** to prevent the container from resizing automatically based on the contents inside.
   - **AutoSize = False** ensures the container's size remains fixed at the values set in the `Size` property.

3. **`MaximumSize` and `MinimumSize`**:
   - Ensure that **`MaximumSize`** and **`MinimumSize`** are either not set or set to the appropriate values. For example, set them to **`0, 0`** to avoid any constraints on the width or height.

### Example:
```text
Size: 800, 600
AutoSize: False
MaximumSize: 0, 0
MinimumSize: 0, 0
```

## `ElementHost1` Settings:
- **Purpose**: `ElementHost1` is a Windows Forms control used to host the WPF `ReportWizardPanel1`.

### Key Properties:
1. **`Dock`**: Set the `Dock` property of `ElementHost1` to **`Fill`**. This ensures that the `ElementHost` will automatically expand to fill the entire `ReportWizardTaskPaneContainer` when it is displayed.
   - **Dock = Fill** ensures that the `ElementHost` adjusts to the size of its parent container (`ReportWizardTaskPaneContainer`), both in terms of width and height.

2. **`Anchor`**: The `Anchor` property is typically ignored when `Dock = Fill` is set. Therefore, you don’t need to adjust this property, as `Dock = Fill` takes precedence in determining the layout behavior.

### Example:
```text
Dock: Fill
Anchor: Top, Left (This is ignored when Dock = Fill)
```

## `ReportWizardPanel1` (WPF Control) Settings:
- **Purpose**: `ReportWizardPanel1` is the WPF control hosted inside the `ElementHost`.

### Key Properties:
1. **Design-Time Width and Height**: In the XAML file for `ReportWizardPanel1`, the `d:DesignWidth` and `d:DesignHeight` properties specify the design-time dimensions for the control, but they do not affect the runtime behavior when it is hosted inside the `ElementHost`.
   - **d:DesignWidth = 800**
   - **d:DesignHeight = 450**

2. **Runtime Behavior**: The actual size of `ReportWizardPanel1` will be determined by the size of the `ElementHost`, which is influenced by the parent container (`ReportWizardTaskPaneContainer`). Since the `ElementHost` is set to **`Dock = Fill`**, `ReportWizardPanel1` will automatically resize to fit within the Task Pane as it adjusts to the container’s size.

### Example:
```xml
<UserControl x:Class="ReportWizardPanel"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
             xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
             mc:Ignorable="d"
             d:DesignHeight="450" d:DesignWidth="800">
  <Grid>
      <!-- Your controls here -->
  </Grid>
</UserControl>
```

---

## Final Configuration Overview:
- **`ReportWizardTaskPaneContainer`** should have the **`Size`** set to the desired width (e.g., **800px**) and height (e.g., **600px**), with **`AutoSize = False`** to prevent resizing based on content.
- **`ElementHost1`** inside the container should have **`Dock = Fill`**, which ensures that it resizes automatically with the container, filling the entire available width and height.
- The **`ReportWizardPanel1`** (WPF control) will automatically adjust to the size of the `ElementHost`, which is controlled by the parent container (`ReportWizardTaskPaneContainer`).

<!-- @nested-tags:task-pane -->