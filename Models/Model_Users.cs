namespace MTM_Inventory_Application.Models;

internal class Model_Users
{
    #region Properties

    public string FullName { get; set; } = string.Empty;
    public string HideChangeLog { get; set; } = "false";
    public int Id { get; set; } = 0;
    public string LastShownVersion { get; set; } = "0.0.0.0";
    public string Pin { get; set; } = "0000";
    public string Shift { get; set; } = string.Empty;
    public int ThemeFontSize { get; set; } = 9;
    public string ThemeName { get; set; } = "Default";
    public string User { get; set; } = string.Empty;
    public string VisualPassword { get; set; } = "Password";
    public string VisualUserName { get; set; } = "User Name";
    public bool VitsUser { get; set; } = false;
    public string WipServerAddress { get; set; } = "172.16.1.104";
    public string WipServerPort { get; set; } = "3306";

    #endregion
}