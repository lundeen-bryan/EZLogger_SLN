# About EZL_CTN 

The EZL_CTN stands for EZLogger Court Number so it's to track the court numbers for each patient. This could be converted to a stored procedure but for now, to see how it behaves I wrote it as a table.

```sql
WITH
Step1 AS (
		-- This CTE step extracts necessary columns from Offense Tbl that match CPS
		SELECT
		  [FirstNbr] = OFC.First_Hospital_Case_Nbr
		, [PatientNumber] = CPS.Case_Number
		, [AdmKey] = OFC.ADM_Key
		, [County] = LTRIM(RTRIM(OFC.County_Of_Commit_Text))
		, [Dept] = SUBSTRING(OFC.Court_ID, 7, LEN(OFC.Court_ID))
		, [CourtNum] = OFC.Court_Case_Nbr
		FROM
		MHNODSSQL1P.ODS.dbo.Offense AS OFC
		INNER JOIN
		MHNODSSQL1P.ODS.dbo.Current_Patient_Status AS CPS
		ON
			OFC.ADM_Key = CPS.ADM_Key
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

The next query will create a table

```sql
IF OBJECT_ID(
	'CoRTReport24.dbo.EZL_CTN', 'U'
) IS NOT NULL
DROP TABLE CoRTReport24.dbo.EZL_CTN;

WITH
Step1 AS (
		-- This CTE step extracts necessary columns from Offense Tbl that match CPS
		SELECT
		  [FirstNbr] = OFC.First_Hospital_Case_Nbr
		, [PatientNumber] = CPS.Case_Number
		, [AdmKey] = OFC.ADM_Key
		, [County] = LTRIM(RTRIM(OFC.County_Of_Commit_Text))
		, [Dept] = SUBSTRING(OFC.Court_ID, 7, LEN(OFC.Court_ID))
		, [CourtNum] = OFC.Court_Case_Nbr
		FROM
		MHNODSSQL1P.ODS.dbo.Offense AS OFC
		INNER JOIN
		MHNODSSQL1P.ODS.dbo.Current_Patient_Status AS CPS
		ON
			OFC.ADM_Key = CPS.ADM_Key
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
		, [AdmKey]
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
    B.[FirstNbr]
	, B.[PatientNumber]
	, B.[County]
	, B.[Dept]
	, C.[CourtNumber]
INTO nshsql1p.CoRTReport24.dbo.EZL_CTN
FROM BestRows AS B
LEFT JOIN CourtAgg AS C
ON B.[FirstNbr] = C.[FirstNbr]
AND B.[County] = C.[County]
ORDER BY B.[FirstNbr], B.[County]
```

Lastly, this is used to create the index for the table.

```plaintext
ALTER TABLE nshsql1p.CoRTReport24.dbo.EZL_CTN
ADD CTNID INT IDENTITY(1,1) PRIMARY KEY;
```