# RenamerTool

## Overview
The RenamerTool is a utility that standardizes naming conventions for WinForms controls and variables in the MTM WIP Application. It ensures all controls follow the pattern: `{ClassName}_{ControlType}_{Name}_{Number}`.

## Usage

### Preview Mode (Recommended First)
```bash
dotnet run --project Tools/RenamerTool/RenamerTool.csproj -- --preview
```

### Execute Mode
```bash
dotnet run --project Tools/RenamerTool/RenamerTool.csproj
```

### Direct C# Execution
```csharp
var renamer = new RenamerTool("Tools/RenamerTool/Logs", previewMode: true);
renamer.ProcessDirectory(".");
renamer.PrintSummary();
renamer.SaveLog();
```

## Features

- **Preview Mode**: Shows what changes would be made without actually modifying files
- **Comprehensive Logging**: Creates detailed logs of all renamings in `Tools/RenamerTool/Logs/`
- **Pattern Matching**: Uses regex to ensure only whole words are renamed (avoids partial matches)
- **Recursive Processing**: Processes all `.cs` files in the directory tree

## Current Renaming Mappings

The tool currently handles the following control renamings in the Transactions form:

| Old Name | New Name |
|----------|----------|
| `comboSortBy` | `Transactions_ComboBox_SortBy` |
| `lblSortBy` | `Transactions_Label_SortBy` |
| `txtSearchPartID` | `Transactions_TextBox_SearchPartID` |
| `lblSearchPartID` | `Transactions_Label_SearchPartID` |
| `btnReset` | `Transactions_Button_Reset` |
| `tabControlMain` | `Transactions_TabControl_Main` |
| `tabPartEntry` | `Transactions_TabPage_PartEntry` |
| `tabPartRemoval` | `Transactions_TabPage_PartRemoval` |
| `tabPartTransfer` | `Transactions_TabPage_PartTransfer` |
| `dataGridTransactions` | `Transactions_DataGridView_Transactions` |
| `panelBottom` | `Transactions_Panel_Bottom` |
| `lblSortByUser` | `Transactions_Label_SortByUser` |
| `comboUser` | `Transactions_ComboBox_User` |
| `lblUser` | `Transactions_Label_User` |
| `comboUserName` | `Transactions_ComboBox_UserName` |
| `lblUserName` | `Transactions_Label_UserName` |
| `comboShift` | `Transactions_ComboBox_Shift` |
| `lblShift` | `Transactions_Label_Shift` |

## Output

The tool generates:
1. **Console Output**: Real-time progress and summary
2. **Log Files**: Detailed logs saved to `Tools/RenamerTool/Logs/RenamerTool_Run_{timestamp}.log`
3. **Summary Report**: Count of renamings and files affected

## Example Log Entry
```
[2025-01-13 10:30:45] Renamed 'comboSortBy' to 'Transactions_ComboBox_SortBy' in Forms/Transactions/Transactions.cs (3 occurrences)
[2025-01-13 10:30:45] Successfully updated file: Forms/Transactions/Transactions.cs
```

## Troubleshooting

### Common Issues

1. **File Access Errors**: Ensure you have write permissions to the target files
2. **Pattern Conflicts**: Check log files for unexpected replacements
3. **Backup Missing**: Always run in preview mode first to verify changes

### Error Resolution

- **Build Errors After Renaming**: Check the log file for any unexpected replacements
- **Missing References**: Verify all control references were updated consistently
- **Unexpected Changes**: Review the regex patterns and renaming mappings

### Safety Features

- **Preview Mode**: Always test changes before applying them
- **Whole Word Matching**: Uses `\b` regex boundaries to avoid partial matches
- **Detailed Logging**: Every change is logged for review and rollback if needed

## Extension

To add new renaming mappings, modify the `InitializeRenamingMap()` method in `RenamerTool.cs`:

```csharp
private void InitializeRenamingMap()
{
    var newMappings = new Dictionary<string, string>
    {
        {"oldControlName", "NewFormName_ControlType_Name"},
        // Add more mappings...
    };
    
    foreach (var mapping in newMappings)
    {
        _renamingMap[mapping.Key] = mapping.Value;
    }
}
```

## Version History

- **v1.0**: Initial version with Transactions form support
- **v1.1**: Added preview mode and comprehensive logging
- **v1.2**: Enhanced pattern matching and error handling