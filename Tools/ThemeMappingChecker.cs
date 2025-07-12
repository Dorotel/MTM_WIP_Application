using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms; // Add this for MessageBox
using MTM_Inventory_Application.Models;

internal class ThemeMappingChecker
{
    public static void RunCheck()
    {
        // Path to your theming file(s)
        var themeFilePath = @"C:\Users\johnk\source\repos\MTM_WIP_Application\Core\Core_Themes.cs";
        var outputFilePath = "ThemeMappingReport.txt";
        var success = false;

        try
        {
            using var writer = new StreamWriter(outputFilePath, false);

            if (!File.Exists(themeFilePath))
            {
                writer.WriteLine($"File not found: {themeFilePath}");
                MessageBox.Show($"File not found: {themeFilePath}", "Theme Mapping Check", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            var themeFileContent = File.ReadAllText(themeFilePath);

            // Get all public properties of Model_UserUiColors
            var colorProps = typeof(Model_UserUiColors)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(p => p.Name)
                .ToList();

            // Check if each property is referenced in the theming file
            var unmapped = colorProps
                .Where(prop => !themeFileContent.Contains(prop))
                .ToList();

            if (unmapped.Count == 0)
            {
                writer.WriteLine("All Model_UserUiColors properties are referenced in the theming logic.");
                success = true;
            }
            else
            {
                writer.WriteLine(
                    "The following Model_UserUiColors properties are NOT referenced in the theming logic:");
                foreach (var prop in unmapped)
                    writer.WriteLine($"  - {prop}");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Theme mapping check failed: {ex.Message}", "Theme Mapping Check", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            return;
        }

        if (success)
            MessageBox.Show(
                "All Model_UserUiColors properties are referenced in the theming logic.\nSee ThemeMappingReport.txt for details.",
                "Theme Mapping Check", MessageBoxButtons.OK, MessageBoxIcon.Information);
        else
            MessageBox.Show(
                "Some Model_UserUiColors properties are NOT referenced in the theming logic.\nSee ThemeMappingReport.txt for details.",
                "Theme Mapping Check", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
}