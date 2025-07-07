using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using ClosedXML.Excel;
using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Forms.MainForm.Classes;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;
using MTM_Inventory_Application.Services;
using MySql.Data.MySqlClient;
using Color = System.Drawing.Color;

namespace MTM_Inventory_Application.Controls.MainForm;

public partial class Control_AdvancedInventory : UserControl
{
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public static Forms.MainForm.MainForm? MainFormInstance { get; set; }

    #region Constructor and Initialization

    public Control_AdvancedInventory()
    {
        try
        {
            LoggingUtility.Log("Control_AdvancedInventory constructor entered.");
            InitializeComponent();

            // Set tooltips for Single tab buttons using shortcut constants
            var toolTip = new ToolTip();
            toolTip.SetToolTip(AdvancedInventory_Single_Button_Send,
                $"Shortcut: {Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_AdvInv_Send)}");
            toolTip.SetToolTip(AdvancedInventory_Single_Button_Save,
                $"Shortcut: {Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_AdvInv_Save)}");
            toolTip.SetToolTip(AdvancedInventory_Single_Button_Reset,
                $"Shortcut: {Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_AdvInv_Reset)}");
            toolTip.SetToolTip(AdvancedInventory_Single_Button_Normal,
                $"Shortcut: {Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_AdvInv_Normal)}");
            // Set tooltips for MultiLoc tab buttons
            toolTip.SetToolTip(AdvancedInventory_MultiLoc_Button_AddLoc,
                $"Shortcut: {Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_AdvInv_Multi_AddLoc)}");
            toolTip.SetToolTip(AdvancedInventory_MultiLoc_Button_SaveAll,
                $"Shortcut: {Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_AdvInv_Multi_SaveAll)}");
            toolTip.SetToolTip(AdvancedInventory_MultiLoc_Button_Reset,
                $"Shortcut: {Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_AdvInv_Multi_Reset)}");
            toolTip.SetToolTip(AdvancedInventory_Multi_Button_Normal,
                $"Shortcut: {Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_AdvInv_Multi_Normal)}");
            // Set tooltips for Import tab buttons
            toolTip.SetToolTip(AdvancedInventory_Import_Button_OpenExcel,
                $"Shortcut: {Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_AdvInv_Import_OpenExcel)}");
            toolTip.SetToolTip(AdvancedInventory_Import_Button_ImportExcel,
                $"Shortcut: {Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_AdvInv_Import_ImportExcel)}");
            toolTip.SetToolTip(AdvancedInventory_Import_Button_Save,
                $"Shortcut: {Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_AdvInv_Import_Save)}");
            toolTip.SetToolTip(AdvancedInventory_Import_Button_Normal,
                $"Shortcut: {Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_AdvInv_Import_Normal)}");

            Helper_UI_ComboBoxes.ApplyStandardComboBoxProperties(AdvancedInventory_Single_ComboBox_Part);
            Helper_UI_ComboBoxes.ApplyStandardComboBoxProperties(AdvancedInventory_Single_ComboBox_Op);
            Helper_UI_ComboBoxes.ApplyStandardComboBoxProperties(AdvancedInventory_Single_ComboBox_Loc);
            Helper_UI_ComboBoxes.ApplyStandardComboBoxProperties(AdvancedInventory_MultiLoc_ComboBox_Part);
            Helper_UI_ComboBoxes.ApplyStandardComboBoxProperties(AdvancedInventory_MultiLoc_ComboBox_Op);
            Helper_UI_ComboBoxes.ApplyStandardComboBoxProperties(AdvancedInventory_MultiLoc_ComboBox_Loc);

            AdvancedInventory_Single_Button_Reset.TabStop = false;
            AdvancedInventory_MultiLoc_Button_Reset.TabStop = false;

            // Set ComboBox ForeColor to Red initially
            AdvancedInventory_Single_ComboBox_Part.ForeColor =
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
            AdvancedInventory_Single_ComboBox_Op.ForeColor =
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
            AdvancedInventory_Single_ComboBox_Loc.ForeColor =
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
            AdvancedInventory_MultiLoc_ComboBox_Part.ForeColor =
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
            AdvancedInventory_MultiLoc_ComboBox_Op.ForeColor =
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
            AdvancedInventory_MultiLoc_ComboBox_Loc.ForeColor =
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;

            WireUpEvents();

            AdvancedInventory_MultiLoc_ListView_Preview.View = View.Details;
            if (AdvancedInventory_MultiLoc_ListView_Preview.Columns.Count == 0)
            {
                AdvancedInventory_MultiLoc_ListView_Preview.Columns.Add("Part", 80);
                AdvancedInventory_MultiLoc_ListView_Preview.Columns.Add("Operation", 80);
                AdvancedInventory_MultiLoc_ListView_Preview.Columns.Add("Location", 150);
                AdvancedInventory_MultiLoc_ListView_Preview.Columns.Add("Quantity", 80);
            }

            AdvancedInventory_Single_ListView.View = View.Details;
            if (AdvancedInventory_Single_ListView.Columns.Count == 0)
            {
                AdvancedInventory_Single_ListView.Columns.Add("Part", 80);
                AdvancedInventory_Single_ListView.Columns.Add("Operation", 80);
                AdvancedInventory_Single_ListView.Columns.Add("Location", 100);
                AdvancedInventory_Single_ListView.Columns.Add("Quantity", 80);
            }

            if (AdvancedInventory_TabControl == null)
            {
                LoggingUtility.LogApplicationError(
                    new InvalidOperationException("TabControl 'AdvancedInventory_TabControl' not found."));
                throw new InvalidOperationException("TabControl 'AdvancedInventory_TabControl' not found.");
            }

            if (AdvancedInventory_TabControl_Import == null)
            {
                LoggingUtility.LogApplicationError(
                    new InvalidOperationException("Tab 'AdvancedInventory_TabControl_Import' not found."));
                throw new InvalidOperationException("Tab 'AdvancedInventory_TabControl_Import' not found.");
            }

            ValidateQtyTextBox(AdvancedInventory_Single_TextBox_Qty, "[ Enter Valid Quantity ]");
            ValidateQtyTextBox(AdvancedInventory_Single_TextBox_Count, "[ How Many Transactions ]");
            ValidateQtyTextBox(AdvancedInventory_MultiLoc_TextBox_Qty, "[ Enter Valid Quantity ]");


            Enter += (s, e) =>
            {
                if (AdvancedInventory_TabControl.SelectedIndex == 0 && AdvancedInventory_Single_ComboBox_Part.Visible &&
                    AdvancedInventory_Single_ComboBox_Part.Enabled)
                {
                    AdvancedInventory_Single_ComboBox_Part.Focus();
                    AdvancedInventory_Single_ComboBox_Part.SelectAll();
                }
                else if (AdvancedInventory_TabControl.SelectedIndex == 1 &&
                         AdvancedInventory_MultiLoc_ComboBox_Part.Visible &&
                         AdvancedInventory_MultiLoc_ComboBox_Part.Enabled)
                {
                    AdvancedInventory_MultiLoc_ComboBox_Part.Focus();
                    AdvancedInventory_MultiLoc_ComboBox_Part.SelectAll();
                }
            };
            Core_Themes.ApplyFocusHighlighting(this);
            LoggingUtility.Log("Control_AdvancedInventory constructor exited.");
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false, "Control_AdvancedInventory_Ctor");
        }
    }

    #endregion

    #region Form Load

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        try
        {
            await LoadAllComboBoxesAsync();
            AdvancedInventory_Single_ComboBox_Part.Visible = true;
            AdvancedInventory_Single_ComboBox_Op.Visible = true;
            AdvancedInventory_Single_ComboBox_Loc.Visible = true;
            AdvancedInventory_MultiLoc_ComboBox_Part.Visible = true;
            AdvancedInventory_MultiLoc_ComboBox_Op.Visible = true;
            AdvancedInventory_MultiLoc_ComboBox_Loc.Visible = true;
            // Ensure ForeColor is red after async load
            AdvancedInventory_Single_ComboBox_Part.ForeColor =
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
            AdvancedInventory_Single_ComboBox_Op.ForeColor =
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
            AdvancedInventory_Single_ComboBox_Loc.ForeColor =
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
            AdvancedInventory_MultiLoc_ComboBox_Part.ForeColor =
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
            AdvancedInventory_MultiLoc_ComboBox_Op.ForeColor =
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
            AdvancedInventory_MultiLoc_ComboBox_Loc.ForeColor =
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
            AdvancedInventory_Single_ComboBox_Part.Focus();
            Core_Themes.ApplyFocusHighlighting(this);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true, "Control_AdvancedInventory.OnLoad");
        }
    }

    #endregion

    #region ComboBox Loading

    private async Task LoadAllComboBoxesAsync()
    {
        try
        {
            await Helper_UI_ComboBoxes.FillPartComboBoxesAsync(AdvancedInventory_Single_ComboBox_Part);
            await Helper_UI_ComboBoxes.FillOperationComboBoxesAsync(AdvancedInventory_Single_ComboBox_Op);
            await Helper_UI_ComboBoxes.FillLocationComboBoxesAsync(AdvancedInventory_Single_ComboBox_Loc);
            await Helper_UI_ComboBoxes.FillPartComboBoxesAsync(AdvancedInventory_MultiLoc_ComboBox_Part);
            await Helper_UI_ComboBoxes.FillOperationComboBoxesAsync(AdvancedInventory_MultiLoc_ComboBox_Op);
            await Helper_UI_ComboBoxes.FillLocationComboBoxesAsync(AdvancedInventory_MultiLoc_ComboBox_Loc);

            LoggingUtility.Log("Control_AdvancedInventory ComboBoxes loaded.");
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true,
                "Control_AdvancedInventory_LoadAllComboBoxesAsync");
        }
    }

    #endregion

    #region Event Wiring

    private void WireUpEvents()
    {
        try
        {
            // ComboBox events
            AdvancedInventory_Single_ComboBox_Part.SelectedIndexChanged += (s, e) =>
            {
                Helper_UI_ComboBoxes.ValidateComboBoxItem(AdvancedInventory_Single_ComboBox_Part,
                    "[ Enter Part Number ]");
                UpdateSingleSaveButtonState();
                LoggingUtility.Log("Single Part ComboBox selection changed.");
            };
            AdvancedInventory_Single_ComboBox_Part.Leave += (s, e) =>
            {
                Helper_UI_ComboBoxes.ValidateComboBoxItem(AdvancedInventory_Single_ComboBox_Part,
                    "[ Enter Part Number ]");
            };

            AdvancedInventory_Single_ComboBox_Op.SelectedIndexChanged += (s, e) =>
            {
                Helper_UI_ComboBoxes.ValidateComboBoxItem(AdvancedInventory_Single_ComboBox_Op, "[ Enter Operation ]");
                UpdateSingleSaveButtonState();
                LoggingUtility.Log("Single Op ComboBox selection changed.");
            };
            AdvancedInventory_Single_ComboBox_Op.Leave += (s, e) =>
            {
                Helper_UI_ComboBoxes.ValidateComboBoxItem(AdvancedInventory_Single_ComboBox_Op,
                    "[ Enter Operation ]");
            };

            AdvancedInventory_Single_ComboBox_Loc.SelectedIndexChanged += (s, e) =>
            {
                Helper_UI_ComboBoxes.ValidateComboBoxItem(AdvancedInventory_Single_ComboBox_Loc, "[ Enter Location ]");
                UpdateSingleSaveButtonState();
                LoggingUtility.Log("Single Loc ComboBox selection changed.");
            };
            AdvancedInventory_Single_ComboBox_Loc.Leave += (s, e) =>
            {
                Helper_UI_ComboBoxes.ValidateComboBoxItem(AdvancedInventory_Single_ComboBox_Loc,
                    "[ Enter Location ]");
            };

            AdvancedInventory_MultiLoc_ComboBox_Part.SelectedIndexChanged += (s, e) =>
            {
                Helper_UI_ComboBoxes.ValidateComboBoxItem(AdvancedInventory_MultiLoc_ComboBox_Part,
                    "[ Enter Part Number ]");
                UpdateMultiSaveButtonState();
                LoggingUtility.Log("Multi Part ComboBox selection changed.");
            };
            AdvancedInventory_MultiLoc_ComboBox_Part.Leave += (s, e) =>
            {
                Helper_UI_ComboBoxes.ValidateComboBoxItem(AdvancedInventory_MultiLoc_ComboBox_Part,
                    "[ Enter Part Number ]");
            };

            AdvancedInventory_MultiLoc_ComboBox_Op.SelectedIndexChanged += (s, e) =>
            {
                Helper_UI_ComboBoxes.ValidateComboBoxItem(AdvancedInventory_MultiLoc_ComboBox_Op,
                    "[ Enter Operation ]");
                UpdateMultiSaveButtonState();
                LoggingUtility.Log("Multi Op ComboBox selection changed.");
            };
            AdvancedInventory_MultiLoc_ComboBox_Op.Leave += (s, e) =>
            {
                Helper_UI_ComboBoxes.ValidateComboBoxItem(AdvancedInventory_MultiLoc_ComboBox_Op,
                    "[ Enter Operation ]");
            };

            AdvancedInventory_MultiLoc_ComboBox_Loc.SelectedIndexChanged += (s, e) =>
            {
                Helper_UI_ComboBoxes.ValidateComboBoxItem(AdvancedInventory_MultiLoc_ComboBox_Loc,
                    "[ Enter Location ]");
                UpdateMultiSaveButtonState();
                LoggingUtility.Log("Multi Loc ComboBox selection changed.");
            };
            AdvancedInventory_MultiLoc_ComboBox_Loc.Leave += (s, e) =>
            {
                Helper_UI_ComboBoxes.ValidateComboBoxItem(AdvancedInventory_MultiLoc_ComboBox_Loc,
                    "[ Enter Location ]");
            };

            // TextBox events
            AdvancedInventory_Single_TextBox_Qty.Text = "[ Enter Valid Quantity ]";
            AdvancedInventory_Single_TextBox_Qty.TextChanged += (s, e) =>
            {
                InventoryTextBoxQty_TextChanged(AdvancedInventory_Single_TextBox_Qty, "[ Enter Valid Quantity ]");
                ValidateQtyTextBox(AdvancedInventory_Single_TextBox_Qty, "[ Enter Valid Quantity ]");
                UpdateSingleSaveButtonState();
                LoggingUtility.Log("Single Qty TextBox changed.");
            };
            AdvancedInventory_Single_TextBox_Qty.Leave += (s, e) =>
            {
                ValidateQtyTextBox(AdvancedInventory_Single_TextBox_Qty, "[ Enter Valid Quantity ]");
            };

            AdvancedInventory_Single_TextBox_Qty.Enter += (s, e) => AdvancedInventory_Single_TextBox_Qty.SelectAll();
            AdvancedInventory_Single_TextBox_Qty.Click += (s, e) => AdvancedInventory_Single_TextBox_Qty.SelectAll();
            AdvancedInventory_Single_TextBox_Qty.KeyDown += (sender, e) =>
            {
                MainFormControlHelper.AdjustQuantityByKey_Quantity(sender, e, "[ Enter Valid Quantity ]",
                    Model_AppVariables.UserUiColors.TextBoxForeColor ?? Color.Black,
                    Model_AppVariables.UserUiColors.TextBoxErrorForeColor ?? Color.Red);
            };

            AdvancedInventory_Single_TextBox_Count.Text = "[ How Many Transactions ]";
            AdvancedInventory_Single_TextBox_Count.TextChanged += (s, e) =>
            {
                ValidateQtyTextBox(AdvancedInventory_Single_TextBox_Count, "[ How Many Transactions ]");
                UpdateSingleSaveButtonState();
                LoggingUtility.Log("Single Count TextBox changed.");
            };
            AdvancedInventory_Single_TextBox_Count.Leave += (s, e) =>
            {
                ValidateQtyTextBox(AdvancedInventory_Single_TextBox_Count, "[ How Many Transactions ]");
            };

            AdvancedInventory_Single_TextBox_Count.Enter +=
                (s, e) => AdvancedInventory_Single_TextBox_Count.SelectAll();
            AdvancedInventory_Single_TextBox_Count.Click +=
                (s, e) => AdvancedInventory_Single_TextBox_Count.SelectAll();
            AdvancedInventory_Single_TextBox_Count.KeyDown += (sender, e) =>
            {
                MainFormControlHelper.AdjustQuantityByKey_Transfers(sender, e, "[ How Many Transactions ]",
                    Model_AppVariables.UserUiColors.TextBoxForeColor ?? Color.Black,
                    Model_AppVariables.UserUiColors.TextBoxErrorForeColor ?? Color.Red);
            };

            AdvancedInventory_MultiLoc_TextBox_Qty.Text = "[ Enter Valid Quantity ]";
            AdvancedInventory_MultiLoc_TextBox_Qty.TextChanged += (s, e) =>
            {
                ValidateQtyTextBox(AdvancedInventory_MultiLoc_TextBox_Qty, "[ Enter Valid Quantity ]");
                UpdateMultiSaveButtonState();
                LoggingUtility.Log("MultiLoc Qty TextBox changed.");
            };
            AdvancedInventory_MultiLoc_TextBox_Qty.Leave += (s, e) =>
            {
                ValidateQtyTextBox(AdvancedInventory_MultiLoc_TextBox_Qty, "[ Enter Valid Quantity ]");
            };
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                "Control_AdvancedInventory_WireUpEvents");
        }
    }

    #endregion

    #region Validation and Utility Methods

    private static void InventoryTextBoxQty_TextChanged(TextBox textBox, string placeholder)
    {
        try
        {
            var text = textBox.Text.Trim();
            var isValid = int.TryParse(text, out var qty) && qty > 0;
            if (isValid)
            {
                textBox.ForeColor = Model_AppVariables.UserUiColors.TextBoxForeColor ?? Color.Black;
            }
            else
            {
                textBox.ForeColor = Model_AppVariables.UserUiColors.TextBoxErrorForeColor ?? Color.Red;
                if (text != placeholder)
                {
                    textBox.Text = placeholder;
                    textBox.SelectionStart = textBox.Text.Length;
                }
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
        }
    }

    private void UpdateSingleSaveButtonState()
    {
        AdvancedInventory_Single_Button_Save.Enabled = AdvancedInventory_Single_ListView.Items.Count > 0;
        var partValid = AdvancedInventory_Single_ComboBox_Part.SelectedIndex > 0 &&
                        !string.IsNullOrWhiteSpace(AdvancedInventory_Single_ComboBox_Part.Text);
        var opValid = AdvancedInventory_Single_ComboBox_Op.SelectedIndex > 0 &&
                      !string.IsNullOrWhiteSpace(AdvancedInventory_Single_ComboBox_Op.Text);
        var locValid = AdvancedInventory_Single_ComboBox_Loc.SelectedIndex > 0 &&
                       !string.IsNullOrWhiteSpace(AdvancedInventory_Single_ComboBox_Loc.Text);
        var qtyValid = int.TryParse(AdvancedInventory_Single_TextBox_Qty.Text.Trim(), out var qty) && qty > 0;
        var countValid = int.TryParse(AdvancedInventory_Single_TextBox_Count.Text.Trim(), out var count) && count > 0;

        AdvancedInventory_Single_Button_Send.Enabled = partValid && opValid && locValid && qtyValid && countValid;
    }

    private void UpdateMultiSaveButtonState()
    {
        var partValid = AdvancedInventory_MultiLoc_ComboBox_Part.SelectedIndex > 0 &&
                        !string.IsNullOrWhiteSpace(AdvancedInventory_MultiLoc_ComboBox_Part.Text);
        var opValid = AdvancedInventory_MultiLoc_ComboBox_Op.SelectedIndex > 0 &&
                      !string.IsNullOrWhiteSpace(AdvancedInventory_MultiLoc_ComboBox_Op.Text);
        var locValid = AdvancedInventory_MultiLoc_ComboBox_Loc.SelectedIndex > 0 &&
                       !string.IsNullOrWhiteSpace(AdvancedInventory_MultiLoc_ComboBox_Loc.Text);
        var qtyValid = int.TryParse(AdvancedInventory_MultiLoc_TextBox_Qty.Text.Trim(), out var qty) && qty > 0;
        AdvancedInventory_MultiLoc_Button_AddLoc.Enabled = partValid && opValid && locValid && qtyValid;
        AdvancedInventory_MultiLoc_Button_SaveAll.Enabled =
            AdvancedInventory_MultiLoc_ListView_Preview.Items.Count > 0 && partValid && opValid;
    }

    public static void ValidateQtyTextBox(TextBox textBox, string placeholder)
    {
        var text = textBox.Text.Trim();
        var isValid = int.TryParse(text, out var value) && value > 0;
        if (isValid)
        {
            textBox.ForeColor = Model_AppVariables.UserUiColors.TextBoxForeColor ?? Color.Black;
        }
        else
        {
            textBox.ForeColor = Model_AppVariables.UserUiColors.TextBoxErrorForeColor ?? Color.Red;
            if (text != placeholder)
            {
                textBox.Text = placeholder;
                textBox.SelectionStart = textBox.Text.Length;
            }
        }
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        try
        {
            // Only handle shortcuts if the Single tab is selected
            if (AdvancedInventory_TabControl.SelectedTab == AdvancedInventory_TabControl_Single)
            {
                if (keyData == Core_WipAppVariables.Shortcut_AdvInv_Send)
                    if (AdvancedInventory_Single_Button_Send.Visible && AdvancedInventory_Single_Button_Send.Enabled)
                    {
                        AdvancedInventory_Single_Button_Send.PerformClick();
                        return true;
                    }

                if (keyData == Core_WipAppVariables.Shortcut_AdvInv_Save)
                    if (AdvancedInventory_Single_Button_Save.Visible && AdvancedInventory_Single_Button_Save.Enabled)
                    {
                        AdvancedInventory_Single_Button_Save.PerformClick();
                        return true;
                    }

                if (keyData == Core_WipAppVariables.Shortcut_AdvInv_Reset)
                    if (AdvancedInventory_Single_Button_Reset.Visible && AdvancedInventory_Single_Button_Reset.Enabled)
                    {
                        AdvancedInventory_Single_Button_Reset.PerformClick();
                        return true;
                    }

                if (keyData == Core_WipAppVariables.Shortcut_AdvInv_Normal)
                    if (AdvancedInventory_Single_Button_Normal.Visible &&
                        AdvancedInventory_Single_Button_Normal.Enabled)
                    {
                        AdvancedInventory_Single_Button_Normal.PerformClick();
                        return true;
                    }
            }

            // MultiLoc tab
            if (AdvancedInventory_TabControl.SelectedTab == AdvancedInventory_TabControl_MultiLoc)
            {
                if (keyData == Core_WipAppVariables.Shortcut_AdvInv_Multi_AddLoc)
                    if (AdvancedInventory_MultiLoc_Button_AddLoc.Visible &&
                        AdvancedInventory_MultiLoc_Button_AddLoc.Enabled)
                    {
                        AdvancedInventory_MultiLoc_Button_AddLoc.PerformClick();
                        return true;
                    }

                if (keyData == Core_WipAppVariables.Shortcut_AdvInv_Multi_SaveAll)
                    if (AdvancedInventory_MultiLoc_Button_SaveAll.Visible &&
                        AdvancedInventory_MultiLoc_Button_SaveAll.Enabled)
                    {
                        AdvancedInventory_MultiLoc_Button_SaveAll.PerformClick();
                        return true;
                    }

                if (keyData == Core_WipAppVariables.Shortcut_AdvInv_Multi_Reset)
                    if (AdvancedInventory_MultiLoc_Button_Reset.Visible &&
                        AdvancedInventory_MultiLoc_Button_Reset.Enabled)
                    {
                        AdvancedInventory_MultiLoc_Button_Reset.PerformClick();
                        return true;
                    }

                if (keyData == Core_WipAppVariables.Shortcut_AdvInv_Multi_Normal)
                    if (AdvancedInventory_Multi_Button_Normal.Visible && AdvancedInventory_Multi_Button_Normal.Enabled)
                    {
                        AdvancedInventory_Multi_Button_Normal.PerformClick();
                        return true;
                    }
            }

            // Import tab
            if (AdvancedInventory_TabControl.SelectedTab == AdvancedInventory_TabControl_Import)
            {
                if (keyData == Core_WipAppVariables.Shortcut_AdvInv_Import_OpenExcel)
                    if (AdvancedInventory_Import_Button_OpenExcel.Visible &&
                        AdvancedInventory_Import_Button_OpenExcel.Enabled)
                    {
                        AdvancedInventory_Import_Button_OpenExcel.PerformClick();
                        return true;
                    }

                if (keyData == Core_WipAppVariables.Shortcut_AdvInv_Import_ImportExcel)
                    if (AdvancedInventory_Import_Button_ImportExcel.Visible &&
                        AdvancedInventory_Import_Button_ImportExcel.Enabled)
                    {
                        AdvancedInventory_Import_Button_ImportExcel.PerformClick();
                        return true;
                    }

                if (keyData == Core_WipAppVariables.Shortcut_AdvInv_Import_Save)
                    if (AdvancedInventory_Import_Button_Save.Visible && AdvancedInventory_Import_Button_Save.Enabled)
                    {
                        AdvancedInventory_Import_Button_Save.PerformClick();
                        return true;
                    }

                if (keyData == Core_WipAppVariables.Shortcut_AdvInv_Import_Normal)
                    if (AdvancedInventory_Import_Button_Normal.Visible &&
                        AdvancedInventory_Import_Button_Normal.Enabled)
                    {
                        AdvancedInventory_Import_Button_Normal.PerformClick();
                        return true;
                    }
            }

            // Remove Advanced shortcut (global for this form)
            if (keyData == Core_WipAppVariables.Shortcut_Remove_Advanced)
                if (MainFormInstance != null && MainFormInstance.MainForm_Control_AdvancedRemove != null)
                {
                    MainFormInstance.MainForm_AdvancedInventory.Visible = false;
                    MainFormInstance.MainForm_Control_AdvancedRemove.Visible = true;
                    MainFormInstance.MainForm_TabControl.SelectedIndex = 2; // Assuming Remove tab index is 2
                    return true;
                }

            if (keyData == Keys.Enter)
            {
                SelectNextControl(ActiveControl, true, true, true, true);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                "Control_AdvancedInventory_ProcessCmdKey");
            return false;
        }
    }

    #endregion

    #region Single Entry Actions

    private async Task AdvancedInventory_Single_HardResetAsync()
    {
        AdvancedInventory_Single_Button_Reset.Enabled = false;
        try
        {
            MainFormInstance?.TabLoadingProgress?.ShowProgress();
            MainFormInstance?.TabLoadingProgress?.UpdateProgress(10, "Resetting Advanced Inventory (Single)...");
            Debug.WriteLine("[DEBUG] AdvancedInventory Single HardReset - start");

            // Update status strip to show reset is in progress
            if (MainFormInstance != null)
            {
                Debug.WriteLine("[DEBUG] Updating status strip for hard reset");
                MainFormInstance.MainForm_StatusStrip_Disconnected.Text = @"Please wait while resetting...";
                MainFormInstance.MainForm_StatusStrip_Disconnected.Visible = true;
                MainFormInstance.MainForm_StatusStrip_SavedStatus.Visible = false;
            }

            MainFormInstance?.TabLoadingProgress?.UpdateProgress(30, "Resetting data tables...");
            // Hide controls during reset
            Debug.WriteLine("[DEBUG] Hiding ComboBoxes");
            AdvancedInventory_Single_ComboBox_Part.Visible = false;
            AdvancedInventory_Single_ComboBox_Op.Visible = false;
            AdvancedInventory_Single_ComboBox_Loc.Visible = false;

            // Unbind DataSource before DataTable reset
            Debug.WriteLine("[DEBUG] Unbinding ComboBox DataSources");
            AdvancedInventory_Single_ComboBox_Part.DataSource = null;
            AdvancedInventory_Single_ComboBox_Op.DataSource = null;
            AdvancedInventory_Single_ComboBox_Loc.DataSource = null;

            // Reset the DataTables and reinitialize them
            Debug.WriteLine("[DEBUG] Resetting and refreshing all ComboBox DataTables");
            await Helper_UI_ComboBoxes.ResetAndRefreshAllDataTablesAsync();
            Debug.WriteLine("[DEBUG] DataTables reset complete");

            MainFormInstance?.TabLoadingProgress?.UpdateProgress(60, "Refilling combo boxes...");
            // Refill each combobox with proper data
            Debug.WriteLine("[DEBUG] Refilling Part ComboBox");
            await Helper_UI_ComboBoxes.FillPartComboBoxesAsync(AdvancedInventory_Single_ComboBox_Part);
            Debug.WriteLine("[DEBUG] Refilling Operation ComboBox");
            await Helper_UI_ComboBoxes.FillOperationComboBoxesAsync(AdvancedInventory_Single_ComboBox_Op);
            Debug.WriteLine("[DEBUG] Refilling Location ComboBox");
            await Helper_UI_ComboBoxes.FillLocationComboBoxesAsync(AdvancedInventory_Single_ComboBox_Loc);

            // Reset UI fields
            Debug.WriteLine("[DEBUG] Resetting UI fields");
            MainFormControlHelper.ResetComboBox(AdvancedInventory_MultiLoc_ComboBox_Part,
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);
            MainFormControlHelper.ResetComboBox(AdvancedInventory_MultiLoc_ComboBox_Op,
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);
            MainFormControlHelper.ResetComboBox(AdvancedInventory_MultiLoc_ComboBox_Loc,
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);
            MainFormControlHelper.ResetTextBox(AdvancedInventory_MultiLoc_TextBox_Qty,
                Model_AppVariables.UserUiColors.TextBoxErrorForeColor ?? Color.Red,
                "[ Enter Valid Quantity ]");
            MainFormControlHelper.ResetComboBox(AdvancedInventory_Single_ComboBox_Part,
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);
            MainFormControlHelper.ResetComboBox(AdvancedInventory_Single_ComboBox_Op,
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);
            MainFormControlHelper.ResetComboBox(AdvancedInventory_Single_ComboBox_Loc,
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);
            MainFormControlHelper.ResetTextBox(AdvancedInventory_Single_TextBox_Qty,
                Model_AppVariables.UserUiColors.TextBoxErrorForeColor ?? Color.Red,
                "[ Enter Valid Quantity ]");
            MainFormControlHelper.ResetTextBox(AdvancedInventory_Single_TextBox_Count,
                Model_AppVariables.UserUiColors.TextBoxErrorForeColor ?? Color.Red,
                "[ Enter How Many Times ]");
            AdvancedInventory_Single_RichTextBox_Notes.Text = string.Empty;
            AdvancedInventory_Single_ListView.Items.Clear();

            // Restore controls and focus
            Debug.WriteLine("[DEBUG] Restoring ComboBox visibility and focus");
            AdvancedInventory_Single_ComboBox_Part.Visible = true;
            AdvancedInventory_Single_ComboBox_Op.Visible = true;
            AdvancedInventory_Single_ComboBox_Loc.Visible = true;
            AdvancedInventory_Single_ComboBox_Part.Focus();

            // Update Save/Send button state
            UpdateSingleSaveButtonState();

            Debug.WriteLine("[DEBUG] AdvancedInventory Single HardReset - end");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[ERROR] Exception in AdvancedInventory Single HardReset: {ex}");
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                "AdvancedInventory_Single_HardResetAsync");
        }
        finally
        {
            Debug.WriteLine("[DEBUG] AdvancedInventory Single HardReset button re-enabled");
            AdvancedInventory_Single_Button_Reset.Enabled = true;
            if (MainFormInstance != null)
            {
                Debug.WriteLine("[DEBUG] Restoring status strip after reset");
                MainFormInstance.MainForm_StatusStrip_Disconnected.Visible = false;
                MainFormInstance.MainForm_StatusStrip_SavedStatus.Visible = true;
                MainFormInstance.MainForm_StatusStrip_Disconnected.Text =
                    @"Disconnected from Server, please standby...";
                MainFormInstance.TabLoadingProgress?.HideProgress();
            }
        }
    }

    private void AdvancedInventory_Single_SoftReset()
    {
        AdvancedInventory_Single_Button_Reset.Enabled = false;
        try
        {
            if (MainFormInstance != null)
            {
                Debug.WriteLine("[DEBUG] Updating status strip for Soft Reset");
                MainFormInstance.MainForm_StatusStrip_Disconnected.Text = @"Please wait while resetting...";
                MainFormInstance.MainForm_StatusStrip_Disconnected.Visible = true;
                MainFormInstance.MainForm_StatusStrip_SavedStatus.Visible = false;
            }

            // Reset UI fields
            Debug.WriteLine("[DEBUG] Resetting UI fields");
            MainFormControlHelper.ResetComboBox(AdvancedInventory_MultiLoc_ComboBox_Part,
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);
            MainFormControlHelper.ResetComboBox(AdvancedInventory_MultiLoc_ComboBox_Op,
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);
            MainFormControlHelper.ResetComboBox(AdvancedInventory_MultiLoc_ComboBox_Loc,
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);
            MainFormControlHelper.ResetTextBox(AdvancedInventory_MultiLoc_TextBox_Qty,
                Model_AppVariables.UserUiColors.TextBoxErrorForeColor ?? Color.Red,
                "[ Enter Valid Quantity ]");
            MainFormControlHelper.ResetComboBox(AdvancedInventory_Single_ComboBox_Part,
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);
            MainFormControlHelper.ResetComboBox(AdvancedInventory_Single_ComboBox_Op,
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);
            MainFormControlHelper.ResetComboBox(AdvancedInventory_Single_ComboBox_Loc,
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);
            MainFormControlHelper.ResetTextBox(AdvancedInventory_Single_TextBox_Qty,
                Model_AppVariables.UserUiColors.TextBoxErrorForeColor ?? Color.Red,
                "[ Enter Valid Quantity ]");
            MainFormControlHelper.ResetTextBox(AdvancedInventory_Single_TextBox_Count,
                Model_AppVariables.UserUiColors.TextBoxErrorForeColor ?? Color.Red,
                "[ Enter How Many Times ]");
            AdvancedInventory_Single_RichTextBox_Notes.Text = string.Empty;
            AdvancedInventory_Single_ListView.Items.Clear();

            // Update Save/Send button state
            UpdateSingleSaveButtonState();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[ERROR] Exception in AdvancedInventory Single SoftReset: {ex}");
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                "AdvancedInventory_Single_SoftReset");
        }
        finally
        {
            Debug.WriteLine("[DEBUG] AdvancedInventory Single SoftReset button re-enabled");
            AdvancedInventory_Single_Button_Reset.Enabled = true;
            AdvancedInventory_Single_ComboBox_Part.Focus();
            if (MainFormInstance != null)
            {
                MainFormInstance.MainForm_StatusStrip_Disconnected.Visible = false;
                MainFormInstance.MainForm_StatusStrip_SavedStatus.Visible = true;
                MainFormInstance.MainForm_StatusStrip_Disconnected.Text =
                    @"Disconnected from Server, please standby...";
            }
        }
    }

    private async void AdvancedInventory_Single_Button_Reset_Click(object? sender, EventArgs e)
    {
        AdvancedInventory_Single_Button_Reset.Enabled = false;
        try
        {
            // Check if Shift key is held down
            if ((ModifierKeys & Keys.Shift) == Keys.Shift)
                await AdvancedInventory_Single_HardResetAsync();
            else
                AdvancedInventory_Single_SoftReset();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[ERROR] Exception in AdvancedInventory Single Reset: {ex}");
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                "AdvancedInventory_Single_Button_Reset_Click");
        }
    }

    private async void AdvancedInventory_Single_Button_Save_Click(object? sender, EventArgs e)
    {
        try
        {
            LoggingUtility.Log("AdvancedInventory_Single_Button_Save_Click entered.");

            // Only process if there are items in the ListView
            if (AdvancedInventory_Single_ListView.Items.Count == 0)
            {
                MessageBox.Show("No items to inventory. Please add at least one item to the list.", @"No Items",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var partIds = new HashSet<string>();
            var operations = new HashSet<string>();
            var locations = new HashSet<string>();
            var totalQty = 0;
            var savedCount = 0;
            foreach (ListViewItem item in AdvancedInventory_Single_ListView.Items)
            {
                var partId = item.SubItems.Count > 0 ? item.SubItems[0].Text : "";
                var op = item.SubItems.Count > 1 ? item.SubItems[1].Text : "";
                var loc = item.SubItems.Count > 2 ? item.SubItems[2].Text : "";
                var qtyText = item.SubItems.Count > 3 ? item.SubItems[3].Text : "";
                var notes = item.SubItems.Count > 4 ? item.SubItems[4].Text : "";

                if (string.IsNullOrWhiteSpace(partId) || string.IsNullOrWhiteSpace(op) ||
                    string.IsNullOrWhiteSpace(loc) || !int.TryParse(qtyText, out var qty) || qty <= 0)
                {
                    LoggingUtility.LogApplicationError(new Exception(
                        $"Invalid data in ListView item: Part={partId}, Op={op}, Loc={loc}, Qty={qtyText}"));
                    continue;
                }

                Model_AppVariables.PartId = partId;
                Model_AppVariables.Operation = op;
                Model_AppVariables.Location = loc;
                Model_AppVariables.Notes = notes;
                Model_AppVariables.InventoryQuantity = qty;
                Model_AppVariables.User ??= Environment.UserName;
                Model_AppVariables.PartType ??= "";

                await Dao_Inventory.AddInventoryItemAsync(
                    partId,
                    loc,
                    op,
                    qty,
                    Model_AppVariables.PartType ?? "",
                    Model_AppVariables.User,
                    "", // batchNumber
                    notes,
                    true);

                partIds.Add(partId);
                operations.Add(op);
                locations.Add(loc);
                totalQty += qty;
                savedCount++;
            }

            MessageBox.Show(
                $@"{savedCount} inventory transaction(s) saved successfully.",
                @"Success",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            LoggingUtility.Log(
                $"Saved {savedCount} inventory transaction(s) from ListView.");

            // Update status strip
            if (MainFormInstance != null && savedCount > 0)
            {
                var time = DateTime.Now.ToString("hh:mm tt");
                var locDisplay = locations.Count > 1 ? "Multiple Locations" : locations.FirstOrDefault() ?? "";
                if (partIds.Count == 1 && operations.Count == 1)
                    MainFormInstance.MainForm_StatusStrip_SavedStatus.Text =
                        $"Last Inventoried: {partIds.First()} (Op: {operations.First()}), Location: {locDisplay}, Quantity: {totalQty} @ {time}";
                else if (partIds.Count == 1 && operations.Count > 1)
                    MainFormInstance.MainForm_StatusStrip_SavedStatus.Text =
                        $"Last Inventoried: {partIds.First()} (Multiple Ops), Location: {locDisplay}, Quantity: {totalQty} @ {time}";
                else
                    MainFormInstance.MainForm_StatusStrip_SavedStatus.Text =
                        $"Last Inventoried: Multiple Part IDs, Location: {locDisplay}, Quantity: Multiple @ {time}";
            }

            // Optionally reset the form after save
            AdvancedInventory_Single_Button_Reset_Click(null, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true,
                "AdvancedInventory_Single_Button_Save_Click");
        }
    }

    private void AdvancedInventory_Single_Button_Send_Click(object sender, EventArgs e)
    {
        try
        {
            Debug.WriteLine("Send button clicked");
            LoggingUtility.Log("Send button clicked");

            // Get values from controls
            var partId = AdvancedInventory_Single_ComboBox_Part.Text;
            var op = AdvancedInventory_Single_ComboBox_Op.Text;
            var loc = AdvancedInventory_Single_ComboBox_Loc.Text;
            var qtyText = AdvancedInventory_Single_TextBox_Qty.Text.Trim();
            var countText = AdvancedInventory_Single_TextBox_Count.Text.Trim();
            var notes = AdvancedInventory_Single_RichTextBox_Notes.Text.Trim();

            Debug.WriteLine($"partId: {partId}, op: {op}, loc: {loc}, qtyText: {qtyText}, countText: {countText}");

            // Validate input
            if (string.IsNullOrWhiteSpace(partId) || AdvancedInventory_Single_ComboBox_Part.SelectedIndex <= 0)
            {
                MessageBox.Show(@"Please select a valid Part.", @"Validation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                AdvancedInventory_Single_ComboBox_Part.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(op) || AdvancedInventory_Single_ComboBox_Op.SelectedIndex <= 0)
            {
                MessageBox.Show(@"Please select a valid Operation.", @"Validation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                AdvancedInventory_Single_ComboBox_Op.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(loc) || AdvancedInventory_Single_ComboBox_Loc.SelectedIndex <= 0)
            {
                MessageBox.Show(@"Please select a valid Location.", @"Validation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                AdvancedInventory_Single_ComboBox_Loc.Focus();
                return;
            }

            if (!int.TryParse(qtyText, out var qty) || qty <= 0)
            {
                MessageBox.Show(@"Please enter a valid quantity.", @"Validation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                AdvancedInventory_Single_TextBox_Qty.Focus();
                return;
            }

            if (!int.TryParse(countText, out var count) || count <= 0)
            {
                MessageBox.Show(@"Please enter a valid transaction count.", @"Validation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                AdvancedInventory_Single_TextBox_Count.Focus();
                return;
            }

            // Add the specified number of entries to the ListView
            for (var i = 0; i < count; i++)
            {
                var listViewItem = new ListViewItem(new[]
                {
                    partId,
                    op,
                    loc,
                    qty.ToString()
                });
                AdvancedInventory_Single_ListView.Items.Add(listViewItem);
                Debug.WriteLine(
                    $"Added item to ListView: Part={partId}, Op={op}, Loc={loc}, Qty={qty}, Notes={notes}");
            }

            // Optionally clear the fields after sending
            MainFormControlHelper.ResetComboBox(AdvancedInventory_Single_ComboBox_Part,
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);
            MainFormControlHelper.ResetComboBox(AdvancedInventory_Single_ComboBox_Op,
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);
            MainFormControlHelper.ResetComboBox(AdvancedInventory_Single_ComboBox_Loc,
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);
            MainFormControlHelper.ResetTextBox(AdvancedInventory_Single_TextBox_Qty,
                Model_AppVariables.UserUiColors.TextBoxErrorForeColor ?? Color.Red,
                "[ Enter Valid Quantity ]");
            MainFormControlHelper.ResetTextBox(AdvancedInventory_Single_TextBox_Count,
                Model_AppVariables.UserUiColors.TextBoxErrorForeColor ?? Color.Red,
                "[ Enter How Many Times ]");
            AdvancedInventory_Single_RichTextBox_Notes.Text = string.Empty;

            AdvancedInventory_Single_ComboBox_Part.Focus();

            // Update Save button state
            UpdateSingleSaveButtonState();
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                "AdvancedInventory_Single_Button_Send_Click");
        }
    }

    private static void AdvancedInventory_Button_Normal_Click(object? sender, EventArgs e)
    {
        try
        {
            if (Service_Timer_VersionChecker.MainFormInstance == null)
            {
                LoggingUtility.Log("MainForm instance is null, cannot open Advanced Inventory Entry.");
                return;
            }

            if (MainFormInstance != null)
            {
                MainFormInstance.MainForm_Control_InventoryTab.Visible = true;
                MainFormInstance.MainForm_AdvancedInventory.Visible = false;
                // Set MainForm_TabControl.SelectedIndex = 0
                MainFormInstance.MainForm_TabControl.SelectedIndex = 0;
                // Set all InventoryTab ComboBoxes' SelectedIndex = 0 and focus on Part ComboBox
                var invTab = MainFormInstance.MainForm_Control_InventoryTab;
                if (invTab != null)
                {
                    var part = invTab.Control_InventoryTab_ComboBox_Part;
                    var op = invTab.GetType().GetField("Control_InventoryTab_ComboBox_Operation",
                            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                        ?.GetValue(invTab) as ComboBox;
                    var loc = invTab.GetType().GetField("Control_InventoryTab_ComboBox_Location",
                            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                        ?.GetValue(invTab) as ComboBox;
                    if (part != null)
                    {
                        part.SelectedIndex = 0;
                        part.Focus();
                        part.SelectAll();
                        part.BackColor = Model_AppVariables.UserUiColors.ControlFocusedBackColor ?? Color.LightBlue;
                    }

                    if (op != null) op.SelectedIndex = 0;
                    if (loc != null) loc.SelectedIndex = 0;
                }
            }
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                "Control_InventoryTab_Button_AdvancedInventory_Click");
        }
    }

    #endregion

    #region Multi-Location Actions

    private async Task AdvancedInventory_MultiLoc_HardResetAsync()
    {
        AdvancedInventory_MultiLoc_Button_Reset.Enabled = false;
        try
        {
            MainFormInstance?.TabLoadingProgress?.ShowProgress();
            MainFormInstance?.TabLoadingProgress?.UpdateProgress(10, "Resetting Advanced Inventory (MultiLoc)...");
            Debug.WriteLine("[DEBUG] AdvancedInventory MultiLoc HardReset - start");
            // Update status strip to show reset is in progress
            if (MainFormInstance != null)
            {
                Debug.WriteLine("[DEBUG] Updating status strip for hard reset");
                MainFormInstance.MainForm_StatusStrip_Disconnected.Text = @"Please wait while resetting...";
                MainFormInstance.MainForm_StatusStrip_Disconnected.Visible = true;
                MainFormInstance.MainForm_StatusStrip_SavedStatus.Visible = false;
            }

            MainFormInstance?.TabLoadingProgress?.UpdateProgress(30, "Resetting data tables...");
            // Hide controls during reset
            Debug.WriteLine("[DEBUG] Hiding ComboBoxes");
            AdvancedInventory_MultiLoc_ComboBox_Part.Visible = false;
            AdvancedInventory_MultiLoc_ComboBox_Op.Visible = false;
            AdvancedInventory_MultiLoc_ComboBox_Loc.Visible = false;

            // Unbind DataSource before DataTable reset
            Debug.WriteLine("[DEBUG] Unbinding ComboBox DataSources");
            AdvancedInventory_MultiLoc_ComboBox_Part.DataSource = null;
            AdvancedInventory_MultiLoc_ComboBox_Op.DataSource = null;
            AdvancedInventory_MultiLoc_ComboBox_Loc.DataSource = null;

            // Reset the DataTables and reinitialize them
            Debug.WriteLine("[DEBUG] Resetting and refreshing all ComboBox DataTables");
            await Helper_UI_ComboBoxes.ResetAndRefreshAllDataTablesAsync();
            Debug.WriteLine("[DEBUG] DataTables reset complete");

            MainFormInstance?.TabLoadingProgress?.UpdateProgress(60, "Refilling combo boxes...");
            // Refill each combobox with proper data
            Debug.WriteLine("[DEBUG] Refilling Part ComboBox");
            await Helper_UI_ComboBoxes.FillPartComboBoxesAsync(AdvancedInventory_MultiLoc_ComboBox_Part);
            Debug.WriteLine("[DEBUG] Refilling Operation ComboBox");
            await Helper_UI_ComboBoxes.FillOperationComboBoxesAsync(AdvancedInventory_MultiLoc_ComboBox_Op);
            Debug.WriteLine("[DEBUG] Refilling Location ComboBox");
            await Helper_UI_ComboBoxes.FillLocationComboBoxesAsync(AdvancedInventory_MultiLoc_ComboBox_Loc);

            // Reset UI fields
            Debug.WriteLine("[DEBUG] Resetting UI fields");
            MainFormControlHelper.ResetComboBox(AdvancedInventory_MultiLoc_ComboBox_Part,
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);
            MainFormControlHelper.ResetComboBox(AdvancedInventory_MultiLoc_ComboBox_Op,
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);
            MainFormControlHelper.ResetComboBox(AdvancedInventory_MultiLoc_ComboBox_Loc,
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);
            MainFormControlHelper.ResetTextBox(AdvancedInventory_MultiLoc_TextBox_Qty,
                Model_AppVariables.UserUiColors.TextBoxErrorForeColor ?? Color.Red,
                "[ Enter Valid Quantity ]");
            MainFormControlHelper.ResetComboBox(AdvancedInventory_Single_ComboBox_Part,
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);
            MainFormControlHelper.ResetComboBox(AdvancedInventory_Single_ComboBox_Op,
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);
            MainFormControlHelper.ResetComboBox(AdvancedInventory_Single_ComboBox_Loc,
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);
            MainFormControlHelper.ResetTextBox(AdvancedInventory_Single_TextBox_Qty,
                Model_AppVariables.UserUiColors.TextBoxErrorForeColor ?? Color.Red,
                "[ Enter Valid Quantity ]");
            MainFormControlHelper.ResetTextBox(AdvancedInventory_Single_TextBox_Count,
                Model_AppVariables.UserUiColors.TextBoxErrorForeColor ?? Color.Red,
                "[ Enter How Many Times ]");
            AdvancedInventory_MultiLoc_RichTextBox_Notes.Text = string.Empty;
            AdvancedInventory_MultiLoc_ListView_Preview.Items.Clear();
            AdvancedInventory_MultiLoc_ComboBox_Part.Enabled = true;
            AdvancedInventory_MultiLoc_ComboBox_Op.Enabled = true;

            // Restore controls and focus
            Debug.WriteLine("[DEBUG] Restoring ComboBox visibility and focus");
            AdvancedInventory_MultiLoc_ComboBox_Part.Visible = true;
            AdvancedInventory_MultiLoc_ComboBox_Op.Visible = true;
            AdvancedInventory_MultiLoc_ComboBox_Loc.Visible = true;
            AdvancedInventory_MultiLoc_ComboBox_Part.Focus();

            // Update Save/AddLoc button state
            UpdateMultiSaveButtonState();

            Debug.WriteLine("[DEBUG] AdvancedInventory MultiLoc HardReset - end");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[ERROR] Exception in AdvancedInventory MultiLoc HardReset: {ex}");
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                "AdvancedInventory_MultiLoc_HardResetAsync");
        }
        finally
        {
            Debug.WriteLine("[DEBUG] AdvancedInventory MultiLoc HardReset button re-enabled");
            AdvancedInventory_MultiLoc_Button_Reset.Enabled = true;
            if (MainFormInstance != null)
            {
                Debug.WriteLine("[DEBUG] Restoring status strip after reset");
                MainFormInstance.MainForm_StatusStrip_Disconnected.Visible = false;
                MainFormInstance.MainForm_StatusStrip_SavedStatus.Visible = true;
                MainFormInstance.MainForm_StatusStrip_Disconnected.Text =
                    @"Disconnected from Server, please standby...";
                MainFormInstance.TabLoadingProgress?.HideProgress();
            }
        }
    }

    private void AdvancedInventory_MultiLoc_SoftReset()
    {
        AdvancedInventory_MultiLoc_Button_Reset.Enabled = false;
        try
        {
            if (MainFormInstance != null)
            {
                Debug.WriteLine("[DEBUG] Updating status strip for Soft Reset");
                MainFormInstance.MainForm_StatusStrip_Disconnected.Text = @"Please wait while resetting...";
                MainFormInstance.MainForm_StatusStrip_Disconnected.Visible = true;
                MainFormInstance.MainForm_StatusStrip_SavedStatus.Visible = false;
            }

            // Reset UI fields
            Debug.WriteLine("[DEBUG] Resetting UI fields");
            MainFormControlHelper.ResetComboBox(AdvancedInventory_MultiLoc_ComboBox_Part,
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);
            MainFormControlHelper.ResetComboBox(AdvancedInventory_MultiLoc_ComboBox_Op,
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);
            MainFormControlHelper.ResetComboBox(AdvancedInventory_MultiLoc_ComboBox_Loc,
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);
            MainFormControlHelper.ResetTextBox(AdvancedInventory_MultiLoc_TextBox_Qty,
                Model_AppVariables.UserUiColors.TextBoxErrorForeColor ?? Color.Red,
                "[ Enter Valid Quantity ]");
            MainFormControlHelper.ResetComboBox(AdvancedInventory_Single_ComboBox_Part,
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);
            MainFormControlHelper.ResetComboBox(AdvancedInventory_Single_ComboBox_Op,
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);
            MainFormControlHelper.ResetComboBox(AdvancedInventory_Single_ComboBox_Loc,
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);
            MainFormControlHelper.ResetTextBox(AdvancedInventory_Single_TextBox_Qty,
                Model_AppVariables.UserUiColors.TextBoxErrorForeColor ?? Color.Red,
                "[ Enter Valid Quantity ]");
            MainFormControlHelper.ResetTextBox(AdvancedInventory_Single_TextBox_Count,
                Model_AppVariables.UserUiColors.TextBoxErrorForeColor ?? Color.Red,
                "[ Enter How Many Times ]");
            AdvancedInventory_MultiLoc_RichTextBox_Notes.Text = string.Empty;
            AdvancedInventory_MultiLoc_ListView_Preview.Items.Clear();
            AdvancedInventory_MultiLoc_ComboBox_Part.Enabled = true;
            AdvancedInventory_MultiLoc_ComboBox_Op.Enabled = true;

            // Update Save/AddLoc button state
            UpdateMultiSaveButtonState();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[ERROR] Exception in AdvancedInventory MultiLoc SoftReset: {ex}");
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                "AdvancedInventory_MultiLoc_SoftReset");
        }
        finally
        {
            Debug.WriteLine("[DEBUG] AdvancedInventory MultiLoc SoftReset button re-enabled");
            AdvancedInventory_MultiLoc_Button_Reset.Enabled = true;
            AdvancedInventory_MultiLoc_ComboBox_Part.Focus();
            if (MainFormInstance != null)
            {
                MainFormInstance.MainForm_StatusStrip_Disconnected.Visible = false;
                MainFormInstance.MainForm_StatusStrip_SavedStatus.Visible = true;
                MainFormInstance.MainForm_StatusStrip_Disconnected.Text =
                    @"Disconnected from Server, please standby...";
            }
        }
    }

    private async void AdvancedInventory_MultiLoc_Button_Reset_Click(object? sender, EventArgs e)
    {
        AdvancedInventory_MultiLoc_Button_Reset.Enabled = false;
        try
        {
            // Check if Shift key is held down
            if ((ModifierKeys & Keys.Shift) == Keys.Shift)
                await AdvancedInventory_MultiLoc_HardResetAsync();
            else
                AdvancedInventory_MultiLoc_SoftReset();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[ERROR] Exception in AdvancedInventory MultiLoc Reset: {ex}");
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                "AdvancedInventory_MultiLoc_Button_Reset_Click");
        }
    }

    private void AdvancedInventory_MultiLoc_Button_AddLoc_Click(object? sender, EventArgs e)
    {
        try
        {
            LoggingUtility.Log("AdvancedInventory_MultiLoc_Button_AddLoc_Click entered.");

            // Get values from controls
            var partId = AdvancedInventory_MultiLoc_ComboBox_Part.Text;
            var op = AdvancedInventory_MultiLoc_ComboBox_Op.Text;
            var loc = AdvancedInventory_MultiLoc_ComboBox_Loc.Text;
            var qtyText = AdvancedInventory_MultiLoc_TextBox_Qty.Text.Trim();
            var notes = AdvancedInventory_MultiLoc_RichTextBox_Notes.Text.Trim();

            // Validate input
            if (string.IsNullOrWhiteSpace(partId) || AdvancedInventory_MultiLoc_ComboBox_Part.SelectedIndex <= 0)
            {
                MessageBox.Show(@"Please select a valid Part.", @"Validation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                AdvancedInventory_MultiLoc_ComboBox_Part.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(op) || AdvancedInventory_MultiLoc_ComboBox_Op.SelectedIndex <= 0)
            {
                MessageBox.Show(@"Please select a valid Operation.", @"Validation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                AdvancedInventory_MultiLoc_ComboBox_Op.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(loc) || AdvancedInventory_MultiLoc_ComboBox_Loc.SelectedIndex <= 0)
            {
                MessageBox.Show(@"Please select a valid Location.", @"Validation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                AdvancedInventory_MultiLoc_ComboBox_Loc.Focus();
                return;
            }

            if (!int.TryParse(qtyText, out var qty) || qty <= 0)
            {
                MessageBox.Show(@"Please enter a valid quantity.", @"Validation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                AdvancedInventory_MultiLoc_TextBox_Qty.Focus();
                return;
            }

            // Prevent duplicate location entries
            foreach (ListViewItem item in AdvancedInventory_MultiLoc_ListView_Preview.Items)
                if (string.Equals(item.SubItems[0].Text, loc, StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show(@"This location has already been added.", @"Duplicate Entry", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    AdvancedInventory_MultiLoc_ComboBox_Loc.Focus();
                    return;
                }

            // Add to ListView
            var listViewItem = new ListViewItem(new[]
            {
                loc,
                qty.ToString(),
                notes
            });
            AdvancedInventory_MultiLoc_ListView_Preview.Items.Add(listViewItem);

            LoggingUtility.Log(
                $"Added MultiLoc entry: PartId = {partId}, Op = {op}, Loc={loc}, Qty={qty}, Notes={notes}");

            // Disable part ComboBox after the first location is added
            if (AdvancedInventory_MultiLoc_ListView_Preview.Items.Count == 1)
                AdvancedInventory_MultiLoc_ComboBox_Part.Enabled = false;

            // Reset only the location, quantity, and notes fields for next entry
            MainFormControlHelper.ResetComboBox(AdvancedInventory_MultiLoc_ComboBox_Loc,
                Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red, 0);
            AdvancedInventory_MultiLoc_ComboBox_Loc.Focus();

            UpdateMultiSaveButtonState();
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                "AdvancedInventory_MultiLoc_Button_AddLoc_Click");
        }
    }

    private async void AdvancedInventory_MultiLoc_Button_SaveAll_Click(object? sender, EventArgs e)
    {
        try
        {
            LoggingUtility.Log("AdvancedInventory_MultiLoc_Button_SaveAll_Click entered.");

            // Validate that there is at least one entry to save
            if (AdvancedInventory_MultiLoc_ListView_Preview.Items.Count == 0)
            {
                MessageBox.Show(@"Please add at least one location entry before saving.", @"No Entries",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get shared values from controls
            var partId = AdvancedInventory_MultiLoc_ComboBox_Part.Text;
            var op = AdvancedInventory_MultiLoc_ComboBox_Op.Text;

            if (string.IsNullOrWhiteSpace(partId) || AdvancedInventory_MultiLoc_ComboBox_Part.SelectedIndex <= 0)
            {
                MessageBox.Show(@"Please select a valid Part.", @"Validation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                AdvancedInventory_MultiLoc_ComboBox_Part.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(op) || AdvancedInventory_MultiLoc_ComboBox_Op.SelectedIndex <= 0)
            {
                MessageBox.Show(@"Please select a valid Operation.", @"Validation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                AdvancedInventory_MultiLoc_ComboBox_Op.Focus();
                return;
            }

            // Save each entry in the ListView
            var locations = new HashSet<string>();
            var totalQty = 0;
            var savedCount = 0;
            foreach (ListViewItem item in AdvancedInventory_MultiLoc_ListView_Preview.Items)
            {
                var loc = item.SubItems[0].Text;
                var qtyText = item.SubItems[1].Text;
                var notes = item.SubItems[2].Text;

                if (!int.TryParse(qtyText, out var qty) || qty <= 0)
                {
                    LoggingUtility.LogApplicationError(
                        new Exception($"Invalid quantity for location '{loc}': '{qtyText}'"));
                    continue;
                }

                // Set Model_AppVariables (if needed)
                Model_AppVariables.PartId = partId;
                Model_AppVariables.Operation = op;
                Model_AppVariables.Location = loc;
                Model_AppVariables.Notes = notes;
                Model_AppVariables.InventoryQuantity = qty;
                Model_AppVariables.User ??= Environment.UserName;
                Model_AppVariables.PartType ??= "";

                await Dao_Inventory.AddInventoryItemAsync(
                    partId,
                    loc,
                    op,
                    qty,
                    Model_AppVariables.PartType ?? "",
                    Model_AppVariables.User,
                    "", // batchNumber
                    notes,
                    true);

                locations.Add(loc);
                totalQty += qty;
                savedCount++;
            }

            MessageBox.Show(
                $@"{savedCount} inventory transaction(s) saved successfully.",
                @"Success",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            LoggingUtility.Log(
                $"Saved {savedCount} multi-location inventory transaction(s) for Part: {partId}, Op: {op}");

            // Update status strip
            if (MainFormInstance != null && savedCount > 0)
            {
                var time = DateTime.Now.ToString("hh:mm tt");
                var locDisplay = locations.Count > 1 ? "Multiple Locations" : locations.FirstOrDefault() ?? "";
                MainFormInstance.MainForm_StatusStrip_SavedStatus.Text =
                    $"Last Inventoried: {partId} (Op: {op}), Location: {locDisplay}, Quantity: {totalQty} @ {time}";
            }

            // Optionally reset the form after save
            AdvancedInventory_MultiLoc_Button_Reset_Click(null, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            await Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, true,
                "AdvancedInventory_MultiLoc_Button_SaveAll_Click");
        }
    }

    #endregion

    #region Excel Export/Import Helpers

    private static string GetWipAppExcelUserFolder()
    {
        // Get the log file path to determine the log directory
        var server = new MySqlConnectionStringBuilder(Model_AppVariables.ConnectionString).Server;
        var userName = Model_AppVariables.User ?? Environment.UserName;
        var logFilePath = Helper_Database_Variables.GetLogFilePath(server, userName);
        var logDir = Directory.GetParent(logFilePath)?.Parent?.FullName ?? "";
        // Place Excel files as a sibling to the log folder
        var excelRoot = Path.Combine(logDir, "WIP App Excel Files");
        var userFolder = Path.Combine(excelRoot, userName);
        if (!Directory.Exists(userFolder))
            Directory.CreateDirectory(userFolder);
        return userFolder;
    }

    private static string GetUserExcelFilePath()
    {
        var userFolder = GetWipAppExcelUserFolder();
        var fileName = $"{Model_AppVariables.User ?? Environment.UserName}_import.xlsx";
        return Path.Combine(userFolder, fileName);
    }

    private void AdvancedInventory_Import_Button_OpenExcel_Click(object? sender, EventArgs e)
    {
        try
        {
            var excelPath = GetUserExcelFilePath();
            if (!File.Exists(excelPath))
            {
                // Ensure the user folder exists
                var userFolder = Path.GetDirectoryName(excelPath);
                if (!Directory.Exists(userFolder))
                    Directory.CreateDirectory(userFolder!);
                // Copy template file to user's Excel file path
                var templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Controls",
                    "MainForm", "WIPAppTemplate.xlsx");
                if (File.Exists(templatePath))
                {
                    File.Copy(templatePath, excelPath, false);
                }
                else
                {
                    MessageBox.Show($"Excel template not found: {templatePath}", @"Template Not Found",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // Always use the default handler for .xlsx files
            Process.Start(new ProcessStartInfo(excelPath) { UseShellExecute = true });
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            MessageBox.Show($"Failed to open Excel file: {ex.Message}", @"Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    #endregion

    #region Excel Import/Export Actions

    private void AdvancedInventory_Import_Button_ImportExcel_Click(object? sender, EventArgs e)
    {
        try
        {
            var excelPath = GetUserExcelFilePath();
            if (!File.Exists(excelPath))
            {
                MessageBox.Show("Excel file not found. Please create or open the Excel file first.", @"File Not Found",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var dt = new DataTable();
            using (var workbook = new XLWorkbook(excelPath))
            {
                var worksheet = workbook.Worksheet("Tab 1");
                if (worksheet == null)
                {
                    MessageBox.Show("Worksheet 'Tab 1' not found in the Excel file.", @"Worksheet Not Found",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Get the used range
                var usedRange = worksheet.RangeUsed();
                if (usedRange == null)
                {
                    MessageBox.Show("No data found in 'Tab 1'.", @"No Data", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                var colCount = usedRange.ColumnCount();
                var rowCount = usedRange.RowCount();

                // Add columns from the first row
                var headerRow = usedRange.Row(1);
                for (var col = 1; col <= colCount; col++)
                {
                    var colName = headerRow.Cell(col).GetValue<string>();
                    if (string.IsNullOrWhiteSpace(colName))
                        colName = $"Column{col}";
                    dt.Columns.Add(colName);
                }

                // Add data rows
                for (var row = 2; row <= rowCount; row++)
                {
                    var dataRow = dt.NewRow();
                    for (var col = 1; col <= colCount; col++)
                        dataRow[col - 1] = usedRange.Row(row).Cell(col).GetValue<string>();
                    dt.Rows.Add(dataRow);
                }
            }

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("No data found in the Excel file to import.", @"No Data", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            AdvancedInventory_Import_DataGridView.DataSource = dt;
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            MessageBox.Show($"Failed to import Excel data: {ex.Message}", @"Import Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private async void AdvancedInventory_Import_Button_Save_Click(object? sender, EventArgs e)
    {
        if (AdvancedInventory_Import_DataGridView.DataSource == null)
            return;

        var dgv = AdvancedInventory_Import_DataGridView;
        var rowsToRemove = new List<DataGridViewRow>();
        var anyError = false;

        // Get DataTables from ComboBoxes' DataSource
        var partTable = AdvancedInventory_Single_ComboBox_Part.DataSource as DataTable;
        var opTable = AdvancedInventory_Single_ComboBox_Op.DataSource as DataTable;
        var locTable = AdvancedInventory_Single_ComboBox_Loc.DataSource as DataTable;

        // Get valid values from DataTables
        var validParts =
            partTable?.AsEnumerable().Select(r => r.Field<string>("Item Number"))
                .Where(s => !string.IsNullOrWhiteSpace(s)).ToHashSet(StringComparer.OrdinalIgnoreCase) ??
            new HashSet<string?>();
        var validOps =
            opTable?.AsEnumerable().Select(r => r.Field<string>("Operation")).Where(s => !string.IsNullOrWhiteSpace(s))
                .ToHashSet(StringComparer.OrdinalIgnoreCase) ?? new HashSet<string?>();
        var validLocs =
            locTable?.AsEnumerable().Select(r => r.Field<string>("Location")).Where(s => !string.IsNullOrWhiteSpace(s))
                .ToHashSet(StringComparer.OrdinalIgnoreCase) ?? new HashSet<string?>();

        // Load Excel file for row removal
        var excelPath = GetUserExcelFilePath();
        XLWorkbook? workbook = null;
        IXLWorksheet? worksheet = null;
        if (File.Exists(excelPath))
        {
            workbook = new XLWorkbook(excelPath);
            worksheet = workbook.Worksheet("Tab 1");
        }

        // Collect all Excel row numbers to delete (1-based)
        var excelRowsToDelete = new List<int>();

        foreach (DataGridViewRow row in dgv.Rows)
        {
            if (row.IsNewRow) continue;

            var rowValid = true;
            foreach (DataGridViewCell cell in row.Cells)
                cell.Style.ForeColor = Model_AppVariables.UserUiColors.TextBoxForeColor ?? Color.Black;

            var part = row.Cells["Part"].Value?.ToString() ?? "";
            var op = row.Cells["Operation"].Value?.ToString() ?? "";
            var loc = row.Cells["Location"].Value?.ToString() ?? "";
            var qtyText = row.Cells["Quantity"].Value?.ToString() ?? "";
            var notesOriginal = row.Cells["Notes"].Value?.ToString() ?? "";
            var notes = "Excel Import: " + notesOriginal;

            // Validate against ComboBox DataTables
            if (!validParts.Contains(part))
            {
                row.Cells["Part"].Style.ForeColor =
                    Model_AppVariables.UserUiColors.TextBoxErrorForeColor ?? Color.Red;
                rowValid = false;
            }

            if (!validOps.Contains(op))
            {
                row.Cells["Operation"].Style.ForeColor =
                    Model_AppVariables.UserUiColors.TextBoxErrorForeColor ?? Color.Red;
                rowValid = false;
            }

            if (!validLocs.Contains(loc))
            {
                row.Cells["Location"].Style.ForeColor =
                    Model_AppVariables.UserUiColors.TextBoxErrorForeColor ?? Color.Red;
                rowValid = false;
            }

            // Quantity must be a number above 0
            if (!int.TryParse(qtyText, out var qty) || qty <= 0)
            {
                row.Cells["Quantity"].Style.ForeColor =
                    Model_AppVariables.UserUiColors.TextBoxErrorForeColor ?? Color.Red;
                rowValid = false;
            }

            if (rowValid)
                try
                {
                    await Dao_Inventory.AddInventoryItemAsync(
                        part, loc, op, qty, "", Model_AppVariables.User ?? Environment.UserName, "", notes, true);

                    // Find the Excel row number to delete (match by order, not by value)
                    if (worksheet != null)
                    {
                        var usedRange = worksheet.RangeUsed();
                        if (usedRange != null)
                        {
                            var headerRow = usedRange.FirstRow().RowNumber();
                            var lastRow = usedRange.LastRow().RowNumber();
                            var excelRowIndex = headerRow + 1 + row.Index;
                            if (excelRowIndex <= lastRow)
                                excelRowsToDelete.Add(excelRowIndex);
                        }
                    }

                    rowsToRemove.Add(row);
                }
                catch (Exception ex)
                {
                    LoggingUtility.LogApplicationError(ex);
                    foreach (DataGridViewCell cell in row.Cells)
                        cell.Style.ForeColor = Model_AppVariables.UserUiColors.TextBoxErrorForeColor ?? Color.Red;
                    rowValid = false;
                    anyError = true;
                }
            else
                anyError = true;
        }

        // Delete rows from Excel in descending order to avoid shifting
        if (worksheet != null && excelRowsToDelete.Count > 0)
        {
            excelRowsToDelete.Sort((a, b) => b.CompareTo(a));
            foreach (var rowNum in excelRowsToDelete)
                worksheet.Row(rowNum).Delete();

            // Push remaining rows up: remove empty rows at the end
            var usedRange = worksheet.RangeUsed();
            if (usedRange != null)
            {
                var headerRow = usedRange.FirstRow().RowNumber();
                var lastRow = worksheet.LastRowUsed()?.RowNumber() ?? headerRow;
                for (var i = lastRow; i > headerRow; i--)
                {
                    var isEmpty = worksheet.Row(i).CellsUsed().All(c => string.IsNullOrWhiteSpace(c.GetString()));
                    if (isEmpty)
                        worksheet.Row(i).Delete();
                }
            }

            workbook?.Save();
        }

        foreach (var row in rowsToRemove)
            if (!row.IsNewRow)
                dgv.Rows.Remove(row);

        RefreshImportDataGridView();

        if (!anyError)
        {
            MessageBox.Show("All transactions saved successfully.", @"Success", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            if (MainFormInstance != null)
                MainFormInstance.MainForm_StatusStrip_SavedStatus.Text =
                    $"Last Import: {DateTime.Now:hh:mm tt} ({dgv.Rows.Count} rows imported)";
        }
        else
        {
            MessageBox.Show("Some rows could not be saved. Please correct highlighted errors.", @"Validation Error",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void RefreshImportDataGridView()
    {
        var excelPath = GetUserExcelFilePath();
        if (!File.Exists(excelPath))
            return;

        // Store highlighted cells before refresh
        var highlightMap = new Dictionary<int, HashSet<string>>();
        if (AdvancedInventory_Import_DataGridView.DataSource is DataTable)
            foreach (DataGridViewRow row in AdvancedInventory_Import_DataGridView.Rows)
            {
                if (row.IsNewRow) continue;
                var cols = new HashSet<string>();
                foreach (DataGridViewCell cell in row.Cells)
                    if (cell.Style.ForeColor == (Model_AppVariables.UserUiColors.TextBoxErrorForeColor ?? Color.Red))
                        if (cell.OwningColumn != null)
                            cols.Add(cell.OwningColumn.Name);
                if (cols.Count > 0)
                    highlightMap[row.Index] = cols;
            }

        var dt = new DataTable();
        using (var workbook = new XLWorkbook(excelPath))
        {
            var worksheet = workbook.Worksheet("Tab 1");
            if (worksheet == null)
                return;

            var usedRange = worksheet.RangeUsed();
            if (usedRange == null)
                return;

            var colCount = usedRange.ColumnCount();
            var rowCount = usedRange.RowCount();

            var headerRow = usedRange.Row(1);
            for (var col = 1; col <= colCount; col++)
            {
                var colName = headerRow.Cell(col).GetValue<string>();
                if (string.IsNullOrWhiteSpace(colName))
                    colName = $"Column{col}";
                dt.Columns.Add(colName);
            }

            for (var row = 2; row <= rowCount; row++)
            {
                var dataRow = dt.NewRow();
                for (var col = 1; col <= colCount; col++)
                    dataRow[col - 1] = usedRange.Row(row).Cell(col).GetValue<string>();
                dt.Rows.Add(dataRow);
            }
        }

        AdvancedInventory_Import_DataGridView.DataSource = dt;

        // Restore highlights after refresh and re-validate Quantity
        foreach (DataGridViewRow row in AdvancedInventory_Import_DataGridView.Rows)
        {
            if (row.IsNewRow) continue;

            // Restore previous highlights
            if (highlightMap.TryGetValue(row.Index, out var cols))
                foreach (DataGridViewCell cell in row.Cells)
                    if (cell.OwningColumn != null && cols.Contains(cell.OwningColumn.Name))
                        cell.Style.ForeColor = Model_AppVariables.UserUiColors.TextBoxErrorForeColor ?? Color.Red;

            // Always validate Quantity column
            if (row.DataGridView != null && row.DataGridView.Columns.Contains("Quantity"))
            {
                var qtyCell = row.Cells["Quantity"];
                var qtyText = qtyCell.Value?.ToString() ?? "";
                if (!int.TryParse(qtyText, out var qty) || qty <= 0)
                    qtyCell.Style.ForeColor = Model_AppVariables.UserUiColors.TextBoxErrorForeColor ?? Color.Red;
            }
        }
    }

    #endregion
}