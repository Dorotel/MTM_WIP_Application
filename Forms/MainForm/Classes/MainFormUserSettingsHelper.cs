using System.Diagnostics;
using System.Threading.Tasks;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Forms.MainForm.Classes;

/// <summary>
///     Provides helper methods for loading, saving, and displaying user colors and changelog.
/// </summary>
public static class MainFormUserSettingsHelper
{
    /// <summary>
    ///     Loads user colors on form load, including the last shown version of the changelog.
    /// </summary>
    public static async Task LoadUserSettingsAsync()
    {
        Debug.WriteLine("[DEBUG] Loading user theme settings from DB");
        // Load last shown changelog version
        var lastShownVersion = await Dao_User.GetLastShownVersionAsync(Model_AppVariables.User);
        if (lastShownVersion != Model_AppVariables.Version)
        {
            await Dao_User.SetHideChangeLogAsync(Model_AppVariables.User, "false");
            if (Model_AppVariables.Version != null)
                await Dao_User.SetLastShownVersionAsync(Model_AppVariables.User, Model_AppVariables.Version);
        }

        // Load user colors from DB and assign to Model_AppVariables
        Model_AppVariables.WipServerAddress = await Dao_User.GetWipServerAddressAsync(Model_AppVariables.User);
        Model_AppVariables.WipServerPort = await Dao_User.GetWipServerPortAsync(Model_AppVariables.User);
        Model_AppVariables.VisualUserName = await Dao_User.GetVisualUserNameAsync(Model_AppVariables.User);
        Model_AppVariables.VisualPassword = await Dao_User.GetVisualPasswordAsync(Model_AppVariables.User);
        Model_AppVariables.WipDataGridTheme = await Dao_User.GetThemeNameAsync(Model_AppVariables.User);

        // CRITICAL FIX: Synchronize ThemeName with WipDataGridTheme
        Model_AppVariables.WipDataGridTheme = Model_AppVariables.ThemeName;

        Model_AppVariables.UserShift = null; // Set elsewhere if needed

        var fontSize = await Dao_User.GetThemeFontSizeAsync(Model_AppVariables.User);
        Model_AppVariables.ThemeFontSize = fontSize ?? 9;
        Debug.WriteLine("[DEBUG] Finished loading user theme settings from DB");
    }
}