Imports System.Windows.Forms

Public Class AboutEZLoggerHost
    ''' <summary>
    ''' Handles the Load event of the AboutEZLoggerHost form.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    ''' <remarks>
    ''' This method is called when the form is loaded. It creates a new instance of the AboutEZLogger control,
    ''' sets the ElementHost1 to fill the entire form, and assigns the AboutEZLogger control as the child of ElementHost1.
    ''' </remarks>
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim myControl As New AboutEZLogger()
        ElementHost1.Dock = DockStyle.Fill
        ElementHost1.Child = myControl
    End Sub
End Class
