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
-- Table structure for table `usr_users`
--

DROP TABLE IF EXISTS `usr_users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `usr_users` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `User` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Full Name` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `Shift` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '1',
  `VitsUser` tinyint(1) NOT NULL DEFAULT '0',
  `Pin` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `LastShownVersion` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '0.0.0.0',
  `HideChangeLog` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT 'false',
  `Theme_Name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT 'Default (Black and White)',
  `Theme_FontSize` int NOT NULL DEFAULT '9',
  `VisualUserName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT 'User Name',
  `VisualPassword` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT 'Password',
  `WipServerAddress` varchar(15) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '172.16.1.104',
  `WipServerPort` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL DEFAULT '3306',
  PRIMARY KEY (`ID`),
  UNIQUE KEY `uq_user` (`User`)
) ENGINE=InnoDB AUTO_INCREMENT=79 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `usr_users`
--

LOCK TABLES `usr_users` WRITE;
/*!40000 ALTER TABLE `usr_users` DISABLE KEYS */;
INSERT INTO `usr_users` VALUES (1,'[ All Users ]',NULL,'1',0,NULL,'0.0.0.0','false','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(2,'DLAFOND',NULL,'1',0,NULL,'0.0.0.0','false','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(3,'JEGBERT',NULL,'1',0,NULL,'0.0.0.0','false','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(4,'RECEIVING',NULL,'1',0,NULL,'4.5.0.3','true','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(5,'SHOP2',NULL,'1',0,NULL,'4.5.1.0','true','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(6,'SHIPPING',NULL,'1',0,NULL,'0.0.0.0','false','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(7,'DHAMMONS',NULL,'1',0,NULL,'4.6.0.0','true','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(8,'JMAUER',NULL,'1',0,NULL,'4.6.0.0','true','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(9,'KWILKER',NULL,'1',0,NULL,'4.5.0.2','true','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(10,'MHANDLER',NULL,'1',0,NULL,'0.0.0.0','false','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(11,'MIKESAMZ',NULL,'1',0,NULL,'4.6.0.0','true','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(12,'MLAURIN',NULL,'1',0,NULL,'4.6.0.0','true','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(13,'MLEDVINA',NULL,'1',0,NULL,'4.6.0.0','true','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(14,'NPITSCH',NULL,'1',0,NULL,'4.6.0.0','true','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(15,'PBAHR',NULL,'1',0,NULL,'4.6.0.0','false','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(16,'ROOT',NULL,'1',0,NULL,'0.0.0.0','false','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(17,'SCARBON',NULL,'1',0,NULL,'0.0.0.0','false','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(18,'TTELETZKE',NULL,'1',0,NULL,'4.6.0.0','false','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(19,'TLINDLOFF',NULL,'1',0,NULL,'0.0.0.0','false','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(20,'ABEEMAN',NULL,'1',0,NULL,'4.6.0.0','false','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(21,'DHAGENOW',NULL,'1',0,NULL,'4.6.0.0','true','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(22,'JORNALES',NULL,'1',0,NULL,'4.5.0.2','false','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(23,'TSMAXWELL',NULL,'1',0,NULL,'4.6.0.0','true','Default (Black and White)',11,'User Name','Password','172.16.1.104','3306'),(24,'CMUCHOWSKI',NULL,'1',0,NULL,'4.5.1.0','true','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(25,'NWUNSCH',NULL,'1',0,NULL,'4.5.0.2','false','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(26,'GWHITSON',NULL,'1',0,NULL,'4.5.0.3','false','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(27,'JCASTRO',NULL,'1',0,NULL,'4.6.0.0','false','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(28,'MVOSS',NULL,'1',0,NULL,'4.5.2.0','true','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(29,'NLEE',NULL,'1',0,NULL,'4.5.2.0','false','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(30,'JMILLER',NULL,'1',0,NULL,'0.0.0.0','false','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(31,'SSNYDER',NULL,'1',0,NULL,'4.5.1.0','false','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(32,'CSNYDER',NULL,'1',0,NULL,'4.5.1.0','false','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(33,'BAUSTIN',NULL,'1',0,NULL,'4.5.0.3','true','Light Red',9,'User Name','Password','172.16.1.104','3306'),(34,'DEBLAFOND',NULL,'1',0,NULL,'4.6.0.0','true','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(35,'ASCHULTZ',NULL,'1',0,NULL,'0.0.0.0','false','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(36,'SJACKSON',NULL,'1',0,NULL,'4.6.0.0','true','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(37,'DRIEBE',NULL,'1',0,NULL,'4.5.1.0','true','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(38,'TRADDATZ',NULL,'1',0,NULL,'4.5.1.0','true','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(39,'SDETTLAFF',NULL,'1',0,NULL,'4.6.0.0','true','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(40,'JWETAK',NULL,'1',0,NULL,'4.6.0.0','false','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(41,'KSKATTEBO',NULL,'1',0,NULL,'4.6.0.0','true','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(42,'AGAUTHIER',NULL,'1',0,NULL,'4.5.0.2','true','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(43,'MBECKER',NULL,'1',0,NULL,'4.6.0.0','true','Light Blue',10,'User Name','Password','172.16.1.104','3306'),(44,'RLESSER',NULL,'1',0,NULL,'0.0.0.0','false','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(45,'AGROELLE',NULL,'1',0,NULL,'4.6.0.0','true','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(46,'CEHLENBECK',NULL,'1',0,NULL,'4.6.0.0','true','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(47,'BNEUMAN',NULL,'1',0,NULL,'4.6.0.0','true','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(48,'MTMDC',NULL,'1',0,NULL,'4.5.1.0','true','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(49,'KDREWIESKE',NULL,'1',0,NULL,'4.6.0.0','true','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(50,'MHERNANDEZ',NULL,'1',0,NULL,'4.6.0.0','true','Light Blue',10,'User Name','Password','172.16.1.104','3306'),(51,'KLEE',NULL,'1',0,NULL,'4.6.0.0','true','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(52,'ADMININT',NULL,'1',0,NULL,'4.6.0.0','true','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(53,'CALVAREZ',NULL,'1',0,NULL,'4.6.0.0','false','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(54,'TYANG',NULL,'1',0,NULL,'4.6.0.0','true','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(55,'KSMITH',NULL,'1',0,NULL,'4.6.0.0','true','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(56,'JKOLL',NULL,'1',0,NULL,'4.6.0.0','true','Default (Black and White)',9,'JKOLL','KOLL','172.16.1.104','3306'),(57,'JBEHRMANN',NULL,'1',0,NULL,'4.6.0.0','false','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(58,'MDRESSEL',NULL,'1',0,NULL,'4.6.0.0','true','Light Blue',11,'User Name','Password','172.16.1.104','3306'),(59,'DSMITH',NULL,'1',0,NULL,'4.6.0.0','true','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(60,'APIESCHEL',NULL,'1',0,NULL,'4.6.0.0','false','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(61,'JPATTERSON',NULL,'1',0,NULL,'4.5.1.0','false','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(62,'VKINGSBURY',NULL,'1',0,NULL,'0.0.0.0','false','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(63,'JHAFNER',NULL,'1',0,NULL,'4.5.1.0','true','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(64,'JHERMAN',NULL,'1',0,NULL,'0.0.0.0','false','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(65,'DSANCHEZ',NULL,'1',0,NULL,'4.6.0.0','true','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(66,'TLOHSE',NULL,'1',0,NULL,'4.6.0.0','false','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(67,'JOHNK',NULL,'1',0,NULL,'4.6.0.0','true','Default (Black and White)',9,'JKOLL','KOLL','172.16.1.104','3306'),(68,'BSTEEVES',NULL,'1',0,NULL,'4.6.0.0','true','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(69,'JHALLOCK',NULL,'1',0,NULL,'4.6.0.0','true','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(70,'GGERK',NULL,'1',0,NULL,'4.6.0.0','true','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(71,'TMUELLER',NULL,'1',0,NULL,'0.0.0.0','false','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(72,'CLOR',NULL,'1',0,NULL,'4.6.0.0','true','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(73,'AGAY',NULL,'1',0,NULL,'4.6.0.0','true','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(74,'NPIESCHEL',NULL,'1',0,NULL,'4.6.0.0','true','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(75,'LBARTS',NULL,'1',0,NULL,'4.6.0.0','true','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(76,'ESMITH',NULL,'1',0,NULL,'4.6.0.0','false','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(77,'JRODRIGUEZ',NULL,'1',0,NULL,'4.6.0.0','true','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306'),(78,'PFLOOR',NULL,'1',0,NULL,'4.6.0.0','true','Default (Black and White)',9,'User Name','Password','172.16.1.104','3306');
/*!40000 ALTER TABLE `usr_users` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-06-09 11:08:24
