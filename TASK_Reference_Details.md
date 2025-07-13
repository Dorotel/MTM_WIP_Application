# Task Reference Details: Naming, Code Style, and Automation

## 1. Naming Convention

- **Pattern:** `{ClassName}_{ControlType}_{Name}_{Number (if applicable)}`
- **No duplicate segments** (do not repeat the class name).
- **PascalCase** for each segment.
- **Omit the “Number” part** if not applicable.
- **Apply to:** All methods, variables, and WinForms controls (public and private).
- **Do not rename:** Files themselves, 3rd party code, or resources unless required by a dependent file (e.g., designer).

### Examples

| Before                | After                                 |
|-----------------------|---------------------------------------|
| `button1`             | `MainForm_Button_Save`                |
| `comboSortBy`         | `Transactions_ComboBox_SortBy`        |
| `tabControlMain`      | `Transactions_TabControl_Main`        |
| `lblUserName`         | `Transactions_Label_UserName`         |
| `txtSearchPartID`     | `Transactions_TextBox_SearchPartID`   |
| `btnReset`            | `Transactions_Button_Reset`           |
| `tabPartEntry`        | `Transactions_TabPage_PartEntry`      |

---

## 2. .editorconfig and Code Style

- **No comments** in C# files (`//` or `/* ... */`).
- **Use `#region` blocks** to organize: Fields, Properties, Constructors, Methods, Events, etc.
- **Naming rules for controls:**  
  - PascalCase with underscores as word separators.
  - (Best effort) Suffix for control type (e.g., `ComboBox`, `Label`, `Button`).
- **.editorconfig** is updated to enforce formatting, naming, and region organization.
- **Visual Studio and Code Cleanup** will only warn/suggest for naming violations; they will not auto-rename existing code.

---

## 3. Automation Tools/Scripts

- **Location:** Each tool in its own folder under `Tools/` at the repo root.
- **README:** Each tool’s folder must include a `README.md` with usage and troubleshooting.
- **Preview Mode:** Tools/scripts must support a preview mode before running.
- **Logging:** Tools/scripts must provide a report of all renamed items and their old/new names.
  - _Example: `Tools/{ToolName}/Logs/{RunName}_{Date (ex. 06-06-2025)}`_

---

## 4. PlantUML Diagrams

- **Class diagram** for each form/user control and its controls.
- **Dependency diagram** for relationships between forms/user controls and directly used models, helpers, or data access classes.
- **Format:** `.puml`
- **Location:** `Documents/Diagrams/`
- **Reference:** Diagrams should be referenced (not embedded) in documentation.

---

## 5. Testing and Validation

- **Build:** Solution must build successfully after renaming.
- **Tests:** All unit/integration tests must pass.
- **Manual Testing:** Provide a summary of any manual testing steps performed (e.g., UI smoke test).

---

## 6. Review and Documentation

- **Summary:** Provide a summary of major changes in the PR description.
- **Changelog/Migration Guide:** If renaming is extensive, combine with the summary.
- **Location:** Place the summary and changelog/migration guide in `Documents/Updates/`.

---

## 7. Pull Request Process

- **Commits:** Split the PR into multiple commits (e.g., one for renaming, one for diagrams, one for tooling).
- **Reviewers:** No specific reviewers or teams need to be tagged for review.

---

## 8. Implementation Notes

- For large-scale changes, consider using a Roslyn analyzer/fixer or a custom script to automate comment removal and region insertion.
- All code and tools/scripts must be compatible with .NET 8.

---

**For further details, see the main PR prompt or contact the repository maintainer.**
