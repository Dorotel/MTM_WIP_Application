using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Models;
using MTM_Inventory_Application.Controls.SettingsForm;
using MTM_Inventory_Application.Controls.Shared;
using System.Data;
using System.Text.Json;

namespace MTM_Inventory_Application.Forms.Settings;

public partial class SettingsForm : Form
{
    private bool _hasChanges = false;
    private readonly Dictionary<string, Panel> _settingsPanels;
    private string? _originalThemeName;
    private ProgressBarUserControl _loadingProgress = null!;

    public SettingsForm()
    {
        InitializeComponent();

        // Initialize settings panels dictionary
        _settingsPanels = new Dictionary<string, Panel>
        {
            ["Database"] = databasePanel,
            ["Add Part Number"] = addPartPanel,
            ["Edit Part Number"] = editPartPanel,
            ["Remove Part Number"] = removePartPanel,
            ["Add Operation"] = addOperationPanel,
            ["Edit Operation"] = editOperationPanel,
            ["Remove Operation"] = removeOperationPanel,
            ["Add Location"] = addLocationPanel,
            ["Edit Location"] = editLocationPanel,
            ["Remove Location"] = removeLocationPanel,
            ["Add Item Type"] = addItemTypePanel,
            ["Edit Item Type"] = editItemTypePanel,
            ["Remove Item Type"] = removeItemTypePanel,
            ["Theme"] = themePanel,
            ["Shortcuts"] = shortcutsPanel,
            ["About"] = aboutPanel
        };

        // Store the original theme name for later comparison
        _originalThemeName = Model_AppVariables.ThemeName;

        // Initialize progress control
        InitializeProgressControl();

        // Wire up themeComboBox event
        themeComboBox.SelectedIndexChanged += ThemeComboBox_SelectedIndexChanged;

        // Wire up shortcutsDataGridView event
        shortcutsDataGridView.CellBeginEdit += ShortcutsDataGridView_CellBeginEdit;

        // Initialize user controls
        InitializeUserControls();

        // Initialize the form
        InitializeForm();
    }

    private void InitializeUserControls()
    {
        // Initialize Add Part Control
        var addPartControl = new AddPartControl();
        addPartControl.Dock = DockStyle.Fill;
        addPartControl.PartAdded += (s, e) =>
        {
            // Refresh other controls when a part is added
            UpdateStatus("Part added successfully - lists refreshed");
        };
        addPartPanel.Controls.Add(addPartControl);

        // Initialize Edit Part Control
        var editPartControl = new EditPartControl();
        editPartControl.Dock = DockStyle.Fill;
        editPartControl.PartUpdated += (s, e) =>
        {
            // Refresh other controls when a part is updated
            UpdateStatus("Part updated successfully - lists refreshed");
        };
        editPartPanel.Controls.Add(editPartControl);

        // Initialize Remove Part Control
        var removePartControl = new RemovePartControl();
        removePartControl.Dock = DockStyle.Fill;
        removePartControl.PartRemoved += (s, e) =>
        {
            // Refresh other controls when a part is removed
            UpdateStatus("Part removed successfully - lists refreshed");
        };
        removePartPanel.Controls.Add(removePartControl);

        // Initialize Add Operation Control
        var addOperationControl = new AddOperationControl();
        addOperationControl.Dock = DockStyle.Fill;
        addOperationControl.OperationAdded += (s, e) =>
        {
            UpdateStatus("Operation added successfully - lists refreshed");
        };
        addOperationPanel.Controls.Add(addOperationControl);

        // Initialize Edit Operation Control
        var editOperationControl = new EditOperationControl();
        editOperationControl.Dock = DockStyle.Fill;
        editOperationControl.OperationUpdated += (s, e) =>
        {
            UpdateStatus("Operation updated successfully - lists refreshed");
        };
        editOperationPanel.Controls.Add(editOperationControl);

        // Initialize Remove Operation Control
        var removeOperationControl = new RemoveOperationControl();
        removeOperationControl.Dock = DockStyle.Fill;
        removeOperationControl.OperationRemoved += (s, e) =>
        {
            UpdateStatus("Operation removed successfully - lists refreshed");
        };
        removeOperationPanel.Controls.Add(removeOperationControl);

        // Initialize Add Location Control
        var addLocationControl = new AddLocationControl();
        addLocationControl.Dock = DockStyle.Fill;
        addLocationControl.LocationAdded += (s, e) =>
        {
            UpdateStatus("Location added successfully - lists refreshed");
        };
        addLocationPanel.Controls.Add(addLocationControl);

        // Initialize Edit Location Control
        var editLocationControl = new EditLocationControl();
        editLocationControl.Dock = DockStyle.Fill;
        editLocationControl.LocationUpdated += (s, e) =>
        {
            UpdateStatus("Location updated successfully - lists refreshed");
        };
        editLocationPanel.Controls.Add(editLocationControl);

        // Initialize Remove Location Control
        var removeLocationControl = new RemoveLocationControl();
        removeLocationControl.Dock = DockStyle.Fill;
        removeLocationControl.LocationRemoved += (s, e) =>
        {
            UpdateStatus("Location removed successfully - lists refreshed");
        };
        removeLocationPanel.Controls.Add(removeLocationControl);

        // Initialize Add Item Type Control
        var addItemTypeControl = new AddItemTypeControl();
        addItemTypeControl.Dock = DockStyle.Fill;
        addItemTypeControl.ItemTypeAdded += (s, e) =>
        {
            UpdateStatus("Item type added successfully - lists refreshed");
        };
        addItemTypePanel.Controls.Add(addItemTypeControl);

        // Initialize Edit Item Type Control
        var editItemTypeControl = new EditItemTypeControl();
        editItemTypeControl.Dock = DockStyle.Fill;
        editItemTypeControl.ItemTypeUpdated += (s, e) =>
        {
            UpdateStatus("Item type updated successfully - lists refreshed");
        };
        editItemTypePanel.Controls.Add(editItemTypeControl);

        // Initialize Remove Item Type Control
        var removeItemTypeControl = new RemoveItemTypeControl();
        removeItemTypeControl.Dock = DockStyle.Fill;
        removeItemTypeControl.ItemTypeRemoved += (s, e) =>
        {
            UpdateStatus("Item type removed successfully - lists refreshed");
        };
        removeItemTypePanel.Controls.Add(removeItemTypeControl);
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
            ShowLoadingProgress("Loading settings...");
            UpdateLoadingProgress(10, "Loading settings...");

            // Load database connection settings
            UpdateLoadingProgress(25, "Loading database settings...");
            await LoadDatabaseSettings();

            // Load theme settings
            UpdateLoadingProgress(50, "Loading theme settings...");
            await LoadThemeSettings();

            // Load shortcuts
            UpdateLoadingProgress(75, "Loading shortcuts...");
            await LoadShortcuts();

            // Load about information
            UpdateLoadingProgress(90, "Loading about information...");
            LoadAboutInfo();

            UpdateLoadingProgress(100, "Settings loaded successfully");
            UpdateStatus("Settings loaded successfully");
            _hasChanges = false;

            // Hide progress after a brief delay
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

            serverTextBox.Text = await Dao_User.GetWipServerAddressAsync(user) ?? "172.16.1.104";
            portTextBox.Text = await Dao_User.GetWipServerPortAsync(user) ?? "3306";
            databaseTextBox.Text = await Dao_User.GetDatabaseAsync(user) ?? "mtm_wip_application";
            usernameTextBox.Text = await Dao_User.GetVisualUserNameAsync(user) ?? "";
            passwordTextBox.Text = await Dao_User.GetVisualPasswordAsync(user) ?? "";
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

    private async void categoryListBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (categoryListBox.SelectedItem == null)
            return;

        var selected = categoryListBox.SelectedItem.ToString()!;
        ShowLoadingProgress($"Loading {selected} settings...");
        UpdateLoadingProgress(0, $"Loading {selected} settings...");

        // Load the relevant panel with progress updates
        await LoadPanelAsync(selected);

        UpdateLoadingProgress(100, $"{selected} loaded");
        await Task.Delay(300);
        HideLoadingProgress();
        ShowPanel(selected);
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
            default:
                // For Add/Edit/Remove Part panels, just a short delay for effect
                UpdateLoadingProgress(50, $"Loading {panelName}...");
                await Task.Delay(200);
                UpdateLoadingProgress(80, $"{panelName} loaded");
                break;
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

            // If theme has changed, reinitialize theme on all forms
            if (_originalThemeName != themeComboBox.SelectedItem?.ToString())
            {
                Model_AppVariables.ThemeName = themeComboBox.SelectedItem?.ToString();
                // Reapply theme to all open forms
                foreach (Form openForm in Application.OpenForms) Core_Themes.ApplyTheme(openForm);
            }

            UpdateLoadingProgress(100, "Settings saved successfully");

            // Hide progress after a brief delay
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
            await Dao_User.SetDatabaseAsync(user, databaseTextBox.Text);
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
            // Reset database settings to defaults
            serverTextBox.Text = "172.16.1.104";
            portTextBox.Text = "3306";
            databaseTextBox.Text = "mtm_wip_application";
            usernameTextBox.Text = "";
            passwordTextBox.Text = "";

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


    #region Progress Control

    private void InitializeProgressControl()
    {
        try
        {
            // Create and configure the progress control
            _loadingProgress = new ProgressBarUserControl
            {
                Size = new Size(350, 120),
                Visible = false,
                Anchor = AnchorStyles.None,
                StatusText = "Loading settings..."
            };

            // Position the progress control at the center of the form
            _loadingProgress.Location = new Point(
                (Width - _loadingProgress.Width) / 2,
                (Height - _loadingProgress.Height) / 2
            );

            // Add to form so it appears on top
            Controls.Add(_loadingProgress);
            _loadingProgress.BringToFront();
        }
        catch (Exception ex)
        {
            // Log error but don't fail initialization
            UpdateStatus($"Warning: Could not initialize progress control - {ex.Message}");
        }
    }

    private void ShowLoadingProgress(string status = "Loading...")
    {
        try
        {
            if (_loadingProgress != null)
            {
                // Center the progress control on the form
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
}