namespace MTM_WIP_Application.Models;

internal class HistoryRemove
{
    public int Id { get; set; } // Maps to ID Primary, int(11), NOT NULL, AUTO_INCREMENT
    public string Location { get; set; } = string.Empty; // Maps to Location, varchar(100), NOT NULL
    public string PartId { get; set; } = string.Empty; // Maps to Part ID, varchar(300), NOT NULL
    public int Quantity { get; set; } // Maps to Quantity, int(100), NOT NULL
    public DateTime Time { get; set; } = DateTime.Now; // Maps to Time, timestamp, NOT NULL, default CURRENT_TIMESTAMP
    public string Type { get; set; } = string.Empty; // Maps to Type, varchar(1000), NOT NULL
    public string User { get; set; } = string.Empty; // Maps to User, varchar(100), NOT NULL
}