// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace MTM_Inventory_Application.Models;

internal class Model_VersionHistory
{
    #region Properties

    public int Id { get; set; }
    public string Notes { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;

    #endregion
}