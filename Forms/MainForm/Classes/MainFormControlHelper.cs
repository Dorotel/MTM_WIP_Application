using System.Drawing;
using System.Windows.Forms;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Forms.MainForm.Classes;

/// <summary>
///     Provides helper methods for common control operations in the main form.
/// </summary>
public static class MainFormControlHelper
{
    public static void ClearAllTextBoxes(Control parent)
    {
        foreach (Control control in parent.Controls)
            if (control is TextBox textBox)
                textBox.Clear();
            else if (control.HasChildren) ClearAllTextBoxes(control);
    }

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

    public static void SetActiveControl(Form form, Control control)
    {
        if (form == null || control == null) return;
        if (form.InvokeRequired)
            form.Invoke(new MethodInvoker(() =>
            {
                form.ActiveControl = control;
                // Removed SelectAll/DeselectAll logic
            }));
        else
            form.ActiveControl = control;
        // Removed SelectAll/DeselectAll logic
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
        invalidColor ??= Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
        ;

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
        invalidColor ??= Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
        ;

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

    public static void ResetRichTextBox(RichTextBox richTextBox, Color color, string text)
    {
        if (richTextBox == null) return;
        if (richTextBox.InvokeRequired)
        {
            richTextBox.Invoke(new MethodInvoker(() =>
            {
                richTextBox.ForeColor = color;
                richTextBox.Text = text;
            }));
        }
        else
        {
            richTextBox.ForeColor = color;
            richTextBox.Text = text;
        }
    }
}