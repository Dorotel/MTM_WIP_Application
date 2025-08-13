# MASTER REFACTOR PROMPT: MTM_WIP_Application Database & Environment Compliance Update

## Meta
- **Target Files:**  
  - `MTM_WIP_Application\README.md`  
  - `Database\UpdatedStoredProcedures\README.md`  
  - `.github\ISSUE_TEMPLATE\online-refactor.yml`  
  - All files referenced by `MTM_WIP_Application\README.md`  
  - All UpdatedDatabase and UpdatedStoredProcedures files  
  - `deploy_procedures.bat`, `deploy_procedures.sh`  
  - `.github\copilot-instructions.md`
- **Base Branch:** main
- **Feature Branch:** refactor/db-env-structure/<yyyymmdd>
- **Documentation Reference:**  
  - [copilot-instructions.md](.github/copilot-instructions.md)  
  - [MTM_WIP_Application\README.md](README.md)

---

## 1. Database File Structure Compliance

### New Structure
- **CurrentDatabase**, **CurrentServer**, **CurrentStoredProcedures**:  
  - These folders contain the live, production database/server/stored procedure files.  
  - **DO NOT ALTER** these files. They are for reference only.
- **UpdatedDatabase**, **UpdatedStoredProcedures**:  
  - These folders contain the files that the repo uses for development, testing, and deployment.  
  - All changes and updates must be made here.

### README and Documentation Updates
- Update all documentation to clearly distinguish between "Current" (reference only) and "Updated" (active for repo/deployment).
- In `MTM_WIP_Application\README.md` and `Database\UpdatedStoredProcedures\README.md`, add explicit notes and diagrams showing:
  - Which folders/files are mutable and which are locked.
  - The workflow for updating procedures and databases (always via Updated* folders).

---

## 2. Environment-Specific Database and Server Logic

### Database Name Logic
- **Debug Mode:**  
  - Database name must be `mtm_wip_application_test`
- **Release Mode:**  
  - Database name must be `mtm_wip_application`

### Server Address Logic
- **Release Mode:**  
  - Server address is always `172.16.1.104`
- **Debug Mode:**  
  - If the current computer's IP address is `172.16.1.104`, use `172.16.1.104`
  - Otherwise, use `localhost`

### Implementation Requirements
- Update all configuration, connection string, and environment logic in the repo to follow these rules.
- Document this logic in both `MTM_WIP_Application\README.md` and `.github\copilot-instructions.md` for developer reference.
- Add code samples and environment detection logic in the README for clarity.

---

## 3. UpdatedDatabase and UpdatedStoredProcedures File Changes

- All files in `Database\UpdatedDatabase` and `Database\UpdatedStoredProcedures` must use the database name `mtm_wip_application_test` (not `mtm_wip_application`).
- Update any hardcoded references in these files to reflect the new test database name.
- Ensure all deployment scripts (`deploy_procedures.bat`, `deploy_procedures.sh`) use the correct database name for test/dev environments.

---

## 4. Batch and Shell Script Updates

- Update `deploy_procedures.bat` and `deploy_procedures.sh` to use `mtm_wip_application_test` for all operations.
- Add comments in these scripts explaining the environment logic and database name usage.

---

## 5. Copilot Instructions and Online Refactor Template Updates

### .github/copilot-instructions.md
- Add a section detailing the new database/server environment logic.
- Update all DAO, deployment, and environment-related instructions to reference the new file structure and rules.
- Include explicit compliance requirements for environment detection and database/server selection.

### .github/ISSUE_TEMPLATE/online-refactor.yml
- Update the template to include environment-specific instructions:
  - When refactoring, always check and update database/server logic as per the new rules.
  - Add checklist items for verifying database name and server address compliance in all affected files.

---

## 6. Dependency Updates

- For every file referenced by `MTM_WIP_Application\README.md`, review and update:
  - Any database/server connection logic to follow the new environment rules.
  - Any documentation or comments referencing the old database/server names or file structure.
  - Ensure all code, config, and documentation is consistent with the new standards.

---

## 7. Region Organization and Compliance

- All C# files must continue to follow the mandatory region organization and method ordering as specified in [.github/copilot-instructions.md](.github/copilot-instructions.md).
- When updating any code for environment logic, ensure region compliance is maintained.

---

## 8. Deliverables

- Updated `MTM_WIP_Application\README.md` and `Database\UpdatedStoredProcedures\README.md` with new structure, environment logic, and compliance notes.
- Updated `.github\copilot-instructions.md` and `.github\ISSUE_TEMPLATE\online-refactor.yml` with new environment and file structure logic.
- All `UpdatedDatabase` and `UpdatedStoredProcedures` files updated to use `mtm_wip_application_test`.
- Updated batch and shell scripts for deployment.
- All dependent files referenced by the README updated for compliance.
- A summary of all changes, including before/after examples for environment logic and file structure.

---

## 9. Example Environment Logic (for README and Instructions)

```csharp
#if DEBUG
    string databaseName = "mtm_wip_application_test";
    string serverAddress = (GetLocalIpAddress() == "172.16.1.104") ? "172.16.1.104" : "localhost";
#else
    string databaseName = "mtm_wip_application";
    string serverAddress = "172.16.1.104";
#endif
```

---

## 10. Acceptance Criteria

- All documentation and code reflect the new database/server environment logic and file structure.
- No references to the old database name in any Updated* files or deployment scripts.
- All region organization and method ordering rules are maintained.
- Online refactor template and copilot instructions are updated for future compliance.
- All changes are atomic, well-documented, and regression tested.

---

## 11. Rollback Plan

- Retain original files in a feature branch.
- Revert branch if any regression or compliance issue is found.

---

**Proceed with this refactor only after generating a full Pre-Refactor Report and awaiting explicit approval.**

---

This prompt is designed to be copy-pasted into a new markdown file for use as a master refactor instruction set. It merges your current standards with the new requirements and provides clear, actionable steps for updating the repository.