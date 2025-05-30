namespace MTM_WIP_Application.Models;

internal class VersionHistory
{
    public int Id { get; set; } // Maps to ID Primary, int(11), NOT NULL, AUTO_INCREMENT
    public string Notes { get; set; } = string.Empty; // Maps to Notes, longtext, NOT NULL
    public string Version { get; set; } = string.Empty; // Maps to Version, varchar(50), NOT NULL
}