using System.Reflection;
using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Models;
using MySql.Data.MySqlClient;

namespace MTM_Inventory_Application.Controls.MainForm
{
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

            // Ensure TableLayoutPanel row styles are set for autosizing at runtime
            Control_QuickButtons_TableLayoutPanel_Main.RowCount = 10;
            Control_QuickButtons_TableLayoutPanel_Main.RowStyles.Clear();
            for (int i = 0; i < 10; i++)
            {
                Control_QuickButtons_TableLayoutPanel_Main.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            }

            quickButtons = new List<Button>
            {
                Control_QuickButtons_Button_Button1,
                Control_QuickButtons_Button_Button2,
                Control_QuickButtons_Button_Button3,
                Control_QuickButtons_Button_Button4,
                Control_QuickButtons_Button_Button5,
                Control_QuickButtons_Button_Button6,
                Control_QuickButtons_Button_Button7,
                Control_QuickButtons_Button_Button8,
                Control_QuickButtons_Button_Button9,
                Control_QuickButtons_Button_Button10
            };

            if (quickButtons.Count > 0)
            {
                int maxWidth = quickButtons.Max(b => b.Width);
                int maxHeight = quickButtons.Max(b => b.Height);
                foreach (Button btn in quickButtons)
                {
                    btn.Size = new Size(maxWidth, maxHeight);
                    btn.Click += QuickButton_Click;
                }
            }

            LoadLast10Transactions(Model_AppVariables.User);

            // Apply comprehensive DPI scaling and runtime layout adjustments
            Core_Themes.ApplyDpiScaling(this);
            Core_Themes.ApplyRuntimeLayoutAdjustments(this);
        }

        #endregion

        #region Methods

        public void LoadLast10Transactions(string currentUser)
        {
            try
            {
                string connectionString = Helper_Database_Variables.GetConnectionString(null, null, null, null);
                using MySqlConnection conn = new(connectionString);
                using MySqlCommand cmd = new("sys_last_10_transactions_Get_ByUser", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("p_User", currentUser);

                conn.Open();
                using MySqlDataReader? reader = cmd.ExecuteReader();

                int i = 0;
                while (reader.Read() && quickButtons != null && i < quickButtons.Count)
                {
                    string partId = reader["PartID"].ToString() ?? string.Empty;
                    string operation = reader["Operation"].ToString() ?? string.Empty;
                    int quantity = reader["Quantity"] is int q ? q : Convert.ToInt32(reader["Quantity"]);
                    DateTime dateTime = reader["ReceiveDate"] is DateTime dt
                        ? dt
                        : Convert.ToDateTime(reader["ReceiveDate"]);

                    // Format: (Operation) - [PartID x Quantity] (single line, no wrapping)
                    string rawText = $"({operation}) - [{partId} x {quantity}]";
                    quickButtons[i].Text = TruncateTextToFitSingleLine(rawText, quickButtons[i]);
                    quickButtons[i].TextAlign = ContentAlignment.MiddleCenter;
                    quickButtons[i].UseMnemonic = false;
                    quickButtons[i].Padding = Padding.Empty;
                    quickButtons[i].Margin = Padding.Empty;
                    quickButtons[i].AutoSize = false;
                    quickButtons[i].AutoEllipsis = true;
                    quickButtons[i].MaximumSize = quickButtons[i].Size;

                    // Tooltip: Part ID: {PartID}, Operation: {Operation}, Quantity: {Quantity}\nDate Added: {Date Added}
                    string tooltipText =
                        $"Part ID: {partId}, Operation: {operation}, Quantity: {quantity}\nDate Added: {dateTime:MM/dd/yyyy hh:mm:ss tt}";
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
                Logging.LoggingUtility.LogApplicationError(ex);
            }
        }

        private static void QuickButton_Click(object? sender, EventArgs? e)
        {
            if (sender is not Button btn || btn.Tag is null)
            {
                return;
            }

            dynamic tag = btn.Tag;
            string partId = tag.partId;
            string operation = tag.operation;
            int quantity = tag.quantity;

            Forms.MainForm.MainForm? mainForm = MainFormInstance;
            if (mainForm == null)
            {
                return;
            }

            void SetComboBoxes(object control, string partField, string opField, string part, string op)
            {
                SetComboBoxText(control, partField, part);
                SetComboBoxText(control, opField, op);
            }

            void TriggerEnterEvent(Control control)
            {
                EventArgs enterEventArgs = EventArgs.Empty;
                MethodInfo? onEnterMethod = control.GetType().GetMethod("OnEnter",
                    BindingFlags.NonPublic | BindingFlags.Instance);
                onEnterMethod?.Invoke(control, new object[] { enterEventArgs });
            }

            void SetFocusOnControl(object parentControl, string fieldName)
            {
                FieldInfo? field = parentControl.GetType().GetField(fieldName,
                    BindingFlags.NonPublic | BindingFlags.Public |
                    BindingFlags.Instance);
                if (field?.GetValue(parentControl) is Control targetControl && targetControl.CanFocus)
                {
                    targetControl.Focus();
                    TriggerEnterEvent(targetControl);
                }
            }

            void ClickSearchButtonIfAvailable(object control, string fieldName)
            {
                FieldInfo? field = control.GetType().GetField(fieldName,
                    BindingFlags.NonPublic | BindingFlags.Instance);
                if (field?.GetValue(control) is Button searchButton && searchButton.Enabled && searchButton.Visible)
                {
                    searchButton.PerformClick();
                }
            }

            if (mainForm.MainForm_Control_InventoryTab?.Visible == true)
            {
                Control_InventoryTab? inv = mainForm.MainForm_Control_InventoryTab;
                SetComboBoxes(inv, "Control_InventoryTab_ComboBox_Part", "Control_InventoryTab_ComboBox_Operation",
                    partId, operation);
                SetTextBoxText(inv, "Control_InventoryTab_TextBox_Quantity", quantity.ToString());
                SetFocusOnControl(inv, "Control_InventoryTab_ComboBox_Location");
                return;
            }

            if (mainForm.MainForm_RemoveTabNormalControl?.Visible == true)
            {
                Control_RemoveTab? rem = mainForm.MainForm_RemoveTabNormalControl;
                SetComboBoxes(rem, "Control_RemoveTab_ComboBox_Part", "Control_RemoveTab_ComboBox_Operation", partId,
                    operation);
                rem.Focus();
                TriggerEnterEvent(rem);
                ClickSearchButtonIfAvailable(rem, "Control_RemoveTab_Button_Search");
                return;
            }

            if (mainForm.MainForm_Control_TransferTab?.Visible == true)
            {
                Control_TransferTab? trn = mainForm.MainForm_Control_TransferTab;
                SetComboBoxes(trn, "Control_TransferTab_ComboBox_Part", "Control_TransferTab_ComboBox_Operation",
                    partId, operation);
                trn.Focus();
                TriggerEnterEvent(trn);
                ClickSearchButtonIfAvailable(trn, "Control_TransferTab_Button_Search");
                return;
            }

            if (mainForm.MainForm_AdvancedInventory?.Visible == true)
            {
                Control_AdvancedInventory? advInv = mainForm.MainForm_AdvancedInventory;
                FieldInfo? tabControlField = advInv.GetType().GetField("AdvancedInventory_TabControl",
                    BindingFlags.NonPublic | BindingFlags.Instance);
                TabControl? tabControl = tabControlField?.GetValue(advInv) as TabControl;
                if (tabControl != null)
                {
                    TabPage? selectedTab = tabControl.SelectedTab;
                    if (selectedTab != null && selectedTab.Name == "AdvancedInventory_TabControl_MultiLoc")
                    {
                        SetComboBoxes(advInv, "AdvancedInventory_MultiLoc_ComboBox_Part",
                            "AdvancedInventory_MultiLoc_ComboBox_Op", partId, operation);
                        SetTextBoxText(advInv, "AdvancedInventory_MultiLoc_TextBox_Qty", quantity.ToString());
                    }
                    else
                    {
                        SetComboBoxes(advInv, "AdvancedInventory_Single_ComboBox_Part",
                            "AdvancedInventory_Single_ComboBox_Op", partId, operation);
                        SetTextBoxText(advInv, "AdvancedInventory_Single_TextBox_Qty", quantity.ToString());
                    }
                }

                advInv.Focus();
                TriggerEnterEvent(advInv);
                return;
            }

            if (mainForm.MainForm_Control_AdvancedRemove?.Visible == true)
            {
                Control_AdvancedRemove? advRem = mainForm.MainForm_Control_AdvancedRemove;
                SetComboBoxes(advRem, "Control_AdvancedRemove_ComboBox_Part", "Control_AdvancedRemove_ComboBox_Op",
                    partId, operation);
                advRem.Focus();
                TriggerEnterEvent(advRem);
                ClickSearchButtonIfAvailable(advRem, "Control_AdvancedRemove_Button_Search");
                return;
            }
        }

        private static string TruncateTextToFitSingleLine(string text, Button btn)
        {
            // Ensure only a single line, truncate with ellipsis if needed
            using Graphics g = btn.CreateGraphics();
            Font font = btn.Font;
            const string ellipsis = "...";
            int maxWidth = btn.Width - 3;
            string result = text.Replace("\r", "").Replace("\n", " "); // Remove newlines
            while (result.Length > 0 && g.MeasureString(result, font).Width > maxWidth)
            {
                result = result[..^1];
            }

            if (result.Length < text.Length)
            {
                result += ellipsis;
            }

            return result;
        }

        private static void SetComboBoxText(object control, string fieldName, string value)
        {
            FieldInfo? field = control.GetType().GetField(fieldName,
                BindingFlags.NonPublic | BindingFlags.Public |
                BindingFlags.Instance);
            if (field?.GetValue(control) is not ComboBox cb)
            {
                return;
            }

            cb.SelectedIndex = cb.FindStringExact(value);
            if (cb.SelectedIndex < 0)
            {
                cb.Text = value;
            }

            cb.ForeColor = Model_AppVariables.UserUiColors.ComboBoxForeColor ?? Color.Black;
        }

        private static void SetTextBoxText(object control, string fieldName, string value)
        {
            FieldInfo? field = control.GetType().GetField(fieldName,
                BindingFlags.NonPublic | BindingFlags.Instance);
            if (field?.GetValue(control) is TextBox tb)
            {
                tb.Text = value;
                tb.ForeColor = Model_AppVariables.UserUiColors.TextBoxForeColor ?? Color.Black;
            }
        }

        #endregion
    }
}
