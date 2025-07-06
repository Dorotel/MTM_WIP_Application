# MTM WIP Application

**Professional Inventory Management System for Manufacturing Environments**

[![Version](https://img.shields.io/badge/version-4.6.0.0-blue.svg)](https://github.com/Dorotel/MTM_WIP_Application)
[![.NET](https://img.shields.io/badge/.NET-9.0-purple.svg)](https://dotnet.microsoft.com/)
[![Platform](https://img.shields.io/badge/platform-Windows-lightgrey.svg)](https://www.microsoft.com/windows)
[![License](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE.txt)

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [System Requirements](#system-requirements)
- [Installation & Setup](#installation--setup)
- [Configuration Guide](#configuration-guide)
- [User Guide](#user-guide)
- [Database Setup](#database-setup)
- [Troubleshooting](#troubleshooting)
- [Best Practices](#best-practices)
- [Updating & Maintenance](#updating--maintenance)
- [Support](#support)

## Overview

The MTM WIP (Work In Progress) Application is a comprehensive inventory management system designed specifically for manufacturing environments. Built with C#/.NET 9.0 and Windows Forms, it provides real-time inventory tracking, advanced reporting capabilities, and seamless integration with existing manufacturing workflows.

### Key Screenshots

#### Main Application Interface
![Main Interface](docs/screenshots/main-interface.html)
*The main application window showing inventory management tabs and quick action buttons*

#### User Login Screen
![Login Screen](docs/screenshots/login-screen.html)
*Secure authentication with database connection status*

#### Settings Configuration
![Settings Screen](docs/screenshots/settings-screen.html)
*Comprehensive configuration options for database and application settings*

#### Advanced Inventory Operations
![Advanced Inventory](docs/screenshots/advanced-inventory.html)
*Advanced inventory management with bulk operations and filtering capabilities*

## Features

### 🔧 **Core Inventory Management**
- **Add Items**: Quick addition of new items to inventory with work order tracking
- **Remove Items**: Efficient removal with quantity validation and audit trails
- **Transfer Items**: Location-to-location transfers with full traceability
- **Advanced Operations**: Bulk inventory operations for high-volume processing

### 📊 **Reporting & Analytics**
- Real-time inventory status monitoring
- Historical transaction reporting
- Excel integration for data export
- Custom report generation
- Analytics dashboard for inventory trends

### 🔗 **Database Integration**
- MySQL backend with optimized stored procedures
- Real-time connection monitoring
- Automatic reconnection handling
- Data integrity validation
- Backup and recovery support

### 🎨 **User Experience**
- Customizable themes and UI preferences
- Intuitive tabbed interface
- Quick action buttons for common tasks
- Keyboard shortcuts for power users
- Tooltips and help system

### 🔒 **Security & Authentication**
- User authentication system
- Role-based access control
- Audit logging for all transactions
- Secure database connections
- Data encryption support

## System Requirements

### Minimum Requirements
- **Operating System**: Windows 10 (64-bit) or Windows Server 2016
- **Framework**: .NET 9.0 Runtime
- **Memory**: 4 GB RAM
- **Storage**: 500 MB available disk space
- **Network**: TCP/IP connection for database access

### Recommended Requirements
- **Operating System**: Windows 11 or Windows Server 2022
- **Framework**: .NET 9.0 Runtime (latest version)
- **Memory**: 8 GB RAM or higher
- **Storage**: 2 GB available disk space (for logs and temp files)
- **Network**: Gigabit Ethernet for optimal database performance

### Database Requirements
- **MySQL**: Version 8.0 or higher
- **Memory**: 2 GB RAM dedicated to MySQL
- **Storage**: Minimum 10 GB for database files
- **Network**: Accessible via TCP/IP (default port 3306)

## Installation & Setup

### Step 1: Download and Extract
1. Download the latest release from the [Releases page](https://github.com/Dorotel/MTM_WIP_Application/releases)
2. Extract the ZIP file to your desired installation directory (e.g., `C:\MTM_WIP_Application`)
3. Ensure the user has read/write permissions to the installation directory

### Step 2: Install Prerequisites
1. **Install .NET 9.0 Runtime**:
   - Download from [Microsoft .NET Downloads](https://dotnet.microsoft.com/download/dotnet/9.0)
   - Run the installer as Administrator
   - Verify installation: Open Command Prompt and run `dotnet --version`

2. **Install Visual C++ Redistributable** (if not already installed):
   - Download from [Microsoft Visual C++ Downloads](https://docs.microsoft.com/en-us/cpp/windows/latest-supported-vc-redist)
   - Install the x64 version

### Step 3: Database Preparation
1. **MySQL Server Setup**:
   ```sql
   -- Create database
   CREATE DATABASE mtm_inventory CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
   
   -- Create user
   CREATE USER 'mtm_user'@'%' IDENTIFIED BY 'secure_password_here';
   GRANT ALL PRIVILEGES ON mtm_inventory.* TO 'mtm_user'@'%';
   FLUSH PRIVILEGES;
   ```

2. **Import Database Schema**:
   - Locate the `database_schema.sql` file in the installation directory
   - Import using MySQL Workbench or command line:
   ```bash
   mysql -u mtm_user -p mtm_inventory < database_schema.sql
   ```

### Step 4: First Launch
1. Navigate to the installation directory
2. Double-click `MTM_Inventory_Application.exe`
3. Configure database connection settings (see Configuration Guide below)
4. Test the connection and complete initial setup

## Configuration Guide

### Database Configuration

Upon first launch, you'll need to configure the database connection:

1. **Open Settings**: Click `File` → `Settings` or press `Ctrl+,`
2. **Database Tab**: Navigate to the Database configuration section
3. **Enter Connection Details**:
   - **Server Address**: IP address or hostname of your MySQL server
   - **Port**: MySQL port (default: 3306)
   - **Database Name**: `mtm_inventory` (or your chosen database name)
   - **Username**: Database username (`mtm_user`)
   - **Password**: Database password

4. **Test Connection**: Click "Test Connection" to verify settings
5. **Save Configuration**: Click "Save Changes" to store the configuration

### Application Settings

#### Theme Configuration
- Navigate to `Settings` → `Theme`
- Choose from available themes:
  - **Default**: Standard Windows appearance
  - **Dark Mode**: Dark theme for reduced eye strain
  - **High Contrast**: Enhanced visibility for accessibility

#### Export Settings
- Configure default export locations
- Set Excel template preferences
- Customize report formats

#### Printing Configuration
- Set default printer
- Configure page layouts
- Set margins and formatting options

## User Guide

### Getting Started

#### Logging In
1. Launch the application
2. Enter your username and password
3. Verify database connection status (green indicator)
4. Click "Sign In" to access the main application

#### Main Interface Overview

The main application window consists of:
- **Menu Bar**: Access to File, Edit, and View options
- **Tab Control**: Switch between Inventory, Remove, and Transfer operations
- **Quick Actions Panel**: Common tasks and shortcuts
- **Status Bar**: Connection status and application state

### Core Operations

#### Adding Items to Inventory

1. **Select the "New Transaction" Tab**
2. **Enter Work Order Information**:
   - Work Order Number: Enter the manufacturing work order
   - Location: Select the storage or production location
   - Part Number: Specify the part or component identifier
   - Quantity: Enter the number of items

3. **Submit Transaction**:
   - Click "Add to Inventory"
   - Verify the transaction appears in the recent transactions list
   - Print or export transaction record if needed

#### Removing Items from Inventory

1. **Switch to "Remove" Tab**
2. **Search for Items**:
   - Enter part number or work order
   - Select items from the search results
   - Verify quantities available

3. **Process Removal**:
   - Specify removal quantity
   - Add removal reason/notes
   - Confirm the removal operation

#### Transferring Items Between Locations

1. **Navigate to "Transfer" Tab**
2. **Select Source Items**:
   - Choose items to transfer
   - Verify current location and quantities

3. **Specify Destination**:
   - Select target location
   - Enter transfer quantities
   - Add transfer notes if required

4. **Execute Transfer**:
   - Review transfer details
   - Confirm the transfer operation
   - Print transfer documentation

### Advanced Features

#### Bulk Operations
- Use the Advanced tabs for high-volume operations
- Import data from Excel templates
- Process multiple transactions simultaneously

#### Reporting and Analytics
- Access reports via `View` → `Analytics`
- Generate custom reports based on date ranges
- Export data to Excel for further analysis

#### Quick Actions
Use the Quick Actions panel for:
- Viewing last 10 transactions
- Accessing frequently used locations
- Quick printing and export functions

## Database Setup

### Database Schema

The application uses a MySQL database with the following key tables:

- **inventory_items**: Main inventory records
- **transactions**: All inventory movements
- **locations**: Storage and production locations
- **work_orders**: Manufacturing work order details
- **users**: Application user accounts
- **audit_log**: Comprehensive audit trail

### Stored Procedures

The application utilizes optimized stored procedures for:
- Inventory additions and removals
- Location transfers
- Report generation
- Data validation

### Backup and Recovery

#### Regular Backups
Set up automated MySQL backups:
```bash
# Daily backup script
mysqldump -u mtm_user -p mtm_inventory > backup_$(date +%Y%m%d).sql
```

#### Recovery Procedures
1. Stop the MTM application
2. Restore from backup:
   ```bash
   mysql -u mtm_user -p mtm_inventory < backup_file.sql
   ```
3. Restart the application and verify data integrity

## Troubleshooting

### Common Issues

#### Database Connection Problems

**Symptoms**: "Disconnected from Server" message, unable to load data

**Solutions**:
1. **Check Network Connectivity**:
   - Ping the database server: `ping [server_ip]`
   - Verify firewall settings allow MySQL traffic (port 3306)

2. **Verify MySQL Service**:
   - Ensure MySQL service is running on the server
   - Check MySQL error logs for issues

3. **Test Credentials**:
   - Use MySQL Workbench to test connection with same credentials
   - Verify user permissions: `SHOW GRANTS FOR 'mtm_user'@'%';`

4. **Application Settings**:
   - Review database configuration in Settings
   - Test connection using the built-in test function

#### Application Performance Issues

**Symptoms**: Slow loading, delayed responses, freezing

**Solutions**:
1. **Check System Resources**:
   - Monitor CPU and memory usage
   - Close unnecessary applications
   - Ensure adequate disk space

2. **Database Optimization**:
   - Review MySQL performance settings
   - Check for large transaction logs
   - Consider database indexing optimization

3. **Network Issues**:
   - Test network latency to database server
   - Consider local database instance for better performance

#### Data Synchronization Issues

**Symptoms**: Inconsistent data, missing transactions

**Solutions**:
1. **Verify Database Integrity**:
   ```sql
   CHECK TABLE inventory_items;
   CHECK TABLE transactions;
   ```

2. **Review Audit Logs**:
   - Check application logs in the installation directory
   - Review database audit logs for errors

3. **Data Reconciliation**:
   - Run built-in data validation reports
   - Compare with physical inventory counts

### Error Messages

#### "Failed to connect to database"
- Check database server status
- Verify network connectivity
- Confirm connection credentials

#### "Access denied for user"
- Verify username and password
- Check MySQL user permissions
- Confirm database grants

#### "Table doesn't exist"
- Verify database schema installation
- Check database name in configuration
- Re-import database schema if necessary

### Log Files

Application logs are stored in:
- **Installation Directory**: `Logs\application.log`
- **User Directory**: `%APPDATA%\MTM_WIP_Application\Logs`

Log files contain:
- Application startup and shutdown events
- Database connection status
- Error messages and stack traces
- User activity and transactions

## Best Practices

### Data Management

#### Regular Maintenance
1. **Daily Tasks**:
   - Review transaction logs for errors
   - Monitor database connection status
   - Backup critical data

2. **Weekly Tasks**:
   - Review inventory discrepancies
   - Analyze usage patterns and trends
   - Update user permissions as needed

3. **Monthly Tasks**:
   - Perform full database backup
   - Review and archive old transaction data
   - Update application to latest version

#### Data Entry Standards
1. **Work Order Numbers**:
   - Use consistent formatting (e.g., WO-YYYY####)
   - Validate against manufacturing system
   - Avoid duplicate entries

2. **Part Numbers**:
   - Follow company naming conventions
   - Include revision levels when applicable
   - Maintain master parts list

3. **Location Codes**:
   - Use standardized location identifiers
   - Keep location list current and accurate
   - Remove obsolete locations promptly

### Security Recommendations

#### User Management
1. **Account Security**:
   - Use strong passwords (minimum 8 characters)
   - Change passwords regularly (every 90 days)
   - Disable inactive user accounts

2. **Access Control**:
   - Assign minimal necessary permissions
   - Review user access quarterly
   - Log all administrative actions

#### Database Security
1. **Connection Security**:
   - Use encrypted connections when possible
   - Restrict database access to required IPs
   - Regularly update MySQL server

2. **Data Protection**:
   - Enable MySQL binary logging
   - Configure automated backups
   - Test restore procedures regularly

### Performance Optimization

#### Application Performance
1. **System Resources**:
   - Allocate sufficient RAM to the application
   - Use SSD storage for better performance
   - Keep Windows and drivers updated

2. **Database Performance**:
   - Regular database maintenance
   - Monitor query performance
   - Optimize database indexes

#### Network Considerations
1. **Local vs. Remote Database**:
   - Consider local database for single-user scenarios
   - Use dedicated network for database traffic
   - Monitor network latency and bandwidth

## Updating & Maintenance

### Application Updates

#### Checking for Updates
The application includes automatic update checking:
1. Updates are checked at startup
2. Notifications appear in the status bar
3. Manual check: `Help` → `Check for Updates`

#### Installing Updates
1. **Download Update**:
   - Download from official release page
   - Verify file integrity and source

2. **Backup Current Installation**:
   - Copy current installation directory
   - Export application settings
   - Backup database

3. **Install Update**:
   - Close the application
   - Extract new files over existing installation
   - Restart application and verify functionality

#### Version Compatibility
- Database schema updates are handled automatically
- Configuration files are preserved during updates
- Review release notes for breaking changes

### Database Maintenance

#### Regular Maintenance Tasks
1. **Weekly**:
   ```sql
   -- Optimize tables
   OPTIMIZE TABLE inventory_items, transactions;
   
   -- Check for corrupted tables
   CHECK TABLE inventory_items, transactions;
   ```

2. **Monthly**:
   ```sql
   -- Analyze table statistics
   ANALYZE TABLE inventory_items, transactions;
   
   -- Repair tables if needed
   REPAIR TABLE table_name;
   ```

#### Data Archiving
1. **Archive Old Transactions**:
   ```sql
   -- Archive transactions older than 2 years
   CREATE TABLE transactions_archive AS 
   SELECT * FROM transactions 
   WHERE transaction_date < DATE_SUB(NOW(), INTERVAL 2 YEAR);
   
   DELETE FROM transactions 
   WHERE transaction_date < DATE_SUB(NOW(), INTERVAL 2 YEAR);
   ```

2. **Maintain Audit Logs**:
   - Archive logs older than 1 year
   - Maintain compliance records as required
   - Compress archived data for storage efficiency

### System Health Monitoring

#### Key Metrics to Monitor
1. **Application Performance**:
   - Response times for common operations
   - Memory usage and CPU utilization
   - Error rates and frequency

2. **Database Health**:
   - Connection pool utilization
   - Query execution times
   - Database size and growth rate

3. **Data Integrity**:
   - Transaction consistency checks
   - Inventory count reconciliation
   - Audit trail completeness

## Support

### Getting Help

#### Documentation Resources
- **User Manual**: Complete application documentation
- **Video Tutorials**: Step-by-step instructional videos
- **FAQ**: Frequently asked questions and solutions
- **Release Notes**: Latest features and bug fixes

#### Community Support
- **GitHub Issues**: Report bugs and request features
- **Discussion Forums**: Community-driven support and tips
- **Knowledge Base**: Searchable support articles

#### Professional Support
For enterprise customers and critical issues:
- **Email Support**: support@mtm-applications.com
- **Phone Support**: 1-800-MTM-HELP
- **Priority Support**: 24/7 support for critical issues

### Reporting Issues

When reporting issues, please include:
1. **Application Version**: Found in `Help` → `About`
2. **Operating System**: Windows version and build
3. **Database Version**: MySQL version and configuration
4. **Error Messages**: Complete error text and screenshots
5. **Steps to Reproduce**: Detailed steps that led to the issue
6. **Log Files**: Recent application and database logs

### Feature Requests

We welcome feature requests and suggestions:
1. **GitHub Issues**: Create a feature request issue
2. **User Forums**: Discuss ideas with the community
3. **Direct Contact**: Email feature requests with business justification

---

## License

This project is licensed under the MIT License - see the [LICENSE.txt](LICENSE.txt) file for details.

## Acknowledgments

- **Development Team**: MTM Applications Development Team
- **Database Design**: Optimized for manufacturing environments
- **UI/UX Design**: User-centered design principles
- **Testing**: Comprehensive testing in production environments

---

**© 2025 MTM Applications. All rights reserved.**

*For the latest updates and documentation, visit: [https://github.com/Dorotel/MTM_WIP_Application](https://github.com/Dorotel/MTM_WIP_Application)*