namespace MTM_WIP_Application.Models;

internal class Model_HistoryRemove
{
    public int Id { get; set; } // Maps to ID Primary, int(11), NOT NULL, AUTO_INCREMENT
    public string PartId { get; set; } = string.Empty; // Maps to PartID, varchar(300), NOT NULL
    public string Location { get; set; } = string.Empty; // Maps to Location, varchar(100), NOT NULL
    public string Operation { get; set; } = string.Empty; // Maps to Operation, varchar(100), NULL
    public int Quantity { get; set; } // Maps to Quantity, int(11), NOT NULL
    public string ItemType { get; set; } = "WIP"; // Maps to ItemType, varchar(100), NOT NULL, default WIP

    public DateTime ReceiveDate { get; set; } =
        DateTime.Now; // Maps to ReceiveDate, datetime, NOT NULL, default CURRENT_TIMESTAMP

    public DateTime LastUpdated { get; set; } =
        DateTime.Now; // Maps to LastUpdated, datetime, NOT NULL, on update CURRENT_TIMESTAMP

    public string User { get; set; } = string.Empty; // Maps to User, varchar(100), NOT NULL
    public string BatchNumber { get; set; } = string.Empty; // Maps to BatchNumber, varchar(6), NULL
    public string Notes { get; set; } = string.Empty; // Maps to Notes, varchar(1000), NULL
}