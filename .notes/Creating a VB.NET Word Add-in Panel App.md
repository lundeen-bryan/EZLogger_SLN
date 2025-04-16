 Creating a VB.NET Word Add-in Panel App body { font-family: Arial, sans-serif; margin: 20px; line-height: 1.6; } h1, h2, h3 { color: #333; } pre { background-color: #f4f4f4; padding: 10px; border: 1px solid #ddd; overflow-x: auto; } code { background-color: #f4f4f4; padding: 2px 4px; } .step { margin-bottom: 20px; } ul, ol { margin-left: 20px; } a { text-decoration: none; color: #0066cc; }

Creating a VB.NET Word Add-in Panel App
=======================================

This guide details the steps to build a Word add-in in VB.NET that functions as a panel app (custom task pane) using a WPF User Control. The add-in is toggled by a Ribbon button and hosts the WPF panel within a Windows Forms container.

Table of Contents
-----------------

1.  [Project Setup](#project-setup)
2.  [Creating the WPF User Control (ReportWizardPanel)](#wpf-control)
3.  [Adding a Ribbon Control (EZLoggerRibbon)](#ribbon-control)
4.  [Creating a Windows Forms Container (ReportWizardTaskPaneContainer)](#taskpane-container)
5.  [Modifying the ThisAddIn.vb Module](#addin-code)
6.  [Making the WPF Panel Scrollable](#adding-scrollviewer)
7.  [Adding the Ribbon Button Event Handler](#ribbon-event)
8.  [Testing the Add-in in Word](#testing)
9.  [Committing Your Progress](#commit)

1\. Project Setup
-----------------

Follow these steps to create your project:

1.  Launch Visual Studio 2022 and create a new project.
2.  Select the **Word VSTO Add-in** template using VB.NET.
3.  Enter the following details:
    *   **Project Name:** EZLogger
    *   **Location:** `C:\Users\lunde\repos\cs\ezlogger\`
    *   **Solution Name:** EZLogger\_SLN
    *   **Framework:** .NET Framework 4.7.2
4.  Build the solution to confirm that the project is set up correctly.

2\. Creating the WPF User Control (ReportWizardPanel)
-----------------------------------------------------

This control will serve as the visual interface for your panel.

1.  Right-click the project in Solution Explorer and select **Add > New Item**.
2.  Choose **WPF User Control** from the list.
3.  Name it **ReportWizardPanel**.
4.  Design your panel by adding controls (such as labels, text boxes, and navigation buttons) from the Toolbox.

3\. Adding a Ribbon Control (EZLoggerRibbon)
--------------------------------------------

Create a Ribbon control that contains the button to toggle your panel.

1.  Right-click the project and select **Add > New Item**.
2.  Choose **Ribbon (Visual Designer)** and name it **EZLoggerRibbon**.
3.  In the Ribbon designer:
    1.  Add a new group and name it **WizardGroup** (this group can contain multiple buttons).
    2.  Add a button to this group and name it **ReportWizardButton**.

4\. Creating a Windows Forms Container (ReportWizardTaskPaneContainer)
----------------------------------------------------------------------

Because the `CustomTaskPanes.Add` method expects a Windows Forms UserControl, you must create a container to host your WPF control.

1.  Right-click the project and select **Add > New Item**.
2.  Select the **User Control** (Windows Forms) template.
3.  Name it **ReportWizardTaskPaneContainer**.
4.  Open the design view of **ReportWizardTaskPaneContainer** and drag an **ElementHost** control from the Toolbox onto the UserControl.
5.  Set the **Dock** property of the ElementHost (ensure it is named **ElementHost1**) to `Fill`.

5\. Modifying the ThisAddIn.vb Module
-------------------------------------

Update `ThisAddIn.vb` so that your WPF panel is hosted inside a custom task pane. Replace the file's content with the following code (or integrate these changes):

Imports System.Windows.Forms.Integration

Public Class ThisAddIn

    Private myTaskPane As Microsoft.Office.Tools.CustomTaskPane

    Private Sub ThisAddIn\_Startup() Handles Me.Startup
        ' Create an instance of the container UserControl
        Dim myTaskPaneContainer As New ReportWizardTaskPaneContainer()

        ' Set the ElementHost within the container to fill the UserControl
        myTaskPaneContainer.ElementHost1.Dock = System.Windows.Forms.DockStyle.Fill

        ' Create an instance of the WPF User Control (ReportWizardPanel)
        Dim myReportWizardPanel As New ReportWizardPanel()

        ' Assign the WPF control to the ElementHost's Child property
        myTaskPaneContainer.ElementHost1.Child = myReportWizardPanel

        ' Add the container to the CustomTaskPanes collection with the title "Report Wizard"
        myTaskPane = Me.CustomTaskPanes.Add(myTaskPaneContainer, "Report Wizard")

        ' Initially hide the task pane
        myTaskPane.Visible = False
    End Sub

    Private Sub ThisAddIn\_Shutdown() Handles Me.Shutdown
        ' Any necessary cleanup can be done here
    End Sub

    ' Expose the task pane so it can be accessed from the Ribbon code
    Public ReadOnly Property ReportWizardTaskPane As Microsoft.Office.Tools.CustomTaskPane
        Get
            Return myTaskPane
        End Get
    End Property
End Class


6\. Making the WPF Panel Scrollable
-----------------------------------

If your WPF interface exceeds the vertical space of the task pane, you can wrap the contents in a `ScrollViewer` so users can scroll.

Open your WPF control (e.g., `UserConfigView.xaml`) and wrap the entire layout `Grid` with a `ScrollViewer` like this:

This ensures the entire panel becomes scrollable when content overflows vertically.

7\. Adding the Ribbon Button Event Handler
------------------------------------------

Next, add the code to toggle the task pane's visibility when the `ReportWizardButton` is clicked.

1.  Open the Ribbon code-behind file (`EZLoggerRibbon.vb`).
2.  Add the following event handler:

Private Sub ReportWizardButton\_Click(sender As Object, e As RibbonControlEventArgs) Handles ReportWizardButton.Click
    Globals.ThisAddIn.ReportWizardTaskPane.Visible = Not Globals.ThisAddIn.ReportWizardTaskPane.Visible
End Sub


This code toggles the `Visible` property of the task pane each time the button is clicked.

8\. Testing the Add-in in Word
------------------------------

1.  Press **F5** or click the **Start** button in Visual Studio. This will launch Microsoft Word with your add-in loaded.
2.  In Word, locate the custom ribbon (**EZLoggerRibbon**) and click the **ReportWizardButton**.
3.  The task pane titled "Report Wizard" should appear. Click the button again to toggle it off.

9\. Committing Your Progress
----------------------------

After verifying that everything works as expected, commit your progress to your GitHub repository. A sample commit message might be:

"Implemented Report Wizard task pane with toggle functionality"


Additional Tips
---------------

*   If you see errors related to `ElementHost`, make sure that the `WindowsFormsIntegration` assembly reference is added to your project.
*   If changes are not reflected immediately, try cleaning and rebuilding your solution.
*   Review the Visual Studio Output window for any errors or warnings that might assist in troubleshooting.

By following these steps, you will have created a Word add-in in VB.NET that displays a WPF-based custom task pane. The pane can be toggled on and off using a Ribbon button, serving as a panel app or windows pane inside Word.

<!-- @nested-tags:task-pane -->