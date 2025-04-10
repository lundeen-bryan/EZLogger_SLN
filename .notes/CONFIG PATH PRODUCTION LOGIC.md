# SIDENOTE FOR FUTURE ME (PRODUCTION LOGIC)

In development mode, we use a hardcoded path to global_config.json
(defined under #If DEBUG) so we can test easily while working from source.

In production, however, VSTO add-ins are not run from the bin folder.
They are loaded by Microsoft Word into a shadow-copy folder that makes
Assembly.GetExecutingAssembly().Location and similar methods unreliable.

To handle this, we store the global_config.json path in a user-specific
file called local_user_config.json, located in:
  %UserProfile%\OneDrive - Department of State Hospitals\Documents\.ezlogger\
or a fallback location if OneDrive isn't configured.

This local_user_config.json acts like a pointer to shared files
(global config, templates, SQLite database, etc.) stored in a synced
SharePoint folder.

This method mimics the architecture of the legacy VBA version and ensures:

- Environment-independent file resolution
- Support for multiple user setups
- Safer, more maintainable configuration

TL;DR:

In dev: Load directly from repo path.

In prod: Read from local_user_config.json, which points to global_config.json.
