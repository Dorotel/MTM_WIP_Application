-- ================================================================================
-- MTM INVENTORY APPLICATION - SYSTEM ROLE MANAGEMENT STORED PROCEDURES
-- ================================================================================
-- File: 02_System_Role_Procedures.sql
-- Purpose: System roles, user access control, and authorization procedures
-- Created: August 10, 2025
-- Updated: August 10, 2025 - UNIFORM PARAMETER NAMING (WITH p_ prefixes)
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
        SET p_ErrorMsg = CONCAT('Role assignment not found for user ID: ', p_UserID, ' and role ID: ', p_RoleID);
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

-- Get role information by ID
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
        SET p_ErrorMsg = CONCAT('Database error occurred while retrieving role with ID: ', p_ID);
    END;
    
    SELECT COUNT(*) INTO v_Count FROM sys_roles WHERE ID = p_ID;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Role not found with ID: ', p_ID);
        SELECT NULL as ID, NULL as RoleName, NULL as Description;
    ELSE
        SELECT * FROM sys_roles WHERE ID = p_ID LIMIT 1;
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Role retrieved successfully with ID: ', p_ID);
    END IF;
END $$
DELIMITER ;

-- ================================================================================
-- USER ACCESS CONTROL PROCEDURES
-- ================================================================================

-- Set user access type with status reporting
DELIMITER $$
CREATE PROCEDURE sys_SetUserAccessType(
    IN p_UserName VARCHAR(100),
    IN p_AccessType VARCHAR(50),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_UserCount INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while setting access type for user: ', p_UserName);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- Check if user exists
    SELECT COUNT(*) INTO v_UserCount FROM usr_users WHERE User = p_UserName;
    IF v_UserCount = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('User not found: ', p_UserName);
        ROLLBACK;
    ELSE
        -- Update user access type
        UPDATE usr_users 
        SET AccessType = p_AccessType, 
            ModifiedDate = NOW() 
        WHERE User = p_UserName;
        
        SET v_RowsAffected = ROW_COUNT();
        
        IF v_RowsAffected > 0 THEN
            SET p_Status = 0;
            SET p_ErrorMsg = CONCAT('Access type updated successfully for user: ', p_UserName);
        ELSE
            SET p_Status = 2;
            SET p_ErrorMsg = CONCAT('No changes made to access type for user: ', p_UserName);
        END IF;
        
        COMMIT;
    END IF;
END $$
DELIMITER ;

-- Get user access type information
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
        SET p_ErrorMsg = 'Database error occurred while retrieving user access information';
    END;
    
    SELECT COUNT(*) INTO v_Count FROM usr_users;
    
    -- Return user access information
    SELECT 
        ID,
        User,
        FullName,
        AccessType,
        VitsUser,
        CreatedDate,
        ModifiedDate
    FROM usr_users 
    ORDER BY FullName;
    
    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('Retrieved user access information for ', v_Count, ' users');
END $$
DELIMITER ;

-- Get user ID by username
DELIMITER $$
CREATE PROCEDURE sys_GetUserIdByName(
    IN p_UserName VARCHAR(100),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_UserID INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while retrieving user ID for: ', p_UserName);
    END;
    
    SELECT COUNT(*) INTO v_Count FROM usr_users WHERE User = p_UserName;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('User not found: ', p_UserName);
        SELECT NULL as UserID;
    ELSE
        SELECT ID INTO v_UserID FROM usr_users WHERE User = p_UserName LIMIT 1;
        SELECT v_UserID as UserID;
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('User ID retrieved successfully for: ', p_UserName);
    END IF;
END $$
DELIMITER ;

-- Get role ID by role name
DELIMITER $$
CREATE PROCEDURE sys_GetRoleIdByName(
    IN p_RoleName VARCHAR(100),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_RoleID INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while retrieving role ID for: ', p_RoleName);
    END;
    
    SELECT COUNT(*) INTO v_Count FROM sys_roles WHERE RoleName = p_RoleName;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Role not found: ', p_RoleName);
        SELECT NULL as RoleID;
    ELSE
        SELECT ID INTO v_RoleID FROM sys_roles WHERE RoleName = p_RoleName LIMIT 1;
        SELECT v_RoleID as RoleID;
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Role ID retrieved successfully for: ', p_RoleName);
    END IF;
END $$
DELIMITER ;

-- ================================================================================
-- END OF SYSTEM ROLE PROCEDURES
-- ================================================================================
