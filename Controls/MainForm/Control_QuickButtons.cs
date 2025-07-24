using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Models;
using MTM_Inventory_Application.Logging;
using MySql.Data.MySqlClient;

namespace MTM_Inventory_Application.Controls.MainForm
{
    public partial class Control_QuickButtons : UserControl
    {
        #region Fields

        internal static List<Button>? quickButtons;
        public static Forms.MainForm.MainForm? MainFormInstance { get; set; }
        private int dragSourceIndex = -1;
        private Button? dragOverTargetButton = null;

        #endregion

        #region Constructors

        public Control_QuickButtons()
        {
            InitializeComponent();

            Control_QuickButtons_TableLayoutPanel_Main.RowCount = 10;
            Control_QuickButtons_TableLayoutPanel_Main.RowStyles.Clear();
            for (int i = 0; i < 10; i++)
                Control_QuickButtons_TableLayoutPanel_Main.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
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
            foreach (var btn in quickButtons)
            {
                btn.Click += QuickButton_Click;
                btn.MouseDown += QuickButton_MouseDown;
                btn.AllowDrop = true;
                btn.DragOver += QuickButton_DragOver;
                btn.DragDrop += QuickButton_DragDrop;
                btn.DragLeave += QuickButton_DragLeave;
            }
            LoadLast10Transactions(Model_AppVariables.User);
            Core_Themes.ApplyDpiScaling(this);
            Core_Themes.ApplyRuntimeLayoutAdjustments(this);
            menuItemRemove.Click += MenuItemRemove_Click;
            menuItemEdit.Click += MenuItemEdit_Click;
        }

        #endregion

        #region Methods

        public void LoadLast10Transactions(string currentUser)
        {
            try
            {
                string connectionString = Helper_Database_Variables.GetConnectionString(null, null, null, null);
                using MySqlConnection conn = new(connectionString);
                using MySqlCommand cmd = new("sys_last_10_transactions_Get_ByUser_1", conn)
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
                    int position = reader["Position"] is int p ? p : Convert.ToInt32(reader["Position"]);
                    string rawText = $"({operation}) - [{partId} x {quantity}]";
                    quickButtons[i].Text = TruncateTextToFitSingleLine(rawText, quickButtons[i]);
                    quickButtons[i].UseMnemonic = false;
                    quickButtons[i].Padding = Padding.Empty;
                    quickButtons[i].Margin = Padding.Empty;
                    string tooltipText = $"Part ID: {partId}, Operation: {operation}, Quantity: {quantity}\nPosition: {position}";
                    Control_QuickButtons_Tooltip.SetToolTip(quickButtons[i], tooltipText);
                    quickButtons[i].Tag = new { partId, operation, quantity, position };
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
                LoggingUtility.LogApplicationError(ex);
            }
        }

        private static string TruncateTextToFitSingleLine(string text, Button btn)
        {
            using Graphics g = btn.CreateGraphics();
            Font font = btn.Font;
            const string ellipsis = "...";
            int maxWidth = btn.Width - 3;
            string result = text.Replace("\r", "").Replace("\n", " ");
            while (result.Length > 0 && g.MeasureString(result, font).Width > maxWidth)
                result = result[..^1];
            if (result.Length < text.Length)
                result += ellipsis;
            return result;
        }

        private static void SetComboBoxText(object control, string fieldName, string value)
        {
            FieldInfo? field = control.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (field?.GetValue(control) is not ComboBox cb) return;
            cb.SelectedIndex = cb.FindStringExact(value);
            if (cb.SelectedIndex < 0) cb.Text = value;
            cb.ForeColor = Model_AppVariables.UserUiColors.ComboBoxForeColor ?? Color.Black;
        }

        private static void SetTextBoxText(object control, string fieldName, string value)
        {
            FieldInfo? field = control.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (field?.GetValue(control) is TextBox tb)
            {
                tb.Text = value;
                tb.ForeColor = Model_AppVariables.UserUiColors.TextBoxForeColor ?? Color.Black;
            }
        }

        private void QuickButton_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && sender is Button btn && quickButtons != null)
            {
                dragSourceIndex = quickButtons.IndexOf(btn);
                if (dragSourceIndex >= 0 && btn.Tag != null && !string.IsNullOrEmpty(btn.Text))
                    btn.DoDragDrop(btn, DragDropEffects.Move);
            }
        }

        private void QuickButton_DragOver(object? sender, DragEventArgs e)
        {
            if (e.Data?.GetDataPresent(typeof(Button)) == true && sender is Button targetBtn)
            {
                e.Effect = DragDropEffects.Move;
                if (dragOverTargetButton != targetBtn)
                {
                    if (dragOverTargetButton != null)
                        ResetQuickButtonHighlight(dragOverTargetButton);
                    dragOverTargetButton = targetBtn;
                    var theme = Core_Themes.Core_AppThemes.GetCurrentTheme();
                    var highlightColor = theme.Colors.ButtonHoverBackColor ?? Color.LightBlue;
                    dragOverTargetButton.BackColor = highlightColor;
                }
            }
        }

        private async void QuickButton_DragDrop(object? sender, DragEventArgs e)
        {
            int tgtIdx = -1;
            if (dragOverTargetButton != null)
            {
                tgtIdx = quickButtons?.IndexOf(dragOverTargetButton) ?? -1;
                ResetQuickButtonHighlight(dragOverTargetButton);
                dragOverTargetButton = null;
            }
            if (quickButtons == null) return;
            if (e.Data?.GetData(typeof(Button)) is Button srcBtn)
            {
                int srcIdx = quickButtons.IndexOf(srcBtn);
                if (srcIdx == -1 || tgtIdx == -1 || srcIdx == tgtIdx) return;
                string user = Model_AppVariables.User;
                try
                {
                    await Dao_QuickButtons.MoveQuickButtonAsync(user, srcIdx, tgtIdx);
                }
                catch (MySqlException ex)
                {
                    LoggingUtility.LogDatabaseError(ex);
                }
                LoadLast10Transactions(user);
            }
        }

        private void QuickButton_DragLeave(object? sender, EventArgs e)
        {
            if (dragOverTargetButton != null)
            {
                ResetQuickButtonHighlight(dragOverTargetButton);
                dragOverTargetButton = null;
            }
        }

        private void ResetQuickButtonHighlight(Button btn)
        {
            var theme = Core_Themes.Core_AppThemes.GetCurrentTheme();
            btn.BackColor = theme.Colors.ButtonBackColor ?? SystemColors.Control;
        }

        private static void QuickButton_Click(object? sender, EventArgs? e)
        {
            if (sender is not Button btn || btn.Tag is null) return;
            dynamic tag = btn.Tag;
            string partId = tag.partId;
            string operation = tag.operation;
            int quantity = tag.quantity;
            Forms.MainForm.MainForm? mainForm = MainFormInstance;
            if (mainForm == null) return;
            void SetComboBoxes(object control, string partField, string opField, string part, string op)
            {
                SetComboBoxText(control, partField, part);
                SetComboBoxText(control, opField, op);
            }
            void TriggerEnterEvent(Control control)
            {
                EventArgs enterEventArgs = EventArgs.Empty;
                MethodInfo? onEnterMethod = control.GetType().GetMethod("OnEnter", BindingFlags.NonPublic | BindingFlags.Instance);
                onEnterMethod?.Invoke(control, new object[] { enterEventArgs });
            }
            void SetFocusOnControl(object parentControl, string fieldName)
            {
                FieldInfo? field = parentControl.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if (field?.GetValue(parentControl) is Control targetControl && targetControl.CanFocus)
                {
                    targetControl.Focus();
                    TriggerEnterEvent(targetControl);
                }
            }
            void ClickSearchButtonIfAvailable(object control, string fieldName)
            {
                FieldInfo? field = control.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
                if (field?.GetValue(control) is Button searchButton && searchButton.Enabled && searchButton.Visible)
                    searchButton.PerformClick();
            }
            if (mainForm.MainForm_UserControl_InventoryTab?.Visible == true)
            {
                Control_InventoryTab? inv = mainForm.MainForm_UserControl_InventoryTab;
                SetComboBoxes(inv, "Control_InventoryTab_ComboBox_Part", "Control_InventoryTab_ComboBox_Operation", partId, operation);
                SetTextBoxText(inv, "Control_InventoryTab_TextBox_Quantity", quantity.ToString());
                SetFocusOnControl(inv, "Control_InventoryTab_ComboBox_Location");
                return;
            }
            if (mainForm.MainForm_UserControl_RemoveTab?.Visible == true)
            {
                Control_RemoveTab? rem = mainForm.MainForm_UserControl_RemoveTab;
                SetComboBoxes(rem, "Control_RemoveTab_ComboBox_Part", "Control_RemoveTab_ComboBox_Operation", partId, operation);
                rem.Focus();
                TriggerEnterEvent(rem);
                ClickSearchButtonIfAvailable(rem, "Control_RemoveTab_Button_Search");
                return;
            }
            if (mainForm.MainForm_UserControl_TransferTab?.Visible == true)
            {
                Control_TransferTab? trn = mainForm.MainForm_UserControl_TransferTab;
                SetComboBoxes(trn, "Control_TransferTab_ComboBox_Part", "Control_TransferTab_ComboBox_Operation", partId, operation);
                trn.Focus();
                TriggerEnterEvent(trn);
                ClickSearchButtonIfAvailable(trn, "Control_TransferTab_Button_Search");
                return;
            }
            if (mainForm.MainForm_UserControl_AdvancedInventory?.Visible == true)
            {
                Control_AdvancedInventory? advInv = mainForm.MainForm_UserControl_AdvancedInventory;
                FieldInfo? tabControlField = advInv.GetType().GetField("AdvancedInventory_TabControl", BindingFlags.NonPublic | BindingFlags.Instance);
                TabControl? tabControl = tabControlField?.GetValue(advInv) as TabControl;
                if (tabControl != null)
                {
                    TabPage? selectedTab = tabControl.SelectedTab;
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
            if (mainForm.MainForm_UserControl_AdvancedRemove?.Visible == true)
            {
                Control_AdvancedRemove? advRem = mainForm.MainForm_UserControl_AdvancedRemove;
                SetComboBoxes(advRem, "Control_AdvancedRemove_ComboBox_Part", "Control_AdvancedRemove_ComboBox_Op", partId, operation);
                advRem.Focus();
                TriggerEnterEvent(advRem);
                ClickSearchButtonIfAvailable(advRem, "Control_AdvancedRemove_Button_Search");
                return;
            }
        }

        private async void MenuItemEdit_Click(object? sender, EventArgs? e)
        {
            if (Control_QuickButtons_ContextMenu.SourceControl is Button btn && btn.Tag != null && quickButtons != null)
            {
                int idx = quickButtons.IndexOf(btn);
                dynamic tag = btn.Tag;
                string oldPartId = tag.partId;
                string oldOperation = tag.operation;
                int oldQuantity = tag.quantity;
                string user = Model_AppVariables.User;
                using var dlg = new QuickButtonEditDialog(oldPartId, oldOperation, oldQuantity);
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        await Dao_QuickButtons.UpdateQuickButtonAsync(user, idx, dlg.PartId, dlg.Operation, dlg.Quantity);
                    }
                    catch (MySqlException ex)
                    {
                        LoggingUtility.LogDatabaseError(ex);
                    }
                    LoadLast10Transactions(user);
                }
            }
        }

        private async void MenuItemRemove_Click(object? sender, EventArgs? e)
        {
            if (Control_QuickButtons_ContextMenu.SourceControl is Button btn && quickButtons != null)
            {
                int idx = quickButtons.IndexOf(btn);
                if (idx >= 0 && btn.Tag is not null)
                {
                    string user = Model_AppVariables.User;
                    try
                    {
                        await Dao_QuickButtons.RemoveQuickButtonAndShiftAsync(user, idx);
                    }
                    catch (MySqlException ex)
                    {
                        LoggingUtility.LogDatabaseError(ex);
                    }
                    LoadLast10Transactions(user);
                }
            }
        }

        private class QuickButtonEditDialog : Form
        {
            public string PartId { get; private set; }
            public string Operation { get; private set; }
            public int Quantity { get; private set; }
            private TextBox txtPartId;
            private TextBox txtOperation;
            private NumericUpDown numQuantity;
            private Button btnOk;
            private Button btnCancel;
            public QuickButtonEditDialog(string partId, string operation, int quantity)
            {
                Text = "Edit Quick Button";
                Width = 300;
                Height = 200;
                FormBorderStyle = FormBorderStyle.FixedDialog;
                StartPosition = FormStartPosition.CenterParent;
                MaximizeBox = false;
                MinimizeBox = false;
                ShowInTaskbar = false;
                Label lblPartId = new() { Text = "Part ID", Left = 10, Top = 15, Width = 80 };
                txtPartId = new() { Left = 100, Top = 10, Width = 160, Text = partId };
                Label lblOperation = new() { Text = "Operation", Left = 10, Top = 45, Width = 80 };
                txtOperation = new() { Left = 100, Top = 40, Width = 160, Text = operation };
                Label lblQuantity = new() { Text = "Quantity", Left = 10, Top = 75, Width = 80 };
                numQuantity = new() { Left = 100, Top = 70, Width = 80, Minimum = 1, Maximum = 100000, Value = quantity };
                btnOk = new() { Text = "OK", Left = 60, Width = 80, Top = 110, DialogResult = DialogResult.OK };
                btnCancel = new() { Text = "Cancel", Left = 150, Width = 80, Top = 110, DialogResult = DialogResult.Cancel };
                btnOk.Click += (s, e) =>
                {
                    PartId = txtPartId.Text.Trim();
                    Operation = txtOperation.Text.Trim();
                    Quantity = (int)numQuantity.Value;
                    DialogResult = DialogResult.OK;
                    Close();
                };
                btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
                Controls.Add(lblPartId);
                Controls.Add(txtPartId);
                Controls.Add(lblOperation);
                Controls.Add(txtOperation);
                Controls.Add(lblQuantity);
                Controls.Add(numQuantity);
                Controls.Add(btnOk);
                Controls.Add(btnCancel);
            }
        }
        #endregion
    }
}
