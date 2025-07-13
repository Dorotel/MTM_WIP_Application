using System;

namespace MTM_Inventory_Application.Models;

internal class Model_CurrentInventory
{
    #region Properties

    public DateTime DateTime { get; set; } = DateTime.Now;
    public int Id { get; set; }
    public string ItemNumber { get; set; } = string.Empty;
    public string ItemType { get; set; } = "WIP";
    public string Location { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public string Op { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string User { get; set; } = string.Empty;

    #endregion
}