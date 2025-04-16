Imports System.Windows
Imports System.Windows.Forms
Imports MessageBox = System.Windows.MessageBox
Imports EZLogger.Helpers

Namespace Handlers
    Public Class ConfigViewHandler
        Public Sub HandleCreateConfigClick()
            ' Step 1: Ensure local_user_config.json exists in %USERPROFILE%\.ezlogger
            Dim localConfigPath As String = ConfigHelper.EnsureLocalUserConfigFileExists()
            If String.IsNullOrEmpty(localConfigPath) Then
                MessageBox.Show("Failed to create or locate local config file.", "Setup Failed")
                Return
            End If

            ' Step 2: Prompt the user to select their global_config.json from the EZLogger_Databases SharePoint folder
            Dim globalConfigPath As String = ConfigHelper.PromptForGlobalConfigFile()
            If String.IsNullOrEmpty(globalConfigPath) Then
                MessageBox.Show("Global config selection was cancelled or invalid.", "Setup Incomplete")
                Return
            End If

            ' Step 3: Write the global config path into local_user_config.json under sp_filepath.global_config_file
            ConfigHelper.UpdateLocalConfigWithGlobalPath(globalConfigPath)

            ' Step 4: Notify the user of success
            MessageBox.Show("Configuration setup complete!" & Environment.NewLine &
                    "Local config stored at:" & Environment.NewLine & localConfigPath,
                    "EZLogger Setup Complete")

            ' Optional: Update visible labels in the form
            ' (Assumes you're calling this from a host form that can access the text blocks)
            ' configView.txtblock_local_config.Text = localConfigPath
            ' configView.txtblock_global_config.Text = globalConfigPath
        End Sub

        Public Sub HandleSaveConfigClick()
            MsgBox("You clicked Save Config")
        End Sub

        Public Sub AddAlertButtonClick()
            MsgBox("You clicked Add Alert button")
        End Sub

        Public Sub EditAlertButtonClick()
            MsgBox("You clicked Edit Alert button")
        End Sub

        Public Sub DeleteAlertButtonClick()
            MsgBox("You clicked Delete Alert button")
        End Sub

        Public Sub AddCountyAlertButtonClick()
            MsgBox("You clicked Add County Alert button")
        End Sub

        Public Sub EditCountyAlertButtonClick()
            MsgBox("You clicked Edit County Alert button")
        End Sub

        Public Sub DeleteCountyAlertButtonClick()
            MsgBox("You clicked Delete County Alert button")
        End Sub

        Public Sub HandleEditEmailClick(r1 As System.Windows.Controls.RadioButton, r2 As System.Windows.Controls.RadioButton, r3 As System.Windows.Controls.RadioButton)
            If r1.IsChecked = True Then
                MsgBox("Secretaries radio is selected")
            ElseIf r2.IsChecked = True Then
                MsgBox("Friday radio is selected")
            ElseIf r3.IsChecked = True Then
                MsgBox("Competent radio is selected")
            Else
                MsgBox("No option is selected")
            End If
        End Sub

    End Class
End Namespace
