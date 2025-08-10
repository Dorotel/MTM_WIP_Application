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
using MTM_Inventory_Application.Controls.Shared;

namespace MTM_Inventory_Application.Controls.MainForm
{
    public partial class Control_QuickButtons : UserControl
    {
        #region Fields

        internal List<Button>? quickButtons; // Changed from static to instance
        public static Forms.MainForm.MainForm? MainFormInstance { get; set; }
        private Helper_StoredProcedureProgress? _progressHelper;

        #endregion

        #region Progress Control Methods

        /// <summary>
        /// Set progress controls for visual feedback during operations
        /// </summary>
        /// <param name="progressBar">Progress bar control</param>
        /// <param name="statusLabel">Status label control</param>
        public void SetProgressControls(ToolStripProgressBar progressBar, ToolStripStatusLabel statusLabel)
        {
            _progressHelper = Helper_StoredProcedureProgress.Create(progressBar, statusLabel, 
                this.FindForm() ?? throw new InvalidOperationException("Control must be added to a form"));
        }

        #endregion

        #region Constructors

        public Control_QuickButtons()
        {
            InitializeComponent();

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
            foreach (Button btn in quickButtons)
            {
                btn.Click += QuickButton_Click; // Now instance method
            }

            LoadLast10Transactions(Model_AppVariables.User);
            Core_Themes.ApplyDpiScaling(this);
            Core_Themes.ApplyRuntimeLayoutAdjustments(this);
            menuItemRemove.Click += MenuItemRemove_Click;
            menuItemReorder.Click += MenuItemReorder_Click;
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
                // Fill buttons with data from DB
                while (reader.Read() && quickButtons != null && i < quickButtons.Count)
                {
                    string? partId = reader["PartID"] == DBNull.Value ? null : reader["PartID"].ToString();
                    string? operation = reader["Operation"] == DBNull.Value ? null : reader["Operation"].ToString();
                    int? quantity = reader["Quantity"] == DBNull.Value ? null :
                        reader["Quantity"] is int q ? q : Convert.ToInt32(reader["Quantity"]);
                    // Always set position 1-10, no duplicates
                    int displayPosition = i + 1;
                    string rawText = partId != null && operation != null && quantity != null
                        ? $"({operation}) - [{partId} x {quantity}]"
                        : string.Empty;
                    quickButtons[i].Text = TruncateTextToFitSingleLine(rawText, quickButtons[i]);
                    quickButtons[i].UseMnemonic = false;
                    quickButtons[i].Padding = Padding.Empty;
                    quickButtons[i].Margin = Padding.Empty;
                    string tooltipText = partId != null && operation != null && quantity != null
                        ? $"Part ID: {partId}, Operation: {operation}, Quantity: {quantity}\nPosition: {displayPosition}"
                        : $"Position: {displayPosition}";
                    Control_QuickButtons_Tooltip.SetToolTip(quickButtons[i], tooltipText);
                    quickButtons[i].Tag = new { partId, operation, quantity, position = displayPosition };
                    quickButtons[i].Visible = partId != null && operation != null && quantity != null;
                    i++;
                }

                // Fill remaining buttons as empty but with unique position
                if (quickButtons != null)
                {
                    for (; i < quickButtons.Count; i++)
                    {
                        quickButtons[i].Text = string.Empty;
                        Control_QuickButtons_Tooltip.SetToolTip(quickButtons[i], $"Position: {i + 1}");
                        quickButtons[i].Tag = new
                        {
                            partId = (string?)null,
                            operation = (string?)null,
                            quantity = (int?)null,
                            position = i + 1
                        };
                        quickButtons[i].Visible = false;
                    }

                    // Force UI refresh: clear and re-add only visible buttons
                    Control_QuickButtons_TableLayoutPanel_Main.SuspendLayout();
                    Control_QuickButtons_TableLayoutPanel_Main.Controls.Clear();
                    for (int j = 0; j < quickButtons.Count; j++)
                    {
                        if (quickButtons[j].Visible)
                        {
                            Control_QuickButtons_TableLayoutPanel_Main.Controls.Add(quickButtons[j], 0, j);
                        }
                    }

                    Control_QuickButtons_TableLayoutPanel_Main.ResumeLayout();
                }
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
            }
        }

        private string TruncateTextToFitSingleLine(string text, Button btn) // Changed from static to instance
        {
            using Graphics g = btn.CreateGraphics();
            Font font = btn.Font;
            const string ellipsis = "...";
            int maxWidth = btn.Width - 3;
            string result = text.Replace("\r", "").Replace("\n", " ");
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
                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
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
            FieldInfo? field = control.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (field?.GetValue(control) is TextBox tb)
            {
                tb.Text = value;
                tb.ForeColor = Model_AppVariables.UserUiColors.TextBoxForeColor ?? Color.Black;
            }
        }

        private void QuickButton_Click(object? sender, EventArgs? e) // Changed from static to instance method
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
                MethodInfo? onEnterMethod =
                    control.GetType().GetMethod("OnEnter", BindingFlags.NonPublic | BindingFlags.Instance);
                onEnterMethod?.Invoke(control, new object[] { enterEventArgs });
            }

            void SetFocusOnControl(object parentControl, string fieldName)
            {
                FieldInfo? field = parentControl.GetType().GetField(fieldName,
                    BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                if (field?.GetValue(parentControl) is Control targetControl && targetControl.CanFocus)
                {
                    targetControl.Focus();
                    TriggerEnterEvent(targetControl);
                }
            }

            void ClickSearchButtonIfAvailable(object control, string fieldName)
            {
                FieldInfo? field =
                    control.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
                if (field?.GetValue(control) is Button searchButton && searchButton.Enabled && searchButton.Visible)
                {
                    searchButton.PerformClick();
                }
            }

            if (mainForm.MainForm_UserControl_InventoryTab?.Visible == true)
            {
                Control_InventoryTab? inv = mainForm.MainForm_UserControl_InventoryTab;
                SetComboBoxes(inv, "Control_InventoryTab_ComboBox_Part", "Control_InventoryTab_ComboBox_Operation",
                    partId, operation);
                SetTextBoxText(inv, "Control_InventoryTab_TextBox_Quantity", quantity.ToString());
                SetFocusOnControl(inv, "Control_InventoryTab_ComboBox_Location");
                return;
            }

            if (mainForm.MainForm_UserControl_RemoveTab?.Visible == true)
            {
                Control_RemoveTab? rem = mainForm.MainForm_UserControl_RemoveTab;
                SetComboBoxes(rem, "Control_RemoveTab_ComboBox_Part", "Control_RemoveTab_ComboBox_Operation", partId,
                    operation);
                rem.Focus();
                TriggerEnterEvent(rem);
                ClickSearchButtonIfAvailable(rem, "Control_RemoveTab_Button_Search");
                return;
            }

            if (mainForm.MainForm_UserControl_TransferTab?.Visible == true)
            {
                Control_TransferTab? trn = mainForm.MainForm_UserControl_TransferTab;
                SetComboBoxes(trn, "Control_TransferTab_ComboBox_Part", "Control_TransferTab_ComboBox_Operation",
                    partId, operation);
                trn.Focus();
                TriggerEnterEvent(trn);
                ClickSearchButtonIfAvailable(trn, "Control_TransferTab_Button_Search");
                return;
            }

            if (mainForm.MainForm_UserControl_AdvancedInventory?.Visible == true)
            {
                Control_AdvancedInventory? advInv = mainForm.MainForm_UserControl_AdvancedInventory;
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

            if (mainForm.MainForm_UserControl_AdvancedRemove?.Visible == true)
            {
                Control_AdvancedRemove? advRem = mainForm.MainForm_UserControl_AdvancedRemove;
                SetComboBoxes(advRem, "Control_AdvancedRemove_ComboBox_Part", "Control_AdvancedRemove_ComboBox_Op",
                    partId, operation);
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

                using QuickButtonEditDialog dlg = new(oldPartId, oldOperation, oldQuantity);
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    try
                    {
                        await Dao_QuickButtons.UpdateQuickButtonAsync(user, idx, dlg.PartId, dlg.Operation,
                            dlg.Quantity);
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
                    int prevVisibleCount = quickButtons.Count(b => b.Visible);
                    try
                    {
                        await Dao_QuickButtons.RemoveQuickButtonAndShiftAsync(user, idx);
                    }
                    catch (MySqlException ex)
                    {
                        LoggingUtility.LogDatabaseError(ex);
                    }

                    LoadLast10Transactions(user);
                    // Fallback: check if removal succeeded visually
                    int newVisibleCount = quickButtons.Count(b => b.Visible);
                    if (newVisibleCount == prevVisibleCount)
                    {
                        LoggingUtility.Log($"QuickButton removal failed or did not update UI for index {idx}.");
                        MessageBox.Show(
                            @"Failed to remove the quick button. Please try again or restart the application.",
                            @"Remove Quick Button", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    LoggingUtility.Log($"MenuItemRemove_Click: Invalid button index or Tag is null. idx={idx}");
                }
            }
        }

        private async void MenuItemReorder_Click(object? sender, EventArgs? e)
        {
            if (quickButtons == null)
            {
                return;
            }

            using QuickButtonOrderDialog dlg = new(quickButtons);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                List<Button> newOrder = dlg.GetButtonOrder();
                string user = Model_AppVariables.User;
                // Remove all quick buttons for the user from the server
                await Dao_QuickButtons.DeleteAllQuickButtonsForUserAsync(user);
                // Re-add them in the exact order shown in ListView
                for (int i = 0; i < newOrder.Count; i++)
                {
                    Button btn = newOrder[i];
                    dynamic tag = btn.Tag;
                    string partId = tag?.partId ?? string.Empty;
                    string operation = tag?.operation ?? string.Empty;
                    int quantity = tag?.quantity ?? 0;
                    // Use the new method that doesn't shift - directly insert at position i+1
                    await Dao_QuickButtons.AddQuickButtonAtPositionAsync(user, partId, operation, quantity, i + 1);
                }

                // Update UI
                quickButtons = newOrder;
                Control_QuickButtons_TableLayoutPanel_Main.Controls.Clear();
                for (int i = 0; i < quickButtons.Count; i++)
                {
                    Control_QuickButtons_TableLayoutPanel_Main.Controls.Add(quickButtons[i], 0, i);
                }

                LoadLast10Transactions(user);
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
                txtPartId = new TextBox { Left = 100, Top = 10, Width = 160, Text = partId };
                Label lblOperation = new() { Text = "Operation", Left = 10, Top = 45, Width = 80 };
                txtOperation = new TextBox { Left = 100, Top = 40, Width = 160, Text = operation };
                Label lblQuantity = new() { Text = "Quantity", Left = 10, Top = 75, Width = 80 };
                numQuantity = new NumericUpDown
                {
                    Left = 100,
                    Top = 70,
                    Width = 80,
                    Minimum = 1,
                    Maximum = 100000,
                    Value = quantity
                };
                btnOk = new Button
                {
                    Text = "OK",
                    Left = 60,
                    Width = 80,
                    Top = 110,
                    DialogResult = DialogResult.OK
                };
                btnCancel = new Button
                {
                    Text = "Cancel",
                    Left = 150,
                    Width = 80,
                    Top = 110,
                    DialogResult = DialogResult.Cancel
                };
                btnOk.Click += (s, e) =>
                {
                    PartId = txtPartId.Text.Trim();
                    Operation = txtOperation.Text.Trim();
                    Quantity = (int)numQuantity.Value;
                    DialogResult = DialogResult.OK;
                    Close();
                };
                btnCancel.Click += (s, e) =>
                {
                    DialogResult = DialogResult.Cancel;
                    Close();
                };
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

        private class QuickButtonOrderDialog : Form
        {
            private readonly ListView listView;
            private readonly Button btnOK;
            private readonly Button btnCancel;
            private readonly Button btnEdit;
            private readonly Label lblInstructions;
            private readonly List<Button> buttonOrder;
            private int dragIndex = -1;

            public QuickButtonOrderDialog(List<Button> buttons)
            {
                Text = "Change Quick Button Order";
                Size = new Size(500, 500);
                FormBorderStyle = FormBorderStyle.FixedDialog;
                StartPosition = FormStartPosition.CenterParent;
                MinimizeBox = false;
                MaximizeBox = false;
                ShowInTaskbar = false;
                buttonOrder = new List<Button>(buttons.Where(b => b.Visible));
                listView = new ListView
                {
                    Dock = DockStyle.Top,
                    Height = 250,
                    View = View.Details,
                    FullRowSelect = true,
                    AllowDrop = true,
                    HeaderStyle = ColumnHeaderStyle.Nonclickable
                };
                listView.Columns.Add("Position", 70, HorizontalAlignment.Center);
                listView.Columns.Add("Part ID", 120, HorizontalAlignment.Left);
                listView.Columns.Add("Operation", 120, HorizontalAlignment.Left);
                listView.Columns.Add("Quantity", 80, HorizontalAlignment.Right);
                for (int i = 0; i < buttonOrder.Count; i++)
                {
                    Button btn = buttonOrder[i];
                    dynamic tag = btn.Tag;
                    string partId = tag?.partId ?? "";
                    string op = tag?.operation ?? "";
                    string qty = tag?.quantity?.ToString() ?? "";
                    ListViewItem lvi = new(new[] { (i + 1).ToString(), partId, op, qty });
                    listView.Items.Add(lvi);
                }

                listView.MouseDown += ListView_MouseDown;
                listView.ItemDrag += ListView_ItemDrag;
                listView.DragEnter += ListView_DragEnter;
                listView.DragDrop += ListView_DragDrop;
                listView.KeyDown += ListView_KeyDown;
                listView.SelectedIndexChanged += ListView_SelectedIndexChanged;

                btnEdit = new Button { Text = "Edit", Dock = DockStyle.Top, Enabled = false, Height = 32 };
                btnEdit.Click += BtnEdit_Click;

                lblInstructions = new Label
                {
                    Text =
                        "How to use this form:\n\n- Drag and drop rows to change the order.\n- Use Shift+Up/Down to move a selected row.\n- Select a row and click 'Edit' to change its details.\n- Click OK to save your changes.",
                    Dock = DockStyle.Top,
                    Height = 90,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Padding = new Padding(8),
                    Font = new Font(Font.FontFamily, 10, FontStyle.Regular),
                    AutoSize = false
                };

                btnOK = new Button { Text = "OK", DialogResult = DialogResult.OK, Dock = DockStyle.Bottom };
                btnCancel = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel, Dock = DockStyle.Bottom };

                Controls.Add(btnOK);
                Controls.Add(btnCancel);
                Controls.Add(lblInstructions);
                Controls.Add(btnEdit);
                Controls.Add(listView);

                AcceptButton = btnOK;
                CancelButton = btnCancel;

                // DPI scaling and layout adjustments
                Core_Themes.ApplyDpiScaling(this);
                Core_Themes.ApplyRuntimeLayoutAdjustments(this);
            }

            private void ListView_SelectedIndexChanged(object? sender, EventArgs e) =>
                btnEdit.Enabled = listView.SelectedIndices.Count > 0;

            private void BtnEdit_Click(object? sender, EventArgs e)
            {
                if (listView.SelectedIndices.Count == 0)
                {
                    return;
                }

                int idx = listView.SelectedIndices[0];
                Button btn = buttonOrder[idx];
                dynamic tag = btn.Tag;
                string oldPartId = tag?.partId ?? "";
                string oldOperation = tag?.operation ?? "";
                int oldQuantity = tag?.quantity ?? 1;
                using QuickButtonEditDialog dlg = new(oldPartId, oldOperation, oldQuantity);
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    // Update button's tag and text
                    btn.Tag = new
                    {
                        partId = dlg.PartId, operation = dlg.Operation, quantity = dlg.Quantity, position = idx + 1
                    };
                    btn.Text = $"({dlg.Operation}) - [{dlg.PartId} x {dlg.Quantity}]";
                    // Update ListView row
                    ListViewItem lvi = listView.Items[idx];
                    lvi.SubItems[1].Text = dlg.PartId;
                    lvi.SubItems[2].Text = dlg.Operation;
                    lvi.SubItems[3].Text = dlg.Quantity.ToString();
                }
            }

            private void ListView_MouseDown(object? sender, MouseEventArgs e) =>
                dragIndex = listView.GetItemAt(e.X, e.Y)?.Index ?? -1;

            private void ListView_ItemDrag(object? sender, ItemDragEventArgs e)
            {
                if (listView.SelectedItems.Count > 0)
                {
                    listView.DoDragDrop(listView.SelectedItems[0], DragDropEffects.Move);
                }
            }

            private void ListView_DragEnter(object? sender, DragEventArgs e)
            {
                if (e.Data?.GetDataPresent(typeof(ListViewItem)) == true)
                {
                    e.Effect = DragDropEffects.Move;
                }
            }

            private void ListView_DragDrop(object? sender, DragEventArgs e)
            {
                if (e.Data?.GetDataPresent(typeof(ListViewItem)) == true)
                {
                    Point cp = listView.PointToClient(new Point(e.X, e.Y));
                    ListViewItem dragItem = (ListViewItem)e.Data.GetData(typeof(ListViewItem));
                    int dropIndex = listView.GetItemAt(cp.X, cp.Y)?.Index ?? listView.Items.Count - 1;
                    if (dragItem.Index == dropIndex || dragItem.Index < 0)
                    {
                        return;
                    }

                    Button btn = buttonOrder[dragItem.Index];
                    buttonOrder.RemoveAt(dragItem.Index);
                    listView.Items.RemoveAt(dragItem.Index);
                    buttonOrder.Insert(dropIndex, btn);
                    listView.Items.Insert(dropIndex, (ListViewItem)dragItem.Clone());
                    listView.Items[dropIndex].Selected = true;
                    // Update positions
                    for (int i = 0; i < listView.Items.Count; i++)
                    {
                        listView.Items[i].SubItems[0].Text = (i + 1).ToString();
                    }
                }
            }

            private void ListView_KeyDown(object? sender, KeyEventArgs e)
            {
                if (e.Shift && listView.SelectedIndices.Count > 0)
                {
                    int idx = listView.SelectedIndices[0];
                    if (e.KeyCode == Keys.Up && idx > 0)
                    {
                        Button btn = buttonOrder[idx];
                        buttonOrder.RemoveAt(idx);
                        buttonOrder.Insert(idx - 1, btn);
                        ListViewItem lvi = (ListViewItem)listView.Items[idx].Clone();
                        listView.Items.RemoveAt(idx);
                        listView.Items.Insert(idx - 1, lvi);
                        listView.Items[idx - 1].Selected = true;
                        // Update positions
                        for (int i = 0; i < listView.Items.Count; i++)
                        {
                            listView.Items[i].SubItems[0].Text = (i + 1).ToString();
                        }

                        e.Handled = true;
                    }
                    else if (e.KeyCode == Keys.Down && idx < listView.Items.Count - 1)
                    {
                        Button btn = buttonOrder[idx];
                        buttonOrder.RemoveAt(idx);
                        buttonOrder.Insert(idx + 1, btn);
                        ListViewItem lvi = (ListViewItem)listView.Items[idx].Clone();
                        listView.Items.RemoveAt(idx);
                        listView.Items.Insert(idx + 1, lvi);
                        listView.Items[idx + 1].Selected = true;
                        // Update positions
                        for (int i = 0; i < listView.Items.Count; i++)
                        {
                            listView.Items[i].SubItems[0].Text = (i + 1).ToString();
                        }

                        e.Handled = true;
                    }
                }
            }

            public List<Button> GetButtonOrder()
            {
                // Return the button order as currently shown in the ListView
                List<Button> result = new();
                for (int i = 0; i < listView.Items.Count; i++)
                {
                    // Find the button that corresponds to this ListView item
                    ListViewItem listViewItem = listView.Items[i];
                    string partId = listViewItem.SubItems[1].Text;
                    string operation = listViewItem.SubItems[2].Text;
                    string quantity = listViewItem.SubItems[3].Text;

                    // Find the button with matching data
                    Button? matchingButton = buttonOrder.FirstOrDefault(btn =>
                    {
                        dynamic tag = btn.Tag;
                        return tag?.partId == partId &&
                               tag?.operation == operation &&
                               tag?.quantity?.ToString() == quantity;
                    });

                    if (matchingButton != null)
                    {
                        result.Add(matchingButton);
                    }
                }

                return result;
            }
        }

        #endregion
    }
}
