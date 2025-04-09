## 📘 Plan: Pull Patient Data from SQLite and Display in VB.NET

> See Unreleased section of CHANGELOG.md for features planned to be released



### Summary

For EZLogger to be able to use templates, and save copies of reports to SharePoint, the user needs a local synced folder from the SharePoint Documents Library. In order for the code to use that path to save documents to the library, we need a `local_user_config.json` file. So one of the first things EZLogger needs to do is check that those files and directories exist so it can use them. The code will check the root directory of the exe file for the `global_config.json` file where it will find where the local user's file should be. The global config file will be there even if the local config file is not. So with helpers we would need to check for the `global_config.json` file first, then get the filepath to the user's OneDrive Documents path where they should have a `.ezlogger` hidden directory with the `local_user_config.json` file is located. These measures may seem confusing, but they are an extra layer of security so that files are not openly exposed to hackers if they get into a users computer. In production EZLogger should have a sign in or it should force the Office login to be triggered. Until Azure login is approved, we will just use a mock-version of the login screen where we save a session "cookie" in the `.ezlogger` directory called `session.json` to keep the user logged in for the day instead of needing to log in with every use. So this pretty well seals up security issues with private data. I suggest using the existing CoRTReport24 login user/pass since it's already in a database.

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
