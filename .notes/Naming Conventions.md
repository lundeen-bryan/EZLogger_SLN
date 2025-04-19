# EZLogger Naming Conventions Cheat Sheet

This cheat sheet documents the naming conventions used throughout the EZLogger project, with a focus on readability, consistency, and domain-driven clarity. It reflects a hybrid between practical coding standards and a lightweight, context-first adaptation of Hungarian notation.

---

## 🧠 General Rule
> Use **ContextFirst + ControlTypeSuffix**

This groups controls and variables by their functional domain (e.g., "NinetyDay", "Patient", "ReportCycle"), and uses a brief suffix to indicate the UI control type.

---

## 🔤 Suffix Legend for UI Controls

| Control Type     | Suffix | Example              |
|------------------|--------|----------------------|
| Button           | `Btn`  | `SaveReportBtn`      |
| TextBox          | `Txt`  | `PatientNameTxt`     |
| Label            | `Lbl`  | `NinetyDayDueLbl`    |
| RadioButton      | `Rdo`  | `NinetyDayRdo`       |
| CheckBox         | `Chk`  | `NeedsReviewChk`     |
| ComboBox         | `Cbo`  | `ReportTypeCbo`      |
| DatePicker       | `Pick` | `AdmissionDatePick`  |
| ListBox          | `Lbx`  | `CoverPagesLbx`      |
| Grid/DataGrid    | `Grid` | `PatientDataGrid`    |

---

## ✅ Best Practices

- **Context is King**: Name based on the logical domain or function (e.g., `NinetyDay`, `AssignedTo`, `Classification`).
- **Group by Functionality**: Controls that work together should share a common prefix (`NinetyDayRdo`, `NinetyDayLbl`, `NinetyDayPick`).
- **Be Brief, but Clear**: Avoid over-describing (e.g., use `AssignedToCbo` instead of `PatientAssignedToComboBox`).
- **Avoid Vague Names**: Stay away from `Button1`, `TextBox2`, or similar.
- **Keep IntelliSense Useful**: Typing a prefix like `NinetyDay` should narrow down all related controls.

---

## 🛠️ Event Handler Naming (Optional Convention)

| Handler Type         | Pattern                         | Example                          |
|----------------------|----------------------------------|----------------------------------|
| Button Click         | `Handle[Context]BtnClick`       | `HandleSaveReportBtnClick()`     |
| Radio Select         | `Handle[Context]RdoChecked`     | `HandleClassificationRdoChecked()` |
| ComboBox Changed     | `Handle[Context]CboChanged`     | `HandleReportTypeCboChanged()`   |

---

## 💬 Notes
- These conventions are optimized for a WPF-over-WinForms hybrid architecture.
- Form host files should always pass `Me` when instantiating WPF views, e.g.:

```vb
Dim view As New ReportTypeView(Me)
ElementHost1.Child = view
```

- Data models (e.g., `PatientCls`) do not require these suffixes — use standard PascalCase properties.

---

## 📦 Folder Naming Tips (Optional)

| Purpose          | Suggested Folder Name |
|------------------|------------------------|
| Views (WPF)      | `Views`                |
| Hosts (WinForms) | `HostForms`            |
| Logic Handlers   | `Handlers`             |
| Shared Helpers   | `Helpers`              |
| Config Data      | `config` or `data`     |
| Documentation    | `.notes`               |

---

This sheet can evolve as the app grows. Update as new patterns emerge!

