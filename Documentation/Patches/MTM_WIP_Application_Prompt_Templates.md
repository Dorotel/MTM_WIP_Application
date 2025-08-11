# MTM WIP Application - GitHub Copilot Prompt Templates

**Last updated:** 2025-01-13  
**Repository:** Dorotel/MTM_WIP_Application  
**Purpose:** Comprehensive collection of prompt templates for GitHub Copilot to generate consistent code following established patterns in the MTM Inventory Application.

---

## Table of Contents

1. [UserControl Creation Templates](#usercontrol-creation-templates)
2. [Database Operation Templates](#database-operation-templates)
3. [UI Component Templates](#ui-component-templates)
4. [Error Handling Templates](#error-handling-templates)
5. [Theme and DPI Templates](#theme-and-dpi-templates)
6. [Business Logic Templates](#business-logic-templates)
7. [Testing and Validation Templates](#testing-and-validation-templates)
8. [Form Design Templates](#form-design-templates)
9. [Data Model Templates](#data-model-templates)
10. [Service and Helper Templates](#service-and-helper-templates)

---

## UserControl Creation Templates

### 1. Create Standard UserControl
```
Create a new UserControl named Control_[TabName] following the established MTM pattern with:
- Standard constructor with Core_Themes.ApplyDpiScaling, Core_Themes.ApplyRuntimeLayoutAdjustments
- Progress helper integration with SetProgressControls method
- MainFormInstance static property
- ApplyPrivileges method for role-based access control
- ProcessCmdKey method for keyboard shortcuts
- Standard initialization methods: Initialize(), SetupTooltips(), SetupInitialColors(), WireUpEvents(), LoadInitialData()
- Proper error handling with LoggingUtility and try-catch blocks
- Uses Helper_StoredProcedureProgress for visual feedback
```

### 2. Create Settings Form UserControl
```
Create a settings form UserControl named Control_[Operation]_[Entity] following the MTM pattern for managing [Entity] data with:
- Form validation using the standard pattern with red error colors
- Database operations using Helper_Database_StoredProcedure with progress reporting
- ComboBox setup with placeholder text "[ Enter [Entity] ]"
- Button state management and privilege checking
- Event handling for Save/Cancel/Reset operations
- Integration with the StatusStrip progress system
- Proper field validation with focus management
```

### 3. Create MainForm Tab UserControl
```
Create a MainForm tab UserControl named Control_[TabName]Tab following the established pattern with:
- DataGridView setup with Core_Themes.ApplyThemeToDataGridView and Core_Themes.SizeDataGrid
- ComboBox event wiring with validation and color changes
- Search functionality with async database calls
- Reset functionality supporting both soft and hard reset (Shift+Click)
- Transfer/Add/Remove operations with progress reporting
- Panel toggle functionality for right panel collapse/expand
- Print functionality using Core_DgvPrinter
- Multi-row selection support with summary status updates
```

---

## Database Operation Templates

### 4. Create Database DAO Method
```
Create a DAO method in [DaoClass] to call stored procedure [procedure_name] using the Helper_Database_StoredProcedure pattern with:
- Async/await pattern with proper exception handling
- Parameters dictionary without "p_" prefix (added automatically)
- Progress reporting integration for UI feedback
- Result validation and error handling
- LoggingUtility for database errors
- Return StoredProcedureResult<T> with IsSuccess property
- Proper connection string from Model_AppVariables.ConnectionString
```

### 5. Create Data Retrieval Method
```
Create an async method to retrieve [entity] data using Helper_Database_StoredProcedure.ExecuteDataTableWithStatus with:
- Stored procedure call to [procedure_name] 
- Progress reporting from 10% to 100%
- Error handling with red progress bar on failure
- Success handling with green progress bar and record count
- DataTable result processing
- Integration with DataGridView setup if needed
- Logging for database operations
```

### 6. Create Data Modification Method
```
Create an async method to [operation] [entity] using Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus with:
- Input validation with progress helper error reporting
- Stored procedure call with proper parameters
- Progress updates at 10%, 30%, 60%, and 100%
- Success/failure feedback with color-coded progress bars
- Transaction history logging if applicable
- MainFormInstance status strip updates
- Proper exception handling and logging
```

---

## UI Component Templates

### 7. Create ComboBox Setup
```
Wire up ComboBox events for Control_[TabName]_ComboBox_[Name] using the standard MTM pattern with:
- SelectedIndexChanged event for validation and button state updates
- Enter event for focused background color (ControlFocusedBackColor)
- Leave event for normal background color (ControlBackColor)  
- Color validation: valid selections use ComboBoxForeColor, invalid use ComboBoxErrorForeColor
- Helper_UI_ComboBoxes.ValidateComboBoxItem calls
- UpdateButtonStates() calls after changes
- Error logging with try-catch blocks
```

### 8. Create DataGridView Setup Method
```
Create a method SetupDataGridView for Control_[TabName]_DataGridView_[Name] following the standard pattern with:
- SuspendLayout/ResumeLayout for performance
- Column visibility control using defined columnsToShow array
- Column reordering by DisplayIndex
- Core_Themes.ApplyThemeToDataGridView for theme application
- Core_Themes.SizeDataGrid for sizing
- Auto-select first row with proper selection clearing
- SelectionChanged event for row-based logic
```

### 9. Create Button State Management
```
Create UpdateButtonStates method following the Control_TransferTab pattern with:
- Enable/disable logic based on ComboBox selections (SelectedIndex > 0)
- DataGridView row count and selection validation
- Business logic validation (e.g., same location checks)
- Privilege-based button visibility and enablement
- Complex state management with boolean flags
- Try-catch error handling with LoggingUtility
- Support for multiple button states and dependencies
```

---

## Error Handling Templates

### 10. Create Standard Async Method with Error Handling
```
Create an async method [MethodName] following the standard MTM error handling pattern with:
- _progressHelper?.ShowProgress() at start
- Progress updates at key points (10%, 30%, 60%, 100%)
- LoggingUtility.Log for operation tracking
- Input validation with _progressHelper?.ShowError for failures
- Database operations with result.IsSuccess checking
- Success feedback with _progressHelper?.ShowSuccess
- Exception handling with LoggingUtility.LogApplicationError
- Dao_ErrorLog.HandleException_GeneralError_CloseApp for critical errors
- Finally block with _progressHelper?.HideProgress()
```

### 11. Create Sync Method with Error Handling  
```
Create a synchronous method [MethodName] following the standard MTM pattern with:
- Try-catch block structure
- LoggingUtility.Log for operation start
- Business logic implementation
- Exception handling with LoggingUtility.LogApplicationError
- Dao_ErrorLog.HandleException_GeneralError_CloseApp with async=false
- Method name logging using StringBuilder pattern
```

### 12. Create Form Validation Method
```
Create a validation method ValidateForm for Control_[TabName] with:
- Input validation for required fields
- ComboBox.HasValidSelection() extension method usage
- TextBox null/empty checking
- Focus management for invalid fields
- MessageBox.Show with ValidationError icon and title
- Return boolean for validation success/failure
- Error color application for invalid controls
- Progress helper error reporting for validation failures
```

---

## Theme and DPI Templates

### 13. Apply Theme to UserControl
```
Create theme application code for Control_[TabName] following the THEME POLICY with:
- Core_Themes.ApplyDpiScaling(this) in constructor only
- Core_Themes.ApplyRuntimeLayoutAdjustments(this) in constructor only
- Core_Themes.ApplyFocusHighlighting(this) for focus management
- Model_AppVariables.UserUiColors for color theming
- SetupInitialColors method for ComboBox error colors
- Proper theme application timing (constructor, settings, DPI change only)
- No theme calls in arbitrary event handlers
```

### 14. Create DPI Scaling Support
```
Add DPI scaling support to [ControlName] following the MTM pattern with:
- Core_Themes.ApplyDpiScaling call in constructor
- Core_Themes.ApplyRuntimeLayoutAdjustments for layout optimization
- SuspendLayout/ResumeLayout during bulk operations
- Proper control hierarchy scaling
- Runtime layout adjustments for TableLayoutPanel, GroupBox, SplitContainer
- DPI change event handling if needed
- Margin and padding adjustments for scaling
```

---

## Business Logic Templates

### 15. Create Single Item Processing
```
Create a method ProcessSingle[Entity] following the MTM business logic pattern with:
- DataRowView extraction from DataGridView row
- Safe data extraction using SafeToString() extension methods
- Input validation with progress helper error reporting  
- Database operation using appropriate DAO method
- Transaction history logging with Model_TransactionHistory
- MainFormInstance status strip updates with operation summary
- Error handling with operation continuation for batch processing
- Success feedback with detailed operation information
```

### 16. Create Multiple Item Processing
```
Create a method ProcessMultiple[Entities] for batch operations with:
- Loop through DataGridViewSelectedRowCollection
- Summary tracking (TotalItems, TotalQuantity, PartIds HashSet)
- Individual item processing with error isolation
- Continue processing on individual item errors
- Aggregated status strip updates with summary information
- Complex status text based on operation diversity
- Progress reporting for batch operation completion
- Transaction history for each successful operation
```

### 17. Create Transfer Operation
```
Create a transfer operation method following the Control_TransferTab pattern with:
- Source and destination validation
- Quantity validation and bounds checking
- Single vs multiple row handling
- Partial quantity transfer logic
- Full quantity transfer logic
- Transaction history recording
- Status strip updates with transfer summary
- Business rule validation (e.g., same location checks)
- Progress reporting throughout operation
```

---

## Testing and Validation Templates

### 18. Create Progress Testing Scenarios
```
Create test methods for Helper_StoredProcedureProgress following the standard pattern with:
- TestErrorScenarios: database connection, validation, general errors
- TestSuccessScenarios: progress flow from start to completion
- TestWarningScenarios: non-fatal issues and warnings
- Progress bar color validation (red for errors, green for success)
- Status text format validation with proper prefixes
- Progress percentage validation and bounds checking
- Cross-thread operation testing with Invoke requirements
```

### 19. Create Form Validation Testing
```
Create validation test helper ValidateForm with:
- Tuple parameter pattern: (ComboBox control, string placeholder)[]
- HasValidSelection() extension method usage
- Placeholder text processing for user-friendly field names
- MessageBox display with consistent error formatting
- Focus management for first invalid control
- Boolean return for validation pass/fail
- Integration with progress helper error reporting
```

---

## Form Design Templates

### 20. Create Reset Functionality
```
Create reset functionality for Control_[TabName] following the soft/hard reset pattern with:
- Button_Reset_Click with modifier key detection
- Soft reset: UI state reset without database calls
- Hard reset: complete data reload from database (Shift+Click)
- MainFormControlHelper.ResetComboBox calls with error colors
- DataGridView.DataSource clearing and refresh
- Button state resets and control enablement
- Focus management to first control
- Progress reporting during hard reset operations
```

### 21. Create Panel Toggle Functionality
```
Create panel toggle functionality for Control_[TabName]_Button_Toggle_RightPanel with:
- MainFormInstance null checking
- Panel2Collapsed property toggling
- Button text updates (?? for collapsed, ?? for expanded)
- Button color changes (SuccessColor for expanded, ErrorColor for collapsed)
- Helper_UI_ComboBoxes.DeselectAllComboBoxText call
- Keyboard shortcut support (Alt+Left/Right)
- State synchronization across controls
```

### 22. Create Print Functionality
```
Create print functionality for DataGridView following the Control_TransferTab pattern with:
- Data validation (rows count > 0)
- Visible column collection for print layout
- Core_DgvPrinter setup and configuration
- DataGridView.Tag assignment for print metadata
- Progress reporting during print preparation
- Error handling for print failures
- User feedback with MessageBox for print status
```

---

## Data Model Templates

### 23. Create Data Model Class
```
Create a data model class Model_[Entity] following the MTM pattern with:
- Properties using PascalCase naming convention
- Nullable reference types where appropriate (.NET 8 compatible)
- Default values for required properties
- Data validation attributes if needed
- ToString() override for debugging
- INotifyPropertyChanged implementation if needed for UI binding
- Constructor with optional parameters
- XML documentation for all public members
```

### 24. Create DAO Result Model
```
Create a result model following the Model_DaoResult pattern with:
- IsSuccess boolean property (Status == 0)
- Status integer property (0=Success, 1=Warning, -1=Error)
- ErrorMessage string property with default empty string
- Generic Data property for payload
- Exception property for error details
- Static factory methods: Success(), Warning(), Error()
- Proper null handling for data payload
- Status code constants matching MySQL stored procedure standards
```

---

## Service and Helper Templates

### 25. Create Helper Class
```
Create a helper class Helper_[Purpose] following the MTM helper pattern with:
- Static methods for utility functions
- Proper exception handling and logging
- Thread-safe operations where needed
- Extension method patterns where appropriate
- Model_AppVariables integration for configuration
- LoggingUtility integration for error tracking
- Async/await support for long-running operations
- Null checking and defensive programming
```

### 26. Create Service Class
```
Create a service class Service_[Purpose] following the MTM service pattern with:
- Initialize() method for service setup
- MainFormInstance property integration
- Background task support with proper cancellation
- Event-driven architecture where appropriate
- Cleanup methods for resource disposal
- Configuration integration with Model_AppVariables
- Logging integration for service operations
- Error handling with graceful degradation
```

### 27. Create Extension Methods
```
Create extension methods following the MTM Extensions pattern with:
- SafeToString() for null-safe string conversion
- SafeToInt() for safe integer parsing
- HasValidSelection() for ComboBox validation
- GetSelectedDataRowView() for DataGridView operations  
- GetSelectedDataRowViews() for multi-selection support
- Proper null handling and default values
- Generic type support where appropriate
- Documentation for usage patterns
```

---

## Database Stored Procedure Templates

### 28. Create Stored Procedure Call
```
Create a method to call MySQL stored procedure [procedure_name] with:
- Helper_Database_StoredProcedure.Execute[Type]WithStatus usage
- Parameters dictionary with no "p_" prefix (added automatically)
- Standard output parameters: p_Status INT, p_ErrorMsg VARCHAR(255)
- MySQL 5.7.24 compatibility considerations
- Progress helper integration for UI feedback
- Async/await pattern with ConfigureAwait(false)
- Connection string from Model_AppVariables.ConnectionString
- Proper result processing and error handling
```

### 29. Create Complex Database Operation
```
Create a complex database operation method with:
- Multiple stored procedure calls in sequence
- Transaction-like behavior with rollback on failure
- Progress reporting across multiple operations
- Intermediate result validation
- Batch processing with partial failure handling
- Summary result compilation
- Detailed error reporting for each step
- Performance optimization with connection reuse
```

---

## Integration Templates

### 30. Create MainForm Integration
```
Create MainForm integration for Control_[TabName] with:
- SetProgressControls method call during form initialization
- MainFormInstance static property assignment
- StatusStrip integration for progress reporting
- Tab-specific keyboard shortcuts in ProcessCmdKey
- Theme application after control addition
- Proper cleanup in form disposal
- Event handler registration for form events
```

---

## Usage Examples

### How to Use These Templates

1. **Copy the relevant prompt template** for your specific need
2. **Replace placeholder values** like `[TabName]`, `[Entity]`, `[MethodName]` with your actual values
3. **Paste into GitHub Copilot** as a comment or chat prompt
4. **Review and customize** the generated code to match your specific requirements
5. **Test thoroughly** to ensure proper integration with existing patterns

### Template Combinations

Many real-world scenarios require combining multiple templates:

- **New Settings Tab**: Use templates #2, #4, #7, #10, #12, #20
- **New MainForm Tab**: Use templates #3, #8, #9, #15, #21, #22
- **New DAO Class**: Use templates #4, #5, #6, #24, #28
- **New Business Logic**: Use templates #15, #16, #17, #25

### Customization Guidelines

- Always maintain the **MTM naming conventions**
- Include **proper error handling** and logging
- Apply **theme and DPI scaling** consistently  
- Use **Helper_StoredProcedureProgress** for visual feedback
- Follow **privilege management** patterns
- Integrate with **Model_AppVariables** for configuration

---

This comprehensive collection of prompt templates ensures consistent code generation that follows the established patterns in your MTM Inventory Application. Each template is designed to work with GitHub Copilot to produce code that integrates seamlessly with your existing codebase.
