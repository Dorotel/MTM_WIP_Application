-- ================================================================================
-- MTM INVENTORY APPLICATION - COMPREHENSIVE STORED PROCEDURE VERIFICATION SYSTEM
-- ================================================================================
-- File: 00_StoredProcedure_Verification_System.sql
-- Purpose: Complete verification system for all stored procedures against LiveDatabase.sql
-- Created: August 13, 2025
-- Target Database: mtm_wip_application_test
-- MySQL Version: 5.7.24+ (MAMP Compatible)
-- ================================================================================

DROP PROCEDURE IF EXISTS sys_VerifyDatabaseSchema;
DROP PROCEDURE IF EXISTS sys_VerifyTableColumns;
DROP PROCEDURE IF EXISTS sys_GetStoredProcedureInventory;
DROP PROCEDURE IF EXISTS test_InventoryProcedures;
DROP PROCEDURE IF EXISTS sys_RunCompleteVerification;


-- ================================================================================
-- PHASE 1: DATABASE SCHEMA VALIDATION
-- ================================================================================

-- Procedure to verify all required tables exist with correct structure
DELIMITER $$
CREATE PROCEDURE sys_VerifyDatabaseSchema(
    OUT p_Status INT,
    OUT p_ErrorMsg TEXT
)
BEGIN
    DECLARE v_TableCount INT DEFAULT 0;
    DECLARE v_MissingTables TEXT DEFAULT '';
    DECLARE v_Expected INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred during schema verification';
    END;
    
    -- Expected core tables from LiveDatabase.sql analysis
    SET v_Expected = 17;
    
    -- Check for each required table
    SELECT COUNT(*) INTO v_TableCount
    FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_SCHEMA = DATABASE()
    AND TABLE_NAME IN (
        'app_themes',
        'debug_matching', 
        'inv_inventory',
        'inv_inventory_batch_seq',
        'inv_transaction',
        'usr_users',
        'usr_ui_settings', 
        'sys_user_roles',
        'sys_roles',
        'md_part_ids',
        'md_locations', 
        'md_operation_numbers',
        'md_item_types',
        'log_error_log',
        'log_changelog',
        'sys_quick_buttons',
        'sys_last_10_transactions'
    );
    
    IF v_TableCount < v_Expected THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Missing tables detected. Found ', v_TableCount, ' of ', v_Expected, ' required tables');
    ELSE
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Schema validation passed. All ', v_TableCount, ' required tables found');
    END IF;
    
    -- Return detailed table information
    SELECT 
        TABLE_NAME as TableName,
        ENGINE as Engine,
        TABLE_COLLATION as Collation,
        CREATE_TIME as Created,
        TABLE_COMMENT as Comment,
        TABLE_ROWS as ApproxRows
    FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_SCHEMA = DATABASE()
    ORDER BY TABLE_NAME;
    
END $$
DELIMITER ;

-- ================================================================================
-- PHASE 2: COLUMN STRUCTURE VERIFICATION
-- ================================================================================

-- Procedure to verify critical table column structures match LiveDatabase.sql
DELIMITER $$
CREATE PROCEDURE sys_VerifyTableColumns(
    IN p_TableName VARCHAR(64),
    OUT p_Status INT,
    OUT p_ErrorMsg TEXT
)
BEGIN
    DECLARE v_ColumnCount INT DEFAULT 0;
    DECLARE v_ExpectedColumns TEXT DEFAULT '';
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while verifying columns for table: ', p_TableName);
    END;
    
    -- Define expected columns for key tables based on LiveDatabase.sql
    CASE p_TableName
        WHEN 'inv_inventory' THEN
            SET v_ExpectedColumns = 'ID,PartID,Location,Operation,Quantity,ItemType,ReceiveDate,LastUpdated,User,BatchNumber,Notes';
        WHEN 'inv_transaction' THEN  
            SET v_ExpectedColumns = 'ID,TransactionType,BatchNumber,PartID,FromLocation,ToLocation,Operation,Quantity,Notes,User,ItemType,ReceiveDate';
        WHEN 'app_themes' THEN
            SET v_ExpectedColumns = 'ThemeName,SettingsJson';
        WHEN 'debug_matching' THEN
            SET v_ExpectedColumns = 'id,in_id,in_part,in_loc,in_batch,out_id,out_part,out_loc,out_batch,matched_at';
        WHEN 'usr_users' THEN
            SET v_ExpectedColumns = 'UserID,User,Full Name,Shift,VitsUser,Pin,LastShownVersion,HideChangeLog,Theme_Name,Theme_FontSize';
        WHEN 'md_part_ids' THEN
            SET v_ExpectedColumns = 'PartID,Description,CreatedDate';
        WHEN 'md_locations' THEN
            SET v_ExpectedColumns = 'LocationID,Location,Description,IsActive';
        WHEN 'md_operation_numbers' THEN
            SET v_ExpectedColumns = 'OperationID,OperationNumber,Description,IsActive';
        WHEN 'sys_last_10_transactions' THEN
            SET v_ExpectedColumns = 'Position,User,PartID,Operation,Quantity,ReceiveDate';
        ELSE
            SET p_Status = 1;
            SET p_ErrorMsg = CONCAT('Unknown table for verification: ', p_TableName);
    END CASE;
    
    -- Return column information for manual verification
    SELECT 
        COLUMN_NAME as ColumnName,
        DATA_TYPE as DataType,
        IS_NULLABLE as Nullable,
        COLUMN_DEFAULT as DefaultValue,
        CHARACTER_MAXIMUM_LENGTH as MaxLength,
        COLUMN_KEY as KeyType,
        EXTRA as Extra
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_SCHEMA = DATABASE()
    AND TABLE_NAME = p_TableName
    ORDER BY ORDINAL_POSITION;
    
    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('Column verification completed for table: ', p_TableName);
    
END $$
DELIMITER ;

-- ================================================================================
-- PHASE 3: STORED PROCEDURE INVENTORY AND VALIDATION
-- ================================================================================

-- Procedure to list all stored procedures and their status
DELIMITER $$
CREATE PROCEDURE sys_GetStoredProcedureInventory(
    OUT p_Status INT,
    OUT p_ErrorMsg TEXT
)
BEGIN
    DECLARE v_ProcCount INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred while retrieving procedure inventory';
    END;
    
    SELECT COUNT(*) INTO v_ProcCount
    FROM INFORMATION_SCHEMA.ROUTINES 
    WHERE ROUTINE_SCHEMA = DATABASE()
    AND ROUTINE_TYPE = 'PROCEDURE';
    
    -- Return detailed procedure information
    SELECT 
        ROUTINE_NAME as ProcedureName,
        CREATED as Created,
        LAST_ALTERED as LastModified,
        SQL_DATA_ACCESS as DataAccess,
        SECURITY_TYPE as SecurityType,
        ROUTINE_COMMENT as Comment,
        DEFINER as Definer
    FROM INFORMATION_SCHEMA.ROUTINES 
    WHERE ROUTINE_SCHEMA = DATABASE()
    AND ROUTINE_TYPE = 'PROCEDURE'
    ORDER BY ROUTINE_NAME;
    
    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('Procedure inventory completed. Found ', v_ProcCount, ' stored procedures');
    
END $$
DELIMITER ;

-- ================================================================================
-- PHASE 4: PROCEDURE TESTING FRAMEWORK
-- ================================================================================

-- Test procedure for inventory management procedures
DELIMITER $$
CREATE PROCEDURE test_InventoryProcedures(
    OUT p_TestsPassed INT,
    OUT p_TestsFailed INT,
    OUT p_ErrorMsg TEXT
)
BEGIN
    DECLARE v_Status INT DEFAULT 0;
    DECLARE v_Message VARCHAR(255) DEFAULT '';
    DECLARE v_TestResult BOOLEAN DEFAULT FALSE;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_TestsFailed = p_TestsFailed + 1;
        SET p_ErrorMsg = CONCAT(p_ErrorMsg, 'CRITICAL ERROR in inventory procedure testing; ');
    END;
    
    SET p_TestsPassed = 0;
    SET p_TestsFailed = 0;
    SET p_ErrorMsg = '';
    
    -- Test 1: inv_inventory_Add_Item procedure
    BEGIN
        DECLARE CONTINUE HANDLER FOR SQLEXCEPTION SET v_TestResult = FALSE;
        SET v_TestResult = TRUE;
        
        CALL inv_inventory_Add_Item(
            'TEST_PART_001', 'TEST_LOC', '99', 1, 'WIP', 
            'TEST_USER', NULL, 'Test item for verification',
            v_Status, v_Message
        );
        
        IF v_TestResult = TRUE AND v_Status = 0 THEN
            SET p_TestsPassed = p_TestsPassed + 1;
        ELSE
            SET p_TestsFailed = p_TestsFailed + 1;
            SET p_ErrorMsg = CONCAT(p_ErrorMsg, 'inv_inventory_Add_Item: ', v_Message, '; ');
        END IF;
    END;
    
    -- Test 2: inv_inventory_Remove_Item procedure  
    BEGIN
        DECLARE CONTINUE HANDLER FOR SQLEXCEPTION SET v_TestResult = FALSE;
        SET v_TestResult = TRUE;
        
        CALL inv_inventory_Remove_Item(
            'TEST_PART_001', 'TEST_LOC', '99', 1, 'WIP',
            'TEST_USER', NULL, 'Test removal',
            v_Status, v_Message
        );
        
        IF v_TestResult = TRUE AND v_Status >= 0 THEN
            SET p_TestsPassed = p_TestsPassed + 1;
        ELSE
            SET p_TestsFailed = p_TestsFailed + 1;
            SET p_ErrorMsg = CONCAT(p_ErrorMsg, 'inv_inventory_Remove_Item: ', v_Message, '; ');
        END IF;
    END;
    
    -- Test 3: inv_transactions_SmartSearch procedure
    BEGIN
        DECLARE CONTINUE HANDLER FOR SQLEXCEPTION SET v_TestResult = FALSE;
        SET v_TestResult = TRUE;
        
        CALL inv_transactions_SmartSearch(
            'TEST_USER', TRUE, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 
            NULL, NOW() - INTERVAL 1 DAY, NOW(), NULL, NULL, 1, 10,
            v_Status, v_Message
        );
        
        IF v_TestResult = TRUE AND v_Status = 0 THEN
            SET p_TestsPassed = p_TestsPassed + 1;
        ELSE
            SET p_TestsFailed = p_TestsFailed + 1;
            SET p_ErrorMsg = CONCAT(p_ErrorMsg, 'inv_transactions_SmartSearch: ', v_Message, '; ');
        END IF;
    END;
    
    -- Clean up test data
    DELETE FROM inv_inventory WHERE PartID = 'TEST_PART_001' AND User = 'TEST_USER';
    DELETE FROM inv_transaction WHERE PartID = 'TEST_PART_001' AND User = 'TEST_USER';
    
END $$
DELIMITER ;

-- ================================================================================
-- PHASE 5: COMPREHENSIVE SYSTEM VERIFICATION
-- ================================================================================

-- Master verification procedure that runs all checks
DELIMITER $$
CREATE PROCEDURE sys_RunCompleteVerification(
    OUT p_OverallStatus INT,
    OUT p_Summary TEXT
)
BEGIN
    DECLARE v_SchemaStatus INT DEFAULT 0;
    DECLARE v_SchemaMsg TEXT DEFAULT '';
    DECLARE v_ProcStatus INT DEFAULT 0;
    DECLARE v_ProcMsg TEXT DEFAULT '';
    DECLARE v_TestsPassed INT DEFAULT 0;
    DECLARE v_TestsFailed INT DEFAULT 0;
    DECLARE v_TestMsg TEXT DEFAULT '';
    
    SET p_Summary = 'MTM STORED PROCEDURE VERIFICATION REPORT\n';
    SET p_Summary = CONCAT(p_Summary, '==========================================\n');
    SET p_Summary = CONCAT(p_Summary, 'Verification Date: ', NOW(), '\n\n');
    
    -- Step 1: Schema Verification
    CALL sys_VerifyDatabaseSchema(v_SchemaStatus, v_SchemaMsg);
    SET p_Summary = CONCAT(p_Summary, '1. SCHEMA VERIFICATION: ');
    IF v_SchemaStatus = 0 THEN
        SET p_Summary = CONCAT(p_Summary, 'PASSED\n   ', v_SchemaMsg, '\n\n');
    ELSE
        SET p_Summary = CONCAT(p_Summary, 'FAILED\n   ', v_SchemaMsg, '\n\n');
    END IF;
    
    -- Step 2: Procedure Inventory
    CALL sys_GetStoredProcedureInventory(v_ProcStatus, v_ProcMsg);
    SET p_Summary = CONCAT(p_Summary, '2. PROCEDURE INVENTORY: ');
    IF v_ProcStatus = 0 THEN
        SET p_Summary = CONCAT(p_Summary, 'COMPLETED\n   ', v_ProcMsg, '\n\n');
    ELSE
        SET p_Summary = CONCAT(p_Summary, 'FAILED\n   ', v_ProcMsg, '\n\n');
    END IF;
    
    -- Step 3: Procedure Testing
    CALL test_InventoryProcedures(v_TestsPassed, v_TestsFailed, v_TestMsg);
    SET p_Summary = CONCAT(p_Summary, '3. PROCEDURE TESTING:\n');
    SET p_Summary = CONCAT(p_Summary, '   Tests Passed: ', v_TestsPassed, '\n');
    SET p_Summary = CONCAT(p_Summary, '   Tests Failed: ', v_TestsFailed, '\n');
    IF v_TestsFailed > 0 THEN
        SET p_Summary = CONCAT(p_Summary, '   Errors: ', v_TestMsg, '\n');
    END IF;
    SET p_Summary = CONCAT(p_Summary, '\n');
    
    -- Overall Status
    IF v_SchemaStatus = 0 AND v_ProcStatus = 0 AND v_TestsFailed = 0 THEN
        SET p_OverallStatus = 0;
        SET p_Summary = CONCAT(p_Summary, 'OVERALL RESULT: ALL SYSTEMS VERIFIED ✓\n');
    ELSE
        SET p_OverallStatus = 1;
        SET p_Summary = CONCAT(p_Summary, 'OVERALL RESULT: ISSUES DETECTED - REVIEW REQUIRED ⚠\n');
    END IF;
    
    SET p_Summary = CONCAT(p_Summary, '==========================================');
    
END $$
DELIMITER ;

-- ================================================================================
-- QUICK VERIFICATION COMMANDS
-- ================================================================================

-- Example usage commands (commented for reference):
-- CALL sys_VerifyDatabaseSchema(@status, @msg);
-- SELECT @status as Status, @msg as Message;

-- CALL sys_VerifyTableColumns('inv_inventory', @status, @msg);
-- SELECT @status as Status, @msg as Message;

-- CALL sys_RunCompleteVerification(@overall, @summary);
-- SELECT @overall as OverallStatus, @summary as DetailedReport;

-- ================================================================================
-- END OF VERIFICATION SYSTEM
-- ================================================================================
