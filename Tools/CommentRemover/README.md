# CommentRemover

## Overview
The CommentRemover tool automatically removes all comments from C# source files while preserving string literals and essential code structure. This ensures the codebase follows the requirement of having no comments in C# files.

## Usage

### Preview Mode (Recommended First)
```bash
dotnet run --project Tools/CommentRemover/CommentRemover.csproj -- --preview
```

### Execute Mode
```bash
dotnet run --project Tools/CommentRemover/CommentRemover.csproj
```

### Direct C# Execution
```csharp
var remover = new CommentRemover("Tools/CommentRemover/Logs", previewMode: true);
remover.ProcessDirectory(".");
remover.PrintSummary();
remover.SaveLog();
```

## Features

- **Safe Comment Removal**: Preserves comments inside string literals
- **Multiple Comment Types**: Handles both single-line (`//`) and multi-line (`/* */`) comments
- **Preview Mode**: Shows what changes would be made without modifying files
- **Comprehensive Logging**: Creates detailed logs of all removals
- **Smart Filtering**: Skips auto-generated files like AssemblyInfo.cs and .Designer.cs files

## Comment Types Handled

### Single-Line Comments
- Full line comments starting with `//`
- Inline comments at the end of code lines

### Multi-Line Comments
- Block comments using `/* ... */` syntax
- Comments spanning multiple lines

### Protected Content
- Comments inside string literals (preserved)
- Comments inside character literals (preserved)

## Example Processing

**Before:**
```csharp
// This is a comment to be removed
public class MyClass
{
    private string message = "This // is not a comment";
    
    /* Multi-line comment
       to be removed */
    public void Method()
    {
        Console.WriteLine("Hello"); // Inline comment
    }
}
```

**After:**
```csharp
public class MyClass
{
    private string message = "This // is not a comment";
    
    public void Method()
    {
        Console.WriteLine("Hello");
    }
}
```

## Output

The tool generates:
1. **Console Output**: Real-time progress and summary
2. **Log Files**: Detailed logs saved to `Tools/CommentRemover/Logs/CommentRemover_Run_{timestamp}.log`
3. **Summary Report**: Count of files processed and comments removed

## Example Log Entry
```
[2025-01-13 10:30:45] Successfully updated file: Forms/Transactions/Transactions.cs (removed 15 comment lines)
[2025-01-13 10:30:45] No comments found in: Models/Model_Transactions.cs
```

## Safety Features

- **Preview Mode**: Test changes before applying them
- **String Literal Protection**: Preserves comments inside quotes
- **Selective Processing**: Skips auto-generated and designer files
- **Detailed Logging**: Every change is logged for review

## Excluded Files

The tool automatically skips:
- `AssemblyInfo.cs` files
- `Resources.Designer.cs` files  
- `.Designer.cs` files (WinForms designer code)

## Troubleshooting

### Common Issues

1. **Unexpected String Modifications**: Check for unescaped quotes in string literals
2. **Missing Code**: Verify the comment detection logic didn't remove code
3. **Build Errors**: Review log files for any unintended removals

### Error Resolution

- **String Literal Issues**: The tool uses escape character detection to avoid modifying string contents
- **Multi-line Comment Problems**: Check for nested comments or unusual comment patterns
- **Performance Issues**: Large files may take longer to process

### Validation Steps

1. Run in preview mode first
2. Review the log file for unexpected changes
3. Compile the project after running to ensure no syntax errors
4. Use source control to review diffs

## Configuration

The tool can be extended by modifying the file filtering logic in `ProcessDirectory()`:

```csharp
var files = Directory.GetFiles(directoryPath, pattern, SearchOption.AllDirectories)
                  .Where(f => !f.Contains("AssemblyInfo.cs") && 
                             !f.Contains("Resources.Designer.cs") && 
                             !f.Contains(".Designer.cs"))
                  .ToArray();
```

## Version History

- **v1.0**: Initial version with basic comment removal
- **v1.1**: Added string literal protection
- **v1.2**: Enhanced multi-line comment handling
- **v1.3**: Added preview mode and comprehensive logging