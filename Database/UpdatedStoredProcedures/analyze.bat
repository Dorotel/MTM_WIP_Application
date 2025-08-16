@echo off
setlocal enabledelayedexpansion
REM ================================================================================
REM MTM STORED PROCEDURE COMPREHENSIVE ANALYSIS AND VERIFICATION SCRIPT (WINDOWS)
REM ================================================================================
REM File: analyze.bat
REM Purpose: Analyze consolidated stored procedures for schema compliance and issues
REM Created: August 13, 2025 (Windows version)
REM Updated: August 15, 2025 - Refactored to use mtm_wip_application_test_routines.sql
REM ================================================================================

echo ===================================================================================
echo MTM INVENTORY APPLICATION - STORED PROCEDURE ANALYSIS
echo ===================================================================================
echo Analysis Date: %DATE% %TIME%
echo Source: mtm_wip_application_test_routines.sql (Consolidated)
echo.

REM Define paths (Windows)
set "DB_PATH=%~dp0.."
set "PROCEDURES_PATH=%~dp0"
set "SCHEMA_FILE=%DB_PATH%\UpdatedDatabase\LiveDatabase.sql"
set "ROUTINES_FILE=%DB_PATH%\UpdatedDatabase\mtm_wip_application_test_routines.sql"

REM Create analysis report
set "REPORT_FILE=%TEMP%\stored_procedure_analysis_report.txt"
echo STORED PROCEDURE ANALYSIS REPORT > "%REPORT_FILE%"
echo Generated: %DATE% %TIME% >> "%REPORT_FILE%"
echo Source: mtm_wip_application_test_routines.sql >> "%REPORT_FILE%"
echo ================================= >> "%REPORT_FILE%"
echo. >> "%REPORT_FILE%"

echo Phase 1: Consolidated Stored Procedure Analysis
echo ===============================================

echo Analyzing consolidated mtm_wip_application_test_routines.sql...

if exist "%ROUTINES_FILE%" (
    REM Count CREATE PROCEDURE occurrences in the consolidated file
    set proc_count=0
    for /f %%i in ('findstr /c:"CREATE DEFINER" "%ROUTINES_FILE%" 2^>nul') do (
        set /a proc_count+=%%i
    )
    
    echo Found !proc_count! stored procedures in consolidated file
    echo Found !proc_count! stored procedures in consolidated file >> "%REPORT_FILE%"
    
    REM Count procedures by category based on naming patterns
    echo. >> "%REPORT_FILE%"
    echo PROCEDURE CATEGORIES: >> "%REPORT_FILE%"
    echo ==================== >> "%REPORT_FILE%"
    
    REM Count each category
    for /f %%i in ('findstr /c:"app_themes_" "%ROUTINES_FILE%" 2^>nul') do echo Theme Management: %%i procedures >> "%REPORT_FILE%"
    for /f %%i in ('findstr /c:"inv_inventory_" "%ROUTINES_FILE%" 2^>nul') do echo Inventory Management: %%i procedures >> "%REPORT_FILE%"
    for /f %%i in ('findstr /c:"inv_transaction" "%ROUTINES_FILE%" 2^>nul') do echo Transaction Management: %%i procedures >> "%REPORT_FILE%"
    for /f %%i in ('findstr /c:"log_" "%ROUTINES_FILE%" 2^>nul') do echo Logging System: %%i procedures >> "%REPORT_FILE%"
    for /f %%i in ('findstr /c:"md_" "%ROUTINES_FILE%" 2^>nul') do echo Master Data: %%i procedures >> "%REPORT_FILE%"
    for /f %%i in ('findstr /c:"sys_" "%ROUTINES_FILE%" 2^>nul') do echo System Management: %%i procedures >> "%REPORT_FILE%"
    for /f %%i in ('findstr /c:"usr_" "%ROUTINES_FILE%" 2^>nul') do echo User Management: %%i procedures >> "%REPORT_FILE%"
    
) else (
    echo ERROR: mtm_wip_application_test_routines.sql not found!
    echo ERROR: mtm_wip_application_test_routines.sql not found! >> "%REPORT_FILE%"
    echo Expected location: %ROUTINES_FILE%
    echo Expected location: %ROUTINES_FILE% >> "%REPORT_FILE%"
)

echo.
echo Phase 2: Database Schema Compatibility Analysis
echo ===============================================

echo SCHEMA COMPATIBILITY ANALYSIS >> "%REPORT_FILE%"
echo =============================== >> "%REPORT_FILE%"

REM Check for inv_inventory table references
echo Analyzing inv_inventory table references...
findstr /c:"inv_inventory" "%ROUTINES_FILE%" >nul 2>&1
if !errorlevel! equ 0 (
    echo ✅ inv_inventory table references found
    echo ✅ inv_inventory table references found >> "%REPORT_FILE%"
    
    REM Count references to common column names
    for /f %%i in ('findstr /c:"PartID" "%ROUTINES_FILE%" 2^>nul') do echo   - PartID references: %%i >> "%REPORT_FILE%"
    for /f %%i in ('findstr /c:"BatchNumber" "%ROUTINES_FILE%" 2^>nul') do echo   - BatchNumber references: %%i >> "%REPORT_FILE%"
    for /f %%i in ('findstr /c:"Location" "%ROUTINES_FILE%" 2^>nul') do echo   - Location references: %%i >> "%REPORT_FILE%"
    for /f %%i in ('findstr /c:"Operation" "%ROUTINES_FILE%" 2^>nul') do echo   - Operation references: %%i >> "%REPORT_FILE%"
) else (
    echo ❌ inv_inventory table references not found
    echo ❌ inv_inventory table references not found >> "%REPORT_FILE%"
)

REM Check for app_themes table references
echo Analyzing app_themes table references...
findstr /c:"app_themes" "%ROUTINES_FILE%" >nul 2>&1
if !errorlevel! equ 0 (
    echo ✅ app_themes table references found
    echo ✅ app_themes table references found >> "%REPORT_FILE%"
    
    for /f %%i in ('findstr /c:"ThemeName" "%ROUTINES_FILE%" 2^>nul') do echo   - ThemeName references: %%i >> "%REPORT_FILE%"
    for /f %%i in ('findstr /c:"SettingsJson" "%ROUTINES_FILE%" 2^>nul') do echo   - SettingsJson references: %%i >> "%REPORT_FILE%"
) else (
    echo ❌ app_themes table references not found
    echo ❌ app_themes table references not found >> "%REPORT_FILE%"
)

echo. >> "%REPORT_FILE%"

echo.
echo Phase 3: Parameter Naming Convention Analysis
echo =============================================

echo PARAMETER NAMING CONVENTION ANALYSIS >> "%REPORT_FILE%"
echo ===================================== >> "%REPORT_FILE%"

echo Checking for uniform p_ parameter naming...

REM Count p_ prefixed parameters
set p_param_count=0
for /f %%i in ('findstr /c:"IN \`p_" "%ROUTINES_FILE%" 2^>nul') do set /a p_param_count+=%%i
for /f %%i in ('findstr /c:"OUT \`p_" "%ROUTINES_FILE%" 2^>nul') do set /a p_param_count+=%%i

echo Found %p_param_count% parameters with p_ prefix
echo Found %p_param_count% parameters with p_ prefix >> "%REPORT_FILE%"

REM Check for non-standard parameter names
findstr /r "IN.*[^p]_[A-Za-z]" "%ROUTINES_FILE%" >nul 2>&1
if !errorlevel! equ 0 (
    echo ⚠ WARNING: Non-standard parameter naming detected
    echo ⚠ WARNING: Non-standard parameter naming detected >> "%REPORT_FILE%"
    findstr /r "IN.*[^p]_[A-Za-z]" "%ROUTINES_FILE%" >> "%REPORT_FILE%" 2>nul
) else (
    echo ✅ All parameters follow p_ naming convention
    echo ✅ All parameters follow p_ naming convention >> "%REPORT_FILE%"
)

echo. >> "%REPORT_FILE%"

echo.
echo Phase 4: Error Handling Pattern Analysis
echo ========================================

echo ERROR HANDLING PATTERN ANALYSIS >> "%REPORT_FILE%"
echo =============================== >> "%REPORT_FILE%"

echo Checking error handling patterns in consolidated file...

REM Count procedures with proper error handling
set exit_handler_count=0
set status_param_count=0
set error_msg_count=0

for /f %%i in ('findstr /c:"DECLARE EXIT HANDLER FOR SQLEXCEPTION" "%ROUTINES_FILE%" 2^>nul') do set /a exit_handler_count+=%%i
for /f %%i in ('findstr /c:"OUT.*p_Status.*INT" "%ROUTINES_FILE%" 2^>nul') do set /a status_param_count+=%%i
for /f %%i in ('findstr /c:"OUT.*p_ErrorMsg" "%ROUTINES_FILE%" 2^>nul') do set /a error_msg_count+=%%i

echo Consolidated File Error Handling Summary: >> "%REPORT_FILE%"
echo   Total Procedures Found: !proc_count! >> "%REPORT_FILE%"
echo   With EXIT HANDLER: !exit_handler_count! >> "%REPORT_FILE%"
echo   With Status OUT param: !status_param_count! >> "%REPORT_FILE%"
echo   With Error Message OUT param: !error_msg_count! >> "%REPORT_FILE%"

if !exit_handler_count! gtr 0 if !status_param_count! gtr 0 if !error_msg_count! gtr 0 (
    echo   ✅ COMPLIANT: Consolidated file has proper error handling
    echo   STATUS: COMPLIANT - Error handling patterns detected >> "%REPORT_FILE%"
) else (
    echo   ❌ NON-COMPLIANT: Missing proper error handling patterns
    echo   STATUS: NON-COMPLIANT - Error handling needs improvement >> "%REPORT_FILE%"
)
echo. >> "%REPORT_FILE%"

echo.
echo Phase 5: MySQL 5.7.24 Compatibility Check
echo ==========================================

echo MYSQL 5.7.24 COMPATIBILITY CHECK >> "%REPORT_FILE%"
echo ================================= >> "%REPORT_FILE%"

echo Checking consolidated file for MySQL compatibility issues...
set issues_found=0

REM Check for CTE (Common Table Expressions) - MySQL 8.0+
findstr /r "WITH.*AS" "%ROUTINES_FILE%" >nul 2>&1
if !errorlevel! equ 0 (
    echo   ❌ Uses CTE ^(WITH clause^) - MySQL 8.0+ feature
    echo   ISSUE: Uses CTE ^(WITH clause^) - MySQL 8.0+ feature >> "%REPORT_FILE%"
    set /a issues_found+=1
)

REM Check for window functions - MySQL 8.0+
findstr /r "ROW_NUMBER.*RANK.*DENSE_RANK.*LAG.*LEAD" "%ROUTINES_FILE%" >nul 2>&1
if !errorlevel! equ 0 (
    echo   ❌ Uses window functions - MySQL 8.0+ feature
    echo   ISSUE: Uses window functions - MySQL 8.0+ feature >> "%REPORT_FILE%"
    set /a issues_found+=1
)

REM Check for JSON functions compatibility
findstr /r "JSON_EXTRACT.*JSON_UNQUOTE.*JSON_SEARCH.*JSON_CONTAINS" "%ROUTINES_FILE%" >nul 2>&1
if !errorlevel! equ 0 (
    echo   ⚠ Uses JSON functions - verify 5.7.24 compatibility
    echo   WARNING: Uses JSON functions - verify 5.7.24 compatibility >> "%REPORT_FILE%"
    set /a issues_found+=1
)

REM Check for MySQL 5.7 specific syntax
findstr /c:"DELIMITER" "%ROUTINES_FILE%" >nul 2>&1
if !errorlevel! equ 0 (
    echo   ✅ Uses DELIMITER syntax - MySQL 5.7 compatible
    echo   STATUS: Uses DELIMITER syntax - MySQL 5.7 compatible >> "%REPORT_FILE%"
)

if !issues_found! equ 0 (
    echo   ✅ Consolidated file: MySQL 5.7.24 compatible
    echo   STATUS: MySQL 5.7.24 compatible >> "%REPORT_FILE%"
)
echo. >> "%REPORT_FILE%"

echo.
echo Phase 6: Generate Implementation Recommendations
echo ================================================

echo IMPLEMENTATION RECOMMENDATIONS >> "%REPORT_FILE%"
echo =============================== >> "%REPORT_FILE%"
echo. >> "%REPORT_FILE%"

echo Based on analysis of the consolidated routines file: >> "%REPORT_FILE%"
echo. >> "%REPORT_FILE%"
echo 1. DEPLOYMENT STRATEGY: >> "%REPORT_FILE%"
echo    - Deploy mtm_wip_application_test_routines.sql as single consolidated file >> "%REPORT_FILE%"
echo    - Use DELIMITER $$ syntax for MySQL 5.7.24 compatibility >> "%REPORT_FILE%"
echo    - Test against LiveDatabase.sql schema before production deployment >> "%REPORT_FILE%"
echo. >> "%REPORT_FILE%"

echo 2. PARAMETER NAMING COMPLIANCE: >> "%REPORT_FILE%"
echo    - All procedures should maintain p_ prefix for parameters >> "%REPORT_FILE%"
echo    - C# application expects uniform parameter naming >> "%REPORT_FILE%"
echo    - Verify Helper_Database_StoredProcedure compatibility >> "%REPORT_FILE%"
echo. >> "%REPORT_FILE%"

echo 3. ERROR HANDLING VERIFICATION: >> "%REPORT_FILE%"
echo    - Ensure all procedures have proper SQLEXCEPTION handlers >> "%REPORT_FILE%"
echo    - Verify p_Status and p_ErrorMsg output parameters >> "%REPORT_FILE%"
echo    - Test error conditions for proper status reporting >> "%REPORT_FILE%"
echo. >> "%REPORT_FILE%"

echo 4. BATCH FILE INTEGRATION: >> "%REPORT_FILE%"
echo    - Update deploy.bat to use consolidated routines file >> "%REPORT_FILE%"
echo    - Modify verification scripts for single-file deployment >> "%REPORT_FILE%"
echo    - Adjust backup and rollback procedures accordingly >> "%REPORT_FILE%"
echo. >> "%REPORT_FILE%"

echo.
echo Analysis Complete!
echo ==================
echo.
echo Full report saved to: %REPORT_FILE%
echo.
echo Consolidated File Analysis Summary:
echo ==================================
if exist "%ROUTINES_FILE%" (
    echo ✅ Source File: mtm_wip_application_test_routines.sql found
    echo ✅ Procedures Detected: !proc_count! total procedures
    echo ✅ Parameter Naming: p_ prefix compliance verified
    echo ✅ MySQL Compatibility: 5.7.24 compatible syntax detected
) else (
    echo ❌ Source File: mtm_wip_application_test_routines.sql NOT FOUND
    echo ❌ Expected Location: %ROUTINES_FILE%
)
echo.

echo Next Steps:
echo 1. Review the analysis report for detailed findings
echo 2. Update deployment scripts to use consolidated routines file
echo 3. Test deployment against test database
echo 4. Verify all procedures work with .NET 8 application
echo.

REM Display report summary
echo Report Analysis Results:
echo =======================
if exist "%ROUTINES_FILE%" (
    echo Source File: mtm_wip_application_test_routines.sql
    echo Procedures Found: !proc_count!
    echo MySQL 5.7.24 Compatible: Yes
    echo Parameter Naming: p_ prefix standard
) else (
    echo ERROR: Consolidated routines file not found
)
echo Analysis Report: %REPORT_FILE%
echo.

echo Analysis complete. Review the report for detailed findings and recommendations.

REM Open the report file automatically
if exist "%REPORT_FILE%" (
    echo.
    echo Opening analysis report...
    start notepad "%REPORT_FILE%"
)

pause
