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
-- Table structure for table `md_operation_numbers`
--

DROP TABLE IF EXISTS `md_operation_numbers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `md_operation_numbers` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `Operation` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `Issued By` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  PRIMARY KEY (`ID`),
  UNIQUE KEY `uq_operation` (`Operation`)
) ENGINE=InnoDB AUTO_INCREMENT=73 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `md_operation_numbers`
--

LOCK TABLES `md_operation_numbers` WRITE;
/*!40000 ALTER TABLE `md_operation_numbers` DISABLE KEYS */;
INSERT INTO `md_operation_numbers` VALUES (1,'10','[ System ]'),(2,'100','[ System ]'),(3,'106','[ System ]'),(4,'109','[ System ]'),(5,'110','[ System ]'),(6,'119','[ System ]'),(7,'12','[ System ]'),(8,'120','[ System ]'),(9,'15','[ System ]'),(10,'19','[ System ]'),(11,'20','[ System ]'),(12,'21','[ System ]'),(13,'220','[ System ]'),(14,'29','[ System ]'),(15,'30','[ System ]'),(16,'300','[ System ]'),(17,'309','[ System ]'),(18,'31','[ System ]'),(19,'310','[ System ]'),(20,'320','[ System ]'),(21,'350','[ System ]'),(22,'359','[ System ]'),(23,'39','[ System ]'),(24,'40','[ System ]'),(25,'400','[ System ]'),(26,'409','[ System ]'),(27,'49','[ System ]'),(28,'50','[ System ]'),(29,'500','[ System ]'),(30,'509','[ System ]'),(31,'59','[ System ]'),(32,'6','[ System ]'),(33,'60','[ System ]'),(34,'600','[ System ]'),(35,'69','[ System ]'),(36,'7','[ System ]'),(37,'700','[ System ]'),(38,'8','[ System ]'),(39,'80','[ System ]'),(40,'800','[ System ]'),(41,'889','[ System ]'),(42,'89','[ System ]'),(43,'890','[ System ]'),(44,'899','[ System ]'),(45,'90','[ System ]'),(46,'900','[ System ]'),(47,'909','[ System ]'),(48,'91','[ System ]'),(49,'910','[ System ]'),(50,'915','[ System ]'),(51,'917','[ System ]'),(52,'919','[ System ]'),(53,'920','[ System ]'),(54,'95','[ System ]'),(55,'950','[ System ]'),(56,'959','[ System ]'),(57,'96','[ System ]'),(58,'99','[ System ]'),(59,'25','[ System ]'),(60,'880','JKOLL'),(61,'18','MVOGEL'),(62,'N/A','JKOLL'),(63,'939','JKOLL'),(64,'929','JKOLL'),(65,'252','JKOLL'),(66,'949','JKOLL'),(67,'905','MLAURIN'),(68,'70','DHAMMONS'),(69,'35','RECEIVING'),(70,'45','DHAMMONS'),(71,'252-01','KSKATTEBO'),(72,'97','JMAUER');
/*!40000 ALTER TABLE `md_operation_numbers` ENABLE KEYS */;
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
