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
