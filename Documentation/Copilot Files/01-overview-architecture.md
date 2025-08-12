# 1–3. Overview, Tech Stack, and Solution Architecture

1. Project Overview
MTM Inventory Application is a Windows desktop application (WinForms) for managing WIP (Work In Progress) inventory. It provides:
- Master Data Management: Parts, Operations, Locations, Item Types, Users
- Inventory Operations: Add, Remove, Transfer inventory items with full audit trail
- Transaction History: Complete tracking of all inventory movements
- User Management: Role-based access control (Admin, Normal, Read-Only)
- Advanced Theming: Comprehensive theme system with DPI scaling support
- Progress Reporting: Standardized progress bars with color-coded status feedback
- Database Integration: MySQL stored procedures exclusively for all data operations
- Quick Button System: Personalized quick access to frequently used inventory combinations
- Version Management: Automatic version checking with semantic versioning support

Primary Users and Goals
- Inventory/Production Staff: Add/remove/transfer inventory with clear visual feedback
- Supervisors/Admins: Manage master data and user roles
- All Users: Consistent UI experience with robust error handling

Application Architecture
- Windows Forms Desktop Application targeting .NET 8
- MySQL 5.7.24+ Database with MAMP support
- Stored Procedure Only database access pattern
- Centralized Progress/Status System using StatusStrip
- Advanced Theming Engine with DPI scaling
- Uniform parameter naming (auto p_ prefix)
- Enhanced DAO Pattern with DaoResult<T>

2. Tech Stack and Runtime

Core Technologies
- .NET SDK: .NET 8
- C# Language Version: C# 12
- UI Framework: Windows Forms
- Target OS: Windows 10/11
- Database: MySQL 5.7.24+ (MAMP-compatible)
- Data Access: ADO.NET with custom DAO pattern (stored procedures only)

Key Dependencies
- MySql.Data.MySqlClient
- System.Text.Json
- Microsoft.Web.WebView2
- ClosedXML

Development Environment
- Visual Studio 2022 (WinForms workload)
- MySQL Server or MAMP
- Example connection string:
```
Server=localhost;Port=3306;Database=mtm_wip_application;Uid=root;Pwd=root;Allow User Variables=True;
```

3. Solution Architecture and Layout

Directory Structure (authoritative)
```
MTM_Inventory_Application/
├─ Controls/                     # All UserControl implementations
│  ├─ Addons/                    # Specialized controls (Connection strength, etc.)
│  ├─ MainForm/                  # Main application tabs and controls
│  ├─ SettingsForm/              # Settings dialog controls
│  └─ Shared/                    # Reusable controls
├─ Core/                         # Core application services
│  ├─ Core_Themes.cs             # Advanced theming and DPI scaling
│  ├─ Core_WipAppVariables.cs    # Application-wide constants
│  └─ Core_DgvPrinter.cs         # DataGridView printing utilities
├─ Data/                         # Data access layer (DAOs)
├─ Database/                     # Database scripts and stored procedures
│  └─ StoredProcedures/          # 74+ procedures with uniform p_ parameter naming
├─ Documentation/                # Comprehensive patch history and guides
│  ├─ Patches/                   # Historical fix documentation (30+ patches)
│  └─ Guides/                    # Technical architecture and setup guides
├─ Forms/                        # Form definitions
├─ Helpers/                      # Utility classes and helpers
│  ├─ Helper_FileIO.cs           # File I/O operations
│  ├─ Helper_Json.cs             # JSON parsing/serialization
│  └─ Helper_UI_ComboBoxes.cs    # ComboBox management
├─ Logging/                      # Centralized logging system
├─ Models/                       # Data models and DTOs
├─ Services/                     # Background services and utilities
│  ├─ Service_Timer_VersionChecker.cs  # Version checking service
│  └─ Service_ErrorHandler.cs          # Error handling service
└─ Program.cs                    # Application entry point with comprehensive startup
```

Naming Conventions
- Controls: Control_[TabName]_[ControlType]_[Name]
- Methods: [ClassName]_[Action]_[Details]
- Fields: _camelCase (underscore prefix)
- Properties: PascalCase
- Constants: UPPER_CASE
- Events: [ControlName]_[EventType]