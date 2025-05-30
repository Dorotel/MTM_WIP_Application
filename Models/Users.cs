namespace MTM_WIP_Application.Models;

/// <summary>
///     Represents a user in the MTM WIP Application, mapping to the users table and related user settings.
/// </summary>
internal class Users
{
    public string FullName { get; set; } = string.Empty; // Maps to Full Name, varchar(100), NOT NULL

    public string HideChangeLog { get; set; } =
        "false"; // Maps to HideChangeLog, varchar(50), NOT NULL, default 'false'

    public int Id { get; set; } = 0; // Maps to ID Primary, int, NOT NULL, AUTO_INCREMENT

    public string LastShownVersion { get; set; } =
        "0.0.0.0"; // Maps to LastShownVersion, varchar(50), NOT NULL, default '0.0.0.0'

    public string Pin { get; set; } = "0000"; // Maps to Pin, varchar(4), NOT NULL, default '0000'
    public string Shift { get; set; } = string.Empty; // Maps to Shift, varchar(100), NOT NULL

    public int ThemeFontSize { get; set; } = 9; // Maps to Theme_FontSize, int, NOT NULL, default 9

    public string ThemeName { get; set; } =
        "Default (Black and White)"; // Maps to Theme_Name, varchar(50), NOT NULL, default 'Default (Black and White)'

    public string User { get; set; } = string.Empty; // Maps to User, varchar(100), NOT NULL

    public string VisualPassword { get; set; } =
        "Password"; // Maps to VisualPassword, varchar(50), NOT NULL, default 'Password'

    public string VisualUserName { get; set; } =
        "User Name"; // Maps to VisualUserName, varchar(50), NOT NULL, default 'User Name'

    public bool VitsUser { get; set; } = false; // Maps to VitsUser, tinyint(1), NOT NULL, default 0

    public string WipServerAddress { get; set; } =
        "172.16.1.104"; // Maps to WipServerAddress, varchar(12), NOT NULL, default '172.16.1.104'

    public string WipServerPort { get; set; } = "3306"; // Maps to WipServerPort, varchar(6), NOT NULL, default '3306'
}