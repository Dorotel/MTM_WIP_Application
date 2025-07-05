# MTM WIP Application - Quick Installation Guide

## Prerequisites

1. **Windows 10/11** or **Windows Server 2016+**
2. **.NET 9.0 Runtime** - Download from [Microsoft .NET](https://dotnet.microsoft.com/download/dotnet/9.0)
3. **MySQL Server 8.0+** - For database backend
4. **Administrative privileges** for installation

## Installation Steps

### 1. Download & Extract
- Download the latest release ZIP file
- Extract to `C:\MTM_WIP_Application\`
- Ensure folder has read/write permissions

### 2. Database Setup
```sql
-- Create database and user
CREATE DATABASE mtm_inventory CHARACTER SET utf8mb4;
CREATE USER 'mtm_user'@'%' IDENTIFIED BY 'your_secure_password';
GRANT ALL PRIVILEGES ON mtm_inventory.* TO 'mtm_user'@'%';
FLUSH PRIVILEGES;
```

### 3. First Launch
1. Double-click `MTM_Inventory_Application.exe`
2. Configure database settings:
   - Server: `localhost` (or your MySQL server IP)
   - Port: `3306`
   - Database: `mtm_inventory`
   - Username: `mtm_user`
   - Password: `your_secure_password`
3. Click "Test Connection" to verify
4. Click "Save Changes"

### 4. Initial Setup
- The application will create necessary database tables automatically
- Default admin account will be created
- You can now start using the application

## Quick Start
1. Login with your credentials
2. Use the "New Transaction" tab to add items
3. Access settings via File → Settings
4. Export data using the Quick Actions panel

## Support
- Check the full [README.md](README.md) for detailed documentation
- Report issues on [GitHub Issues](https://github.com/Dorotel/MTM_WIP_Application/issues)