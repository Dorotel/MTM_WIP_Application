// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace MTM_Inventory_Application.Models;

internal class Model_HistoryRemove
{
    #region Properties

    public int Id { get; set; }
    public string PartId { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Operation { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string ItemType { get; set; } = "WIP";
    public DateTime ReceiveDate { get; set; } = DateTime.Now;
    public DateTime LastUpdated { get; set; } = DateTime.Now;
    public string User { get; set; } = string.Empty;
    public string BatchNumber { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;

    #endregion
}
