# ================================================================================
# MTM INVENTORY APPLICATION - STORED PROCEDURES DEPLOYMENT GUIDE
# ================================================================================
# File: README.md
# Purpose: Comprehensive guide for deploying stored procedures
# Created: August 10, 2025
# Updated: For MySQL 5.7.24 and MAMP compatibility
# Target Database: mtm_wip_application
# MySQL Version: 5.7.24+ (MAMP Compatible)
# ================================================================================

# MTM Inventory Application - Stored Procedures Deployment

This directory contains all the stored procedures required for the MTM Inventory Application, along with deployment scripts and documentation optimized for MySQL 5.7.24 and MAMP environments.

## Overview

The MTM Inventory Application has been architected to use stored procedures exclusively for database operations, eliminating hardcoded SQL statements to improve security, maintainability, and performance. This deployment package includes:

- **User Management Procedures** - Authentication, settings, and user CRUD operations
- **System Role Procedures** - Role-based access control and permissions
- **Master Data Procedures** - Part numbers, operations, locations, and item types
- **Inventory Procedures** - Inventory tracking, transactions, and batch management

## Files Structure

```
Database/StoredProcedures/
??? 01_User_Management_Procedures.sql      # Core user management procedures (MySQL 5.7 Compatible)
??? 02_System_Role_Procedures.sql          # Role and access control procedures
??? 03_Master_Data_Procedures.sql          # Master data management procedures
??? 04_Inventory_Procedures.sql            # Inventory and transaction procedures
??? 05_Error_Log_Procedures.sql            # Error logging procedures (NEW)
??? deploy_procedures.sh                   # Linux/Mac/MAMP deployment script
??? README.md                             # This file
```

## Prerequisites

### System Requirements
- **MySQL Server 5.7.24 or higher** (compatible with MAMP)
- MySQL client tools (included with MAMP)
- Administrative access to the target database
- **MAMP** (recommended for development) or standard MySQL installation

### MAMP-Specific Requirements
- MAMP 4.0+ with MySQL 5.7.24+
- Apache and MySQL services running in MAMP
- Default MAMP credentials (usually root/root)
- Target database created via phpMyAdmin or MySQL client

### Database Requirements
- Target database `mtm_wip_application` must exist
- User account with sufficient privileges:
  - `CREATE ROUTINE` privilege
  - `ALTER ROUTINE` privilege
  - `EXECUTE` privilege
  - `SELECT`, `INSERT`, `UPDATE`, `DELETE` on all application tables

### Application Requirements
- MTM Inventory Application (.NET 8)
- All application DAO classes must be using the stored procedure architecture
- Connection string properly configured for stored procedure calls

## MySQL 5.7.24 Compatibility Notes

### Differences from MySQL 8.0
- **BOOLEAN Parameters**: Replaced with `TINYINT(1)` for broader compatibility
- **JSON Handling**: Uses `TEXT` data type for JSON parameters to ensure compatibility
- **Error Handling**: Enhanced error handling patterns compatible with MySQL 5.7
- **Syntax Validation**: All procedures tested with MySQL 5.7.24

### MAMP Considerations
- Default port: **3306** (some older MAMP versions use 8889)
- Default credentials: **root/root**
- MySQL binaries location: `/Applications/MAMP/Library/bin` (macOS) or `C:\MAMP\bin\mysql\bin` (Windows)
- Access phpMyAdmin at: `http://localhost/phpMyAdmin` (when MAMP is running)

## Deployment Methods

### Method 1: MAMP Automated Deployment (Recommended)

#### For MAMP on Windows
```cmd
# Navigate to stored procedures directory
cd Database\StoredProcedures

# Deploy with MAMP defaults (root/root)
deploy_procedures.bat -h localhost -u root -p root -d mtm_wip_application

# Deploy with custom MAMP installation path
deploy_procedures.bat --mamp-path "C:\MAMP" -p root

# Deploy with older MAMP (port 8889)
deploy_procedures.bat -P 8889 -u root -p root -d mtm_wip_application
```

#### For MAMP on macOS/Linux
```bash
# Navigate to stored procedures directory
cd Database/StoredProcedures

# Make script executable
chmod +x deploy_procedures.sh

# Deploy with MAMP defaults
./deploy_procedures.sh -h localhost -u root -p root -d mtm_wip_application

# Deploy with custom MySQL path
./deploy_procedures.sh --mysql-path /Applications/MAMP/Library/bin -p root

# Deploy with older MAMP (port 8889)
./deploy_procedures.sh -P 8889 -u root -p root -d mtm_wip_application
```

#### MAMP Deployment Script Parameters
- `-h, --host` - Database host (default: localhost)
- `-P, --port` - Database port (default: 3306, older MAMP: 8889)
- `-u, --user` - Database username (default: root)
- `-p, --password` - Database password (MAMP default: root)
- `-d, --database` - Database name (default: mtm_wip_application)
- `--mamp-path` - MAMP installation path (Windows only)
- `--mysql-path` - Custom MySQL binary path (macOS/Linux)

### Method 2: MAMP Manual Deployment via phpMyAdmin

1. **Start MAMP** and ensure Apache/MySQL services are running
2. **Open phpMyAdmin** at `http://localhost/phpMyAdmin`
3. **Select database** `mtm_wip_application` (create if it doesn't exist)
4. **Import SQL files** in the following order:
   - `01_User_Management_Procedures.sql`
   - `02_System_Role_Procedures.sql`
   - `03_Master_Data_Procedures.sql`
   - `04_Inventory_Procedures.sql`
   - `05_Error_Log_Procedures.sql`   # Import new error log procedures

### Method 3: MAMP Command Line Deployment

#### Windows (with MAMP)
```cmd
# Add MAMP MySQL to PATH or use full path
set PATH=C:\MAMP\bin\mysql\bin;%PATH%

# Execute each file in order
mysql -h localhost -P 3306 -u root -p mtm_wip_application < 01_User_Management_Procedures.sql
mysql -h localhost -P 3306 -u root -p mtm_wip_application < 02_System_Role_Procedures.sql
mysql -h localhost -P 3306 -u root -p mtm_wip_application < 03_Master_Data_Procedures.sql
mysql -h localhost -P 3306 -u root -p mtm_wip_application < 04_Inventory_Procedures.sql
mysql -h localhost -P 3306 -u root -p mtm_wip_application < 05_Error_Log_Procedures.sql
```

#### macOS (with MAMP)
```bash
# Add MAMP MySQL to PATH
export PATH="/Applications/MAMP/Library/bin:$PATH"

# Execute each file in order
mysql -h localhost -P 3306 -u root -p mtm_wip_application < 01_User_Management_Procedures.sql
mysql -h localhost -P 3306 -u root -p mtm_wip_application < 02_System_Role_Procedures.sql
mysql -h localhost -P 3306 -u root -p mtm_wip_application < 03_Master_Data_Procedures.sql
mysql -h localhost -P 3306 -u root -p mtm_wip_application < 04_Inventory_Procedures.sql
mysql -h localhost -P 3306 -u root -p mtm_wip_application < 05_Error_Log_Procedures.sql
```

## MAMP Troubleshooting

### Common MAMP Issues

1. **"Can't connect to MySQL"**
   - Ensure MAMP Apache and MySQL services are running (green lights in MAMP control panel)
   - Check port number (3306 for newer MAMP, 8889 for older versions)
   - Verify credentials (usually root/root for MAMP)

2. **"Access denied for user"**
   - Default MAMP credentials are `root/root`
   - Check MAMP control panel for actual port and credentials
   - Ensure user has necessary privileges

3. **"Database doesn't exist"**
   - Create database via phpMyAdmin: `http://localhost/phpMyAdmin`
   - Or create via command line: `CREATE DATABASE mtm_wip_application;`

4. **"MySQL command not found"**
   - Use full path to MAMP MySQL: `C:\MAMP\bin\mysql\bin\mysql.exe` (Windows)
   - Or add MAMP bin to PATH environment variable
   - Use `--mamp-path` or `--mysql-path` deployment script options

5. **"Procedures already exist"**
   - Scripts include `DROP PROCEDURE IF EXISTS` statements
   - If issues persist, drop procedures manually via phpMyAdmin

### MAMP Version Detection
The deployment scripts automatically detect common MAMP installations:
- **Windows**: `C:\MAMP\bin\mysql\bin\`
- **macOS**: `/Applications/MAMP/Library/bin/`
- **Linux (XAMPP/LAMPP)**: `/opt/lampp/bin/`

## Procedure Categories (MySQL 5.7.24 Compatible)

### 1. User Management Procedures (01_User_Management_Procedures.sql)

| Procedure Name | Purpose | MySQL 5.7 Changes |
|----------------|---------|-------------------|
| `usr_ui_settings_Delete_ByUserId` | Clean deletion of user UI settings | ? Compatible |
| `usr_users_GetFullName_ByUser` | Retrieve user full name by username | ? Compatible |
| `usr_ui_settings_GetSettingsJson_ByUserId` | Get user interface settings JSON | ? TEXT parameters |
| `usr_users_GetUserSetting_ByUserAndField` | Get specific user setting (legacy support) | ? Compatible |
| `usr_users_SetUserSetting_ByUserAndField` | Set specific user setting dynamically | ? Compatible |
| `usr_user_roles_GetRoleId_ByUserId` | Get user role ID by user identifier | ? Compatible |
| `usr_users_Add_User` | Create new user with all settings | ? TINYINT(1) for boolean |
| `usr_users_Update_User` | Update existing user information | ? Compatible |
| `usr_users_Delete_User` | Delete user account | ? Compatible |
| `usr_users_Get_All` | Retrieve all users | ? Compatible |
| `usr_users_Get_ByUser` | Get user by username | ? Compatible |
| `usr_users_Exists` | Check if user exists | ? Compatible |

### 2. System Role Procedures (02_System_Role_Procedures.sql)

| Procedure Name | Purpose | MySQL 5.7 Status |
|----------------|---------|------------------|
| `sys_user_roles_Add` | Add user role assignment | ? Compatible |
| `sys_user_roles_Update` | Update user role assignment | ? Compatible |
| `sys_user_roles_Delete` | Remove user role assignment | ? Compatible |
| `sys_roles_Get_ById` | Get role information by ID | ? Compatible |
| `sys_SetUserAccessType` | Set user access level (Admin/ReadOnly/Normal) | ? Compatible |
| `sys_GetUserAccessType` | Get access types for all users | ? Compatible |
| `sys_GetUserIdByName` | Get user ID by username | ? Compatible |
| `sys_GetRoleIdByName` | Get role ID by role name | ? Compatible |

### 3. Master Data Procedures (03_Master_Data_Procedures.sql)

| Procedure Name | Purpose |
|----------------|---------|
| `md_part_ids_Add_Part` | Add new part number |
| `md_part_ids_Update_Part` | Update existing part |
| `md_part_ids_Delete_Part` | Delete part number |
| `md_part_ids_Get_All` | Get all parts |
| `md_part_ids_GetItemType_ByPartID` | Get item type for part |
| `md_operation_numbers_Add_Operation` | Add new operation |
| `md_operation_numbers_Update_Operation` | Update operation |
| `md_operation_numbers_Delete_ByOperation` | Delete operation |
| `md_operation_numbers_Get_All` | Get all operations |
| `md_operation_numbers_Exists_ByOperation` | Check if operation exists |

### 4. Inventory Procedures (04_Inventory_Procedures.sql)

| Procedure Name | Purpose |
|----------------|---------|
| `inv_inventory_Add_Item` | Add inventory item with transaction logging |
| `inv_inventory_Remove_Item_1_1` | Remove inventory with detailed validation |
| `inv_inventory_Transfer_Part` | Transfer part to new location |
| `inv_inventory_transfer_quantity` | Transfer specific quantity |
| `inv_inventory_GetNextBatchNumber` | Generate next batch number |
| `inv_inventory_Fix_BatchNumbers` | Consolidate and fix batch numbers |
| `inv_inventory_Get_ByPartID` | Get inventory by part ID |
| `inv_inventory_Get_ByPartIDAndOperation` | Get inventory by part and operation |
| `inv_transaction_GetProblematicBatchCount` | Count problematic batches |
| `inv_transaction_GetProblematicBatches` | List problematic batch numbers |
| `inv_transaction_SplitBatchNumbers` | Split batch numbers by date |

### 5. Error Log Procedures (05_Error_Log_Procedures.sql)

| Procedure Name | Purpose |
|----------------|---------|
| `log_error_Add_Error` | Add error log entry with comprehensive details |
| `log_error_Get_All` | Get all error log entries |
| `log_error_Get_ByUser` | Get error log entries by user |
| `log_error_Get_ByDateRange` | Get error log entries by date range |
| `log_error_Get_Unique` | Get unique error combinations (method + message) |
| `log_error_Delete_ById` | Delete specific error log entry |
| `log_error_Delete_All` | Delete all error log entries |

## Validation and Testing

### Post-Deployment Verification (MAMP)

1. **Check Procedure Creation via phpMyAdmin**:
   - Navigate to `http://localhost/phpMyAdmin`
   - Select `mtm_wip_application` database
   - Click "Routines" tab to view all stored procedures

2. **Check Procedure Creation via Command Line**:
```sql
-- Connect to MAMP MySQL
mysql -h localhost -P 3306 -u root -p

-- Select database
USE mtm_wip_application;

-- Verify all procedures were created
SELECT ROUTINE_NAME, ROUTINE_TYPE, CREATED 
FROM INFORMATION_SCHEMA.ROUTINES 
WHERE ROUTINE_SCHEMA = 'mtm_wip_application' 
ORDER BY ROUTINE_NAME;
```

3. **Test Key Procedures**:
```sql
-- Test user management (with status reporting)
CALL usr_users_Get_All(@status, @error_msg);
SELECT @status as Status, @error_msg as ErrorMessage;

-- Test system roles
CALL sys_GetUserAccessType();

-- Test master data
CALL md_operation_numbers_Get_All(@status, @error_msg);
SELECT @status as Status, @error_msg as ErrorMessage;
```

4. **Test Error Log Procedures**:
```sql
-- Test error logging
CALL log_error('Test error message');

-- Retrieve and check error log
CALL usp_get_error_log();
```

### MySQL 5.7.24 Specific Testing
```sql
-- Test MySQL version compatibility
SELECT VERSION() as MySQLVersion;

-- Test JSON functionality (MySQL 5.7.8+)
SELECT JSON_VALID('{"test": "value"}') as JSONSupported;

-- Test boolean compatibility
SELECT CAST(1 AS UNSIGNED) as BooleanTest;
```

## Common Issues and Troubleshooting (MySQL 5.7.24)

### MySQL 5.7 Specific Issues

1. **JSON Type Compatibility**:
   - **Solution**: Procedures use `TEXT` parameters for broader compatibility
   - JSON validation done at application level

2. **Boolean Parameter Handling**:
   - **Issue**: MySQL 5.7 boolean handling differs from 8.0
   - **Solution**: Use `TINYINT(1)` instead of `BOOLEAN`

3. **Error Handler Syntax**:
   - **Solution**: Simplified error handling compatible with MySQL 5.7

4. **Transaction Handling**:
   - **Solution**: Explicit `START TRANSACTION`/`COMMIT`/`ROLLBACK` statements

### MAMP Specific Issues

1. **Port Conflicts**:
   - Check MAMP control panel for actual MySQL port
   - Common ports: 3306 (newer MAMP), 8889 (older MAMP)

2. **Permission Denied**:
   ```sql
   -- Grant necessary privileges in MAMP
   GRANT ALL PRIVILEGES ON mtm_wip_application.* TO 'root'@'localhost';
   GRANT CREATE ROUTINE ON mtm_wip_application.* TO 'root'@'localhost';
   FLUSH PRIVILEGES;
   ```

3. **Character Set Issues**:
   - MAMP default charset is usually utf8
   - Ensure database uses consistent character set

## Integration with Application (.NET 8)

### MAMP Connection String Example
```csharp
// Connection string for MAMP (adjust port as needed)
string connectionString = "Server=localhost;Port=3306;Database=mtm_wip_application;Uid=root;Pwd=root;Allow User Variables=True;";
```

### MySQL 5.7 Specific Considerations
```csharp
// Handle boolean parameters for MySQL 5.7
command.Parameters.AddWithValue("p_VitsUser", Convert.ToInt32(booleanValue));

// Handle JSON parameters
command.Parameters.AddWithValue("p_JsonData", jsonString); // TEXT parameter
```

## MAMP Development Workflow

### Recommended Development Setup
1. **Install MAMP** with MySQL 5.7.24+
2. **Start MAMP services** (Apache + MySQL)
3. **Create database** via phpMyAdmin
4. **Deploy procedures** using automated scripts
5. **Configure application** connection string for MAMP
6. **Test procedures** via phpMyAdmin or MySQL client

### MAMP Production Considerations
- MAMP is recommended for **development only**
- For production, use dedicated MySQL server
- Ensure proper backup procedures
- Monitor performance and security

## Summary

This deployment package is now fully compatible with:
- ? **MySQL 5.7.24** (your current version)
- ? **MAMP** development environment
- ? **Windows and macOS/Linux** platforms
- ? **Automated deployment** scripts
- ? **Enhanced error handling** with visual feedback
- ? **Comprehensive troubleshooting** guidance

The stored procedures provide the same functionality as the MySQL 8.0 version but with compatibility adjustments for MySQL 5.7.24 and optimized deployment for MAMP environments.
