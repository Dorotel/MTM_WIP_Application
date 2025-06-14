namespace MTM_WIP_Application.Models;

internal class Model_HistoryTransfer
{
    public int Id { get; set; } // Maps to Id, INT(11), NOT NULL, AUTO_INCREMENT
    public string NewLocation { get; set; } = string.Empty; // Maps to NewLocation, VARCHAR(100), NOT NULL
    public string OldLocation { get; set; } = string.Empty; // Maps to OldLocation, VARCHAR(100), NOT NULL
    public string PartId { get; set; } = string.Empty; // Maps to PartId, VARCHAR(300), NOT NULL
    public string PartType { get; set; } = string.Empty; // Maps to PartType, VARCHAR(1000), NOT NULL
    public int Quantity { get; set; } // Maps to Quantity, INT(100), NOT NULL
    public DateTime Time { get; set; } = DateTime.Now; // Maps to Time, TIMESTAMP, NOT NULL, DEFAULT CURRENT_TIMESTAMP
    public string User { get; set; } = string.Empty; // Maps to User, VARCHAR(100), NOT NULL
}