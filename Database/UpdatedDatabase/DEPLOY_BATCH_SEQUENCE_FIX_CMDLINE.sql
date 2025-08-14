-- ================================================================================
-- BATCH SEQUENCE FIX DEPLOYMENT SCRIPT - MYSQL COMMAND LINE VERSION
-- ================================================================================
-- File: Database/UpdatedDatabase/DEPLOY_BATCH_SEQUENCE_FIX_CMDLINE.sql
-- Purpose: Complete deployment of the batch sequence fix using SOURCE commands
-- Created: January 27, 2025
-- Usage: Run this in MySQL command line client (not phpMyAdmin)
-- Command: mysql -h localhost -u root -p mtm_wip_application_test < DEPLOY_BATCH_SEQUENCE_FIX_CMDLINE.sql
-- ================================================================================

SELECT 'Starting Batch Sequence Table Fix Deployment (MySQL Command Line Version)...' as Status;

-- Step 1: Create the batch sequence table
SOURCE BATCH_SEQUENCE_TABLE_FIX.sql;

SELECT 'Batch sequence table created and initialized.' as Status;

-- Step 2: Deploy the updated inventory procedures
SOURCE ../UpdatedStoredProcedures/04_Inventory_Procedures.sql;

SELECT 'Updated inventory procedures deployed.' as Status;

-- Step 3: Final verification
SELECT 'Final Verification Results:' as Status;

-- Check that the table exists and has data
SELECT 
    'inv_inventory_batch_seq Table Status' as Check_Type,
    COUNT(*) as Record_Count,
    MAX(last_batch_number) as Current_Batch_Number
FROM inv_inventory_batch_seq;

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

SELECT '?? Batch Sequence Fix Deployment Complete!' as Status;
SELECT '?? Next Steps:' as Info;
SELECT '1. Test inventory additions in your application' as Step_1;
SELECT '2. Verify each addition creates separate rows with unique batch numbers' as Step_2;  
SELECT '3. Confirm no quantity consolidation occurs' as Step_3;
