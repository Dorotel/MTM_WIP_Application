

namespace MTM_Inventory_Application.Models;

public class Model_UserBasedShortcutBar
{
    #region Properties

    public int Id { get; set; }
    public string Op { get; set; } = string.Empty;
    public string PartId { get; set; } = string.Empty;
    public int Quantity { get; set; }

    #endregion
}