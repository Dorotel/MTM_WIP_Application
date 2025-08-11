# Enhanced Stored Procedure Error Handling Implementation Guide

## Overview

This document provides a comprehensive guide for implementing the enhanced stored procedure error handling system in the MTM Inventory Application. The system provides visual feedback with red progress bars on errors and standardized success/failure status reporting.

## Key Components

### 1. Helper_StoredProcedureProgress Class
Location: `Helpers/Helper_StoredProcedureProgress.cs`

This class provides enhanced progress reporting with visual error feedback:
- **Green progress bars** for success states
- **Red progress bars** for error states  
- **Standard progress bars** for normal operations
- Thread-safe UI updates
- Comprehensive status messaging

### 2. Helper_Database_StoredProcedure Class
Location: `Data/Helper_Database_StoredProcedure.cs`

Enhanced database helper that supports stored procedure status reporting:
- Automatic handling of `p_Status` and `p_ErrorMsg` output parameters
- Progress reporting integration
- Comprehensive exception handling
- Support for all SQL operation types (SELECT, INSERT, UPDATE, DELETE)

### 3. StoredProcedureResult Classes
Generic and non-generic result classes for standardized return values:
- `IsSuccess` property for quick status checking
- `Status` property (0 = success, 1 = warning, -1 = error)
- `ErrorMessage` property with detailed error information
- `Exception` property for detailed exception information

## Stored Procedure Standards

All stored procedures must include these output parameters:
```sql
OUT p_Status INT,
OUT p_ErrorMsg VARCHAR(255)
```

### Status Codes
- **0**: Success - Operation completed successfully
- **1**: Warning - Operation completed with warnings (e.g., "User not found")
- **-1**: Error - Operation failed due to database or validation error

### Example Stored Procedure Structure
```sql
DELIMITER $$
CREATE PROCEDURE example_procedure(
    IN p_InputParam VARCHAR(100),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred during operation';
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- Validation logic
    SELECT COUNT(*) INTO v_Count FROM table WHERE field = p_InputParam;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = 'Record not found';
        ROLLBACK;
    ELSE
        -- Main operation logic here
        
        SET p_Status = 0;
        SET p_ErrorMsg = 'Operation completed successfully';
        COMMIT;
    END IF;
END $$
DELIMITER ;
```

## Implementation Steps

### Step 1: Update Control to Support Enhanced Progress System

```csharp
public partial class Control_Example : UserControl
{
    #region Fields
    private Helper_StoredProcedureProgress? _progressHelper;
    #endregion

    #region Public Methods
    public void SetProgressControls(ToolStripProgressBar progressBar, ToolStripStatusLabel statusLabel)
    {
        _progressHelper = Helper_StoredProcedureProgress.Create(progressBar, statusLabel, 
            this.FindForm() ?? throw new InvalidOperationException("Control must be added to a form"));
    }
    #endregion
}
```

### Step 2: Update Parent Form to Inject Progress Controls

```csharp
// In SettingsForm.InitializeUserControls()
Control_Example controlExample = new() { Dock = DockStyle.Fill };
SettingsForm_Panel_Example.Controls.Add(controlExample);

// Pass the ToolStrip progress controls
controlExample.SetProgressControls(SettingsForm_ProgressBar, SettingsForm_StatusText);
```

### Step 3: Replace Direct DAO Calls with Enhanced Database Calls

**Before (Direct DAO Call):**
```csharp
await Dao_Operation.InsertOperation(operationNumber, user);
```

**After (Enhanced Database Call):**
```csharp
var result = await Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus(
    Model_AppVariables.ConnectionString,
    "md_operation_numbers_Add_Operation",
    new Dictionary<string, object>
    {
        ["Operation"] = operationNumber,
        ["IssuedBy"] = user
    },
    _progressHelper,
    true
);

if (!result.IsSuccess)
{
    _progressHelper?.ShowError($"Error creating operation: {result.ErrorMessage}");
    return;
}

_progressHelper?.ShowSuccess("Operation created successfully!");
```

### Step 4: Add Proper Error Handling to Event Handlers

```csharp
private async void Button_Save_Click(object sender, EventArgs e)
{
    try
    {
        Button_Save.Enabled = false;

        _progressHelper?.ShowProgress("Starting operation...");
        _progressHelper?.UpdateProgress(10, "Validating input...");

        // Validation logic with error feedback
        if (string.IsNullOrWhiteSpace(TextBox_Input.Text))
        {
            _progressHelper?.ShowError("Input is required");
            TextBox_Input.Focus();
            return;
        }

        _progressHelper?.UpdateProgress(30, "Processing request...");

        // Database operation with enhanced error handling
        var result = await Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus(
            Model_AppVariables.ConnectionString,
            "procedure_name",
            parameters,
            _progressHelper,
            true
        );

        if (!result.IsSuccess)
        {
            _progressHelper?.ShowError($"Operation failed: {result.ErrorMessage}");
            return;
        }

        _progressHelper?.ShowSuccess("Operation completed successfully!");
        
        // Success actions
        ClearForm();
        EventRaised?.Invoke(this, EventArgs.Empty);
        
    }
    catch (Exception ex)
    {
        _progressHelper?.ShowError($"Unexpected error: {ex.Message}");
        LoggingUtility.LogApplicationError(ex);
    }
    finally
    {
        Button_Save.Enabled = true;
    }
}
```

## Visual Feedback Behaviors

### Success State
- Progress bar: **Green** color
- Status text: **Green** "SUCCESS: [message]"
- Progress value: 100%

### Error State  
- Progress bar: **Red** color
- Status text: **Red** "ERROR: [message]"
- Progress value: Maintains current value or specified value

### Normal Progress
- Progress bar: **Default** color
- Status text: **Default** color with progress percentage
- Progress value: Specified percentage (0-100)

### Warning State
- Progress bar: **Red** color (same as error)
- Status text: **Red** "ERROR: Warning: [message]"
- Used for non-fatal issues like "User not found"

## Database Helper Methods Available

### ExecuteDataTableWithStatus
For SELECT operations that return data:
```csharp
var result = await Helper_Database_StoredProcedure.ExecuteDataTableWithStatus(
    connectionString, procedureName, parameters, progressHelper, useAsync);
```

### ExecuteNonQueryWithStatus  
For INSERT, UPDATE, DELETE operations:
```csharp
var result = await Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus(
    connectionString, procedureName, parameters, progressHelper, useAsync);
```

### ExecuteScalarWithStatus
For operations returning single values:
```csharp
var result = await Helper_Database_StoredProcedure.ExecuteScalarWithStatus(
    connectionString, procedureName, parameters, progressHelper, useAsync);
```

### ExecuteWithCustomOutput
For procedures with custom output parameters:
```csharp
var outputParams = new Dictionary<string, MySqlDbType>
{
    ["Status"] = MySqlDbType.Int32,
    ["ErrorMsg"] = MySqlDbType.VarChar,
    ["CustomOutput"] = MySqlDbType.VarChar
};

var result = await Helper_Database_StoredProcedure.ExecuteWithCustomOutput(
    connectionString, procedureName, inputParams, outputParams, progressHelper, useAsync);
```

## Required Using Statements

Add these using statements to controls implementing the enhanced system:
```csharp
using System.Collections.Generic;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Models;
using MTM_Inventory_Application.Logging;
```

## Testing the Implementation

### Test Success Path
1. Execute operation with valid data
2. Verify green progress bar at completion
3. Verify success message display
4. Verify form behavior (clearing, events, etc.)

### Test Error Path
1. Execute operation with invalid data or simulate database error
2. Verify red progress bar during error
3. Verify error message display with "ERROR:" prefix
4. Verify form remains in appropriate state for user correction

### Test Warning Path
1. Execute operation that returns warning status (e.g., record not found)
2. Verify red progress bar (warnings displayed as errors visually)
3. Verify warning message display
4. Verify appropriate user guidance

## Best Practices

1. **Always enable/disable buttons** during operations to prevent duplicate submissions
2. **Use try-catch-finally blocks** for comprehensive error handling
3. **Provide specific error messages** that help users understand and correct issues
4. **Focus appropriate controls** when validation fails
5. **Clear forms appropriately** after successful operations
6. **Use consistent progress step percentages** across similar operations
7. **Test both success and failure scenarios** thoroughly
8. **Log unexpected exceptions** using LoggingUtility
9. **Provide user-friendly error messages** while logging technical details

## Migration Checklist

When migrating existing controls to the enhanced system:

- [ ] Add `Helper_StoredProcedureProgress?` field
- [ ] Implement `SetProgressControls()` method  
- [ ] Update parent form to inject progress controls
- [ ] Replace direct DAO calls with enhanced database helper calls
- [ ] Add comprehensive error handling to all operations
- [ ] Update stored procedures to include status output parameters
- [ ] Test success, error, and warning scenarios
- [ ] Verify visual feedback works correctly (red progress bars on errors)
- [ ] Update any related documentation

## Examples in Current Implementation

### Control_Add_User
Location: `Controls/SettingsForm/Control_Add_User.cs`
- Complete implementation with user creation workflow
- Demonstrates validation error handling
- Shows database operation error handling
- Implements success feedback and form clearing

### Control_Add_Operation  
Location: `Controls/SettingsForm/Control_Add_Operation.cs`
- Simpler implementation for basic CRUD operations
- Shows existence checking with error feedback
- Demonstrates creation operation with success feedback

### SettingsForm Progress Methods
Location: `Forms/Settings/SettingsForm.cs`
- Enhanced progress control methods
- Integration with Helper_StoredProcedureProgress
- Support for processing stored procedure results

These examples serve as templates for implementing the enhanced error handling system in other controls throughout the application.
