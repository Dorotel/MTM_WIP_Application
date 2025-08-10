using System.Diagnostics;
using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Services
{
    internal static class Service_OnStartup_AppDataCleaner
    {
        #region Public Methods

        public static void DeleteDirectoryContents(string directoryPath)
        {
            try
            {
                if (Directory.Exists(directoryPath))
                {
                    foreach (string file in Directory.GetFiles(directoryPath))
                    {
                        File.Delete(file);
                    }

                    foreach (string subDirectory in Directory.GetDirectories(directoryPath))
                    {
                        Directory.Delete(subDirectory, true);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingUtility.Log($"Error deleting contents of directory {directoryPath}: {ex.Message}");
            }
        }

        public static void WipeAppDataFolders()
        {
            try
            {
                string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "MTM_WIP_APP");
                string localAppDataPath =
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "MTM_WIP_APP");
                DeleteDirectoryIfExists(appDataPath);
                DeleteDirectoryIfExists(localAppDataPath);
            }
            catch (Exception ex)
            {
                LoggingUtility.Log($"Error wiping MTM_WIP_APP folders: {ex.Message}");
            }
        }

        #endregion

        #region Private Methods

        private static void DeleteDirectoryIfExists(string path)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
            }
            catch (Exception ex)
            {
                LoggingUtility.Log($"Error deleting directory {path}: {ex.Message}");
            }
        }

        #endregion
    }
}
