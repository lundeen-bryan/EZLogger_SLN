# SaveFileView - EZLogger - DesignDoc

**Design Document (First Draft - Revised per Feedback)**
_Last updated: 2025-04-26_

---

## 1. Purpose

The `SaveFileView` in EZLogger replicates and improves upon the functionality of the legacy EZLogger VBA Save form.
It allows the user to **Move** or **Copy** the active Word document into a structured folder system based on patient information, report type, and report date.

The feature ensures all forensic reports follow a consistent file naming and storage convention, enabling easier search, retrieval, and SharePoint integration.

---

## 2. User Workflow Overview

1. The user fills out required fields (patient information, report type, report date).
2. The user selects either:
   - **Move** (relocate and delete the old file), or
   - **Copy** (create a new file and leave the original untouched).
3. The user clicks **ShowPathBtn**:
   - System generates the expected save path and filename.
   - Displays the generated path in `NewFileNameTxtBlk`.
   - Copies the folder name (patient name) to clipboard for ease of navigation.
4. The user clicks **SaveBtn**:
   - System opens a **Save As** dialog, prefilled with the generated filename and folder.
   - If user confirms save:
     - New file is saved.
     - (Move only) Old file is deleted if possible.
     - Document properties are updated (e.g., Unique ID).
     - Confirmation message is shown.
   - If user cancels Save As:
     - No further action is taken.

---

## 3. UI Components

| Control Name         | Purpose |
|:---------------------|:--------|
| `MoveOptBtn`          | Selects Move operation. |
| `CopyOptBtn`          | Selects Copy operation. |
| `ShowPathBtn`         | Generates and displays the proposed full save path and filename. |
| `SaveBtn`             | Opens Save As dialog to complete the save process. |
| `NewFileNameTxtBlk`   | Displays the generated full save path. |
| `ReportTypeCbo`       | User selects the report type (preloaded from config). |
| `ReportDateTxt`       | User enters or selects report date. |
| `PatientNameTxtBlk`   | Displays patient's name pulled from document properties. |

*Note*: Control abbreviations follow standard: `Txt`, `Cbo`, `Btn`, `Opt`, `TxtBlk`.

---

## 4. Configuration Dependencies

SaveFileView requires valid paths loaded from `local_user_config.json`:

| Config Key | Purpose |
|:-----------|:--------|
| `sp_filepath.user_forensic_library` | Root folder for **Copy** operation. |
| `sp_filepath.all_penal_codes`        | Root folder for **Move** operation. |

If these values are missing:
- Warn the user to recreate their config file.
- Prevent proceeding with Save operation.

---

## 5. Technical Behavior

### 5.1 Preloading Behavior

- On form load:
  - Read `local_user_config.json`.
  - Store move and copy root paths in memory.

### 5.2 ShowPathBtn Behavior

- Validate that either Move or Copy option is selected.
- Build the full save path using:
  - `<Root Path>\\<First Letter of Patient Lastname>\\<Patient Name> <Report Type> <Report Date>.docx`
- Display the path in `NewFileNameTxtBlk`.
- Copy the folder name (patient name) to clipboard using `ClipboardHelper`.

### 5.3 SaveBtn Behavior

- Validate that either Move or Copy option is selected.
- Open **Save As** dialog:
  - Initial folder set to root path (Move/Copy path).
  - Filename pre-filled.
- If user saves:
  - Save the file.
  - (If Move) Attempt to delete original file.
  - Update Word document properties (e.g., Unique ID).
  - Show confirmation message (different for Move vs Copy).
- If user cancels:
  - Exit quietly with no action.

---

## 6. Filename Generation

Generated filename format:
`<Patient Name> <Report Type> <Report Date>.docx`

Example:
```
Smith, John Progress Report 2025-04-26.docx
```

- **Patient Name**: Taken from Word document property `Patient Name`.
- **Report Type**: Selected from `ReportTypeCbo`.
- **Report Date**: Entered by user in `ReportDateTxt`.
- **Extension**: Always `.docx` (auto-correct `.doc` if needed).

---

## 7. Error Handling

| Situation | Handling |
|:----------|:---------|
| Move/Copy option not selected | Show warning message. Prevent further steps. |
| Paths missing from config | Show critical warning. User must recreate config. |
| Old file locked and cannot delete | Show warning but allow new file to remain saved. |
| Save As canceled | Exit quietly with no action. |
| Clipboard copy fails | Log internal error; allow user to continue. |

---

## 8. Dependencies

- `ClipboardHelper` module: For copying patient folder names to clipboard.
- `ConfigPathHelper` module: For loading local config paths.
- `DocumentPropertyHelper` module: For reading and writing Word document properties.
- `CustomMsgBox` control: For displaying user-friendly messages.

---

## 9. Future Enhancements

None planned for MVP.

Autosave and save logs will **not** be implemented. Dynamic folder picking is supported inherently through the Save As dialog.

---

# End of Document