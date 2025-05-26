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
        DataGridView? dataGrid,
        BindingSource blankBindingSource,
        ComboBox comboBoxPart,
        ComboBox comboBoxOp,
        ComboBox comboBoxShowAll,
        Button buttonSearch,
        Button buttonDelete,
        Button buttonPrint,
        Button buttonEdit,
        PictureBox imageNothingFound,
        ToolStripMenuItem menuStripFilePrint,
        ToolStripMenuItem menuStripFileSave,
        params ComboBox[] enableComboBoxes) // Removed 'menuStripView'
    {
        if (dataGrid != null)
        {
            dataGrid.DataSource = blankBindingSource;
            dataGrid.Update();
        }

        MainFormControlHelper.ResetComboBox(comboBoxPart, Color.Red, 0);
        MainFormControlHelper.ResetComboBox(comboBoxOp, Color.Red, 0);
        MainFormControlHelper.ResetComboBox(comboBoxShowAll, Color.Red, 0);

        buttonSearch.Enabled = false;
        buttonDelete.Enabled = false;
        buttonPrint.Enabled = false;
        if (buttonEdit != null) buttonEdit.Visible = false;
        if (imageNothingFound != null) imageNothingFound.Visible = false;

        menuStripFilePrint.Enabled = false;
        menuStripFileSave.Enabled = false;

        MainFormControlHelper.EnableComboBoxes(enableComboBoxes);

        if (comboBoxPart != null && comboBoxPart.FindForm() is Form form)
            MainFormControlHelper.SetActiveControl(form, comboBoxPart);
    }

    public static void ResetTransferTab(
        ComboBox comboBoxPart,
        ComboBox comboBoxLoc,
        DataGridView? dataGrid,
        BindingSource blankBindingSource,
        Button? buttonSearch,
        Button? buttonSave,
        ToolStripMenuItem? menuStripFileSave,
        TextBox? textBoxQty,
        PictureBox? imageNothing)
    {
        // Remove unnecessary assignment
        MainFormControlHelper.ResetComboBox(comboBoxPart, Color.Red, 0);
        MainFormControlHelper.ResetComboBox(comboBoxLoc, Color.Red, 0);

        if (dataGrid != null)
        {
            dataGrid.DataSource = blankBindingSource;
            dataGrid.Update();
        }

        comboBoxPart.Enabled = true;
        if (buttonSearch != null) buttonSearch.Enabled = false;
        if (buttonSave != null) buttonSave.Enabled = false;
        if (menuStripFileSave != null) menuStripFileSave.Enabled = false;

        if (textBoxQty != null)
        {
            textBoxQty.Clear();
            textBoxQty.Enabled = false;
        }

        if (imageNothing != null) imageNothing.Visible = false;

        // Remove unnecessary assignment
    }
}