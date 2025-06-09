CREATE DATABASE  IF NOT EXISTS `mtm_wip_application` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `mtm_wip_application`;
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
-- Table structure for table `sys_user_roles`
--

DROP TABLE IF EXISTS `sys_user_roles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `sys_user_roles` (
  `UserID` int NOT NULL,
  `RoleID` int NOT NULL,
  `AssignedBy` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `AssignedAt` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`UserID`,`RoleID`),
  KEY `idx_userid` (`UserID`),
  KEY `idx_roleid` (`RoleID`),
  CONSTRAINT `sys_user_roles_ibfk_1` FOREIGN KEY (`UserID`) REFERENCES `usr_users` (`ID`) ON DELETE CASCADE,
  CONSTRAINT `sys_user_roles_ibfk_2` FOREIGN KEY (`RoleID`) REFERENCES `sys_roles` (`ID`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sys_user_roles`
--

LOCK TABLES `sys_user_roles` WRITE;
/*!40000 ALTER TABLE `sys_user_roles` DISABLE KEYS */;
INSERT INTO `sys_user_roles` VALUES (1,3,'[ System Migration ]','2025-06-01 22:02:03'),(2,1,'[ System Migration ]','2025-06-01 22:02:03'),(3,1,'[ System Migration ]','2025-06-01 22:02:03'),(4,1,'[ System Migration ]','2025-06-01 22:02:03'),(5,3,'[ System Migration ]','2025-06-01 22:02:03'),(6,3,'[ System Migration ]','2025-06-01 22:02:03'),(7,1,'[ System Migration ]','2025-06-01 22:02:03'),(8,1,'[ System Migration ]','2025-06-01 22:02:03'),(9,1,'[ System Migration ]','2025-06-01 22:02:03'),(10,3,'[ System Migration ]','2025-06-01 22:02:03'),(11,3,'[ System Migration ]','2025-06-01 22:02:03'),(12,1,'[ System Migration ]','2025-06-01 22:02:03'),(13,3,'[ System Migration ]','2025-06-01 22:02:03'),(14,1,'[ System Migration ]','2025-06-01 22:02:03'),(15,3,'[ System Migration ]','2025-06-01 22:02:03'),(16,3,'[ System Migration ]','2025-06-01 22:02:03'),(17,1,'[ System Migration ]','2025-06-01 22:02:03'),(18,1,'[ System Migration ]','2025-06-01 22:02:03'),(19,3,'[ System Migration ]','2025-06-01 22:02:03'),(20,1,'[ System Migration ]','2025-06-01 22:02:03'),(21,3,'[ System Migration ]','2025-06-01 22:02:03'),(22,1,'[ System Migration ]','2025-06-01 22:02:03'),(23,3,'[ System Migration ]','2025-06-01 22:02:03'),(24,1,'[ System Migration ]','2025-06-01 22:02:03'),(25,1,'[ System Migration ]','2025-06-01 22:02:03'),(26,3,'[ System Migration ]','2025-06-01 22:02:03'),(27,3,'[ System Migration ]','2025-06-01 22:02:03'),(28,1,'[ System Migration ]','2025-06-01 22:02:03'),(29,1,'[ System Migration ]','2025-06-01 22:02:03'),(30,3,'[ System Migration ]','2025-06-01 22:02:03'),(31,1,'[ System Migration ]','2025-06-01 22:02:03'),(32,3,'[ System Migration ]','2025-06-01 22:02:03'),(33,3,'[ System Migration ]','2025-06-01 22:02:03'),(34,1,'[ System Migration ]','2025-06-01 22:02:03'),(35,3,'[ System Migration ]','2025-06-01 22:02:03'),(36,3,'[ System Migration ]','2025-06-01 22:02:03'),(37,3,'[ System Migration ]','2025-06-01 22:02:03'),(38,1,'[ System Migration ]','2025-06-01 22:02:03'),(39,3,'[ System Migration ]','2025-06-01 22:02:03'),(40,1,'[ System Migration ]','2025-06-01 22:02:03'),(41,1,'[ System Migration ]','2025-06-01 22:02:03'),(42,1,'[ System Migration ]','2025-06-01 22:02:03'),(43,3,'[ System Migration ]','2025-06-01 22:02:03'),(44,1,'[ System Migration ]','2025-06-01 22:02:03'),(45,1,'[ System Migration ]','2025-06-01 22:02:03'),(46,1,'[ System Migration ]','2025-06-01 22:02:03'),(47,3,'[ System Migration ]','2025-06-01 22:02:03'),(48,3,'[ System Migration ]','2025-06-01 22:02:03'),(49,1,'[ System Migration ]','2025-06-01 22:02:03'),(50,1,'[ System Migration ]','2025-06-01 22:02:03'),(51,1,'[ System Migration ]','2025-06-01 22:02:03'),(52,1,'[ System Migration ]','2025-06-01 22:02:03'),(53,3,'[ System Migration ]','2025-06-01 22:02:03'),(54,3,'[ System Migration ]','2025-06-01 22:02:03'),(55,1,'[ System Migration ]','2025-06-01 22:02:03'),(56,1,'[ System Migration ]','2025-06-01 22:02:03'),(57,1,'[ System Migration ]','2025-06-01 22:02:03'),(58,1,'[ System Migration ]','2025-06-01 22:02:03'),(59,1,'[ System Migration ]','2025-06-01 22:02:03'),(60,1,'[ System Migration ]','2025-06-01 22:02:03'),(61,1,'[ System Migration ]','2025-06-01 22:02:03'),(62,1,'[ System Migration ]','2025-06-01 22:02:03'),(63,1,'[ System Migration ]','2025-06-01 22:02:03'),(64,1,'[ System Migration ]','2025-06-01 22:02:03'),(65,3,'[ System Migration ]','2025-06-01 22:02:03'),(66,3,'[ System Migration ]','2025-06-01 22:02:03'),(67,3,'[ System Migration ]','2025-06-01 22:02:03'),(68,1,'[ System Migration ]','2025-06-01 22:02:03'),(69,2,'[ System Migration ]','2025-06-01 22:02:03'),(70,3,'[ System Migration ]','2025-06-01 22:02:03'),(71,3,'[ System Migration ]','2025-06-01 22:02:03'),(72,3,'[ System Migration ]','2025-06-01 22:02:03'),(73,3,'[ System Migration ]','2025-06-01 22:02:03'),(74,3,'[ System Migration ]','2025-06-01 22:02:03'),(75,1,'[ System Migration ]','2025-06-01 22:02:03'),(76,3,'[ System Migration ]','2025-06-01 22:02:03'),(77,3,'[ System Migration ]','2025-06-01 22:02:03'),(78,3,'[ System Migration ]','2025-06-01 22:02:03');
/*!40000 ALTER TABLE `sys_user_roles` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-06-09 11:08:25
