# =============================================================================
# ERROR LOG STORED PROCEDURES - MySQL 5.7.24 + MAMP COMPATIBLE
# =============================================================================
# File: 05_Error_Log_Procedures.sql
# Purpose: Stored procedures for error logging and management
# Target Database: mtm_wip_application_test
# MySQL Version: 5.7.24+ (MAMP Compatible)
# Created: January 13, 2025
# Updated: August 10, 2025 - UNIFORM PARAMETER NAMING (WITH p_ prefixes)
# =============================================================================

USE mtm_wip_application;

-- =============================================================================
-- ERROR LOG TABLE CREATION (if not exists)
-- =============================================================================

CREATE TABLE IF NOT EXISTS `log_error` (
    `ID` int(11) NOT NULL AUTO_INCREMENT,
    `User` varchar(100) DEFAULT NULL,
    `Severity` varchar(50) DEFAULT NULL,
    `ErrorType` varchar(100) DEFAULT NULL,
    `ErrorMessage` text,
    `StackTrace` text,
    `ModuleName` varchar(100) DEFAULT NULL,
    `MethodName` varchar(100) DEFAULT NULL,
    `AdditionalInfo` text,
    `MachineName` varchar(100) DEFAULT NULL,
    `OSVersion` varchar(100) DEFAULT NULL,
    `AppVersion` varchar(50) DEFAULT NULL,
    `ErrorTime` datetime DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (`ID`),
    KEY `idx_user` (`User`),
    KEY `idx_error_time` (`ErrorTime`),
    KEY `idx_severity` (`Severity`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

DELIMITER $$

-- =============================================================================
-- 1. LOG ERROR TO DATABASE
-- =============================================================================

DROP PROCEDURE IF EXISTS `log_error_Add_Error`$$

CREATE PROCEDURE `log_error_Add_Error`(
    IN p_User VARCHAR(100),
    IN p_Severity VARCHAR(50),
    IN p_ErrorType VARCHAR(100),
    IN p_ErrorMessage TEXT,
    IN p_StackTrace TEXT,
    IN p_ModuleName VARCHAR(100),
    IN p_MethodName VARCHAR(100),
    IN p_AdditionalInfo TEXT,
    IN p_MachineName VARCHAR(100),
    IN p_OSVersion VARCHAR(100),
    IN p_AppVersion VARCHAR(50),
    IN p_ErrorTime DATETIME,
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        GET DIAGNOSTICS CONDITION 1
            @sqlstate = RETURNED_SQLSTATE, 
            @errno = MYSQL_ERRNO, 
            @text = MESSAGE_TEXT;
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error in log_error_Add_Error: ', @text);
    END;

    SET p_Status = 0;
    SET p_ErrorMsg = '';

    START TRANSACTION;

    -- Input validation
    IF p_User IS NULL OR p_User = '' THEN
        SET p_Status = 1;
        SET p_ErrorMsg = 'Warning: User parameter is empty, using default';
        SET p_User = 'Unknown';
    END IF;

    IF p_Severity IS NULL OR p_Severity = '' THEN
        SET p_Severity = 'Error';
    END IF;

    IF p_ErrorTime IS NULL THEN
        SET p_ErrorTime = NOW();
    END IF;

    -- Insert error log record
    INSERT INTO `log_error` (
        `User`, `Severity`, `ErrorType`, `ErrorMessage`, `StackTrace`, 
        `ModuleName`, `MethodName`, `AdditionalInfo`, `MachineName`, 
        `OSVersion`, `AppVersion`, `ErrorTime`
    ) VALUES (
        p_User, p_Severity, p_ErrorType, p_ErrorMessage, p_StackTrace,
        p_ModuleName, p_MethodName, p_AdditionalInfo, p_MachineName,
        p_OSVersion, p_AppVersion, p_ErrorTime
    );

    IF ROW_COUNT() > 0 THEN
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Error logged successfully for user: ', p_User);
    ELSE
        SET p_Status = 1;
        SET p_ErrorMsg = 'Warning: Error log entry was not created';
    END IF;

    COMMIT;
END$$

-- =============================================================================
-- 2. GET ALL ERRORS
-- =============================================================================

DROP PROCEDURE IF EXISTS `log_error_Get_All`$$

CREATE PROCEDURE `log_error_Get_All`(
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        GET DIAGNOSTICS CONDITION 1
            @sqlstate = RETURNED_SQLSTATE, 
            @errno = MYSQL_ERRNO, 
            @text = MESSAGE_TEXT;
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error in log_error_Get_All: ', @text);
    END;

    SET p_Status = 0;
    SET p_ErrorMsg = '';

    SELECT COUNT(*) INTO v_Count FROM log_error;
    
    SELECT 
        ID,
        User,
        ErrorMessage,
        StackTrace,
        MethodName,
        ErrorType,
        LoggedDate
    FROM log_error 
    ORDER BY LoggedDate DESC;

    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('Retrieved ', v_Count, ' error log entries successfully');
END$$

-- =============================================================================
-- 3. GET ERRORS BY USER
-- =============================================================================

DROP PROCEDURE IF EXISTS `log_error_Get_ByUser`$$

CREATE PROCEDURE `log_error_Get_ByUser`(
    IN p_User VARCHAR(100),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        GET DIAGNOSTICS CONDITION 1
            @sqlstate = RETURNED_SQLSTATE, 
            @errno = MYSQL_ERRNO, 
            @text = MESSAGE_TEXT;
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error in log_error_Get_ByUser: ', @text);
    END;

    SET p_Status = 0;
    SET p_ErrorMsg = '';

    -- Input validation
    IF p_User IS NULL OR p_User = '' THEN
        SET p_Status = -1;
        SET p_ErrorMsg = 'Error: User parameter is required';
    ELSE
        SELECT * FROM `log_error` 
        WHERE `User` = p_User 
        ORDER BY `ErrorTime` DESC;

        SELECT COUNT(*) INTO v_Count FROM `log_error` WHERE `User` = p_User;
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Retrieved ', v_Count, ' error log entries for user: ', p_User);
    END IF;
END$$

-- =============================================================================
-- 4. GET ERRORS BY DATE RANGE
-- =============================================================================

DROP PROCEDURE IF EXISTS `log_error_Get_ByDateRange`$$

CREATE PROCEDURE `log_error_Get_ByDateRange`(
    IN p_StartDate DATETIME,
    IN p_EndDate DATETIME,
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        GET DIAGNOSTICS CONDITION 1
            @sqlstate = RETURNED_SQLSTATE, 
            @errno = MYSQL_ERRNO, 
            @text = MESSAGE_TEXT;
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error in log_error_Get_ByDateRange: ', @text);
    END;

    SET p_Status = 0;
    SET p_ErrorMsg = '';

    -- Input validation
    IF p_StartDate IS NULL OR p_EndDate IS NULL THEN
        SET p_Status = -1;
        SET p_ErrorMsg = 'Error: Start date and end date parameters are required';
    ELSEIF p_StartDate > p_EndDate THEN
        SET p_Status = -1;
        SET p_ErrorMsg = 'Error: Start date must be less than or equal to end date';
    ELSE
        SELECT * FROM `log_error` 
        WHERE `ErrorTime` BETWEEN p_StartDate AND p_EndDate 
        ORDER BY `ErrorTime` DESC;

        SELECT COUNT(*) INTO v_Count FROM `log_error` 
        WHERE `ErrorTime` BETWEEN p_StartDate AND p_EndDate;
        
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Retrieved ', v_Count, ' error log entries between ', 
                               DATE_FORMAT(p_StartDate, '%Y-%m-%d %H:%i:%s'), ' and ', 
                               DATE_FORMAT(p_EndDate, '%Y-%m-%d %H:%i:%s'));
    END IF;
END$$

-- =============================================================================
-- 5. GET UNIQUE ERRORS (METHOD NAME + ERROR MESSAGE)
-- =============================================================================

DROP PROCEDURE IF EXISTS `log_error_Get_Unique`$$

CREATE PROCEDURE `log_error_Get_Unique`(
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        GET DIAGNOSTICS CONDITION 1
            @sqlstate = RETURNED_SQLSTATE, 
            @errno = MYSQL_ERRNO, 
            @text = MESSAGE_TEXT;
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error in log_error_Get_Unique: ', @text);
    END;

    SET p_Status = 0;
    SET p_ErrorMsg = '';

    SELECT DISTINCT `MethodName`, `ErrorMessage` 
    FROM `log_error` 
    WHERE `MethodName` IS NOT NULL AND `ErrorMessage` IS NOT NULL
    ORDER BY `MethodName`, `ErrorMessage`;

    SELECT COUNT(DISTINCT CONCAT(IFNULL(`MethodName`, ''), '|', IFNULL(`ErrorMessage`, ''))) 
    INTO v_Count FROM `log_error` 
    WHERE `MethodName` IS NOT NULL AND `ErrorMessage` IS NOT NULL;

    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('Retrieved ', v_Count, ' unique error combinations');
END$$

-- =============================================================================
-- 6. DELETE ERROR BY ID
-- =============================================================================

DROP PROCEDURE IF EXISTS `log_error_Delete_ById`$$

CREATE PROCEDURE `log_error_Delete_ById`(
    IN p_Id INT,
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        GET DIAGNOSTICS CONDITION 1
            @sqlstate = RETURNED_SQLSTATE, 
            @errno = MYSQL_ERRNO, 
            @text = MESSAGE_TEXT;
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error in log_error_Delete_ById: ', @text);
    END;

    SET p_Status = 0;
    SET p_ErrorMsg = '';

    START TRANSACTION;

    -- Input validation
    IF p_Id IS NULL OR p_Id <= 0 THEN
        SET p_Status = -1;
        SET p_ErrorMsg = 'Error: Valid ID parameter is required';
    ELSE
        -- Check if record exists
        SELECT COUNT(*) INTO v_Count FROM `log_error` WHERE `ID` = p_Id;
        
        IF v_Count = 0 THEN
            SET p_Status = 1;
            SET p_ErrorMsg = CONCAT('Warning: Error log entry with ID ', p_Id, ' not found');
        ELSE
            DELETE FROM `log_error` WHERE `ID` = p_Id;
            
            IF ROW_COUNT() > 0 THEN
                SET p_Status = 0;
                SET p_ErrorMsg = CONCAT('Error log entry with ID ', p_Id, ' deleted successfully');
            ELSE
                SET p_Status = 1;
                SET p_ErrorMsg = CONCAT('Warning: Error log entry with ID ', p_Id, ' was not deleted');
            END IF;
        END IF;
    END IF;

    COMMIT;
END$$

-- =============================================================================
-- 7. DELETE ALL ERRORS
-- =============================================================================

DROP PROCEDURE IF EXISTS `log_error_Delete_All`$$

CREATE PROCEDURE `log_error_Delete_All`(
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        GET DIAGNOSTICS CONDITION 1
            @sqlstate = RETURNED_SQLSTATE, 
            @errno = MYSQL_ERRNO, 
            @text = MESSAGE_TEXT;
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error in log_error_Delete_All: ', @text);
    END;

    SET p_Status = 0;
    SET p_ErrorMsg = '';

    START TRANSACTION;

    -- Get count before deletion
    SELECT COUNT(*) INTO v_Count FROM `log_error`;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = 'Warning: No error log entries found to delete';
    ELSE
        DELETE FROM `log_error`;
        
        IF ROW_COUNT() > 0 THEN
            SET p_Status = 0;
            SET p_ErrorMsg = CONCAT('Successfully deleted ', v_Count, ' error log entries');
        ELSE
            SET p_Status = 1;
            SET p_ErrorMsg = 'Warning: No error log entries were deleted';
        END IF;
    END IF;

    COMMIT;
END$$

DELIMITER ;

-- =============================================================================
-- VERIFICATION QUERIES
-- =============================================================================

-- Show all created procedures
SELECT ROUTINE_NAME as 'Error Log Procedures Created' 
FROM INFORMATION_SCHEMA.ROUTINES 
WHERE ROUTINE_SCHEMA = 'mtm_wip_application' 
  AND ROUTINE_NAME LIKE 'log_error_%'
ORDER BY ROUTINE_NAME;
