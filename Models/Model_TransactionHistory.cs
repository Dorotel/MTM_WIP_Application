// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace MTM_Inventory_Application.Models;

internal class Model_TransactionHistory
{
    #region Properties

    public int Id { get; set; }
    public string TransactionType { get; set; } = string.Empty;
    public string PartId { get; set; } = string.Empty;
    public string? FromLocation { get; set; }
    public string? ToLocation { get; set; }
    public string? Operation { get; set; }
    public int Quantity { get; set; }
    public string? Notes { get; set; }
    public string User { get; set; } = string.Empty;
    public string? ItemType { get; set; }
    public string? BatchNumber { get; set; }
    public DateTime DateTime { get; set; } = DateTime.Now;

    #endregion
}