# RegionOrganizer

## Overview
The RegionOrganizer tool automatically organizes C# class members into standardized `#region` blocks. It groups Fields, Properties, Constructors, Methods, and Events into separate regions for better code organization.

## Usage

### Preview Mode (Recommended First)
```bash
dotnet run --project Tools/RegionOrganizer/RegionOrganizer.csproj -- --preview
```

### Execute Mode
```bash
dotnet run --project Tools/RegionOrganizer/RegionOrganizer.csproj
```

### Direct C# Execution
```csharp
var organizer = new RegionOrganizer("Tools/RegionOrganizer/Logs", previewMode: true);
organizer.ProcessDirectory(".");
organizer.PrintSummary();
organizer.SaveLog();
```

## Features

- **Automatic Region Creation**: Creates standardized `#region` blocks
- **Smart Categorization**: Automatically categorizes class members
- **Preview Mode**: Shows what changes would be made without modifying files
- **Comprehensive Logging**: Creates detailed logs of all changes
- **Indentation Preservation**: Maintains proper code indentation

## Region Categories

The tool organizes code into the following regions:

1. **Fields** - Private and public fields
2. **Properties** - Properties with getters/setters
3. **Constructors** - Class constructors
4. **Methods** - All methods and functions
5. **Events** - Event handlers and event declarations

## Example Organization

**Before:**
```csharp
public class MyClass
{
    private string _field1;
    
    public string Property1 { get; set; }
    
    public MyClass()
    {
        _field1 = "test";
    }
    
    private void Method1()
    {
        // Method implementation
    }
    
    public event EventHandler SomeEvent;
}
```

**After:**
```csharp
public class MyClass
{
    #region Fields
    
    private string _field1;
    
    #endregion
    
    #region Properties
    
    public string Property1 { get; set; }
    
    #endregion
    
    #region Constructors
    
    public MyClass()
    {
        _field1 = "test";
    }
    
    #endregion
    
    #region Methods
    
    private void Method1()
    {
        // Method implementation
    }
    
    #endregion
    
    #region Events
    
    public event EventHandler SomeEvent;
    
    #endregion
}
```

## Classification Rules

### Fields
- Lines ending with semicolon (`;`)
- No parentheses `()`
- No curly braces `{}`
- Variable declarations

### Properties
- Contains `{ get` or `{ set`
- Contains `{` but no `(` and no `=`
- Property declarations

### Constructors
- Contains parentheses `()`
- Method name matches class name
- Constructor declarations

### Methods
- Contains parentheses `()`
- Has method signature
- All other method declarations

### Events
- Contains `event` keyword
- Contains `EventHandler`
- Contains `+=` or `-=` operators

## Output

The tool generates:
1. **Console Output**: Real-time progress and summary
2. **Log Files**: Detailed logs saved to `Tools/RegionOrganizer/Logs/RegionOrganizer_Run_{timestamp}.log`
3. **Summary Report**: Count of files organized and unchanged

## Example Log Entry
```
[2025-01-13 10:30:45] Successfully organized regions in: Forms/Transactions/Transactions.cs
[2025-01-13 10:30:45] No changes needed in: Models/Model_Transactions.cs
```

## Safety Features

- **Preview Mode**: Test changes before applying them
- **Selective Processing**: Skips auto-generated and designer files
- **Indentation Preservation**: Maintains existing code formatting
- **Detailed Logging**: Every change is logged for review

## Excluded Files

The tool automatically skips:
- `AssemblyInfo.cs` files
- `Resources.Designer.cs` files
- `.Designer.cs` files (WinForms designer code)

## Troubleshooting

### Common Issues

1. **Incorrect Categorization**: Check the classification rules for edge cases
2. **Indentation Problems**: Verify the indentation detection logic
3. **Missing Regions**: Ensure class structure is properly detected

### Error Resolution

- **Classification Issues**: The tool uses pattern matching to categorize members
- **Indentation Problems**: The tool preserves existing indentation patterns
- **Build Errors**: Review log files for any unintended changes

### Validation Steps

1. Run in preview mode first
2. Review the log file for unexpected changes
3. Compile the project after running to ensure no syntax errors
4. Use source control to review diffs

## Configuration

The tool can be extended by modifying the categorization logic in `CategorizeClassMember()`:

```csharp
private void CategorizeClassMember(string line, Dictionary<string, List<string>> regions, string classIndentation)
{
    // Add custom categorization logic here
    if (IsCustomType(trimmedLine))
    {
        regions["CustomRegion"].Add(indentedLine);
    }
}
```

## Limitations

- Only processes classes (not interfaces, structs, or enums)
- Assumes standard C# syntax patterns
- May need adjustment for complex generic types or nested classes

## Version History

- **v1.0**: Initial version with basic region organization
- **v1.1**: Added smart categorization logic
- **v1.2**: Enhanced indentation preservation
- **v1.3**: Added preview mode and comprehensive logging