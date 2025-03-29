Imports System.Windows
Imports System.Windows.Controls

Public Class TaskStepControl
    Inherits UserControl

    ' Constructor to load XAML
    Public Sub New()
        ' This method is auto-generated and connects the XAML to this code-behind
        InitializeComponent()
    End Sub

    ' === ButtonContent Dependency Property ===
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

    ' === CheckBoxContent Dependency Property ===
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
