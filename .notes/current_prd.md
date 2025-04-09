# Product Requirements Document: EZLogger

## 1. Introduction

**Purpose:**
EZLogger is a VSTO Office Add-in developed in VB.NET with WPF using the MVVM pattern. It supports analysts who review and finalize forensic reports written by doctors. The tool streamlines the workflow by automating patient information validation, saving data to SharePoint, and converting documents to PDF. A built-in Configuration Editor guides users through a one-time setup process to configure file paths and locations used throughout the application.

**Scope:**
EZLogger integrates into Microsoft Word and works with local and synced file systems. It validates report data against patient records, manages file storage in locally synced SharePoint folders (via OneDrive), and generates PDFs for distribution. The Configuration Editor is only used the first time EZLogger is launched to help users set up local paths, including those for templates, doctor lists, and SharePoint sync folders.

---

## 2. Features and Functionalities

### 2.1 EZLogger Add-in

- **Data Validation:**
  - Validates critical fields such as Name, DOB, PFN, DOJ number, admission date, and hearing date using hospital ODS data.
  - Displays mismatches and logs them for review.

- **Document Management:**
  - Adds cover pages using local templates selected by the user.
  - Converts Word reports to PDF using naming conventions.
  - Saves PDFs and Word documents to a synced SharePoint folder (via OneDrive).

- **Configuration Handling:**
  - On first launch, EZLogger opens the Configuration Editor to collect and save local paths needed by the application.
  - These include locations of templates, doctor lists, and SharePoint sync folders.

### 2.2 Configuration Editor (One-Time Setup)

- **First-Time Launch:**
  - Automatically starts when EZLogger is used for the first time.
  - Guides the analyst through setting up essential paths required for day-to-day use.

- **What It Configures:**
  - Local folder for the SharePoint sync (used to save finalized documents).
  - Path to the doctor list file.
  - Any other file locations the app needs to read from or write to.

- **Design:**
  - Form-based interface embedded within EZLogger.
  - Simplified and user-friendly—no direct JSON editing required.
  - Saves all settings into `local_user_config.json` or `global_config.json`.

---

## 3. User Interface

- **Ribbon Integration:**
  - EZLogger adds a custom tab in Word’s ribbon, where users can access validation, PDF conversion, and save functions.

- **Embedded Views:**
  - Uses WPF UserControls hosted in WinForms ElementHost containers for seamless integration with Word.
  - Includes panels for Report Type, Author selection, and data previews.

- **Config Editor:**
  - Launches in a guided dialog format.
  - Closes automatically when setup is complete and won’t appear again unless manually triggered.

---

## 4. Security and Permissions

- **User Role:**
  - All features are designed for analysts. No role-based permissions are enforced beyond what Windows/SharePoint already apply.

- **Local File Handling:**
  - Configuration settings are saved locally per user.
  - No remote config server or shared editing—each analyst sets up their own environment.

---

## 5. Performance Requirements

- **Startup Performance:**
  - Config Editor must load instantly on first launch.
  - Regular EZLogger operations should not noticeably slow down Word.

- **File Access:**
  - Reads and writes from/to the synced SharePoint folder must handle potential OneDrive sync delays gracefully.

---

## 6. Deployment and Maintenance

- **Installation:**
  - EZLogger is distributed as a VSTO add-in.
  - Configuration setup is automatic on first use, minimizing IT involvement.

- **Maintenance:**
  - Once the config is set, it typically doesn't need to be edited again.
  - No ongoing maintenance is expected unless file paths or sync folders change.

---

This updated PRD reflects the real-world flow of how analysts use EZLogger, including the one-time setup for file paths and SharePoint sync, and clarifies that the Configuration Editor is a built-in part of the experience.
