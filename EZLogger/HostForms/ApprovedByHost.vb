Imports System.Windows.Forms

Public Class ApprovedByHost
    ''' <summary>
    ''' Initializes the form and sets up the ApprovedByControl within the ElementHost.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">An EventArgs that contains the event data.</param>
    ''' <remarks>
    ''' This method is called when the form loads. It creates a new ApprovedByControl,
    ''' sets the ElementHost to fill the form, and assigns the control as the ElementHost's child.
    ''' </remarks>
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim myControl As New ApprovedByControl()
        ElementHost1.Dock = DockStyle.Fill
        ElementHost1.Child = myControl
    End Sub
End Class
