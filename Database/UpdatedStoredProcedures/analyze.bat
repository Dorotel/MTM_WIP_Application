@echo off
setlocal enabledelayedexpansion
REM ================================================================================
REM MTM STORED PROCEDURE COMPREHENSIVE ANALYSIS AND VERIFICATION SCRIPT (WINDOWS)
REM ================================================================================
REM File: analyze.bat
REM Purpose: Analyze all stored procedures for schema compliance and issues
REM Created: August 13, 2025 (Windows version)
REM ================================================================================

echo ===================================================================================
echo MTM INVENTORY APPLICATION - STORED PROCEDURE ANALYSIS
echo ===================================================================================
echo Analysis Date: %DATE% %TIME%
echo.

REM Define paths (Windows)
set "DB_PATH=%~dp0.."
set "PROCEDURES_PATH=%~dp0"
set "SCHEMA_FILE=%DB_PATH%\UpdatedDatabase\LiveDatabase.sql"

REM Create analysis report
set "REPORT_FILE=%TEMP%\stored_procedure_analysis_report.txt"
echo STORED PROCEDURE ANALYSIS REPORT > "%REPORT_FILE%"
echo Generated: %DATE% %TIME% >> "%REPORT_FILE%"
echo ================================= >> "%REPORT_FILE%"
echo. >> "%REPORT_FILE%"

echo Phase 1: Stored Procedure File Inventory
echo =========================================

REM Count procedures in each file
set total_procedures=0
for %%f in ("%PROCEDURES_PATH%\*.sql") do (
    if exist "%%f" (
        set "filename=%%~nxf"
        
        REM Count CREATE PROCEDURE occurrences
        set proc_count=0
        for /f %%i in ('findstr /c:"CREATE PROCEDURE" "%%f" 2^>nul') do (
            set /a proc_count+=1
        )
        
        set /a total_procedures+=!proc_count!
        echo !filename!: !proc_count! procedures
        echo !filename!: !proc_count! procedures >> "%REPORT_FILE%"
    )
)

echo.
echo Total Stored Procedures Found: %total_procedures%
echo Total Stored Procedures Found: %total_procedures% >> "%REPORT_FILE%"
echo. >> "%REPORT_FILE%"

echo.
echo Phase 2: Database Schema Analysis
echo ==================================

echo CORE TABLE STRUCTURES FROM LIVEDATABASE.SQL >> "%REPORT_FILE%"
echo ============================================ >> "%REPORT_FILE%"

REM Check for inv_inventory table
echo Analyzing inv_inventory table structure...
findstr /c:"CREATE TABLE" /c:"inv_inventory" "%SCHEMA_FILE%" >nul 2>&1
if !errorlevel! equ 0 (
    echo ? inv_inventory table found
    echo. >> "%REPORT_FILE%"
    echo inv_inventory table columns: >> "%REPORT_FILE%"
    findstr /r "ID.*PartID.*Location.*Operation.*Quantity.*ItemType.*ReceiveDate.*LastUpdated.*User.*BatchNumber.*Notes" "%SCHEMA_FILE%" >> "%REPORT_FILE%" 2>nul || echo   Schema extraction incomplete >> "%REPORT_FILE%"
) else (
    echo ? inv_inventory table not found in schema
)

REM Check for inv_transaction table
echo Analyzing inv_transaction table structure...
findstr /c:"CREATE TABLE" /c:"inv_transaction" "%SCHEMA_FILE%" >nul 2>&1
if !errorlevel! equ 0 (
    echo ? inv_transaction table found
    echo. >> "%REPORT_FILE%"
    echo inv_transaction table columns: >> "%REPORT_FILE%"
    findstr /r "ID.*TransactionType.*BatchNumber.*PartID.*FromLocation.*ToLocation.*Operation.*Quantity.*Notes.*User.*ItemType.*ReceiveDate" "%SCHEMA_FILE%" >> "%REPORT_FILE%" 2>nul || echo   Schema extraction incomplete >> "%REPORT_FILE%"
) else (
    echo ? inv_transaction table not found in schema
)

REM Check for app_themes table
echo Analyzing app_themes table structure...
findstr /c:"CREATE TABLE" /c:"app_themes" "%SCHEMA_FILE%" >nul 2>&1
if !errorlevel! equ 0 (
    echo ? app_themes table found
    echo. >> "%REPORT_FILE%"
    echo app_themes table columns: >> "%REPORT_FILE%"
    findstr /r "ThemeName.*SettingsJson" "%SCHEMA_FILE%" >> "%REPORT_FILE%" 2>nul || echo   Schema extraction incomplete >> "%REPORT_FILE%"
) else (
    echo ? app_themes table not found in schema
)

echo. >> "%REPORT_FILE%"

echo.
echo Phase 3: Column Name Verification
echo ==================================

echo COLUMN NAME VERIFICATION RESULTS >> "%REPORT_FILE%"
echo ================================ >> "%REPORT_FILE%"

echo Checking for common column name mismatches...

REM Define common column name issues
set "issues[0]=FullName:Full Name"
set "issues[1]=TransactionDate:ReceiveDate"
set "issues[2]=DateTime:ReceiveDate"
set "issues[3]=TimeStamp:ReceiveDate"
set "issues[4]=FirstName:First_Name"
set "issues[5]=LastName:Last_Name"
set "issues[6]=UserName:User_Name"

for %%f in ("%PROCEDURES_PATH%\*.sql") do (
    if exist "%%f" (
        set "filename=%%~nxf"
        echo Analyzing !filename!... >> "%REPORT_FILE%"
        
        REM Check for each common issue
        for /l %%i in (0,1,6) do (
            set "issue=!issues[%%i]!"
            for /f "tokens=1,2 delims=:" %%a in ("!issue!") do (
                set "wrong_col=%%a"
                set "correct_col=%%b"
                
                findstr /c:"!wrong_col!" "%%f" >nul 2>&1
                if !errorlevel! equ 0 (
                    echo   ? ISSUE: !filename! contains '!wrong_col!' ^(should be '!correct_col!'^)
                    echo   ISSUE: !filename! contains '!wrong_col!' ^(should be '!correct_col!'^ >> "%REPORT_FILE%"
                )
            )
        )
        echo. >> "%REPORT_FILE%"
    )
)

echo.
echo Phase 4: Error Handling Pattern Analysis
echo ========================================

echo ERROR HANDLING PATTERN ANALYSIS >> "%REPORT_FILE%"
echo =============================== >> "%REPORT_FILE%"

echo Checking error handling patterns...
for %%f in ("%PROCEDURES_PATH%\*.sql") do (
    if exist "%%f" (
        set "filename=%%~nxf"
        
        REM Count procedures with proper error handling
        set exit_handler_count=0
        set status_param_count=0
        set error_msg_count=0
        set total_proc_count=0
        
        for /f %%i in ('findstr /c:"DECLARE EXIT HANDLER FOR SQLEXCEPTION" "%%f" 2^>nul') do set /a exit_handler_count+=1
        for /f %%i in ('findstr /c:"OUT.*p_Status.*INT" "%%f" 2^>nul') do set /a status_param_count+=1
        for /f %%i in ('findstr /c:"OUT.*p_ErrorMsg" "%%f" 2^>nul') do set /a error_msg_count+=1
        for /f %%i in ('findstr /c:"CREATE PROCEDURE" "%%f" 2^>nul') do set /a total_proc_count+=1
        
        echo !filename! Error Handling Summary: >> "%REPORT_FILE%"
        echo   Total Procedures: !total_proc_count! >> "%REPORT_FILE%"
        echo   With EXIT HANDLER: !exit_handler_count! >> "%REPORT_FILE%"
        echo   With Status OUT param: !status_param_count! >> "%REPORT_FILE%"
        echo   With Error Message OUT param: !error_msg_count! >> "%REPORT_FILE%"
        
        if !total_proc_count! gtr 0 (
            if !exit_handler_count! equ !total_proc_count! if !status_param_count! equ !total_proc_count! if !error_msg_count! equ !total_proc_count! (
                echo   ? COMPLIANT: All procedures have proper error handling
                echo   STATUS: COMPLIANT >> "%REPORT_FILE%"
            ) else (
                echo   ? NON-COMPLIANT: Some procedures missing proper error handling
                echo   STATUS: NON-COMPLIANT >> "%REPORT_FILE%"
            )
        )
        echo. >> "%REPORT_FILE%"
    )
)

echo.
echo Phase 5: MySQL 5.7.24 Compatibility Check
echo ==========================================

echo MYSQL 5.7.24 COMPATIBILITY CHECK >> "%REPORT_FILE%"
echo ================================= >> "%REPORT_FILE%"

echo Checking for MySQL compatibility issues...
for %%f in ("%PROCEDURES_PATH%\*.sql") do (
    if exist "%%f" (
        set "filename=%%~nxf"
        set issues_found=0
        
        REM Check for CTE (Common Table Expressions) - MySQL 8.0+
        findstr /r "WITH.*AS" "%%f" >nul 2>&1
        if !errorlevel! equ 0 (
            echo   ? !filename!: Uses CTE ^(WITH clause^) - MySQL 8.0+ feature
            echo   ISSUE: !filename! uses CTE ^(WITH clause^) - MySQL 8.0+ feature >> "%REPORT_FILE%"
            set /a issues_found+=1
        )
        
        REM Check for window functions - MySQL 8.0+
        findstr /r "ROW_NUMBER.*RANK.*DENSE_RANK.*LAG.*LEAD" "%%f" >nul 2>&1
        if !errorlevel! equ 0 (
            echo   ? !filename!: Uses window functions - MySQL 8.0+ feature
            echo   ISSUE: !filename! uses window functions - MySQL 8.0+ feature >> "%REPORT_FILE%"
            set /a issues_found+=1
        )
        
        REM Check for JSON functions compatibility
        findstr /r "JSON_EXTRACT.*JSON_UNQUOTE.*JSON_SEARCH.*JSON_CONTAINS" "%%f" >nul 2>&1
        if !errorlevel! equ 0 (
            echo   ? !filename!: Uses JSON functions - verify 5.7.24 compatibility
            echo   WARNING: !filename! uses JSON functions - verify 5.7.24 compatibility >> "%REPORT_FILE%"
            set /a issues_found+=1
        )
        
        if !issues_found! equ 0 (
            echo   ? !filename!: MySQL 5.7.24 compatible
            echo   STATUS: !filename! - MySQL 5.7.24 compatible >> "%REPORT_FILE%"
        )
        echo. >> "%REPORT_FILE%"
    )
)

echo.
echo Phase 6: Generate Fix Recommendations
echo ====================================

echo FIX RECOMMENDATIONS >> "%REPORT_FILE%"
echo =================== >> "%REPORT_FILE%"
echo. >> "%REPORT_FILE%"

echo Based on the analysis, here are the recommended fixes: >> "%REPORT_FILE%"
echo. >> "%REPORT_FILE%"
echo 1. COLUMN NAME FIXES: >> "%REPORT_FILE%"
echo    - Update any references to 'FullName' to match actual schema >> "%REPORT_FILE%"
echo    - Ensure 'TransactionDate' references use 'ReceiveDate' from actual tables >> "%REPORT_FILE%"
echo    - Verify all column names match LiveDatabase.sql exactly >> "%REPORT_FILE%"
echo. >> "%REPORT_FILE%"

echo 2. ERROR HANDLING STANDARDIZATION: >> "%REPORT_FILE%"
echo    - All procedures should have 'DECLARE EXIT HANDLER FOR SQLEXCEPTION' >> "%REPORT_FILE%"
echo    - All procedures should have 'OUT p_Status INT' parameter >> "%REPORT_FILE%"
echo    - All procedures should have 'OUT p_ErrorMsg VARCHAR(255)' parameter >> "%REPORT_FILE%"
echo    - Follow the pattern used in inv_inventory_Remove_Item >> "%REPORT_FILE%"
echo. >> "%REPORT_FILE%"

echo 3. MYSQL 5.7.24 COMPATIBILITY: >> "%REPORT_FILE%"
echo    - Replace any MySQL 8.0+ specific features with 5.7.24 alternatives >> "%REPORT_FILE%"
echo    - Test all JSON operations for 5.7.24 compatibility >> "%REPORT_FILE%"
echo    - Avoid window functions, use traditional GROUP BY approaches >> "%REPORT_FILE%"
echo. >> "%REPORT_FILE%"

echo.
echo Analysis Complete!
echo ==================
echo.
echo Full report saved to: %REPORT_FILE%
echo.
echo Next Steps:
echo 1. Review the analysis report
echo 2. Apply recommended fixes to stored procedure files
echo 3. Run the verification system (00_StoredProcedure_Verification_System.sql)
echo 4. Test all procedures against LiveDatabase.sql
echo.

REM Display report summary
echo Report Summary:
echo ===============
echo Total Procedures Analyzed: %total_procedures%

REM Count SQL files
set file_count=0
for %%f in ("%PROCEDURES_PATH%\*.sql") do set /a file_count+=1

echo Schema Files Checked: %file_count%
echo Analysis Report Location: %REPORT_FILE%
echo.

echo Analysis complete. Review the report for detailed findings and recommendations.

REM Open the report file automatically
if exist "%REPORT_FILE%" (
    echo.
    echo Opening analysis report...
    start notepad "%REPORT_FILE%"
)

pause
