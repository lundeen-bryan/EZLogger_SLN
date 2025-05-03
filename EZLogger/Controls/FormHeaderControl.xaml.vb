Imports System.Windows
Imports System.Windows.Controls

Namespace EZLogger.Controls
    Partial Public Class FormHeaderControl
        Inherits UserControl

        ' Define the dependency property
        Public Shared ReadOnly HeaderTextProperty As DependencyProperty =
            DependencyProperty.Register("HeaderText", GetType(String), GetType(FormHeaderControl), New PropertyMetadata("Default Header"))

        Public Sub New()
            InitializeComponent()
        End Sub

        ' Wrapper property
        Public Property HeaderText As String
            Get
                Return GetValue(HeaderTextProperty)
            End Get
            Set(value As String)
                SetValue(HeaderTextProperty, value)
            End Set
        End Property
    End Class
End Namespace

