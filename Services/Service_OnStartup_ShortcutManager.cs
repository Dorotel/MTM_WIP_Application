using System.Diagnostics;
using IWshRuntimeLibrary;
using MTM_WIP_Application.Logging;
using File = System.IO.File;

namespace MTM_WIP_Application.Services;

internal static class Service_OnStartup_ShortcutManager
{
    private static void CreateShortcut(string shortcutPath, string targetPath, string description)
    {
        Debug.WriteLine($"Creating shortcut at {shortcutPath}...");
        ApplicationLog.Log($"Creating shortcut at {shortcutPath}...");

        try
        {
            var shell = new WshShell();
            var shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);
            shortcut.TargetPath = targetPath;
            shortcut.Description = description;
            shortcut.Save();

            Debug.WriteLine($"Shortcut created at {shortcutPath}");
            ApplicationLog.Log($"Shortcut created at {shortcutPath}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error creating shortcut: {ex.Message}");
            ApplicationLog.Log($"Error creating shortcut: {ex.Message}");
        }
    }

    public static void EnsureApplicationShortcut()
    {
        Debug.WriteLine("Ensuring application shortcut...");
        ApplicationLog.Log("Ensuring application shortcut...");

        try
        {
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var shortcutName = "MTM WIP App (Live).lnk";
            var shortcutPath = Path.Combine(desktopPath, shortcutName);
            var applicationPath = Application.ExecutablePath;

            Debug.WriteLine("Checking for existing shortcuts...");
            ApplicationLog.Log("Checking for existing shortcuts...");
            var existingShortcuts = Directory.GetFiles(desktopPath, "*.lnk")
                .Where(file => GetShortcutTarget(file) == applicationPath)
                .ToList();

            foreach (var shortcut in existingShortcuts)
                if (!shortcut.Equals(shortcutPath, StringComparison.OrdinalIgnoreCase))
                {
                    Debug.WriteLine($"Deleting existing shortcut: {shortcut}");
                    File.Delete(shortcut);
                    ApplicationLog.Log($"Deleted existing shortcut: {shortcut}");
                }

            if (!File.Exists(shortcutPath))
            {
                Debug.WriteLine($"Creating new shortcut: {shortcutPath}");
                CreateShortcut(shortcutPath, applicationPath, "MTM WIP Application");
                ApplicationLog.Log($"Created new shortcut: {shortcutPath}");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error ensuring application shortcut: {ex.Message}");
            ApplicationLog.Log($"Error ensuring application shortcut: {ex.Message}");
        }
    }

    private static string GetShortcutTarget(string shortcutPath)
    {
        Debug.WriteLine($"Getting shortcut target for {shortcutPath}...");
        ApplicationLog.Log($"Getting shortcut target for {shortcutPath}...");

        try
        {
            var shell = new WshShell();
            var shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);
            return shortcut.TargetPath;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error getting shortcut target: {ex.Message}");
            ApplicationLog.Log($"Error getting shortcut target: {ex.Message}");
            return string.Empty;
        }
    }
}