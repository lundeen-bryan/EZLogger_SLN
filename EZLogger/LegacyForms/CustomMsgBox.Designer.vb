<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CustomMsgBox
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
        Me.TextBoxMessageToUser = New System.Windows.Forms.TextBox()
        Me.ButtonYes = New System.Windows.Forms.Button()
        Me.ButtonNo = New System.Windows.Forms.Button()
        Me.ButtonOk = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'TextBoxMessageToUser
        '
        Me.TextBoxMessageToUser.AcceptsReturn = True
        Me.TextBoxMessageToUser.AcceptsTab = True
        Me.TextBoxMessageToUser.BackColor = System.Drawing.Color.Black
        Me.TextBoxMessageToUser.Font = New System.Drawing.Font("Lucida Console", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxMessageToUser.ForeColor = System.Drawing.Color.Lime
        Me.TextBoxMessageToUser.Location = New System.Drawing.Point(13, 13)
        Me.TextBoxMessageToUser.Margin = New System.Windows.Forms.Padding(6)
        Me.TextBoxMessageToUser.Multiline = True
        Me.TextBoxMessageToUser.Name = "TextBoxMessageToUser"
        Me.TextBoxMessageToUser.Size = New System.Drawing.Size(353, 154)
        Me.TextBoxMessageToUser.TabIndex = 0
        Me.TextBoxMessageToUser.Text = "messageToUser"
        Me.TextBoxMessageToUser.UseWaitCursor = True
        '
        'ButtonYes
        '
        Me.ButtonYes.Font = New System.Drawing.Font("Lucida Fax", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonYes.Location = New System.Drawing.Point(13, 174)
        Me.ButtonYes.Name = "ButtonYes"
        Me.ButtonYes.Size = New System.Drawing.Size(114, 35)
        Me.ButtonYes.TabIndex = 1
        Me.ButtonYes.Text = "&Yes"
        Me.ButtonYes.UseVisualStyleBackColor = True
        '
        'ButtonNo
        '
        Me.ButtonNo.Font = New System.Drawing.Font("Lucida Fax", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonNo.Location = New System.Drawing.Point(133, 173)
        Me.ButtonNo.Name = "ButtonNo"
        Me.ButtonNo.Size = New System.Drawing.Size(114, 35)
        Me.ButtonNo.TabIndex = 2
        Me.ButtonNo.Text = "&No"
        Me.ButtonNo.UseVisualStyleBackColor = True
        '
        'ButtonOk
        '
        Me.ButtonOk.Font = New System.Drawing.Font("Lucida Fax", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonOk.Location = New System.Drawing.Point(252, 173)
        Me.ButtonOk.Name = "ButtonOk"
        Me.ButtonOk.Size = New System.Drawing.Size(114, 35)
        Me.ButtonOk.TabIndex = 3
        Me.ButtonOk.Text = "&Ok"
        Me.ButtonOk.UseVisualStyleBackColor = True
        '
        'CustomMsgBox
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(378, 221)
        Me.Controls.Add(Me.ButtonOk)
        Me.Controls.Add(Me.ButtonNo)
        Me.Controls.Add(Me.ButtonYes)
        Me.Controls.Add(Me.TextBoxMessageToUser)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "CustomMsgBox"
        Me.ShowIcon = False
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents TextBoxMessageToUser As Windows.Forms.TextBox
    Friend WithEvents ButtonYes As Windows.Forms.Button
    Friend WithEvents ButtonNo As Windows.Forms.Button
    Friend WithEvents ButtonOk As Windows.Forms.Button
End Class
