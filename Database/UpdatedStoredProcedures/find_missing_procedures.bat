@echo off
REM ================================================================================
REM MTM INVENTORY APPLICATION - MISSING PROCEDURES DETECTION & CONSOLIDATION TOOL
REM ================================================================================
REM File: find_missing_procedures.bat
REM Purpose: Search for procedures, functions, triggers missing from consolidated file
REM          and add them to mtm_wip_application_test_routines.sql
REM Created: August 15, 2025
REM Target: Compare all SQL files in UpdatedStoredProcedures\ against consolidated file
REM Source: mtm_wip_application_test_routines.sql
REM ================================================================================

setlocal enabledelayedexpansion

echo ==================================================================================
echo MTM MISSING PROCEDURES DETECTION AND CONSOLIDATION TOOL
echo ==================================================================================
echo Running Date: %DATE% %TIME%
echo Source: All SQL files in UpdatedStoredProcedures\ directory
echo Target: mtm_wip_application_test_routines.sql (Consolidated)
echo.

REM Set paths
set "STORED_PROC_DIR=%~dp0"
set "CONSOLIDATED_FILE=%~dp0..\UpdatedDatabase\mtm_wip_application_test_routines.sql"
set "TEMP_DIR=%TEMP%\MTM_Procedure_Analysis"
set "MISSING_REPORT=%TEMP_DIR%\missing_procedures_report.txt"
set "NEW_CONSOLIDATED=%TEMP_DIR%\mtm_wip_application_test_routines_updated.sql"

REM Create temp directory
if not exist "%TEMP_DIR%" mkdir "%TEMP_DIR%"

echo Phase 1: Environment Verification
echo =================================

REM Check if consolidated file exists
if not exist "%CONSOLIDATED_FILE%" (
    echo [ERROR] Consolidated file not found!
    echo [ERROR] Expected: %CONSOLIDATED_FILE%
    echo [INFO] Please ensure mtm_wip_application_test_routines.sql exists
    pause
    exit /b 1
)

echo [INFO] Found consolidated file: %CONSOLIDATED_FILE%

REM Check for SQL files in UpdatedStoredProcedures directory
set sql_file_count=0
echo [INFO] Scanning for SQL files in: %STORED_PROC_DIR%

for %%f in ("%STORED_PROC_DIR%*.sql") do (
    set /a sql_file_count+=1
    echo [INFO] Found SQL file: %%~nxf
)

if %sql_file_count%==0 (
    echo [WARNING] No SQL files found in UpdatedStoredProcedures directory
    echo [INFO] Expected files according to documentation:
    echo [INFO]   00_StoredProcedure_Verification_System.sql
    echo [INFO]   01_User_Management_Procedures.sql
    echo [INFO]   02_System_Role_Procedures.sql
    echo [INFO]   03_Master_Data_Procedures.sql
    echo [INFO]   04_Inventory_Procedures.sql
    echo [INFO]   05_Error_Log_Procedures.sql
    echo [INFO]   06_Quick_Button_Procedures.sql
    echo [INFO]   07_Changelog_Version_Procedures.sql
    echo [INFO]   08_Theme_Management_Procedures.sql
    echo [INFO]   99_Database_Testing_Suite.sql
    echo.
    echo [WARNING] Continuing analysis with consolidated file only...
) else (
    echo [INFO] Found %sql_file_count% SQL files for analysis
)
echo.

echo Phase 2: Consolidated File Analysis
echo ===================================

echo [INFO] Analyzing consolidated file procedures...

REM Extract procedure names from consolidated file
echo [INFO] Extracting procedure names from consolidated file...
set consolidated_proc_count=0

REM Create consolidated procedures list
findstr /R /C:"^CREATE.*PROCEDURE" "%CONSOLIDATED_FILE%" > "%TEMP_DIR%\consolidated_procedures.txt" 2>nul

REM Count procedures in consolidated file
for /f %%i in ('findstr /c:"CREATE DEFINER" "%CONSOLIDATED_FILE%" 2^>nul') do (
    set /a consolidated_proc_count+=%%i
)

echo [INFO] Found %consolidated_proc_count% procedures in consolidated file

REM Extract functions from consolidated file
findstr /R /C:"^CREATE.*FUNCTION" "%CONSOLIDATED_FILE%" > "%TEMP_DIR%\consolidated_functions.txt" 2>nul
set consolidated_func_count=0
for /f %%i in ('findstr /c:"CREATE.*FUNCTION" "%CONSOLIDATED_FILE%" 2^>nul') do (
    set /a consolidated_func_count+=%%i
)

REM Extract triggers from consolidated file
findstr /R /C:"^CREATE.*TRIGGER" "%CONSOLIDATED_FILE%" > "%TEMP_DIR%\consolidated_triggers.txt" 2>nul
set consolidated_trigger_count=0
for /f %%i in ('findstr /c:"CREATE.*TRIGGER" "%CONSOLIDATED_FILE%" 2^>nul') do (
    set /a consolidated_trigger_count+=%%i
)

echo [INFO] Found %consolidated_func_count% functions in consolidated file
echo [INFO] Found %consolidated_trigger_count% triggers in consolidated file
echo.

echo Phase 3: Individual SQL Files Analysis
echo =======================================

if %sql_file_count%==0 (
    echo [INFO] Skipping individual file analysis - no SQL files found
    goto :report_generation
)

REM Initialize counters
set total_missing_procedures=0
set total_missing_functions=0
set total_missing_triggers=0

REM Create missing items report header
echo MTM INVENTORY APPLICATION - MISSING PROCEDURES ANALYSIS REPORT > "%MISSING_REPORT%"
echo Generated: %DATE% %TIME% >> "%MISSING_REPORT%"
echo ========================================================================= >> "%MISSING_REPORT%"
echo. >> "%MISSING_REPORT%"

REM Analyze each SQL file
for %%f in ("%STORED_PROC_DIR%*.sql") do (
    echo [INFO] Analyzing file: %%~nxf
    
    REM Extract procedures from current file
    findstr /R /C:"^CREATE.*PROCEDURE" "%%f" > "%TEMP_DIR%\current_file_procedures.txt" 2>nul
    
    REM Extract functions from current file
    findstr /R /C:"^CREATE.*FUNCTION" "%%f" > "%TEMP_DIR%\current_file_functions.txt" 2>nul
    
    REM Extract triggers from current file
    findstr /R /C:"^CREATE.*TRIGGER" "%%f" > "%TEMP_DIR%\current_file_triggers.txt" 2>nul
    
    set file_missing_count=0
    
    REM Check for missing procedures
    if exist "%TEMP_DIR%\current_file_procedures.txt" (
        for /f "tokens=*" %%p in (%TEMP_DIR%\current_file_procedures.txt) do (
            set "proc_line=%%p"
            REM Extract procedure name (simplified approach)
            for /f "tokens=3 delims=` " %%n in ("!proc_line!") do (
                set "proc_name=%%n"
                REM Check if procedure exists in consolidated file
                findstr /C:"!proc_name!" "%CONSOLIDATED_FILE%" >nul 2>&1
                if errorlevel 1 (
                    echo [MISSING PROCEDURE] !proc_name! from %%~nxf >> "%MISSING_REPORT%"
                    set /a total_missing_procedures+=1
                    set /a file_missing_count+=1
                )
            )
        )
    )
    
    REM Check for missing functions
    if exist "%TEMP_DIR%\current_file_functions.txt" (
        for /f "tokens=*" %%p in (%TEMP_DIR%\current_file_functions.txt) do (
            set "func_line=%%p"
            for /f "tokens=3 delims=` " %%n in ("!func_line!") do (
                set "func_name=%%n"
                findstr /C:"!func_name!" "%CONSOLIDATED_FILE%" >nul 2>&1
                if errorlevel 1 (
                    echo [MISSING FUNCTION] !func_name! from %%~nxf >> "%MISSING_REPORT%"
                    set /a total_missing_functions+=1
                    set /a file_missing_count+=1
                )
            )
        )
    )
    
    REM Check for missing triggers
    if exist "%TEMP_DIR%\current_file_triggers.txt" (
        for /f "tokens=*" %%p in (%TEMP_DIR%\current_file_triggers.txt) do (
            set "trigger_line=%%p"
            for /f "tokens=3 delims=` " %%n in ("!trigger_line!") do (
                set "trigger_name=%%n"
                findstr /C:"!trigger_name!" "%CONSOLIDATED_FILE%" >nul 2>&1
                if errorlevel 1 (
                    echo [MISSING TRIGGER] !trigger_name! from %%~nxf >> "%MISSING_REPORT%"
                    set /a total_missing_triggers+=1
                    set /a file_missing_count+=1
                )
            )
        )
    )
    
    if !file_missing_count! GTR 0 (
        echo [FOUND] !file_missing_count! missing items in %%~nxf
    ) else (
        echo [COMPLETE] All items from %%~nxf found in consolidated file
    )
)

:report_generation
echo.
echo Phase 4: Missing Items Report Generation
echo ========================================

set total_missing_items=0
set /a total_missing_items=total_missing_procedures+total_missing_functions+total_missing_triggers

echo. >> "%MISSING_REPORT%"
echo SUMMARY: >> "%MISSING_REPORT%"
echo ======== >> "%MISSING_REPORT%"
echo Missing Procedures: %total_missing_procedures% >> "%MISSING_REPORT%"
echo Missing Functions: %total_missing_functions% >> "%MISSING_REPORT%"
echo Missing Triggers: %total_missing_triggers% >> "%MISSING_REPORT%"
echo Total Missing Items: %total_missing_items% >> "%MISSING_REPORT%"
echo. >> "%MISSING_REPORT%"

if %total_missing_items% GTR 0 (
    echo [FOUND] %total_missing_items% missing items need to be added to consolidated file
    echo [INFO] Missing items breakdown:
    echo [INFO]   - Procedures: %total_missing_procedures%
    echo [INFO]   - Functions: %total_missing_functions%
    echo [INFO]   - Triggers: %total_missing_triggers%
    echo.
    
    echo Phase 5: Consolidated File Update Process
    echo =========================================
    
    choice /M "Do you want to automatically add missing items to the consolidated file"
    if !errorlevel!==1 (
        call :update_consolidated_file
    ) else (
        echo [INFO] Skipped automatic update. See report for manual additions needed.
    )
) else (
    echo [SUCCESS] No missing items found! Consolidated file is complete.
    echo [INFO] All procedures, functions, and triggers from UpdatedStoredProcedures
    echo [INFO] directory are already present in mtm_wip_application_test_routines.sql
)

echo.
echo Phase 6: Analysis Report
echo ========================
echo [INFO] Analysis completed successfully
echo [INFO] Report saved to: %MISSING_REPORT%
echo [INFO] Consolidated file has: %consolidated_proc_count% procedures
echo [INFO] Consolidated file has: %consolidated_func_count% functions  
echo [INFO] Consolidated file has: %consolidated_trigger_count% triggers
echo [INFO] Missing items found: %total_missing_items%

if exist "%MISSING_REPORT%" (
    echo.
    echo [INFO] Opening missing items report...
    start notepad "%MISSING_REPORT%"
)

echo.
echo ==================================================================================
echo MISSING PROCEDURES ANALYSIS COMPLETE
echo ==================================================================================
echo Summary:
echo   - Consolidated file: %consolidated_proc_count% procedures, %consolidated_func_count% functions, %consolidated_trigger_count% triggers
echo   - SQL files analyzed: %sql_file_count%
echo   - Missing items: %total_missing_items%
echo   - Report: %MISSING_REPORT%
echo.
echo Next Steps:
if %total_missing_items% GTR 0 (
    echo   1. Review the missing items report
    echo   2. Manually add missing procedures to consolidated file, OR
    echo   3. Re-run this tool and choose automatic update option
    echo   4. Deploy updated consolidated file: deploy.bat
    echo   5. Verify deployment: run_verification.bat
) else (
    echo   1. Your consolidated file is complete!
    echo   2. Deploy with: deploy.bat
    echo   3. Verify with: run_verification.bat
)
echo.

pause
goto :cleanup

:update_consolidated_file
echo [INFO] Starting automatic consolidated file update...

REM Copy original consolidated file
copy "%CONSOLIDATED_FILE%" "%NEW_CONSOLIDATED%" >nul
if errorlevel 1 (
    echo [ERROR] Failed to create backup of consolidated file
    goto :cleanup
)

echo [INFO] Created working copy: %NEW_CONSOLIDATED%

REM Add missing items to the consolidated file
set items_added=0

for %%f in ("%STORED_PROC_DIR%*.sql") do (
    echo [INFO] Processing missing items from: %%~nxf
    
    REM Extract all CREATE statements that aren't in consolidated file
    for /f "tokens=*" %%p in ('findstr /R /C:"^CREATE.*PROCEDURE\|^CREATE.*FUNCTION\|^CREATE.*TRIGGER" "%%f" 2^>nul') do (
        set "create_line=%%p"
        REM Extract routine name (simplified)
        for /f "tokens=3 delims=` " %%n in ("!create_line!") do (
            set "routine_name=%%n"
            REM Check if routine exists in consolidated file
            findstr /C:"!routine_name!" "%CONSOLIDATED_FILE%" >nul 2>&1
            if errorlevel 1 (
                echo [ADDING] !routine_name! to consolidated file
                
                REM Add separator comment
                echo. >> "%NEW_CONSOLIDATED%"
                echo -- ============================================================================ >> "%NEW_CONSOLIDATED%"
                echo -- MISSING PROCEDURE ADDED FROM: %%~nxf >> "%NEW_CONSOLIDATED%"
                echo -- ADDED BY: find_missing_procedures.bat on %DATE% %TIME% >> "%NEW_CONSOLIDATED%"
                echo -- ============================================================================ >> "%NEW_CONSOLIDATED%"
                
                REM Extract the complete procedure/function/trigger definition
                call :extract_routine_definition "%%f" "!routine_name!" "%NEW_CONSOLIDATED%"
                set /a items_added+=1
            )
        )
    )
)

if !items_added! GTR 0 (
    echo [SUCCESS] Added !items_added! missing items to consolidated file
    echo [INFO] Updated file: %NEW_CONSOLIDATED%
    echo.
    
    choice /M "Replace original consolidated file with updated version"
    if !errorlevel!==1 (
        REM Create backup of original
        copy "%CONSOLIDATED_FILE%" "%CONSOLIDATED_FILE%.backup_%date:~10,4%%date:~4,2%%date:~7,2%_%time:~0,2%%time:~3,2%" >nul
        
        REM Replace original with updated version
        copy "%NEW_CONSOLIDATED%" "%CONSOLIDATED_FILE%" >nul
        if errorlevel 1 (
            echo [ERROR] Failed to replace consolidated file
        ) else (
            echo [SUCCESS] Consolidated file updated successfully!
            echo [INFO] Backup created: %CONSOLIDATED_FILE%.backup_*
            echo [INFO] Updated consolidated file now contains !items_added! additional items
        )
    ) else (
        echo [INFO] Updated file available at: %NEW_CONSOLIDATED%
        echo [INFO] Manually copy to replace consolidated file if desired
    )
) else (
    echo [INFO] No items were added - all procedures already exist in consolidated file
)

goto :eof

:extract_routine_definition
REM Extract complete procedure/function/trigger definition from source file
REM Parameters: %1=source_file, %2=routine_name, %3=target_file
set "source_file=%~1"
set "routine_name=%~2"
set "target_file=%~3"

REM Simple approach: find CREATE line and copy until END$$ or similar
set in_routine=0
for /f "tokens=*" %%l in ('type "%source_file%"') do (
    set "line=%%l"
    
    REM Check if we're starting the routine
    echo !line! | findstr /C:"%routine_name%" >nul 2>&1
    if !errorlevel!==0 (
        set in_routine=1
    )
    
    REM If we're in the routine, copy the line
    if !in_routine!==1 (
        echo !line! >> "%target_file%"
        
        REM Check if we've reached the end
        echo !line! | findstr /C:"END$$" >nul 2>&1
        if !errorlevel!==0 (
            set in_routine=0
        )
    )
)

goto :eof

:cleanup
REM Clean up temporary files
if exist "%TEMP_DIR%\consolidated_procedures.txt" del "%TEMP_DIR%\consolidated_procedures.txt"
if exist "%TEMP_DIR%\consolidated_functions.txt" del "%TEMP_DIR%\consolidated_functions.txt"
if exist "%TEMP_DIR%\consolidated_triggers.txt" del "%TEMP_DIR%\consolidated_triggers.txt"
if exist "%TEMP_DIR%\current_file_procedures.txt" del "%TEMP_DIR%\current_file_procedures.txt"
if exist "%TEMP_DIR%\current_file_functions.txt" del "%TEMP_DIR%\current_file_functions.txt"
if exist "%TEMP_DIR%\current_file_triggers.txt" del "%TEMP_DIR%\current_file_triggers.txt"

exit /b 0
