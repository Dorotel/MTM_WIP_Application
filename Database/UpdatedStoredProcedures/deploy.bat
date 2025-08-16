@echo off
REM ================================================================================
REM MTM INVENTORY APPLICATION - CONSOLIDATED STORED PROCEDURES DEPLOYMENT SCRIPT
REM ================================================================================
REM File: deploy.bat
REM Purpose: Deploy consolidated stored procedures from mtm_wip_application_test_routines.sql
REM Created: August 10, 2025
REM Updated: August 15, 2025 - Refactored to use consolidated routines file
REM Target Database: mtm_wip_application_test (development/test database)
REM MySQL Version: 5.7.24+ (MAMP Compatible)
REM 
REM ENVIRONMENT LOGIC:
REM - This script deploys the consolidated mtm_wip_application_test_routines.sql file
REM - For production deployment, use database name: mtm_wip_application
REM - Debug Mode (C#): Uses mtm_wip_application_test and localhost or 172.16.1.104
REM - Release Mode (C#): Uses mtm_wip_application and always 172.16.1.104
REM ================================================================================

setlocal enabledelayedexpansion

REM Default MAMP configuration - UPDATED FOR TEST DATABASE
set DB_HOST=localhost
set DB_PORT=3306
set DB_NAME=mtm_wip_application_test
set DB_USER=root
set DB_PASSWORD=root

REM MAMP MySQL path (adjust if needed)
set MYSQL_PATH=C:\MAMP\bin\mysql\bin
set MYSQLDUMP_PATH=C:\MAMP\bin\mysql\bin

REM Path to consolidated routines file
set "ROUTINES_FILE=%~dp0..\UpdatedDatabase\mtm_wip_application_test_routines.sql"

REM Check if MAMP MySQL path exists, otherwise try standard path
if not exist "%MYSQL_PATH%\mysql.exe" (
    echo [WARNING] MAMP MySQL not found at %MYSQL_PATH%
    echo [INFO] Trying to use system MySQL...
    set MYSQL_PATH=
    set MYSQLDUMP_PATH=
)

REM Parse command line arguments
:parse_args
if "%~1"=="" goto :validate_params
if "%~1"=="-h" (
    set DB_HOST=%~2
    shift
    shift
    goto :parse_args
)
if "%~1"=="--host" (
    set DB_HOST=%~2
    shift
    shift
    goto :parse_args
)
if "%~1"=="-P" (
    set DB_PORT=%~2
    shift
    shift
    goto :parse_args
)
if "%~1"=="--port" (
    set DB_PORT=%~2
    shift
    shift
    goto :parse_args
)
if "%~1"=="-u" (
    set DB_USER=%~2
    shift
    shift
    goto :parse_args
)
if "%~1"=="--user" (
    set DB_USER=%~2
    shift
    shift
    goto :parse_args
)
if "%~1"=="-p" (
    set DB_PASSWORD=%~2
    shift
    shift
    goto :parse_args
)
if "%~1"=="--password" (
    set DB_PASSWORD=%~2
    shift
    shift
    goto :parse_args
)
if "%~1"=="-d" (
    set DB_NAME=%~2
    shift
    shift
    goto :parse_args
)
if "%~1"=="--database" (
    set DB_NAME=%~2
    shift
    shift
    goto :parse_args
)
if "%~1"=="--mamp-path" (
    set MYSQL_PATH=%~2\bin\mysql\bin
    set MYSQLDUMP_PATH=%~2\bin\mysql\bin
    shift
    shift
    goto :parse_args
)
if "%~1"=="--help" (
    call :show_usage
    exit /b 0
)
echo [ERROR] Unknown option: %~1
call :show_usage
exit /b 1

:validate_params
if "%DB_PASSWORD%"=="" (
    echo [ERROR] Database password is required. Use -p option or set DB_PASSWORD environment variable.
    echo [INFO] For MAMP, default password is usually 'root'
    exit /b 1
)

REM Set MySQL command with path
if "%MYSQL_PATH%"=="" (
    set MYSQL_CMD=mysql
    set MYSQLDUMP_CMD=mysqldump
) else (
    set MYSQL_CMD="%MYSQL_PATH%\mysql.exe"
    set MYSQLDUMP_CMD="%MYSQLDUMP_PATH%\mysqldump.exe"
)

REM Print header
echo ================================
echo MTM INVENTORY APPLICATION - CONSOLIDATED STORED PROCEDURES DEPLOYMENT
echo ================================
echo [INFO] Target: MySQL %DB_HOST%:%DB_PORT%/%DB_NAME%
echo [INFO] User: %DB_USER%
echo [INFO] MySQL Client: !MYSQL_CMD!
echo [INFO] Source: mtm_wip_application_test_routines.sql (Consolidated)
echo [INFO] UNIFORM PARAMETER NAMING: WITH p_ prefixes
echo ================================

REM Check if consolidated routines file exists
if not exist "%ROUTINES_FILE%" (
    echo [ERROR] Consolidated routines file not found!
    echo [ERROR] Expected location: %ROUTINES_FILE%
    echo [INFO] Please ensure mtm_wip_application_test_routines.sql exists in UpdatedDatabase folder
    exit /b 1
)

echo [INFO] Found consolidated routines file: %ROUTINES_FILE%

REM Test database connection
echo [INFO] Testing database connection...
!MYSQL_CMD! -h%DB_HOST% -P%DB_PORT% -u%DB_USER% -p%DB_PASSWORD% -e "SELECT VERSION() as 'MySQL Version', NOW() as 'Current Time';" %DB_NAME% 2>nul
if errorlevel 1 (
    echo [ERROR] Cannot connect to database. Please check your credentials and ensure MAMP is running.
    echo [INFO] Common MAMP connection parameters:
    echo [INFO]   Host: localhost
    echo [INFO]   Port: 3306 ^(or 8889 for older MAMP versions^)
    echo [INFO]   User: root
    echo [INFO]   Password: root
    echo [INFO]   Database: %DB_NAME%
    echo [INFO] Make sure MAMP Apache and MySQL services are started.
    exit /b 1
)
echo [INFO] Database connection successful

REM Create backup
echo [INFO] Creating backup of existing procedures...
set backup_file=stored_procedures_backup_%date:~10,4%%date:~4,2%%date:~7,2%_%time:~0,2%%time:~3,2%%time:~6,2%.sql
set backup_file=!backup_file: =0!
!MYSQLDUMP_CMD! -h%DB_HOST% -P%DB_PORT% -u%DB_USER% -p%DB_PASSWORD% --routines --no-create-info --no-data --no-create-db %DB_NAME% > %backup_file% 2>nul
if errorlevel 1 (
    echo [WARNING] Backup creation failed, but continuing with deployment...
) else (
    echo [INFO] Backup created: %backup_file%
)

REM Count procedures in consolidated file
echo [INFO] Analyzing consolidated procedures file...
set proc_count=0
for /f %%i in ('findstr /c:"CREATE DEFINER" "%ROUTINES_FILE%" 2^>nul') do (
    set /a proc_count+=%%i
)
echo [INFO] Found %proc_count% procedures in consolidated file

REM Deploy consolidated procedures
echo [INFO] Deploying consolidated stored procedures ^(UNIFORM p_ prefixes^)...
!MYSQL_CMD! -h%DB_HOST% -P%DB_PORT% -u%DB_USER% -p%DB_PASSWORD% %DB_NAME% < "%ROUTINES_FILE%"

if errorlevel 1 (
    echo [ERROR] Consolidated procedures deployment failed
    echo [ERROR] Check the following:
    echo [ERROR]   1. Database %DB_NAME% exists and is accessible
    echo [ERROR]   2. User %DB_USER% has CREATE ROUTINE privileges
    echo [ERROR]   3. mtm_wip_application_test_routines.sql syntax is valid
    echo [ERROR]   4. MySQL version is 5.7.24 or higher
    exit /b 1
) else (
    echo [SUCCESS] Consolidated procedures deployed successfully!
)

REM MySQL 5.7.24 Compatibility Check
echo [INFO] Checking MySQL version compatibility...
!MYSQL_CMD! -h%DB_HOST% -P%DB_PORT% -u%DB_USER% -p%DB_PASSWORD% -e "SELECT VERSION();" %DB_NAME% 2>nul
if errorlevel 1 (
    echo [WARNING] Could not verify MySQL version
) else (
    echo [INFO] MySQL version check completed - procedures optimized for MySQL 5.7.24+
)

REM Verify deployment by counting deployed procedures
echo [INFO] Verifying deployment...
!MYSQL_CMD! -h%DB_HOST% -P%DB_PORT% -u%DB_USER% -p%DB_PASSWORD% -e "SELECT COUNT(*) as 'Deployed Procedures' FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_SCHEMA='%DB_NAME%' AND ROUTINE_TYPE='PROCEDURE';" %DB_NAME% 2>nul

REM Summary
echo ================================
echo DEPLOYMENT SUMMARY
echo ================================
echo [INFO] Source: mtm_wip_application_test_routines.sql
echo [INFO] Procedures in file: %proc_count%
echo [INFO] UNIFORM PARAMETER NAMING: All procedures use p_ prefixes

echo [SUCCESS] Consolidated stored procedures deployment completed!
echo [SUCCESS] UNIFORM PARAMETER NAMING implementation complete!
echo [INFO] Features deployed from consolidated file:
echo [INFO]   - User Management with p_ prefixes
echo [INFO]   - System Roles with p_ prefixes  
echo [INFO]   - Master Data with p_ prefixes
echo [INFO]   - Inventory Management with p_ prefixes
echo [INFO]   - Error Logging with p_ prefixes
echo [INFO]   - Quick Buttons with p_ prefixes
echo [INFO]   - Changelog/Version with p_ prefixes
echo [INFO]   - Theme Management with p_ prefixes
echo [INFO] Total: ~%proc_count% procedures with uniform p_ parameter naming
echo [INFO] Deployment completed for MySQL 5.7.24 ^(MAMP Compatible^)
exit /b 0

:show_usage
echo Usage: %0 [options]
echo.
echo MTM Inventory Application - Consolidated Stored Procedures Deployment
echo UNIFORM PARAMETER NAMING: All procedures use p_ prefixes for consistency
echo Source: mtm_wip_application_test_routines.sql
echo.
echo Options:
echo   -h, --host HOST        Database host ^(default: localhost^)
echo   -P, --port PORT        Database port ^(default: 3306^)
echo   -u, --user USER        Database username ^(default: root^)
echo   -p, --password PASS    Database password ^(default: root for MAMP^)
echo   -d, --database DB      Database name ^(default: mtm_wip_application_test^)
echo   --mamp-path PATH       MAMP installation path ^(default: C:\MAMP^)
echo   --help                 Show this help message
echo.
echo Environment variables:
echo   DB_HOST, DB_PORT, DB_USER, DB_PASSWORD, DB_NAME
echo.
echo Consolidated deployment:
echo   Single file: mtm_wip_application_test_routines.sql
echo   Contains: ~96 procedures with p_ parameter naming
echo   Categories: User Management, System Roles, Master Data, Inventory,
echo              Error Logging, Quick Buttons, Changelog, Theme Management
echo.
echo MAMP Examples:
echo   %0 -h localhost -u root -p root -d mtm_wip_application_test
echo   %0 --mamp-path "C:\MAMP" -p root
echo   %0 -P 8889 -p root  ^(for older MAMP versions^)
echo.
echo MAMP Troubleshooting:
echo   1. Start MAMP and ensure Apache/MySQL services are running
echo   2. Check MAMP control panel for correct port ^(usually 3306^)
echo   3. Default MAMP credentials are usually root/root
echo   4. Ensure target database exists in phpMyAdmin
echo   5. Verify mtm_wip_application_test_routines.sql exists in UpdatedDatabase folder
echo.
exit /b 0
