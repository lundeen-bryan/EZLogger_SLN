Imports Microsoft.Office.Interop.Word
Imports EZLogger.Helpers
Imports System.IO
Imports System.Windows.Forms

''' <summary>
''' Contains manual developer-triggered tests for insert logic.
''' This module is not called automatically and is safe to include in production builds.
''' </summary>
Public Module ManualLogTest

    ''' <summary>
    ''' Manually tests InsertPrcTable using dummy data and the active Word document.
    ''' </summary>
    Public Sub RunInsertPrcTableTest()
        Dim doc As Document = DocumentHelper.GetActiveWordDocument()
        If doc Is Nothing Then
            MessageBox.Show("No active Word document found. Please open a report and try again.",
                            "Missing Document", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim prcData As New Dictionary(Of String, Object) From {
            {"PatientNumber", "TEST1234"},
            {"FirstPatientNumber", "TESTFIRST"},
            {"Created", DateTime.UtcNow.ToString("yyyy-MM-dd")},
            {"Filename", Path.GetFileName(doc.FullName)},
            {"PatientName", "Unit Test, Case"},
            {"Name", "Unit Test"},
            {"DueDate", DBNull.Value},
            {"RushStatus", "ON TIME"},
            {"ReportDate", DBNull.Value},
            {"ReportType", "Test"},
            {"ReportCycle", "N/A"},
            {"County", "TestCounty"},
            {"Classification", "PC1026"},
            {"Evaluator", "Test Evaluator"},
            {"ApprovedBy", "Test Supervisor"},
            {"ProcessedBy", "Test User"},
            {"Program", "U"},
            {"Unit", "Alpha"},
            {"DueDateOffset", 0},
            {"Commitment", DBNull.Value},
            {"Admission", DBNull.Value},
            {"Expiration", DBNull.Value},
            {"CourtNumber", "000-TEST"},
            {"Charges", "Test Charges"},
            {"Sex", "X"},
            {"Dob", DBNull.Value},
            {"Age", "999"},
            {"Language", "English"},
            {"Pages", "1"},
            {"Psychiatrist", "Dr. Test"},
            {"UID", "UID-1234"},
            {"MinuteOrder", False},
            {"Malingering", False},
            {"IMO", False},
            {"JBCT", False},
            {"TCAR", DBNull.Value},
            {"TcarOffset", 0}
        }

        DatabaseHelper.InsertPrcTable(prcData)
    End Sub

End Module
