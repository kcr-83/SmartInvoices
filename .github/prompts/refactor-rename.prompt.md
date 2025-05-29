# Naming Consistency Refactor

Your goal is to propose a consistent and descriptive naming scheme for the provided codebase.

## Requirements

- Review all relevant names (classes, files, variables, methods, etc.).
- Follow the [Project Naming Guideline](../project-naming-guideline.md).
- For each rename, provide:
  - The current name.
  - The proposed better name.
  - A one-line reason for the change (clarity, convention, intent, etc.).
- Use a diff-style list: currentName → betterName // reason.

## Example Output

```
oldService → InvoiceService // Clarifies the domain and follows PascalCase for services.
user_ctrl → UserController // Follows Nest.js naming conventions for controllers.
```
