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
        Me.DatabaseGroup = Me.Factory.CreateRibbonGroup
        Me.ToolsGroup = Me.Factory.CreateRibbonGroup
        Me.SetupGroup = Me.Factory.CreateRibbonGroup
        Me.SettingsButton = Me.Factory.CreateRibbonButton
        Me.AboutButton = Me.Factory.CreateRibbonButton
        Me.CoverWizardButton = Me.Factory.CreateRibbonButton
        Me.SaveButton = Me.Factory.CreateRibbonButton
        Me.ConvertButton = Me.Factory.CreateRibbonButton
        Me.SyncButton = Me.Factory.CreateRibbonButton
        Me.TypoButton = Me.Factory.CreateRibbonButton
        Me.DatabaseMenu = Me.Factory.CreateRibbonMenu
        Me.HelpButton = Me.Factory.CreateRibbonButton
        Me.Tab1.SuspendLayout()
        Me.WizardGroup.SuspendLayout()
        Me.DatabaseGroup.SuspendLayout()
        Me.ToolsGroup.SuspendLayout()
        Me.SetupGroup.SuspendLayout()
        Me.SuspendLayout()
        '
        'Tab1
        '
        Me.Tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office
        Me.Tab1.Groups.Add(Me.WizardGroup)
        Me.Tab1.Groups.Add(Me.DatabaseGroup)
        Me.Tab1.Groups.Add(Me.ToolsGroup)
        Me.Tab1.Groups.Add(Me.SetupGroup)
        Me.Tab1.Label = "EZ Logger"
        Me.Tab1.Name = "Tab1"
        '
        'WizardGroup
        '
        Me.WizardGroup.Items.Add(Me.ReportWizardButton)
        Me.WizardGroup.Items.Add(Me.CoverWizardButton)
        Me.WizardGroup.Label = "Wizards"
        Me.WizardGroup.Name = "WizardGroup"
        '
        'ReportWizardButton
        '
        Me.ReportWizardButton.Image = Global.EZLogger.My.Resources.Resources.Wizard1
        Me.ReportWizardButton.Label = "Report Wizard"
        Me.ReportWizardButton.Name = "ReportWizardButton"
        Me.ReportWizardButton.ShowImage = True
        '
        'DatabaseGroup
        '
        Me.DatabaseGroup.Items.Add(Me.DatabaseMenu)
        Me.DatabaseGroup.Label = "Databases"
        Me.DatabaseGroup.Name = "DatabaseGroup"
        '
        'ToolsGroup
        '
        Me.ToolsGroup.Items.Add(Me.SaveButton)
        Me.ToolsGroup.Items.Add(Me.ConvertButton)
        Me.ToolsGroup.Items.Add(Me.TypoButton)
        Me.ToolsGroup.Label = "Tools"
        Me.ToolsGroup.Name = "ToolsGroup"
        '
        'SetupGroup
        '
        Me.SetupGroup.Items.Add(Me.HelpButton)
        Me.SetupGroup.Items.Add(Me.SettingsButton)
        Me.SetupGroup.Items.Add(Me.AboutButton)
        Me.SetupGroup.Items.Add(Me.SyncButton)
        Me.SetupGroup.Label = "Setup Commands"
        Me.SetupGroup.Name = "SetupGroup"
        '
        'SettingsButton
        '
        Me.SettingsButton.Label = "Settings"
        Me.SettingsButton.Name = "SettingsButton"
        Me.SettingsButton.ShowImage = True
        '
        'AboutButton
        '
        Me.AboutButton.Label = "About"
        Me.AboutButton.Name = "AboutButton"
        Me.AboutButton.ShowImage = True
        '
        'CoverWizardButton
        '
        Me.CoverWizardButton.Label = "Cover Letter Wizard"
        Me.CoverWizardButton.Name = "CoverWizardButton"
        Me.CoverWizardButton.ShowImage = True
        '
        'SaveButton
        '
        Me.SaveButton.Label = "Save Files"
        Me.SaveButton.Name = "SaveButton"
        Me.SaveButton.ShowImage = True
        '
        'ConvertButton
        '
        Me.ConvertButton.Label = "Convert To PDF"
        Me.ConvertButton.Name = "ConvertButton"
        Me.ConvertButton.ShowImage = True
        '
        'SyncButton
        '
        Me.SyncButton.Label = "Sync With SharePoint"
        Me.SyncButton.Name = "SyncButton"
        Me.SyncButton.ShowImage = True
        '
        'TypoButton
        '
        Me.TypoButton.Label = "Report a Typo"
        Me.TypoButton.Name = "TypoButton"
        Me.TypoButton.ShowImage = True
        '
        'DatabaseMenu
        '
        Me.DatabaseMenu.Dynamic = True
        Me.DatabaseMenu.Label = "Select a Database"
        Me.DatabaseMenu.Name = "DatabaseMenu"
        Me.DatabaseMenu.ShowImage = True
        '
        'HelpButton
        '
        Me.HelpButton.Label = "Help"
        Me.HelpButton.Name = "HelpButton"
        Me.HelpButton.ShowImage = True
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
        Me.DatabaseGroup.ResumeLayout(False)
        Me.DatabaseGroup.PerformLayout()
        Me.ToolsGroup.ResumeLayout(False)
        Me.ToolsGroup.PerformLayout()
        Me.SetupGroup.ResumeLayout(False)
        Me.SetupGroup.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Tab1 As Microsoft.Office.Tools.Ribbon.RibbonTab
    Friend WithEvents WizardGroup As Microsoft.Office.Tools.Ribbon.RibbonGroup
    Friend WithEvents ReportWizardButton As Microsoft.Office.Tools.Ribbon.RibbonButton
    Friend WithEvents DatabaseGroup As Microsoft.Office.Tools.Ribbon.RibbonGroup
    Friend WithEvents ToolsGroup As Microsoft.Office.Tools.Ribbon.RibbonGroup
    Friend WithEvents SetupGroup As Microsoft.Office.Tools.Ribbon.RibbonGroup
    Friend WithEvents SettingsButton As Microsoft.Office.Tools.Ribbon.RibbonButton
    Friend WithEvents CoverWizardButton As Microsoft.Office.Tools.Ribbon.RibbonButton
    Friend WithEvents SaveButton As Microsoft.Office.Tools.Ribbon.RibbonButton
    Friend WithEvents AboutButton As Microsoft.Office.Tools.Ribbon.RibbonButton
    Friend WithEvents ConvertButton As Microsoft.Office.Tools.Ribbon.RibbonButton
    Friend WithEvents SyncButton As Microsoft.Office.Tools.Ribbon.RibbonButton
    Friend WithEvents TypoButton As Microsoft.Office.Tools.Ribbon.RibbonButton
    Friend WithEvents DatabaseMenu As Microsoft.Office.Tools.Ribbon.RibbonMenu
    Friend WithEvents HelpButton As Microsoft.Office.Tools.Ribbon.RibbonButton
End Class

Partial Class ThisRibbonCollection

    <System.Diagnostics.DebuggerNonUserCode()> _
    Friend ReadOnly Property EZLoggerRibbon() As EZLoggerRibbon
        Get
            Return Me.GetRibbon(Of EZLoggerRibbon)()
        End Get
    End Property
End Class
