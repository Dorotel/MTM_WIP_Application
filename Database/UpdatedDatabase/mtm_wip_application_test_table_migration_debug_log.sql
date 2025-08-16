
-- --------------------------------------------------------

--
-- Table structure for table `migration_debug_log`
--

CREATE TABLE `migration_debug_log` (
  `id` int(11) NOT NULL,
  `message` varchar(255) DEFAULT NULL,
  `created_at` datetime DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Truncate table before insert `migration_debug_log`
--

TRUNCATE TABLE `migration_debug_log`;
--
-- Dumping data for table `migration_debug_log`
--

INSERT INTO `migration_debug_log` (`id`, `message`, `created_at`) VALUES
(1, 'Processing user: [ All Users ]', '2025-07-15 09:33:51'),
(2, 'Assigned role: User', '2025-07-15 09:33:51'),
(3, 'User-role already exists for: [ All Users ] | Role: User', '2025-07-15 09:33:51'),
(4, 'Processing user: DLAFOND', '2025-07-15 09:33:51'),
(5, 'Assigned role: Admin', '2025-07-15 09:33:51'),
(6, 'Migrated user: DLAFOND with role: Admin', '2025-07-15 09:33:51');
