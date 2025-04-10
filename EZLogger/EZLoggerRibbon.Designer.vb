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
        Me.WizardGrp = Me.Factory.CreateRibbonGroup
        Me.DatabasesGrp = Me.Factory.CreateRibbonGroup
        Me.ToolsGrp = Me.Factory.CreateRibbonGroup
        Me.SetupGrp = Me.Factory.CreateRibbonGroup
        Me.TestGrp = Me.Factory.CreateRibbonGroup
        Me.ReportWizardBtn = Me.Factory.CreateRibbonButton
        Me.DatabaseMnu = Me.Factory.CreateRibbonMenu
        Me.item1 = Me.Factory.CreateRibbonButton
        Me.PatientInfoBtn = Me.Factory.CreateRibbonButton
        Me.SettingsBtn = Me.Factory.CreateRibbonButton
        Me.AboutBtn = Me.Factory.CreateRibbonButton
        Me.MsgBoxBtn = Me.Factory.CreateRibbonButton
        Me.Tab1.SuspendLayout()
        Me.WizardGrp.SuspendLayout()
        Me.DatabasesGrp.SuspendLayout()
        Me.ToolsGrp.SuspendLayout()
        Me.SetupGrp.SuspendLayout()
        Me.TestGrp.SuspendLayout()
        Me.SuspendLayout()
        '
        'Tab1
        '
        Me.Tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office
        Me.Tab1.Groups.Add(Me.WizardGrp)
        Me.Tab1.Groups.Add(Me.DatabasesGrp)
        Me.Tab1.Groups.Add(Me.ToolsGrp)
        Me.Tab1.Groups.Add(Me.SetupGrp)
        Me.Tab1.Groups.Add(Me.TestGrp)
        Me.Tab1.Label = "EZ Logger"
        Me.Tab1.Name = "Tab1"
        '
        'WizardGrp
        '
        Me.WizardGrp.Items.Add(Me.ReportWizardBtn)
        Me.WizardGrp.Label = "Wizards"
        Me.WizardGrp.Name = "WizardGrp"
        '
        'DatabasesGrp
        '
        Me.DatabasesGrp.Items.Add(Me.DatabaseMnu)
        Me.DatabasesGrp.Label = "Databases"
        Me.DatabasesGrp.Name = "DatabasesGrp"
        '
        'ToolsGrp
        '
        Me.ToolsGrp.Items.Add(Me.PatientInfoBtn)
        Me.ToolsGrp.Label = "Tools"
        Me.ToolsGrp.Name = "ToolsGrp"
        '
        'SetupGrp
        '
        Me.SetupGrp.Items.Add(Me.SettingsBtn)
        Me.SetupGrp.Items.Add(Me.AboutBtn)
        Me.SetupGrp.Label = "Setup Tools"
        Me.SetupGrp.Name = "SetupGrp"
        '
        'TestGrp
        '
        Me.TestGrp.Items.Add(Me.MsgBoxBtn)
        Me.TestGrp.Label = "Testing"
        Me.TestGrp.Name = "TestGrp"
        '
        'ReportWizardBtn
        '
        Me.ReportWizardBtn.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge
        Me.ReportWizardBtn.Image = Global.EZLogger.My.Resources.Resources.Wizard1
        Me.ReportWizardBtn.Label = "Report Wizard"
        Me.ReportWizardBtn.Name = "ReportWizardBtn"
        Me.ReportWizardBtn.ShowImage = True
        '
        'DatabaseMnu
        '
        Me.DatabaseMnu.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge
        Me.DatabaseMnu.Image = Global.EZLogger.My.Resources.Resources.database
        Me.DatabaseMnu.Items.Add(Me.item1)
        Me.DatabaseMnu.Label = "Dababases"
        Me.DatabaseMnu.Name = "DatabaseMnu"
        Me.DatabaseMnu.ShowImage = True
        '
        'item1
        '
        Me.item1.Label = "Button2"
        Me.item1.Name = "item1"
        Me.item1.ShowImage = True
        '
        'PatientInfoBtn
        '
        Me.PatientInfoBtn.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge
        Me.PatientInfoBtn.Label = "Patient Info"
        Me.PatientInfoBtn.Name = "PatientInfoBtn"
        Me.PatientInfoBtn.ShowImage = True
        '
        'SettingsBtn
        '
        Me.SettingsBtn.Image = Global.EZLogger.My.Resources.Resources.cog
        Me.SettingsBtn.Label = "Settings"
        Me.SettingsBtn.Name = "SettingsBtn"
        Me.SettingsBtn.ShowImage = True
        '
        'AboutBtn
        '
        Me.AboutBtn.Image = Global.EZLogger.My.Resources.Resources.about
        Me.AboutBtn.Label = "Aboout"
        Me.AboutBtn.Name = "AboutBtn"
        Me.AboutBtn.ShowImage = True
        '
        'MsgBoxBtn
        '
        Me.MsgBoxBtn.Label = "MsgBox"
        Me.MsgBoxBtn.Name = "MsgBoxBtn"
        '
        'EZLoggerRibbon
        '
        Me.Name = "EZLoggerRibbon"
        Me.RibbonType = "Microsoft.Word.Document"
        Me.Tabs.Add(Me.Tab1)
        Me.Tab1.ResumeLayout(False)
        Me.Tab1.PerformLayout()
        Me.WizardGrp.ResumeLayout(False)
        Me.WizardGrp.PerformLayout()
        Me.DatabasesGrp.ResumeLayout(False)
        Me.DatabasesGrp.PerformLayout()
        Me.ToolsGrp.ResumeLayout(False)
        Me.ToolsGrp.PerformLayout()
        Me.SetupGrp.ResumeLayout(False)
        Me.SetupGrp.PerformLayout()
        Me.TestGrp.ResumeLayout(False)
        Me.TestGrp.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Tab1 As Microsoft.Office.Tools.Ribbon.RibbonTab
    Friend WithEvents WizardGrp As Microsoft.Office.Tools.Ribbon.RibbonGroup
    Friend WithEvents ReportWizardBtn As Microsoft.Office.Tools.Ribbon.RibbonButton
    Friend WithEvents DatabasesGrp As Microsoft.Office.Tools.Ribbon.RibbonGroup
    Friend WithEvents DatabaseMnu As Microsoft.Office.Tools.Ribbon.RibbonMenu
    Friend WithEvents ToolsGrp As Microsoft.Office.Tools.Ribbon.RibbonGroup
    Friend WithEvents PatientInfoBtn As Microsoft.Office.Tools.Ribbon.RibbonButton
    Friend WithEvents SetupGrp As Microsoft.Office.Tools.Ribbon.RibbonGroup
    Friend WithEvents SettingsBtn As Microsoft.Office.Tools.Ribbon.RibbonButton
    Friend WithEvents AboutBtn As Microsoft.Office.Tools.Ribbon.RibbonButton
    Friend WithEvents TestGrp As Microsoft.Office.Tools.Ribbon.RibbonGroup
    Friend WithEvents MsgBoxBtn As Microsoft.Office.Tools.Ribbon.RibbonButton
    Friend WithEvents item1 As Microsoft.Office.Tools.Ribbon.RibbonButton
End Class

Partial Class ThisRibbonCollection

    <System.Diagnostics.DebuggerNonUserCode()> _
    Friend ReadOnly Property Ribbon1() As EZLoggerRibbon
        Get
            Return Me.GetRibbon(Of EZLoggerRibbon)()
        End Get
    End Property
End Class
