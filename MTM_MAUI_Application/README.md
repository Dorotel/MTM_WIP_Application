# MTM WIP Application - MAUI Version

This is the .NET MAUI cross-platform version of the MTM WIP (Work In Progress) inventory management application. 

## Overview

The MTM WIP Application has been migrated from WinForms to .NET MAUI to provide cross-platform support across Windows, macOS, iOS, and Android while maintaining full compatibility with the existing MySQL database and stored procedures.

## Features

- **Cross-Platform**: Runs on Windows, macOS, iOS, and Android
- **Modern Architecture**: MVVM pattern with dependency injection
- **Database Compatibility**: Uses existing MySQL database and 282+ stored procedures
- **Inventory Management**: Add, transfer, remove, and track inventory items
- **Transaction History**: Complete audit trail of all operations
- **Real-time Validation**: Input validation and error handling
- **Dynamic Theming**: Modern UI with theme support

## Technology Stack

- **.NET 8 MAUI** - Cross-platform UI framework
- **MySQL** - Database server (preserves existing schema)
- **CommunityToolkit.Mvvm** - MVVM pattern implementation
- **Dependency Injection** - Service management
- **XAML** - Declarative UI

## Prerequisites

To build and run this application, you need:

### Required
- **Visual Studio 2022** (17.8 or later) with the following workloads:
  - .NET Multi-platform App UI development
  - .NET desktop development
- **.NET 8 SDK** (8.0.100 or later)
- **MySQL Server** (5.7 or later) with the MTM WIP database

### Platform-Specific Requirements

#### Windows
- Windows 10 version 1809 (build 17763) or later
- Windows App SDK

#### macOS
- macOS 11.0 or later
- Xcode 13.3 or later

#### iOS
- iOS 11.0 or later
- Xcode 13.3 or later

#### Android
- Android 5.0 (API level 21) or later
- Android SDK

## Quick Start

### 1. Clone the Repository
```bash
git clone https://github.com/Dorotel/MTM_Inventory_Applicaiton_MAUI.git
cd MTM_Inventory_Applicaiton_MAUI
```

### 2. Install MAUI Workloads
```bash
dotnet workload restore
```

### 3. Configure Database Connection
Edit `MTM_MAUI_Application/appsettings.json`:
```json
{
  "Database": {
    "Name": "mtm_wip_application",
    "UserId": "your_username",
    "Password": "your_password",
    "Port": "3306"
  }
}
```

### 4. Build and Run

#### Windows
```bash
dotnet build -f net8.0-windows10.0.19041.0
dotnet run --project MTM_MAUI_Application --framework net8.0-windows10.0.19041.0
```

#### macOS
```bash
dotnet build -f net8.0-maccatalyst
dotnet run --project MTM_MAUI_Application --framework net8.0-maccatalyst
```

#### Android (requires Android device/emulator)
```bash
dotnet build -f net8.0-android
dotnet run --project MTM_MAUI_Application --framework net8.0-android
```

#### iOS (requires iOS device/simulator on macOS)
```bash
dotnet build -f net8.0-ios
dotnet run --project MTM_MAUI_Application --framework net8.0-ios
```

## Project Structure

```
MTM_MAUI_Application/
├── Models/              # Data models
├── Services/            # Business logic and data access
├── ViewModels/          # MVVM view models
├── Views/               # XAML pages and views
├── Platforms/           # Platform-specific implementations
├── Resources/           # App resources (images, fonts, styles)
└── appsettings.json     # Configuration
```

## Configuration

### Database Settings
The application uses the same MySQL database as the original WinForms version. Configure the connection in `appsettings.json`:

- **Name**: Database name (typically `mtm_wip_application`)
- **UserId**: MySQL username
- **Password**: MySQL password  
- **Port**: MySQL port (default 3306)

### Server Address Detection
The application automatically detects the appropriate server address:
- **Production**: Always uses `172.16.1.104`
- **Development**: Uses `172.16.1.104` if current IP matches, otherwise `localhost`

## Available Features

### ✅ Complete
- **Inventory Entry**: Add new inventory items with validation
- **Core Services**: Database, user management, error handling
- **MVVM Architecture**: Fully implemented with dependency injection

### 🚧 In Development
- **Transfer Operations**: Move items between locations
- **Remove Operations**: Remove items from inventory
- **Transaction History**: Search and filter transaction history
- **Settings Management**: User preferences and configuration

## Database Compatibility

All existing stored procedures and database schema remain unchanged:
- `inv_inventory_Add_Item`
- `inv_inventory_Get_ByPartId`
- `usr_users_Get_ByUser`
- Plus 279+ other procedures

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test on relevant platforms
5. Submit a pull request

## Support

For technical support or questions:
- Create an issue in this repository
- Contact the development team

## License

© Manitowoc Tool and Manufacturing. All rights reserved.

---

**Note**: This MAUI application maintains 100% compatibility with the existing MySQL database and can run alongside the original WinForms application during the transition period.