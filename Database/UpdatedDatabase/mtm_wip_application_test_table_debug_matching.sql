
-- --------------------------------------------------------

--
-- Table structure for table `debug_matching`
--

CREATE TABLE `debug_matching` (
  `id` int(11) NOT NULL,
  `in_id` int(11) DEFAULT NULL,
  `in_part` varchar(100) DEFAULT NULL,
  `in_loc` varchar(100) DEFAULT NULL,
  `in_batch` varchar(100) DEFAULT NULL,
  `out_id` int(11) DEFAULT NULL,
  `out_part` varchar(100) DEFAULT NULL,
  `out_loc` varchar(100) DEFAULT NULL,
  `out_batch` varchar(100) DEFAULT NULL,
  `matched_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Truncate table before insert `debug_matching`
--

TRUNCATE TABLE `debug_matching`;