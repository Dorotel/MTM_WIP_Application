-- ================================================================================
-- MTM INVENTORY APPLICATION - USER MANAGEMENT STORED PROCEDURES
-- ================================================================================
-- File: 01_User_Management_Procedures.sql
-- Purpose: Core user management, authentication, and settings procedures
-- Created: August 10, 2025
-- Updated: August 10, 2025 - UNIFORM PARAMETER NAMING (WITH p_ prefixes)
-- Target Database: mtm_wip_application_test
-- MySQL Version: 5.7.24+ (MAMP Compatible)
-- ================================================================================

-- Drop procedures if they exist (for clean deployment)
DROP PROCEDURE IF EXISTS usr_ui_settings_Delete_ByUserId;
DROP PROCEDURE IF EXISTS usr_users_GetFullName_ByUser;
DROP PROCEDURE IF EXISTS usr_ui_settings_GetSettingsJson_ByUserId;
DROP PROCEDURE IF EXISTS usr_users_GetUserSetting_ByUserAndField;
DROP PROCEDURE IF EXISTS usr_users_SetUserSetting_ByUserAndField;
DROP PROCEDURE IF EXISTS usr_user_roles_GetRoleId_ByUserId;
DROP PROCEDURE IF EXISTS usr_users_Add_User;
DROP PROCEDURE IF EXISTS usr_users_Update_User;
DROP PROCEDURE IF EXISTS usr_users_Delete_User;
DROP PROCEDURE IF EXISTS usr_users_Get_All;
DROP PROCEDURE IF EXISTS usr_users_Get_ByUser;
DROP PROCEDURE IF EXISTS usr_users_Exists;
DROP PROCEDURE IF EXISTS usr_ui_settings_SetThemeJson;
DROP PROCEDURE IF EXISTS usr_ui_settings_SetJsonSetting;
DROP PROCEDURE IF EXISTS usr_ui_settings_GetJsonSetting;
DROP PROCEDURE IF EXISTS usr_ui_settings_GetShortcutsJson;
DROP PROCEDURE IF EXISTS usr_ui_settings_SetShortcutsJson;

-- ================================================================================
-- USER SETTINGS PROCEDURES
-- ================================================================================

-- Delete user UI settings by user ID (for clean user removal)
DELIMITER $$
CREATE PROCEDURE usr_ui_settings_Delete_ByUserId(
    IN p_UserId VARCHAR(64),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while deleting user settings for user: ', p_UserId);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    DELETE FROM usr_ui_settings WHERE UserId = p_UserId;
    
    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('User settings deleted successfully for user: ', p_UserId);
    
    COMMIT;
END $$
DELIMITER ;

-- Get user full name by username with status reporting
DELIMITER $$
CREATE PROCEDURE usr_users_GetFullName_ByUser(
    IN p_User VARCHAR(100),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while retrieving full name for user: ', p_User);
    END;
    
    SELECT COUNT(*) INTO v_Count FROM usr_users WHERE User = p_User;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('User not found: ', p_User);
    ELSE
        SELECT `Full Name` FROM usr_users WHERE User = p_User LIMIT 1;
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Full name retrieved successfully for user: ', p_User);
    END IF;
END $$
DELIMITER ;

-- Get user interface settings JSON by user ID with status reporting (MySQL 5.7 Compatible)
DELIMITER $$
CREATE PROCEDURE usr_ui_settings_GetSettingsJson_ByUserId(
    IN p_UserId VARCHAR(100),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while retrieving settings for user: ', p_UserId);
    END;
    
    SELECT COUNT(*) INTO v_Count FROM usr_ui_settings WHERE UserId = p_UserId;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('No settings found for user: ', p_UserId);
        SELECT NULL as SettingsJson;
    ELSE
        SELECT SettingsJson FROM usr_ui_settings WHERE UserId = p_UserId LIMIT 1;
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Settings retrieved successfully for user: ', p_UserId);
    END IF;
END $$
DELIMITER ;

-- Get specific user setting by user and field name (legacy support)
DELIMITER $$
CREATE PROCEDURE usr_users_GetUserSetting_ByUserAndField(
    IN p_User VARCHAR(100), 
    IN p_Field VARCHAR(100),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_FieldValue TEXT DEFAULT NULL;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while retrieving setting ', p_Field, ' for user: ', p_User);
    END;
    
    -- Check if user exists
    SELECT COUNT(*) INTO v_Count FROM usr_users WHERE User = p_User;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('User not found: ', p_User);
        SELECT NULL as FieldValue;
    ELSE
        -- Use fixed field names instead of dynamic SQL for MySQL 5.7.24 compatibility
        CASE p_Field
            WHEN 'LastShownVersion' THEN SELECT LastShownVersion INTO v_FieldValue FROM usr_users WHERE User = p_User LIMIT 1;
            WHEN 'HideChangeLog' THEN SELECT HideChangeLog INTO v_FieldValue FROM usr_users WHERE User = p_User LIMIT 1;
            WHEN 'Theme_Name' THEN SELECT Theme_Name INTO v_FieldValue FROM usr_users WHERE User = p_User LIMIT 1;
            WHEN 'Theme_FontSize' THEN SELECT CAST(Theme_FontSize AS CHAR) INTO v_FieldValue FROM usr_users WHERE User = p_User LIMIT 1;
            WHEN 'VisualUserName' THEN SELECT VisualUserName INTO v_FieldValue FROM usr_users WHERE User = p_User LIMIT 1;
            WHEN 'VisualPassword' THEN SELECT VisualPassword INTO v_FieldValue FROM usr_users WHERE User = p_User LIMIT 1;
            WHEN 'WipServerAddress' THEN SELECT WipServerAddress INTO v_FieldValue FROM usr_users WHERE User = p_User LIMIT 1;
            WHEN 'WIPDatabase' THEN SELECT WIPDatabase INTO v_FieldValue FROM usr_users WHERE User = p_User LIMIT 1;
            WHEN 'WipServerPort' THEN SELECT WipServerPort INTO v_FieldValue FROM usr_users WHERE User = p_User LIMIT 1;
            WHEN 'FullName' THEN SELECT `Full Name` INTO v_FieldValue FROM usr_users WHERE User = p_User LIMIT 1;
            WHEN 'Shift' THEN SELECT Shift INTO v_FieldValue FROM usr_users WHERE User = p_User LIMIT 1;
            WHEN 'Pin' THEN SELECT Pin INTO v_FieldValue FROM usr_users WHERE User = p_User LIMIT 1;
            ELSE SET v_FieldValue = NULL;
        END CASE;
        
        SELECT v_FieldValue as FieldValue;
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Setting ', p_Field, ' retrieved successfully for user: ', p_User);
    END IF;
END $$
DELIMITER ;

-- Set specific user setting by user and field name (dynamic field updates)
DELIMITER $$
CREATE PROCEDURE usr_users_SetUserSetting_ByUserAndField(
    IN p_User VARCHAR(100), 
    IN p_Field VARCHAR(100), 
    IN p_Value TEXT,
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while setting ', p_Field, ' for user: ', p_User);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- Check if user exists, if not create basic user record
    SELECT COUNT(*) INTO v_Count FROM usr_users WHERE User = p_User;
    
    IF v_Count = 0 THEN
        -- Create basic user record with required fields
        INSERT INTO usr_users (User, `Full Name`) 
        VALUES (p_User, p_User);
    END IF;
    
    -- Use fixed field names instead of dynamic SQL for MySQL 5.7.24 compatibility
    CASE p_Field
        WHEN 'LastShownVersion' THEN 
            UPDATE usr_users SET LastShownVersion = p_Value, ModifiedDate = NOW() WHERE User = p_User;
        WHEN 'HideChangeLog' THEN 
            UPDATE usr_users SET HideChangeLog = p_Value, ModifiedDate = NOW() WHERE User = p_User;
        WHEN 'Theme_Name' THEN 
            UPDATE usr_users SET Theme_Name = p_Value, ModifiedDate = NOW() WHERE User = p_User;
        WHEN 'Theme_FontSize' THEN 
            UPDATE usr_users SET Theme_FontSize = CAST(p_Value AS UNSIGNED), ModifiedDate = NOW() WHERE User = p_User;
        WHEN 'VisualUserName' THEN 
            UPDATE usr_users SET VisualUserName = p_Value, ModifiedDate = NOW() WHERE User = p_User;
        WHEN 'VisualPassword' THEN 
            UPDATE usr_users SET VisualPassword = p_Value, ModifiedDate = NOW() WHERE User = p_User;
        WHEN 'WipServerAddress' THEN 
            UPDATE usr_users SET WipServerAddress = p_Value, ModifiedDate = NOW() WHERE User = p_User;
        WHEN 'WIPDatabase' THEN 
            UPDATE usr_users SET WIPDatabase = p_Value, ModifiedDate = NOW() WHERE User = p_User;
        WHEN 'WipServerPort' THEN 
            UPDATE usr_users SET WipServerPort = p_Value, ModifiedDate = NOW() WHERE User = p_User;
        WHEN 'FullName' THEN 
            UPDATE usr_users SET `Full Name` = p_Value WHERE User = p_User;
        WHEN 'Shift' THEN 
            UPDATE usr_users SET Shift = p_Value, ModifiedDate = NOW() WHERE User = p_User;
        WHEN 'Pin' THEN 
            UPDATE usr_users SET Pin = p_Value, ModifiedDate = NOW() WHERE User = p_User;
        ELSE 
            SET p_Status = 1;
            SET p_ErrorMsg = CONCAT('Unknown field: ', p_Field);
            ROLLBACK;
    END CASE;
    
    IF p_Status IS NULL THEN
        SET v_RowsAffected = ROW_COUNT();
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Setting ', p_Field, ' updated successfully for user: ', p_User, ' (Rows affected: ', v_RowsAffected, ')');
        COMMIT;
    END IF;
END $$
DELIMITER ;

-- ================================================================================
-- USER ROLE PROCEDURES
-- ================================================================================

-- Get user role ID by user ID with status reporting
DELIMITER $$
CREATE PROCEDURE usr_user_roles_GetRoleId_ByUserId(
    IN p_UserID INT,
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while retrieving role for user ID: ', p_UserID);
    END;
    
    SELECT COUNT(*) INTO v_Count FROM sys_user_roles WHERE UserID = p_UserID;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('No role assignment found for user ID: ', p_UserID);
        SELECT NULL as RoleID;
    ELSE
        SELECT RoleID FROM sys_user_roles WHERE UserID = p_UserID LIMIT 1;
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Role retrieved successfully for user ID: ', p_UserID);
    END IF;
END $$
DELIMITER ;

-- ================================================================================
-- USER CRUD PROCEDURES
-- ================================================================================

-- Add new user with all required fields (MySQL 5.7 Compatible)
DELIMITER $$
CREATE PROCEDURE usr_users_Add_User(
    IN p_User VARCHAR(100),
    IN p_FullName VARCHAR(200),
    IN p_Shift VARCHAR(50),
    IN p_VitsUser TINYINT(1), -- MySQL 5.7 compatible boolean
    IN p_Pin VARCHAR(20),
    IN p_LastShownVersion VARCHAR(20),
    IN p_HideChangeLog VARCHAR(10),
    IN p_Theme_Name VARCHAR(50),
    IN p_Theme_FontSize INT,
    IN p_VisualUserName VARCHAR(100),
    IN p_VisualPassword VARCHAR(100),
    IN p_WipServerAddress VARCHAR(100),
    IN p_WIPDatabase VARCHAR(100),
    IN p_WipServerPort VARCHAR(10),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while creating user: ', p_User);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- Check if user already exists
    SELECT COUNT(*) INTO v_Count FROM usr_users WHERE User = p_User;
    
    IF v_Count > 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('User already exists: ', p_User);
        ROLLBACK;
    ELSE
        INSERT INTO usr_users (
            User, `Full Name`, Shift, VitsUser, Pin,
            LastShownVersion, HideChangeLog, Theme_Name, Theme_FontSize,
            VisualUserName, VisualPassword, WipServerAddress, 
            WIPDatabase, WipServerPort
        ) VALUES (
            p_User, p_FullName, p_Shift, p_VitsUser, p_Pin,
            p_LastShownVersion, p_HideChangeLog, p_Theme_Name, p_Theme_FontSize,
            p_VisualUserName, p_VisualPassword, p_WipServerAddress,
            p_WIPDatabase, p_WipServerPort
        );
        
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('User created successfully: ', p_User);
        COMMIT;
    END IF;
END $$
DELIMITER ;

-- Update existing user
DELIMITER $$
CREATE PROCEDURE usr_users_Update_User(
    IN p_User VARCHAR(100),
    IN p_FullName VARCHAR(200),
    IN p_Shift VARCHAR(50),
    IN p_Pin VARCHAR(20),
    IN p_VisualUserName VARCHAR(100),
    IN p_VisualPassword VARCHAR(100),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while updating user: ', p_User);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- Check if user exists
    SELECT COUNT(*) INTO v_Count FROM usr_users WHERE User = p_User;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('User not found: ', p_User);
        ROLLBACK;
    ELSE
        UPDATE usr_users 
        SET `Full Name` = p_FullName,
            Shift = p_Shift,
            Pin = p_Pin,
            VisualUserName = p_VisualUserName,
            VisualPassword = p_VisualPassword
        WHERE User = p_User;
        
        SET v_RowsAffected = ROW_COUNT();
        
        IF v_RowsAffected > 0 THEN
            SET p_Status = 0;
            SET p_ErrorMsg = CONCAT('User updated successfully: ', p_User);
        ELSE
            SET p_Status = 2;
            SET p_ErrorMsg = CONCAT('No changes made to user: ', p_User);
        END IF;
        
        COMMIT;
    END IF;
END $$
DELIMITER ;

-- Delete user by username
DELIMITER $$
CREATE PROCEDURE usr_users_Delete_User(
    IN p_User VARCHAR(100),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while deleting user: ', p_User);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- Check if user exists
    SELECT COUNT(*) INTO v_Count FROM usr_users WHERE User = p_User;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('User not found: ', p_User);
        ROLLBACK;
    ELSE
        DELETE FROM usr_users WHERE User = p_User;
        SET v_RowsAffected = ROW_COUNT();
        
        IF v_RowsAffected > 0 THEN
            SET p_Status = 0;
            SET p_ErrorMsg = CONCAT('User deleted successfully: ', p_User);
        ELSE
            SET p_Status = 2;
            SET p_ErrorMsg = CONCAT('Failed to delete user: ', p_User);
        END IF;
        
        COMMIT;
    END IF;
END $$
DELIMITER ;

-- Get all users with status reporting
DELIMITER $$
CREATE PROCEDURE usr_users_Get_All(
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred while retrieving all users';
    END;
    
    SELECT COUNT(*) INTO v_Count FROM usr_users;
    SELECT * FROM usr_users ORDER BY `Full Name`;
    
    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('Retrieved ', v_Count, ' users successfully');
END $$
DELIMITER ;

-- Get user by username with status reporting
DELIMITER $$
CREATE PROCEDURE usr_users_Get_ByUser(
    IN p_User VARCHAR(100),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while retrieving user: ', p_User);
    END;
    
    SELECT COUNT(*) INTO v_Count FROM usr_users WHERE User = p_User;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('User not found: ', p_User);
        SELECT NULL as User, NULL as `Full Name`; -- Return empty result set with structure
    ELSE
        SELECT * FROM usr_users WHERE User = p_User LIMIT 1;
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('User retrieved successfully: ', p_User);
    END IF;
END $$
DELIMITER ;

-- Check if user exists with status reporting
DELIMITER $$
CREATE PROCEDURE usr_users_Exists(
    IN p_User VARCHAR(100),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while checking user existence: ', p_User);
    END;
    
    SELECT COUNT(*) INTO v_Count FROM usr_users WHERE User = p_User;
    SELECT v_Count as UserExists;
    
    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('User existence check completed for: ', p_User, ' (Exists: ', IF(v_Count > 0, 'Yes', 'No'), ')');
END $$
DELIMITER ;

-- ================================================================================
-- USER INTERFACE SETTINGS PROCEDURES (MySQL 5.7 Compatible with JSON)
-- ================================================================================

-- Set theme JSON settings (MySQL 5.7.8+ supports JSON)
DELIMITER $$
CREATE PROCEDURE usr_ui_settings_SetThemeJson(
    IN p_UserId VARCHAR(100),
    IN p_ThemeJson TEXT, -- Use TEXT for broader compatibility
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred while setting theme JSON';
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    INSERT INTO usr_ui_settings (UserId, SettingsJson, CreatedDate, ModifiedDate)
    VALUES (p_UserId, p_ThemeJson, NOW(), NOW())
    ON DUPLICATE KEY UPDATE 
        SettingsJson = p_ThemeJson,
        ModifiedDate = NOW();
    
    SET p_Status = 0;
    SET p_ErrorMsg = 'Theme JSON updated successfully';
    
    COMMIT;
END $$
DELIMITER ;

-- Set JSON setting for grid views 
DELIMITER $$
CREATE PROCEDURE usr_ui_settings_SetJsonSetting(
    IN p_UserId VARCHAR(100),
    IN p_DgvName VARCHAR(100),
    IN p_SettingJson TEXT, -- Use TEXT for broader compatibility
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred while setting JSON setting';
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    INSERT INTO usr_ui_settings (UserId, DgvName, SettingsJson, CreatedDate, ModifiedDate)
    VALUES (p_UserId, p_DgvName, p_SettingJson, NOW(), NOW())
    ON DUPLICATE KEY UPDATE 
        SettingsJson = p_SettingJson,
        ModifiedDate = NOW();
    
    SET p_Status = 0;
    SET p_ErrorMsg = 'JSON setting updated successfully';
    
    COMMIT;
END $$
DELIMITER ;

-- Get JSON setting for grid views with status reporting
DELIMITER $$
CREATE PROCEDURE usr_ui_settings_GetJsonSetting(
    IN p_UserId VARCHAR(100),
    OUT p_SettingJson TEXT, -- Use TEXT for broader compatibility
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred while getting JSON setting';
    END;
    
    SELECT COUNT(*) INTO v_Count FROM usr_ui_settings WHERE UserId = p_UserId;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('No settings found for user: ', p_UserId);
        SET p_SettingJson = NULL;
    ELSE
        SELECT SettingsJson INTO p_SettingJson
        FROM usr_ui_settings 
        WHERE UserId = p_UserId 
        LIMIT 1;
        
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('JSON setting retrieved successfully for user: ', p_UserId);
    END IF;
END $$
DELIMITER ;

-- Get shortcuts JSON with status reporting
DELIMITER $$
CREATE PROCEDURE usr_ui_settings_GetShortcutsJson(
    IN p_UserId VARCHAR(100),
    OUT p_ShortcutsJson TEXT, -- Use TEXT for broader compatibility
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred while getting shortcuts JSON';
    END;
    
    SELECT COUNT(*) INTO v_Count FROM usr_ui_settings WHERE UserId = p_UserId;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('No shortcuts found for user: ', p_UserId);
        SET p_ShortcutsJson = NULL;
    ELSE
        SELECT ShortcutsJson INTO p_ShortcutsJson
        FROM usr_ui_settings 
        WHERE UserId = p_UserId 
        LIMIT 1;
        
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Shortcuts JSON retrieved successfully for user: ', p_UserId);
    END IF;
END $$
DELIMITER ;

-- Set shortcuts JSON
DELIMITER $$
CREATE PROCEDURE usr_ui_settings_SetShortcutsJson(
    IN p_UserId VARCHAR(100),
    IN p_ShortcutsJson TEXT, -- Use TEXT for broader compatibility
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred while setting shortcuts JSON';
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    INSERT INTO usr_ui_settings (UserId, ShortcutsJson, CreatedDate, ModifiedDate)
    VALUES (p_UserId, p_ShortcutsJson, NOW(), NOW())
    ON DUPLICATE KEY UPDATE 
        ShortcutsJson = p_ShortcutsJson,
        ModifiedDate = NOW();
    
    SET p_Status = 0;
    SET p_ErrorMsg = 'Shortcuts JSON updated successfully';
    
    COMMIT;
END $$
DELIMITER ;

-- ================================================================================
-- END OF USER MANAGEMENT PROCEDURES
-- ================================================================================
