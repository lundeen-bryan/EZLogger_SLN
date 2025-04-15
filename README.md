# EZLogger v0.0.1

EZLogger is an Office Add-In developed using Visual Studio Tools for Office (VSTO) in VB.NET. The add-in integrates with Microsoft Word to support forensic report workflows, including patient lookup, due date automation, database tasks, and SharePoint integration.

## 🧩 Project Overview

EZLogger is designed for analysts who review and finalize court-related forensic reports submitted by doctors. It provides tools to streamline repetitive document tasks and automate interaction with synced SharePoint folders, patient databases, and document metadata.

Key goals include:
- Reduce manual data entry in Word reports
- Log report processing steps
- Improve accuracy by pulling patient data directly from a database
- Automate due date calculations for PC1370 reports
- Enable task management for pending and completed reports

## 🏗 Architecture & Design Philosophy

EZLogger was initially built following a full MVVM (Model-View-ViewModel) pattern to take advantage of WPF's data binding features. However, during development, I realized that full MVVM is impractical in a VSTO environment due to the lifecycle and limitations of WPF controls hosted inside `ElementHost` containers. As a result, I adopted a hybrid approach:

- Views are still written in WPF and XAML.
- Logic is delegated to handler classes (e.g., `ReportWizardHandler.vb`).
- Buttons are wired using `.xaml.vb` files with event-driven code.
- Helpers provide shared logic for config loading, Word automation, and database access.

This pragmatic design ensures clean separation of concerns where possible, while accepting VSTO's constraints.

### 🔖 Project Structure

```plaintext
EZLogger_SLN/
├── Views/               # WPF UserControls for each UI screen
├── Handlers/            # Event logic for each screen (e.g., ReportWizardHandler)
├── Helpers/             # Reusable utilities (e.g., DocumentPropertyHelper, ConfigHelper)
├── HostForms/           # WinForms containers for WPF screens
├── data/                # SQLite database for local patient record lookups
├── .ezlogger/           # Local user data and logs (e.g., processed reports JSON)
```

## ✅ Key Features

- Retrieve patient data from a SQLite database based on patient number in Word footer
- Write patient details into Word custom document properties for SharePoint search
- Automate due date calculations for PC1370 reports
- Load ComboBox values from config files (JSON)
- Log processed reports to a user-specific log file for review
- Maintain analyst notes for follow-up cases
- Works seamlessly with SharePoint Document Libraries synced via OneDrive

## 🖥 Requirements

To build or run the EZLogger add-in:
- **Microsoft Word** (Office 365 or Office 2019)
- **Microsoft Visual Studio 2022**
- **.NET Framework 4.7.2 or higher**
- **VSTO (Visual Studio Tools for Office)**

## 🚀 Setup Instructions

1. **Clone the Repository**

   ```bash
   git clone https://github.com/lundeen-bryan/EZLogger_SLN.git
   ```

   Or download the ZIP and extract locally.

2. **Open the Project in Visual Studio**

   - Open `EZLogger_SLN.sln` in Visual Studio 2022.
   - Ensure Microsoft Word is installed on your system.

3. **Restore NuGet Packages**

   - Go to **Tools > NuGet Package Manager > Restore NuGet Packages** if any are missing.

4. **Build and Debug**

   - Press **F5** to launch Word with the add-in attached.

5. **Deploy**

   - For production, package using ClickOnce or Office deployment methods.

## 🧠 Why VB.NET and VSTO?

I chose VB.NET and VSTO for the following reasons:
- The original system was built in VBA, making VB.NET a natural progression.
- VSTO provides direct, stable access to Word's object model.
- Many hospital systems still run Office desktop apps, making VSTO-based add-ins viable.

## 📌 Versioning Strategy

I follow semantic versioning:

- **0.x.y**: Development builds
- **1.0.0+**: Production release
  - MAJOR: Breaking changes
  - MINOR: Backward-compatible features
  - PATCH: Bug fixes

### Implementation Date

Versioning began with v0.0.1 on 2025-04-08.

## 🤝 Contributing

I'm currently the sole developer, but contributions are welcome. To contribute:
1. Fork the repository
2. Make your changes
3. Submit a pull request with a clear explanation

## 📝 Changelog Strategy

I update the `CHANGELOG.md` alongside feature or bug fix commits. Git commit messages help populate the changelog and can be exported for review.

## 🪪 License

This project is licensed under a custom license for non-commercial use only. Viewing and modifying the code for educational or internal development purposes is permitted. **Commercial use, resale, or redistribution is prohibited** without written permission.

See the full license terms in [LICENSE.txt](LICENSE.txt).

## 📬 Contact

For questions or bug reports, use GitHub Issues or email [lundeen-bryan@github].

