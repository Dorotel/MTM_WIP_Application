using System.ComponentModel;
using System.Data;
using System.Text;
using DocumentFormat.OpenXml.Office.PowerPoint.Y2021.M06.Main;
using MTM_WIP_Application.Core;
using MTM_WIP_Application.Data;
using MTM_WIP_Application.Forms.MainForm.Classes;
using MTM_WIP_Application.Helpers;
using MTM_WIP_Application.Logging;
using MTM_WIP_Application.Services;
using MySql.Data.MySqlClient;

namespace MTM_WIP_Application.Controls.MainForm;

#region InventoryTab

public partial class ControlInventoryTab : UserControl
{
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public static Forms.MainForm.MainForm? MainFormInstance { get; set; }

    #region Initialization

    public ControlInventoryTab()
    {
        InitializeComponent();
        Service_Timer_VersionChecker.ControlInventoryInstance = this;

        _ = Control_InventoryTab_OnStartup_LoadDataComboBoxesAsync();

        Control_InventoryTab_OnStartup_WireUpEvents();

        SetVersionLabel(Core_WipAppVariables.UserVersion,
            Service_Timer_VersionChecker.LastCheckedDatabaseVersion ?? "unknown");
    }

    #endregion

    #region Startup / ComboBox Loading

    private async Task Control_InventoryTab_OnStartup_LoadDataComboBoxesAsync()
    {
        try
        {
            await Helper_ComboBoxes.FillPartComboBoxesAsync(Control_InventoryTab_ComboBox_Part);
            await Helper_ComboBoxes.FillOperationComboBoxesAsync(Control_InventoryTab_ComboBox_Operation);
            await Helper_ComboBoxes.FillLocationComboBoxesAsync(Control_InventoryTab_ComboBox_Location);

            await Task.Delay(100); // Small delay to ensure controls are ready
            LoggingUtility.Log("Inventory tab ComboBoxes loaded.");
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true,
                "MainForm_LoadInventoryTabComboBoxesAsync");
        }
    }

    #endregion

    #region Key Processing

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        try
        {
            if (keyData == Keys.Enter)
            {
                SelectNextControl(
                    ActiveControl,
                    true,
                    true,
                    true,
                    true
                );
                return true;
            }

            if (MainFormInstance != null && !MainFormInstance.MainForm_SplitContainer_Middle.Panel2Collapsed &&
                keyData == (Keys.Alt | Keys.Right))
            {
                Control_InventoryTab_Button_Toggle_RightPanel.PerformClick();
                return true;
            }

            if (MainFormInstance != null && MainFormInstance.MainForm_SplitContainer_Middle.Panel2Collapsed &&
                keyData == (Keys.Alt | Keys.Left))
            {
                Control_InventoryTab_Button_Toggle_RightPanel.PerformClick();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false, "MainForm_ProcessCmdKey");
            return false;
        }
    }

    #endregion

    #region Button Clicks

    private static void Control_InventoryTab_Button_AdvancedEntry_Click()
    {
        try
        {
            if (Service_Timer_VersionChecker.MainFormInstance is null)
            {
                LoggingUtility.Log("MainForm instance is null, cannot open Advanced Inventory Removal.");
                return;
            }

            if (MainFormInstance is not null) MainFormInstance.MainForm_Control_InventoryTab.Visible = false;
            if (MainFormInstance is not null) MainFormInstance.MainForm_AdvancedInventory.Visible = true;

            if (MainFormInstance?.MainForm_AdvancedInventory is null) return;
            var adv = MainFormInstance.MainForm_AdvancedInventory;

            if (adv.GetType().GetField("AdvancedInventory_Single_ComboBox_Part",
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    ?.GetValue(adv) is ComboBox combo && combo.Items.Count > 0)
            {
                combo.SelectedIndex = 0;
                combo.ForeColor = Color.Red;
                combo.Focus();
                combo.SelectAll();
            }

            if (adv.GetType().GetField("AdvancedInventory_Single_ComboBox_Op",
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    ?.GetValue(adv) is ComboBox op && op.Items.Count > 0)
            {
                op.SelectedIndex = 0;
                op.ForeColor = Color.Red;
            }

            if (adv.GetType().GetField("AdvancedInventory_Single_ComboBox_Loc",
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    ?.GetValue(adv) is ComboBox loc && loc.Items.Count > 0)
            {
                loc.SelectedIndex = 0;
                loc.ForeColor = Color.Red;
            }

            if (adv.GetType().GetField("AdvancedInventory_MultiLoc_ComboBox_Part",
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    ?.GetValue(adv) is ComboBox multiPart && multiPart.Items.Count > 0)
            {
                multiPart.SelectedIndex = 0;
                multiPart.ForeColor = Color.Red;
            }

            if (adv.GetType().GetField("AdvancedInventory_MultiLoc_ComboBox_Op",
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    ?.GetValue(adv) is ComboBox multiOp && multiOp.Items.Count > 0)
            {
                multiOp.SelectedIndex = 0;
                multiOp.ForeColor = Color.Red;
            }

            if (adv.GetType().GetField("AdvancedInventory_MultiLoc_ComboBox_Loc",
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    ?.GetValue(adv) is ComboBox multiLoc && multiLoc.Items.Count > 0)
            {
                multiLoc.SelectedIndex = 0;
                multiLoc.ForeColor = Color.Red;
            }

            if (adv.GetType().GetField("AdvancedInventory_TabControl",
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    ?.GetValue(adv) is TabControl tab)
                tab.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                new StringBuilder().Append("Control_InventoryTab_Button_AdvancedEntry_Click").ToString());
        }
    }

    private async void Control_InventoryTab_Button_Reset_Click()
    {
        try
        {
            LoggingUtility.Log("Inventory Reset button clicked.");

            // Disable reset button to prevent spamming
            Control_InventoryTab_Button_Reset.Enabled = false;

            // Hide controls during reset (optional, for visual feedback)
            Control_InventoryTab_ComboBox_Part.Visible = false;
            Control_InventoryTab_ComboBox_Operation.Visible = false;
            Control_InventoryTab_ComboBox_Location.Visible = false;

            if (MainFormInstance != null)
            {
                MainFormInstance.MainForm_StatusStrip_Disconnected.Text = @"Please wait while resetting...";
                MainFormInstance.MainForm_StatusStrip_Disconnected.Visible = true;
                MainFormInstance.MainForm_StatusStrip_SavedStatus.Visible = false;
            }

            await Helper_ComboBoxes.SetupPartDataTable();
            await Helper_ComboBoxes.SetupOperationDataTable();
            await Helper_ComboBoxes.SetupLocationDataTable();

            await Helper_ComboBoxes.FillPartComboBoxesAsync(Control_InventoryTab_ComboBox_Part);
            await Helper_ComboBoxes.FillOperationComboBoxesAsync(Control_InventoryTab_ComboBox_Operation);
            await Helper_ComboBoxes.FillLocationComboBoxesAsync(Control_InventoryTab_ComboBox_Location);

            // Reset textboxes and notes
            MainFormControlHelper.ResetTextBox(Control_InventoryTab_TextBox_Quantity, Color.Red,
                "[ Enter Valid Quantity ]");
            Control_InventoryTab_RichTextBox_Notes.Text = string.Empty;

            // Restore visibility and focus
            Control_InventoryTab_ComboBox_Part.Visible = true;
            Control_InventoryTab_ComboBox_Operation.Visible = true;
            Control_InventoryTab_ComboBox_Location.Visible = true;
            Control_InventoryTab_ComboBox_Part.Focus();

            Control_InventoryTab_Update_SaveButtonState();

            if (MainFormInstance != null)
            {
                MainFormInstance.MainForm_StatusStrip_Disconnected.Visible = false;
                MainFormInstance.MainForm_StatusStrip_SavedStatus.Visible = true;
                MainFormInstance.MainForm_StatusStrip_Disconnected.Text =
                    @"Disconnected from Server, please standby...";
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false, "MainForm_Inventory_Button_Reset");
        }
        finally
        {
            // Re-enable reset button
            Control_InventoryTab_Button_Reset.Enabled = true;
        }
    }

    private async Task Control_InventoryTab_Button_Save_Click_Async()
    {
        try
        {
            LoggingUtility.Log("Inventory Save button clicked.");

            var partId = Control_InventoryTab_ComboBox_Part.Text;
            var op = Control_InventoryTab_ComboBox_Operation.Text;
            var loc = Control_InventoryTab_ComboBox_Location.Text;
            var qtyText = Control_InventoryTab_TextBox_Quantity.Text.Trim();
            var notes = Control_InventoryTab_RichTextBox_Notes.Text.Trim();

            if (string.IsNullOrWhiteSpace(partId) || Control_InventoryTab_ComboBox_Part.SelectedIndex <= 0)
            {
                MessageBox.Show(@"Please select a valid Part.", @"Validation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                Control_InventoryTab_ComboBox_Part.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(op) || Control_InventoryTab_ComboBox_Operation.SelectedIndex <= 0)
            {
                MessageBox.Show(@"Please select a valid Operation.", @"Validation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                Control_InventoryTab_ComboBox_Operation.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(loc) || Control_InventoryTab_ComboBox_Location.SelectedIndex <= 0)
            {
                MessageBox.Show(@"Please select a valid Location.", @"Validation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                Control_InventoryTab_ComboBox_Location.Focus();
                return;
            }

            if (!int.TryParse(qtyText, out var qty) || qty <= 0)
            {
                MessageBox.Show(@"Please enter a valid quantity.", @"Validation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                Control_InventoryTab_TextBox_Quantity.Focus();
                return;
            }

            Core_WipAppVariables.PartId = partId;
            Core_WipAppVariables.Operation = op;
            Core_WipAppVariables.Location = loc;
            Core_WipAppVariables.Notes = notes;
            Core_WipAppVariables.InventoryQuantity = qty;
            Core_WipAppVariables.User ??= Environment.UserName;

            await Dao_Inventory.AddInventoryItemAsync(
                partId,
                loc,
                op,
                qty,
                "",
                Core_WipAppVariables.User,
                "",
                notes,
                true);

            // --- NEW LOGIC: Add to sys_last_10_transactions if not duplicate ---
            await AddToLast10TransactionsIfUniqueAsync(Core_WipAppVariables.User, partId, op, qty);

            MessageBox.Show(@"Inventory transaction saved successfully.", @"Success", MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            if (MainFormInstance != null)
                MainFormInstance.MainForm_StatusStrip_SavedStatus.Text =
                    $@"Last Inventoried Part: {partId} (Op: {op}), Location: {(string.IsNullOrWhiteSpace(loc) ? "" : loc)}, Quantity: {qty} @ {DateTime.Now:hh:mm tt}";

            Control_InventoryTab_Button_Reset_Click();
            Control_QuickButtons.LoadLast10Transactions(Core_WipAppVariables.User);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true, "MainForm_Inventory_Button_Save");
        }
    }

    // Helper method to add to sys_last_10_transactions if not duplicate
    private static async Task AddToLast10TransactionsIfUniqueAsync(string user, string partId, string operation,
        int quantity)
    {
        var connectionString = Helper_SqlVariables.GetConnectionString(null, null, null, null);
        using var conn = new MySqlConnection(connectionString);
        await conn.OpenAsync();

        // 1. Check for duplicate in last 10
        var checkCmd = new MySqlCommand(@"
        SELECT COUNT(*) FROM (
            SELECT PartID, Operation, Quantity
            FROM sys_last_10_transactions
            WHERE User = @User
            ORDER BY DateTime DESC
            LIMIT 10
        ) AS last10
        WHERE PartID = @PartID AND Operation = @Operation AND Quantity = @Quantity
    ", conn);

        checkCmd.Parameters.AddWithValue("@User", user);
        checkCmd.Parameters.AddWithValue("@PartID", partId);
        checkCmd.Parameters.AddWithValue("@Operation", operation);
        checkCmd.Parameters.AddWithValue("@Quantity", quantity);

        var exists = Convert.ToInt32(await checkCmd.ExecuteScalarAsync()) > 0;
        if (exists)
            return;

        // 2. Insert new transaction
        var insertCmd = new MySqlCommand(@"
        INSERT INTO sys_last_10_transactions (User, PartID, Operation, Quantity)
        VALUES (@User, @PartID, @Operation, @Quantity)
    ", conn);

        insertCmd.Parameters.AddWithValue("@User", user);
        insertCmd.Parameters.AddWithValue("@PartID", partId);
        insertCmd.Parameters.AddWithValue("@Operation", operation);
        insertCmd.Parameters.AddWithValue("@Quantity", quantity);

        await insertCmd.ExecuteNonQueryAsync();
    }

    private void Control_InventoryTab_Button_Toggle_RightPanel_Click(object sender, EventArgs e)
    {
        if (MainFormInstance != null && !MainFormInstance.MainForm_SplitContainer_Middle.Panel2Collapsed)
        {
            MainFormInstance.MainForm_SplitContainer_Middle.Panel2Collapsed = true;

            Control_InventoryTab_Button_Toggle_RightPanel.Text = "←";
            Control_InventoryTab_Button_Toggle_RightPanel.ForeColor = Color.Red;
        }
        else
        {
            if (MainFormInstance != null)
            {
                MainFormInstance.MainForm_SplitContainer_Middle.Panel2Collapsed = false;
                Control_InventoryTab_Button_Toggle_RightPanel.Text = "→";
                Control_InventoryTab_Button_Toggle_RightPanel.ForeColor = Color.Green;
            }
        }

        Helper_ComboBoxes.DeselectAllComboBoxText(this);
    }

    #endregion

    #region ComboBox & UI Events

    private void Control_InventoryTab_ComboBox_Location_SelectedIndexChanged()
    {
        try
        {
            LoggingUtility.Log("Inventory Location ComboBox selection changed.");

            if (Control_InventoryTab_ComboBox_Location.SelectedIndex > 0)
            {
                Control_InventoryTab_ComboBox_Location.ForeColor = Color.Black;
                Core_WipAppVariables.Location = Control_InventoryTab_ComboBox_Location.Text;
            }
            else
            {
                Control_InventoryTab_ComboBox_Location.ForeColor = Color.Red;
                if (Control_InventoryTab_ComboBox_Location.SelectedIndex != 0 &&
                    Control_InventoryTab_ComboBox_Location.Items.Count > 0)
                    Control_InventoryTab_ComboBox_Location.SelectedIndex = 0;
                Core_WipAppVariables.Location = null;
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false, "MainForm_Inventory_ComboBox_Loc");
        }
    }

    private void Control_InventoryTab_ComboBox_Operation_SelectedIndexChanged()
    {
        try
        {
            LoggingUtility.Log("Inventory Op ComboBox selection changed.");

            if (Control_InventoryTab_ComboBox_Operation.SelectedIndex > 0)
            {
                Control_InventoryTab_ComboBox_Operation.ForeColor = Color.Black;
                Core_WipAppVariables.Operation = Control_InventoryTab_ComboBox_Operation.Text;
            }
            else
            {
                Control_InventoryTab_ComboBox_Operation.ForeColor = Color.Red;
                if (Control_InventoryTab_ComboBox_Operation.SelectedIndex != 0 &&
                    Control_InventoryTab_ComboBox_Operation.Items.Count > 0)
                    Control_InventoryTab_ComboBox_Operation.SelectedIndex = 0;
                Core_WipAppVariables.Operation = null;
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false, "MainForm_Inventory_ComboBox_Op");
        }
    }

    private void Control_InventoryTab_ComboBox_Part_SelectedIndexChanged()
    {
        try
        {
            LoggingUtility.Log("Inventory Part ComboBox selection changed.");

            if (Control_InventoryTab_ComboBox_Part.SelectedIndex > 0)
            {
                Control_InventoryTab_ComboBox_Part.ForeColor = Color.Black;
                Core_WipAppVariables.PartId = Control_InventoryTab_ComboBox_Part.Text;
            }
            else
            {
                Control_InventoryTab_ComboBox_Part.ForeColor = Color.Red;
                if (Control_InventoryTab_ComboBox_Part.SelectedIndex != 0 &&
                    Control_InventoryTab_ComboBox_Part.Items.Count > 0)
                    Control_InventoryTab_ComboBox_Part.SelectedIndex = 0;
                Core_WipAppVariables.PartId = null;
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false, "MainForm_Inventory_ComboBox_Part");
        }
    }

    private void Control_InventoryTab_TextBox_Quantity_TextChanged()
    {
        try
        {
            LoggingUtility.Log("Inventory Quantity TextBox changed.");

            var text = Control_InventoryTab_TextBox_Quantity.Text.Trim();
            const string placeholder = "[ Enter Valid Quantity ]";
            var isValid = int.TryParse(text, out var qty) && qty > 0;

            if (isValid)
            {
                Control_InventoryTab_TextBox_Quantity.ForeColor = Color.Black;
            }
            else
            {
                Control_InventoryTab_TextBox_Quantity.ForeColor = Color.Red;
                if (text != placeholder)
                {
                    Control_InventoryTab_TextBox_Quantity.Text = placeholder;
                    Control_InventoryTab_TextBox_Quantity.SelectionStart =
                        Control_InventoryTab_TextBox_Quantity.Text.Length;
                }
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false, "MainForm_Inventory_TextBox_Qty");
        }
    }

    private void Control_InventoryTab_Update_SaveButtonState()
    {
        try
        {
            var partValid = Control_InventoryTab_ComboBox_Part.SelectedIndex > 0 &&
                            !string.IsNullOrWhiteSpace(Control_InventoryTab_ComboBox_Part.Text);
            var opValid = Control_InventoryTab_ComboBox_Operation.SelectedIndex > 0 &&
                          !string.IsNullOrWhiteSpace(Control_InventoryTab_ComboBox_Operation.Text);
            var locValid = Control_InventoryTab_ComboBox_Location.SelectedIndex > 0 &&
                           !string.IsNullOrWhiteSpace(Control_InventoryTab_ComboBox_Location.Text);
            var qtyValid = int.TryParse(Control_InventoryTab_TextBox_Quantity.Text.Trim(), out var qty) && qty > 0;
            Control_InventoryTab_Button_Save.Enabled = partValid && opValid && locValid && qtyValid;
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                "Control_InventoryTab_Update_SaveButtonState");
        }
    }

    private void Control_InventoryTab_OnStartup_WireUpEvents()
    {
        try
        {
            Control_InventoryTab_Button_Save.Click +=
                async (s, e) => await Control_InventoryTab_Button_Save_Click_Async();
            Control_InventoryTab_Button_Reset.Click += (s, e) => Control_InventoryTab_Button_Reset_Click();
            Control_InventoryTab_ComboBox_Part.SelectedIndexChanged += (s, e) =>
            {
                Control_InventoryTab_ComboBox_Part_SelectedIndexChanged();
                Control_InventoryTab_Update_SaveButtonState();
            };
            Control_InventoryTab_ComboBox_Operation.SelectedIndexChanged += (s, e) =>
            {
                Control_InventoryTab_ComboBox_Operation_SelectedIndexChanged();
                Control_InventoryTab_Update_SaveButtonState();
            };
            Control_InventoryTab_ComboBox_Location.SelectedIndexChanged += (s, e) =>
            {
                Control_InventoryTab_ComboBox_Location_SelectedIndexChanged();
                Control_InventoryTab_Update_SaveButtonState();
            };
            Control_InventoryTab_TextBox_Quantity.TextChanged += (s, e) =>
            {
                Control_InventoryTab_TextBox_Quantity_TextChanged();
                Control_InventoryTab_Update_SaveButtonState();
            };


            Control_InventoryTab_Button_AdvancedEntry.Click +=
                (s, e) => Control_InventoryTab_Button_AdvancedEntry_Click();

            Control_InventoryTab_TextBox_Quantity.KeyDown += (sender, e) =>
                MainFormControlHelper.AdjustQuantityByKey_Quantity(sender, e, "[ Enter Valid Quantity ]", Color.Black,
                    Color.Red);

            // Highlight notes RichTextBox on Enter/Leave
            Control_InventoryTab_RichTextBox_Notes.Enter += (s, e) =>
            {
                Control_InventoryTab_RichTextBox_Notes.BackColor = Color.LightBlue;
            };
            Control_InventoryTab_RichTextBox_Notes.Leave += (s, e) =>
            {
                Control_InventoryTab_RichTextBox_Notes.BackColor = SystemColors.Window;
            };

            Control_InventoryTab_ComboBox_Part.Enter += (s, e) =>
            {
                Control_InventoryTab_ComboBox_Part.BackColor = Color.LightBlue;
            };
            Control_InventoryTab_ComboBox_Part.Leave += (s, e) =>
            {
                Control_InventoryTab_ComboBox_Part.BackColor = SystemColors.Window;
                Helper_ComboBoxes.ValidateComboBoxItem(Control_InventoryTab_ComboBox_Part, "[ Enter Part Number ]");
            };

            Control_InventoryTab_ComboBox_Location.Enter += (s, e) =>
            {
                Control_InventoryTab_ComboBox_Location.BackColor = Color.LightBlue;
            };
            Control_InventoryTab_ComboBox_Location.Leave += (s, e) =>
            {
                Control_InventoryTab_ComboBox_Location.BackColor = SystemColors.Window;
                Helper_ComboBoxes.ValidateComboBoxItem(Control_InventoryTab_ComboBox_Location, "[ Enter Location ]");
            };

            Control_InventoryTab_ComboBox_Operation.Enter += (s, e) =>
            {
                Control_InventoryTab_ComboBox_Operation.BackColor = Color.LightBlue;
            };
            Control_InventoryTab_ComboBox_Operation.Leave += (s, e) =>
            {
                Control_InventoryTab_ComboBox_Operation.BackColor = SystemColors.Window;
                Helper_ComboBoxes.ValidateComboBoxItem(Control_InventoryTab_ComboBox_Operation, "[ Enter Operation ]");
            };

            Control_InventoryTab_TextBox_Quantity.Enter += (s, e) =>
            {
                Control_InventoryTab_TextBox_Quantity.BackColor = Color.LightBlue;
            };
            Control_InventoryTab_TextBox_Quantity.Leave += (s, e) =>
            {
                Control_InventoryTab_TextBox_Quantity.BackColor = SystemColors.Window;
            };


            LoggingUtility.Log("Inventory tab events wired up.");
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false, "MainForm_WireUpInventoryTabEvents");
        }
    }

    public void SetVersionLabel(string currentVersion, string serverVersion)
    {
        if (Control_InventoryTab_Label_Version.InvokeRequired)
        {
            Control_InventoryTab_Label_Version.Invoke(new Action(() => SetVersionLabel(currentVersion, serverVersion)));
            return;
        }

        var isOutOfDate = currentVersion != serverVersion;
        Control_InventoryTab_Label_Version.Text =
            $@"Client Version: {currentVersion} | Server Version: {serverVersion}";
        Control_InventoryTab_Label_Version.ForeColor = isOutOfDate ? Color.Red : SystemColors.ControlText;
    }

    #endregion
}

#endregion