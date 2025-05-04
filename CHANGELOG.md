# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog][Keep a Changelog],
and this project adheres to [Semantic Versioning][Semantic Versioning].

## [Unreleased]

- Plan new EZL_DUAL table to track dual commitments separately from EZL
- Add config check code to ensure the user has a user_config.json file before running EZLogger - see legacy fnc_check_config
- Add function to check if a file already exists in a directory
- Update EZL to show a charges column and write function to return charges from ODS
- Functions to convert document to pdf and xps
- Function that writes to excel
- Buttons to add to ribbon: 100% zoom, zoom One Page, Advanced Document Properties, Sharepoint Properties, Accept all changes and stop tracking, Print preview, Open MS Excel, paste plain format

## [0.0.1] - 2025-05-04

### Added

- Introduced a new error dialog feature to improve error communication:
  - Added `ErrorDialogHandler.vb`, `ErrorDialogHost.vb`, `ErrorDialogView.xaml`, and related resource and designer files.
  - Defined WPF UI layout and logic for the error dialog.
  - Enabled localization via `ErrorDialogHost.resx`.

- Implemented centralized error handling and logging:
  - Created `ErrorHelper.vb` for reusable error handling logic.
  - Integrated logging with `LogHelper.vb` using config-defined log paths.
  - Included error simulation and capture in `EZLoggerRibbonXml.vb`.

- Enhanced error dialog behavior and appearance:
  - Added `SetErrorFields` method and wired button handlers in `ErrorDialogView.xaml.vb`.
  - Updated UI with larger font and improved styling in `ErrorDialogView.xaml`.
  - Ensured consistent behavior in `ErrorDialogHost.vb` when displaying errors.
  - Log output is now written to `error_log.txt` to demonstrate functionality.

- Added handler wiring for new error dialog buttons:
  - Implemented `HandleOkClick`, `HandleAbortClick`, and `HandleCopyClick` in `ErrorDialogHandler.vb`.

### Changed

- Standardized file and class naming from `TCAR` to `Tcar` across the project:
  - Renamed `TCARListHandler.vb` to `TcarListHandler.vb`.
  - Updated all relevant method and XAML references.

- Improved usability and modularity of various handlers:
  - Added `HandleCloseClick` in `AboutWinHandler.vb` and refined the same in `ChiefApprovalHandler.vb`.
  - Implemented `HandleEditEmailClick` in `ConfigViewHandler.vb` for email configuration.
  - Enhanced due date logic in `DueDatePprHandler.vb` and `DueDates1370Handler.vb`.
  - Expanded evaluator management methods in `EvaluatorHandler.vb`.
  - Made usability improvements in `FaxCoverHandler.vb`, `OpinionHandler.vb`, `PatientInfoHandler.vb`, and `SaveFileHandler.vb`.

- Improved closing logic for secondary windows in `UpdateInfoView.xaml.vb`.

## [0.0.1] – 2025-05-02

### Added

- Implemented email sending functionality with a dedicated UI:
  - Created `SendEmailHandler` for handling Outlook-based email operations.
  - Designed `SendEmailView.xaml` and `SendEmailHost.vb` for the email interface.
  - Added COM references for Microsoft Outlook integration.
  - Wired Ribbon button to open the new email window.
- Introduced court number support:
  - Added `CourtNumber` property to `PatientCls`.
  - Created `GetCourtNumberByPatientNumber` function in `DatabaseHelper.vb`.
  - Updated `DocumentPropertyHelper.vb` to write `CourtNumber` to document properties.
- Added `HandleCloseClick` method in `AboutWinHandler.vb`.
- Enabled and renamed Config button in `AboutView.xaml`, wired it to open `ConfigHost`.

### Changed

- Enhanced `ReportWizardHandler`:
  - Added logic for Word application automation and document logging.
  - Introduced `ShowBtnBMessage` to update document properties with last report access info.
- Improved name formatting logic in `SaveFileHandler` using title case.
- Updated `MessageBoxConfig`:
  - Added `ShowOk` property to control visibility of OK button.
- Refactored patient-related properties:
  - Renamed `DischargeStatus` to `BedStatus` in `DatabaseHelper.vb`.
  - Added `FirstPatientNumber` to `PatientCls`.
  - Updated `DocumentPropertyHelper.vb` to include `BedStatus`.

### Fixed

- Minor resource and label updates for clarity.

### Documentation

- Updated `CHANGELOG.md` to reflect version 0.0.1 and include filename format info.

## [0.0.1] - 2025-05-01

### Added

- Function to remove MailMerge and template using `CleanMailMergeDocument`
- PatientName and PatientNumber loads in the blue labels when the Add/Edit view opens
- When doc is closed then the `ReportWizardPanel` is cleared of all valules
- New feature to save name to Task list if found on TCAR list

## [0.0.1] - 2025-04-26

### Added

- Added methods in `SaveFileHandler` to load and validate Move/Copy paths, and handle Save As operations with old file deletion.
- Introduced `TryDeleteOldFile` for safe deletion of original files after Move, including legacy file handling.
- Created a design document for `SaveFileView` outlining its purpose, workflow, UI components, and technical behavior.
- Included `WordAppHelper.vb` for Word-related methods.
- Added `FaxCoverView_Loaded` method in `FaxCoverView.xaml.vb` to load cover page options and update page count.
- Added new helper files: BookmarkHelper, CoverTemplateMap, ExportPdfHelper, MailMergeHelper, MetadataHelper, TempFileHelper, and WordTemplateHelper for improved document management functionalities.

### Changed

- Updated `SaveFileView` to wire up new button handlers for Show Path and Save As, and modified UI components accordingly.
- Updated namespace in `FaxCoverHost.vb` and set form properties for better display.
- Modified `ShowFaxCoverMessage` in `FaxCoverHandler.vb` to position the `FaxCoverHost` form correctly.
- Adjusted `SaveFileHost` for proper form positioning.
- Renamed controls in `FaxCoverView.xaml` for clarity and consistency.
- Updated EZLoggerRibbon with new buttons: SetupGroup, HelpButton, SettingsButton, AboutButton, and SyncButton to enhance user interaction.
- Implemented SavePropsButton_Click method to save built-in document properties based on custom properties.
- Modified FaxCoverHandler to include methods for creating fax covers and exporting documents to PDF.
- Updated SaveFileHandler to capture built-in document properties during file saves.
- Improved button click handling and cover page loading in FaxCoverView for better user experience.

## [0.0.1] - 2025-04-25

### Added

- When clipboardhelper copies text it shows a statusbar msg
- Added feature to insert signature and write approved by to doc properties
- Updated `EZLogger.vbproj` to include new Ribbon files and resources. Removed several buttons and groups from `EZLoggerRibbon.Designer.vb`.
- Introduced `EZLoggerRibbonXml` class for XML handling and updated event handlers in `EZLoggerRibbon.vb`. Modified `ThisAddIn` to instantiate the new Ribbon functionality.

### Changed

- WordFooterReader uses clipboardHelper and shows what was copied in the statusbar

### Deprecated

- Removed database group from ribbon to decrease over-engineering

## [0.0.1] - 2025-04-24

### Added

- ShowBtnEMessage(patientNumber) in ReportWizardHandler.vb to perform asynchronous Excel lookups for the "CONREP" provider using ExcelHelper.GetProviderFromHLV.
- Function to insert signature
- Custom BusyControl.xaml with an indeterminate progress bar to visually indicate background activity.
- BusyHost.vb form to host the WPF-based busy control using ElementHost.
- Integration of BusyHost into ShowBtnEMessage with Await Task.Delay(100) and Await Task.Run(...) to ensure responsive UI during long-running Excel operations.
- Error handling and user feedback via MsgBoxHelper when provider values are found or missing.

### Changed

- Btn_E_Click in ReportWizardPanel.xaml.vb now retrieves the patient number from TextBoxPatientNumber and calls ShowBtnEMessage directly.
- Prevents double-clicking of Btn_E with TimerHelper.DisableTemporarily.

## [0.0.1] - 2025-04-21

### Changed

- Updated most documentation in .notes

## [0.0.1] - 2025-04-20

### Added

- Introduced `AboutInfoResult` DTO to encapsulate version metadata from `global_config.json`.
- Created `AboutWinHandler` to centralize config parsing logic and return a structured result object.
- View now only updates UI controls using values from the DTO, improving separation of concerns and maintainability.
- Added `HasError` and `ErrorMessage` pattern to enable consistent error reporting from handlers to views.
- Added an article about DTO models for future documentation
- AddAlertPopup now pops up to let user add name or county to alerts in configview
- `AlertHelper` to find the County or patient number in `global_config.json` and show user an alert
- Added a timer feature to `ReportWizardPanel` so that every button requires the user wait 2 seconds before they can press it again to prevent it from being accidently pressed twice in a row
- Added a close button to the TCAR list

### Changed

- report wizard panel removed and cleaned up old code
- cleaned up PatientInfoView
- Refactored `ConfigView_Loaded` logic to use a `ConfigViewLoadResult` DTO returned by `ConfigViewHandler.HandleViewLoaded`.
- Moved configuration and file validation logic out of the view and into the handler for better separation of concerns.
- The view now only handles UI updates; all business logic (e.g., loading doctor list, checking config files) resides in the handler.
- moved HandleYearDown button click to the handler from the code behind the xaml
- Refactored AboutView to remove direct JSON parsing and file logic from the code-behind.

## [0.0.1] - 2025-04-19

### Added

- `DueDate1370View` to pick due dates for 1370
- `Early90DayLbl` now shows if it had an early 90-Day report
- Added a test helper and button to the ribbon to pull a random patient from the database for testing purposes.
- Added Due date handler to show the PPR due dates form with proper due dates and expiration date
- Added a trash icon to Ribbon to delete all custom document properties

## [0.0.1] - 2025-04-17

### Added

- link between doc properties and the SaveFileView form auto fills when user presses search btn

### Changed

- divided the `ConfigHelper.vb` into `PathHelper` and `ListHelper`
- complete change to `ReportTypeView` which is now split into 2 sub views

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

## [0.0.1] - 2025-04-14

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

## [0.0.1] - 2025-04-13

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

## [0.0.1] - 2025-04-12

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

## [0.0.1] - 2025-04-07

### Changed

- Refactored GetPatientByNumber ~~to GetPatientsByNumber, supporting up to 5 sorted results~~ EZL only has one record per patient number
- Refactored `LookupDatabase_Click` logic out of the view and into `ReportWizardHandler` to follow separation of concerns
- Replacedlegacy MailMerge-based property writing with a direct SQLite-backed workflow. Will need to be refactored for MSSQL later.

### Added

- Temporarily shows matches in a MessageBox when pressing Database button
- `WriteDataToDocProperties(patient As PatientCls)` method to write patient details into Word custom document properties.
- `AgeHelper.CalculateAge()` function to calculate a patient’s age from DOB, with full documentation and legacy logic preserved.
- Conditional confirmation prompt ("Does this information match the report?") using Yes/No dialog before writing document properties.

## [0.0.1] - 2025-04-05

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

## [0.0.1] - 2025-04-04

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

## [0.0.1] - 2025-04-03

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