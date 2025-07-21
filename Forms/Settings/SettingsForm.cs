using System.Reflection;
using System.Text.Json;
using MTM_Inventory_Application.Controls.SettingsForm;
using MTM_Inventory_Application.Controls.Shared;
using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Models;
using MethodInvoker = System.Windows.Forms.MethodInvoker;

namespace MTM_Inventory_Application.Forms.Settings
{
    public partial class SettingsForm : Form
    {
        #region Fields

        private bool _hasChanges = false;
        private readonly Dictionary<string, Panel> _settingsPanels;
        private string? _originalThemeName;
        private Control_ProgressBarUserControl _loadingControlProgress = null!;

        #endregion

        #region Constructors

        public SettingsForm()
        {
            InitializeComponent();

            // Apply comprehensive DPI scaling and runtime layout adjustments
            AutoScaleMode = AutoScaleMode.Dpi;
            Core_Themes.ApplyDpiScaling(this);
            Core_Themes.ApplyRuntimeLayoutAdjustments(this);

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

            SettingsForm_DataGridView_Shortcuts.CellBeginEdit += ShortcutsDataGridView_CellBeginEdit;

            InitializeUserControls();

            InitializeForm();
            SettingsForm_Button_SwitchTheme.Click += SettingsForm_Button_SwitchTheme_Click;
        }

        #endregion

        #region Methods

        private void InitializeCategoryTreeView()
        {
            SettingsForm_TreeView_Category.Nodes.Clear();

            TreeNode databaseNode = SettingsForm_TreeView_Category.Nodes.Add("Database", "Database");

            TreeNode usersNode = SettingsForm_TreeView_Category.Nodes.Add("Users", "Users");
            usersNode.Nodes.Add("Add User", "Add User");
            usersNode.Nodes.Add("Edit User", "Edit User");
            usersNode.Nodes.Add("Delete User", "Delete User");

            TreeNode partNumbersNode = SettingsForm_TreeView_Category.Nodes.Add("Part Numbers", "Part Numbers");
            partNumbersNode.Nodes.Add("Add Part Number", "Add Part Number");
            partNumbersNode.Nodes.Add("Edit Part Number", "Edit Part Number");
            partNumbersNode.Nodes.Add("Remove Part Number", "Remove Part Number");

            TreeNode operationsNode = SettingsForm_TreeView_Category.Nodes.Add("Operations", "Operations");
            operationsNode.Nodes.Add("Add Operation", "Add Operation");
            operationsNode.Nodes.Add("Edit Operation", "Edit Operation");
            operationsNode.Nodes.Add("Remove Operation", "Remove Operation");

            TreeNode locationsNode = SettingsForm_TreeView_Category.Nodes.Add("Locations", "Locations");
            locationsNode.Nodes.Add("Add Location", "Add Location");
            locationsNode.Nodes.Add("Edit Location", "Edit Location");
            locationsNode.Nodes.Add("Remove Location", "Remove Location");

            TreeNode itemTypesNode = SettingsForm_TreeView_Category.Nodes.Add("ItemTypes", "ItemTypes");
            itemTypesNode.Nodes.Add("Add ItemType", "Add ItemType");
            itemTypesNode.Nodes.Add("Edit ItemType", "Edit ItemType");
            itemTypesNode.Nodes.Add("Remove ItemType", "Remove ItemType");

            TreeNode themeNode = SettingsForm_TreeView_Category.Nodes.Add("Theme", "Theme");
            TreeNode shortcutsNode = SettingsForm_TreeView_Category.Nodes.Add("Shortcuts", "Shortcuts");
            TreeNode aboutNode = SettingsForm_TreeView_Category.Nodes.Add("About", "About");

            SettingsForm_TreeView_Category.CollapseAll();

            SettingsForm_TreeView_Category.SelectedNode = databaseNode;
        }

        private void InitializeUserControls()
        {
            Panel SettingsForm_Panel_AddUser = _settingsPanels["Add User"];
            Control_Add_User controlAddUser = new();
            controlAddUser.Dock = DockStyle.Fill;
            controlAddUser.UserAdded += (s, e) => { UpdateStatus("User added successfully."); };
            SettingsForm_Panel_AddUser.Controls.Add(controlAddUser);

            Panel SettingsForm_Panel_EditUser = _settingsPanels["Edit User"];
            Control_Edit_User controlEditUser = new();
            controlEditUser.Dock = DockStyle.Fill;
            controlEditUser.UserEdited += (s, e) => { UpdateStatus("User updated successfully."); };
            SettingsForm_Panel_EditUser.Controls.Add(controlEditUser);

            Panel SettingsForm_Panel_DeleteUser = _settingsPanels["Delete User"];
            Control_Remove_User controlRemoveUser = new();
            controlRemoveUser.Dock = DockStyle.Fill;
            controlRemoveUser.UserRemoved += (s, e) => { UpdateStatus("User removed successfully."); };
            SettingsForm_Panel_DeleteUser.Controls.Add(controlRemoveUser);

            Control_Add_PartID controlAddPartId = new();
            controlAddPartId.Dock = DockStyle.Fill;
            controlAddPartId.PartAdded += (s, e) => { UpdateStatus("Part added successfully - lists refreshed"); };
            SettingsForm_Panel_AddPart.Controls.Add(controlAddPartId);

            Control_Edit_PartID controlEditPartId = new();
            controlEditPartId.Dock = DockStyle.Fill;
            controlEditPartId.PartUpdated += (s, e) => { UpdateStatus("Part updated successfully - lists refreshed"); };
            SettingsForm_Panel_EditPart.Controls.Add(controlEditPartId);

            Control_Remove_PartID controlRemovePartId = new();
            controlRemovePartId.Dock = DockStyle.Fill;
            controlRemovePartId.PartRemoved += (s, e) =>
            {
                UpdateStatus("Part removed successfully - lists refreshed");
            };
            SettingsForm_Panel_RemovePart.Controls.Add(controlRemovePartId);

            Control_Add_Operation controlAddOperation = new();
            controlAddOperation.Dock = DockStyle.Fill;
            controlAddOperation.OperationAdded += (s, e) =>
            {
                UpdateStatus("Operation added successfully - lists refreshed");
            };
            SettingsForm_Panel_AddOperation.Controls.Add(controlAddOperation);

            Control_Edit_Operation controlEditOperation = new();
            controlEditOperation.Dock = DockStyle.Fill;
            controlEditOperation.OperationUpdated += (s, e) =>
            {
                UpdateStatus("Operation updated successfully - lists refreshed");
            };
            SettingsForm_Panel_EditOperation.Controls.Add(controlEditOperation);

            Control_Remove_Operation controlRemoveOperation = new();
            controlRemoveOperation.Dock = DockStyle.Fill;
            controlRemoveOperation.OperationRemoved += (s, e) =>
            {
                UpdateStatus("Operation removed successfully - lists refreshed");
            };
            SettingsForm_Panel_RemoveOperation.Controls.Add(controlRemoveOperation);

            Control_Add_Location controlAddLocation = new();
            controlAddLocation.Dock = DockStyle.Fill;
            controlAddLocation.LocationAdded += (s, e) =>
            {
                UpdateStatus("Location added successfully - lists refreshed");
            };
            SettingsForm_Panel_AddLocation.Controls.Add(controlAddLocation);

            Control_Edit_Location controlEditLocation = new();
            controlEditLocation.Dock = DockStyle.Fill;
            controlEditLocation.LocationUpdated += (s, e) =>
            {
                UpdateStatus("Location updated successfully - lists refreshed");
            };
            SettingsForm_Panel_EditLocation.Controls.Add(controlEditLocation);

            Control_Remove_Location controlRemoveLocation = new();
            controlRemoveLocation.Dock = DockStyle.Fill;
            controlRemoveLocation.LocationRemoved += (s, e) =>
            {
                UpdateStatus("Location removed successfully - lists refreshed");
            };
            SettingsForm_Panel_RemoveLocation.Controls.Add(controlRemoveLocation);

            Control_Add_ItemType controlAddItemType = new();
            controlAddItemType.Dock = DockStyle.Fill;
            controlAddItemType.ItemTypeAdded += (s, e) =>
            {
                UpdateStatus("ItemType added successfully - lists refreshed");
            };
            SettingsForm_Panel_AddItemType.Controls.Add(controlAddItemType);

            Control_Edit_ItemType controlEditItemType = new();
            controlEditItemType.Dock = DockStyle.Fill;
            controlEditItemType.ItemTypeUpdated += (s, e) =>
            {
                UpdateStatus("ItemType updated successfully - lists refreshed");
            };
            SettingsForm_Panel_EditItemType.Controls.Add(controlEditItemType);

            Control_Remove_ItemType controlRemoveItemType = new();
            controlRemoveItemType.Dock = DockStyle.Fill;
            controlRemoveItemType.ItemTypeRemoved += (s, e) =>
            {
                UpdateStatus("ItemType removed successfully - lists refreshed");
            };
            SettingsForm_Panel_RemoveItemType.Controls.Add(controlRemoveItemType);
        }

        private void InitializeForm()
        {
            Text = "Settings - MTM WIP Application";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;

            foreach (Panel panel in _settingsPanels.Values)
            {
                SettingsForm_Panel_Settings.Controls.Add(panel);
                panel.Visible = false;
            }

            InitializeCategoryTreeView();
            ShowPanel("Database");

            LoadSettings();

            ApplyTheme();
            ApplyPrivileges();
        }

        private void ApplyTheme()
        {
            // Ensure theming is performed on the UI thread
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)ApplyTheme);
                return;
            }

            bool comboBoxWasEnabled = SettingsForm_ComboBox_Theme.Enabled;
            SettingsForm_ComboBox_Theme.Enabled = false;

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
            finally
            {
                // Re-enable UI elements
                SettingsForm_ComboBox_Theme.Enabled = comboBoxWasEnabled;
            }
        }

        private void ApplyPrivileges()
        {
            bool isAdmin = Model_AppVariables.UserTypeAdmin;
            bool isNormal = Model_AppVariables.UserTypeNormal;
            bool isReadOnly = Model_AppVariables.UserTypeReadOnly;

            // Rebuild tree to ensure all nodes are present before hiding
            InitializeCategoryTreeView();

            // Helper to find a node by path (root or child)
            TreeNode? FindNodeByPath(params string[] path)
            {
                TreeNodeCollection nodes = SettingsForm_TreeView_Category.Nodes;
                TreeNode? node = null;
                foreach (string name in path)
                {
                    node = (node == null ? nodes.Cast<TreeNode>() : node.Nodes.Cast<TreeNode>()).FirstOrDefault(n =>
                        n.Name == name);
                    if (node == null)
                    {
                        break;
                    }
                }

                return node;
            }

            // Helper to hide a node by path
            void HideNode(params string[] path)
            {
                TreeNode? node = FindNodeByPath(path);
                if (node != null)
                {
                    if (node.Parent == null)
                    {
                        SettingsForm_TreeView_Category.Nodes.Remove(node);
                    }
                    else
                    {
                        node.Parent.Nodes.Remove(node);
                    }
                }
            }

            if (isAdmin)
            {
                // All nodes shown by default
                return;
            }

            if (isNormal)
            {
                SettingsForm_TabControl_Database.Visible = false;
                HideNode("Database");
                HideNode("Users");
                HideNode("Part Numbers", "Edit Part Number");
                HideNode("Part Numbers", "Remove Part Number");
                HideNode("Operations", "Edit Operation");
                HideNode("Operations", "Remove Operation");
                HideNode("Locations", "Edit Location");
                HideNode("Locations", "Remove Location");
                HideNode("ItemTypes", "Edit ItemType");
                HideNode("ItemTypes", "Remove ItemType");
                HideNode("Users", "Edit User");
                HideNode("Users", "Delete User");
            }

            if (isReadOnly)
            {
                HideNode("Database");
                HideNode("Users");
                HideNode("Part Numbers");
                HideNode("Operations");
                HideNode("Locations");
                HideNode("ItemTypes");
                HideNode("Shortcuts");
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
                string user = Model_AppVariables.User;

                SettingsForm_TextBox_Server.Text =
                    await Dao_User.GetWipServerAddressAsync(user) ?? "172.16.1.104"; //172.16.1.104
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
                string[] themeNames = Core_Themes.Core_AppThemes.GetThemeNames().ToArray();
                SettingsForm_ComboBox_Theme.Items.AddRange(themeNames);

                string user = Model_AppVariables.User;
                string? themeName = await Dao_User.GetThemeNameAsync(user);
                int fontSize = await Dao_User.GetThemeFontSizeAsync(user) ?? 9;

                if (!string.IsNullOrEmpty(themeName) && SettingsForm_ComboBox_Theme.Items.Contains(themeName))
                {
                    SettingsForm_ComboBox_Theme.SelectedItem = themeName;
                }
                else if (SettingsForm_ComboBox_Theme.Items.Count > 0)
                {
                    SettingsForm_ComboBox_Theme.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error loading theme settings: {ex.Message}");
                if (SettingsForm_ComboBox_Theme.Items.Count > 0)
                {
                    SettingsForm_ComboBox_Theme.SelectedIndex = 0;
                }
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

                string user = Core_WipAppVariables.User;
                string shortcutsJson = await Dao_User.GetShortcutsJsonAsync(user);

                Dictionary<string, Keys> shortcutDict = Helper_UI_Shortcuts.GetShortcutDictionary();

                Dictionary<string, string> userShortcuts = new();
                if (!string.IsNullOrWhiteSpace(shortcutsJson))
                {
                    try
                    {
                        using JsonDocument doc = JsonDocument.Parse(shortcutsJson);
                        if (doc.RootElement.ValueKind == JsonValueKind.Object &&
                            doc.RootElement.TryGetProperty("Shortcuts", out JsonElement shortcutsElement) &&
                            shortcutsElement.ValueKind == JsonValueKind.Object)
                        {
                            foreach (JsonProperty prop in shortcutsElement.EnumerateObject())
                            {
                                userShortcuts[prop.Name] = prop.Value.GetString() ?? "";
                            }
                        }
                    }
                    catch (JsonException)
                    {
                        UpdateStatus("Warning: Shortcuts JSON is malformed. Using defaults.");
                    }
                }

                foreach (KeyValuePair<string, Keys> kvp in shortcutDict)
                {
                    string action = kvp.Key;
                    Keys defaultKeys = kvp.Value;
                    string shortcutValue =
                        userShortcuts.TryGetValue(action, out string? val) && !string.IsNullOrWhiteSpace(val)
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
                string shortcutString = e.FormattedValue?.ToString() ?? "";

                if (string.IsNullOrWhiteSpace(shortcutString))
                {
                    return;
                }

                try
                {
                    Keys keys = Helper_UI_Shortcuts.FromShortcutString(shortcutString);
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
                string? actionName = SettingsForm_DataGridView_Shortcuts.Rows[e.RowIndex].Cells[0].Value?.ToString();
                string shortcutString =
                    SettingsForm_DataGridView_Shortcuts.Rows[e.RowIndex].Cells[1].Value?.ToString() ?? "";

                if (!string.IsNullOrEmpty(actionName))
                {
                    try
                    {
                        Keys keys = Helper_UI_Shortcuts.FromShortcutString(shortcutString);
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
        }

        private void ShortcutsDataGridView_CellBeginEdit(object? sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == 1 && e.RowIndex >= 0)
            {
                e.Cancel = true;

                string? actionName = SettingsForm_DataGridView_Shortcuts.Rows[e.RowIndex].Cells[0].Value?.ToString();
                string currentShortcut =
                    SettingsForm_DataGridView_Shortcuts.Rows[e.RowIndex].Cells[1].Value?.ToString() ?? "";

                using (Form inputForm = new())
                {
                    inputForm.Text = $"Set Shortcut for '{actionName}'";
                    inputForm.Size = new Size(400, 180);
                    inputForm.StartPosition = FormStartPosition.CenterParent;
                    inputForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                    inputForm.MaximizeBox = false;
                    inputForm.MinimizeBox = false;

                    Label label = new()
                    {
                        Text = "Press the new shortcut key combination:",
                        Location = new Point(10, 20),
                        Size = new Size(360, 20)
                    };
                    TextBox shortcutBox = new()
                    {
                        Location = new Point(10, 45),
                        Size = new Size(360, 20),
                        ReadOnly = true,
                        TabStop = false,
                        BackColor = SystemColors.Control,
                        ForeColor = SystemColors.GrayText
                    };
                    shortcutBox.Text = currentShortcut;

                    Label errorLabel = new()
                    {
                        Text = "",
                        ForeColor = Color.Red,
                        Location = new Point(10, 70),
                        Size = new Size(360, 30),
                        Visible = false
                    };

                    Keys newKeys = Helper_UI_Shortcuts.FromShortcutString(currentShortcut);

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
                        {
                            return;
                        }

                        newKeys = ke.KeyData;
                        shortcutBox.Text = Helper_UI_Shortcuts.ToShortcutString(newKeys);

                        if (newKeys == Keys.None)
                        {
                            errorLabel.Text = "Shortcut cannot be empty.";
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

                    Button okButton = new() { Text = "OK", Location = new Point(215, 110), Size = new Size(75, 23) };
                    Button SettingsForm_Button_Cancel = new()
                    {
                        Text = "Cancel",
                        Location = new Point(295, 110),
                        Size = new Size(75, 23)
                    };

                    okButton.Click += (s, args) =>
                    {
                        if (newKeys == Keys.None)
                        {
                            errorLabel.Text = "Shortcut cannot be empty.";
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

                    inputForm.Controls.AddRange(new Control[]
                    {
                label, shortcutBox, errorLabel, okButton, SettingsForm_Button_Cancel
                    });
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
                        string newShortcut = shortcutBox.Text.Trim();
                        SettingsForm_DataGridView_Shortcuts.Rows[e.RowIndex].Cells[1].Value = newShortcut;
                        if (!string.IsNullOrEmpty(actionName))
                        {
                            Helper_UI_Shortcuts.ApplyShortcutFromDictionary(actionName, newKeys);
                        }

                        _hasChanges = true;
                        UpdateStatus($"Shortcut updated: {actionName}");
                    }
                }
            }
        }

        private bool IsShortcutConflict(string? actionName, Keys newKeys)
        {
            if (string.IsNullOrEmpty(actionName) || newKeys == Keys.None)
            {
                return false;
            }

            string group = GetShortcutGroup(actionName);

            for (int i = 0; i < SettingsForm_DataGridView_Shortcuts.Rows.Count; i++)
            {
                string? otherAction = SettingsForm_DataGridView_Shortcuts.Rows[i].Cells[0].Value?.ToString();
                if (otherAction == actionName)
                {
                    continue;
                }

                if (GetShortcutGroup(otherAction) != group)
                {
                    continue;
                }

                string shortcutString = SettingsForm_DataGridView_Shortcuts.Rows[i].Cells[1].Value?.ToString() ?? "";
                Keys otherKeys = Helper_UI_Shortcuts.FromShortcutString(shortcutString);
                if (otherKeys == newKeys)
                {
                    return true;
                }
            }

            return false;
        }

        private string GetShortcutGroup(string? actionName)
        {
            if (string.IsNullOrEmpty(actionName))
            {
                return "";
            }

            if (actionName.StartsWith("Inventory"))
            {
                return "Inventory";
            }

            if (actionName.StartsWith("Advanced Inventory MultiLoc"))
            {
                return "AdvancedInventoryMultiLoc";
            }

            if (actionName.StartsWith("Advanced Inventory Import"))
            {
                return "AdvancedInventoryImport";
            }

            if (actionName.StartsWith("Advanced Inventory"))
            {
                return "AdvancedInventory";
            }

            if (actionName.StartsWith("Remove"))
            {
                return "Remove";
            }

            if (actionName.StartsWith("Transfer"))
            {
                return "Transfer";
            }

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
            {
                return;
            }

            string selected = e.Node.Name;

            if (e.Node.Nodes.Count > 0)
            {
                return;
            }

            ShowLoadingProgress($"Loading {selected} settings...");
            UpdateLoadingProgress(0, $"Loading {selected} settings...");

            await LoadPanelAsync(selected);

            UpdateLoadingProgress(100, $"{selected} loaded");
            await Task.Delay(300);
            HideLoadingProgress();
            ShowPanel(selected);

            if (_settingsPanels.TryGetValue(selected, out Panel? panel) && panel.Controls.Count > 0)
            {
                Control control = panel.Controls[0];
                MethodInfo? reloadMethod = control.GetType().GetMethod("ReloadComboBoxDataAsync");
                if (reloadMethod != null)
                {
                    Task? task = reloadMethod.Invoke(control, null) as Task;
                    if (task != null)
                    {
                        await task;
                    }
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
            foreach (Panel panel in _settingsPanels.Values)
            {
                panel.Visible = false;
            }

            if (_settingsPanels.ContainsKey(panelName))
            {
                _settingsPanels[panelName].Visible = true;
            }
        }

        private void UpdateStatus(string message) => SettingsForm_Label_Status.Text = message;

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

                if (_originalThemeName != SettingsForm_ComboBox_Theme.Text)
                {
                    Model_AppVariables.ThemeName = SettingsForm_ComboBox_Theme.Text;
                    foreach (Form openForm in Application.OpenForms)
                    {
                        Core_Themes.ApplyTheme(openForm);
                    }
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
                DialogResult result = MessageBox.Show(@"You have unsaved changes. Are you sure you want to cancel?",
                    @"Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    return;
                }
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
                string user = Model_AppVariables.User;

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
                string user = Model_AppVariables.User;
                string? themeName = SettingsForm_ComboBox_Theme.Text;

                var themeObj = new { Theme_Name = themeName, Theme_FontSize = 9 };

                string themeJson = JsonSerializer.Serialize(themeObj);

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
                {
                    SettingsForm_DataGridView_Shortcuts.EndEdit();
                }

                string user = Core_WipAppVariables.User;
                Dictionary<string, string> shortcuts = new();

                for (int i = 0; i < SettingsForm_DataGridView_Shortcuts.Rows.Count; i++)
                {
                    DataGridViewRow row = SettingsForm_DataGridView_Shortcuts.Rows[i];
                    string? actionName = row.Cells[0].Value?.ToString();
                    string shortcutString = row.Cells[1].Value?.ToString() ?? "";

                    if (!string.IsNullOrEmpty(actionName))
                    {
                        shortcuts[actionName] = shortcutString;
                        Helper_UI_Shortcuts.ApplyShortcutFromDictionary(actionName,
                            Helper_UI_Shortcuts.FromShortcutString(shortcutString));
                    }
                }

                string json = JsonSerializer.Serialize(new { Shortcuts = shortcuts });

                await Dao_User.SetShortcutsJsonAsync(user, json);

                UpdateStatus("Shortcuts saved successfully");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to save shortcuts: {ex.Message}");
            }
        }

        private async void SettingsForm_Button_SwitchTheme_Click(object? sender, EventArgs e)
        {
            if (SettingsForm_ComboBox_Theme.SelectedItem is not string themeName ||
                string.IsNullOrWhiteSpace(themeName))
            {
                return;
            }

            SettingsForm_Button_SwitchTheme.Enabled = false;
            ShowLoadingProgress($"Switching to theme '{themeName}'...");

            try
            {
                string? originalThemeName = Model_AppVariables.ThemeName;
                Model_AppVariables.ThemeName = themeName;
                await Task.Delay(100); // Optional: allow UI update
                Core_Themes.ApplyTheme(this);
                Model_AppVariables.ThemeName = originalThemeName;
                UpdateStatus($"Theme switched to '{themeName}'");
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error switching theme: {ex.Message}");
            }
            finally
            {
                await Task.Delay(300);
                HideLoadingProgress();
                SettingsForm_Button_SwitchTheme.Enabled = true;
            }
        }

        private void resetDefaultsButton_Click(object? sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                @"Are you sure you want to reset all settings, theme, and shortcuts to their default values?",
                @"Reset to Defaults",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result != DialogResult.Yes)
            {
                return;
            }

            try
            {
                SettingsForm_TextBox_Server.Text = "172.16.1.104"; //172.16.1.104
                SettingsForm_TextBox_Port.Text = "3306";
                SettingsForm_TextBox_Database.Text = "mtm_wip_application";
                SettingsForm_TextBox_Username.Text = "";
                SettingsForm_TextBox_Password.Text = "";

                string defaultTheme = Core_Themes.Core_AppThemes.GetThemeNames().FirstOrDefault() ?? "";
                if (!string.IsNullOrEmpty(defaultTheme))
                {
                    SettingsForm_ComboBox_Theme.SelectedItem = defaultTheme;
                }

                Dictionary<string, Keys> shortcutDict = Helper_UI_Shortcuts.GetShortcutDictionary();
                SettingsForm_DataGridView_Shortcuts.Rows.Clear();
                foreach (KeyValuePair<string, Keys> kvp in shortcutDict)
                {
                    string action = kvp.Key;
                    Keys defaultKeys = kvp.Value;
                    string shortcutValue = Helper_UI_Shortcuts.ToShortcutString(defaultKeys);

                    Helper_UI_Shortcuts.ApplyShortcutFromDictionary(action, defaultKeys);
                    SettingsForm_DataGridView_Shortcuts.Rows.Add(action, shortcutValue);
                }


                UpdateStatus("All settings reset to defaults. Click Save to apply.");
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error resetting to defaults: {ex.Message}");
            }
        }

        private static void CloseAndResetIfChanged()
        {
            DialogResult result = MessageBox.Show(
                "You have changes that require a restart. Exit and reset the application?",
                "Unsaved Changes",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                // Reset the application (restart)
                Application.Restart();
                Application.ExitThread();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                CloseAndResetIfChanged();
            }
        }

        #endregion

        #region Progress Control

        private void InitializeProgressControl()
        {
            try
            {
                _loadingControlProgress = new Control_ProgressBarUserControl
                {
                    Size = new Size(350, 120),
                    Visible = false,
                    Anchor = AnchorStyles.None,
                    StatusText = "Loading settings..."
                };

                _loadingControlProgress.Location = new Point(
                    (Width - _loadingControlProgress.Width) / 2,
                    (Height - _loadingControlProgress.Height) / 2
                );

                Controls.Add(_loadingControlProgress);
                _loadingControlProgress.BringToFront();
            }
            catch (Exception ex)
            {
                UpdateStatus($"Warning: Could not initialize progress control - {ex.Message}");
            }
        }

        private void ShowLoadingProgress(string status = "Loading...")
        {
            try
            {
                if (_loadingControlProgress != null)
                {
                    _loadingControlProgress.Location = new Point(
                        (Width - _loadingControlProgress.Width) / 2,
                        (Height - _loadingControlProgress.Height) / 2
                    );

                    _loadingControlProgress.StatusText = status;
                    _loadingControlProgress.ShowProgress();
                    _loadingControlProgress.UpdateProgress(0, status);
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
                _loadingControlProgress?.UpdateProgress(progress, status);
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
                _loadingControlProgress?.HideProgress();
            }
            catch (Exception ex)
            {
                UpdateStatus($"Warning: Progress hide error - {ex.Message}");
            }
        }

        #endregion

        #endregion
    }
}
