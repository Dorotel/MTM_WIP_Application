namespace MTM_WIP_Application.Models;

public class Model_UserBasedShortcutBar
{
    public int Id { get; set; } // Maps to ID Primary, int(11), NOT NULL, AUTO_INCREMENT
    public string Op { get; set; } = string.Empty; // Maps to Op, text, NOT NULL
    public string PartId { get; set; } = string.Empty; // Maps to PartID, varchar(1000), NOT NULL
    public int Quantity { get; set; } // Maps to Quantity, int(10), NOT NULL
}