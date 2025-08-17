using System.Reflection;
using MTM_Inventory_Application.Controls.SettingsForm;
using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Models;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Services;

namespace MTM_Inventory_Application.Forms.Settings
{
    public partial class SettingsForm : Form
    {
        #region Fields

        public bool HasChanges = false;
        private readonly Dictionary<string, Panel> _settingsPanels;

        #endregion

        #region Constructors

        public SettingsForm()
        {
            Service_DebugTracer.TraceMethodEntry(new Dictionary<string, object>
            {
                ["FormType"] = nameof(SettingsForm),
                ["InitializationTime"] = DateTime.Now,
                ["Thread"] = Thread.CurrentThread.ManagedThreadId
            }, nameof(SettingsForm), nameof(SettingsForm));

            Service_DebugTracer.TraceUIAction("SETTINGS_FORM_INITIALIZATION", nameof(SettingsForm),
                new Dictionary<string, object>
                {
                    ["Phase"] = "START",
                    ["ComponentType"] = "SettingsForm"
                });

            InitializeComponent();

            AutoScaleMode = AutoScaleMode.Dpi;
            Service_DebugTracer.TraceUIAction("THEME_APPLICATION", nameof(SettingsForm),
                new Dictionary<string, object>
                {
                    ["DpiScaling"] = "APPLIED",
                    ["LayoutAdjustments"] = "APPLIED"
                });
            Core_Themes.ApplyDpiScaling(this);
            Core_Themes.ApplyRuntimeLayoutAdjustments(this);

            Service_DebugTracer.TraceUIAction("SETTINGS_PANELS_INITIALIZATION", nameof(SettingsForm),
                new Dictionary<string, object>
                {
                    ["PanelsCount"] = 19,
                    ["PanelTypes"] = new[] { "Database", "User Management", "Part Management", "Operations", "Locations", "ItemTypes", "Theme", "About" }
                });

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

            Service_DebugTracer.TraceUIAction("INITIALIZE_CONTROLS", nameof(SettingsForm),
                new Dictionary<string, object> { ["Phase"] = "START" });
            InitializeUserControls();
            
            Service_DebugTracer.TraceUIAction("INITIALIZE_FORM", nameof(SettingsForm),
                new Dictionary<string, object> { ["Phase"] = "START" });
            InitializeForm();

            Service_DebugTracer.TraceUIAction("SETTINGS_FORM_INITIALIZATION", nameof(SettingsForm),
                new Dictionary<string, object>
                {
                    ["Phase"] = "COMPLETE",
                    ["Success"] = true,
                    ["HasChanges"] = HasChanges
                });

            Service_DebugTracer.TraceMethodExit(null, nameof(SettingsForm), nameof(SettingsForm));
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
            Control_Shortcuts controlShortcuts = new() { Dock = DockStyle.Fill };
            controlShortcuts.ShortcutsUpdated += (s, e) =>
            {
                UpdateStatus("Shortcuts updated successfully.");
                HasChanges = true;
            };
            controlShortcuts.StatusMessageChanged += (s, message) => { UpdateStatus(message); };
            SettingsForm_Panel_Shortcuts.Controls.Add(controlShortcuts);

            Control_Theme controlTheme = new() { Dock = DockStyle.Fill };
            controlTheme.ThemeChanged += (s, e) =>
            {
                UpdateStatus("Theme changed successfully.");
                HasChanges = true;
            };
            controlTheme.StatusMessageChanged += (s, message) => { UpdateStatus(message); };
            SettingsForm_Panel_Theme.Controls.Add(controlTheme);

            Control_Database controlDatabase = new() { Dock = DockStyle.Fill };
            controlDatabase.DatabaseSettingsUpdated += (s, e) =>
            {
                UpdateStatus("Database settings updated successfully.");
                HasChanges = true;
            };
            controlDatabase.StatusMessageChanged += (s, message) => { UpdateStatus(message); };
            SettingsForm_Panel_Database.Controls.Add(controlDatabase);

            Control_About controlAbout = new() { Dock = DockStyle.Fill };
            controlAbout.StatusMessageChanged += (s, message) => { UpdateStatus(message); };
            SettingsForm_Panel_About.Controls.Add(controlAbout);

            Control_Add_User controlAddUser = new() { Dock = DockStyle.Fill };
            SettingsForm_Panel_AddUser.Controls.Add(controlAddUser);

            // Pass the ToolStrip progress controls
            controlAddUser.SetProgressControls(SettingsForm_ProgressBar, SettingsForm_StatusText);

            controlAddUser.UserAdded += (s, e) =>
            {
                UpdateStatus("User added successfully.");
                HasChanges = true;
            };
            controlAddUser.StatusMessageChanged += (s, message) => { UpdateStatus(message); };

            Control_Edit_User controlEditUser = new() { Dock = DockStyle.Fill };
            SettingsForm_Panel_EditUser.Controls.Add(controlEditUser);
            controlEditUser.UserEdited += (s, e) =>
            {
                UpdateStatus("User updated successfully.");
                HasChanges = true;
            };
            controlEditUser.StatusMessageChanged += (s, message) => { UpdateStatus(message); };

            Control_Remove_User controlDeleteUser = new() { Dock = DockStyle.Fill };
            SettingsForm_Panel_DeleteUser.Controls.Add(controlDeleteUser);

            // Pass the ToolStrip progress controls
            controlDeleteUser.SetProgressControls(SettingsForm_ProgressBar, SettingsForm_StatusText);

            controlDeleteUser.UserRemoved += (s, e) =>
            {
                UpdateStatus("User deleted successfully.");
                HasChanges = true;
            };

            Control_Add_PartID controlAddPart = new() { Dock = DockStyle.Fill };
            SettingsForm_Panel_AddPart.Controls.Add(controlAddPart);
            controlAddPart.PartAdded += (s, e) =>
            {
                UpdateStatus("Part added successfully - lists refreshed");
                HasChanges = true;
            };

            Control_Edit_PartID controlEditPartId = new() { Dock = DockStyle.Fill };
            SettingsForm_Panel_EditPart.Controls.Add(controlEditPartId);
            controlEditPartId.PartUpdated += (s, e) =>
            {
                UpdateStatus("Part updated successfully - lists refreshed");
                HasChanges = true;
            };

            Control_Remove_PartID controlRemovePartId = new() { Dock = DockStyle.Fill };
            SettingsForm_Panel_RemovePart.Controls.Add(controlRemovePartId);
            controlRemovePartId.PartRemoved += (s, e) =>
            {
                UpdateStatus("Part removed successfully - lists refreshed");
                HasChanges = true;
            };

            Control_Add_Operation controlAddOperation = new() { Dock = DockStyle.Fill };
            SettingsForm_Panel_AddOperation.Controls.Add(controlAddOperation);
            
            // Pass the ToolStrip progress controls
            controlAddOperation.SetProgressControls(SettingsForm_ProgressBar, SettingsForm_StatusText);
            
            controlAddOperation.OperationAdded += (s, e) =>
            {
                UpdateStatus("Operation added successfully - lists refreshed");
                HasChanges = true;
            };
            controlAddOperation.StatusMessageChanged += (s, message) => { UpdateStatus(message); };

            Control_Edit_Operation controlEditOperation = new() { Dock = DockStyle.Fill };
            SettingsForm_Panel_EditOperation.Controls.Add(controlEditOperation);
            controlEditOperation.OperationUpdated += (s, e) =>
            {
                UpdateStatus("Operation updated successfully - lists refreshed");
                HasChanges = true;
            };

            Control_Remove_Operation controlRemoveOperation = new() { Dock = DockStyle.Fill };
            SettingsForm_Panel_RemoveOperation.Controls.Add(controlRemoveOperation);
            controlRemoveOperation.OperationRemoved += (s, e) =>
            {
                UpdateStatus("Operation removed successfully - lists refreshed");
                HasChanges = true;
            };

            Control_Add_Location controlAddLocation = new() { Dock = DockStyle.Fill };
            SettingsForm_Panel_AddLocation.Controls.Add(controlAddLocation);
            controlAddLocation.LocationAdded += (s, e) =>
            {
                UpdateStatus("Location added successfully - lists refreshed");
                HasChanges = true;
            };

            Control_Edit_Location controlEditLocation = new() { Dock = DockStyle.Fill };
            SettingsForm_Panel_EditLocation.Controls.Add(controlEditLocation);
            controlEditLocation.LocationUpdated += (s, e) =>
            {
                UpdateStatus("Location updated successfully - lists refreshed");
                HasChanges = true;
            };

            Control_Remove_Location controlRemoveLocation = new() { Dock = DockStyle.Fill };
            SettingsForm_Panel_RemoveLocation.Controls.Add(controlRemoveLocation);
            controlRemoveLocation.LocationRemoved += (s, e) =>
            {
                UpdateStatus("Location removed successfully - lists refreshed");
                HasChanges = true;
            };

            Control_Add_ItemType controlAddItemType = new() { Dock = DockStyle.Fill };
            SettingsForm_Panel_AddItemType.Controls.Add(controlAddItemType);
            controlAddItemType.ItemTypeAdded += (s, e) =>
            {
                UpdateStatus("ItemType added successfully - lists refreshed");
                HasChanges = true;
            };

            Control_Edit_ItemType controlEditItemType = new() { Dock = DockStyle.Fill };
            SettingsForm_Panel_EditItemType.Controls.Add(controlEditItemType);
            controlEditItemType.ItemTypeUpdated += (s, e) =>
            {
                UpdateStatus("ItemType updated successfully - lists refreshed");
                HasChanges = true;
            };

            Control_Remove_ItemType controlRemoveItemType = new() { Dock = DockStyle.Fill };
            SettingsForm_Panel_RemoveItemType.Controls.Add(controlRemoveItemType);
            controlRemoveItemType.ItemTypeRemoved += (s, e) =>
            {
                UpdateStatus("ItemType removed successfully - lists refreshed");
                HasChanges = true;
            };
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
                panel.Visible = false;
            }

            InitializeCategoryTreeView();
            ShowPanel("Database");

            ApplyPrivileges();
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

            ShowProgress($"Loading {selected} settings...");
            UpdateProgress(0, $"Loading {selected} settings...");

            UpdateProgress(100, $"{selected} loaded");
            await Task.Delay(300);
            HideProgress();
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

        private void UpdateStatus(string message) => SettingsForm_StatusText.Text = message;

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (HasChanges)
                {
                    DialogResult result = MessageBox.Show(
                        @"You have changes that require a restart. Exit and reset the application?",
                        @"Unsaved Changes",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        // Allow closing, then restart
                        e.Cancel = false;
                        Application.Restart();
                        Application.ExitThread();
                    }
                    else
                    {
                        // Prevent closing
                        e.Cancel = true;
                    }
                }
                else
                {
                    // Allow closing
                    e.Cancel = false;
                }
            }
            else
            {
                base.OnFormClosing(e);
            }
        }

        #endregion

        #region Progress Control Methods

        private Helper_StoredProcedureProgress? _progressHelper;

        /// <summary>
        /// Get or create progress helper instance
        /// </summary>
        private Helper_StoredProcedureProgress ProgressHelper
        {
            get
            {
                _progressHelper ??= Helper_StoredProcedureProgress.Create(
                    SettingsForm_ProgressBar, 
                    SettingsForm_StatusText, 
                    this);
                return _progressHelper;
            }
        }

        private void ShowProgress(string status = "Loading...")
        {
            try
            {
                ProgressHelper.ShowProgress(status);
            }
            catch (Exception ex)
            {
                UpdateStatus($"Warning: Progress display error - {ex.Message}");
            }
        }

        private void UpdateProgress(int progress, string status)
        {
            try
            {
                ProgressHelper.UpdateProgress(progress, status);
            }
            catch (Exception ex)
            {
                UpdateStatus($"Warning: Progress update error - {ex.Message}");
            }
        }

        private void ShowError(string errorMessage, int? progress = null)
        {
            try
            {
                ProgressHelper.ShowError(errorMessage, progress);
            }
            catch (Exception ex)
            {
                UpdateStatus($"Warning: Error display error - {ex.Message}");
            }
        }

        private void ShowSuccess(string successMessage)
        {
            try
            {
                ProgressHelper.ShowSuccess(successMessage);
            }
            catch (Exception ex)
            {
                UpdateStatus($"Warning: Success display error - {ex.Message}");
            }
        }

        private void HideProgress()
        {
            try
            {
                ProgressHelper.HideProgress();
            }
            catch (Exception ex)
            {
                UpdateStatus($"Warning: Progress hide error - {ex.Message}");
            }
        }

        /// <summary>
        /// Process stored procedure result and update progress accordingly
        /// </summary>
        /// <param name="result">Stored procedure result</param>
        /// <param name="successMessage">Custom success message (optional)</param>
        public void ProcessStoredProcedureResult(StoredProcedureResult result, string successMessage = null)
        {
            try
            {
                if (result.IsSuccess)
                {
                    ShowSuccess(successMessage ?? result.ErrorMessage);
                }
                else
                {
                    ShowError(result.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"Warning: Result processing error - {ex.Message}");
            }
        }

        /// <summary>
        /// Process generic stored procedure result and update progress accordingly
        /// </summary>
        /// <typeparam name="T">Result data type</typeparam>
        /// <param name="result">Stored procedure result</param>
        /// <param name="successMessage">Custom success message (optional)</param>
        public void ProcessStoredProcedureResult<T>(StoredProcedureResult<T> result, string successMessage = null)
        {
            try
            {
                if (result.IsSuccess)
                {
                    ShowSuccess(successMessage ?? result.ErrorMessage);
                }
                else
                {
                    ShowError(result.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"Warning: Result processing error - {ex.Message}");
            }
        }

        #endregion
    }
}
