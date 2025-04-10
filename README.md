# EZLogger v0.0.1

EZLogger is a **VSTO Office Add-In for Microsoft Word**, built in **VB.NET** using the **WPF MVVM** pattern. It modernizes a legacy VBA tool used by forensic analysts to process reports written by doctors. The add-in integrates seamlessly into Word and provides tools for analyzing, logging, and finalizing report content.

---

## Overview

EZLogger helps analysts:
- Verify and manage **patient data** in forensic reports.
- **Compare report content** against a structured database.
- **Log activity and decisions** for audit and review.
- **Insert standardized documentation** from templates.
- **Populate Word fields** dynamically from database or config data.
- **Convert reports to PDF** with finalized content.
- Save and sync documents to a **SharePoint-backed folder**.

---

## Architecture & Key Features

- **Patient Number Recognition**
  - Extracts patient number from document footer.
  - Looks up patient info from an SQLite database.

- **Database Viewer**
  - WPF UI shows matching patient data.

- **Custom Property Writer**
  - Fills Word's custom document properties using confirmed data.

- **Bookmark Replacement & Field Unlinking**
  - Replaces bookmarks with values via a `FillBookmark` method.
  - Fields are unlinked automatically (like `Ctrl+Shift+F9`) using `UnlinkAllFields`.

- **Template Support**
  - Word templates are stored in SharePoint.
  - VB.NET inserts and fills templates without relying on Mail Merge.

- **Configuration System**
  - `local_user_config.json` (user-specific, in OneDrive Documents)
  - `global_config.json` (shared, in SharePoint)

- **Logging System**
  - Logs reports to `.ezlogger\processed_log.json`.
  - View named `LogThisView` acts like a checklist or TODO list.

- **Custom Message Box**
  - Lime green text on black background.
  - Auto-resizing and anchored to parent form.

- **Helper Modules**
  - Includes `DocumentPropertyWriter`, `ClipboardHelper`, `ConfigPathHelper`, and `SenderHelper`.

- **Folder Structure**
  - `Views`: WPF UserControls
  - `Handlers`: Per-view business logic
  - `HostForms`: WinForms hosts for embedding views
  - `Helpers`: Shared utilities and logic
  - `Data`: SQLite DB (development), possible MS SQL in production

---

## Requirements

To use EZLogger, you need:

- **Microsoft Word**: The add-in integrates with Word.
- **Visual Studio 2022**: For building/debugging.
- **.NET Framework 4.7.2 or higher**
- **Visual Studio Tools for Office (VSTO)**

---

## Setup Instructions

1. **Clone the Repository**
   ```bash
   git clone https://github.com/lundeen-bryan/EZLogger_SLN.git
   ```
   Or download as a zip.

2. **Open the Project in Visual Studio**
   - Open `EZLogger_SLN.sln` in Visual Studio 2022.
   - Ensure Word is installed for debugging.

3. **Restore NuGet Packages**
   - Go to **Tools > NuGet Package Manager > Restore NuGet Packages**.

4. **Build the Project**
   - Press **F5** to build and run in Debug mode.

5. **Deploy the Add-In**
   - Use Office deployment tools per your org's policy.

---

## Configuration File Strategy

EZLogger avoids using executable-relative paths due to VSTO’s deployment model. Instead:

- `local_user_config.json` is stored under the user’s OneDrive Documents (`Documents\.ezlogger`).
- It points to:
  - `global_config.json` (SharePoint)
  - shared templates
  - SQLite database or doctor list path

This approach ensures consistency across machines and is based on the VBA version’s config design.

---

## How to Use

After running the add-in:

- Open Microsoft Word.
- Use the **EZ Logger** tab in the ribbon.
- Launch the **Report Wizard** or open **Database Menu** tools.

---

## Forking or Cloning the Repository

1. **Fork**: Use GitHub's "Fork" button.
2. **Clone**:
   ```bash
   git clone https://github.com/lundeen-bryan/EZLogger_SLN.git
   ```
3. **Make Changes**: Submit pull requests with explanations.

---

## Versioning Strategy

- **Development Phase (0.x.y)**:
  - v0.0.1 begins active development
  - Minor = new features, Patch = fixes

- **Production Release (≥1.0.0)**:
  - v1.0.0 = first stable release
  - Follows semantic versioning

> This versioning strategy began on 2025-04-08 with v0.0.1

---

## Contributing

Contributions are welcome! Fork the repo, make your changes, and submit a pull request.

---

## License

This project is licensed under the MIT License. See [LICENSE.txt](LICENSE.txt).

---

## Contact

Questions? Issues? Reach out via GitHub Issues or email [lundeen-bryan@github].

---

## Clarifications

- **Machine learning and Python are not part of this project.**
  They may be used in a separate future tool for analyzing content.

- **Templates** are integral to EZLogger and are filled using custom logic, not Mail Merge.

