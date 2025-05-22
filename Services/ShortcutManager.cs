using MTM_WIP_Application.Logging;
using IWshRuntimeLibrary;
using File = System.IO.File;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTM_WIP_Application.Services;

internal static class ShortcutManager
{
    public static void EnsureApplicationShortcut()
    {
        Debug.WriteLine("Ensuring application shortcut...");
        AppLogger.Log("Ensuring application shortcut...");

        try
        {
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var shortcutName = "MTM WIP App (Live).lnk";
            var shortcutPath = Path.Combine(desktopPath, shortcutName);
            var applicationPath = Application.ExecutablePath;

            Debug.WriteLine("Checking for existing shortcuts...");
            AppLogger.Log("Checking for existing shortcuts...");
            var existingShortcuts = Directory.GetFiles(desktopPath, "*.lnk")
                .Where(file => GetShortcutTarget(file) == applicationPath)
                .ToList();

            foreach (var shortcut in existingShortcuts)
                if (!shortcut.Equals(shortcutPath, StringComparison.OrdinalIgnoreCase))
                {
                    Debug.WriteLine($"Deleting existing shortcut: {shortcut}");
                    File.Delete(shortcut);
                    AppLogger.Log($"Deleted existing shortcut: {shortcut}");
                }

            if (!File.Exists(shortcutPath))
            {
                Debug.WriteLine($"Creating new shortcut: {shortcutPath}");
                CreateShortcut(shortcutPath, applicationPath, "MTM WIP Application");
                AppLogger.Log($"Created new shortcut: {shortcutPath}");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error ensuring application shortcut: {ex.Message}");
            AppLogger.Log($"Error ensuring application shortcut: {ex.Message}");
        }
    }

    private static void CreateShortcut(string shortcutPath, string targetPath, string description)
    {
        Debug.WriteLine($"Creating shortcut at {shortcutPath}...");
        AppLogger.Log($"Creating shortcut at {shortcutPath}...");

        try
        {
            var shell = new WshShell();
            var shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);
            shortcut.TargetPath = targetPath;
            shortcut.Description = description;
            shortcut.Save();

            Debug.WriteLine($"Shortcut created at {shortcutPath}");
            AppLogger.Log($"Shortcut created at {shortcutPath}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error creating shortcut: {ex.Message}");
            AppLogger.Log($"Error creating shortcut: {ex.Message}");
        }
    }

    private static string GetShortcutTarget(string shortcutPath)
    {
        Debug.WriteLine($"Getting shortcut target for {shortcutPath}...");
        AppLogger.Log($"Getting shortcut target for {shortcutPath}...");

        try
        {
            var shell = new WshShell();
            var shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);
            return shortcut.TargetPath;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error getting shortcut target: {ex.Message}");
            AppLogger.Log($"Error getting shortcut target: {ex.Message}");
            return string.Empty;
        }
    }
}