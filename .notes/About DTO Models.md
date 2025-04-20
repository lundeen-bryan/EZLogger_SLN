# Why I Use DTOs in EZLogger

As the solo developer of EZLogger, I made the decision to incorporate **DTOs** (Data Transfer Objects) into the architecture of this project. This documentation is intended to help future contributors understand **why** I introduced this pattern, how it works, and when to use it — especially if you're not familiar with WPF, MVVM, or VSTO development.

---

## What is a DTO?

A **DTO** is a lightweight class whose only purpose is to **carry structured data between parts of an application**. It does **not contain any logic or behavior** — just properties.

In EZLogger, I use DTOs to pass data **from logic handlers to WPF views**. This helps separate backend logic from UI responsibilities. Since WPF and VSTO can easily become messy with tightly-coupled code, DTOs help keep things modular and testable.

---

## Why I Chose DTOs

There were several reasons I decided to use DTOs in EZLogger:

- **To reduce clutter in the view code-behind** — I didn’t want to write JSON parsing or file IO in the views.
- **To keep handlers focused on logic only** — They prepare data and return it in a clean, predictable format.
- **To make error handling consistent** — Many of my DTOs include an `ErrorMessage` property and a `HasError` flag.
- **To future-proof the code** — Even if the DTO is only used in one place for now, it keeps the door open for reuse later.

---

## A Real Example: AboutInfoResult

Here’s a real-world example of how I used a DTO to clean up the logic in the About screen.

Instead of doing this in the view:

- Load a file
- Parse JSON
- Check for errors
- Update five UI controls

I moved the logic to a handler, which returns a DTO with all the data already prepared.

### The DTO

```vbnet
Public Class AboutInfoResult
    Public Property CreatedBy As String
    Public Property SupportEmail As String
    Public Property LastUpdate As String
    Public Property VersionNumber As String
    Public Property LatestChange As String
    Public Property ErrorMessage As String

    Public ReadOnly Property HasError As Boolean
        Get
            Return Not String.IsNullOrEmpty(ErrorMessage)
        End Get
    End Property
End Class
```

### In the View

```vbnet
Dim result = _handler.LoadAboutInfo(ConfigHelper.GetGlobalConfigPath())
If result.HasError Then
    MessageBox.Show(result.ErrorMessage)
Else
    TxtVersion.Text = result.VersionNumber
    TxtCreatedBy.Text = result.CreatedBy
    ' etc...
End If
```

This made the view cleaner and easier to understand. Future devs don’t need to understand JSON parsing — they just read values from a structured object.

---

## When to Use a DTO

If you're working on this project and wondering whether to use a DTO, here are some rules of thumb:

- Use a DTO when you're returning **multiple related values** from a handler or helper.
- Use one when you want to **avoid putting logic into your view**.
- Use one when you need to **report success or failure** back to the view.

---

## Final Thoughts

Even if a DTO is only used in one place, I’ve found that it’s worth it to keep responsibilities clean. A handler shouldn’t touch controls. A view shouldn’t know how to parse a file. By introducing DTOs in places like `AboutView`, `ConfigView`, and others, I’m creating clear boundaries that keep this project maintainable and easier to troubleshoot.

If you’re coming from WinForms or are new to WPF, don’t worry — DTOs are a simple pattern that works well even outside MVVM. This is one of those patterns I brought over to keep things clean while still being pragmatic about development.

