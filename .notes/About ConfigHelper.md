# ConfigHelper Module

## 1. Overview
**Purpose**:
The `ConfigHelper` module manages the reading, writing, and validation of EZLogger configuration files. It ensures the existence of the local config (`local_user_config.json`), allows the user to select a global config file, and retrieves paths or settings needed throughout the application. Configuration files are essentially the settings that show EZLogger where to find file paths or lists that are presented in ComboBoxes, ListBoxes, etc. 

This enables centralized management of file paths like the doctors list, template folder, and the SharePoint-synced global config.

---

## 2. Components Involved

| Type        | Name                               | Description                                       |
|-------------|------------------------------------|---------------------------------------------------|
| Module      | `ConfigHelper.vb`                  | Contains all config-related helper functions      |
| Config File | `%USERPROFILE%\.ezlogger\local_user_config.json` | Stores user-specific paths/settings              |
| Config File | `global_config.json`               | Shared config file synced via SharePoint          |
| UI Element  | `OpenFileDialog`                   | Used to select the global config file             |

---

## 3. Code Snippets

### GetLocalConfigPath

```vb.net
Public Function GetLocalConfigPath() As String
    Return EnsureLocalUserConfigFileExists()
End Function
```

### UpdateLocalConfigWithGlobalPath

```vb.net
Public Sub UpdateLocalConfigWithGlobalPath(globalConfigPath As String)
    ' Reads local_user_config.json, updates sp_filepath.global_config_file with new value
    ' Writes updated JSON back to disk
End Sub
```

### PromptForGlobalConfigFile

```vb.net
Public Function PromptForGlobalConfigFile() As String
    ' Shows OpenFileDialog for selecting global_config.json
    ' Returns selected path or empty string
End Function
```

### EnsureLocalUserConfigFileExists

```vb.net
Public Function EnsureLocalUserConfigFileExists() As String
    ' Ensures the .ezlogger folder and local_user_config.json file exist
    ' Returns the full path to the config file
End Function
```

### GetGlobalConfigPath

```vb.net
Public Function GetGlobalConfigPath() As String
    ' Reads sp_filepath.global_config_file from local_user_config.json
    ' Returns path or shows error if not found
End Function
```

---

## 4. Execution Flow

1. On application startup or config-dependent action, `GetLocalConfigPath()` is called.
2. If the config file does not exist, it is created with default values via `EnsureLocalUserConfigFileExists`.
3. If the global config file is missing, the user is prompted to select one with `PromptForGlobalConfigFile`.
4. The selected path is saved to `local_user_config.json` via `UpdateLocalConfigWithGlobalPath`.
5. When needed, the saved path is read using `GetGlobalConfigPath`.

---

## 5. Sample Input / Output

**local_user_config.json**

```json
{
  "status": "created",
  "created_at": "2025-04-17T21:01:00",
  "sp_filepath": {
    "global_config_file": "C:\\Users\\lunde\\Documents\\EZLogger\\global_config.json"
  }
}
```

**Result of `GetGlobalConfigPath()`**

```vb.net
"C:\Users\lunde\Documents\EZLogger\global_config.json"
```

---

## 6. Use Case

**Scenario**:
A new user installs EZLogger and launches it for the first time. The app checks for the existence of the local configuration file and creates it if necessary. It then prompts the user to select the global configuration file, saving that path for future sessions.

---

## 7. Related Features or Dependencies
- Used by: `ComboBox` and `ListBox` loaders that fetch values from `global_config.json`
- Supports: `Doctor list`, `Cover pages`, `Report types`
- Dependencies: `System.IO`, `System.Text.Json`, `System.Windows.Forms`

---

## 8. User Story

> As a **user of EZLogger**, I want the system to automatically manage my configuration files so I don't have to manually create or edit JSON files in my Documents folder.

---

## 9. Troubleshooting Notes
- If `local_user_config.json` becomes corrupted, the app may throw JSON parsing errors.
- Missing `sp_filepath.global_config_file` will prompt a warning message and return an empty string.
- Always ensure the selected file has a `.json` extension.

---

## 10. Screenshot / Diagram

```
[ User Clicks "Select Config" Button ]
           ↓
[ OpenFileDialog appears ]
           ↓
[ User selects JSON ]
           ↓
[ Path saved to local_user_config.json ]
           ↓
[ Used throughout app to load doctors list, templates, etc. ]
```

---

## 11. Development Status

- [x] Config folder and file auto-create logic
- [x] Path selection via file dialog
- [x] Path saved to JSON and read as needed
- [x] Error handling and messaging
- [ ] (Optional) Add versioning to config files

---