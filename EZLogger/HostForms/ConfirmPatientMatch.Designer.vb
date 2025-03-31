<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ConfirmPatientMatch
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Me.GroupBoxConfirm = New System.Windows.Forms.GroupBox()
        Me.ButtonViewDatabase = New System.Windows.Forms.Button()
        Me.LabelFindMatch = New System.Windows.Forms.Label()
        Me.ButtonYes = New System.Windows.Forms.Button()
        Me.ButtonNo = New System.Windows.Forms.Button()
        Me.GroupBoxConfirm.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBoxConfirm
        '
        Me.GroupBoxConfirm.Controls.Add(Me.ButtonViewDatabase)
        Me.GroupBoxConfirm.Font = New System.Drawing.Font("Lucida Fax", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBoxConfirm.Location = New System.Drawing.Point(12, 12)
        Me.GroupBoxConfirm.Name = "GroupBoxConfirm"
        Me.GroupBoxConfirm.Size = New System.Drawing.Size(276, 97)
        Me.GroupBoxConfirm.TabIndex = 0
        Me.GroupBoxConfirm.TabStop = False
        Me.GroupBoxConfirm.Text = "Confirm Patient Match"
        '
        'ButtonViewDatabase
        '
        Me.ButtonViewDatabase.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonViewDatabase.Location = New System.Drawing.Point(59, 35)
        Me.ButtonViewDatabase.Name = "ButtonViewDatabase"
        Me.ButtonViewDatabase.Size = New System.Drawing.Size(143, 34)
        Me.ButtonViewDatabase.TabIndex = 0
        Me.ButtonViewDatabase.Text = "View Database"
        Me.ButtonViewDatabase.UseVisualStyleBackColor = True
        '
        'LabelFindMatch
        '
        Me.LabelFindMatch.AutoSize = True
        Me.LabelFindMatch.Font = New System.Drawing.Font("Lucida Fax", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelFindMatch.Location = New System.Drawing.Point(9, 121)
        Me.LabelFindMatch.Name = "LabelFindMatch"
        Me.LabelFindMatch.Size = New System.Drawing.Size(292, 18)
        Me.LabelFindMatch.TabIndex = 1
        Me.LabelFindMatch.Text = "Does everything look like a match?"
        '
        'ButtonYes
        '
        Me.ButtonYes.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonYes.Font = New System.Drawing.Font("Lucida Fax", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonYes.Location = New System.Drawing.Point(28, 158)
        Me.ButtonYes.Name = "ButtonYes"
        Me.ButtonYes.Size = New System.Drawing.Size(96, 32)
        Me.ButtonYes.TabIndex = 2
        Me.ButtonYes.Text = "&Yes"
        Me.ButtonYes.UseVisualStyleBackColor = True
        '
        'ButtonNo
        '
        Me.ButtonNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonNo.Font = New System.Drawing.Font("Lucida Fax", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonNo.Location = New System.Drawing.Point(172, 158)
        Me.ButtonNo.Name = "ButtonNo"
        Me.ButtonNo.Size = New System.Drawing.Size(96, 32)
        Me.ButtonNo.TabIndex = 3
        Me.ButtonNo.Text = "&No"
        Me.ButtonNo.UseVisualStyleBackColor = True
        '
        'ConfirmPatientMatch
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(307, 217)
        Me.Controls.Add(Me.ButtonNo)
        Me.Controls.Add(Me.ButtonYes)
        Me.Controls.Add(Me.LabelFindMatch)
        Me.Controls.Add(Me.GroupBoxConfirm)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ConfirmPatientMatch"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.GroupBoxConfirm.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents GroupBoxConfirm As Windows.Forms.GroupBox
    Friend WithEvents ButtonViewDatabase As Windows.Forms.Button
    Friend WithEvents LabelFindMatch As Windows.Forms.Label
    Friend WithEvents ButtonYes As Windows.Forms.Button
    Friend WithEvents ButtonNo As Windows.Forms.Button
End Class
