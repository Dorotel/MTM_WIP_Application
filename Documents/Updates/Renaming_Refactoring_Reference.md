# Workspace Renaming and Refactoring Reference

## Naming Convention
- Pattern: `{ClassName}_{ControlType}_{Name}` (omit Number if not needed)
- PascalCase for each segment
- No duplicate segments
- Applies to: All methods, variables, WinForms controls (public/private)
- Do not rename files, 3rd party code, or resources unless required by a dependent file (e.g., designer)

## Example Mappings
| Before                | After                                 |
|-----------------------|---------------------------------------|
| button1               | MainForm_Button_Save                  |
| comboSortBy           | Transactions_ComboBox_SortBy          |
| tabControlMain        | Transactions_TabControl_Main           |
| lblUserName           | Transactions_Label_UserName            |
| txtSearchPartID       | Transactions_TextBox_SearchPartID      |
| btnReset              | Transactions_Button_Reset              |
| tabPartEntry          | Transactions_TabPage_PartEntry         |

## Steps for Each UserControl/Form
1. For each UserControl/Form in Controls/ and Forms/:
   - Rename all controls, variables, and handlers in both .cs and .Designer.cs files to the pattern.
   - Update all references in code-behind, designer, and any dependent files.
2. Only perform renaming and reference updates (no .editorconfig or config changes).
3. Verify all dependent files are updated for each rename.
4. Ensure solution builds successfully after all changes.

## Automation/Documentation
- Use tools/scripts for bulk renaming if available (see Tools/ directory)
- Log all renamed items and their old/new names
- Place summary and changelog in Documents/Updates/
- Add PlantUML diagrams for each form/user control and its controls (Documents/Diagrams/)

## Testing/Validation
- Build solution after all renaming
- Run all unit/integration tests
- Perform manual UI smoke test

## Reference Files
- TASK_Reference_Details.md
- PR_Naming_Convention_and_Diagrams.md
- Documents/Updates/Refactoring_Summary_*.md

## Commit/PR Guidance
- Use ultra-detailed commit messages (see COMMITPROMPT.md)
- Split PR into logical commits (renaming, diagrams, tooling)
- Provide mapping table and migration guide in PR

---

This file is a quick reference for the workspace-wide renaming and refactoring task. Use it to ensure consistency and completeness.
