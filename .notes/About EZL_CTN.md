# About EZL_CTN 

The EZL_CTN stands for EZLogger Court Number so it's to track the court numbers for each patient. This could be converted to a stored procedure but for now, to see how it behaves I wrote it as a table.

First create a table. Note that a lot of this you have to do in a regular SQL query not in SQL Notebook.

```sql
-- Put this into a sql server query because I don't have permissions to
-- add tables from SQL notebook
USE CoRTReport24;
GO

IF OBJECT_ID(
	'CoRTReport24.dbo.EZL_CTN', 'U'
) IS NOT NULL
DROP TABLE dbo.EZL_CTN;

		SELECT
		  [FirstNbr] = OFC.First_Hospital_Case_Nbr
		, [PatientNumber] = CPS.Case_Number
		, [AdmKey] = OFC.ADM_Key
		, [County] = LTRIM(RTRIM(OFC.County_Of_Commit_Text))
		, [Dept] = SUBSTRING(OFC.Court_ID, 7, LEN(OFC.Court_ID))
		, [CourtNum] = OFC.Court_Case_Nbr
		INTO dbo.EZL_CTN
		FROM
		MHNODSSQL1P.ODS.dbo.Offense AS OFC
		INNER JOIN
		MHNODSSQL1P.ODS.dbo.Current_Patient_Status AS CPS
		ON
			OFC.ADM_Key = CPS.ADM_Key
```

Next you need to add a primary key for the table

```sql
-- Again you have to use a separate query to get this to work not
-- working in SQL Notebook
USE CoRTReport24;
GO

ALTER TABLE dbo.EZL_CTN
ADD CTNID INT IDENTITY(1,1) PRIMARY KEY;
```

## Test the query to see if it shows the table correctly

```sql
SELECT * FROM CoRTReport24.dbo.EZL_CTN
```

## QUERY THE TABLE AS IF IT'S A USP BUT THIS IS JUST TO TEST IT

```sql
WITH
Step1 AS (
		-- This CTE step extracts necessary columns from a specified table
		SELECT *
		FROM nshsql1p.CoRTReport24.dbo.EZL_CTN
		WHERE 1=1 -- Optional: specify conditions
),
Step2 AS (
	-- Remove duplicates from Step1
    SELECT DISTINCT
      [FirstNbr]
			, [PatientNumber]
      , [AdmKey]
      , [County]
      , [Dept]
      , [CourtNum]
    FROM Step1
),
Step3 AS (
		-- Pick dept if it's not blank
    SELECT
      [FirstNbr]
			, [PatientNumber]
      , [AdmKey]
      , [County]
      , [Dept]
      , ROW_NUMBER() OVER(
				PARTITION BY FirstNbr, County
				ORDER BY
				  CASE WHEN Dept IS NULL OR Dept = '' THEN
					  1 ELSE 0 END, AdmKey
			) AS RowNum
    FROM Step2
),
BestRows AS (
	-- Keep only the best rows where rownum = 1
	SELECT
	  [FirstNbr]
		, [PatientNumber]
		, [County]
		, [Dept]
	FROM Step3
	WHERE RowNum = 1
),
CourtAgg AS (
	-- Aggregate the court numbers
	SELECT
	  [FirstNbr]
		, [County]
		, [CourtNumber] = STRING_AGG(CourtNum, '; ')
	FROM (
		SELECT DISTINCT FirstNbr, County, CourtNum
		FROM Step2
	) AS CourtDeduped
	GROUP BY FirstNbr, County
)
SELECT
  B.FirstNbr
	, B.PatientNumber
	, B.County
	, B.Dept
	, C.CourtNumber
FROM BestRows AS B
LEFT JOIN CourtAgg AS C
ON B.FirstNbr = C.FirstNbr
AND B.County = C.County
ORDER BY B.FirstNbr
```

## LASTLY CONVERT THAT QUERY TO A USP

```sql
-- place in a regular SQL query window not in SQL notebook
USE CoRTReport24;
GO

if exists (select 1 from sys.procedures where name = 'uspEZL_CTN')
    drop PROCEDURE [uspEZL_CTN]
GO

CREATE PROCEDURE [dbo].[uspEZL_CTN]
AS
BEGIN

WITH
Step1 AS (
		-- This CTE step extracts necessary columns from EZL_CTN
		SELECT *
		FROM nshsql1p.CoRTReport24.dbo.EZL_CTN
		WHERE 1=1
),
Step2 AS (
	-- Remove duplicates from Step1
    SELECT DISTINCT
      [FirstNbr]
			, [PatientNumber]
      , [AdmKey]
      , [County]
      , [Dept]
      , [CourtNum]
    FROM Step1
),
Step3 AS (
		-- Pick dept if it's not blank
    SELECT
      [FirstNbr]
			, [PatientNumber]
      , [AdmKey]
      , [County]
      , [Dept]
      , ROW_NUMBER() OVER(
				PARTITION BY FirstNbr, County
				ORDER BY
				  CASE WHEN Dept IS NULL OR Dept = '' THEN
					  1 ELSE 0 END, AdmKey
			) AS RowNum
    FROM Step2
),
BestRows AS (
	-- Keep only the best rows where rownum = 1
	SELECT
	  [FirstNbr]
		, [PatientNumber]
		, [County]
		, [Dept]
	FROM Step3
	WHERE RowNum = 1
),
CourtAgg AS (
	-- Aggregate the court numbers
	SELECT
	  [FirstNbr]
		, [County]
		, [CourtNumber] = STRING_AGG(CourtNum, '; ')
	FROM (
		SELECT DISTINCT FirstNbr, County, CourtNum
		FROM Step2
	) AS CourtDeduped
	GROUP BY FirstNbr, County
)
SELECT
  B.FirstNbr
	, B.PatientNumber
	, B.County
	, B.Dept
	, C.CourtNumber
FROM BestRows AS B
LEFT JOIN CourtAgg AS C
ON B.FirstNbr = C.FirstNbr
AND B.County = C.County
ORDER BY B.FirstNbr

END
GO
```