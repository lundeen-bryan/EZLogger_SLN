# PASSING VALUES FROM THE VIEW TO THE HANDLER

In `ReportTypeView`, we retrieve the selected value from the ComboBox (`ReportTypeCbo`) directly in the view's code behind and pass it into the handler, rather than having the handler pull data from the UI.

This pattern is intentional and follows a key design principle:

>"Handlers should not reach into views to get control values."

Instead the view is responsible for reading the control state (such as `SelectedItem` or `SelectedDate`) and sending those values as arguments when calling into the handler.

## Example


```vb
' Example from ReportTypeView.xaml.vb

Private Sub ReportTypeSelectedBtn_Click(sender As Object, e As RoutedEventArgs)
		Dim selectedReportType As String = TryCast(ReportTypeCbo.SelectedItem, String)
		Dim selectedReportDate As String = GetSelectedReportDate()
		_handler.ReportTypeSelectedBtnClick(selectedReportType, selectedReportDate, _hostForm)
End Sub
```

This ensures that:

- Views handle UI concerns (e.g., getting selected ComboBox items)
- Handlers stay clean and focused on processing logic, using only the data passed to them
- Tight coupling between UI controls and logic layers is avoided

It's a good practice for keeping views and handlers decoupled, maintainable, and testable.

<!-- @nested-tags: populate-controls -->
