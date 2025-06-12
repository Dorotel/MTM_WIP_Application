using System.Data;
using MTM_WIP_Application.Data;
using MySql.Data.MySqlClient;

namespace MTM_WIP_Application.Forms.MainForm.Classes;

/// <summary>
///     Provides helper methods for resetting the controls on each MainForm tab.
/// </summary>
public static class MainFormTabResetHelper
{
    public static void ResetInventoryTab(
        ComboBox comboBoxLoc,
        ComboBox comboBoxOp,
        ComboBox comboBoxPart,
        CheckBox checkBoxMulti,
        CheckBox checkBoxMultiDifferent,
        TextBox? textBoxHowMany,
        TextBox textBoxQty,
        RichTextBox richTextBoxNotes,
        Button buttonSave,
        ToolStripMenuItem menuStripFileSave)
    {
        MainFormControlHelper.ResetComboBox(comboBoxLoc, Color.Red, 0);
        MainFormControlHelper.ResetComboBox(comboBoxOp, Color.Red, 0);
        MainFormControlHelper.ResetComboBox(comboBoxPart, Color.Red, 0);

        checkBoxMulti.Checked = false;
        checkBoxMultiDifferent.Checked = false;
        if (textBoxHowMany != null)
        {
            textBoxHowMany.Clear();
            textBoxHowMany.Enabled = false;
        }

        checkBoxMultiDifferent.Enabled = false;

        MainFormControlHelper.ResetTextBox(textBoxQty, Color.Red, "[ Enter Valid Quantity ]");
        richTextBoxNotes.Text = string.Empty;

        if (comboBoxPart.FindForm() is { } form)
            MainFormControlHelper.SetActiveControl(form, comboBoxPart);
        buttonSave.Enabled = false;
        menuStripFileSave.Enabled = false;
    }

    public static void ResetRemoveTab(
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