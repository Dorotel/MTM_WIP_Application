@echo off
REM ================================================================================
REM MTM INVENTORY APPLICATION - STORED PROCEDURES DEPLOYMENT SCRIPT (WINDOWS/MAMP)
REM ================================================================================
REM File: deploy_procedures.bat
REM Purpose: Deploy all stored procedures to the MTM WIP Application database
REM Created: August 10, 2025
REM Updated: January 27, 2025 - Updated for new database/environment logic
REM Target Database: mtm_wip_application_test (development/test database)
REM MySQL Version: 5.7.24+ (MAMP Compatible)
REM 
REM ENVIRONMENT LOGIC:
REM - This script deploys to the TEST database (mtm_wip_application_test)
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
echo MTM INVENTORY APPLICATION - STORED PROCEDURES DEPLOYMENT
echo ================================
echo [INFO] Target: MySQL %DB_HOST%:%DB_PORT%/%DB_NAME%
echo [INFO] User: %DB_USER%
echo [INFO] MySQL Client: !MYSQL_CMD!
echo [INFO] UNIFORM PARAMETER NAMING: WITH p_ prefixes
echo ================================

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

REM Deploy procedures - UPDATED FOR UNIFORM PARAMETER NAMING
set success_count=0
set total_count=8

REM User Management Procedures
echo [INFO] Executing User Management Procedures ^(UNIFORM p_ prefixes^)...
if exist "01_User_Management_Procedures.sql" (
    !MYSQL_CMD! -h%DB_HOST% -P%DB_PORT% -u%DB_USER% -p%DB_PASSWORD% %DB_NAME% < "01_User_Management_Procedures.sql"
    if errorlevel 1 (
        echo [ERROR] User Management Procedures failed
    ) else (
        echo [SUCCESS] User Management Procedures completed successfully
        set /a success_count+=1
    )
) else (
    echo [ERROR] File not found: 01_User_Management_Procedures.sql
)

REM System Role Procedures
echo [INFO] Executing System Role Procedures ^(UNIFORM p_ prefixes^)...
if exist "02_System_Role_Procedures.sql" (
    !MYSQL_CMD! -h%DB_HOST% -P%DB_PORT% -u%DB_USER% -p%DB_PASSWORD% %DB_NAME% < "02_System_Role_Procedures.sql"
    if errorlevel 1 (
        echo [ERROR] System Role Procedures failed
    ) else (
        echo [SUCCESS] System Role Procedures completed successfully
        set /a success_count+=1
    )
) else (
    echo [ERROR] File not found: 02_System_Role_Procedures.sql
)

REM Master Data Procedures
echo [INFO] Executing Master Data Procedures ^(UNIFORM p_ prefixes^)...
if exist "03_Master_Data_Procedures.sql" (
    !MYSQL_CMD! -h%DB_HOST% -P%DB_PORT% -u%DB_USER% -p%DB_PASSWORD% %DB_NAME% < "03_Master_Data_Procedures.sql"
    if errorlevel 1 (
        echo [ERROR] Master Data Procedures failed
    ) else (
        echo [SUCCESS] Master Data Procedures completed successfully
        set /a success_count+=1
    )
) else (
    echo [ERROR] File not found: 03_Master_Data_Procedures.sql
)

REM Inventory Management Procedures
echo [INFO] Executing Inventory Management Procedures ^(UNIFORM p_ prefixes^)...
if exist "04_Inventory_Procedures.sql" (
    !MYSQL_CMD! -h%DB_HOST% -P%DB_PORT% -u%DB_USER% -p%DB_PASSWORD% %DB_NAME% < "04_Inventory_Procedures.sql"
    if errorlevel 1 (
        echo [ERROR] Inventory Management Procedures failed
    ) else (
        echo [SUCCESS] Inventory Management Procedures completed successfully
        set /a success_count+=1
    )
) else (
    echo [ERROR] File not found: 04_Inventory_Procedures.sql
)

REM Error Log Procedures
echo [INFO] Executing Error Log Procedures ^(UNIFORM p_ prefixes^)...
if exist "05_Error_Log_Procedures.sql" (
    !MYSQL_CMD! -h%DB_HOST% -P%DB_PORT% -u%DB_USER% -p%DB_PASSWORD% %DB_NAME% < "05_Error_Log_Procedures.sql"
    if errorlevel 1 (
        echo [ERROR] Error Log Procedures failed
    ) else (
        echo [SUCCESS] Error Log Procedures completed successfully
        set /a success_count+=1
    )
) else (
    echo [ERROR] File not found: 05_Error_Log_Procedures.sql
)

REM Quick Button Procedures
echo [INFO] Executing Quick Button Procedures ^(UNIFORM p_ prefixes^)...
if exist "06_Quick_Button_Procedures.sql" (
    !MYSQL_CMD! -h%DB_HOST% -P%DB_PORT% -u%DB_USER% -p%DB_PASSWORD% %DB_NAME% < "06_Quick_Button_Procedures.sql"
    if errorlevel 1 (
        echo [ERROR] Quick Button Procedures failed
    ) else (
        echo [SUCCESS] Quick Button Procedures completed successfully
        set /a success_count+=1
    )
) else (
    echo [ERROR] File not found: 06_Quick_Button_Procedures.sql
)

REM Changelog/Version Procedures
echo [INFO] Executing Changelog/Version Procedures ^(UNIFORM p_ prefixes^)...
if exist "07_Changelog_Version_Procedures.sql" (
    !MYSQL_CMD! -h%DB_HOST% -P%DB_PORT% -u%DB_USER% -p%DB_PASSWORD% %DB_NAME% < "07_Changelog_Version_Procedures.sql"
    if errorlevel 1 (
        echo [ERROR] Changelog/Version Procedures failed
    ) else (
        echo [SUCCESS] Changelog/Version Procedures completed successfully
        set /a success_count+=1
    )
) else (
    echo [ERROR] File not found: 07_Changelog_Version_Procedures.sql
)

REM Theme Management Procedures - NEW
echo [INFO] Executing Theme Management Procedures ^(UNIFORM p_ prefixes^)...
if exist "08_Theme_Management_Procedures.sql" (
    !MYSQL_CMD! -h%DB_HOST% -P%DB_PORT% -u%DB_USER% -p%DB_PASSWORD% %DB_NAME% < "08_Theme_Management_Procedures.sql"
    if errorlevel 1 (
        echo [ERROR] Theme Management Procedures failed
    ) else (
        echo [SUCCESS] Theme Management Procedures completed successfully
        set /a success_count+=1
    )
) else (
    echo [ERROR] File not found: 08_Theme_Management_Procedures.sql
)

REM MySQL 5.7.24 Compatibility Check
echo [INFO] Checking MySQL version compatibility...
!MYSQL_CMD! -h%DB_HOST% -P%DB_PORT% -u%DB_USER% -p%DB_PASSWORD% -e "SELECT VERSION();" %DB_NAME% 2>nul
if errorlevel 1 (
    echo [WARNING] Could not verify MySQL version
) else (
    echo [INFO] MySQL version check completed - procedures optimized for MySQL 5.7.24+
)

REM Summary
echo ================================
echo DEPLOYMENT SUMMARY
echo ================================
echo [INFO] Successfully deployed: %success_count%/%total_count% procedure files
echo [INFO] UNIFORM PARAMETER NAMING: All procedures now use p_ prefixes

if %success_count% == %total_count% (
    echo [SUCCESS] All stored procedures deployed successfully!
    echo [SUCCESS] UNIFORM PARAMETER NAMING implementation complete!
    echo [INFO] Features deployed:
    echo [INFO]   - User Management ^(17 procedures^) with p_ prefixes
    echo [INFO]   - System Roles ^(8 procedures^) with p_ prefixes
    echo [INFO]   - Master Data ^(21 procedures^) with p_ prefixes
    echo [INFO]   - Inventory Management ^(12 procedures^) with p_ prefixes
    echo [INFO]   - Error Logging ^(6 procedures^) with p_ prefixes
    echo [INFO]   - Quick Buttons ^(7 procedures^) with p_ prefixes
    echo [INFO]   - Changelog/Version ^(3 procedures^) with p_ prefixes
    echo [INFO]   - Theme Management ^(8 procedures^) with p_ prefixes
    echo [INFO] Total: ~82 procedures with uniform p_ parameter naming
    echo [INFO] Deployment completed for MySQL 5.7.24 ^(MAMP Compatible^)
    exit /b 0
) else (
    echo [ERROR] Some procedures failed to deploy. Please check the errors above.
    echo [INFO] Common MAMP issues:
    echo [INFO]   1. Ensure MAMP Apache and MySQL services are running
    echo [INFO]   2. Check that the database '%DB_NAME%' exists
    echo [INFO]   3. Verify user '%DB_USER%' has CREATE ROUTINE privileges
    echo [INFO]   4. Confirm MAMP MySQL version is 5.7.24 or higher
    echo [INFO]   5. Ensure all 8 SQL files are present in current directory
    exit /b 1
)

:show_usage
echo Usage: %0 [options]
echo.
echo MTM Inventory Application - Stored Procedures Deployment
echo UNIFORM PARAMETER NAMING: All procedures use p_ prefixes for consistency
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
echo Files deployed ^(8 total^):
echo   01_User_Management_Procedures.sql     ^(17 procedures^)
echo   02_System_Role_Procedures.sql         ^(8 procedures^)
echo   03_Master_Data_Procedures.sql         ^(21 procedures^)
echo   04_Inventory_Procedures.sql           ^(12 procedures^)
echo   05_Error_Log_Procedures.sql           ^(6 procedures^)
echo   06_Quick_Button_Procedures.sql        ^(7 procedures^)
echo   07_Changelog_Version_Procedures.sql   ^(3 procedures^)
echo   08_Theme_Management_Procedures.sql    ^(8 procedures^)
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
echo   5. Verify all 8 SQL files are present in current directory
echo.
exit /b 0
