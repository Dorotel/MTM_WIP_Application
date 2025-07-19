using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Models;
using MySql.Data.MySqlClient;

namespace MTM_Inventory_Application.Controls.MainForm;

public partial class Control_QuickButtons : UserControl
{
    #region Fields

    private static List<Button>? quickButtons;

    #endregion

    #region Properties

    public static Forms.MainForm.MainForm? MainFormInstance { get; set; }

    #endregion

    #region Constructors

    public Control_QuickButtons()
    {
        InitializeComponent();
        
        // Apply comprehensive DPI scaling and runtime layout adjustments
        Core_Themes.ApplyDpiScaling(this);
        Core_Themes.ApplyRuntimeLayoutAdjustments(this);
        
        quickButtons = new List<Button>
        {
            Control_QuickButtons_Button_Button1, Control_QuickButtons_Button_Button2,
            Control_QuickButtons_Button_Button3, Control_QuickButtons_Button_Button4,
            Control_QuickButtons_Button_Button5,
            Control_QuickButtons_Button_Button6, Control_QuickButtons_Button_Button7,
            Control_QuickButtons_Button_Button8, Control_QuickButtons_Button_Button9,
            Control_QuickButtons_Button_Button10
        };

        if (quickButtons.Count > 0)
        {
            int maxWidth = quickButtons.Max(b => b.Width);
            int maxHeight = quickButtons.Max(b => b.Height);
            foreach (var btn in quickButtons)
            {
                btn.Size = new Size(maxWidth, maxHeight);
                btn.Click += QuickButton_Click;
            }
        }
        LoadLast10Transactions(Model_AppVariables.User);
    }

    #endregion

    #region Methods

    public void LoadLast10Transactions(string currentUser)
    {
        try
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

            int i = 0;
            while (reader.Read() && quickButtons != null && i < quickButtons.Count)
            {
                string partId = reader["PartID"].ToString() ?? string.Empty;
                string operation = reader["Operation"].ToString() ?? string.Empty;
                int quantity = reader["Quantity"] is int q ? q : Convert.ToInt32(reader["Quantity"]);
                DateTime dateTime = reader["ReceiveDate"] is DateTime dt ? dt : Convert.ToDateTime(reader["ReceiveDate"]);

                // Always show PartID on top, Operation on bottom
                string rawText = $"{partId}\n{operation}";
                quickButtons[i].Text = TruncateTextToFitMultiline(rawText, quickButtons[i]);
                quickButtons[i].TextAlign = ContentAlignment.MiddleCenter;
                quickButtons[i].UseMnemonic = false;
                quickButtons[i].Padding = Padding.Empty;
                quickButtons[i].Margin = Padding.Empty;

                string tooltipText = $"Part Number: {partId} | Operation: {operation}\nQuantity: {quantity} | Date/Time: {dateTime:MM/dd/yyyy hh:mm:ss tt}";
                Control_QuickButtons_Tooltip.SetToolTip(quickButtons[i], tooltipText);
                quickButtons[i].Tag = new { partId, operation, quantity, dateTime };
                quickButtons[i].Visible = true;
                i++;
            }

            if (quickButtons != null)
            {
                for (; i < quickButtons.Count; i++)
                {
                    quickButtons[i].Text = string.Empty;
                    Control_QuickButtons_Tooltip.SetToolTip(quickButtons[i], string.Empty);
                    quickButtons[i].Tag = null;
                    quickButtons[i].Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            // Log error for reliability
            MTM_Inventory_Application.Logging.LoggingUtility.LogApplicationError(ex);
        }
    }

    private static void QuickButton_Click(object? sender, EventArgs? e)
    {
        if (sender is not Button btn || btn.Tag is null)
            return;

        dynamic tag = btn.Tag;
        string partId = tag.partId;
        string operation = tag.operation;
        int quantity = tag.quantity;

        var mainForm = MainFormInstance;
        if (mainForm == null)
            return;

        void SetComboBoxes(object control, string partField, string opField, string part, string op)
        {
            SetComboBoxText(control, partField, part);
            SetComboBoxText(control, opField, op);
        }

        void TriggerEnterEvent(Control control)
        {
            var enterEventArgs = EventArgs.Empty;
            var onEnterMethod = control.GetType().GetMethod("OnEnter", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            onEnterMethod?.Invoke(control, new object[] { enterEventArgs });
        }

        void SetFocusOnControl(object parentControl, string fieldName)
        {
            var field = parentControl.GetType().GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            if (field?.GetValue(parentControl) is Control targetControl && targetControl.CanFocus)
            {
                targetControl.Focus();
                TriggerEnterEvent(targetControl);
            }
        }

        void ClickSearchButtonIfAvailable(object control, string fieldName)
        {
            var field = control.GetType().GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field?.GetValue(control) is Button searchButton && searchButton.Enabled && searchButton.Visible)
                searchButton.PerformClick();
        }

        if (mainForm.MainForm_Control_InventoryTab?.Visible == true)
        {
            var inv = mainForm.MainForm_Control_InventoryTab;
            SetComboBoxes(inv, "Control_InventoryTab_ComboBox_Part", "Control_InventoryTab_ComboBox_Operation", partId, operation);
            SetTextBoxText(inv, "Control_InventoryTab_TextBox_Quantity", quantity.ToString());
            SetFocusOnControl(inv, "Control_InventoryTab_ComboBox_Location");
            return;
        }
        if (mainForm.MainForm_RemoveTabNormalControl?.Visible == true)
        {
            var rem = mainForm.MainForm_RemoveTabNormalControl;
            SetComboBoxes(rem, "Control_RemoveTab_ComboBox_Part", "Control_RemoveTab_ComboBox_Operation", partId, operation);
            rem.Focus();
            TriggerEnterEvent(rem);
            ClickSearchButtonIfAvailable(rem, "Control_RemoveTab_Button_Search");
            return;
        }
        if (mainForm.MainForm_Control_TransferTab?.Visible == true)
        {
            var trn = mainForm.MainForm_Control_TransferTab;
            SetComboBoxes(trn, "Control_TransferTab_ComboBox_Part", "Control_TransferTab_ComboBox_Operation", partId, operation);
            trn.Focus();
            TriggerEnterEvent(trn);
            ClickSearchButtonIfAvailable(trn, "Control_TransferTab_Button_Search");
            return;
        }
        if (mainForm.MainForm_AdvancedInventory?.Visible == true)
        {
            var advInv = mainForm.MainForm_AdvancedInventory;
            var tabControlField = advInv.GetType().GetField("AdvancedInventory_TabControl", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var tabControl = tabControlField?.GetValue(advInv) as TabControl;
            if (tabControl != null)
            {
                var selectedTab = tabControl.SelectedTab;
                if (selectedTab != null && selectedTab.Name == "AdvancedInventory_TabControl_MultiLoc")
                {
                    SetComboBoxes(advInv, "AdvancedInventory_MultiLoc_ComboBox_Part", "AdvancedInventory_MultiLoc_ComboBox_Op", partId, operation);
                    SetTextBoxText(advInv, "AdvancedInventory_MultiLoc_TextBox_Qty", quantity.ToString());
                }
                else
                {
                    SetComboBoxes(advInv, "AdvancedInventory_Single_ComboBox_Part", "AdvancedInventory_Single_ComboBox_Op", partId, operation);
                    SetTextBoxText(advInv, "AdvancedInventory_Single_TextBox_Qty", quantity.ToString());
                }
            }
            advInv.Focus();
            TriggerEnterEvent(advInv);
            return;
        }
        if (mainForm.MainForm_Control_AdvancedRemove?.Visible == true)
        {
            var advRem = mainForm.MainForm_Control_AdvancedRemove;
            SetComboBoxes(advRem, "Control_AdvancedRemove_ComboBox_Part", "Control_AdvancedRemove_ComboBox_Op", partId, operation);
            advRem.Focus();
            TriggerEnterEvent(advRem);
            ClickSearchButtonIfAvailable(advRem, "Control_AdvancedRemove_Button_Search");
            return;
        }
    }

    private static string TruncateTextToFitMultiline(string text, Button btn)
    {
        using var g = btn.CreateGraphics();
        var font = btn.Font;
        const string ellipsis = "...";
        var lines = text.Split('\n');
        int maxWidth = btn.Width - 6;
        int maxHeight = btn.Height - 6;

        // Try to fit both lines by shrinking font size if needed
        float fontSize = font.Size;
        Font testFont = font;
        string[] fittedLines = (string[])lines.Clone();
        string totalText = string.Join("\n", fittedLines);
        SizeF textSize = g.MeasureString(totalText, testFont);
        int minFontSize = 6;
        while ((textSize.Width > maxWidth || textSize.Height > maxHeight) && fontSize > minFontSize)
        {
            fontSize -= 0.5f;
            testFont = new Font(font.FontFamily, fontSize, font.Style);
            textSize = g.MeasureString(totalText, testFont);
        }
        // If still doesn't fit, truncate lines
        for (int i = 0; i < fittedLines.Length; i++)
        {
            var line = fittedLines[i];
            while (line.Length > 0 && g.MeasureString(line + ellipsis, testFont).Width > maxWidth)
                line = line[..^1];
            fittedLines[i] = line.Length > 0 && line != lines[i] ? line + ellipsis : line;
        }
        totalText = string.Join("\n", fittedLines);
        // Set the button font to the new size if changed
        if (btn.Font.Size != fontSize)
            btn.Font = new Font(font.FontFamily, fontSize, font.Style);
        return totalText;
    }

    private static void SetComboBoxText(object control, string fieldName, string value)
    {
        var field = control.GetType().GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        if (field?.GetValue(control) is not ComboBox cb) return;
        cb.SelectedIndex = cb.FindStringExact(value);
        if (cb.SelectedIndex < 0)
            cb.Text = value;
        cb.ForeColor = Model_AppVariables.UserUiColors.ComboBoxForeColor ?? Color.Black;
    }

    private static void SetTextBoxText(object control, string fieldName, string value)
    {
        var field = control.GetType().GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (field?.GetValue(control) is TextBox tb)
        {
            tb.Text = value;
            tb.ForeColor = Model_AppVariables.UserUiColors.TextBoxForeColor ?? Color.Black;
        }
    }

    #endregion
}
