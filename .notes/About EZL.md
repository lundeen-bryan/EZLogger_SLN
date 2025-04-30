# ABOUT EZL DATABASE ON CORTREPORT24

This is the main database where EZLogger pulls it data to display to the user. Use the following query to create it:

```sql
USE CoRTReport24;
GO

IF OBJECT_ID(
	'CoRTReport24.dbo.EZL', 'U'
) IS NOT NULL
DROP TABLE dbo.EZL_CTN;

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
INTO dbo.EZL
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

The table will be created without a primary key. So the next statement creates the primary key:

```sql
-- Again you have to use a separate query to get this to work not
-- working in SQL Notebook
USE CoRTReport24;
GO

ALTER TABLE dbo.EZL
ADD EZID INT IDENTITY(1,1) PRIMARY KEY;

```

Pull the data to see if it all comes in:

```ini
SELECT [Commitment]
      ,[Admission]
      ,[Expiration]
      ,[DOB]
      ,[Name]
      ,[FullName]
      ,[PatientNumber]
      ,[Lname]
      ,[Fname]
      ,[Mname]
      ,[Location]
      ,[Program]
      ,[Unit]
      ,[Class]
      ,[CII]
      ,[Gender]
      ,[County]
      ,[Psychiatrist]
      ,[Language]
      ,[Evaluator]
      ,[EZID]
  FROM [CoRTReport24].[dbo].[EZL]
```