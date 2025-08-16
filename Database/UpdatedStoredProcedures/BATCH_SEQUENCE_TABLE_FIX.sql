-- ================================================================================
-- BATCH SEQUENCE TABLE FIX
-- ================================================================================
-- File: Database/UpdatedDatabase/BATCH_SEQUENCE_TABLE_FIX.sql
-- Purpose: Create missing inv_inventory_batch_seq table for proper batch number generation
-- Created: January 27, 2025
-- Database: mtm_wip_application_test
-- MySQL Version: 5.7.24+ (MAMP Compatible)
-- ================================================================================

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

-- Display current state
SELECT 
    last_batch_number as 'Current Batch Number',
    (last_batch_number + 1) as 'Next Batch Number Will Be',
    created_date as 'Created',
    updated_date as 'Last Updated'
FROM `inv_inventory_batch_seq`;

-- ================================================================================
-- VERIFICATION QUERIES
-- ================================================================================

-- Check existing batch numbers in inventory
SELECT 
    'Existing Batch Numbers Analysis' as Info,
    COUNT(*) as Total_Records,
    COUNT(CASE WHEN BatchNumber IS NOT NULL THEN 1 END) as Records_With_Batch,
    COUNT(CASE WHEN BatchNumber REGEXP '^[0-9]+$' THEN 1 END) as Numeric_Batch_Numbers,
    MIN(CAST(CASE WHEN BatchNumber REGEXP '^[0-9]+$' THEN BatchNumber ELSE '0' END AS UNSIGNED)) as Min_Batch,
    MAX(CAST(CASE WHEN BatchNumber REGEXP '^[0-9]+$' THEN BatchNumber ELSE '0' END AS UNSIGNED)) as Max_Batch
FROM inv_inventory;

-- Check sequence table state  
SELECT 
    'Batch Sequence Table State' as Info,
    last_batch_number as Current_Value,
    (last_batch_number + 1) as Next_Generated,
    LPAD(last_batch_number + 1, 10, '0') as Next_Formatted_Batch
FROM inv_inventory_batch_seq;

-- Test the batch number generation procedure if it exists
SELECT 'Testing Batch Generation' as Info;

-- Create a test procedure to verify batch generation works
DROP PROCEDURE IF EXISTS test_batch_generation;

DELIMITER $$
CREATE PROCEDURE test_batch_generation()
BEGIN
    DECLARE v_status INT;
    DECLARE v_error_msg VARCHAR(255);
    
    CALL inv_inventory_GetNextBatchNumber(v_status, v_error_msg);
    SELECT v_status as Status, v_error_msg as Message, 'Batch generation test' as Info;
END $$
DELIMITER ;

-- Run the test
CALL test_batch_generation();

-- Clean up test procedure
DROP PROCEDURE IF EXISTS test_batch_generation;
