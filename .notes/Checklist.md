# EZLogger - Legacy Workflow Reference

This document outlines the original workflow of each task pane button from the legacy version of EZLogger. A "True" checkbox indicates the logic has been translated into the VB.NET version of EZLogger, unchecked means it hasn't. 

---

## A Button – Confirm Patient Number

- [X] Reads patient number from the document footer.
- [X] Displays number in a message box for confirmation.
- [X] On “Yes,” copies the number to clipboard.
- [X] Places the number into the patient number textbox.

---

## B Button – Pull Data from Database

- [X] Pulls data from the database
- [X] Allows the user to confirm the correct patient by reviewing database entries in a msgbox
- [X] Needs to show Court Number, Expiration, Full Name, County, DOB
- [X] Confirmed patient data is loaded into the form for further processing.
- [X] Adds database data to custom document properties

---

## C Button – Select Report Type and Due Dates

- [X] Opens a form displaying report types, report date, and associated due dates.
- [X] Make sure size of all forms are the same size, color and font styles
- [X] User selects the appropriate report type.
- [ ] Selected report type and due dates are used to set scheduling and deadlines for the report.

---

## D Button – Check for Due Report in TCAR

- [ ] Checks SQL table for patient name and due date.
- [ ] Prompts user with confirmation if found.
- [ ] On “Yes,” logs:
  - Name from report
  - Report submitted date
  - Report processed date
- [ ] Appends the data to another SQL logging table.

---

## F Button – Confirm Report Opinion

- [ ] Opens opinion form with a list of preloaded opinion phrases.
- [ ] Allows user to compare phrases to report using First Page / Last Page buttons.
- [ ] User selects opinion and clicks OK.
- [ ] Saves selected opinion to Word custom document properties.

---

## G Button – Select Evaluator / Author

- [ ] Opens Evaluator and Author Information form.
- [ ] Drop-down allows autocomplete while typing.
- [ ] User selects correct author and clicks “Done Selecting Evaluator.”
- [ ] Selected name is written to Word custom document properties.

---

## H Button – Chief Approval & Signature

- [ ] Opens list of approvers (Morgan, Powers, Yang, Judd).
- [ ] User selects name and clicks “Approved By.”
- [ ] Shows confirmation message with selected approver.
- [ ] “Insert Sig” button inserts signature at current cursor location.
- [ ] “Go Back” button returns to main task pane.

---

## I Button – Rename and Save File

- [ ] Opens a Move/Copy form with textboxes and a Search button.
- [ ] Search retrieves:
  - Patient number
  - Report type
  - Report date
  - Name
  - Program
  - Unit
- [ ] Patient last and first names are copied to clipboard.
- [ ] If “Move File”:
  - Shows path
  - Opens Save As dialog
  - Moves file to selected location
- [ ] If “Copy File”:
  - Shows path
  - Opens Save As dialog
  - Copies file to new location

---

## J Button – Select Pages and Convert to PDF

- [ ] Opens form with a list of templates (A–T).
- [ ] User selects a template.
- [ ] Scroll wheel adjusts number of pages (if required).
- [ ] Click “Convert” to save as PDF in Documents folder.
- [ ] “Go Back” returns to previous menu.

---

## K Button – Finalize and Log Report

- [ ] Creates a .txt log file in the Documents folder.
- [ ] Syncs custom document properties to SharePoint document library.
- [ ] Displays final confirmation message box.

---
