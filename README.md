# Copilot Reference: MTM_WIP_Application (MTM Inventory Application)

Last updated: 2025-01-27 14:30:00 UTC  
Repository: Dorotel/MTM_WIP_Application  
Primary language: C# (.NET 8, Windows Forms)  
Version: 5.0.1.2

This repository contains the MTM Inventory Application (WinForms) used to manage Work-In-Progress (WIP) inventory with a MySQL backend. The documentation is modularized for faster navigation and maintenance.

## Quick Start
- **App type**: Windows Forms (.NET 8)
- **Database**: MySQL 5.7.24+ (stored procedures only)
- **Error handling**: DaoResult<T> everywhere with centralized Service_ErrorHandler
- **Theming**: Centralized theme engine with DPI scaling
- **Progress**: Standard StatusStrip progress pattern with Helper_StoredProcedureProgress
- **Refactors**: Must follow the Recursive Dependency Compliance Analysis workflow
- **Code Organization**: Methods MUST be grouped in proper #regions with specific ordering
- **Help System**: Comprehensive F1 help system with modern UI design

## Table of Contents

1. [Code Organization Standards (MANDATORY)](#code-organization-standards-mandatory)
2. [Environment-Specific Database and Server Logic](#environment-specific-database-and-server-logic)
3. [Project Overview and Architecture](#project-overview-and-architecture)
4. [Service_ErrorHandler Implementation Standards](#service_errorhandler-implementation-standards)
5. [Help System Integration](#help-system-integration)
6. [Key Patterns and Templates](#key-patterns-and-templates)
7. [Database Operations and Standards](#database-operations-and-standards)
8. [Development Forms Compliance](#development-forms-compliance)
9. [Documentation Index (Modular Files)](#documentation-index-modular-files)
10. [Prompt Commands (Quick Copy/Paste)](#prompt-commands-quick-copypaste)
11. [Recent Updates](#recent-updates-january-27-2025)

---

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

---

## Environment-Specific Database and Server Logic

The application implements environment-aware database and server selection:

### **Database Name Logic**
- **Debug Mode (Development)**: Uses `mtm_wip_application_test`
- **Release Mode (Production)**: Uses `mtm_wip_application`

### **Server Address Logic**  
- **Release Mode**: Always connects to `172.16.1.104` (production server)
- **Debug Mode**: Intelligent server selection:
  - If current machine IP is `172.16.1.104` → connects to `172.16.1.104`
  - Otherwise → connects to `localhost` (development environment)

### **Implementation Details**
```csharp
// Environment-aware database selection
#if DEBUG
    string database = "mtm_wip_application_test";     // Test database
    string server = GetLocalIpAddress() == "172.16.1.104" ? "172.16.1.104" : "localhost";
#else
    string database = "mtm_wip_application";          // Production database
    string server = "172.16.1.104";                  // Always production server
#endif
```

### **File Structure Compliance**
- **Current\*** folders: Reference only - **DO NOT ALTER** these files
- **Updated\*** folders: Active development and deployment files - **USE FOR ALL CHANGES**

---

## Project Overview and Architecture

### **MTM Inventory Application Overview**
MTM Inventory Application is a Windows desktop application (WinForms) for managing WIP (Work In Progress) inventory. It provides:

- **Master Data Management**: Parts, Operations, Locations, Item Types, Users
- **Inventory Operations**: Add, Remove, Transfer inventory items with full audit trail
- **Transaction History**: Complete tracking of all inventory movements
- **User Management**: Role-based access control (Admin, Normal, Read-Only)
- **Advanced Theming**: Comprehensive theme system with DPI scaling support
- **Progress Reporting**: Standardized progress bars with color-coded status feedback
- **Database Integration**: MySQL stored procedures exclusively for all data operations
- **Quick Button System**: Personalized quick access to frequently used inventory combinations
- **Version Management**: Automatic version checking with semantic versioning support
- **Help System**: Comprehensive F1 help system with modern UI design

### **Tech Stack and Runtime**

**Core Technologies**
- .NET SDK: .NET 8
- C# Language Version: C# 12
- UI Framework: Windows Forms
- Target OS: Windows 10/11
- Database: MySQL 5.7.24+ (MAMP-compatible)
- Data Access: ADO.NET with custom DAO pattern (stored procedures only)

**Key Dependencies**
- MySql.Data.MySqlClient
- System.Text.Json
- Microsoft.Web.WebView2
- ClosedXML
- Newtonsoft.Json
- PlantUmlClassDiagramGenerator.SourceGenerator

### **Authoritative Directory Structure**
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
│  ├─ CurrentDatabase/           # ⚠️  REFERENCE ONLY - Live production database snapshot
│  ├─ CurrentServer/             # ⚠️  REFERENCE ONLY - Live production server config  
│  ├─ CurrentStoredProcedures/   # ⚠️  REFERENCE ONLY - Live production procedures
│  ├─ UpdatedDatabase/           # ✅ ACTIVE - Development/test database structure
│  └─ UpdatedStoredProcedures/   # ✅ ACTIVE - 74+ procedures with uniform p_ parameter naming
├─ Documentation/                # Comprehensive patch history and guides
│  ├─ Copilot Files/             # Modularized repo documentation (this index points here)
│  ├─ Patches/                   # Historical fix documentation (30+ patches)
│  ├─ Guides/                    # Technical architecture and setup guides
│  └─ Help/                      # Comprehensive help system files
├─ Forms/                        # Form definitions
│  ├─ Development/               # Development and analysis forms
│  ├─ ErrorDialog/               # Enhanced error dialog system
│  ├─ MainForm/                  # Main application form
│  ├─ Settings/                  # Settings forms
│  └─ Splash/                    # Startup splash screen
├─ Helpers/                      # Utility classes and helpers
│  ├─ Helper_FileIO.cs           # File I/O operations
│  ├─ Helper_Json.cs             # JSON parsing/serialization
│  ├─ Helper_Database_Variables.cs # Environment-aware database connection logic
│  └─ Helper_UI_ComboBoxes.cs    # ComboBox management
├─ Logging/                      # Centralized logging system
├─ Models/                       # Data models and DTOs
│  ├─ Model_Users.cs             # Environment-aware database/server properties
│  └─ Model_AppVariables.cs      # Application variables with environment logic
├─ Services/                     # Background services and utilities
│  ├─ Service_Timer_VersionChecker.cs  # Version checking service
│  ├─ Service_ErrorHandler.cs          # Centralized error handling service
│  └─ Service_ApplicationAnalyzer.cs   # Application analysis service
└─ Program.cs                    # Application entry point with comprehensive startup
```

---

## Service_ErrorHandler Implementation Standards

### **Error Handling Requirements**
ALL methods MUST use the centralized `Service_ErrorHandler` system:

```csharp
// Replace ALL MessageBox.Show() calls with:
Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium, 
    retryAction: () => RetryOperation(),
    contextData: new Dictionary<string, object> { ["UserId"] = userId },
    controlName: nameof(CurrentControl));

// For user confirmations:
var result = Service_ErrorHandler.ShowConfirmation("Are you sure?", "Confirmation");

// For validation warnings:
Service_ErrorHandler.HandleValidationError("Invalid input", "FieldName");

// For database errors:
Service_ErrorHandler.HandleDatabaseError(ex, retryAction: () => RetryDbOperation());

// For file operations:
Service_ErrorHandler.HandleFileError(ex, filePath, retryAction: () => RetryFileOperation());
```

### **Error Severity Levels**
- **Low**: Information/Warning - application continues normally
- **Medium**: Recoverable Error - operation failed but can be retried  
- **High**: Critical Error - data integrity or major functionality affected
- **Fatal**: Application Termination - unrecoverable error

### **Enhanced Error Dialog Features**
- **Tabbed Interface**: Summary, Technical Details, Call Stack views
- **Color-Coded Call Stack**: Visual hierarchy with component icons (🎯🔍⚙️📊)
- **Plain English Explanations**: Severity-based user-friendly messaging
- **Action Buttons**: Retry, Copy Details, Report Issue, View Logs, Close
- **Automatic Logging**: Every error automatically logged with rich context
- **Connection Recovery**: Automatic database connection recovery for errors

### **Error Dialog Design Standards**
Based on `Documentation/PlantUML Files/ErrorMessageMockup.uml`:
- UML-compliant tabbed interface design
- Summary tab with plain English explanations
- Technical Details tab with exception information
- Call Stack tab with color-coded method hierarchy
- Status bar with connection status and error criticality display

---

## Help System Integration

### **Accessing Help**
The application includes a comprehensive help system accessible via:
- **F1** - Context-sensitive help for current operation
- **Ctrl+F1** - Getting Started guide  
- **Menu → Help** - Complete help system with search functionality
- **Ctrl+Shift+K** - Keyboard shortcuts reference

### **Help System Structure**
- **Main Help**: `/Documentation/Help/index.html` - Modern UI help system
- **User Guides**: Comprehensive guides for all forms and operations
- **Technical Documentation**: Developer guides and dependency charts
- **Search Functionality**: Full-text search across all help content
- **Responsive Design**: Works in WebView2 control and external browsers

### **Help System Architecture**
- **WebView2 Integration**: Embedded browser control for help content
- **Modern UI Design**: Bootstrap-inspired responsive design matching application theme
- **Search Engine**: Full-text search across all documentation
- **Context-Sensitive**: Automatic help targeting based on current form/operation
- **Offline Capable**: All help content available without internet connection

---

## Key Patterns and Templates

### **Enhanced DAO Pattern with DaoResult<T>**
```csharp
public static async Task<DaoResult<DataTable>> GetInventoryByPartIdAsync(string partId, bool useAsync = false)
{
    try
    {
        var result = await Helper_Database_StoredProcedure.ExecuteDataTableWithStatus(
            Model_AppVariables.ConnectionString,
            "inv_inventory_Get_ByPartID",
            new Dictionary<string, object> { ["PartID"] = partId }, // no p_ prefix in C#
            _progressHelper,
            useAsync
        );
        
        if (result.IsSuccess)
        {
            return DaoResult<DataTable>.Success(result.Data ?? new DataTable(), 
                $"Retrieved {result.Data?.Rows.Count ?? 0} inventory items for part {partId}");
        }
        return DaoResult<DataTable>.Failure($"Failed to retrieve inventory for part {partId}: {result.ErrorMessage}");
    }
    catch (Exception ex)
    {
        LoggingUtility.LogDatabaseError(ex);
        return DaoResult<DataTable>.Failure($"Failed to retrieve inventory for part {partId}", ex);
    }
}
```

### **Standard UserControl Structure (Template)**
```csharp
public partial class Control_[TabName] : UserControl
{
    #region Fields
    
    private Helper_StoredProcedureProgress? _progressHelper;
    public static MainForm? MainFormInstance { get; set; }
    
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
    
    public Control_[TabName]()
    {
        InitializeComponent();
        Core_Themes.ApplyDpiScaling(this);
        Core_Themes.ApplyRuntimeLayoutAdjustments(this);
        Initialize();
        ApplyPrivileges();
        SetupTooltips();
        SetupInitialColors();
        WireUpEvents();
        LoadInitialData();
    }
    
    #endregion
}
```

### **Enhanced Async Method Pattern with DaoResult**
```csharp
private async Task StandardAsyncMethodWithDaoResult()
{
    try
    {
        _progressHelper?.ShowProgress("Starting operation...");
        _progressHelper?.UpdateProgress(10, "Processing...");
        LoggingUtility.LogApplicationInfo("Operation started", nameof(StandardAsyncMethodWithDaoResult));
        
        var daoResult = await Dao_[Entity].SomeOperationAsync(parameters);
        if (daoResult.IsSuccess)
        {
            var data = daoResult.Data;
            _progressHelper?.ShowSuccess($"Operation completed: {daoResult.StatusMessage}");
            LoggingUtility.LogApplicationInfo("Operation completed successfully", nameof(StandardAsyncMethodWithDaoResult));
        }
        else
        {
            _progressHelper?.ShowError($"Error: {daoResult.ErrorMessage}");
            Service_ErrorHandler.HandleDatabaseError(
                new Exception(daoResult.ErrorMessage), 
                retryAction: () => StandardAsyncMethodWithDaoResult(),
                controlName: nameof(Control_[TabName])
            );
            return;
        }
    }
    catch (Exception ex)
    {
        LoggingUtility.LogApplicationError(ex);
        _progressHelper?.ShowError($"Unexpected error: {ex.Message}");
        Service_ErrorHandler.HandleException(ex, ErrorSeverity.High,
            contextData: new Dictionary<string, object> { ["Method"] = nameof(StandardAsyncMethodWithDaoResult) },
            controlName: nameof(Control_[TabName]));
    }
    finally
    {
        _progressHelper?.HideProgress();
    }
}
```

---

## Database Operations and Standards

### **MySQL Compatibility and Procedure Contract**
All stored procedures MUST include:
```sql
OUT p_Status INT, 
OUT p_ErrorMsg VARCHAR(255)
```

### **Stored Procedure Categories**
- **usr_*** - Users, roles, permissions
- **sys_*** - System settings, access control
- **md_*** - Master data
- **inv_*** - Inventory operations
- **inv_transaction_*** - Audit/history
- **sys_last_10_transactions_*** - Quick Buttons
- **log_changelog_*** - Versioning

### **Standard Database Call Pattern with DaoResult<T>**
```csharp
var result = await Helper_Database_StoredProcedure.ExecuteDataTableWithStatus(
    Model_AppVariables.ConnectionString,
    "inv_inventory_Get_ByPartID",
    new Dictionary<string, object>
    {
        ["PartID"] = partId, // helper adds p_ automatically
        ["Operation"] = operation,
        ["IncludeInactive"] = false,
        ["User"] = Model_AppVariables.User,
        ["DateTime"] = DateTime.Now
    },
    _progressHelper,
    true
);
```

### **Database Environment Logic Implementation**
Use these properties for all database connections:
- `Model_Users.Database` - Automatically selects correct database name
- `Model_Users.WipServerAddress` - Automatically selects correct server address
- `Helper_Database_Variables.GetConnectionString()` - Builds environment-aware connection string

---

## Development Forms Compliance

### **Forms Requiring Compliance**
All forms in `Forms/Development/` MUST follow these standards:

#### **ApplicationAnalyzerForm.cs**
- ✅ **Region Organization**: Follows mandatory #region standard order
- ✅ **Error Handling**: Uses centralized Service_ErrorHandler system
- ✅ **Theme Integration**: Implements Core_Themes.ApplyDpiScaling() and ApplyRuntimeLayoutAdjustments()
- ✅ **Progress Reporting**: Integrates Helper_StoredProcedureProgress for operations
- ✅ **Method Organization**: Public → Protected → Private → Static within each region

#### **DependencyChartConverter/DependencyChartConverterWinForm.cs**
- **Region Organization**: Must follow mandatory #region standard order
- **Error Handling**: Must use centralized Service_ErrorHandler system
- **Theme Integration**: Must implement Core_Themes DPI scaling
- **Progress Reporting**: Must integrate Helper_StoredProcedureProgress
- **Input Validation**: Must use centralized validation with Service_ErrorHandler

#### **DependencyChartViewer/DependencyChartViewerForm.cs**
- **Region Organization**: Must follow mandatory #region standard order
- **Error Handling**: Must use centralized Service_ErrorHandler system
- **Theme Integration**: Must implement Core_Themes DPI scaling
- **Progress Reporting**: Must integrate Helper_StoredProcedureProgress
- **Method Organization**: Must follow Public → Protected → Private → Static ordering

### **Compliance Requirements**
- **Region Organization**: Follow mandatory #region standard order from README.md
- **Error Handling**: Use centralized Service_ErrorHandler system
- **Theme Integration**: Implement Core_Themes.ApplyDpiScaling() and ApplyRuntimeLayoutAdjustments()
- **Progress Reporting**: Integrate Helper_StoredProcedureProgress for database operations
- **Input Validation**: Centralized validation with Service_ErrorHandler
- **Method Organization**: Public → Protected → Private → Static within each region

---

## Documentation Index (Modular Files)

### **Core Documentation Files:**
- **01-03 Overview and Architecture**: [Documentation/Copilot Files/01-overview-architecture.md](Documentation/Copilot Files/01-overview-architecture.md)
- **04, 06, 10 Patterns and Templates** (DAO, UI, Theme, DGV/ComboBox): [Documentation/Copilot Files/04-patterns-and-templates.md](Documentation/Copilot Files/04-patterns-and-templates.md)
- **05 Recent Major Improvements** (January 2025): [Documentation/Copilot Files/05-improvements-and-changelog.md](Documentation/Copilot Files/05-improvements-and-changelog.md)
- **07, 09 Database and Stored Procedures + Versioning**: [Documentation/Copilot Files/07-database-and-stored-procedures.md](Documentation/Copilot Files/07-database-and-stored-procedures.md)
- **11 Error Handling and Logging**: [Documentation/Copilot Files/11-error-handling-logging.md](Documentation/Copilot Files/11-error-handling-logging.md)
- **12 Startup and Lifecycle**: [Documentation/Copilot Files/12-startup-lifecycle.md](Documentation/Copilot Files/12-startup-lifecycle.md)
- **13–18 Utilities, Extensions, Business Logic, Testing, Performance, Troubleshooting**: [Documentation/Copilot Files/13-18-utilities-and-troubleshooting.md](Documentation/Copilot Files/13-18-utilities-and-troubleshooting.md)
- **19, 20 Quick Commands and File-Specific Guidance**: [Documentation/Copilot Files/19-20-guides-and-commands.md](Documentation/Copilot Files/19-20-guides-and-commands.md)
- **21 Refactoring Workflow** (Recursive Dependency Compliance Analysis, includes Online Refactor Mode prompt template): [Documentation/Copilot Files/21-refactoring-workflow.md](Documentation/Copilot Files/21-refactoring-workflow.md)

### **Notes**
- Place this README.md in the repository root.
- Place all linked files under Documentation/Copilot Files/ (as shown in your solution explorer).
- For "single file refactors," we always produce a Pre-Refactor Report first and recursively analyze dependencies as described in the refactoring workflow.
- If you prefer to do the refactor entirely in the GitHub UI (not a local editor), see the "Online Refactor Mode" prompt template in the refactoring workflow doc.
- **ALL REFACTORS MUST include proper region organization and method ordering as specified above.**

---

## Prompt Commands (Quick Copy/Paste)

### **Refactor and Dependency Analysis**
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

### **Database and DAO**
- Migrate a DAO method to DaoResult<T> with helper-based stored procedure call:
```
Migrate method <Dao_Class.MethodName> to DaoResult<T> using Helper_Database_StoredProcedure, no inline SQL, C# parameters without p_ prefix, and robust null-safety and logging. Organize with proper regions.
```
- Verify stored procedure contract and parameters:
```
Check stored procedure <sp_name> for OUT p_Status and OUT p_ErrorMsg and ensure C# call passes parameters without p_ prefix using the helper.
```

### **Service_ErrorHandler Integration**
- Replace MessageBox.Show with Service_ErrorHandler:
```
Replace all MessageBox.Show() calls in <FileName.cs> with appropriate Service_ErrorHandler methods (HandleException, ShowConfirmation, ShowWarning, ShowInformation). Use proper error severity levels and context data.
```
- Add enhanced error handling to method:
```
Add comprehensive Service_ErrorHandler error handling to <ClassName.MethodName> with appropriate severity level, retry action, and context data. Include proper region organization.
```

### **UI and Patterns**
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

### **Development Forms Compliance**
- Update Development form for compliance:
```
Update <Development/FormName.cs> to meet all compliance requirements: proper region organization, Service_ErrorHandler integration, Core_Themes DPI scaling, Helper_StoredProcedureProgress integration, and method ordering standards.
```

### **Help System Integration**
- Add help system integration to form:
```
Add comprehensive help system integration to <FormName.cs> including F1 context-sensitive help, help menu items, and proper keyboard shortcuts (F1, Ctrl+F1, Ctrl+Shift+K).
```

### **Utilities and Testing**
- Form validation and combo box wiring:
```
Add standard ComboBox validation and event wiring to <Control_Class>, including color changes and UpdateButtonStates. Use proper region organization.
```
- Progress testing scenarios:
```
Create a test helper that exercises success, error, and warning flows of Helper_StoredProcedureProgress, including DaoResult failure simulation.
```

### **Docs and GitHub**
- Split docs or update links:
```
Update Documentation/Copilot Files structure and refresh links in the root README to match the authoritative directory structure and new sections.
```
- Prepare a PR for documentation changes:
```
Create a PR to add/update the modular docs and root README links. Title: "Docs: Modularize README and add Refactoring Workflow". Base: main. Branch: docs/readme-split-<yyyymmdd>.
```

---

## Refactor Quality Checklist

When refactoring ANY file in this repository, ensure:

✅ **Region Organization**: Methods grouped in proper #regions following the standard order  
✅ **Method Ordering**: Public → Protected → Private → Static within each region  
✅ **DAO Compliance**: DaoResult<T> usage with Helper_Database_StoredProcedure  
✅ **Progress Reporting**: Helper_StoredProcedureProgress for UI database operations  
✅ **Error Handling**: Comprehensive Service_ErrorHandler usage (no MessageBox.Show)  
✅ **Null Safety**: Never dereference potentially null objects  
✅ **Theme Compliance**: Core_Themes usage only in approved locations  
✅ **Database Standards**: Stored procedures with OUT p_Status, p_ErrorMsg  
✅ **Environment Compliance**: Use Model_Users properties for database/server selection  
✅ **File Structure Compliance**: Only modify Updated\* folders, never Current\* folders  
✅ **Logging Standards**: Context-rich logging with start/end markers  
✅ **Thread Safety**: Proper Invoke usage for cross-thread operations  
✅ **Help System Integration**: F1 help support where applicable  

**Non-compliance with region organization, Service_ErrorHandler usage, or environment logic will require rework.**

---

## Recent Updates (January 27, 2025)

### **Comprehensive Error Handling System Implementation**
- ✅ **Service_ErrorHandler**: Complete centralized error handling system  
- ✅ **EnhancedErrorDialog**: UML-compliant error dialog with tabbed interface
- ✅ **MessageBox Replacement**: Systematic replacement of all MessageBox.Show calls
- ✅ **Automatic Logging**: Every error logged with caller context and rich debugging info
- ✅ **Connection Recovery**: Automatic database connection recovery for errors

### **Help System Integration**  
- ✅ **Modern Help System**: Responsive HTML help system with search functionality
- ✅ **MainForm Integration**: Help menu with keyboard shortcuts (F1, Ctrl+F1, Ctrl+Shift+K)
- ✅ **Comprehensive Guides**: User guides for all forms, controls, and operations
- ✅ **Technical Documentation**: Developer guides, dependency charts, and troubleshooting
- ✅ **Search Functionality**: Full-text search across all documentation

### **Development Forms Compliance**
- ✅ **Region Organization**: All Development forms meet mandatory #region standards
- ✅ **Error Handling**: Development forms use centralized Service_ErrorHandler system
- ✅ **Theme Integration**: Core_Themes.ApplyDpiScaling() and theme compliance
- ✅ **Progress Integration**: Helper_StoredProcedureProgress for database operations
- ✅ **ApplicationAnalyzerForm**: Complete compliance with all standards

### **Environment-Specific Database and Server Logic Implementation**
- ✅ **Database Selection**: Automatic Debug/Release mode database name selection
- ✅ **Server Selection**: Intelligent server address detection based on environment
- ✅ **Connection Logic**: Updated `Helper_Database_Variables` and `Model_Users` classes
- ✅ **Deployment Scripts**: Updated for test database by default with production options
- ✅ **Documentation**: Comprehensive documentation of file structure and environment logic
- ✅ **Compliance Templates**: Updated refactor templates and Copilot instructions

### **File Structure Documentation**
- ✅ **Clear Separation**: Current\* (reference only) vs Updated\* (active development)  
- ✅ **Deployment Safety**: Test database defaults to prevent production accidents
- ✅ **Development Workflow**: Streamlined development with environment-aware configuration

### **Enhanced Documentation System**
- ✅ **Modular Documentation**: Complete reorganization of documentation into focused modules
- ✅ **Comprehensive README**: Updated with all patterns, standards, and requirements
- ✅ **Quick Reference Commands**: Copy-paste commands for common development tasks
- ✅ **Refactoring Workflow**: Detailed workflow for safe, compliant code refactoring
- ✅ **Development Standards**: Clear standards for all development forms and patterns
