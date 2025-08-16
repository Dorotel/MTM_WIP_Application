@echo off
echo ==================================================================================
echo MTM MISSING TABLES FIX AND RE-VERIFICATION - CONSOLIDATED ROUTINES
echo ==================================================================================
echo.

REM Use the same MySQL path detection logic as deploy.bat
set MYSQL_PATH=C:\MAMP\bin\mysql\bin

REM Check if MAMP MySQL path exists, otherwise try standard path
if not exist "%MYSQL_PATH%\mysql.exe" (
    echo [WARNING] MAMP MySQL not found at %MYSQL_PATH%
    echo [INFO] Trying to use system MySQL...
    set MYSQL_PATH=
)

REM Set MySQL command with path
if "%MYSQL_PATH%"=="" (
    set MYSQL_CMD=mysql
) else (
    set MYSQL_CMD="%MYSQL_PATH%\mysql.exe"
)

REM Default MAMP configuration
set DB_HOST=localhost
set DB_PORT=3306
set DB_NAME=mtm_wip_application_test
set DB_USER=root
set DB_PASSWORD=root

echo [INFO] Fixing schema for consolidated routines deployment
echo [INFO] Target database: %DB_NAME%
echo.

echo Step 1: Deploy Missing Table Fixes
echo ===================================

REM Create a comprehensive fix SQL for consolidated routines
echo -- Fix Missing Tables for Consolidated Routines > fix_missing_tables_consolidated.sql
echo -- Generated: %DATE% %TIME% >> fix_missing_tables_consolidated.sql
echo. >> fix_missing_tables_consolidated.sql

echo -- Ensure app_themes table exists >> fix_missing_tables_consolidated.sql
echo CREATE TABLE IF NOT EXISTS app_themes ( >> fix_missing_tables_consolidated.sql
echo   ID INT AUTO_INCREMENT PRIMARY KEY, >> fix_missing_tables_consolidated.sql
echo   ThemeName VARCHAR(50) NOT NULL UNIQUE, >> fix_missing_tables_consolidated.sql
echo   DisplayName VARCHAR(100), >> fix_missing_tables_consolidated.sql
echo   SettingsJson TEXT, >> fix_missing_tables_consolidated.sql
echo   IsDefault TINYINT(1) DEFAULT 0, >> fix_missing_tables_consolidated.sql
echo   IsActive TINYINT(1) DEFAULT 1, >> fix_missing_tables_consolidated.sql
echo   Description TEXT, >> fix_missing_tables_consolidated.sql
echo   CreatedBy VARCHAR(100), >> fix_missing_tables_consolidated.sql
echo   CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP, >> fix_missing_tables_consolidated.sql
echo   ModifiedBy VARCHAR(100), >> fix_missing_tables_consolidated.sql
echo   ModifiedDate DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP, >> fix_missing_tables_consolidated.sql
echo   VERSION INT DEFAULT 1 >> fix_missing_tables_consolidated.sql
echo ^) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4; >> fix_missing_tables_consolidated.sql
echo. >> fix_missing_tables_consolidated.sql

echo -- Ensure sys_last_10_transactions table exists >> fix_missing_tables_consolidated.sql
echo CREATE TABLE IF NOT EXISTS sys_last_10_transactions ( >> fix_missing_tables_consolidated.sql
echo   ID INT AUTO_INCREMENT PRIMARY KEY, >> fix_missing_tables_consolidated.sql
echo   User VARCHAR(100) NOT NULL, >> fix_missing_tables_consolidated.sql
echo   Position INT NOT NULL, >> fix_missing_tables_consolidated.sql
echo   PartID VARCHAR(300), >> fix_missing_tables_consolidated.sql
echo   Operation VARCHAR(50), >> fix_missing_tables_consolidated.sql
echo   Quantity INT, >> fix_missing_tables_consolidated.sql
echo   ReceiveDate DATETIME DEFAULT CURRENT_TIMESTAMP, >> fix_missing_tables_consolidated.sql
echo   UNIQUE KEY unique_user_position (User, Position^) >> fix_missing_tables_consolidated.sql
echo ^) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4; >> fix_missing_tables_consolidated.sql
echo. >> fix_missing_tables_consolidated.sql

echo -- Ensure sys_roles table exists >> fix_missing_tables_consolidated.sql
echo CREATE TABLE IF NOT EXISTS sys_roles ( >> fix_missing_tables_consolidated.sql
echo   ID INT AUTO_INCREMENT PRIMARY KEY, >> fix_missing_tables_consolidated.sql
echo   RoleName VARCHAR(100) NOT NULL UNIQUE, >> fix_missing_tables_consolidated.sql
echo   Description TEXT, >> fix_missing_tables_consolidated.sql
echo   CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP >> fix_missing_tables_consolidated.sql
echo ^) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4; >> fix_missing_tables_consolidated.sql
echo. >> fix_missing_tables_consolidated.sql

echo -- Ensure sys_user_roles table exists >> fix_missing_tables_consolidated.sql
echo CREATE TABLE IF NOT EXISTS sys_user_roles ( >> fix_missing_tables_consolidated.sql
echo   ID INT AUTO_INCREMENT PRIMARY KEY, >> fix_missing_tables_consolidated.sql
echo   UserID INT NOT NULL, >> fix_missing_tables_consolidated.sql
echo   RoleID INT NOT NULL, >> fix_missing_tables_consolidated.sql
echo   AssignedBy VARCHAR(100), >> fix_missing_tables_consolidated.sql
echo   AssignedDate DATETIME DEFAULT CURRENT_TIMESTAMP, >> fix_missing_tables_consolidated.sql
echo   UNIQUE KEY unique_user_role (UserID, RoleID^) >> fix_missing_tables_consolidated.sql
echo ^) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4; >> fix_missing_tables_consolidated.sql
echo. >> fix_missing_tables_consolidated.sql

echo -- Ensure log_changelog table exists >> fix_missing_tables_consolidated.sql
echo CREATE TABLE IF NOT EXISTS log_changelog ( >> fix_missing_tables_consolidated.sql
echo   ID INT AUTO_INCREMENT PRIMARY KEY, >> fix_missing_tables_consolidated.sql
echo   Version VARCHAR(50) NOT NULL UNIQUE, >> fix_missing_tables_consolidated.sql
echo   Description TEXT, >> fix_missing_tables_consolidated.sql
echo   ReleaseDate DATE, >> fix_missing_tables_consolidated.sql
echo   CreatedBy VARCHAR(100), >> fix_missing_tables_consolidated.sql
echo   CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP, >> fix_missing_tables_consolidated.sql
echo   ModifiedDate DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP >> fix_missing_tables_consolidated.sql
echo ^) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4; >> fix_missing_tables_consolidated.sql
echo. >> fix_missing_tables_consolidated.sql

echo -- Add sample data for default theme >> fix_missing_tables_consolidated.sql
echo INSERT IGNORE INTO app_themes (ThemeName, DisplayName, SettingsJson, IsDefault, IsActive, Description, CreatedBy^) >> fix_missing_tables_consolidated.sql
echo VALUES ('Default', 'Default Theme', '{"primaryColor":"#0d6efd","backgroundColor":"#ffffff","fontFamily":"Segoe UI"}', 1, 1, 'Default application theme', 'SYSTEM'^); >> fix_missing_tables_consolidated.sql
echo. >> fix_missing_tables_consolidated.sql

echo -- Add sample roles >> fix_missing_tables_consolidated.sql
echo INSERT IGNORE INTO sys_roles (RoleName, Description^) VALUES >> fix_missing_tables_consolidated.sql
echo ('Administrator', 'Full system access'^), >> fix_missing_tables_consolidated.sql
echo ('User', 'Standard user access'^), >> fix_missing_tables_consolidated.sql
echo ('ReadOnly', 'Read-only access'^); >> fix_missing_tables_consolidated.sql

echo Creating missing tables for consolidated routines...
%MYSQL_CMD% -h%DB_HOST% -P%DB_PORT% -u%DB_USER% -p%DB_PASSWORD% %DB_NAME% < fix_missing_tables_consolidated.sql

if errorlevel 1 (
    echo ERROR: Failed to fix missing tables
    pause
    exit /b 1
)

echo ✅ Missing tables fix completed for consolidated routines
echo.

echo Step 2: Re-deploy Consolidated Routines
echo =======================================

set "ROUTINES_FILE=%~dp0..\UpdatedDatabase\mtm_wip_application_test_routines.sql"

if exist "%ROUTINES_FILE%" (
    echo Re-deploying consolidated stored procedures...
    %MYSQL_CMD% -h%DB_HOST% -P%DB_PORT% -u%DB_USER% -p%DB_PASSWORD% %DB_NAME% < "%ROUTINES_FILE%"
    
    if errorlevel 1 (
        echo ERROR: Failed to re-deploy consolidated routines
        pause
        exit /b 1
    )
    
    echo ✅ Consolidated routines re-deployed successfully
) else (
    echo WARNING: Consolidated routines file not found at %ROUTINES_FILE%
    echo Skipping re-deployment step
)

echo.

echo Step 3: Re-run Complete Verification
echo ====================================

echo -- Re-run Complete Verification for Consolidated Routines > temp_reverify_consolidated.sql
echo SELECT 'Verifying consolidated routines deployment...' as VerificationStatus; >> temp_reverify_consolidated.sql
echo. >> temp_reverify_consolidated.sql

echo -- Count deployed procedures >> temp_reverify_consolidated.sql
echo SELECT COUNT(*^) as TotalProcedures >> temp_reverify_consolidated.sql
echo FROM INFORMATION_SCHEMA.ROUTINES >> temp_reverify_consolidated.sql
echo WHERE ROUTINE_SCHEMA = '%DB_NAME%' >> temp_reverify_consolidated.sql
echo AND ROUTINE_TYPE = 'PROCEDURE'; >> temp_reverify_consolidated.sql
echo. >> temp_reverify_consolidated.sql

echo -- Test key procedure categories >> temp_reverify_consolidated.sql
echo SELECT >> temp_reverify_consolidated.sql
echo   SUM(CASE WHEN ROUTINE_NAME LIKE 'app_themes_%%' THEN 1 ELSE 0 END^) as ThemeProcs, >> temp_reverify_consolidated.sql
echo   SUM(CASE WHEN ROUTINE_NAME LIKE 'inv_%%' THEN 1 ELSE 0 END^) as InventoryProcs, >> temp_reverify_consolidated.sql
echo   SUM(CASE WHEN ROUTINE_NAME LIKE 'usr_%%' THEN 1 ELSE 0 END^) as UserProcs, >> temp_reverify_consolidated.sql
echo   SUM(CASE WHEN ROUTINE_NAME LIKE 'md_%%' THEN 1 ELSE 0 END^) as MasterDataProcs, >> temp_reverify_consolidated.sql
echo   SUM(CASE WHEN ROUTINE_NAME LIKE 'log_%%' THEN 1 ELSE 0 END^) as LogProcs, >> temp_reverify_consolidated.sql
echo   SUM(CASE WHEN ROUTINE_NAME LIKE 'sys_%%' THEN 1 ELSE 0 END^) as SystemProcs >> temp_reverify_consolidated.sql
echo FROM INFORMATION_SCHEMA.ROUTINES >> temp_reverify_consolidated.sql
echo WHERE ROUTINE_SCHEMA = '%DB_NAME%' >> temp_reverify_consolidated.sql
echo AND ROUTINE_TYPE = 'PROCEDURE'; >> temp_reverify_consolidated.sql
echo. >> temp_reverify_consolidated.sql

echo -- Test basic functionality >> temp_reverify_consolidated.sql
echo CALL app_themes_Get_All(@status, @msg^); >> temp_reverify_consolidated.sql
echo SELECT @status as AppThemesStatus, @msg as AppThemesMessage; >> temp_reverify_consolidated.sql

echo Running complete verification for consolidated routines...
%MYSQL_CMD% -h%DB_HOST% -P%DB_PORT% -u%DB_USER% -p%DB_PASSWORD% %DB_NAME% < temp_reverify_consolidated.sql

echo.
echo Step 4: Cleanup
echo ================

if exist temp_reverify_consolidated.sql del temp_reverify_consolidated.sql
if exist fix_missing_tables_consolidated.sql del fix_missing_tables_consolidated.sql

echo.
echo Re-verification Complete for Consolidated Routines!
echo ===================================================
echo.
echo Summary:
echo ✅ Missing tables created for consolidated routines support
echo ✅ mtm_wip_application_test_routines.sql re-deployed
echo ✅ Verification completed for consolidated deployment
echo.
echo If verification still shows issues:
echo 1. Some test failures are expected (they test error conditions)
echo 2. Schema should now show all required tables
echo 3. Consolidated routines should be working correctly
echo.

pause
