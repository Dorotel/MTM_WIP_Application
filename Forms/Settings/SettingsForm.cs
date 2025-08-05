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

        public bool HasChanges = false;
        private readonly Dictionary<string, Panel> _settingsPanels;
        private Control_ProgressBarUserControl _loadingControlProgress = null!;



        #endregion

        #region Constructors

        public SettingsForm()
        {
            InitializeComponent();

         
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

            InitializeProgressControl();


            InitializeUserControls();

            InitializeForm();
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
            var controlShortcuts = new Control_Shortcuts
            {
                Dock = DockStyle.Fill
            };
            controlShortcuts.ShortcutsUpdated += (s, e) =>
            {
                UpdateStatus("Shortcuts updated successfully.");
                HasChanges = true;
            };
            controlShortcuts.StatusMessageChanged += (s, message) =>
            {
                UpdateStatus(message);
            };
            SettingsForm_Panel_Shortcuts.Controls.Add(controlShortcuts);

            var controlTheme = new Control_Theme
            {
                Dock = DockStyle.Fill
            };
            controlTheme.ThemeChanged += (s, e) =>
            {
                UpdateStatus("Theme changed successfully.");
                HasChanges = true;
            };
            controlTheme.StatusMessageChanged += (s, message) =>
            {
                UpdateStatus(message);
            };
            SettingsForm_Panel_Theme.Controls.Add(controlTheme);

            var controlDatabase = new Control_Database
            {
                Dock = DockStyle.Fill
            };
            controlDatabase.DatabaseSettingsUpdated += (s, e) =>
            {
                UpdateStatus("Database settings updated successfully.");
                HasChanges = true;
            };
            controlDatabase.StatusMessageChanged += (s, message) =>
            {
                UpdateStatus(message);
            };
            SettingsForm_Panel_Database.Controls.Add(controlDatabase);

            //var controlAbout = new Control_About
            //{
            //   Dock = DockStyle.Fill
            //};
            //SettingsForm_Panel_About.Controls.Add(controlAbout);

            var controlAbout = new Control_About
            {
                Dock = DockStyle.Fill
            };
            controlAbout.StatusMessageChanged += (s, message) =>
            {
                UpdateStatus(message);
            };
            SettingsForm_Panel_About.Controls.Add(controlAbout);

            var controlAddUser = new Control_Add_User
            {
                Dock = DockStyle.Fill
            };
            SettingsForm_Panel_AddUser.Controls.Add(controlAddUser);
            controlAddUser.UserAdded += (s, e) =>
            {
                UpdateStatus("User added successfully.");
                HasChanges = true;
            };
            controlAddUser.StatusMessageChanged += (s, message) =>
            {
                UpdateStatus(message);
            };


            var controlEditUser = new Control_Edit_User
            {
                Dock = DockStyle.Fill
            };
            SettingsForm_Panel_EditUser.Controls.Add(controlEditUser);
            controlEditUser.UserEdited += (s, e) =>
            {
                UpdateStatus("User updated successfully.");
                HasChanges = true;
            };
            controlEditUser.StatusMessageChanged += (s, message) =>
            {
                UpdateStatus(message);
            };

            var controlDeleteUser = new Control_Remove_User
            {
                Dock = DockStyle.Fill
            };
            SettingsForm_Panel_DeleteUser.Controls.Add(controlDeleteUser);
            controlDeleteUser.UserRemoved += (s, e) =>
            {
                UpdateStatus("User deleted successfully.");
                HasChanges = true;
            };

            var controlAddPart = new Control_Add_PartID
            {
                Dock = DockStyle.Fill
            };
            SettingsForm_Panel_AddPart.Controls.Add(controlAddPart);
            controlAddPart.PartAdded += (s, e) =>
            {
                UpdateStatus("Part added successfully - lists refreshed");
                HasChanges = true;
            };


            var controlEditPartId = new Control_Edit_PartID
            {
                Dock = DockStyle.Fill
            };
            SettingsForm_Panel_EditPart.Controls.Add(controlEditPartId);
            controlEditPartId.PartUpdated += (s, e) =>
            {
                UpdateStatus("Part updated successfully - lists refreshed");
                HasChanges = true;
            };

            var controlRemovePartId = new Control_Remove_PartID
            {
                Dock = DockStyle.Fill
            };
            SettingsForm_Panel_RemovePart.Controls.Add(controlRemovePartId);
            controlRemovePartId.PartRemoved += (s, e) =>
            {
                UpdateStatus("Part removed successfully - lists refreshed");
                HasChanges = true;
            };

            var controlAddOperation = new Control_Add_Operation
            {
                Dock = DockStyle.Fill
            };
            SettingsForm_Panel_AddOperation.Controls.Add(controlAddOperation);
            controlAddOperation.OperationAdded += (s, e) =>
            {
                UpdateStatus("Operation added successfully - lists refreshed");
                HasChanges = true;
            };

            var controlEditOperation = new Control_Edit_Operation
            {
                Dock = DockStyle.Fill
            };
            SettingsForm_Panel_EditOperation.Controls.Add(controlEditOperation);
            controlEditOperation.OperationUpdated += (s, e) =>
            {
                UpdateStatus("Operation updated successfully - lists refreshed");
                HasChanges = true;
            };

            var controlRemoveOperation = new Control_Remove_Operation
            {
                Dock = DockStyle.Fill
            };
            SettingsForm_Panel_RemoveOperation.Controls.Add(controlRemoveOperation);
            controlRemoveOperation.OperationRemoved += (s, e) =>
            {
                UpdateStatus("Operation removed successfully - lists refreshed");
                HasChanges = true;
            };

            var controlAddLocation = new Control_Add_Location
            {
                Dock = DockStyle.Fill
            };
            SettingsForm_Panel_AddLocation.Controls.Add(controlAddLocation);
            controlAddLocation.LocationAdded += (s, e) =>
            {
                UpdateStatus("Location added successfully - lists refreshed");
                HasChanges = true;
            };

            var controlEditLocation = new Control_Edit_Location
            {
                Dock = DockStyle.Fill
            };
            SettingsForm_Panel_EditLocation.Controls.Add(controlEditLocation);
            controlEditLocation.LocationUpdated += (s, e) =>
            {
                UpdateStatus("Location updated successfully - lists refreshed");
                HasChanges = true;
            };

            var controlRemoveLocation = new Control_Remove_Location
            {
                Dock = DockStyle.Fill
            };
            SettingsForm_Panel_RemoveLocation.Controls.Add(controlRemoveLocation);
            controlRemoveLocation.LocationRemoved += (s, e) =>
            {
                UpdateStatus("Location removed successfully - lists refreshed");
                HasChanges = true;
            };

            var controlAddItemType = new Control_Add_ItemType
            {
                Dock = DockStyle.Fill
            };
            SettingsForm_Panel_AddItemType.Controls.Add(controlAddItemType);
            controlAddItemType.ItemTypeAdded += (s, e) =>
            {
                UpdateStatus("ItemType added successfully - lists refreshed");
                HasChanges = true;
            };

            var controlEditItemType = new Control_Edit_ItemType
            {
                Dock = DockStyle.Fill
            };
            SettingsForm_Panel_EditItemType.Controls.Add(controlEditItemType);
            controlEditItemType.ItemTypeUpdated += (s, e) =>
            {
                UpdateStatus("ItemType updated successfully - lists refreshed");
                HasChanges = true;
            };

            var controlRemoveItemType = new Control_Remove_ItemType
            {
                Dock = DockStyle.Fill
            };
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

            ShowLoadingProgress($"Loading {selected} settings...");
            UpdateLoadingProgress(0, $"Loading {selected} settings...");

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


        private void CloseAndResetIfChanged()
        {
            if (HasChanges)
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
            else
            {
                Application.Exit();
                return;
            }
        }

        // Update OnFormClosing to call the instance method
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

    }
}
