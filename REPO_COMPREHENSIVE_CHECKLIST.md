# MTM_WIP_Application – Comprehensive Repository Structure, Database, Privilege System & Workflow Checklist

This master checklist brings together all requirements, policies, business rules, workflow, privilege enforcement, database schema, and documentation standards for the `Dorotel/MTM_WIP_Application` repository.  
**It is the single source of truth for refactor, development, deployment, and maintenance.**

---

## 1. Repository Structure

- [ ] **Root Layout**
  - [ ] Only essential files at repository root (`README.md`, `.gitignore`, `.editorconfig`, `LICENSE`, solution `.sln`, `DATABASE_SCHEMA.sql`, `MYSQL_SERVER_CHANGES.md`, etc.)
  - [ ] Use a clear, descriptive solution file (e.g., `MTM_WIP_Application.sln`)
- [ ] **Source Code Organization**
  - [ ] All source code in `src/` or `MTM_WIP_Application/`
  - [ ] For multi-project: subfolders like `src/Domain/`, `src/Application/`, `src/Infrastructure/`, `src/Presentation/`
- [ ] **Tests**
  - [ ] Dedicated `tests/` directory at repo root, with a project like `MTM_WIP_Application.UnitTests`
  - [ ] Test project mirrors source structure, only contains successfully tested unit tests
- [ ] **Documentation**
  - [ ] Root `README.md`
  - [ ] Additional docs in `docs/` if needed (e.g., developer guides, onboarding)
  - [ ] This checklist (`REPO_COMPREHENSIVE_CHECKLIST.md`) and `MYSQL_SERVER_CHANGES.md` in the root
  - [ ] `DATABASE_SCHEMA.sql` in the root (see Section 11)
- [ ] **Configuration & Assets**
  - [ ] Config files (e.g., `appsettings.json`) in respective project folders
  - [ ] Static assets in `assets/` or `resources/`
- [ ] **.gitignore**
  - [ ] Covers all platform- and IDE-specific files (Visual Studio, Rider, Mac/Windows, etc.)

#### Issues & Solutions

- **Breaking refactor:** Plan in phases, start with non-breaking changes, use feature branches, backup before migration.
- **Broken references:** Move files via VS Solution Explorer, auto-update, revalidate `.sln` and project references.

---

## 2. C# File Refactoring & Code Quality

- [ ] **Structure**
  - [ ] One public type per file; file name matches type
  - [ ] Consistent, descriptive naming
  - [ ] Organize code with regions for Fields, Properties, Constructors, Methods, Events, etc.
- [ ] **Namespaces**
  - [ ] Hierarchical, descriptive (e.g., `MTM_WIP_Application.Domain.Models`)
- [ ] **Usings**
  - [ ] Placed outside namespace, System first, sorted, no unused usings
- [ ] **Class Design**
  - [ ] Explicit access modifiers, private/protected by default, auto-properties, readonly/const, explicit interface impls
  - [ ] Dispose pattern if needed
- [ ] **Code Quality**
  - [ ] Remove dead code, split large methods, avoid magic numbers/strings, clear naming, consistent formatting, XML docs (optional), exception handling/logging

#### Issues & Solutions

- **Multiple public types, inconsistent regions:** Use refactoring tools, enforce in code reviews.
- **Namespace renames breaking reflection:** Search all references, config, string-based calls.

---

## 3. Unit Tests

- [ ] **Setup**
  - [ ] Add a test project, reference main project, use xUnit/NUnit/MSTest
- [ ] **Coverage**
  - [ ] Unit tests for all public/critical methods/classes refactored, only include verified tests
  - [ ] Each test covers a single logical unit, uses AAA pattern, descriptive names
- [ ] **Structure**
  - [ ] Tests mirror source layout, grouped in `ClassNameTests`
- [ ] **Quality**
  - [ ] Mock external dependencies, avoid trivial code tests, ensure independence/repeatability

#### Issues & Solutions

- **No/legacy tests:** Add for new/refactored code first, introduce dependency injection.
- **Slow/flaky tests:** Mock/fake external dependencies, separate business logic from DB access.

---

## 4. Advanced Organization & Best Practices

- [ ] **Solution Structure**
  - [ ] Single `.sln` at root, references all projects; subfolders for `src/`, `tests/`, etc.
- [ ] **Editor & Style**
  - [ ] `.editorconfig`, `stylecop.json`, Roslyn Analyzers
- [ ] **CI/CD**
  - [ ] GitHub Actions/Azure Pipelines for build/test on every PR/commit, status badges in `README.md`
- [ ] **Architecture**
  - [ ] (Optional) Clean/Onion Architecture with layers: Domain, Application, Infrastructure, UI
- [ ] **Documentation & Contribution**
  - [ ] Up-to-date `README.md`, `CONTRIBUTING.md`, `CHANGELOG.md`, solution/project-level XML docs
- [ ] **Dependency Management**
  - [ ] Central via `Directory.Packages.props`, regular review, Dependabot
- [ ] **Version Control**
  - [ ] Tailor `.gitignore`, branch protection, PR reviews & CI, semantic versioning, git tags
- [ ] **Config & Secrets**
  - [ ] Never commit secrets, use user-secrets/env vars for dev
- [ ] **Templates**
  - [ ] `ISSUE_TEMPLATE`, `PULL_REQUEST_TEMPLATE` for contributions
- [ ] **Discoverability**
  - [ ] Directory `README.md` for subfolders, GitHub Projects/Issues for backlog, `CODEOWNERS` for reviews, Discussions/Wiki for Q&A

#### Issues & Solutions

- **Not following style rules:** Enforce in CI and reviews, document clearly.
- **Legacy project migration:** Refactor layers gradually, avoid "big bang."

---

## 5. User Role Privilege Enforcement

### **Role Definitions (Authoritative Matrix)**

| Role      | Read | Write | Inventory/Transactions Write | User/Settings/Admin | Search Only |
|-----------|------|-------|-----------------------------|---------------------|-------------|
| **Admin** | Yes  | Yes   | Yes                         | Yes                | Yes         |
| **Normal**| Yes  | No*   | Yes (only `inv_inventory`, `inv_transaction`) | No | Yes   |
| **Read-Only**| Yes| No    | No                          | No                 | Yes         |

\*Normal users can only write to `inv_inventory` and `inv_transaction`.

#### **Business Rule Summary**
- **Admin:** Full access to all features, tables, and settings.
- **Normal:** May read all, may only write to `inv_inventory` and `inv_transaction`. No admin features.
- **Read-Only:** May only use search features. No write, update, or delete anywhere.

#### **Enforcement Strategy**

**Data Layer:**
- Before any write (insert/update/delete):
    - If Admin: allow.
    - If Normal: allow only if table is `inv_inventory` or `inv_transaction`; otherwise, block.
    - If Read-Only: block all writes.
- Before any administrative operation: allow only for Admin.

**UI Layer:**
- On load, check user type:
    - Admin: all controls enabled.
    - Normal: only inventory/transaction write controls enabled; admin features disabled.
    - Read-Only: only search controls enabled; all others disabled.
- Event handlers for restricted actions: early-exit with user feedback if not permitted.

**Testing:**
- Log in as each role, verify all restrictions in UI and direct data access.
- Attempt to bypass UI; ensure privilege checks at all DAO/proc/data entrypoints.

**Code Example:**

```csharp
// DAO/data-layer privilege enforcement
public void UpdateInventoryItem(InventoryItem item) {
    if (Model_AppVariables.UserTypeAdmin) {
        // Allow all
    } else if (Model_AppVariables.UserTypeNormal) {
        if (!IsInventoryOrTransactionTable(item.TableName))
            throw new UnauthorizedAccessException("Normal users cannot modify this table.");
    } else {
        throw new UnauthorizedAccessException("Read-Only users cannot modify data.");
    }
    // ...proceed with write
}

// UI enforcement
private void SetControlsByRole() {
    if (Model_AppVariables.UserTypeAdmin) {
        EnableAllControls();
    } else if (Model_AppVariables.UserTypeNormal) {
        EnableInventoryControlsOnly();
        DisableAdminAndOtherWriteControls();
    } else {
        EnableSearchOnly();
        DisableAllWriteAndAdminControls();
    }
}
```

---

## 6. File-by-File Privilege Enforcement Guidance

- **Data/Dao_System.cs:** Role loaded and cached at login/session. Centralize role check logic.
- **Data/Dao_User.cs & DAOs:** Add role check at start of every write. Use helper (e.g., `EnsureNotReadOnly()`).
- **Controls/SettingsForm/EditUserControl.cs:** Disable editing controls and check roles in event handlers.
- **Models/Model_Users.cs:** User model must reflect and synchronize user role.
- **Database:** `sys_roles` and `sys_user_roles` must match application logic; use seed/migration scripts.

---

## 7. MySQL Server Change Documentation

- [ ] **Schema Change Log**
  - **File:** `MYSQL_SERVER_CHANGES.md` (root)
  - **When:** Any change to schema, tables, roles, permissions, procedures, or anything in `mtm_wip_application`.
  - **Contents:** Summary, date, author, reason, SQL, notes/rollback.
  - **Update:** Before merging/releasing code that depends on MySQL changes.
- **Example Entry:**
  ```
  ## Add ReadOnly Role to sys_roles

  **Date:** 2025-07-12  
  **By:** Dorotel  
  **Reason:** To support granular user privilege enforcement.

  **SQL:**
  ```sql
  INSERT INTO sys_roles (RoleName) VALUES ('ReadOnly');
  ```

  **Notes:**  
  - Assign affected users via sys_user_roles.
  ```

---

## 8. Code/Feature Ownership & Workflow

- **Responsibility:**  
  - Refactor and all code areas (UI, DAO, business logic) handled by GitHub Copilot.
- **Review & Approval:**  
  - Dorotel reviews and approves all changes before merging.
- **Approach:**  
  - Default: all-at-once PR. If breaking changes/migration/staged needed, Copilot proposes incremental PRs.
- **Review Process:**  
  - Dorotel's approval required for merge. Adhere to this checklist.

---

## 9. Recommended Refactor & Maintenance Sequence

1. **Prep:** Review checklist, backup repo, create feature branch.
2. **Repo Structure & Docs:** Organize root, directories, config, docs, add `.editorconfig`, CI scripts.
3. **Code Refactor:** Apply standards, refactor files/namespaces, enforce code quality.
4. **Privilege System:** Standardize roles/tables, update models, centralize role loading.
5. **Enforcement:** Implement data/UI checks everywhere, log unauthorized attempts.
6. **Tests:** Set up projects, add/run tests for privilege/business logic.
7. **Advanced/CI:** Set up build/test automation, enforce policies, automate dependency updates.
8. **Final Review:** Manual/user testing for all roles, update docs, merge/tag/release.

---

## 10. Additional Implementation Details & Maintenance

- **Explicit code examples** for privilege enforcement (see Section 5).
- **Location of files:**  
  - `REPO_COMPREHENSIVE_CHECKLIST.md`, `MYSQL_SERVER_CHANGES.md`, `DATABASE_SCHEMA.sql` in root.
  - Further docs in `/docs` if needed.
- **Checklist Maintenance:**  
  - Dorotel responsible for keeping this up to date with every major change/release.
- **Updating Privilege Matrix:**  
  - If roles or permissions change, update matrix/rules, tests, docs, and `MYSQL_SERVER_CHANGES.md`.
  - Always keep `DATABASE_SCHEMA.sql` and documentation in sync with actual DB.

---

## 11. Database Schema Reference

- **File:** `DATABASE_SCHEMA.sql` at repo root.
- **Purpose:** Contains the **entire database server schema** (all tables, procs, triggers, structure), including for `mtm_wip_application` and any other relevant schemas.
- **Database Name:**  
  All application code, procedures, and privilege logic must access the MySQL database named:  
  `mtm_wip_application`
- **Usage:**  
  - Use as the single source of truth for all data access, privilege enforcement, and security logic.
  - Any schema or procedure change: update both `DATABASE_SCHEMA.sql` and `MYSQL_SERVER_CHANGES.md`.

---

> **This checklist is the authoritative source for all future development, refactor, onboarding, and maintenance for the MTM_WIP_Application project and database.**