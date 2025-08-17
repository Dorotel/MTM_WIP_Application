# Database Integration Migration Guide

## Overview
This section contains all database integration components that maintain 100% compatibility with existing MySQL infrastructure.

## Connection Management (3-Database-Integration/Connection-Management)
Critical files for database connectivity:

### Key Components:
- **Helper_Database_Variables.cs** - Connection string management with environment detection
- **Helper_Database_StoredProcedure.cs** - Stored procedure execution wrapper

### Environment Detection Logic:
```csharp
// From Helper_Database_Variables.cs
public static string GetConnectionString()
{
    // Production: Always 172.16.1.104
    // Debug: 172.16.1.104 if current IP matches, otherwise localhost
    // This logic MUST be preserved in MAUI
}
```

### MAUI Implementation:
```csharp
// In MauiProgram.cs
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        
        // Configure database connection
        var connectionString = Helper_Database_Variables.GetConnectionString();
        builder.Services.AddSingleton(new MySqlConnection(connectionString));
        
        return builder.Build();
    }
}
```

## Stored Procedures (3-Database-Integration/Stored-Procedures)
Complete database schema and stored procedure documentation:

### Database Structure:
- **282+ Stored Procedures** - All must be preserved exactly as implemented
- **17 Tables** - Complete schema compatibility maintained
- **Production Database**: `mtm_wip_application`
- **Debug Database**: `mtm_wip_application_test`

### Key Stored Procedure Categories:
1. **usr_*** - User management procedures
2. **inv_*** - Inventory operations procedures  
3. **trn_*** - Transaction procedures
4. **sys_*** - System configuration procedures

### Critical Database Files:
- **stored_procedures_backup_20250813_203724.sql** - Complete procedure definitions
- **UpdatedStoredProcedures/** - Latest procedure updates

## Database Access Patterns

### DAO Pattern Implementation:
All DAO classes use consistent patterns that MUST be preserved:

```csharp
// Example from Dao_Inventory.cs
public async Task<DaoResult<List<Model_Inventory>>> GetInventoryByPartIdAsync(string partId)
{
    try
    {
        var parameters = new List<MySqlParameter>
        {
            new MySqlParameter("p_PartID", partId)
        };
        
        var result = await Helper_Database_StoredProcedure
            .ExecuteDataTableWithStatus("inv_inventory_GetByPartID", parameters);
            
        return new DaoResult<List<Model_Inventory>>(success: true, data: inventoryList);
    }
    catch (Exception ex)
    {
        await Dao_ErrorLog.LogErrorToDatabaseAsync(ex, "GetInventoryByPartIdAsync");
        return new DaoResult<List<Model_Inventory>>(success: false, errorMessage: ex.Message);
    }
}
```

## MAUI Database Integration

### 1. Preserve All Existing Components:
- Keep Helper_Database_Variables.cs exactly as implemented
- Keep Helper_Database_StoredProcedure.cs exactly as implemented  
- Keep all DAO classes exactly as implemented
- Maintain all stored procedure calls exactly as implemented

### 2. Dependency Injection Setup:
```csharp
// Register database services
builder.Services.AddTransient<Dao_Inventory>();
builder.Services.AddTransient<Dao_User>();
builder.Services.AddTransient<Dao_Transactions>();
builder.Services.AddTransient<Dao_ErrorLog>();
// ... register all DAO classes
```

### 3. ViewModel Integration:
```csharp
public partial class InventoryViewModel : ObservableObject
{
    private readonly Dao_Inventory _inventoryDao;
    
    public InventoryViewModel(Dao_Inventory inventoryDao)
    {
        _inventoryDao = inventoryDao;
    }
    
    [RelayCommand]
    private async Task LoadInventoryAsync()
    {
        var result = await _inventoryDao.GetInventoryByPartIdAsync(SelectedPartId);
        if (result.Success)
        {
            InventoryItems.Clear();
            foreach (var item in result.Data)
            {
                InventoryItems.Add(item);
            }
        }
        else
        {
            // Handle error using existing error handling patterns
        }
    }
}
```

## Server Configuration

### Production Environment:
- **Server**: 172.16.1.104 (always)
- **Database**: mtm_wip_application
- **Port**: 3306 (standard MySQL)

### Debug Environment:
- **Server**: 172.16.1.104 (if IP matches) or localhost
- **Database**: mtm_wip_application_test  
- **Port**: 3306

### Connection String Format:
```
Server={server};Database={database};Uid={username};Pwd={password};
```

## Migration Checklist

- [ ] Copy Helper_Database_Variables.cs to MAUI project
- [ ] Copy Helper_Database_StoredProcedure.cs to MAUI project
- [ ] Copy all DAO classes to MAUI project
- [ ] Register all database services in dependency injection
- [ ] Test connection string logic on all target platforms
- [ ] Verify stored procedure calls work identically
- [ ] Validate error handling patterns
- [ ] Test production/debug environment switching

## Important Notes

1. **Zero Database Changes**: No stored procedures, tables, or database structure changes
2. **Exact Compatibility**: All database operations must work identically to WinForms version
3. **Environment Detection**: Server address detection logic must work on all MAUI platforms
4. **Error Handling**: Preserve existing database error handling and logging patterns