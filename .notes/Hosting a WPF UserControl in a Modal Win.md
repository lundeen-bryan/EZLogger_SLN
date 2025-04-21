Hosting a WPF UserControl in a Modal Windows Form (VB.NET VSTO Add-in)
======================================================================

This guide explains how to host a WPF `UserControl` inside a modal-like Windows Form window in a VB.NET VSTO Word Add-in.

Table of Contents
-----------------

1.  [Create the WPF UserControl](#create-usercontrol)
2.  [Create the Windows Form with ElementHost](#create-form)
3.  [Write Code to Load the WPF Control](#form-logic)
4.  [Add the Ribbon Button Logic](#ribbon-code)
5.  [Rename for Clarity (Optional)](#optional-names)
6.  [Test the Modal Window](#testing)

1\. Create the WPF UserControl
------------------------------

1.  Right-click the project and select **Add > New Item**.
2.  Choose **WPF User Control** and name it `UserControl1.xaml`.
3.  Design the control, for example by adding a Label that says "About This Add-in".

2\. Create the Windows Form with ElementHost
--------------------------------------------

1.  Right-click the Hosts folder and select **Add > Windows Form**.
2.  Name it `Form1.vb`.
3.  In the Form designer, drag an **ElementHost** control from the Toolbox onto the form.
4.  Set its **Name** to `ElementHost1` and **Dock** to `Fill`.

3\. Write Code to Load the WPF Control
--------------------------------------

Double-click on the Form to open the code view and add the following to `Form1_Load`:

(see "code_behind_WinForm_Host.json" for template code)

```vb
Imports System.Windows.Forms.Integration
    Public Class Form1[the Hosting form name]
        Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            Dim myControl As New UserControl1()[The view or control name]
            ElementHost1.Dock = DockStyle.Fill
            ElementHost1.Child = myControl
        End Sub
    End Class
```

4\. Add the Ribbon Button Logic
-------------------------------

Open your Ribbon class file (e.g., `EZLoggerRibbon.vb`) and add:

```vb
    Private Sub AboutButton_Click(sender As Object, e As RibbonControlEventArgs) Handles AboutButton.Click
        Dim aboutForm As New Form1()
        aboutForm.StartPosition = FormStartPosition.CenterScreen
        aboutForm.Show() ' Use ShowDialog() for true modal behavior
    End Sub
```

If you want to block interaction with Word while the window is open, replace `Show()` with `ShowDialog()`.

5\. Rename for Clarity (Optional)
---------------------------------

You can rename these components to better reflect their purpose:

*   **Form1** → `AboutWindow`
*   **UserControl1** → `AboutControl`

Be sure to update all references accordingly in your code.

6\. Test the Modal Window
-------------------------

1.  Press **F5** in Visual Studio to launch Word with your Add-in.
2.  Click the Ribbon button (e.g., **About**).
3.  The modal-like window should appear, centered, hosting your WPF control.

Tips
----

*   Ensure the `WindowsFormsIntegration` assembly is referenced in your project.
*   Use `ShowDialog()` if you want to disable Word interaction while the form is open.
*   Keep WPF logic inside the UserControl to keep the Form clean and reusable.

<!-- @nested-tags:wpf-user-control -->