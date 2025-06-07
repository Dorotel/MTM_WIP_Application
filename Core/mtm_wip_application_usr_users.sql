-- MySQL dump 10.13  Distrib 8.0.42, for Win64 (x86_64)
--
-- Host: localhost    Database: mtm_wip_application
-- ------------------------------------------------------
-- Server version	8.0.42

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `usr_users`
--

DROP TABLE IF EXISTS `usr_users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `usr_users` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `User` varchar(100) NOT NULL,
  `Full Name` varchar(200) DEFAULT NULL,
  `Shift` varchar(50) NOT NULL DEFAULT '1',
  `VitsUser` tinyint(1) NOT NULL DEFAULT '0',
  `Pin` varchar(50) DEFAULT NULL,
  `LastShownVersion` varchar(50) NOT NULL DEFAULT '0.0.0.0',
  `HideChangeLog` varchar(50) NOT NULL DEFAULT 'false',
  `Theme_Name` varchar(50) NOT NULL DEFAULT 'Default (Black and White)',
  `Theme_FontSize` int NOT NULL DEFAULT '9',
  `VisualUserName` varchar(50) NOT NULL DEFAULT 'User Name',
  `VisualPassword` varchar(50) NOT NULL DEFAULT 'Password',
  `WipServerAddress` varchar(15) NOT NULL DEFAULT '172.16.1.104',
  `WipServerPort` varchar(10) NOT NULL DEFAULT '3306',
  PRIMARY KEY (`ID`),
  UNIQUE KEY `uq_user` (`User`)
) ENGINE=InnoDB AUTO_INCREMENT=79 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-06-06 11:21:35
