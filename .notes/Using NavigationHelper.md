# Using the NavigationHelper Module in EZLogger

## Overview
The `NavigationHelper` module in the EZLogger project provides a standardized way to navigate to the **first** or **last** page of the active Microsoft Word document. This is especially useful in forms where users need quick access to key pages without manually scrolling.

This guide explains:
- What the `NavigationHelper` module does
- Preconditions to check before implementation
- How to call it from your handlers
- How to wire it up in your WPF views
- How to optionally convert the feature into a reusable control

---

## Module Location
`EZLogger\Helpers\NavigationHelper.vb`

## Public Methods

```vb
Public Sub GoToFirstPage(wordApp As Word.Application)
```

Navigates to the first page of the active Word document.

```vb
Public Sub GoToLastPage(wordApp As Word.Application)
```

Navigates to the last page of the active Word document.

---

## Preconditions and Preparation
Before adding code:

### In the WPF View:
- Confirm you have or want buttons named `BtnFirstPage` and `BtnLastPage`, or your own consistent naming pattern (e.g., `BtnOpinionFirstPage`, `BtnOpinionLastPage`).
- If similar button handlers already exist (e.g., `BtnFirstPage_Click`), **delete or rename them** to avoid duplicate handlers or conflicting wiring.
- Verify the `x:Name` properties in the XAML match the names you're using in code.

### In the Handler:
- Ensure the handler class (e.g., `{HandlerNameHere}`) includes a constructor that requires a `Word.Application` object.

```vb
  Public Sub New(wordApp As Word.Application)
      _wordApp = wordApp
  End Sub
```

- Whenever you create a new instance of this handler, you **must** pass the `Word.Application` explicitly:

```vb
  Dim handler As New {HandlerNameHere}(Globals.ThisAddIn.Application)
```

- ❗ Failure to do this will result in a **compile-time error**, because VB.NET does not allow parameterless constructor calls when none is defined.

Once these are verified, proceed to implementation.

---

## Sample Usage in a Handler
Create or update a handler class (e.g., `{HandlerNameHere}.vb`) and call the `NavigationHelper` methods:

This code would go in a handler not in the code behind a xaml user control

```vb
Imports Word = Microsoft.Office.Interop.Word
Imports EZLogger.Helpers

Public Class {HandlerNameHere}

    Private ReadOnly _wordApp As Word.Application

    Public Sub New(wordApp As Word.Application)
        _wordApp = wordApp
    End Sub

    Public Sub HandleFirstPageClick()
        NavigationHelper.GoToFirstPage(_wordApp)
    End Sub

    Public Sub HandleLastPageClick()
        NavigationHelper.GoToLastPage(_wordApp)
    End Sub

End Class
```

---

## Wiring Up the View
Inside your WPF form (e.g., `SomeView.xaml.vb`), wire up the buttons like this:

```vb
Private _handler As New {HandlerNameHere}(Globals.ThisAddIn.Application)

Private Sub BtnFirstPage_Click(sender As Object, e As RoutedEventArgs)
    _handler.HandleFirstPageClick()
End Sub

Private Sub BtnLastPage_Click(sender As Object, e As RoutedEventArgs)
    _handler.HandleLastPageClick()
End Sub
```

If you prefer to use `AddHandler` in the constructor:

```vb
AddHandler BtnFirstPage.Click, AddressOf BtnFirstPage_Click
AddHandler BtnLastPage.Click, AddressOf BtnLastPage_Click
```

Make sure you do **not** mix `Handles` with `AddHandler` for the same event.

---

## Optional: Creating a Reusable Control
You can also encapsulate this feature in a drop-in `UserControl` named `NavigationButtons.xaml`. This allows consistent reuse across forms:

```xml
<StackPanel Orientation="Horizontal">
    <Button x:Name="BtnFirstPage" Content="First Page" />
    <Button x:Name="BtnLastPage" Content="Last Page" />
</StackPanel>
```

In the `.xaml.vb` code-behind, raise events or call `NavigationHelper` directly. Then, drop this control into any view and pass the `Word.Application` instance as needed.

---

## Best Practices
- Always use `Globals.ThisAddIn.Application` to get the current Word application instance.
- Keep UI logic in the view or handler, not in the helper module.
- Handle exceptions gracefully with `Try...Catch` blocks as done in the helper.
- Clean up or remove any pre-existing duplicate logic to avoid conflicts.
- Be consistent when passing dependencies into constructors — this improves maintainability and makes unit testing easier.

---

## Conclusion
The `NavigationHelper` module simplifies page navigation logic and promotes reuse across forms(Views). By wiring it through handlers and keeping button logic clean, you ensure that the UI stays consistent and the code remains maintainable.

For even more convenience, consider implementing a `NavigationButtons` control to reduce code duplication.