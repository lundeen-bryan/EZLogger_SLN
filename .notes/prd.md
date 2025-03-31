**EZLogger Product Requirements Document (PRD)**


**1. Introduction**

**Purpose:**
EZLogger is a VSTO Office Add-in built with VB.NET and WPF using the MVVM pattern. It streamlines the workflow for forensic report processing by automating validation of patient identification data, saving information to SharePoint, and converting documents to PDF for sharing.

**Scope:**
EZLogger operates within Microsoft Word and integrates with hospital systems including the ODS (for patient data) and internal SQL Server databases (for sheriff, DA, court, and CONREP address information). It replaces a legacy VBA-based solution and offers a modern, reliable interface.


**2. Features and Functionalities**

- **Data Validation:** Validates Name, DOB, PFN, DOJ number, admission date, hearing date, and more against ODS records. Flags mismatches visually and in logs.
- **SharePoint Integration:** Saves data and documents to SharePoint via REST API or CSOM. Supports retries and logging.
- **PDF Conversion:** Converts Word docs to PDF with naming/location options. Optionally saves to SharePoint.
- **Cover Page Generation:** Adds cover pages using local templates. Users select from a form similar to the legacy version.


**3. User Interface (UI)**

- Adds a custom ribbon tab with buttons: Validate Data, Add Cover Page, Save to SharePoint, Convert to PDF, Settings.
- Optional task pane for user guidance.
- Forms are upgraded from VBA to WPF, showing patient info and validation results.


**4. Error Handling and Logging**

- Logs stored in AppData.
- Message boxes alert users to critical failures.


**5. Configuration and Settings**

- Shared config file for global settings (e.g. SharePoint URLs).
- Per-user config for paths, preferences.


**6. Security and Compliance**

- Encrypted transmission to SharePoint.
- HIPAA compliant.


**7. Performance and Reliability**

- Processing under 5 seconds expected.
- Retry logic and fallback to local saving if SharePoint fails.


**8. Deployment and Installation**

- Includes dependencies like VSTO runtime.
- Supports enterprise deployment and versioned updates.


**9. Testing and QA**

- Unit, integration, and UAT with clinical staff.


**10. Documentation and Support**

- Includes user guides and help links.
- Support through internal IT or clinical systems team.


**11. Issues to Address with Converting to VB.NET**

- **INI Files:** The legacy VBA version uses .ini files for logging and settings. VB.NET does not support .ini files natively, though interop is possible. Consider transitioning to a more modern format such as JSON.
- **Logging:** Current logs are written as .ini files and stored on a shared network drive. Evaluate whether structured logging in JSON format (saved locally then optionally synced to SharePoint) would provide better readability, security, and reliability.
- **User and Global Config File Storage:**
  - **User config** is currently stored in a hidden folder in the user's Documents. Consider moving to `%AppData%` for consistency with Windows application conventions.
  - **Global config** is synced from a SharePoint Document Library. Consider pulling the config programmatically from SharePoint instead of relying on sync folders, to improve reliability and version control.

