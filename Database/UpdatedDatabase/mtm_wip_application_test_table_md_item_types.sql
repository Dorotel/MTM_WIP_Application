
-- --------------------------------------------------------

--
-- Table structure for table `md_item_types`
--

CREATE TABLE `md_item_types` (
  `ID` int(11) NOT NULL,
  `ItemType` varchar(100) NOT NULL,
  `IssuedBy` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Truncate table before insert `md_item_types`
--

TRUNCATE TABLE `md_item_types`;
--
-- Dumping data for table `md_item_types`
--

INSERT INTO `md_item_types` (`ID`, `ItemType`, `IssuedBy`) VALUES
(2, 'WIP', 'JOHNK'),
(3, 'Dunnage', '[ System ]'),
(4, 'Outside Service', '[ System ]'),
(5, 'Other', '[ System ]'),
(6, 'A0006876776', 'DSANCHEZ');
