Imports System.Windows
Imports System.Windows.Forms
Imports EZLogger.Models
Imports MessageBox = System.Windows.MessageBox
Imports EZLogger.Helpers
Imports System.IO
Imports System.Text.Json

Namespace Handlers
    Public Class ConfigViewHandler
        Public Sub HandleSetupFolderPathsClick()
            ' Step 1: Prompt user for folders
            Dim dbPath As String = ConfigHelper.PromptForFolder("Select the EZLogger_Databases folder")
            Dim libPath As String = ConfigHelper.PromptForFolder("Select the Forensic Reports Library folder")
            Dim edoPath As String = ConfigHelper.PromptForFolder("Select the EDO - Forensic Office folder")

            ' Step 2: Ensure all folders were selected
            If String.IsNullOrEmpty(dbPath) OrElse String.IsNullOrEmpty(libPath) OrElse String.IsNullOrEmpty(edoPath) Then
                MessageBox.Show("Please select all required folders.", "Incomplete Setup", MessageBoxButton.OK, MessageBoxImage.Warning)
                Return
            End If

            ' Step 3: Load the config file
            Dim configPath As String = ConfigHelper.GetLocalConfigPath()
            Dim configText As String = File.ReadAllText(configPath)
            Dim config = JsonSerializer.Deserialize(Of Models.LocalUserConfig)(configText)

            ' Step 4: Update config fields
            config.sp_filepath.databases = dbPath
            config.sp_filepath.user_forensic_database = dbPath
            config.sp_filepath.user_forensic_library = libPath
            config.sp_filepath.court_contact = Path.Combine(dbPath, "Court_Contact_Database.xlsx")
            config.sp_filepath.da_contact_database = Path.Combine(dbPath, "Da_Contact_Database.xlsx")
            config.sp_filepath.doctors_list = Path.Combine(dbPath, "Doctors.txt")
            config.sp_filepath.hlv_data = Path.Combine(dbPath, "HLV_Report_Database.xlsm")
            config.sp_filepath.hlv_due = Path.Combine(dbPath, "HLV Due for Visit.txt")
            config.sp_filepath.ods_filepath = Path.Combine(dbPath, "ODS.xlsm")
            config.sp_filepath.properties_list = Path.Combine(dbPath, "document_properties.txt")
            config.sp_filepath.templates = Path.Combine(dbPath, "Templates")

            config.edo_filepath.forensic_office = edoPath
            config.edo_filepath.processed_reports = "\Programming\EZ Logger\ProcessedReports"
            config.edo_filepath.tcars_folder = "\1370 FS Report Tracking"

            ' Step 5: Save updated config
            Dim options As New JsonSerializerOptions With {.WriteIndented = True}
            File.WriteAllText(configPath, JsonSerializer.Serialize(config, options))

            MessageBox.Show("EZLogger config paths saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information)
        End Sub

        Public Sub HandleTestFolderPickerClick()
            Dim selectedPath As String = ConfigHelper.PromptForFolder("Select your EZLogger_Databases folder")

            If Not String.IsNullOrEmpty(selectedPath) Then
                MessageBox.Show("You selected: " & selectedPath, "Folder Picker Test")
            Else
                MessageBox.Show("No folder was selected.", "Folder Picker Test")
            End If
        End Sub

        Public Sub SaveDoctorsList(doctorsText As String)
            Dim filePath As String = ListHelper.GetDoctorListFilePath()
            File.WriteAllText(filePath, doctorsText)
            MessageBox.Show("Doctor list saved.")
        End Sub

        ''' <summary>
        ''' Performs logic needed when ConfigView is loaded.
        ''' Returns the doctors list, local config path, and global config path status message.
        ''' </summary>
        Public Function HandleViewLoaded() As ConfigViewLoadResult
            Dim result As New ConfigViewLoadResult()
            result.DoctorList = ListHelper.GetDoctorList()

            Dim localConfigPath As String = ConfigHelper.GetLocalConfigPath()
            result.LocalConfigPath = localConfigPath

            Dim globalConfigPath As String = ConfigHelper.GetGlobalConfigPath()
            result.GlobalConfigPathMessage = If(String.IsNullOrEmpty(globalConfigPath),
                                        "Global config path not set. Please click [C] to configure it.",
                                        globalConfigPath)

            ' Load config and populate new paths
            Try
                Dim jsonText As String = File.ReadAllText(localConfigPath)
                Dim config = JsonSerializer.Deserialize(Of Models.LocalUserConfig)(jsonText)

                result.ForensicDatabasePath = config.sp_filepath.user_forensic_database
                result.ForensicLibraryPath = config.sp_filepath.user_forensic_library
                result.ForensicOfficePath = config.edo_filepath.forensic_office
            Catch ex As Exception
                ' Fallback if config isn't valid
                result.ForensicDatabasePath = "(config not loaded)"
                result.ForensicLibraryPath = "(config not loaded)"
                result.ForensicOfficePath = "(config not loaded)"
            End Try

            Return result
        End Function

        Public Sub HandleCreateConfigClick()
            ' Step 1: Ensure local_user_config.json exists
            Dim localConfigPath As String = ConfigHelper.EnsureLocalUserConfigFileExists()
            If String.IsNullOrEmpty(localConfigPath) Then
                MessageBox.Show("Failed to create or locate local config file.", "Setup Failed")
                Return
            End If

            ' Step 2: Prompt the user to select global_config.json
            Dim globalConfigPath As String = ConfigHelper.PromptForGlobalConfigFile()
            If String.IsNullOrEmpty(globalConfigPath) Then
                MessageBox.Show("Global config selection was cancelled or invalid.", "Setup Incomplete")
                Return
            End If

            ' Step 3: Load config and apply global_config_file
            Dim configText As String = File.ReadAllText(localConfigPath)
            Dim config = JsonSerializer.Deserialize(Of Models.LocalUserConfig)(configText)
            config.sp_filepath.global_config_file = globalConfigPath

            ' Step 4: Prompt for supporting folders
            Dim dbPath As String = ConfigHelper.PromptForFolder("Select the EZLogger_Databases folder")
            Dim libPath As String = ConfigHelper.PromptForFolder("Select the Forensic Reports Library folder")
            Dim edoPath As String = ConfigHelper.PromptForFolder("Select the EDO - Forensic Office folder")

            If String.IsNullOrEmpty(dbPath) OrElse String.IsNullOrEmpty(libPath) OrElse String.IsNullOrEmpty(edoPath) Then
                MessageBox.Show("All folder paths are required to complete setup.", "Missing Info", MessageBoxButton.OK, MessageBoxImage.Warning)
                Return
            End If

            ' Step 5: Fill in other sp_filepath and edo_filepath values
            config.sp_filepath.databases = dbPath
            config.sp_filepath.user_forensic_database = dbPath
            config.sp_filepath.user_forensic_library = libPath
            config.sp_filepath.court_contact = Path.Combine(dbPath, "Court_Contact_Database.xlsx")
            config.sp_filepath.da_contact_database = Path.Combine(dbPath, "Da_Contact_Database.xlsx")
            config.sp_filepath.doctors_list = Path.Combine(dbPath, "Doctors.txt")
            config.sp_filepath.hlv_data = Path.Combine(dbPath, "HLV_Report_Database.xlsm")
            'config.sp_filepath.hlv_due = Path.Combine(dbPath, "HLV Due for Visit.txt")
            'config.sp_filepath.ods_filepath = Path.Combine(dbPath, "ODS.xlsm")
            'config.sp_filepath.properties_list = Path.Combine(dbPath, "document_properties.txt")
            config.sp_filepath.templates = Path.Combine(dbPath, "Templates")

            config.edo_filepath.forensic_office = edoPath
            config.edo_filepath.processed_reports = "\Programming\EZ Logger\ProcessedReports"
            config.edo_filepath.tcars_folder = "\1370 FS Report Tracking"

            ' Step 6: Write back to config
            Dim options As New JsonSerializerOptions With {.WriteIndented = True}
            File.WriteAllText(localConfigPath, JsonSerializer.Serialize(config, options))

            MessageBox.Show("Configuration setup complete!" & Environment.NewLine &
                    "Local config stored at:" & Environment.NewLine & localConfigPath,
                    "EZLogger Setup Complete")
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
