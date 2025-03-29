# Reusable Snippets for WPF UserControls in VSCode

This document contains two reusable VSCode snippet templates for building WPF `UserControl` components quickly and consistently using VB.NET.

---

## ✅ How to Use These Snippets in VSCode

1. Open VSCode.
2. Press `Ctrl+Shift+P` (or `Cmd+Shift+P` on Mac) to open the Command Palette.
3. Type `Preferences: Configure User Snippets` and select it.
4. Choose a global or project-specific snippet file (e.g., `vb.json`, `wpf.json`, or create a new one).
5. Copy and paste the relevant snippet(s) from this document into your chosen file.
6. Save the file.
7. Type the snippet `prefix` (e.g., `wpfusercontrol` or `vbusercontrol`) in your `.xaml` or `.vb` file and press `Tab` to expand it.

---

## 📄 Snippet 1: WPF UserControl XAML Template (XAML File)

This snippet generates the XAML structure of a custom WPF `UserControl`, including a `GroupBox`, `Button`, and `CheckBox`, all bound to dependency properties.

### **Snippet Name**: `wpfusercontrol`

```json
"Generic WPF UserControl Template": {
  "prefix": "wpfusercontrol",
  "body": [
    "<UserControl x:Class=\"$1\"",
    "             xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"",
    "             xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"",
    "             xmlns:mc=\"http://schemas.openxmlformats.org/markup-compatibility/2006\"",
    "             xmlns:d=\"http://schemas.microsoft.com/expression/blend/2008\"",
    "             xmlns:local=\"clr-namespace:$2\"",
    "             mc:Ignorable=\"d\"",
    "             d:DesignHeight=\"200\" d:DesignWidth=\"400\">",
    "",
    "    <Grid>",
    "        <GroupBox Margin=\"10\" Padding=\"5\"",
    "                  Header=\"{Binding ${3:GroupHeader}, RelativeSource={RelativeSource AncestorType=UserControl}}\">",
    "            <StackPanel Orientation=\"Horizontal\" Margin=\"5\">",
    "                <Button Content=\"{Binding ${4:PrimaryButtonContent}, RelativeSource={RelativeSource AncestorType=UserControl}}\"",
    "                        Height=\"30\" Width=\"80\" Margin=\"0,0,10,0\"/>",
    "",
    "                <CheckBox Content=\"{Binding ${5:CheckBoxLabel}, RelativeSource={RelativeSource AncestorType=UserControl}}\"",
    "                          IsChecked=\"{Binding ${6:IsCheckBoxChecked}, RelativeSource={RelativeSource AncestorType=UserControl}}\"",
    "                          Margin=\"0\"/>",
    "            </StackPanel>",
    "        </GroupBox>",
    "    </Grid>",
    "</UserControl>"
  ],
  "description": "A reusable, generic WPF UserControl with dependency property bindings"
}
```

### 🔧 Placeholder Fields:

| Placeholder | Description |
|-------------|-------------|
| `$1` | The full class name of the UserControl (e.g., `MyProject.MyControl`) |
| `$2` | The CLR namespace of your project (e.g., `MyProjectNamespace`) |
| `$3` | The property bound to the `GroupBox` header |
| `$4` | The property bound to the `Button` content |
| `$5` | The property bound to the `CheckBox` content |
| `$6` | The property bound to the `CheckBox` checked state |

---

## 📄 Snippet 2: VB.NET Code-Behind Template (xaml.vb File)

This snippet generates a reusable VB.NET class for a WPF `UserControl`, including one dependency property.

### **Snippet Name**: `vbusercontrol`

```json
"VB.NET WPF UserControl Code-Behind": {
  "prefix": "vbusercontrol",
  "body": [
    "Imports System.Windows",
    "Imports System.Windows.Controls",
    "",
    "Public Class ${1:MyUserControl}",
    "    Inherits UserControl",
    "",
    "    ' Constructor",
    "    Public Sub New()",
    "        InitializeComponent()",
    "    End Sub",
    "",
    "    ' === ${2:PropertyName} Dependency Property ===",
    "    Public Shared ReadOnly ${2:PropertyName}Property As DependencyProperty =",
    "        DependencyProperty.Register(",
    "            \"${2:PropertyName}\",",
    "            GetType(${3:String}),",
    "            GetType(${1:MyUserControl}),",
    "            New PropertyMetadata(${4:\"Default Value\"}))",
    "",
    "    Public Property ${2:PropertyName} As ${3:String}",
    "        Get",
    "            Return CType(GetValue(${2:PropertyName}Property), ${3:String})",
    "        End Get",
    "        Set(value As ${3:String})",
    "            SetValue(${2:PropertyName}Property, value)",
    "        End Set",
    "    End Property",
    "",
    "End Class"
  ],
  "description": "Creates a VB.NET code-behind template for a WPF UserControl with one dependency property"
}
```

### 🔧 Placeholder Fields:

| Placeholder | Description |
|-------------|-------------|
| `$1` | The name of the UserControl class (e.g., `TaskStepControl`) |
| `$2` | The name of the dependency property (e.g., `ButtonContent`) |
| `$3` | The data type of the property (e.g., `String`, `Boolean`) |
| `$4` | The default value for the property (e.g., `"Save"` or `False`) |

---

## 🧪 Example Workflow

1. In `TaskStepControl.xaml`, type `wpfusercontrol` and press `Tab`. Fill in:
    - Class name
    - Namespace
    - Property names

2. In `TaskStepControl.xaml.vb`, type `vbusercontrol` and press `Tab`. Fill in:
    - Control class name
    - Property name and type

3. Repeat the `vbusercontrol` snippet multiple times for each property you want to expose.
