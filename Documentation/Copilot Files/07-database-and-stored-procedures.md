# 7, 9. Database Operations and Stored Procedures; Version Management

MySQL Compatibility and Procedure Contract
- All procedures include:
```
OUT p_Status INT, OUT p_ErrorMsg VARCHAR(255)
```

Stored Procedure Categories
- usr_* (users, roles, permissions)
- sys_* (system settings, access control)
- md_* (master data)
- inv_* (inventory operations)
- inv_transaction_* (audit/history)
- sys_last_10_transactions_* (Quick Buttons)
- log_changelog_* (versioning)

Standard Call Pattern with DaoResult<T>
```csharp
var result = await Helper_Database_StoredProcedure.ExecuteDataTableWithStatus(
    Model_AppVariables.ConnectionString,
    "inv_inventory_Get_ByPartID",
    new Dictionary<string, object>
    {
        ["PartID"] = partId, // helper adds p_ automatically
        ["Operation"] = operation,
        ["IncludeInactive"] = false
    },
    _progressHelper,
    true
);
```

Version Management
- Semantic version comparison
- Automatic background timer (every 30s)
```csharp
var versionResult = await Helper_Database_StoredProcedure.ExecuteDataTableWithStatus(
    connectionString, "log_changelog_Get_Current", null, null, true);
if (versionResult.IsSuccess && versionResult.Data?.Rows.Count > 0)
{
    string databaseVersion = versionResult.Data.Rows[0]["Version"]?.ToString() ?? "Unknown";
    UpdateVersionDisplay(Model_AppVariables.UserVersion, databaseVersion);
}
```