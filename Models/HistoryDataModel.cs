namespace MTM_WIP_Application.Models;

internal class HistoryInventory
{
    public int Id { get; set; } // Maps to ID Primary, int(11), NOT NULL, AUTO_INCREMENT
    public string User { get; set; } = string.Empty; // Maps to User, varchar(100), NOT NULL
    public string PartId { get; set; } = string.Empty; // Maps to Part ID, varchar(300), NOT NULL
    public string Location { get; set; } = string.Empty; // Maps to Location, varchar(100), NOT NULL
    public string Type { get; set; } = string.Empty; // Maps to Type, varchar(1000), NOT NULL
    public int Quantity { get; set; } // Maps to Quantity, int(100), NOT NULL
    public DateTime Time { get; set; } = DateTime.Now; // Maps to Time, timestamp, NOT NULL, default CURRENT_TIMESTAMP
}

internal class HistoryRemove
{
    public int Id { get; set; } // Maps to ID Primary, int(11), NOT NULL, AUTO_INCREMENT
    public string User { get; set; } = string.Empty; // Maps to User, varchar(100), NOT NULL
    public string PartId { get; set; } = string.Empty; // Maps to Part ID, varchar(300), NOT NULL
    public string Location { get; set; } = string.Empty; // Maps to Location, varchar(100), NOT NULL
    public string Type { get; set; } = string.Empty; // Maps to Type, varchar(1000), NOT NULL
    public int Quantity { get; set; } // Maps to Quantity, int(100), NOT NULL
    public DateTime Time { get; set; } = DateTime.Now; // Maps to Time, timestamp, NOT NULL, default CURRENT_TIMESTAMP
}

internal class HistoryTransfer
{
    public int Id { get; set; } // Maps to Id, INT(11), NOT NULL, AUTO_INCREMENT
    public string OldLocation { get; set; } = string.Empty; // Maps to OldLocation, VARCHAR(100), NOT NULL
    public string NewLocation { get; set; } = string.Empty; // Maps to NewLocation, VARCHAR(100), NOT NULL
    public string PartId { get; set; } = string.Empty; // Maps to PartId, VARCHAR(300), NOT NULL
    public int Quantity { get; set; } // Maps to Quantity, INT(100), NOT NULL
    public DateTime Time { get; set; } = DateTime.Now; // Maps to Time, TIMESTAMP, NOT NULL, DEFAULT CURRENT_TIMESTAMP
    public string User { get; set; } = string.Empty; // Maps to User, VARCHAR(100), NOT NULL
    public string PartType { get; set; } = string.Empty; // Maps to PartType, VARCHAR(1000), NOT NULL
}