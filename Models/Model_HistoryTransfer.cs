// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace MTM_Inventory_Application.Models;

internal class Model_HistoryTransfer
{
    #region Properties

    public int Id { get; set; }
    public string NewLocation { get; set; } = string.Empty;
    public string OldLocation { get; set; } = string.Empty;
    public string PartId { get; set; } = string.Empty;
    public string PartType { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public DateTime Time { get; set; } = DateTime.Now;
    public string User { get; set; } = string.Empty;

    #endregion
}