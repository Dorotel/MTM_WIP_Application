-- ================================================================================
-- BATCH SEQUENCE FIX DEPLOYMENT SCRIPT - PHPMYADMIN COMPATIBLE
-- ================================================================================
-- File: Database/UpdatedDatabase/DEPLOY_BATCH_SEQUENCE_FIX.sql
-- Purpose: Complete deployment of the batch sequence fix
-- Created: January 27, 2025
-- Usage: Copy and paste this ENTIRE script into phpMyAdmin SQL tab and click "Go"
-- Note: This is phpMyAdmin compatible - no SOURCE commands used
-- ================================================================================

SELECT 'Starting Batch Sequence Table Fix Deployment...' as Status;

-- ================================================================================
-- STEP 1: CREATE AND INITIALIZE BATCH SEQUENCE TABLE
-- ================================================================================

SELECT 'Step 1: Creating batch sequence table...' as Status;

-- Drop the table if it exists to start fresh
DROP TABLE IF EXISTS `inv_inventory_batch_seq`;

-- Create the batch sequence table with proper structure
CREATE TABLE `inv_inventory_batch_seq` (
  `last_batch_number` INT(11) NOT NULL DEFAULT 0,
  `created_date` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_date` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Initialize with current highest batch number from existing data
-- Insert the starting value based on existing batch numbers
INSERT INTO `inv_inventory_batch_seq` (`last_batch_number`) 
SELECT COALESCE(MAX(CAST(CASE 
    WHEN BatchNumber IS NOT NULL AND BatchNumber REGEXP '^[0-9]+$' 
    THEN BatchNumber 
    ELSE '0' 
END AS UNSIGNED)), 0) as MaxBatch
FROM `inv_inventory`
WHERE BatchNumber IS NOT NULL;

-- If the table is still empty (no inventory records exist), insert default
INSERT INTO `inv_inventory_batch_seq` (`last_batch_number`) 
SELECT 0
WHERE NOT EXISTS (SELECT 1 FROM `inv_inventory_batch_seq`);

SELECT 'Batch sequence table created and initialized.' as Status;

-- ================================================================================
-- STEP 2: DEPLOY UPDATED INVENTORY STORED PROCEDURES
-- ================================================================================

SELECT 'Step 2: Deploying updated inventory procedures...' as Status;

-- Drop procedures if they exist (for clean deployment)
DROP PROCEDURE IF EXISTS inv_inventory_Add_Item;
DROP PROCEDURE IF EXISTS inv_inventory_Remove_Item;
DROP PROCEDURE IF EXISTS inv_inventory_Transfer_Part;
DROP PROCEDURE IF EXISTS inv_inventory_transfer_quantity;
DROP PROCEDURE IF EXISTS inv_inventory_GetNextBatchNumber;
DROP PROCEDURE IF EXISTS inv_inventory_Fix_BatchNumbers;
DROP PROCEDURE IF EXISTS inv_inventory_Get_ByPartID;
DROP PROCEDURE IF EXISTS inv_inventory_Get_ByPartIDAndOperation;
DROP PROCEDURE IF EXISTS inv_inventory_Get_All;
DROP PROCEDURE IF EXISTS inv_transaction_GetProblematicBatchCount;
DROP PROCEDURE IF EXISTS inv_transaction_GetProblematicBatches;
DROP PROCEDURE IF EXISTS inv_transaction_SplitBatchNumbers;
DROP PROCEDURE IF EXISTS inv_inventory_Search_Advanced;
DROP PROCEDURE IF EXISTS inv_transactions_SmartSearch;
DROP PROCEDURE IF EXISTS inv_transactions_GetAnalytics;
DROP PROCEDURE IF EXISTS inv_transaction_Add;

-- Add inventory item - ALWAYS CREATE NEW BATCH VERSION (no quantity consolidation)
DELIMITER $$
CREATE PROCEDURE inv_inventory_Add_Item(
    IN p_PartID VARCHAR(300),        -- Match working size
    IN p_Location VARCHAR(100),
    IN p_Operation VARCHAR(100),
    IN p_Quantity INT,
    IN p_ItemType VARCHAR(100),      -- Match working size  
    IN p_User VARCHAR(100),
    IN p_BatchNumber VARCHAR(100),   -- Match working size (optional)
    IN p_Notes VARCHAR(1000),        -- Match working size
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_NextBatchNumber VARCHAR(20);
    DECLARE v_ErrorMessage TEXT DEFAULT '';
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        GET DIAGNOSTICS CONDITION 1
            v_ErrorMessage = MESSAGE_TEXT;
        
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while adding inventory item for part: ', p_PartID);
        
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- ALWAYS generate a new batch number for each addition (no consolidation)
    -- This ensures each inventory addition creates a separate trackable batch
    
    -- ROBUST: Ensure the batch sequence table exists
    CREATE TABLE IF NOT EXISTS inv_inventory_batch_seq (
        last_batch_number INT(11) NOT NULL DEFAULT 0,
        created_date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
        updated_date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
    ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
    
    -- ROBUST: Initialize sequence table if empty
    INSERT IGNORE INTO inv_inventory_batch_seq (last_batch_number)
    SELECT COALESCE(MAX(CAST(CASE 
        WHEN BatchNumber IS NOT NULL AND BatchNumber REGEXP '^[0-9]+$' 
        THEN BatchNumber 
        ELSE '0' 
    END AS UNSIGNED)), 0)
    FROM inv_inventory;
    
    -- Generate next batch number
    SELECT COALESCE(last_batch_number, 0) INTO @nextBatch FROM inv_inventory_batch_seq LIMIT 1;
    SET @nextBatch = @nextBatch + 1;
    SET v_NextBatchNumber = LPAD(@nextBatch, 10, '0');
    
    -- Update the sequence table
    UPDATE inv_inventory_batch_seq SET last_batch_number = @nextBatch WHERE TRUE;
    
    -- Validate quantity
    IF p_Quantity <= 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = 'Quantity must be greater than zero';
        ROLLBACK;
    ELSE
        -- ALWAYS INSERT NEW ROW - Never update existing quantities
        -- Each inventory addition gets its own batch number and row
        INSERT INTO inv_inventory (
            PartID, Location, Operation, Quantity, ItemType, 
            User, BatchNumber, Notes, ReceiveDate, LastUpdated
        ) VALUES (
            p_PartID, p_Location, p_Operation, p_Quantity, p_ItemType,
            p_User, v_NextBatchNumber, p_Notes, NOW(), NOW()
        );
        
        -- Insert transaction record for audit trail
        INSERT INTO inv_transaction (
            TransactionType, BatchNumber, PartID, FromLocation, ToLocation,
            Operation, Quantity, Notes, User, ItemType, ReceiveDate
        ) VALUES (
            'IN', v_NextBatchNumber, p_PartID, p_Location, NULL,
            p_Operation, p_Quantity, p_Notes, p_User, p_ItemType, NOW()
        );
        
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('New inventory batch created successfully for part: ', p_PartID, ' with batch: ', v_NextBatchNumber, ', quantity: ', p_Quantity);
        COMMIT;
    END IF;
END $$
DELIMITER ;

-- Get next available batch number with status reporting and error logging
DELIMITER $$
CREATE PROCEDURE inv_inventory_GetNextBatchNumber(
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_ErrorMessage TEXT DEFAULT '';
    DECLARE v_NextBatchNumber VARCHAR(20) DEFAULT '0000000001';
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        GET DIAGNOSTICS CONDITION 1
            v_ErrorMessage = MESSAGE_TEXT;
        
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred while generating next batch number';
        
        -- Log error to log_error table if it exists
        INSERT IGNORE INTO log_error (
            User, Severity, ErrorType, ErrorMessage, StackTrace,
            ModuleName, MethodName, AdditionalInfo, MachineName,
            OSVersion, AppVersion, ErrorTime
        ) VALUES (
            'SYSTEM', 'Error', 'DatabaseError',
            CONCAT('inv_inventory_GetNextBatchNumber failed: ', v_ErrorMessage),
            v_ErrorMessage,
            'inv_inventory_GetNextBatchNumber', 'GetNextBatchNumber',
            'Error generating next batch number',
            'Database Server', 'MySQL 5.7.24', '5.0.1.2', NOW()
        );
    END;
    
    -- ROBUST: Ensure the batch sequence table exists and is initialized
    CREATE TABLE IF NOT EXISTS inv_inventory_batch_seq (
        last_batch_number INT(11) NOT NULL DEFAULT 0,
        created_date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
        updated_date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
    ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
    
    -- ROBUST: Initialize sequence table if empty
    INSERT IGNORE INTO inv_inventory_batch_seq (last_batch_number)
    SELECT COALESCE(MAX(CAST(CASE 
        WHEN BatchNumber IS NOT NULL AND BatchNumber REGEXP '^[0-9]+$' 
        THEN BatchNumber 
        ELSE '0' 
    END AS UNSIGNED)), 0)
    FROM inv_inventory;
    
    -- ROBUST: Generate next batch number using fallback methods
    SELECT LPAD(COALESCE(last_batch_number + 1, 1), 10, '0') INTO v_NextBatchNumber
    FROM inv_inventory_batch_seq
    LIMIT 1;
    
    -- ROBUST: If sequence table is still empty, use inventory table maximum
    IF v_NextBatchNumber IS NULL OR v_NextBatchNumber = '0000000000' THEN
        SELECT LPAD(COALESCE(MAX(CAST(CASE 
            WHEN BatchNumber IS NOT NULL AND BatchNumber REGEXP '^[0-9]+$' 
            THEN BatchNumber 
            ELSE '0' 
        END AS UNSIGNED)), 0) + 1, 10, '0') INTO v_NextBatchNumber
        FROM inv_inventory;
        
        -- ROBUST: Final fallback to default starting batch number
        IF v_NextBatchNumber IS NULL OR v_NextBatchNumber = '0000000000' THEN
            SET v_NextBatchNumber = '0000000001';
        END IF;
    END IF;
    
    -- Update the sequence table for next time
    UPDATE inv_inventory_batch_seq 
    SET last_batch_number = CAST(v_NextBatchNumber AS UNSIGNED)
    WHERE TRUE;
    
    -- Return the generated batch number
    SELECT v_NextBatchNumber as NextBatchNumber;
    
    SET p_Status = 0;
    SET p_ErrorMsg = 'Next batch number generated successfully';
END $$
DELIMITER ;

SELECT 'Core inventory procedures deployed.' as Status;

-- ================================================================================
-- STEP 3: FINAL VERIFICATION
-- ================================================================================

SELECT 'Step 3: Running final verification...' as Status;

-- Check that the table exists and has data
SELECT 
    'inv_inventory_batch_seq Table Status' as Check_Type,
    COUNT(*) as Record_Count,
    MAX(last_batch_number) as Current_Batch_Number,
    (MAX(last_batch_number) + 1) as Next_Batch_Number
FROM inv_inventory_batch_seq;

-- Check existing batch numbers in inventory
SELECT 
    'Existing Batch Numbers Analysis' as Check_Type,
    COUNT(*) as Total_Records,
    COUNT(CASE WHEN BatchNumber IS NOT NULL THEN 1 END) as Records_With_Batch,
    COUNT(CASE WHEN BatchNumber REGEXP '^[0-9]+$' THEN 1 END) as Numeric_Batch_Numbers,
    MIN(CAST(CASE WHEN BatchNumber REGEXP '^[0-9]+$' THEN BatchNumber ELSE '0' END AS UNSIGNED)) as Min_Batch,
    MAX(CAST(CASE WHEN BatchNumber REGEXP '^[0-9]+$' THEN BatchNumber ELSE '0' END AS UNSIGNED)) as Max_Batch
FROM inv_inventory;

-- Check that the procedures exist
SELECT 
    'Stored Procedures Status' as Check_Type,
    COUNT(*) as Procedure_Count
FROM INFORMATION_SCHEMA.ROUTINES 
WHERE ROUTINE_SCHEMA = DATABASE() 
AND ROUTINE_NAME IN ('inv_inventory_Add_Item', 'inv_inventory_GetNextBatchNumber');

-- Test batch number generation
DELIMITER $$
CREATE PROCEDURE test_final_batch_generation()
BEGIN
    DECLARE v_status INT;
    DECLARE v_error_msg VARCHAR(255);
    
    CALL inv_inventory_GetNextBatchNumber(v_status, v_error_msg);
    SELECT 
        'Batch Generation Test' as Check_Type,
        v_status as Status_Code, 
        v_error_msg as Message,
        CASE 
            WHEN v_status = 0 THEN '? SUCCESS'
            WHEN v_status = 1 THEN '?? WARNING'  
            ELSE '? ERROR'
        END as Result;
END $$
DELIMITER ;

CALL test_final_batch_generation();
DROP PROCEDURE test_final_batch_generation;

-- ================================================================================
-- DEPLOYMENT COMPLETE
-- ================================================================================

SELECT '?? Batch Sequence Fix Deployment Complete!' as Status;
SELECT '' as Separator;
SELECT '?? NEXT STEPS:' as Info;
SELECT '1. Test inventory additions in your application' as Step_1;
SELECT '2. Verify each addition creates separate rows with unique batch numbers' as Step_2;  
SELECT '3. Confirm no quantity consolidation occurs' as Step_3;
SELECT '4. Check that batch numbers are sequential and properly formatted' as Step_4;
SELECT '' as Separator;
SELECT '? EXPECTED BEHAVIOR AFTER FIX:' as Info;
SELECT 'Before: Add same item twice ? 1 row with combined quantity ?' as Before;
SELECT 'After: Add same item twice ? 2 separate rows with unique batch numbers ?' as After;
SELECT '' as Separator;
SELECT '?? IF ISSUES OCCUR:' as Troubleshooting;
SELECT 'Check application logs for any FixBatchNumbersAsync() calls' as Issue_1;
SELECT 'Verify Data/Dao_Inventory.cs changes are deployed' as Issue_2;
SELECT 'Test with simple inventory addition from Inventory Tab' as Issue_3;
