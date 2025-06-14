using System.Diagnostics;
using IWshRuntimeLibrary;
using MTM_WIP_Application.Logging;
using File = System.IO.File;

namespace MTM_WIP_Application.Services;

internal static class Service_OnStartup_ShortcutManager
{
    #region Public Methods

    public static void EnsureApplicationShortcut()
    {
        try
        {
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var shortcutName = "MTM WIP App (Live).lnk";
            var shortcutPath = Path.Combine(desktopPath, shortcutName);
            var applicationPath = Application.ExecutablePath;
            var existingShortcuts = Directory.GetFiles(desktopPath, "*.lnk")
                .Where(file => GetShortcutTarget(file) == applicationPath)
                .ToList();
            foreach (var shortcut in existingShortcuts)
                if (!shortcut.Equals(shortcutPath, StringComparison.OrdinalIgnoreCase))
                    File.Delete(shortcut);

            if (!File.Exists(shortcutPath)) CreateShortcut(shortcutPath, applicationPath, "MTM WIP Application");
        }
        catch (Exception ex)
        {
            LoggingUtility.Log($"Error ensuring application shortcut: {ex.Message}");
        }
    }

    #endregion

    #region Private Methods

    private static void CreateShortcut(string shortcutPath, string targetPath, string description)
    {
        try
        {
            var shell = new WshShell();
            var shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);
            shortcut.TargetPath = targetPath;
            shortcut.Description = description;
            shortcut.Save();
        }
        catch (Exception ex)
        {
            LoggingUtility.Log($"Error creating shortcut: {ex.Message}");
        }
    }

    private static string GetShortcutTarget(string shortcutPath)
    {
        try
        {
            var shell = new WshShell();
            var shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);
            return shortcut.TargetPath;
        }
        catch (Exception ex)
        {
            LoggingUtility.Log($"Error getting shortcut target: {ex.Message}");
            return string.Empty;
        }
    }

    #endregion
}