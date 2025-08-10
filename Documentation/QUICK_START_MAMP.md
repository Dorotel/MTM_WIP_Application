# ?? Quick Start Guide - MTM Inventory Application with MAMP

## Your Current Environment
- **MySQL Version**: 5.7.24 (MAMP)
- **Client**: MAMP 
- **OS**: Windows/macOS (adjust commands accordingly)

## ? **Step-by-Step Deployment**

### **Step 1: Ensure MAMP is Running**
1. **Start MAMP application**
2. **Click "Start Servers"** or ensure both Apache and MySQL show **green lights**
3. **Note the ports**: Usually 3306 for MySQL (check MAMP control panel)

### **Step 2: Create Database (if needed)**
**Option A - Via phpMyAdmin (Recommended):**
1. Open browser: `http://localhost/phpMyAdmin`
2. Click **"New"** in left sidebar
3. Database name: `mtm_wip_application`
4. Click **"Create"**

**Option B - Via Command Line:**
```sql
-- Connect to MAMP MySQL
mysql -h localhost -P 3306 -u root -p

-- Create database
CREATE DATABASE IF NOT EXISTS mtm_wip_application;
USE mtm_wip_application;
```

### **Step 3: Deploy Stored Procedures**

**Windows:**
```cmd
# Navigate to your project directory
cd C:\Users\johnk\source\repos\MTM_WIP_Application\Database\StoredProcedures

# Deploy with MAMP defaults (root/root)
deploy_procedures.bat -u root -p root -d mtm_wip_application

# If you get "command not found", try with MAMP path:
deploy_procedures.bat --mamp-path "C:\MAMP" -u root -p root -d mtm_wip_application
```

**macOS:**
```bash
# Navigate to your project directory
cd /path/to/MTM_WIP_Application/Database/StoredProcedures

# Make script executable
chmod +x deploy_procedures.sh

# Deploy with MAMP defaults
./deploy_procedures.sh -u root -p root -d mtm_wip_application

# If you need custom MAMP path:
./deploy_procedures.sh --mysql-path /Applications/MAMP/Library/bin -u root -p root
```

### **Step 4: Verify Deployment**
**Via phpMyAdmin:**
1. Open: `http://localhost/phpMyAdmin`
2. Select `mtm_wip_application` database
3. Click **"Routines"** tab
4. You should see **35+ stored procedures**

**Via Command Line:**
```sql
mysql -h localhost -P 3306 -u root -p mtm_wip_application

-- Check procedures
SELECT COUNT(*) as ProcedureCount FROM INFORMATION_SCHEMA.ROUTINES 
WHERE ROUTINE_SCHEMA = 'mtm_wip_application';

-- Should return count > 30
```

### **Step 5: Update Application Connection String**
In your application, ensure connection string points to MAMP:

```csharp
// Example for your MAMP setup
string connectionString = "Server=localhost;Port=3306;Database=mtm_wip_application;Uid=root;Pwd=root;Allow User Variables=True;";
```

## ?? **Troubleshooting Common Issues**

### **"Can't connect to MySQL server"**
```bash
# Check MAMP MySQL status
# Ensure green light for MySQL in MAMP control panel

# Check correct port (usually 3306)
# Some older MAMP versions use port 8889:
deploy_procedures.bat -P 8889 -u root -p root -d mtm_wip_application
```

### **"Access denied for user"**
```bash
# MAMP default credentials are usually root/root
# Try without password (some MAMP setups):
deploy_procedures.bat -u root -p "" -d mtm_wip_application

# Or check MAMP control panel for actual credentials
```

### **"MySQL command not found"**
```bash
# Windows - Add MAMP to PATH or use full path:
deploy_procedures.bat --mamp-path "C:\MAMP"

# macOS - Use custom path:
./deploy_procedures.sh --mysql-path /Applications/MAMP/Library/bin
```

### **"Database doesn't exist"**
```sql
-- Create via phpMyAdmin or command line:
CREATE DATABASE mtm_wip_application;
```

## ?? **Testing the Enhanced Error Handling**

### **Step 1: Run Your Application**
- Build and run MTM Inventory Application
- Navigate to Settings ? Users ? Add User

### **Step 2: Test Error Scenarios**
1. **Leave First Name empty** ? Click Save
   - ? Progress bar should turn **RED**
   - ? Status should show: **"ERROR: First name is required"**

2. **Try to create duplicate user** ? Enter existing username ? Click Save
   - ? Progress bar should turn **RED** 
   - ? Status should show: **"ERROR: User already exists: [username]"**

### **Step 3: Test Success Scenarios**
1. **Fill all required fields correctly** ? Click Save
   - ? Progress bar should turn **GREEN**
   - ? Status should show: **"SUCCESS: User '[name]' created successfully!"**

## ?? **What You Now Have**

### **Enhanced Error Handling System**
- ? **Red Progress Bars** on all database errors
- ? **Detailed Error Messages** from stored procedures
- ? **Success Feedback** with green progress bars
- ? **Consistent User Experience** across all settings forms

### **MySQL 5.7.24 Compatibility**
- ? **Native Compatibility** with your MySQL version
- ? **MAMP Integration** optimized deployment
- ? **Robust Error Handling** at database level
- ? **Performance Optimized** stored procedures

### **Development Benefits**
- ? **Professional UI** with color-coded feedback
- ? **Easier Debugging** with specific error messages
- ? **Consistent Patterns** for adding new features
- ? **Production Ready** error handling system

## ?? **Quick Commands Reference**

```bash
# Deploy procedures (Windows MAMP)
deploy_procedures.bat -u root -p root

# Deploy procedures (macOS MAMP)  
./deploy_procedures.sh -u root -p root

# Check MySQL version
mysql -h localhost -P 3306 -u root -p -e "SELECT VERSION();"

# List all procedures
mysql -h localhost -P 3306 -u root -p mtm_wip_application -e "SHOW PROCEDURE STATUS WHERE Db='mtm_wip_application';"

# Test a procedure
mysql -h localhost -P 3306 -u root -p mtm_wip_application -e "CALL usr_users_Get_All(@status, @error); SELECT @status, @error;"
```

## ?? **If You Need Help**

If you encounter any issues:

1. **Check MAMP Status**: Ensure both Apache and MySQL are running
2. **Verify Ports**: Check MAMP control panel for correct MySQL port
3. **Check Credentials**: Default is usually root/root
4. **Database Exists**: Create `mtm_wip_application` database if needed
5. **Check Logs**: Look at MAMP error logs for specific issues

Your enhanced stored procedure error handling system is now ready to use with your MySQL 5.7.24 MAMP setup! ??
