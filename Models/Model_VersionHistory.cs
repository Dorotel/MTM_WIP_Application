

namespace MTM_Inventory_Application.Models;

internal class Model_VersionHistory
{
    #region Properties

    public int Id { get; set; }
    public string Notes { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;

    #endregion
}