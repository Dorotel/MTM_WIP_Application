# Copilot Instructions for this repository

Always consult the root README.md and the docs under Documentation/Copilot Files/ before answering.
Key rules:
- WinForms (.NET 8), MySQL stored procedures only
- Use DaoResult<T>, no inline SQL, C# params without p_ prefix (helper adds it)
- Follow UI progress, theming, and null-safety patterns
- **MANDATORY: All code must use proper #region organization and method ordering**

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

