using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Models;
using System.Data;
using System.Text.Json;

namespace MTM_Inventory_Application.Forms.Settings;

public partial class SettingsForm : Form
{
    private bool _hasChanges = false;
    private readonly Dictionary<string, Panel> _settingsPanels;
    private string? _originalThemeName;

    public SettingsForm()
    {
        InitializeComponent();

        // Initialize settings panels dictionary
        _settingsPanels = new Dictionary<string, Panel>
        {
            ["Database"] = databasePanel,
            ["Theme"] = themePanel,
            ["Shortcuts"] = shortcutsPanel,
            ["About"] = aboutPanel
        };

        // Store the original theme name for later comparison
        _originalThemeName = Model_AppVariables.ThemeName;

        // Wire up themeComboBox event
        themeComboBox.SelectedIndexChanged += ThemeComboBox_SelectedIndexChanged;

        // Wire up shortcutsDataGridView event
        shortcutsDataGridView.CellBeginEdit += ShortcutsDataGridView_CellBeginEdit;

        // Initialize the form
        InitializeForm();
    }

    private void ThemeComboBox_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (themeComboBox.SelectedItem is string themeName && !string.IsNullOrWhiteSpace(themeName))
        {
            // Temporarily apply the selected theme to the settings window only
            var theme = Core_Themes.Core_AppThemes.GetTheme(themeName);
            // Temporarily set the theme for preview
            var originalThemeName = Model_AppVariables.ThemeName;
            Model_AppVariables.ThemeName = themeName;
            Core_Themes.ApplyTheme(this);
            Model_AppVariables.ThemeName = originalThemeName;
        }
    }

    private void InitializeForm()
    {
        // Set form properties
        Text = "Settings - MTM WIP Application";
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;

        // Add all panels to the settings panel container
        foreach (var panel in _settingsPanels.Values)
        {
            settingsPanel.Controls.Add(panel);
            panel.Visible = false;
        }

        // Set default selection
        categoryListBox.SelectedIndex = 0;
        ShowPanel("Database");

        // Load current settings
        LoadSettings();

        // Apply theme to this form
        ApplyTheme();
    }

    private void ApplyTheme()
    {
        try
        {
            Core_Themes.ApplyTheme(this);
            Core_Themes.ApplyThemeToDataGridView(shortcutsDataGridView);
            Core_Themes.SizeDataGrid(shortcutsDataGridView);
        }
        catch (Exception ex)
        {
            // If theme application fails, continue without theming
            UpdateStatus($"Warning: Could not apply theme - {ex.Message}");
        }
    }

    private async void LoadSettings()
    {
        try
        {
            UpdateStatus("Loading settings...");

            // Load database connection settings
            await LoadDatabaseSettings();

            // Load theme settings
            await LoadThemeSettings();

            // Load shortcuts
            await LoadShortcuts();

            // Load about information
            LoadAboutInfo();

            // Load database objects
            await LoadDatabaseObjects();

            UpdateStatus("Settings loaded successfully");
            _hasChanges = false;
        }
        catch (Exception ex)
        {
            UpdateStatus($"Error loading settings: {ex.Message}");
        }
    }

    private async Task LoadDatabaseSettings()
    {
        try
        {
            var user = Model_AppVariables.User;

            serverTextBox.Text = await Dao_User.GetWipServerAddressAsync(user) ?? "localhost";
            portTextBox.Text = await Dao_User.GetWipServerPortAsync(user) ?? "3306";
            databaseTextBox.Text = await Dao_User.GetDatabaseAsync(user) ?? "mtm_wip_application";
            usernameTextBox.Text = await Dao_User.GetVisualUserNameAsync(user) ?? "";
            passwordTextBox.Text = await Dao_User.GetVisualPasswordAsync(user) ?? "";
            timeoutTextBox.Text = "30";
            timeoutTextBox.Enabled = false;
            autoReconnectCheckBox.Checked = true;
            autoReconnectCheckBox.Enabled = false;
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
            // Load available themes from Core_Themes.Core_AppThemes
            themeComboBox.Items.Clear();
            var themeNames = Core_Themes.Core_AppThemes.GetThemeNames().ToArray();
            themeComboBox.Items.AddRange(themeNames);

            var user = Model_AppVariables.User;
            var themeName = await Dao_User.GetThemeNameAsync(user);
            var fontSize = await Dao_User.GetThemeFontSizeAsync(user) ?? 9;

            // Set current theme
            if (!string.IsNullOrEmpty(themeName) && themeComboBox.Items.Contains(themeName))
                themeComboBox.SelectedItem = themeName;
            else if (themeComboBox.Items.Count > 0) themeComboBox.SelectedIndex = 0; // Default to first theme

            fontSizeNumericUpDown.Value = Math.Max(8, Math.Min(20, fontSize));
        }
        catch (Exception ex)
        {
            UpdateStatus($"Error loading theme settings: {ex.Message}");
            if (themeComboBox.Items.Count > 0)
                themeComboBox.SelectedIndex = 0;
            fontSizeNumericUpDown.Value = 9;
        }
    }

    private async Task LoadShortcuts()
    {
        try
        {
            // Configure DataGridView
            shortcutsDataGridView.Columns.Clear();
            shortcutsDataGridView.Columns.Add("Action", "Action");
            shortcutsDataGridView.Columns.Add("Shortcut", "Shortcut");
            shortcutsDataGridView.Rows.Clear();

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
                    // Log or handle malformed JSON, fallback to defaults
                    UpdateStatus("Warning: Shortcuts JSON is malformed. Using defaults.");
                }

            foreach (var kvp in shortcutDict)
            {
                var action = kvp.Key;
                var defaultKeys = kvp.Value;
                var shortcutValue = userShortcuts.TryGetValue(action, out var val) && !string.IsNullOrWhiteSpace(val)
                    ? val
                    : Helper_UI_Shortcuts.ToShortcutString(defaultKeys);

                // Update in-memory shortcut for runtime
                Helper_UI_Shortcuts.ApplyShortcutFromDictionary(action,
                    Helper_UI_Shortcuts.FromShortcutString(shortcutValue));

                shortcutsDataGridView.Rows.Add(action, shortcutValue);
            }

            shortcutsDataGridView.ReadOnly = false;
            shortcutsDataGridView.AllowUserToAddRows = false;
            shortcutsDataGridView.AllowUserToDeleteRows = false;
            shortcutsDataGridView.Columns[0].ReadOnly = true;
            shortcutsDataGridView.Columns[1].ReadOnly = false;
            shortcutsDataGridView.CellValueChanged += ShortcutsDataGridView_CellValueChanged;
            shortcutsDataGridView.CellValidating += ShortcutsDataGridView_CellValidating;
        }
        catch (Exception ex)
        {
            UpdateStatus($"Error loading shortcuts: {ex.Message}");
        }
    }

    private void ShortcutsDataGridView_CellValidating(object? sender, DataGridViewCellValidatingEventArgs e)
    {
        if (e.ColumnIndex == 1) // Shortcut column
        {
            var shortcutString = e.FormattedValue?.ToString() ?? "";

            // Allow empty shortcuts
            if (string.IsNullOrWhiteSpace(shortcutString)) return;

            // Validate shortcut format
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
        if (e.ColumnIndex == 1 && e.RowIndex >= 0) // Shortcut column
        {
            var actionName = shortcutsDataGridView.Rows[e.RowIndex].Cells[0].Value?.ToString();
            var shortcutString = shortcutsDataGridView.Rows[e.RowIndex].Cells[1].Value?.ToString() ?? "";

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
            e.Cancel = true; // Prevent direct editing

            var actionName = shortcutsDataGridView.Rows[e.RowIndex].Cells[0].Value?.ToString();
            var currentShortcut = shortcutsDataGridView.Rows[e.RowIndex].Cells[1].Value?.ToString() ?? "";

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

                    // Ignore modifier-only presses
                    if (ke.KeyCode == Keys.ControlKey || ke.KeyCode == Keys.ShiftKey || ke.KeyCode == Keys.Menu)
                        return;

                    newKeys = ke.KeyData;
                    shortcutBox.Text = Helper_UI_Shortcuts.ToShortcutString(newKeys);

                    // Validation: must include at least one modifier
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
                var cancelButton = new Button
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
                cancelButton.Click += (s, args) =>
                {
                    inputForm.DialogResult = DialogResult.Cancel;
                    inputForm.Close();
                };

                inputForm.Controls.AddRange(new Control[] { label, shortcutBox, errorLabel, okButton, cancelButton });
                inputForm.AcceptButton = okButton;
                inputForm.CancelButton = cancelButton;

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
                    shortcutsDataGridView.Rows[e.RowIndex].Cells[1].Value = newShortcut;
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

        for (var i = 0; i < shortcutsDataGridView.Rows.Count; i++)
        {
            var otherAction = shortcutsDataGridView.Rows[i].Cells[0].Value?.ToString();
            if (otherAction == actionName) continue;
            if (GetShortcutGroup(otherAction) != group) continue;

            var shortcutString = shortcutsDataGridView.Rows[i].Cells[1].Value?.ToString() ?? "";
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
            versionLabel.Text = $"Version: {Model_AppVariables.Version ?? "Unknown"}";
        }
        catch (Exception ex)
        {
            UpdateStatus($"Error loading about info: {ex.Message}");
        }
    }

    private async Task LoadDatabaseObjects()
    {
        try
        {
            await LoadParts();
            await LoadOperations();
            await LoadLocations();
            await LoadUsers();
        }
        catch (Exception ex)
        {
            UpdateStatus($"Error loading database objects: {ex.Message}");
        }
    }

    private async Task LoadParts()
    {
        try
        {
            // This would need to be implemented in Dao_Part
            // For now, just set up the grid structure
            partsDataGridView.Columns.Clear();
            partsDataGridView.Columns.Add("ItemNumber", "Item Number");
            partsDataGridView.Columns.Add("Type", "Type");
            partsDataGridView.Columns.Add("IssuedBy", "Issued By");
            partsDataGridView.ReadOnly = true;
            partsDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }
        catch (Exception ex)
        {
            UpdateStatus($"Error loading parts: {ex.Message}");
        }
    }

    private async Task LoadOperations()
    {
        try
        {
            operationsDataGridView.Columns.Clear();
            operationsDataGridView.Columns.Add("Operation", "Operation");
            operationsDataGridView.Columns.Add("IssuedBy", "Issued By");
            operationsDataGridView.ReadOnly = true;
            operationsDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }
        catch (Exception ex)
        {
            UpdateStatus($"Error loading operations: {ex.Message}");
        }
    }

    private async Task LoadLocations()
    {
        try
        {
            locationsDataGridView.Columns.Clear();
            locationsDataGridView.Columns.Add("Location", "Location");
            locationsDataGridView.Columns.Add("IssuedBy", "Issued By");
            locationsDataGridView.ReadOnly = true;
            locationsDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }
        catch (Exception ex)
        {
            UpdateStatus($"Error loading locations: {ex.Message}");
        }
    }

    private async Task LoadUsers()
    {
        try
        {
            usersDataGridView.Columns.Clear();
            usersDataGridView.Columns.Add("User", "User");
            usersDataGridView.Columns.Add("FullName", "Full Name");
            usersDataGridView.Columns.Add("Shift", "Shift");
            usersDataGridView.ReadOnly = true;
            usersDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }
        catch (Exception ex)
        {
            UpdateStatus($"Error loading users: {ex.Message}");
        }
    }

    private void ShowPanel(string panelName)
    {
        // Hide all panels
        foreach (var panel in _settingsPanels.Values) panel.Visible = false;

        // Show selected panel
        if (_settingsPanels.ContainsKey(panelName)) _settingsPanels[panelName].Visible = true;
    }

    private void UpdateStatus(string message)
    {
        statusLabel.Text = message;
        Application.DoEvents();
    }

    #region Event Handlers

    private void categoryListBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (categoryListBox.SelectedItem != null) ShowPanel(categoryListBox.SelectedItem.ToString()!);
    }

    private async void saveButton_Click(object sender, EventArgs e)
    {
        try
        {
            UpdateStatus("Saving settings...");

            await SaveDatabaseSettings();
            await SaveSettingsJson();
            await SaveShortcutsJson();

            _hasChanges = false;
            UpdateStatus("Settings saved successfully");

            // If theme has changed, reinitialize theme on all forms
            if (_originalThemeName != themeComboBox.SelectedItem?.ToString())
            {
                Model_AppVariables.ThemeName = themeComboBox.SelectedItem?.ToString();
                // Reapply theme to all open forms
                foreach (Form openForm in Application.OpenForms) Core_Themes.ApplyTheme(openForm);
            }

            MessageBox.Show("Settings saved successfully!", "Settings", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            UpdateStatus($"Error saving settings: {ex.Message}");
            MessageBox.Show($"Error saving settings: {ex.Message}", "Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void cancelButton_Click(object sender, EventArgs e)
    {
        if (_hasChanges)
        {
            var result = MessageBox.Show("You have unsaved changes. Are you sure you want to cancel?",
                "Unsaved Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.No)
                return;
        }

        // If theme was changed, reapply the original theme
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

            await Dao_User.SetWipServerAddressAsync(user, serverTextBox.Text);
            await Dao_User.SetWipServerPortAsync(user, portTextBox.Text);
            await Dao_User.SetVisualUserNameAsync(user, usernameTextBox.Text);
            await Dao_User.SetVisualPasswordAsync(user, passwordTextBox.Text);
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
            var themeName = themeComboBox.SelectedItem?.ToString();
            var fontSize = (int)fontSizeNumericUpDown.Value;

            var themeObj = new
            {
                Theme_Name = themeName,
                Theme_FontSize = fontSize
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
            // Ensure any edits are committed before saving
            if (shortcutsDataGridView.IsCurrentCellInEditMode)
                shortcutsDataGridView.EndEdit();

            var user = Core_WipAppVariables.User;
            var shortcuts = new Dictionary<string, string>();

            for (var i = 0; i < shortcutsDataGridView.Rows.Count; i++)
            {
                var row = shortcutsDataGridView.Rows[i];
                var actionName = row.Cells[0].Value?.ToString();
                var shortcutString = row.Cells[1].Value?.ToString() ?? "";

                if (!string.IsNullOrEmpty(actionName))
                {
                    shortcuts[actionName] = shortcutString;
                    // Update in-memory shortcut for runtime
                    Helper_UI_Shortcuts.ApplyShortcutFromDictionary(actionName,
                        Helper_UI_Shortcuts.FromShortcutString(shortcutString));
                }
            }

            // Wrap in { "Shortcuts": { ... } }
            var json = JsonSerializer.Serialize(new { Shortcuts = shortcuts });

            await Dao_User.SetShortcutsJsonAsync(user, json);

            UpdateStatus("Shortcuts saved successfully");
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to save shortcuts: {ex.Message}");
        }
    }

    private async void resetDefaultsButton_Click(object? sender, EventArgs e)
    {
        var result = MessageBox.Show(
            "Are you sure you want to reset all settings, theme, and shortcuts to their default values?",
            "Reset to Defaults",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (result != DialogResult.Yes)
            return;

        try
        {
            // Reset database settings to defaults
            serverTextBox.Text = "localhost";
            portTextBox.Text = "3306";
            databaseTextBox.Text = "mtm_wip_application";
            usernameTextBox.Text = "";
            passwordTextBox.Text = "";
            timeoutTextBox.Text = "30";
            autoReconnectCheckBox.Checked = true;

            // Reset theme to default
            var defaultTheme = Core_Themes.Core_AppThemes.GetThemeNames().FirstOrDefault() ?? "";
            if (!string.IsNullOrEmpty(defaultTheme))
                themeComboBox.SelectedItem = defaultTheme;
            fontSizeNumericUpDown.Value = 9;

            // Reset shortcuts to defaults
            var shortcutDict = Helper_UI_Shortcuts.GetShortcutDictionary();
            shortcutsDataGridView.Rows.Clear();
            foreach (var kvp in shortcutDict)
            {
                var action = kvp.Key;
                var defaultKeys = kvp.Value;
                var shortcutValue = Helper_UI_Shortcuts.ToShortcutString(defaultKeys);

                Helper_UI_Shortcuts.ApplyShortcutFromDictionary(action, defaultKeys);
                shortcutsDataGridView.Rows.Add(action, shortcutValue);
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

    #region Database Object Management Event Handlers

    private void addPartToolStripButton_Click(object sender, EventArgs e)
    {
        ShowSimpleInputDialog("Add Part", "Enter Part ID:", async (partId) =>
        {
            if (!string.IsNullOrWhiteSpace(partId))
            {
                await Dao_Part.InsertPart(partId, Model_AppVariables.User, "Standard");
                await LoadParts();
                _hasChanges = true;
                UpdateStatus($"Part '{partId}' added successfully");
            }
        });
    }

    private void editPartToolStripButton_Click(object sender, EventArgs e)
    {
        if (partsDataGridView.SelectedRows.Count > 0)
        {
            var selectedRow = partsDataGridView.SelectedRows[0];
            var partId = selectedRow.Cells["ItemNumber"].Value?.ToString();

            if (!string.IsNullOrWhiteSpace(partId))
                ShowSimpleInputDialog("Edit Part", $"Edit Part ID (current: {partId}):", async (newPartId) =>
                {
                    if (!string.IsNullOrWhiteSpace(newPartId) && newPartId != partId)
                        // This would need to be implemented as an update method
                        UpdateStatus($"Part editing not yet fully implemented");
                });
        }
        else
        {
            MessageBox.Show("Please select a part to edit.", "No Selection", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }

    private async void deletePartToolStripButton_Click(object sender, EventArgs e)
    {
        if (partsDataGridView.SelectedRows.Count > 0)
        {
            var selectedRow = partsDataGridView.SelectedRows[0];
            var partId = selectedRow.Cells["ItemNumber"].Value?.ToString();

            if (!string.IsNullOrWhiteSpace(partId))
            {
                var result = MessageBox.Show($"Are you sure you want to delete part '{partId}'?",
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                    try
                    {
                        await Dao_Part.DeletePart(partId);
                        await LoadParts();
                        _hasChanges = true;
                        UpdateStatus($"Part '{partId}' deleted successfully");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting part: {ex.Message}", "Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
            }
        }
        else
        {
            MessageBox.Show("Please select a part to delete.", "No Selection", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }

    private void addOperationToolStripButton_Click(object sender, EventArgs e)
    {
        ShowSimpleInputDialog("Add Operation", "Enter Operation:", async (operation) =>
        {
            if (!string.IsNullOrWhiteSpace(operation))
            {
                await Dao_Operation.InsertOperation(operation, Model_AppVariables.User);
                await LoadOperations();
                _hasChanges = true;
                UpdateStatus($"Operation '{operation}' added successfully");
            }
        });
    }

    private void editOperationToolStripButton_Click(object sender, EventArgs e)
    {
        if (operationsDataGridView.SelectedRows.Count > 0)
        {
            var selectedRow = operationsDataGridView.SelectedRows[0];
            var operation = selectedRow.Cells["Operation"].Value?.ToString();

            if (!string.IsNullOrWhiteSpace(operation))
                ShowSimpleInputDialog("Edit Operation", $"Edit Operation (current: {operation}):",
                    async (newOperation) =>
                    {
                        if (!string.IsNullOrWhiteSpace(newOperation) && newOperation != operation)
                            UpdateStatus($"Operation editing not yet fully implemented");
                    });
        }
        else
        {
            MessageBox.Show("Please select an operation to edit.", "No Selection", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }

    private async void deleteOperationToolStripButton_Click(object sender, EventArgs e)
    {
        if (operationsDataGridView.SelectedRows.Count > 0)
        {
            var selectedRow = operationsDataGridView.SelectedRows[0];
            var operation = selectedRow.Cells["Operation"].Value?.ToString();

            if (!string.IsNullOrWhiteSpace(operation))
            {
                var result = MessageBox.Show($"Are you sure you want to delete operation '{operation}'?",
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                    try
                    {
                        await Dao_Operation.DeleteOperation(operation);
                        await LoadOperations();
                        _hasChanges = true;
                        UpdateStatus($"Operation '{operation}' deleted successfully");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting operation: {ex.Message}", "Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
            }
        }
        else
        {
            MessageBox.Show("Please select an operation to delete.", "No Selection", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }

    private void addLocationToolStripButton_Click(object sender, EventArgs e)
    {
        ShowSimpleInputDialog("Add Location", "Enter Location:", async (location) =>
        {
            if (!string.IsNullOrWhiteSpace(location))
            {
                await Dao_Location.InsertLocation(location, Model_AppVariables.User);
                await LoadLocations();
                _hasChanges = true;
                UpdateStatus($"Location '{location}' added successfully");
            }
        });
    }

    private void editLocationToolStripButton_Click(object sender, EventArgs e)
    {
        if (locationsDataGridView.SelectedRows.Count > 0)
        {
            var selectedRow = locationsDataGridView.SelectedRows[0];
            var location = selectedRow.Cells["Location"].Value?.ToString();

            if (!string.IsNullOrWhiteSpace(location))
                ShowSimpleInputDialog("Edit Location", $"Edit Location (current: {location}):", async (newLocation) =>
                {
                    if (!string.IsNullOrWhiteSpace(newLocation) && newLocation != location)
                        UpdateStatus($"Location editing not yet fully implemented");
                });
        }
        else
        {
            MessageBox.Show("Please select a location to edit.", "No Selection", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }

    private async void deleteLocationToolStripButton_Click(object sender, EventArgs e)
    {
        if (locationsDataGridView.SelectedRows.Count > 0)
        {
            var selectedRow = locationsDataGridView.SelectedRows[0];
            var location = selectedRow.Cells["Location"].Value?.ToString();

            if (!string.IsNullOrWhiteSpace(location))
            {
                var result = MessageBox.Show($"Are you sure you want to delete location '{location}'?",
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                    try
                    {
                        await Dao_Location.DeleteLocation(location);
                        await LoadLocations();
                        _hasChanges = true;
                        UpdateStatus($"Location '{location}' deleted successfully");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting location: {ex.Message}", "Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
            }
        }
        else
        {
            MessageBox.Show("Please select a location to delete.", "No Selection", MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }

    private void addUserToolStripButton_Click(object sender, EventArgs e)
    {
        MessageBox.Show(
            "User management requires a more complex dialog. This feature will be implemented in a future update.",
            "Feature Not Available", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void editUserToolStripButton_Click(object sender, EventArgs e)
    {
        MessageBox.Show(
            "User management requires a more complex dialog. This feature will be implemented in a future update.",
            "Feature Not Available", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void deleteUserToolStripButton_Click(object sender, EventArgs e)
    {
        MessageBox.Show(
            "User management requires a more complex dialog. This feature will be implemented in a future update.",
            "Feature Not Available", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    #endregion

    #region Helper Methods

    private void ShowSimpleInputDialog(string title, string prompt, Func<string, Task> onConfirm)
    {
        using var inputForm = new Form();
        inputForm.Text = title;
        inputForm.Size = new Size(400, 150);
        inputForm.StartPosition = FormStartPosition.CenterParent;
        inputForm.FormBorderStyle = FormBorderStyle.FixedDialog;
        inputForm.MaximizeBox = false;
        inputForm.MinimizeBox = false;

        var label = new Label { Text = prompt, Location = new Point(10, 20), Size = new Size(360, 20) };
        var textBox = new TextBox { Location = new Point(10, 45), Size = new Size(360, 20) };
        var okButton = new Button { Text = "OK", Location = new Point(215, 75), Size = new Size(75, 23) };
        var cancelButton = new Button { Text = "Cancel", Location = new Point(295, 75), Size = new Size(75, 23) };

        okButton.Click += async (s, e) =>
        {
            if (!string.IsNullOrWhiteSpace(textBox.Text))
            {
                inputForm.DialogResult = DialogResult.OK;
                inputForm.Close();
                try
                {
                    await onConfirm(textBox.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        };

        cancelButton.Click += (s, e) =>
        {
            inputForm.DialogResult = DialogResult.Cancel;
            inputForm.Close();
        };

        inputForm.Controls.AddRange(new Control[] { label, textBox, okButton, cancelButton });
        inputForm.AcceptButton = okButton;
        inputForm.CancelButton = cancelButton;

        // Apply theme if possible
        try
        {
            Core_Themes.ApplyTheme(inputForm);
        }
        catch
        {
            // Theme application failed, continue without theming
        }

        inputForm.ShowDialog(this);
    }

    #endregion
}