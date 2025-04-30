# THE PLAN TO IMPLEMENT DATA FOR THE APP

## DUMMY DATABASE

The plan is to create a dummy database that holds the fields we would want to see in the final product. This will be a SQLite database. It won't matter much what columns are there as long as we generally know that the data can be pulled from real life servers at some point. Essentially this will mockup the spreadsheets and columns in the existing Excel database and will be a real database file in SQLite. Only one file to manage. Later when migrating to the real life server, we will have to use SQL to create the tables we need.

### TABLES AND COLUMNS

__EZL__

This is the main EZ Logger table and will hold the following columns.

- commitment_date
- admission_date
- expiration
- dob
- fullname
- patient_number
- Lname
- Fname
- Mname
- bed_status
- p
- u
- class
- county
- language
- assigned_to
- revoke_date
_ court_numbers
- department

__EZL_IST__

This holds the IST only patients.

- patient_number
- p
- u
- fullname
- commitment_date
- admission_date
- report_cycle
- current_due_date
- psychiatrist
- evaluator
- ninety_days_from_admit
- nine_mos_from_admit
- fifteen_from_admit
- twenty_one_from_admit
- final_report_due
- county
- sex
- dob
- dual_status
- discharge_status
- comment (current due date cycle comment)

__Notifications__

- patient_number
- comment_date
- category
- comment

### OTHER NOTABLE DATABASES TO LOG TO

We also log details to databases. Currently these are stored as .ini files and could remain that way, but SQL would be better

not_sent (meaning we don't know the HLV when the report is processed)
tcars (log of the tcars completed)
typo_log
processed_report.ini (this is a simple list of reports processed)

Use the following database to show all the fields needed for the EZL table

```SQL
SELECT
		[Commitment] = CPS.Rpt_LC_Start_Date
	, [Admission] = ADM.Admission_Date
	, [Expiration] = CPS.Rpt_LC_End_Date
	, [DOB] = ADM.DOB
	,	[Name] = CPS.Patient_Lastname + ', ' + CPS.Patient_Firstname
	,	[FullName] = IIF(
			CPS.Patient_Middlename != NULL,
			CPS.Patient_Lastname + ', ' + CPS.Patient_Firstname + ' ' + CPS.Patient_Middlename,
			CPS.Patient_Lastname + ', ' + CPS.Patient_Firstname
		)
	, [PatientNumber] = CPS.Case_Number
	, [Lname] = CPS.Patient_Lastname
	, [Fname] = CPS.Patient_Firstname
	, [Mname] = CPS.Patient_Middlename
	, [Location] = CPS.Status_Text
	, [Program]	= CASE
			WHEN CPS.[Program] = 'V' THEN '5'
			WHEN CPS.[Program] = 'IV' THEN '4'
			WHEN CPS.[Program] = 'III' THEN '3'
			WHEN CPS.[Program] = 'II' THEN '2'
			ELSE '1'
		END
	, [Unit] = CPS.Unit
	, [Class] = CPS.Rpt_Legal_Class_Text
	, [CII] = ADM.CII_Number
	, [Gender] = ADM.Sex
	, [County] = CPS.Rpt_LC_County_Text
	, [Psychiatrist] = CPS.Attending_Physician_Name
	, [Language] = ADM.Language
	, [Evaluator] = DRS.AssignedPhy
FROM MHNODSSQL1P.ODS.dbo.Admission AS ADM
INNER JOIN
MHNODSSQL1P.ODS.dbo.Current_Patient_Status AS CPS
ON
	ADM.ADM_Key = CPS.ADM_Key
INNER JOIN
nshsql1p.CoRTReport24.dbo.AssignedDrs AS DRS
ON
	CPS.First_Hospital_Case_Nbr = DRS.CaseNum
WHERE 1 = 1
		AND ADM.Case_Number = CPS.Case_Number
ORDER BY CPS.Patient_Lastname
```

The new data query for SQLite should look like this:

```sql
CREATE TABLE EZL (
    EZLID INTEGER PRIMARY KEY AUTOINCREMENT,
    patient_number TEXT NOT NULL, -- should be in the format of starting with number '4' followed by 7 digits. The first 3 of the 7 digits should be '219' the numbers that follow can be random
    commitment_date TEXT, -- in ISO date format
    admission_date TEXT, -- ISO date format
    expiration TEXT, -- ISO date format
    dob TEXT, -- This is the date of birth in ISO date format
    name TEXT, -- Name is only the Last name in all caps followed by ", " followed by the first name in sentence case
    fullname TEXT, -- fullname is LAST, First Middle name with the last name only in upper case but first and middle in sentence case
    lname TEXT, -- UPPER CASE
    fname TEXT, -- Proper case
    mname TEXT, -- Proper case
    location TEXT, -- would always be "RES. ON UNIT" but some can randomly be "DISCHARGED" or "OUT TO COURT"
    program TEXT, -- a number from 1 to 5
    unit TEXT, -- the letter 'T' followd by a number between 1 to 10
    classification TEXT,  -- renamed from "class"
    cii TEXT, -- I think this is an 9-digit alphanumeric identifier; can be random
    gender TEXT, -- either M for male or F for female majority are male by default
    county TEXT, -- one of 50 main counties in California always upper case
    psychiatrist TEXT, -- Lastname, Firstname always in proper case or sentence case
    language TEXT, -- majority will be English but 20% can be Spanish, followed by a few other languages of random choice but at least 90% speak english
    evaluator TEXT -- Same as psychiatrist this is Lastname, Firstname in Proper case
);

```