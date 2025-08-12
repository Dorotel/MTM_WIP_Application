-- ================================================================================
-- MTM INVENTORY APPLICATION - THEME MANAGEMENT STORED PROCEDURES
-- ================================================================================
-- File: 08_Theme_Management_Procedures.sql
-- Purpose: Application theme management and user interface customization procedures
-- Created: August 10, 2025
-- Updated: August 10, 2025 - UNIFORM PARAMETER NAMING (WITH p_ prefixes)
-- Target Database: mtm_wip_application
-- MySQL Version: 5.7.24+ (MAMP Compatible)
-- ================================================================================

-- Drop procedures if they exist (for clean deployment)
DROP PROCEDURE IF EXISTS app_themes_Get_All;
DROP PROCEDURE IF EXISTS app_themes_Get_ByName;
DROP PROCEDURE IF EXISTS app_themes_Add_Theme;
DROP PROCEDURE IF EXISTS app_themes_Update_Theme;
DROP PROCEDURE IF EXISTS app_themes_Delete_Theme;
DROP PROCEDURE IF EXISTS app_themes_Exists;
DROP PROCEDURE IF EXISTS app_themes_Get_UserTheme;
DROP PROCEDURE IF EXISTS app_themes_Set_UserTheme;

-- Drop tables if they exist (for clean deployment)
DROP TABLE IF EXISTS app_themes;

-- ================================================================================
-- CREATE THEME TABLES
-- ================================================================================

-- Application themes table for storing theme definitions
CREATE TABLE IF NOT EXISTS app_themes (
    ID INT AUTO_INCREMENT PRIMARY KEY,
    ThemeName VARCHAR(50) NOT NULL UNIQUE,
    DisplayName VARCHAR(100) NOT NULL,
    SettingsJson TEXT NOT NULL, -- JSON containing theme color definitions
    IsDefault TINYINT(1) DEFAULT 0, -- Whether this is a default system theme
    IsActive TINYINT(1) DEFAULT 1, -- Whether this theme is available for use
    CreatedDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CreatedBy VARCHAR(100) DEFAULT 'SYSTEM',
    ModifiedDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    ModifiedBy VARCHAR(100) DEFAULT 'SYSTEM',
    Description TEXT NULL, -- Optional description of the theme
    VERSION INT DEFAULT 1, -- Theme version for future updates
    INDEX idx_theme_name (ThemeName),
    INDEX idx_active (IsActive),
    INDEX idx_default (IsDefault)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- ================================================================================
-- INSERT DEFAULT THEMES
-- ================================================================================

-- Insert Default Light Theme
INSERT IGNORE INTO app_themes (ThemeName, DisplayName, SettingsJson, IsDefault, IsActive, Description, CreatedBy) VALUES (
    'Default',
    'Default Light Theme',
    '{
        "FormBackColor": "#FFFFFF",
        "FormForeColor": "#000000",
        "ControlBackColor": "#FFFFFF",
        "ControlForeColor": "#000000",
        "ButtonBackColor": "#F0F0F0",
        "ButtonForeColor": "#000000",
        "ButtonHoverBackColor": "#E0E0E0",
        "ButtonPressedBackColor": "#D0D0D0",
        "TextBoxBackColor": "#FFFFFF",
        "TextBoxForeColor": "#000000",
        "ComboBoxBackColor": "#FFFFFF",
        "ComboBoxForeColor": "#000000",
        "ComboBoxErrorForeColor": "#FF0000",
        "DataGridBackColor": "#FFFFFF",
        "DataGridForeColor": "#000000",
        "DataGridHeaderBackColor": "#F0F0F0",
        "DataGridHeaderForeColor": "#000000",
        "DataGridRowBackColor": "#FFFFFF",
        "DataGridAltRowBackColor": "#F0F8FF",
        "DataGridSelectionBackColor": "#316AC5",
        "DataGridSelectionForeColor": "#FFFFFF"
    }',
    1,
    1,
    'Default light theme with standard Windows colors',
    'SYSTEM'
);

-- Insert Dark Theme
INSERT IGNORE INTO app_themes (ThemeName, DisplayName, SettingsJson, IsDefault, IsActive, Description, CreatedBy) VALUES (
    'Dark',
    'Dark Theme',
    '{
        "FormBackColor": "#2D2D30",
        "FormForeColor": "#FFFFFF",
        "ControlBackColor": "#2D2D30",
        "ControlForeColor": "#FFFFFF",
        "ButtonBackColor": "#3C3C3C",
        "ButtonForeColor": "#FFFFFF",
        "ButtonHoverBackColor": "#505050",
        "ButtonPressedBackColor": "#282828",
        "TextBoxBackColor": "#1E1E1E",
        "TextBoxForeColor": "#FFFFFF",
        "ComboBoxBackColor": "#1E1E1E",
        "ComboBoxForeColor": "#FFFFFF",
        "ComboBoxErrorForeColor": "#FF6464",
        "DataGridBackColor": "#2D2D30",
        "DataGridForeColor": "#FFFFFF",
        "DataGridHeaderBackColor": "#3C3C3C",
        "DataGridHeaderForeColor": "#FFFFFF",
        "DataGridRowBackColor": "#2D2D30",
        "DataGridAltRowBackColor": "#37373A",
        "DataGridSelectionBackColor": "#3399FF",
        "DataGridSelectionForeColor": "#FFFFFF"
    }',
    1,
    1,
    'Professional dark theme for low-light environments',
    'SYSTEM'
);

-- Insert Blue Theme
INSERT IGNORE INTO app_themes (ThemeName, DisplayName, SettingsJson, IsDefault, IsActive, Description, CreatedBy) VALUES (
    'Blue',
    'Blue Professional Theme',
    '{
        "FormBackColor": "#F0F8FF",
        "FormForeColor": "#191919",
        "ControlBackColor": "#F0F8FF",
        "ControlForeColor": "#191919",
        "ButtonBackColor": "#4682B4",
        "ButtonForeColor": "#FFFFFF",
        "ButtonHoverBackColor": "#6495ED",
        "ButtonPressedBackColor": "#1E5E8C",
        "TextBoxBackColor": "#FFFFFF",
        "TextBoxForeColor": "#000000",
        "ComboBoxBackColor": "#FFFFFF",
        "ComboBoxForeColor": "#000000",
        "ComboBoxErrorForeColor": "#FF0000",
        "DataGridBackColor": "#FFFFFF",
        "DataGridForeColor": "#000000",
        "DataGridHeaderBackColor": "#4682B4",
        "DataGridHeaderForeColor": "#FFFFFF",
        "DataGridRowBackColor": "#FFFFFF",
        "DataGridAltRowBackColor": "#E6F0FF",
        "DataGridSelectionBackColor": "#4682B4",
        "DataGridSelectionForeColor": "#FFFFFF"
    }',
    1,
    1,
    'Professional blue theme with steel blue accents',
    'SYSTEM'
);

-- ================================================================================
-- THEME MANAGEMENT PROCEDURES
-- ================================================================================

-- Get all themes from app_themes table (direct table query as stored procedure)
DELIMITER $$
CREATE PROCEDURE app_themes_Get_All(
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred while retrieving themes';
    END;
    
    -- Simply return all rows from app_themes table (like the old Helper_Database_Core.ExecuteDataTable approach)
    SELECT * FROM app_themes WHERE IsActive = 1 ORDER BY ThemeName;
    
    SET p_Status = 0;
    SET p_ErrorMsg = 'Themes retrieved successfully';
END $$
DELIMITER ;

-- Get specific theme by name with status reporting
DELIMITER $$
CREATE PROCEDURE app_themes_Get_ByName(
    IN p_ThemeName VARCHAR(50),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while retrieving theme: ', p_ThemeName);
    END;
    
    SELECT COUNT(*) INTO v_Count FROM app_themes WHERE ThemeName = p_ThemeName AND IsActive = 1;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Theme not found or inactive: ', p_ThemeName);
        -- Return empty result set with structure
        SELECT NULL as ThemeName, NULL as SettingsJson LIMIT 0;
    ELSE
        SELECT 
            ID,
            ThemeName,
            DisplayName,
            SettingsJson,
            IsDefault,
            IsActive,
            Description,
            CreatedDate,
            CreatedBy,
            ModifiedDate,
            ModifiedBy,
            VERSION
        FROM app_themes 
        WHERE ThemeName = p_ThemeName AND IsActive = 1
        LIMIT 1;
        
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Theme retrieved successfully: ', p_ThemeName);
    END IF;
END $$
DELIMITER ;

-- Add new theme with validation
DELIMITER $$
CREATE PROCEDURE app_themes_Add_Theme(
    IN p_ThemeName VARCHAR(50),
    IN p_DisplayName VARCHAR(100),
    IN p_SettingsJson TEXT,
    IN p_Description TEXT,
    IN p_CreatedBy VARCHAR(100),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_ThemeId INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while creating theme: ', p_ThemeName);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- Validate theme name is not empty
    IF p_ThemeName IS NULL OR TRIM(p_ThemeName) = '' THEN
        SET p_Status = 1;
        SET p_ErrorMsg = 'Theme name cannot be empty';
        ROLLBACK;
    -- Check if theme already exists
    ELSEIF EXISTS(SELECT 1 FROM app_themes WHERE ThemeName = p_ThemeName) THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Theme already exists: ', p_ThemeName);
        ROLLBACK;
    -- Validate JSON format (basic check)
    ELSEIF p_SettingsJson IS NULL OR TRIM(p_SettingsJson) = '' THEN
        SET p_Status = 1;
        SET p_ErrorMsg = 'Theme settings JSON cannot be empty';
        ROLLBACK;
    ELSE
        INSERT INTO app_themes (
            ThemeName,
            DisplayName,
            SettingsJson,
            IsDefault,
            IsActive,
            Description,
            CreatedBy,
            CreatedDate
        ) VALUES (
            p_ThemeName,
            IFNULL(p_DisplayName, p_ThemeName),
            p_SettingsJson,
            0, -- New themes are never default
            1, -- New themes are active by default
            p_Description,
            IFNULL(p_CreatedBy, 'SYSTEM'),
            NOW()
        );
        
        SET v_ThemeId = LAST_INSERT_ID();
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Theme created successfully: ', p_ThemeName, ' (ID: ', v_ThemeId, ')');
        COMMIT;
    END IF;
END $$
DELIMITER ;

-- Update existing theme
DELIMITER $$
CREATE PROCEDURE app_themes_Update_Theme(
    IN p_ThemeName VARCHAR(50),
    IN p_DisplayName VARCHAR(100),
    IN p_SettingsJson TEXT,
    IN p_Description TEXT,
    IN p_ModifiedBy VARCHAR(100),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while updating theme: ', p_ThemeName);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- Check if theme exists and is not a default system theme
    SELECT COUNT(*) INTO v_Count FROM app_themes WHERE ThemeName = p_ThemeName AND IsDefault = 0;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Theme not found or is a protected system theme: ', p_ThemeName);
        ROLLBACK;
    ELSE
        UPDATE app_themes 
        SET DisplayName = IFNULL(p_DisplayName, DisplayName),
            SettingsJson = IFNULL(p_SettingsJson, SettingsJson),
            Description = p_Description,
            ModifiedBy = IFNULL(p_ModifiedBy, 'SYSTEM'),
            ModifiedDate = NOW(),
            VERSION = VERSION + 1
        WHERE ThemeName = p_ThemeName AND IsDefault = 0;
        
        SET v_RowsAffected = ROW_COUNT();
        
        IF v_RowsAffected > 0 THEN
            SET p_Status = 0;
            SET p_ErrorMsg = CONCAT('Theme updated successfully: ', p_ThemeName);
        ELSE
            SET p_Status = 2;
            SET p_ErrorMsg = CONCAT('No changes made to theme: ', p_ThemeName);
        END IF;
        
        COMMIT;
    END IF;
END $$
DELIMITER ;

-- Delete theme (soft delete by setting IsActive = 0)
DELIMITER $$
CREATE PROCEDURE app_themes_Delete_Theme(
    IN p_ThemeName VARCHAR(50),
    IN p_ModifiedBy VARCHAR(100),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while deleting theme: ', p_ThemeName);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- Check if theme exists and is not a default system theme
    SELECT COUNT(*) INTO v_Count FROM app_themes WHERE ThemeName = p_ThemeName AND IsDefault = 0 AND IsActive = 1;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Theme not found, already deleted, or is a protected system theme: ', p_ThemeName);
        ROLLBACK;
    ELSE
        -- Soft delete by setting IsActive = 0
        UPDATE app_themes 
        SET IsActive = 0,
            ModifiedBy = IFNULL(p_ModifiedBy, 'SYSTEM'),
            ModifiedDate = NOW()
        WHERE ThemeName = p_ThemeName AND IsDefault = 0;
        
        SET v_RowsAffected = ROW_COUNT();
        
        IF v_RowsAffected > 0 THEN
            SET p_Status = 0;
            SET p_ErrorMsg = CONCAT('Theme deleted successfully: ', p_ThemeName);
        ELSE
            SET p_Status = 2;
            SET p_ErrorMsg = CONCAT('Failed to delete theme: ', p_ThemeName);
        END IF;
        
        COMMIT;
    END IF;
END $$
DELIMITER ;

-- Check if theme exists
DELIMITER $$
CREATE PROCEDURE app_themes_Exists(
    IN p_ThemeName VARCHAR(50),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while checking theme existence: ', p_ThemeName);
    END;
    
    SELECT COUNT(*) INTO v_Count FROM app_themes WHERE ThemeName = p_ThemeName AND IsActive = 1;
    SELECT v_Count as ThemeExists;
    
    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('Theme existence check completed for: ', p_ThemeName, ' (Exists: ', IF(v_Count > 0, 'Yes', 'No'), ')');
END $$
DELIMITER ;

-- ================================================================================
-- USER THEME PREFERENCE PROCEDURES
-- ================================================================================

-- Get user's selected theme
DELIMITER $$
CREATE PROCEDURE app_themes_Get_UserTheme(
    IN p_UserId VARCHAR(100),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_ThemeName VARCHAR(50) DEFAULT NULL;
    DECLARE v_ThemeExists INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while retrieving user theme for: ', p_UserId);
    END;
    
    -- Get user's theme preference from usr_users table
    SELECT Theme_Name INTO v_ThemeName 
    FROM usr_users 
    WHERE User = p_UserId 
    LIMIT 1;
    
    -- If no theme set or theme doesn't exist, default to 'Default' theme
    IF v_ThemeName IS NULL OR v_ThemeName = '' THEN
        SET v_ThemeName = 'Default';
    ELSE
        -- Check if the user's preferred theme actually exists and is active
        SELECT COUNT(*) INTO v_ThemeExists 
        FROM app_themes 
        WHERE ThemeName = v_ThemeName AND IsActive = 1;
        
        -- If theme doesn't exist, fall back to Default
        IF v_ThemeExists = 0 THEN
            SET v_ThemeName = 'Default';
        END IF;
    END IF;
    
    -- Return the theme data
    SELECT 
        t.ID,
        t.ThemeName,
        t.DisplayName,
        t.SettingsJson,
        t.IsDefault,
        t.Description
    FROM app_themes t
    WHERE t.ThemeName = v_ThemeName AND t.IsActive = 1
    LIMIT 1;
    
    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('User theme retrieved successfully for: ', p_UserId, ' (Theme: ', v_ThemeName, ')');
END $$
DELIMITER ;

-- Set user's theme preference
DELIMITER $$
CREATE PROCEDURE app_themes_Set_UserTheme(
    IN p_UserId VARCHAR(100),
    IN p_ThemeName VARCHAR(50),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_UserExists INT DEFAULT 0;
    DECLARE v_ThemeExists INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while setting theme for user: ', p_UserId);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- Check if user exists
    SELECT COUNT(*) INTO v_UserExists FROM usr_users WHERE User = p_UserId;
    
    IF v_UserExists = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('User not found: ', p_UserId);
        ROLLBACK;
    ELSE
        -- Check if theme exists and is active
        SELECT COUNT(*) INTO v_ThemeExists FROM app_themes WHERE ThemeName = p_ThemeName AND IsActive = 1;
        
        IF v_ThemeExists = 0 THEN
            SET p_Status = 1;
            SET p_ErrorMsg = CONCAT('Theme not found or inactive: ', p_ThemeName);
            ROLLBACK;
        ELSE
            -- Update user's theme preference
            UPDATE usr_users 
            SET Theme_Name = p_ThemeName,
                ModifiedDate = NOW()
            WHERE User = p_UserId;
            
            SET v_RowsAffected = ROW_COUNT();
            
            IF v_RowsAffected > 0 THEN
                SET p_Status = 0;
                SET p_ErrorMsg = CONCAT('Theme set successfully for user: ', p_UserId, ' (Theme: ', p_ThemeName, ')');
            ELSE
                SET p_Status = 2;
                SET p_ErrorMsg = CONCAT('No changes made for user: ', p_UserId);
            END IF;
            
            COMMIT;
        END IF;
    END IF;
END $$
DELIMITER ;

-- ================================================================================
-- END OF THEME MANAGEMENT PROCEDURES
-- ================================================================================
