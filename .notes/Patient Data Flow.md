# Data Flow: Patient Info to Document Properties

This document describes how patient information is retrieved from the SQLite database and written into Word document custom properties within the EZLogger VSTO application.

## Overview

The data flow is driven by event handlers or ribbon button clicks. The flow begins with retrieving data from the database using `PatientDatabaseHandler`, and ends with writing that data into Word custom document properties using `DocumentPropertyWriter`.

## Step-by-Step Flow

1. **Patient Number Input**: The user enters or selects a patient number.
2. **Data Retrieval**: `PatientDatabaseHandler.GetPatientByNumber` is called, which queries the SQLite database (EZL table) to get patient information. A JOIN may include data from the `EZL_IST` table, such as `early_ninety_day`.
3. **Data Returned**: A `PatientCls` object is returned (or a dictionary, depending on refactoring).
4. **Property Writing**: The returned data is passed to `DocumentPropertyWriter.WriteDataToDocProperties`, which writes each field as a custom document property.

## Mermaid Diagram

```mermaid
flowchart TD
    A[User Input: Patient Number] --> B[Call PatientDatabaseHandler.GetPatientByNumber]
    B --> C[Query EZL and EZL_IST tables in SQLite]
    C --> D[Return Patient Data (e.g., PatientCls)]
    D --> E[Call DocumentPropertyWriter.WriteDataToDocProperties]
    E --> F[Write Custom Document Properties in Word]
```

## Example Tables Involved

- `EZL`: Core patient data
- `EZL_IST`: Report tracking, including `early_ninety_day`

## Next Steps

- Refactor to support direct dictionaries or anonymous objects instead of PatientCls.
- Expand the data flow to include UI elements that consume document property data.

---

*Document version: 2025-04-12*

