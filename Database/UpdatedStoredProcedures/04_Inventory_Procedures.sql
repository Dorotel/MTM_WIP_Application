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
DROP PROCEDURE IF EXISTS inv_transactions_Search;
DROP PROCEDURE IF EXISTS inv_transaction_Add;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `inv_inventory_Add_Item`(
    IN p_PartID VARCHAR(300),
    IN p_Location VARCHAR(100),
    IN p_Operation VARCHAR(100),
    IN p_Quantity INT,
    IN p_ItemType VARCHAR(100),
    IN p_User VARCHAR(100),
    IN p_BatchNumber VARCHAR(100),
    IN p_Notes VARCHAR(1000),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_NextBatchNumber VARCHAR(20);
    DECLARE v_ErrorMessage TEXT DEFAULT '';
    DECLARE v_CurrentMax INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        GET DIAGNOSTICS CONDITION 1 v_ErrorMessage = MESSAGE_TEXT;
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while adding inventory item for part: ', p_PartID);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    CREATE TABLE IF NOT EXISTS inv_inventory_batch_seq (
        last_batch_number INT(11) NOT NULL DEFAULT 0,
        created_date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
        updated_date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
    ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
    
    IF (SELECT COUNT(*) FROM inv_inventory_batch_seq) = 0 THEN
        SELECT COALESCE(MAX(CAST(CASE 
            WHEN BatchNumber IS NOT NULL AND BatchNumber REGEXP '^[0-9]+$' 
            THEN BatchNumber ELSE '0' END AS UNSIGNED)), 0) INTO v_CurrentMax FROM inv_inventory;
        INSERT INTO inv_inventory_batch_seq (last_batch_number) VALUES (v_CurrentMax);
    END IF;
    
    UPDATE inv_inventory_batch_seq SET last_batch_number = last_batch_number + 1 WHERE TRUE;
    SELECT last_batch_number INTO @nextBatch FROM inv_inventory_batch_seq LIMIT 1;
    SET v_NextBatchNumber = LPAD(@nextBatch, 10, '0');
    
    IF p_Quantity <= 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = 'Quantity must be greater than zero';
        ROLLBACK;
    ELSE
        INSERT INTO inv_inventory (PartID, Location, Operation, Quantity, ItemType, User, BatchNumber, Notes, ReceiveDate, LastUpdated)
        VALUES (p_PartID, p_Location, p_Operation, p_Quantity, p_ItemType, p_User, v_NextBatchNumber, p_Notes, NOW(), NOW());
        
        INSERT INTO inv_transaction (TransactionType, BatchNumber, PartID, FromLocation, ToLocation, Operation, Quantity, Notes, User, ItemType, ReceiveDate)
        VALUES ('IN', v_NextBatchNumber, p_PartID, p_Location, NULL, p_Operation, p_Quantity, p_Notes, p_User, p_ItemType, NOW());
        
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('New inventory batch created successfully for part: ', p_PartID, ' with batch: ', v_NextBatchNumber, ', quantity: ', p_Quantity);
        COMMIT;
    END IF;
END$$
DELIMITER ;

DELIMITER $$
CREATE PROCEDURE inv_inventory_Remove_Item(
    IN p_PartID VARCHAR(300),
    IN p_Location VARCHAR(100),
    IN p_Operation VARCHAR(100),
    IN p_Quantity INT,
    IN p_ItemType VARCHAR(100),
    IN p_User VARCHAR(100),
    IN p_BatchNumber VARCHAR(100),
    IN p_Notes VARCHAR(1000),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_RowsAffected INT DEFAULT 0;
    DECLARE v_ErrorMessage TEXT DEFAULT '';
    DECLARE v_RecordCount INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        GET DIAGNOSTICS CONDITION 1 v_ErrorMessage = MESSAGE_TEXT;
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while removing inventory item for part: ', p_PartID);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    IF p_Quantity <= 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = 'Quantity must be greater than zero';
        ROLLBACK;
    ELSE
        SELECT COUNT(*) INTO v_RecordCount FROM inv_inventory WHERE PartID = p_PartID AND Location = p_Location AND Operation = p_Operation;
          
        IF v_RecordCount = 0 THEN
            SET p_Status = 1;
            SET p_ErrorMsg = CONCAT('No inventory records found for PartID: ', p_PartID, ', Location: ', p_Location, ', Operation: ', p_Operation);
        ELSE
            DELETE FROM inv_inventory WHERE PartID = p_PartID AND Location = p_Location AND Operation = p_Operation AND Quantity = p_Quantity
                AND (p_BatchNumber IS NULL OR p_BatchNumber = '' OR BatchNumber = p_BatchNumber)
                AND (p_Notes IS NULL OR p_Notes = '' OR Notes IS NULL OR Notes = '' OR Notes = p_Notes) LIMIT 1;
            
            SET v_RowsAffected = ROW_COUNT();
            
            IF v_RowsAffected = 0 THEN
                DELETE FROM inv_inventory WHERE PartID = p_PartID AND Location = p_Location AND Operation = p_Operation AND Quantity = p_Quantity LIMIT 1;
                SET v_RowsAffected = ROW_COUNT();
            END IF;
            
            IF v_RowsAffected > 0 THEN
                INSERT INTO inv_transaction (TransactionType, PartID, FromLocation, Operation, Quantity, ItemType, User, BatchNumber, Notes, ReceiveDate)
                VALUES ('OUT', p_PartID, p_Location, p_Operation, p_Quantity, p_ItemType, p_User, p_BatchNumber, p_Notes, NOW());
                
                SET p_Status = 0;
                SET p_ErrorMsg = CONCAT('Inventory item removed successfully for part: ', p_PartID, ', quantity: ', p_Quantity);
            ELSE
                SET p_Status = 1;
                SET p_ErrorMsg = CONCAT('No matching inventory item found for removal. Found ', v_RecordCount, ' records for PartID: ', p_PartID, ', Location: ', p_Location, ', Operation: ', p_Operation, ' but none matched Quantity: ', p_Quantity);
            END IF;
        END IF;
        COMMIT;
    END IF;
END $$
DELIMITER ;

DELIMITER $$
CREATE PROCEDURE inv_inventory_Transfer_Part(
    IN p_BatchNumber VARCHAR(300),
    IN p_PartID VARCHAR(300),
    IN p_Operation VARCHAR(100),
    IN p_NewLocation VARCHAR(100),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_RowsAffected INT DEFAULT 0;
    DECLARE v_ErrorMessage TEXT DEFAULT '';
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        GET DIAGNOSTICS CONDITION 1 v_ErrorMessage = MESSAGE_TEXT;
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while transferring part: ', p_PartID);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    IF EXISTS (SELECT 1 FROM inv_inventory WHERE BatchNumber = p_BatchNumber AND PartID = p_PartID AND Operation = p_Operation) THEN
        UPDATE inv_inventory SET Location = p_NewLocation, LastUpdated = NOW() WHERE BatchNumber = p_BatchNumber AND PartID = p_PartID AND Operation = p_Operation;
        SET v_RowsAffected = ROW_COUNT();
        
        IF v_RowsAffected > 0 THEN
            SET p_Status = 0;
            SET p_ErrorMsg = CONCAT('Part transferred successfully to ', p_NewLocation);
        ELSE
            SET p_Status = 2;
            SET p_ErrorMsg = CONCAT('No transfer occurred for part: ', p_PartID);
        END IF;
    ELSE
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('No inventory found for part: ', p_PartID, ', batch: ', p_BatchNumber, ', operation: ', p_Operation);
    END IF;
    COMMIT;
END $$
DELIMITER ;

DELIMITER $$
CREATE PROCEDURE inv_inventory_transfer_quantity(
    IN p_BatchNumber VARCHAR(300),
    IN p_PartID VARCHAR(300),
    IN p_Operation VARCHAR(100),
    IN p_TransferQuantity INT,
    IN p_OriginalQuantity INT,
    IN p_NewLocation VARCHAR(100),
    IN p_User VARCHAR(100),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_ErrorMessage TEXT DEFAULT '';
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        GET DIAGNOSTICS CONDITION 1 v_ErrorMessage = MESSAGE_TEXT;
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while transferring quantity for part: ', p_PartID);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    IF p_TransferQuantity <= 0 OR p_TransferQuantity > p_OriginalQuantity THEN
        SET p_Status = 1;
        SET p_ErrorMsg = 'Invalid transfer quantity';
        ROLLBACK;
    ELSE
        UPDATE inv_inventory SET Quantity = Quantity - p_TransferQuantity, LastUpdated = NOW()
        WHERE BatchNumber = p_BatchNumber AND PartID = p_PartID AND Operation = p_Operation AND Quantity = p_OriginalQuantity;
        
        INSERT INTO inv_inventory (BatchNumber, PartID, Operation, Quantity, Location, User, ItemType, ReceiveDate, LastUpdated)
        SELECT p_BatchNumber, p_PartID, p_Operation, p_TransferQuantity, p_NewLocation, p_User, ItemType, NOW(), NOW()
        FROM inv_inventory WHERE BatchNumber = p_BatchNumber AND PartID = p_PartID AND Operation = p_Operation LIMIT 1;
        
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Quantity transferred successfully from original location to ', p_NewLocation);
        COMMIT;
    END IF;
END $$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `inv_inventory_GetNextBatchNumber`(
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_ErrorMessage TEXT DEFAULT '';
    DECLARE v_NextBatchNumber VARCHAR(20) DEFAULT '0000000001';
    DECLARE v_CurrentMax INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        GET DIAGNOSTICS CONDITION 1 v_ErrorMessage = MESSAGE_TEXT;
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred while generating next batch number';
        INSERT IGNORE INTO log_error (User, Severity, ErrorType, ErrorMessage, StackTrace, ModuleName, MethodName, AdditionalInfo, MachineName, OSVersion, AppVersion, ErrorTime)
        VALUES ('SYSTEM', 'Error', 'DatabaseError', CONCAT('inv_inventory_GetNextBatchNumber failed: ', v_ErrorMessage), v_ErrorMessage,
        'inv_inventory_GetNextBatchNumber', 'GetNextBatchNumber', 'Error generating next batch number', 'Database Server', 'MySQL 5.7.24', '5.0.1.2', NOW());
    END;
    
    CREATE TABLE IF NOT EXISTS inv_inventory_batch_seq (
        last_batch_number INT(11) NOT NULL DEFAULT 0,
        created_date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
        updated_date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
    ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
    
    IF (SELECT COUNT(*) FROM inv_inventory_batch_seq) = 0 THEN
        SELECT COALESCE(MAX(CAST(CASE WHEN BatchNumber IS NOT NULL AND BatchNumber REGEXP '^[0-9]+$' 
        THEN BatchNumber ELSE '0' END AS UNSIGNED)), 0) INTO v_CurrentMax FROM inv_inventory;
        INSERT INTO inv_inventory_batch_seq (last_batch_number) VALUES (v_CurrentMax);
    END IF;
    
    SELECT LPAD(COALESCE(last_batch_number + 1, 1), 10, '0') INTO v_NextBatchNumber FROM inv_inventory_batch_seq LIMIT 1;
    
    IF v_NextBatchNumber IS NULL OR v_NextBatchNumber = '0000000000' THEN
        SELECT LPAD(COALESCE(MAX(CAST(CASE WHEN BatchNumber IS NOT NULL AND BatchNumber REGEXP '^[0-9]+$' 
        THEN BatchNumber ELSE '0' END AS UNSIGNED)), 0) + 1, 10, '0') INTO v_NextBatchNumber FROM inv_inventory;
        
        IF v_NextBatchNumber IS NULL OR v_NextBatchNumber = '0000000000' THEN
            SET v_NextBatchNumber = '0000000001';
        END IF;
    END IF;
    
    SELECT v_NextBatchNumber as NextBatchNumber;
    SET p_Status = 0;
    SET p_ErrorMsg = 'Next batch number generated successfully';
END$$
DELIMITER ;

DELIMITER $$
CREATE PROCEDURE inv_inventory_Get_ByPartID(
    IN p_PartID VARCHAR(100),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_ErrorMessage TEXT DEFAULT '';
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        GET DIAGNOSTICS CONDITION 1 v_ErrorMessage = MESSAGE_TEXT;
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while retrieving inventory for part: ', p_PartID);
    END;
    
    SELECT COUNT(*) INTO v_Count FROM inv_inventory WHERE PartID = p_PartID;
    SELECT * FROM inv_inventory WHERE PartID = p_PartID ORDER BY Location, Operation;
    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('Retrieved ', v_Count, ' inventory records for part: ', p_PartID);
END $$
DELIMITER ;

DELIMITER $$
CREATE PROCEDURE inv_inventory_Get_ByPartIDAndOperation(
    IN p_PartID VARCHAR(100),
    IN p_Operation VARCHAR(100),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_ErrorMessage TEXT DEFAULT '';
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        GET DIAGNOSTICS CONDITION 1 v_ErrorMessage = MESSAGE_TEXT;
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while retrieving inventory for part: ', p_PartID, ', operation: ', p_Operation);
    END;
    
    SELECT COUNT(*) INTO v_Count FROM inv_inventory WHERE PartID = p_PartID AND Operation = p_Operation;
    SELECT * FROM inv_inventory WHERE PartID = p_PartID AND Operation = p_Operation ORDER BY Location;
    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('Retrieved ', v_Count, ' inventory records for part: ', p_PartID, ', operation: ', p_Operation);
END $$
DELIMITER ;

DELIMITER $$
CREATE PROCEDURE inv_inventory_Get_All(
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_ErrorMessage TEXT DEFAULT '';
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        GET DIAGNOSTICS CONDITION 1 v_ErrorMessage = MESSAGE_TEXT;
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred while retrieving all inventory';
    END;
    
    SELECT COUNT(*) INTO v_Count FROM inv_inventory;
    SELECT * FROM inv_inventory ORDER BY PartID, Location, Operation;
    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('Retrieved ', v_Count, ' inventory records successfully');
END $$
DELIMITER ;

DELIMITER $$
CREATE PROCEDURE inv_transaction_Add(
    IN p_TransactionType ENUM('IN','OUT','TRANSFER'),
    IN p_PartID VARCHAR(300),
    IN p_BatchNumber VARCHAR(100),
    IN p_FromLocation VARCHAR(300),
    IN p_ToLocation VARCHAR(100),
    IN p_Operation VARCHAR(100),
    IN p_Quantity INT,
    IN p_Notes VARCHAR(1000),
    IN p_User VARCHAR(100),
    IN p_ItemType VARCHAR(100),
    IN p_ReceiveDate DATETIME,
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_ErrorMessage TEXT DEFAULT '';
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        GET DIAGNOSTICS CONDITION 1 v_ErrorMessage = MESSAGE_TEXT;
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while adding transaction: ', v_ErrorMessage);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    INSERT INTO inv_transaction (TransactionType, PartID, BatchNumber, FromLocation, ToLocation, Operation, Quantity, Notes, User, ItemType, ReceiveDate)
    VALUES (p_TransactionType, p_PartID, p_BatchNumber, p_FromLocation, p_ToLocation, p_Operation, p_Quantity, p_Notes, p_User, p_ItemType, p_ReceiveDate);
    
    SET p_Status = 0;
    SET p_ErrorMsg = 'Transaction added successfully';
    COMMIT;
END $$
DELIMITER ;

DELIMITER $$
CREATE PROCEDURE inv_transactions_Search(
    IN p_UserName VARCHAR(100),
    IN p_IsAdmin BOOLEAN,
    IN p_PartID VARCHAR(300),
    IN p_BatchNumber VARCHAR(50),
    IN p_FromLocation VARCHAR(100),
    IN p_ToLocation VARCHAR(100),
    IN p_Operation VARCHAR(100),
    IN p_TransactionType VARCHAR(20),
    IN p_Quantity INT,
    IN p_Notes TEXT,
    IN p_ItemType VARCHAR(100),
    IN p_FromDate DATETIME,
    IN p_ToDate DATETIME,
    IN p_SortColumn VARCHAR(50),
    IN p_SortDescending BOOLEAN,
    IN p_Page INT,
    IN p_PageSize INT,
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_ErrorMessage VARCHAR(255) DEFAULT '';
    DECLARE v_Offset INT DEFAULT 0;
    DECLARE v_OrderBy VARCHAR(100) DEFAULT '';
    DECLARE v_SortDirection VARCHAR(4) DEFAULT 'ASC';
    DECLARE v_WhereClause TEXT DEFAULT '';
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        GET DIAGNOSTICS CONDITION 1 v_ErrorMessage = MESSAGE_TEXT;
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Transaction search failed: ', v_ErrorMessage);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    IF p_Page < 1 THEN SET p_Page = 1; END IF;
    IF p_PageSize < 1 OR p_PageSize > 1000 THEN SET p_PageSize = 20; END IF;
    SET v_Offset = (p_Page - 1) * p_PageSize;
    SET v_SortDirection = IF(p_SortDescending = TRUE, 'DESC', 'ASC');
    
    CASE LOWER(COALESCE(p_SortColumn, 'receivedate'))
        WHEN 'receivedate' THEN SET v_OrderBy = CONCAT('ReceiveDate ', v_SortDirection);
        WHEN 'quantity' THEN SET v_OrderBy = CONCAT('Quantity ', v_SortDirection);
        WHEN 'user' THEN SET v_OrderBy = CONCAT('User ', v_SortDirection);
        WHEN 'itemtype' THEN SET v_OrderBy = CONCAT('ItemType ', v_SortDirection);
        WHEN 'partid' THEN SET v_OrderBy = CONCAT('PartID ', v_SortDirection);
        WHEN 'operation' THEN SET v_OrderBy = CONCAT('Operation ', v_SortDirection);
        ELSE SET v_OrderBy = CONCAT('ReceiveDate ', v_SortDirection);
    END CASE;
    
    SET v_WhereClause = 'WHERE 1=1 ';
    
    IF NOT p_IsAdmin AND p_UserName IS NOT NULL AND LENGTH(TRIM(p_UserName)) > 0 THEN
        SET v_WhereClause = CONCAT(v_WhereClause, 'AND User = ''', REPLACE(p_UserName, '''', ''''''), ''' ');
    END IF;
    
    IF p_PartID IS NOT NULL AND LENGTH(TRIM(p_PartID)) > 0 THEN
        SET v_WhereClause = CONCAT(v_WhereClause, 'AND PartID = ''', REPLACE(p_PartID, '''', ''''''), ''' ');
    END IF;
    
    IF p_BatchNumber IS NOT NULL AND LENGTH(TRIM(p_BatchNumber)) > 0 THEN
        SET v_WhereClause = CONCAT(v_WhereClause, 'AND BatchNumber = ''', REPLACE(p_BatchNumber, '''', ''''''), ''' ');
    END IF;
    
    IF p_FromLocation IS NOT NULL AND LENGTH(TRIM(p_FromLocation)) > 0 THEN
        SET v_WhereClause = CONCAT(v_WhereClause, 'AND FromLocation = ''', REPLACE(p_FromLocation, '''', ''''''), ''' ');
    END IF;
    
    IF p_ToLocation IS NOT NULL AND LENGTH(TRIM(p_ToLocation)) > 0 THEN
        SET v_WhereClause = CONCAT(v_WhereClause, 'AND ToLocation = ''', REPLACE(p_ToLocation, '''', ''''''), ''' ');
    END IF;
    
    IF p_Operation IS NOT NULL AND LENGTH(TRIM(p_Operation)) > 0 THEN
        SET v_WhereClause = CONCAT(v_WhereClause, 'AND Operation = ''', REPLACE(p_Operation, '''', ''''''), ''' ');
    END IF;
    
    IF p_TransactionType IS NOT NULL AND LENGTH(TRIM(p_TransactionType)) > 0 THEN
        SET v_WhereClause = CONCAT(v_WhereClause, 'AND TransactionType = ''', REPLACE(p_TransactionType, '''', ''''''), ''' ');
    END IF;
    
    IF p_Quantity IS NOT NULL THEN
        SET v_WhereClause = CONCAT(v_WhereClause, 'AND Quantity = ', p_Quantity, ' ');
    END IF;
    
    IF p_Notes IS NOT NULL AND LENGTH(TRIM(p_Notes)) > 0 THEN
        SET v_WhereClause = CONCAT(v_WhereClause, 'AND Notes LIKE ''%', REPLACE(p_Notes, '''', ''''''), '%'' ');
    END IF;
    
    IF p_ItemType IS NOT NULL AND LENGTH(TRIM(p_ItemType)) > 0 THEN
        SET v_WhereClause = CONCAT(v_WhereClause, 'AND ItemType = ''', REPLACE(p_ItemType, '''', ''''''), ''' ');
    END IF;
    
    IF p_FromDate IS NOT NULL THEN
        SET v_WhereClause = CONCAT(v_WhereClause, 'AND ReceiveDate >= ''', p_FromDate, ''' ');
    END IF;
    
    IF p_ToDate IS NOT NULL THEN
        SET v_WhereClause = CONCAT(v_WhereClause, 'AND ReceiveDate <= ''', p_ToDate, ''' ');
    END IF;
    
    SET @sql = CONCAT(
        'SELECT ID, TransactionType, BatchNumber, PartID, FromLocation, ToLocation, ',
        'Operation, Quantity, Notes, User, ItemType, ReceiveDate ',
        'FROM inv_transaction ',
        v_WhereClause,
        'ORDER BY ', v_OrderBy, ' ',
        'LIMIT ', v_Offset, ', ', p_PageSize
    );
    
    PREPARE stmt FROM @sql;
    EXECUTE stmt;
    DEALLOCATE PREPARE stmt;
    COMMIT;
    
    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('Transaction search completed successfully for user: ', COALESCE(p_UserName, 'ALL'));
END$$
DELIMITER ;

DELIMITER $$
CREATE PROCEDURE inv_transactions_SmartSearch(
    IN p_WhereClause TEXT,
    IN p_Page INT,
    IN p_PageSize INT,
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_ErrorMessage VARCHAR(255) DEFAULT '';
    DECLARE v_Offset INT DEFAULT 0;
    DECLARE v_FinalWhereClause TEXT DEFAULT '';
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        GET DIAGNOSTICS CONDITION 1 v_ErrorMessage = MESSAGE_TEXT;
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Smart search failed: ', v_ErrorMessage);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- Parameter validation
    IF p_Page < 1 THEN SET p_Page = 1; END IF;
    IF p_PageSize < 1 OR p_PageSize > 1000 THEN SET p_PageSize = 20; END IF;
    SET v_Offset = (p_Page - 1) * p_PageSize;
    
    -- Build final WHERE clause
    IF p_WhereClause IS NOT NULL AND LENGTH(TRIM(p_WhereClause)) > 0 THEN
        SET v_FinalWhereClause = CONCAT('WHERE ', TRIM(p_WhereClause));
    ELSE
        SET v_FinalWhereClause = 'WHERE 1=1';
    END IF;
    
    -- Build and execute dynamic query
    SET @sql = CONCAT(
        'SELECT ID, TransactionType, BatchNumber, PartID, FromLocation, ToLocation, ',
        'Operation, Quantity, Notes, User, ItemType, ReceiveDate ',
        'FROM inv_transaction ',
        v_FinalWhereClause, ' ',
        'ORDER BY ReceiveDate DESC ',
        'LIMIT ', v_Offset, ', ', p_PageSize
    );
    
    PREPARE stmt FROM @sql;
    EXECUTE stmt;
    DEALLOCATE PREPARE stmt;
    COMMIT;
    
    SET p_Status = 0;
    SET p_ErrorMsg = 'Smart search completed successfully';
END$$
DELIMITER ;

DELIMITER $$
CREATE PROCEDURE inv_transactions_GetAnalytics(
    IN p_UserName VARCHAR(100),
    IN p_IsAdmin BOOLEAN,
    IN p_FromDate DATETIME,
    IN p_ToDate DATETIME,
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_ErrorMessage VARCHAR(255) DEFAULT '';
    DECLARE v_WhereClause TEXT DEFAULT '';
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        GET DIAGNOSTICS CONDITION 1 v_ErrorMessage = MESSAGE_TEXT;
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Analytics generation failed: ', v_ErrorMessage);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    SET v_WhereClause = 'WHERE 1=1 ';
    
    IF NOT p_IsAdmin AND p_UserName IS NOT NULL AND LENGTH(TRIM(p_UserName)) > 0 THEN
        SET v_WhereClause = CONCAT(v_WhereClause, 'AND User = ''', REPLACE(p_UserName, '''', ''''''), ''' ');
    END IF;
    
    IF p_FromDate IS NOT NULL THEN
        SET v_WhereClause = CONCAT(v_WhereClause, 'AND ReceiveDate >= ''', p_FromDate, ''' ');
    END IF;
    
    IF p_ToDate IS NOT NULL THEN
        SET v_WhereClause = CONCAT(v_WhereClause, 'AND ReceiveDate <= ''', p_ToDate, ''' ');
    END IF;
    
    SET @sql = CONCAT(
        'SELECT ',
        '    COUNT(*) as TotalTransactions, ',
        '    SUM(CASE WHEN TransactionType = ''IN'' THEN 1 ELSE 0 END) as InTransactions, ',
        '    SUM(CASE WHEN TransactionType = ''OUT'' THEN 1 ELSE 0 END) as OutTransactions, ',
        '    SUM(CASE WHEN TransactionType = ''TRANSFER'' THEN 1 ELSE 0 END) as TransferTransactions, ',
        '    COALESCE(SUM(Quantity), 0) as TotalQuantity, ',
        '    COUNT(DISTINCT PartID) as UniquePartIds, ',
        '    COUNT(DISTINCT User) as ActiveUsers, ',
        '    COALESCE((SELECT PartID FROM inv_transaction t2 ', v_WhereClause, ' GROUP BY PartID ORDER BY SUM(Quantity) DESC LIMIT 1), '''') as TopPartId, ',
        '    COALESCE((SELECT User FROM inv_transaction t3 ', v_WhereClause, ' GROUP BY User ORDER BY COUNT(*) DESC LIMIT 1), '''') as TopUser ',
        'FROM inv_transaction t1 ',
        v_WhereClause
    );
    
    PREPARE stmt FROM @sql;
    EXECUTE stmt;
    DEALLOCATE PREPARE stmt;
    COMMIT;
    
    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('Transaction analytics generated successfully for user: ', COALESCE(p_UserName, 'ALL'));
END$$
DELIMITER ;
