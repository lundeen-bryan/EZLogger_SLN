Partial Class EZLoggerRibbon
    Inherits Microsoft.Office.Tools.Ribbon.RibbonBase

    <System.Diagnostics.DebuggerNonUserCode()> _
    Public Sub New(ByVal container As System.ComponentModel.IContainer)
        MyClass.New()

        'Required for Windows.Forms Class Composition Designer support
        If (container IsNot Nothing) Then
            container.Add(Me)
        End If

    End Sub

    <System.Diagnostics.DebuggerNonUserCode()> _
    Public Sub New()
        MyBase.New(Globals.Factory.GetRibbonFactory())

        'This call is required by the Component Designer.
        InitializeComponent()

    End Sub

    'Component overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Component Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Component Designer
    'It can be modified using the Component Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Tab1 = Me.Factory.CreateRibbonTab
        Me.WizardGroup = Me.Factory.CreateRibbonGroup
        Me.ReportWizardButton = Me.Factory.CreateRibbonButton
        Me.Tab1.SuspendLayout()
        Me.WizardGroup.SuspendLayout()
        Me.SuspendLayout()
        '
        'Tab1
        '
        Me.Tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office
        Me.Tab1.Groups.Add(Me.WizardGroup)
        Me.Tab1.Label = "EZ Logger"
        Me.Tab1.Name = "Tab1"
        '
        'WizardGroup
        '
        Me.WizardGroup.Items.Add(Me.ReportWizardButton)
        Me.WizardGroup.Label = "Wizards"
        Me.WizardGroup.Name = "WizardGroup"
        '
        'ReportWizardButton
        '
        Me.ReportWizardButton.Label = "Report Wizard"
        Me.ReportWizardButton.Name = "ReportWizardButton"
        Me.ReportWizardButton.ShowImage = True
        '
        'EZLoggerRibbon
        '
        Me.Name = "EZLoggerRibbon"
        Me.RibbonType = "Microsoft.Word.Document"
        Me.Tabs.Add(Me.Tab1)
        Me.Tab1.ResumeLayout(False)
        Me.Tab1.PerformLayout()
        Me.WizardGroup.ResumeLayout(False)
        Me.WizardGroup.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Tab1 As Microsoft.Office.Tools.Ribbon.RibbonTab
    Friend WithEvents WizardGroup As Microsoft.Office.Tools.Ribbon.RibbonGroup
    Friend WithEvents ReportWizardButton As Microsoft.Office.Tools.Ribbon.RibbonButton
End Class

Partial Class ThisRibbonCollection

    <System.Diagnostics.DebuggerNonUserCode()> _
    Friend ReadOnly Property EZLoggerRibbon() As EZLoggerRibbon
        Get
            Return Me.GetRibbon(Of EZLoggerRibbon)()
        End Get
    End Property
End Class
