using System;
using System.IO;

namespace MTM_Inventory_Application.Data;

#region Dao_File

internal class Dao_File
{
    #region Fields

    // Add fields here if needed in the future

    #endregion

    #region Constructors

    // Add constructors here if needed in the future

    #endregion

    #region File Methods

    /// <summary>
    /// Writes the specified version string to a file named "version.txt" in the application's root directory.
    /// </summary>
    /// <param name="version">The version string to write.</param>
    public static void WriteVersionToRoot(string version)
    {
        try
        {
            // Get the application's root directory
            var rootPath = AppDomain.CurrentDomain.BaseDirectory;
            var versionFilePath = Path.Combine(rootPath, "version.txt");
            File.WriteAllText(versionFilePath, version);
        }
        catch (Exception ex)
        {
            // Optionally log the error using your logging utility
            Logging.LoggingUtility.LogApplicationError(ex);
        }
    }

    #endregion

    #region Helpers

    // Add helper methods here

    #endregion
}

#endregion