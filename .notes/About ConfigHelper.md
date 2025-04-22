# ConfigHelper Module Documentation

---

## About This Article

This article provides technical documentation for the `ConfigHelper` module used in the EZLogger application. The module is responsible for managing configuration files that store application-specific paths and settings. It handles creation, reading, updating, and validation of the local and global configuration files.

---

## Applies To

- EZLogger v1.0 and above
- VB.NET

---

## Prerequisites

- Basic understanding of VB.NET
- Familiarity with JSON and file I/O operations
- Understanding of Windows Forms (`OpenFileDialog`)

---

## Summary

The `ConfigHelper` module ensures the existence and correctness of configuration files required by the EZLogger application. It provides helper functions to:

- Ensure local config file exists (`local_user_config.json`)
- Prompt user to select a global config file (`global_config.json`)
- Store and retrieve config file paths from JSON

---

## Key Components

| Type        | Name                                                | Description                                          |
|-------------|-----------------------------------------------------|------------------------------------------------------|
| Module      | `ConfigHelper.vb`                                   | Contains helper functions for configuration handling |
| Config File | `%USERPROFILE%\.ezlogger\local_user_config.json`     | Stores user-specific paths and settings              |
| Config File | `global_config.json`                                | Shared config file synced via SharePoint             |
| UI Element  | `OpenFileDialog`                                    | Prompts user to select the global configuration file |

---

## Code Examples

### GetLocalConfigPath
```vbnet
Public Function GetLocalConfigPath() As String
    Return EnsureLocalUserConfigFileExists()
End Function
```

### UpdateLocalConfigWithGlobalPath
```vbnet
Public Sub UpdateLocalConfigWithGlobalPath(globalConfigPath As String)
    ' Reads local_user_config.json, updates sp_filepath.global_config_file with new value
    ' Writes updated JSON back to disk
End Sub
```

### PromptForGlobalConfigFile
```vbnet
Public Function PromptForGlobalConfigFile() As String
    ' Shows OpenFileDialog for selecting global_config.json
    ' Returns selected path or empty string
End Function
```

### EnsureLocalUserConfigFileExists
```vbnet
Public Function EnsureLocalUserConfigFileExists() As String
    ' Ensures the .ezlogger folder and local_user_config.json file exist
    ' Returns the full path to the config file
End Function
```

### GetGlobalConfigPath
```vbnet
Public Function GetGlobalConfigPath() As String
    ' Reads sp_filepath.global_config_file from local_user_config.json
    ' Returns path or shows error if not found
End Function
```

---

## Execution Flow

1. Application startup triggers `GetLocalConfigPath()`.
2. If `local_user_config.json` does not exist, it is created via `EnsureLocalUserConfigFileExists()`.
3. If the global config file path is not present, the user is prompted via `PromptForGlobalConfigFile()`.
4. The selected path is saved using `UpdateLocalConfigWithGlobalPath()`.
5. Future usage retrieves the path with `GetGlobalConfigPath()`.

---

## Sample Input / Output

**Sample `local_user_config.json`**
```json
{
  "status": "created",
  "created_at": "2025-04-17T21:01:00",
  "sp_filepath": {
    "global_config_file": "C:\\Users\\lunde\\Documents\\EZLogger\\global_config.json"
  }
}
```

**Result from `GetGlobalConfigPath()`**
```vbnet
"C:\Users\lunde\Documents\EZLogger\global_config.json"
```

---

## Use Case

**Scenario**:
A new user installs EZLogger and launches it. The application checks for the local configuration file and creates it if missing. It then prompts the user to select a shared global config file, which is saved and used for all future sessions.

---

## Related Features or Dependencies

- **Used by**: UI components like `ComboBox`, `ListBox` (to load lists from `global_config.json`)
- **Supports**: Doctors list, cover pages, report types
- **Dependencies**:
  - `System.IO`
  - `System.Text.Json`
  - `System.Windows.Forms`

---

## User Story

> As a **user of EZLogger**, I want the system to automatically manage my configuration files so I don't have to manually create or edit JSON files on my computer.

---

## Troubleshooting

- If `local_user_config.json` is corrupted, JSON parsing errors may occur.
- Missing `sp_filepath.global_config_file` results in a prompt or an empty string return.
- Ensure only `.json` files are selected in file dialogs.
- Use the "C" button in EZLogger’s Config Manager to re-create local settings.

---

## Diagram

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

## Development Status

- [x] Config folder and file auto-create logic
- [x] Path selection via file dialog
- [x] Path saved to JSON and read as needed
- [x] Error handling and messaging
- [ ] (Optional) Add versioning to config files

---

## See Also
