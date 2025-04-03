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
        Me.ReportWizardBox = Me.Factory.CreateRibbonBox
        Me.ReportWizardButton = Me.Factory.CreateRibbonButton
        Me.CoverLetterWizardBox = Me.Factory.CreateRibbonBox
        Me.CoverWizardButton = Me.Factory.CreateRibbonButton
        Me.DatabaseGroup = Me.Factory.CreateRibbonGroup
        Me.DbBtnBox = Me.Factory.CreateRibbonBox
        Me.DatabaseMenu = Me.Factory.CreateRibbonMenu
        Me.ToolsGroup = Me.Factory.CreateRibbonGroup
        Me.SaveBtnBox = Me.Factory.CreateRibbonBox
        Me.SaveButton = Me.Factory.CreateRibbonButton
        Me.PdfBtnBox = Me.Factory.CreateRibbonBox
        Me.ConvertButton = Me.Factory.CreateRibbonButton
        Me.TypoBtnBox = Me.Factory.CreateRibbonBox
        Me.TypoButton = Me.Factory.CreateRibbonButton
        Me.EmailButton = Me.Factory.CreateRibbonButton
        Me.SetupGroup = Me.Factory.CreateRibbonGroup
        Me.HelpButton = Me.Factory.CreateRibbonButton
        Me.SettingsButton = Me.Factory.CreateRibbonButton
        Me.AboutButton = Me.Factory.CreateRibbonButton
        Me.SyncButton = Me.Factory.CreateRibbonButton
        Me.Button1 = Me.Factory.CreateRibbonButton
        Me.Tab1.SuspendLayout()
        Me.WizardGroup.SuspendLayout()
        Me.ReportWizardBox.SuspendLayout()
        Me.CoverLetterWizardBox.SuspendLayout()
        Me.DatabaseGroup.SuspendLayout()
        Me.DbBtnBox.SuspendLayout()
        Me.ToolsGroup.SuspendLayout()
        Me.SaveBtnBox.SuspendLayout()
        Me.PdfBtnBox.SuspendLayout()
        Me.TypoBtnBox.SuspendLayout()
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
        Me.WizardGroup.Items.Add(Me.ReportWizardBox)
        Me.WizardGroup.Items.Add(Me.CoverLetterWizardBox)
        Me.WizardGroup.Label = "Wizards"
        Me.WizardGroup.Name = "WizardGroup"
        '
        'ReportWizardBox
        '
        Me.ReportWizardBox.BoxStyle = Microsoft.Office.Tools.Ribbon.RibbonBoxStyle.Vertical
        Me.ReportWizardBox.Items.Add(Me.ReportWizardButton)
        Me.ReportWizardBox.Name = "ReportWizardBox"
        '
        'ReportWizardButton
        '
        Me.ReportWizardButton.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge
        Me.ReportWizardButton.Image = Global.EZLogger.My.Resources.Resources.Wizard1
        Me.ReportWizardButton.Label = "Report Wizard"
        Me.ReportWizardButton.Name = "ReportWizardButton"
        Me.ReportWizardButton.ShowImage = True
        '
        'CoverLetterWizardBox
        '
        Me.CoverLetterWizardBox.BoxStyle = Microsoft.Office.Tools.Ribbon.RibbonBoxStyle.Vertical
        Me.CoverLetterWizardBox.Items.Add(Me.CoverWizardButton)
        Me.CoverLetterWizardBox.Name = "CoverLetterWizardBox"
        '
        'CoverWizardButton
        '
        Me.CoverWizardButton.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge
        Me.CoverWizardButton.Image = Global.EZLogger.My.Resources.Resources.Wizard2
        Me.CoverWizardButton.Label = "Cover Page Wizard"
        Me.CoverWizardButton.Name = "CoverWizardButton"
        Me.CoverWizardButton.ShowImage = True
        '
        'DatabaseGroup
        '
        Me.DatabaseGroup.Items.Add(Me.DbBtnBox)
        Me.DatabaseGroup.Label = "Databases"
        Me.DatabaseGroup.Name = "DatabaseGroup"
        '
        'DbBtnBox
        '
        Me.DbBtnBox.Items.Add(Me.DatabaseMenu)
        Me.DbBtnBox.Name = "DbBtnBox"
        '
        'DatabaseMenu
        '
        Me.DatabaseMenu.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge
        Me.DatabaseMenu.Dynamic = True
        Me.DatabaseMenu.Image = Global.EZLogger.My.Resources.Resources.database
        Me.DatabaseMenu.Label = "Select a Database"
        Me.DatabaseMenu.Name = "DatabaseMenu"
        Me.DatabaseMenu.ShowImage = True
        '
        'ToolsGroup
        '
        Me.ToolsGroup.Items.Add(Me.SaveBtnBox)
        Me.ToolsGroup.Items.Add(Me.Button1)
        Me.ToolsGroup.Items.Add(Me.PdfBtnBox)
        Me.ToolsGroup.Items.Add(Me.TypoBtnBox)
        Me.ToolsGroup.Items.Add(Me.EmailButton)
        Me.ToolsGroup.Label = "Tools"
        Me.ToolsGroup.Name = "ToolsGroup"
        '
        'SaveBtnBox
        '
        Me.SaveBtnBox.Items.Add(Me.SaveButton)
        Me.SaveBtnBox.Name = "SaveBtnBox"
        '
        'SaveButton
        '
        Me.SaveButton.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge
        Me.SaveButton.Image = Global.EZLogger.My.Resources.Resources.floppy
        Me.SaveButton.Label = "Save Files"
        Me.SaveButton.Name = "SaveButton"
        Me.SaveButton.ShowImage = True
        '
        'PdfBtnBox
        '
        Me.PdfBtnBox.Items.Add(Me.ConvertButton)
        Me.PdfBtnBox.Name = "PdfBtnBox"
        '
        'ConvertButton
        '
        Me.ConvertButton.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge
        Me.ConvertButton.Image = Global.EZLogger.My.Resources.Resources.pdf1
        Me.ConvertButton.Label = "Convert To PDF"
        Me.ConvertButton.Name = "ConvertButton"
        Me.ConvertButton.ShowImage = True
        '
        'TypoBtnBox
        '
        Me.TypoBtnBox.Items.Add(Me.TypoButton)
        Me.TypoBtnBox.Name = "TypoBtnBox"
        '
        'TypoButton
        '
        Me.TypoButton.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge
        Me.TypoButton.Image = Global.EZLogger.My.Resources.Resources.typo
        Me.TypoButton.Label = "Report a Typo"
        Me.TypoButton.Name = "TypoButton"
        Me.TypoButton.ShowImage = True
        '
        'EmailButton
        '
        Me.EmailButton.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge
        Me.EmailButton.Image = Global.EZLogger.My.Resources.Resources.email
        Me.EmailButton.Label = "Email Report"
        Me.EmailButton.Name = "EmailButton"
        Me.EmailButton.ShowImage = True
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
        'HelpButton
        '
        Me.HelpButton.Image = Global.EZLogger.My.Resources.Resources.help
        Me.HelpButton.Label = "Help"
        Me.HelpButton.Name = "HelpButton"
        Me.HelpButton.ShowImage = True
        '
        'SettingsButton
        '
        Me.SettingsButton.Image = Global.EZLogger.My.Resources.Resources.cog
        Me.SettingsButton.Label = "Settings"
        Me.SettingsButton.Name = "SettingsButton"
        Me.SettingsButton.ShowImage = True
        '
        'AboutButton
        '
        Me.AboutButton.Image = Global.EZLogger.My.Resources.Resources.about
        Me.AboutButton.Label = "About"
        Me.AboutButton.Name = "AboutButton"
        Me.AboutButton.ShowImage = True
        '
        'SyncButton
        '
        Me.SyncButton.Image = Global.EZLogger.My.Resources.Resources.sharepoint
        Me.SyncButton.Label = "Sync With SharePoint"
        Me.SyncButton.Name = "SyncButton"
        Me.SyncButton.ShowImage = True
        '
        'Button1
        '
        Me.Button1.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge
        Me.Button1.Image = Global.EZLogger.My.Resources.Resources.about
        Me.Button1.Label = "Patient Info"
        Me.Button1.Name = "Button1"
        Me.Button1.ShowImage = True
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
        Me.ReportWizardBox.ResumeLayout(False)
        Me.ReportWizardBox.PerformLayout()
        Me.CoverLetterWizardBox.ResumeLayout(False)
        Me.CoverLetterWizardBox.PerformLayout()
        Me.DatabaseGroup.ResumeLayout(False)
        Me.DatabaseGroup.PerformLayout()
        Me.DbBtnBox.ResumeLayout(False)
        Me.DbBtnBox.PerformLayout()
        Me.ToolsGroup.ResumeLayout(False)
        Me.ToolsGroup.PerformLayout()
        Me.SaveBtnBox.ResumeLayout(False)
        Me.SaveBtnBox.PerformLayout()
        Me.PdfBtnBox.ResumeLayout(False)
        Me.PdfBtnBox.PerformLayout()
        Me.TypoBtnBox.ResumeLayout(False)
        Me.TypoBtnBox.PerformLayout()
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
    Friend WithEvents ReportWizardBox As Microsoft.Office.Tools.Ribbon.RibbonBox
    Friend WithEvents CoverLetterWizardBox As Microsoft.Office.Tools.Ribbon.RibbonBox
    Friend WithEvents DbBtnBox As Microsoft.Office.Tools.Ribbon.RibbonBox
    Friend WithEvents SaveBtnBox As Microsoft.Office.Tools.Ribbon.RibbonBox
    Friend WithEvents PdfBtnBox As Microsoft.Office.Tools.Ribbon.RibbonBox
    Friend WithEvents TypoBtnBox As Microsoft.Office.Tools.Ribbon.RibbonBox
    Friend WithEvents EmailButton As Microsoft.Office.Tools.Ribbon.RibbonButton
    Friend WithEvents Button1 As Microsoft.Office.Tools.Ribbon.RibbonButton
End Class

Partial Class ThisRibbonCollection

    <System.Diagnostics.DebuggerNonUserCode()> _
    Friend ReadOnly Property EZLoggerRibbon() As EZLoggerRibbon
        Get
            Return Me.GetRibbon(Of EZLoggerRibbon)()
        End Get
    End Property
End Class
