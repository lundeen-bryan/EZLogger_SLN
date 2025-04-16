# EZLogger MVP Checklist

Target MVP Deadline: **May 1st**

---

## ✅ Core Functionality

### 1. Patient Data Handling

- [X] Extract patient number from active Word document footer
- [X] Query SQLite (`EZL` table) for matching patient record
- [X] Load patient data into form (e.g., `PatientInfoView`)
- [X] Allow manual edits if needed
- [X] Write patient fields to Word custom document properties

### 2. Due Date Calculation

- [X] Populate due dates based on commitment date logic
- [X] Apply special logic for PC1370 vs. non-PC1370
- [X] Ensure calendar pickers and labels update correctly
- [X] Show Early 90-Day label when appropriate

### 3. Export to PDF

- [ ] Generate PDF from active Word document
- [ ] Save PDF to local folder defined in config
- [ ] Handle overwrite or filename conflicts gracefully

### 4. Logging to SQLite

- [ ] Insert processed report entries into `Log` table
- [ ] Include: patient number, evaluator, report type, timestamp, PDF filename
- [ ] Display logged records in `LogThisView`

### 5. Configuration Integration

- [X] Load global config (`global_config.json`) for report types and other settings
- [X] Load user config (`local_user_config.json`) for paths and user-specific values
- [ ] Use config to populate ComboBoxes (e.g., report types, doctor list)

### 6. Core UI & Workflow

- [ ] Ensure all major views are functional: `ReportWizardPanel`, `ReportTypeView`, `PatientInfoView`
- [ ] Wire all buttons to handlers (even if using placeholder logic)
- [X] Confirm `CustomMsgBox` works with OK / Yes / No options
- [ ] Confirm everything works inside the Word task pane (WinForms ElementHost)

---

## 🟨 Deferred / Post-MVP Features

- [ ] SharePoint sync and metadata upload
- [ ] Full config editor interface
- [ ] Authentication / login system
- [ ] Formal unit testing
- [ ] Advanced error handling and logging

---

### Notes:

- Focus is on delivering a working workflow, not polishing UI or writing all helper tests yet.
- PDF export and due date calculator are considered **core features**, not optional.
- Logs must be written directly to SQLite, not to JSON files.

<!-- @nested-tags:prd -->