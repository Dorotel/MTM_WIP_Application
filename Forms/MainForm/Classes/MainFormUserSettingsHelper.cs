using MTM_WIP_Application.Core;
using MTM_WIP_Application.Data;
using MTM_WIP_Application.Forms.Changelog;
using System.Data;
using System.Windows.Forms;

namespace MTM_WIP_Application.Forms.MainForm.Classes;

/// <summary>
///     Provides helper methods for loading, saving, and displaying user settings and changelog.
/// </summary>
public static class MainFormUserSettingsHelper
{
    /// <summary>
    ///     Loads user settings on form load, including the last shown version of the changelog.
    /// </summary>
    public static async Task LoadUserSettingsAsync()
    {
        // Load last shown changelog version
        var lastShownVersion = await UserDao.GetLastShownVersionAsync(WipAppVariables.User);
        if (lastShownVersion != WipAppVariables.Version)
        {
            await UserDao.SetHideChangeLogAsync(WipAppVariables.User, "false");
            if (WipAppVariables.Version != null)
                await UserDao.SetLastShownVersionAsync(WipAppVariables.User, WipAppVariables.Version);
        }

        // Load user settings from DB and assign to WipAppVariables
        WipAppVariables.WipServerAddress = await UserDao.GetWipServerAddressAsync(WipAppVariables.User);
        WipAppVariables.WipServerPort = await UserDao.GetWipServerPortAsync(WipAppVariables.User);
        WipAppVariables.VisualUserName = await UserDao.GetVisualUserNameAsync(WipAppVariables.User);
        WipAppVariables.VisualPassword = await UserDao.GetVisualPasswordAsync(WipAppVariables.User);
        WipAppVariables.WipDataGridTheme = await UserDao.GetThemeNameAsync(WipAppVariables.User);
        WipAppVariables.UserShift = null; // Set elsewhere if needed

        var fontSize = await UserDao.GetThemeFontSizeAsync(WipAppVariables.User);
        WipAppVariables.ThemeFontSize = fontSize ?? 9;
    }

    /// <summary>
    ///     Saves user settings when the form is closing.
    /// </summary>
    public static async Task SaveUserSettingsAsync()
    {
        // Save user settings from WipAppVariables to DB using stored procedures
        await UserDao.SetWipServerAddressAsync(WipAppVariables.User, WipAppVariables.WipServerAddress ?? "");
        await UserDao.SetWipServerPortAsync(WipAppVariables.User, WipAppVariables.WipServerPort ?? "");
        await UserDao.SetThemeNameAsync(WipAppVariables.User, WipAppVariables.WipDataGridTheme ?? "");
        await UserDao.SetThemeFontSizeAsync(WipAppVariables.User, (int)WipAppVariables.ThemeFontSize);
        await UserDao.SetVisualUserNameAsync(WipAppVariables.User, WipAppVariables.VisualUserName ?? "");
        await UserDao.SetVisualPasswordAsync(WipAppVariables.User, WipAppVariables.VisualPassword ?? "");
    }

    /// <summary>
    ///     Shows the changelog on startup if the changelog visibility toggle is set to false for the current user.
    /// </summary>
    public static async Task ShowChangeLogIfNeededAsync(Form mainForm)
    {
        var show = await UserDao.GetHideChangeLogAsync(WipAppVariables.User);
        if (show == "false")
        {
            var change = new ChangeLogForm();
            mainForm.Enabled = false;
            change.FormClosed += (_, _) => mainForm.Enabled = true;
            change.Show();
        }
    }
}