How to Expose Properties in a WPF UserControl (Step-by-Step)
============================================================

This guide will walk you through how to create a custom control in WPF that allows you to set the content of a button and a checkbox from outside the control — for example, in a window or another control that uses your custom control.

* * *

Step 1: Create the XAML File for Your Custom Control
----------------------------------------------------

In Visual Studio, create a new **UserControl** called `TaskStepControl.xaml`. Then, open the file and replace its contents with the following:

```xml
    <UserControl x:Class="TaskStepControl"
                 xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
                 xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
                 xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
                 xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
                 xmlns:local="clr-namespace:EZLogger"
                 mc:Ignorable="d"
                 d:DesignHeight="450" d:DesignWidth="800">
        <Grid>
            <GroupBox Margin="5,5,0,0"
                      VerticalAlignment="Top"
                      FontFamily="Lucida Fax"
                      Height="80"
                      BorderBrush="Black"
                      Foreground="#FF464646"
                      HorizontalAlignment="Left"
                      Width="335">
                <StackPanel Margin="-5,-14,-7,-6" Orientation="Horizontal">
                    <Button Content="{Binding ButtonContent, RelativeSource={RelativeSource AncestorType=UserControl}}"
                            Height="35"
                            Width="45"
                            Margin="12,6,0,0"/>
                    <CheckBox Content="{Binding CheckBoxContent, RelativeSource={RelativeSource AncestorType=UserControl}}"
                              Height="25"
                              Width="130"
                              Margin="6,12,0,0"/>
                </StackPanel>
            </GroupBox>
        </Grid>
    </UserControl>
```

This creates a basic layout with a button and a checkbox. The `Content` of both elements is bound to properties we’ll expose next.

Note the use of curly brackets and the keywords Binding and RelativeSource are necessary to make it work in the properties tab of a user control.

* * *

Step 2: Create the Code-Behind File (xaml.vb)
---------------------------------------------

Now open the `TaskStepControl.xaml.vb` file and replace any existing code with this:

```vb
    Imports System.Windows
    Imports System.Windows.Controls
    Public Class TaskStepControl
        Inherits UserControl
        ' Constructor
        Public Sub New()
            InitializeComponent()
        End Sub
        ' Define ButtonContent as a dependency property
        Public Shared ReadOnly ButtonContentProperty As DependencyProperty =
            DependencyProperty.Register(
                "ButtonContent",
                GetType(String),
                GetType(TaskStepControl),
                New PropertyMetadata("Button"))
        Public Property ButtonContent As String
            Get
                Return CType(GetValue(ButtonContentProperty), String)
            End Get
            Set(value As String)
                SetValue(ButtonContentProperty, value)
            End Set
        End Property
        ' Define CheckBoxContent as a dependency property
        Public Shared ReadOnly CheckBoxContentProperty As DependencyProperty =
            DependencyProperty.Register(
                "CheckBoxContent",
                GetType(String),
                GetType(TaskStepControl),
                New PropertyMetadata("CheckBox"))
        Public Property CheckBoxContent As String
            Get
                Return CType(GetValue(CheckBoxContentProperty), String)
            End Get
            Set(value As String)
                SetValue(CheckBoxContentProperty, value)
            End Set
        End Property
    End Class
```

These are dependency properties. They let the control expose values that can be set from XAML or changed dynamically in code.

* * *

Step 3: Use the Control in a Window or Another Control
------------------------------------------------------

Now go to the XAML of your main window — for example, `MainWindow.xaml` — and do the following:

### Step 3.1: Add the Namespace

At the top of your XAML file, add this to the `<Window>` element:

```xml
    xmlns:local="clr-namespace:EZLogger"
```

This lets you access the `TaskStepControl` from within the window.

### Step 3.2: Use the Control

Inside the window’s layout, add the control like this:

```xml
    <local:TaskStepControl
        ButtonContent="Run"
        CheckBoxContent="Enable Logging"
        Margin="20" />
```


This renders your custom control with a button that says "Run" and a checkbox that says "Enable Logging".

* * *

Step 4: (Optional) Set the Properties in Code
---------------------------------------------

If you gave your control a name in the window’s XAML:

```xml
    <local:TaskStepControl x:Name="MyTaskStepControl"
                            ButtonContent="Save"
                            CheckBoxContent="Confirm" />
```


You can set or change the property values in your code-behind file like this:

```vb
MyTaskStepControl.ButtonContent = "Start"
MyTaskStepControl.CheckBoxContent = "Agree to Terms"
```


* * *

Success!
--------

You’ve now created a reusable control with exposed properties. This same technique works for other controls too — like setting text, colors, visibility, and more.

### Keep in Mind:

*   Use dependency properties for any values you want to make public from a UserControl.
*   Always bind using `RelativeSource AncestorType=UserControl` when referencing properties on the same control.
*   Call `InitializeComponent()` in the constructor or your XAML won't show up at runtime.

<!-- @nested-tags:wpf-user-control -->