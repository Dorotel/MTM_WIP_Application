-- ================================================================================
-- MTM INVENTORY APPLICATION - SYSTEM ROLE MANAGEMENT STORED PROCEDURES
-- ================================================================================
-- File: 02_System_Role_Procedures.sql
-- Purpose: System roles, user access control, and authorization procedures
-- Created: August 10, 2025
-- Updated: For MySQL 5.7.24 compatibility (MAMP Compatible)
-- Target Database: mtm_wip_application
-- MySQL Version: 5.7.24+ (MAMP Compatible)
-- ================================================================================

-- Drop procedures if they exist (for clean deployment)
DROP PROCEDURE IF EXISTS sys_user_roles_Add;
DROP PROCEDURE IF EXISTS sys_user_roles_Update;
DROP PROCEDURE IF EXISTS sys_user_roles_Delete;
DROP PROCEDURE IF EXISTS sys_roles_Get_ById;
DROP PROCEDURE IF EXISTS sys_SetUserAccessType;
DROP PROCEDURE IF EXISTS sys_GetUserAccessType;
DROP PROCEDURE IF EXISTS sys_GetUserIdByName;
DROP PROCEDURE IF EXISTS sys_GetRoleIdByName;

-- ================================================================================
-- USER ROLE ASSIGNMENT PROCEDURES
-- ================================================================================

-- Add user role assignment with status reporting
DELIMITER $$
CREATE PROCEDURE sys_user_roles_Add(
    IN p_UserID INT,
    IN p_RoleID INT,
    IN p_AssignedBy VARCHAR(100),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_UserCount INT DEFAULT 0;
    DECLARE v_RoleCount INT DEFAULT 0;
    DECLARE v_ExistingCount INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while adding role assignment for user ID: ', p_UserID);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- Validate user exists
    SELECT COUNT(*) INTO v_UserCount FROM usr_users WHERE ID = p_UserID;
    IF v_UserCount = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('User ID not found: ', p_UserID);
        ROLLBACK;
    ELSE
        -- Validate role exists  
        SELECT COUNT(*) INTO v_RoleCount FROM sys_roles WHERE ID = p_RoleID;
        IF v_RoleCount = 0 THEN
            SET p_Status = 1;
            SET p_ErrorMsg = CONCAT('Role ID not found: ', p_RoleID);
            ROLLBACK;
        ELSE
            -- Check if assignment already exists
            SELECT COUNT(*) INTO v_ExistingCount FROM sys_user_roles WHERE UserID = p_UserID AND RoleID = p_RoleID;
            IF v_ExistingCount > 0 THEN
                SET p_Status = 1;
                SET p_ErrorMsg = CONCAT('Role assignment already exists for user ID: ', p_UserID);
                ROLLBACK;
            ELSE
                INSERT INTO sys_user_roles (UserID, RoleID, AssignedBy, AssignedDate)
                VALUES (p_UserID, p_RoleID, p_AssignedBy, NOW());
                
                SET p_Status = 0;
                SET p_ErrorMsg = CONCAT('Role assignment added successfully for user ID: ', p_UserID);
                COMMIT;
            END IF;
        END IF;
    END IF;
END $$
DELIMITER ;

-- Update user role assignment with status reporting
DELIMITER $$
CREATE PROCEDURE sys_user_roles_Update(
    IN p_UserID INT,
    IN p_NewRoleID INT,
    IN p_AssignedBy VARCHAR(100),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_UserCount INT DEFAULT 0;
    DECLARE v_RoleCount INT DEFAULT 0;
    DECLARE v_ExistingCount INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while updating role assignment for user ID: ', p_UserID);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- Validate user exists
    SELECT COUNT(*) INTO v_UserCount FROM usr_users WHERE ID = p_UserID;
    IF v_UserCount = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('User ID not found: ', p_UserID);
        ROLLBACK;
    ELSE
        -- Validate new role exists
        SELECT COUNT(*) INTO v_RoleCount FROM sys_roles WHERE ID = p_NewRoleID;
        IF v_RoleCount = 0 THEN
            SET p_Status = 1;
            SET p_ErrorMsg = CONCAT('Role ID not found: ', p_NewRoleID);
            ROLLBACK;
        ELSE
            -- Check if user has any role assignment
            SELECT COUNT(*) INTO v_ExistingCount FROM sys_user_roles WHERE UserID = p_UserID;
            IF v_ExistingCount = 0 THEN
                -- Insert new role assignment
                INSERT INTO sys_user_roles (UserID, RoleID, AssignedBy, AssignedDate)
                VALUES (p_UserID, p_NewRoleID, p_AssignedBy, NOW());
                
                SET p_Status = 0;
                SET p_ErrorMsg = CONCAT('New role assignment created for user ID: ', p_UserID);
            ELSE
                -- Update existing role assignment
                UPDATE sys_user_roles 
                SET RoleID = p_NewRoleID,
                    AssignedBy = p_AssignedBy,
                    AssignedDate = NOW()
                WHERE UserID = p_UserID;
                
                SET v_RowsAffected = ROW_COUNT();
                
                IF v_RowsAffected > 0 THEN
                    SET p_Status = 0;
                    SET p_ErrorMsg = CONCAT('Role assignment updated successfully for user ID: ', p_UserID);
                ELSE
                    SET p_Status = 2;
                    SET p_ErrorMsg = CONCAT('No changes made to role assignment for user ID: ', p_UserID);
                END IF;
            END IF;
            COMMIT;
        END IF;
    END IF;
END $$
DELIMITER ;

-- Delete user role assignment with status reporting
DELIMITER $$
CREATE PROCEDURE sys_user_roles_Delete(
    IN p_UserID INT,
    IN p_RoleID INT,
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_ExistingCount INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while deleting role assignment for user ID: ', p_UserID);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- Check if assignment exists
    SELECT COUNT(*) INTO v_ExistingCount FROM sys_user_roles WHERE UserID = p_UserID AND RoleID = p_RoleID;
    
    IF v_ExistingCount = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Role assignment not found for user ID: ', p_UserID, ', role ID: ', p_RoleID);
        ROLLBACK;
    ELSE
        DELETE FROM sys_user_roles WHERE UserID = p_UserID AND RoleID = p_RoleID;
        SET v_RowsAffected = ROW_COUNT();
        
        IF v_RowsAffected > 0 THEN
            SET p_Status = 0;
            SET p_ErrorMsg = CONCAT('Role assignment deleted successfully for user ID: ', p_UserID);
        ELSE
            SET p_Status = 2;
            SET p_ErrorMsg = CONCAT('Failed to delete role assignment for user ID: ', p_UserID);
        END IF;
        
        COMMIT;
    END IF;
END $$
DELIMITER ;

-- ================================================================================
-- ROLE INFORMATION PROCEDURES
-- ================================================================================

-- Get role information by ID with status reporting
DELIMITER $$
CREATE PROCEDURE sys_roles_Get_ById(
    IN p_ID INT,
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while retrieving role ID: ', p_ID);
    END;
    
    SELECT COUNT(*) INTO v_Count FROM sys_roles WHERE ID = p_ID;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Role not found with ID: ', p_ID);
        SELECT NULL as ID, NULL as RoleName; -- Return empty result set with structure
    ELSE
        SELECT * FROM sys_roles WHERE ID = p_ID LIMIT 1;
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Role retrieved successfully for ID: ', p_ID);
    END IF;
END $$
DELIMITER ;

-- ================================================================================
-- ACCESS CONTROL PROCEDURES
-- ================================================================================

-- Set user access type (Admin, ReadOnly, Normal) with status reporting
DELIMITER $$
CREATE PROCEDURE sys_SetUserAccessType(
    IN p_UserName VARCHAR(100),
    IN p_AccessType VARCHAR(50),
    IN p_AssignedBy VARCHAR(100),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_UserID INT DEFAULT 0;
    DECLARE v_RoleID INT DEFAULT 0;
    DECLARE v_UserCount INT DEFAULT 0;
    DECLARE v_RoleCount INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while setting access type for user: ', p_UserName);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- Get user ID
    SELECT COUNT(*) INTO v_UserCount FROM usr_users WHERE User = p_UserName;
    IF v_UserCount = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('User not found: ', p_UserName);
        ROLLBACK;
    ELSE
        SELECT ID INTO v_UserID FROM usr_users WHERE User = p_UserName LIMIT 1;
        
        -- Get role ID based on access type
        SELECT COUNT(*) INTO v_RoleCount FROM sys_roles WHERE RoleName = p_AccessType;
        IF v_RoleCount = 0 THEN
            SET p_Status = 1;
            SET p_ErrorMsg = CONCAT('Role not found: ', p_AccessType);
            ROLLBACK;
        ELSE
            SELECT ID INTO v_RoleID FROM sys_roles WHERE RoleName = p_AccessType LIMIT 1;
            
            -- Update or insert user role
            INSERT INTO sys_user_roles (UserID, RoleID, AssignedBy, AssignedDate)
            VALUES (v_UserID, v_RoleID, p_AssignedBy, NOW())
            ON DUPLICATE KEY UPDATE 
                RoleID = v_RoleID,
                AssignedBy = p_AssignedBy,
                AssignedDate = NOW();
            
            SET p_Status = 0;
            SET p_ErrorMsg = CONCAT('Access type set to ', p_AccessType, ' for user: ', p_UserName);
            COMMIT;
        END IF;
    END IF;
END $$
DELIMITER ;

-- Get user access types for all users with status reporting
DELIMITER $$
CREATE PROCEDURE sys_GetUserAccessType(
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred while retrieving user access types';
    END;
    
    SELECT COUNT(*) INTO v_Count FROM usr_users;
    
    SELECT 
        u.ID,
        u.User,
        COALESCE(r.RoleName, 'None') as RoleName
    FROM usr_users u
    LEFT JOIN sys_user_roles ur ON u.ID = ur.UserID
    LEFT JOIN sys_roles r ON ur.RoleID = r.ID
    ORDER BY u.User;
    
    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('Retrieved access types for ', v_Count, ' users successfully');
END $$
DELIMITER ;

-- Get user ID by username with status reporting
DELIMITER $$
CREATE PROCEDURE sys_GetUserIdByName(
    IN p_UserName VARCHAR(100),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while retrieving user ID for: ', p_UserName);
    END;
    
    SELECT COUNT(*) INTO v_Count FROM usr_users WHERE User = p_UserName;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('User not found: ', p_UserName);
        SELECT NULL as ID;
    ELSE
        SELECT ID FROM usr_users WHERE User = p_UserName LIMIT 1;
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('User ID retrieved successfully for: ', p_UserName);
    END IF;
END $$
DELIMITER ;

-- Get role ID by role name with status reporting  
DELIMITER $$
CREATE PROCEDURE sys_GetRoleIdByName(
    IN p_RoleName VARCHAR(100),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while retrieving role ID for: ', p_RoleName);
    END;
    
    SELECT COUNT(*) INTO v_Count FROM sys_roles WHERE RoleName = p_RoleName;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Role not found: ', p_RoleName);
        SELECT NULL as ID;
    ELSE
        SELECT ID FROM sys_roles WHERE RoleName = p_RoleName LIMIT 1;
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Role ID retrieved successfully for: ', p_RoleName);
    END IF;
END $$
DELIMITER ;

-- ================================================================================
-- END OF SYSTEM ROLE PROCEDURES
-- ================================================================================
