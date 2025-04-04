# Shared ComboBox Population and Syncing Strategy in EZLogger

## Purpose
This document explains how the report type ComboBoxes in `ReportWizardPanel` and `ReportTypeView` are populated using a shared list, how the selected item is passed between the forms, and how this architecture can be reused to populate other controls like ListBoxes or additional ComboBoxes.

## Summary of Behavior
- The report type list is defined once in `ReportTypeHandler.GetReportTypes()`.
- Both `ReportWizardPanel` and `ReportTypeView` use this list to populate their ComboBoxes.
- When a user opens the `ReportTypeView` form, the current selection is passed to it.
- If the user changes the selection and closes the form, the new value is synced back to the main panel.

---

## Components and Key Logic

### 1. Shared Method in `ReportTypeHandler.vb`
```vb
Public Function GetReportTypes() As List(Of String)
    Return New List(Of String) From {
        "1370(b)(1)",
        "1372(a)(1)",
        "UNLIKELY 1370(b)(1)",
        "PPR"
    }
End Function
```

### 2. Populate ComboBox in `ReportWizardPanel.xaml.vb`
```vb
Private Sub ReportWizardPanel_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
    ReportTypeCbo.ItemsSource = rthandler.GetReportTypes()
End Sub
```

### 3. Open Form and Sync Selection
```vb
Private Sub ConfirmReportTypeButton_Click(sender As Object, e As RoutedEventArgs)
    Dim selectedItem = ReportTypeCbo.SelectedItem

    If selectedItem IsNot Nothing Then
        Dim currentSelection As String = selectedItem.ToString()
        Dim newSelection As String = rthandler.OnConfirmReportTypeButtonClick(currentSelection)

        If Not String.IsNullOrWhiteSpace(newSelection) AndAlso newSelection <> currentSelection Then
            ReportTypeCbo.SelectedItem = newSelection
        End If
    Else
        MessageBox.Show("Please select a report type first.", "No Selection")
    End If
End Sub
```

### 4. Update `OnConfirmReportTypeButtonClick` to Pass and Return Value
```vb
Public Function OnConfirmReportTypeButtonClick(reportType As String) As String
    Dim host As New ReportTypeHost()
    Dim reportTypeView = CType(host.ElementHost1.Child, ReportTypeView)
    reportTypeView.InitialSelectedReportType = reportType

    host.ShowDialog()
    Return reportTypeView.ReportTypeViewCbo.SelectedItem?.ToString()
End Function
```

### 5. Initialize ComboBox in `ReportTypeView.xaml.vb`
```vb
Public Property InitialSelectedReportType As String

Private Sub ReportTypeView_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
    Dim reportTypes As List(Of String) = rthandler.GetReportTypes()
    ReportTypeViewCbo.ItemsSource = reportTypes

    If Not String.IsNullOrEmpty(InitialSelectedReportType) AndAlso reportTypes.Contains(InitialSelectedReportType) Then
        ReportTypeViewCbo.SelectedItem = InitialSelectedReportType
    End If
End Sub
```

---

## How to Reuse This Pattern for Other Controls

1. **Define a Shared Data Method**
   - Use a `List(Of String)` or other appropriate type.
   - Place it in a shared handler or utility class.

2. **Load the Data in Your Control**
   - For WPF:
     ```vb
     MyComboBox.ItemsSource = SharedHandler.GetItems()
     ```
   - For WinForms:
     ```vb
     MyComboBox.Items.AddRange(SharedHandler.GetItems().ToArray())
     ```

3. **Sync Between Forms or Views**
   - Pass the selected value via a public property.
   - Use `ShowDialog()` to wait for the user's selection.
   - Return the selected value and update the original control.

4. **Validation (Optional but Recommended)**
   - Always check for `Nothing` or empty selection.
   - Notify users with a message box if needed.

---

## Benefits
- Promotes DRY (Don't Repeat Yourself) principle
- Works across WPF and WinForms
- Makes forms easier to test and maintain
- Keeps data centralized and consistent

---

## Future Improvements
- Convert data source to load from config file or database
- Add unit tests for shared data logic
- Create a generic `ControlHelper` class to load lists into any control type

