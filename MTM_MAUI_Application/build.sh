#!/bin/bash

# MTM MAUI Application Build Script
# This script helps set up and build the MTM MAUI application

echo "MTM WIP Application - MAUI Build Script"
echo "======================================"

# Check if .NET is installed
if ! command -v dotnet &> /dev/null; then
    echo "❌ .NET is not installed. Please install .NET 8 SDK first."
    echo "   Download from: https://dotnet.microsoft.com/download/dotnet/8.0"
    exit 1
fi

# Check .NET version
DOTNET_VERSION=$(dotnet --version)
echo "✅ .NET Version: $DOTNET_VERSION"

# Check if MAUI workloads are installed
echo ""
echo "🔍 Checking MAUI workloads..."
if ! dotnet workload list | grep -q "maui"; then
    echo "⚠️  MAUI workloads not found. Installing..."
    dotnet workload restore
    if [ $? -eq 0 ]; then
        echo "✅ MAUI workloads installed successfully"
    else
        echo "❌ Failed to install MAUI workloads"
        exit 1
    fi
else
    echo "✅ MAUI workloads are installed"
fi

# Restore packages
echo ""
echo "📦 Restoring packages..."
dotnet restore
if [ $? -eq 0 ]; then
    echo "✅ Packages restored successfully"
else
    echo "❌ Failed to restore packages"
    exit 1
fi

# Build for Windows (if on Windows or cross-platform build)
echo ""
echo "🔨 Building for Windows..."
if dotnet build -f net8.0-windows10.0.19041.0 --configuration Debug; then
    echo "✅ Windows build successful"
else
    echo "⚠️  Windows build failed (this may be expected on non-Windows systems)"
fi

# Build for macOS (if on macOS)
if [[ "$OSTYPE" == "darwin"* ]]; then
    echo ""
    echo "🔨 Building for macOS..."
    if dotnet build -f net8.0-maccatalyst --configuration Debug; then
        echo "✅ macOS build successful"
    else
        echo "⚠️  macOS build failed"
    fi
fi

echo ""
echo "🎉 Build process completed!"
echo ""
echo "Next steps:"
echo "1. Configure your database connection in appsettings.json"
echo "2. Run the application:"
echo "   - Windows: dotnet run --framework net8.0-windows10.0.19041.0"
if [[ "$OSTYPE" == "darwin"* ]]; then
    echo "   - macOS: dotnet run --framework net8.0-maccatalyst"
fi
echo "3. For mobile platforms, use Visual Studio or deploy to connected devices"
echo ""
echo "For more information, see the README.md file."