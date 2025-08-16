
-- --------------------------------------------------------

--
-- Table structure for table `md_operation_numbers`
--

CREATE TABLE `md_operation_numbers` (
  `ID` int(11) NOT NULL,
  `Operation` varchar(100) NOT NULL,
  `IssuedBy` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Truncate table before insert `md_operation_numbers`
--

TRUNCATE TABLE `md_operation_numbers`;
--
-- Dumping data for table `md_operation_numbers`
--

INSERT INTO `md_operation_numbers` (`ID`, `Operation`, `IssuedBy`) VALUES
(79, '10', '[ System ]'),
(80, '100', '[ System ]'),
(81, '106', '[ System ]'),
(82, '109', '[ System ]'),
(83, '110', '[ System ]'),
(84, '119', '[ System ]'),
(85, '12', '[ System ]'),
(86, '120', '[ System ]'),
(87, '15', '[ System ]'),
(88, '18', 'MVOGEL'),
(89, '19', '[ System ]'),
(90, '20', '[ System ]'),
(91, '21', '[ System ]'),
(92, '220', '[ System ]'),
(93, '25', '[ System ]'),
(94, '252', 'JKOLL'),
(95, '252-01', 'KSKATTEBO'),
(96, '29', '[ System ]'),
(97, '30', '[ System ]'),
(98, '300', '[ System ]'),
(99, '309', '[ System ]'),
(100, '31', '[ System ]'),
(101, '310', '[ System ]'),
(102, '320', '[ System ]'),
(103, '35', 'RECEIVING'),
(104, '350', '[ System ]'),
(105, '359', '[ System ]'),
(106, '39', '[ System ]'),
(107, '40', '[ System ]'),
(108, '400', '[ System ]'),
(109, '409', '[ System ]'),
(110, '45', 'DHAMMONS'),
(111, '49', '[ System ]'),
(112, '50', '[ System ]'),
(113, '500', '[ System ]'),
(114, '509', '[ System ]'),
(115, '59', '[ System ]'),
(116, '6', '[ System ]'),
(117, '60', '[ System ]'),
(118, '600', '[ System ]'),
(119, '69', '[ System ]'),
(120, '7', '[ System ]'),
(121, '70', 'DHAMMONS'),
(122, '700', '[ System ]'),
(123, '8', '[ System ]'),
(124, '80', '[ System ]'),
(125, '800', '[ System ]'),
(126, '880', 'JKOLL'),
(127, '889', '[ System ]'),
(128, '89', '[ System ]'),
(129, '890', '[ System ]'),
(130, '899', '[ System ]'),
(131, '90', '[ System ]'),
(132, '900', '[ System ]'),
(133, '905', 'MLAURIN'),
(134, '909', '[ System ]'),
(135, '91', '[ System ]'),
(136, '910', '[ System ]'),
(137, '915', '[ System ]'),
(138, '917', '[ System ]'),
(139, '919', '[ System ]'),
(140, '920', '[ System ]'),
(141, '929', 'JKOLL'),
(142, '939', 'JKOLL'),
(143, '949', 'JKOLL'),
(144, '95', '[ System ]'),
(145, '950', '[ System ]'),
(146, '959', '[ System ]'),
(147, '96', '[ System ]'),
(148, '97', 'JMAUER'),
(149, '99', '[ System ]'),
(150, 'N/A', 'JKOLL');
