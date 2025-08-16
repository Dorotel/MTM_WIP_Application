namespace MTM_MAUI_Application.Models;

/// <summary>
/// Represents a user in the MTM WIP system
/// </summary>
public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Shift { get; set; } = string.Empty;
    public bool VitsUser { get; set; }
    public string Pin { get; set; } = string.Empty;
    public string LastShownVersion { get; set; } = string.Empty;
    public bool HideChangeLog { get; set; }
    public string ThemeName { get; set; } = "Default";
    public int ThemeFontSize { get; set; } = 12;
    public string VisualUserName { get; set; } = string.Empty;
    public string VisualPassword { get; set; } = string.Empty;
    public string WipServerAddress { get; set; } = string.Empty;
    public string WipDatabase { get; set; } = string.Empty;
    public string WipServerPort { get; set; } = "3306";
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime? ModifiedDate { get; set; }
}