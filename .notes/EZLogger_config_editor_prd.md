# Product Requirements Document: EZLogger Config Editor

## Overview
This document outlines the requirements for a WPF-based configuration editor in the EZLogger VSTO Word Add-in. The editor enables users to load, view, and edit selected parts of a configuration JSON file (either global or user-specific) without needing to interact with the raw JSON syntax.

---

## Objective
To allow non-technical users to manage specific editable sections of the EZLogger configuration file via a tab-based form UI. The editor should support full CRUD for allowed sections and defer all other sections to developer control.

---

## Scope
- Load a single config file (`global_config.json` or `user_config.json`) from a fixed location during testing
- Show tabs to separate user config from global config
- Allow users to add, update, and delete entries in editable sections
- Save all changes to the loaded JSON file only when the **Save** button is clicked

---

## File Handling
- **Default save/load location (prototype)**: `./temp/`
- Only one config file is open at a time
- Load the appropriate file based on which button is clicked (e.g. "Edit Global Config", "Edit User Config")
- No need for manual file browsing
- No support for export/backup

---

## UI Design
- **Tabbed interface**
  - One tab for **User Config Settings** (from `user_config.json`)
  - Multiple tabs for **Global Config Sections** (from `global_config.json`)

### User Config Tab
- The "User Config" tab contains:
  - Fields for selecting:
    - Local document save folder
    - Local templates folder
    - Path to the global config file
  - Use folder/file dialogs for path selection
  - Settings are saved only when **Save** is clicked

### Global Config Tabs
- **Editable Sections (visible to user):**
  - `county_alerts` (key = county name, value = alert message)
  - `email_list` (subkey: `secretaries` — editable as email list)
  - `Alerts` (key = patient number, value = alert text)
- **Non-editable Sections (hidden or read-only):**
  - `edo_filepath`, `cdo_filepath`
  - `Weekly`
  - `listbox` (includes `report_type`, `opinions`, `cover_pages`)
  - `log_files`, `log_files_status_bar`
  - `version`

---

## Editing Behavior
- Display section content using intuitive controls (e.g. TextBoxes, ListBoxes, or multi-line `key=value` input areas)
- `email_list.secretaries` may use a ListBox with Add/Remove buttons
- `county_alerts` and `Alerts` may use a multi-line editor with delimiter support (`key=value` per line)
- User can:
  - Add a new entry
  - Edit an existing entry
  - Delete an entry
- **Validation:**
  - Keys must be unique within each section
  - Trim whitespace from keys and values
  - Do not enforce strict value formats (e.g., allow free text, even if email is malformed)

---

## Save Behavior
- No auto-saving
- All changes are only written to file when user clicks the **Save** button
- Closing the form without saving will discard unsaved changes
- No undo/reset functionality

---

## Future Enhancements (Out of Scope)
- Support for switching between environments (dev vs production)
- Config section templating or schema validation
- Import/export or version history
- Dynamic merging of user config into global config

---

## Developer Notes
- Use `System.Text.Json` to load/save the config file
- Consider wrapping config access in a `ConfigManager` class for easier maintenance
- During prototype phase, hardcode path to config file in `./temp/`
- Ensure parser skips `_comment` fields when serializing/deserializing config sections

---

## Success Criteria
- User can open either config file using the appropriate button
- Editable sections are displayed with usable inputs
- Users can successfully add/edit/delete values
- File is only changed when saved
- No crashes or data corruption on malformed or partial input

