using System.ComponentModel;
using System.Data;
using System.Text;
using MTM_WIP_Application.Core;
using MTM_WIP_Application.Data;
using MTM_WIP_Application.Forms.AdvancedInventoryEntryForm;
using MTM_WIP_Application.Forms.MainForm.Classes;
using MTM_WIP_Application.Logging;
using MTM_WIP_Application.Services;
using MySql.Data.MySqlClient;

namespace MTM_WIP_Application.Controls.MainForm;

public partial class Control_InventoryTab : UserControl
{
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public static Forms.MainForm.MainForm? MainFormInstance { get; set; }

    public Control_InventoryTab()
    {
        InitializeComponent();
        Control_InventoryTab_Initialize();
        Control_InventoryTab_ComboBox_Part.ForeColor = Color.Red;
        Control_InventoryTab_ComboBox_Operation.ForeColor = Color.Red;
        Control_InventoryTab_ComboBox_Location.ForeColor = Color.Red;
        Control_InventoryTab_TextBox_Quantity.ForeColor = Color.Red;
        _ = Control_InventoryTab_OnStartup_LoadComboBoxes();
    }

    public void Control_InventoryTab_Initialize()
    {
        Control_InventoryTab_ComboBox_Location.Visible = false;
        Control_InventoryTab_ComboBox_Operation.Visible = false;
        Control_InventoryTab_ComboBox_Part.Visible = false;
        Control_InventoryTab_Button_Reset.TabStop = false;

        SetVersionLabel(WipAppVariables.UserVersion, VersionCheckerService.LastCheckedDatabaseVersion ?? "unknown");
    }

    private static void Control_InventoryTab_Button_AdvancedEntry_Click()
    {
        try
        {
            if (VersionCheckerService.MainFormInstance == null)
            {
                AppLogger.Log("MainForm instance is null, cannot open Advanced Inventory Entry.");
                return;
            }

            var advancedEntryForm = new AdvancedInventoryEntryForm();
            advancedEntryForm.ShowDialog(VersionCheckerService.MainFormInstance);
            AppLogger.Log("Inventory Advanced Entry button clicked.");
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false,
                "Control_InventoryTab_Button_AdvancedEntry_Click");
        }
    }

    private async void Control_InventoryTab_Button_Reset_Click()
    {
        try
        {
            AppLogger.Log("Inventory Reset button clicked.");
            Control_InventoryTab_ComboBox_Location.Visible = false;
            Control_InventoryTab_ComboBox_Operation.Visible = false;
            Control_InventoryTab_ComboBox_Part.Visible = false;


            // Reinitialize ComboBox DataTables
            await MainFormComboBoxDataHelper.FillComboBoxAsync(
                "md_part_ids_Get_All",
                new MySqlConnection(WipAppVariables.ConnectionString),
                new MySqlDataAdapter(),
                new DataTable(),
                Control_InventoryTab_ComboBox_Part,
                "Item Number",
                "ID",
                "[ Enter Part ID ]",
                CommandType.StoredProcedure);

            await MainFormComboBoxDataHelper.FillComboBoxAsync(
                "md_operation_numbers_Get_All",
                new MySqlConnection(WipAppVariables.ConnectionString),
                new MySqlDataAdapter(),
                new DataTable(),
                Control_InventoryTab_ComboBox_Operation,
                "Operation",
                "Operation",
                "[ Enter Op # ]",
                CommandType.StoredProcedure);

            await MainFormComboBoxDataHelper.FillComboBoxAsync(
                "md_locations_Get_All",
                new MySqlConnection(WipAppVariables.ConnectionString),
                new MySqlDataAdapter(),
                new DataTable(),
                Control_InventoryTab_ComboBox_Location,
                "Location",
                "Location",
                "[ Enter Location ]",
                CommandType.StoredProcedure);

            if (MainFormInstance != null)
                MainFormTabResetHelper.ResetInventoryTab(
                    Control_InventoryTab_ComboBox_Location,
                    Control_InventoryTab_ComboBox_Operation,
                    Control_InventoryTab_ComboBox_Part,
                    new CheckBox(),
                    new CheckBox(),
                    null,
                    Control_InventoryTab_TextBox_Quantity,
                    Control_InventoryTab_RichTextBox_Notes,
                    Control_InventoryTab_Button_Save,
                    MainFormInstance.MainForm_MenuStrip_File_Save
                );

            Control_InventoryTab_ComboBox_Location.Visible = true;
            Control_InventoryTab_ComboBox_Operation.Visible = true;
            Control_InventoryTab_ComboBox_Part.Visible = true;
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false, "MainForm_Inventory_Button_Reset");
        }
    }

    private async Task Control_InventoryTab_Button_Save_Click_Async()
    {
        try
        {
            AppLogger.Log("Inventory Save button clicked.");

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

            WipAppVariables.PartId = partId;
            WipAppVariables.Operation = op;
            WipAppVariables.Location = loc;
            WipAppVariables.Notes = notes;
            WipAppVariables.InventoryQuantity = qty;
            WipAppVariables.User ??= Environment.UserName;
            WipAppVariables.PartType ??= "";

            // FIXED: Use AddInventoryItemAsync instead of InventoryTab_SaveAsync
            await InventoryDao.AddInventoryItemAsync(
                partId,
                loc,
                op,
                qty,
                WipAppVariables.PartType ?? "",
                WipAppVariables.User,
                "", // batchNumber
                notes,
                true);

            MessageBox.Show(@"Inventory transaction saved successfully.", @"Success", MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            if (MainFormInstance != null)
                MainFormInstance.MainForm_StatusStrip_SavedStatus.Text =
                    $@"Last Inventoried Part: {partId} (Op: {op}), Location: {loc}, Quantity: {qty} @ {DateTime.Now:hh:mm tt}";

            Control_InventoryTab_Button_Reset_Click();
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, true, "MainForm_Inventory_Button_Save");
        }
    }

    private void Control_InventoryTab_ComboBox_Location_SelectedIndexChanged()
    {
        try
        {
            AppLogger.Log("Inventory Location ComboBox selection changed.");

            if (Control_InventoryTab_ComboBox_Location.SelectedIndex > 0)
            {
                Control_InventoryTab_ComboBox_Location.ForeColor = Color.Black;
                WipAppVariables.Location = Control_InventoryTab_ComboBox_Location.Text;
            }
            else
            {
                Control_InventoryTab_ComboBox_Location.ForeColor = Color.Red;
                if (Control_InventoryTab_ComboBox_Location.SelectedIndex != 0 &&
                    Control_InventoryTab_ComboBox_Location.Items.Count > 0)
                    Control_InventoryTab_ComboBox_Location.SelectedIndex = 0;
                WipAppVariables.Location = null;
            }
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false, "MainForm_Inventory_ComboBox_Loc");
        }
    }

    private void Control_InventoryTab_ComboBox_Operation_SelectedIndexChanged()
    {
        try
        {
            AppLogger.Log("Inventory Op ComboBox selection changed.");

            if (Control_InventoryTab_ComboBox_Operation.SelectedIndex > 0)
            {
                Control_InventoryTab_ComboBox_Operation.ForeColor = Color.Black;
                WipAppVariables.Operation = Control_InventoryTab_ComboBox_Operation.Text;
            }
            else
            {
                Control_InventoryTab_ComboBox_Operation.ForeColor = Color.Red;
                if (Control_InventoryTab_ComboBox_Operation.SelectedIndex != 0 &&
                    Control_InventoryTab_ComboBox_Operation.Items.Count > 0)
                    Control_InventoryTab_ComboBox_Operation.SelectedIndex = 0;
                WipAppVariables.Operation = null;
            }
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false, "MainForm_Inventory_ComboBox_Op");
        }
    }

    private void Control_InventoryTab_ComboBox_Part_SelectedIndexChanged()
    {
        try
        {
            AppLogger.Log("Inventory Part ComboBox selection changed.");

            if (Control_InventoryTab_ComboBox_Part.SelectedIndex > 0)
            {
                Control_InventoryTab_ComboBox_Part.ForeColor = Color.Black;
                WipAppVariables.PartId = Control_InventoryTab_ComboBox_Part.Text;
            }
            else
            {
                Control_InventoryTab_ComboBox_Part.ForeColor = Color.Red;
                if (Control_InventoryTab_ComboBox_Part.SelectedIndex != 0 &&
                    Control_InventoryTab_ComboBox_Part.Items.Count > 0)
                    Control_InventoryTab_ComboBox_Part.SelectedIndex = 0;
                WipAppVariables.PartId = null;
            }
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false, "MainForm_Inventory_ComboBox_Part");
        }
    }

    private void Control_InventoryTab_TextBox_Quantity_TextChanged()
    {
        try
        {
            AppLogger.Log("Inventory Quantity TextBox changed.");

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
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false, "MainForm_Inventory_TextBox_Qty");
        }
    }

    private async Task Control_InventoryTab_LoadData_ComboBoxes_Async()
    {
        try
        {
            await using var connection = new MySqlConnection(WipAppVariables.ConnectionString);

            var comboBoxSets =
                new (MySqlDataAdapter Adapter, DataTable Table, ComboBox ComboBox, string ProcName, string Display,
                    string Value, string Placeholder, CommandType CommandType)[]
                    {
                        (new MySqlDataAdapter(), new DataTable(), Control_InventoryTab_ComboBox_Part,
                            "md_part_ids_Get_All", "Item Number", "ID", "[ Enter Part ID ]",
                            CommandType.StoredProcedure),
                        (new MySqlDataAdapter(), new DataTable(), Control_InventoryTab_ComboBox_Operation,
                            "md_operation_numbers_Get_All", "Operation", "Operation", "[ Enter Op # ]",
                            CommandType.StoredProcedure),
                        (new MySqlDataAdapter(), new DataTable(), Control_InventoryTab_ComboBox_Location,
                            "md_locations_Get_All", "Location", "Location", "[ Enter Location ]",
                            CommandType.StoredProcedure)
                    };

            foreach (var (adapter, table, comboBox, procName, display, value, placeholder, cmdType) in comboBoxSets)
                await MainFormComboBoxDataHelper.FillComboBoxAsync(
                    procName,
                    connection,
                    adapter,
                    table,
                    comboBox,
                    display,
                    value,
                    placeholder,
                    cmdType
                );
            AppLogger.Log("Inventory tab ComboBoxes loaded.");
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, true,
                "MainForm_LoadInventoryTabComboBoxesAsync");
        }
    }

    private async Task Control_InventoryTab_OnStartup_LoadComboBoxes()
    {
        try
        {
            await Control_InventoryTab_LoadData_ComboBoxes_Async();
            Control_InventoryTab_OnStartup_WireUpEvents();
            AppLogger.Log("Initial setup of ComboBoxes in the Inventory Tab.");
            if (MainFormInstance != null)
                MainFormTabResetHelper.ResetInventoryTab(
                    Control_InventoryTab_ComboBox_Location,
                    Control_InventoryTab_ComboBox_Operation,
                    Control_InventoryTab_ComboBox_Part,
                    new CheckBox(),
                    new CheckBox(),
                    null,
                    Control_InventoryTab_TextBox_Quantity,
                    Control_InventoryTab_RichTextBox_Notes,
                    Control_InventoryTab_Button_Save,
                    MainFormInstance.MainForm_MenuStrip_File_Save
                );
            Control_InventoryTab_ComboBox_Location.Visible = true;
            Control_InventoryTab_ComboBox_Operation.Visible = true;
            Control_InventoryTab_ComboBox_Part.Visible = true;

            try
            {
                WipAppVariables.UserFullName = await UserDao.GetUserFullNameAsync(WipAppVariables.User, true);
                AppLogger.Log($"User full name loaded: {WipAppVariables.UserFullName}");
            }
            catch (Exception ex)
            {
                AppLogger.LogApplicationError(ex);
                await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, true,
                    new StringBuilder().Append("Control_InventoryTab_OnStartup_GetUserFullName").ToString());
            }
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            await ErrorLogDao.HandleException_GeneralError_CloseApp(ex, true, "Control_InventoryTab_OnStartup");
        }
    }

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

            return base.ProcessCmdKey(ref msg, keyData);
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false, "MainForm_ProcessCmdKey");
            return false;
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
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false,
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
            Control_InventoryTab_TextBox_Quantity.Enter += (s, e) => Control_InventoryTab_TextBox_Quantity.SelectAll();
            Control_InventoryTab_TextBox_Quantity.Click += (s, e) => Control_InventoryTab_TextBox_Quantity.SelectAll();


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
            };

            Control_InventoryTab_ComboBox_Location.Enter += (s, e) =>
            {
                Control_InventoryTab_ComboBox_Location.BackColor = Color.LightBlue;
            };
            Control_InventoryTab_ComboBox_Location.Leave += (s, e) =>
            {
                Control_InventoryTab_ComboBox_Location.BackColor = SystemColors.Window;
            };

            Control_InventoryTab_ComboBox_Operation.Enter += (s, e) =>
            {
                Control_InventoryTab_ComboBox_Operation.BackColor = Color.LightBlue;
            };
            Control_InventoryTab_ComboBox_Operation.Leave += (s, e) =>
            {
                Control_InventoryTab_ComboBox_Operation.BackColor = SystemColors.Window;
            };

            Control_InventoryTab_TextBox_Quantity.Enter += (s, e) =>
            {
                Control_InventoryTab_TextBox_Quantity.BackColor = Color.LightBlue;
            };
            Control_InventoryTab_TextBox_Quantity.Leave += (s, e) =>
            {
                Control_InventoryTab_TextBox_Quantity.BackColor = SystemColors.Window;
            };


            AppLogger.Log("Inventory tab events wired up.");
        }
        catch (Exception ex)
        {
            AppLogger.LogApplicationError(ex);
            _ = ErrorLogDao.HandleException_GeneralError_CloseApp(ex, false, "MainForm_WireUpInventoryTabEvents");
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

    private void Control_InventoryTab_Button_Toggle_RightPanel_Click(object sender, EventArgs e)
    {
        if (MainFormInstance != null && !MainFormInstance.MainForm_InventoryTab_SplitContainer_Middle.Panel2Collapsed)
        {
            MainFormInstance.MainForm_InventoryTab_SplitContainer_Middle.Panel2Collapsed = true;
            Control_InventoryTab_Button_Toggle_RightPanel.Text = @"Toggle Panel (Off)";
        }
        else
        {
            if (MainFormInstance != null)
                MainFormInstance.MainForm_InventoryTab_SplitContainer_Middle.Panel2Collapsed = false;
            Control_InventoryTab_Button_Toggle_RightPanel.Text = @"Toggle Panel (On)";
        }
    }
}