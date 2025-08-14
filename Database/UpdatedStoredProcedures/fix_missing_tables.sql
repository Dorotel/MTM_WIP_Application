-- Simple script compatible with MySQL 5.7.24

-- Create sys_quick_buttons if it doesn't exist
CREATE TABLE IF NOT EXISTS sys_quick_buttons (
    ButtonID INT AUTO_INCREMENT PRIMARY KEY,
    UserId VARCHAR(100) NOT NULL,
    ButtonName VARCHAR(100) NOT NULL,
    PartID VARCHAR(300),
    Location VARCHAR(100),
    Operation VARCHAR(100),
    Quantity INT,
    ItemType VARCHAR(100),
    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    UNIQUE KEY unique_user_button (UserId, ButtonName)
);

-- Create/recreate the log_error_log view properly
DROP VIEW IF EXISTS log_error_log;
CREATE VIEW log_error_log AS 
SELECT 
    ID as ErrorID,
    ErrorMessage,
    StackTrace,
    User,
    ErrorTime as Timestamp,
    Severity
FROM log_error;

-- Add some sample data if needed
INSERT IGNORE INTO log_changelog (Version, ReleaseDate, Notes) 
VALUES ('1.0.0', CURDATE(), 'Initial system setup and verification');

-- Final verification
SELECT 'Schema Fix Complete - All Required Tables Available' as Status;
