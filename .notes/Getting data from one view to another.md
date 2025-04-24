# Getting Data From One View To Another

> When user selects a row in PatientInfoView - typically a key-value pair from the document properties - we need to pass that selection into UpdateInfoView so the user can edit it

To do this we declare two string variables:

```vb
Public Property InitialPropertyName As String = ""
Public Property InitialPropertyValue As String = ""
```

These act as temporary holders for the selected property's name and value.

## Why we do this:

Since both views are hosted separately in their own WinForms `ElementHost`, we can't bind them directly or use MVVM-style data binding across views. Instead we treat these variables as simple message-passing fields.

## How it works:

1. In `PatientInfoHandler`, when the "Edit" button is clicked, we retrieve the selected row's key and value.
2. We then create an instance of `UpdateInfoHost`, assign the values to `UpdateInfoView.InitialPropertyName` and `InitialPropertyValue`, and display the host.
3. `UpdateInfoView` loads these values in it's _Loaded event and fills in the appropriate `TextBoxPropertyName` and `TextBoxPropertyValue` controls.

This pattern provides a simple way to pass values between views in a VSTO environment without relying on static state or shared singletons. 

<!-- @nested-tags:reusing-properties -->
