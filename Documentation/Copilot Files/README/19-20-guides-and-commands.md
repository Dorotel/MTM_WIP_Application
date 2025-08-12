# 19–20. Quick Reference Commands and File-Specific Guidance

19. Copilot Prompt Examples
- “Create a method to call stored procedure inv_inventory_Add_Item using Helper_Database_StoredProcedure pattern with DaoResult<T> handling...”
- “Create a new UserControl following the Control_[TabName] pattern with progress controls...”
- “Add enhanced async error handling with DaoResult<T> pattern...”

20. File-Specific Guidance
Program.cs
- Keep global exception handlers
- Maintain startup order
- Keep theme initialization order

MainForm.cs
- Set MainFormInstance on all UserControls
- Handle DPI changes properly
- Maintain StatusStrip progress

DAOs
- Always return DaoResult<T>
- Use Helper_Database_StoredProcedure exclusively
- Never return null data (use empty DataTable/collections)

Controls
- Progress reporting for async DB calls
- Null safety on DataTable operations
- Consistent error messaging