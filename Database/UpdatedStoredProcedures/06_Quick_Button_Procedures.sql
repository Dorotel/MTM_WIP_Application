-- ================================================================================
-- MTM INVENTORY APPLICATION - QUICK BUTTON STORED PROCEDURES
-- ================================================================================
-- File: 06_Quick_Button_Procedures.sql
-- Purpose: User quick button management for rapid transaction access
-- Created: August 10, 2025
-- Updated: August 10, 2025 - UNIFORM PARAMETER NAMING (WITH p_ prefixes) - FIXED COLUMN NAMES
-- Target Database: mtm_wip_application
-- MySQL Version: 5.7.24+ (MAMP Compatible)
-- ================================================================================

-- Drop procedures if they exist (for clean deployment)
DROP PROCEDURE IF EXISTS sys_last_10_transactions_Get_ByUser_1;
DROP PROCEDURE IF EXISTS sys_last_10_transactions_Update_ByUserAndPosition_1;
DROP PROCEDURE IF EXISTS sys_last_10_transactions_RemoveAndShift_ByUser_1;
DROP PROCEDURE IF EXISTS sys_last_10_transactions_Add_AtPosition_1;
DROP PROCEDURE IF EXISTS sys_last_10_transactions_Move_1;
DROP PROCEDURE IF EXISTS sys_last_10_transactions_DeleteAll_ByUser;
DROP PROCEDURE IF EXISTS sys_last_10_transactions_AddOrShift_ByUser;

-- ================================================================================
-- QUICK BUTTON MANAGEMENT PROCEDURES
-- ================================================================================

-- Get user's last 10 transactions with status reporting
DELIMITER $$
CREATE PROCEDURE sys_last_10_transactions_Get_ByUser_1(
    IN p_User VARCHAR(100),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
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
        ReceiveDate  -- FIXED: Changed from DateTime to ReceiveDate to match actual table structure
    FROM sys_last_10_transactions 
    WHERE User = p_User 
    ORDER BY Position;
    
    SET p_Status = 0;
    SET p_ErrorMsg = CONCAT('Retrieved ', v_Count, ' quick buttons for user: ', p_User);
END $$
DELIMITER ;

-- Update quick button at specific position
DELIMITER $$
CREATE PROCEDURE sys_last_10_transactions_Update_ByUserAndPosition_1(
    IN p_User VARCHAR(100),
    IN p_Position INT,
    IN p_PartID VARCHAR(300),
    IN p_Operation VARCHAR(50),
    IN p_Quantity INT,
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while updating quick button at position ', p_Position, ' for user: ', p_User);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- Validate position range
    IF p_Position < 1 OR p_Position > 10 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Invalid position: ', p_Position, '. Position must be between 1 and 10.');
        ROLLBACK;
    ELSE
        -- Check if position exists for user
        SELECT COUNT(*) INTO v_Count FROM sys_last_10_transactions WHERE User = p_User AND Position = p_Position;
        
        IF v_Count = 0 THEN
            -- Insert new record - FIXED: Changed DateTime to ReceiveDate
            INSERT INTO sys_last_10_transactions (User, Position, PartID, Operation, Quantity, ReceiveDate)
            VALUES (p_User, p_Position, p_PartID, p_Operation, p_Quantity, NOW());
            SET p_Status = 0;
            SET p_ErrorMsg = CONCAT('Quick button created at position ', p_Position, ' for user: ', p_User);
        ELSE
            -- Update existing record - FIXED: Changed DateTime to ReceiveDate
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
END $$
DELIMITER ;

-- Remove quick button and shift remaining positions up
DELIMITER $$
CREATE PROCEDURE sys_last_10_transactions_RemoveAndShift_ByUser_1(
    IN p_User VARCHAR(100),
    IN p_Position INT,
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    DECLARE v_RowsAffected INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while removing quick button at position ', p_Position, ' for user: ', p_User);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- Validate position range
    IF p_Position < 1 OR p_Position > 10 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Invalid position: ', p_Position, '. Position must be between 1 and 10.');
        ROLLBACK;
    ELSE
        -- Check if position exists for user
        SELECT COUNT(*) INTO v_Count FROM sys_last_10_transactions WHERE User = p_User AND Position = p_Position;
        
        IF v_Count = 0 THEN
            SET p_Status = 1;
            SET p_ErrorMsg = CONCAT('No quick button found at position ', p_Position, ' for user: ', p_User);
            ROLLBACK;
        ELSE
            -- Delete the record at specified position
            DELETE FROM sys_last_10_transactions WHERE User = p_User AND Position = p_Position;
            SET v_RowsAffected = ROW_COUNT();
            
            -- Shift remaining positions up
            UPDATE sys_last_10_transactions 
            SET Position = Position - 1 
            WHERE User = p_User AND Position > p_Position;
            
            SET p_Status = 0;
            SET p_ErrorMsg = CONCAT('Quick button removed from position ', p_Position, ' and remaining positions shifted up for user: ', p_User);
            COMMIT;
        END IF;
    END IF;
END $$
DELIMITER ;

-- Add quick button at specific position (shifts existing positions down)
DELIMITER $$
CREATE PROCEDURE sys_last_10_transactions_Add_AtPosition_1(
    IN p_User VARCHAR(100),
    IN p_Position INT,
    IN p_PartID VARCHAR(300),
    IN p_Operation VARCHAR(50),
    IN p_Quantity INT,
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while adding quick button at position ', p_Position, ' for user: ', p_User);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- Validate position range
    IF p_Position < 1 OR p_Position > 10 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Invalid position: ', p_Position, '. Position must be between 1 and 10.');
        ROLLBACK;
    ELSE
        -- Shift existing positions down to make room
        UPDATE sys_last_10_transactions 
        SET Position = Position + 1 
        WHERE User = p_User AND Position >= p_Position AND Position < 10;
        
        -- Remove position 10 if it exists (bumped out)
        DELETE FROM sys_last_10_transactions WHERE User = p_User AND Position = 10;
        
        -- Insert new record at specified position - FIXED: Changed DateTime to ReceiveDate
        INSERT INTO sys_last_10_transactions (User, Position, PartID, Operation, Quantity, ReceiveDate)
        VALUES (p_User, p_Position, p_PartID, p_Operation, p_Quantity, NOW());
        
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Quick button added at position ', p_Position, ' for user: ', p_User);
        COMMIT;
    END IF;
END $$
DELIMITER ;

-- Move quick button from one position to another
DELIMITER $$
CREATE PROCEDURE sys_last_10_transactions_Move_1(
    IN p_User VARCHAR(100),
    IN p_FromPosition INT,
    IN p_ToPosition INT,
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
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
    
    -- Validate position ranges
    IF p_FromPosition < 1 OR p_FromPosition > 10 OR p_ToPosition < 1 OR p_ToPosition > 10 THEN
        SET p_Status = 1;
        SET p_ErrorMsg = CONCAT('Invalid positions. From: ', p_FromPosition, ', To: ', p_ToPosition, '. Positions must be between 1 and 10.');
        ROLLBACK;
    ELSEIF p_FromPosition = p_ToPosition THEN
        SET p_Status = 1;
        SET p_ErrorMsg = 'Source and destination positions cannot be the same.';
        ROLLBACK;
    ELSE
        -- Check if source position exists
        SELECT COUNT(*), PartID, Operation, Quantity 
        INTO v_Count, v_PartID, v_Operation, v_Quantity
        FROM sys_last_10_transactions 
        WHERE User = p_User AND Position = p_FromPosition;
        
        IF v_Count = 0 THEN
            SET p_Status = 1;
            SET p_ErrorMsg = CONCAT('No quick button found at position ', p_FromPosition, ' for user: ', p_User);
            ROLLBACK;
        ELSE
            -- Delete source position
            DELETE FROM sys_last_10_transactions WHERE User = p_User AND Position = p_FromPosition;
            
            -- Shift positions based on move direction
            IF p_ToPosition < p_FromPosition THEN
                -- Moving up: shift positions down
                UPDATE sys_last_10_transactions 
                SET Position = Position + 1 
                WHERE User = p_User AND Position >= p_ToPosition AND Position < p_FromPosition;
            ELSE
                -- Moving down: shift positions up
                UPDATE sys_last_10_transactions 
                SET Position = Position - 1 
                WHERE User = p_User AND Position > p_FromPosition AND Position <= p_ToPosition;
            END IF;
            
            -- Insert at new position - FIXED: Changed DateTime to ReceiveDate
            INSERT INTO sys_last_10_transactions (User, Position, PartID, Operation, Quantity, ReceiveDate)
            VALUES (p_User, p_ToPosition, v_PartID, v_Operation, v_Quantity, NOW());
            
            SET p_Status = 0;
            SET p_ErrorMsg = CONCAT('Quick button moved from position ', p_FromPosition, ' to ', p_ToPosition, ' for user: ', p_User);
            COMMIT;
        END IF;
    END IF;
END $$
DELIMITER ;

-- Delete all quick buttons for user
DELIMITER $$
CREATE PROCEDURE sys_last_10_transactions_DeleteAll_ByUser(
    IN p_User VARCHAR(100),
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
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
END $$
DELIMITER ;

-- Add or shift quick button (smart positioning)
DELIMITER $$
CREATE PROCEDURE sys_last_10_transactions_AddOrShift_ByUser(
    IN p_User VARCHAR(100),
    IN p_PartID VARCHAR(300),
    IN p_Operation VARCHAR(50),
    IN p_Quantity INT,
    OUT p_Status INT,
    OUT p_ErrorMsg VARCHAR(255)
)
BEGIN
    DECLARE v_ExistingPosition INT DEFAULT 0;
    DECLARE v_Count INT DEFAULT 0;
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Status = -1;
        SET p_ErrorMsg = CONCAT('Database error occurred while adding/shifting quick button for user: ', p_User);
        ROLLBACK;
    END;
    
    START TRANSACTION;
    
    -- FIXED: Check if PartID and Operation combination already exists (ignore Quantity)
    -- Only PartID and Operation matter for uniqueness, Quantity is not a factor
    SELECT Position INTO v_ExistingPosition
    FROM sys_last_10_transactions 
    WHERE User = p_User AND PartID = p_PartID AND Operation = p_Operation
    LIMIT 1;
    
    IF v_ExistingPosition > 0 THEN
        -- Update existing button with new quantity and move to position 1
        UPDATE sys_last_10_transactions 
        SET Quantity = p_Quantity, ReceiveDate = NOW()
        WHERE User = p_User AND Position = v_ExistingPosition;
        
        -- Move existing to position 1 (if not already there)
        IF v_ExistingPosition != 1 THEN
            CALL sys_last_10_transactions_Move_1(p_User, v_ExistingPosition, 1, @move_status, @move_msg);
        END IF;
        
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Updated existing quick button quantity and moved to position 1 for user: ', p_User);
    ELSE
        -- Add new at position 1 (shifts others down)
        CALL sys_last_10_transactions_Add_AtPosition_1(p_User, 1, p_PartID, p_Operation, p_Quantity, @add_status, @add_msg);
        SET p_Status = 0;
        SET p_ErrorMsg = CONCAT('Added new quick button at position 1 for user: ', p_User);
    END IF;
    
    COMMIT;
END $$
DELIMITER ;

-- ================================================================================
-- END OF QUICK BUTTON PROCEDURES
-- ================================================================================
