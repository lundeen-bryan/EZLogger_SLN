# How to Populate WPF ComboBoxes from a JSON Config File in VB.NET

This guide walks you through populating a WPF `ComboBox` with values stored in a `global_config.json` file using VB.NET. It follows a pattern used in EZLogger to keep your app modular, maintainable, and friendly to non-technical users.

---

## Prerequisites
- A WPF project in VB.NET
- A JSON config file structured like:

```json
{
  "listbox": {
    "opinions": ["Competent", "Not Yet Competent", "Malingering"],
    "report_type": ["PPR", "1370(b)(1)"]
  }
}
```

- A second config file, `local_user_config.json`, that includes the path to `global_config.json`:

```json
{
  "sp_filepath": {
    "global_config_file": "C:\\path\\to\\global_config.json"
  }
}
```

---

## Step 1: Create the ConfigHelper
Create a helper module `ConfigHelper.vb` to load values from the config files.

### Essential Methods Only
```vbnet
Namespace EZLogger.Helpers
    Public Module ConfigHelper

        ' Hardcoded path to the local user config (prototype phase)
        Private ReadOnly localConfigPath As String = "C:\Users\you\yourproject\temp\local_user_config.json"

        ' Get path to global config from the local config
        Public Function GetGlobalConfigPath() As String
            ' Read local config and extract sp_filepath.global_config_file
            ' [Implementation of nested JSON reading logic goes here]
        End Function

        ' Get list of opinions from the global config
        Public Function GetOpinionList() As List(Of String)
            ' Load global config, navigate to listbox.opinions, and return the values
        End Function

        ' Get list of report types from the global config
        Public Function GetReportTypeList() As List(Of String)
            ' Load global config, navigate to listbox.report_type, and return the values
        End Function

    End Module
End Namespace
```

> Tip: You can include error handling and logging inside each method, especially for missing files or invalid JSON.

---

## Step 2: Bind to the ComboBox in Code-Behind
In your `.xaml.vb` file, add the following:

### For Opinion ComboBox:
```vbnet
Imports EZLogger.Helpers

Public Class OpinionView
    Private Sub OpinionView_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        OpinionCbo.ItemsSource = ConfigHelper.GetOpinionList()
    End Sub
End Class
```

### For Report Type ComboBox:
```vbnet
ReportTypeCbo.ItemsSource = ConfigHelper.GetReportTypeList()
```

---

## Troubleshooting and Pitfalls

### 1. **Missing or incorrect file paths**
- Make sure `local_user_config.json` exists and includes the correct path to `global_config.json`.
- Tip: Always check for empty strings before trying to load files.

### 2. **Incorrect or nested JSON structure**
- If `global_config_file` is nested (e.g. inside `sp_filepath`), be sure your code navigates into that object properly.
- Use `TryGetProperty` to safely check and access nested keys.

### 3. **ComboBox not showing values**
- Confirm your control is named correctly in XAML (e.g., `x:Name="OpinionCbo"`).
- Ensure `ItemsSource` is assigned in the `Loaded` event.

### 4. **Hardcoded values still appearing**
- Remove `<ComboBoxItem>` entries from XAML if you're binding dynamically, or they may conflict with `ItemsSource`.

### 5. **Build errors due to namespace issues**
- Make sure the helper module is in the correct namespace and your form is importing it properly with `Imports EZLogger.Helpers`

---

Now you have a reusable way to dynamically populate dropdowns from config files. 🎉

<!-- @nested-tags:populate-controls -->