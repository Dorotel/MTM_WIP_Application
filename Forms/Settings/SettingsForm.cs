

using System.Text.Json;
using MTM_Inventory_Application.Controls.SettingsForm;
using MTM_Inventory_Application.Controls.Shared;
using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Forms.Settings;

public partial class SettingsForm : Form
{
    #region Fields
    

    private bool _hasChanges = false;
    private readonly Dictionary<string, Panel> _settingsPanels;
    private string? _originalThemeName;
    private ProgressBarUserControl _loadingProgress = null!;

    private void ShowLoadingProgress(string status = "Loading...")
    
    #endregion
    
    #region Constructors
    

    public SettingsForm()
    {
        InitializeComponent();

        _settingsPanels = new Dictionary<string, Panel>
        {
            ["Database"] = SettingsForm_Panel_Database,
            ["Add User"] = SettingsForm_Panel_AddUser,
            ["Edit User"] = SettingsForm_Panel_EditUser,
            ["Delete User"] = SettingsForm_Panel_DeleteUser,
            ["Add Part Number"] = SettingsForm_Panel_AddPart,
            ["Edit Part Number"] = SettingsForm_Panel_EditPart,
            ["Remove Part Number"] = SettingsForm_Panel_RemovePart,
            ["Add Operation"] = SettingsForm_Panel_AddOperation,
            ["Edit Operation"] = SettingsForm_Panel_EditOperation,
            ["Remove Operation"] = SettingsForm_Panel_RemoveOperation,
            ["Add Location"] = SettingsForm_Panel_AddLocation,
            ["Edit Location"] = SettingsForm_Panel_EditLocation,
            ["Remove Location"] = SettingsForm_Panel_RemoveLocation,
            ["Add ItemType"] = SettingsForm_Panel_AddItemType,
            ["Edit ItemType"] = SettingsForm_Panel_EditItemType,
            ["Remove ItemType"] = SettingsForm_Panel_RemoveItemType,
            ["Theme"] = SettingsForm_Panel_Theme,
            ["Shortcuts"] = SettingsForm_Panel_Shortcuts,
            ["About"] = SettingsForm_Panel_About
        };

        _originalThemeName = Model_AppVariables.ThemeName;

        InitializeProgressControl();

        SettingsForm_ComboBox_Theme.SelectedIndexChanged += ThemeComboBox_SelectedIndexChanged;

        SettingsForm_DataGridView_Shortcuts.CellBeginEdit += ShortcutsDataGridView_CellBeginEdit;

        InitializeUserControls();

        InitializeForm();
    }
    
    #endregion
    
    #region Methods
    

    private void InitializeCategoryTreeView()
    {
        SettingsForm_TreeView_Category.Nodes.Clear();

        var databaseNode = SettingsForm_TreeView_Category.Nodes.Add("Database", "Database");

        var usersNode = SettingsForm_TreeView_Category.Nodes.Add("Users", "Users");
        usersNode.Nodes.Add("Add User", "Add User");
        usersNode.Nodes.Add("Edit User", "Edit User");
        usersNode.Nodes.Add("Delete User", "Delete User");

        var partNumbersNode = SettingsForm_TreeView_Category.Nodes.Add("Part Numbers", "Part Numbers");
        partNumbersNode.Nodes.Add("Add Part Number", "Add Part Number");
        partNumbersNode.Nodes.Add("Edit Part Number", "Edit Part Number");
        partNumbersNode.Nodes.Add("Remove Part Number", "Remove Part Number");

        var operationsNode = SettingsForm_TreeView_Category.Nodes.Add("Operations", "Operations");
        operationsNode.Nodes.Add("Add Operation", "Add Operation");
        operationsNode.Nodes.Add("Edit Operation", "Edit Operation");
        operationsNode.Nodes.Add("Remove Operation", "Remove Operation");

        var locationsNode = SettingsForm_TreeView_Category.Nodes.Add("Locations", "Locations");
        locationsNode.Nodes.Add("Add Location", "Add Location");
        locationsNode.Nodes.Add("Edit Location", "Edit Location");
        locationsNode.Nodes.Add("Remove Location", "Remove Location");

        var itemTypesNode = SettingsForm_TreeView_Category.Nodes.Add("ItemTypes", "ItemTypes");
        itemTypesNode.Nodes.Add("Add ItemType", "Add ItemType");
        itemTypesNode.Nodes.Add("Edit ItemType", "Edit ItemType");
        itemTypesNode.Nodes.Add("Remove ItemType", "Remove ItemType");

        var themeNode = SettingsForm_TreeView_Category.Nodes.Add("Theme", "Theme");
        var shortcutsNode = SettingsForm_TreeView_Category.Nodes.Add("Shortcuts", "Shortcuts");
        var aboutNode = SettingsForm_TreeView_Category.Nodes.Add("About", "About");

        SettingsForm_TreeView_Category.CollapseAll();

        SettingsForm_TreeView_Category.SelectedNode = databaseNode;
    }

    private void InitializeUserControls()
    {
        var SettingsForm_Panel_AddUser = _settingsPanels["Add User"];
        var addUserControl = new AddUserControl();
        addUserControl.Dock = DockStyle.Fill;
        addUserControl.UserAdded += (s, e) => { UpdateStatus("User added successfully."); };
        SettingsForm_Panel_AddUser.Controls.Add(addUserControl);

        var SettingsForm_Panel_EditUser = _settingsPanels["Edit User"];
        var editUserControl = new EditUserControl();
        editUserControl.Dock = DockStyle.Fill;
        editUserControl.UserEdited += (s, e) => { UpdateStatus("User updated successfully."); };
        SettingsForm_Panel_EditUser.Controls.Add(editUserControl);

        var SettingsForm_Panel_DeleteUser = _settingsPanels["Delete User"];
        var removeUserControl = new RemoveUserControl();
        removeUserControl.Dock = DockStyle.Fill;
        removeUserControl.UserRemoved += (s, e) => { UpdateStatus("User removed successfully."); };
        SettingsForm_Panel_DeleteUser.Controls.Add(removeUserControl);

        var addPartControl = new AddPartControl();
        addPartControl.Dock = DockStyle.Fill;
        addPartControl.PartAdded += (s, e) => { UpdateStatus("Part added successfully - lists refreshed"); };
        SettingsForm_Panel_AddPart.Controls.Add(addPartControl);

        var editPartControl = new EditPartControl();
        editPartControl.Dock = DockStyle.Fill;
        editPartControl.PartUpdated += (s, e) => { UpdateStatus("Part updated successfully - lists refreshed"); };
        SettingsForm_Panel_EditPart.Controls.Add(editPartControl);

        var removePartControl = new RemovePartControl();
        removePartControl.Dock = DockStyle.Fill;
        removePartControl.PartRemoved += (s, e) => { UpdateStatus("Part removed successfully - lists refreshed"); };
        SettingsForm_Panel_RemovePart.Controls.Add(removePartControl);

        var addOperationControl = new AddOperationControl();
        addOperationControl.Dock = DockStyle.Fill;
        addOperationControl.OperationAdded += (s, e) =>
        {
            UpdateStatus("Operation added successfully - lists refreshed");
        };
        SettingsForm_Panel_AddOperation.Controls.Add(addOperationControl);

        var editOperationControl = new EditOperationControl();
        editOperationControl.Dock = DockStyle.Fill;
        editOperationControl.OperationUpdated += (s, e) =>
        {
            UpdateStatus("Operation updated successfully - lists refreshed");
        };
        SettingsForm_Panel_EditOperation.Controls.Add(editOperationControl);

        var removeOperationControl = new RemoveOperationControl();
        removeOperationControl.Dock = DockStyle.Fill;
        removeOperationControl.OperationRemoved += (s, e) =>
        {
            UpdateStatus("Operation removed successfully - lists refreshed");
        };
        SettingsForm_Panel_RemoveOperation.Controls.Add(removeOperationControl);

        var addLocationControl = new AddLocationControl();
        addLocationControl.Dock = DockStyle.Fill;
        addLocationControl.LocationAdded += (s, e) =>
        {
            UpdateStatus("Location added successfully - lists refreshed");
        };
        SettingsForm_Panel_AddLocation.Controls.Add(addLocationControl);

        var editLocationControl = new EditLocationControl();
        editLocationControl.Dock = DockStyle.Fill;
        editLocationControl.LocationUpdated += (s, e) =>
        {
            UpdateStatus("Location updated successfully - lists refreshed");
        };
        SettingsForm_Panel_EditLocation.Controls.Add(editLocationControl);

        var removeLocationControl = new RemoveLocationControl();
        removeLocationControl.Dock = DockStyle.Fill;
        removeLocationControl.LocationRemoved += (s, e) =>
        {
            UpdateStatus("Location removed successfully - lists refreshed");
        };
        SettingsForm_Panel_RemoveLocation.Controls.Add(removeLocationControl);

        var addItemTypeControl = new AddItemTypeControl();
        addItemTypeControl.Dock = DockStyle.Fill;
        addItemTypeControl.ItemTypeAdded += (s, e) =>
        {
            UpdateStatus("ItemType added successfully - lists refreshed");
        };
        SettingsForm_Panel_AddItemType.Controls.Add(addItemTypeControl);

        var editItemTypeControl = new EditItemTypeControl();
        editItemTypeControl.Dock = DockStyle.Fill;
        editItemTypeControl.ItemTypeUpdated += (s, e) =>
        {
            UpdateStatus("ItemType updated successfully - lists refreshed");
        };
        SettingsForm_Panel_EditItemType.Controls.Add(editItemTypeControl);

        var removeItemTypeControl = new RemoveItemTypeControl();
        removeItemTypeControl.Dock = DockStyle.Fill;
        removeItemTypeControl.ItemTypeRemoved += (s, e) =>
        {
            UpdateStatus("ItemType removed successfully - lists refreshed");
        };
        SettingsForm_Panel_RemoveItemType.Controls.Add(removeItemTypeControl);
    }

    private void ThemeComboBox_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (SettingsForm_ComboBox_Theme.SelectedItem is string themeName && !string.IsNullOrWhiteSpace(themeName))
        {
            var theme = Core_Themes.Core_AppThemes.GetTheme(themeName);
            var originalThemeName = Model_AppVariables.ThemeName;
            Model_AppVariables.ThemeName = themeName;
            Core_Themes.ApplyTheme(this);
            Model_AppVariables.ThemeName = originalThemeName;
        }
    }

    private void InitializeForm()
    {
        Text = "Settings - MTM WIP Application";
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;

        foreach (var panel in _settingsPanels.Values)
        {
            SettingsForm_Panel_Settings.Controls.Add(panel);
            panel.Visible = false;
        }

        InitializeCategoryTreeView();
        ShowPanel("Database");

        LoadSettings();

        ApplyTheme();
    }

    private void ApplyTheme()
    {
        try
        {
            Core_Themes.ApplyTheme(this);
            Core_Themes.ApplyThemeToDataGridView(SettingsForm_DataGridView_Shortcuts);
            Core_Themes.SizeDataGrid(SettingsForm_DataGridView_Shortcuts);
        }
        catch (Exception ex)
        {
            UpdateStatus($"Warning: Could not apply theme - {ex.Message}");
        }
    }

    private async void LoadSettings()
    {
        try
        {
            ShowLoadingProgress("Loading settings...");
            UpdateLoadingProgress(10, "Loading settings...");

            UpdateLoadingProgress(25, "Loading database settings...");
            await LoadDatabaseSettings();

            UpdateLoadingProgress(50, "Loading theme settings...");
            await LoadThemeSettings();

            UpdateLoadingProgress(75, "Loading shortcuts...");
            await LoadShortcuts();

            UpdateLoadingProgress(90, "Loading about information...");
            LoadAboutInfo();

            UpdateLoadingProgress(100, "Settings loaded successfully");
            UpdateStatus("Settings loaded successfully");
            _hasChanges = false;

            await Task.Delay(500);
            HideLoadingProgress();
        }
        catch (Exception ex)
        {
            HideLoadingProgress();
            UpdateStatus($"Error loading settings: {ex.Message}");
        }
    }

    private async Task LoadDatabaseSettings()
    {
        try
        {
            var user = Model_AppVariables.User;

            SettingsForm_TextBox_Server.Text = await Dao_User.GetWipServerAddressAsync(user) ?? "172.16.1.104";
            SettingsForm_TextBox_Port.Text = await Dao_User.GetWipServerPortAsync(user) ?? "3306";
            SettingsForm_TextBox_Database.Text = await Dao_User.GetDatabaseAsync(user) ?? "mtm_wip_application";
            SettingsForm_TextBox_Username.Text = await Dao_User.GetVisualUserNameAsync(user) ?? "";
            SettingsForm_TextBox_Password.Text = await Dao_User.GetVisualPasswordAsync(user) ?? "";
        }
        catch (Exception ex)
        {
            UpdateStatus($"Error loading database settings: {ex.Message}");
        }
    }

    private async Task LoadThemeSettings()
    {
        try
        {
            SettingsForm_ComboBox_Theme.Items.Clear();
            var themeNames = Core_Themes.Core_AppThemes.GetThemeNames().ToArray();
            SettingsForm_ComboBox_Theme.Items.AddRange(themeNames);

            var user = Model_AppVariables.User;
            var themeName = await Dao_User.GetThemeNameAsync(user);
            var fontSize = await Dao_User.GetThemeFontSizeAsync(user) ?? 9;

            if (!string.IsNullOrEmpty(themeName) && SettingsForm_ComboBox_Theme.Items.Contains(themeName))
                SettingsForm_ComboBox_Theme.SelectedItem = themeName;
            else if (SettingsForm_ComboBox_Theme.Items.Count > 0) SettingsForm_ComboBox_Theme.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            UpdateStatus($"Error loading theme settings: {ex.Message}");
            if (SettingsForm_ComboBox_Theme.Items.Count > 0)
                SettingsForm_ComboBox_Theme.SelectedIndex = 0;
        }
    }

    private async Task LoadShortcuts()
    {
        try
        {
            SettingsForm_DataGridView_Shortcuts.Columns.Clear();
            SettingsForm_DataGridView_Shortcuts.Columns.Add("Action", "Action");
            SettingsForm_DataGridView_Shortcuts.Columns.Add("Shortcut", "Shortcut");
            SettingsForm_DataGridView_Shortcuts.Rows.Clear();

            var user = Core_WipAppVariables.User;
            var shortcutsJson = await Dao_User.GetShortcutsJsonAsync(user);

            var shortcutDict = Helper_UI_Shortcuts.GetShortcutDictionary();

            Dictionary<string, string> userShortcuts = new();
            if (!string.IsNullOrWhiteSpace(shortcutsJson))
                try
                {
                    using var doc = JsonDocument.Parse(shortcutsJson);
                    if (doc.RootElement.ValueKind == JsonValueKind.Object &&
                        doc.RootElement.TryGetProperty("Shortcuts", out var shortcutsElement) &&
                        shortcutsElement.ValueKind == JsonValueKind.Object)
                        foreach (var prop in shortcutsElement.EnumerateObject())
                            userShortcuts[prop.Name] = prop.Value.GetString() ?? "";
                }
                catch (JsonException)
                {
                    UpdateStatus("Warning: Shortcuts JSON is malformed. Using defaults.");
                }

            foreach (var kvp in shortcutDict)
            {
                var action = kvp.Key;
                var defaultKeys = kvp.Value;
                var shortcutValue = userShortcuts.TryGetValue(action, out var val) && !string.IsNullOrWhiteSpace(val)
                    ? val
                    : Helper_UI_Shortcuts.ToShortcutString(defaultKeys);

                Helper_UI_Shortcuts.ApplyShortcutFromDictionary(action,
                    Helper_UI_Shortcuts.FromShortcutString(shortcutValue));

                SettingsForm_DataGridView_Shortcuts.Rows.Add(action, shortcutValue);
            }

            SettingsForm_DataGridView_Shortcuts.ReadOnly = false;
            SettingsForm_DataGridView_Shortcuts.AllowUserToAddRows = false;
            SettingsForm_DataGridView_Shortcuts.AllowUserToDeleteRows = false;
            SettingsForm_DataGridView_Shortcuts.Columns[0].ReadOnly = true;
            SettingsForm_DataGridView_Shortcuts.Columns[1].ReadOnly = false;
            SettingsForm_DataGridView_Shortcuts.CellValueChanged += ShortcutsDataGridView_CellValueChanged;
            SettingsForm_DataGridView_Shortcuts.CellValidating += ShortcutsDataGridView_CellValidating;
        }
        catch (Exception ex)
        {
            UpdateStatus($"Error loading shortcuts: {ex.Message}");
        }
    }

    private void ShortcutsDataGridView_CellValidating(object? sender, DataGridViewCellValidatingEventArgs e)
    {
        if (e.ColumnIndex == 1)
        {
            var shortcutString = e.FormattedValue?.ToString() ?? "";

            if (string.IsNullOrWhiteSpace(shortcutString)) return;

            try
            {
                var keys = Helper_UI_Shortcuts.FromShortcutString(shortcutString);
                if (keys == Keys.None && !string.IsNullOrWhiteSpace(shortcutString))
                {
                    e.Cancel = true;
                    UpdateStatus("Invalid shortcut format. Use combinations like 'CTRL + S' or 'ALT + F1'");
                }
            }
            catch
            {
                e.Cancel = true;
                UpdateStatus("Invalid shortcut format. Use combinations like 'CTRL + S' or 'ALT + F1'");
            }
        }
    }

    private void ShortcutsDataGridView_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.ColumnIndex == 1 && e.RowIndex >= 0)
        {
            var actionName = SettingsForm_DataGridView_Shortcuts.Rows[e.RowIndex].Cells[0].Value?.ToString();
            var shortcutString = SettingsForm_DataGridView_Shortcuts.Rows[e.RowIndex].Cells[1].Value?.ToString() ?? "";

            if (!string.IsNullOrEmpty(actionName))
                try
                {
                    var keys = Helper_UI_Shortcuts.FromShortcutString(shortcutString);
                    Helper_UI_Shortcuts.ApplyShortcutFromDictionary(actionName, keys);
                    _hasChanges = true;
                    UpdateStatus($"Shortcut updated: {actionName}");
                }
                catch (Exception ex)
                {
                    UpdateStatus($"Error updating shortcut: {ex.Message}");
                }
        }
    }

    private void ShortcutsDataGridView_CellBeginEdit(object? sender, DataGridViewCellCancelEventArgs e)
    {
        if (e.ColumnIndex == 1 && e.RowIndex >= 0)
        {
            e.Cancel = true;

            var actionName = SettingsForm_DataGridView_Shortcuts.Rows[e.RowIndex].Cells[0].Value?.ToString();
            var currentShortcut = SettingsForm_DataGridView_Shortcuts.Rows[e.RowIndex].Cells[1].Value?.ToString() ?? "";

            using (var inputForm = new Form())
            {
                inputForm.Text = $"Set Shortcut for '{actionName}'";
                inputForm.Size = new Size(400, 180);
                inputForm.StartPosition = FormStartPosition.CenterParent;
                inputForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                inputForm.MaximizeBox = false;
                inputForm.MinimizeBox = false;

                var label = new Label
                {
                    Text = "Press the new shortcut key combination:",
                    Location = new Point(10, 20),
                    Size = new Size(360, 20)
                };
                var shortcutBox = new TextBox
                {
                    Location = new Point(10, 45),
                    Size = new Size(360, 20),
                    ReadOnly = true,
                    TabStop = false,
                    BackColor = SystemColors.Control,
                    ForeColor = SystemColors.GrayText
                };
                shortcutBox.Text = currentShortcut;

                var errorLabel = new Label
                {
                    Text = "",
                    ForeColor = Color.Red,
                    Location = new Point(10, 70),
                    Size = new Size(360, 30),
                    Visible = false
                };

                var newKeys = Helper_UI_Shortcuts.FromShortcutString(currentShortcut);

                inputForm.KeyPreview = true;
                inputForm.KeyDown += (s, ke) =>
                {
                    if (ke.KeyCode == Keys.Escape)
                    {
                        inputForm.DialogResult = DialogResult.Cancel;
                        inputForm.Close();
                        return;
                    }

                    if (ke.KeyCode == Keys.ControlKey || ke.KeyCode == Keys.ShiftKey || ke.KeyCode == Keys.Menu)
                        return;

                    newKeys = ke.KeyData;
                    shortcutBox.Text = Helper_UI_Shortcuts.ToShortcutString(newKeys);

                    var hasModifier = (newKeys & Keys.Control) == Keys.Control ||
                                      (newKeys & Keys.Alt) == Keys.Alt ||
                                      (newKeys & Keys.Shift) == Keys.Shift;

                    if (!hasModifier)
                    {
                        errorLabel.Text = "Shortcut must include ALT, CTRL, SHIFT, or a combination.";
                        errorLabel.Visible = true;
                    }
                    else if (IsShortcutConflict(actionName, newKeys))
                    {
                        errorLabel.Text = "This shortcut is already assigned to another action in the same tab.";
                        errorLabel.Visible = true;
                    }
                    else
                    {
                        errorLabel.Text = "";
                        errorLabel.Visible = false;
                    }

                    ke.SuppressKeyPress = true;
                };

                var okButton = new Button { Text = "OK", Location = new Point(215, 110), Size = new Size(75, 23) };
                var SettingsForm_Button_Cancel = new Button
                { Text = "Cancel", Location = new Point(295, 110), Size = new Size(75, 23) };

                okButton.Click += (s, args) =>
                {
                    var hasModifier = (newKeys & Keys.Control) == Keys.Control ||
                                      (newKeys & Keys.Alt) == Keys.Alt ||
                                      (newKeys & Keys.Shift) == Keys.Shift;

                    if (newKeys == Keys.None || !hasModifier)
                    {
                        errorLabel.Text = "Shortcut must include ALT, CTRL, SHIFT, or a combination.";
                        errorLabel.Visible = true;
                        return;
                    }

                    if (IsShortcutConflict(actionName, newKeys))
                    {
                        errorLabel.Text = "This shortcut is already assigned to another action in the same tab.";
                        errorLabel.Visible = true;
                        return;
                    }

                    errorLabel.Text = "";
                    errorLabel.Visible = false;
                    inputForm.DialogResult = DialogResult.OK;
                    inputForm.Close();
                };
                SettingsForm_Button_Cancel.Click += (s, args) =>
                {
                    inputForm.DialogResult = DialogResult.Cancel;
                    inputForm.Close();
                };

                inputForm.Controls.AddRange(new Control[] { label, shortcutBox, errorLabel, okButton, SettingsForm_Button_Cancel });
                inputForm.AcceptButton = okButton;
                inputForm.CancelButton = SettingsForm_Button_Cancel;

                try
                {
                    Core_Themes.ApplyTheme(inputForm);
                }
                catch
                {
                }

                if (inputForm.ShowDialog(this) == DialogResult.OK)
                {
                    var newShortcut = shortcutBox.Text.Trim();
                    SettingsForm_DataGridView_Shortcuts.Rows[e.RowIndex].Cells[1].Value = newShortcut;
                    if (!string.IsNullOrEmpty(actionName))
                        Helper_UI_Shortcuts.ApplyShortcutFromDictionary(actionName, newKeys);
                    _hasChanges = true;
                    UpdateStatus($"Shortcut updated: {actionName}");
                }
            }
        }
    }

    private bool IsShortcutConflict(string? actionName, Keys newKeys)
    {
        if (string.IsNullOrEmpty(actionName) || newKeys == Keys.None)
            return false;

        var group = GetShortcutGroup(actionName);

        for (var i = 0; i < SettingsForm_DataGridView_Shortcuts.Rows.Count; i++)
        {
            var otherAction = SettingsForm_DataGridView_Shortcuts.Rows[i].Cells[0].Value?.ToString();
            if (otherAction == actionName) continue;
            if (GetShortcutGroup(otherAction) != group) continue;

            var shortcutString = SettingsForm_DataGridView_Shortcuts.Rows[i].Cells[1].Value?.ToString() ?? "";
            var otherKeys = Helper_UI_Shortcuts.FromShortcutString(shortcutString);
            if (otherKeys == newKeys)
                return true;
        }

        return false;
    }

    private string GetShortcutGroup(string? actionName)
    {
        if (string.IsNullOrEmpty(actionName)) return "";
        if (actionName.StartsWith("Inventory")) return "Inventory";
        if (actionName.StartsWith("Advanced Inventory MultiLoc")) return "AdvancedInventoryMultiLoc";
        if (actionName.StartsWith("Advanced Inventory Import")) return "AdvancedInventoryImport";
        if (actionName.StartsWith("Advanced Inventory")) return "AdvancedInventory";
        if (actionName.StartsWith("Remove")) return "Remove";
        if (actionName.StartsWith("Transfer")) return "Transfer";
        return "";
    }

    private void LoadAboutInfo()
    {
        try
        {
            appNameLabel.Text = "MTM WIP Application";
            SettingsForm_Label_Version.Text = $"Version: {Model_AppVariables.Version ?? "Unknown"}";
        }
        catch (Exception ex)
        {
            UpdateStatus($"Error loading about info: {ex.Message}");
        }
    }

    private async void CategoryTreeView_AfterSelect(object sender, TreeViewEventArgs e)
    {
        if (e.Node == null || string.IsNullOrEmpty(e.Node.Name))
            return;

        var selected = e.Node.Name;

        if (e.Node.Nodes.Count > 0)
            return;

        ShowLoadingProgress($"Loading {selected} settings...");
        UpdateLoadingProgress(0, $"Loading {selected} settings...");

        await LoadPanelAsync(selected);

        UpdateLoadingProgress(100, $"{selected} loaded");
        await Task.Delay(300);
        HideLoadingProgress();
        ShowPanel(selected);

        if (_settingsPanels.TryGetValue(selected, out var panel) && panel.Controls.Count > 0)
        {
            var control = panel.Controls[0];
            var reloadMethod = control.GetType().GetMethod("ReloadComboBoxDataAsync");
            if (reloadMethod != null)
            {
                var task = reloadMethod.Invoke(control, null) as Task;
                if (task != null) await task;
            }
        }
    }

    private async Task LoadPanelAsync(string panelName)
    {
        switch (panelName)
        {
            case "Database":
                UpdateLoadingProgress(20, "Loading database settings...");
                await LoadDatabaseSettings();
                UpdateLoadingProgress(80, "Database settings loaded");
                break;
            case "Theme":
                UpdateLoadingProgress(20, "Loading theme settings...");
                await LoadThemeSettings();
                UpdateLoadingProgress(80, "Theme settings loaded");
                break;
            case "Shortcuts":
                UpdateLoadingProgress(20, "Loading shortcuts...");
                await LoadShortcuts();
                UpdateLoadingProgress(80, "Shortcuts loaded");
                break;
            case "About":
                UpdateLoadingProgress(20, "Loading about info...");
                LoadAboutInfo();
                UpdateLoadingProgress(80, "About info loaded");
                break;
            case "Add User":
                UpdateLoadingProgress(50, "Loading Add User...");
                await Task.Delay(200);
                UpdateLoadingProgress(80, "Add User loaded");
                break;
            case "Edit User":
            case "Delete User":
                UpdateLoadingProgress(50, $"Loading {panelName}...");
                await Task.Delay(200);
                UpdateLoadingProgress(80, $"{panelName} loaded");
                break;
            default:
                UpdateLoadingProgress(50, $"Loading {panelName}...");
                await Task.Delay(200);
                UpdateLoadingProgress(80, $"{panelName} loaded");
                break;
        }
    }

    private void ShowPanel(string panelName)
    {
        foreach (var panel in _settingsPanels.Values) panel.Visible = false;

        if (_settingsPanels.ContainsKey(panelName)) _settingsPanels[panelName].Visible = true;
    }

    private void UpdateStatus(string message)
    {
        SettingsForm_Label_Status.Text = message;
    }

    #region Event Handlers

    private async void saveButton_Click(object sender, EventArgs e)
    {
        try
        {
            ShowLoadingProgress("Saving settings...");
            UpdateLoadingProgress(10, "Saving settings...");

            UpdateLoadingProgress(25, "Saving database settings...");
            await SaveDatabaseSettings();

            UpdateLoadingProgress(50, "Saving application settings...");
            await SaveSettingsJson();

            UpdateLoadingProgress(75, "Saving shortcuts...");
            await SaveShortcutsJson();

            UpdateLoadingProgress(90, "Applying theme changes...");
            _hasChanges = false;
            UpdateStatus("Settings saved successfully");

            if (_originalThemeName != SettingsForm_ComboBox_Theme.SelectedItem?.ToString())
            {
                Model_AppVariables.ThemeName = SettingsForm_ComboBox_Theme.SelectedItem?.ToString();
                foreach (Form openForm in Application.OpenForms) Core_Themes.ApplyTheme(openForm);
            }

            UpdateLoadingProgress(100, "Settings saved successfully");

            await Task.Delay(500);
            HideLoadingProgress();

            MessageBox.Show(@"Settings saved successfully!", @"Settings", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            HideLoadingProgress();
            UpdateStatus($"Error saving settings: {ex.Message}");
            MessageBox.Show($@"Error saving settings: {ex.Message}", @"Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void cancelButton_Click(object sender, EventArgs e)
    {
        if (_hasChanges)
        {
            var result = MessageBox.Show(@"You have unsaved changes. Are you sure you want to cancel?",
                @"Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.No)
                return;
        }

        if (_originalThemeName != Model_AppVariables.ThemeName)
        {
            Model_AppVariables.ThemeName = _originalThemeName;
            Core_Themes.ApplyTheme(this);
        }

        DialogResult = DialogResult.Cancel;
        Close();
    }

    private async Task SaveDatabaseSettings()
    {
        try
        {
            var user = Model_AppVariables.User;

            await Dao_User.SetWipServerAddressAsync(user, SettingsForm_TextBox_Server.Text);
            await Dao_User.SetDatabaseAsync(user, SettingsForm_TextBox_Database.Text);
            await Dao_User.SetWipServerPortAsync(user, SettingsForm_TextBox_Port.Text);
            await Dao_User.SetVisualUserNameAsync(user, SettingsForm_TextBox_Username.Text);
            await Dao_User.SetVisualPasswordAsync(user, SettingsForm_TextBox_Password.Text);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to save database settings: {ex.Message}");
        }
    }

    private async Task SaveSettingsJson()
    {
        try
        {
            var user = Model_AppVariables.User;
            var themeName = SettingsForm_ComboBox_Theme.SelectedItem?.ToString();

            var themeObj = new
            {
                Theme_Name = themeName,
                Theme_FontSize = 9
            };

            var themeJson = JsonSerializer.Serialize(themeObj);

            await Dao_User.SetSettingsJsonAsync(user, themeJson);

            UpdateStatus("Theme settings saved successfully");
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to save theme settings: {ex.Message}");
        }
    }

    private async Task SaveShortcutsJson()
    {
        try
        {
            if (SettingsForm_DataGridView_Shortcuts.IsCurrentCellInEditMode)
                SettingsForm_DataGridView_Shortcuts.EndEdit();

            var user = Core_WipAppVariables.User;
            var shortcuts = new Dictionary<string, string>();

            for (var i = 0; i < SettingsForm_DataGridView_Shortcuts.Rows.Count; i++)
            {
                var row = SettingsForm_DataGridView_Shortcuts.Rows[i];
                var actionName = row.Cells[0].Value?.ToString();
                var shortcutString = row.Cells[1].Value?.ToString() ?? "";

                if (!string.IsNullOrEmpty(actionName))
                {
                    shortcuts[actionName] = shortcutString;
                    Helper_UI_Shortcuts.ApplyShortcutFromDictionary(actionName,
                        Helper_UI_Shortcuts.FromShortcutString(shortcutString));
                }
            }

            var json = JsonSerializer.Serialize(new { Shortcuts = shortcuts });

            await Dao_User.SetShortcutsJsonAsync(user, json);

            UpdateStatus("Shortcuts saved successfully");
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to save shortcuts: {ex.Message}");
        }
    }

    private void resetDefaultsButton_Click(object? sender, EventArgs e)
    {
        var result = MessageBox.Show(
            @"Are you sure you want to reset all settings, theme, and shortcuts to their default values?",
            @"Reset to Defaults",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (result != DialogResult.Yes)
            return;

        try
        {
            SettingsForm_TextBox_Server.Text = "172.16.1.104";
            SettingsForm_TextBox_Port.Text = "3306";
            SettingsForm_TextBox_Database.Text = "mtm_wip_application";
            SettingsForm_TextBox_Username.Text = "";
            SettingsForm_TextBox_Password.Text = "";

            var defaultTheme = Core_Themes.Core_AppThemes.GetThemeNames().FirstOrDefault() ?? "";
            if (!string.IsNullOrEmpty(defaultTheme))
                SettingsForm_ComboBox_Theme.SelectedItem = defaultTheme;

            var shortcutDict = Helper_UI_Shortcuts.GetShortcutDictionary();
            SettingsForm_DataGridView_Shortcuts.Rows.Clear();
            foreach (var kvp in shortcutDict)
            {
                var action = kvp.Key;
                var defaultKeys = kvp.Value;
                var shortcutValue = Helper_UI_Shortcuts.ToShortcutString(defaultKeys);

                Helper_UI_Shortcuts.ApplyShortcutFromDictionary(action, defaultKeys);
                SettingsForm_DataGridView_Shortcuts.Rows.Add(action, shortcutValue);
            }

            _hasChanges = true;
            UpdateStatus("All settings reset to defaults. Click Save to apply.");
        }
        catch (Exception ex)
        {
            UpdateStatus($"Error resetting to defaults: {ex.Message}");
        }
    }

    #endregion

    #region Progress Control

    private void InitializeProgressControl()
    {
        try
        {
            _loadingProgress = new ProgressBarUserControl
            {
                Size = new Size(350, 120),
                Visible = false,
                Anchor = AnchorStyles.None,
                StatusText = "Loading settings..."
            };

            _loadingProgress.Location = new Point(
                (Width - _loadingProgress.Width) / 2,
                (Height - _loadingProgress.Height) / 2
            );

            Controls.Add(_loadingProgress);
            _loadingProgress.BringToFront();
        }
        catch (Exception ex)
        {
            UpdateStatus($"Warning: Could not initialize progress control - {ex.Message}");
        }
    }
    {
        try
        {
            if (_loadingProgress != null)
            {
                _loadingProgress.Location = new Point(
                    (Width - _loadingProgress.Width) / 2,
                    (Height - _loadingProgress.Height) / 2
                );

                _loadingProgress.StatusText = status;
                _loadingProgress.ShowProgress();
                _loadingProgress.UpdateProgress(0, status);
            }
        }
        catch (Exception ex)
        {
            UpdateStatus($"Warning: Progress display error - {ex.Message}");
        }
    }

    private void UpdateLoadingProgress(int progress, string status)
    {
        try
        {
            _loadingProgress?.UpdateProgress(progress, status);
        }
        catch (Exception ex)
        {
            UpdateStatus($"Warning: Progress update error - {ex.Message}");
        }
    }

    private void HideLoadingProgress()
    {
        try
        {
            _loadingProgress?.HideProgress();
        }
        catch (Exception ex)
        {
            UpdateStatus($"Warning: Progress hide error - {ex.Message}");
        }
    }

    #endregion

    
    #endregion
}