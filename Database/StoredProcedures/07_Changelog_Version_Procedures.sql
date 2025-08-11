-- ================================================================================
-- MTM INVENTORY APPLICATION - CHANGELOG AND VERSION STORED PROCEDURES
-- ================================================================================
-- File: 07_Changelog_Version_Procedures.sql
-- Purpose: Application version tracking and changelog management
-- Created: August 10, 2025
-- Updated: August 10, 2025 - UNIFORM PARAMETER NAMING (WITH p_ prefixes) - MySQL 5.7.24 COMPATIBLE
-- Target Database: mtm_wip_application
-- MySQL Version: 5.7.24+ (MAMP Compatible)
-- ================================================================================

-- Drop procedures if they exist (for clean deployment)
DROP PROCEDURE IF EXISTS log_changelog_Get_Current;
DROP PROCEDURE IF EXISTS log_changelog_Get_All;
DROP PROCEDURE IF EXISTS log_changelog_Add_Entry;
DROP PROCEDURE IF EXISTS log_changelog_Initialize_Default_Data;

-- Create table if it doesn't exist (MySQL 5.7.24 Compatible)
CREATE TABLE IF NOT EXISTS log_changelog (
    ID INT AUTO_INCREMENT PRIMARY KEY,
    Version VARCHAR(50) NOT NULL,
    Description TEXT,
    ReleaseDate DATE,
    CreatedBy VARCHAR(100),
    CreatedDate DATETIME DEFAULT NOW(),
    ModifiedDate DATETIME DEFAULT NOW() ON UPDATE NOW(),
    INDEX idx_version (Version),
    INDEX idx_release_date (ReleaseDate)
);

-- ================================================================================
-- CHANGELOG AND VERSION PROCEDURES
-- ================================================================================

-- Get current version information with status reporting (by highest version number)
DELIMITER $$
CREATE PROCEDURE log_changelog_Get_Current(
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred while retrieving current version information';
    END;
    
    -- Check if changelog table has any entries
    SELECT COUNT(*) INTO v_Count FROM log_changelog;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = 'No version information found in changelog';
        SELECT 
            'Unknown' as Version,
            'No changelog entries found' as Description,
            NULL as ReleaseDate,
            'System' as CreatedBy,
            NOW() as CreatedDate;
    ELSE
        -- Get the highest version number (semantic version ordering)
        -- This query sorts versions properly handling semantic versioning (e.g., 1.0.0, 1.0.1, 1.1.0, 2.0.0)
        SELECT 
            Version,
            Description,
            ReleaseDate,
            CreatedBy,
            CreatedDate
        FROM log_changelog 
        ORDER BY 
            -- Convert version to sortable format for proper semantic version comparison
            CAST(SUBSTRING_INDEX(Version, '.', 1) AS UNSIGNED) DESC,
            CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(Version, '.', 2), '.', -1) AS UNSIGNED) DESC,
            CAST(SUBSTRING_INDEX(Version, '.', -1) AS UNSIGNED) DESC,
            -- Fallback to string comparison for non-standard version formats
            Version DESC,
            -- Use CreatedDate as final tiebreaker
            CreatedDate DESC
        LIMIT 1;
        
        SET p_Status = 0;
        SET p_ErrorMsg = 'Current version (highest version number) retrieved successfully';
    END IF;
END $$
DELIMITER ;

-- Get all changelog entries with status reporting (ordered by highest version first)
DELIMITER $$
CREATE PROCEDURE log_changelog_Get_All(
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred while retrieving changelog entries';
    END;
    
    SELECT COUNT(*) INTO v_Count FROM log_changelog;
    
    SELECT 
        ID,
        Version,
        Description,
        ReleaseDate,
        CreatedBy,
        CreatedDate,
        ModifiedDate
    FROM log_changelog 
    ORDER BY 
        -- Sort by highest version number first (semantic version ordering)
        CAST(SUBSTRING_INDEX(Version, '.', 1) AS UNSIGNED) DESC,
        CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(Version, '.', 2), '.', -1) AS UNSIGNED) DESC,
        CAST(SUBSTRING_INDEX(Version, '.', -1) AS UNSIGNED) DESC,
        -- Fallback to string comparison for non-standard version formats
        Version DESC,
        -- Use CreatedDate as final tiebreaker
        CreatedDate DESC;
    
    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('Retrieved ', v_Count, ' changelog entries successfully (ordered by highest version)');
END $$
DELIMITER ;

-- Add new changelog entry with status reporting
DELIMITER $$
CREATE PROCEDURE log_changelog_Add_Entry(
    IN p_Version VARCHAR(50),
    IN p_Description TEXT,
    IN p_ReleaseDate DATE,
    IN p_CreatedBy VARCHAR(100),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while adding changelog entry for version: ', p_Version);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- Check if this version already exists
    SELECT COUNT(*) INTO v_Count FROM log_changelog WHERE Version = p_Version;
    
    IF v_Count > 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Version already exists in changelog: ', p_Version);
        ROLLBACK;
    ELSE
        INSERT INTO log_changelog (
            Version,
            Description,
            ReleaseDate,
            CreatedBy,
            CreatedDate
        ) VALUES (
            p_Version,
            p_Description,
            p_ReleaseDate,
            p_CreatedBy,
            NOW()
        );
        
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Changelog entry added successfully for version: ', p_Version);
        COMMIT;
    END IF;
END $$
DELIMITER ;

-- Initialize default data if table is empty (MySQL 5.7.24 Compatible)
DELIMITER $$
CREATE PROCEDURE log_changelog_Initialize_Default_Data(
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred while initializing default changelog data';
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- Check if table is empty
    SELECT COUNT(*) INTO v_Count FROM log_changelog;
    
    IF v_Count = 0 THEN
        -- Insert multiple sample versions to demonstrate version ordering
        INSERT INTO log_changelog (Version, Description, ReleaseDate, CreatedBy, CreatedDate) VALUES
        ('1.0.0', 'MTM Inventory Application - Initial Release with uniform parameter naming system, MySQL 5.7.24 compatibility, and comprehensive stored procedure architecture.', CURDATE(), 'SYSTEM', NOW()),
        ('1.0.1', 'Bug fixes and performance improvements for version checking system.', CURDATE(), 'SYSTEM', NOW()),
        ('1.1.0', 'Added enhanced error handling and improved user interface feedback.', CURDATE(), 'SYSTEM', NOW());
        
        SET p_Status = 0;
        SET p_ErrorMsg = 'Default changelog data initialized successfully with sample versions';
    ELSE
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Changelog table already contains ', v_Count, ' entries - skipping initialization');
    END IF;
    
    COMMIT;
END $$
DELIMITER ;

-- ================================================================================
-- END OF CHANGELOG AND VERSION PROCEDURES
-- ================================================================================
