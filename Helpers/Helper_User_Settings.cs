using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Forms.Changelog;

namespace MTM_Inventory_Application.Helpers;

/// <summary>
///     Provides helper methods for loading, saving, and displaying user settings and changelog.
/// </summary>
public static class Helper_User_Settings
{
    /// <summary>
    ///     Loads user settings on form load, including the last shown version of the changelog.
    /// </summary>
    public static async Task LoadUserSettingsAsync()
    {
        // Load last shown changelog version
        var lastShownVersion = await Dao_User.GetLastShownVersionAsync(Core_WipAppVariables.User);
        if (lastShownVersion != Core_WipAppVariables.Version)
        {
            await Dao_User.SetHideChangeLogAsync(Core_WipAppVariables.User, "false");
            if (Core_WipAppVariables.Version != null)
                await Dao_User.SetLastShownVersionAsync(Core_WipAppVariables.User, Core_WipAppVariables.Version);
        }

        // Load user settings from DB and assign to Core_WipAppVariables
        Core_WipAppVariables.WipServerAddress = await Dao_User.GetWipServerAddressAsync(Core_WipAppVariables.User);
        Core_WipAppVariables.WipServerPort = await Dao_User.GetWipServerPortAsync(Core_WipAppVariables.User);
        Core_WipAppVariables.VisualUserName = await Dao_User.GetVisualUserNameAsync(Core_WipAppVariables.User);
        Core_WipAppVariables.VisualPassword = await Dao_User.GetVisualPasswordAsync(Core_WipAppVariables.User);
        Core_WipAppVariables.WipDataGridTheme = await Dao_User.GetThemeNameAsync(Core_WipAppVariables.User);
        Core_WipAppVariables.UserShift = null; // Set elsewhere if needed

        var fontSize = await Dao_User.GetThemeFontSizeAsync(Core_WipAppVariables.User);
        Core_WipAppVariables.ThemeFontSize = fontSize ?? 9;
    }

    /// <summary>
    ///     Saves user settings when the form is closing.
    /// </summary>
    public static async Task SaveUserSettingsAsync()
    {
        // Save user settings from Core_WipAppVariables to DB using stored procedures
        await Dao_User.SetWipServerAddressAsync(Core_WipAppVariables.User, Core_WipAppVariables.WipServerAddress ?? "");
        await Dao_User.SetWipServerPortAsync(Core_WipAppVariables.User, Core_WipAppVariables.WipServerPort ?? "");
        await Dao_User.SetThemeNameAsync(Core_WipAppVariables.User, Core_WipAppVariables.WipDataGridTheme ?? "");
        await Dao_User.SetThemeFontSizeAsync(Core_WipAppVariables.User, (int)Core_WipAppVariables.ThemeFontSize);
        await Dao_User.SetVisualUserNameAsync(Core_WipAppVariables.User, Core_WipAppVariables.VisualUserName ?? "");
        await Dao_User.SetVisualPasswordAsync(Core_WipAppVariables.User, Core_WipAppVariables.VisualPassword ?? "");
    }

    /// <summary>
    ///     Shows the changelog on startup if the changelog visibility toggle is set to false for the current user.
    /// </summary>
    public static async Task ShowChangeLogIfNeededAsync(Form mainForm)
    {
        var show = await Dao_User.GetHideChangeLogAsync(Core_WipAppVariables.User);
        if (show == "false")
        {
            var change = new ChangeLogForm();
            mainForm.Enabled = false;
            change.FormClosed += (_, _) => mainForm.Enabled = true;
            change.Show();
        }
    }
}