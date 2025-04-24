# How to Add Keyboard Shortcuts to WPF Buttons Hosted in WinForms (EZLogger Pattern)

> Editor's note: it's easiest to simply put the underscore in front of a letter that you want to use as an accelerator in the xaml as "content" for a button. However this article is geared towards use cases where you want to add button functionality to situations where the keyboard shortcut is captured by the main Word object and triggers the Word Ribbon rather than the view. 

This guide explains how to make any WPF button respond to a keyboard shortcut (like Alt+S) when the button is hosted inside a WinForms `ElementHost` — as used in EZLogger.

Note that for most cases it works well enough to add an underscore before the letter that you want to use as an accelerator key (in the XAML), however, sometimes in VSTO the Word Object is in front and then keyboard shortcuts are activated in the Word document Ribbon instead. So this article applies to those cases where you need to set focus back on the WPF user control. 

## Requirements
- The WPF `UserControl` is hosted inside a WinForms form (`ElementHost1.Child = New SomeView()`)
- The button already exists in the XAML and has a name (e.g., `BtnSave`)
- The form uses `KeyPreview = True`
- `ShortcutHandler.vb` is available in `EZLogger.Helpers`

## Step-by-Step: Add a Shortcut to a WPF Button

### 1. Define the WPF Button with an Access Key (Optional but Recommended)

In your `*.xaml` file:

```xml
<Button x:Name="BtnSave" Content="_Save" />
```

The underscore before "S" creates a visual access key — it underlines the letter when the user holds Alt to indicate that the button has an accelerator. This is purely a visual cue and does not trigger functionality by itself.

### 2. Set Up a Click Event in the View's Code-Behind

In the code-behind for the WPF view (e.g., `UpdateInfoView.xaml.vb`):

```vbnet
Private Sub BtnSave_Click(sender As Object, e As RoutedEventArgs) Handles BtnSave.Click
    ' This method will be triggered when the button is clicked, either manually or programmatically
    _handler.HandleSaveClick()
End Sub
```

- `Handles BtnSave.Click` links this method to the `BtnSave` control in the XAML.
- This event will be triggered when the user clicks the button with a mouse or when we simulate a click using a keyboard shortcut.

Make sure the button is named properly in XAML:

```xml
<Button x:Name="BtnSave" Content="_Save" />
```

### 3. Register the Shortcut in the Host WinForms Form

In your host form (e.g., `UpdateInfoHost.vb`), follow these steps.

#### Add the following imports at the top:

```vb
Imports Keys = System.Windows.Forms.Keys
Imports System.Windows.Forms
Imports System.Windows.Forms.Keys
Imports System.Windows.Input
Imports EZLogger.Helpers
Imports EZLogger.Views
```

#### In the Load Event, wire up the ElementHost and ShortcutHandler

```vb
Private _shortcutHandler As ShortcutHandler

Private Sub UpdateInfoHost_Load(sender As Object, e As EventArgs) Handles MyBase.Load
    ' Instantiate and assign the WPF UserControl to the host
    Dim view As New UpdateInfoView()
    ElementHost1.Child = view

    ' Set form appearance and behavior
    Me.ClientSize = New Drawing.Size(485, 687)
    Me.Text = "Update Info"
    Me.MinimizeBox = False
    Me.MaximizeBox = False
    Me.ShowIcon = False
    Me.FormBorderStyle = FormBorderStyle.FixedSingle
    Me.StartPosition = FormStartPosition.CenterScreen

    ' Resize the ElementHost manually (optional)
    ElementHost1.Width = Me.ClientSize.Width - 40
    ElementHost1.Height = Me.ClientSize.Height - 40
    ElementHost1.Location = New Drawing.Point(20, 20)

    ' Enable shortcut key preview
    Me.KeyPreview = True

    ' Register Alt+S to simulate button click
    _shortcutHandler = New ShortcutHandler(Me)
    _shortcutHandler.RegisterShortcut(Keys.S, Keys.Alt, Sub()
        Dim activeView = TryCast(ElementHost1.Child, UpdateInfoView)
        If activeView IsNot Nothing Then
            activeView.BtnSave.RaiseEvent(New RoutedEventArgs(System.Windows.Controls.Button.ClickEvent))
        End If
    End Sub)
End Sub
```

> For more on how Load events work in this pattern, please see the other article on **"Load Events in WinForms-Hosted WPF Views"**.

## Reuse Tip
If you’re doing this often, you can make a helper method to reduce repetition when simulating button clicks from keyboard shortcuts. This helper receives a WPF `Button` and programmatically raises its `Click` event using WPF's routed event system:

```vb
Public Sub TriggerButtonClick(button As System.Windows.Controls.Button)
    button.RaiseEvent(New RoutedEventArgs(System.Windows.Controls.Button.ClickEvent))
End Sub
```

### Example 1: Trigger Save Button
If you want `Alt+S` to trigger `BtnSave`:

```vb
_shortcutHandler.RegisterShortcut(Keys.S, Keys.Alt, Sub()
    Dim view = TryCast(ElementHost1.Child, UpdateInfoView)
    If view IsNot Nothing Then
        TriggerButtonClick(view.BtnSave)
    End If
End Sub)
```

### Example 2: Trigger Generate ID Button
If you want `Alt+G` to trigger `BtnGenerateId`:

```vb
_shortcutHandler.RegisterShortcut(Keys.G, Keys.Alt, Sub()
    Dim view = TryCast(ElementHost1.Child, UpdateInfoView)
    If view IsNot Nothing Then
        TriggerButtonClick(view.BtnGenerateId)
    End If
End Sub)
```

This allows you to keep all button click simulation logic consistent and reusable.

```vb
Public Sub TriggerButtonClick(button As System.Windows.Controls.Button)
    button.RaiseEvent(New RoutedEventArgs(System.Windows.Controls.Button.ClickEvent))
End Sub
```

Then your shortcut registration becomes:

```vb
RegisterShortcut(Keys.S, Keys.Alt, Sub() TriggerButtonClick(view.BtnSave))
```

## Important Notes
- `KeyPreview = True` is required on the host form for shortcut detection.
- Shortcuts only work when the host form is active.
- You can use any key combo (Alt, Ctrl, Shift) — just avoid conflicts with Word or OS shortcuts.

## Summary
To wire a keyboard shortcut to a WPF button:
1. Define a visual access key with `_` in the XAML button content (optional).
2. Set up a click handler in the view's code-behind.
3. Register the keyboard shortcut in the WinForm using `ShortcutHandler`.

This pattern ensures consistent keyboard behavior across all WPF panels hosted in EZLogger.

<!-- @nested-tags:wpf-user-control -->