// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Models;
using MySql.Data.MySqlClient;

namespace MTM_Inventory_Application.Controls.MainForm;

/// <summary>
/// Represents the control for quick access buttons in the main form.
/// </summary>
public partial class Control_QuickButtons : UserControl
{
    #region Fields

    private static List<Button>? quickButtons;

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the instance of the main form.
    /// </summary>
    public static Forms.MainForm.MainForm? MainFormInstance { get; set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Control_QuickButtons"/> class.
    /// </summary>
    public Control_QuickButtons()
    {
        InitializeComponent();
        quickButtons = new List<Button>
        {
            Control_QuickButtons_Button_Button1, Control_QuickButtons_Button_Button2,
            Control_QuickButtons_Button_Button3, Control_QuickButtons_Button_Button4,
            Control_QuickButtons_Button_Button5,
            Control_QuickButtons_Button_Button6, Control_QuickButtons_Button_Button7,
            Control_QuickButtons_Button_Button8, Control_QuickButtons_Button_Button9,
            Control_QuickButtons_Button_Button10
        };

        // Ensure all buttons are the same size after initialization
        if (quickButtons != null && quickButtons.Count > 0)
        {
            var maxWidth = quickButtons.Max(b => b.Width);
            var maxHeight = quickButtons.Max(b => b.Height);
            foreach (var btn in quickButtons) btn.Size = new Size(maxWidth, maxHeight);
        }

        LoadLast10Transactions(Model_AppVariables.User);
        if (quickButtons != null)
            foreach (var btn in quickButtons)
                btn.Click += QuickButton_Click;
        LoadLast10Transactions(Model_AppVariables.User);
    }

    #endregion

    #region Methods

    /// <summary>
    /// Loads the last 10 transactions for the specified user.
    /// </summary>
    /// <param name="currentUser">The current user.</param>
    public void LoadLast10Transactions(string currentUser)
    {
        var connectionString = Helper_Database_Variables.GetConnectionString(null, null, null, null);
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
            var dateTime = Convert.ToDateTime(reader["ReceiveDate"]);

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

        // Set remaining buttons to hidden if no data
        if (quickButtons != null)
            for (; i < quickButtons.Count; i++)
            {
                quickButtons[i].Text = string.Empty;
                Control_QuickButtons_Tooltip.SetToolTip(quickButtons[i], string.Empty);
                quickButtons[i].Tag = null;
                quickButtons[i].Visible = false;
            }
    }

    /// <summary>
    /// Handles the click event for quick buttons.
    /// </summary>
    private static void QuickButton_Click(object? sender, EventArgs? e)
    {
        if (sender is not Button btn || btn.Tag is not { } tagObj)
            return;

        // Extract data from Tag (anonymous type)
        dynamic tag = tagObj;
        string partId = tag.partId;
        string operation = tag.operation;
        int quantity = tag.quantity;

        var mainForm = MainFormInstance;
        if (mainForm == null)
            return;

        // Helper local function to set ComboBox values
        static void SetComboBoxes(object control, string partField, string opField, string part, string op)
        {
            SetComboBoxText(control, partField, part);
            SetComboBoxText(control, opField, op);
        }

        // Helper local function to trigger Enter event on control
        static void TriggerEnterEvent(Control control)
        {
            if (control != null)
            {
                var enterEventArgs = new EventArgs();
                var onEnterMethod = control.GetType().GetMethod("OnEnter",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                onEnterMethod?.Invoke(control, new object[] { enterEventArgs });
            }
        }

        // Helper local function to set focus on a specific control by field name
        static void SetFocusOnControl(object parentControl, string fieldName)
        {
            var field = parentControl.GetType().GetField(fieldName,
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.Instance);
            if (field?.GetValue(parentControl) is Control targetControl && targetControl.CanFocus)
            {
                targetControl.Focus();
                TriggerEnterEvent(targetControl);
            }
        }

        // Inventory Tab
        if (mainForm.MainForm_Control_InventoryTab?.Visible == true)
        {
            var inv = mainForm.MainForm_Control_InventoryTab;
            SetComboBoxes(inv, "Control_InventoryTab_ComboBox_Part", "Control_InventoryTab_ComboBox_Operation", partId,
                operation);
            SetTextBoxText(inv, "Control_InventoryTab_TextBox_Quantity", quantity.ToString());

            // Set focus specifically on the Location ComboBox to get proper highlighting
            SetFocusOnControl(inv, "Control_InventoryTab_ComboBox_Location");
            return;
        }

        // Remove Tab
        if (mainForm.MainForm_RemoveTabNormalControl?.Visible == true)
        {
            var rem = mainForm.MainForm_RemoveTabNormalControl;
            SetComboBoxes(rem, "Control_RemoveTab_ComboBox_Part", "Control_RemoveTab_ComboBox_Operation", partId,
                operation);
            rem.Focus();
            TriggerEnterEvent(rem);

            ClickSearchButtonIfAvailable(rem, "Control_RemoveTab_Button_Search");
            return;
        }

        // Transfer Tab
        if (mainForm.MainForm_Control_TransferTab?.Visible == true)
        {
            var trn = mainForm.MainForm_Control_TransferTab;
            SetComboBoxes(trn, "Control_TransferTab_ComboBox_Part", "Control_TransferTab_ComboBox_Operation", partId,
                operation);
            trn.Focus();
            TriggerEnterEvent(trn);

            ClickSearchButtonIfAvailable(trn, "Control_TransferTab_Button_Search");
            return;
        }

        // Advanced Inventory
        if (mainForm.MainForm_AdvancedInventory?.Visible == true)
        {
            var advInv = mainForm.MainForm_AdvancedInventory;
            var tabControlField = advInv.GetType().GetField("AdvancedInventory_TabControl",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var tabControl = tabControlField?.GetValue(advInv) as TabControl;
            if (tabControl != null)
            {
                // Check which tab is selected
                var selectedTab = tabControl.SelectedTab;
                if (selectedTab != null && selectedTab.Name == "AdvancedInventory_TabControl_MultiLoc")
                {
                    // MultiLoc tab: set part, op, and quantity in MultiLoc controls
                    SetComboBoxes(advInv, "AdvancedInventory_MultiLoc_ComboBox_Part",
                        "AdvancedInventory_MultiLoc_ComboBox_Op", partId, operation);
                    SetTextBoxText(advInv, "AdvancedInventory_MultiLoc_TextBox_Qty", quantity.ToString());
                }
                else
                {
                    // Single tab: set part, op, and quantity in Single controls
                    SetComboBoxes(advInv, "AdvancedInventory_Single_ComboBox_Part",
                        "AdvancedInventory_Single_ComboBox_Op", partId, operation);
                    SetTextBoxText(advInv, "AdvancedInventory_Single_TextBox_Qty", quantity.ToString());
                }
            }

            advInv.Focus();
            TriggerEnterEvent(advInv);
            return;
        }

        // Advanced Remove
        if (mainForm.MainForm_Control_AdvancedRemove?.Visible == true)
        {
            var advRem = mainForm.MainForm_Control_AdvancedRemove;
            SetComboBoxes(advRem, "Control_AdvancedRemove_ComboBox_Part", "Control_AdvancedRemove_ComboBox_Op", partId,
                operation);
            advRem.Focus();
            TriggerEnterEvent(advRem);

            ClickSearchButtonIfAvailable(advRem, "Control_AdvancedRemove_Button_Search");
            return;
        }

        return;

        // Helper local function to click a search button by field name if enabled and visible
        static void ClickSearchButtonIfAvailable(object control, string fieldName)
        {
            var field = control.GetType().GetField(fieldName,
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field?.GetValue(control) is Button searchButton && searchButton.Enabled && searchButton.Visible)
                searchButton.PerformClick();
        }
    }

    /// <summary>
    /// Truncates multiline text to fit the button size.
    /// </summary>
    /// <param name="text">The text to truncate.</param>
    /// <param name="btn">The button to fit the text into.</param>
    /// <returns>The truncated text.</returns>
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

    /// <summary>
    /// Sets the text of a ComboBox by field name.
    /// </summary>
    /// <param name="control">The parent control.</param>
    /// <param name="fieldName">The field name of the ComboBox.</param>
    /// <param name="value">The value to set.</param>
    private static void SetComboBoxText(object control, string fieldName, string value)
    {
        var field = control.GetType().GetField(fieldName,
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public |
            System.Reflection.BindingFlags.Instance);
        if (field?.GetValue(control) is not ComboBox cb) return;
        cb.SelectedIndex = cb.FindStringExact(value);
        if (cb.SelectedIndex < 0)
            cb.Text = value;
        cb.ForeColor = Model_AppVariables.UserUiColors.ComboBoxForeColor ?? Color.Black;
    }

    /// <summary>
    /// Sets the text of a TextBox by field name.
    /// </summary>
    /// <param name="control">The parent control.</param>
    /// <param name="fieldName">The field name of the TextBox.</param>
    /// <param name="value">The value to set.</param>
    private static void SetTextBoxText(object control, string fieldName, string value)
    {
        var field = control.GetType().GetField(fieldName,
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (field?.GetValue(control) is TextBox tb)
        {
            tb.Text = value;
            tb.ForeColor = Model_AppVariables.UserUiColors.TextBoxForeColor ?? Color.Black;
        }
    }

    #endregion
}