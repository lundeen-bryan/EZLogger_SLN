# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog][Keep a Changelog],
and this project adheres to [Semantic Versioning][Semantic Versioning].

## [Unreleased][Unreleased]

- Plan new EZL_DUAL table to track dual commitments separately from EZL
- Grid display and patient lookup logic supporting multiple legal statuses
- Adjust UI to reflect dual status where relevant with a dual status field on Report Wizard Panel.
- Use CustomMsgBox form in place of Windows.MessageBox -- low priority if it enhances some aspect of the UI
- Refactor anywhere that it says Windows.MessageBox instead to use Imports MessageBox = System.Windows.MessageBox
- Cleanup and remove remnants of WriteMailMergeDataToDocProperties since MailMerge can't read from SQLite db files
- Add config check code to ensure the user has a user_config.json file before running EZLogger - see legacy fnc_check_config
- Add function to clear existing document properties before the user can add new properties see legacy fnc_clear_document_properties
- Add function in a module to copy to clipboard since it will be used a lot in this app
- Add function to get sender's name and add it as a document property as "ProcessedBy"
- Add function to position forms on the same screen as the Word application in the top left corner rather than in the center of the main screen see fnc_position_form in legacy
- Add function to return full filepath or filename or ext of open word document
- Add function to check if a file already exists in a directory
- Function to return user's temp file path
- Return users documents path for finding the user_config.json file
- Update EZL to show a charges column and write function to return charges from ODS
- Function to write metadata to the sharepoint site when the file is uploaded/saved there
- Functiion to close the document without notifications to the user
- Functions to convert document to pdf and xps
- Function to go to last page and first page and header/footer of document
- Function to insert signature
- Function that writes to excel
- Buttons to add to ribbon: 100% zoom, zoom One Page, Advanced Document Properties, Sharepoint Properties, Accept all changes and stop tracking, Print preview, Open MS Excel, paste plain format

## [0.0.1] - 2025-04-16

### Added

- contact databases for saving in sharepoint saved to data folder
- Added keyboard shortcut keybinding to UpdateInfo and PatientInfo forms

## [0.0.1] - 2025-04-15

### Added

- Introduced `GetLocalConfigPath()` in `ConfigHelper.vb` to dynamically resolve and ensure the existence of `%USERPROFILE%\.ezlogger\local_user_config.json`.
- Created `PromptForGlobalConfigFile()` to allow users to select the global configuration file from a synced SharePoint folder.
- Implemented `UpdateLocalConfigWithGlobalPath()` to write the global config path into the local config file under `sp_filepath.global_config_file`.
- Added manual test method `Test_UpdateLocalConfigWithGlobalPath()` in `TestHelper.vb` to validate config updates during development.
- Added new WPF button handler `BtnCreateConfig` to walk through config file setup (local path + global config selection).

### Changed

- Refactored all usages of hardcoded `localConfigPath` to dynamically call `GetLocalConfigPath()` instead.
- Removed obsolete `Private ReadOnly localConfigPath = ...` field from `ConfigHelper.vb`.

### Fixed

- ConfigView now shows appropriate fallback messages when config files are missing.
- Clicking `[C]` properly creates the config file and updates UI with the paths.

## [0.0.1][0.0.1] - 2025-04-14

### Changed

- Updated `ReportWizardHandler.vb` to link opinion form opening button functionality
- Modified `OpinionView.Xaml.vb` to use `OpinionHandler` and accept optional host form

### Added

- new methods in `OpinionHandler.vb` for user interactions, including form opening in far left side
- Added code snippets for code behind xaml form and handler
- Wired up AuthorView and SeniorView forms to show when H & G buttons are pressed respectively

### Removed

- `OpinionViewHandler` was named incorrectly and now it's consolidated with `OpinionHandler`
- Removed outdated even handler in `ReportWizardHandler`
- Removed `UserPathHelper` as it was a duplicate of `EnvironmentHelper`

## [0.0.1][0.0.1] - 2025-04-13

### Added

- `TCARListView.xaml` WPF UserControl to display active TCAR referrals from `tcar_list` table.
- `TCARListHost.vb` as a WinForms host form for embedding the TCAR WPF view.
- `TCARRecord.vb` model for mapping database rows to displayable data.
- `TCARListHandler.vb` with methods to:
- Load active TCAR records (`LoadAllActive`)
- Handle "Select" button click and write TCAR data to Word custom document properties.

### Changed

- Updated `ReportWizardPanel.xaml.vb` to wire Btn_D to launch `TCARListHost`.
- Updated `tcar_list` table schema to include `patient_name` for standalone lookup.

### Fixed

- Resolved issue where the TCAR list view was displaying empty due to missing SQL query logic.
- Corrected handler logic to call `WriteCustomProperty` instead of non-existent `WriteProperty`.

## [0.0.1][0.0.1] - 2025-04-12

### Added

- Implemented `HandleAcceptPPR` method in `ReportTypeHandler.vb` to calculate days until/since due date and update Word custom properties
- Enhanced layout settings in `ReportTypeHost` for better form presentation
- Added event handler in `ReportTypeView` for the new buton to trigger the PPR handling method
- check for early 90-Day reports in database using DatabaseHelper
- added colulmn in db for early_ninety_day
- M/F for each patient in EZL db
- Updated documentation on data flow
- when pt has an early 90 day report it shows the label on the ReportTypeView

### Changed

- Changed DocumentPropertyWriter to DocumentPropertyHelper
- Changed `ReportWizardHandler` and `ReportWizardPanel.xaml.vb` to be easier to recognize by name

### Deprecated

- PatientDatabaseHandler now we can use DatabaseHelper instead

## [0.0.1][0.0.1] - 2025-04-07

### Changed

- Refactored GetPatientByNumber ~~to GetPatientsByNumber, supporting up to 5 sorted results~~ EZL only has one record per patient number
- Refactored `LookupDatabase_Click` logic out of the view and into `ReportWizardHandler` to follow separation of concerns
- Replacedlegacy MailMerge-based property writing with a direct SQLite-backed workflow. Will need to be refactored for MSSQL later.

### Added

- Temporarily shows matches in a MessageBox when pressing Database button
- `WriteDataToDocProperties(patient As PatientCls)` method to write patient details into Word custom document properties.
- `AgeHelper.CalculateAge()` function to calculate a patient’s age from DOB, with full documentation and legacy logic preserved.
- Conditional confirmation prompt ("Does this information match the report?") using Yes/No dialog before writing document properties.

## [0.0.1][0.0.1] - 2025-04-05

### Added

- `CheifApprovalView` user control.
- Documentation: *About the ConfigHelper.md* explaining design choices.

### Changed

- `EZLogger.vbproj` to include new view files.
- `ConfigHelper.vb`: Added support for loading doctor names from JSON.
- `ConfigView`: Updated UI and logic for managing doctor lists.
- `global_config.json`: Added alerts and email distribution lists.
- `Doctors.txt`: Appended new doctor entries.

### Cleaned

- Removed unused imports from `OpinionView.vb`, `ReportAuthorView.vb`, and `ReportWizardPanel.vb`.

## [0.0.1][0.0.1] - 2025-04-04

- Improved formatting and content clarity in HTML guides, including step-by-step instructions for WPF UserControls, event handling, and Windows Forms integration.
- Markdown files created/updated for better documentation accessibility, with clear sections on project setup, control creation, and property exposure.
- Added a detailed document explaining the shared ComboBox population and syncing strategy, including how to reuse the pattern for other controls. (`.notes/Using this for all ReportType ComboBoxes.md`)
- Created a shared method `GetReportTypes` in `ReportTypeHandler` to return a list of report types. (`EZLogger/Handlers/ReportTypeHandler.vb`)
- Modified `ReportWizardPanel` to use the shared `GetReportTypes` method for populating the ComboBox. (`EZLogger/Views/ReportWizardPanel.xaml.vb`)
- Updated `ReportTypeView` to initialize the ComboBox with the shared report types and sync the selected item with `ReportWizardPanel`. (`EZLogger/Views/ReportTypeView.xaml.vb`)
- Added new fields to `global_config.json` and `local_user_config.json` to support the ComboBox and other control values. (`temp/global_config.json`, `temp/local_user_config.json`)
- Added `ConfigHelper.vb` in Helpers module to manage loading config paths and values from JSON.
- Updated `ReportTypeHandler.vb` to retrieve report types dynamically from `global_config.json` instead of hardcoding.
- Modified `OpinionView.xaml.vb` to populate `OpinionCbo` with values from the "opinions" section of the global config.
- Implemented `ReportWizardPanel_Loaded` to populate `ReportTypeCbo` from the global config.
- Refactored and structured `global_config.json` and `local_user_config.json` to support cleaner config access patterns.
- Updated `EZLogger.vbproj` to include new handlers and forms for report authors. Added `GetDoctorList` function in `ConfigHelper.vb` to load doctors from a config file. Modified `ReportWizardPanel.xaml` to open the new author form. Updated `local_user_config.json` with the path for the doctors list. Created `AuthorHandler` class to manage author form interactions. Added `ReportAuthorHost`, its designer, and the `ReportAuthorView` for author selection functionality.

## [0.0.1][0.0.1] - 2025-04-03

### Updated

- Added `FormHeaderControl` with `HeaderText` dependency property for customizable headers.
- Updated `OpinionHandler` to open `OpinionHost` form on button click.
- Integrated WPF content into `OpinionHost` using `ElementHost`.
- Added `OpinionHost.resx` resource file for localization and resource management.
- Replaced custom button controls with standard WPF `Button` controls in `OpinionView`, `PatientInfoView`, and `ReportWizardPanel` for consistency.
- Linked `ReportWizardPanel` to new opinion form functionality.
- Included new XAML and code-behind files in project file.
- Add report type selection feature and related files
- This update introduces a new feature for selecting report types, including the addition of `ReportTypeHost`, `ReportTypeView`, and their associated designer and resource files. The project file has been updated to include these new components, ensuring they are compiled correctly.
- The `ReportTypeHandler` class now opens the `ReportTypeHost` form upon button click, improving user interaction. Additionally, the `ReportWizardPanel.xaml` button text has been updated to reflect this new functionality.
- A changelog has been added to document these changes, adhering to the Keep a Changelog format. Resource files have also been included to support localization for the new feature.

<!-- Links -->

<!-- Versions -->

[keep a changelog]: https://keepachangelog.com/en/1.0.0/
[semantic versioning]: https://semver.org/spec/v2.0.0.html
[unreleased]: https://github.com/lundeen-bryan/EZLogger-SLN/compare/v0.0.2...HEAD
[0.0.2]: https://github.com/lundeen-bryan/EZLogger-SLN/compare/v0.0.1...v0.0.2
[0.0.1]: https://github.com/lundeen-bryan/EZLogger-SLN/releases/tag/v0.0.1