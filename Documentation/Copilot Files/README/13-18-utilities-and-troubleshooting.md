# 13–18. Constants, Extensions, Business Logic, Testing, Performance, Troubleshooting

13. Common Application Constants
```csharp
public const string CURRENT_VERSION = "5.0.1.2";
```

14. Extension Methods and Utilities
```csharp
public static string SafeToString(this object? obj, string defaultValue = "") =>
    obj?.ToString() ?? defaultValue;
public static bool IsNullOrEmpty(this DataTable? table) => table == null || table.Rows.Count == 0;
public static DataTable EnsureNotNull(this DataTable? table) => table ?? new DataTable();
```

15. Business Logic Patterns
- Single and multiple item processing with DaoResult handling and status updates.

16. Testing and Validation
- Form validation helpers
- Progress testing scenarios with DaoResult simulation

17. Performance and Best Practices
- Stored procedures only
- Async/await on DB operations
- Progress reporting for >1s ops
- SuspendLayout/ResumeLayout for bulk UI updates

18. Troubleshooting Guide
- NullReferenceException in search (fixed via DaoResult)
- DPI scaling problems (always apply DPI scaling calls)
- Progress bar not showing (ensure SetProgressControls)
- Stored procedure parameter issues (use helper; no direct p_ in C#)