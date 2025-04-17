# Writing Clean Code in EZLogger

As the EZLogger project continues to grow in size and complexity, it's critical to keep the codebase clean, maintainable, and easy to understand. This article is a personal reference to help ensure that the code remains easy to work with both now and in the future.

---

## What Is Clean Code?

Clean code is code that:
- Is easy to read and understand
- Has a clear structure
- Is modular and reusable
- Avoids duplication (DRY: Don't Repeat Yourself)
- Handles errors gracefully
- Follows consistent naming and formatting conventions

In short, clean code is written for humans first, then for machines.

---

## Clean Code Checklist

Use this list when reviewing new code:

- [ ] **Function Length**: Functions should be short (ideally under 30 lines) and focused on doing one thing.
- [ ] **Naming**: Use clear, descriptive names. E.g., `GetListFromGlobalConfig` is better than `LoadList`.
- [ ] **Separation of Concerns**: Keep UI code, logic, and data access in separate files/modules.
- [ ] **Handlers**: Move all event logic into handler classes. Do not place logic directly in the code-behind.
- [ ] **Avoid Duplication**: Centralize reusable logic into helpers or shared modules.
- [ ] **Comment Wisely**: Use comments to explain why something is done, not what is done.
- [ ] **Consistent Style**: Use consistent indentation, spacing, and naming (e.g., PascalCase for methods).
- [ ] **Early Returns**: Use early return patterns to reduce nesting.
- [ ] **Small Files**: Keep files under ~300 lines. Break large files into logical units.
- [ ] **Descriptive UI Event Names**: If using visual names like `Btn_A`, add comments indicating their purpose.

---

## Examples from EZLogger

### Example 1: Clear Function Names

**Bad:**
```vb.net
Public Function LoadList()
```
**Good:**
```vb.net
Public Function GetCoverPagesFromGlobalConfig() As List(Of String)
```
This makes the intent of the function obvious, even without reading its body.

---

### Example 2: Small, Focused Functions

**Bad:** A single function loads the config file, parses it, populates the UI, and sets events.

**Good:** Split this into:
- `LoadConfigJson()`
- `GetListFromGlobalConfig()`
- `PopulateListBoxWithCoverPages()`

Each of these can be independently tested and reused.

---

### Example 3: Reuse Instead of Repeating

Before:
```vb.net
Dim json = File.ReadAllText(path)
Dim doc = JsonDocument.Parse(json)
Dim list = New List(Of String)
' Repeat for each list type
```

After:
```vb.net
Dim list = ConfigHelper.GetListFromGlobalConfig("listbox", "cover_pages")
```
This reduces duplication and centralizes error handling.

---

## DRY (Don't Repeat Yourself)

Always ask:
- Am I repeating this code in multiple places?
- Can this logic live in a shared helper module?
- Can this be simplified into a single reusable method?

### Use Case: Button Wiring in Views

EZLogger uses:
```vb.net
AddHandler Btn_A.Click, AddressOf _handler.HandleButtonAClick
```
All logic lives in the handler:
```vb.net
Public Sub HandleButtonAClick(view As ReportWizardPanel)
    ' Do task here
End Sub
```
This pattern is DRY, readable, and testable.

---

## Final Thoughts

Clean code is not about perfection—it's about **clarity**. If you or someone else can quickly find and fix a bug, refactor safely, or extend a feature, you've written clean code. Treat your code like a product others will inherit. EZLogger’s success depends not just on functionality, but maintainability.

Keep this article nearby, and revisit it when you feel stuck or if things start to get messy. Clean code is a practice—refine it every time you write.

---

*Last updated: April 2025*

---

<!-- @nested-tags:clean_code -->