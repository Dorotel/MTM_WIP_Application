# ================================================================================
# MTM INVENTORY APPLICATION - STORED PROCEDURES DEPLOYMENT GUIDE (WINDOWS)
# ================================================================================
# File: README.md
# Purpose: Windows-focused deployment guide for stored procedures using .bat files
# Created: August 13, 2025
# Updated: August 13, 2025 - Windows .bat files only
# Target Database: mtm_wip_application (production) / mtm_wip_application_test (development)
# MySQL Version: 5.7.24+ (MAMP Compatible)
# Platform: Windows only
# ================================================================================

# MTM Inventory Application - Windows Deployment Guide

This directory contains all stored procedures and Windows deployment tools for the MTM Inventory Application. All deployment is handled through Windows .bat files optimized for MAMP and MySQL 5.7.24.

## 📁 File Structure

### **Stored Procedure Files (10 files)**
```
Database/UpdatedStoredProcedures/
├── 00_StoredProcedure_Verification_System.sql  # System verification procedures (5 procedures)
├── 01_User_Management_Procedures.sql           # User authentication & settings (17 procedures)
├── 02_System_Role_Procedures.sql               # Role-based access control (8 procedures)
├── 03_Master_Data_Procedures.sql               # Parts, operations, locations (23 procedures)
├── 04_Inventory_Procedures.sql                 # Inventory tracking & transactions (15 procedures)
├── 05_Error_Log_Procedures.sql                 # Error logging system (7 procedures)
├── 06_Quick_Button_Procedures.sql              # Quick button management (7 procedures)
├── 07_Changelog_Version_Procedures.sql         # Version management (4 procedures)
├── 08_Theme_Management_Procedures.sql          # UI theme system (8 procedures)
└── 99_Database_Testing_Suite.sql               # Testing utilities (2 procedures)

Total: ~96 Stored Procedures
```

### **Windows Deployment Tools (5 .bat files)**
```
├── deploy.bat                    # 🚀 MAIN DEPLOYMENT - Deploy all stored procedures
├── analyze.bat                   # 🔍 CODE ANALYSIS - Analyze procedures for issues
├── run_verification.bat          # ✅ VERIFICATION - Test deployed procedures
├── final_verification.bat        # 🏁 FINAL CHECK - Quick verification status
└── fix_and_reverify.bat         # 🔧 FIX & TEST - Fix schema issues and re-verify
```

## 🚀 Quick Start Guide

### **Step 1: Ensure MAMP is Running**
- Start MAMP application
- Verify Apache and MySQL services are running (green lights)
- Note the MySQL port (usually 3306, sometimes 8889 for older MAMP)

### **Step 2: Deploy All Procedures**
```cmd
# Navigate to the directory
cd "C:\Users\[YourUsername]\source\repos\MTM_WIP_Application\Database\UpdatedStoredProcedures"

# Deploy all procedures (MAMP defaults)
deploy.bat

# Or with custom parameters
deploy.bat -h localhost -u root -p root -d mtm_wip_application
```

### **Step 3: Verify Deployment**
```cmd
# Run verification
run_verification.bat

# Or quick final check
final_verification.bat
```

## 🔧 Detailed .bat File Guide

### **1. deploy.bat** - Main Deployment Script
**Purpose**: Deploy all stored procedures to your MySQL database

#### **Basic Usage**:
```cmd
# Deploy with MAMP defaults (recommended)
deploy.bat

# Deploy to test database
deploy.bat -d mtm_wip_application_test

# Deploy with custom credentials
deploy.bat -h localhost -u myuser -p mypass -d mydatabase
```

#### **Available Parameters**:
- `-h, --host` - Database host (default: localhost)
- `-P, --port` - Database port (default: 3306)
- `-u, --user` - Database username (default: root)
- `-p, --password` - Database password (default: root)
- `-d, --database` - Database name (default: mtm_wip_application)
- `--mamp-path` - Custom MAMP installation path

#### **What It Does**:
- ✅ Tests database connection
- ✅ Creates backup of existing procedures
- ✅ Deploys all 10 SQL files in order
- ✅ Provides success/failure feedback
- ✅ Shows total procedure count (~96 procedures)

---

### **2. analyze.bat** - Code Analysis Tool
**Purpose**: Analyze stored procedures for potential issues and compatibility

#### **Usage**:
```cmd
# Run analysis on all procedures
analyze.bat
```

#### **What It Analyzes**:
- ✅ **Procedure Count**: Counts procedures in each file
- ✅ **Schema Validation**: Checks table structures
- ✅ **Column Names**: Verifies column name consistency
- ✅ **Error Handling**: Analyzes error handling patterns
- ✅ **MySQL Compatibility**: Checks MySQL 5.7.24 compatibility
- ✅ **Fix Recommendations**: Provides improvement suggestions

#### **Output**:
- Creates detailed analysis report: `%TEMP%\stored_procedure_analysis_report.txt`
- Opens report automatically in Notepad
- Console output with summary of findings

---

### **3. run_verification.bat** - System Verification
**Purpose**: Deploy and test the verification system

#### **Usage**:
```cmd
# Deploy verification system and run tests
run_verification.bat
```

#### **What It Does**:
- ✅ Deploys verification stored procedures
- ✅ Tests database schema (17 required tables)
- ✅ Verifies procedure inventory (~96+ procedures expected)
- ✅ Runs comprehensive system verification
- ✅ Provides detailed status report

#### **Expected Results**:
```
SCHEMA VERIFICATION: PASSED
PROCEDURE INVENTORY: COMPLETED (77+ procedures found)
PROCEDURE TESTING: 1-3 tests (some "failures" are expected)
```

---

### **4. final_verification.bat** - Quick Status Check
**Purpose**: Quick verification of system status

#### **Usage**:
```cmd
# Quick system health check
final_verification.bat
```

#### **What It Does**:
- ⚡ Fast execution (assumes verification system is deployed)
- ✅ Runs complete verification procedure
- ✅ Shows overall system status
- ✅ Provides pass/fail summary

#### **Expected Output**:
```
OverallStatus: 0 (success) or 1 (issues detected)
FinalResult: "✅ ALL SYSTEMS VERIFIED" or "⚠ Issues detected"
```

---

### **5. fix_and_reverify.bat** - Schema Repair Tool
**Purpose**: Fix missing tables/schema issues and re-verify

#### **Usage**:
```cmd
# Fix schema issues and re-run verification
fix_and_reverify.bat
```

#### **What It Does**:
- 🔧 Creates missing tables (sys_quick_buttons, log_error_log view)
- 🔧 Fixes column name mismatches
- 🔧 Adds sample data where needed
- ✅ Re-runs complete verification
- ✅ Shows before/after comparison

#### **When to Use**:
- When verification shows "Missing tables detected"
- When procedures fail due to schema mismatches
- After major database changes

## 💡 Environment-Specific Usage

### **Development Environment** (Recommended)
```cmd
# Deploy to test database for development
deploy.bat -d mtm_wip_application_test

# Analyze and verify development setup
analyze.bat
run_verification.bat
```

### **Production Environment**
```cmd
# Deploy to production database
deploy.bat -d mtm_wip_application -h 172.16.1.104

# Verify production deployment
final_verification.bat
```

### **Local MAMP Development** (Most Common)
```cmd
# Standard MAMP deployment (all defaults work)
deploy.bat

# Verify MAMP deployment
run_verification.bat
```

## 🔍 Troubleshooting Guide

### **Common Issues and Solutions**

#### **1. "mysql command not found"**
**Problem**: MAMP MySQL not in system PATH
**Solutions**:
```cmd
# Option 1: Use MAMP path parameter
deploy.bat --mamp-path "C:\MAMP"

# Option 2: Add MAMP to PATH manually
set PATH=C:\MAMP\bin\mysql\bin;%PATH%
deploy.bat

# Option 3: Use full path to mysql (the .bat files handle this automatically)
```

#### **2. "Can't connect to database"**
**Problem**: MAMP not running or wrong credentials
**Solutions**:
- Start MAMP and ensure MySQL service is running
- Check MAMP control panel for correct port (3306 or 8889)
- Verify credentials (MAMP default is usually root/root)
```cmd
# Check different port
deploy.bat -P 8889
```

#### **3. "Database doesn't exist"**
**Problem**: Target database not created
**Solutions**:
- Create database in phpMyAdmin: `http://localhost/phpMyAdmin`
- Or create via command line:
```cmd
mysql -h localhost -P 3306 -u root -p -e "CREATE DATABASE mtm_wip_application;"
```

#### **4. "Some procedures failed to deploy"**
**Problem**: Procedure conflicts or syntax errors
**Solutions**:
```cmd
# Run analysis first
analyze.bat

# Fix schema issues
fix_and_reverify.bat

# Re-deploy
deploy.bat
```

#### **5. "Verification shows missing tables"**
**Problem**: Database schema incomplete
**Solution**:
```cmd
# Automatic fix
fix_and_reverify.bat
```

## 📊 Expected Results

### **Successful Deployment**:
```
[SUCCESS] All stored procedures deployed successfully!
[INFO] Total: ~96 procedures with uniform p_ parameter naming
```

### **Successful Verification**:
```
SCHEMA VERIFICATION: PASSED (17/17 required tables found)
PROCEDURE INVENTORY: COMPLETED (77+ stored procedures)
OVERALL RESULT: ✅ ALL SYSTEMS VERIFIED AND WORKING!
```

### **Successful Analysis**:
```
Total Procedures Analyzed: 96
Schema Files Checked: 10
MySQL 5.7.24 Compatibility: ✅ All files compatible
```

## 🎯 Integration with .NET 8 Application

### **Connection String Configuration**
The deployed procedures work with your .NET 8 application using:

```csharp
// Automatic environment detection
string connectionString = Helper_Database_Variables.GetConnectionString();
string database = Model_Users.Database;        // mtm_wip_application or mtm_wip_application_test
string server = Model_Users.WipServerAddress;  // localhost or 172.16.1.104
```

### **Calling Stored Procedures**
```csharp
// All procedures use uniform p_ parameter naming
var result = await Helper_Database_StoredProcedure.ExecuteDataTableWithStatus(
    connectionString,
    "usr_users_Get_All",  // Procedure name
    null,                 // No parameters
    progressHelper,       // Optional progress reporting
    true                  // Show progress
);
```

## 🧪 Testing and Validation Requirements

### **MANDATORY: All stored procedures MUST be tested against LiveDatabase.sql**

Starting from this update, all stored procedures must be validated against the actual database schema before deployment:

1. **Schema Compatibility**: Procedures must work with the table structures defined in `Database/UpdatedDatabase/LiveDatabase.sql`
2. **Parameter Validation**: All IN/OUT parameters must match what the C# application expects
3. **Error Handling**: All procedures must include proper error handling with `OUT p_Status INT, OUT p_ErrorMsg VARCHAR(255)` parameters

### **Testing Workflow**:

#### **Before Creating/Modifying Procedures**:
```bash
# 1. Validate current database schema
mysql -h localhost -u root -p < Database/UpdatedDatabase/LiveDatabase.sql

# 2. Test your changes
mysql -h localhost -u root -p -e "CALL your_new_procedure(test_params, @status, @error); SELECT @status, @error;"
```

#### **After Creating/Modifying Procedures**:
```bash
# 1. Deploy and verify
deploy.bat
run_verification.bat

# 2. Test critical Settings Form procedures
mysql -h localhost -u root -p -e "
CALL usr_users_Add_User('TESTUSER', 'Test User', 'First', 0, '1234', '1.0.0', 'false', 'Default', 9, '', '', 'localhost', 'mtm_wip_application_test', '3306', @status, @msg);
SELECT @status as Status, @msg as Message;
"
```

### **Settings Form Critical Procedures**:
These procedures are essential for Settings Form functionality and MUST be tested:

- `usr_users_Add_User` - Adding new users
- `usr_users_Update_User` - Updating existing users  
- `usr_users_Delete_User` - Removing users
- `md_part_ids_Add_Part` - Adding new part IDs
- `md_locations_Add_Location` - Adding new locations
- `md_operation_numbers_Add_Operation` - Adding new operations
- `md_item_types_Add_ItemType` - Adding new item types

## 📋 Maintenance Workflow

### **Regular Development Workflow**:
1. **Deploy**: `deploy.bat` - Deploy latest procedure changes
2. **Verify**: `run_verification.bat` - Ensure everything works
3. **Develop**: Use procedures in your .NET 8 application
4. **Analyze**: `analyze.bat` - Periodic code quality checks

### **Troubleshooting Workflow**:
1. **Analyze**: `analyze.bat` - Identify issues
2. **Fix**: `fix_and_reverify.bat` - Repair schema/tables
3. **Deploy**: `deploy.bat` - Re-deploy procedures
4. **Verify**: `final_verification.bat` - Confirm fixes

### **Production Deployment Workflow**:
1. **Test**: Deploy and verify in development environment first
2. **Backup**: Ensure production database is backed up
3. **Deploy**: `deploy.bat -d mtm_wip_application -h 172.16.1.104`
4. **Verify**: `final_verification.bat` - Confirm production deployment

## 🎉 Summary

This Windows-focused deployment system provides:

- ✅ **5 Specialized .bat Files** for different deployment tasks
- ✅ **96+ Stored Procedures** across 10 SQL files
- ✅ **Complete MAMP Integration** with automatic path detection
- ✅ **MySQL 5.7.24 Compatibility** tested and verified
- ✅ **Comprehensive Error Handling** with helpful messages
- ✅ **Automated Verification** system with detailed reporting
- ✅ **Production-Ready** deployment with backup creation

**Your MTM Inventory Application database layer is now fully automated and Windows-optimized!** 🚀
