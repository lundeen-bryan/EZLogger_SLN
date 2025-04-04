# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog],
and this project adheres to [Semantic Versioning].

## [Unreleased]

- /

## [1.0.2] - 2025-04-04

- Improved formatting and content clarity in HTML guides, including step-by-step instructions for WPF UserControls, event handling, and Windows Forms integration.
- Markdown files created/updated for better documentation accessibility, with clear sections on project setup, control creation, and property exposure.

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