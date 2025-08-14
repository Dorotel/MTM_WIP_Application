-- ================================================================================
-- MTM INVENTORY APPLICATION - MASTER DATA STORED PROCEDURES
-- ================================================================================
-- File: 03_Master_Data_Procedures.sql
-- Purpose: Master data management for parts, operations, locations, and item types
-- Created: August 10, 2025
-- Updated: August 10, 2025 - UNIFORM PARAMETER NAMING (WITH p_ prefixes)
-- Target Database: mtm_wip_application_test
-- MySQL Version: 5.7.24+ (MAMP Compatible)
-- ================================================================================

-- Drop procedures if they exist (for clean deployment)
DROP PROCEDURE IF EXISTS md_part_ids_Get_All;
DROP PROCEDURE IF EXISTS md_part_ids_Get_ByItemNumber;
DROP PROCEDURE IF EXISTS md_part_ids_GetItemType_ByPartID;
DROP PROCEDURE IF EXISTS md_part_ids_Exists_ByPartID;
DROP PROCEDURE IF EXISTS md_part_ids_Add_Part;
DROP PROCEDURE IF EXISTS md_part_ids_Update_Part;
DROP PROCEDURE IF EXISTS md_part_ids_Delete_ByItemNumber;
DROP PROCEDURE IF EXISTS md_operation_numbers_Get_All;
DROP PROCEDURE IF EXISTS md_operation_numbers_Exists_ByOperation;
DROP PROCEDURE IF EXISTS md_operation_numbers_Add_Operation;
DROP PROCEDURE IF EXISTS md_operation_numbers_Update_Operation;
DROP PROCEDURE IF EXISTS md_operation_numbers_Delete_ByOperation;
DROP PROCEDURE IF EXISTS md_locations_Get_All;
DROP PROCEDURE IF EXISTS md_locations_Exists_ByLocation;
DROP PROCEDURE IF EXISTS md_locations_Add_Location;
DROP PROCEDURE IF EXISTS md_locations_Update_Location;
DROP PROCEDURE IF EXISTS md_locations_Delete_ByLocation;
DROP PROCEDURE IF EXISTS md_item_types_Get_All;
DROP PROCEDURE IF EXISTS md_item_types_GetDistinct;
DROP PROCEDURE IF EXISTS md_item_types_Exists_ByItemType;
DROP PROCEDURE IF EXISTS md_item_types_Add_ItemType;
DROP PROCEDURE IF EXISTS md_item_types_Update_ItemType;
DROP PROCEDURE IF EXISTS md_item_types_Delete_ByType;

-- ================================================================================
-- PART ID MANAGEMENT PROCEDURES
-- ================================================================================

-- Get all part IDs with status reporting
DELIMITER $$
CREATE PROCEDURE md_part_ids_Get_All(
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
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
END $$
DELIMITER ;

-- Get part ID by specific part ID
DELIMITER $$
CREATE PROCEDURE md_part_ids_Get_ByItemNumber(
    IN p_PartID VARCHAR(300),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
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
END $$
DELIMITER ;

-- Get item type by part ID
DELIMITER $$
CREATE PROCEDURE md_part_ids_GetItemType_ByPartID(
    IN p_PartID VARCHAR(300),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
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
END $$
DELIMITER ;

-- Check if part ID exists
DELIMITER $$
CREATE PROCEDURE md_part_ids_Exists_ByPartID(
    IN p_PartID VARCHAR(300),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
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
END $$
DELIMITER ;

-- Add new part ID
DELIMITER $$
CREATE PROCEDURE md_part_ids_Add_Part(
    IN p_PartID VARCHAR(300), -- Match current p_ItemNumber parameter (but keep consistent naming)
    IN p_Customer VARCHAR(300), -- Add missing parameter from current procedure
    IN p_Description VARCHAR(300), -- Add missing parameter from current procedure
    IN p_IssuedBy VARCHAR(100),
    IN p_ItemType VARCHAR(100), -- Match current parameter size
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while adding part ID: ', p_PartID);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- Check if part ID already exists
    SELECT COUNT(*) INTO v_Count FROM md_part_ids WHERE PartID = p_PartID;
    
    IF v_Count > 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Part ID already exists: ', p_PartID);
        ROLLBACK;
    ELSE
        INSERT INTO md_part_ids (PartID, Customer, Description, IssuedBy, ItemType)
        VALUES (p_PartID, p_Customer, p_Description, p_IssuedBy, p_ItemType);
        
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Part ID added successfully: ', p_PartID);
        COMMIT;
    END IF;
END $$
DELIMITER ;

-- Update existing part ID
DELIMITER $$
CREATE PROCEDURE md_part_ids_Update_Part(
    IN p_ID INT,
    IN p_ItemNumber VARCHAR(300), -- Match current procedure parameter name
    IN p_Customer VARCHAR(300), -- Add missing parameter from current procedure
    IN p_Description VARCHAR(300), -- Add missing parameter from current procedure
    IN p_IssuedBy VARCHAR(100),
    IN p_ItemType VARCHAR(100), -- Match current parameter size
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while updating part ID: ', p_ItemNumber);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- Check if ID exists
    SELECT COUNT(*) INTO v_Count FROM md_part_ids WHERE ID = p_ID;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Part ID not found for ID: ', p_ID);
        ROLLBACK;
    ELSE
        UPDATE md_part_ids 
        SET PartID = p_ItemNumber,
            Customer = p_Customer,
            Description = p_Description,
            IssuedBy = p_IssuedBy,
            ItemType = p_ItemType
        WHERE ID = p_ID;
        
        SET v_RowsAffected = ROW_COUNT();
        
        IF v_RowsAffected > 0 THEN
            SET p_Status = 0;
            SET p_ErrorMsg = CONCAT('Part updated successfully for ID: ', p_ID);
        ELSE
            SET p_Status = 2;
            SET p_ErrorMsg = CONCAT('No changes made to part for ID: ', p_ID);
        END IF;
        
        COMMIT;
    END IF;
END $$
DELIMITER ;

-- Delete part ID
DELIMITER $$
CREATE PROCEDURE md_part_ids_Delete_ByItemNumber(
    IN p_PartID VARCHAR(300),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while deleting part ID: ', p_PartID);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- Check if part ID exists
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
END $$
DELIMITER ;

-- ================================================================================
-- OPERATION NUMBER MANAGEMENT PROCEDURES
-- ================================================================================

-- Get all operation numbers with status reporting
DELIMITER $$
CREATE PROCEDURE md_operation_numbers_Get_All(
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
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
END $$
DELIMITER ;

-- Check if operation exists
DELIMITER $$
CREATE PROCEDURE md_operation_numbers_Exists_ByOperation(
    IN p_Operation VARCHAR(50),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
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
END $$
DELIMITER ;

-- Add new operation
DELIMITER $$
CREATE PROCEDURE md_operation_numbers_Add_Operation(
    IN p_Operation VARCHAR(50),
    IN p_IssuedBy VARCHAR(100),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while adding operation: ', p_Operation);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- Check if operation already exists
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
END $$
DELIMITER ;

-- Update existing operation
DELIMITER $$
CREATE PROCEDURE md_operation_numbers_Update_Operation(
    IN p_OldOperation VARCHAR(50),
    IN p_NewOperation VARCHAR(50),
    IN p_IssuedBy VARCHAR(100),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while updating operation: ', p_OldOperation);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- Check if old operation exists
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
END $$
DELIMITER ;

-- Delete operation
DELIMITER $$
CREATE PROCEDURE md_operation_numbers_Delete_ByOperation(
    IN p_Operation VARCHAR(50),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while deleting operation: ', p_Operation);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- Check if operation exists
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
END $$
DELIMITER ;

-- ================================================================================
-- LOCATION MANAGEMENT PROCEDURES
-- ================================================================================

-- Get all locations with status reporting
DELIMITER $$
CREATE PROCEDURE md_locations_Get_All(
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
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
END $$
DELIMITER ;

-- Check if location exists
DELIMITER $$
CREATE PROCEDURE md_locations_Exists_ByLocation(
    IN p_Location VARCHAR(100),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
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
END $$
DELIMITER ;

-- Add new location
DELIMITER $$
CREATE PROCEDURE md_locations_Add_Location(
    IN p_Location VARCHAR(100),
    IN p_Building VARCHAR(100),
    IN p_IssuedBy VARCHAR(100),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while adding location: ', p_Location);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- Check if location already exists
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
END $$
DELIMITER ;

-- Update existing location
DELIMITER $$
CREATE PROCEDURE md_locations_Update_Location(
    IN p_OldLocation VARCHAR(100),
    IN p_NewLocation VARCHAR(100),
    IN p_Building VARCHAR(100),
    IN p_IssuedBy VARCHAR(100),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while updating location: ', p_OldLocation);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- Check if old location exists
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
END $$
DELIMITER ;

-- Delete location
DELIMITER $$
CREATE PROCEDURE md_locations_Delete_ByLocation(
    IN p_Location VARCHAR(100),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while deleting location: ', p_Location);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- Check if location exists
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
END $$
DELIMITER ;

-- ================================================================================
-- ITEM TYPE MANAGEMENT PROCEDURES
-- ================================================================================

-- Get all item types with status reporting
DELIMITER $$
CREATE PROCEDURE md_item_types_Get_All(
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
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
END $$
DELIMITER ;

-- Get distinct item types for dropdown lists
DELIMITER $$
CREATE PROCEDURE md_item_types_GetDistinct(
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
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
END $$
DELIMITER ;

-- Check if item type exists
DELIMITER $$
CREATE PROCEDURE md_item_types_Exists_ByItemType(
    IN p_ItemType VARCHAR(50),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
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
END $$
DELIMITER ;

-- Add new item type
DELIMITER $$
CREATE PROCEDURE md_item_types_Add_ItemType(
    IN p_ItemType VARCHAR(50),
    IN p_IssuedBy VARCHAR(100),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while adding item type: ', p_ItemType);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- Check if item type already exists
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
END $$
DELIMITER ;

-- Update existing item type
DELIMITER $$
CREATE PROCEDURE md_item_types_Update_ItemType(
    IN p_OldItemType VARCHAR(50),
    IN p_NewItemType VARCHAR(50),
    IN p_IssuedBy VARCHAR(100),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while updating item type: ', p_OldItemType);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- Check if old item type exists
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
END $$
DELIMITER ;

-- Delete item type
DELIMITER $$
CREATE PROCEDURE md_item_types_Delete_ByType(
    IN p_ItemType VARCHAR(50),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while deleting item type: ', p_ItemType);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- Check if item type exists
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
END $$
DELIMITER ;

-- ================================================================================
-- END OF MASTER DATA PROCEDURES
-- ================================================================================
