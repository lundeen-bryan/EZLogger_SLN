﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CoverPageWizardPaneContainer
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.ElementHost1 = New System.Windows.Forms.Integration.ElementHost()
        Me.CoverPageWizardPane1 = New EZLogger.CoverPageWizardPane()
        Me.LabelCoverPage = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'ElementHost1
        '
        Me.ElementHost1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ElementHost1.Location = New System.Drawing.Point(0, 0)
        Me.ElementHost1.Name = "ElementHost1"
        Me.ElementHost1.Size = New System.Drawing.Size(150, 150)
        Me.ElementHost1.TabIndex = 0
        Me.ElementHost1.Text = "ElementHost1"
        Me.ElementHost1.Child = Me.CoverPageWizardPane1
        '
        'LabelCoverPage
        '
        Me.LabelCoverPage.AutoSize = True
        Me.LabelCoverPage.Location = New System.Drawing.Point(13, 53)
        Me.LabelCoverPage.Name = "LabelCoverPage"
        Me.LabelCoverPage.Size = New System.Drawing.Size(125, 13)
        Me.LabelCoverPage.TabIndex = 1
        Me.LabelCoverPage.Text = "Cover Page Wizard Here"
        '
        'CoverPageWizardPaneContainer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.LabelCoverPage)
        Me.Controls.Add(Me.ElementHost1)
        Me.Name = "CoverPageWizardPaneContainer"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ElementHost1 As Windows.Forms.Integration.ElementHost
    Friend WithEvents LabelCoverPage As Windows.Forms.Label
    Friend CoverPageWizardPane1 As CoverPageWizardPane
End Class
