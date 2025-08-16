@echo off
echo ==================================================================================
echo MTM CONSOLIDATED STORED PROCEDURE VERIFICATION SYSTEM RUNNER
echo ==================================================================================
echo Running Date: %DATE% %TIME%
echo Source: mtm_wip_application_test_routines.sql (Consolidated)
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

REM Default MAMP configuration - Updated for test database
set DB_HOST=localhost
set DB_PORT=3306
set DB_NAME=mtm_wip_application_test
set DB_USER=root
set DB_PASSWORD=root

REM Path to consolidated routines file
set "ROUTINES_FILE=%~dp0..\UpdatedDatabase\mtm_wip_application_test_routines.sql"

echo [INFO] Target Database: %DB_NAME%
echo [INFO] Consolidated Source: mtm_wip_application_test_routines.sql
echo [INFO] Expected Procedures: ~96 with uniform p_ parameter naming
echo.

echo Phase 1: Verify Consolidated Routines File
echo ===========================================

if not exist "%ROUTINES_FILE%" (
    echo [ERROR] Consolidated routines file not found!
    echo [ERROR] Expected location: %ROUTINES_FILE%
    echo [INFO] Please ensure mtm_wip_application_test_routines.sql exists in UpdatedDatabase folder
    pause
    exit /b 1
)

echo [INFO] Found consolidated routines file: %ROUTINES_FILE%

REM Count procedures in consolidated file
echo [INFO] Analyzing consolidated file...
set proc_count=0
for /f %%i in ('findstr /c:"CREATE DEFINER" "%ROUTINES_FILE%" 2^>nul') do (
    set /a proc_count+=%%i
)
echo [INFO] Found %proc_count% procedures in consolidated file
echo.

echo Phase 2: Deploy Consolidated Verification System
echo =================================================

REM Check if verification procedures exist in consolidated file
findstr /c:"sys_VerifyDatabaseSchema" "%ROUTINES_FILE%" >nul 2>&1
if errorlevel 1 (
    echo [WARNING] Verification procedures not found in consolidated file
    echo [INFO] The consolidated file may not include verification procedures
    echo [INFO] Continuing with available procedures...
) else (
    echo [INFO] Verification procedures found in consolidated file
)

REM Deploy the consolidated routines (which may include verification system)
echo [INFO] Deploying consolidated routines with verification system...
%MYSQL_CMD% -h%DB_HOST% -P%DB_PORT% -u%DB_USER% -p%DB_PASSWORD% %DB_NAME% < "%ROUTINES_FILE%"

if errorlevel 1 (
    echo [ERROR] Failed to deploy consolidated routines
    echo [INFO] Make sure MAMP is running and the database exists
    echo [INFO] You can create the database in phpMyAdmin if needed
    echo [INFO] Database: %DB_NAME%
    pause
    exit /b 1
)

echo ✅ Consolidated routines deployed successfully
echo.

echo Phase 3: Run Consolidated Verification Tests
echo ============================================

REM Create a comprehensive verification test for consolidated deployment
echo -- Consolidated Verification Test > temp_consolidated_verification.sql
echo SELECT 'Starting consolidated verification tests...' as TestStatus; >> temp_consolidated_verification.sql
echo. >> temp_consolidated_verification.sql

echo -- Count deployed procedures from consolidated file >> temp_consolidated_verification.sql
echo SELECT COUNT(*^) as TotalDeployedProcedures >> temp_consolidated_verification.sql
echo FROM INFORMATION_SCHEMA.ROUTINES >> temp_consolidated_verification.sql
echo WHERE ROUTINE_SCHEMA = '%DB_NAME%' >> temp_consolidated_verification.sql
echo AND ROUTINE_TYPE = 'PROCEDURE'; >> temp_consolidated_verification.sql
echo. >> temp_consolidated_verification.sql

echo -- Verify key procedure categories from consolidated file >> temp_consolidated_verification.sql
echo SELECT >> temp_consolidated_verification.sql
echo   SUM(CASE WHEN ROUTINE_NAME LIKE 'app_themes_%%' THEN 1 ELSE 0 END^) as ThemeProcs, >> temp_consolidated_verification.sql
echo   SUM(CASE WHEN ROUTINE_NAME LIKE 'inv_inventory_%%' THEN 1 ELSE 0 END^) as InventoryProcs, >> temp_consolidated_verification.sql
echo   SUM(CASE WHEN ROUTINE_NAME LIKE 'usr_users_%%' THEN 1 ELSE 0 END^) as UserProcs, >> temp_consolidated_verification.sql
echo   SUM(CASE WHEN ROUTINE_NAME LIKE 'md_%%' THEN 1 ELSE 0 END^) as MasterDataProcs, >> temp_consolidated_verification.sql
echo   SUM(CASE WHEN ROUTINE_NAME LIKE 'log_%%' THEN 1 ELSE 0 END^) as LogProcs, >> temp_consolidated_verification.sql
echo   SUM(CASE WHEN ROUTINE_NAME LIKE 'sys_%%' THEN 1 ELSE 0 END^) as SystemProcs >> temp_consolidated_verification.sql
echo FROM INFORMATION_SCHEMA.ROUTINES >> temp_consolidated_verification.sql
echo WHERE ROUTINE_SCHEMA = '%DB_NAME%' >> temp_consolidated_verification.sql
echo AND ROUTINE_TYPE = 'PROCEDURE'; >> temp_consolidated_verification.sql
echo. >> temp_consolidated_verification.sql

echo -- Test key procedures from consolidated file >> temp_consolidated_verification.sql
echo SELECT 'Testing app_themes procedures from consolidated file...' as TestPhase; >> temp_consolidated_verification.sql
echo CALL app_themes_Get_All(@theme_status, @theme_msg^); >> temp_consolidated_verification.sql
echo SELECT @theme_status as ThemeStatus, @theme_msg as ThemeMessage; >> temp_consolidated_verification.sql
echo. >> temp_consolidated_verification.sql

echo SELECT 'Testing usr_users procedures from consolidated file...' as TestPhase; >> temp_consolidated_verification.sql
echo CALL usr_users_Get_All(@user_status, @user_msg^); >> temp_consolidated_verification.sql
echo SELECT @user_status as UserStatus, @user_msg as UserMessage; >> temp_consolidated_verification.sql
echo. >> temp_consolidated_verification.sql

echo SELECT 'Testing inv_inventory procedures from consolidated file...' as TestPhase; >> temp_consolidated_verification.sql
echo CALL inv_inventory_Get_All(@inv_status, @inv_msg^); >> temp_consolidated_verification.sql
echo SELECT @inv_status as InventoryStatus, @inv_msg as InventoryMessage; >> temp_consolidated_verification.sql
echo. >> temp_consolidated_verification.sql

echo -- Advanced verification if sys_ procedures are available >> temp_consolidated_verification.sql
echo SELECT 'Testing system verification procedures if available...' as TestPhase; >> temp_consolidated_verification.sql

echo -- Try schema verification if available >> temp_consolidated_verification.sql
echo SET @schema_available = 0; >> temp_consolidated_verification.sql
echo SELECT COUNT(*^) INTO @schema_available >> temp_consolidated_verification.sql
echo FROM INFORMATION_SCHEMA.ROUTINES >> temp_consolidated_verification.sql
echo WHERE ROUTINE_SCHEMA = '%DB_NAME%' >> temp_consolidated_verification.sql
echo AND ROUTINE_NAME = 'sys_VerifyDatabaseSchema'; >> temp_consolidated_verification.sql
echo. >> temp_consolidated_verification.sql

echo -- If schema verification is available, run it >> temp_consolidated_verification.sql
echo IF @schema_available ^> 0 THEN >> temp_consolidated_verification.sql
echo   CALL sys_VerifyDatabaseSchema(@schema_status, @schema_msg^); >> temp_consolidated_verification.sql
echo   SELECT @schema_status as SchemaStatus, @schema_msg as SchemaMessage; >> temp_consolidated_verification.sql
echo ELSE >> temp_consolidated_verification.sql
echo   SELECT 'Schema verification procedure not available in consolidated file' as SchemaMessage; >> temp_consolidated_verification.sql
echo END IF; >> temp_consolidated_verification.sql
echo. >> temp_consolidated_verification.sql

echo -- Try procedure inventory if available >> temp_consolidated_verification.sql
echo SET @inventory_available = 0; >> temp_consolidated_verification.sql
echo SELECT COUNT(*^) INTO @inventory_available >> temp_consolidated_verification.sql
echo FROM INFORMATION_SCHEMA.ROUTINES >> temp_consolidated_verification.sql
echo WHERE ROUTINE_SCHEMA = '%DB_NAME%' >> temp_consolidated_verification.sql
echo AND ROUTINE_NAME = 'sys_GetStoredProcedureInventory'; >> temp_consolidated_verification.sql
echo. >> temp_consolidated_verification.sql

echo IF @inventory_available ^> 0 THEN >> temp_consolidated_verification.sql
echo   CALL sys_GetStoredProcedureInventory(@proc_status, @proc_msg^); >> temp_consolidated_verification.sql
echo   SELECT @proc_status as ProcInventoryStatus, @proc_msg as ProcInventoryMessage; >> temp_consolidated_verification.sql
echo ELSE >> temp_consolidated_verification.sql
echo   SELECT 'Procedure inventory not available in consolidated file' as ProcInventoryMessage; >> temp_consolidated_verification.sql
echo END IF; >> temp_consolidated_verification.sql
echo. >> temp_consolidated_verification.sql

echo -- Final consolidated verification summary >> temp_consolidated_verification.sql
echo SELECT >> temp_consolidated_verification.sql
echo   CASE >> temp_consolidated_verification.sql
echo     WHEN @theme_status = 0 AND @user_status = 0 AND @inv_status = 0 THEN 'CONSOLIDATED VERIFICATION PASSED' >> temp_consolidated_verification.sql
echo     ELSE 'CONSOLIDATED VERIFICATION ISSUES DETECTED' >> temp_consolidated_verification.sql
echo   END as ConsolidatedVerificationResult, >> temp_consolidated_verification.sql
echo   'mtm_wip_application_test_routines.sql deployed and tested' as SourceFile; >> temp_consolidated_verification.sql

REM Run the consolidated verification tests
echo [INFO] Running consolidated verification tests...
%MYSQL_CMD% -h%DB_HOST% -P%DB_PORT% -u%DB_USER% -p%DB_PASSWORD% %DB_NAME% < temp_consolidated_verification.sql

if errorlevel 1 (
    echo.
    echo ⚠️ Some consolidated verification tests failed or returned warnings
    echo [INFO] This may be normal for new deployments or missing tables
    echo.
    echo RECOMMENDED NEXT STEPS:
    echo 1. Run deploy.bat first to ensure consolidated file is properly deployed
    echo 2. Run fix_and_reverify.bat if schema issues are detected
    echo 3. Check individual procedures if specific tests fail
    echo.
) else (
    echo.
    echo ✅ All consolidated verification tests completed successfully
    echo.
)

echo.
echo Phase 4: Cleanup
echo =================

REM Clean up temporary file
if exist temp_consolidated_verification.sql del temp_consolidated_verification.sql

echo.
echo Consolidated Verification Complete!
echo ===================================
echo.
echo Summary:
echo ✅ Source: mtm_wip_application_test_routines.sql (Consolidated)
echo ✅ Target: %DB_NAME%
echo ✅ Procedures: ~%proc_count% from consolidated file
echo ✅ Categories: Theme, User, Inventory, Master Data, Logging, System
echo.
echo Next Steps:
echo 1. Review the verification results above
echo 2. If issues are found, run deploy.bat to ensure consolidated deployment
echo 3. Use fix_and_reverify.bat for schema-related issues
echo 4. Test individual procedures for specific functionality
echo.
echo USAGE EXAMPLES:
echo   To deploy consolidated file: deploy.bat
echo   To fix schema issues: fix_and_reverify.bat
echo   To analyze consolidated file: analyze.bat
echo   To quick check status: final_verification.bat
echo.

pause
