
-- --------------------------------------------------------

--
-- Table structure for table `sys_roles`
--

CREATE TABLE `sys_roles` (
  `ID` int(11) NOT NULL,
  `RoleName` varchar(50) NOT NULL,
  `Description` varchar(255) DEFAULT NULL,
  `Permissions` varchar(1000) DEFAULT NULL,
  `IsSystem` tinyint(1) NOT NULL DEFAULT '0' COMMENT 'True for built-in roles',
  `CreatedBy` varchar(100) NOT NULL,
  `CreatedAt` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Truncate table before insert `sys_roles`
--

TRUNCATE TABLE `sys_roles`;
--
-- Dumping data for table `sys_roles`
--

INSERT INTO `sys_roles` (`ID`, `RoleName`, `Description`, `Permissions`, `IsSystem`, `CreatedBy`, `CreatedAt`) VALUES
(1, 'Admin', 'Administrative access with all permissions', NULL, 1, '[ System ]', '2025-06-01 21:49:08'),
(2, 'ReadOnly', 'Read-only access to data', NULL, 1, '[ System ]', '2025-06-01 21:49:08'),
(3, 'User', 'Standard user access', NULL, 1, '[ System ]', '2025-06-01 21:49:08');
