
-- --------------------------------------------------------

--
-- Table structure for table `sys_user_roles`
--

CREATE TABLE `sys_user_roles` (
  `UserID` int(11) NOT NULL,
  `RoleID` int(11) NOT NULL,
  `AssignedBy` varchar(100) NOT NULL,
  `AssignedAt` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Truncate table before insert `sys_user_roles`
--

TRUNCATE TABLE `sys_user_roles`;
--
-- Dumping data for table `sys_user_roles`
--

INSERT INTO `sys_user_roles` (`UserID`, `RoleID`, `AssignedBy`, `AssignedAt`) VALUES
(1, 3, 'migration-script', '2025-07-15 16:22:38'),
(2, 1, 'migration-script', '2025-07-15 16:22:38'),
(3, 1, 'migration-script', '2025-07-15 16:22:38'),
(4, 1, 'migration-script', '2025-07-15 16:22:38'),
(5, 3, 'migration-script', '2025-07-15 16:22:38'),
(6, 3, 'migration-script', '2025-07-15 16:22:38'),
(7, 1, 'migration-script', '2025-07-15 16:22:38'),
(8, 1, 'migration-script', '2025-07-15 16:22:38'),
(9, 1, 'migration-script', '2025-07-15 16:22:38'),
(10, 3, 'migration-script', '2025-07-15 16:22:38'),
(11, 1, 'migration-script', '2025-07-15 16:22:38'),
(12, 1, 'migration-script', '2025-07-15 16:22:38'),
(13, 3, 'migration-script', '2025-07-15 16:22:38'),
(14, 1, 'migration-script', '2025-07-15 16:22:38'),
(15, 1, 'migration-script', '2025-07-15 16:22:38'),
(16, 3, 'migration-script', '2025-07-15 16:22:38'),
(17, 1, 'migration-script', '2025-07-15 16:22:38'),
(18, 1, 'migration-script', '2025-07-15 16:22:38'),
(19, 3, 'migration-script', '2025-07-15 16:22:38'),
(20, 1, 'migration-script', '2025-07-15 16:22:38'),
(21, 3, 'migration-script', '2025-07-15 16:22:38'),
(22, 1, 'migration-script', '2025-07-15 16:22:38'),
(23, 3, 'migration-script', '2025-07-15 16:22:38'),
(24, 1, 'migration-script', '2025-07-15 16:22:38'),
(25, 1, 'migration-script', '2025-07-15 16:22:38'),
(26, 3, 'migration-script', '2025-07-15 16:22:38'),
(27, 3, 'migration-script', '2025-07-15 16:22:38'),
(28, 1, 'migration-script', '2025-07-15 16:22:38'),
(29, 1, 'migration-script', '2025-07-15 16:22:38'),
(30, 3, 'migration-script', '2025-07-15 16:22:38'),
(31, 1, 'migration-script', '2025-07-15 16:22:38'),
(32, 3, 'migration-script', '2025-07-15 16:22:38'),
(33, 3, 'migration-script', '2025-07-15 16:22:38'),
(34, 1, 'migration-script', '2025-07-15 16:22:38'),
(35, 3, 'migration-script', '2025-07-15 16:22:38'),
(36, 3, 'migration-script', '2025-07-15 16:22:38'),
(37, 3, 'migration-script', '2025-07-15 16:22:38'),
(38, 1, 'migration-script', '2025-07-15 16:22:38'),
(39, 3, 'migration-script', '2025-07-15 16:22:38'),
(40, 1, 'migration-script', '2025-07-15 16:22:38'),
(41, 1, 'migration-script', '2025-07-15 16:22:38'),
(42, 1, 'migration-script', '2025-07-15 16:22:38'),
(43, 3, 'migration-script', '2025-07-15 16:22:38'),
(44, 1, 'migration-script', '2025-07-15 16:22:38'),
(45, 1, 'migration-script', '2025-07-15 16:22:38'),
(46, 1, 'migration-script', '2025-07-15 16:22:38'),
(47, 3, 'migration-script', '2025-07-15 16:22:38'),
(48, 3, 'migration-script', '2025-07-15 16:22:38'),
(49, 1, 'migration-script', '2025-07-15 16:22:38'),
(50, 1, 'migration-script', '2025-07-15 16:22:38'),
(51, 1, 'migration-script', '2025-07-15 16:22:38'),
(52, 1, 'migration-script', '2025-07-15 16:22:38'),
(53, 3, 'migration-script', '2025-07-15 16:22:38'),
(54, 3, 'migration-script', '2025-07-15 16:22:38'),
(55, 1, 'migration-script', '2025-07-15 16:22:38'),
(56, 1, 'migration-script', '2025-07-15 16:22:38'),
(57, 1, 'migration-script', '2025-07-15 16:22:38'),
(58, 1, 'migration-script', '2025-07-15 16:22:38'),
(59, 1, 'migration-script', '2025-07-15 16:22:38'),
(60, 1, 'migration-script', '2025-07-15 16:22:38'),
(61, 1, 'migration-script', '2025-07-15 16:22:38'),
(62, 1, 'migration-script', '2025-07-15 16:22:38'),
(63, 1, 'migration-script', '2025-07-15 16:22:38'),
(64, 1, 'migration-script', '2025-07-15 16:22:38'),
(65, 3, 'migration-script', '2025-07-15 16:22:38'),
(66, 3, 'migration-script', '2025-07-15 16:22:38'),
(67, 1, 'johnk', '2025-08-06 08:15:39'),
(68, 1, 'migration-script', '2025-07-15 16:22:38'),
(69, 2, 'migration-script', '2025-07-15 16:22:38'),
(70, 3, 'migration-script', '2025-07-15 16:22:38'),
(71, 3, 'migration-script', '2025-07-15 16:22:38'),
(72, 3, 'migration-script', '2025-07-15 16:22:38'),
(73, 3, 'migration-script', '2025-07-15 16:22:38'),
(74, 3, 'migration-script', '2025-07-15 16:22:38'),
(75, 1, 'migration-script', '2025-07-15 16:22:38'),
(76, 3, 'migration-script', '2025-07-15 16:22:38'),
(77, 3, 'migration-script', '2025-07-15 16:22:38'),
(78, 3, 'migration-script', '2025-07-15 16:22:38'),
(149, 1, 'migration-script', '2025-07-15 16:22:38'),
(179, 3, 'migration-script', '2025-07-15 16:22:38'),
(310, 3, 'jkoll', '2025-07-27 22:53:38');
