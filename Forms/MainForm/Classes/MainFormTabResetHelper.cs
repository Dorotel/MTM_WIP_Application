namespace MTM_Inventory_Application.Forms.MainForm.Classes;

/// <summary>
///     Provides helper methods for resetting the controls on each MainForm tab.
/// </summary>
public static class MainFormTabResetHelper
{
    public static void ResetRemoveTab(
        ComboBox comboBoxPart,
        ComboBox comboBoxOp,
        Button buttonSearch,
        Button buttonDelete)
    {
    }

    public static void ResetTransferTab(
        ComboBox comboBoxPart,
        ComboBox comboBoxOp,
        Button buttonSearch,
        Button buttonDelete)
    {
        MainFormControlHelper.ResetComboBox(comboBoxPart, Color.Red, 0);
        MainFormControlHelper.ResetComboBox(comboBoxOp, Color.Red, 0);

        buttonSearch.Enabled = false;
        buttonDelete.Enabled = false;

        if (comboBoxPart.FindForm() is { } form)
            MainFormControlHelper.SetActiveControl(form, comboBoxPart);
    }
}