<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ReportWizardTaskPaneContainer
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.ElementHost1 = New System.Windows.Forms.Integration.ElementHost()
        'Me.ReportWizardPanel1 = New EZLogger.EZLogger.ReportWizardPanel()
        Me.SuspendLayout()
        '
        'ElementHost1
        '
        Me.ElementHost1.BackColor = System.Drawing.SystemColors.Control
        Me.ElementHost1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ElementHost1.Location = New System.Drawing.Point(0, 0)
        Me.ElementHost1.Name = "ElementHost1"
        Me.ElementHost1.Size = New System.Drawing.Size(302, 319)
        Me.ElementHost1.TabIndex = 0
        Me.ElementHost1.Text = "ElementHost1"
        Me.ElementHost1.Child = Me.ReportWizardPanel1
        '
        'ReportWizardTaskPaneContainer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.ElementHost1)
        Me.Name = "ReportWizardTaskPaneContainer"
        Me.Size = New System.Drawing.Size(302, 319)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents ElementHost1 As Windows.Forms.Integration.ElementHost
    Friend ReportWizardPanel1 As ReportWizardPanel
End Class
