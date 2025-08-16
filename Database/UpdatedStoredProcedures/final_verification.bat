@echo off
echo ==================================================================================
echo MTM FINAL VERIFICATION RUNNER - CONSOLIDATED ROUTINES
echo ==================================================================================

REM Use the same MySQL path detection logic
set MYSQL_PATH=C:\MAMP\bin\mysql\bin
if not exist "%MYSQL_PATH%\mysql.exe" set MYSQL_PATH=
if "%MYSQL_PATH%"=="" (
    set MYSQL_CMD=mysql
) else (
    set MYSQL_CMD="%MYSQL_PATH%\mysql.exe"
)

set DB_HOST=localhost
set DB_PORT=3306
set DB_NAME=mtm_wip_application_test
set DB_USER=root
set DB_PASSWORD=root

echo [INFO] Verifying deployment of consolidated mtm_wip_application_test_routines.sql
echo [INFO] Target database: %DB_NAME%
echo ==============================

REM Count deployed procedures first
echo [INFO] Counting deployed procedures...
echo -- Count Deployed Procedures > final_verify.sql
echo SELECT COUNT(*) as DeployedProcedures >> final_verify.sql
echo FROM INFORMATION_SCHEMA.ROUTINES >> final_verify.sql  
echo WHERE ROUTINE_SCHEMA = '%DB_NAME%' >> final_verify.sql
echo AND ROUTINE_TYPE = 'PROCEDURE'; >> final_verify.sql
echo. >> final_verify.sql

REM Check for key procedure categories from consolidated file
echo -- Verify Key Procedure Categories >> final_verify.sql
echo SELECT >> final_verify.sql
echo   SUM(CASE WHEN ROUTINE_NAME LIKE 'app_themes_%%' THEN 1 ELSE 0 END) as ThemeProcs, >> final_verify.sql
echo   SUM(CASE WHEN ROUTINE_NAME LIKE 'inv_inventory_%%' THEN 1 ELSE 0 END) as InventoryProcs, >> final_verify.sql
echo   SUM(CASE WHEN ROUTINE_NAME LIKE 'usr_users_%%' THEN 1 ELSE 0 END) as UserProcs, >> final_verify.sql
echo   SUM(CASE WHEN ROUTINE_NAME LIKE 'md_%%' THEN 1 ELSE 0 END) as MasterDataProcs, >> final_verify.sql
echo   SUM(CASE WHEN ROUTINE_NAME LIKE 'log_%%' THEN 1 ELSE 0 END) as LogProcs, >> final_verify.sql
echo   SUM(CASE WHEN ROUTINE_NAME LIKE 'sys_%%' THEN 1 ELSE 0 END) as SystemProcs >> final_verify.sql
echo FROM INFORMATION_SCHEMA.ROUTINES >> final_verify.sql
echo WHERE ROUTINE_SCHEMA = '%DB_NAME%' >> final_verify.sql
echo AND ROUTINE_TYPE = 'PROCEDURE'; >> final_verify.sql
echo. >> final_verify.sql

REM Test a few key procedures from consolidated file
echo -- Test Key Procedures >> final_verify.sql
echo SELECT 'Testing app_themes_Get_All procedure...' as TestStatus; >> final_verify.sql
echo CALL app_themes_Get_All(@status1, @msg1); >> final_verify.sql
echo SELECT @status1 as ThemeTestStatus, @msg1 as ThemeTestMessage; >> final_verify.sql
echo. >> final_verify.sql

echo SELECT 'Testing usr_users_Get_All procedure...' as TestStatus; >> final_verify.sql
echo CALL usr_users_Get_All(@status2, @msg2); >> final_verify.sql
echo SELECT @status2 as UserTestStatus, @msg2 as UserTestMessage; >> final_verify.sql
echo. >> final_verify.sql

echo SELECT 'Testing inv_inventory_Get_All procedure...' as TestStatus; >> final_verify.sql
echo CALL inv_inventory_Get_All(@status3, @msg3); >> final_verify.sql
echo SELECT @status3 as InventoryTestStatus, @msg3 as InventoryTestMessage; >> final_verify.sql
echo. >> final_verify.sql

REM Final consolidated verification result
echo -- Final Verification Result >> final_verify.sql
echo SELECT >> final_verify.sql
echo   CASE >> final_verify.sql
echo     WHEN @status1 = 0 AND @status2 = 0 AND @status3 = 0 THEN 0 >> final_verify.sql
echo     ELSE 1 >> final_verify.sql
echo   END as OverallStatus, >> final_verify.sql
echo   CASE >> final_verify.sql
echo     WHEN @status1 = 0 AND @status2 = 0 AND @status3 = 0 THEN '✅ CONSOLIDATED ROUTINES VERIFIED AND WORKING!' >> final_verify.sql
echo     ELSE '⚠ Some consolidated routines may have issues - check individual test results' >> final_verify.sql
echo   END as FinalResult; >> final_verify.sql

echo Running final verification of consolidated procedures...
%MYSQL_CMD% -h%DB_HOST% -P%DB_PORT% -u%DB_USER% -p%DB_PASSWORD% %DB_NAME% < final_verify.sql

del final_verify.sql

echo.
echo ==================================================================================
echo CONSOLIDATED ROUTINES VERIFICATION COMPLETE!
echo ==================================================================================
echo [INFO] Source: mtm_wip_application_test_routines.sql  
echo [INFO] Verified procedure categories from consolidated file
echo [INFO] Tested key procedures for functionality
echo ==================================================================================
pause
