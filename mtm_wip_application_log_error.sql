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
-- Table structure for table `log_error`
--

DROP TABLE IF EXISTS `log_error`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `log_error` (
  `ID` int NOT NULL AUTO_INCREMENT,
  `User` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `Severity` enum('Information','Warning','Error','Critical','High') CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL DEFAULT 'Error',
  `ErrorType` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `ErrorMessage` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL,
  `StackTrace` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `ModuleName` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `MethodName` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `AdditionalInfo` text CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci,
  `MachineName` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `OSVersion` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `AppVersion` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `ErrorTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`ID`),
  KEY `idx_errortime` (`ErrorTime`),
  KEY `idx_user` (`User`),
  KEY `idx_severity` (`Severity`),
  KEY `idx_errortype` (`ErrorType`)
) ENGINE=InnoDB AUTO_INCREMENT=49 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `log_error`
--

LOCK TABLES `log_error` WRITE;
/*!40000 ALTER TABLE `log_error` DISABLE KEYS */;
INSERT INTO `log_error` VALUES (41,'JOHNK','Critical','System.Exception','SQL Error occurred in method: TestCaller, control: TestControl. Exception: Test SQL exception',NULL,'ErrorLogDao','TestCaller',NULL,'JOHNSPC','Microsoft Windows NT 10.0.26100.0','MTM Application','2025-06-07 09:25:14'),(42,'JOHNK','Critical','Moq.MockException','Error occurred in method: , control: . Exception: ISqlHelper.ExecuteNonQueryAsync(\"sys_user_roles_Set_AccessType\", Dictionary<string, object>, False, CommandType.StoredProcedure) invocation failed with mock behavior Strict.\r\nAll invocations on the mock must have a corresponding setup.','   at Moq.FailForStrictMock.Handle(Invocation invocation, Mock mock) in /_/src/Moq/Interception/InterceptionAspects.cs:line 182\r\n   at Moq.Mock.Moq.IInterceptor.Intercept(Invocation invocation) in /_/src/Moq/Interception/Mock.cs:line 27\r\n   at Moq.CastleProxyFactory.Interceptor.Intercept(IInvocation underlying) in /_/src/Moq/Interception/CastleProxyFactory.cs:line 107\r\n   at Castle.DynamicProxy.AbstractInvocation.Proceed()\r\n   at Castle.Proxies.ISqlHelperProxy.ExecuteNonQueryAsync(String procedureOrSql, Dictionary`2 parameters, Boolean useAsync, CommandType commandType)\r\n   at MTM_WIP_Application.Data.SystemDao.SetUserAccessTypeAsync(String userName, String accessType, Boolean useAsync) in C:\\Users\\johnk\\source\\repos\\MTM_WIP_Application\\Data\\SystemDao.cs:line 51','ErrorLogDao','',NULL,'JOHNSPC','Microsoft Windows NT 10.0.26100.0','MTM Application','2025-06-07 09:55:14'),(43,'JOHNK','Critical','Moq.MockException','Error occurred in method: , control: . Exception: ISqlHelper.ExecuteNonQueryAsync(\"sys_user_roles_Set_AccessType\", Dictionary<string, object>, False, CommandType.StoredProcedure) invocation failed with mock behavior Strict.\r\nAll invocations on the mock must have a corresponding setup.','   at Moq.FailForStrictMock.Handle(Invocation invocation, Mock mock) in /_/src/Moq/Interception/InterceptionAspects.cs:line 182\r\n   at Moq.Mock.Moq.IInterceptor.Intercept(Invocation invocation) in /_/src/Moq/Interception/Mock.cs:line 27\r\n   at Moq.CastleProxyFactory.Interceptor.Intercept(IInvocation underlying) in /_/src/Moq/Interception/CastleProxyFactory.cs:line 107\r\n   at Castle.DynamicProxy.AbstractInvocation.Proceed()\r\n   at Castle.Proxies.ISqlHelperProxy.ExecuteNonQueryAsync(String procedureOrSql, Dictionary`2 parameters, Boolean useAsync, CommandType commandType)\r\n   at MTM_WIP_Application.Data.SystemDao.SetUserAccessTypeAsync(String userName, String accessType, Boolean useAsync) in C:\\Users\\johnk\\source\\repos\\MTM_WIP_Application\\Data\\SystemDao.cs:line 51','ErrorLogDao','',NULL,'JOHNSPC','Microsoft Windows NT 10.0.26100.0','MTM Application','2025-06-07 09:57:09'),(44,'JOHNK','Critical','System.ArgumentNullException','Error occurred in method: DgvPrinter.Print, control: . Exception: Value cannot be null. (Parameter \'dgv\')','   at MTM_WIP_Application.Core.DgvPrinter.Print(DataGridView dgv) in C:\\Users\\johnk\\source\\repos\\MTM_WIP_Application\\Core\\DgvPrinter.cs:line 40','ErrorLogDao','DgvPrinter.Print',NULL,'JOHNSPC','Microsoft Windows NT 10.0.26100.0','MTM Application','2025-06-07 10:32:08'),(45,'JOHNK','Critical','MySql.Data.MySqlClient.MySqlException','Error occurred in method: , control: . Exception: Procedure or function \'usr_users_Get_Setting\' cannot be found in database \'\' Verify that user \'JOHNK\'@\'localhost\' has enough privileges to execute','   at MySql.Data.MySqlClient.ProcedureCache.GetProcDataAsync(MySqlConnection connection, String spName, Boolean execAsync)\r\n   at MySql.Data.MySqlClient.ProcedureCache.AddNewAsync(MySqlConnection connection, String spName, Boolean execAsync)\r\n   at MySql.Data.MySqlClient.ProcedureCache.GetProcedureAsync(MySqlConnection conn, String spName, String cacheKey, Boolean execAsync)\r\n   at MySql.Data.MySqlClient.StoredProcedure.GetParametersAsync(String procName, Boolean execAsync)\r\n   at MySql.Data.MySqlClient.StoredProcedure.CheckParametersAsync(String spName, Boolean execAsync)\r\n   at MySql.Data.MySqlClient.StoredProcedure.Resolve(Boolean preparing)\r\n   at MySql.Data.MySqlClient.MySqlCommand.ExecuteReaderAsync(CommandBehavior behavior, Boolean execAsync, CancellationToken cancellationToken)\r\n   at MySql.Data.MySqlClient.MySqlCommand.ExecuteScalarAsync(Boolean execAsync, CancellationToken cancellationToken)\r\n   at MTM_WIP_Application.Data.SqlHelper.ExecuteScalar(String procedureOrSql, Dictionary`2 parameters, Boolean useAsync, CommandType commandType) in C:\\Users\\johnk\\source\\repos\\MTM_WIP_Application\\Data\\SqlHelper.cs:line 106\r\n   at MTM_WIP_Application.Data.UserDao.GetUserSettingAsync(String field, String user, Boolean useAsync) in C:\\Users\\johnk\\source\\repos\\MTM_WIP_Application\\Data\\UserDao.cs:line 81','ErrorLogDao','',NULL,'JOHNSPC','Microsoft Windows NT 10.0.26100.0','MTM Application','2025-06-07 10:40:33'),(46,'JOHNK','Critical','MySql.Data.MySqlClient.MySqlException','Error occurred in method: , control: . Exception: Procedure or function \'usr_users_Get_Setting\' cannot be found in database \'\' Verify that user \'JOHNK\'@\'localhost\' has enough privileges to execute','   at MySql.Data.MySqlClient.ProcedureCache.GetProcDataAsync(MySqlConnection connection, String spName, Boolean execAsync)\r\n   at MySql.Data.MySqlClient.ProcedureCache.AddNewAsync(MySqlConnection connection, String spName, Boolean execAsync)\r\n   at MySql.Data.MySqlClient.ProcedureCache.GetProcedureAsync(MySqlConnection conn, String spName, String cacheKey, Boolean execAsync)\r\n   at MySql.Data.MySqlClient.StoredProcedure.GetParametersAsync(String procName, Boolean execAsync)\r\n   at MySql.Data.MySqlClient.StoredProcedure.CheckParametersAsync(String spName, Boolean execAsync)\r\n   at MySql.Data.MySqlClient.StoredProcedure.Resolve(Boolean preparing)\r\n   at MySql.Data.MySqlClient.MySqlCommand.ExecuteReaderAsync(CommandBehavior behavior, Boolean execAsync, CancellationToken cancellationToken)\r\n   at MySql.Data.MySqlClient.MySqlCommand.ExecuteScalarAsync(Boolean execAsync, CancellationToken cancellationToken)\r\n   at MTM_WIP_Application.Data.SqlHelper.ExecuteScalar(String procedureOrSql, Dictionary`2 parameters, Boolean useAsync, CommandType commandType) in C:\\Users\\johnk\\source\\repos\\MTM_WIP_Application\\Data\\SqlHelper.cs:line 106\r\n   at MTM_WIP_Application.Data.UserDao.GetUserSettingAsync(String field, String user, Boolean useAsync) in C:\\Users\\johnk\\source\\repos\\MTM_WIP_Application\\Data\\UserDao.cs:line 81','ErrorLogDao','',NULL,'JOHNSPC','Microsoft Windows NT 10.0.26100.0','MTM Application','2025-06-07 10:46:39'),(47,'JOHNK','Critical','MySql.Data.MySqlClient.MySqlException','Error occurred in method: , control: . Exception: Procedure or function \'usr_users_Get_Setting\' cannot be found in database \'\' Verify that user \'JOHNK\'@\'localhost\' has enough privileges to execute','   at MySql.Data.MySqlClient.ProcedureCache.GetProcDataAsync(MySqlConnection connection, String spName, Boolean execAsync)\r\n   at MySql.Data.MySqlClient.ProcedureCache.AddNewAsync(MySqlConnection connection, String spName, Boolean execAsync)\r\n   at MySql.Data.MySqlClient.ProcedureCache.GetProcedureAsync(MySqlConnection conn, String spName, String cacheKey, Boolean execAsync)\r\n   at MySql.Data.MySqlClient.StoredProcedure.GetParametersAsync(String procName, Boolean execAsync)\r\n   at MySql.Data.MySqlClient.StoredProcedure.CheckParametersAsync(String spName, Boolean execAsync)\r\n   at MySql.Data.MySqlClient.StoredProcedure.Resolve(Boolean preparing)\r\n   at MySql.Data.MySqlClient.MySqlCommand.ExecuteReaderAsync(CommandBehavior behavior, Boolean execAsync, CancellationToken cancellationToken)\r\n   at MySql.Data.MySqlClient.MySqlCommand.ExecuteScalarAsync(Boolean execAsync, CancellationToken cancellationToken)\r\n   at MTM_WIP_Application.Data.SqlHelper.ExecuteScalar(String procedureOrSql, Dictionary`2 parameters, Boolean useAsync, CommandType commandType) in C:\\Users\\johnk\\source\\repos\\MTM_WIP_Application\\Data\\SqlHelper.cs:line 106\r\n   at MTM_WIP_Application.Data.UserDao.GetUserSettingAsync(String field, String user, Boolean useAsync) in C:\\Users\\johnk\\source\\repos\\MTM_WIP_Application\\Data\\UserDao.cs:line 81','ErrorLogDao','',NULL,'JOHNSPC','Microsoft Windows NT 10.0.26100.0','MTM Application','2025-06-07 10:48:23'),(48,'JOHNK','Critical','MySql.Data.MySqlClient.MySqlException','Error occurred in method: , control: . Exception: Procedure or function \'usr_users_Get_Setting\' cannot be found in database \'\' Verify that user \'JOHNK\'@\'localhost\' has enough privileges to execute','   at MySql.Data.MySqlClient.ProcedureCache.GetProcDataAsync(MySqlConnection connection, String spName, Boolean execAsync)\r\n   at MySql.Data.MySqlClient.ProcedureCache.AddNewAsync(MySqlConnection connection, String spName, Boolean execAsync)\r\n   at MySql.Data.MySqlClient.ProcedureCache.GetProcedureAsync(MySqlConnection conn, String spName, String cacheKey, Boolean execAsync)\r\n   at MySql.Data.MySqlClient.StoredProcedure.GetParametersAsync(String procName, Boolean execAsync)\r\n   at MySql.Data.MySqlClient.StoredProcedure.CheckParametersAsync(String spName, Boolean execAsync)\r\n   at MySql.Data.MySqlClient.StoredProcedure.Resolve(Boolean preparing)\r\n   at MySql.Data.MySqlClient.MySqlCommand.ExecuteReaderAsync(CommandBehavior behavior, Boolean execAsync, CancellationToken cancellationToken)\r\n   at MySql.Data.MySqlClient.MySqlCommand.ExecuteScalarAsync(Boolean execAsync, CancellationToken cancellationToken)\r\n   at MTM_WIP_Application.Data.SqlHelper.ExecuteScalar(String procedureOrSql, Dictionary`2 parameters, Boolean useAsync, CommandType commandType) in C:\\Users\\johnk\\source\\repos\\MTM_WIP_Application\\Data\\SqlHelper.cs:line 106\r\n   at MTM_WIP_Application.Data.UserDao.GetUserSettingAsync(String field, String user, Boolean useAsync) in C:\\Users\\johnk\\source\\repos\\MTM_WIP_Application\\Data\\UserDao.cs:line 81','ErrorLogDao','',NULL,'JOHNSPC','Microsoft Windows NT 10.0.26100.0','MTM Application','2025-06-07 10:48:29');
/*!40000 ALTER TABLE `log_error` ENABLE KEYS */;
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
