@echo off
echo ==================================================================================
echo MTM STORED PROCEDURE VERIFICATION SYSTEM RUNNER
echo ==================================================================================
echo Running Date: %DATE% %TIME%
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
set DB_NAME=mtm_wip_application
set DB_USER=root
set DB_PASSWORD=root

echo Phase 1: Deploy Verification System
echo ====================================

REM Deploy the verification system
echo Deploying verification system...
%MYSQL_CMD% -h%DB_HOST% -P%DB_PORT% -u%DB_USER% -p%DB_PASSWORD% %DB_NAME% < 00_StoredProcedure_Verification_System.sql

if errorlevel 1 (
    echo ERROR: Failed to deploy verification system
    echo INFO: Make sure MAMP is running and the database exists
    echo INFO: You can create the database in phpMyAdmin if needed
    pause
    exit /b 1
)

echo ? Verification system deployed successfully
echo.

echo Phase 2: Run Schema Verification
echo =================================

REM Create a test SQL script
echo -- Schema Verification Test > temp_verification_test.sql
echo CALL sys_VerifyDatabaseSchema(@status, @msg); >> temp_verification_test.sql
echo SELECT @status as Status, @msg as Message; >> temp_verification_test.sql
echo. >> temp_verification_test.sql

echo -- Procedure Inventory >> temp_verification_test.sql
echo CALL sys_GetStoredProcedureInventory(@status2, @msg2); >> temp_verification_test.sql
echo SELECT @status2 as Status, @msg2 as Message; >> temp_verification_test.sql
echo. >> temp_verification_test.sql

echo -- Complete Verification >> temp_verification_test.sql
echo CALL sys_RunCompleteVerification(@overall, @summary); >> temp_verification_test.sql
echo SELECT @overall as OverallStatus, @summary as DetailedReport; >> temp_verification_test.sql

REM Run the verification tests
echo Running verification tests...
%MYSQL_CMD% -h%DB_HOST% -P%DB_PORT% -u%DB_USER% -p%DB_PASSWORD% %DB_NAME% < temp_verification_test.sql

if errorlevel 1 (
    echo.
    echo ? Some verification tests failed or returned warnings
    echo This is normal if stored procedures are not yet deployed
    echo.
    echo RECOMMENDED NEXT STEPS:
    echo 1. Run deploy.bat first to deploy all stored procedures
    echo 2. Then run this verification again
    echo.
) else (
    echo.
    echo ? All verification tests completed successfully
    echo.
)

echo.
echo Phase 3: Cleanup
echo =================

REM Clean up temporary file
if exist temp_verification_test.sql del temp_verification_test.sql

echo.
echo Verification Complete!
echo ======================
echo.
echo Next Steps:
echo 1. Review the verification results above
echo 2. If issues are found, run deploy.bat to ensure all procedures are deployed
echo 3. Check individual stored procedures if specific tests fail
echo.
echo USAGE EXAMPLES:
echo   To deploy all procedures first: deploy.bat
echo   To verify a specific table: Use individual verification procedures in MySQL
echo.

pause
