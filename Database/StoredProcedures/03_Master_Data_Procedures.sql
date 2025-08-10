-- ================================================================================
-- MTM INVENTORY APPLICATION - MASTER DATA STORED PROCEDURES
-- ================================================================================
-- File: 03_Master_Data_Procedures.sql
-- Purpose: Part numbers, operations, locations, and item types management
-- Created: August 10, 2025
-- Updated: For MySQL 5.7.24 compatibility (MAMP Compatible)
-- Target Database: mtm_wip_application
-- MySQL Version: 5.7.24+ (MAMP Compatible)
-- ================================================================================

-- Drop procedures if they exist (for clean deployment)
DROP PROCEDURE IF EXISTS md_part_ids_Add_Part;
DROP PROCEDURE IF EXISTS md_part_ids_Update_Part;
DROP PROCEDURE IF EXISTS md_part_ids_Delete_Part;
DROP PROCEDURE IF EXISTS md_part_ids_Delete_ByItemNumber;
DROP PROCEDURE IF EXISTS md_part_ids_Get_All;
DROP PROCEDURE IF EXISTS md_part_ids_Get_ByItemNumber;
DROP PROCEDURE IF EXISTS md_part_ids_GetItemType_ByPartID;
DROP PROCEDURE IF EXISTS md_part_ids_Exists_ByItemNumber;
DROP PROCEDURE IF EXISTS md_operation_numbers_Add_Operation;
DROP PROCEDURE IF EXISTS md_operation_numbers_Update_Operation;
DROP PROCEDURE IF EXISTS md_operation_numbers_Delete_ByOperation;
DROP PROCEDURE IF EXISTS md_operation_numbers_Get_All;
DROP PROCEDURE IF EXISTS md_operation_numbers_Exists_ByOperation;
DROP PROCEDURE IF EXISTS md_locations_Add_Location;
DROP PROCEDURE IF EXISTS md_locations_Update_Location;
DROP PROCEDURE IF EXISTS md_locations_Delete_Location;
DROP PROCEDURE IF EXISTS md_locations_Get_All;
DROP PROCEDURE IF EXISTS md_locations_Exists_ByLocation;
DROP PROCEDURE IF EXISTS md_item_types_Add_ItemType;
DROP PROCEDURE IF EXISTS md_item_types_Update_ItemType;
DROP PROCEDURE IF EXISTS md_item_types_Delete_ItemType;
DROP PROCEDURE IF EXISTS md_item_types_Get_All;
DROP PROCEDURE IF EXISTS md_item_types_GetDistinct;
DROP PROCEDURE IF EXISTS md_item_types_Exists_ByItemType;

-- ================================================================================
-- PART MANAGEMENT PROCEDURES (MySQL 5.7.24 Compatible)
-- ================================================================================

-- Add new part with status reporting
DELIMITER $$
CREATE PROCEDURE md_part_ids_Add_Part(
    IN p_ItemNumber VARCHAR(100),
    IN p_Customer VARCHAR(100),
    IN p_Description TEXT,
    IN p_IssuedBy VARCHAR(100),
    IN p_ItemType VARCHAR(50),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while creating part: ', p_ItemNumber);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- Check if part already exists
    SELECT COUNT(*) INTO v_Count FROM md_part_ids WHERE ItemNumber = p_ItemNumber;
    
    IF v_Count > 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Part already exists: ', p_ItemNumber);
        ROLLBACK;
    ELSE
        INSERT INTO md_part_ids (
            ItemNumber, Customer, Description, IssuedBy, ItemType, CreatedDate
        ) VALUES (
            p_ItemNumber, p_Customer, p_Description, p_IssuedBy, p_ItemType, NOW()
        );
        
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Part created successfully: ', p_ItemNumber);
        COMMIT;
    END IF;
END $$
DELIMITER ;

-- Update existing part with status reporting
DELIMITER $$
CREATE PROCEDURE md_part_ids_Update_Part(
    IN p_ID INT,
    IN p_ItemNumber VARCHAR(100),
    IN p_Customer VARCHAR(100),
    IN p_Description TEXT,
    IN p_IssuedBy VARCHAR(100),
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
        SET p_ErrorMsg = CONCAT('Database error occurred while updating part ID: ', p_ID);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- Check if part exists
    SELECT COUNT(*) INTO v_Count FROM md_part_ids WHERE ID = p_ID;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Part not found with ID: ', p_ID);
        ROLLBACK;
    ELSE
        UPDATE md_part_ids 
        SET ItemNumber = p_ItemNumber,
            Customer = p_Customer,
            Description = p_Description,
            IssuedBy = p_IssuedBy,
            ItemType = p_ItemType,
            ModifiedDate = NOW()
        WHERE ID = p_ID;
        
        SET v_RowsAffected = ROW_COUNT();
        
        IF v_RowsAffected > 0 THEN
            SET p_Status = 0;
            SET p_ErrorMsg = CONCAT('Part updated successfully: ', p_ItemNumber);
        ELSE
            SET p_Status = 2;
            SET p_ErrorMsg = CONCAT('No changes made to part ID: ', p_ID);
        END IF;
        
        COMMIT;
    END IF;
END $$
DELIMITER ;

-- Delete part by ID with status reporting
DELIMITER $$
CREATE PROCEDURE md_part_ids_Delete_Part(
    IN p_ID INT,
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;
    DECLARE v_ItemNumber VARCHAR(100);
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while deleting part ID: ', p_ID);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- Check if part exists and get item number
    SELECT COUNT(*), MAX(ItemNumber) INTO v_Count, v_ItemNumber FROM md_part_ids WHERE ID = p_ID;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Part not found with ID: ', p_ID);
        ROLLBACK;
    ELSE
        DELETE FROM md_part_ids WHERE ID = p_ID;
        SET v_RowsAffected = ROW_COUNT();
        
        IF v_RowsAffected > 0 THEN
            SET p_Status = 0;
            SET p_ErrorMsg = CONCAT('Part deleted successfully: ', v_ItemNumber);
        ELSE
            SET p_Status = 2;
            SET p_ErrorMsg = CONCAT('Failed to delete part ID: ', p_ID);
        END IF;
        
        COMMIT;
    END IF;
END $$
DELIMITER ;

-- Delete part by item number with status reporting
DELIMITER $$
CREATE PROCEDURE md_part_ids_Delete_ByItemNumber(
    IN p_ItemNumber VARCHAR(100),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while deleting part: ', p_ItemNumber);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- Check if part exists
    SELECT COUNT(*) INTO v_Count FROM md_part_ids WHERE ItemNumber = p_ItemNumber;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Part not found: ', p_ItemNumber);
        ROLLBACK;
    ELSE
        DELETE FROM md_part_ids WHERE ItemNumber = p_ItemNumber;
        SET v_RowsAffected = ROW_COUNT();
        
        IF v_RowsAffected > 0 THEN
            SET p_Status = 0;
            SET p_ErrorMsg = CONCAT('Part deleted successfully: ', p_ItemNumber);
        ELSE
            SET p_Status = 2;
            SET p_ErrorMsg = CONCAT('Failed to delete part: ', p_ItemNumber);
        END IF;
        
        COMMIT;
    END IF;
END $$
DELIMITER ;

-- Get all parts with status reporting
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
        SET p_ErrorMsg = 'Database error occurred while retrieving all parts';
    END;
    
    SELECT COUNT(*) INTO v_Count FROM md_part_ids;
    SELECT * FROM md_part_ids ORDER BY ItemNumber;
    
    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('Retrieved ', v_Count, ' parts successfully');
END $$
DELIMITER ;

-- Get part by item number with status reporting
DELIMITER $$
CREATE PROCEDURE md_part_ids_Get_ByItemNumber(
    IN p_ItemNumber VARCHAR(100),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while retrieving part: ', p_ItemNumber);
    END;
    
    SELECT COUNT(*) INTO v_Count FROM md_part_ids WHERE ItemNumber = p_ItemNumber;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Part not found: ', p_ItemNumber);
        SELECT NULL as ID, NULL as ItemNumber; -- Return empty result set with structure
    ELSE
        SELECT * FROM md_part_ids WHERE ItemNumber = p_ItemNumber LIMIT 1;
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Part retrieved successfully: ', p_ItemNumber);
    END IF;
END $$
DELIMITER ;

-- Get item type by part ID with status reporting
DELIMITER $$
CREATE PROCEDURE md_part_ids_GetItemType_ByPartID(
    IN p_PartID VARCHAR(100),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while retrieving item type for part: ', p_PartID);
    END;
    
    SELECT COUNT(*) INTO v_Count FROM md_part_ids WHERE ItemNumber = p_PartID;
    
    IF v_Count = 0 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Part not found: ', p_PartID);
        SELECT NULL as ItemType;
    ELSE
        SELECT ItemType FROM md_part_ids WHERE ItemNumber = p_PartID LIMIT 1;
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Item type retrieved successfully for part: ', p_PartID);
    END IF;
END $$
DELIMITER ;

-- Check if part exists by item number with status reporting
DELIMITER $$
CREATE PROCEDURE md_part_ids_Exists_ByItemNumber(
    IN p_ItemNumber VARCHAR(100),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while checking part existence: ', p_ItemNumber);
    END;
    
    SELECT COUNT(*) INTO v_Count FROM md_part_ids WHERE ItemNumber = p_ItemNumber;
    SELECT v_Count as PartExists;
    
    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('Part existence check completed for: ', p_ItemNumber, ' (Exists: ', IF(v_Count > 0, 'Yes', 'No'), ')');
END $$
DELIMITER ;

-- ================================================================================
-- OPERATION MANAGEMENT PROCEDURES (Already Updated in Previous Edit)
-- ================================================================================

-- Add new operation with status reporting (Already defined above)
-- Update operation with status reporting (Already defined above)
-- Delete operation by operation name with status reporting (Already defined above)
-- Get all operations with status reporting (Already defined above)
-- Check if operation exists with status reporting (Already defined above)

-- ================================================================================
-- LOCATION MANAGEMENT PROCEDURES (MySQL 5.7.24 Compatible)
-- ================================================================================

-- Add new location with status reporting
DELIMITER $$
CREATE PROCEDURE md_locations_Add_Location(
    IN p_Location VARCHAR(100),
    IN p_Building VARCHAR(100),
    IN p_Description TEXT,
    IN p_IssuedBy VARCHAR(100),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while creating location: ', p_Location);
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
        INSERT INTO md_locations (Location, Building, Description, IssuedBy, CreatedDate)
        VALUES (p_Location, p_Building, p_Description, p_IssuedBy, NOW());
        
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Location created successfully: ', p_Location);
        COMMIT;
    END IF;
END $$
DELIMITER ;

-- Update location with status reporting
DELIMITER $$
CREATE PROCEDURE md_locations_Update_Location(
    IN p_Location VARCHAR(100),
    IN p_NewLocation VARCHAR(100),
    IN p_Building VARCHAR(100),
    IN p_Description TEXT,
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
        SET p_ErrorMsg = CONCAT('Database error occurred while updating location: ', p_Location);
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
        -- Check if new location name already exists (if different from current)
        IF p_Location != p_NewLocation THEN
            SELECT COUNT(*) INTO v_Count FROM md_locations WHERE Location = p_NewLocation;
            IF v_Count > 0 THEN
                SET p_Status = 1;
                SET p_ErrorMsg = CONCAT('New location name already exists: ', p_NewLocation);
                ROLLBACK;
            ELSE
                UPDATE md_locations 
                SET Location = p_NewLocation,
                    Building = p_Building,
                    Description = p_Description,
                    IssuedBy = p_IssuedBy,
                    ModifiedDate = NOW()
                WHERE Location = p_Location;
                
                SET v_RowsAffected = ROW_COUNT();
            END IF;
        ELSE
            UPDATE md_locations 
            SET Building = p_Building,
                Description = p_Description,
                IssuedBy = p_IssuedBy,
                ModifiedDate = NOW()
            WHERE Location = p_Location;
            
            SET v_RowsAffected = ROW_COUNT();
        END IF;
        
        IF v_RowsAffected > 0 THEN
            SET p_Status = 0;
            SET p_ErrorMsg = CONCAT('Location updated successfully from ', p_Location, ' to ', p_NewLocation);
        ELSE
            SET p_Status = 2;
            SET p_ErrorMsg = CONCAT('No changes made to location: ', p_Location);
        END IF;
        
        COMMIT;
    END IF;
END $$
DELIMITER ;

-- Delete location with status reporting
DELIMITER $$
CREATE PROCEDURE md_locations_Delete_Location(
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

-- Check if location exists with status reporting
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

-- ================================================================================
-- ITEM TYPE MANAGEMENT PROCEDURES (MySQL 5.7.24 Compatible)
-- ================================================================================

-- Add new item type with status reporting
DELIMITER $$
CREATE PROCEDURE md_item_types_Add_ItemType(
    IN p_ItemType VARCHAR(100),
    IN p_Description TEXT,
    IN p_IssuedBy VARCHAR(100),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while creating item type: ', p_ItemType);
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
        INSERT INTO md_item_types (ItemType, Description, IssuedBy, CreatedDate)
        VALUES (p_ItemType, p_Description, p_IssuedBy, NOW());
        
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Item type created successfully: ', p_ItemType);
        COMMIT;
    END IF;
END $$
DELIMITER ;

-- Update item type with status reporting
DELIMITER $$
CREATE PROCEDURE md_item_types_Update_ItemType(
    IN p_ItemType VARCHAR(100),
    IN p_NewItemType VARCHAR(100),
    IN p_Description TEXT,
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
        SET p_ErrorMsg = CONCAT('Database error occurred while updating item type: ', p_ItemType);
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
        -- Check if new item type name already exists (if different from current)
        IF p_ItemType != p_NewItemType THEN
            SELECT COUNT(*) INTO v_Count FROM md_item_types WHERE ItemType = p_NewItemType;
            IF v_Count > 0 THEN
                SET p_Status = 1;
                SET p_ErrorMsg = CONCAT('New item type name already exists: ', p_NewItemType);
                ROLLBACK;
            ELSE
                UPDATE md_item_types 
                SET ItemType = p_NewItemType,
                    Description = p_Description,
                    IssuedBy = p_IssuedBy,
                    ModifiedDate = NOW()
                WHERE ItemType = p_ItemType;
                
                SET v_RowsAffected = ROW_COUNT();
            END IF;
        ELSE
            UPDATE md_item_types 
            SET Description = p_Description,
                IssuedBy = p_IssuedBy,
                ModifiedDate = NOW()
            WHERE ItemType = p_ItemType;
            
            SET v_RowsAffected = ROW_COUNT();
        END IF;
        
        IF v_RowsAffected > 0 THEN
            SET p_Status = 0;
            SET p_ErrorMsg = CONCAT('Item type updated successfully from ', p_ItemType, ' to ', p_NewItemType);
        ELSE
            SET p_Status = 2;
            SET p_ErrorMsg = CONCAT('No changes made to item type: ', p_ItemType);
        END IF;
        
        COMMIT;
    END IF;
END $$
DELIMITER ;

-- Delete item type with status reporting
DELIMITER $$
CREATE PROCEDURE md_item_types_Delete_ItemType(
    IN p_ItemType VARCHAR(100),
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

-- Get distinct item types (for dropdown lists) with status reporting
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

-- Check if item type exists with status reporting
DELIMITER $$
CREATE PROCEDURE md_item_types_Exists_ByItemType(
    IN p_ItemType VARCHAR(100),
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

-- ================================================================================
-- END OF MASTER DATA PROCEDURES
-- ================================================================================
