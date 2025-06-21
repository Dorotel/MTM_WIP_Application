using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MTM_WIP_Application.Core;
using MTM_WIP_Application.Helpers;
using MySql.Data.MySqlClient;

namespace MTM_WIP_Application.Controls.MainForm;

public partial class Control_QuickButtons : UserControl
{
    private static List<Button>? quickButtons;

    public static Forms.MainForm.MainForm? MainFormInstance { get; set; }

    public Control_QuickButtons()
    {
        InitializeComponent();
        quickButtons =
        [
            Control_QuickButtons_Button_Button1, Control_QuickButtons_Button_Button2,
            Control_QuickButtons_Button_Button3, Control_QuickButtons_Button_Button4,
            Control_QuickButtons_Button_Button5,
            Control_QuickButtons_Button_Button6, Control_QuickButtons_Button_Button7,
            Control_QuickButtons_Button_Button8, Control_QuickButtons_Button_Button9,
            Control_QuickButtons_Button_Button10
        ];
        LoadLast10Transactions(Core_WipAppVariables.User);
        foreach (var btn in quickButtons)
            btn.Click += QuickButton_Click;
        LoadLast10Transactions(Core_WipAppVariables.User);
    }

    public static void LoadLast10Transactions(string currentUser)
    {
        var connectionString = Helper_SqlVariables.GetConnectionString(null, null, null, null);
        using var conn = new MySqlConnection(connectionString);
        using var cmd = new MySqlCommand("sys_last_10_transactions_Get_ByUser", conn)
        {
            CommandType = System.Data.CommandType.StoredProcedure
        };
        cmd.Parameters.AddWithValue("p_User", currentUser);

        conn.Open();
        using var reader = cmd.ExecuteReader();

        var i = 0;
        while (reader.Read() && quickButtons != null && i < quickButtons.Count)
        {
            var partId = reader["PartID"].ToString();
            var operation = reader["Operation"].ToString();
            var quantity = Convert.ToInt32(reader["Quantity"]);
            var dateTime = Convert.ToDateTime(reader["DateTime"]);

            // Compose the text as two lines: line 1 = part number, line 2 = operation
            var rawText = $"{partId}\nOp: {operation}";
            quickButtons[i].Text = TruncateTextToFitMultiline(rawText, quickButtons[i]);
            quickButtons[i].TextAlign = ContentAlignment.MiddleCenter;
            quickButtons[i].UseMnemonic = false; // Prevents '&' from being interpreted as a mnemonic
            quickButtons[i].Padding = new Padding(0);
            quickButtons[i].Margin = new Padding(0);

            var tooltipText =
                $"Part Number: {partId} | Operation: {operation}\nQuantity: {quantity} | Date/Time: {dateTime:MM/dd/yyyy hh:mm:ss tt}";
            Control_QuickButtons_Tooltip.SetToolTip(quickButtons[i], tooltipText);
            quickButtons[i].Tag = new { partId, operation, quantity, dateTime };
            quickButtons[i].Visible = true;
            i++;
        }

        // Set remaining buttons to "[ No Data ]"
        if (quickButtons != null)
            for (; i < quickButtons.Count; i++)
            {
                quickButtons[i].Text = @"[ No Data ]";
                Control_QuickButtons_Tooltip.SetToolTip(quickButtons[i], string.Empty);
                quickButtons[i].Tag = null;
                quickButtons[i].Visible = false;
            }
    }

    // Helper: Truncate multiline text to fit button size
    private static string TruncateTextToFitMultiline(string text, Button btn)
    {
        using var g = btn.CreateGraphics();
        var font = btn.Font;
        var ellipsis = "...";
        var lines = text.Split('\n');
        var maxWidth = btn.Width - 6;
        var maxHeight = btn.Height - 6;

        // Truncate each line if needed
        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            while (line.Length > 0 && g.MeasureString(line + ellipsis, font).Width > maxWidth)
                line = line[..^1];
            lines[i] = line.Length > 0 && line != lines[i] ? line + ellipsis : line;
        }

        // If total height is too much, truncate second line
        var totalText = string.Join("\n", lines);
        while (g.MeasureString(totalText, font).Height > maxHeight && lines.Length > 1 && lines[1].Length > 0)
        {
            lines[1] = lines[1][..^1];
            totalText = string.Join("\n", lines[0], lines[1] + ellipsis);
        }

        return string.Join("\n", lines);
    }

    private static void QuickButton_Click(object? sender, EventArgs? e)
    {
        if (sender is not Button btn || btn.Tag is not { } tagObj)
            return;

        // Extract data from Tag (anonymous type)
        dynamic tag = tagObj;
        string partId = tag.partId;
        string operation = tag.operation;
        int quantity = tag.quantity;

        // Use the static MainFormInstance property
        var mainForm = MainFormInstance;
        if (mainForm == null)
            return;

        // Inventory Tab
        if (mainForm.MainForm_Control_InventoryTab?.Visible == true)
        {
            var inv = mainForm.MainForm_Control_InventoryTab;
            SetComboBoxText(inv, "Control_InventoryTab_ComboBox_Part", partId);
            SetComboBoxText(inv, "Control_InventoryTab_ComboBox_Operation", operation);
            SetTextBoxText(inv, "Control_InventoryTab_TextBox_Quantity", quantity.ToString());
            inv.Focus();
            return;
        }

        // Remove Tab
        if (mainForm.MainForm_RemoveTabNormalControl?.Visible == true)
        {
            var rem = mainForm.MainForm_RemoveTabNormalControl;
            SetComboBoxText(rem, "Control_RemoveTab_ComboBox_Part", partId);
            SetComboBoxText(rem, "Control_RemoveTab_ComboBox_Operation", operation);
            rem.Focus();
            return;
        }

        // Transfer Tab
        if (mainForm.MainForm_Control_TransferTab?.Visible == true)
        {
            var trn = mainForm.MainForm_Control_TransferTab;
            SetComboBoxText(trn, "Control_TransferTab_ComboBox_Part", partId);
            SetComboBoxText(trn, "Control_TransferTab_ComboBox_Operation", operation);
            trn.Focus();
            return;
        }

        // Advanced Inventory
        if (mainForm.MainForm_AdvancedInventory?.Visible == true)
        {
            var advInv = mainForm.MainForm_AdvancedInventory;
            SetComboBoxText(advInv, "AdvancedInventory_Single_ComboBox_Part", partId);
            SetComboBoxText(advInv, "AdvancedInventory_Single_ComboBox_Op", operation);
            SetTextBoxText(advInv, "AdvancedInventory_Single_TextBox_Qty", quantity.ToString());
            advInv.Focus();
            return;
        }

        // Advanced Remove
        if (mainForm.MainForm_Control_AdvancedRemove?.Visible == true)
        {
            var advRem = mainForm.MainForm_Control_AdvancedRemove;
            SetComboBoxText(advRem, "Control_AdvancedSearch_ComboBox_Part", partId);
            SetComboBoxText(advRem, "Control_AdvancedSearch_ComboBox_Op", operation);
            advRem.Focus();
            return;
        }
    }

    // Helper to set ComboBox text by field name
    private static void SetComboBoxText(object control, string fieldName, string value)
    {
        var field = control.GetType().GetField(fieldName,
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public |
            System.Reflection.BindingFlags.Instance);
        if (field?.GetValue(control) is not ComboBox cb) return;
        cb.SelectedIndex = cb.FindStringExact(value);
        if (cb.SelectedIndex < 0)
            cb.Text = value;
        cb.ForeColor = Color.Black;
    }

    // Helper to set TextBox text by field name
    private static void SetTextBoxText(object control, string fieldName, string value)
    {
        var field = control.GetType().GetField(fieldName,
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (field?.GetValue(control) is TextBox tb)
        {
            tb.Text = value;
            tb.ForeColor = Color.Black;
        }
    }
}