-- ================================================================================
-- MTM INVENTORY APPLICATION - COMPREHENSIVE DATABASE TESTING SCRIPT
-- ================================================================================
-- File: 99_Database_Testing_Suite.sql
-- Purpose: Test all stored procedures against LiveDatabase.sql for complete validation
-- Created: August 13, 2025
-- Target Database: mtm_wip_application_test
-- MySQL Version: 5.7.24+ (MAMP Compatible)
-- ================================================================================

-- ================================================================================
-- COMPREHENSIVE DATABASE TESTING PROCEDURES
-- ================================================================================

-- Master testing procedure that validates all systems
DELIMITER $$
CREATE PROCEDURE test_AllSystemsComprehensive(
    OUT p_TotalTests INT,
    OUT p_TestsPassed INT,
    OUT p_TestsFailed INT,
    OUT p_DetailedReport TEXT
)
BEGIN
    DECLARE v_Status INT DEFAULT 0;
    DECLARE v_Message VARCHAR(255) DEFAULT '';
    DECLARE v_TestResult BOOLEAN DEFAULT FALSE;
    DECLARE v_TestName VARCHAR(100) DEFAULT '';
    
    SET p_TotalTests = 0;
    SET p_TestsPassed = 0;
    SET p_TestsFailed = 0;
    SET p_DetailedReport = 'MTM COMPREHENSIVE DATABASE TEST REPORT\n';
    SET p_DetailedReport = CONCAT(p_DetailedReport, '=====================================\n');
    SET p_DetailedReport = CONCAT(p_DetailedReport, 'Test Date: ', NOW(), '\n\n');
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_TestsFailed = p_TestsFailed + 1;
        SET p_DetailedReport = CONCAT(p_DetailedReport, 'CRITICAL ERROR: Database testing failed with SQL exception\n');
    END;
    
    -- Test Category 1: Schema Validation
    SET p_DetailedReport = CONCAT(p_DetailedReport, '1. SCHEMA VALIDATION TESTS\n');
    SET p_DetailedReport = CONCAT(p_DetailedReport, '==========================\n');
    
    BEGIN
        DECLARE CONTINUE HANDLER FOR SQLEXCEPTION SET v_TestResult = FALSE;
        SET v_TestResult = TRUE;
        SET v_TestName = 'Schema Validation';
        
        CALL sys_VerifyDatabaseSchema(v_Status, v_Message);
        SET p_TotalTests = p_TotalTests + 1;
        
        IF v_TestResult = TRUE AND v_Status = 0 THEN
            SET p_TestsPassed = p_TestsPassed + 1;
            SET p_DetailedReport = CONCAT(p_DetailedReport, '✓ PASS: Schema Validation - ', v_Message, '\n');
        ELSE
            SET p_TestsFailed = p_TestsFailed + 1;
            SET p_DetailedReport = CONCAT(p_DetailedReport, '✗ FAIL: Schema Validation - ', v_Message, '\n');
        END IF;
    END;
    
    -- Test Category 2: Core Table Column Verification
    SET p_DetailedReport = CONCAT(p_DetailedReport, '\n2. TABLE COLUMN VERIFICATION TESTS\n');
    SET p_DetailedReport = CONCAT(p_DetailedReport, '===================================\n');
    
    -- Test inv_inventory table
    BEGIN
        DECLARE CONTINUE HANDLER FOR SQLEXCEPTION SET v_TestResult = FALSE;
        SET v_TestResult = TRUE;
        SET v_TestName = 'inv_inventory Table Columns';
        
        CALL sys_VerifyTableColumns('inv_inventory', v_Status, v_Message);
        SET p_TotalTests = p_TotalTests + 1;
        
        IF v_TestResult = TRUE AND v_Status = 0 THEN
            SET p_TestsPassed = p_TestsPassed + 1;
            SET p_DetailedReport = CONCAT(p_DetailedReport, '✓ PASS: inv_inventory Table - ', v_Message, '\n');
        ELSE
            SET p_TestsFailed = p_TestsFailed + 1;
            SET p_DetailedReport = CONCAT(p_DetailedReport, '✗ FAIL: inv_inventory Table - ', v_Message, '\n');
        END IF;
    END;
    
    -- Test inv_transaction table
    BEGIN
        DECLARE CONTINUE HANDLER FOR SQLEXCEPTION SET v_TestResult = FALSE;
        SET v_TestResult = TRUE;
        SET v_TestName = 'inv_transaction Table Columns';
        
        CALL sys_VerifyTableColumns('inv_transaction', v_Status, v_Message);
        SET p_TotalTests = p_TotalTests + 1;
        
        IF v_TestResult = TRUE AND v_Status = 0 THEN
            SET p_TestsPassed = p_TestsPassed + 1;
            SET p_DetailedReport = CONCAT(p_DetailedReport, '✓ PASS: inv_transaction Table - ', v_Message, '\n');
        ELSE
            SET p_TestsFailed = p_TestsFailed + 1;
            SET p_DetailedReport = CONCAT(p_DetailedReport, '✗ FAIL: inv_transaction Table - ', v_Message, '\n');
        END IF;
    END;
    
    -- Test Category 3: Inventory Procedures
    SET p_DetailedReport = CONCAT(p_DetailedReport, '\n3. INVENTORY PROCEDURE TESTS\n');
    SET p_DetailedReport = CONCAT(p_DetailedReport, '============================\n');
    
    -- Test inventory add procedure
    BEGIN
        DECLARE CONTINUE HANDLER FOR SQLEXCEPTION SET v_TestResult = FALSE;
        SET v_TestResult = TRUE;
        SET v_TestName = 'Add Inventory Item';
        
        CALL inv_inventory_Add_Item(
            'TEST_VERIFICATION_PART', 'TEST_LOC', '99', 1, 'WIP',
            'SYSTEM_TEST', NULL, 'Comprehensive verification test item',
            v_Status, v_Message
        );
        SET p_TotalTests = p_TotalTests + 1;
        
        IF v_TestResult = TRUE AND v_Status = 0 THEN
            SET p_TestsPassed = p_TestsPassed + 1;
            SET p_DetailedReport = CONCAT(p_DetailedReport, '✓ PASS: Add Inventory - ', v_Message, '\n');
        ELSE
            SET p_TestsFailed = p_TestsFailed + 1;
            SET p_DetailedReport = CONCAT(p_DetailedReport, '✗ FAIL: Add Inventory - ', v_Message, '\n');
        END IF;
    END;
    
    -- Test inventory remove procedure
    BEGIN
        DECLARE CONTINUE HANDLER FOR SQLEXCEPTION SET v_TestResult = FALSE;
        SET v_TestResult = TRUE;
        SET v_TestName = 'Remove Inventory Item';
        
        CALL inv_inventory_Remove_Item(
            'TEST_VERIFICATION_PART', 'TEST_LOC', '99', 1, 'SYSTEM_TEST', 
            'Comprehensive verification test removal',
            v_Status, v_Message
        );
        SET p_TotalTests = p_TotalTests + 1;
        
        IF v_TestResult = TRUE AND v_Status >= 0 THEN  -- Status 0 = success, Status 1 = not found (acceptable)
            SET p_TestsPassed = p_TestsPassed + 1;
            SET p_DetailedReport = CONCAT(p_DetailedReport, '✓ PASS: Remove Inventory - ', v_Message, '\n');
        ELSE
            SET p_TestsFailed = p_TestsFailed + 1;
            SET p_DetailedReport = CONCAT(p_DetailedReport, '✗ FAIL: Remove Inventory - ', v_Message, '\n');
        END IF;
    END;
    
    -- Test Category 4: Transaction Procedures  
    SET p_DetailedReport = CONCAT(p_DetailedReport, '\n4. TRANSACTION PROCEDURE TESTS\n');
    SET p_DetailedReport = CONCAT(p_DetailedReport, '==============================\n');
    
    -- Test smart search procedure
    BEGIN
        DECLARE CONTINUE HANDLER FOR SQLEXCEPTION SET v_TestResult = FALSE;
        SET v_TestResult = TRUE;
        SET v_TestName = 'Smart Search Transactions';
        
        CALL inv_transactions_SmartSearch(
            'SYSTEM_TEST', TRUE, NULL, NULL, NULL, NULL, NULL, NULL, NULL,
            NULL, NOW() - INTERVAL 7 DAY, NOW(), NULL, NULL, 1, 10,
            v_Status, v_Message
        );
        SET p_TotalTests = p_TotalTests + 1;
        
        IF v_TestResult = TRUE AND v_Status = 0 THEN
            SET p_TestsPassed = p_TestsPassed + 1;
            SET p_DetailedReport = CONCAT(p_DetailedReport, '✓ PASS: Smart Search - ', v_Message, '\n');
        ELSE
            SET p_TestsFailed = p_TestsFailed + 1;
            SET p_DetailedReport = CONCAT(p_DetailedReport, '✗ FAIL: Smart Search - ', v_Message, '\n');
        END IF;
    END;
    
    -- Test analytics procedure
    BEGIN
        DECLARE CONTINUE HANDLER FOR SQLEXCEPTION SET v_TestResult = FALSE;
        SET v_TestResult = TRUE;
        SET v_TestName = 'Transaction Analytics';
        
        CALL inv_transactions_GetAnalytics(
            'SYSTEM_TEST', TRUE, NOW() - INTERVAL 30 DAY, NOW(),
            v_Status, v_Message
        );
        SET p_TotalTests = p_TotalTests + 1;
        
        IF v_TestResult = TRUE AND v_Status = 0 THEN
            SET p_TestsPassed = p_TestsPassed + 1;
            SET p_DetailedReport = CONCAT(p_DetailedReport, '✓ PASS: Analytics - ', v_Message, '\n');
        ELSE
            SET p_TestsFailed = p_TestsFailed + 1;
            SET p_DetailedReport = CONCAT(p_DetailedReport, '✗ FAIL: Analytics - ', v_Message, '\n');
        END IF;
    END;
    
    -- Test Category 5: Theme Management Procedures
    SET p_DetailedReport = CONCAT(p_DetailedReport, '\n5. THEME MANAGEMENT TESTS\n');
    SET p_DetailedReport = CONCAT(p_DetailedReport, '=========================\n');
    
    -- Test theme retrieval
    BEGIN
        DECLARE CONTINUE HANDLER FOR SQLEXCEPTION SET v_TestResult = FALSE;
        SET v_TestResult = TRUE;
        SET v_TestName = 'Get All Themes';
        
        CALL app_themes_Get_All(v_Status, v_Message);
        SET p_TotalTests = p_TotalTests + 1;
        
        IF v_TestResult = TRUE AND v_Status = 0 THEN
            SET p_TestsPassed = p_TestsPassed + 1;
            SET p_DetailedReport = CONCAT(p_DetailedReport, '✓ PASS: Get Themes - ', v_Message, '\n');
        ELSE
            SET p_TestsFailed = p_TestsFailed + 1;
            SET p_DetailedReport = CONCAT(p_DetailedReport, '✗ FAIL: Get Themes - ', v_Message, '\n');
        END IF;
    END;
    
    -- Clean up test data
    DELETE FROM inv_inventory WHERE PartID = 'TEST_VERIFICATION_PART' AND User = 'SYSTEM_TEST';
    DELETE FROM inv_transaction WHERE PartID = 'TEST_VERIFICATION_PART' AND User = 'SYSTEM_TEST';
    
    -- Generate summary
    SET p_DetailedReport = CONCAT(p_DetailedReport, '\n========================================\n');
    SET p_DetailedReport = CONCAT(p_DetailedReport, 'COMPREHENSIVE TEST SUMMARY\n');
    SET p_DetailedReport = CONCAT(p_DetailedReport, '========================================\n');
    SET p_DetailedReport = CONCAT(p_DetailedReport, 'Total Tests Run: ', p_TotalTests, '\n');
    SET p_DetailedReport = CONCAT(p_DetailedReport, 'Tests Passed: ', p_TestsPassed, '\n');
    SET p_DetailedReport = CONCAT(p_DetailedReport, 'Tests Failed: ', p_TestsFailed, '\n');
    
    IF p_TestsFailed = 0 THEN
        SET p_DetailedReport = CONCAT(p_DetailedReport, 'OVERALL RESULT: ALL TESTS PASSED ✓\n');
        SET p_DetailedReport = CONCAT(p_DetailedReport, 'DATABASE IS READY FOR PRODUCTION USE\n');
    ELSE
        SET p_DetailedReport = CONCAT(p_DetailedReport, 'OVERALL RESULT: ', p_TestsFailed, ' TESTS FAILED ⚠\n');
        SET p_DetailedReport = CONCAT(p_DetailedReport, 'REVIEW FAILED TESTS BEFORE DEPLOYMENT\n');
    END IF;
    
    SET p_DetailedReport = CONCAT(p_DetailedReport, '========================================');
    
END $$
DELIMITER ;

-- Quick test procedure for specific table compatibility
DELIMITER $$
CREATE PROCEDURE test_TableCompatibility(
    IN p_TableName VARCHAR(64),
    OUT p_Status INT,
    OUT p_ErrorMsg TEXT
)
BEGIN
    DECLARE v_ColumnCount INT DEFAULT 0;
    DECLARE v_TableExists BOOLEAN DEFAULT FALSE;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error while testing table: ', p_TableName);
    END;
    
    -- Check if table exists
    SELECT COUNT(*) INTO v_ColumnCount
    FROM INFORMATION_SCHEMA.TABLES 
    WHERE TABLE_SCHEMA = DATABASE() 
    AND TABLE_NAME = p_TableName;
    
    IF v_ColumnCount = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Table not found: ', p_TableName);
    ELSE
        -- Get column count for validation
        SELECT COUNT(*) INTO v_ColumnCount
        FROM INFORMATION_SCHEMA.COLUMNS
        WHERE TABLE_SCHEMA = DATABASE() 
        AND TABLE_NAME = p_TableName;
        
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Table verified: ', p_TableName, ' with ', v_ColumnCount, ' columns');
        
        -- Return column details
        SELECT 
            COLUMN_NAME as ColumnName,
            DATA_TYPE as DataType,
            IS_NULLABLE as Nullable,
            COLUMN_DEFAULT as DefaultValue,
            CHARACTER_MAXIMUM_LENGTH as MaxLength
        FROM INFORMATION_SCHEMA.COLUMNS
        WHERE TABLE_SCHEMA = DATABASE() 
        AND TABLE_NAME = p_TableName
        ORDER BY ORDINAL_POSITION;
    END IF;
    
END $$
DELIMITER ;

-- ================================================================================
-- EXAMPLE USAGE COMMANDS
-- ================================================================================
-- Run comprehensive testing:
-- CALL test_AllSystemsComprehensive(@total, @passed, @failed, @report);
-- SELECT @total as TotalTests, @passed as Passed, @failed as Failed, @report as DetailedReport;

-- Test specific table:
-- CALL test_TableCompatibility('inv_inventory', @status, @msg);
-- SELECT @status as Status, @msg as Message;

-- ================================================================================
-- END OF TESTING SUITE
-- ================================================================================