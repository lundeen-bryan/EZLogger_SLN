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
        Me.ToolsGroup = Me.Factory.CreateRibbonGroup
        Me.DeleteDocPropsBtn = Me.Factory.CreateRibbonButton
        Me.Button1 = Me.Factory.CreateRibbonButton
        Me.PdfBtnBox = Me.Factory.CreateRibbonBox
        Me.ConvertButton = Me.Factory.CreateRibbonButton
        Me.TypoBtnBox = Me.Factory.CreateRibbonBox
        Me.TypoButton = Me.Factory.CreateRibbonButton
        Me.EmailButton = Me.Factory.CreateRibbonButton
        Me.BtnCloseDoc = Me.Factory.CreateRibbonButton
        Me.SetupGroup = Me.Factory.CreateRibbonGroup
        Me.HelpButton = Me.Factory.CreateRibbonButton
        Me.SettingsButton = Me.Factory.CreateRibbonButton
        Me.AboutButton = Me.Factory.CreateRibbonButton
        Me.SyncButton = Me.Factory.CreateRibbonButton
        Me.Group1 = Me.Factory.CreateRibbonGroup
        Me.Button2 = Me.Factory.CreateRibbonButton
        Me.Button3 = Me.Factory.CreateRibbonButton
        Me.RandomPatientNumberButton = Me.Factory.CreateRibbonButton
        Me.BtnTestFolder = Me.Factory.CreateRibbonButton
        Me.LookupHlvBtn = Me.Factory.CreateRibbonButton
        Me.EZLoggerMenu = Me.Factory.CreateRibbonMenu
        Me.Tab1.SuspendLayout()
        Me.WizardGroup.SuspendLayout()
        Me.ReportWizardBox.SuspendLayout()
        Me.ToolsGroup.SuspendLayout()
        Me.PdfBtnBox.SuspendLayout()
        Me.TypoBtnBox.SuspendLayout()
        Me.SetupGroup.SuspendLayout()
        Me.Group1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Tab1
        '
        Me.Tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office
        Me.Tab1.Groups.Add(Me.SetupGroup)
        Me.Tab1.Groups.Add(Me.WizardGroup)
        Me.Tab1.Groups.Add(Me.ToolsGroup)
        Me.Tab1.Groups.Add(Me.Group1)
        Me.Tab1.Label = "EZ Logger"
        Me.Tab1.Name = "Tab1"
        '
        'WizardGroup
        '
        Me.WizardGroup.Items.Add(Me.ReportWizardBox)
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
        Me.ReportWizardButton.Image = Global.EZLogger.My.Resources.Resources.mouse_icon
        Me.ReportWizardButton.Label = "Process Report"
        Me.ReportWizardButton.Name = "ReportWizardButton"
        Me.ReportWizardButton.ShowImage = True
        '
        'ToolsGroup
        '
        Me.ToolsGroup.Items.Add(Me.DeleteDocPropsBtn)
        Me.ToolsGroup.Items.Add(Me.Button1)
        Me.ToolsGroup.Items.Add(Me.PdfBtnBox)
        Me.ToolsGroup.Items.Add(Me.TypoBtnBox)
        Me.ToolsGroup.Items.Add(Me.EmailButton)
        Me.ToolsGroup.Items.Add(Me.BtnCloseDoc)
        Me.ToolsGroup.Label = "Tools"
        Me.ToolsGroup.Name = "ToolsGroup"
        '
        'DeleteDocPropsBtn
        '
        Me.DeleteDocPropsBtn.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge
        Me.DeleteDocPropsBtn.Image = Global.EZLogger.My.Resources.Resources.trash_icon
        Me.DeleteDocPropsBtn.Label = "Delete Properties"
        Me.DeleteDocPropsBtn.Name = "DeleteDocPropsBtn"
        Me.DeleteDocPropsBtn.ShowImage = True
        '
        'Button1
        '
        Me.Button1.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge
        Me.Button1.Image = Global.EZLogger.My.Resources.Resources.about
        Me.Button1.Label = "Patient Info"
        Me.Button1.Name = "Button1"
        Me.Button1.ShowImage = True
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
        'BtnCloseDoc
        '
        Me.BtnCloseDoc.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge
        Me.BtnCloseDoc.Image = Global.EZLogger.My.Resources.Resources.folder
        Me.BtnCloseDoc.Label = "Close Document"
        Me.BtnCloseDoc.Name = "BtnCloseDoc"
        Me.BtnCloseDoc.ShowImage = True
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
        'Group1
        '
        Me.Group1.Items.Add(Me.Button2)
        Me.Group1.Items.Add(Me.Button3)
        Me.Group1.Items.Add(Me.RandomPatientNumberButton)
        Me.Group1.Items.Add(Me.BtnTestFolder)
        Me.Group1.Items.Add(Me.LookupHlvBtn)
        Me.Group1.Label = "Test Group"
        Me.Group1.Name = "Group1"
        '
        'Button2
        '
        Me.Button2.Label = "Test Button"
        Me.Button2.Name = "Button2"
        '
        'Button3
        '
        Me.Button3.Label = "TestConfig"
        Me.Button3.Name = "Button3"
        '
        'RandomPatientNumberButton
        '
        Me.RandomPatientNumberButton.Label = "Random Patient"
        Me.RandomPatientNumberButton.Name = "RandomPatientNumberButton"
        '
        'BtnTestFolder
        '
        Me.BtnTestFolder.Label = "Test Folder Picker"
        Me.BtnTestFolder.Name = "BtnTestFolder"
        '
        'LookupHlvBtn
        '
        Me.LookupHlvBtn.Label = "LookupHLV"
        Me.LookupHlvBtn.Name = "LookupHlvBtn"
        '
        'EZLoggerMenu
        '
        Me.EZLoggerMenu.Label = "EZLogger"
        Me.EZLoggerMenu.Name = "EZLoggerMenu"
        Me.EZLoggerMenu.ShowImage = True
        '
        'EZLoggerRibbon
        '
        Me.Name = "EZLoggerRibbon"
        '
        'EZLoggerRibbon.OfficeMenu
        '
        Me.OfficeMenu.Items.Add(Me.EZLoggerMenu)
        Me.RibbonType = "Microsoft.Word.Document"
        Me.Tabs.Add(Me.Tab1)
        Me.Tab1.ResumeLayout(False)
        Me.Tab1.PerformLayout()
        Me.WizardGroup.ResumeLayout(False)
        Me.WizardGroup.PerformLayout()
        Me.ReportWizardBox.ResumeLayout(False)
        Me.ReportWizardBox.PerformLayout()
        Me.ToolsGroup.ResumeLayout(False)
        Me.ToolsGroup.PerformLayout()
        Me.PdfBtnBox.ResumeLayout(False)
        Me.PdfBtnBox.PerformLayout()
        Me.TypoBtnBox.ResumeLayout(False)
        Me.TypoBtnBox.PerformLayout()
        Me.SetupGroup.ResumeLayout(False)
        Me.SetupGroup.PerformLayout()
        Me.Group1.ResumeLayout(False)
        Me.Group1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Tab1 As Microsoft.Office.Tools.Ribbon.RibbonTab
    Friend WithEvents WizardGroup As Microsoft.Office.Tools.Ribbon.RibbonGroup
    Friend WithEvents ReportWizardButton As Microsoft.Office.Tools.Ribbon.RibbonButton
    Friend WithEvents ToolsGroup As Microsoft.Office.Tools.Ribbon.RibbonGroup
    Friend WithEvents SetupGroup As Microsoft.Office.Tools.Ribbon.RibbonGroup
    Friend WithEvents SettingsButton As Microsoft.Office.Tools.Ribbon.RibbonButton
    Friend WithEvents AboutButton As Microsoft.Office.Tools.Ribbon.RibbonButton
    Friend WithEvents ConvertButton As Microsoft.Office.Tools.Ribbon.RibbonButton
    Friend WithEvents SyncButton As Microsoft.Office.Tools.Ribbon.RibbonButton
    Friend WithEvents TypoButton As Microsoft.Office.Tools.Ribbon.RibbonButton
    Friend WithEvents HelpButton As Microsoft.Office.Tools.Ribbon.RibbonButton
    Friend WithEvents ReportWizardBox As Microsoft.Office.Tools.Ribbon.RibbonBox
    Friend WithEvents PdfBtnBox As Microsoft.Office.Tools.Ribbon.RibbonBox
    Friend WithEvents TypoBtnBox As Microsoft.Office.Tools.Ribbon.RibbonBox
    Friend WithEvents EmailButton As Microsoft.Office.Tools.Ribbon.RibbonButton
    Friend WithEvents Button1 As Microsoft.Office.Tools.Ribbon.RibbonButton
    Friend WithEvents Button2 As Microsoft.Office.Tools.Ribbon.RibbonButton
    Friend WithEvents BtnCloseDoc As Microsoft.Office.Tools.Ribbon.RibbonButton
    Friend WithEvents Button3 As Microsoft.Office.Tools.Ribbon.RibbonButton
    Friend WithEvents Group1 As Microsoft.Office.Tools.Ribbon.RibbonGroup
    Friend WithEvents RandomPatientNumberButton As Microsoft.Office.Tools.Ribbon.RibbonButton
    Friend WithEvents DeleteDocPropsBtn As Microsoft.Office.Tools.Ribbon.RibbonButton
    Friend WithEvents BtnTestFolder As Microsoft.Office.Tools.Ribbon.RibbonButton
    Friend WithEvents LookupHlvBtn As Microsoft.Office.Tools.Ribbon.RibbonButton
    Friend WithEvents EZLoggerMenu As Microsoft.Office.Tools.Ribbon.RibbonMenu
End Class

Partial Class ThisRibbonCollection

    <System.Diagnostics.DebuggerNonUserCode()> _
    Friend ReadOnly Property EZLoggerRibbon() As EZLoggerRibbon
        Get
            Return Me.GetRibbon(Of EZLoggerRibbon)()
        End Get
    End Property
End Class
