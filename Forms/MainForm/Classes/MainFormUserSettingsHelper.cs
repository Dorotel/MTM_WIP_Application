using MTM_WIP_Application.Core;
using MTM_WIP_Application.Data;
using MTM_WIP_Application.Forms.Changelog;

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
        var lastShownVersion = await ChangeLogDao.Primary_ChangeLog_Get_LastShownAsync();
        if (lastShownVersion != WipAppVariables.Version)
        {
            await ChangeLogDao.Primary_ChangeLog_Set_SwitchAsync("false", WipAppVariables.User);
            if (WipAppVariables.Version != null)
                await ChangeLogDao.Primary_ChangeLog_Set_LastShownAsync(WipAppVariables.Version, WipAppVariables.User);
        }

        // Load user settings from DB and assign to WipAppVariables
        WipAppVariables.WipServerAddress = await ChangeLogDao.GetWipServerAddressAsync(WipAppVariables.User);
        WipAppVariables.WipServerPort = await ChangeLogDao.GetWipServerPortAsync(WipAppVariables.User);
        WipAppVariables.VisualUserName = await ChangeLogDao.Primary_ChangeLog_Get_Visual_UserAsync();
        WipAppVariables.VisualPassword = await ChangeLogDao.Primary_ChangeLog_Get_Visual_PasswordAsync();
        WipAppVariables.WipDataGridTheme = await ChangeLogDao.Primary_ChangeLog_Get_Theme_NameAsync();
        WipAppVariables.UserShift = null; // Set elsewhere if needed

        var fontSize = await ChangeLogDao.Primary_ChangeLog_Get_Visual_Theme_FontSizeAsync();
        WipAppVariables.ThemeFontSize = fontSize ?? 9;
    }

    /// <summary>
    ///     Saves user settings when the form is closing.
    /// </summary>
    public static async Task SaveUserSettingsAsync()
    {
        // Save user settings from WipAppVariables to DB
        await ChangeLogDao.SetWipServerAddressAsync(WipAppVariables.User, WipAppVariables.WipServerAddress ?? "");
        await ChangeLogDao.SetWipServerPortAsync(WipAppVariables.User, WipAppVariables.WipServerPort ?? "");
        await ChangeLogDao.Primary_ChangeLog_Set_Theme_NameAsync(WipAppVariables.WipDataGridTheme ?? "",
            WipAppVariables.User);
        await ChangeLogDao.Primary_ChangeLog_Set_Theme_FontSizeAsync(WipAppVariables.ThemeFontSize.ToString(),
            WipAppVariables.User);
        await ChangeLogDao.Primary_ChangeLog_Set_Visual_UserAsync(WipAppVariables.VisualUserName ?? "",
            WipAppVariables.User);
        await ChangeLogDao.Primary_ChangeLog_Set_Visual_PasswordAsync(WipAppVariables.VisualPassword ?? "",
            WipAppVariables.User);
    }

    /// <summary>
    ///     Shows the changelog on startup if the changelog visibility toggle is set to false for the current user.
    /// </summary>
    public static async Task ShowChangeLogIfNeededAsync(Form mainForm)
    {
        var show = await ChangeLogDao.Primary_ChangeLog_Get_ToggleAsync();
        if (show == "false")
        {
            var change = new ChangeLogForm();
            mainForm.Enabled = false;
            change.FormClosed += (_, _) => mainForm.Enabled = true;
            change.Show();
        }
    }
}