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
│  ├─ CurrentDatabase/           # ⚠️  REFERENCE ONLY - Live production database snapshot
│  ├─ CurrentServer/             # ⚠️  REFERENCE ONLY - Live production server config  
│  ├─ CurrentStoredProcedures/   # ⚠️  REFERENCE ONLY - Live production procedures
│  ├─ UpdatedDatabase/           # ✅ ACTIVE - Development/test database structure
│  └─ UpdatedStoredProcedures/   # ✅ ACTIVE - 74+ procedures with uniform p_ parameter naming
├─ Documentation/                # Comprehensive patch history and guides
│  ├─ Copilot Files/             # Modularized repo documentation (this index points here)
│  ├─ Patches/                   # Historical fix documentation (30+ patches)
│  └─ Guides/                    # Technical architecture and setup guides
├─ Forms/                        # Form definitions
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
│  └─ Service_ErrorHandler.cs          # Error handling service
└─ Program.cs                    # Application entry point with comprehensive startup
```

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

## Comprehensive Documentation Index

### **Core Technical Documentation**
- **1–3 Overview and Architecture**: [Documentation/Copilot Files/01-overview-architecture.md](Documentation/Copilot%20Files/01-overview-architecture.md)
- **4, 6, 10 Patterns and Templates** (DAO, UI, Theme, DGV/ComboBox): [Documentation/Copilot Files/04-patterns-and-templates.md](Documentation/Copilot%20Files/04-patterns-and-templates.md)
- **5 Recent Major Improvements** (Aug 2025): [Documentation/Copilot Files/05-improvements-and-changelog.md](Documentation/Copilot%20Files/05-improvements-and-changelog.md)
- **7, 9 Database and Stored Procedures + Versioning**: [Documentation/Copilot Files/07-database-and-stored-procedures.md](Documentation/Copilot%20Files/07-database-and-stored-procedures.md)
- **11 Error Handling and Logging**: [Documentation/Copilot Files/11-error-handling-logging.md](Documentation/Copilot%20Files/11-error-handling-logging.md)
- **12 Startup and Lifecycle**: [Documentation/Copilot Files/12-startup-lifecycle.md](Documentation/Copilot%20Files/12-startup-lifecycle.md)
- **13–18 Utilities and Troubleshooting**: [Documentation/Copilot Files/13-18-utilities-and-troubleshooting.md](Documentation/Copilot%20Files/13-18-utilities-and-troubleshooting.md)
- **19, 20 Quick Commands and Guidance**: [Documentation/Copilot Files/19-20-guides-and-commands.md](Documentation/Copilot%20Files/19-20-guides-and-commands.md)
- **21 Refactoring Workflow**: [Documentation/Copilot Files/21-refactoring-workflow.md](Documentation/Copilot%20Files/21-refactoring-workflow.md)

### **Modern UI and Standards Documentation**
- **22 Modern UI Design Standards**: [Documentation/Copilot Files/22-modern-ui-standards.md](Documentation/Copilot%20Files/22-modern-ui-standards.md)
- **23 User Guide Integration**: [Documentation/Copilot Files/23-user-guide-integration.md](Documentation/Copilot%20Files/23-user-guide-integration.md)
- **24 Help System Architecture**: [Documentation/Copilot Files/24-help-system-architecture.md](Documentation/Copilot%20Files/24-help-system-architecture.md)
- **25 Service_ErrorHandler Standards**: [Documentation/Copilot Files/25-service-errorhandler-standards.md](Documentation/Copilot%20Files/25-service-errorhandler-standards.md)
- **26 Database Stored Procedure Compliance**: [Documentation/Copilot Files/26-database-stored-procedure-compliance.md](Documentation/Copilot%20Files/26-database-stored-procedure-compliance.md)

### **User Documentation and Guides**
- **Complete User Guide**: [Documentation/Guides/USER_GUIDE_COMPLETE.md](Documentation/Guides/USER_GUIDE_COMPLETE.md)
- **Inventory Operations Guide**: [Documentation/Guides/CONTROL_INVENTORY_TAB_GUIDE.md](Documentation/Guides/CONTROL_INVENTORY_TAB_GUIDE.md)
- **Remove Operations Guide**: [Documentation/Guides/CONTROL_REMOVE_TAB_GUIDE.md](Documentation/Guides/CONTROL_REMOVE_TAB_GUIDE.md)
- **Transfer Operations Guide**: [Documentation/Guides/CONTROL_TRANSFER_TAB_GUIDE.md](Documentation/Guides/CONTROL_TRANSFER_TAB_GUIDE.md)
- **Keyboard Shortcuts Reference**: [Documentation/Guides/KEYBOARD_SHORTCUTS_REFERENCE.md](Documentation/Guides/KEYBOARD_SHORTCUTS_REFERENCE.md)

### **Technical Architecture Guides**
- **Advanced Technical Architecture**: [Documentation/Guides/ADVANCED_TECHNICAL_ARCHITECTURE.md](Documentation/Guides/ADVANCED_TECHNICAL_ARCHITECTURE.md)
- **UI Redesign Patterns**: [Documentation/Guides/UI_REDESIGN_PATTERNS.md](Documentation/Guides/UI_REDESIGN_PATTERNS.md)
- **Environment Configuration**: [Documentation/Guides/ENVIRONMENT_CONFIGURATION.md](Documentation/Guides/ENVIRONMENT_CONFIGURATION.md)
- **Progress Control Implementation**: [Documentation/Guides/PROGRESS_CONTROL_IMPLEMENTATION.md](Documentation/Guides/PROGRESS_CONTROL_IMPLEMENTATION.md)
- **Modern Theme System**: [Documentation/Guides/MODERN_THEME_SYSTEM.md](Documentation/Guides/MODERN_THEME_SYSTEM.md)
- **Database Operations Guide**: [Documentation/Guides/DATABASE_OPERATIONS_GUIDE.md](Documentation/Guides/DATABASE_OPERATIONS_GUIDE.md)

### **Dependency Charts and Analysis**
- **Professional HTML Charts**: [Documentation/Dependency Charts/HTML/](Documentation/Dependency%20Charts/HTML/)
- **Markdown Dependency Charts**: [Documentation/Dependency Charts/](Documentation/Dependency%20Charts/)
- **PlantUML Architectural Diagrams**: [Documentation/PlantUML Files/](Documentation/PlantUML%20Files/)

### **Help System (HTML)**
- **Main Help Portal**: [Documentation/Help/index.html](Documentation/Help/index.html)
- **Getting Started Guide**: [Documentation/Help/getting-started.html](Documentation/Help/getting-started.html)
- **Keyboard Shortcuts**: [Documentation/Help/keyboard-shortcuts.html](Documentation/Help/keyboard-shortcuts.html)
- **Troubleshooting Guide**: [Documentation/Help/troubleshooting.html](Documentation/Help/troubleshooting.html)

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
✅ **Environment Compliance**: Use Model_Users properties for database/server selection  
✅ **File Structure Compliance**: Only modify Updated\* folders, never Current\* folders  
✅ **Logging Standards**: Context-rich logging with start/end markers  
✅ **Thread Safety**: Proper Invoke usage for cross-thread operations  

**Non-compliance with region organization or environment logic will require rework.**

## Modern UI Design Patterns

### **Responsive Design Standards**
The application follows modern UI design patterns inspired by the Transactions.html template:

- **Color Scheme**: Bootstrap-inspired colors with professional gradients
  - Primary: `#0d6efd` (Blue)
  - Success: `#198754` (Green) 
  - Warning: `#ffc107` (Yellow)
  - Danger: `#dc3545` (Red)
  - Dark: `#2c3e50` (Header backgrounds)

- **Typography**: Segoe UI font family with consistent sizing hierarchy
  - Headers: 2.5em (main), 2em (section), 1.5em (subsection)
  - Body text: 1em with 1.5 line height
  - Code elements: Consolas/Monaco monospace

- **Layout Patterns**: 
  - Three-panel layouts: 300px-1fr-320px with responsive breakpoints
  - Grid-based card layouts with consistent spacing (20px gaps)
  - Header bars with linear gradients (135deg)

- **Interactive Elements**:
  - Button hover effects with transform scaling (1.05)
  - Color-coded status indicators with icons
  - Progressive disclosure with collapsible sections

### **Component Library Standards**
- **Header Components**: Gradient backgrounds with title and subtitle areas
- **Navigation**: Card-based navigation with hover states
- **Data Tables**: Professional DataGridView with theme compliance
- **Forms**: Input validation with color-coded feedback
- **Progress Indicators**: StatusStrip integration with Helper_StoredProcedureProgress

### **Accessibility Compliance**
- **WCAG 2.1 AA**: Proper contrast ratios and keyboard navigation
- **Screen Reader Support**: Semantic HTML and ARIA labels
- **High DPI Support**: Core_Themes.ApplyDpiScaling() integration

## Troubleshooting Quick Start

### **Common Issues and Solutions**

#### **Database Connection Issues**
```bash
# Check environment configuration
Environment: Debug → mtm_wip_application_test, localhost/172.16.1.104
Environment: Release → mtm_wip_application, 172.16.1.104

# Verify connection string in Helper_Database_Variables.GetConnectionString()
# Check MySQL service status and credentials
```

#### **Build and Compilation Issues**
```bash
# Clean and rebuild solution
dotnet clean
dotnet restore  
dotnet build --configuration Release

# Check for missing references or packages
# Verify .NET 8 SDK installation
```

#### **UI Rendering and Theme Issues**
```csharp
// Ensure proper theme application in constructors:
Core_Themes.ApplyDpiScaling(this);
Core_Themes.ApplyRuntimeLayoutAdjustments(this);

// Check for null references in UI controls
// Verify UserControl inheritance and InitializeComponent() calls
```

#### **Progress Control Integration Issues**
```csharp
// Verify progress helper initialization:
_progressHelper = Helper_StoredProcedureProgress.Create(progressBar, statusLabel, parentForm);

// Check for proper ShowProgress/UpdateProgress/ShowSuccess patterns
// Ensure try/catch/finally blocks with progress cleanup
```

#### **Error Handling and Logging Issues**
```csharp
// Replace MessageBox.Show with Service_ErrorHandler:
Service_ErrorHandler.HandleException(ex, ErrorSeverity.Medium, 
    retryAction: () => RetryOperation(),
    contextData: new Dictionary<string, object> { ["UserId"] = userId });

// Check LoggingUtility integration and log file permissions
```

#### **Keyboard Shortcuts Not Working**
```csharp
// Verify ProcessCmdKey implementation:
protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
{
    // Handle F1, Ctrl+F1, Ctrl+Shift+K shortcuts
    return base.ProcessCmdKey(ref msg, keyData);
}
```

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

### **Integration with MainForm**
The help system is integrated into the MainForm MenuStrip with the following menu structure:
```
Help Menu:
├── Getting Started (Ctrl+F1)
├── User Guide (F1)  
├── Keyboard Shortcuts (Ctrl+Shift+K)
├── Troubleshooting Guide
├── System Requirements
├── About MTM Inventory (Ctrl+Alt+A)
```

## Development Forms Compliance Requirements

### **Forms/Development Folder Standards**
All forms in the `Forms/Development/` folder MUST comply with the following standards:

#### **Mandatory Region Organization**
```csharp
#region Fields
#region Properties  
#region Progress Control Methods
#region Constructors
#region [Specific Functionality] // e.g., "Dependency Analysis", "Chart Generation"
#region Key Processing
#region Button Clicks
#region ComboBox & UI Events
#region Helpers
#region Cleanup
```

#### **Error Handling Compliance**
- **MUST** use `Service_ErrorHandler` instead of `MessageBox.Show`
- **MUST** implement comprehensive try/catch blocks with context logging
- **MUST** use `DaoResult<T>` for all database operations
- **MUST** include retry mechanisms for recoverable errors

#### **Theme and UI Standards**
```csharp
// Required in constructor:
Core_Themes.ApplyDpiScaling(this);
Core_Themes.ApplyRuntimeLayoutAdjustments(this);
```

#### **Progress Integration Requirements**
```csharp
// Required for database operations:
_progressHelper = Helper_StoredProcedureProgress.Create(progressBar, statusLabel, this);
```

#### **Database Operation Standards**
- **MUST** use environment-aware database selection (`Model_Users.Database`)
- **MUST** use server address from `Model_Users.WipServerAddress`
- **MUST** follow stored procedure parameter conventions (no p_ prefix in C#)
- **MUST** handle OUT p_Status and p_ErrorMsg parameters

### **Development Forms Audit Checklist**
- [ ] **Region Organization**: Proper #region structure implemented
- [ ] **Error Handling**: Service_ErrorHandler integration complete
- [ ] **Theme Compliance**: Core_Themes applied in constructor
- [ ] **Progress Integration**: Helper_StoredProcedureProgress implemented
- [ ] **Database Standards**: Environment-aware connection logic
- [ ] **Method Ordering**: Public → Protected → Private → Static
- [ ] **Null Safety**: All potential null references handled
## Database Stored Procedure Verification Requirements

### **Stored Procedure Compliance Standards**
All stored procedures in the `Database/UpdatedStoredProcedures/` folder MUST meet these requirements:

#### **Parameter Naming Convention**
- **MySQL Parameters**: Use `p_` prefix (e.g., `p_UserID`, `p_PartNumber`)
- **C# Parameters**: Remove `p_` prefix (e.g., `UserID`, `PartNumber`)
- **Consistent Naming**: PascalCase for all parameter names
- **Required Outputs**: Every procedure MUST have `OUT p_Status INT` and `OUT p_ErrorMsg VARCHAR(500)`

#### **Standard Parameter Examples**
```sql
-- MySQL Stored Procedure Declaration:
PROCEDURE sp_GetUserByID(
    IN p_UserID INT,
    IN p_IncludeInactive BOOLEAN,
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(500)
)

-- C# Helper_Database_StoredProcedure Call:
var parameters = new Dictionary<string, object>
{
    ["UserID"] = userId,           // No p_ prefix
    ["IncludeInactive"] = includeInactive
};
```

#### **Status Code Standards**
```sql
-- Success codes:
SET p_Status = 0;   -- Success with data
SET p_Status = 1;   -- Success with no data found

-- Error codes:
SET p_Status = -1;  -- Invalid parameters
SET p_Status = -2;  -- Database constraint violation
SET p_Status = -3;  -- Record not found
SET p_Status = -4;  -- Permission denied
SET p_Status = -5;  -- General database error
```

#### **Error Message Standards**
```sql
-- Clear, user-friendly error messages:
SET p_ErrorMsg = 'User not found with the specified ID';
SET p_ErrorMsg = 'Invalid parameter: UserID cannot be null or zero';
SET p_ErrorMsg = 'Database connection error: Unable to execute query';
```

### **Verification Checklist for All Stored Procedures**
- [ ] **Parameter Naming**: Consistent p_ prefix in MySQL, removed in C#
- [ ] **Output Parameters**: OUT p_Status INT and OUT p_ErrorMsg VARCHAR(500) present
- [ ] **Error Handling**: All error conditions return appropriate status codes
- [ ] **Data Validation**: Input parameters validated with meaningful error messages
- [ ] **Transaction Safety**: Proper ROLLBACK on errors, COMMIT on success
- [ ] **Performance**: Optimized queries with proper indexing considerations
- [ ] **Security**: SQL injection prevention and parameter sanitization

### **Database Environment Logic Verification**
```csharp
// Verify environment-aware database selection:
#if DEBUG
    string database = "mtm_wip_application_test";     // Test database
    string server = GetLocalIpAddress() == "172.16.1.104" ? "172.16.1.104" : "localhost";
#else
    string database = "mtm_wip_application";          // Production database
    string server = "172.16.1.104";                  // Always production server
#endif
```

### **Deployment Verification Steps**
1. **Test Database First**: Always deploy to `mtm_wip_application_test` for validation
2. **Parameter Verification**: Confirm all C# calls match MySQL parameter names (without p_)
3. **Status Code Testing**: Test all error conditions return proper status codes
4. **Performance Testing**: Verify query execution time meets performance requirements
5. **Integration Testing**: Test with actual application forms and controls
6. **Production Deployment**: Only after successful test database validation

### **Compliance Audit Tools**
- **Automated Verification**: Scripts to validate parameter naming consistency
- **Performance Monitoring**: Query execution time tracking
- **Error Code Coverage**: Testing all possible status code paths
- **Integration Testing**: Form-level testing with actual stored procedure calls

---

### **Performance Optimization Tips**
- Use async/await for database operations
- Implement proper disposal patterns in #region Cleanup
- Cache frequently accessed data in static properties
- Use DaoResult<T> for efficient error handling without exceptions

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

---

## Recent Updates (January 27, 2025)

### Comprehensive Error Handling System Implementation
- ✅ **Service_ErrorHandler**: Complete centralized error handling system  
- ✅ **EnhancedErrorDialog**: UML-compliant error dialog with tabbed interface
- ✅ **MessageBox Replacement**: Systematic replacement of all MessageBox.Show calls
- ✅ **Automatic Logging**: Every error logged with caller context and rich debugging info
- ✅ **Connection Recovery**: Automatic database connection recovery for errors

### Help System Integration  
- ✅ **Modern Help System**: Responsive HTML help system with search functionality
- ✅ **MainForm Integration**: Help menu with keyboard shortcuts (F1, Ctrl+F1, Ctrl+Shift+K)
- ✅ **Comprehensive Guides**: User guides for all forms, controls, and operations
- ✅ **Technical Documentation**: Developer guides, dependency charts, and troubleshooting
- ✅ **Search Functionality**: Full-text search across all documentation

### Development Forms Compliance
- ✅ **Region Organization**: All Development forms meet mandatory #region standards
- ✅ **Error Handling**: Development forms use centralized Service_ErrorHandler system
- ✅ **Theme Integration**: Core_Themes.ApplyDpiScaling() and theme compliance
- ✅ **Progress Integration**: Helper_StoredProcedureProgress for database operations

### Environment-Specific Database and Server Logic Implementation
- ✅ **Database Selection**: Automatic Debug/Release mode database name selection
- ✅ **Server Selection**: Intelligent server address detection based on environment
- ✅ **Connection Logic**: Updated `Helper_Database_Variables` and `Model_Users` classes
- ✅ **Deployment Scripts**: Updated for test database by default with production options
- ✅ **Documentation**: Comprehensive documentation of file structure and environment logic
- ✅ **Compliance Templates**: Updated refactor templates and Copilot instructions

### File Structure Documentation
- ✅ **Clear Separation**: Current\* (reference only) vs Updated\* (active development)  
- ✅ **Deployment Safety**: Test database defaults to prevent production accidents
- ✅ **Development Workflow**: Streamlined development with environment-aware configuration

### Modern UI Design Patterns
- ✅ **Responsive Design**: Transactions.html modern UI template with gradient headers
- ✅ **Component Library**: Bootstrap-inspired color scheme (#0d6efd, #198754, #ffc107)
- ✅ **Typography Standards**: Segoe UI font family with consistent sizing hierarchy
- ✅ **Layout Patterns**: Three-panel layouts (300px-1fr-320px) with responsive breakpoints
- ✅ **Visual Elements**: Modern toolbar with hover effects, card-based layouts
- ✅ **Accessibility**: WCAG 2.1 AA compliance with proper contrast ratios

### Database Stored Procedure Verification
- ✅ **Parameter Standards**: Uniform p_ parameter naming convention across 74+ procedures
- ✅ **Output Compliance**: All procedures use OUT p_Status and OUT p_ErrorMsg
- ✅ **Connection Logic**: Environment-aware database selection (test/production)
- ✅ **Deployment Safety**: Updated procedures in UpdatedStoredProcedures folder only
- ✅ **Verification Tools**: Stored procedure compliance audit and verification scripts
