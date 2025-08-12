# 4, 6, 10. Key Patterns and Templates

4. Enhanced DAO Pattern with DaoResult<T>
```csharp
public static async Task<DaoResult<DataTable>> GetInventoryByPartIdAsync(string partId, bool useAsync = false)
{
    try
    {
        DataTable result = await HelperDatabaseCore.ExecuteDataTable(
            "inv_inventory_Get_ByPartID",
            new Dictionary<string, object> { { "p_PartID", partId } },
            useAsync, CommandType.StoredProcedure);
        return DaoResult<DataTable>.Success(result, $"Retrieved {result.Rows.Count} inventory items for part {partId}");
    }
    catch (Exception ex)
    {
        LoggingUtility.LogDatabaseError(ex);
        return DaoResult<DataTable>.Failure($"Failed to retrieve inventory for part {partId}", ex);
    }
}
```

Database Access Pattern (Stored Procedures Only)
```csharp
var result = await Helper_Database_StoredProcedure.ExecuteNonQueryWithStatus(
    Model_AppVariables.ConnectionString,
    "stored_procedure_name",
    new Dictionary<string, object>
    {
        ["Parameter1"] = value1, // no p_ prefix in C#
        ["Parameter2"] = value2,
        ["User"] = Model_AppVariables.User,
        ["DateTime"] = DateTime.Now
    },
    _progressHelper,
    true
);
```

6. Standard UserControl Structure (Template)
```csharp
public partial class Control_[TabName] : UserControl
{
    private Helper_StoredProcedureProgress? _progressHelper;

    public Control_[TabName]()
    {
        InitializeComponent();
        Core_Themes.ApplyDpiScaling(this);
        Core_Themes.ApplyRuntimeLayoutAdjustments(this);
        Initialize();
        ApplyPrivileges();
        SetupTooltips();
        SetupInitialColors();
        WireUpEvents();
        LoadInitialData();
    }

    public void SetProgressControls(ToolStripProgressBar progressBar, ToolStripStatusLabel statusLabel)
    {
        _progressHelper = Helper_StoredProcedureProgress.Create(progressBar, statusLabel,
            this.FindForm() ?? throw new InvalidOperationException("Control must be added to a form"));
    }
}
```

10. UI Patterns and Standards
- DataGridView setup with null safety
- ComboBox validation and event wiring
- Reset pattern (Soft vs Hard)
```csharp
private void SetupDataGridView(DataGridView dgv, DataTable? data)
{
    data ??= new DataTable();
    dgv.SuspendLayout();
    dgv.DataSource = data;
    string[] columnsToShow = { "Location", "PartID", "Operation", "Quantity", "Notes" };
    var visible = new HashSet<string>(columnsToShow);
    foreach (DataGridViewColumn c in dgv.Columns) c.Visible = visible.Contains(c.Name);
    for (int i=0;i<columnsToShow.Length;i++) if (dgv.Columns.Contains(columnsToShow[i])) dgv.Columns[columnsToShow[i]].DisplayIndex = i;
    if (data.Rows.Count > 0)
    {
        Core_Themes.ApplyThemeToDataGridView(dgv);
        Core_Themes.SizeDataGrid(dgv);
        dgv.ClearSelection();
        if (dgv.Rows.Count > 0) dgv.Rows[0].Selected = true;
    }
    dgv.ResumeLayout();
}
```