using MTM_WIP_Application.Logging;
using System.Diagnostics;

namespace MTM_WIP_Application.Services;

internal static class AppDataCleaner
{
    public static void WipeAppDataFolders()
    {
        Debug.WriteLine("Wiping AppData folders...");
        AppLogger.Log("Wiping AppData folders...");

        try
        {
            var appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "MTM_WIP_APP");
            var localAppDataPath =
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "MTM_WIP_APP");

            DeleteDirectoryIfExists(appDataPath);
            DeleteDirectoryIfExists(localAppDataPath);

            AppLogger.Log("MTM_WIP_APP folders wiped from AppData and LocalAppData.");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error wiping MTM_WIP_APP folders: {ex.Message}");
            AppLogger.Log($"Error wiping MTM_WIP_APP folders: {ex.Message}");
        }
    }

    private static void DeleteDirectoryIfExists(string path)
    {
        Debug.WriteLine($"Checking if directory exists: {path}");
        AppLogger.Log($"Checking if directory exists: {path}");

        try
        {
            if (Directory.Exists(path))
            {
                Debug.WriteLine($"Directory exists. Deleting: {path}");
                Directory.Delete(path, true);
                AppLogger.Log($"Deleted directory: {path}");
            }
            else
            {
                Debug.WriteLine($"Directory does not exist: {path}");
                AppLogger.Log($"Directory does not exist: {path}");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error deleting directory {path}: {ex.Message}");
            AppLogger.Log($"Error deleting directory {path}: {ex.Message}");
        }
    }

    public static void DeleteDirectoryContents(string directoryPath)
    {
        try
        {
            if (Directory.Exists(directoryPath))
            {
                Debug.WriteLine($"Deleting contents of directory: {directoryPath}");
                AppLogger.Log($"Deleting contents of directory: {directoryPath}");

                foreach (var file in Directory.GetFiles(directoryPath))
                {
                    Debug.WriteLine($"Deleting file: {file}");
                    AppLogger.Log($"Deleting file: {file}");
                    File.Delete(file);
                }

                foreach (var subDirectory in Directory.GetDirectories(directoryPath))
                {
                    Debug.WriteLine($"Deleting subdirectory: {subDirectory}");
                    AppLogger.Log($"Deleting subdirectory: {subDirectory}");
                    Directory.Delete(subDirectory, true);
                }
            }
            else
            {
                Debug.WriteLine($"Directory does not exist: {directoryPath}");
                AppLogger.Log($"Directory does not exist: {directoryPath}");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error deleting contents of directory {directoryPath}: {ex.Message}");
            AppLogger.Log($"Error deleting contents of directory {directoryPath}: {ex.Message}");
        }
    }
}