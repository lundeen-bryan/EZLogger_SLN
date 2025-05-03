' Namespace=EZLogger/Handlers
' Filename=ConfigViewHandler.vb
' !See Label Footer for notes

Imports EZLogger.Helpers
Imports EZLogger.Models
Imports System.IO
Imports System.Windows
Imports MessageBox = System.Windows.MessageBox
Imports System.Text.Json

Namespace Handlers
    Public Class ConfigViewHandler

        ''' <summary>
        ''' Handles the click event for adding a new county alert.
        ''' This method opens a popup for user input, and if confirmed, adds or updates the county alert in the global configuration file.
        ''' </summary>
        ''' <remarks>
        ''' The method performs the following steps:
        ''' 1. Opens an AddAlertPopup dialog to get user input.
        ''' 2. If the user confirms, retrieves the alert key and value from the popup.
        ''' 3. Loads the global configuration file.
        ''' 4. Deserializes the JSON content of the file.
        ''' 5. Adds or updates the county alert in the configuration.
        ''' 6. Serializes and saves the updated configuration back to the file.
        ''' </remarks>
        Public Sub AddCountyAlertButtonClick()
            Dim popup As New AddAlertPopup(True)
            Dim result = popup.ShowDialog()

            If result = True Then
                Dim key = popup.AlertKey
                Dim value = popup.AlertValue

                Dim globalPath As String = ConfigHelper.GetGlobalConfigPath()
                If String.IsNullOrEmpty(globalPath) OrElse Not File.Exists(globalPath) Then Exit Sub

                Dim jsonText As String = File.ReadAllText(globalPath)
                Dim rootDict = JsonSerializer.Deserialize(Of Dictionary(Of String, Object))(jsonText)

                Dim countyDict As Dictionary(Of String, String)
                If rootDict.ContainsKey("county_alerts") Then
                    countyDict = JsonSerializer.Deserialize(Of Dictionary(Of String, String))(rootDict("county_alerts").ToString())
                Else
                    countyDict = New Dictionary(Of String, String)()
                End If

                countyDict(key) = value
                rootDict("county_alerts") = countyDict

                Dim options As New JsonSerializerOptions With {.WriteIndented = True}
                File.WriteAllText(globalPath, JsonSerializer.Serialize(rootDict, options))
            End If
        End Sub

        ''' <summary>
        ''' Handles the click event for adding a new alert.
        ''' This method opens a popup for user input, and if confirmed, adds or updates the alert in the global configuration file.
        ''' </summary>
        ''' <remarks>
        ''' The method performs the following steps:
        ''' 1. Opens an AddAlertPopup dialog to get user input.
        ''' 2. If the user confirms, retrieves the alert key and value from the popup.
        ''' 3. Loads the global configuration file.
        ''' 4. Deserializes the JSON content of the file.
        ''' 5. Adds or updates the alert in the configuration.
        ''' 6. Serializes and saves the updated configuration back to the file.
        ''' </remarks>
        Public Sub AddAlertButtonClick()
            Dim popup As New AddAlertPopup(False)
            Dim result = popup.ShowDialog()

            If result = True Then
                Dim key = popup.AlertKey
                Dim value = popup.AlertValue

                Dim globalPath As String = ConfigHelper.GetGlobalConfigPath()
                If String.IsNullOrEmpty(globalPath) OrElse Not File.Exists(globalPath) Then Exit Sub

                Dim jsonText As String = File.ReadAllText(globalPath)
                Dim rootDict = JsonSerializer.Deserialize(Of Dictionary(Of String, Object))(jsonText)

                ' Get or create Alerts section
                Dim alertDict As Dictionary(Of String, String)
                If rootDict.ContainsKey("Alerts") Then
                    alertDict = JsonSerializer.Deserialize(Of Dictionary(Of String, String))(rootDict("Alerts").ToString())
                Else
                    alertDict = New Dictionary(Of String, String)()
                End If

                ' Add or update
                alertDict(key) = value
                rootDict("Alerts") = alertDict

                ' Save updated JSON
                Dim options As New JsonSerializerOptions With {.WriteIndented = True}
                File.WriteAllText(globalPath, JsonSerializer.Serialize(rootDict, options))
            End If
        End Sub

        ''' <summary>
        ''' Deletes a patient alert from the global configuration file based on the provided patient number.
        ''' </summary>
        ''' <param name="patientNumber">The unique identifier of the patient whose alert should be deleted.</param>
        ''' <remarks>
        ''' This method performs the following steps:
        ''' 1. Retrieves the global configuration file path.
        ''' 2. Reads and parses the JSON content of the file.
        ''' 3. Checks if the "Alerts" section exists in the configuration.
        ''' 4. If the patient number exists in the alerts, it removes the corresponding alert.
        ''' 5. Updates the configuration file with the modified alerts.
        ''' If the global configuration file doesn't exist or the "Alerts" section is not present, the method will exit without making any changes.
        ''' </remarks>
        Public Sub DeletePatientAlertByKey(patientNumber As String)
            Dim globalPath As String = ConfigHelper.GetGlobalConfigPath()
            If String.IsNullOrEmpty(globalPath) OrElse Not File.Exists(globalPath) Then Exit Sub

            Dim jsonText As String = File.ReadAllText(globalPath)
            Dim doc = JsonDocument.Parse(jsonText)

            Dim rootDict = JsonSerializer.Deserialize(Of Dictionary(Of String, Object))(jsonText)

            If rootDict.ContainsKey("Alerts") Then
                Dim alertDict = JsonSerializer.Deserialize(Of Dictionary(Of String, String))(
            doc.RootElement.GetProperty("Alerts").ToString())

                If alertDict.ContainsKey(patientNumber) Then
                    alertDict.Remove(patientNumber)
                End If

                rootDict("Alerts") = alertDict

                Dim options As New JsonSerializerOptions With {.WriteIndented = True}
                File.WriteAllText(globalPath, JsonSerializer.Serialize(rootDict, options))
            End If
        End Sub

        ''' <summary>
        ''' Deletes a county alert from the global configuration file based on the provided county name.
        ''' </summary>
        ''' <param name="countyName">The name of the county whose alert should be deleted.</param>
        ''' <remarks>
        ''' This method performs the following steps:
        ''' 1. Retrieves the global configuration file path.
        ''' 2. Reads and parses the JSON content of the file.
        ''' 3. Checks if the "county_alerts" section exists in the configuration.
        ''' 4. If the county name exists in the alerts, it removes the corresponding alert.
        ''' 5. Updates the configuration file with the modified county alerts.
        ''' If the global configuration file doesn't exist or the "county_alerts" section is not present, the method will exit without making any changes.
        ''' </remarks>
        Public Sub DeleteCountyAlertByKey(countyName As String)
            Dim globalPath As String = ConfigHelper.GetGlobalConfigPath()
            If String.IsNullOrEmpty(globalPath) OrElse Not File.Exists(globalPath) Then Exit Sub

            Dim jsonText As String = File.ReadAllText(globalPath)
            Dim doc = JsonDocument.Parse(jsonText)

            Dim rootDict = JsonSerializer.Deserialize(Of Dictionary(Of String, Object))(jsonText)

            If rootDict.ContainsKey("county_alerts") Then
                Dim countyAlerts = JsonSerializer.Deserialize(Of Dictionary(Of String, String))(
            doc.RootElement.GetProperty("county_alerts").ToString())

                If countyAlerts.ContainsKey(countyName) Then
                    countyAlerts.Remove(countyName)
                End If

                rootDict("county_alerts") = countyAlerts

                ' Save the updated config
                Dim options As New JsonSerializerOptions With {.WriteIndented = True}
                File.WriteAllText(globalPath, JsonSerializer.Serialize(rootDict, options))
            End If
        End Sub


        ''' <summary>
        ''' Loads patient alerts from the global configuration file and formats them as a list of strings.
        ''' </summary>
        ''' <returns>
        ''' A List(Of String) containing formatted patient alerts. Each string in the list represents
        ''' an alert in the format "PatientNumber = AlertMessage".
        ''' </returns>
        ''' <remarks>
        ''' This function performs the following steps:
        ''' 1. Retrieves the path to the global configuration file.
        ''' 2. Calls ConfigHelper.GetPatientAlerts to obtain the patient alerts from the configuration.
        ''' 3. Formats each alert as a string with the patient number as the key and the alert message as the value.
        ''' 4. Returns the formatted alerts as a List(Of String).
        ''' </remarks>
        Public Function LoadPatientAlerts() As List(Of String)
            Dim globalPath As String = ConfigHelper.GetGlobalConfigPath()
            Dim alerts = ConfigHelper.GetPatientAlerts(globalPath)

            Return alerts.Select(Function(kvp) $"{kvp.Key} = {kvp.Value}").ToList()
        End Function

        ''' <summary>
        ''' Loads county alerts from the global configuration file and formats them as a list of strings.
        ''' </summary>
        ''' <returns>
        ''' A List(Of String) containing formatted county alerts. Each string in the list represents
        ''' an alert in the format "CountyName = AlertMessage".
        ''' </returns>
        ''' <remarks>
        ''' This function performs the following steps:
        ''' 1. Retrieves the path to the global configuration file.
        ''' 2. Calls ConfigHelper.GetCountyAlerts to obtain the county alerts from the configuration.
        ''' 3. Formats each alert as a string with the county name as the key and the alert message as the value.
        ''' 4. Returns the formatted alerts as a List(Of String).
        ''' </remarks>
        Public Function LoadCountyAlerts() As List(Of String)
            Dim globalPath As String = ConfigHelper.GetGlobalConfigPath()
            Dim countyAlerts = ConfigHelper.GetCountyAlerts(globalPath)

            Return countyAlerts.Select(Function(kvp) $"{kvp.Key} = {kvp.Value}").ToList()
        End Function

        ''' <summary>
        ''' Handles the setup of folder paths for the EZLogger application.
        ''' This method prompts the user to select essential folders, updates the configuration with the selected paths,
        ''' and saves the updated configuration to the local config file.
        ''' </summary>
        ''' <remarks>
        ''' The method performs the following steps:
        ''' 1. Prompts the user to select the EZLogger_Databases, Forensic Reports Library, and EDO - Forensic Office folders.
        ''' 2. Validates that all required folders have been selected.
        ''' 3. Loads the existing local configuration file.
        ''' 4. Updates the configuration with the new folder paths and derived file paths.
        ''' 5. Saves the updated configuration back to the local config file.
        ''' 6. Displays a success message to the user.
        ''' </remarks>
        ''' <exception cref="System.IO.FileNotFoundException">Thrown when the local configuration file cannot be found.</exception>
        ''' <exception cref="System.IO.IOException">Thrown when there's an error reading from or writing to the configuration file.</exception>
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

        ''' <summary>
        ''' Handles the click event for testing the folder picker functionality.
        ''' This method prompts the user to select a folder and displays the selected path or a message if no folder was selected.
        ''' </summary>
        ''' <remarks>
        ''' This method performs the following steps:
        ''' 1. Prompts the user to select the EZLogger_Databases folder using ConfigHelper.PromptForFolder.
        ''' 2. Checks if a folder was selected.
        ''' 3. Displays a message box with the selected path if a folder was chosen, or a message indicating no selection otherwise.
        ''' </remarks>
        Public Sub HandleTestFolderPickerClick()
            Dim selectedPath As String = ConfigHelper.PromptForFolder("Select your EZLogger_Databases folder")

            If Not String.IsNullOrEmpty(selectedPath) Then
                MessageBox.Show("You selected: " & selectedPath, "Folder Picker Test")
            Else
                MessageBox.Show("No folder was selected.", "Folder Picker Test")
            End If
        End Sub

        ''' <summary>
        ''' Saves the provided list of doctors to a file.
        ''' </summary>
        ''' <param name="doctorsText">A string containing the list of doctors, typically with each doctor's name on a separate line.</param>
        ''' <remarks>
        ''' This method performs the following steps:
        ''' 1. Retrieves the file path for the doctors list using ListHelper.GetDoctorListFilePath().
        ''' 2. Writes the provided doctorsText to the file, overwriting any existing content.
        ''' 3. Displays a message box to confirm that the doctor list has been saved.
        ''' </remarks>
        Public Sub SaveDoctorsList(doctorsText As String)
            Dim filePath As String = ListHelper.GetDoctorListFilePath()
            File.WriteAllText(filePath, doctorsText)
            MessageBox.Show("Doctor list saved.")
        End Sub

        ''' <summary>
        ''' Handles the loading of the configuration view, populating various configuration paths and settings.
        ''' </summary>
        ''' <returns>
        ''' A ConfigViewLoadResult object containing:
        ''' - DoctorList: A list of doctors retrieved from the system.
        ''' - LocalConfigPath: The path to the local configuration file.
        ''' - GlobalConfigPathMessage: A message indicating the status of the global configuration path.
        ''' - ForensicDatabasePath: The path to the forensic wizard database on SharePoint.
        ''' - ForensicLibraryPath: The path to the forensic library on SharePoint.
        ''' - ForensicOfficePath: The path to the forensic office on EDO.
        ''' </returns>
        ''' <remarks>
        ''' This function performs the following operations:
        ''' 1. Retrieves the doctor list.
        ''' 2. Gets the local and global configuration paths.
        ''' 3. Attempts to load the local configuration file and extract relevant paths.
        ''' 4. If the configuration loading fails, it sets default values for the paths.
        ''' </remarks>
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

        ''' <summary>
        ''' Handles the creation and setup of the EZLogger configuration.
        ''' </summary>
        ''' <remarks>
        ''' This method performs the following steps:
        ''' 1. Ensures the local user configuration file exists.
        ''' 2. Prompts the user to select the global configuration file.
        ''' 3. Loads the local configuration and applies the global configuration file path.
        ''' 4. Prompts the user to select essential folders (EZLogger_Databases, Forensic Reports Library, and EDO - Forensic Office).
        ''' 5. Updates the configuration with the selected paths and derived file paths.
        ''' 6. Saves the updated configuration back to the local config file.
        ''' </remarks>
        ''' <exception cref="System.IO.FileNotFoundException">Thrown when the local or global configuration file cannot be found or created.</exception>
        ''' <exception cref="System.IO.IOException">Thrown when there's an error reading from or writing to the configuration files.</exception>
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

        Public Sub DeleteAlertButtonClick()
            MsgBox("You clicked Delete Alert button")
        End Sub

        Public Sub DeleteCountyAlertButtonClick()
            MsgBox("You clicked Delete County Alert button")
        End Sub

        ''' <summary>
        ''' Handles the click event for editing email settings based on selected radio buttons.
        ''' </summary>
        ''' <param name="r1">RadioButton representing the "Secretaries" option.</param>
        ''' <param name="r2">RadioButton representing the "Friday" option.</param>
        ''' <param name="r3">RadioButton representing the "Competent" option.</param>
        ''' <remarks>
        ''' This method checks which radio button is selected and displays a message box accordingly.
        ''' If no radio button is selected, it shows a message indicating that no option is selected.
        ''' </remarks>
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

' Footer:
''===========================================================================================
'' Filename: .......... ConfigViewHandler.vb
'' Description: ....... handles the config view
'' Created: ........... 2025-05-02
'' Updated: ........... 2025-05-02
'' Installs to: ....... EZLogger/Handlers
'' Compatibility: ..... VSTO
'' Contact Author: .... lundeen-bryan
'' Copyright:  ........ ©2025. All rights reserved.
'' Notes: ............. _
' (1) notes_here
''===========================================================================================