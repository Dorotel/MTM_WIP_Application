# Copilot Reference: MTM_WIP_Application (MTM Inventory Application)

**Last updated:** 2025-08-11  
**Repository:** Dorotel/MTM_WIP_Application  
**Primary language:** C# (.NET 8, Windows Forms)  
**Purpose:** Comprehensive reference for GitHub Copilot and contributors with concrete, repo-specific context, conventions, and commands for effective development.

---

## 1. Project Overview

### What This Application Does
MTM Inventory Application is a Windows desktop application (WinForms) for managing WIP (Work In Progress) inventory. It provides:

- **Master Data Management:** Parts, Operations, Locations, Item Types, Users
- **Inventory Operations:** Add, Remove, Transfer inventory items with full audit trail
- **Transaction History:** Complete tracking of all inventory movements
- **User Management:** Role-based access control (Admin, Normal, Read-Only)
- **Advanced Theming:** Comprehensive theme system with DPI scaling support
- **Progress Reporting:** Standardized progress bars with color-coded status feedback
- **Database Integration:** MySQL stored procedures exclusively for all data operations
- **Quick Button System:** Personalized quick access to frequently used inventory combinations
- **Version Management:** Automatic version checking with semantic versioning support

### Primary Users and Goals
- **Inventory/Production Staff:** Add/remove/transfer inventory with clear visual feedback
- **Supervisors/Admins:** Manage master data and user roles with comprehensive controls
- **All Users:** Consistent UI experience with robust error handling and responsive forms

### Application Architecture
- **Windows Forms Desktop Application** targeting .NET 8
- **MySQL 5.7.24+ Database** with MAMP development support
- **Stored Procedure Only** database access pattern for security and consistency
- **Centralized Progress/Status System** using StatusStrip pattern throughout
- **Advanced Theming Engine** with real-time DPI scaling support
- **Uniform Parameter Naming** with automatic p_ prefix handling

---

## 2. Tech Stack and Runtime

### Core Technologies
- **.NET SDK:** .NET 8
- **C# Language Version:** C# 12
- **UI Framework:** Windows Forms (WinForms)
- **Target OS:** Windows 10/11
- **Database:** MySQL 5.7.24+ (MAMP-compatible for development)
- **Data Access:** ADO.NET with custom DAO pattern calling stored procedures exclusively

### Key Dependencies
- **MySql.Data.MySqlClient:** MySQL database connectivity
- **System.Text.Json:** JSON serialization (via Helper_Json only)
- **Custom Progress System:** `Helper_StoredProcedureProgress` for visual feedback
- **Advanced Theme Engine:** `Core_Themes` with comprehensive DPI scaling
- **File I/O Helper:** `Helper_FileIO` for all file operations
- **JSON Helper:** `Helper_Json` for all JSON parsing/serialization
- **Error Handling:** `Service_ErrorHandler` for standardized error management

### Development Environment
- **Visual Studio 2022** (recommended) with WinForms workload
- **MySQL Server** or **MAMP** for local development
- **Connection String Example (MAMP) as well as default Connection String:** 
  ```
  Server=localhost;Port=3306;Database=mtm_wip_application;Uid=root;Pwd=root;Allow User Variables=True;
  ```

---

## 3. Solution Architecture and Layout

### Directory Structure
```
MTM_Inventory_Application/
├── Controls/                     # All UserControl implementations
│   ├── Addons/                  # Specialized controls (Connection strength, etc.)
│   ├── MainForm/                # Main application tabs and controls
│   ├── SettingsForm/            # Settings dialog controls
│   └── Shared/                  # Reusable controls
├── Core/                        # Core application services
│   ├── Core_Themes.cs           # Advanced theming and DPI scaling
│   ├── Core_WipAppVariables.cs  # Application-wide constants
│   └── Core_DgvPrinter.cs       # DataGridView printing utilities
├── Data/                        # Data access layer (DAOs)
├── Database/                    # Database scripts and stored procedures
│   └── StoredProcedures/        # 74+ procedures with uniform p_ parameter naming
├── Documentation/               # Comprehensive patch history and guides
│   ├── Patches/                 # Historical fix documentation (30+ patches)
│   └── Guides/                  # Technical architecture and setup guides
├── Forms/                       # Form definitions
├── Helpers/                     # Utility classes and helpers
│   ├── Helper_FileIO.cs         # File I/O operations
│   ├── Helper_Json.cs           # JSON parsing/serialization
│   └── Helper_UI_ComboBoxes.cs  # ComboBox management
├── Logging/                     # Centralized logging system
├── Models/                      # Data models and DTOs
├── Services/                    # Background services and utilities
│   ├── Service_Timer_VersionChecker.cs # Version checking service
│   └── Service_ErrorHandler.cs  # Error handling service
└── Program.cs                   # Application entry point with comprehensive startup
```

### Naming Conventions
- **Controls:** `Control_[TabName]_[ControlType]_[Name]` (e.g., `Control_TransferTab_Button_Search`)
- **Methods:** `[ClassName]_[Action]_[Details]` (e.g., `Control_TransferTab_Button_Search_Click`)
- **Fields:** `_camelCase` with underscore prefix
- **Properties:** `PascalCase`
- **Constants:** `UPPER_CASE` with underscores
- **Events:** `[ControlName]_[EventType]` (e.g., `ComboBox_SelectedIndexChanged`)

---

## 4. Key Architectural Patterns

### Database Access Pattern (Stored Procedures Only)
```csharp
// STANDARD PATTERN - Use Helper_Database_StoredProcedure for all database operations
// IMPORTANT: Parameters passed WITHOUT p_ prefix (added automatically by helper)
var result = await Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus(
    Model_AppVariables.ConnectionString,
    "stored_procedure_name",
    new Dictionary<string, object>
    {
        ["Parameter1"] = value1, // NO "p_" prefix - added automatically
        ["Parameter2"] = value2,
        ["User"] = Model_AppVariables.User,
        ["DateTime"] = DateTime.Now
    },
    _progressHelper,        // Always include progress helper
    true                   // Use async
);

if (!result.IsSuccess)
{
    _progressHelper?.ShowError($"Error: {result.ErrorMessage}");
    return;
}
```

### File I/O and JSON Standards
- **Always use:** `Helper_FileIO` for all file operations
- **Always use:** `Helper_Json` for all JSON serialization/deserialization
- **Do not use:** `System.IO` or `System.Text.Json` directly in business logic

### Progress Reporting Pattern (StatusStrip Standard)
```csharp
// EVERY UserControl must implement this pattern
public partial class Control_[TabName] : UserControl
{
    private Helper_StoredProcedureProgress? _progressHelper;

    public void SetProgressControls(ToolStripProgressBar progressBar, ToolStripStatusLabel statusLabel)
    {
        _progressHelper = Helper_StoredProcedureProgress.Create(progressBar, statusLabel, 
            this.FindForm() ?? throw new InvalidOperationException("Control must be added to a form"));
    }

    private async Task StandardAsyncMethodPattern()
    {
        try
        {
            _progressHelper?.ShowProgress();
            _progressHelper?.UpdateProgress(10, "Starting operation...");

            // ... business logic ...

            _progressHelper?.ShowSuccess("Operation completed successfully!");
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            Service_ErrorHandler.HandleException(ex, nameof(StandardAsyncMethodPattern));
            _progressHelper?.ShowError($"Error: {ex.Message}");
        }
        finally
        {
            _progressHelper?.HideProgress();
        }
    }
}
```

### Theme Application Pattern
```csharp
// THEME POLICY: Only call theme methods in these specific locations:
// 1. UserControl/Form constructors
// 2. Settings dialog after theme change
// 3. DPI change event handlers

public Control_[TabName]()
{
    InitializeComponent();

    // REQUIRED: Apply DPI scaling and theme
    Core_Themes.ApplyDpiScaling(this);           // DPI scaling for all controls
    Core_Themes.ApplyRuntimeLayoutAdjustments(this); // Layout optimization
    Core_Themes.ApplyTheme(this);               // Theme application

    Initialize();
    ApplyPrivileges();
    SetupTooltips();
    WireUpEvents();
    LoadInitialData();
}
```

---

## 5. Anti-Patterns to Avoid

### Database Anti-Patterns
```csharp
// ❌ Do NOT do this:
using var cmd = new MySqlCommand("stored_procedure_name", connection);
cmd.CommandType = CommandType.StoredProcedure;
cmd.Parameters.AddWithValue("p_Parameter1", value1); // Causes p_p_Parameter1

// ❌ Avoid Direct MySQL usage in modern DAO
using var connection = new MySqlConnection(connectionString);
using var command = new MySqlCommand(sql, connection);

// ❌ Parameter prefixes with Helper_Database_StoredProcedure
["p_ParameterName"] = value  // Causes p_p_ParameterName
```

### Error Handling Anti-Patterns
```csharp
// ❌ Recursive error handling - can cause infinite loops
catch (Exception ex) {
    await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync); // Can cause loops
}

// ❌ Don't call error handlers recursively during startup
```

### File I/O and JSON Anti-Patterns
```csharp
// ❌ Do not use directly in business logic:
System.IO.File.ReadAllText(path);
System.Text.Json.JsonSerializer.Serialize(obj);

// ✅ Use instead:
Helper_FileIO.ReadAllText(path);
Helper_Json.Serialize(obj);
