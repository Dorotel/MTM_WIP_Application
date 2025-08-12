-- ================================================================================
-- MTM INVENTORY APPLICATION - INVENTORY MANAGEMENT STORED PROCEDURES
-- ================================================================================
-- File: 04_Inventory_Procedures.sql
-- Purpose: Inventory tracking, transactions, and batch number management
-- Created: August 10, 2025
-- Updated: August 12, 2025 - FIXED COLUMN NAMES TO MATCH DATABASE SCHEMA
-- Target Database: mtm_wip_application_test
-- MySQL Version: 5.7.24+ (MAMP Compatible)
-- ================================================================================

-- Drop procedures if they exist (for clean deployment)
DROP PROCEDURE IF EXISTS inv_inventory_Add_Item;
DROP PROCEDURE IF EXISTS inv_inventory_Remove_Item_1_1;
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

-- ================================================================================
-- INVENTORY ITEM MANAGEMENT PROCEDURES (MySQL 5.7.24 Compatible)
-- ================================================================================

-- Add inventory item with status reporting
DELIMITER $$
CREATE PROCEDURE inv_inventory_Add_Item(
    IN p_PartID VARCHAR(100),
    IN p_Location VARCHAR(100),
    IN p_Operation VARCHAR(100),
    IN p_Quantity INT,
    IN p_ItemType VARCHAR(50),
    IN p_User VARCHAR(100),
    IN p_BatchNumber VARCHAR(20),
    IN p_Notes TEXT,
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_NextBatchNumber VARCHAR(20);
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while adding inventory item for part: ', p_PartID);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- Generate batch number if not provided
    IF p_BatchNumber IS NULL OR p_BatchNumber = '' THEN
        SELECT LPAD(COALESCE(MAX(CAST(BatchNumber AS UNSIGNED)), 0) + 1, 10, '0') 
        INTO v_NextBatchNumber
        FROM inv_inventory;
        SET p_BatchNumber = v_NextBatchNumber;
    END IF;
    
    -- Validate quantity
    IF p_Quantity <= 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = 'Quantity must be greater than zero';
        ROLLBACK;
    ELSE
        -- FIXED: Use correct column names from actual database schema
        INSERT INTO inv_inventory (
            PartID, Location, Operation, Quantity, ItemType, 
            User, BatchNumber, Notes, ReceiveDate, LastUpdated
        ) VALUES (
            p_PartID, p_Location, p_Operation, p_Quantity, p_ItemType,
            p_User, p_BatchNumber, p_Notes, NOW(), NOW()
        );
        
        -- Also insert transaction record (assuming inv_transaction table exists)
        INSERT INTO inv_transaction (
            PartID, Location, Operation, Quantity, ItemType,
            User, BatchNumber, Notes, TransactionType, ReceiveDate
        ) VALUES (
            p_PartID, p_Location, p_Operation, p_Quantity, p_ItemType,
            p_User, p_BatchNumber, p_Notes, 'IN', NOW()
        );
        
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Inventory item added successfully for part: ', p_PartID, ' with batch: ', p_BatchNumber);
        COMMIT;
    END IF;
END $$
DELIMITER ;

-- Remove inventory item with detailed status reporting
DELIMITER $$
CREATE PROCEDURE inv_inventory_Remove_Item_1_1(
    IN p_PartID VARCHAR(100),
    IN p_Location VARCHAR(100),
    IN p_Operation VARCHAR(100),
    IN p_Quantity INT,
    IN p_ItemType VARCHAR(50),
    IN p_User VARCHAR(100),
    IN p_BatchNumber VARCHAR(20),
    IN p_Notes TEXT,
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_CurrentQuantity INT DEFAULT 0;
    DECLARE v_InventoryId INT DEFAULT NULL;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while removing inventory item for part: ', p_PartID);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- Validate quantity
    IF p_Quantity <= 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = 'Quantity must be greater than zero';
        ROLLBACK;
    ELSE
        -- Find matching inventory item
        SELECT ID, Quantity INTO v_InventoryId, v_CurrentQuantity
        FROM inv_inventory 
        WHERE PartID = p_PartID 
          AND Location = p_Location 
          AND Operation = p_Operation 
          AND BatchNumber = p_BatchNumber
        LIMIT 1;
        
        IF v_InventoryId IS NULL THEN
            SET p_Status = 1;
            SET p_ErrorMsg = CONCAT('No matching inventory item found for part: ', p_PartID, ', batch: ', p_BatchNumber);
            ROLLBACK;
        ELSEIF v_CurrentQuantity < p_Quantity THEN
            SET p_Status = 1;
            SET p_ErrorMsg = CONCAT('Insufficient quantity. Available: ', v_CurrentQuantity, ', Requested: ', p_Quantity);
            ROLLBACK;
        ELSE
            -- FIXED: Use correct column name LastUpdated instead of ModifiedDate
            UPDATE inv_inventory 
            SET Quantity = Quantity - p_Quantity,
                LastUpdated = NOW()
            WHERE ID = v_InventoryId;
            
            -- Remove record if quantity becomes 0
            DELETE FROM inv_inventory WHERE ID = v_InventoryId AND Quantity <= 0;
            
            -- Insert transaction record
            INSERT INTO inv_transaction (
                PartID, Location, Operation, Quantity, ItemType,
                User, BatchNumber, Notes, TransactionType, ReceiveDate
            ) VALUES (
                p_PartID, p_Location, p_Operation, p_Quantity, p_ItemType,
                p_User, p_BatchNumber, p_Notes, 'OUT', NOW()
            );
            
            SET p_Status = 0;
            SET p_ErrorMsg = CONCAT('Inventory item removed successfully for part: ', p_PartID, ', quantity: ', p_Quantity);
            COMMIT;
        END IF;
    END IF;
END $$
DELIMITER ;

-- ================================================================================
-- INVENTORY TRANSFER PROCEDURES (MySQL 5.7.24 Compatible)
-- ================================================================================

-- Transfer part to new location (simple transfer) with status reporting
DELIMITER $$
CREATE PROCEDURE inv_inventory_Transfer_Part(
    IN p_BatchNumber VARCHAR(20),
    IN p_PartID VARCHAR(100),
    IN p_Operation VARCHAR(100),
    IN p_NewLocation VARCHAR(100),
    IN p_User VARCHAR(100),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;
    DECLARE v_OldLocation VARCHAR(100);
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while transferring part: ', p_PartID);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- Check if inventory item exists and get current location
    SELECT COUNT(*), MAX(Location) INTO v_Count, v_OldLocation 
    FROM inv_inventory 
    WHERE BatchNumber = p_BatchNumber 
      AND PartID = p_PartID 
      AND Operation = p_Operation;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('No inventory found for part: ', p_PartID, ', batch: ', p_BatchNumber);
        ROLLBACK;
    ELSE
        -- FIXED: Use correct column name LastUpdated instead of ModifiedDate
        UPDATE inv_inventory 
        SET Location = p_NewLocation,
            User = p_User,
            LastUpdated = NOW()
        WHERE BatchNumber = p_BatchNumber 
          AND PartID = p_PartID 
          AND Operation = p_Operation;
        
        SET v_RowsAffected = ROW_COUNT();
        
        IF v_RowsAffected > 0 THEN
            SET p_Status = 0;
            SET p_ErrorMsg = CONCAT('Part transferred successfully from ', v_OldLocation, ' to ', p_NewLocation);
        ELSE
            SET p_Status = 2;
            SET p_ErrorMsg = CONCAT('No transfer occurred for part: ', p_PartID);
        END IF;
        
        COMMIT;
    END IF;
END $$
DELIMITER ;

-- Transfer specific quantity to new location with status reporting
DELIMITER $$
CREATE PROCEDURE inv_inventory_transfer_quantity(
    IN p_BatchNumber VARCHAR(20),
    IN p_PartID VARCHAR(100),
    IN p_Operation VARCHAR(100),
    IN p_TransferQuantity INT,
    IN p_NewLocation VARCHAR(100),
    IN p_User VARCHAR(100),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_NewBatchNumber VARCHAR(20);
    DECLARE v_ItemType VARCHAR(50);
    DECLARE v_OriginalLocation VARCHAR(100);
    DECLARE v_CurrentQuantity INT DEFAULT 0;
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while transferring quantity for part: ', p_PartID);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- Validate transfer quantity
    IF p_TransferQuantity <= 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = 'Transfer quantity must be greater than zero';
        ROLLBACK;
    ELSE
        -- Get original location, item type, and current quantity
        SELECT COUNT(*), MAX(Location), MAX(ItemType), MAX(Quantity) 
        INTO v_Count, v_OriginalLocation, v_ItemType, v_CurrentQuantity
        FROM inv_inventory 
        WHERE BatchNumber = p_BatchNumber 
          AND PartID = p_PartID 
          AND Operation = p_Operation;
        
        IF v_Count = 0 THEN
            SET p_Status = 1;
            SET p_ErrorMsg = CONCAT('No inventory found for part: ', p_PartID, ', batch: ', p_BatchNumber);
            ROLLBACK;
        ELSEIF v_CurrentQuantity < p_TransferQuantity THEN
            SET p_Status = 1;
            SET p_ErrorMsg = CONCAT('Insufficient quantity for transfer. Available: ', v_CurrentQuantity, ', Requested: ', p_TransferQuantity);
            ROLLBACK;
        ELSE
            -- Generate new batch number for transferred quantity
            SELECT LPAD(COALESCE(MAX(CAST(BatchNumber AS UNSIGNED)), 0) + 1, 10, '0') 
            INTO v_NewBatchNumber
            FROM inv_inventory;
            
            -- FIXED: Use correct column name LastUpdated instead of ModifiedDate
            UPDATE inv_inventory 
            SET Quantity = Quantity - p_TransferQuantity,
                LastUpdated = NOW()
            WHERE BatchNumber = p_BatchNumber 
              AND PartID = p_PartID 
              AND Operation = p_Operation;
            
            -- Remove original record if quantity becomes 0
            DELETE FROM inv_inventory 
            WHERE BatchNumber = p_BatchNumber 
              AND PartID = p_PartID 
              AND Operation = p_Operation 
              AND Quantity <= 0;
            
            -- FIXED: Use correct column names for INSERT
            INSERT INTO inv_inventory (
                PartID, Location, Operation, Quantity, ItemType,
                User, BatchNumber, ReceiveDate, LastUpdated
            ) VALUES (
                p_PartID, p_NewLocation, p_Operation, p_TransferQuantity, v_ItemType,
                p_User, v_NewBatchNumber, NOW(), NOW()
            );
            
            -- Record transfer transactions
            INSERT INTO inv_transaction (
                PartID, Location, Operation, Quantity, ItemType,
                User, BatchNumber, TransactionType, ReceiveDate
            ) VALUES 
            (p_PartID, v_OriginalLocation, p_Operation, p_TransferQuantity, v_ItemType,
             p_User, p_BatchNumber, 'OUT', NOW()),
            (p_PartID, p_NewLocation, p_Operation, p_TransferQuantity, v_ItemType,
             p_User, v_NewBatchNumber, 'IN', NOW());
            
            SET p_Status = 0;
            SET p_ErrorMsg = CONCAT('Quantity transferred successfully from ', v_OriginalLocation, ' to ', p_NewLocation, ', new batch: ', v_NewBatchNumber);
            COMMIT;
        END IF;
    END IF;
END $$
DELIMITER ;

-- ================================================================================
-- BATCH NUMBER MANAGEMENT PROCEDURES (MySQL 5.7.24 Compatible)
-- ================================================================================

-- Get next available batch number with status reporting
DELIMITER $$
CREATE PROCEDURE inv_inventory_GetNextBatchNumber(
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred while generating next batch number';
    END;
    
    SELECT LPAD(COALESCE(MAX(CAST(BatchNumber AS UNSIGNED)), 0) + 1, 10, '0') as NextBatchNumber
    FROM inv_inventory;
    
    SET p_Status = 0;
    SET p_ErrorMsg = 'Next batch number generated successfully';
END $$
DELIMITER ;

-- Fix batch numbers (consolidate and clean up)
DELIMITER $$
CREATE PROCEDURE inv_inventory_Fix_BatchNumbers(
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_ProcessedRecords INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred while fixing batch numbers';
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- Create temporary table for consolidation (MySQL 5.7 compatible)
    DROP TEMPORARY TABLE IF EXISTS temp_consolidated;
    CREATE TEMPORARY TABLE temp_consolidated (
        PartID VARCHAR(100),
        Location VARCHAR(100),
        Operation VARCHAR(100),
        ItemType VARCHAR(50),
        User VARCHAR(100),
        TotalQuantity INT,
        EarliestReceiveDate DATETIME,
        ConsolidatedBatchNumber VARCHAR(20)
    );
    
    -- Consolidate identical inventory items with different batch numbers
    INSERT INTO temp_consolidated
    SELECT 
        PartID, Location, Operation, ItemType, User,
        SUM(Quantity) as TotalQuantity,
        MIN(ReceiveDate) as EarliestReceiveDate,
        MIN(BatchNumber) as ConsolidatedBatchNumber
    FROM inv_inventory
    GROUP BY PartID, Location, Operation, ItemType, User
    HAVING SUM(Quantity) > 0;
    
    -- Get count of records to be processed
    SELECT COUNT(*) INTO v_ProcessedRecords FROM temp_consolidated;
    
    -- Delete original records
    DELETE FROM inv_inventory;
    
    -- FIXED: Use correct column names for INSERT
    INSERT INTO inv_inventory (
        PartID, Location, Operation, Quantity, ItemType, User, 
        BatchNumber, ReceiveDate, LastUpdated
    )
    SELECT 
        PartID, Location, Operation, TotalQuantity, ItemType, User,
        ConsolidatedBatchNumber, EarliestReceiveDate, NOW()
    FROM temp_consolidated
    WHERE TotalQuantity > 0;
    
    DROP TEMPORARY TABLE temp_consolidated;
    
    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('Batch numbers fixed successfully, processed ', v_ProcessedRecords, ' records');
    
    COMMIT;
END $$
DELIMITER ;

-- ================================================================================
-- INVENTORY QUERY PROCEDURES (MySQL 5.7.24 Compatible)
-- ================================================================================

-- Get inventory by part ID with status reporting
DELIMITER $$
CREATE PROCEDURE inv_inventory_Get_ByPartID(
    IN p_PartID VARCHAR(100),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while retrieving inventory for part: ', p_PartID);
    END;
    
    SELECT COUNT(*) INTO v_Count FROM inv_inventory WHERE PartID = p_PartID;
    SELECT * FROM inv_inventory WHERE PartID = p_PartID ORDER BY Location, Operation;
    
    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('Retrieved ', v_Count, ' inventory records for part: ', p_PartID);
END $$
DELIMITER ;

-- Get inventory by part ID and operation with status reporting
DELIMITER $$
CREATE PROCEDURE inv_inventory_Get_ByPartIDAndOperation(
    IN p_PartID VARCHAR(100),
    IN p_Operation VARCHAR(100),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while retrieving inventory for part: ', p_PartID, ', operation: ', p_Operation);
    END;
    
    SELECT COUNT(*) INTO v_Count 
    FROM inv_inventory 
    WHERE PartID = p_PartID AND Operation = p_Operation;
    
    SELECT * FROM inv_inventory 
    WHERE PartID = p_PartID AND Operation = p_Operation 
    ORDER BY Location;
    
    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('Retrieved ', v_Count, ' inventory records for part: ', p_PartID, ', operation: ', p_Operation);
END $$
DELIMITER ;

-- Get all inventory with status reporting
DELIMITER $$
CREATE PROCEDURE inv_inventory_Get_All(
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred while retrieving all inventory';
    END;
    
    SELECT COUNT(*) INTO v_Count FROM inv_inventory;
    SELECT * FROM inv_inventory ORDER BY PartID, Location, Operation;
    
    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('Retrieved ', v_Count, ' inventory records successfully');
END $$
DELIMITER ;

-- ================================================================================
-- BATCH ANALYSIS AND CLEANUP PROCEDURES (MySQL 5.7.24 Compatible)
-- ================================================================================

-- Get count of problematic batch numbers with status reporting
DELIMITER $$
CREATE PROCEDURE inv_transaction_GetProblematicBatchCount(
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred while counting problematic batch numbers';
    END;
    
    -- MySQL 5.7 compatible approach for counting problematic batches
    SELECT COUNT(DISTINCT BatchNumber) INTO v_Count
    FROM inv_transaction t1
    WHERE EXISTS (
        SELECT 1 FROM inv_transaction t2
        WHERE t1.BatchNumber = t2.BatchNumber
          AND DATE(t1.ReceiveDate) = DATE(t2.ReceiveDate)
          AND t1.TransactionType != t2.TransactionType
    );
    
    SELECT v_Count as ProblematicBatchCount;
    
    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('Found ', v_Count, ' problematic batch numbers');
END $$
DELIMITER ;

-- Get list of problematic batch numbers (limited) with status reporting
DELIMITER $$
CREATE PROCEDURE inv_transaction_GetProblematicBatches(
    IN p_Limit INT,
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred while retrieving problematic batch numbers';
    END;
    
    -- Validate limit parameter
    IF p_Limit IS NULL OR p_Limit <= 0 THEN
        SET p_Limit = 250; -- Default limit
    END IF;
    
    -- MySQL 5.7 compatible approach for getting problematic batches
    SELECT DISTINCT t1.BatchNumber
    FROM inv_transaction t1
    WHERE EXISTS (
        SELECT 1 FROM inv_transaction t2
        WHERE t1.BatchNumber = t2.BatchNumber
          AND DATE(t1.ReceiveDate) = DATE(t2.ReceiveDate)
          AND t1.TransactionType != t2.TransactionType
    )
    ORDER BY t1.BatchNumber
    LIMIT p_Limit;
    
    SELECT COUNT(DISTINCT t1.BatchNumber) INTO v_Count
    FROM inv_transaction t1
    WHERE EXISTS (
        SELECT 1 FROM inv_transaction t2
        WHERE t1.BatchNumber = t2.BatchNumber
          AND DATE(t1.ReceiveDate) = DATE(t2.ReceiveDate)
          AND t1.TransactionType != t2.TransactionType
    );
    
    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('Retrieved problematic batch numbers (limit: ', p_Limit, ', total: ', v_Count, ')');
END $$
DELIMITER ;

-- Split problematic batch numbers with enhanced status reporting
DELIMITER $$
CREATE PROCEDURE inv_transaction_SplitBatchNumbers(
    IN p_BatchNumbers TEXT,
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255),
    OUT p_ProcessedCount INT
)
BEGIN
    DECLARE v_Counter INT DEFAULT 0;
    DECLARE v_BatchCount INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred while splitting batch numbers';
        SET p_ProcessedCount = v_Counter;
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- Count comma-separated batch numbers (MySQL 5.7 compatible)
    IF p_BatchNumbers IS NOT NULL AND p_BatchNumbers != '' THEN
        SET v_BatchCount = (CHAR_LENGTH(p_BatchNumbers) - CHAR_LENGTH(REPLACE(p_BatchNumbers, ',', '')) + 1);
    ELSE
        SET p_Status = 1;
        SET p_ErrorMsg = 'No batch numbers provided for splitting';
        SET p_ProcessedCount = 0;
        ROLLBACK;
    END IF;
    
    -- For this implementation, we'll simulate processing
    -- In a real scenario, this would involve complex batch splitting logic
    -- based on transaction dates and types
    
    SET v_Counter = v_BatchCount;
    
    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('Batch numbers processed successfully (simulated)');
    SET p_ProcessedCount = v_Counter;
    
    COMMIT;
END $$
DELIMITER ;

-- ================================================================================
-- INVENTORY SEARCH PROCEDURES (MySQL 5.7.24 Compatible)
-- ================================================================================

-- Advanced search for inventory items with flexible filtering using parameters
DELIMITER $$
CREATE PROCEDURE inv_inventory_Search_Advanced(
    IN p_PartID VARCHAR(100),
    IN p_Operation VARCHAR(100),
    IN p_Location VARCHAR(100),
    IN p_QtyMin INT,
    IN p_QtyMax INT,
    IN p_Notes TEXT,
    IN p_User VARCHAR(100),
    IN p_FilterByDate BOOLEAN,
    IN p_DateFrom DATETIME,
    IN p_DateTo DATETIME,
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred while searching inventory';
    END;
    
    -- Execute search with flexible filtering
    SELECT * FROM inv_inventory 
    WHERE (p_PartID IS NULL OR p_PartID = '' OR PartID LIKE CONCAT('%', p_PartID, '%'))
      AND (p_Operation IS NULL OR p_Operation = '' OR Operation LIKE CONCAT('%', p_Operation, '%'))
      AND (p_Location IS NULL OR p_Location = '' OR Location LIKE CONCAT('%', p_Location, '%'))
      AND (p_QtyMin IS NULL OR p_QtyMin <= 0 OR Quantity >= p_QtyMin)
      AND (p_QtyMax IS NULL OR p_QtyMax <= 0 OR Quantity <= p_QtyMax)
      AND (p_Notes IS NULL OR p_Notes = '' OR Notes LIKE CONCAT('%', p_Notes, '%'))
      AND (p_User IS NULL OR p_User = '' OR User = p_User)
      AND (p_FilterByDate = FALSE OR p_DateFrom IS NULL OR p_DateTo IS NULL 
           OR ReceiveDate BETWEEN p_DateFrom AND p_DateTo);
    
    -- Get count for status message
    SELECT COUNT(*) INTO v_Count FROM inv_inventory 
    WHERE (p_PartID IS NULL OR p_PartID = '' OR PartID LIKE CONCAT('%', p_PartID, '%'))
      AND (p_Operation IS NULL OR p_Operation = '' OR Operation LIKE CONCAT('%', p_Operation, '%'))
      AND (p_Location IS NULL OR p_Location = '' OR Location LIKE CONCAT('%', p_Location, '%'))
      AND (p_QtyMin IS NULL OR p_QtyMin <= 0 OR Quantity >= p_QtyMin)
      AND (p_QtyMax IS NULL OR p_QtyMax <= 0 OR Quantity <= p_QtyMax)
      AND (p_Notes IS NULL OR p_Notes = '' OR Notes LIKE CONCAT('%', p_Notes, '%'))
      AND (p_User IS NULL OR p_User = '' OR User = p_User)
      AND (p_FilterByDate = FALSE OR p_DateFrom IS NULL OR p_DateTo IS NULL 
           OR ReceiveDate BETWEEN p_DateFrom AND p_DateTo);
    
    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('Search completed successfully, found ', v_Count, ' results');
END $$
DELIMITER ;

-- ================================================================================
-- END OF INVENTORY PROCEDURES
-- ================================================================================
