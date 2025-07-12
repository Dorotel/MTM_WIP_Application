# MTM WIP Application

A comprehensive Windows Forms application for managing inventory and work-in-progress tracking with MySQL database integration.

## Features

- **Inventory Management**: Track current inventory levels, transfers, and removals
- **User Role System**: Admin, Normal, and Read-Only user privileges
- **Transaction History**: Complete audit trail of all inventory operations
- **Analytics Dashboard**: Visual reporting and analytics
- **Multi-User Support**: Concurrent user access with role-based permissions
- **MySQL Integration**: Robust database connectivity with stored procedures

## User Roles

| Role | Read | Write | Inventory/Transactions | User/Settings/Admin | Search |
|------|------|-------|------------------------|-------------------|---------|
| **Admin** | ✓ | ✓ | ✓ | ✓ | ✓ |
| **Normal** | ✓ | Limited* | ✓ | ✗ | ✓ |
| **Read-Only** | ✓ | ✗ | ✗ | ✗ | ✓ |

*Normal users can only write to `inv_inventory` and `inv_transaction` tables.

## Prerequisites

- .NET 8.0 or higher
- Windows OS (Windows Forms application)
- MySQL Server 5.7 or higher
- Visual Studio 2022 or Visual Studio Code

## Database Setup

1. Create MySQL database named `mtm_wip_application`
2. Execute the schema from `DATABASE_SCHEMA.sql`
3. Configure connection string in application settings

## Getting Started

1. Clone the repository
2. Open `MTM_WIP_Application.sln` in Visual Studio
3. Configure database connection in `Helper_Database_Variables.cs`
4. Build and run the application

## Development

For comprehensive development guidelines, see:
- `REPO_COMPREHENSIVE_CHECKLIST.md` - Complete development standards
- `CONTRIBUTING.md` - Contribution guidelines
- `MYSQL_SERVER_CHANGES.md` - Database change log

## Project Structure

```
MTM_WIP_Application/
├── Controls/           # UI Controls and Forms
├── Core/              # Core business logic
├── Data/              # Data access layer (DAOs)
├── Forms/             # Windows Forms UI
├── Helpers/           # Utility classes
├── Models/            # Data models
├── Properties/        # Assembly properties
├── Resources/         # Application resources
├── Services/          # Business services
└── tests/            # Unit tests
```

## License

See `LICENSE.txt` for details.

## Maintainer

- **@Dorotel** - Project maintainer and final reviewer
- **@copilot** - Refactoring and code development agent