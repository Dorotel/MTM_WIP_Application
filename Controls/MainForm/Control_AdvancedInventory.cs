using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using ClosedXML.Excel;
using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Forms.ErrorDialog;
using MTM_Inventory_Application.Forms.MainForm.Classes;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;
using MTM_Inventory_Application.Services;
using MySql.Data.MySqlClient;
using Color = System.Drawing.Color;

namespace MTM_Inventory_Application.Controls.MainForm
{
    public partial class Control_AdvancedInventory : UserControl
    {
        #region Fields

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

        #region Constructor and Initialization

        public Control_AdvancedInventory()
        {
            try
            {
                LoggingUtility.Log("Control_AdvancedInventory constructor entered.");
                InitializeComponent();

                // Apply comprehensive DPI scaling and runtime layout adjustments
                // THEME POLICY: Only update theme on startup, in settings menu, or on DPI change.
                // Do NOT call theme update methods from arbitrary event handlers or business logic.
                Core_Themes.ApplyDpiScaling(this); // Allowed: Form initialization
                Core_Themes.ApplyRuntimeLayoutAdjustments(this); // Allowed: Form initialization

                ToolTip toolTip = new();
                toolTip.SetToolTip(AdvancedInventory_Single_Button_Send,
                    $"Shortcut: {Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_AdvInv_Send)}");
                toolTip.SetToolTip(AdvancedInventory_Single_Button_Save,
                    $"Shortcut: {Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_AdvInv_Save)}");
                toolTip.SetToolTip(AdvancedInventory_Single_Button_Reset,
                    $"Shortcut: {Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_AdvInv_Reset)}");
                toolTip.SetToolTip(AdvancedInventory_Single_Button_Normal,
                    $"Shortcut: {Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_AdvInv_Normal)}");
                toolTip.SetToolTip(AdvancedInventory_MultiLoc_Button_AddLoc,
                    $"Shortcut: {Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_AdvInv_Multi_AddLoc)}");
                toolTip.SetToolTip(AdvancedInventory_MultiLoc_Button_SaveAll,
                    $"Shortcut: {Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_AdvInv_Multi_SaveAll)}");
                toolTip.SetToolTip(AdvancedInventory_MultiLoc_Button_Reset,
                    $"Shortcut: {Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_AdvInv_Multi_Reset)}");
                toolTip.SetToolTip(AdvancedInventory_Multi_Button_Normal,
                    $"Shortcut: {Helper_UI_Shortcuts.ToShortcutString(Core_WipAppVariables.Shortcut_AdvInv_Multi_Normal)}");
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
                    if (AdvancedInventory_TabControl.SelectedIndex == 0 &&
                        AdvancedInventory_Single_ComboBox_Part.Visible &&
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
                    Helper_UI_ComboBoxes.ValidateComboBoxItem(AdvancedInventory_Single_ComboBox_Op,
                        "[ Enter Operation ]");
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
                    Helper_UI_ComboBoxes.ValidateComboBoxItem(AdvancedInventory_Single_ComboBox_Loc,
                        "[ Enter Location ]");
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

                AdvancedInventory_Single_TextBox_Qty.Enter +=
                    (s, e) => AdvancedInventory_Single_TextBox_Qty.SelectAll();
                AdvancedInventory_Single_TextBox_Qty.Click +=
                    (s, e) => AdvancedInventory_Single_TextBox_Qty.SelectAll();
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
                string text = textBox.Text.Trim();
                bool isValid = int.TryParse(text, out int qty) && qty > 0;
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
            bool partValid = AdvancedInventory_Single_ComboBox_Part.SelectedIndex > 0 &&
                             !string.IsNullOrWhiteSpace(AdvancedInventory_Single_ComboBox_Part.Text);
            bool opValid = AdvancedInventory_Single_ComboBox_Op.SelectedIndex > 0 &&
                           !string.IsNullOrWhiteSpace(AdvancedInventory_Single_ComboBox_Op.Text);
            bool locValid = AdvancedInventory_Single_ComboBox_Loc.SelectedIndex > 0 &&
                            !string.IsNullOrWhiteSpace(AdvancedInventory_Single_ComboBox_Loc.Text);
            bool qtyValid = int.TryParse(AdvancedInventory_Single_TextBox_Qty.Text.Trim(), out int qty) && qty > 0;
            bool countValid = int.TryParse(AdvancedInventory_Single_TextBox_Count.Text.Trim(), out int count) &&
                              count > 0;

            AdvancedInventory_Single_Button_Send.Enabled = partValid && opValid && locValid && qtyValid && countValid;
        }

        private void UpdateMultiSaveButtonState()
        {
            bool partValid = AdvancedInventory_MultiLoc_ComboBox_Part.SelectedIndex > 0 &&
                             !string.IsNullOrWhiteSpace(AdvancedInventory_MultiLoc_ComboBox_Part.Text);
            bool opValid = AdvancedInventory_MultiLoc_ComboBox_Op.SelectedIndex > 0 &&
                           !string.IsNullOrWhiteSpace(AdvancedInventory_MultiLoc_ComboBox_Op.Text);
            bool locValid = AdvancedInventory_MultiLoc_ComboBox_Loc.SelectedIndex > 0 &&
                            !string.IsNullOrWhiteSpace(AdvancedInventory_MultiLoc_ComboBox_Loc.Text);
            bool qtyValid = int.TryParse(AdvancedInventory_MultiLoc_TextBox_Qty.Text.Trim(), out int qty) && qty > 0;
            AdvancedInventory_MultiLoc_Button_AddLoc.Enabled = partValid && opValid && locValid && qtyValid;
            AdvancedInventory_MultiLoc_Button_SaveAll.Enabled =
                AdvancedInventory_MultiLoc_ListView_Preview.Items.Count > 0 && partValid && opValid;
        }

        public static void ValidateQtyTextBox(TextBox textBox, string placeholder)
        {
            string text = textBox.Text.Trim();
            bool isValid = int.TryParse(text, out int value) && value > 0;
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
                if (AdvancedInventory_TabControl.SelectedTab == AdvancedInventory_TabControl_Single)
                {
                    if (keyData == Core_WipAppVariables.Shortcut_AdvInv_Send)
                    {
                        if (AdvancedInventory_Single_Button_Send.Visible &&
                            AdvancedInventory_Single_Button_Send.Enabled)
                        {
                            AdvancedInventory_Single_Button_Send.PerformClick();
                            return true;
                        }
                    }

                    if (keyData == Core_WipAppVariables.Shortcut_AdvInv_Save)
                    {
                        if (AdvancedInventory_Single_Button_Save.Visible &&
                            AdvancedInventory_Single_Button_Save.Enabled)
                        {
                            AdvancedInventory_Single_Button_Save.PerformClick();
                            return true;
                        }
                    }

                    if (keyData == Core_WipAppVariables.Shortcut_AdvInv_Reset)
                    {
                        if (AdvancedInventory_Single_Button_Reset.Visible &&
                            AdvancedInventory_Single_Button_Reset.Enabled)
                        {
                            AdvancedInventory_Single_Button_Reset.PerformClick();
                            return true;
                        }
                    }

                    if (keyData == Core_WipAppVariables.Shortcut_AdvInv_Normal)
                    {
                        if (AdvancedInventory_Single_Button_Normal.Visible &&
                            AdvancedInventory_Single_Button_Normal.Enabled)
                        {
                            AdvancedInventory_Single_Button_Normal.PerformClick();
                            return true;
                        }
                    }
                }

                if (AdvancedInventory_TabControl.SelectedTab == AdvancedInventory_TabControl_MultiLoc)
                {
                    if (keyData == Core_WipAppVariables.Shortcut_AdvInv_Multi_AddLoc)
                    {
                        if (AdvancedInventory_MultiLoc_Button_AddLoc.Visible &&
                            AdvancedInventory_MultiLoc_Button_AddLoc.Enabled)
                        {
                            AdvancedInventory_MultiLoc_Button_AddLoc.PerformClick();
                            return true;
                        }
                    }

                    if (keyData == Core_WipAppVariables.Shortcut_AdvInv_Multi_SaveAll)
                    {
                        if (AdvancedInventory_MultiLoc_Button_SaveAll.Visible &&
                            AdvancedInventory_MultiLoc_Button_SaveAll.Enabled)
                        {
                            AdvancedInventory_MultiLoc_Button_SaveAll.PerformClick();
                            return true;
                        }
                    }

                    if (keyData == Core_WipAppVariables.Shortcut_AdvInv_Multi_Reset)
                    {
                        if (AdvancedInventory_MultiLoc_Button_Reset.Visible &&
                            AdvancedInventory_MultiLoc_Button_Reset.Enabled)
                        {
                            AdvancedInventory_MultiLoc_Button_Reset.PerformClick();
                            return true;
                        }
                    }

                    if (keyData == Core_WipAppVariables.Shortcut_AdvInv_Multi_Normal)
                    {
                        if (AdvancedInventory_Multi_Button_Normal.Visible &&
                            AdvancedInventory_Multi_Button_Normal.Enabled)
                        {
                            AdvancedInventory_Multi_Button_Normal.PerformClick();
                            return true;
                        }
                    }
                }

                if (AdvancedInventory_TabControl.SelectedTab == AdvancedInventory_TabControl_Import)
                {
                    if (keyData == Core_WipAppVariables.Shortcut_AdvInv_Import_OpenExcel)
                    {
                        if (AdvancedInventory_Import_Button_OpenExcel.Visible &&
                            AdvancedInventory_Import_Button_OpenExcel.Enabled)
                        {
                            AdvancedInventory_Import_Button_OpenExcel.PerformClick();
                            return true;
                        }
                    }

                    if (keyData == Core_WipAppVariables.Shortcut_AdvInv_Import_ImportExcel)
                    {
                        if (AdvancedInventory_Import_Button_ImportExcel.Visible &&
                            AdvancedInventory_Import_Button_ImportExcel.Enabled)
                        {
                            AdvancedInventory_Import_Button_ImportExcel.PerformClick();
                            return true;
                        }
                    }

                    if (keyData == Core_WipAppVariables.Shortcut_AdvInv_Import_Save)
                    {
                        if (AdvancedInventory_Import_Button_Save.Visible &&
                            AdvancedInventory_Import_Button_Save.Enabled)
                        {
                            AdvancedInventory_Import_Button_Save.PerformClick();
                            return true;
                        }
                    }

                    if (keyData == Core_WipAppVariables.Shortcut_AdvInv_Import_Normal)
                    {
                        if (AdvancedInventory_Import_Button_Normal.Visible &&
                            AdvancedInventory_Import_Button_Normal.Enabled)
                        {
                            AdvancedInventory_Import_Button_Normal.PerformClick();
                            return true;
                        }
                    }
                }

                if (keyData == Core_WipAppVariables.Shortcut_Remove_Advanced)
                {
                    if (MainFormInstance != null && MainFormInstance.MainForm_UserControl_AdvancedRemove != null)
                    {
                        MainFormInstance.MainForm_UserControl_AdvancedInventory.Visible = false;
                        MainFormInstance.MainForm_UserControl_AdvancedRemove.Visible = true;
                        MainFormInstance.MainForm_TabControl.SelectedIndex = 2;
                        return true;
                    }
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
                _progressHelper?.ShowProgress();
                _progressHelper?.UpdateProgress(10, "Resetting Advanced Inventory (Single)...");
                Debug.WriteLine("[DEBUG] AdvancedInventory Single HardReset - start");

                if (MainFormInstance != null)
                {
                    Debug.WriteLine("[DEBUG] Updating status strip for hard reset");
                    MainFormInstance.MainForm_StatusStrip_Disconnected.Text = @"Please wait while resetting...";
                    MainFormInstance.MainForm_StatusStrip_Disconnected.Visible = true;
                    MainFormInstance.MainForm_StatusStrip_SavedStatus.Visible = false;
                }

                _progressHelper?.UpdateProgress(30, "Resetting data tables...");

                Debug.WriteLine("[DEBUG] Unbinding ComboBox DataSources");
                AdvancedInventory_Single_ComboBox_Part.DataSource = null;
                AdvancedInventory_Single_ComboBox_Op.DataSource = null;
                AdvancedInventory_Single_ComboBox_Loc.DataSource = null;

                Debug.WriteLine("[DEBUG] Resetting and refreshing all ComboBox DataTables");
                await Helper_UI_ComboBoxes.ResetAndRefreshAllDataTablesAsync();
                Debug.WriteLine("[DEBUG] DataTables reset complete");

                _progressHelper?.UpdateProgress(60, "Refilling combo boxes...");
                Debug.WriteLine("[DEBUG] Refilling Part ComboBox");
                await Helper_UI_ComboBoxes.FillPartComboBoxesAsync(AdvancedInventory_Single_ComboBox_Part);
                Debug.WriteLine("[DEBUG] Refilling Operation ComboBox");
                await Helper_UI_ComboBoxes.FillOperationComboBoxesAsync(AdvancedInventory_Single_ComboBox_Op);
                Debug.WriteLine("[DEBUG] Refilling Location ComboBox");
                await Helper_UI_ComboBoxes.FillLocationComboBoxesAsync(AdvancedInventory_Single_ComboBox_Loc);

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

                AdvancedInventory_Single_ComboBox_Part.ForeColor =
                    Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red;
                AdvancedInventory_Single_ComboBox_Op.ForeColor =
                    Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red;
                AdvancedInventory_Single_ComboBox_Loc.ForeColor =
                    Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red;
                AdvancedInventory_Single_TextBox_Qty.ForeColor =
                    Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red;
                AdvancedInventory_Single_TextBox_Count.ForeColor =
                    Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red;

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
                    _progressHelper?.HideProgress();
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

                Debug.WriteLine("[DEBUG] Resetting UI fields");
                MainFormControlHelper.ResetComboBox(AdvancedInventory_MultiLoc_ComboBox_Part,
                    Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red, 0);
                MainFormControlHelper.ResetComboBox(AdvancedInventory_MultiLoc_ComboBox_Op,
                    Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red, 0);
                MainFormControlHelper.ResetComboBox(AdvancedInventory_MultiLoc_ComboBox_Loc,
                    Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red, 0);
                MainFormControlHelper.ResetTextBox(AdvancedInventory_MultiLoc_TextBox_Qty,
                    Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red,
                    "[ Enter Valid Quantity ]");
                MainFormControlHelper.ResetComboBox(AdvancedInventory_Single_ComboBox_Part,
                    Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red, 0);
                MainFormControlHelper.ResetComboBox(AdvancedInventory_Single_ComboBox_Op,
                    Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red, 0);
                MainFormControlHelper.ResetComboBox(AdvancedInventory_Single_ComboBox_Loc,
                    Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red, 0);
                MainFormControlHelper.ResetTextBox(AdvancedInventory_Single_TextBox_Qty,
                    Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red,
                    "[ Enter Valid Quantity ]");
                MainFormControlHelper.ResetTextBox(AdvancedInventory_Single_TextBox_Count,
                    Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red,
                    "[ Enter How Many Times ]");
                AdvancedInventory_Single_RichTextBox_Notes.Text = string.Empty;
                AdvancedInventory_Single_ListView.Items.Clear();

                AdvancedInventory_Single_ComboBox_Part.ForeColor =
                    Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red;
                AdvancedInventory_Single_ComboBox_Op.ForeColor =
                    Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red;
                AdvancedInventory_Single_ComboBox_Loc.ForeColor =
                    Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red;
                AdvancedInventory_Single_TextBox_Qty.ForeColor =
                    Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red;
                AdvancedInventory_Single_TextBox_Count.ForeColor =
                    Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red;

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
                if ((ModifierKeys & Keys.Shift) == Keys.Shift)
                {
                    await AdvancedInventory_Single_HardResetAsync();
                }
                else
                {
                    AdvancedInventory_Single_SoftReset();
                }
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

                if (AdvancedInventory_Single_ListView.Items.Count == 0)
                {
                    Service_ErrorHandler.ShowWarning(@"No items to inventory. Please add at least one item to the list.", @"No Items");
                    return;
                }

                HashSet<string> partIds = new();
                HashSet<string> operations = new();
                HashSet<string> locations = new();
                int totalQty = 0;
                int savedCount = 0;
                foreach (ListViewItem item in AdvancedInventory_Single_ListView.Items)
                {
                    // Always pass null/empty for batchNumber to ensure each transaction gets a unique batch number
                    string partId = item.SubItems.Count > 0 ? item.SubItems[0].Text : "";
                    string op = item.SubItems.Count > 1 ? item.SubItems[1].Text : "";
                    string loc = item.SubItems.Count > 2 ? item.SubItems[2].Text : "";
                    string qtyText = item.SubItems.Count > 3 ? item.SubItems[3].Text : "";
                    string notes = item.SubItems.Count > 4 ? item.SubItems[4].Text : "";

                    if (string.IsNullOrWhiteSpace(partId) || string.IsNullOrWhiteSpace(op) ||
                        string.IsNullOrWhiteSpace(loc) || !int.TryParse(qtyText, out int qty) || qty <= 0)
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

                    // Pass null/empty for batchNumber for unique batch per transaction
                    await Dao_Inventory.AddInventoryItemAsync(
                        partId,
                        loc,
                        op,
                        qty,
                        Model_AppVariables.PartType ?? "",
                        Model_AppVariables.User,
                        null, // <-- ensure unique batch number
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

                if (MainFormInstance != null && savedCount > 0)
                {
                    string time = DateTime.Now.ToString("hh:mm tt");
                    string locDisplay = locations.Count > 1 ? "Multiple Locations" : locations.FirstOrDefault() ?? "";
                    if (partIds.Count == 1 && operations.Count == 1)
                    {
                        MainFormInstance.MainForm_StatusStrip_SavedStatus.Text =
                            $"Last Inventoried: {partIds.First()} (Op: {operations.First()}), Location: {locDisplay}, Quantity: {totalQty} @ {time}";
                    }
                    else if (partIds.Count == 1 && operations.Count > 1)
                    {
                        MainFormInstance.MainForm_StatusStrip_SavedStatus.Text =
                            $"Last Inventoried: {partIds.First()} (Multiple Ops), Location: {locDisplay}, Quantity: {totalQty} @ {time}";
                    }
                    else
                    {
                        MainFormInstance.MainForm_StatusStrip_SavedStatus.Text =
                            $"Last Inventoried: Multiple Part IDs, Location: {locDisplay}, Quantity: Multiple @ {time}";
                    }
                }

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

                string partId = AdvancedInventory_Single_ComboBox_Part.Text;
                string op = AdvancedInventory_Single_ComboBox_Op.Text;
                string loc = AdvancedInventory_Single_ComboBox_Loc.Text;
                string qtyText = AdvancedInventory_Single_TextBox_Qty.Text.Trim();
                string countText = AdvancedInventory_Single_TextBox_Count.Text.Trim();
                string notes = AdvancedInventory_Single_RichTextBox_Notes.Text.Trim();

                Debug.WriteLine($"partId: {partId}, op: {op}, loc: {loc}, qtyText: {qtyText}, countText: {countText}");

                if (string.IsNullOrWhiteSpace(partId) || AdvancedInventory_Single_ComboBox_Part.SelectedIndex <= 0)
                {
                    Service_ErrorHandler.HandleValidationError("Please select a valid Part.", "Part");
                    AdvancedInventory_Single_ComboBox_Part.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(op) || AdvancedInventory_Single_ComboBox_Op.SelectedIndex <= 0)
                {
                    Service_ErrorHandler.HandleValidationError("Please select a valid Operation.", "Operation");
                    AdvancedInventory_Single_ComboBox_Op.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(loc) || AdvancedInventory_Single_ComboBox_Loc.SelectedIndex <= 0)
                {
                    Service_ErrorHandler.HandleValidationError("Please select a valid Location.", "Location");
                    AdvancedInventory_Single_ComboBox_Loc.Focus();
                    return;
                }

                if (!int.TryParse(qtyText, out int qty) || qty <= 0)
                {
                    Service_ErrorHandler.HandleValidationError("Please enter a valid quantity.", "Quantity");
                    AdvancedInventory_Single_TextBox_Qty.Focus();
                    return;
                }

                if (!int.TryParse(countText, out int count) || count <= 0)
                {
                    MessageBox.Show(@"Please enter a valid transaction count.", @"Validation Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    AdvancedInventory_Single_TextBox_Count.Focus();
                    return;
                }

                for (int i = 0; i < count; i++)
                {
                    ListViewItem listViewItem = new([
                        partId,
                        op,
                        loc,
                        qty.ToString()
                    ]);
                    AdvancedInventory_Single_ListView.Items.Add(listViewItem);
                    Debug.WriteLine(
                        $"Added item to ListView: Part={partId}, Op={op}, Loc={loc}, Qty={qty}, Notes={notes}");
                }

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

                UpdateSingleSaveButtonState();
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                _ = Dao_ErrorLog.HandleException_GeneralError_CloseApp(ex, false,
                    "AdvancedInventory_Single_Button_Send_Click");
            }
        }

        private void AdvancedInventory_Button_Normal_Click(object? sender, EventArgs e)
        {
            try
            {
                if (Service_Timer_VersionChecker.MainFormInstance is null)
                {
                    LoggingUtility.Log("MainForm instance is null, cannot open Advanced Inventory Entry.");
                    return;
                }

                if (MainFormInstance is not null)
                {
                    MainFormInstance.MainForm_UserControl_InventoryTab.Visible = true;
                    MainFormInstance.MainForm_UserControl_AdvancedInventory.Visible = false;
                    MainFormInstance.MainForm_TabControl.SelectedIndex = 0;
                    Control_InventoryTab? invTab = MainFormInstance.MainForm_UserControl_InventoryTab;
                    if (invTab is not null)
                    {
                        ComboBox? part = invTab.Control_InventoryTab_ComboBox_Part;
                        ComboBox? op = invTab.GetType().GetField("Control_InventoryTab_ComboBox_Operation",
                                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                            ?.GetValue(invTab) as ComboBox;
                        ComboBox? loc = invTab.GetType().GetField("Control_InventoryTab_ComboBox_Location",
                                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                            ?.GetValue(invTab) as ComboBox;
                        if (part is not null)
                        {
                            part.SelectedIndex = 0;
                            part.Focus();
                            part.SelectAll();
                            part.BackColor = Model_AppVariables.UserUiColors.ControlFocusedBackColor ?? Color.LightBlue;
                        }

                        if (op is not null)
                        {
                            op.SelectedIndex = 0;
                        }

                        if (loc is not null)
                        {
                            loc.SelectedIndex = 0;
                        }
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
                _progressHelper?.ShowProgress();
                _progressHelper?.UpdateProgress(10, "Resetting Advanced Inventory (MultiLoc)...");
                Debug.WriteLine("[DEBUG] AdvancedInventory MultiLoc HardReset - start");
                if (MainFormInstance != null)
                {
                    Debug.WriteLine("[DEBUG] Updating status strip for hard reset");
                    MainFormInstance.MainForm_StatusStrip_Disconnected.Text = @"Please wait while resetting...";
                    MainFormInstance.MainForm_StatusStrip_Disconnected.Visible = true;
                    MainFormInstance.MainForm_StatusStrip_SavedStatus.Visible = false;
                }

                _progressHelper?.UpdateProgress(30, "Resetting data tables...");
                Debug.WriteLine("[DEBUG] Hiding ComboBoxes");

                Debug.WriteLine("[DEBUG] Unbinding ComboBox DataSources");
                AdvancedInventory_MultiLoc_ComboBox_Part.DataSource = null;
                AdvancedInventory_MultiLoc_ComboBox_Op.DataSource = null;
                AdvancedInventory_MultiLoc_ComboBox_Loc.DataSource = null;

                Debug.WriteLine("[DEBUG] Resetting and refreshing all ComboBox DataTables");
                await Helper_UI_ComboBoxes.ResetAndRefreshAllDataTablesAsync();
                Debug.WriteLine("[DEBUG] DataTables reset complete");

                _progressHelper?.UpdateProgress(60, "Refilling combo boxes...");
                Debug.WriteLine("[DEBUG] Refilling Part ComboBox");
                await Helper_UI_ComboBoxes.FillPartComboBoxesAsync(AdvancedInventory_MultiLoc_ComboBox_Part);
                Debug.WriteLine("[DEBUG] Refilling Operation ComboBox");
                await Helper_UI_ComboBoxes.FillOperationComboBoxesAsync(AdvancedInventory_MultiLoc_ComboBox_Op);
                Debug.WriteLine("[DEBUG] Refilling Location ComboBox");
                await Helper_UI_ComboBoxes.FillLocationComboBoxesAsync(AdvancedInventory_MultiLoc_ComboBox_Loc);

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

                AdvancedInventory_MultiLoc_ComboBox_Part.ForeColor =
                    Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red;
                AdvancedInventory_MultiLoc_ComboBox_Op.ForeColor =
                    Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red;
                AdvancedInventory_MultiLoc_ComboBox_Loc.ForeColor =
                    Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red;
                AdvancedInventory_MultiLoc_TextBox_Qty.ForeColor =
                    Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red;

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
                    _progressHelper?.HideProgress();
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

                Debug.WriteLine("[DEBUG] Resetting UI fields");
                MainFormControlHelper.ResetComboBox(AdvancedInventory_MultiLoc_ComboBox_Part,
                    Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red, 0);
                MainFormControlHelper.ResetComboBox(AdvancedInventory_MultiLoc_ComboBox_Op,
                    Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red, 0);
                MainFormControlHelper.ResetComboBox(AdvancedInventory_MultiLoc_ComboBox_Loc,
                    Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red, 0);
                MainFormControlHelper.ResetTextBox(AdvancedInventory_MultiLoc_TextBox_Qty,
                    Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red,
                    "[ Enter Valid Quantity ]");
                MainFormControlHelper.ResetComboBox(AdvancedInventory_Single_ComboBox_Part,
                    Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red, 0);
                MainFormControlHelper.ResetComboBox(AdvancedInventory_Single_ComboBox_Op,
                    Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red, 0);
                MainFormControlHelper.ResetComboBox(AdvancedInventory_Single_ComboBox_Loc,
                    Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red, 0);
                MainFormControlHelper.ResetTextBox(AdvancedInventory_Single_TextBox_Qty,
                    Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red,
                    "[ Enter Valid Quantity ]");
                MainFormControlHelper.ResetTextBox(AdvancedInventory_Single_TextBox_Count,
                    Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red,
                    "[ Enter How Many Times ]");
                AdvancedInventory_MultiLoc_RichTextBox_Notes.Text = string.Empty;
                AdvancedInventory_MultiLoc_ListView_Preview.Items.Clear();
                AdvancedInventory_MultiLoc_ComboBox_Part.Enabled = true;
                AdvancedInventory_MultiLoc_ComboBox_Op.Enabled = true;

                AdvancedInventory_MultiLoc_ComboBox_Part.ForeColor =
                    Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red;
                AdvancedInventory_MultiLoc_ComboBox_Op.ForeColor =
                    Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red;
                AdvancedInventory_MultiLoc_ComboBox_Loc.ForeColor =
                    Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red;
                AdvancedInventory_MultiLoc_TextBox_Qty.ForeColor =
                    Model_AppVariables.UserUiColors.ErrorColor ?? Color.Red;

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
                if ((ModifierKeys & Keys.Shift) == Keys.Shift)
                {
                    await AdvancedInventory_MultiLoc_HardResetAsync();
                }
                else
                {
                    AdvancedInventory_MultiLoc_SoftReset();
                }
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

                string partId = AdvancedInventory_MultiLoc_ComboBox_Part.Text;
                string op = AdvancedInventory_MultiLoc_ComboBox_Op.Text;
                string loc = AdvancedInventory_MultiLoc_ComboBox_Loc.Text;
                string qtyText = AdvancedInventory_MultiLoc_TextBox_Qty.Text.Trim();
                string notes = AdvancedInventory_MultiLoc_RichTextBox_Notes.Text.Trim();

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

                if (!int.TryParse(qtyText, out int qty) || qty <= 0)
                {
                    MessageBox.Show(@"Please enter a valid quantity.", @"Validation Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    AdvancedInventory_MultiLoc_TextBox_Qty.Focus();
                    return;
                }

                foreach (ListViewItem item in AdvancedInventory_MultiLoc_ListView_Preview.Items)
                {
                    // Always pass null/empty for batchNumber to ensure each transaction gets a unique batch number
                    if (string.Equals(item.SubItems[0].Text, loc, StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show(@"This location has already been added.", @"Duplicate Entry",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        AdvancedInventory_MultiLoc_ComboBox_Loc.Focus();
                        return;
                    }
                }

                ListViewItem listViewItem = new([
                    loc,
                    qty.ToString(),
                    notes
                ]);
                AdvancedInventory_MultiLoc_ListView_Preview.Items.Add(listViewItem);

                LoggingUtility.Log(
                    $"Added MultiLoc entry: PartId = {partId}, Op = {op}, Loc={loc}, Qty={qty}, Notes={notes}");

                if (AdvancedInventory_MultiLoc_ListView_Preview.Items.Count == 1)
                {
                    AdvancedInventory_MultiLoc_ComboBox_Part.Enabled = false;
                }

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

                if (AdvancedInventory_MultiLoc_ListView_Preview.Items.Count == 0)
                {
                    MessageBox.Show(@"Please add at least one location entry before saving.", @"No Entries",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string partId = AdvancedInventory_MultiLoc_ComboBox_Part.Text;
                string op = AdvancedInventory_MultiLoc_ComboBox_Op.Text;

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

                HashSet<string> locations = new();
                int totalQty = 0;
                int savedCount = 0;
                foreach (ListViewItem item in AdvancedInventory_MultiLoc_ListView_Preview.Items)
                {
                    // Always pass null/empty for batchNumber to ensure each transaction gets a unique batch number
                    string loc = item.SubItems[0].Text;
                    string qtyText = item.SubItems[1].Text;
                    string notes = item.SubItems[2].Text;

                    if (!int.TryParse(qtyText, out int qty) || qty <= 0)
                    {
                        LoggingUtility.LogApplicationError(
                            new Exception($"Invalid quantity for location '{loc}': '{qtyText}'"));
                        continue;
                    }

                    Model_AppVariables.PartId = partId;
                    Model_AppVariables.Operation = op;
                    Model_AppVariables.Location = loc;
                    Model_AppVariables.Notes = notes;
                    Model_AppVariables.InventoryQuantity = qty;
                    Model_AppVariables.User ??= Environment.UserName;
                    Model_AppVariables.PartType ??= "";

                    // Pass null/empty for batchNumber for unique batch per transaction
                    await Dao_Inventory.AddInventoryItemAsync(
                        partId,
                        loc,
                        op,
                        qty,
                        Model_AppVariables.PartType ?? "",
                        Model_AppVariables.User,
                        null, // <-- ensure unique batch number
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

                if (MainFormInstance != null && savedCount > 0)
                {
                    string time = DateTime.Now.ToString("hh:mm tt");
                    string locDisplay = locations.Count > 1 ? "Multiple Locations" : locations.FirstOrDefault() ?? "";
                    MainFormInstance.MainForm_StatusStrip_SavedStatus.Text =
                        $"Last Inventoried: {partId} (Op: {op}), Location: {locDisplay}, Quantity: {totalQty} @ {time}";
                }

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

        private static async Task<string> GetWipAppExcelUserFolderAsync()
        {
            string? server = new MySqlConnectionStringBuilder(Model_AppVariables.ConnectionString).Server;
            string userName = Model_AppVariables.User ?? Environment.UserName;
            string logFilePath = await Helper_Database_Variables.GetLogFilePathAsync(server, userName);
            string logDir = Directory.GetParent(logFilePath)?.Parent?.FullName ?? "";
            string excelRoot = Path.Combine(logDir, "WIP App Excel Files");
            string userFolder = Path.Combine(excelRoot, userName);
            if (!Directory.Exists(userFolder))
            {
                Directory.CreateDirectory(userFolder);
            }

            return userFolder;
        }

        private static async Task<string> GetUserExcelFilePathAsync()
        {
            string userFolder = await GetWipAppExcelUserFolderAsync();
            string fileName = $"{Model_AppVariables.User ?? Environment.UserName}_import.xlsx";
            return Path.Combine(userFolder, fileName);
        }

        // Fix for CS8600: Converting null literal or possible null value to non-nullable type.
        // The problematic line is:
        // string excelPath = GetUserExcelFilePathAsync().ToString();
        // GetUserExcelFilePathAsync() returns a Task<string>, so .ToString() does not return the path, but the type name.
        // The correct way is to await the Task and ensure the result is not null.

        private async void AdvancedInventory_Import_Button_OpenExcel_Click(object? sender, EventArgs e)
        {
            try
            {
                string excelPath = await GetUserExcelFilePathAsync();
                if (!File.Exists(excelPath))
                {
                    string? userFolder = Path.GetDirectoryName(excelPath);
                    if (!string.IsNullOrEmpty(userFolder) && !Directory.Exists(userFolder))
                    {
                        Directory.CreateDirectory(userFolder!);
                    }

                    string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Controls",
                        "MainForm", "WIPAppTemplate.xlsx");
                    if (File.Exists(templatePath))
                    {
                        File.Copy(templatePath, excelPath, false);
                    }
                    else
                    {
                        MessageBox.Show($@"Excel template not found: {templatePath}", @"Template Not Found",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                Process.Start(new ProcessStartInfo(excelPath) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                MessageBox.Show($@"Failed to open Excel file: {ex.Message}", @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // Fix for CS8600 in AdvancedInventory_Import_Button_ImportExcel_Click and other usages
        private async void AdvancedInventory_Import_Button_ImportExcel_Click(object? sender, EventArgs e)
        {
            try
            {
                string excelPath = await GetUserExcelFilePathAsync();
                if (!File.Exists(excelPath))
                {
                    MessageBox.Show(@"Excel file not found. Please create or open the Excel file first.",
                        @"File Not Found",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataTable dt = new();
                using (XLWorkbook workbook = new(excelPath))
                {
                    IXLWorksheet? worksheet = workbook.Worksheet("Tab 1");
                    if (worksheet == null)
                    {
                        MessageBox.Show(@"Worksheet 'Tab 1' not found in the Excel file.", @"Worksheet Not Found",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    IXLRange? usedRange = worksheet.RangeUsed();
                    if (usedRange == null)
                    {
                        MessageBox.Show(@"No data found in 'Tab 1'.", @"No Data", MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        return;
                    }

                    int colCount = usedRange.ColumnCount();
                    int rowCount = usedRange.RowCount();

                    IXLRangeRow? headerRow = usedRange.Row(1);
                    for (int col = 1; col <= colCount; col++)
                    {
                        string? colName = headerRow.Cell(col).GetValue<string>();
                        if (string.IsNullOrWhiteSpace(colName))
                        {
                            colName = $"Column{col}";
                        }

                        dt.Columns.Add(colName);
                    }

                    for (int row = 2; row <= rowCount; row++)
                    {
                        DataRow dataRow = dt.NewRow();
                        for (int col = 1; col <= colCount; col++)
                        {
                            dataRow[col - 1] = usedRange.Row(row).Cell(col).GetValue<string>();
                        }

                        dt.Rows.Add(dataRow);
                    }
                }

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show(@"No data found in the Excel file to import.", @"No Data", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                AdvancedInventory_Import_DataGridView.DataSource = dt;
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                MessageBox.Show($@"Failed to import Excel data: {ex.Message}", @"Import Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // Fix for CS8600 in AdvancedInventory_Import_Button_Save_Click and RefreshImportDataGridView
        private async void AdvancedInventory_Import_Button_Save_Click(object? sender, EventArgs e)
        {
            if (AdvancedInventory_Import_DataGridView.DataSource == null)
            {
                return;
            }

            DataGridView? dgv = AdvancedInventory_Import_DataGridView;
            List<DataGridViewRow> rowsToRemove = new();
            bool anyError = false;

            DataTable? partTable = AdvancedInventory_Single_ComboBox_Part.DataSource as DataTable;
            DataTable? opTable = AdvancedInventory_Single_ComboBox_Op.DataSource as DataTable;
            DataTable? locTable = AdvancedInventory_Single_ComboBox_Loc.DataSource as DataTable;

            HashSet<string?> validParts =
                partTable?.AsEnumerable().Select(r => r.Field<string>("PartID"))
                    .Where(s => !string.IsNullOrWhiteSpace(s)).ToHashSet(StringComparer.OrdinalIgnoreCase) ??
                [];
            HashSet<string?> validOps =
                opTable?.AsEnumerable().Select(r => r.Field<string>("Operation"))
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .ToHashSet(StringComparer.OrdinalIgnoreCase) ?? [];
            HashSet<string?> validLocs =
                locTable?.AsEnumerable().Select(r => r.Field<string>("Location"))
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .ToHashSet(StringComparer.OrdinalIgnoreCase) ?? [];

            string excelPath = await GetUserExcelFilePathAsync();
            XLWorkbook? workbook = null;
            IXLWorksheet? worksheet = null;
            if (File.Exists(excelPath))
            {
                workbook = new XLWorkbook(excelPath);
                worksheet = workbook.Worksheet("Tab 1");
            }

            List<int> excelRowsToDelete = new();

            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow)
                {
                    continue;
                }

                // Always pass null/empty for batchNumber to ensure each transaction gets a unique batch number
                bool rowValid = true;
                foreach (DataGridViewCell cell in row.Cells)
                {
                    cell.Style.ForeColor = Model_AppVariables.UserUiColors.TextBoxForeColor ?? Color.Black;
                }

                string part = row.Cells["Part"].Value?.ToString() ?? "";
                string op = row.Cells["Operation"].Value?.ToString() ?? "";
                string loc = row.Cells["Location"].Value?.ToString() ?? "";
                string qtyText = row.Cells["Quantity"].Value?.ToString() ?? "";
                string notesOriginal = row.Cells["Notes"].Value?.ToString() ?? "";
                string notes = "Excel Import: " + notesOriginal;

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

                if (!int.TryParse(qtyText, out int qty) || qty <= 0)
                {
                    row.Cells["Quantity"].Style.ForeColor =
                        Model_AppVariables.UserUiColors.TextBoxErrorForeColor ?? Color.Red;
                    rowValid = false;
                }

                if (rowValid)
                {
                    try
                    {
                        // Pass null/empty for batchNumber for unique batch per transaction
                        await Dao_Inventory.AddInventoryItemAsync(
                            part, loc, op, qty, "", Model_AppVariables.User ?? Environment.UserName, null, notes, true);
                    }
                    catch (Exception ex)
                    {
                        LoggingUtility.LogApplicationError(ex);
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            cell.Style.ForeColor = Model_AppVariables.UserUiColors.TextBoxErrorForeColor ?? Color.Red;
                        }

                        rowValid = false;
                        anyError = true;
                    }
                }
                else
                {
                    anyError = true;
                }
            }

            if (worksheet != null && excelRowsToDelete.Count > 0)
            {
                excelRowsToDelete.Sort((a, b) => b.CompareTo(a));
                foreach (int rowNum in excelRowsToDelete)
                {
                    worksheet.Row(rowNum).Delete();
                }

                IXLRange? usedRange = worksheet.RangeUsed();
                if (usedRange != null)
                {
                    int headerRow = usedRange.FirstRow().RowNumber();
                    int lastRow = worksheet.LastRowUsed()?.RowNumber() ?? headerRow;
                }

                workbook?.Save();
            }

            foreach (DataGridViewRow row in rowsToRemove)
            {
                if (!row.IsNewRow)
                {
                    dgv.Rows.Remove(row);
                }
            }

            RefreshImportDataGridView();

            if (!anyError)
            {
                MessageBox.Show(@"All transactions saved successfully.", @"Success", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                if (MainFormInstance != null)
                {
                    MainFormInstance.MainForm_StatusStrip_SavedStatus.Text =
                        $"Last Import: {DateTime.Now:hh:mm tt} ({dgv.Rows.Count} rows imported)";
                }
            }
            else
            {
                MessageBox.Show(@"Some rows could not be saved. Please correct highlighted errors.",
                    @"Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void RefreshImportDataGridView()
        {
            string excelPath = await GetUserExcelFilePathAsync();
            if (!File.Exists(excelPath))
            {
                return;
            }

            Dictionary<int, HashSet<string>> highlightMap = new();
            if (AdvancedInventory_Import_DataGridView.DataSource is DataTable)
            {
                foreach (DataGridViewRow row in AdvancedInventory_Import_DataGridView.Rows)
                {
                    if (row.IsNewRow)
                    {
                        continue;
                    }

                    HashSet<string> cols = new();
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Style.ForeColor ==
                            (Model_AppVariables.UserUiColors.TextBoxErrorForeColor ?? Color.Red))
                        {
                            if (cell.OwningColumn != null)
                            {
                                cols.Add(cell.OwningColumn.Name);
                            }
                        }
                    }

                    if (cols.Count > 0)
                    {
                        highlightMap[row.Index] = cols;
                    }
                }
            }

            DataTable dt = new();
            using (XLWorkbook workbook = new(excelPath))
            {
                IXLWorksheet? worksheet = workbook.Worksheet("Tab 1");
                if (worksheet == null)
                {
                    return;
                }

                IXLRange? usedRange = worksheet.RangeUsed();
                if (usedRange == null)
                {
                    return;
                }

                int colCount = usedRange.ColumnCount();
                int rowCount = usedRange.RowCount();

                IXLRangeRow? headerRow = usedRange.Row(1);
                for (int col = 1; col <= colCount; col++)
                {
                    string? colName = headerRow.Cell(col).GetValue<string>();
                    if (string.IsNullOrWhiteSpace(colName))
                    {
                        colName = $"Column{col}";
                    }

                    dt.Columns.Add(colName);
                }

                for (int row = 2; row <= rowCount; row++)
                {
                    DataRow dataRow = dt.NewRow();
                    for (int col = 1; col <= colCount; col++)
                    {
                        dataRow[col - 1] = usedRange.Row(row).Cell(col).GetValue<string>();
                    }

                    dt.Rows.Add(dataRow);
                }
            }

            AdvancedInventory_Import_DataGridView.DataSource = dt;

            foreach (DataGridViewRow row in AdvancedInventory_Import_DataGridView.Rows)
            {
                if (row.IsNewRow)
                {
                    continue;
                }

                if (highlightMap.TryGetValue(row.Index, out HashSet<string>? cols))
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.OwningColumn != null && cols.Contains(cell.OwningColumn.Name))
                        {
                            cell.Style.ForeColor = Model_AppVariables.UserUiColors.TextBoxErrorForeColor ?? Color.Red;
                        }
                    }
                }

                if (row.DataGridView != null && row.DataGridView.Columns.Contains("Quantity"))
                {
                    DataGridViewCell? qtyCell = row.Cells["Quantity"];
                    string qtyText = qtyCell.Value?.ToString() ?? "";
                    if (!int.TryParse(qtyText, out int qty) || qty <= 0)
                    {
                        qtyCell.Style.ForeColor = Model_AppVariables.UserUiColors.TextBoxErrorForeColor ?? Color.Red;
                    }
                }
            }
        }

        #endregion

        private void AdvancedInventory_MultiLoc_Label_Notes_Click(object sender, EventArgs e)
        {

        }
    }
}

# endregion
