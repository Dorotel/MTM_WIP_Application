# Copilot Instructions for this repository

Always consult the root README.md and the docs under Documentation/Copilot Files/ before answering.

## Key Rules
- WinForms (.NET 8), MySQL stored procedures only
- Use DaoResult<T>, no inline SQL, C# params without p_ prefix (helper adds it)
- Follow UI progress, theming, and null-safety patterns
- **MANDATORY: All code must use proper #region organization and method ordering**

## Environment and Database Rules

### **Database File Structure** (CRITICAL)
- **Current\*** folders: Reference only - **DO NOT ALTER** (CurrentDatabase, CurrentServer, CurrentStoredProcedures)
- **Updated\*** folders: Active development - **USE FOR ALL CHANGES** (UpdatedDatabase, UpdatedStoredProcedures)

### **Environment-Specific Logic**
**Database Names:**
- Debug Mode: `mtm_wip_application_test`
- Release Mode: `mtm_wip_application`

**Server Addresses:**
- Release Mode: Always `172.16.1.104`
- Debug Mode: `172.16.1.104` if current IP matches, otherwise `localhost`

**Implementation:**
- Use `Model_Users.Database` and `Model_Users.WipServerAddress` properties
- These properties automatically handle environment detection
- Connection strings use `Helper_Database_Variables.GetConnectionString()`

## Code Organization Requirements (MANDATORY)

When refactoring or creating ANY C# file, MUST follow:

### **Standard Region Order:**
1. `#region Fields` - Private fields, static instances, progress helpers
2. `#region Properties` - Public properties, getters/setters  
3. `#region Progress Control Methods` - SetProgressControls and progress-related methods
4. `#region Constructors` - Constructor and initialization
5. `#region [Specific Functionality]` - Business logic regions
6. `#region Key Processing` - ProcessCmdKey and keyboard shortcuts
7. `#region Button Clicks` - Event handlers for button clicks
8. `#region ComboBox & UI Events` - UI event handlers and validation
9. `#region Helpers` or `#region Private Methods` - Helper and utility methods
10. `#region Cleanup` or `#region Disposal` - Cleanup and disposal methods

### **Method Ordering Within Regions:**
- Public methods first ? Protected methods ? Private methods ? Static methods

If a user asks for a refactor:
- Generate a Pre-Refactor Report first (see Documentation/Copilot Files/21-refactoring-workflow.md)
- **MUST include region organization analysis and planning**
- For online refactors, use the MASTER REFRACTOR PROMPT (Online Mode)
- **ALL refactored files MUST have proper region organization**

When unsure or context is missing:
- Ask the user to attach the specific file(s), or
- Search the workspace for the relevant files and sections

**Non-compliance with region organization will require rework.**

