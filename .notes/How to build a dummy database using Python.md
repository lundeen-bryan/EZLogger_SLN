# How to Build a Dummy Database Using Python

Creating a realistic dummy database is incredibly useful for testing your application logic before live data is available. In this guide, we'll walk through how to generate structured, meaningful data using Python. Specifically, we will use the structure from the EZLogger project—a forensic reporting tool built using VB.NET and WPF—as our example.

We'll focus on four tables:
- `EZL`: the main patient table
- `EZL_IST`: report cycle status for PC1370 patients
- `EZL_NTS`: patient notes
- `EZL_PRC`: processed report metadata

## Step 1: Set Up Your Tools

You'll need the following Python libraries:
```python
import sqlite3
import random
from datetime import datetime, timedelta
import uuid
```
These are built-in libraries, so no installation is necessary.

## Step 2: Define Helper Functions
Use helper functions to generate random but realistic values.
```python
def random_date(start, end):
    return start + timedelta(days=random.randint(0, (end - start).days))

def recent_date(days_back=45):
    return datetime.today() - timedelta(days=random.randint(0, days_back))

def name_with_suffix(suffixes):
    first_names = ['Alex', 'Jamie', 'Morgan', 'Jordan', 'Taylor']
    last_names = ['Smith', 'Jones', 'Brown', 'Lee', 'Garcia']
    return f"{random.choice(first_names)} {random.choice(last_names)}, {random.choice(suffixes)}"

def court_number():
    year = random.randint(20, 24)
    return f"{year}CR{random.randint(10000, 99999)}"

def generate_patient_number():
    prefix = random.choice(["218", "219"])
    mid = f"{random.randint(0, 999):03}"
    suffix = f"{random.randint(0, 9)}"
    return f"{prefix}{mid}-{suffix}"
```

## Step 3: Define the Database Schema
Create your tables with SQLite.
```python
conn = sqlite3.connect("ezlogger.db")
cursor = conn.cursor()

cursor.executescript("""
DROP TABLE IF EXISTS EZL;
DROP TABLE IF EXISTS EZL_IST;
DROP TABLE IF EXISTS EZL_NTS;
DROP TABLE IF EXISTS EZL_PRC;

CREATE TABLE EZL (
    patient_number TEXT PRIMARY KEY,
    commitment_date TEXT,
    admission_date TEXT,
    expiration TEXT,
    dob TEXT,
    fullname TEXT,
    lname TEXT,
    fname TEXT,
    mname TEXT,
    bed_status TEXT,
    p TEXT,
    u TEXT,
    class TEXT,
    county TEXT,
    language TEXT,
    assigned_to TEXT,
    revoke_date TEXT,
    court_numbers TEXT,
    department TEXT
);

CREATE TABLE EZL_IST (
    patient_number TEXT,
    report_cycle TEXT,
    current_due_date TEXT,
    evaluator TEXT,
    ninety_days_from_admit TEXT,
    nine_from_admit TEXT,
    fifteen_from_admit TEXT,
    twenty_one_from_admit TEXT,
    final_report_due TEXT,
    dual_status TEXT,
    comment TEXT
);

CREATE TABLE EZL_NTS (
    patient_number TEXT,
    comment_date TEXT,
    category TEXT,
    comment TEXT
);

CREATE TABLE EZL_PRC (
    patient_number TEXT,
    filename TEXT,
    fullname TEXT,
    due_date TEXT,
    rush_status TEXT,
    report_date TEXT,
    report_type TEXT,
    report_cycle TEXT,
    county TEXT,
    class TEXT,
    evaluator TEXT,
    approved_by TEXT,
    processed_by TEXT,
    program TEXT,
    unit TEXT,
    days_since_due TEXT,
    commitment TEXT,
    admission TEXT,
    expiration TEXT,
    court_numbers TEXT,
    charges TEXT,
    sex TEXT,
    dob TEXT,
    age TEXT,
    language TEXT,
    pages TEXT,
    psychiatrist TEXT,
    unique_id TEXT,
    malingering TEXT,
    imo TEXT,
    jbct TEXT,
    tcar_date TEXT,
    days_since_tcar TEXT
);
""")
```

## Step 4: Generate and Insert Dummy Data
Loop through and populate all four tables with about 100 entries, following real-world constraints (e.g., class-specific rules, unique patient numbers, realistic cycles).

Key logic includes:
- Making sure `EZL` has unique `patient_number` values only.
- Populating `EZL_IST` **only** for patients with class `PC1370`, with one row per report cycle (90-day, 9-month, etc.).
- Creating multiple comments per patient in `EZL_NTS`, with date and category.
- Filling `EZL_PRC` with randomized metadata for each processed report.

## Step 5: Save and Reuse
Once inserted, save your `.db` file. You can now test your front-end or backend code with realistic content.

To reuse the process:
- Adjust the count (e.g., generate 1,000 rows instead of 100).
- Tweak helper functions to reflect updated data needs.
- Reuse the schema and insert logic.

## Conclusion
Building a dummy database in Python is not only doable but incredibly flexible. You can control every detail—from names and dates to domain-specific logic like report cycles or court departments. For the EZLogger project, this method allows you to rapidly test features and simulate edge cases without needing real patient data.

When you're ready to expand your app, you'll already have data models and logic that reflect the real-world use case.

---

Need help generating a custom version? Just tweak the helper functions or table structures as your project grows!

