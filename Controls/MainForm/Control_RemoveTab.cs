// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel;
using System.Data;
using System.Text;
using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Forms.MainForm.Classes;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;
using MTM_Inventory_Application.Services;
using static System.Int32;

namespace MTM_Inventory_Application.Controls.MainForm;

#region RemoveTab

/// <summary>
/// Represents the Remove Tab control in the application.
/// Handles inventory removal operations, undo functionality, and UI interactions.
/// </summary>
public partial class ControlRemoveTab : UserControl
{
    #region Fields

    private readonly List<Model_HistoryRemove> _lastRemovedItems = [];

    #endregion

    #region Properties

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public static Forms.MainForm.MainForm? MainFormInstance { get; set; }

    #endregion

    #region Constructors

    public ControlRemoveTab()
    {
        InitializeComponent();
        Control_RemoveTab_Initialize();
        Helper_UI_ComboBoxes.ApplyStandardComboBoxProperties(Control_RemoveTab_ComboBox_Part);
        Helper_UI_ComboBoxes.ApplyStandardComboBoxProperties(Control_RemoveTab_ComboBox_Operation);
        Control_RemoveTab_ComboBox_Part.ForeColor =
            Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
        Control_RemoveTab_ComboBox_Operation.ForeColor =
            Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
        Control_RemoveTab_Image_NothingFound.Visible = false;
        _ = Control_RemoveTab_OnStartup_LoadComboBoxesAsync();
        if (Control_RemoveTab_Panel_Footer.Controls["Control_RemoveTab_Button_Undo"] == null)
        {
            var undoButton = new Button
            {
                Name = "Control_RemoveTab_Button_Undo",
                Text = "Undo",
                Enabled = false,
                AutoSize = true,
                Anchor = AnchorStyles.Right
            };
            undoButton.Click += Control_RemoveTab_Button_Undo_Click;
            Control_RemoveTab_Panel_Footer.Controls.Add(undoButton);
        }

        // Set tooltips for Remove tab buttons using shortcut constants
        var toolTip = new ToolTip();
        toolTip.SetToolTip(Control_RemoveTab_Button_Search,
            $"Shortcut: {Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_Remove_Search)}");
        toolTip.SetToolTip(Control_RemoveTab_Button_Delete,
            $"Shortcut: {Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_Remove_Delete)}");
        toolTip.SetToolTip(Control_RemoveTab_Button_Reset,
            $"Shortcut: {Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_Remove_Reset)}");
        var undoBtn = Control_RemoveTab_Panel_Footer.Controls["Control_RemoveTab_Button_Undo"] as Button;
        if (undoBtn != null)
            toolTip.SetToolTip(undoBtn,
                $"Shortcut: {Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_Remove_Undo)}");
    }

    #endregion

    #region Initialization

    public void Control_RemoveTab_Initialize()
    {
        Control_RemoveTab_Button_Reset.TabStop = false;
    }

    #endregion

    #region Startup / ComboBox Loading

    private async Task Control_RemoveTab_OnStartup_LoadComboBoxesAsync()
    {
        try
        {
            await Control_RemoveTab_OnStartup_LoadDataComboBoxesAsync();
            Control_RemoveTab_OnStartup_WireUpEvents();
            LoggingUtility.Log("Initial setup of ComboBoxes in the Remove Tab.");
            Control_RemoveTab_Button_Search.Enabled = false;
            Control_RemoveTab_Button_Delete.Enabled = false;
            Control_RemoveTab_ComboBox_Operation.Visible = true;
            Control_RemoveTab_ComboBox_Part.Visible = true;

            try
            {
                Model_AppVariables.UserFullName =
                    await Dao_User.GetUserFullNameAsync(Model_AppVariables.User, true);
                LoggingUtility.Log($"User full name loaded: {Model_AppVariables.UserFullName}");
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true,
                    new StringBuilder().Append("Control_RemoveTab_OnStartup_GetUserFullName").ToString());
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true,
                new StringBuilder().Append("Control_RemoveTab_OnStartup").ToString());
        }
    }

    public async Task Control_RemoveTab_OnStartup_LoadDataComboBoxesAsync()
    {
        try
        {
            await Helper_UI_ComboBoxes.FillPartComboBoxesAsync(Control_RemoveTab_ComboBox_Part);
            await Helper_UI_ComboBoxes.FillOperationComboBoxesAsync(Control_RemoveTab_ComboBox_Operation);
            LoggingUtility.Log("Remove tab ComboBoxes loaded.");
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true,
                new StringBuilder().Append("MainForm_LoadRemoveTabComboBoxesAsync").ToString());
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

            if (keyData == Keys.Delete)
            {
                // Simulate Delete button click
                Control_RemoveTab_Button_Delete.PerformClick();
                return true;
            }

            if (MainFormInstance != null && !MainFormInstance.MainForm_SplitContainer_Middle.Panel2Collapsed &&
                keyData == (Keys.Alt | Keys.Right))
            {
                Control_RemoveTab_Button_Toggle_RightPanel.PerformClick();
                return true;
            }

            if (MainFormInstance != null && MainFormInstance.MainForm_SplitContainer_Middle.Panel2Collapsed &&
                keyData == (Keys.Alt | Keys.Left))
            {
                Control_RemoveTab_Button_Toggle_RightPanel.PerformClick();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                new StringBuilder().Append("MainForm_ProcessCmdKey").ToString());
            return false;
        }
    }

    #endregion

    #region Button Clicks

    private async void Control_RemoveTab_Button_Delete_Click(object? sender, EventArgs? e)
    {
        try
        {
            var dgv = Control_RemoveTab_DataGridView_Main;
            var selectedCount = dgv.SelectedRows.Count;
            LoggingUtility.Log($"Delete clicked. Selected rows: {selectedCount}");

            if (selectedCount == 0)
            {
                LoggingUtility.Log("No rows selected for deletion.");
                return;
            }

            // Build summary for confirmation
            var sb = new StringBuilder();
            foreach (DataGridViewRow row in dgv.SelectedRows)
            {
                var partId = row.Cells["PartID"].Value?.ToString() ?? "";
                var location = row.Cells["Location"].Value?.ToString() ?? "";
                var operation = row.Cells["Operation"].Value?.ToString() ?? "";
                var quantity = row.Cells["Quantity"].Value?.ToString() ?? "";
                sb.AppendLine($"PartID: {partId}, Location: {location}, Operation: {operation}, Quantity: {quantity}");
            }

            var summary = sb.ToString();

            var confirmResult = MessageBox.Show(
                $@"The following items will be deleted:

{summary}Are you sure?",
                @"Confirm Deletion",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmResult != DialogResult.Yes)
            {
                LoggingUtility.Log("User cancelled deletion.");
                return;
            }

            // Call DAO to remove items
            var removedCount = await Dao_Inventory.RemoveInventoryItemsFromDataGridViewAsync(dgv);

            // Optionally update undo and status logic here...

            LoggingUtility.Log($"{removedCount} inventory items deleted.");
            Control_RemoveTab_Button_Search_Click(null, null);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true,
                new StringBuilder().Append("Control_RemoveTab_Button_Delete_Click").ToString());
        }
    }

    private async void Control_RemoveTab_Button_Undo_Click(object? sender, EventArgs? e)
    {
        if (_lastRemovedItems.Count == 0)
            return;

        try
        {
            foreach (var item in _lastRemovedItems)
                await Dao_Inventory.AddInventoryItemAsync(
                    item.PartId,
                    item.Location,
                    item.Operation,
                    item.Quantity,
                    item.ItemType,
                    item.User,
                    item.BatchNumber,
                    "Removal reversed via Undo Button.",
                    true
                );

            MessageBox.Show(@"Undo successful. Removed items have been restored.", @"Undo", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            LoggingUtility.Log("Undo: Removed items restored.");

            _lastRemovedItems.Clear();
            if (Control_RemoveTab_Panel_Footer.Controls["Control_RemoveTab_Button_Undo"] is Button undoBtn)
                undoBtn.Enabled = false;

            Control_RemoveTab_Button_Search_Click(null, null);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            MessageBox.Show(@"Undo failed: " + ex.Message, @"Undo Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void Control_RemoveTab_Button_Reset_Click()
    {
        Control_RemoveTab_Button_Reset.Enabled = false;
        try
        {
            // Check if Shift key is held down
            if ((ModifierKeys & Keys.Shift) == Keys.Shift)
                _ = Control_RemoveTab_HardReset();
            else
                Control_RemoveTab_SoftReset();
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                new StringBuilder().Append("MainForm_Remove_Button_Reset_Click").ToString());
        }
    }

    private async Task Control_RemoveTab_HardReset()
    {
        Control_RemoveTab_Button_Reset.Enabled = false;
        try
        {
            // Show progress bar
            MainFormInstance?.TabLoadingProgress?.ShowProgress();
            MainFormInstance?.TabLoadingProgress?.UpdateProgress(10, "Resetting Remove tab...");

            // 1) Hide controls during reset
            Control_RemoveTab_ComboBox_Part.Visible = false;
            Control_RemoveTab_ComboBox_Operation.Visible = false;

            // 2) Update status strip to show reset is in progress
            if (MainFormInstance != null)
            {
                MainFormInstance.MainForm_StatusStrip_Disconnected.Text = @"Please wait while resetting...";
                MainFormInstance.MainForm_StatusStrip_Disconnected.Visible = true;
                MainFormInstance.MainForm_StatusStrip_SavedStatus.Visible = false;
            }

            MainFormInstance?.TabLoadingProgress?.UpdateProgress(30, "Resetting data tables...");
            // 3) Reset the DataTables and reinitialize them
            await Helper_UI_ComboBoxes.ResetAndRefreshAllDataTablesAsync();

            MainFormInstance?.TabLoadingProgress?.UpdateProgress(60, "Refilling combo boxes...");
            // 4) Refill each combobox with proper data
            await Helper_UI_ComboBoxes.FillPartComboBoxesAsync(Control_RemoveTab_ComboBox_Part);
            await Helper_UI_ComboBoxes.FillOperationComboBoxesAsync(Control_RemoveTab_ComboBox_Operation);

            // 5) Clear DataGridView
            Control_RemoveTab_DataGridView_Main.DataSource = null;
            Control_RemoveTab_Image_NothingFound.Visible = false;

            // 6) Reset UI fields
            MainFormControlHelper.ResetComboBox(Control_RemoveTab_ComboBox_Part,
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);
            MainFormControlHelper.ResetComboBox(Control_RemoveTab_ComboBox_Operation,
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);

            // 7) Restore controls and focus
            Control_RemoveTab_ComboBox_Operation.Visible = true;
            Control_RemoveTab_ComboBox_Part.Visible = true;
            Control_RemoveTab_ComboBox_Part.Focus();

            // 8) Update button states
            Control_RemoveTab_Button_Search.Enabled = true;
            Control_RemoveTab_Button_Delete.Enabled = false;
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                new StringBuilder().Append("MainForm_Remove_HardReset").ToString());
        }
        finally
        {
            if (MainFormInstance != null)
            {
                MainFormInstance.MainForm_StatusStrip_Disconnected.Visible = false;
                MainFormInstance.MainForm_StatusStrip_SavedStatus.Visible = true;
                MainFormInstance.MainForm_StatusStrip_Disconnected.Text =
                    @"Disconnected from Server, please standby...";
                MainFormInstance.TabLoadingProgress?.HideProgress();
            }

            Control_RemoveTab_Button_Reset.Enabled = true;
        }
    }

    private void Control_RemoveTab_SoftReset()
    {
        Control_RemoveTab_Button_Reset.Enabled = false;
        try
        {
            if (MainFormInstance != null)
            {
                MainFormInstance.MainForm_StatusStrip_Disconnected.Text = @"Please wait while resetting...";
                MainFormInstance.MainForm_StatusStrip_Disconnected.Visible = true;
                MainFormInstance.MainForm_StatusStrip_SavedStatus.Visible = false;
            }

            // Reset UI fields
            MainFormControlHelper.ResetComboBox(Control_RemoveTab_ComboBox_Part,
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);
            MainFormControlHelper.ResetComboBox(Control_RemoveTab_ComboBox_Operation,
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);

            Control_RemoveTab_DataGridView_Main.DataSource = null;
            Control_RemoveTab_Image_NothingFound.Visible = false;

            Control_RemoveTab_Button_Search.Enabled = true;
            Control_RemoveTab_Button_Delete.Enabled = false;
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                new StringBuilder().Append("MainForm_Remove_SoftReset").ToString());
        }
        finally
        {
            Control_RemoveTab_Button_Reset.Enabled = true;
            Control_RemoveTab_ComboBox_Part.Focus();
            if (MainFormInstance != null)
            {
                MainFormInstance.MainForm_StatusStrip_Disconnected.Visible = false;
                MainFormInstance.MainForm_StatusStrip_SavedStatus.Visible = true;
                MainFormInstance.MainForm_StatusStrip_Disconnected.Text =
                    @"Disconnected from Server, please standby...";
            }
        }
    }

    private static void Control_RemoveTab_Button_AdvancedItemRemoval_Click()
    {
        try
        {
            if (Service_Timer_VersionChecker.MainFormInstance == null)
            {
                LoggingUtility.Log("MainForm instance is null, cannot open Advanced Inventory Removal.");
                return;
            }

            if (MainFormInstance != null) MainFormInstance.MainForm_RemoveTabNormalControl.Visible = false;
            if (MainFormInstance != null) MainFormInstance.MainForm_Control_AdvancedRemove.Visible = true;

            // Reset all Control_AdvancedRemove ComboBoxes' SelectedIndex to 0 and color to Red, focus Part ComboBox
            var adv = MainFormInstance?.MainForm_Control_AdvancedRemove;
            if (adv != null)
            {
                if (adv.Controls.Find("Control_AdvancedRemove_ComboBox_Part", true).FirstOrDefault() is ComboBox part)
                {
                    part.SelectedIndex = 0;
                    part.ForeColor = Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
                    part.Focus();
                }

                if (adv.Controls.Find("Control_AdvancedRemove_ComboBox_Op", true).FirstOrDefault() is ComboBox op)
                {
                    op.SelectedIndex = 0;
                    op.ForeColor = Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
                }

                if (adv.Controls.Find("Control_AdvancedRemove_ComboBox_Loc", true).FirstOrDefault() is ComboBox loc)
                {
                    loc.SelectedIndex = 0;
                    loc.ForeColor = Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
                }

                if (adv.Controls.Find("Control_AdvancedRemove_ComboBox_User", true).FirstOrDefault() is ComboBox user)
                {
                    user.SelectedIndex = 0;
                    user.ForeColor = Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
                }
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                new StringBuilder().Append("Control_RemoveTab_Button_AdvancedItemRemoval_Click").ToString());
        }
    }

    private static void Control_RemoveTab_Button_Normal_Click(object? sender, EventArgs? e)
    {
        try
        {
            if (Service_Timer_VersionChecker.MainFormInstance == null)
            {
                LoggingUtility.Log("MainForm instance is null, cannot return to normal Remove tab.");
                return;
            }

            if (MainFormInstance != null) MainFormInstance.MainForm_RemoveTabNormalControl.Visible = true;
            if (MainFormInstance != null) MainFormInstance.MainForm_Control_AdvancedRemove.Visible = false;
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                new StringBuilder().Append("Control_RemoveTab_Button_Normal_Click").ToString());
        }
    }

    private async void Control_RemoveTab_Button_Search_Click(object? sender, EventArgs? e)
    {
        try
        {
            LoggingUtility.Log("RemoveTab Search button clicked.");

            var partId = Control_RemoveTab_ComboBox_Part.Text;
            var op = Control_RemoveTab_ComboBox_Operation.Text;

            if (string.IsNullOrWhiteSpace(partId) || Control_RemoveTab_ComboBox_Part.SelectedIndex <= 0)
            {
                MessageBox.Show(@"Please select a valid Part.", @"Validation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                Control_RemoveTab_ComboBox_Part.Focus();
                return;
            }

            DataTable results;

            if (!string.IsNullOrWhiteSpace(op) && Control_RemoveTab_ComboBox_Operation.SelectedIndex > 0)
                results = await Dao_Inventory.GetInventoryByPartIdAndOperationAsync(partId, op, true);
            else
                results = await Dao_Inventory.GetInventoryByPartIdAsync(partId, true);

            Control_RemoveTab_DataGridView_Main.DataSource = results;
            Control_RemoveTab_DataGridView_Main.ClearSelection();

            foreach (DataGridViewColumn column in Control_RemoveTab_DataGridView_Main.Columns)
                column.Visible = true;

            Core_Themes.ApplyThemeToDataGridView(Control_RemoveTab_DataGridView_Main);
            Core_Themes.SizeDataGrid(Control_RemoveTab_DataGridView_Main);

            Control_RemoveTab_Image_NothingFound.Visible = results.Rows.Count == 0;
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true,
                new StringBuilder().Append("Control_RemoveTab_Button_Search_Click").ToString());
        }
    }

    #endregion

    #region ComboBox & UI Events

    private void Control_RemoveTab_ComboBox_Operation_SelectedIndexChanged()
    {
        try
        {
            LoggingUtility.Log("Inventory Op ComboBox selection changed.");

            if (Control_RemoveTab_ComboBox_Operation.SelectedIndex > 0)
            {
                Control_RemoveTab_ComboBox_Operation.ForeColor =
                    Model_AppVariables.UserUiColors.ComboBoxForeColor ?? Color.Black;
                Model_AppVariables.Operation = Control_RemoveTab_ComboBox_Operation.Text;
            }
            else
            {
                Control_RemoveTab_ComboBox_Operation.ForeColor =
                    Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
                if (Control_RemoveTab_ComboBox_Operation.SelectedIndex != 0 &&
                    Control_RemoveTab_ComboBox_Operation.Items.Count > 0)
                    Control_RemoveTab_ComboBox_Operation.SelectedIndex = 0;
                Model_AppVariables.Operation = null;
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                new StringBuilder().Append("MainForm_Inventory_ComboBox_Op").ToString());
        }
    }

    private void Control_RemoveTab_ComboBox_Part_SelectedIndexChanged()
    {
        try
        {
            LoggingUtility.Log("Inventory Part ComboBox selection changed.");

            if (Control_RemoveTab_ComboBox_Part.SelectedIndex > 0)
            {
                Control_RemoveTab_ComboBox_Part.ForeColor =
                    Model_AppVariables.UserUiColors.ComboBoxForeColor ?? Color.Black;
                Model_AppVariables.PartId = Control_RemoveTab_ComboBox_Part.Text;
            }
            else
            {
                Control_RemoveTab_ComboBox_Part.ForeColor =
                    Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
                if (Control_RemoveTab_ComboBox_Part.SelectedIndex != 0 &&
                    Control_RemoveTab_ComboBox_Part.Items.Count > 0)
                    Control_RemoveTab_ComboBox_Part.SelectedIndex = 0;
                Model_AppVariables.PartId = null;
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                new StringBuilder().Append("MainForm_Inventory_ComboBox_Part").ToString());
        }
    }

    private void Control_RemoveTab_Update_ButtonStates()
    {
        try
        {
            Control_RemoveTab_Button_Search.Enabled = Control_RemoveTab_ComboBox_Part.SelectedIndex > 0;
            var hasData = Control_RemoveTab_DataGridView_Main.Rows.Count > 0;
            var hasSelection = Control_RemoveTab_DataGridView_Main.SelectedRows.Count > 0;
            Control_RemoveTab_Button_Delete.Enabled = hasData && hasSelection;
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                new StringBuilder().Append("Control_RemoveTab_Update_ButtonStates").ToString());
        }
    }

    private void Control_RemoveTab_OnStartup_WireUpEvents()
    {
        try
        {
            Control_RemoveTab_Button_Reset.Click += (s, e) => Control_RemoveTab_Button_Reset_Click();
            Control_RemoveTab_ComboBox_Part.SelectedIndexChanged += (s, e) =>
            {
                Control_RemoveTab_ComboBox_Part_SelectedIndexChanged();
                Helper_UI_ComboBoxes.ValidateComboBoxItem(Control_RemoveTab_ComboBox_Part, "[ Enter Part Number ]");
                Control_RemoveTab_Update_ButtonStates();
            };
            Control_RemoveTab_ComboBox_Part.Leave += (s, e) =>
            {
                Helper_UI_ComboBoxes.ValidateComboBoxItem(Control_RemoveTab_ComboBox_Part, "[ Enter Part Number ]");
            };

            Control_RemoveTab_ComboBox_Operation.SelectedIndexChanged += (s, e) =>
            {
                Control_RemoveTab_ComboBox_Operation_SelectedIndexChanged();
                Helper_UI_ComboBoxes.ValidateComboBoxItem(Control_RemoveTab_ComboBox_Operation, "[ Enter Operation ]");
                Control_RemoveTab_Update_ButtonStates();
            };
            Control_RemoveTab_ComboBox_Operation.Leave += (s, e) =>
            {
                Helper_UI_ComboBoxes.ValidateComboBoxItem(Control_RemoveTab_ComboBox_Operation,
                    "[ Enter Operation ]");
            };

            Control_RemoveTab_Button_AdvancedItemRemoval.Click +=
                (s, e) => Control_RemoveTab_Button_AdvancedItemRemoval_Click();

            // Add event handler for Back to Normal button in advanced control
            if (MainFormInstance != null)
            {
                var adv = MainFormInstance.MainForm_Control_AdvancedRemove;
                var btn = adv.Controls.Find("Control_AdvancedRemove_Button_Normal", true);
                if (btn.Length > 0 && btn[0] is Button normalBtn)
                {
                    normalBtn.Click -= Control_RemoveTab_Button_Normal_Click;
                    normalBtn.Click += Control_RemoveTab_Button_Normal_Click;
                }
            }

            Control_RemoveTab_ComboBox_Part.Enter += (s, e) =>
            {
                Control_RemoveTab_ComboBox_Part.BackColor =
                    Model_AppVariables.UserUiColors.ControlFocusedBackColor ?? Color.LightBlue;
            };
            Control_RemoveTab_ComboBox_Part.Leave += (s, e) =>
            {
                Control_RemoveTab_ComboBox_Part.BackColor =
                    Model_AppVariables.UserUiColors.ControlBackColor ?? SystemColors.Window;
                Helper_UI_ComboBoxes.ValidateComboBoxItem(Control_RemoveTab_ComboBox_Part, "[ Enter Part Number ]");
            };

            Control_RemoveTab_ComboBox_Operation.Enter += (s, e) =>
            {
                Control_RemoveTab_ComboBox_Operation.BackColor =
                    Model_AppVariables.UserUiColors.ControlFocusedBackColor ?? Color.LightBlue;
            };
            Control_RemoveTab_ComboBox_Operation.Leave += (s, e) =>
            {
                Control_RemoveTab_ComboBox_Operation.BackColor =
                    Model_AppVariables.UserUiColors.ControlBackColor ?? SystemColors.Window;
                Helper_UI_ComboBoxes.ValidateComboBoxItem(Control_RemoveTab_ComboBox_Operation, "[ Enter Operation ]");
            };

            Control_RemoveTab_DataGridView_Main.SelectionChanged += (s, e) => Control_RemoveTab_Update_ButtonStates();

            Control_RemoveTab_Button_Delete.Click += Control_RemoveTab_Button_Delete_Click;

            LoggingUtility.Log("Removal tab events wired up.");
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                new StringBuilder().Append("MainForm_WireUpRemoveTabEvents").ToString());
        }
    }

    private void Control_RemoveTab_Button_Toggle_RightPanel_Click(object sender, EventArgs e)
    {
        if (MainFormInstance != null && !MainFormInstance.MainForm_SplitContainer_Middle.Panel2Collapsed)
        {
            MainFormInstance.MainForm_SplitContainer_Middle.Panel2Collapsed = true;

            Control_RemoveTab_Button_Toggle_RightPanel.Text = "←";
            Control_RemoveTab_Button_Toggle_RightPanel.ForeColor =
                Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red;
        }
        else
        {
            if (MainFormInstance != null)
            {
                MainFormInstance.MainForm_SplitContainer_Middle.Panel2Collapsed = false;
                Control_RemoveTab_Button_Toggle_RightPanel.Text = "→";
                Control_RemoveTab_Button_Toggle_RightPanel.ForeColor =
                    Model_AppVariables.UserUiColors.SuccessColor ?? Color.Green;
            }
        }

        Helper_UI_ComboBoxes.DeselectAllComboBoxText(this);
    }

    #endregion

    #region Helpers

    private List<(string PartID, string Location, int Quantity)> GetSelectedItemsToDelete(out string summary)
    {
        var sb = new StringBuilder();
        var itemsToDelete = new List<(string PartID, string Location, int Quantity)>();
        foreach (DataGridViewRow row in Control_RemoveTab_DataGridView_Main.SelectedRows)
            if (row.DataBoundItem is DataRowView drv)
            {
                var partId = drv["PartID"]?.ToString() ?? "";
                var location = drv["Location"]?.ToString() ?? "";
                var quantityStr = drv["Quantity"]?.ToString() ?? "";
                if (!TryParse(quantityStr, out var quantity))
                {
                    LoggingUtility.LogApplicationError(new Exception(
                        $"Invalid quantity value: '{quantityStr}' for PartID={partId}, Location={location}"));
                    continue;
                }

                sb.AppendLine($"PartID: {partId}, Location: {location}, Quantity: {quantity}");
                LoggingUtility.Log($"Selected for deletion: PartID={partId}, Location={location}, Quantity={quantity}");

                itemsToDelete.Add((partId, location, quantity));
            }

        summary = sb.ToString();
        return itemsToDelete;
    }

    #endregion
}

#endregion