# Ultra-Detailed Commit Message Generation Prompt

Use this prompt with your AI tool to generate the most exhaustive, precise, and maintainable commit messages possible. This prompt is designed to capture every relevant detail for future reviewers and maintainers.

---

## Example Output Format
## Refactor: Standardize UserControl/File Naming and Update References

---

## Comprehensive Commit Body

### What

- Renamed all UserControl and related files in `Controls/SettingsForm/` to follow the new `{ClassName}_{ControlType}_{Name}` convention (e.g., `EditUserControl` ? `Control_Edit_User`, `RemoveUserControl` ? `Control_Remove_User`, etc.).
- Added new files for renamed controls: `Control_Edit_User.cs`, `Control_Edit_User.Designer.cs`, `Control_Remove_User.cs`, and their `.resx` resources.
- Removed legacy files: `EditUserControl.cs`, `EditUserControl.Designer.cs`, `RemoveUserControl.cs`, and related resources.
- Updated all references in:
  - `Forms/Settings/SettingsForm.cs`
  - `Forms/Settings/SettingsForm.Designer.cs`
  - `Forms/MainForm/MainForm.Designer.cs`
  - `Controls/Shared/ProgressBarUserControl.cs`
  - `MTM_Inventory_Application.csproj`
- Added `Documents/Updates/Renaming_Refactoring_Reference.md` to document the renaming/refactoring process, conventions, and provide a mapping table.
- Removed obsolete documentation and tooling files related to previous naming/diagram conventions and refactoring scripts.

### Why

- Motivation: To enforce a consistent, descriptive, and maintainable naming convention for all UserControls and related files, improving code readability, maintainability, and onboarding for new developers.
- Problems Solved: Addressed technical debt from inconsistent or ambiguous control/file names, reduced risk of naming collisions, and improved traceability across the codebase.
- Business/User Value: Enhances maintainability, reduces onboarding time, and supports future refactoring and automation efforts.
- Prior Issues/Requests: Aligns with workspace documentation and prior PR guidance for naming and refactoring.

### How

- Implementation: Used a systematic approach to rename all UserControl and related files, updating both code-behind and designer files, as well as all references in forms, project files, and resources.
- Design Rationale: Adopted a `{ClassName}_{ControlType}_{Name}` pattern for clarity and scalability. Chose not to rename third-party or resource files unless required by dependencies.
- Alternatives Considered: Considered partial renaming or only updating new files, but full standardization was selected for long-term maintainability.
- Control Flow/API Changes: Updated all instantiations, event handlers, and references to the new control names. Ensured all dependent files and project references were updated.
- Documentation: Added a comprehensive reference file (`Renaming_Refactoring_Reference.md`) and removed outdated documentation/tools.

### Scope & Impact

- Affected Modules: All UserControls in `Controls/SettingsForm/`, forms referencing these controls, shared controls, and the main project file.
- Ripple Effects: Requires all developers to use the new naming convention for future work. May impact any scripts or tools referencing old names.
- Breaking Changes: None to public APIs, but all internal references to renamed controls must be updated.
- Dependencies/CI: Project file updated; no external dependencies affected.

### Testing

- Manual Testing: Performed manual UI smoke tests to verify all controls load and function as expected after renaming.
- Automated Testing: Built the solution and ran all unit/integration tests to ensure no regressions.
- Test Coverage: All affected forms and controls were exercised; no new test cases added, but all existing tests passed.
- Validation Steps: Build the solution, run all tests, and manually open and interact with all affected forms.
- Known Issues: None identified; all references updated and validated.

### Migration/Deployment

- Migration Steps: No data migrations required. All renaming is code-level.
- Deployment Notes: Ensure all developers pull the latest changes and update any local scripts/tools referencing old control names.
- Rollback: Revert this commit to restore previous naming.
- QA Guidance: Review all renamed files and references; verify UI and functionality.

### References/Traceability

- See `Documents/Updates/Renaming_Refactoring_Reference.md` for mapping and rationale.
- See `COMMITPROMPT.md` for commit message structure.
- Supersedes prior documentation in `PR_Naming_Convention_and_Diagrams.md` and `TASK_Reference_Details.md`.

### TODOs/Follow-ups

- Update any external documentation or scripts referencing old control names.
- Consider automating future renaming with improved tooling.
- Monitor for any missed references in less commonly used forms or scripts.

---

## Affected Entities Table

| File/Entity                                             | Change   | Purpose/Notes                                              | Cross-Reference                        |
|---------------------------------------------------------|----------|------------------------------------------------------------|----------------------------------------|
| Controls/SettingsForm/EditUserControl.cs                | Removed  | Legacy control, replaced by new naming                     | See new: Control_Edit_User.cs          |
| Controls/SettingsForm/EditUserControl.Designer.cs       | Removed  | Legacy designer, replaced by new naming                    | See new: Control_Edit_User.Designer.cs |
| Controls/SettingsForm/RemoveUserControl.cs              | Removed  | Legacy control, replaced by new naming                     | See new: Control_Remove_User.cs        |
| Controls/SettingsForm/RemoveUserControl.Designer.cs     | Renamed  | Standardized to new naming convention                      | Control_Remove_User.Designer.cs        |
| Controls/SettingsForm/Control_Edit_User.cs              | Added    | New control with standardized name                         |                                        |
| Controls/SettingsForm/Control_Edit_User.Designer.cs     | Added    | New designer with standardized name                        |                                        |
| Controls/SettingsForm/Control_Remove_User.cs            | Added    | New control with standardized name                         |                                        |
| Controls/SettingsForm/Control_Remove_User.Designer.cs   | Renamed  | Standardized to new naming convention                      |                                        |
| Controls/SettingsForm/Control_Edit_User.resx            | Added    | Resource for new control                                   |                                        |
| Controls/SettingsForm/Control_Remove_User.resx          | Renamed  | Resource for new control                                   |                                        |
| Forms/Settings/SettingsForm.cs                          | Edited   | Updated references to renamed controls                     |                                        |
| Forms/Settings/SettingsForm.Designer.cs                 | Edited   | Updated references to renamed controls                     |                                        |
| Forms/MainForm/MainForm.Designer.cs                     | Edited   | Updated references to renamed controls                     |                                        |
| Controls/Shared/ProgressBarUserControl.cs               | Edited   | Updated references to renamed controls                     |                                        |
| MTM_Inventory_Application.csproj                        | Edited   | Updated to include/remove renamed/added/removed files      |                                        |
| Documents/Updates/Renaming_Refactoring_Reference.md     | Added    | Documents naming convention, mapping, and migration steps  |                                        |
| PR_Naming_Convention_and_Diagrams.md                    | Removed  | Obsolete documentation                                     | See new: Renaming_Refactoring_Reference.md |
| TASK_Reference_Details.md                               | Removed  | Obsolete documentation                                     | See new: Renaming_Refactoring_Reference.md |
| Tools/CommentRemover/*, Tools/DiagramGenerator/*, etc.  | Removed  | Obsolete scripts/tools for previous naming conventions      |                                        |

---

## Changelog Summary

- **Added:**  
  - `Control_Edit_User.cs`, `Control_Edit_User.Designer.cs`, `Control_Edit_User.resx`
  - `Control_Remove_User.cs`, `Control_Remove_User.Designer.cs`, `Control_Remove_User.resx`
  - `Documents/Updates/Renaming_Refactoring_Reference.md`
- **Changed:**  
  - Updated all references in forms, designer files, and project file to use new control names.
- **Removed:**  
  - Legacy controls and designers: `EditUserControl.*`, `RemoveUserControl.*`, and related resources.
  - Obsolete documentation and tooling: `PR_Naming_Convention_and_Diagrams.md`, `TASK_Reference_Details.md`, `Tools/CommentRemover/*`, `Tools/DiagramGenerator/*`, `Tools/RegionOrganizer/*`, `Tools/RenamerTool/*`.

---

**Sign-off:**  
_Signed-off-by: [author] <email>_
---

## Prompt

> Generate an **ultra-detailed, professional commit message** for the following code changes. The commit message must strictly adhere to the following structure and requirements:
>
> 1. **Summary Line**
>    - Begin with a single, concise summary line (50–72 characters) that clearly expresses the primary intent and impact of the commit.
>
> 2. **Comprehensive Commit Body**
>    - Provide a multi-paragraph, deeply detailed explanation that covers:
>      - **What:**  
>        - List all changes, additions, removals, refactors, and fixes at every level (UI, backend, database, infrastructure, tests, configuration, documentation, code style).
>        - Highlight both major and minor changes, including variable renames, comments, and formatting.
>      - **Why:**  
>        - For each change, explain the motivation, problem solved, or business/user value delivered.
>        - Discuss technical debt addressed, performance/security/usability/maintainability improvements, and any prior issues or requests.
>      - **How:**  
>        - Break down the implementation approach for each major change.
>        - Explain the rationale for design patterns, algorithms, libraries, or frameworks chosen.
>        - Summarize any alternatives considered, and why they were not selected.
>        - Describe changes to control flow, data models, APIs, schemas, configuration, and external integrations.
>      - **Scope & Impact:**  
>        - Explicitly list all affected modules, components, features, workflows, and user interactions.
>        - Document any ripple effects, breaking changes, or updates to dependencies/build/CI environments.
>        - Note any changes to external/public interfaces, schemas, or protocols.
>      - **Testing:**  
>        - Describe all testing performed (manual, automated, unit, integration, performance, security).
>        - List new or updated test cases, test coverage, and outcomes.
>        - Provide steps to reproduce, verify, or validate the change.
>        - Mention known issues, limitations, or test gaps.
>      - **Migration/Deployment:**  
>        - Specify any migration steps, data transformations, or deployment notes.
>        - Describe rollback or recovery procedures, if applicable.
>        - Offer guidance for QA, reviewers, and future maintainers.
>      - **References/Traceability:**  
>        - Link to all relevant issues, tickets, pull requests, specs, discussions, and external documentation.
>      - **TODOs/Follow-ups:**  
>        - List outstanding tasks, technical debt, or further work required.
>        - Flag areas for additional investigation or improvement.
>
> 3. **Affected Entities Table**
>    - Provide a Markdown table enumerating all files, classes, functions, methods, and configuration keys affected. For each, note:
>      - The specific change (add, edit, remove, rename, refactor, etc.)
>      - The purpose of the change
>      - Any cross-references to related code or documentation
>
> 4. **Rich Markdown Formatting**
>    - Structure the message with section headings (`##`, `###`), bullet points, numbered lists, and code blocks.
>    - Include before/after code snippets or configuration diffs for all non-trivial changes, clearly annotated.
>    - Use tables to summarize changes, test results, or migration steps where appropriate.
>
> 5. **Changelog Summary**
>    - Conclude with a section explicitly summarizing everything added, changed, fixed, removed, or deprecated, using a clear changelog format.
>
> 6. **Sign-off (if required)**
>    - End with a sign-off line (e.g., `Signed-off-by: [author] <email>`) if your project policy requires it.
>
> ---
>
> **Input Diff:**  
> ```
>
> READ DIFF.md
>
> ```
>
> ---
>
> **Guidance:**  
> - Be maximally exhaustive, precise, and unambiguous.
> - Assume the reader is technical and familiar with the codebase, but capture every nuance, decision, and rationale for future-proofing.
> - Follow and exceed best practices for conventional commits and changelogs.
> - Over-document rather than under-document to maximize clarity, traceability, and maintainability.

---

**How to use:**  
- Copy this prompt into your AI tool, replacing `[PASTE YOUR DIFF HERE]` with your actual code diff or a summary of your changes.
