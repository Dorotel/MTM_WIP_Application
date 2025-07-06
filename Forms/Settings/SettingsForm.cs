using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Models;
using System.Data;

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
            databaseTextBox.Text = "mtm_wip_application";
            usernameTextBox.Text = await Dao_User.GetVisualUserNameAsync(user) ?? "";
            passwordTextBox.Text = await Dao_User.GetVisualPasswordAsync(user) ?? "";
            timeoutTextBox.Text = "30";
            autoReconnectCheckBox.Checked = true;
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
            // Load saved shortcuts from database
            await LoadShortcutsFromDatabase();

            // Configure shortcuts DataGridView
            shortcutsDataGridView.Columns.Clear();
            shortcutsDataGridView.Columns.Add("Action", "Action");
            shortcutsDataGridView.Columns.Add("Shortcut", "Shortcut");

            // Clear existing rows
            shortcutsDataGridView.Rows.Clear();

            // Get current shortcuts from Core_WipAppVariables
            var shortcuts = Core_WipAppVariables.GetShortcutDictionary();

            // Add shortcut rows
            foreach (var shortcut in shortcuts)
            {
                shortcutsDataGridView.Rows.Add(shortcut.Key, Core_WipAppVariables.ToShortcutString(shortcut.Value));
            }

            // Configure grid for editing
            shortcutsDataGridView.ReadOnly = false;
            shortcutsDataGridView.AllowUserToAddRows = false;
            shortcutsDataGridView.AllowUserToDeleteRows = false;
            shortcutsDataGridView.Columns[0].ReadOnly = true; // Action column is read-only
            shortcutsDataGridView.Columns[1].ReadOnly = false; // Shortcut column is editable
            
            // Add event handlers for cell editing
            shortcutsDataGridView.CellValueChanged += ShortcutsDataGridView_CellValueChanged;
            shortcutsDataGridView.CellValidating += ShortcutsDataGridView_CellValidating;
        }
        catch (Exception ex)
        {
            UpdateStatus($"Error loading shortcuts: {ex.Message}");
        }
    }

    private async Task LoadShortcutsFromDatabase()
    {
        try
        {
            var shortcutsJson = await Dao_User.GetShortcutsAsync(Core_WipAppVariables.User);
            if (!string.IsNullOrEmpty(shortcutsJson))
            {
                var shortcuts = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(shortcutsJson);
                if (shortcuts != null)
                {
                    foreach (var shortcut in shortcuts)
                    {
                        var keys = Core_WipAppVariables.FromShortcutString(shortcut.Value);
                        Core_WipAppVariables.ApplyShortcutFromDictionary(shortcut.Key, keys);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // If loading fails, use default shortcuts (no action needed)
            UpdateStatus($"Using default shortcuts: {ex.Message}");
        }
    }

    private void ShortcutsDataGridView_CellValidating(object? sender, DataGridViewCellValidatingEventArgs e)
    {
        if (e.ColumnIndex == 1) // Shortcut column
        {
            var shortcutString = e.FormattedValue?.ToString() ?? "";
            
            // Allow empty shortcuts
            if (string.IsNullOrWhiteSpace(shortcutString))
            {
                return;
            }

            // Validate shortcut format
            try
            {
                var keys = Core_WipAppVariables.FromShortcutString(shortcutString);
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
            {
                try
                {
                    var keys = Core_WipAppVariables.FromShortcutString(shortcutString);
                    Core_WipAppVariables.ApplyShortcutFromDictionary(actionName, keys);
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
            await SaveThemeSettings();
            await SaveShortcuts();

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

    private async Task SaveThemeSettings()
    {
        try
        {
            var user = Model_AppVariables.User;

            if (themeComboBox.SelectedItem != null)
                await Dao_User.SetThemeNameAsync(user, themeComboBox.SelectedItem.ToString()!);

            await Dao_User.SetThemeFontSizeAsync(user, (int)fontSizeNumericUpDown.Value);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to save theme settings: {ex.Message}");
        }
    }

    private async Task SaveShortcuts()
    {
        try
        {
            // Create a dictionary to store current shortcuts
            var shortcuts = new Dictionary<string, string>();
            
            // Collect shortcuts from the DataGridView
            for (int i = 0; i < shortcutsDataGridView.Rows.Count; i++)
            {
                var row = shortcutsDataGridView.Rows[i];
                var actionName = row.Cells[0].Value?.ToString();
                var shortcutString = row.Cells[1].Value?.ToString() ?? "";
                
                if (!string.IsNullOrEmpty(actionName))
                {
                    shortcuts[actionName] = shortcutString;
                }
            }
            
            // Serialize to JSON and save to database
            var shortcutsJson = System.Text.Json.JsonSerializer.Serialize(shortcuts);
            await Dao_User.SetShortcutsAsync(Core_WipAppVariables.User, shortcutsJson);
            
            UpdateStatus("Shortcuts saved successfully");
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to save shortcuts: {ex.Message}");
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