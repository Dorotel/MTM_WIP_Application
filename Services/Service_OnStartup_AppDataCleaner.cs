using System.Diagnostics;
using MTM_WIP_Application.Logging;

namespace MTM_WIP_Application.Services;

internal static class Service_OnStartup_AppDataCleaner
{
    public static void DeleteDirectoryContents(string directoryPath)
    {
        try
        {
            if (Directory.Exists(directoryPath))
            {
                Debug.WriteLine($"Deleting contents of directory: {directoryPath}");
                ApplicationLog.Log($"Deleting contents of directory: {directoryPath}");

                foreach (var file in Directory.GetFiles(directoryPath))
                {
                    Debug.WriteLine($"Deleting file: {file}");
                    ApplicationLog.Log($"Deleting file: {file}");
                    File.Delete(file);
                }

                foreach (var subDirectory in Directory.GetDirectories(directoryPath))
                {
                    Debug.WriteLine($"Deleting subdirectory: {subDirectory}");
                    ApplicationLog.Log($"Deleting subdirectory: {subDirectory}");
                    Directory.Delete(subDirectory, true);
                }
            }
            else
            {
                Debug.WriteLine($"Directory does not exist: {directoryPath}");
                ApplicationLog.Log($"Directory does not exist: {directoryPath}");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error deleting contents of directory {directoryPath}: {ex.Message}");
            ApplicationLog.Log($"Error deleting contents of directory {directoryPath}: {ex.Message}");
        }
    }

    private static void DeleteDirectoryIfExists(string path)
    {
        Debug.WriteLine($"Checking if directory exists: {path}");
        ApplicationLog.Log($"Checking if directory exists: {path}");

        try
        {
            if (Directory.Exists(path))
            {
                Debug.WriteLine($"Directory exists. Deleting: {path}");
                Directory.Delete(path, true);
                ApplicationLog.Log($"Deleted directory: {path}");
            }
            else
            {
                Debug.WriteLine($"Directory does not exist: {path}");
                ApplicationLog.Log($"Directory does not exist: {path}");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error deleting directory {path}: {ex.Message}");
            ApplicationLog.Log($"Error deleting directory {path}: {ex.Message}");
        }
    }

    public static void WipeAppDataFolders()
    {
        Debug.WriteLine("Wiping AppData folders...");
        ApplicationLog.Log("Wiping AppData folders...");

        try
        {
            var appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "MTM_WIP_APP");
            var localAppDataPath =
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "MTM_WIP_APP");

            DeleteDirectoryIfExists(appDataPath);
            DeleteDirectoryIfExists(localAppDataPath);

            ApplicationLog.Log("MTM_WIP_APP folders wiped from AppData and LocalAppData.");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error wiping MTM_WIP_APP folders: {ex.Message}");
            ApplicationLog.Log($"Error wiping MTM_WIP_APP folders: {ex.Message}");
        }
    }
}