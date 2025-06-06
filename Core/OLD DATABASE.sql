-- phpMyAdmin SQL Dump
-- version 5.2.2
-- https://www.phpmyadmin.net/
--
-- Host: localhost:3306
-- Generation Time: Jun 02, 2025 at 01:49 PM
-- Server version: 5.7.24
-- PHP Version: 8.3.1

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `mtm database`
--

-- --------------------------------------------------------

--
-- Table structure for table `input_history`
--

CREATE TABLE `input_history` (
  `ID` int(11) NOT NULL,
  `User` varchar(100) NOT NULL,
  `Part ID` varchar(300) NOT NULL,
  `Location` varchar(100) NOT NULL,
  `Type` varchar(1000) NOT NULL,
  `Quantity` int(100) NOT NULL,
  `Time` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `input_history_backup`
--

CREATE TABLE `input_history_backup` (
  `ID` int(11) NOT NULL,
  `User` varchar(100) NOT NULL,
  `Part ID` varchar(300) NOT NULL,
  `Location` varchar(100) NOT NULL,
  `Type` varchar(1000) NOT NULL,
  `Quantity` int(100) NOT NULL,
  `Time` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `inventory_balance`
--

CREATE TABLE `inventory_balance` (
  `ID` int(11) NOT NULL,
  `Part ID` varchar(300) NOT NULL,
  `Location` varchar(100) NOT NULL,
  `Op` varchar(100) DEFAULT NULL,
  `Quantity` int(11) NOT NULL DEFAULT '0',
  `Type` varchar(100) NOT NULL DEFAULT 'WIP',
  `Last_Updated` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `inventory_transactions`
--

CREATE TABLE `inventory_transactions` (
  `transaction_id` int(11) NOT NULL,
  `transaction_type` enum('IN','OUT','TRANSFER','ADJUST') NOT NULL,
  `item_number` varchar(300) NOT NULL,
  `operation` varchar(100) DEFAULT NULL,
  `location_from` varchar(100) DEFAULT NULL,
  `location_to` varchar(100) NOT NULL,
  `quantity` int(11) NOT NULL,
  `notes` varchar(1000) DEFAULT NULL,
  `user_id` varchar(100) NOT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `items`
--

CREATE TABLE `items` (
  `item_id` int(11) NOT NULL,
  `item_number` varchar(100) NOT NULL,
  `description` varchar(255) DEFAULT NULL,
  `item_type` varchar(50) NOT NULL DEFAULT 'WIP',
  `created_by` varchar(100) NOT NULL,
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `item_types`
--

CREATE TABLE `item_types` (
  `Type` varchar(200) NOT NULL,
  `ID` int(11) NOT NULL,
  `Issued By` varchar(200) NOT NULL DEFAULT '[ System ]',
  `Date` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `last_10_transactions`
--

CREATE TABLE `last_10_transactions` (
  `ID` int(11) DEFAULT NULL,
  `PartID` varchar(1000) NOT NULL,
  `Op` text NOT NULL,
  `Quantity` int(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `leads`
--

CREATE TABLE `leads` (
  `ID` int(11) NOT NULL,
  `USER` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `locations`
--

CREATE TABLE `locations` (
  `Location` varchar(50) DEFAULT NULL,
  `ID` int(11) NOT NULL,
  `Issued By` varchar(1000) NOT NULL DEFAULT '[ System ]'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `operations`
--

CREATE TABLE `operations` (
  `operation_id` int(11) NOT NULL,
  `operation_code` varchar(100) NOT NULL,
  `created_at` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `created_by` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `operation_numbers`
--

CREATE TABLE `operation_numbers` (
  `Operation` varchar(100) NOT NULL,
  `ID` int(11) NOT NULL,
  `Issued By` varchar(1000) NOT NULL DEFAULT '[ System ]'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `output_history`
--

CREATE TABLE `output_history` (
  `ID` int(11) NOT NULL,
  `User` varchar(100) NOT NULL,
  `Part ID` varchar(300) NOT NULL,
  `Location` varchar(100) NOT NULL,
  `Quantity` int(100) NOT NULL,
  `Time` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `part_ids`
--

CREATE TABLE `part_ids` (
  `Item Number` varchar(200) DEFAULT NULL,
  `ID` int(11) NOT NULL,
  `Issued By` varchar(100) NOT NULL DEFAULT '[ System ]',
  `Type` varchar(200) NOT NULL DEFAULT 'WIP'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `part_requirement`
--

CREATE TABLE `part_requirement` (
  `ID` varchar(100) DEFAULT NULL,
  `DESCRIPTION` varchar(10000) DEFAULT NULL,
  `SEQUENCE_NO` int(100) DEFAULT NULL,
  `PART_ID` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `program_information`
--

CREATE TABLE `program_information` (
  `ID` int(11) NOT NULL,
  `Version` varchar(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `readonly`
--

CREATE TABLE `readonly` (
  `ID` int(100) NOT NULL,
  `USER` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `remove_history`
--

CREATE TABLE `remove_history` (
  `ID` int(11) NOT NULL,
  `User` varchar(100) NOT NULL,
  `Part ID` varchar(300) NOT NULL,
  `Location` varchar(100) NOT NULL,
  `Type` varchar(1000) NOT NULL,
  `Quantity` int(100) NOT NULL,
  `Time` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `saved_locations`
--

CREATE TABLE `saved_locations` (
  `ID` int(100) NOT NULL,
  `Location` varchar(100) NOT NULL,
  `Item Number` varchar(300) NOT NULL,
  `Op` varchar(100) NOT NULL DEFAULT 'None',
  `Notes` varchar(1000) NOT NULL DEFAULT '',
  `Quantity` int(100) NOT NULL,
  `Date_Time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `User` varchar(100) NOT NULL,
  `Item Type` varchar(200) NOT NULL DEFAULT 'WIP'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `saved_variables`
--

CREATE TABLE `saved_variables` (
  `Variable_Name` varchar(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `transfer_history`
--

CREATE TABLE `transfer_history` (
  `Id` int(11) NOT NULL,
  `OldLocation` varchar(100) NOT NULL,
  `NewLocation` varchar(100) NOT NULL,
  `PartId` varchar(300) NOT NULL,
  `Quantity` int(100) NOT NULL,
  `Time` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `User` varchar(100) NOT NULL,
  `PartType` varchar(1000) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `ID` int(100) NOT NULL,
  `User` varchar(100) NOT NULL,
  `Full Name` varchar(100) NOT NULL,
  `Shift` varchar(100) NOT NULL,
  `HideChangeLog` varchar(50) NOT NULL DEFAULT 'false',
  `LastShownVersion` varchar(50) NOT NULL DEFAULT '0.0.0.0',
  `VisualUserName` varchar(50) NOT NULL DEFAULT 'User Name',
  `VisualPassword` varchar(50) NOT NULL DEFAULT 'Password',
  `Theme_Name` varchar(50) NOT NULL DEFAULT 'Default (Black and White)',
  `Theme_FontSize` int(11) NOT NULL DEFAULT '9',
  `Pin` varchar(4) NOT NULL DEFAULT '0000',
  `VitsUser` tinyint(1) NOT NULL DEFAULT '0',
  `WipServerAddress` varchar(12) NOT NULL DEFAULT '172.16.1.104',
  `WipServerPort` varchar(6) NOT NULL DEFAULT '3306'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `wipapp_errorlog`
--

CREATE TABLE `wipapp_errorlog` (
  `ID` int(11) NOT NULL,
  `Method` text NOT NULL,
  `Error` text NOT NULL,
  `User` text NOT NULL,
  `DateTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Control` varchar(128) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `wipapp_version_history`
--

CREATE TABLE `wipapp_version_history` (
  `ID` int(11) NOT NULL,
  `Version` varchar(50) NOT NULL,
  `Notes` longtext NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `input_history`
--
ALTER TABLE `input_history`
  ADD PRIMARY KEY (`ID`);

--
-- Indexes for table `input_history_backup`
--
ALTER TABLE `input_history_backup`
  ADD PRIMARY KEY (`ID`);

--
-- Indexes for table `inventory_balance`
--
ALTER TABLE `inventory_balance`
  ADD PRIMARY KEY (`ID`),
  ADD UNIQUE KEY `idx_part_location_op` (`Part ID`,`Location`,`Op`),
  ADD KEY `idx_part_id` (`Part ID`),
  ADD KEY `idx_location` (`Location`);

--
-- Indexes for table `inventory_transactions`
--
ALTER TABLE `inventory_transactions`
  ADD PRIMARY KEY (`transaction_id`),
  ADD KEY `idx_item_number` (`item_number`),
  ADD KEY `idx_location_to` (`location_to`),
  ADD KEY `idx_timestamp` (`timestamp`);

--
-- Indexes for table `items`
--
ALTER TABLE `items`
  ADD PRIMARY KEY (`item_id`),
  ADD UNIQUE KEY `idx_item_number` (`item_number`);

--
-- Indexes for table `item_types`
--
ALTER TABLE `item_types`
  ADD PRIMARY KEY (`ID`);

--
-- Indexes for table `leads`
--
ALTER TABLE `leads`
  ADD PRIMARY KEY (`ID`);

--
-- Indexes for table `locations`
--
ALTER TABLE `locations`
  ADD PRIMARY KEY (`ID`);

--
-- Indexes for table `operations`
--
ALTER TABLE `operations`
  ADD PRIMARY KEY (`operation_id`),
  ADD UNIQUE KEY `idx_operation_code` (`operation_code`);

--
-- Indexes for table `operation_numbers`
--
ALTER TABLE `operation_numbers`
  ADD PRIMARY KEY (`ID`);

--
-- Indexes for table `output_history`
--
ALTER TABLE `output_history`
  ADD PRIMARY KEY (`ID`);

--
-- Indexes for table `part_ids`
--
ALTER TABLE `part_ids`
  ADD PRIMARY KEY (`ID`);

--
-- Indexes for table `program_information`
--
ALTER TABLE `program_information`
  ADD PRIMARY KEY (`ID`);

--
-- Indexes for table `readonly`
--
ALTER TABLE `readonly`
  ADD PRIMARY KEY (`ID`);

--
-- Indexes for table `remove_history`
--
ALTER TABLE `remove_history`
  ADD PRIMARY KEY (`ID`);

--
-- Indexes for table `saved_locations`
--
ALTER TABLE `saved_locations`
  ADD PRIMARY KEY (`ID`);

--
-- Indexes for table `saved_variables`
--
ALTER TABLE `saved_variables`
  ADD PRIMARY KEY (`Variable_Name`);

--
-- Indexes for table `transfer_history`
--
ALTER TABLE `transfer_history`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`ID`);

--
-- Indexes for table `wipapp_errorlog`
--
ALTER TABLE `wipapp_errorlog`
  ADD PRIMARY KEY (`ID`);

--
-- Indexes for table `wipapp_version_history`
--
ALTER TABLE `wipapp_version_history`
  ADD PRIMARY KEY (`ID`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `input_history`
--
ALTER TABLE `input_history`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `input_history_backup`
--
ALTER TABLE `input_history_backup`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `inventory_balance`
--
ALTER TABLE `inventory_balance`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `inventory_transactions`
--
ALTER TABLE `inventory_transactions`
  MODIFY `transaction_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `items`
--
ALTER TABLE `items`
  MODIFY `item_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `item_types`
--
ALTER TABLE `item_types`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `leads`
--
ALTER TABLE `leads`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `locations`
--
ALTER TABLE `locations`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `operations`
--
ALTER TABLE `operations`
  MODIFY `operation_id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `operation_numbers`
--
ALTER TABLE `operation_numbers`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `output_history`
--
ALTER TABLE `output_history`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `part_ids`
--
ALTER TABLE `part_ids`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `program_information`
--
ALTER TABLE `program_information`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `readonly`
--
ALTER TABLE `readonly`
  MODIFY `ID` int(100) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `remove_history`
--
ALTER TABLE `remove_history`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `saved_locations`
--
ALTER TABLE `saved_locations`
  MODIFY `ID` int(100) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `transfer_history`
--
ALTER TABLE `transfer_history`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `ID` int(100) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `wipapp_errorlog`
--
ALTER TABLE `wipapp_errorlog`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `wipapp_version_history`
--
ALTER TABLE `wipapp_version_history`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
