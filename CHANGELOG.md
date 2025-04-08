# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog],
and this project adheres to [Semantic Versioning].

## [Unreleased]

- Plan new EZL_DUAL table to track dual commitments separately from EZL
- Use for future grid display and patient lookup logic supporting multiple legal statuses
- Adjust UI to reflect dual status where relevant with a dual status field on Report Wizard Panel.
- Use CustomMsgBox form in place of Windows.MessageBox -- low priority

## [1.0.4] - 2025-04-07

### Changed

- Refactored GetPatientByNumber to GetPatientsByNumber, supporting up to 5 sorted results

### Added

- Temporarily shows matches in a MessageBox

## [1.0.3] - 2025-04-05

### Added

- `CheifApprovalView` user control.
- Documentation: *About the ConfigPathHelper.md* explaining design choices.

### Changed

- `EZLogger.vbproj` to include new view files.
- `ConfigPathHelper.vb`: Added support for loading doctor names from JSON.
- `ConfigView`: Updated UI and logic for managing doctor lists.
- `global_config.json`: Added alerts and email distribution lists.
- `Doctors.txt`: Appended new doctor entries.

### Cleaned

- Removed unused imports from `OpinionView.vb`, `ReportAuthorView.vb`, and `ReportWizardPanel.vb`.


## [1.0.2] - 2025-04-04

- Improved formatting and content clarity in HTML guides, including step-by-step instructions for WPF UserControls, event handling, and Windows Forms integration.
- Markdown files created/updated for better documentation accessibility, with clear sections on project setup, control creation, and property exposure.
- Added a detailed document explaining the shared ComboBox population and syncing strategy, including how to reuse the pattern for other controls. (`.notes/Using this for all ReportType ComboBoxes.md`)
- Created a shared method `GetReportTypes` in `ReportTypeHandler` to return a list of report types. (`EZLogger/Handlers/ReportTypeHandler.vb`)
- Modified `ReportWizardPanel` to use the shared `GetReportTypes` method for populating the ComboBox. (`EZLogger/Views/ReportWizardPanel.xaml.vb`)
- Updated `ReportTypeView` to initialize the ComboBox with the shared report types and sync the selected item with `ReportWizardPanel`. (`EZLogger/Views/ReportTypeView.xaml.vb`)
- Added new fields to `global_config.json` and `local_user_config.json` to support the ComboBox and other control values. (`temp/global_config.json`, `temp/local_user_config.json`)
- Added `ConfigPathHelper.vb` in Helpers module to manage loading config paths and values from JSON.
- Updated `ReportTypeHandler.vb` to retrieve report types dynamically from `global_config.json` instead of hardcoding.
- Modified `OpinionView.xaml.vb` to populate `OpinionCbo` with values from the "opinions" section of the global config.
- Implemented `ReportWizardPanel_Loaded` to populate `ReportTypeCbo` from the global config.
- Refactored and structured `global_config.json` and `local_user_config.json` to support cleaner config access patterns.
- Updated `EZLogger.vbproj` to include new handlers and forms for report authors. Added `GetDoctorList` function in `ConfigPathHelper.vb` to load doctors from a config file. Modified `ReportWizardPanel.xaml` to open the new author form. Updated `local_user_config.json` with the path for the doctors list. Created `AuthorHandler` class to manage author form interactions. Added `ReportAuthorHost`, its designer, and the `ReportAuthorView` for author selection functionality.

## [1.0.1] - 2025-04-03

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
[keep a changelog]: https://keepachangelog.com/en/1.0.0/
[semantic versioning]: https://semver.org/spec/v2.0.0.html

<!-- Versions -->
[unreleased]: https://github.com/lundeen-bryan/EZLogger-SLN/compare/v0.0.2...HEAD
[0.0.2]: https://github.com/lundeen-bryan/EZLogger-SLN/compare/v0.0.1...v0.0.2
[0.0.1]: https://github.com/lundeen-bryan/EZLogger-SLN/releases/tag/v0.0.1