# File-by-File Refactor Plan for MySQL DELETE Operations

## General Workflow for All DELETE Buttons
1. Gather Data to Send to DAO: Collect all necessary fields from the UI (e.g., selected rows, IDs, user, etc.).
2. Send Data to DAO: Pass the gathered data to the appropriate DAO method.
3. DAO - Validate Data Exists on Server: Before deletion, check if the data exists in the database. If not, return an error.
4. Call Stored Procedure: If validation passes, execute the stored procedure to perform the deletion.
5. Validate Data Was Removed: After deletion, check if the data was actually removed. If not, return an error.
6. Add to inv_transactions (if applicable): If the deletion is inventory-related, log the transaction in the inv_transactions table.

---

## Controls/MainForm/Control_RemoveTab.cs
- **Why**: This file contains the main inventory removal tab, including the delete button logic.
- **What to Refactor**:
  - Refactor `Control_RemoveTab_Button_Delete_Click` to:
    - Gather all selected rows and extract required fields.
    - For each row, call a new DAO method that:
      - Validates the item exists in the database.
      - Calls the stored procedure to delete.
      - Validates the item was deleted.
      - If successful, logs the transaction in `inv_transactions`.
    - Collect and display errors for any failed deletions.
    - Only add to `_lastRemovedItems` and enable Undo if the deletion succeeded.
- **Why Each Step**:
  - Ensures UI and DB are always in sync.
  - Prevents "phantom deletes" (UI thinks it's gone, DB does not).
  - Provides user feedback for partial failures.

---

## Controls/MainForm/Control_AdvancedRemove.cs
- **Why**: This file handles advanced inventory removal, including multi-row deletes.
- **What to Refactor**:
  - Refactor `Control_AdvancedRemove_Button_Delete_Click` to:
    - Gather all selected rows and extract required fields.
    - For each row, call the new DAO method (as above).
    - Collect and display errors for any failed deletions.
    - Only add to `_lastRemovedItems` and enable Undo if the deletion succeeded.
- **Why Each Step**:
  - Same as above, but for advanced/filtered removals.

---

## Data/Dao_Inventory.cs
- **Why**: This is the main DAO for inventory operations.
- **What to Refactor**:
  - Add a new method (e.g., `DeleteInventoryItemWithValidationAsync`) that:
    1. Validates the item exists (e.g., by querying with all key fields).
    2. Calls the stored procedure to delete.
    3. Validates the item was deleted (e.g., by re-querying).
    4. If successful, logs the transaction in `inv_transactions` via `Dao_History`.
    5. Returns a status and error message.
  - Refactor `RemoveInventoryItemsFromDataGridViewAsync` to use this new method for each row.
- **Why Each Step**:
  - Centralizes all validation and logging logic.
  - Ensures all UI code uses the same robust workflow.

---

## Data/Dao_History.cs
- **Why**: Handles logging to `inv_transactions`.
- **What to Refactor**:
  - Ensure `AddTransactionHistoryAsync` is called for every successful inventory deletion.
  - If not already, ensure all required fields are logged (type, part, location, operation, quantity, user, batch, date).
- **Why Each Step**:
  - Maintains a complete audit trail for all inventory removals.

---

## Controls/MainForm/Control_InventoryTab.cs, Control_TransferTab.cs, Control_QuickButtons.cs
- **Why**: These may have delete or remove actions (e.g., transfer out, quick remove).
- **What to Refactor**:
  - Audit for any button that results in a DELETE or inventory removal.
  - Refactor to use the new DAO workflow as above.
- **Why Each Step**:
  - Ensures all inventory removals, regardless of UI entry point, follow the same robust process.

---

## Data/Dao_Part.cs, Dao_Operation.cs, Dao_Location.cs, Dao_ItemType.cs
- **Why**: These DAOs handle deletes for master data (parts, operations, locations, item types).
- **What to Refactor**:
  - For each `Delete*` method:
    1. Validate the item exists before deletion.
    2. Call the stored procedure or SQL to delete.
    3. Validate the item was deleted.
    4. Return a status and error message.
  - (No need to log to `inv_transactions` unless inventory is affected.)
- **Why Each Step**:
  - Prevents accidental deletes of non-existent data.
  - Provides clear error feedback to the UI.

---

## UI Feedback and Error Handling
- **Why**: Users need to know if a delete failed and why.
- **What to Refactor**:
  - All delete buttons should display a summary of what was deleted and what failed (with reasons).
  - If nothing was deleted, show a clear message.
- **Why Each Step**:
  - Improves user trust and reduces confusion.

---

## Testing and Validation
- **Why**: To ensure the new workflow is robust.
- **What to Do**:
  - Add/expand unit tests for DAO delete methods.
  - Manually test all delete buttons for:
    - Success (item is deleted, transaction is logged).
    - Failure (item does not exist, error is shown).
    - Partial success (some items deleted, some not).
- **Why Each Step**:
  - Ensures the refactor meets requirements and is reliable.

---

## Next Steps

1. Implement the above changes file by file.
2. Test each UI and DAO delete workflow.
3. Document all changes and update user documentation as needed.
