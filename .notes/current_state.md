# EZLogger Project Summary (As of Current Stage)

## **Project Purpose**
EZLogger is a VSTO Office Add-in developed in VB.NET using a hybrid **WinForms + WPF** architecture. It assists forensic clinicians in preparing court reports by collecting report metadata, validating document content, and preparing files for export to SharePoint and PDF.

---

## **User Interface Architecture**

### **WPF Views** (`Views\`)
- All views are **WPF UserControls**, hosted within **WinForms container forms** under `HostForms\`.
- Each view handles a specific section of the report pipeline:
  - `ReportWizardPanel` is the main control center.
  - `OpinionView`, `ReportAuthorView`, `ReportTypeView`, `ChiefApprovalView`, etc., are modular views for sub-tasks.
  - `MoveCopyView`, `CoverPageView`, `PatientInfoView` support file handling, cover pages, and patient info.

### **Host Forms** (`HostForms\`)
- Every WPF view is wrapped inside a `Form` using `ElementHost` (e.g., `MoveCopyHost`, `OpinionHost`).
- Forms are launched from the `ReportWizardPanel` using logic stored in the `Handlers\` folder.

### **Custom Controls**
- `CustomMsgBox` is a fully custom, lime-on-black message box control with Yes/No/OK options.
- A shared enum `CustomMsgBoxResult` is used to capture the user’s choice.

---

## **Code Organization**

### **Handlers (`Handlers\`)**
- Each WPF view has a corresponding handler class that contains button logic and actions (e.g., `OpinionHandler`, `MoveCopyHandler`).
- Handlers are triggered by views, passing in any necessary context (e.g., the host form).

### **Helpers (`Helpers\`)**
- `ConfigPathHelper` is used to retrieve config values like report types or file paths.
- `MessageBoxConfig` defines the data structure passed into `CustomMsgBox`.

### **Controls**
- Includes shared UI elements like `FormHeaderControl` and the `CustomMsgBoxControl`.

### **ViewModels**
- Currently includes a single `MainVM.vb`, prepared for future binding logic if needed.

---

## **Assets (`Resources\`)**
- Includes custom icons, PNGs, and theme assets used by views and toolbars (e.g., floppy.png, Wizard1.png).

---

## **Ribbon & Entry Points**
- `EZLoggerRibbon.vb` defines the add-in’s entry point from the Office Ribbon.
- `ThisAddIn.vb` handles application-level lifecycle events.

---

## **Current Completion**
- All WPF views and host forms are complete.
- Button wiring is done via `Handlers`, and multiple buttons trigger the `CustomMsgBox` as a test.
- `ReportWizardPanel` launches sub-forms from a centralized dashboard.
- The architecture is solid and modular — ready to integrate data.

---

## **Next Step: Database Integration**
The app now needs to:
- Connect to a local or portable SQLite database (or MS SQL in production)
- Load dynamic data (e.g., patient info, report types, cover page templates)
- Allow users to write data back (e.g., save file metadata, logging)
