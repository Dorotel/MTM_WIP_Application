@echo off
echo ==================================================================================
echo MTM FINAL VERIFICATION RUNNER
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
set DB_NAME=mtm_wip_application
set DB_USER=root
set DB_PASSWORD=root

echo Running Final Verification...
echo ==============================

echo -- Final Complete Verification > final_verify.sql
echo CALL sys_RunCompleteVerification(@overall, @summary); >> final_verify.sql
echo SELECT >> final_verify.sql  
echo   @overall as OverallStatus, >> final_verify.sql
echo   CASE  >> final_verify.sql
echo     WHEN @overall = 0 THEN '? ALL SYSTEMS VERIFIED AND WORKING!' >> final_verify.sql
echo     ELSE '? Some issues detected - see detailed report' >> final_verify.sql
echo   END as FinalResult; >> final_verify.sql
echo SELECT @summary as DetailedReport; >> final_verify.sql

%MYSQL_CMD% -h%DB_HOST% -P%DB_PORT% -u%DB_USER% -p%DB_PASSWORD% %DB_NAME% < final_verify.sql

del final_verify.sql

echo.
echo ==================================================================================
echo VERIFICATION COMPLETE!
echo ==================================================================================
pause
