namespace MTM_WIP_Application.Forms.MainForm.Classes;

/// <summary>
///     Provides helper methods for common control operations in the main form.
/// </summary>
public static class MainFormControlHelper
{
    /// <summary>
    ///     Recursively clears all TextBox controls within a parent control.
    /// </summary>
    public static void ClearAllTextBoxes(Control parent)
    {
        foreach (Control control in parent.Controls)
            if (control is TextBox textBox)
                textBox.Clear();
            else if (control.HasChildren) ClearAllTextBoxes(control);
    }

    /// <summary>
    ///     Clears the text of all TextBox controls in the provided collection.
    /// </summary>
    public static void ClearTextBoxes(params Control[] controls)
    {
        foreach (var control in controls)
            if (control is TextBox textBox)
            {
                if (textBox.InvokeRequired)
                    textBox.Invoke(new MethodInvoker(() => textBox.Clear()));
                else
                    textBox.Clear();
            }
    }

    /// <summary>
    ///     Enables the specified ComboBoxes by setting their Enabled property to true.
    /// </summary>
    public static void EnableComboBoxes(params ComboBox[] comboBoxes)
    {
        if (comboBoxes == null) return;
        foreach (var comboBox in comboBoxes)
        {
            if (comboBox == null) continue;
            if (comboBox.InvokeRequired)
                comboBox.Invoke(new MethodInvoker(() => comboBox.Enabled = true));
            else
                comboBox.Enabled = true;
        }
    }

    /// <summary>
    ///     Resets the specified ComboBox to its default state by setting its foreground color and selected index.
    /// </summary>
    public static void ResetComboBox(ComboBox comboBox, Color color, int selectedIndex)
    {
        if (comboBox == null) return;
        if (comboBox.InvokeRequired)
        {
            comboBox.Invoke(new MethodInvoker(() =>
            {
                comboBox.ForeColor = color;
                comboBox.SelectedIndex = selectedIndex;
            }));
        }
        else
        {
            comboBox.ForeColor = color;
            comboBox.SelectedIndex = selectedIndex;
        }
    }

    /// <summary>
    ///     Resets the specified TextBox to its default state by setting its foreground color and text.
    /// </summary>
    public static void ResetTextBox(TextBox textBox, Color color, string text)
    {
        if (textBox == null) return;
        if (textBox.InvokeRequired)
        {
            textBox.Invoke(new MethodInvoker(() =>
            {
                textBox.ForeColor = color;
                textBox.Text = text;
            }));
        }
        else
        {
            textBox.ForeColor = color;
            textBox.Text = text;
        }
    }

    /// <summary>
    ///     Sets the specified control as the active control and focuses on it.
    ///     If the control is a TextBoxBase, it selects all text within the control.
    /// </summary>
    public static void SetActiveControl(Form form, Control control)
    {
        if (form == null || control == null) return;
        if (form.InvokeRequired)
        {
            form.Invoke(new MethodInvoker(() =>
            {
                form.ActiveControl = control;
                if (control is TextBoxBase textBoxBase)
                    textBoxBase.SelectAll();
                control.Focus();
            }));
        }
        else
        {
            form.ActiveControl = control;
            if (control is TextBoxBase textBoxBase)
                textBoxBase.SelectAll();
            control.Focus();
        }
    }

    /// <summary>
    ///     Enables or disables a set of controls.
    /// </summary>
    public static void SetControlsEnabled(bool enabled, params Control[] controls)
    {
        foreach (var control in controls)
            if (control.InvokeRequired)
                control.Invoke(new MethodInvoker(() => control.Enabled = enabled));
            else
                control.Enabled = enabled;
    }

    /// <summary>
    ///     Sets focus to the specified control.
    /// </summary>
    public static void SetFocus(Control control)
    {
        if (control.InvokeRequired)
            control.Invoke(new MethodInvoker(() => { control.Focus(); }));
        else
            control.Focus();
    }
}