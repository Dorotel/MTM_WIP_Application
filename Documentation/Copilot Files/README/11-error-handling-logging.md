# 11. Error Handling and Logging

Enhanced Async Method Pattern with DaoResult
```csharp
private async Task StandardAsyncMethodWithDaoResult()
{
    try
    {
        _progressHelper?.ShowProgress();
        _progressHelper?.UpdateProgress(10, "Starting operation...");
        LoggingUtility.Log("Operation started.");
        var daoResult = await Dao_[Entity].SomeOperationAsync(parameters);
        if (daoResult.IsSuccess)
        {
            var data = daoResult.Data;
            _progressHelper?.ShowSuccess($"Operation completed: {daoResult.StatusMessage}");
        }
        else
        {
            LoggingUtility.Log($"Operation failed: {daoResult.ErrorMessage}");
            _progressHelper?.ShowError($"Error: {daoResult.ErrorMessage}");
            MessageBox.Show($"Operation failed: {daoResult.ErrorMessage}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
    }
    catch (Exception ex)
    {
        LoggingUtility.LogApplicationError(ex);
        _progressHelper?.ShowError($"Unexpected error: {ex.Message}");
        await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true, nameof(StandardAsyncMethodWithDaoResult));
    }
    finally
    {
        _progressHelper?.HideProgress();
    }
}
```

Logging Standards
```csharp
LoggingUtility.Log("Normal operation message");
LoggingUtility.LogApplicationError(exception);
LoggingUtility.LogDatabaseError(exception);
```