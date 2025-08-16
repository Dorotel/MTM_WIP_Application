@echo off
REM MTM MAUI Application Build Script for Windows
REM This script helps set up and build the MTM MAUI application

echo MTM WIP Application - MAUI Build Script
echo ======================================

REM Check if .NET is installed
where dotnet >nul 2>nul
if %ERRORLEVEL% neq 0 (
    echo ❌ .NET is not installed. Please install .NET 8 SDK first.
    echo    Download from: https://dotnet.microsoft.com/download/dotnet/8.0
    pause
    exit /b 1
)

REM Check .NET version
for /f "tokens=*" %%i in ('dotnet --version') do set DOTNET_VERSION=%%i
echo ✅ .NET Version: %DOTNET_VERSION%

REM Check if MAUI workloads are installed
echo.
echo 🔍 Checking MAUI workloads...
dotnet workload list | findstr "maui" >nul
if %ERRORLEVEL% neq 0 (
    echo ⚠️  MAUI workloads not found. Installing...
    dotnet workload restore
    if %ERRORLEVEL% equ 0 (
        echo ✅ MAUI workloads installed successfully
    ) else (
        echo ❌ Failed to install MAUI workloads
        pause
        exit /b 1
    )
) else (
    echo ✅ MAUI workloads are installed
)

REM Restore packages
echo.
echo 📦 Restoring packages...
dotnet restore
if %ERRORLEVEL% equ 0 (
    echo ✅ Packages restored successfully
) else (
    echo ❌ Failed to restore packages
    pause
    exit /b 1
)

REM Build for Windows
echo.
echo 🔨 Building for Windows...
dotnet build -f net8.0-windows10.0.19041.0 --configuration Debug
if %ERRORLEVEL% equ 0 (
    echo ✅ Windows build successful
) else (
    echo ❌ Windows build failed
    pause
    exit /b 1
)

echo.
echo 🎉 Build process completed!
echo.
echo Next steps:
echo 1. Configure your database connection in appsettings.json
echo 2. Run the application: dotnet run --framework net8.0-windows10.0.19041.0
echo 3. For mobile platforms, use Visual Studio or deploy to connected devices
echo.
echo For more information, see the README.md file.
echo.
pause