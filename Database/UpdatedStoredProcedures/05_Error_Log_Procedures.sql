-- ================================================================================
-- MTM INVENTORY APPLICATION - ERROR LOG STORED PROCEDURES
-- ================================================================================
-- File: 05_Error_Log_Procedures.sql
-- Purpose: Error logging and management for comprehensive application monitoring
-- Created: January 13, 2025
-- Updated: August 10, 2025 - UNIFORM PARAMETER NAMING (WITH p_ prefixes)
-- Target Database: mtm_wip_application_test
-- MySQL Version: 5.7.24+ (MAMP Compatible)
-- ================================================================================

-- Drop procedures if they exist (for clean deployment)
DROP PROCEDURE IF EXISTS log_error_Add_Error;
DROP PROCEDURE IF EXISTS log_error_Get_All;
DROP PROCEDURE IF EXISTS log_error_Get_ByUser;
DROP PROCEDURE IF EXISTS log_error_Get_ByDateRange;
DROP PROCEDURE IF EXISTS log_error_Get_Unique;
DROP PROCEDURE IF EXISTS log_error_Delete_ById;
DROP PROCEDURE IF EXISTS log_error_Delete_All;

-- ================================================================================
-- ERROR LOG MANAGEMENT PROCEDURES
-- ================================================================================

-- Log error to database with comprehensive error information
DELIMITER $$
CREATE PROCEDURE log_error_Add_Error(
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
        SET p_ErrorMsg = CONCAT('Database error occurred while logging error for user: ', p_User, ' - ', @text);
    END;

    START TRANSACTION;

    -- Input validation
    IF p_User IS NULL OR p_User = '' THEN
        SET p_User = 'Unknown';
    END IF;

    IF p_Severity IS NULL OR p_Severity = '' THEN
        SET p_Severity = 'Error';
    END IF;

    IF p_ErrorTime IS NULL THEN
        SET p_ErrorTime = NOW();
    END IF;

    -- Insert error log record
    INSERT INTO log_error (
        User, Severity, ErrorType, ErrorMessage, StackTrace, 
        ModuleName, MethodName, AdditionalInfo, MachineName, 
        OSVersion, AppVersion, ErrorTime
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
        SET p_ErrorMsg = CONCAT('Warning: Error log entry was not created for user: ', p_User);
    END IF;

    COMMIT;
END $$
DELIMITER ;

-- Get all error log entries
DELIMITER $$
CREATE PROCEDURE log_error_Get_All(
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred while retrieving all error logs';
    END;

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
END $$
DELIMITER ;

-- Get error log entries by user
DELIMITER $$
CREATE PROCEDURE log_error_Get_ByUser(
    IN p_User VARCHAR(100),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while retrieving error logs for user: ', p_User);
    END;

    -- Input validation
    IF p_User IS NULL OR p_User = '' THEN
        SET p_Status = 1;
        SET p_ErrorMsg = 'User parameter is required';
    ELSE
        SELECT COUNT(*) INTO v_Count FROM log_error WHERE User = p_User;
        
        SELECT * FROM log_error 
        WHERE User = p_User 
        ORDER BY ErrorTime DESC;

        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Retrieved ', v_Count, ' error log entries for user: ', p_User);
    END IF;
END $$
DELIMITER ;

-- Get error log entries by date range
DELIMITER $$
CREATE PROCEDURE log_error_Get_ByDateRange(
    IN p_StartDate DATETIME,
    IN p_EndDate DATETIME,
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while retrieving error logs for date range: ', 
                               DATE_FORMAT(p_StartDate, '%Y-%m-%d'), ' to ', DATE_FORMAT(p_EndDate, '%Y-%m-%d'));
    END;

    -- Input validation
    IF p_StartDate IS NULL OR p_EndDate IS NULL THEN
        SET p_Status = 1;
        SET p_ErrorMsg = 'Start date and end date parameters are required';
    ELSEIF p_StartDate > p_EndDate THEN
        SET p_Status = 1;
        SET p_ErrorMsg = 'Start date must be less than or equal to end date';
    ELSE
        SELECT COUNT(*) INTO v_Count FROM log_error 
        WHERE ErrorTime BETWEEN p_StartDate AND p_EndDate;
        
        SELECT * FROM log_error 
        WHERE ErrorTime BETWEEN p_StartDate AND p_EndDate 
        ORDER BY ErrorTime DESC;
        
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Retrieved ', v_Count, ' error log entries between ', 
                               DATE_FORMAT(p_StartDate, '%Y-%m-%d %H:%i:%s'), ' and ', 
                               DATE_FORMAT(p_EndDate, '%Y-%m-%d %H:%i:%s'));
    END IF;
END $$
DELIMITER ;

-- Get unique error combinations (method name + error message)
DELIMITER $$
CREATE PROCEDURE log_error_Get_Unique(
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred while retrieving unique error combinations';
    END;

    SELECT COUNT(DISTINCT CONCAT(IFNULL(MethodName, ''), '|', IFNULL(ErrorMessage, ''))) 
    INTO v_Count FROM log_error 
    WHERE MethodName IS NOT NULL AND ErrorMessage IS NOT NULL;

    SELECT DISTINCT MethodName, ErrorMessage 
    FROM log_error 
    WHERE MethodName IS NOT NULL AND ErrorMessage IS NOT NULL
    ORDER BY MethodName, ErrorMessage;

    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('Retrieved ', v_Count, ' unique error combinations successfully');
END $$
DELIMITER ;

-- Delete error log entry by ID
DELIMITER $$
CREATE PROCEDURE log_error_Delete_ById(
    IN p_Id INT,
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while deleting error log entry with ID: ', p_Id);
    END;

    START TRANSACTION;

    -- Input validation
    IF p_Id IS NULL OR p_Id <= 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = 'Valid ID parameter is required';
        ROLLBACK;
    ELSE
        -- Check if record exists
        SELECT COUNT(*) INTO v_Count FROM log_error WHERE ID = p_Id;
        
        IF v_Count = 0 THEN
            SET p_Status = 1;
            SET p_ErrorMsg = CONCAT('Error log entry with ID ', p_Id, ' not found');
            ROLLBACK;
        ELSE
            DELETE FROM log_error WHERE ID = p_Id;
            SET v_RowsAffected = ROW_COUNT();
            
            IF v_RowsAffected > 0 THEN
                SET p_Status = 0;
                SET p_ErrorMsg = CONCAT('Error log entry with ID ', p_Id, ' deleted successfully');
            ELSE
                SET p_Status = 2;
                SET p_ErrorMsg = CONCAT('Error log entry with ID ', p_Id, ' was not deleted');
            END IF;
            
            COMMIT;
        END IF;
    END IF;
END $$
DELIMITER ;

-- Delete all error log entries
DELIMITER $$
CREATE PROCEDURE log_error_Delete_All(
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred while deleting all error log entries';
    END;

    START TRANSACTION;

    -- Get count before deletion
    SELECT COUNT(*) INTO v_Count FROM log_error;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = 'No error log entries found to delete';
        ROLLBACK;
    ELSE
        DELETE FROM log_error;
        SET v_RowsAffected = ROW_COUNT();
        
        IF v_RowsAffected > 0 THEN
            SET p_Status = 0;
            SET p_ErrorMsg = CONCAT('Successfully deleted ', v_RowsAffected, ' error log entries');
        ELSE
            SET p_Status = 2;
            SET p_ErrorMsg = 'No error log entries were deleted';
        END IF;
        
        COMMIT;
    END IF;
END $$
DELIMITER ;

-- ================================================================================
-- END OF ERROR LOG PROCEDURES
-- ================================================================================
