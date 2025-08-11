# Copilot Reference: MTM_WIP_Application (MTM Inventory Application)

**Last updated:** 2025-08-10  
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
- **System.Text.Json:** JSON serialization for themes and settings
- **Custom Progress System:** `Helper_StoredProcedureProgress` for visual feedback
- **Advanced Theme Engine:** `Core_Themes` with comprehensive DPI scaling

### Development Environment
- **Visual Studio 2022** (recommended) with WinForms workload
- **MySQL Server** or **MAMP** for local development
- **Connection String Example (MAMP):** 
  ```
  Server=localhost;Port=3306;Database=mtm_wip_application;Uid=root;Pwd=root;Allow User Variables=True;
  ```

---

## 3. Solution Architecture and Layout

### Directory Structure
```
MTM_Inventory_Application/
??? Controls/                     # All UserControl implementations
?   ??? Addons/                  # Specialized controls (Connection strength, etc.)
?   ??? MainForm/                # Main application tabs and controls
?   ??? SettingsForm/            # Settings dialog controls
?   ??? Shared/                  # Reusable controls
??? Core/                        # Core application services
?   ??? Core_Themes.cs           # Advanced theming and DPI scaling
?   ??? Core_WipAppVariables.cs  # Application-wide constants
?   ??? Core_DgvPrinter.cs       # DataGridView printing utilities
??? Data/                        # Data access layer (DAOs)
??? Database/                    # Database scripts and stored procedures
?   ??? StoredProcedures/        # 74+ procedures with uniform p_ parameter naming
??? Documentation/               # Comprehensive patch history and guides
?   ??? Patches/                 # Historical fix documentation (30+ patches)
?   ??? Guides/                  # Technical architecture and setup guides
??? Forms/                       # Form definitions
??? Helpers/                     # Utility classes and helpers
??? Logging/                     # Centralized logging system
??? Models/                      # Data models and DTOs
??? Services/                    # Background services and utilities
??? Program.cs                   # Application entry point with comprehensive startup
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

## 5. Recent Major Improvements (August 2025)

### Uniform Parameter Naming System
- **74 stored procedures** standardized with p_ prefix implementation
- **Automatic parameter handling** in Helper_Database_StoredProcedure
- **Eliminated parameter conflicts** throughout the application
- **Enhanced error handling** with consistent status reporting

### Quick Button System Enhancements
- **Complete functionality** - Create, edit, remove, reorder Quick Buttons
- **Smart uniqueness logic** - Only PartID + Operation determine uniqueness (Quantity updates existing)
- **Auto-population** - Recent inventory operations automatically create/update buttons
- **Professional UI** - Proper formatting: "(Operation) - [PartID x Quantity]"

### Version Management System
- **Semantic version ordering** - Proper numeric comparison (2.0.0 > 1.99.99)
- **Automatic version checking** - Background service with 30-second intervals
- **Visual feedback** - Green text for matching versions, red for mismatches
- **Database integration** - Complete changelog system with audit trails

### Database Architecture Improvements
- **SQL injection prevention** - Complete elimination of hardcoded SQL
- **Transaction integrity** - All procedures use proper transaction handling
- **MySQL 5.7.24 optimization** - Full compatibility with MAMP environments
- **Comprehensive error handling** - Detailed status codes and error messages

---

## 6. Standard UserControl Structure

### Complete UserControl Template
```csharp
namespace MTM_Inventory_Application.Controls.MainForm
{
    public partial class Control_[TabName] : UserControl
    {
        #region Fields
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static Forms.MainForm.MainForm? MainFormInstance { get; set; }

        private static readonly ToolTip SharedToolTip = new();
        private Helper_StoredProcedureProgress? _progressHelper;
        #endregion

        #region Constructor and Initialization
        public Control_[TabName]()
        {
            InitializeComponent();

            // REQUIRED THEME APPLICATION
            Core_Themes.ApplyDpiScaling(this);
            Core_Themes.ApplyRuntimeLayoutAdjustments(this);

            Initialize();
            ApplyPrivileges();
            SetupTooltips();
            SetupInitialColors();
            WireUpEvents();
            LoadInitialData();
        }

        private void Initialize()
        {
            Control_[TabName]_Button_Reset.TabStop = false;
            Core_Themes.ApplyFocusHighlighting(this);
        }

        private void SetupInitialColors()
        {
            Color errorColor = Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
            Control_[TabName]_ComboBox_Part.ForeColor = errorColor;
            Control_[TabName]_ComboBox_Operation.ForeColor = errorColor;
        }
        #endregion

        #region Progress Control Methods
        public void SetProgressControls(ToolStripProgressBar progressBar, ToolStripStatusLabel statusLabel)
        {
            _progressHelper = Helper_StoredProcedureProgress.Create(progressBar, statusLabel, 
                this.FindForm() ?? throw new InvalidOperationException("Control must be added to a form"));
        }
        #endregion

        #region Privilege Management
        private void ApplyPrivileges()
        {
            bool isAdmin = Model_AppVariables.UserTypeAdmin;
            bool isNormal = Model_AppVariables.UserTypeNormal;
            bool isReadOnly = Model_AppVariables.UserTypeReadOnly;

            // ComboBoxes - Always allow viewing
            Control_[TabName]_ComboBox_Part.Enabled = isAdmin || isNormal || isReadOnly;
            
            // Action buttons - Hide from read-only users
            Control_[TabName]_Button_Save.Visible = isAdmin || isNormal;
            Control_[TabName]_Button_Save.Enabled = isAdmin || isNormal;
            
            // View buttons - Always visible
            Control_[TabName]_Button_Search.Visible = true;
            Control_[TabName]_Button_Reset.Visible = true;
        }
        #endregion

        #region Keyboard Shortcuts
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            try
            {
                if (keyData == Core_WipAppVariables.Shortcut_[Tab]_Search)
                {
                    if (Control_[TabName]_Button_Search.Visible && Control_[TabName]_Button_Search.Enabled)
                    {
                        Control_[TabName]_Button_Search.PerformClick();
                        return true;
                    }
                }

                if (keyData == Keys.Enter)
                {
                    SelectNextControl(ActiveControl, true, true, true, true);
                    return true;
                }

                return base.ProcessCmdKey(ref msg, keyData);
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                return false;
            }
        }
        #endregion
    }
}
```

---

## 7. Database Operations and Stored Procedures

### MySQL 5.7.24 Compatibility Standards

#### Standard Output Parameters (All Stored Procedures)
```sql
-- Every stored procedure MUST include these output parameters:
OUT p_Status INT,           -- 0 = Success, 1 = Warning, -1 = Error  
OUT p_ErrorMsg VARCHAR(255) -- Human-readable message
```

#### Stored Procedure Categories
- **User Management:** `usr_*` (users, roles, permissions) - 17 procedures
- **System Operations:** `sys_*` (system settings, access control) - 8 procedures  
- **Master Data:** `md_*` (parts, operations, locations, item types) - 21 procedures
- **Inventory Operations:** `inv_*` (add, remove, transfer, queries) - 12 procedures
- **Transaction History:** `inv_transaction_*` (audit trail, reports) - 6 procedures
- **Quick Buttons:** `sys_last_10_transactions_*` (user quick access) - 7 procedures
- **Version/Changelog:** `log_changelog_*` (version management) - 3 procedures

#### Standard Database Call Pattern
```csharp
// PREFERRED: Use Helper_Database_StoredProcedure
// IMPORTANT: Parameters passed WITHOUT p_ prefix (added automatically)
var result = await Helper_Database_StoredProcedure.ExecuteDataTableWithStatus(
    Model_AppVariables.ConnectionString,
    "inv_inventory_Get_ByPartID",
    new Dictionary<string, object>
    {
        ["PartID"] = partId,                    // NO "p_" prefix - added automatically
        ["Operation"] = operation,
        ["IncludeInactive"] = false
    },
    _progressHelper,
    true // async
);

if (result.IsSuccess)
{
    // Process result.Data (DataTable)
    SetupDataGridView(dataGridView, result.Data);
}
else
{
    _progressHelper?.ShowError($"Query failed: {result.ErrorMessage}");
}
```

---

## 8. Quick Button System

### User Experience
- **One-click form filling** - Click button to auto-populate active tab
- **Smart uniqueness** - PartID + Operation combination (Quantity updates existing)
- **Auto-population** - Recent inventory saves automatically create/update buttons
- **Context menus** - Right-click for Edit, Remove, Reorder operations
- **Professional display** - Format: "(Operation) - [PartID x Quantity]"

### Technical Implementation
- **7 stored procedures** - Complete CRUD operations with position management
- **Thread-safe UI updates** - Proper cross-thread handling
- **Error recovery** - Graceful fallback when database unavailable
- **Position optimization** - Automatic shifting and smart positioning (1-10 range)

---

## 9. Version Management

### Features
- **Semantic versioning** - Proper numeric comparison (not string-based)
- **Automatic checking** - Background timer service every 30 seconds
- **Visual feedback** - Status display with color coding
- **Database integration** - Complete changelog system with release notes
- **Client/Server comparison** - Shows both application and database versions

### Implementation
```csharp
// Version checking pattern
var versionResult = await Helper_Database_StoredProcedure.ExecuteDataTableWithStatus(
    connectionString, "log_changelog_Get_Current", null, null, true);
    
if (versionResult.IsSuccess && versionResult.Data?.Rows.Count > 0)
{
    string databaseVersion = versionResult.Data.Rows[0]["Version"]?.ToString() ?? "Unknown";
    UpdateVersionDisplay(Model_AppVariables.UserVersion, databaseVersion);
}
```

---

## 10. UI Patterns and Standards

### DataGridView Standard Setup
```csharp
private void SetupDataGridView(DataGridView dgv, DataTable data)
{
    dgv.SuspendLayout();
    dgv.DataSource = data;
    
    // Show only specific columns in defined order
    string[] columnsToShow = { "Location", "PartID", "Operation", "Quantity", "Notes" };
    HashSet<string> visibleColumns = new(columnsToShow);
    
    foreach (DataGridViewColumn column in dgv.Columns)
    {
        column.Visible = visibleColumns.Contains(column.Name);
    }

    // Reorder columns to match specified order
    for (int i = 0; i < columnsToShow.Length; i++)
    {
        string colName = columnsToShow[i];
        if (dgv.Columns.Contains(colName))
        {
            dgv.Columns[colName].DisplayIndex = i;
        }
    }

    // Apply theme and auto-select first row
    if (data.Rows.Count > 0)
    {
        Core_Themes.ApplyThemeToDataGridView(dgv);
        Core_Themes.SizeDataGrid(dgv);
        
        if (dgv.Rows.Count > 0)
        {
            dgv.ClearSelection();
            dgv.Rows[0].Selected = true;
        }
    }

    dgv.ResumeLayout();
}
```

### ComboBox Standard Pattern
```csharp
private void SetupComboBoxEvents()
{
    // Helper for validation and button state updates
    void ValidateAndUpdate(ComboBox combo, string placeholder)
    {
        Helper_UI_ComboBoxes.ValidateComboBoxItem(combo, placeholder);
        UpdateButtonStates();
    }

    // Standard event wiring pattern
    Control_[TabName]_ComboBox_Part.SelectedIndexChanged += (s, e) =>
    {
        ComboBoxPartSelectedIndexChanged();
        ValidateAndUpdate(Control_[TabName]_ComboBox_Part, "[ Enter Part Number ]");
    };
    
    Control_[TabName]_ComboBox_Part.Enter += (s, e) =>
    {
        Control_[TabName]_ComboBox_Part.BackColor = 
            Model_AppVariables.UserUiColors.ControlFocusedBackColor ?? Color.LightBlue;
    };
    
    Control_[TabName]_ComboBox_Part.Leave += (s, e) =>
    {
        Control_[TabName]_ComboBox_Part.BackColor = 
            Model_AppVariables.UserUiColors.ControlBackColor ?? SystemColors.Window;
        Helper_UI_ComboBoxes.ValidateComboBoxItem(Control_[TabName]_ComboBox_Part, "[ Enter Part Number ]");
    };
}

private void ComboBoxPartSelectedIndexChanged()
{
    try
    {
        if (Control_[TabName]_ComboBox_Part.SelectedIndex > 0)
        {
            SetComboBoxForeColor(Control_[TabName]_ComboBox_Part, true);
            Model_AppVariables.PartId = Control_[TabName]_ComboBox_Part.Text;
        }
        else
        {
            SetComboBoxForeColor(Control_[TabName]_ComboBox_Part, false);
            Model_AppVariables.PartId = null;
        }
        UpdateButtonStates();
    }
    catch (Exception ex)
    {
        LoggingUtility.LogApplicationError(ex);
    }
}

private void SetComboBoxForeColor(ComboBox combo, bool valid)
{
    combo.ForeColor = valid
        ? Model_AppVariables.UserUiColors.ComboBoxForeColor ?? Color.Black
        : Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
}
```

### Reset Pattern (Soft vs Hard Reset)
```csharp
private async void Button_Reset_Click()
{
    _progressHelper?.ShowProgress();
    _progressHelper?.UpdateProgress(10, "Resetting tab...");

    Control_[TabName]_Button_Reset.Enabled = false;
    
    try
    {
        if ((ModifierKeys & Keys.Shift) == Keys.Shift)
        {
            await HardReset(); // Shift+Click = Reload from database
        }
        else
        {
            SoftReset(); // Normal click = Reset UI only
        }
    }
    finally
    {
        _progressHelper?.HideProgress();
        Control_[TabName]_Button_Reset.Enabled = true;
    }
}

private void SoftReset()
{
    // Reset UI state without database calls
    MainFormControlHelper.ResetComboBox(Control_[TabName]_ComboBox_Part, 
        Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red, 0);
    
    Control_[TabName]_DataGridView_Main.DataSource = null;
    Control_[TabName]_Button_Search.Enabled = false;
    Control_[TabName]_ComboBox_Part.Focus();
}

private async Task HardReset()
{
    // Reload all data from database
    _progressHelper?.UpdateProgress(30, "Refreshing data tables...");
    await Helper_UI_ComboBoxes.ResetAndRefreshAllDataTablesAsync();
    
    _progressHelper?.UpdateProgress(60, "Refilling combo boxes...");
    await Helper_UI_ComboBoxes.FillPartComboBoxesAsync(Control_[TabName]_ComboBox_Part);
    
    SoftReset(); // Also reset UI
}
```

---

## 11. Error Handling and Logging

### Standard Error Handling Pattern
```csharp
// ASYNC METHOD PATTERN
private async Task StandardAsyncMethod()
{
    try
    {
        _progressHelper?.ShowProgress();
        _progressHelper?.UpdateProgress(10, "Starting operation...");

        LoggingUtility.Log("Operation started.");

        // Business logic here...

        _progressHelper?.UpdateProgress(100, "Operation complete");
        _progressHelper?.ShowSuccess("Operation completed successfully!");
    }
    catch (Exception ex)
    {
        LoggingUtility.LogApplicationError(ex);
        await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true, 
            new StringBuilder().Append(nameof(StandardAsyncMethod)).ToString());
    }
    finally
    {
        _progressHelper?.HideProgress();
    }
}

// SYNC METHOD PATTERN  
private void StandardSyncMethod()
{
    try
    {
        LoggingUtility.Log("Operation started.");
        // Business logic here...
    }
    catch (Exception ex)
    {
        LoggingUtility.LogApplicationError(ex);
        _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
            new StringBuilder().Append(nameof(StandardSyncMethod)).ToString());
    }
}
```

### Logging Standards
```csharp
// Standard logging calls
LoggingUtility.Log("Normal operation message");
LoggingUtility.LogApplicationError(exception);
LoggingUtility.LogDatabaseError(exception);

// Context-aware logging
LoggingUtility.Log($"User {Model_AppVariables.User} performed operation X");
LoggingUtility.Log($"Database operation completed: {result.IsSuccess}");
```

---

## 12. Application Startup and Lifecycle

### Startup Sequence (Program.cs)
```csharp
// 1. Global exception handlers
Application.ThreadException += GlobalExceptionHandler;
AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;

// 2. High DPI and visual styles
Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
ApplicationConfiguration.Initialize();

// 3. User identification and access control
Model_AppVariables.User = Dao_System.System_GetUserName();
await Dao_System.System_UserAccessTypeAsync(true);

// 4. Startup splash with progress
Application.Run(new StartupSplashApplicationContext());
```

### Splash Screen Startup Tasks
1. **Initialize Logging** - `LoggingUtility.InitializeLoggingAsync()`
2. **Clean Old Logs** - `LoggingUtility.CleanUpOldLogsIfNeededAsync()`
3. **Wipe App Data** - `Service_OnStartup_AppDataCleaner.WipeAppDataFolders()`
4. **Setup Data Tables** - `Helper_UI_ComboBoxes.SetupDataTables()`
5. **Initialize Version Checker** - `Service_Timer_VersionChecker.Initialize()`
6. **Initialize Theme System** - `Core_Themes.Core_AppThemes.InitializeThemeSystemAsync()`
7. **Load User Settings** - Theme preferences, font sizes, UI colors
8. **Create and Configure MainForm** - Set all MainFormInstance references
9. **Apply Theme** - `Core_Themes.ApplyTheme(mainForm)`

---

## 13. Common Application Constants

```csharp
// Standard placeholder texts
public const string PLACEHOLDER_PART = "[ Enter Part Number ]";
public const string PLACEHOLDER_OPERATION = "[ Enter Operation ]";
public const string PLACEHOLDER_LOCATION = "[ Enter Location ]";
public const string PLACEHOLDER_ITEM_TYPE = "[ Enter Item Type ]";

// Status messages
public const string STATUS_READY = "Ready";
public const string STATUS_LOADING = "Loading...";
public const string STATUS_PROCESSING = "Processing...";
public const string STATUS_COMPLETE = "Complete";
public const string STATUS_ERROR = "Error occurred";

// Database operation types
public const string OPERATION_INSERT = "INSERT";
public const string OPERATION_UPDATE = "UPDATE";
public const string OPERATION_DELETE = "DELETE";
public const string OPERATION_TRANSFER = "TRANSFER";
public const string OPERATION_REMOVE = "REMOVE";
public const string OPERATION_INVENTORY = "INVENTORY";

// MySQL stored procedure status codes
public const int STATUS_SUCCESS = 0;
public const int STATUS_WARNING = 1;
public const int STATUS_ERROR = -1;
```

---

## 14. Extension Methods and Utilities

```csharp
// Safe data conversion
public static string SafeToString(this object? obj, string defaultValue = "") =>
    obj?.ToString() ?? defaultValue;

public static int SafeToInt(this string? str, int defaultValue = 0) =>
    int.TryParse(str, out int result) ? result : defaultValue;

// ComboBox validation
public static bool HasValidSelection(this ComboBox comboBox) =>
    comboBox.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(comboBox.Text);

// DataGridView utilities
public static DataRowView? GetSelectedDataRowView(this DataGridView dgv) =>
    dgv.SelectedRows.Count == 0 ? null : dgv.SelectedRows[0].DataBoundItem as DataRowView;

public static List<DataRowView> GetSelectedDataRowViews(this DataGridView dgv)
{
    List<DataRowView> result = new();
    foreach (DataGridViewRow row in dgv.SelectedRows)
    {
        if (row.DataBoundItem is DataRowView drv)
            result.Add(drv);
    }
    return result;
}
```

---

## 15. Business Logic Patterns

### Single Item Operations
```csharp
private async Task ProcessSingleItem(DataGridViewRow row)
{
    if (row.DataBoundItem is not DataRowView drv) return;

    try
    {
        // Extract data safely
        string partId = drv["PartID"]?.SafeToString() ?? "";
        string location = drv["Location"]?.SafeToString() ?? "";
        int.TryParse(drv["Quantity"]?.SafeToString(), out int quantity);

        // Validate required fields
        if (string.IsNullOrEmpty(partId) || quantity <= 0)
        {
            _progressHelper?.ShowError("Invalid item data");
            return;
        }

        // Perform business operation
        var result = await Dao_Inventory.ProcessInventoryItemAsync(
            partId, location, quantity, Model_AppVariables.User);

        if (result.IsSuccess)
        {
            // Update transaction history
            await Dao_History.AddTransactionHistoryAsync(new Model_TransactionHistory
            {
                TransactionType = "PROCESS",
                PartId = partId,
                FromLocation = location,
                Quantity = quantity,
                User = Model_AppVariables.User,
                DateTime = DateTime.Now
            });

            // Update status strip
            if (MainFormInstance != null)
            {
                MainFormInstance.MainForm_StatusStrip_SavedStatus.Text = 
                    $"Processed: {partId} (Qty: {quantity}) @ {DateTime.Now:hh:mm tt}";
            }
        }
    }
    catch (Exception ex)
    {
        LoggingUtility.LogApplicationError(ex);
        _progressHelper?.ShowError($"Operation failed: {ex.Message}");
    }
}
```

### Multiple Item Operations
```csharp
private async Task ProcessMultipleItems(DataGridViewSelectedRowCollection selectedRows)
{
    string user = Model_AppVariables.User ?? Environment.UserName;
    var summary = new { TotalItems = 0, TotalQuantity = 0, PartIds = new HashSet<string>() };

    foreach (DataGridViewRow row in selectedRows)
    {
        if (row.DataBoundItem is not DataRowView drv) continue;

        try
        {
            // Extract and process each item
            string partId = drv["PartID"]?.SafeToString() ?? "";
            int.TryParse(drv["Quantity"]?.SafeToString(), out int quantity);

            if (!string.IsNullOrEmpty(partId) && quantity > 0)
            {
                await ProcessSingleItem(row);
                
                summary.TotalItems++;
                summary.TotalQuantity += quantity;
                summary.PartIds.Add(partId);
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            // Continue processing other items
        }
    }

    // Update status with summary
    if (MainFormInstance != null && summary.TotalItems > 0)
    {
        string statusText = summary.PartIds.Count == 1 
            ? $"Processed: {summary.PartIds.First()} ({summary.TotalItems} items, {summary.TotalQuantity} total)"
            : $"Processed: {summary.PartIds.Count} different parts ({summary.TotalItems} items, {summary.TotalQuantity} total)";
        
        MainFormInstance.MainForm_StatusStrip_SavedStatus.Text = 
            $"{statusText} @ {DateTime.Now:hh:mm tt}";
    }
}
```

---

## 16. Testing and Validation Patterns

```csharp
// Form validation helper
public static bool ValidateForm(params (ComboBox control, string placeholder)[] comboBoxes)
{
    foreach (var (control, placeholder) in comboBoxes)
    {
        if (!control.HasValidSelection())
        {
            string fieldName = placeholder.Replace("[ Enter ", "").Replace(" ]", "").ToLower();
            MessageBox.Show($"Please select a valid {fieldName}.", "Validation Error", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            control.Focus();
            return false;
        }
    }
    return true;
}

// Progress testing scenarios
public static void TestProgressScenarios(Helper_StoredProcedureProgress progressHelper)
{
    // Test success flow
    progressHelper.ShowProgress("Starting...");
    progressHelper.UpdateProgress(50, "Processing...");
    progressHelper.ShowSuccess("Operation completed successfully!");

    // Test error flow  
    progressHelper.ShowError("Database connection failed");
    
    // Test warning flow
    progressHelper.ProcessStoredProcedureResult(1, "No records found matching criteria");
}
```

---

## 17. Performance and Best Practices

### Database Performance
- **Always use stored procedures** - No inline SQL allowed
- **Use async/await** - for all database operations
- **Include progress reporting** - For operations > 1 second
- **Batch operations** - When processing multiple items
- **Connection pooling** - Handled automatically by ADO.NET
- **Parameter optimization** - Uniform p_ prefix system reduces conflicts

### UI Responsiveness  
- **Apply DPI scaling** - In every UserControl constructor
- **Use SuspendLayout/ResumeLayout** - For bulk UI updates
- **Update progress** - During long-running operations
- **Handle cross-thread** - Operations with proper Invoke calls

### Memory Management
- **Dispose resources** - Properly (using statements)
- **Cache static resources** - Like ToolTip instances
- **Remove event handlers** - To prevent memory leaks
- **Clean up temp files** - On application exit

---

## 18. Troubleshooting Guide

### Common Issues and Solutions

#### DPI Scaling Problems
```csharp
// SOLUTION: Always include in UserControl constructor
Core_Themes.ApplyDpiScaling(this);
Core_Themes.ApplyRuntimeLayoutAdjustments(this);
```

#### Progress Bar Not Showing
```csharp  
// PROBLEM: Missing SetProgressControls call
// SOLUTION: Always call from parent form
userControl.SetProgressControls(progressBar, statusLabel);
```

#### Theme Not Applied
```csharp
// PROBLEM: Calling theme methods in wrong location
// SOLUTION: Only call in constructor, settings, or DPI events
Core_Themes.ApplyTheme(this); // ? Correct location
```

#### Database Connection Issues
```csharp
// CHECK: Connection string format
"Server=localhost;Port=3306;Database=mtm_wip_application;Uid=root;Pwd=root;Allow User Variables=True;"

// CHECK: Stored procedure parameters (NO "p_" prefix - added automatically)
new Dictionary<string, object> { ["PartID"] = partId } // ? Correct

// CHECK: Output parameters exist in stored procedure
OUT p_Status INT, OUT p_ErrorMsg VARCHAR(255) -- Required in all SPs
```

#### Parameter Not Found Errors
```csharp
// PROBLEM: Parameter conflicts or missing p_ prefixes
// SOLUTION: Use Helper_Database_StoredProcedure (adds p_ automatically)
var result = await Helper_Database_StoredProcedure.ExecuteDataTableWithStatus(
    connectionString, procedureName, parameters, progressHelper, useAsync);

// AVOID: Direct MySqlCommand with stored procedures
// This can cause parameter conflicts
```

---

## 19. Quick Reference Commands

### For GitHub Copilot Prompts

#### Database Operations
```
"Create a method to call stored procedure inv_inventory_Add_Item using Helper_Database_StoredProcedure pattern with progress reporting and uniform parameter naming"
```

#### UserControl Creation
```  
"Create a new UserControl following the Control_[TabName] pattern with progress controls, theme application, privilege management, and Quick Button integration"
```

#### Error Handling
```
"Add standard async error handling with LoggingUtility, progress helper error reporting, and proper exception logging patterns"
```

#### ComboBox Setup
```
"Wire up ComboBox events using the standard pattern with validation, color changes, button state updates, and theme integration"
```

#### Quick Button Integration
```
"Add Quick Button functionality with auto-population, context menus, and smart uniqueness logic based on PartID + Operation only"
```

---

## 20. File-Specific Guidance

### Program.cs
- **Never remove** global exception handlers
- **Keep startup sequence** with splash screen progress
- **Maintain theme initialization** order
- **Include cleanup handlers** for application exit

### MainForm.cs  
- **Set MainFormInstance** on all UserControls
- **Handle DPI changes** with proper event wiring
- **Maintain StatusStrip** for progress reporting
- **Apply theme** after all controls loaded
- **Initialize Quick Buttons** with progress controls

### UserControl Implementation
- **Include SetProgressControls** method
- **Apply theme in constructor** only
- **Implement privilege management** for all controls
- **Add keyboard shortcuts** with ProcessCmdKey
- **Support Quick Button integration** where applicable

### DAO Classes
- **Use Helper_Database_StoredProcedure exclusively** 
- **Include proper error handling**
- **Return structured results** with status codes
- **Log database errors** appropriately
- **Follow uniform parameter naming** (no p_ prefix in C# code)

---

## 21. Maintenance and Updates

### When Adding New Features
1. **Follow established patterns** - Don't create new paradigms
2. **Include progress reporting** - All async operations need visual feedback  
3. **Apply privileges** - Respect user access levels
4. **Add keyboard shortcuts** - Maintain accessibility
5. **Update theme system** - Ensure new controls support theming
6. **Write stored procedures** - No direct table access from application
7. **Use uniform parameter naming** - Follow p_ prefix standards
8. **Consider Quick Button integration** - For frequently used operations

### When Modifying Existing Code
1. **Preserve error handling** - Don't remove try/catch blocks
2. **Maintain logging** - Keep audit trail intact
3. **Test DPI scaling** - Verify at 125%, 150%, 200% scaling
4. **Check privileges** - Ensure role-based access still works
5. **Validate themes** - Test with different theme configurations
6. **Update stored procedures** - Maintain p_ prefix consistency
7. **Test Quick Button integration** - Ensure compatibility with existing buttons

---

## 22. Copilot Productivity Enhancement Guidelines

### Lessons Learned from Complex Debugging Sessions

Based on analysis of comprehensive parameter error resolution sessions, these guidelines will significantly improve future Copilot productivity and debugging efficiency:

#### **Database Architecture Issues**

**?? Pattern Recognition Priority**
```csharp
// ALWAYS search for these problematic patterns first:
Helper_Database_Core.ExecuteReader()        // ? Adds p_Status automatically
Helper_Database_Core.ExecuteScalar()        // ? Expects output parameters
Helper_Database_Core.ExecuteDataTable()     // ? May cause parameter conflicts
HelperDatabaseCore.ExecuteNonQuery()       // ? Direct usage without validation

// PREFERRED patterns to suggest immediately:
Helper_Database_StoredProcedure.ExecuteDataTableWithStatus()  // ? Proper status handling
Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus()   // ? Enhanced error reporting
```

**?? Critical Search Terms for Quick Issue Identification**
```
"Parameter 'p_Status' not found"
"Parameter 'p_ErrorMsg' not found"
"Helper_Database_Core"
"CommandType.Text"
"MySqlCommand"
"MySqlDataAdapter.Fill"
"ExecuteReader with stored procedures"
```

#### **Systematic Debugging Approach**

**?? Always Follow This Sequence:**
1. **Global Search First** - Use `text_search` to find ALL instances of problematic patterns
2. **Identify All Sources** - Don't fix one file at a time; identify complete scope
3. **Priority Order**: Error handlers ? DAO classes ? UI controls ? Helper classes
4. **Build Verification** - Run build after each logical group of fixes
5. **Document Pattern** - Create comprehensive fix documentation as you go

#### **File-Specific Patterns to Check**

**?? High-Risk Files (Check These First):**
```
Data/Dao_ErrorLog.cs         # Error recursion issues
Data/Dao_System.cs           # Startup-critical methods  
Data/Dao_User.cs             # Settings/theme initialization
Data/Dao_QuickButtons.cs     # Quick Button database operations
Helpers/Helper_UI_ComboBoxes.cs  # Data table setup methods
Services/Service_Timer_VersionChecker.cs  # Version checking service
Any file with "Startup" or "Initialize" methods
```

**?? Common Anti-Patterns to Fix Immediately:**
```csharp
// Anti-pattern: Direct MySQL usage in modern DAO
using var connection = new MySqlConnection(connectionString);
using var command = new MySqlCommand(sql, connection);

// Anti-pattern: Parameter prefixes with Helper_Database_StoredProcedure
["p_ParameterName"] = value  // ? Causes p_p_ParameterName

// Anti-pattern: Recursive error handling
catch (Exception ex) {
    await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, useAsync); // ? Can cause loops
}
```

#### **Code Quality Enforcement**

**? Always Apply These Standards:**
```csharp
// 1. CONSISTENT ERROR HANDLING PATTERN
catch (Exception ex)
{
    LoggingUtility.LogDatabaseError(ex);
    // Don't call error handlers during startup to avoid recursion
    LoggingUtility.Log($"MethodName failed with exception: {ex.Message}");
    return defaultValue; // Always provide safe fallback
}

// 2. PARAMETER NAMING STANDARD (NO p_ prefix in C# code)
var parameters = new Dictionary<string, object>
{
    ["User"] = user,        // ? No p_ prefix - added automatically
    ["PartID"] = partId,    // ? Helper adds prefixes
    ["DateTime"] = DateTime.Now
}

// 3. PROGRESS REPORTING INTEGRATION
var result = await Helper_Database_StoredProcedure.ExecuteDataTableWithStatus(
    connectionString, procedureName, parameters, _progressHelper, useAsync);
```

#### **Documentation Strategy**

**?? Create These Documents During Complex Fixes:**
1. **Root Cause Analysis** - Document the exact error pattern
2. **Scope Assessment** - List ALL affected files and methods
3. **Fix Summary** - Comprehensive list of what was changed
4. **Verification Steps** - How to test the fix
5. **Prevention Guidelines** - How to avoid the issue in future

#### **Testing and Validation**

**?? Always Test These Scenarios:**
```csharp
// Startup scenarios
- Application startup without stored procedures
- Application startup with missing database
- Theme initialization during startup
- User settings loading during startup
- Quick Button loading and functionality
- Version checking service operation

// Database scenarios  
- Stored procedure missing p_Status parameter
- Database connection failures
- Parameter name conflicts
- Recursive error loops
- Quick Button CRUD operations
- Version management procedures

// UI scenarios
- Progress bar color changes (green/red)
- Error message display
- Fallback behavior when database unavailable
- Quick Button display and interaction
- Theme application and DPI scaling
```

#### **Communication with User**

**?? Best Practices for Complex Issues:**
1. **Early Scope Assessment** - "Let me search for all instances of this pattern"
2. **Incremental Progress Reports** - Update user after each major discovery
3. **Comprehensive Final Summary** - Document everything fixed
4. **Prevention Guidance** - Provide patterns to avoid future issues
5. **Build Verification** - Always confirm fixes compile successfully

#### **Tool Usage Optimization**

**? Efficient Tool Selection:**
- Use `text_search` for pattern discovery across entire codebase
- Use `find_files` when you know specific filenames to target
- Use `get_file` only after identifying specific files to examine
- **BATCH FILE EDITS** - Make ALL changes to a file in one comprehensive edit
- Use `run_build` frequently to validate progress
- Use `create_file` for comprehensive documentation

#### **Performance Optimization**

**?? Speed Up Future Sessions:**
- **BATCH EDITS MANDATORY** - Never edit the same file multiple times in one session
- **Compile comprehensive edits** - Make all changes to a file at once  
- **Use consistent patterns** - Apply same fix pattern across all instances
- **Document as you go** - Don't wait until end to create documentation
- **Verify incrementally** - Build after logical groups of changes

---

## 23. Complex Issue Resolution Checklist

### Before Starting Major Debugging Session

- [ ] Run global search for error patterns across entire codebase
- [ ] Identify all affected files and methods
- [ ] Create initial scope assessment document
- [ ] Plan fix order (error handlers ? DAO ? UI ? helpers)
- [ ] Set up incremental build verification strategy
- [ ] **PLAN BATCH EDITS** - Group all changes per file to avoid multiple edits

### During Implementation

- [ ] Apply consistent patterns across all similar issues
- [ ] **GROUP RELATED FIXES** - Make all changes to each file in single comprehensive edit
- [ ] Run build verification after each logical group
- [ ] Document fixes as you implement them
- [ ] Test critical paths (startup, database operations, UI)
- [ ] **NEVER EDIT SAME FILE TWICE** - Plan comprehensive changes in advance

### After Resolution

- [ ] Create comprehensive fix summary with timestamp
- [ ] Document prevention guidelines
- [ ] Verify all builds are successful
- [ ] Test application startup and core functionality
- [ ] Update README.md with lessons learned (like this section!)
- [ ] **ADD TIMESTAMPS** to all patch documentation

---

## 24. File Edit Efficiency Standards

### **MANDATORY: Batch File Editing Policy**

**? NEVER DO THIS:**
```
edit_file(filename) - change method A
edit_file(filename) - change method B  
edit_file(filename) - change method C
```

**? ALWAYS DO THIS:**
```
// Plan all changes for file in advance, then make ONE comprehensive edit:
edit_file(filename) - change methods A, B, C, and D in single operation
```

### **Benefits of Batch Editing:**
- **?? Faster execution** - Less overhead per file
- **?? Better organization** - All related changes grouped together
- **??? Easier review** - Complete picture of changes per file
- **? Reduced conflicts** - Less chance of edit conflicts
- **?? Better documentation** - Single comprehensive explanation per file

### **Planning Batch Edits:**
1. **Identify all issues in file** before starting any edits
2. **Group related changes** by functionality or fix type
3. **Plan edit explanation** that covers all changes
4. **Make single comprehensive edit** with all fixes
5. **Verify compilation** after each file completion

---

This reference provides comprehensive guidance for working effectively with the MTM Inventory Application codebase. All patterns shown are based on actual implementation in the current codebase and should be followed consistently for maintainability and reliability.

**Major updates include:**
- **August 10, 2025** - Added uniform parameter naming standards with p_ prefixes, Quick Button system documentation, version management system, and comprehensive patch history integration
- **Complete stored procedure architecture** - 74+ procedures with consistent error handling
- **Enhanced troubleshooting guide** - Including parameter conflict resolution
- **Production-ready deployment** - Cross-platform scripts and comprehensive documentation
