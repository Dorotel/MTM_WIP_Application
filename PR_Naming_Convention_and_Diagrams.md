# Pull Request Preparation: Standardize Naming Convention & Add Relationship Diagrams

## Objective

This PR will:
- **Standardize all method, variable, and WinForms control names** in all projects to the pattern:  
  `{ClassName}_{ControlType}_{Name}_{Number (if applicable)}`
- **Avoid duplicate segments** in names (do not repeat the class name).
- **Apply renaming to both public and private members.**
- **Update all references** to renamed items throughout the codebase to ensure consistency and prevent errors.
- **Not rename files themselves**—only the items inside them.

---

## Scope Clarification

- **Include:** All source code, auto-generated, designer, and third-party files (but do not rename anything 3rd party—only your source code).
- **Exclude:** Resources (`.resx`), comments, or documentation unless another file's name is dependent on it being changed (e.g., a designer file).
- **Omit the “Number” part** if not applicable.
- **Use PascalCase** for each segment (e.g., `Transactions_ComboBox_SortBy`).

---

## Naming Convention Examples

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

## Additional Requirements

### Automation Tools/Scripts

- **Location:** Place each tool in its own folder inside the `Tools` directory at the root of the repo.
  - _Example: `Tools/RenamerTool/`, `Tools/DiagramGenerator/`_
- **README:** Each tool’s folder must include a `README.md` with usage examples and troubleshooting steps.
- **Preview Mode:** Tools/scripts must support a preview mode before running.
- **Logging:** Tools/scripts must provide a report of all renamed items and their old/new names.
  - _Example: `Tools/{ToolName}/Logs/{RunName}_{Date (ex. 06-06-2025)}`_

### PlantUML Relationship Diagrams

- **Class diagram** for each form/user control and its controls.
- **Dependency diagram** showing relationships between forms/user controls and any directly used models, helpers, or data access classes.
- **Format:** Diagrams must be in `.puml` format.
- **Location:** Place all diagrams in the `Documents/Diagrams` folder at the root of the repo (create the folder if it does not exist).
- **Reference:** Diagrams should be referenced (not embedded) in documentation.

---

## Code Style & EditorConfig

- **.editorconfig** is updated to enforce formatting, naming, and region organization.
- **All C# files must:**
  - Not contain any comments (`//` or `/* ... */`).
  - Use `#region` blocks to organize: Fields, Properties, Constructors, Methods, Events, etc.
  - Example:
    ```
    #region Fields
    ...
    #endregion
    #region Properties
    ...
    #endregion
    #region Constructors
    ...
    #endregion
    #region Methods
    ...
    #endregion
    #region Events
    ...
    #endregion
    ```
- **Naming rules for controls:**  
  - PascalCase with underscores as word separators.
  - (Best effort) Suffix for control type (e.g., `ComboBox`, `Label`, `Button`).

---

## Testing and Validation

- **Build:** The solution must build successfully after renaming.  
  _Note: Use Windows to build WinForms projects if needed._
- **Tests:** All unit/integration tests must be run and pass after changes.
- **Manual Testing:** Provide a summary of any manual testing steps performed (e.g., UI smoke test).

---

## Review and Documentation

- **Summary:** Provide a summary of major changes in the PR description.
- **Changelog/Migration Guide:** If renaming is extensive, combine with the summary.
- **Location:** Place the summary and changelog/migration guide in the `Documents/Updates` folder.

---

## Pull Request Process

- **Commits:** Split the PR into multiple commits (e.g., one for renaming, one for diagrams, one for tooling).
- **Reviewers:** No specific reviewers or teams need to be tagged for review.

---

## Implementation Notes

- **.editorconfig** and Code Cleanup will only warn/suggest for naming violations; they will not auto-rename existing code. Use refactoring tools/scripts for bulk renaming.
- For large-scale changes, consider using a Roslyn analyzer/fixer or a custom script to automate comment removal and region insertion.
- All code and tools/scripts must be compatible with .NET 8.

---

## Reference

For all technical details, naming rules, code style, and automation guidance, see [TASK_Reference_Details.md](TASK_Reference_Details.md) in the root of this repository.

---

## Checklist

- [ ] All methods, variables, and controls renamed to `{ClassName}_{ControlType}_{Name}_{Number}` pattern.
- [ ] No duplicate segments in names.
- [ ] All references updated.
- [ ] No files renamed.
- [ ] All C# files have no comments and use regions for organization.
- [ ] All tools/scripts in `Tools/` with README and logs.
- [ ] All diagrams in `Documents/Diagrams` as `.puml`.
- [ ] Solution builds and all tests pass.
- [ ] Manual UI smoke test performed.
- [ ] Summary and changelog in `Documents/Updates`.
- [ ] PR split into logical commits.

---

**For any questions or clarifications, please refer to this document or [TASK_Reference_Details.md](TASK_Reference_Details.md) or contact the repository maintainer.**
