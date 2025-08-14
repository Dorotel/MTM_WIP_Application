@echo off
echo ==================================================================================
echo MTM MISSING TABLES FIX AND RE-VERIFICATION
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
set DB_NAME=mtm_wip_application
set DB_USER=root
set DB_PASSWORD=root

echo Step 1: Fix Missing Tables
echo ==========================

echo Creating any missing tables...
%MYSQL_CMD% -h%DB_HOST% -P%DB_PORT% -u%DB_USER% -p%DB_PASSWORD% %DB_NAME% < fix_missing_tables.sql

if errorlevel 1 (
    echo ERROR: Failed to fix missing tables
    pause
    exit /b 1
)

echo ? Missing tables fix completed
echo.

echo Step 2: Re-run Complete Verification
echo =====================================

echo -- Re-run Complete Verification > temp_reverify.sql
echo CALL sys_RunCompleteVerification(@overall, @summary); >> temp_reverify.sql
echo SELECT @overall as OverallStatus, @summary as DetailedReport; >> temp_reverify.sql

echo Running complete verification...
%MYSQL_CMD% -h%DB_HOST% -P%DB_PORT% -u%DB_USER% -p%DB_PASSWORD% %DB_NAME% < temp_reverify.sql

echo.
echo Step 3: Cleanup
echo ================

if exist temp_reverify.sql del temp_reverify.sql

echo.
echo Re-verification Complete!
echo =========================
echo.
echo If the verification still shows issues:
echo 1. Some test failures are expected (they test error conditions)
echo 2. Schema should now show all 17 tables found
echo 3. Most procedures should be working correctly
echo.

pause
