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
                if (comboBox.Items.Count > 0 && selectedIndex >= 0 && selectedIndex < comboBox.Items.Count)
                    comboBox.SelectedIndex = selectedIndex;
                else
                    comboBox.SelectedIndex = -1;
            }));
        }
        else
        {
            comboBox.ForeColor = color;
            if (comboBox.Items.Count > 0 && selectedIndex >= 0 && selectedIndex < comboBox.Items.Count)
                comboBox.SelectedIndex = selectedIndex;
            else
                comboBox.SelectedIndex = -1;
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
                    control.Focus();
            }));
        }
        else
        {
            form.ActiveControl = control;
            if (control is TextBoxBase textBoxBase)
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

    public static void AdjustQuantityByKey_Transfers(
        object? sender,
        KeyEventArgs e,
        string placeholder = "[ Enter Valid Quantity ]",
        Color? validColor = null,
        Color? invalidColor = null)
    {
        if (sender is not TextBox textBox)
            return;

        validColor ??= Color.Black;
        invalidColor ??= Color.Red;

        var isPlaceholder = textBox.Text.Trim() == placeholder;
        var isNumber = int.TryParse(textBox.Text.Trim(), out var value);

        void SetValueOrPlaceholder(int newValue)
        {
            if (newValue <= 0)
            {
                textBox.Text = placeholder;
                textBox.ForeColor = invalidColor.Value;
            }
            else
            {
                textBox.Text = newValue.ToString();
                textBox.ForeColor = validColor.Value;
                textBox.SelectionStart = textBox.Text.Length;
            }
        }

        if (isPlaceholder || isNumber)
        {
            var current = isNumber ? value : 0;
            int newValue;

            if (e.KeyCode == Keys.Left && !e.Shift)
            {
                newValue = current - 1;
                SetValueOrPlaceholder(newValue);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Right && !e.Shift)
            {
                newValue = current + 1;
                SetValueOrPlaceholder(newValue);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Up && !e.Shift)
            {
                newValue = current + 5;
                SetValueOrPlaceholder(newValue);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Down && !e.Shift)
            {
                newValue = current - 5;
                SetValueOrPlaceholder(newValue);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Left && e.Shift)
            {
                newValue = current - 10;
                SetValueOrPlaceholder(newValue);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Right && e.Shift)
            {
                newValue = current + 10;
                SetValueOrPlaceholder(newValue);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Up && e.Shift)
            {
                newValue = current + 50;
                SetValueOrPlaceholder(newValue);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Down && e.Shift)
            {
                newValue = current - 50;
                SetValueOrPlaceholder(newValue);
                e.Handled = true;
            }
        }
    }

    public static void AdjustQuantityByKey_Quantity(
        object? sender,
        KeyEventArgs e,
        string placeholder = "[ Enter Valid Quantity ]",
        Color? validColor = null,
        Color? invalidColor = null)
    {
        if (sender is not TextBox textBox)
            return;

        validColor ??= Color.Black;
        invalidColor ??= Color.Red;

        var isPlaceholder = textBox.Text.Trim() == placeholder;
        var isNumber = int.TryParse(textBox.Text.Trim(), out var value);

        void SetValueOrPlaceholder(int newValue)
        {
            if (newValue <= 0)
            {
                textBox.Text = placeholder;
                textBox.ForeColor = invalidColor.Value;
            }
            else
            {
                textBox.Text = newValue.ToString();
                textBox.ForeColor = validColor.Value;
                textBox.SelectionStart = textBox.Text.Length;
            }
        }

        if (isPlaceholder || isNumber)
        {
            var current = isNumber ? value : 0;
            int newValue;

            if (e.KeyCode == Keys.Left && !e.Shift)
            {
                newValue = current - 10;
                SetValueOrPlaceholder(newValue);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Right && !e.Shift)
            {
                newValue = current + 10;
                SetValueOrPlaceholder(newValue);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Up && !e.Shift)
            {
                newValue = current + 1000;
                SetValueOrPlaceholder(newValue);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Down && !e.Shift)
            {
                newValue = current - 1000;
                SetValueOrPlaceholder(newValue);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Left && e.Shift)
            {
                newValue = current - 10000;
                SetValueOrPlaceholder(newValue);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Right && e.Shift)
            {
                newValue = current + 10000;
                SetValueOrPlaceholder(newValue);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Up && e.Shift)
            {
                newValue = current + 100;
                SetValueOrPlaceholder(newValue);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Down && e.Shift)
            {
                newValue = current - 100;
                SetValueOrPlaceholder(newValue);
                e.Handled = true;
            }
        }
    }
}