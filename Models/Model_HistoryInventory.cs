

namespace MTM_Inventory_Application.Models;

internal class Model_HistoryInventory
{
    #region Properties

    public int Id { get; set; }
    public string Location { get; set; } = string.Empty;
    public string PartId { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public DateTime Time { get; set; } = DateTime.Now;
    public string Type { get; set; } = string.Empty;
    public string User { get; set; } = string.Empty;

    #endregion
}