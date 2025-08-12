# Copilot Reference: MTM_WIP_Application (MTM Inventory Application)

Last updated: 2025-01-27 14:30:00 UTC  
Repository: Dorotel/MTM_WIP_Application  
Primary language: C# (.NET 8, Windows Forms)  
Version: 5.0.1.2

This repository contains the MTM Inventory Application (WinForms) used to manage Work-In-Progress (WIP) inventory with a MySQL backend. The documentation is modularized for faster navigation and maintenance.

Quick start
- App type: Windows Forms (.NET 8)
- Database: MySQL 5.7.24+ (stored procedures only)
- Error handling: DaoResult<T> everywhere
- Theming: Centralized theme engine with DPI scaling
- Progress: Standard StatusStrip progress pattern
- Refactors: Must follow the Recursive Dependency Compliance Analysis workflow
- **Code Organization: Methods MUST be grouped in proper #regions with specific ordering**

## Code Organization Standards (MANDATORY)

**All C# files MUST follow this region organization pattern:**

### **Standard Region Order:**
1. **`#region Fields`** - Private fields, static instances, progress helpers
2. **`#region Properties`** - Public properties, getters/setters  
3. **`#region Progress Control Methods`** - SetProgressControls and progress-related methods
4. **`#region Constructors`** - Constructor and initialization
   - **`#region Initialization`** - Sub-region for complex initialization logic
5. **`#region [Specific Functionality]`** - Business logic regions (e.g., "Database Connectivity", "UI Events")
6. **`#region Key Processing`** - ProcessCmdKey and keyboard shortcuts
7. **`#region Button Clicks`** - Event handlers for button clicks
8. **`#region ComboBox & UI Events`** - UI event handlers and validation
9. **`#region Helpers`** or **`#region Private Methods`** - Helper and utility methods
10. **`#region Cleanup`** or **`#region Disposal`** - Cleanup and disposal methods

### **Method Ordering Within Regions:**
- **Public methods** first
- **Protected methods** second  
- **Private methods** third
- **Static methods** at the end of each access level
- **Async methods** grouped together when possible

### **Example Region Structure:**
```csharp
public partial class Control_ExampleTab : UserControl
{
    #region Fields
    
    private Helper_StoredProcedureProgress? _progressHelper;
    public static Forms.MainForm.MainForm? MainFormInstance { get; set; }
    
    #endregion

    #region Properties
    
    public bool IsDataLoaded { get; private set; }
    
    #endregion

    #region Progress Control Methods
    
    public void SetProgressControls(ToolStripProgressBar progressBar, ToolStripStatusLabel statusLabel)
    {
        _progressHelper = Helper_StoredProcedureProgress.Create(progressBar, statusLabel, 
            this.FindForm() ?? throw new InvalidOperationException("Control must be added to a form"));
    }
    
    #endregion

    #region Constructors
    
    public Control_ExampleTab()
    {
        InitializeComponent();
        Core_Themes.ApplyDpiScaling(this);
        Core_Themes.ApplyRuntimeLayoutAdjustments(this);
        WireUpEvents();
        ApplyPrivileges();
    }
    
    #endregion

    #region Database Operations
    
    public async Task<DaoResult<DataTable>> LoadDataAsync()
    {
        // Implementation using Helper_Database_StoredProcedure
    }
    
    private async Task<bool> ValidateDataAsync()
    {
        // Private database validation
    }
    
    #endregion

    #region Button Clicks
    
    private async void Button_Save_Click(object sender, EventArgs e)
    {
        // Button event handler
    }
    
    #endregion

    #region Helpers
    
    private void UpdateButtonStates()
    {
        // Helper method
    }
    
    #endregion
}
```

Authoritative directory structure
```
MTM_Inventory_Application/
├─ Controls/                     # All UserControl implementations
│  ├─ Addons/                    # Specialized controls (Connection strength, etc.)
│  ├─ MainForm/                  # Main application tabs and controls
│  ├─ SettingsForm/              # Settings dialog controls
│  └─ Shared/                    # Reusable controls
├─ Core/                         # Core application services
│  ├─ Core_Themes.cs             # Advanced theming and DPI scaling
│  ├─ Core_WipAppVariables.cs    # Application-wide constants
│  └─ Core_DgvPrinter.cs         # DataGridView printing utilities
├─ Data/                         # Data access layer (DAOs)
├─ Database/                     # Database scripts and stored procedures
│  └─ StoredProcedures/          # 74+ procedures with uniform p_ parameter naming
├─ Documentation/                # Comprehensive patch history and guides
│  ├─ Copilot Files/             # Modularized repo documentation (this index points here)
│  ├─ Patches/                   # Historical fix documentation (30+ patches)
│  └─ Guides/                    # Technical architecture and setup guides
├─ Forms/                        # Form definitions
├─ Helpers/                      # Utility classes and helpers
│  ├─ Helper_FileIO.cs           # File I/O operations
│  ├─ Helper_Json.cs             # JSON parsing/serialization
│  └─ Helper_UI_ComboBoxes.cs    # ComboBox management
├─ Logging/                      # Centralized logging system
├─ Models/                       # Data models and DTOs
├─ Services/                     # Background services and utilities
│  ├─ Service_Timer_VersionChecker.cs  # Version checking service
│  └─ Service_ErrorHandler.cs          # Error handling service
└─ Program.cs                    # Application entry point with comprehensive startup
```

Documentation index (modular files)
- 1–3 Overview and Architecture: (Documentation/Copilot Files/01-overview-architecture.md)
- 4, 6, 10 Patterns and Templates (DAO, UI, Theme, DGV/ComboBox): (Documentation/Copilot Files/04-patterns-and-templates.md)
- 5 Recent Major Improvements (Aug 2025): (Documentation/Copilot Files/05-improvements-and-changelog.md)
- 7, 9 Database and Stored Procedures + Versioning: (Documentation/Copilot Files/07-database-and-stored-procedures.md)
- 11 Error Handling and Logging: (Documentation/Copilot Files/11-error-handling-logging.md)
- 12 Startup and Lifecycle: (Documentation/Copilot Files/12-startup-lifecycle.md)
- 13–18 Utilities, Extensions, Business Logic, Testing, Performance, Troubleshooting: (Documentation/Copilot Files/13-18-utilities-and-troubleshooting.md)
- 19, 20 Quick Commands and File-Specific Guidance: (Documentation/Copilot Files/19-20-guides-and-commands.md)
- 21 Refactoring Workflow (Recursive Dependency Compliance Analysis, includes Online Refactor Mode prompt template): (Documentation/Copilot Files/21-refactoring-workflow.md)

Notes
- Place this README.md in the repository root.
- Place all linked files under Documentation/Copilot Files/ (as shown in your solution explorer image).
- For "single file refactors," we always produce a Pre-Refactor Report first and recursively analyze dependencies as described in the refactoring workflow. If you prefer to do the refactor entirely in the GitHub UI (not a local editor), see the "Online Refactor Mode" prompt template in the refactoring workflow doc.
- **ALL REFACTORS MUST include proper region organization and method ordering as specified above.**

---

Prompt Commands (Quick Copy/Paste)

Refactor and Dependency Analysis
- Analysis only (no code changes):
```
Analyze dependencies for refactoring file: <relative/path/FileName.cs>. Do not refactor yet.
```
- Full workflow (report first, then wait for approval):
```
Refactor file: <relative/path/FileName.cs>. Begin with full recursive dependency compliance report per docs/21-refactoring-workflow.md, then await my approval before making any changes. Include proper region organization and method ordering.
```
- Exclude items and regenerate report:
```
Exclude these files from the refactor scope: <list of files or globs>. Regenerate the Pre-Refactor Report.
```
- JSON report output:
```
Refactor file: <relative/path/FileName.cs>. Generate the Pre-Refactor Report in JSON format in addition to Markdown.
```
- Online refactor mode (do it on GitHub, not the editor):
```
Generate the MASTER REFRACTOR PROMPT (Online Mode) for file <relative/path/FileName.cs> with base branch <main|develop> and feature branch refactor/<file-stem>/<yyyymmdd>. Include all checklist items and deliverables from docs/21-refactoring-workflow.md.
```

Database and DAO
- Migrate a DAO method to DaoResult<T> with helper-based stored procedure call:
```
Migrate method <Dao_Class.MethodName> to DaoResult<T> using Helper_Database_StoredProcedure, no inline SQL, C# parameters without p_ prefix, and robust null-safety and logging. Organize with proper regions.
```
- Verify stored procedure contract and parameters:
```
Check stored procedure <sp_name> for OUT p_Status and OUT p_ErrorMsg and ensure C# call passes parameters without p_ prefix using the helper.
```

UI and Patterns
- Add progress reporting to a control method that performs DB I/O:
```
Add Helper_StoredProcedureProgress usage to <Control_Class.Method>, with ShowProgress/UpdateProgress/ShowSuccess/ShowError and proper try/catch/finally. Organize in appropriate regions.
```
- Create a new UserControl that follows all standards:
```
Create a new UserControl following Control_[TabName] template with theme application in constructor, ApplyPrivileges, keyboard shortcuts, progress controls, DaoResult-based data loading, and proper region organization.
```
- Null-safe DataGridView setup:
```
Generate a DataGridView setup method that is null-safe and applies theme and column ordering as per docs.
```

Utilities and Testing
- Form validation and combo box wiring:
```
Add standard ComboBox validation and event wiring to <Control_Class>, including color changes and UpdateButtonStates. Use proper region organization.
```
- Progress testing scenarios:
```
Create a test helper that exercises success, error, and warning flows of Helper_StoredProcedureProgress, including DaoResult failure simulation.
```

Docs and GitHub
- Split docs or update links:
```
Update Documentation/Copilot Files structure and refresh links in the root README to match the authoritative directory structure and new sections.
```
- Prepare a PR for documentation changes:
```
Create a PR to add/update the modular docs and root README links. Title: "Docs: Modularize README and add Refactoring Workflow". Base: main. Branch: docs/readme-split-<yyyymmdd>.
```

## Refactor Quality Checklist

When refactoring ANY file in this repository, ensure:

✅ **Region Organization**: Methods grouped in proper #regions following the standard order  
✅ **Method Ordering**: Public → Protected → Private → Static within each region  
✅ **DAO Compliance**: DaoResult<T> usage with Helper_Database_StoredProcedure  
✅ **Progress Reporting**: Helper_StoredProcedureProgress for UI database operations  
✅ **Error Handling**: Comprehensive try/catch with LoggingUtility  
✅ **Null Safety**: Never dereference potentially null objects  
✅ **Theme Compliance**: Core_Themes usage only in approved locations  
✅ **Database Standards**: Stored procedures with OUT p_Status, p_ErrorMsg  
✅ **Logging Standards**: Context-rich logging with start/end markers  
✅ **Thread Safety**: Proper Invoke usage for cross-thread operations  

**Non-compliance with region organization will require rework.**
