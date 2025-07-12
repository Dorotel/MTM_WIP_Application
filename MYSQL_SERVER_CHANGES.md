# MySQL Server Changes Log

This document tracks all changes made to the MySQL server schema, roles, permissions, and procedures for the `mtm_wip_application` database.

## Overview

All changes must be documented here before merging code that depends on database modifications. Each entry should include:
- Date and author
- Reason for change
- SQL statements executed
- Rollback procedures (if applicable)
- Impact assessment

---

## 2025-01-12 - Initial Database Schema

**Date:** 2025-01-12  
**By:** System Setup  
**Reason:** Initial database structure for MTM WIP Application

**SQL:**
```sql
-- Create database
CREATE DATABASE IF NOT EXISTS `mtm_wip_application` DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;
USE `mtm_wip_application`;

-- User roles system
CREATE TABLE `sys_roles` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `RoleName` varchar(50) NOT NULL,
  `Description` varchar(255) DEFAULT NULL,
  `CreatedAt` timestamp DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `RoleName` (`RoleName`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- User role assignments
CREATE TABLE `sys_user_roles` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `UserID` int(11) NOT NULL,
  `RoleID` int(11) NOT NULL,
  `AssignedBy` varchar(100) NOT NULL,
  `AssignedAt` timestamp DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `UserID_RoleID` (`UserID`, `RoleID`),
  KEY `idx_user_roles_user` (`UserID`),
  KEY `idx_user_roles_role` (`RoleID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Insert default roles
INSERT INTO `sys_roles` (`RoleName`, `Description`) VALUES 
('Admin', 'Full system access - all read/write operations'),
('Normal', 'Limited access - read all, write to inventory/transactions only'),
('ReadOnly', 'Read-only access - search and view only');
```

**Notes:**
- Establishes three-tier role system as per privilege matrix
- Admin: Full access to all features and tables
- Normal: Read all, write limited to inv_inventory and inv_transaction tables
- ReadOnly: Search and view only, no write operations

---

## 2025-01-12 - Role-Based Access Control Implementation

**Date:** 2025-01-12  
**By:** copilot  
**Reason:** Implement comprehensive role-based access control per REPO_COMPREHENSIVE_CHECKLIST.md requirements

**SQL:**
```sql
-- Add stored procedures for role management
DELIMITER $$

CREATE PROCEDURE `sys_roles_Get_All`()
BEGIN
    SELECT ID, RoleName, Description, CreatedAt FROM sys_roles ORDER BY RoleName;
END$$

CREATE PROCEDURE `sys_roles_Get_ById`(IN p_ID INT)
BEGIN
    SELECT ID, RoleName, Description, CreatedAt FROM sys_roles WHERE ID = p_ID;
END$$

CREATE PROCEDURE `sys_user_roles_Add`(IN p_UserID INT, IN p_RoleID INT, IN p_AssignedBy VARCHAR(100))
BEGIN
    INSERT INTO sys_user_roles (UserID, RoleID, AssignedBy) VALUES (p_UserID, p_RoleID, p_AssignedBy);
END$$

CREATE PROCEDURE `sys_user_roles_Update`(IN p_UserID INT, IN p_NewRoleID INT, IN p_AssignedBy VARCHAR(100))
BEGIN
    DELETE FROM sys_user_roles WHERE UserID = p_UserID;
    INSERT INTO sys_user_roles (UserID, RoleID, AssignedBy) VALUES (p_UserID, p_NewRoleID, p_AssignedBy);
END$$

CREATE PROCEDURE `sys_user_roles_Delete`(IN p_UserID INT, IN p_RoleID INT)
BEGIN
    DELETE FROM sys_user_roles WHERE UserID = p_UserID AND RoleID = p_RoleID;
END$$

DELIMITER ;
```

**Notes:**
- Provides stored procedures for role management operations
- Supports single role per user (updates replace existing roles)
- Tracks assignment auditing with AssignedBy field

---

## Future Changes

All future database changes must be documented here following the same format. This ensures:
- Complete audit trail of schema evolution
- Proper coordination between database and application code
- Rollback procedures for troubleshooting
- Team awareness of database dependencies

## Change Requirements

Before making any database changes:
1. Document the change here first
2. Test the change in development environment
3. Update `DATABASE_SCHEMA.sql` to reflect current state
4. Ensure application code is updated to match schema changes
5. Get approval from @Dorotel before applying to production