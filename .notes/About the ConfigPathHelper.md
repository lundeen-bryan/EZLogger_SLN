# Why Use a Module Instead of a Class for ConfigPathHelper in VB.NET

When building helper utilities like `ConfigPathHelper.vb` in a VB.NET WPF project, using a **Module** instead of a **Class** has clear benefits—especially when the helper is meant to provide shared functions across the entire application.

---

## ✅ What is a Module in VB.NET?
A `Module` is a container for variables and methods that are shared across the application. All members in a module are implicitly `Shared`, which means:

- You **don't need to instantiate** the module to use it.
- Functions can be called directly: `ConfigPathHelper.GetDoctorList()`.

This is ideal for utility code that doesn’t need to store state or create multiple instances.

---

## 🔧 Example Comparison

### Using a Module:
```vbnet
Public Module ConfigPathHelper
    Public Function GetDoctorList() As List(Of String)
        ' ...code here...
    End Function
End Module
```
Usage:
```vbnet
Dim doctors = ConfigPathHelper.GetDoctorList()
```

### Using a Class:
```vbnet
Public Class ConfigPathHelper
    Public Function GetDoctorList() As List(Of String)
        ' ...code here...
    End Function
End Class
```
Usage:
```vbnet
Dim helper As New ConfigPathHelper()
Dim doctors = helper.GetDoctorList()
```

---

## 💡 Why Use a Module for ConfigPathHelper?

### 1. **Stateless and Shared**
- The methods don’t rely on or maintain internal state.
- They’re just utilities for reading config files and returning values.

### 2. **Cleaner Code**
- No need to create unnecessary object instances.
- Your calling code stays simple and readable.

### 3. **Consistent with Purpose**
- The `ConfigPathHelper` is a collection of stateless tools.
- Modules reflect that "toolbox" purpose better than a class.

---

## 🧠 When NOT to Use a Module
If you need to:
- Maintain internal state or instance-specific data
- Inherit from another class
- Use object-oriented features like polymorphism

...then a `Class` is the right choice.

---

## ✅ Summary
Use a `Module` for `ConfigPathHelper` because it contains stateless, utility-style functions that should be accessible throughout your project without creating object instances. It keeps your code simpler, cleaner, and aligns with VB.NET best practices for shared helpers.

If later you find you need to store instance-specific state or configuration settings, you can always refactor it into a `Class`.