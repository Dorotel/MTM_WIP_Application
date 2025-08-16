
DELIMITER $$
--
-- Procedures
--
CREATE DEFINER=`root`@`localhost` PROCEDURE `app_themes_Add_Theme` (IN `p_ThemeName` VARCHAR(50), IN `p_DisplayName` VARCHAR(100), IN `p_SettingsJson` TEXT, IN `p_Description` TEXT, IN `p_CreatedBy` VARCHAR(100))   BEGIN
    DECLARE p_Status INT DEFAULT 0;
    DECLARE p_ErrorMsg VARCHAR(255) DEFAULT '';
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_ThemeId INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while creating theme: ', p_ThemeName);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    
    IF p_ThemeName IS NULL OR TRIM(p_ThemeName) = '' THEN
        SET p_Status = 1;
        SET p_ErrorMsg = 'Theme name cannot be empty';
        ROLLBACK;
    
    ELSEIF EXISTS(SELECT 1 FROM app_themes WHERE ThemeName = p_ThemeName) THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Theme already exists: ', p_ThemeName);
        ROLLBACK;
    
    ELSEIF p_SettingsJson IS NULL OR TRIM(p_SettingsJson) = '' THEN
        SET p_Status = 1;
        SET p_ErrorMsg = 'Theme settings JSON cannot be empty';
        ROLLBACK;
    ELSE
        INSERT INTO app_themes (
            ThemeName,
            DisplayName,
            SettingsJson,
            IsDefault,
            IsActive,
            Description,
            CreatedBy,
            CreatedDate
        ) VALUES (
            p_ThemeName,
            IFNULL(p_DisplayName, p_ThemeName),
            p_SettingsJson,
            0, 
            1, 
            p_Description,
            IFNULL(p_CreatedBy, 'SYSTEM'),
            NOW()
        );
        
        SET v_ThemeId = LAST_INSERT_ID();
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Theme created successfully: ', p_ThemeName, ' (ID: ', v_ThemeId, ')');
        COMMIT;
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `app_themes_Delete_Theme` (IN `p_ThemeName` VARCHAR(50), IN `p_ModifiedBy` VARCHAR(100))   BEGIN
    DECLARE p_Status INT DEFAULT 0;
    DECLARE p_ErrorMsg VARCHAR(255) DEFAULT '';
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while deleting theme: ', p_ThemeName);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    
    SELECT COUNT(*) INTO v_Count FROM app_themes WHERE ThemeName = p_ThemeName AND IsDefault = 0 AND IsActive = 1;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Theme not found, already deleted, or is a protected system theme: ', p_ThemeName);
        ROLLBACK;
    ELSE
        
        UPDATE app_themes 
        SET IsActive = 0,
            ModifiedBy = IFNULL(p_ModifiedBy, 'SYSTEM'),
            ModifiedDate = NOW()
        WHERE ThemeName = p_ThemeName AND IsDefault = 0;
        
        SET v_RowsAffected = ROW_COUNT();
        
        IF v_RowsAffected > 0 THEN
            SET p_Status = 0;
            SET p_ErrorMsg = CONCAT('Theme deleted successfully: ', p_ThemeName);
        ELSE
            SET p_Status = 2;
            SET p_ErrorMsg = CONCAT('Failed to delete theme: ', p_ThemeName);
        END IF;
        
        COMMIT;
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `app_themes_Exists` (IN `p_ThemeName` VARCHAR(50))   BEGIN
    DECLARE p_Status INT DEFAULT 0;
    DECLARE p_ErrorMsg VARCHAR(255) DEFAULT '';
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while checking theme existence: ', p_ThemeName);
    END;
    
    SELECT COUNT(*) INTO v_Count FROM app_themes WHERE ThemeName = p_ThemeName AND IsActive = 1;
    SELECT v_Count as ThemeExists;
    
    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('Theme existence check completed for: ', p_ThemeName, ' (Exists: ', IF(v_Count > 0, 'Yes', 'No'), ')');
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `app_themes_Get_All` (OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred while retrieving themes';
    END;
    
    
    SELECT * FROM app_themes ORDER BY ThemeName;
    
    SET p_Status = 0;
    SET p_ErrorMsg = 'Themes retrieved successfully';
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `app_themes_Get_ByName` (IN `p_ThemeName` VARCHAR(50), OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while retrieving theme: ', p_ThemeName);
    END;
    
    SELECT COUNT(*) INTO v_Count FROM app_themes WHERE ThemeName = p_ThemeName;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Theme not found: ', p_ThemeName);
        
        SELECT NULL as ThemeName, NULL as SettingsJson LIMIT 0;
    ELSE
        SELECT 
            ID,
            ThemeName,
            DisplayName,
            SettingsJson,
            IsDefault,
            IsActive,
            Description,
            CreatedDate,
            CreatedBy,
            ModifiedDate,
            ModifiedBy,
            VERSION
        FROM app_themes 
        WHERE ThemeName = p_ThemeName AND IsActive = 1
        LIMIT 1;
        
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Theme retrieved successfully: ', p_ThemeName);
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `app_themes_Get_UserTheme` (IN `p_UserId` VARCHAR(100))   BEGIN
    DECLARE p_Status INT DEFAULT 0;
    DECLARE p_ErrorMsg VARCHAR(255) DEFAULT '';
    DECLARE v_ThemeName VARCHAR(50) DEFAULT NULL;
    DECLARE v_ThemeExists INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while retrieving user theme for: ', p_UserId);
    END;
    
    
    SELECT Theme_Name INTO v_ThemeName 
    FROM usr_users 
    WHERE User = p_UserId 
    LIMIT 1;
    
    
    IF v_ThemeName IS NULL OR v_ThemeName = '' THEN
        SET v_ThemeName = 'Default';
    ELSE
        
        SELECT COUNT(*) INTO v_ThemeExists 
        FROM app_themes 
        WHERE ThemeName = v_ThemeName AND IsActive = 1;
        
        
        IF v_ThemeExists = 0 THEN
            SET v_ThemeName = 'Default';
        END IF;
    END IF;
    
    
    SELECT 
        t.ID,
        t.ThemeName,
        t.DisplayName,
        t.SettingsJson,
        t.IsDefault,
        t.Description
    FROM app_themes t
    WHERE t.ThemeName = v_ThemeName AND t.IsActive = 1
    LIMIT 1;
    
    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('User theme retrieved successfully for: ', p_UserId, ' (Theme: ', v_ThemeName, ')');
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `app_themes_Set_UserTheme` (IN `p_UserId` VARCHAR(100), IN `p_ThemeName` VARCHAR(50))   BEGIN
    DECLARE p_Status INT DEFAULT 0;
    DECLARE p_ErrorMsg VARCHAR(255) DEFAULT '';
    DECLARE v_UserExists INT DEFAULT 0;
    DECLARE v_ThemeExists INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while setting theme for user: ', p_UserId);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    
    SELECT COUNT(*) INTO v_UserExists FROM usr_users WHERE User = p_UserId;
    
    IF v_UserExists = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('User not found: ', p_UserId);
        ROLLBACK;
    ELSE
        
        SELECT COUNT(*) INTO v_ThemeExists FROM app_themes WHERE ThemeName = p_ThemeName AND IsActive = 1;
        
        IF v_ThemeExists = 0 THEN
            SET p_Status = 1;
            SET p_ErrorMsg = CONCAT('Theme not found or inactive: ', p_ThemeName);
            ROLLBACK;
        ELSE
            
            UPDATE usr_users 
            SET Theme_Name = p_ThemeName,
                ModifiedDate = NOW()
            WHERE User = p_UserId;
            
            SET v_RowsAffected = ROW_COUNT();
            
            IF v_RowsAffected > 0 THEN
                SET p_Status = 0;
                SET p_ErrorMsg = CONCAT('Theme set successfully for user: ', p_UserId, ' (Theme: ', p_ThemeName, ')');
            ELSE
                SET p_Status = 2;
                SET p_ErrorMsg = CONCAT('No changes made for user: ', p_UserId);
            END IF;
            
            COMMIT;
        END IF;
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `app_themes_Update_Theme` (IN `p_ThemeName` VARCHAR(50), IN `p_DisplayName` VARCHAR(100), IN `p_SettingsJson` TEXT, IN `p_Description` TEXT, IN `p_ModifiedBy` VARCHAR(100))   BEGIN
    DECLARE p_Status INT DEFAULT 0;
    DECLARE p_ErrorMsg VARCHAR(255) DEFAULT '';
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while updating theme: ', p_ThemeName);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    
    SELECT COUNT(*) INTO v_Count FROM app_themes WHERE ThemeName = p_ThemeName AND IsDefault = 0;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Theme not found or is a protected system theme: ', p_ThemeName);
        ROLLBACK;
    ELSE
        UPDATE app_themes 
        SET DisplayName = IFNULL(p_DisplayName, DisplayName),
            SettingsJson = IFNULL(p_SettingsJson, SettingsJson),
            Description = p_Description,
            ModifiedBy = IFNULL(p_ModifiedBy, 'SYSTEM'),
            ModifiedDate = NOW(),
            VERSION = VERSION + 1
        WHERE ThemeName = p_ThemeName AND IsDefault = 0;
        
        SET v_RowsAffected = ROW_COUNT();
        
        IF v_RowsAffected > 0 THEN
            SET p_Status = 0;
            SET p_ErrorMsg = CONCAT('Theme updated successfully: ', p_ThemeName);
        ELSE
            SET p_Status = 2;
            SET p_ErrorMsg = CONCAT('No changes made to theme: ', p_ThemeName);
        END IF;
        
        COMMIT;
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `inv_inventory_Add_Item` (IN `p_PartID` VARCHAR(300), IN `p_Location` VARCHAR(100), IN `p_Operation` VARCHAR(100), IN `p_Quantity` INT, IN `p_ItemType` VARCHAR(100), IN `p_User` VARCHAR(100), IN `p_BatchNumber` VARCHAR(100), IN `p_Notes` VARCHAR(1000), OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_NextBatchNumber VARCHAR(20);
    DECLARE v_ErrorMessage TEXT DEFAULT '';
    DECLARE v_CurrentMax INT DEFAULT 0;
    
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
    
    -- FIXED: Initialize sequence table only once if completely empty
    IF (SELECT COUNT(*) FROM inv_inventory_batch_seq) = 0 THEN
        -- Get the current maximum batch number from existing inventory
        SELECT COALESCE(MAX(CAST(CASE 
            WHEN BatchNumber IS NOT NULL AND BatchNumber REGEXP '^[0-9]+$' 
            THEN BatchNumber 
            ELSE '0' 
        END AS UNSIGNED)), 0) INTO v_CurrentMax
        FROM inv_inventory;
        
        -- Insert initial record with current max
        INSERT INTO inv_inventory_batch_seq (last_batch_number) VALUES (v_CurrentMax);
    END IF;
    
    -- FIXED: Atomic increment and retrieval
    UPDATE inv_inventory_batch_seq 
    SET last_batch_number = last_batch_number + 1
    WHERE TRUE;
    
    -- Get the newly incremented value
    SELECT last_batch_number INTO @nextBatch FROM inv_inventory_batch_seq LIMIT 1;
    SET v_NextBatchNumber = LPAD(@nextBatch, 10, '0');
    
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
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `inv_inventory_Fix_BatchNumbers` (OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_ProcessedRecords INT DEFAULT 0;
    DECLARE v_ErrorMessage TEXT DEFAULT '';
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        GET DIAGNOSTICS CONDITION 1
            v_ErrorMessage = MESSAGE_TEXT;
        
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred while fixing batch numbers';
        
        -- Log error to log_error table
        INSERT INTO log_error (
            User, Severity, ErrorType, ErrorMessage, StackTrace,
            ModuleName, MethodName, AdditionalInfo, MachineName,
            OSVersion, AppVersion, ErrorTime
        ) VALUES (
            'SYSTEM', 'Error', 'DatabaseError',
            CONCAT('inv_inventory_Fix_BatchNumbers failed: ', v_ErrorMessage),
            v_ErrorMessage,
            'inv_inventory_Fix_BatchNumbers', 'Fix_BatchNumbers',
            'Error during batch number consolidation',
            'Database Server', 'MySQL 5.7.24', '5.0.1.2', NOW()
        );
        
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
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `inv_inventory_GetNextBatchNumber` (OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_ErrorMessage TEXT DEFAULT '';
    DECLARE v_NextBatchNumber VARCHAR(20) DEFAULT '0000000001';
    DECLARE v_CurrentMax INT DEFAULT 0;
    
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
    
    -- FIXED: Initialize sequence table only once if completely empty
    IF (SELECT COUNT(*) FROM inv_inventory_batch_seq) = 0 THEN
        -- Get the current maximum batch number from existing inventory
        SELECT COALESCE(MAX(CAST(CASE 
            WHEN BatchNumber IS NOT NULL AND BatchNumber REGEXP '^[0-9]+$' 
            THEN BatchNumber 
            ELSE '0' 
        END AS UNSIGNED)), 0) INTO v_CurrentMax
        FROM inv_inventory;
        
        -- Insert initial record with current max
        INSERT INTO inv_inventory_batch_seq (last_batch_number) VALUES (v_CurrentMax);
    END IF;
    
    -- FIXED: Just peek at the next number without incrementing
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
    
    -- Return the generated batch number (DON'T update the sequence here - let Add_Item do it)
    SELECT v_NextBatchNumber as NextBatchNumber;
    
    SET p_Status = 0;
    SET p_ErrorMsg = 'Next batch number generated successfully';
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `inv_inventory_Get_All` (OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_ErrorMessage TEXT DEFAULT '';
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        GET DIAGNOSTICS CONDITION 1
            v_ErrorMessage = MESSAGE_TEXT;
        
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred while retrieving all inventory';
        
        -- Log error to log_error table
        INSERT INTO log_error (
            User, Severity, ErrorType, ErrorMessage, StackTrace,
            ModuleName, MethodName, AdditionalInfo, MachineName,
            OSVersion, AppVersion, ErrorTime
        ) VALUES (
            'SYSTEM', 'Error', 'DatabaseError',
            CONCAT('inv_inventory_Get_All failed: ', v_ErrorMessage),
            v_ErrorMessage,
            'inv_inventory_Get_All', 'Get_All',
            'Error retrieving all inventory records',
            'Database Server', 'MySQL 5.7.24', '5.0.1.2', NOW()
        );
    END;
    
    SELECT COUNT(*) INTO v_Count FROM inv_inventory;
    SELECT * FROM inv_inventory ORDER BY PartID, Location, Operation;
    
    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('Retrieved ', v_Count, ' inventory records successfully');
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `inv_inventory_Get_ByPartID` (IN `p_PartID` VARCHAR(100), OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_ErrorMessage TEXT DEFAULT '';
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        GET DIAGNOSTICS CONDITION 1
            v_ErrorMessage = MESSAGE_TEXT;
        
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while retrieving inventory for part: ', p_PartID);
        
        -- Log error to log_error table
        INSERT INTO log_error (
            User, Severity, ErrorType, ErrorMessage, StackTrace,
            ModuleName, MethodName, AdditionalInfo, MachineName,
            OSVersion, AppVersion, ErrorTime
        ) VALUES (
            'SYSTEM', 'Error', 'DatabaseError',
            CONCAT('inv_inventory_Get_ByPartID failed: ', v_ErrorMessage),
            v_ErrorMessage,
            'inv_inventory_Get_ByPartID', 'Get_ByPartID',
            'PartID: ', p_PartID,
            'Database Server', 'MySQL 5.7.24', '5.0.1.2', NOW()
        );
    END;
    
    SELECT COUNT(*) INTO v_Count FROM inv_inventory WHERE PartID = p_PartID;
    SELECT * FROM inv_inventory WHERE PartID = p_PartID ORDER BY Location, Operation;
    
    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('Retrieved ', v_Count, ' inventory records for part: ', p_PartID);
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `inv_inventory_Get_ByPartIDAndOperation` (IN `p_PartID` VARCHAR(100), IN `p_Operation` VARCHAR(100), OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_ErrorMessage TEXT DEFAULT '';
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        GET DIAGNOSTICS CONDITION 1
            v_ErrorMessage = MESSAGE_TEXT;
        
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while retrieving inventory for part: ', p_PartID, ', operation: ', p_Operation);
        
        -- Log error to log_error table
        INSERT INTO log_error (
            User, Severity, ErrorType, ErrorMessage, StackTrace,
            ModuleName, MethodName, AdditionalInfo, MachineName,
            OSVersion, AppVersion, ErrorTime
        ) VALUES (
            'SYSTEM', 'Error', 'DatabaseError',
            CONCAT('inv_inventory_Get_ByPartIDAndOperation failed: ', v_ErrorMessage),
            v_ErrorMessage,
            'inv_inventory_Get_ByPartIDAndOperation', 'Get_ByPartIDAndOperation',
            'PartID: ', p_PartID, ', Operation: ', p_Operation,
            'Database Server', 'MySQL 5.7.24', '5.0.1.2', NOW()
        );
    END;
    
    SELECT COUNT(*) INTO v_Count 
    FROM inv_inventory 
    WHERE PartID = p_PartID AND Operation = p_Operation;
    
    SELECT * FROM inv_inventory 
    WHERE PartID = p_PartID AND Operation = p_Operation 
    ORDER BY Location;
    
    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('Retrieved ', v_Count, ' inventory records for part: ', p_PartID, ', operation: ', p_Operation);
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `inv_inventory_Remove_Item` (IN `p_PartID` VARCHAR(300), IN `p_Location` VARCHAR(100), IN `p_Operation` VARCHAR(100), IN `p_Quantity` INT, IN `p_ItemType` VARCHAR(100), IN `p_User` VARCHAR(100), IN `p_BatchNumber` VARCHAR(100), IN `p_Notes` VARCHAR(1000), OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_RowsAffected INT DEFAULT 0;
    DECLARE v_ErrorMessage TEXT DEFAULT '';
    DECLARE v_RecordCount INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        GET DIAGNOSTICS CONDITION 1
            v_ErrorMessage = MESSAGE_TEXT;
        
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
        -- ENHANCED: Check what records exist first for debugging
        SELECT COUNT(*) INTO v_RecordCount 
        FROM inv_inventory
        WHERE PartID = p_PartID
          AND Location = p_Location
          AND Operation = p_Operation;
          
        IF v_RecordCount = 0 THEN
            SET p_Status = 1;
            SET p_ErrorMsg = CONCAT('No inventory records found for PartID: ', p_PartID, ', Location: ', p_Location, ', Operation: ', p_Operation);
        ELSE
            -- FLEXIBLE: Try to match by primary fields first, then be more specific if multiple records
            DELETE FROM inv_inventory
            WHERE PartID = p_PartID
              AND Location = p_Location
              AND Operation = p_Operation
              AND Quantity = p_Quantity
              -- RELAXED: Only match BatchNumber if it's provided and not empty
              AND (p_BatchNumber IS NULL OR p_BatchNumber = '' OR BatchNumber = p_BatchNumber)
              -- RELAXED: Be flexible with Notes comparison
              AND (p_Notes IS NULL OR p_Notes = '' OR Notes IS NULL OR Notes = '' OR Notes = p_Notes)
            LIMIT 1;
            
            SET v_RowsAffected = ROW_COUNT();
            
            -- If exact match didn't work, try more flexible matching
            IF v_RowsAffected = 0 THEN
                DELETE FROM inv_inventory
                WHERE PartID = p_PartID
                  AND Location = p_Location  
                  AND Operation = p_Operation
                  AND Quantity = p_Quantity
                LIMIT 1;
                
                SET v_RowsAffected = ROW_COUNT();
            END IF;
            
            -- FIXED: Correct status logic (0 = success, 1 = not found)
            IF v_RowsAffected > 0 THEN
                -- Insert transaction record for audit trail
                INSERT INTO inv_transaction (
                    TransactionType, PartID, FromLocation, Operation, 
                    Quantity, ItemType, User, BatchNumber, Notes, ReceiveDate
                ) VALUES (
                    'OUT', p_PartID, p_Location, p_Operation,
                    p_Quantity, p_ItemType, p_User, p_BatchNumber, p_Notes, NOW()
                );
                
                SET p_Status = 0;  -- ? SUCCESS
                SET p_ErrorMsg = CONCAT('Inventory item removed successfully for part: ', p_PartID, ', quantity: ', p_Quantity);
            ELSE
                SET p_Status = 1;  -- ? NOT FOUND
                SET p_ErrorMsg = CONCAT('No matching inventory item found for removal. Found ', v_RecordCount, ' records for PartID: ', p_PartID, ', Location: ', p_Location, ', Operation: ', p_Operation, ' but none matched Quantity: ', p_Quantity);
            END IF;
        END IF;
        
        COMMIT;
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `inv_inventory_Search_Advanced` (IN `p_PartID` VARCHAR(100), IN `p_Operation` VARCHAR(100), IN `p_Location` VARCHAR(100), IN `p_QtyMin` INT, IN `p_QtyMax` INT, IN `p_Notes` TEXT, IN `p_User` VARCHAR(100), IN `p_FilterByDate` BOOLEAN, IN `p_DateFrom` DATETIME, IN `p_DateTo` DATETIME, OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_ErrorMessage TEXT DEFAULT '';
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        GET DIAGNOSTICS CONDITION 1
            v_ErrorMessage = MESSAGE_TEXT;
        
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred while searching inventory';
        
        -- Log error to log_error table
        INSERT INTO log_error (
            User, Severity, ErrorType, ErrorMessage, StackTrace,
            ModuleName, MethodName, AdditionalInfo, MachineName,
            OSVersion, AppVersion, ErrorTime
        ) VALUES (
            COALESCE(p_User, 'SYSTEM'), 'Error', 'DatabaseError',
            CONCAT('inv_inventory_Search_Advanced failed: ', v_ErrorMessage),
            v_ErrorMessage,
            'inv_inventory_Search_Advanced', 'Search_Advanced',
            CONCAT('PartID: ', COALESCE(p_PartID, 'NULL'), ', Operation: ', COALESCE(p_Operation, 'NULL'), ', Location: ', COALESCE(p_Location, 'NULL')),
            'Database Server', 'MySQL 5.7.24', '5.0.1.2', NOW()
        );
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
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `inv_inventory_Transfer_Part` (IN `p_BatchNumber` VARCHAR(300), IN `p_PartID` VARCHAR(300), IN `p_Operation` VARCHAR(100), IN `p_NewLocation` VARCHAR(100), OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_RowsAffected INT DEFAULT 0;
    DECLARE v_ErrorMessage TEXT DEFAULT '';
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        GET DIAGNOSTICS CONDITION 1
            v_ErrorMessage = MESSAGE_TEXT;
        
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while transferring part: ', p_PartID);
        
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- Use the WORKING validation and update logic from CurrentStoredProcedures.sql
    IF EXISTS (
        SELECT 1 FROM inv_inventory
        WHERE BatchNumber = p_BatchNumber
          AND PartID = p_PartID
          AND Operation = p_Operation
    ) THEN
        -- Update the location (working logic)
        UPDATE inv_inventory
        SET Location = p_NewLocation,
            LastUpdated = NOW()
        WHERE BatchNumber = p_BatchNumber
          AND PartID = p_PartID
          AND Operation = p_Operation;
        
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
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `inv_inventory_transfer_quantity` (IN `p_BatchNumber` VARCHAR(300), IN `p_PartID` VARCHAR(300), IN `p_Operation` VARCHAR(100), IN `p_TransferQuantity` INT, IN `p_OriginalQuantity` INT, IN `p_NewLocation` VARCHAR(100), IN `p_User` VARCHAR(100), OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_ErrorMessage TEXT DEFAULT '';
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        GET DIAGNOSTICS CONDITION 1
            v_ErrorMessage = MESSAGE_TEXT;
        
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while transferring quantity for part: ', p_PartID);
        
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- Use the WORKING validation logic from CurrentStoredProcedures.sql
    IF p_TransferQuantity <= 0 OR p_TransferQuantity > p_OriginalQuantity THEN
        SET p_Status = 1;
        SET p_ErrorMsg = 'Invalid transfer quantity';
        ROLLBACK;
    ELSE
        -- FIXED: Update remaining quantity at old location while preserving original user
        UPDATE inv_inventory
        SET Quantity = Quantity - p_TransferQuantity,
            LastUpdated = NOW()
            -- DO NOT UPDATE User - preserve original user who created this inventory
        WHERE BatchNumber = p_BatchNumber
          AND PartID = p_PartID
          AND Operation = p_Operation
          AND Quantity = p_OriginalQuantity;
        
        -- Insert new record for transferred quantity at new location (with current user)
        INSERT INTO inv_inventory (
            BatchNumber, PartID, Operation, Quantity, Location, 
            User, ItemType, ReceiveDate, LastUpdated
        )
        SELECT 
            p_BatchNumber, p_PartID, p_Operation, p_TransferQuantity, p_NewLocation,
            p_User, ItemType, NOW(), NOW()
        FROM inv_inventory 
        WHERE BatchNumber = p_BatchNumber AND PartID = p_PartID AND Operation = p_Operation
        LIMIT 1;
        
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Quantity transferred successfully from original location to ', p_NewLocation);
        COMMIT;
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `inv_transactions_GetAnalytics` (IN `p_UserName` VARCHAR(100), IN `p_IsAdmin` BOOLEAN, IN `p_FromDate` DATETIME, IN `p_ToDate` DATETIME, OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_ErrorMessage TEXT DEFAULT '';
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        GET DIAGNOSTICS CONDITION 1
            v_ErrorMessage = MESSAGE_TEXT;
        
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred while retrieving analytics';
        
        -- Log error to log_error table
        INSERT INTO log_error (
            User, Severity, ErrorType, ErrorMessage, StackTrace,
            ModuleName, MethodName, AdditionalInfo, MachineName,
            OSVersion, AppVersion, ErrorTime
        ) VALUES (
            COALESCE(p_UserName, 'SYSTEM'), 'Error', 'DatabaseError',
            CONCAT('inv_transactions_GetAnalytics failed: ', v_ErrorMessage),
            v_ErrorMessage,
            'inv_transactions_GetAnalytics', 'GetAnalytics',
            CONCAT('UserName: ', COALESCE(p_UserName, 'NULL'), ', IsAdmin: ', p_IsAdmin, ', DateRange: ', COALESCE(p_FromDate, 'NULL'), ' to ', COALESCE(p_ToDate, 'NULL')),
            'Database Server', 'MySQL 5.7.24', '5.0.1.2', NOW()
        );
        
        ROLLBACK;
    END;
    
    SELECT 
        COUNT(*) as TotalTransactions,
        SUM(CASE WHEN TransactionType = 'IN' THEN 1 ELSE 0 END) as InTransactions,
        SUM(CASE WHEN TransactionType = 'OUT' THEN 1 ELSE 0 END) as OutTransactions,
        SUM(CASE WHEN TransactionType = 'TRANSFER' THEN 1 ELSE 0 END) as TransferTransactions,
        SUM(Quantity) as TotalQuantity,
        COUNT(DISTINCT PartID) as UniquePartIds,
        COUNT(DISTINCT User) as ActiveUsers,
        (SELECT PartID FROM inv_transaction t2 
         WHERE (p_IsAdmin = TRUE OR t2.User = p_UserName)
         AND (p_FromDate IS NULL OR t2.ReceiveDate >= p_FromDate)
         AND (p_ToDate IS NULL OR t2.ReceiveDate <= p_ToDate)
         GROUP BY PartID ORDER BY COUNT(*) DESC LIMIT 1) as TopPartId,
        (SELECT User FROM inv_transaction t3 
         WHERE (p_IsAdmin = TRUE OR t3.User = p_UserName)
         AND (p_FromDate IS NULL OR t3.ReceiveDate >= p_FromDate)
         AND (p_ToDate IS NULL OR t3.ReceiveDate <= p_ToDate)
         GROUP BY User ORDER BY COUNT(*) DESC LIMIT 1) as TopUser
    FROM inv_transaction t
    WHERE 
        (p_IsAdmin = TRUE OR t.User = p_UserName)
        AND (p_FromDate IS NULL OR t.ReceiveDate >= p_FromDate)
        AND (p_ToDate IS NULL OR t.ReceiveDate <= p_ToDate);
    
    SET p_Status = 0;
    SET p_ErrorMsg = 'Analytics retrieved successfully';
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `inv_transactions_SmartSearch` (IN `p_UserName` VARCHAR(100), IN `p_IsAdmin` BOOLEAN, IN `p_PartID` VARCHAR(300), IN `p_BatchNumber` VARCHAR(300), IN `p_Operation` VARCHAR(100), IN `p_Notes` VARCHAR(1000), IN `p_User` VARCHAR(100), IN `p_ItemType` VARCHAR(100), IN `p_Quantity` INT, IN `p_TransactionTypes` VARCHAR(500), IN `p_FromDate` DATETIME, IN `p_ToDate` DATETIME, IN `p_Locations` VARCHAR(500), IN `p_GeneralSearch` VARCHAR(500), IN `p_Page` INT, IN `p_PageSize` INT, OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Offset INT DEFAULT 0;
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_ErrorMessage TEXT DEFAULT '';
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        GET DIAGNOSTICS CONDITION 1
            v_ErrorMessage = MESSAGE_TEXT;
        
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred during smart search';
        
        -- Log error to log_error table
        INSERT INTO log_error (
            User, Severity, ErrorType, ErrorMessage, StackTrace,
            ModuleName, MethodName, AdditionalInfo, MachineName,
            OSVersion, AppVersion, ErrorTime
        ) VALUES (
            COALESCE(p_UserName, 'SYSTEM'), 'Error', 'DatabaseError',
            CONCAT('inv_transactions_SmartSearch failed: ', v_ErrorMessage),
            v_ErrorMessage,
            'inv_transactions_SmartSearch', 'SmartSearch',
            CONCAT('UserName: ', COALESCE(p_UserName, 'NULL'), ', IsAdmin: ', p_IsAdmin, ', Page: ', p_Page, ', PageSize: ', p_PageSize),
            'Database Server', 'MySQL 5.7.24', '5.0.1.2', NOW()
        );
        
        ROLLBACK;
    END;
    
    -- Calculate offset for pagination
    SET v_Offset = (p_Page - 1) * p_PageSize;
    
    -- Build dynamic WHERE clause based on provided parameters
    SELECT 
        ID,
        TransactionType,
        BatchNumber,
        PartID,
        FromLocation,
        ToLocation,
        Operation,
        Quantity,
        Notes,
        User,
        ItemType,
        ReceiveDate
    FROM inv_transaction t
    WHERE 
        -- Admin check: non-admin users only see their own transactions
        (p_IsAdmin = TRUE OR t.User = p_UserName)
        
        -- Part ID search (exact or partial match)
        AND (p_PartID IS NULL OR p_PartID = '' OR t.PartID LIKE CONCAT('%', p_PartID, '%'))
        
        -- Batch number search
        AND (p_BatchNumber IS NULL OR p_BatchNumber = '' OR t.BatchNumber LIKE CONCAT('%', p_BatchNumber, '%'))
        
        -- Operation search
        AND (p_Operation IS NULL OR p_Operation = '' OR t.Operation LIKE CONCAT('%', p_Operation, '%'))
        
        -- Notes search
        AND (p_Notes IS NULL OR p_Notes = '' OR t.Notes LIKE CONCAT('%', p_Notes, '%'))
        
        -- User search
        AND (p_User IS NULL OR p_User = '' OR t.User LIKE CONCAT('%', p_User, '%'))
        
        -- Item type search
        AND (p_ItemType IS NULL OR p_ItemType = '' OR t.ItemType = p_ItemType)
        
        -- Quantity search
        AND (p_Quantity IS NULL OR p_Quantity <= 0 OR t.Quantity = p_Quantity)
        
        -- Transaction type filter (comma-separated list)
        AND (p_TransactionTypes IS NULL OR p_TransactionTypes = '' 
             OR FIND_IN_SET(t.TransactionType, p_TransactionTypes) > 0)
        
        -- Date range filter
        AND (p_FromDate IS NULL OR t.ReceiveDate >= p_FromDate)
        AND (p_ToDate IS NULL OR t.ReceiveDate <= p_ToDate)
        
        -- Location filter (searches both FromLocation and ToLocation)
        AND (p_Locations IS NULL OR p_Locations = '' 
             OR FIND_IN_SET(t.FromLocation, p_Locations) > 0
             OR FIND_IN_SET(t.ToLocation, p_Locations) > 0)
        
        -- General search (searches across multiple text fields)
        AND (p_GeneralSearch IS NULL OR p_GeneralSearch = ''
             OR t.PartID LIKE CONCAT('%', p_GeneralSearch, '%')
             OR t.BatchNumber LIKE CONCAT('%', p_GeneralSearch, '%')
             OR t.Notes LIKE CONCAT('%', p_Notes, '%')
             OR t.User LIKE CONCAT('%', p_GeneralSearch, '%')
             OR t.FromLocation LIKE CONCAT('%', p_GeneralSearch, '%')
             OR t.ToLocation LIKE CONCAT('%', p_GeneralSearch, '%')
             OR t.Operation LIKE CONCAT('%', p_GeneralSearch, '%'))
    
    ORDER BY t.ReceiveDate DESC
    LIMIT p_PageSize OFFSET v_Offset;
    
    -- Get count for analytics
    SELECT COUNT(*) INTO v_Count 
    FROM inv_transaction t
    WHERE 
        (p_IsAdmin = TRUE OR t.User = p_UserName)
        AND (p_PartID IS NULL OR p_PartID = '' OR t.PartID LIKE CONCAT('%', p_PartID, '%'))
        AND (p_BatchNumber IS NULL OR p_BatchNumber = '' OR t.BatchNumber LIKE CONCAT('%', p_BatchNumber, '%'))
        AND (p_Operation IS NULL OR p_Operation = '' OR t.Operation LIKE CONCAT('%', p_Operation, '%'))
        AND (p_Notes IS NULL OR p_Notes = '' OR t.Notes LIKE CONCAT('%', p_Notes, '%'))
        AND (p_User IS NULL OR p_User = '' OR t.User LIKE CONCAT('%', p_User, '%'))
        AND (p_ItemType IS NULL OR p_ItemType = '' OR t.ItemType = p_ItemType)
        AND (p_Quantity IS NULL OR p_Quantity <= 0 OR t.Quantity = p_Quantity)
        AND (p_TransactionTypes IS NULL OR p_TransactionTypes = '' 
             OR FIND_IN_SET(t.TransactionType, p_TransactionTypes) > 0)
        AND (p_FromDate IS NULL OR t.ReceiveDate >= p_FromDate)
        AND (p_ToDate IS NULL OR t.ReceiveDate <= p_ToDate)
        AND (p_Locations IS NULL OR p_Locations = '' 
             OR FIND_IN_SET(t.FromLocation, p_Locations) > 0
             OR FIND_IN_SET(t.ToLocation, p_Locations) > 0)
        AND (p_GeneralSearch IS NULL OR p_GeneralSearch = ''
             OR t.PartID LIKE CONCAT('%', p_GeneralSearch, '%')
             OR t.BatchNumber LIKE CONCAT('%', p_GeneralSearch, '%')
             OR t.Notes LIKE CONCAT('%', p_Notes, '%')
             OR t.User LIKE CONCAT('%', p_GeneralSearch, '%')
             OR t.FromLocation LIKE CONCAT('%', p_GeneralSearch, '%')
             OR t.ToLocation LIKE CONCAT('%', p_GeneralSearch, '%')
             OR t.Operation LIKE CONCAT('%', p_GeneralSearch, '%'));
    
    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('Smart search completed, found ', v_Count, ' matching transactions');
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `inv_transaction_Add` (IN `p_TransactionType` ENUM('IN','OUT','TRANSFER'), IN `p_PartID` VARCHAR(300), IN `p_BatchNumber` VARCHAR(100), IN `p_FromLocation` VARCHAR(300), IN `p_ToLocation` VARCHAR(100), IN `p_Operation` VARCHAR(100), IN `p_Quantity` INT, IN `p_Notes` VARCHAR(1000), IN `p_User` VARCHAR(100), IN `p_ItemType` VARCHAR(100), IN `p_ReceiveDate` DATETIME, OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_ErrorMessage TEXT DEFAULT '';
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        GET DIAGNOSTICS CONDITION 1
            v_ErrorMessage = MESSAGE_TEXT;
        
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while adding transaction: ', v_ErrorMessage);
        
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- Insert transaction record (using working structure from CurrentStoredProcedures.sql)
    INSERT INTO inv_transaction (
        TransactionType, PartID, BatchNumber, FromLocation, ToLocation, 
        Operation, Quantity, Notes, User, ItemType, ReceiveDate
    ) VALUES (
        p_TransactionType, p_PartID, p_BatchNumber, p_FromLocation, p_ToLocation,
        p_Operation, p_Quantity, p_Notes, p_User, p_ItemType, p_ReceiveDate
    );
    
    SET p_Status = 0;
    SET p_ErrorMsg = 'Transaction added successfully';
    
    COMMIT;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `inv_transaction_GetProblematicBatchCount` (OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_ErrorMessage TEXT DEFAULT '';
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        GET DIAGNOSTICS CONDITION 1
            v_ErrorMessage = MESSAGE_TEXT;
        
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred while counting problematic batch numbers';
        
        -- Log error to log_error table
        INSERT INTO log_error (
            User, Severity, ErrorType, ErrorMessage, StackTrace,
            ModuleName, MethodName, AdditionalInfo, MachineName,
            OSVersion, AppVersion, ErrorTime
        ) VALUES (
            'SYSTEM', 'Error', 'DatabaseError',
            CONCAT('inv_transaction_GetProblematicBatchCount failed: ', v_ErrorMessage),
            v_ErrorMessage,
            'inv_transaction_GetProblematicBatchCount', 'GetProblematicBatchCount',
            'Error counting problematic batch numbers',
            'Database Server', 'MySQL 5.7.24', '5.0.1.2', NOW()
        );
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
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `inv_transaction_GetProblematicBatches` (IN `p_Limit` INT, OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_ErrorMessage TEXT DEFAULT '';
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        GET DIAGNOSTICS CONDITION 1
            v_ErrorMessage = MESSAGE_TEXT;
        
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred while retrieving problematic batch numbers';
        
        -- Log error to log_error table
        INSERT INTO log_error (
            User, Severity, ErrorType, ErrorMessage, StackTrace,
            ModuleName, MethodName, AdditionalInfo, MachineName,
            OSVersion, AppVersion, ErrorTime
        ) VALUES (
            'SYSTEM', 'Error', 'DatabaseError',
            CONCAT('inv_transaction_GetProblematicBatches failed: ', v_ErrorMessage),
            v_ErrorMessage,
            'inv_transaction_GetProblematicBatches', 'GetProblematicBatches',
            CONCAT('Limit: ', p_Limit),
            'Database Server', 'MySQL 5.7.24', '5.0.1.2', NOW()
        );
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
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `inv_transaction_SplitBatchNumbers` (IN `p_BatchNumbers` TEXT, OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255), OUT `p_ProcessedCount` INT)   BEGIN
    DECLARE v_Counter INT DEFAULT 0;
    DECLARE v_BatchCount INT DEFAULT 0;
    DECLARE v_ErrorMessage TEXT DEFAULT '';
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        GET DIAGNOSTICS CONDITION 1
            v_ErrorMessage = MESSAGE_TEXT;
        
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred while splitting batch numbers';
        SET p_ProcessedCount = v_Counter;
        
        -- Log error to log_error table
        INSERT INTO log_error (
            User, Severity, ErrorType, ErrorMessage, StackTrace,
            ModuleName, MethodName, AdditionalInfo, MachineName,
            OSVersion, AppVersion, ErrorTime
        ) VALUES (
            'SYSTEM', 'Error', 'DatabaseError',
            CONCAT('inv_transaction_SplitBatchNumbers failed: ', v_ErrorMessage),
            v_ErrorMessage,
            'inv_transaction_SplitBatchNumbers', 'SplitBatchNumbers',
            CONCAT('BatchNumbers: ', SUBSTRING(p_BatchNumbers, 1, 100), '...'),
            'Database Server', 'MySQL 5.7.24', '5.0.1.2', NOW()
        );
        
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
        
        -- Log validation error
        INSERT INTO log_error (
            User, Severity, ErrorType, ErrorMessage, StackTrace,
            ModuleName, MethodName, AdditionalInfo, MachineName,
            OSVersion, AppVersion, ErrorTime
        ) VALUES (
            'SYSTEM', 'Warning', 'ValidationError',
            'inv_transaction_SplitBatchNumbers validation failed: No batch numbers provided',
            'Validation Error',
            'inv_transaction_SplitBatchNumbers', 'SplitBatchNumbers',
            'Empty or null batch numbers parameter',
            'Database Server', 'MySQL 5.7.24', '5.0.1.2', NOW()
        );
        
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
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `log_ArchiveAndPurge` (IN `p_Days` INT)   BEGIN
    IF p_Days IS NULL OR p_Days <= 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'p_Days must be > 0';
    END IF;

    INSERT INTO log_error_archive
    SELECT * FROM log_error
    WHERE ErrorTime < (NOW() - INTERVAL p_Days DAY);

    DELETE FROM log_error
    WHERE ErrorTime < (NOW() - INTERVAL p_Days DAY);
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `log_changelog_Add_Entry` (IN `p_Version` VARCHAR(50), IN `p_Description` TEXT, IN `p_ReleaseDate` DATE, IN `p_CreatedBy` VARCHAR(100), OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while adding changelog entry for version: ', p_Version);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    
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
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `log_changelog_Get_All` (OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
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
        
        CAST(SUBSTRING_INDEX(Version, '.', 1) AS UNSIGNED) DESC,
        CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(Version, '.', 2), '.', -1) AS UNSIGNED) DESC,
        CAST(SUBSTRING_INDEX(Version, '.', -1) AS UNSIGNED) DESC,
        
        Version DESC,
        
        CreatedDate DESC;
    
    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('Retrieved ', v_Count, ' changelog entries successfully (ordered by highest version)');
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `log_changelog_Get_Current` (OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred while retrieving current version information';
    END;
    
    
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
        
        
        SELECT 
            Version,
            Description,
            ReleaseDate,
            CreatedBy,
            CreatedDate
        FROM log_changelog 
        ORDER BY 
            
            CAST(SUBSTRING_INDEX(Version, '.', 1) AS UNSIGNED) DESC,
            CAST(SUBSTRING_INDEX(SUBSTRING_INDEX(Version, '.', 2), '.', -1) AS UNSIGNED) DESC,
            CAST(SUBSTRING_INDEX(Version, '.', -1) AS UNSIGNED) DESC,
            
            Version DESC,
            
            CreatedDate DESC
        LIMIT 1;
        
        SET p_Status = 0;
        SET p_ErrorMsg = 'Current version (highest version number) retrieved successfully';
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `log_changelog_Initialize_Default_Data` (OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred while initializing default changelog data';
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    
    SELECT COUNT(*) INTO v_Count FROM log_changelog;
    
    IF v_Count = 0 THEN
        
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
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `log_error_Add_Error` (IN `p_User` VARCHAR(100), IN `p_Severity` VARCHAR(50), IN `p_ErrorType` VARCHAR(100), IN `p_ErrorMessage` TEXT, IN `p_StackTrace` TEXT, IN `p_ModuleName` VARCHAR(100), IN `p_MethodName` VARCHAR(100), IN `p_AdditionalInfo` TEXT, IN `p_MachineName` VARCHAR(100), IN `p_OSVersion` VARCHAR(100), IN `p_AppVersion` VARCHAR(50), IN `p_ErrorTime` DATETIME, OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
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

    
    IF p_User IS NULL OR p_User = '' THEN
        SET p_User = 'Unknown';
    END IF;

    IF p_Severity IS NULL OR p_Severity = '' THEN
        SET p_Severity = 'Error';
    END IF;

    IF p_ErrorTime IS NULL THEN
        SET p_ErrorTime = NOW();
    END IF;

    
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
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `log_error_Delete_All` (OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred while deleting all error log entries';
    END;

    START TRANSACTION;

    
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
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `log_error_Delete_ById` (IN `p_Id` INT, OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while deleting error log entry with ID: ', p_Id);
    END;

    START TRANSACTION;

    
    IF p_Id IS NULL OR p_Id <= 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = 'Valid ID parameter is required';
        ROLLBACK;
    ELSE
        
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
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `log_error_Get_All` (OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
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
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `log_error_Get_ByDateRange` (IN `p_StartDate` DATETIME, IN `p_EndDate` DATETIME, OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while retrieving error logs for date range: ', 
                               DATE_FORMAT(p_StartDate, '%Y-%m-%d'), ' to ', DATE_FORMAT(p_EndDate, '%Y-%m-%d'));
    END;

    
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
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `log_error_Get_ByUser` (IN `p_User` VARCHAR(100), OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while retrieving error logs for user: ', p_User);
    END;

    
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
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `log_error_Get_Unique` (OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
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
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `log_InsertError` (IN `p_User` VARCHAR(100), IN `p_Severity` ENUM('Information','Warning','Error','Critical'), IN `p_ErrorType` VARCHAR(100), IN `p_ErrorMsg` TEXT, IN `p_StackTrace` TEXT, IN `p_ModuleName` VARCHAR(200), IN `p_MethodName` VARCHAR(200), IN `p_Additional` TEXT)   BEGIN
    -- Intentionally no transaction control here.
    INSERT INTO log_error(
        `User`, Severity, ErrorType, ErrorMessage, StackTrace,
        ModuleName, MethodName, AdditionalInfo,
        MachineName, OSVersion, AppVersion, ErrorTime
    ) VALUES (
        LEFT(COALESCE(p_User,'SYSTEM'),100),
        p_Severity,
        LEFT(p_ErrorType,100),
        LEFT(p_ErrorMsg,65535),
        LEFT(p_StackTrace,65535),
        LEFT(p_ModuleName,200),
        LEFT(p_MethodName,200),
        p_Additional,
        'Database Server',
        'MySQL 5.7.24',
        '5.0.1.2',
        NOW()
    );
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `log_PurgeOld` (IN `p_Days` INT)   BEGIN
    IF p_Days IS NULL OR p_Days <= 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'p_Days must be > 0';
    END IF;

    DELETE FROM log_error
    WHERE ErrorTime < (NOW() - INTERVAL p_Days DAY);
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `md_item_types_Add_ItemType` (IN `p_ItemType` VARCHAR(50), IN `p_IssuedBy` VARCHAR(100), OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while adding item type: ', p_ItemType);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    
    SELECT COUNT(*) INTO v_Count FROM md_item_types WHERE ItemType = p_ItemType;
    
    IF v_Count > 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Item type already exists: ', p_ItemType);
        ROLLBACK;
    ELSE
        INSERT INTO md_item_types (ItemType, IssuedBy)
        VALUES (p_ItemType, p_IssuedBy);
        
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Item type added successfully: ', p_ItemType);
        COMMIT;
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `md_item_types_Delete_ByItemType` (IN `p_ItemType` VARCHAR(50), OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while deleting item type: ', p_ItemType);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    
    SELECT COUNT(*) INTO v_Count FROM md_item_types WHERE ItemType = p_ItemType;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Item type not found: ', p_ItemType);
        ROLLBACK;
    ELSE
        DELETE FROM md_item_types WHERE ItemType = p_ItemType;
        SET v_RowsAffected = ROW_COUNT();
        
        IF v_RowsAffected > 0 THEN
            SET p_Status = 0;
            SET p_ErrorMsg = CONCAT('Item type deleted successfully: ', p_ItemType);
        ELSE
            SET p_Status = 2;
            SET p_ErrorMsg = CONCAT('Failed to delete item type: ', p_ItemType);
        END IF;
        
        COMMIT;
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `md_item_types_Exists_ByItemType` (IN `p_ItemType` VARCHAR(50), OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while checking item type existence: ', p_ItemType);
    END;
    
    SELECT COUNT(*) INTO v_Count FROM md_item_types WHERE ItemType = p_ItemType;
    SELECT v_Count as ItemTypeExists;
    
    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('Item type existence check completed for: ', p_ItemType, ' (Exists: ', IF(v_Count > 0, 'Yes', 'No'), ')');
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `md_item_types_GetDistinct` (OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred while retrieving distinct item types';
    END;
    
    SELECT COUNT(DISTINCT ItemType) INTO v_Count FROM md_item_types;
    SELECT DISTINCT ItemType FROM md_item_types ORDER BY ItemType;
    
    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('Retrieved ', v_Count, ' distinct item types successfully');
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `md_item_types_Get_All` (OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred while retrieving all item types';
    END;
    
    SELECT COUNT(*) INTO v_Count FROM md_item_types;
    SELECT * FROM md_item_types ORDER BY ItemType;
    
    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('Retrieved ', v_Count, ' item types successfully');
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `md_item_types_Update_ItemType` (IN `p_OldItemType` VARCHAR(50), IN `p_NewItemType` VARCHAR(50), IN `p_IssuedBy` VARCHAR(100), OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while updating item type: ', p_OldItemType);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    
    SELECT COUNT(*) INTO v_Count FROM md_item_types WHERE ItemType = p_OldItemType;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Item type not found: ', p_OldItemType);
        ROLLBACK;
    ELSE
        UPDATE md_item_types 
        SET ItemType = p_NewItemType,
            IssuedBy = p_IssuedBy
        WHERE ItemType = p_OldItemType;
        
        SET v_RowsAffected = ROW_COUNT();
        
        IF v_RowsAffected > 0 THEN
            SET p_Status = 0;
            SET p_ErrorMsg = CONCAT('Item type updated successfully from ', p_OldItemType, ' to ', p_NewItemType);
        ELSE
            SET p_Status = 2;
            SET p_ErrorMsg = CONCAT('No changes made to item type: ', p_OldItemType);
        END IF;
        
        COMMIT;
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `md_locations_Add_Location` (IN `p_Location` VARCHAR(100), IN `p_Building` VARCHAR(100), IN `p_IssuedBy` VARCHAR(100), OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while adding location: ', p_Location);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    
    SELECT COUNT(*) INTO v_Count FROM md_locations WHERE Location = p_Location;
    
    IF v_Count > 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Location already exists: ', p_Location);
        ROLLBACK;
    ELSE
        INSERT INTO md_locations (Location, Building, IssuedBy)
        VALUES (p_Location, p_Building, p_IssuedBy);
        
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Location added successfully: ', p_Location);
        COMMIT;
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `md_locations_Delete_ByLocation` (IN `p_Location` VARCHAR(100), OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while deleting location: ', p_Location);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    
    SELECT COUNT(*) INTO v_Count FROM md_locations WHERE Location = p_Location;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Location not found: ', p_Location);
        ROLLBACK;
    ELSE
        DELETE FROM md_locations WHERE Location = p_Location;
        SET v_RowsAffected = ROW_COUNT();
        
        IF v_RowsAffected > 0 THEN
            SET p_Status = 0;
            SET p_ErrorMsg = CONCAT('Location deleted successfully: ', p_Location);
        ELSE
            SET p_Status = 2;
            SET p_ErrorMsg = CONCAT('Failed to delete location: ', p_Location);
        END IF;
        
        COMMIT;
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `md_locations_Exists_ByLocation` (IN `p_Location` VARCHAR(100), OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while checking location existence: ', p_Location);
    END;
    
    SELECT COUNT(*) INTO v_Count FROM md_locations WHERE Location = p_Location;
    SELECT v_Count as LocationExists;
    
    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('Location existence check completed for: ', p_Location, ' (Exists: ', IF(v_Count > 0, 'Yes', 'No'), ')');
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `md_locations_Get_All` (OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred while retrieving all locations';
    END;
    
    SELECT COUNT(*) INTO v_Count FROM md_locations;
    SELECT * FROM md_locations ORDER BY Location;
    
    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('Retrieved ', v_Count, ' locations successfully');
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `md_locations_Update_Location` (IN `p_OldLocation` VARCHAR(100), IN `p_NewLocation` VARCHAR(100), IN `p_Building` VARCHAR(100), IN `p_IssuedBy` VARCHAR(100), OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while updating location: ', p_OldLocation);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    
    SELECT COUNT(*) INTO v_Count FROM md_locations WHERE Location = p_OldLocation;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Location not found: ', p_OldLocation);
        ROLLBACK;
    ELSE
        UPDATE md_locations 
        SET Location = p_NewLocation,
            Building = p_Building,
            IssuedBy = p_IssuedBy
        WHERE Location = p_OldLocation;
        
        SET v_RowsAffected = ROW_COUNT();
        
        IF v_RowsAffected > 0 THEN
            SET p_Status = 0;
            SET p_ErrorMsg = CONCAT('Location updated successfully from ', p_OldLocation, ' to ', p_NewLocation);
        ELSE
            SET p_Status = 2;
            SET p_ErrorMsg = CONCAT('No changes made to location: ', p_OldLocation);
        END IF;
        
        COMMIT;
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `md_operation_numbers_Add_Operation` (IN `p_Operation` VARCHAR(50), IN `p_IssuedBy` VARCHAR(100), OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while adding operation: ', p_Operation);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    
    SELECT COUNT(*) INTO v_Count FROM md_operation_numbers WHERE Operation = p_Operation;
    
    IF v_Count > 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Operation already exists: ', p_Operation);
        ROLLBACK;
    ELSE
        INSERT INTO md_operation_numbers (Operation, IssuedBy)
        VALUES (p_Operation, p_IssuedBy);
        
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Operation added successfully: ', p_Operation);
        COMMIT;
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `md_operation_numbers_Delete_ByOperation` (IN `p_Operation` VARCHAR(50), OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while deleting operation: ', p_Operation);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    
    SELECT COUNT(*) INTO v_Count FROM md_operation_numbers WHERE Operation = p_Operation;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Operation not found: ', p_Operation);
        ROLLBACK;
    ELSE
        DELETE FROM md_operation_numbers WHERE Operation = p_Operation;
        SET v_RowsAffected = ROW_COUNT();
        
        IF v_RowsAffected > 0 THEN
            SET p_Status = 0;
            SET p_ErrorMsg = CONCAT('Operation deleted successfully: ', p_Operation);
        ELSE
            SET p_Status = 2;
            SET p_ErrorMsg = CONCAT('Failed to delete operation: ', p_Operation);
        END IF;
        
        COMMIT;
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `md_operation_numbers_Exists_ByOperation` (IN `p_Operation` VARCHAR(50), OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while checking operation existence: ', p_Operation);
    END;
    
    SELECT COUNT(*) INTO v_Count FROM md_operation_numbers WHERE Operation = p_Operation;
    SELECT v_Count as OperationExists;
    
    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('Operation existence check completed for: ', p_Operation, ' (Exists: ', IF(v_Count > 0, 'Yes', 'No'), ')');
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `md_operation_numbers_Get_All` (OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred while retrieving all operations';
    END;
    
    SELECT COUNT(*) INTO v_Count FROM md_operation_numbers;
    SELECT * FROM md_operation_numbers ORDER BY Operation;
    
    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('Retrieved ', v_Count, ' operations successfully');
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `md_operation_numbers_Update_Operation` (IN `p_OldOperation` VARCHAR(50), IN `p_NewOperation` VARCHAR(50), IN `p_IssuedBy` VARCHAR(100), OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while updating operation: ', p_OldOperation);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    
    SELECT COUNT(*) INTO v_Count FROM md_operation_numbers WHERE Operation = p_OldOperation;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Operation not found: ', p_OldOperation);
        ROLLBACK;
    ELSE
        UPDATE md_operation_numbers 
        SET Operation = p_NewOperation,
            IssuedBy = p_IssuedBy
        WHERE Operation = p_OldOperation;
        
        SET v_RowsAffected = ROW_COUNT();
        
        IF v_RowsAffected > 0 THEN
            SET p_Status = 0;
            SET p_ErrorMsg = CONCAT('Operation updated successfully from ', p_OldOperation, ' to ', p_NewOperation);
        ELSE
            SET p_Status = 2;
            SET p_ErrorMsg = CONCAT('No changes made to operation: ', p_OldOperation);
        END IF;
        
        COMMIT;
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `md_part_ids_Add_PartID` (IN `p_PartID` VARCHAR(300), IN `p_ItemType` VARCHAR(50), IN `p_IssuedBy` VARCHAR(100), OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while adding part ID: ', p_PartID);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    
    SELECT COUNT(*) INTO v_Count FROM md_part_ids WHERE PartID = p_PartID;
    
    IF v_Count > 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Part ID already exists: ', p_PartID);
        ROLLBACK;
    ELSE
        INSERT INTO md_part_ids (PartID, ItemType, IssuedBy)
        VALUES (p_PartID, p_ItemType, p_IssuedBy);
        
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Part ID added successfully: ', p_PartID);
        COMMIT;
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `md_part_ids_Delete_ByPartID` (IN `p_PartID` VARCHAR(300), OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while deleting part ID: ', p_PartID);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    
    SELECT COUNT(*) INTO v_Count FROM md_part_ids WHERE PartID = p_PartID;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Part ID not found: ', p_PartID);
        ROLLBACK;
    ELSE
        DELETE FROM md_part_ids WHERE PartID = p_PartID;
        SET v_RowsAffected = ROW_COUNT();
        
        IF v_RowsAffected > 0 THEN
            SET p_Status = 0;
            SET p_ErrorMsg = CONCAT('Part ID deleted successfully: ', p_PartID);
        ELSE
            SET p_Status = 2;
            SET p_ErrorMsg = CONCAT('Failed to delete part ID: ', p_PartID);
        END IF;
        
        COMMIT;
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `md_part_ids_Exists_ByPartID` (IN `p_PartID` VARCHAR(300), OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while checking part ID existence: ', p_PartID);
    END;
    
    SELECT COUNT(*) INTO v_Count FROM md_part_ids WHERE PartID = p_PartID;
    SELECT v_Count as PartExists;
    
    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('Part ID existence check completed for: ', p_PartID, ' (Exists: ', IF(v_Count > 0, 'Yes', 'No'), ')');
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `md_part_ids_GetItemType_ByPartID` (IN `p_PartID` VARCHAR(300), OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while retrieving item type for part ID: ', p_PartID);
    END;
    
    SELECT COUNT(*) INTO v_Count FROM md_part_ids WHERE PartID = p_PartID;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Part ID not found: ', p_PartID);
        SELECT NULL as ItemType;
    ELSE
        SELECT ItemType FROM md_part_ids WHERE PartID = p_PartID LIMIT 1;
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Item type retrieved successfully for part ID: ', p_PartID);
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `md_part_ids_Get_All` (OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred while retrieving all part IDs';
    END;
    
    SELECT COUNT(*) INTO v_Count FROM md_part_ids;
    SELECT * FROM md_part_ids ORDER BY PartID;
    
    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('Retrieved ', v_Count, ' part IDs successfully');
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `md_part_ids_Get_ByPartID` (IN `p_PartID` VARCHAR(300), OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while retrieving part ID: ', p_PartID);
    END;
    
    SELECT COUNT(*) INTO v_Count FROM md_part_ids WHERE PartID = p_PartID;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Part ID not found: ', p_PartID);
        SELECT NULL as PartID, NULL as ItemType;
    ELSE
        SELECT * FROM md_part_ids WHERE PartID = p_PartID LIMIT 1;
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Part ID retrieved successfully: ', p_PartID);
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `md_part_ids_Update_PartID` (IN `p_OldPartID` VARCHAR(300), IN `p_NewPartID` VARCHAR(300), IN `p_ItemType` VARCHAR(50), IN `p_IssuedBy` VARCHAR(100), OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while updating part ID: ', p_OldPartID);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    
    SELECT COUNT(*) INTO v_Count FROM md_part_ids WHERE PartID = p_OldPartID;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Part ID not found: ', p_OldPartID);
        ROLLBACK;
    ELSE
        UPDATE md_part_ids 
        SET PartID = p_NewPartID,
            ItemType = p_ItemType,
            IssuedBy = p_IssuedBy
        WHERE PartID = p_OldPartID;
        
        SET v_RowsAffected = ROW_COUNT();
        
        IF v_RowsAffected > 0 THEN
            SET p_Status = 0;
            SET p_ErrorMsg = CONCAT('Part ID updated successfully from ', p_OldPartID, ' to ', p_NewPartID);
        ELSE
            SET p_Status = 2;
            SET p_ErrorMsg = CONCAT('No changes made to part ID: ', p_OldPartID);
        END IF;
        
        COMMIT;
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `sys_GetRoleIdByName` (IN `p_RoleName` VARCHAR(100), OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_RoleID INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while retrieving role ID for: ', p_RoleName);
    END;
    
    SELECT COUNT(*) INTO v_Count FROM sys_roles WHERE RoleName = p_RoleName;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Role not found: ', p_RoleName);
        SELECT NULL as RoleID;
    ELSE
        SELECT ID INTO v_RoleID FROM sys_roles WHERE RoleName = p_RoleName LIMIT 1;
        SELECT v_RoleID as RoleID;
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Role ID retrieved successfully for: ', p_RoleName);
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `sys_GetStoredProcedureInventory` (OUT `p_Status` INT, OUT `p_ErrorMsg` TEXT)   BEGIN
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
    
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `sys_GetUserAccessType` (OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred while retrieving user access information';
    END;
    
    SELECT COUNT(*) INTO v_Count FROM usr_users;
    
    
    SELECT 
        ID,
        User,
        `Full Name`,
        AccessType,
        VitsUser,
        CreatedDate,
        ModifiedDate
    FROM usr_users 
    ORDER BY `Full Name`;
    
    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('Retrieved user access information for ', v_Count, ' users');
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `sys_GetUserIdByName` (IN `p_UserName` VARCHAR(100), OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_UserID INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while retrieving user ID for: ', p_UserName);
    END;
    
    SELECT COUNT(*) INTO v_Count FROM usr_users WHERE User = p_UserName;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('User not found: ', p_UserName);
        SELECT NULL as UserID;
    ELSE
        SELECT ID INTO v_UserID FROM usr_users WHERE User = p_UserName LIMIT 1;
        SELECT v_UserID as UserID;
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('User ID retrieved successfully for: ', p_UserName);
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `sys_last_10_transactions_AddOrShift_ByUser` (IN `p_User` VARCHAR(100), IN `p_PartID` VARCHAR(300), IN `p_Operation` VARCHAR(50), IN `p_Quantity` INT, OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_ExistingPosition INT DEFAULT 0;
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while adding/shifting quick button for user: ', p_User);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    
    
    SELECT Position INTO v_ExistingPosition
    FROM sys_last_10_transactions 
    WHERE User = p_User AND PartID = p_PartID AND Operation = p_Operation
    LIMIT 1;
    
    IF v_ExistingPosition > 0 THEN
        
        UPDATE sys_last_10_transactions 
        SET Quantity = p_Quantity, ReceiveDate = NOW()
        WHERE User = p_User AND Position = v_ExistingPosition;
        
        
        IF v_ExistingPosition != 1 THEN
            CALL sys_last_10_transactions_Move(p_User, v_ExistingPosition, 1, @move_status, @move_msg);
        END IF;
        
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Updated existing quick button quantity and moved to position 1 for user: ', p_User);
    ELSE
        
        CALL sys_last_10_transactions_Add_AtPosition(p_User, 1, p_PartID, p_Operation, p_Quantity, @add_status, @add_msg);
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Added new quick button at position 1 for user: ', p_User);
    END IF;
    
    COMMIT;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `sys_last_10_transactions_Add_AtPosition` (IN `p_User` VARCHAR(100), IN `p_Position` INT, IN `p_PartID` VARCHAR(300), IN `p_Operation` VARCHAR(50), IN `p_Quantity` INT, OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while adding quick button at position ', p_Position, ' for user: ', p_User);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    
    IF p_Position < 1 OR p_Position > 10 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Invalid position: ', p_Position, '. Position must be between 1 and 10.');
        ROLLBACK;
    ELSE
        
        UPDATE sys_last_10_transactions 
        SET Position = Position + 1 
        WHERE User = p_User AND Position >= p_Position AND Position < 10;
        
        
        DELETE FROM sys_last_10_transactions WHERE User = p_User AND Position = 10;
        
        
        INSERT INTO sys_last_10_transactions (User, Position, PartID, Operation, Quantity, ReceiveDate)
        VALUES (p_User, p_Position, p_PartID, p_Operation, p_Quantity, NOW());
        
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Quick button added at position ', p_Position, ' for user: ', p_User);
        COMMIT;
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `sys_last_10_transactions_Add_AtPosition_1` (IN `p_User` VARCHAR(100), IN `p_Position` INT, IN `p_PartID` VARCHAR(300), IN `p_Operation` VARCHAR(50), IN `p_Quantity` INT, OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while adding quick button at position ', p_Position, ' for user: ', p_User);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    
    IF p_Position < 1 OR p_Position > 10 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Invalid position: ', p_Position, '. Position must be between 1 and 10.');
        ROLLBACK;
    ELSE
        
        UPDATE sys_last_10_transactions 
        SET Position = Position + 1 
        WHERE User = p_User AND Position >= p_Position AND Position < 10;
        
        
        DELETE FROM sys_last_10_transactions WHERE User = p_User AND Position = 10;
        
        
        INSERT INTO sys_last_10_transactions (User, Position, PartID, Operation, Quantity, ReceiveDate)
        VALUES (p_User, p_Position, p_PartID, p_Operation, p_Quantity, NOW());
        
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Quick button added at position ', p_Position, ' for user: ', p_User);
        COMMIT;
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `sys_last_10_transactions_DeleteAll_ByUser` (IN `p_User` VARCHAR(100), OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while deleting all quick buttons for user: ', p_User);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    SELECT COUNT(*) INTO v_Count FROM sys_last_10_transactions WHERE User = p_User;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('No quick buttons found for user: ', p_User);
        ROLLBACK;
    ELSE
        DELETE FROM sys_last_10_transactions WHERE User = p_User;
        SET v_RowsAffected = ROW_COUNT();
        
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Deleted ', v_RowsAffected, ' quick buttons for user: ', p_User);
        COMMIT;
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `sys_last_10_transactions_Get_ByUser` (IN `p_User` VARCHAR(100), OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while retrieving quick buttons for user: ', p_User);
    END;
    
    SELECT COUNT(*) INTO v_Count FROM sys_last_10_transactions WHERE User = p_User;
    
    SELECT 
        Position,
        User,
        PartID,
        Operation,
        Quantity,
        ReceiveDate  
    FROM sys_last_10_transactions 
    WHERE User = p_User 
    ORDER BY Position;
    
    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('Retrieved ', v_Count, ' quick buttons for user: ', p_User);
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `sys_last_10_transactions_Get_ByUser_1` (IN `p_User` VARCHAR(100), OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while retrieving quick buttons for user: ', p_User);
    END;
    
    SELECT COUNT(*) INTO v_Count FROM sys_last_10_transactions WHERE User = p_User;
    
    SELECT 
        Position,
        User,
        PartID,
        Operation,
        Quantity,
        ReceiveDate  
    FROM sys_last_10_transactions 
    WHERE User = p_User 
    ORDER BY Position;
    
    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('Retrieved ', v_Count, ' quick buttons for user: ', p_User);
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `sys_last_10_transactions_Move` (IN `p_User` VARCHAR(100), IN `p_FromPosition` INT, IN `p_ToPosition` INT, OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_PartID VARCHAR(300);
    DECLARE v_Operation VARCHAR(50);
    DECLARE v_Quantity INT;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while moving quick button from position ', p_FromPosition, ' to ', p_ToPosition, ' for user: ', p_User);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    
    IF p_FromPosition < 1 OR p_FromPosition > 10 OR p_ToPosition < 1 OR p_ToPosition > 10 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Invalid positions. From: ', p_FromPosition, ', To: ', p_ToPosition, '. Positions must be between 1 and 10.');
        ROLLBACK;
    ELSEIF p_FromPosition = p_ToPosition THEN
        SET p_Status = 1;
        SET p_ErrorMsg = 'Source and destination positions cannot be the same.';
        ROLLBACK;
    ELSE
        
        SELECT COUNT(*), PartID, Operation, Quantity 
        INTO v_Count, v_PartID, v_Operation, v_Quantity
        FROM sys_last_10_transactions 
        WHERE User = p_User AND Position = p_FromPosition;
        
        IF v_Count = 0 THEN
            SET p_Status = 1;
            SET p_ErrorMsg = CONCAT('No quick button found at position ', p_FromPosition, ' for user: ', p_User);
            ROLLBACK;
        ELSE
            
            DELETE FROM sys_last_10_transactions WHERE User = p_User AND Position = p_FromPosition;
            
            
            IF p_ToPosition < p_FromPosition THEN
                
                UPDATE sys_last_10_transactions 
                SET Position = Position + 1 
                WHERE User = p_User AND Position >= p_ToPosition AND Position < p_FromPosition;
            ELSE
                
                UPDATE sys_last_10_transactions 
                SET Position = Position - 1 
                WHERE User = p_User AND Position > p_FromPosition AND Position <= p_ToPosition;
            END IF;
            
            
            INSERT INTO sys_last_10_transactions (User, Position, PartID, Operation, Quantity, ReceiveDate)
            VALUES (p_User, p_ToPosition, v_PartID, v_Operation, v_Quantity, NOW());
            
            SET p_Status = 0;
            SET p_ErrorMsg = CONCAT('Quick button moved from position ', p_FromPosition, ' to ', p_ToPosition, ' for user: ', p_User);
            COMMIT;
        END IF;
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `sys_last_10_transactions_Move_1` (IN `p_User` VARCHAR(100), IN `p_FromPosition` INT, IN `p_ToPosition` INT, OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_PartID VARCHAR(300);
    DECLARE v_Operation VARCHAR(50);
    DECLARE v_Quantity INT;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while moving quick button from position ', p_FromPosition, ' to ', p_ToPosition, ' for user: ', p_User);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    
    IF p_FromPosition < 1 OR p_FromPosition > 10 OR p_ToPosition < 1 OR p_ToPosition > 10 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Invalid positions. From: ', p_FromPosition, ', To: ', p_ToPosition, '. Positions must be between 1 and 10.');
        ROLLBACK;
    ELSEIF p_FromPosition = p_ToPosition THEN
        SET p_Status = 1;
        SET p_ErrorMsg = 'Source and destination positions cannot be the same.';
        ROLLBACK;
    ELSE
        
        SELECT COUNT(*), PartID, Operation, Quantity 
        INTO v_Count, v_PartID, v_Operation, v_Quantity
        FROM sys_last_10_transactions 
        WHERE User = p_User AND Position = p_FromPosition;
        
        IF v_Count = 0 THEN
            SET p_Status = 1;
            SET p_ErrorMsg = CONCAT('No quick button found at position ', p_FromPosition, ' for user: ', p_User);
            ROLLBACK;
        ELSE
            
            DELETE FROM sys_last_10_transactions WHERE User = p_User AND Position = p_FromPosition;
            
            
            IF p_ToPosition < p_FromPosition THEN
                
                UPDATE sys_last_10_transactions 
                SET Position = Position + 1 
                WHERE User = p_User AND Position >= p_ToPosition AND Position < p_FromPosition;
            ELSE
                
                UPDATE sys_last_10_transactions 
                SET Position = Position - 1 
                WHERE User = p_User AND Position > p_FromPosition AND Position <= p_ToPosition;
            END IF;
            
            
            INSERT INTO sys_last_10_transactions (User, Position, PartID, Operation, Quantity, ReceiveDate)
            VALUES (p_User, p_ToPosition, v_PartID, v_Operation, v_Quantity, NOW());
            
            SET p_Status = 0;
            SET p_ErrorMsg = CONCAT('Quick button moved from position ', p_FromPosition, ' to ', p_ToPosition, ' for user: ', p_User);
            COMMIT;
        END IF;
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `sys_last_10_transactions_RemoveAndShift_ByUser` (IN `p_User` VARCHAR(100), IN `p_Position` INT, OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while removing quick button at position ', p_Position, ' for user: ', p_User);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    
    IF p_Position < 1 OR p_Position > 10 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Invalid position: ', p_Position, '. Position must be between 1 and 10.');
        ROLLBACK;
    ELSE
        
        SELECT COUNT(*) INTO v_Count FROM sys_last_10_transactions WHERE User = p_User AND Position = p_Position;
        
        IF v_Count = 0 THEN
            SET p_Status = 1;
            SET p_ErrorMsg = CONCAT('No quick button found at position ', p_Position, ' for user: ', p_User);
            ROLLBACK;
        ELSE
            
            DELETE FROM sys_last_10_transactions WHERE User = p_User AND Position = p_Position;
            SET v_RowsAffected = ROW_COUNT();
            
            
            UPDATE sys_last_10_transactions 
            SET Position = Position - 1 
            WHERE User = p_User AND Position > p_Position;
            
            SET p_Status = 0;
            SET p_ErrorMsg = CONCAT('Quick button removed from position ', p_Position, ' and remaining positions shifted up for user: ', p_User);
            COMMIT;
        END IF;
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `sys_last_10_transactions_RemoveAndShift_ByUser_1` (IN `p_User` VARCHAR(100), IN `p_Position` INT, OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while removing quick button at position ', p_Position, ' for user: ', p_User);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    
    IF p_Position < 1 OR p_Position > 10 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Invalid position: ', p_Position, '. Position must be between 1 and 10.');
        ROLLBACK;
    ELSE
        
        SELECT COUNT(*) INTO v_Count FROM sys_last_10_transactions WHERE User = p_User AND Position = p_Position;
        
        IF v_Count = 0 THEN
            SET p_Status = 1;
            SET p_ErrorMsg = CONCAT('No quick button found at position ', p_Position, ' for user: ', p_User);
            ROLLBACK;
        ELSE
            
            DELETE FROM sys_last_10_transactions WHERE User = p_User AND Position = p_Position;
            SET v_RowsAffected = ROW_COUNT();
            
            
            UPDATE sys_last_10_transactions 
            SET Position = Position - 1 
            WHERE User = p_User AND Position > p_Position;
            
            SET p_Status = 0;
            SET p_ErrorMsg = CONCAT('Quick button removed from position ', p_Position, ' and remaining positions shifted up for user: ', p_User);
            COMMIT;
        END IF;
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `sys_last_10_transactions_Update_ByUserAndPosition` (IN `p_User` VARCHAR(100), IN `p_Position` INT, IN `p_PartID` VARCHAR(300), IN `p_Operation` VARCHAR(50), IN `p_Quantity` INT, OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while updating quick button at position ', p_Position, ' for user: ', p_User);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    
    IF p_Position < 1 OR p_Position > 10 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Invalid position: ', p_Position, '. Position must be between 1 and 10.');
        ROLLBACK;
    ELSE
        
        SELECT COUNT(*) INTO v_Count FROM sys_last_10_transactions WHERE User = p_User AND Position = p_Position;
        
        IF v_Count = 0 THEN
            
            INSERT INTO sys_last_10_transactions (User, Position, PartID, Operation, Quantity, ReceiveDate)
            VALUES (p_User, p_Position, p_PartID, p_Operation, p_Quantity, NOW());
            SET p_Status = 0;
            SET p_ErrorMsg = CONCAT('Quick button created at position ', p_Position, ' for user: ', p_User);
        ELSE
            
            UPDATE sys_last_10_transactions 
            SET PartID = p_PartID,
                Operation = p_Operation,
                Quantity = p_Quantity,
                ReceiveDate = NOW()
            WHERE User = p_User AND Position = p_Position;
            
            SET v_RowsAffected = ROW_COUNT();
            
            IF v_RowsAffected > 0 THEN
                SET p_Status = 0;
                SET p_ErrorMsg = CONCAT('Quick button updated at position ', p_Position, ' for user: ', p_User);
            ELSE
                SET p_Status = 2;
                SET p_ErrorMsg = CONCAT('No changes made to quick button at position ', p_Position, ' for user: ', p_User);
            END IF;
        END IF;
        
        COMMIT;
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `sys_last_10_transactions_Update_ByUserAndPosition_1` (IN `p_User` VARCHAR(100), IN `p_Position` INT, IN `p_PartID` VARCHAR(300), IN `p_Operation` VARCHAR(50), IN `p_Quantity` INT, OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while updating quick button at position ', p_Position, ' for user: ', p_User);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    
    IF p_Position < 1 OR p_Position > 10 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Invalid position: ', p_Position, '. Position must be between 1 and 10.');
        ROLLBACK;
    ELSE
        
        SELECT COUNT(*) INTO v_Count FROM sys_last_10_transactions WHERE User = p_User AND Position = p_Position;
        
        IF v_Count = 0 THEN
            
            INSERT INTO sys_last_10_transactions (User, Position, PartID, Operation, Quantity, ReceiveDate)
            VALUES (p_User, p_Position, p_PartID, p_Operation, p_Quantity, NOW());
            SET p_Status = 0;
            SET p_ErrorMsg = CONCAT('Quick button created at position ', p_Position, ' for user: ', p_User);
        ELSE
            
            UPDATE sys_last_10_transactions 
            SET PartID = p_PartID,
                Operation = p_Operation,
                Quantity = p_Quantity,
                ReceiveDate = NOW()
            WHERE User = p_User AND Position = p_Position;
            
            SET v_RowsAffected = ROW_COUNT();
            
            IF v_RowsAffected > 0 THEN
                SET p_Status = 0;
                SET p_ErrorMsg = CONCAT('Quick button updated at position ', p_Position, ' for user: ', p_User);
            ELSE
                SET p_Status = 2;
                SET p_ErrorMsg = CONCAT('No changes made to quick button at position ', p_Position, ' for user: ', p_User);
            END IF;
        END IF;
        
        COMMIT;
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `sys_roles_Get_ById` (IN `p_ID` INT, OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while retrieving role with ID: ', p_ID);
    END;
    
    SELECT COUNT(*) INTO v_Count FROM sys_roles WHERE ID = p_ID;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Role not found with ID: ', p_ID);
        SELECT NULL as ID, NULL as RoleName, NULL as Description;
    ELSE
        SELECT * FROM sys_roles WHERE ID = p_ID LIMIT 1;
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Role retrieved successfully with ID: ', p_ID);
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `sys_SetUserAccessType` (IN `p_UserName` VARCHAR(100), IN `p_AccessType` VARCHAR(50), OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_UserCount INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while setting access type for user: ', p_UserName);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    
    SELECT COUNT(*) INTO v_UserCount FROM usr_users WHERE User = p_UserName;
    IF v_UserCount = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('User not found: ', p_UserName);
        ROLLBACK;
    ELSE
        
        UPDATE usr_users 
        SET AccessType = p_AccessType, 
            ModifiedDate = NOW() 
        WHERE User = p_UserName;
        
        SET v_RowsAffected = ROW_COUNT();
        
        IF v_RowsAffected > 0 THEN
            SET p_Status = 0;
            SET p_ErrorMsg = CONCAT('Access type updated successfully for user: ', p_UserName);
        ELSE
            SET p_Status = 2;
            SET p_ErrorMsg = CONCAT('No changes made to access type for user: ', p_UserName);
        END IF;
        
        COMMIT;
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `sys_user_roles_Add` (IN `p_UserID` INT, IN `p_RoleID` INT, IN `p_AssignedBy` VARCHAR(100), OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_UserCount INT DEFAULT 0;
    DECLARE v_RoleCount INT DEFAULT 0;
    DECLARE v_ExistingCount INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while adding role assignment for user ID: ', p_UserID);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    
    SELECT COUNT(*) INTO v_UserCount FROM usr_users WHERE ID = p_UserID;
    IF v_UserCount = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('User ID not found: ', p_UserID);
        ROLLBACK;
    ELSE
        
        SELECT COUNT(*) INTO v_RoleCount FROM sys_roles WHERE ID = p_RoleID;
        IF v_RoleCount = 0 THEN
            SET p_Status = 1;
            SET p_ErrorMsg = CONCAT('Role ID not found: ', p_RoleID);
            ROLLBACK;
        ELSE
            
            SELECT COUNT(*) INTO v_ExistingCount FROM sys_user_roles WHERE UserID = p_UserID AND RoleID = p_RoleID;
            IF v_ExistingCount > 0 THEN
                SET p_Status = 1;
                SET p_ErrorMsg = CONCAT('Role assignment already exists for user ID: ', p_UserID);
                ROLLBACK;
            ELSE
                INSERT INTO sys_user_roles (UserID, RoleID, AssignedBy, AssignedDate)
                VALUES (p_UserID, p_RoleID, p_AssignedBy, NOW());
                
                SET p_Status = 0;
                SET p_ErrorMsg = CONCAT('Role assignment added successfully for user ID: ', p_UserID);
                COMMIT;
            END IF;
        END IF;
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `sys_user_roles_Delete` (IN `p_UserID` INT, IN `p_RoleID` INT, OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_ExistingCount INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while deleting role assignment for user ID: ', p_UserID);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    
    SELECT COUNT(*) INTO v_ExistingCount FROM sys_user_roles WHERE UserID = p_UserID AND RoleID = p_RoleID;
    IF v_ExistingCount = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Role assignment not found for user ID: ', p_UserID, ' and role ID: ', p_RoleID);
        ROLLBACK;
    ELSE
        DELETE FROM sys_user_roles WHERE UserID = p_UserID AND RoleID = p_RoleID;
        SET v_RowsAffected = ROW_COUNT();
        
        IF v_RowsAffected > 0 THEN
            SET p_Status = 0;
            SET p_ErrorMsg = CONCAT('Role assignment deleted successfully for user ID: ', p_UserID);
        ELSE
            SET p_Status = 2;
            SET p_ErrorMsg = CONCAT('Failed to delete role assignment for user ID: ', p_UserID);
        END IF;
        
        COMMIT;
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `sys_user_roles_Update` (IN `p_UserID` INT, IN `p_NewRoleID` INT, IN `p_AssignedBy` VARCHAR(100), OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_UserCount INT DEFAULT 0;
    DECLARE v_RoleCount INT DEFAULT 0;
    DECLARE v_ExistingCount INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while updating role assignment for user ID: ', p_UserID);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    
    SELECT COUNT(*) INTO v_UserCount FROM usr_users WHERE ID = p_UserID;
    IF v_UserCount = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('User ID not found: ', p_UserID);
        ROLLBACK;
    ELSE
        
        SELECT COUNT(*) INTO v_RoleCount FROM sys_roles WHERE ID = p_NewRoleID;
        IF v_RoleCount = 0 THEN
            SET p_Status = 1;
            SET p_ErrorMsg = CONCAT('Role ID not found: ', p_NewRoleID);
            ROLLBACK;
        ELSE
            
            SELECT COUNT(*) INTO v_ExistingCount FROM sys_user_roles WHERE UserID = p_UserID;
            IF v_ExistingCount = 0 THEN
                
                INSERT INTO sys_user_roles (UserID, RoleID, AssignedBy, AssignedDate)
                VALUES (p_UserID, p_NewRoleID, p_AssignedBy, NOW());
                
                SET p_Status = 0;
                SET p_ErrorMsg = CONCAT('New role assignment created for user ID: ', p_UserID);
            ELSE
                
                UPDATE sys_user_roles 
                SET RoleID = p_NewRoleID,
                    AssignedBy = p_AssignedBy,
                    AssignedDate = NOW()
                WHERE UserID = p_UserID;
                
                SET v_RowsAffected = ROW_COUNT();
                
                IF v_RowsAffected > 0 THEN
                    SET p_Status = 0;
                    SET p_ErrorMsg = CONCAT('Role assignment updated successfully for user ID: ', p_UserID);
                ELSE
                    SET p_Status = 2;
                    SET p_ErrorMsg = CONCAT('No changes made to role assignment for user ID: ', p_UserID);
                END IF;
            END IF;
            
            COMMIT;
        END IF;
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `sys_VerifyDatabaseSchema` (OUT `p_Status` INT, OUT `p_ErrorMsg` TEXT)   BEGIN
    DECLARE v_TableCount INT DEFAULT 0;
    DECLARE v_MissingTables TEXT DEFAULT '';
    DECLARE v_Expected INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred during schema verification';
    END;
    
    -- Expected core tables from LiveDatabase.sql analysis
    SET v_Expected = 11;
    
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
        'md_part_ids',
        'md_locations', 
        'md_operation_numbers'
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
    
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `sys_VerifyTableColumns` (IN `p_TableName` VARCHAR(64), OUT `p_Status` INT, OUT `p_ErrorMsg` TEXT)   BEGIN
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
    
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `test_final_batch_generation` ()   BEGIN
    DECLARE v_status INT;
    DECLARE v_error_msg VARCHAR(255);
    
    CALL inv_inventory_GetNextBatchNumber(v_status, v_error_msg);
    SELECT 
        'Batch Generation Test' as Check_Type,
        v_status as Status_Code, 
        v_error_msg as Message,
        CASE 
            WHEN v_status = 0 THEN '✅ SUCCESS'
            WHEN v_status = 1 THEN '⚠️ WARNING'  
            ELSE '❌ ERROR'
        END as Result;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `usr_ui_settings_Delete_ByUserId` (IN `p_UserId` VARCHAR(64), OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while deleting user settings for user: ', p_UserId);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    DELETE FROM usr_ui_settings WHERE UserId = p_UserId;
    
    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('User settings deleted successfully for user: ', p_UserId);
    
    COMMIT;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `usr_ui_settings_GetJsonSetting` (IN `p_UserId` VARCHAR(100), OUT `p_SettingJson` TEXT, OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred while getting JSON setting';
    END;
    
    SELECT COUNT(*) INTO v_Count FROM usr_ui_settings WHERE UserId = p_UserId;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('No settings found for user: ', p_UserId);
        SET p_SettingJson = NULL;
    ELSE
        SELECT SettingsJson INTO p_SettingJson
        FROM usr_ui_settings 
        WHERE UserId = p_UserId 
        LIMIT 1;
        
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('JSON setting retrieved successfully for user: ', p_UserId);
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `usr_ui_settings_GetSettingsJson_ByUserId` (IN `p_UserId` VARCHAR(100), OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while retrieving settings for user: ', p_UserId);
    END;
    
    SELECT COUNT(*) INTO v_Count FROM usr_ui_settings WHERE UserId = p_UserId;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('No settings found for user: ', p_UserId);
        SELECT NULL as SettingsJson;
    ELSE
        SELECT SettingsJson FROM usr_ui_settings WHERE UserId = p_UserId LIMIT 1;
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Settings retrieved successfully for user: ', p_UserId);
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `usr_ui_settings_GetShortcutsJson` (IN `p_UserId` VARCHAR(100), OUT `p_ShortcutsJson` TEXT, OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred while getting shortcuts JSON';
    END;
    
    SELECT COUNT(*) INTO v_Count FROM usr_ui_settings WHERE UserId = p_UserId;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('No shortcuts found for user: ', p_UserId);
        SET p_ShortcutsJson = NULL;
    ELSE
        SELECT ShortcutsJson INTO p_ShortcutsJson
        FROM usr_ui_settings 
        WHERE UserId = p_UserId 
        LIMIT 1;
        
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Shortcuts JSON retrieved successfully for user: ', p_UserId);
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `usr_ui_settings_SetJsonSetting` (IN `p_UserId` VARCHAR(100), IN `p_DgvName` VARCHAR(100), IN `p_SettingJson` TEXT, OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred while setting JSON setting';
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    INSERT INTO usr_ui_settings (UserId, DgvName, SettingsJson, CreatedDate, ModifiedDate)
    VALUES (p_UserId, p_DgvName, p_SettingJson, NOW(), NOW())
    ON DUPLICATE KEY UPDATE 
        SettingsJson = p_SettingJson,
        ModifiedDate = NOW();
    
    SET p_Status = 0;
    SET p_ErrorMsg = 'JSON setting updated successfully';
    
    COMMIT;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `usr_ui_settings_SetShortcutsJson` (IN `p_UserId` VARCHAR(100), IN `p_ShortcutsJson` TEXT, OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred while setting shortcuts JSON';
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    INSERT INTO usr_ui_settings (UserId, ShortcutsJson, CreatedDate, ModifiedDate)
    VALUES (p_UserId, p_ShortcutsJson, NOW(), NOW())
    ON DUPLICATE KEY UPDATE 
        ShortcutsJson = p_ShortcutsJson,
        ModifiedDate = NOW();
    
    SET p_Status = 0;
    SET p_ErrorMsg = 'Shortcuts JSON updated successfully';
    
    COMMIT;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `usr_ui_settings_SetThemeJson` (IN `p_UserId` VARCHAR(100), IN `p_ThemeJson` TEXT, OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred while setting theme JSON';
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    INSERT INTO usr_ui_settings (UserId, SettingsJson, CreatedDate, ModifiedDate)
    VALUES (p_UserId, p_ThemeJson, NOW(), NOW())
    ON DUPLICATE KEY UPDATE 
        SettingsJson = p_ThemeJson,
        ModifiedDate = NOW();
    
    SET p_Status = 0;
    SET p_ErrorMsg = 'Theme JSON updated successfully';
    
    COMMIT;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `usr_users_Add_User` (IN `p_User` VARCHAR(100), IN `p_FullName` VARCHAR(200), IN `p_Shift` VARCHAR(50), IN `p_VitsUser` TINYINT(1), IN `p_Pin` VARCHAR(20), IN `p_LastShownVersion` VARCHAR(20), IN `p_HideChangeLog` VARCHAR(10), IN `p_Theme_Name` VARCHAR(50), IN `p_Theme_FontSize` INT, IN `p_VisualUserName` VARCHAR(100), IN `p_VisualPassword` VARCHAR(100), IN `p_WipServerAddress` VARCHAR(100), IN `p_WIPDatabase` VARCHAR(100), IN `p_WipServerPort` VARCHAR(10), OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while creating user: ', p_User);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    
    SELECT COUNT(*) INTO v_Count FROM usr_users WHERE User = p_User;
    
    IF v_Count > 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('User already exists: ', p_User);
        ROLLBACK;
    ELSE
        INSERT INTO usr_users (
            User, `Full Name`, Shift, VitsUser, Pin,
            LastShownVersion, HideChangeLog, Theme_Name, Theme_FontSize,
            VisualUserName, VisualPassword, WipServerAddress, 
            WIPDatabase, WipServerPort
        ) VALUES (
            p_User, p_FullName, p_Shift, p_VitsUser, p_Pin,
            p_LastShownVersion, p_HideChangeLog, p_Theme_Name, p_Theme_FontSize,
            p_VisualUserName, p_VisualPassword, p_WipServerAddress,
            p_WIPDatabase, p_WipServerPort
        );
        
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('User created successfully: ', p_User);
        COMMIT;
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `usr_users_Delete_User` (IN `p_User` VARCHAR(100), OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while deleting user: ', p_User);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    
    SELECT COUNT(*) INTO v_Count FROM usr_users WHERE User = p_User;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('User not found: ', p_User);
        ROLLBACK;
    ELSE
        DELETE FROM usr_users WHERE User = p_User;
        SET v_RowsAffected = ROW_COUNT();
        
        IF v_RowsAffected > 0 THEN
            SET p_Status = 0;
            SET p_ErrorMsg = CONCAT('User deleted successfully: ', p_User);
        ELSE
            SET p_Status = 2;
            SET p_ErrorMsg = CONCAT('Failed to delete user: ', p_User);
        END IF;
        
        COMMIT;
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `usr_users_Exists` (IN `p_User` VARCHAR(100), OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while checking user existence: ', p_User);
    END;
    
    SELECT COUNT(*) INTO v_Count FROM usr_users WHERE User = p_User;
    SELECT v_Count as UserExists;
    
    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('User existence check completed for: ', p_User, ' (Exists: ', IF(v_Count > 0, 'Yes', 'No'), ')');
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `usr_users_GetFullName_ByUser` (IN `p_User` VARCHAR(100), OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while retrieving full name for user: ', p_User);
    END;
    
    SELECT COUNT(*) INTO v_Count FROM usr_users WHERE User = p_User;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('User not found: ', p_User);
    ELSE
        SELECT `Full Name` FROM usr_users WHERE User = p_User LIMIT 1;
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Full name retrieved successfully for user: ', p_User);
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `usr_users_GetUserSetting_ByUserAndField` (IN `p_User` VARCHAR(100), IN `p_Field` VARCHAR(100), OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_FieldValue TEXT DEFAULT NULL;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while retrieving setting ', p_Field, ' for user: ', p_User);
    END;
    
    
    SELECT COUNT(*) INTO v_Count FROM usr_users WHERE User = p_User;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('User not found: ', p_User);
        SELECT NULL as FieldValue;
    ELSE
        
        CASE p_Field
            WHEN 'LastShownVersion' THEN SELECT LastShownVersion INTO v_FieldValue FROM usr_users WHERE User = p_User LIMIT 1;
            WHEN 'HideChangeLog' THEN SELECT HideChangeLog INTO v_FieldValue FROM usr_users WHERE User = p_User LIMIT 1;
            WHEN 'Theme_Name' THEN SELECT Theme_Name INTO v_FieldValue FROM usr_users WHERE User = p_User LIMIT 1;
            WHEN 'Theme_FontSize' THEN SELECT CAST(Theme_FontSize AS CHAR) INTO v_FieldValue FROM usr_users WHERE User = p_User LIMIT 1;
            WHEN 'VisualUserName' THEN SELECT VisualUserName INTO v_FieldValue FROM usr_users WHERE User = p_User LIMIT 1;
            WHEN 'VisualPassword' THEN SELECT VisualPassword INTO v_FieldValue FROM usr_users WHERE User = p_User LIMIT 1;
            WHEN 'WipServerAddress' THEN SELECT WipServerAddress INTO v_FieldValue FROM usr_users WHERE User = p_User LIMIT 1;
            WHEN 'WIPDatabase' THEN SELECT WIPDatabase INTO v_FieldValue FROM usr_users WHERE User = p_User LIMIT 1;
            WHEN 'WipServerPort' THEN SELECT WipServerPort INTO v_FieldValue FROM usr_users WHERE User = p_User LIMIT 1;
            WHEN 'FullName' THEN SELECT `Full Name` INTO v_FieldValue FROM usr_users WHERE User = p_User LIMIT 1;
            WHEN 'Shift' THEN SELECT Shift INTO v_FieldValue FROM usr_users WHERE User = p_User LIMIT 1;
            WHEN 'Pin' THEN SELECT Pin INTO v_FieldValue FROM usr_users WHERE User = p_User LIMIT 1;
            ELSE SET v_FieldValue = NULL;
        END CASE;
        
        SELECT v_FieldValue as FieldValue;
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Setting ', p_Field, ' retrieved successfully for user: ', p_User);
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `usr_users_Get_All` (OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = 'Database error occurred while retrieving all users';
    END;
    
    SELECT COUNT(*) INTO v_Count FROM usr_users;
    SELECT * FROM usr_users ORDER BY `Full Name`;
    
    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('Retrieved ', v_Count, ' users successfully');
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `usr_users_Get_ByUser` (IN `p_User` VARCHAR(100), OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while retrieving user: ', p_User);
    END;
    
    SELECT COUNT(*) INTO v_Count FROM usr_users WHERE User = p_User;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('User not found: ', p_User);
        SELECT NULL as User, NULL as `Full Name`; 
    ELSE
        SELECT * FROM usr_users WHERE User = p_User LIMIT 1;
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('User retrieved successfully: ', p_User);
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `usr_users_SetUserSetting_ByUserAndField` (IN `p_User` VARCHAR(100), IN `p_Field` VARCHAR(100), IN `p_Value` TEXT, OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while setting ', p_Field, ' for user: ', p_User);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    
    SELECT COUNT(*) INTO v_Count FROM usr_users WHERE User = p_User;
    
    IF v_Count = 0 THEN
        
        INSERT INTO usr_users (User, `Full Name`) 
        VALUES (p_User, p_User);
    END IF;
    
    
    CASE p_Field
        WHEN 'LastShownVersion' THEN 
            UPDATE usr_users SET LastShownVersion = p_Value, ModifiedDate = NOW() WHERE User = p_User;
        WHEN 'HideChangeLog' THEN 
            UPDATE usr_users SET HideChangeLog = p_Value, ModifiedDate = NOW() WHERE User = p_User;
        WHEN 'Theme_Name' THEN 
            UPDATE usr_users SET Theme_Name = p_Value, ModifiedDate = NOW() WHERE User = p_User;
        WHEN 'Theme_FontSize' THEN 
            UPDATE usr_users SET Theme_FontSize = CAST(p_Value AS UNSIGNED), ModifiedDate = NOW() WHERE User = p_User;
        WHEN 'VisualUserName' THEN 
            UPDATE usr_users SET VisualUserName = p_Value, ModifiedDate = NOW() WHERE User = p_User;
        WHEN 'VisualPassword' THEN 
            UPDATE usr_users SET VisualPassword = p_Value, ModifiedDate = NOW() WHERE User = p_User;
        WHEN 'WipServerAddress' THEN 
            UPDATE usr_users SET WipServerAddress = p_Value, ModifiedDate = NOW() WHERE User = p_User;
        WHEN 'WIPDatabase' THEN 
            UPDATE usr_users SET WIPDatabase = p_Value, ModifiedDate = NOW() WHERE User = p_User;
        WHEN 'WipServerPort' THEN 
            UPDATE usr_users SET WipServerPort = p_Value, ModifiedDate = NOW() WHERE User = p_User;
        WHEN 'FullName' THEN 
            UPDATE usr_users SET `Full Name` = p_Value WHERE User = p_User;
        WHEN 'Shift' THEN 
            UPDATE usr_users SET Shift = p_Value, ModifiedDate = NOW() WHERE User = p_User;
        WHEN 'Pin' THEN 
            UPDATE usr_users SET Pin = p_Value, ModifiedDate = NOW() WHERE User = p_User;
        ELSE 
            SET p_Status = 1;
            SET p_ErrorMsg = CONCAT('Unknown field: ', p_Field);
            ROLLBACK;
    END CASE;
    
    IF p_Status IS NULL THEN
        SET v_RowsAffected = ROW_COUNT();
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Setting ', p_Field, ' updated successfully for user: ', p_User, ' (Rows affected: ', v_RowsAffected, ')');
        COMMIT;
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `usr_users_Update_User` (IN `p_User` VARCHAR(100), IN `p_FullName` VARCHAR(200), IN `p_Shift` VARCHAR(50), IN `p_Pin` VARCHAR(20), IN `p_VisualUserName` VARCHAR(100), IN `p_VisualPassword` VARCHAR(100), OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while updating user: ', p_User);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    
    SELECT COUNT(*) INTO v_Count FROM usr_users WHERE User = p_User;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('User not found: ', p_User);
        ROLLBACK;
    ELSE
        UPDATE usr_users 
        SET `Full Name` = p_FullName,
            Shift = p_Shift,
            Pin = p_Pin,
            VisualUserName = p_VisualUserName,
            VisualPassword = p_VisualPassword
        WHERE User = p_User;
        
        SET v_RowsAffected = ROW_COUNT();
        
        IF v_RowsAffected > 0 THEN
            SET p_Status = 0;
            SET p_ErrorMsg = CONCAT('User updated successfully: ', p_User);
        ELSE
            SET p_Status = 2;
            SET p_ErrorMsg = CONCAT('No changes made to user: ', p_User);
        END IF;
        
        COMMIT;
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `usr_user_roles_GetRoleId_ByUserId` (IN `p_UserID` INT, OUT `p_Status` INT, OUT `p_ErrorMsg` VARCHAR(255))   BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while retrieving role for user ID: ', p_UserID);
    END;
    
    SELECT COUNT(*) INTO v_Count FROM sys_user_roles WHERE UserID = p_UserID;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('No role assignment found for user ID: ', p_UserID);
        SELECT NULL as RoleID;
    ELSE
        SELECT RoleID FROM sys_user_roles WHERE UserID = p_UserID LIMIT 1;
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Role retrieved successfully for user ID: ', p_UserID);
    END IF;
END$$

DELIMITER ;
