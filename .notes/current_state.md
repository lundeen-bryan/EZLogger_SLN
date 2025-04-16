## 📘 Plan: Pull Patient Data from SQLite and Display in VB.NET

### 🔹 Goal
Retrieve patient data from a local SQLite database using the patient number entered into a WPF form (`TextBoxPatientNumber`). As a test, display the patient’s full name and total record count using a `MessageBox`.

---

### 🔹 Project Context
- **Application type:** VB.NET VSTO Add-in for Microsoft Word
- **UI:** WPF UserControl hosted inside a WinForms custom task pane
- **Database:** SQLite file (hardcoded for now, will load from JSON config later)
- **Previous implementation:** Used Word Mail Merge UI (now deprecated in favor of direct DB access)

---

### 👉 Components to Build or Update

#### 1. **Patient Model (New or Existing)**
A class representing a patient record from the `EZL` table.

```vbnet
Public Class Patient
    Public Property PatientNumber As String
    Public Property FullName As String
    Public Property County As String
    ' Add more fields as needed later
End Class
```

---

#### 2. **Database Helper Method (New)**

Add a function to your existing `DatabaseHelper.vb` to query SQLite:

```vbnet
Public Function GetPatientByNumber(patientNumber As String) As Patient
```

- Connect to `ezlogger.db`
- Run `SELECT * FROM EZL WHERE patient_number = ?`
- Map result to a `Patient` object
- Return the `Patient`, or `Nothing` if not found

---

#### 3. **ReportWizardPanel.xaml.vb Button Logic**

In the `LookupDatabase_Click` method:
- Call `DatabaseHelper.GetPatientByNumber(...)`
- Show the patient’s name and row count in a `MessageBox`
- Skip Mail Merge entirely for now

Example:

```vbnet
Dim patient = db.GetPatientByNumber(patientNumber)
If patient IsNot Nothing Then
    MessageBox.Show($"Name: {patient.FullName}")
Else
    MessageBox.Show("Patient not found.")
End If
```

---

### 🗙 Code Cleanup (After Testing)

Once confirmed:
- Remove `MailMerge.OpenDataSource(...)`
- Delete any references to `MailMergeRecipientsEditList`
- Archive or comment out the `WriteMailMergeDataToDocProperties()` logic (if not repurposed)

---

### ✅ Result

After this task is complete, we will have:
- A working, testable SQLite connection
- A verified path from patient number to retrieved patient name
- A simplified, future-proof structure for document automation

<!-- @nested-tags:prd -->