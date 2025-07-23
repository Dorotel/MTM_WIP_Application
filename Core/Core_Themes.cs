using System.Collections.Concurrent;
using System.Data;
using System.Diagnostics;
using System.Text.Json;
using MTM_Inventory_Application.Controls.Addons;
using MTM_Inventory_Application.Controls.MainForm;
using MTM_Inventory_Application.Controls.SettingsForm;
using MTM_Inventory_Application.Controls.Shared;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Models;
using System.Reflection;

namespace MTM_Inventory_Application.Core
{
    /// <summary>
    /// Core theming system that provides comprehensive DPI scaling and UI responsiveness.
    /// 
    /// This class handles:
    /// 1. Runtime DPI scaling for all forms and controls (per Telerik WinForms DPI scaling article)
    /// 2. Async/await UI responsiveness improvements (per Grant Winney async WinForms article)
    /// 3. Theme application with proper color handling
    /// 4. Runtime layout adjustments moved from designer files
    /// 5. Dynamic DPI change handling for multi-monitor scenarios
    /// 
    /// Key Features:
    /// - AutoScaleMode.Dpi set on all forms and controls for proper DPI scaling
    /// - Runtime margin/padding adjustments for TableLayoutPanel, GroupBox, Panel
    /// - SplitContainer distance calculations based on DPI scale
    /// - Event-driven DPI change handling for monitor switching
    /// - Comprehensive control hierarchy traversal for complete coverage
    /// 
    /// References:
    /// - https://www.telerik.com/blogs/winforms-scaling-at-large-dpi-settings-is-it-even-possible-
    /// - https://grantwinney.com/using-async-await-and-task-to-keep-the-winforms-ui-more-responsive/
    /// </summary>
    public static class Core_Themes
    {
        #region Public API

        public static void ApplyTheme(Form form)
        {
            Core_AppThemes.AppTheme theme = Core_AppThemes.GetCurrentTheme();
            string themeName = Core_AppThemes.GetEffectiveThemeName();
            form.SuspendLayout();

            // Apply DPI scaling and layout adjustments first
            // This ensures pixel-perfect scaling at all DPI settings (100%, 125%, 150%, 200%)
            ApplyDpiScaling(form);
            ApplyRuntimeLayoutAdjustments(form);

            // Then apply theme colors
            SetFormTheme(form, theme, themeName);
            ApplyThemeToControls(form.Controls);
            if (form is Forms.MainForm.MainForm mainForm)
            {
                Helper_UI_Shortcuts.UpdateMainFormTabShortcuts(mainForm);
            }
            form.ResumeLayout();
            LoggingUtility.Log($"Global theme '{themeName}' with DPI scaling applied to form '{form.Name}'.");
        }

        public static async Task<Model_UserUiColors> GetUserThemeColorsAsync(string userId)
        {
            Model_AppVariables.ThemeName = await Dao_User.GetSettingsJsonAsync("Theme_Name", userId, true) ?? "Default";
            if (!Core_AppThemes.GetThemeNames().Contains(Model_AppVariables.ThemeName))
            {
                await Core_AppThemes.LoadThemesFromDatabaseAsync();
            }

            Core_AppThemes.AppTheme appTheme = Core_AppThemes.GetTheme(Model_AppVariables.ThemeName);
            return appTheme.Colors;
        }

        public static void ApplyThemeToDataGridView(DataGridView dataGridView)
        {
            if (dataGridView == null)
            {
                return;
            }

            Model_UserUiColors colors = Core_AppThemes.GetCurrentTheme().Colors;

            if (colors.DataGridBackColor.HasValue)
            {
                dataGridView.BackgroundColor = colors.DataGridBackColor.Value;
            }

            if (colors.DataGridForeColor.HasValue)
            {
                dataGridView.ForeColor = colors.DataGridForeColor.Value;
            }

            if (dataGridView.ColumnHeadersDefaultCellStyle != null)
            {
                if (colors.DataGridHeaderBackColor.HasValue)
                {
                    dataGridView.ColumnHeadersDefaultCellStyle.BackColor = colors.DataGridHeaderBackColor.Value;
                }

                if (colors.DataGridHeaderForeColor.HasValue)
                {
                    dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = colors.DataGridHeaderForeColor.Value;
                }
            }

            if (dataGridView.RowsDefaultCellStyle != null)
            {
                if (colors.DataGridRowBackColor.HasValue)
                {
                    dataGridView.RowsDefaultCellStyle.BackColor = colors.DataGridRowBackColor.Value;
                }

                if (colors.DataGridForeColor.HasValue)
                {
                    dataGridView.RowsDefaultCellStyle.ForeColor = colors.DataGridForeColor.Value;
                }
            }

            if (dataGridView.AlternatingRowsDefaultCellStyle != null)
            {
                if (colors.DataGridAltRowBackColor.HasValue)
                {
                    dataGridView.AlternatingRowsDefaultCellStyle.BackColor = colors.DataGridAltRowBackColor.Value;
                }

                if (colors.DataGridForeColor.HasValue)
                {
                    dataGridView.AlternatingRowsDefaultCellStyle.ForeColor = colors.DataGridForeColor.Value;
                }
            }

            if (colors.DataGridGridColor.HasValue)
            {
                dataGridView.GridColor = colors.DataGridGridColor.Value;
            }

            if (colors.DataGridSelectionBackColor.HasValue)
            {
                dataGridView.DefaultCellStyle.SelectionBackColor = colors.DataGridSelectionBackColor.Value;
            }

            if (colors.DataGridSelectionForeColor.HasValue)
            {
                dataGridView.DefaultCellStyle.SelectionForeColor = colors.DataGridSelectionForeColor.Value;
            }

            if (colors.DataGridBorderColor.HasValue)
            {
                OwnerDrawThemeHelper.ApplyDataGridViewBorderColor(dataGridView, colors.DataGridBorderColor.Value);
            }

            // Add column visibility context menu
            AttachColumnVisibilityMenu(dataGridView);
        }

        /// <summary>
        /// Attaches a context menu to a DataGridView header row for toggling column visibility
        /// </summary>
        private static void AttachColumnVisibilityMenu(DataGridView dataGridView)
        {
            // Remove any existing handlers to avoid duplicates
            dataGridView.MouseDown -= DataGridView_MouseDown;
            dataGridView.MouseDown += DataGridView_MouseDown;

            // Create or get existing context menu
            ContextMenuStrip menu = dataGridView.ContextMenuStrip ?? new ContextMenuStrip();
            if (dataGridView.ContextMenuStrip == null)
            {
                dataGridView.ContextMenuStrip = menu;
            }

            // Store original context menu to use when not clicking on header
            dataGridView.Tag = dataGridView.Tag ?? menu;

            static void DataGridView_MouseDown(object? sender, MouseEventArgs e)
            {
                if (sender is not DataGridView dgv || e.Button != MouseButtons.Right)
                    return;

                // Check if click was on the column header area
                var headerRect = dgv.DisplayRectangle;
                headerRect.Height = dgv.ColumnHeadersHeight;

                if (headerRect.Contains(e.Location))
                {
                    ShowColumnVisibilityMenu(dgv, e.Location);
                }
            }
        }

        /// <summary>
        /// Shows the column visibility context menu for a DataGridView
        /// </summary>
        private static void ShowColumnVisibilityMenu(DataGridView dgv, Point location)
        {
            // Get the column that was clicked (if any)
            DataGridViewColumn? clickedColumn = null;
            int headerHeight = dgv.ColumnHeadersHeight;

            // If click is on header row, find which column was clicked
            if (location.Y <= headerHeight)
            {
                int x = location.X;
                int cumulativeWidth = 0;
                foreach (DataGridViewColumn col in dgv.Columns)
                {
                    if (!col.Visible) continue;
                    cumulativeWidth += col.Width;
                    if (x < cumulativeWidth)
                    {
                        clickedColumn = col;
                        break;
                    }
                }
            }

            // Create a new context menu each time to ensure it's up to date
            ContextMenuStrip menu = new();

            // Add a title item
            ToolStripMenuItem titleItem = new("Column Visibility")
            {
                Enabled = false,
                Font = new Font(menu.Font, FontStyle.Bold)
            };
            menu.Items.Add(titleItem);
            menu.Items.Add(new ToolStripSeparator());

            // Add each column as a checkbox menu item
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                ToolStripMenuItem item = new(col.HeaderText)
                {
                    Checked = col.Visible,
                    Tag = col.Name
                };

                item.Click += (s, e) =>
                {
                    if (s is ToolStripMenuItem menuItem && menuItem.Tag is string colName)
                    {
                        DataGridViewColumn? column = dgv.Columns[colName];
                        if (column != null)
                        {
                            // Toggle visibility
                            column.Visible = !column.Visible;
                            menuItem.Checked = column.Visible;
                        }
                    }
                };

                menu.Items.Add(item);
            }

            // Add "Select All" and "Deselect All" options
            menu.Items.Add(new ToolStripSeparator());

            ToolStripMenuItem selectAllItem = new("Select All");
            selectAllItem.Click += (s, e) =>
            {
                foreach (DataGridViewColumn col in dgv.Columns)
                {
                    col.Visible = true;
                }

                // Update all checkboxes
                foreach (ToolStripItem item in menu.Items)
                {
                    if (item is ToolStripMenuItem menuItem && menuItem.Tag is string)
                    {
                        menuItem.Checked = true;
                    }
                }
            };
            menu.Items.Add(selectAllItem);

            ToolStripMenuItem deselectAllItem = new("Deselect All");
            deselectAllItem.Click += (s, e) =>
            {
                // Don't hide all columns - keep at least one visible
                bool hasOneVisible = false;

                foreach (DataGridViewColumn col in dgv.Columns)
                {
                    if (!hasOneVisible)
                    {
                        col.Visible = true;
                        hasOneVisible = true;
                    }
                    else
                    {
                        col.Visible = false;
                    }
                }

                // Update all checkboxes
                foreach (ToolStripItem item in menu.Items)
                {
                    if (item is ToolStripMenuItem menuItem && menuItem.Tag is string colName)
                    {
                        menuItem.Checked = dgv.Columns[colName]?.Visible ?? false;
                    }
                }
            };
            menu.Items.Add(deselectAllItem);

            // Add "Save Grid Settings" option
            menu.Items.Add(new ToolStripSeparator());
            ToolStripMenuItem saveSettingsItem = new("Save Grid Settings");
            saveSettingsItem.Click += async (s, e) =>
            {
                try
                {
                    // Serialize column visibility and order
                    var columns = dgv.Columns.Cast<DataGridViewColumn>()
                        .OrderBy(c => c.DisplayIndex)
                        .Select(c => new
                        {
                            Name = c.Name,
                            Visible = c.Visible,
                            DisplayIndex = c.DisplayIndex
                        }).ToList();
                    string json = JsonSerializer.Serialize(new { Columns = columns });
                    // Get userId (from Model_AppVariables.User)
                    string userId = MTM_Inventory_Application.Models.Model_AppVariables.User;
                    await MTM_Inventory_Application.Data.Dao_User.SetGridViewSettingsJsonAsync(userId, json);
                    MessageBox.Show("Grid settings saved successfully.", "Grid Settings", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving grid settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            menu.Items.Add(saveSettingsItem);

            // Add column reordering options if a specific column was clicked
            if (clickedColumn != null)
            {
                menu.Items.Add(new ToolStripSeparator());

                ToolStripMenuItem columnOrderTitle = new("Column Order")
                {
                    Enabled = false,
                    Font = new Font(menu.Font, FontStyle.Bold)
                };
                menu.Items.Add(columnOrderTitle);

                // Move column left
                ToolStripMenuItem moveLeftItem = new("Move Left")
                {
                    Enabled = clickedColumn.DisplayIndex > 0
                };
                moveLeftItem.Click += (s, e) =>
                {
                    int newIndex = clickedColumn.DisplayIndex - 1;
                    if (newIndex >= 0)
                    {
                        clickedColumn.DisplayIndex = newIndex;
                    }
                };
                menu.Items.Add(moveLeftItem);

                // Move column right
                ToolStripMenuItem moveRightItem = new("Move Right")
                {
                    Enabled = clickedColumn.DisplayIndex < dgv.Columns.Count - 1
                };
                moveRightItem.Click += (s, e) =>
                {
                    int newIndex = clickedColumn.DisplayIndex + 1;
                    if (newIndex < dgv.Columns.Count)
                    {
                        clickedColumn.DisplayIndex = newIndex;
                    }
                };
                menu.Items.Add(moveRightItem);

                // Move to first position
                ToolStripMenuItem moveFirstItem = new("Move to First")
                {
                    Enabled = clickedColumn.DisplayIndex > 0
                };
                moveFirstItem.Click += (s, e) => { clickedColumn.DisplayIndex = 0; };
                menu.Items.Add(moveFirstItem);

                // Move to last position
                ToolStripMenuItem moveLastItem = new("Move to Last")
                {
                    Enabled = clickedColumn.DisplayIndex < dgv.Columns.Count - 1
                };
                moveLastItem.Click += (s, e) => { clickedColumn.DisplayIndex = dgv.Columns.Count - 1; };
                menu.Items.Add(moveLastItem);
            }

            // Show the menu
            menu.Show(dgv, location);
        }

        public static void SizeDataGrid(DataGridView dataGridView) => ThemeAppliersInternal.SizeDataGrid(dataGridView);

        public static void ApplyFocusHighlighting(Control parentControl)
        {
            Core_AppThemes.AppTheme theme = Core_AppThemes.GetCurrentTheme();
            FocusUtils.ApplyFocusEventHandlingToControls(parentControl.Controls, theme.Colors);
        }

        /// <summary>
        /// Applies comprehensive DPI scaling and layout adjustments to a form and all its controls.
        /// This ensures pixel-perfect scaling at all DPI settings (100%, 125%, 150%, 200%).
        /// </summary>
        /// <param name="form">The form to apply DPI scaling to</param>
        public static void ApplyDpiScaling(Form form)
        {
            try
            {
                form.SuspendLayout();
                ApplyDpiScalingToControlHierarchy(form.Controls);
                form.ResumeLayout();
                LoggingUtility.Log($"DPI scaling applied to form '{form.Name}' and all its controls.");
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
            }
        }

        /// <summary>
        /// Applies comprehensive DPI scaling and layout adjustments to a user control and all its controls.
        /// This ensures pixel-perfect scaling at all DPI settings (100%, 125%, 150%, 200%).
        /// </summary>
        /// <param name="userControl">The user control to apply DPI scaling to</param>
        public static void ApplyDpiScaling(UserControl userControl)
        {
            try
            {
                userControl.SuspendLayout();
                ApplyDpiScalingToControlHierarchy(userControl.Controls);
                userControl.ResumeLayout();
                LoggingUtility.Log($"DPI scaling applied to user control '{userControl.Name}' and all its controls.");
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
            }
        }

        /// <summary>
        /// Applies runtime layout adjustments that were moved from designer files.
        /// This includes margin/padding adjustments and SplitContainer configurations.
        /// </summary>
        /// <param name="form">The form to apply layout adjustments to</param>
        public static void ApplyRuntimeLayoutAdjustments(Form form)
        {
            try
            {
                form.SuspendLayout();
                ApplyLayoutAdjustmentsToControlHierarchy(form.Controls);
                form.ResumeLayout();

                LoggingUtility.Log($"Runtime layout adjustments applied to form '{form.Name}'.");
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
            }
        }

        /// <summary>
        /// Applies runtime layout adjustments that were moved from designer files.
        /// This includes margin/padding adjustments and SplitContainer configurations.
        /// </summary>
        /// <param name="userControl">The user control to apply layout adjustments to</param>
        public static void ApplyRuntimeLayoutAdjustments(UserControl userControl)
        {
            try
            {
                userControl.SuspendLayout();
                ApplyLayoutAdjustmentsToControlHierarchy(userControl.Controls);
                userControl.ResumeLayout();

                LoggingUtility.Log($"Runtime layout adjustments applied to user control '{userControl.Name}'.");
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
            }
        }

        /// <summary>
        /// Handles DPI changes at runtime. Call this when the application detects a DPI change
        /// or when a form is moved between monitors with different DPI settings.
        /// </summary>
        /// <param name="form">The form that experienced a DPI change</param>
        /// <param name="oldDpi">The old DPI value</param>
        /// <param name="newDpi">The new DPI value</param>
        public static void HandleDpiChanged(Form form, int oldDpi, int newDpi)
        {
            try
            {
                float scaleFactor = (float)newDpi / oldDpi;
                LoggingUtility.Log(
                    $"DPI changed from {oldDpi} to {newDpi} (scale factor: {scaleFactor:F2}) for form '{form.Name}'");

                form.SuspendLayout();

                // Reapply DPI scaling and layout adjustments with new DPI
                ApplyDpiScaling(form);
                ApplyRuntimeLayoutAdjustments(form);

                // Recursively handle DPI changes for all user controls
                HandleDpiChangedForControlHierarchy(form.Controls, scaleFactor);

                form.ResumeLayout();

                LoggingUtility.Log($"DPI change handling completed for form '{form.Name}'");
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
            }
        }

        /// <summary>
        /// Provides a way to manually trigger DPI scaling refresh for all open forms.
        /// Useful for troubleshooting DPI scaling issues.
        /// </summary>
        public static void RefreshDpiScalingForAllForms()
        {
            try
            {
                foreach (Form form in Application.OpenForms)
                {
                    if (form.IsDisposed || !form.Visible)
                    {
                        continue;
                    }

                    ApplyDpiScaling(form);
                    ApplyRuntimeLayoutAdjustments(form);
                }

                LoggingUtility.Log("DPI scaling refreshed for all open forms");
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
            }
        }

        #endregion

        #region DPI Scaling and Layout Helpers

        /// <summary>
        /// Recursively applies DPI scaling to a control hierarchy.
        /// Sets AutoScaleMode = Dpi on user controls and forms.
        /// </summary>
        private static void ApplyDpiScalingToControlHierarchy(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                try
                {
                    // Recursively apply to child controls
                    if (control.HasChildren && control.Controls.Count > 0)
                    {
                        ApplyDpiScalingToControlHierarchy(control.Controls);
                    }
                }
                catch (Exception ex)
                {
                    LoggingUtility.LogApplicationError(ex);
                }
            }
        }

        /// <summary>
        /// Recursively applies layout adjustments to a control hierarchy.
        /// Handles margins, padding, and SplitContainer configurations for optimal DPI scaling.
        /// </summary>
        private static void ApplyLayoutAdjustmentsToControlHierarchy(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                try
                {
                    ApplyControlSpecificLayoutAdjustments(control);

                    // Recursively apply to child controls
                    if (control.HasChildren && control.Controls.Count > 0)
                    {
                        ApplyLayoutAdjustmentsToControlHierarchy(control.Controls);
                    }
                }
                catch (Exception ex)
                {
                    LoggingUtility.LogApplicationError(ex);
                }
            }
        }

        /// <summary>
        /// Applies control-specific layout adjustments for optimal DPI scaling.
        /// This method handles TableLayoutPanel, GroupBox, Panel, and SplitContainer configurations.
        /// </summary>
        private static void ApplyControlSpecificLayoutAdjustments(Control control)
        {
            switch (control)
            {
                case TableLayoutPanel tlp:
                    // Runtime adjustment: Set minimal margins and padding for TableLayoutPanel
                    tlp.Margin = new Padding(0);
                    tlp.Padding = new Padding(1); // Minimum padding for proper appearance
                    LoggingUtility.Log($"Applied TableLayoutPanel layout adjustments to '{tlp.Name}'");
                    break;

                case GroupBox groupBox:
                    // Runtime adjustment: Set minimal margins and padding for GroupBox
                    groupBox.Margin = new Padding(1);
                    groupBox.Padding = new Padding(3); // Slightly larger padding for groupbox appearance
                    LoggingUtility.Log($"Applied GroupBox layout adjustments to '{groupBox.Name}'");
                    break;

                case Panel panel:
                    // Runtime adjustment: Set minimal margins for Panel
                    panel.Margin = new Padding(0);
                    LoggingUtility.Log($"Applied Panel layout adjustments to '{panel.Name}'");
                    break;

                case SplitContainer splitContainer:
                    // Runtime adjustment: Configure SplitContainer for proper DPI scaling
                    ApplySplitContainerLayoutAdjustments(splitContainer);
                    break;

                case Button button:
                    // Runtime adjustment: Ensure buttons have minimal margins for proper scaling
                    button.Margin = new Padding(1);
                    break;

                case Label label:
                    // Runtime adjustment: Ensure labels have minimal margins for proper scaling
                    label.Margin = new Padding(0);
                    break;
            }
        }

        /// <summary>
        /// Applies specific layout adjustments to SplitContainer controls for optimal DPI scaling.
        /// This method configures splitter distances and ensures panels remain flush and aligned.
        /// </summary>
        private static void ApplySplitContainerLayoutAdjustments(SplitContainer splitContainer)
        {
            try
            {
                // Calculate valid range for SplitterDistance
                int min = splitContainer.Panel1MinSize;
                int max = splitContainer.Width - splitContainer.Panel2MinSize;
                if (max < min)
                {
                    max = min; // Prevent invalid range
                }

                // Only set SplitterDistance if the SplitContainer is large enough
                if (splitContainer.Width > splitContainer.Panel1MinSize + splitContainer.Panel2MinSize)
                {
                    int targetDistance;
                    // Custom logic for Control_TransferTab_SplitContainer_Main: left 20%, right 80%
                    if (splitContainer.Name == "Control_TransferTab_SplitContainer_Main" &&
                        splitContainer.Orientation == Orientation.Vertical && splitContainer.Width > 0)
                    {
                        targetDistance = (int)(splitContainer.Width * 0.35);
                    }
                    else if (splitContainer.Name == "SettingsForm_SplitContainer_Main" &&
                             splitContainer.Orientation == Orientation.Vertical && splitContainer.Width > 0)
                    {
                        targetDistance = (int)(splitContainer.Width * 0.15);
                    }
                    else if (splitContainer.Name == "MainForm_SplitContainer_Middle" &&
                             splitContainer.Orientation == Orientation.Vertical && splitContainer.Width > 0)
                    {
                        targetDistance = (int)(splitContainer.Width * 0.8);
                    }
                    else if (splitContainer.Orientation == Orientation.Vertical)
                    {
                        targetDistance = splitContainer.Width / 2;
                    }
                    else
                    {
                        targetDistance = splitContainer.Height / 2;
                    }

                    targetDistance = Math.Max(min, Math.Min(targetDistance, max));
                    if (splitContainer.SplitterDistance != targetDistance)
                    {
                        splitContainer.SplitterDistance = targetDistance;
                    }
                }

                // Ensure both panels have minimal margins
                splitContainer.Panel1.Margin = new Padding(0);
                splitContainer.Panel2.Margin = new Padding(0);
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
            }
        }

        /// <summary>
        /// Gets the current DPI scale factor compared to 96 DPI (100% scaling).
        /// </summary>
        /// <returns>DPI scale factor (1.0 = 100%, 1.25 = 125%, 1.5 = 150%, 2.0 = 200%)</returns>
        private static float GetCurrentDpiScale()
        {
            try
            {
                // Get the current DPI from the primary screen
                using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
                {
                    float dpiX = graphics.DpiX;
                    return dpiX / 96f; // 96 DPI is 100% scaling
                }
            }
            catch
            {
                return 1.0f; // Default to 100% if unable to get DPI
            }
        }

        /// <summary>
        /// Recursively handles DPI changes for a control hierarchy.
        /// </summary>
        private static void HandleDpiChangedForControlHierarchy(Control.ControlCollection controls, float scaleFactor)
        {
            foreach (Control control in controls)
            {
                try
                {
                    // Apply DPI scaling to user controls
                    if (control is UserControl userControl)
                    {
                        ApplyDpiScaling(userControl);
                        ApplyRuntimeLayoutAdjustments(userControl);
                    }

                    // Apply specific adjustments to important controls
                    ApplyControlSpecificLayoutAdjustments(control);

                    // Recursively handle child controls
                    if (control.HasChildren && control.Controls.Count > 0)
                    {
                        HandleDpiChangedForControlHierarchy(control.Controls, scaleFactor);
                    }
                }
                catch (Exception ex)
                {
                    LoggingUtility.LogApplicationError(ex);
                }
            }
        }

        // Helper to recursively calculate the required size for a control and all its children
        private static Size GetDeepestRequiredSize(Control control)
        {
            if (!control.Visible)
            {
                return Size.Empty;
            }

            if (control.HasChildren && control.Controls.Count > 0)
            {
                int maxRight = 0, maxBottom = 0;
                foreach (Control child in control.Controls)
                {
                    if (!child.Visible)
                    {
                        continue;
                    }

                    Size childRequired = GetDeepestRequiredSize(child);
                    int childRight = child.Left + childRequired.Width + child.Margin.Right;
                    int childBottom = child.Top + childRequired.Height + child.Margin.Bottom;
                    if (childRight > maxRight)
                    {
                        maxRight = childRight;
                    }

                    if (childBottom > maxBottom)
                    {
                        maxBottom = childBottom;
                    }
                }

                // For containers, also consider their own padding
                maxRight += control.Padding.Right;
                maxBottom += control.Padding.Bottom;
                return new Size(maxRight, maxBottom);
            }
            else
            {
                return control.Size;
            }
        }

        #endregion

        /// <summary>
        /// Ensures that the entire parent chain (up to the top-level form) is resized to fully accommodate the given control.
        /// This is especially important after DPI scaling or when loading user controls at runtime.
        /// </summary>
        /// <param name="control">The control to start from (typically a user control being loaded)</param>
        public static void EnsureParentChainAccommodates(Control control)
        {
            try
            {
                // Get the current DPI scale (e.g., 1.0 for 100%, 1.25 for 125%)
                float scale = GetCurrentDpiScale();

                Control? current = control;
                while (current != null && current.Parent != null)
                {
                    Control parent = current.Parent;

                    // Use existing method to get required size
                    Size required = GetDeepestRequiredSize(current);

                    // Apply DPI scaling to required size
                    int scaledWidth = (int)Math.Ceiling(required.Width * scale);
                    int scaledHeight = (int)Math.Ceiling(required.Height * scale);

                    // Calculate the bounds of the control within its parent
                    int requiredRight = current.Left + scaledWidth + current.Margin.Right;
                    int requiredBottom = current.Top + scaledHeight + current.Margin.Bottom;

                    bool resizeNeeded = false;
                    int newWidth = parent.Width;
                    int newHeight = parent.Height;

                    if (requiredRight > parent.Width)
                    {
                        newWidth = requiredRight;
                        resizeNeeded = true;
                    }

                    if (requiredBottom > parent.Height)
                    {
                        newHeight = requiredBottom;
                        resizeNeeded = true;
                    }

                    if (resizeNeeded)
                    {
                        Debug.WriteLine(
                            $"[EnsureParentChainAccommodates] Resizing parent '{parent.Name}' from ({parent.Width}, {parent.Height}) to ({newWidth}, {newHeight})");
                        parent.Width = newWidth;
                        parent.Height = newHeight;

                        if (parent.MaximumSize.Width < newWidth || parent.MaximumSize.Height < newHeight)
                        {
                            parent.MaximumSize = new Size(
                                Math.Max(parent.MaximumSize.Width, newWidth),
                                Math.Max(parent.MaximumSize.Height, newHeight)
                            );
                            Debug.WriteLine(
                                $"[EnsureParentChainAccommodates] Updated parent.MaximumSize to ({parent.MaximumSize.Width}, {parent.MaximumSize.Height})");
                        }

                        parent.PerformLayout();
                        parent.Invalidate();
                    }
                    else
                    {
                        Debug.WriteLine(
                            $"[EnsureParentChainAccommodates] No resize needed for parent '{parent.Name}'.");
                    }

                    current = parent;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[EnsureParentChainAccommodates] Exception: {ex}");
                LoggingUtility.LogApplicationError(ex);
            }
        }

        #region Private Helpers

        private delegate void ControlThemeApplier(Control control, Model_UserUiColors colors);

        private static readonly ConcurrentDictionary<Type, ControlThemeApplier> ThemeAppliers = new()
        {
            [typeof(Button)] = ThemeAppliersInternal.ApplyButtonTheme,
            [typeof(TabControl)] = ThemeAppliersInternal.ApplyTabControlTheme,
            [typeof(TabPage)] = ThemeAppliersInternal.ApplyTabPageTheme,
            [typeof(TextBox)] = ThemeAppliersInternal.ApplyTextBoxTheme,
            [typeof(MaskedTextBox)] = ThemeAppliersInternal.ApplyMaskedTextBoxTheme,
            [typeof(RichTextBox)] = ThemeAppliersInternal.ApplyRichTextBoxTheme,
            [typeof(ComboBox)] = ThemeAppliersInternal.ApplyComboBoxTheme,
            [typeof(ListBox)] = ThemeAppliersInternal.ApplyListBoxTheme,
            [typeof(CheckedListBox)] = ThemeAppliersInternal.ApplyCheckedListBoxTheme,
            [typeof(Label)] = ThemeAppliersInternal.ApplyLabelTheme,
            [typeof(RadioButton)] = ThemeAppliersInternal.ApplyRadioButtonTheme,
            [typeof(CheckBox)] = ThemeAppliersInternal.ApplyCheckBoxTheme,
            [typeof(TreeView)] = ThemeAppliersInternal.ApplyTreeViewTheme,
            [typeof(ListView)] = ThemeAppliersInternal.ApplyListViewTheme,
            [typeof(MenuStrip)] = ThemeAppliersInternal.ApplyMenuStripTheme,
            [typeof(StatusStrip)] = ThemeAppliersInternal.ApplyStatusStripTheme,
            [typeof(ToolStrip)] = ThemeAppliersInternal.ApplyToolStripTheme,
            [typeof(GroupBox)] = ThemeAppliersInternal.ApplyGroupBoxTheme,
            [typeof(Panel)] = ThemeAppliersInternal.ApplyPanelTheme,
            [typeof(SplitContainer)] = ThemeAppliersInternal.ApplySplitContainerTheme,
            [typeof(FlowLayoutPanel)] = ThemeAppliersInternal.ApplyFlowLayoutPanelTheme,
            [typeof(TableLayoutPanel)] = ThemeAppliersInternal.ApplyTableLayoutPanelTheme,
            [typeof(DateTimePicker)] = ThemeAppliersInternal.ApplyDateTimePickerTheme,
            [typeof(MonthCalendar)] = ThemeAppliersInternal.ApplyMonthCalendarTheme,
            [typeof(NumericUpDown)] = ThemeAppliersInternal.ApplyNumericUpDownTheme,
            [typeof(TrackBar)] = ThemeAppliersInternal.ApplyTrackBarTheme,
            [typeof(ProgressBar)] = ThemeAppliersInternal.ApplyProgressBarTheme,
            [typeof(HScrollBar)] = ThemeAppliersInternal.ApplyHScrollBarTheme,
            [typeof(VScrollBar)] = ThemeAppliersInternal.ApplyVScrollBarTheme,
            [typeof(PictureBox)] = ThemeAppliersInternal.ApplyPictureBoxTheme,
            [typeof(PropertyGrid)] = ThemeAppliersInternal.ApplyPropertyGridTheme,
            [typeof(DomainUpDown)] = ThemeAppliersInternal.ApplyDomainUpDownTheme,
            [typeof(WebBrowser)] = ThemeAppliersInternal.ApplyWebBrowserTheme,
            [typeof(UserControl)] = ThemeAppliersInternal.ApplyUserControlTheme,
            [typeof(LinkLabel)] = ThemeAppliersInternal.ApplyLinkLabelTheme,
            [typeof(ContextMenuStrip)] = ThemeAppliersInternal.ApplyContextMenuTheme,
            [typeof(Control_QuickButtons)] = ThemeAppliersInternal.ApplyQuickButtonsTheme,
            [typeof(Control_AdvancedInventory)] = ThemeAppliersInternal.ApplyAdvancedInventoryTheme,
            [typeof(Control_ProgressBarUserControl)] = ThemeAppliersInternal.ApplyProgressBarUserControlTheme,
            [typeof(Control_AdvancedRemove)] = ThemeAppliersInternal.ApplyAdvancedRemoveTheme,
            [typeof(Control_ConnectionStrengthControl)] = ThemeAppliersInternal.ApplyConnectionStrengthTheme,
            [typeof(Control_InventoryTab)] = ThemeAppliersInternal.ApplyInventoryTabTheme,
            [typeof(Control_RemoveTab)] = ThemeAppliersInternal.ApplyRemoveTabTheme,
            [typeof(Control_TransferTab)] = ThemeAppliersInternal.ApplyTransferTabTheme,
            [typeof(Control_Remove_User)] = ThemeAppliersInternal.ApplyUserControlTheme,
            [typeof(Control_Edit_User)] = ThemeAppliersInternal.ApplyUserControlTheme,
            [typeof(Control_Add_User)] = ThemeAppliersInternal.ApplyUserControlTheme,
            [typeof(Control_Add_ItemType)] = ThemeAppliersInternal.ApplyUserControlTheme,
            [typeof(Control_Remove_ItemType)] = ThemeAppliersInternal.ApplyUserControlTheme,
            [typeof(Control_Add_Operation)] = ThemeAppliersInternal.ApplyUserControlTheme,
            [typeof(Control_Add_Location)] = ThemeAppliersInternal.ApplyUserControlTheme,
            [typeof(Control_Add_PartID)] = ThemeAppliersInternal.ApplyUserControlTheme,
            [typeof(Control_Edit_ItemType)] = ThemeAppliersInternal.ApplyUserControlTheme,
            [typeof(Control_Edit_Location)] = ThemeAppliersInternal.ApplyUserControlTheme,
            [typeof(Control_Edit_Operation)] = ThemeAppliersInternal.ApplyUserControlTheme,
            [typeof(Control_Edit_PartID)] = ThemeAppliersInternal.ApplyUserControlTheme,
            [typeof(Control_Remove_Location)] = ThemeAppliersInternal.ApplyUserControlTheme,
            [typeof(Control_Remove_Operation)] = ThemeAppliersInternal.ApplyUserControlTheme,
            [typeof(Control_Remove_PartID)] = ThemeAppliersInternal.ApplyUserControlTheme
        };

        private static void SetFormTheme(Form form, Core_AppThemes.AppTheme theme, string themeName)
        {
            form.BackColor = theme.Colors.FormBackColor ?? Color.White;
            form.ForeColor = theme.Colors.FormForeColor ?? Color.Black;
            form.Font = theme.FormFont ?? new Font(form.Font.Name, Model_AppVariables.ThemeFontSize, form.Font.Style);

            if (!string.IsNullOrWhiteSpace(themeName))
            {
                int idx = form.Text.LastIndexOf('[');
                if (idx > 0)
                {
                    form.Text = form.Text[..idx].TrimEnd();
                }

                string themeDisplay = $"Current Theme: [{themeName}]";
                if (!form.Text.Contains(themeDisplay))
                {
                    form.Text = @$"{form.Text} | {themeDisplay}";
                }
            }
        }

        private static void LogControlColor(Control ctrl, string colorType, string colorSource, Color colorValue)
        {
            string themeName = Core_AppThemes.GetEffectiveThemeName();

            Debug.WriteLine(
                $"[THEME] {ctrl.Name} ({ctrl.GetType().Name}) - {colorType}: {colorSource} = {colorValue} | Theme: {themeName}");
        }

        private static Color DimColor(Color color, double percent)
        {
            percent = Math.Clamp(percent, 0, 1);
            int r = (int)(color.R * (1 - percent));
            int g = (int)(color.G * (1 - percent));
            int b = (int)(color.B * (1 - percent));
            return Color.FromArgb(color.A, r, g, b);
        }

        private static Color MaybeDimIfDisabled(Control control, Color color) =>
            !control.Enabled ? DimColor(color, 0.25) : color;

        private static void AttachEnabledChangedHandler(Control control)
        {
            control.EnabledChanged -= Control_EnabledChanged_ThemeRefresh;
            control.EnabledChanged += Control_EnabledChanged_ThemeRefresh;
        }

        private static void Control_EnabledChanged_ThemeRefresh(object? sender, EventArgs e)
        {
            if (sender is Control ctrl)
            {
                Core_AppThemes.AppTheme theme = Core_AppThemes.GetCurrentTheme();
                ApplyBaseThemeColors(ctrl, theme);
                ApplyControlSpecificTheme(ctrl);
            }
        }

        private static void ApplyThemeToControls(Control.ControlCollection controls)
        {
            Core_AppThemes.AppTheme theme = Core_AppThemes.GetCurrentTheme();
            foreach (Control ctrl in controls)
            {
                try
                {
                    AttachEnabledChangedHandler(ctrl);
                    if (ctrl is DataGridView dgv)
                    {
                        ApplyThemeToDataGridView(dgv);
                        if (theme.Colors.DataGridBackColor.HasValue)
                        {
                            LogControlColor(dgv, "BackColor", "DataGridBackColor",
                                theme.Colors.DataGridBackColor.Value);
                        }

                        if (theme.Colors.DataGridForeColor.HasValue)
                        {
                            LogControlColor(dgv, "ForeColor", "DataGridForeColor",
                                theme.Colors.DataGridForeColor.Value);
                        }
                    }
                    else
                    {
                        Color backColor = theme.Colors.FormBackColor ?? Color.White;
                        Color foreColor = theme.Colors.FormForeColor ?? Color.Black;
                        LogControlColor(ctrl, "BackColor",
                            theme.Colors.FormBackColor.HasValue ? "FormBackColor" : "Default", backColor);
                        LogControlColor(ctrl, "ForeColor",
                            theme.Colors.FormForeColor.HasValue ? "FormForeColor" : "Default", foreColor);

                        ApplyBaseThemeColors(ctrl, theme);
                        ApplyControlSpecificTheme(ctrl);
                        FocusUtils.ApplyFocusEventHandling(ctrl, theme.Colors);
                    }

                    if (ctrl.HasChildren && ctrl.Controls.Count < 10000)
                    {
                        ApplyThemeToControls(ctrl.Controls);
                    }
                }
                catch (Exception ex)
                {
                    LoggingUtility.LogApplicationError(ex);
                }
            }
        }

        private static void ApplyBaseThemeColors(Control control, Core_AppThemes.AppTheme theme)
        {
            Color backColor = theme.Colors.FormBackColor ?? Color.White;
            Color foreColor = theme.Colors.FormForeColor ?? Color.Black;
            backColor = MaybeDimIfDisabled(control, backColor);
            foreColor = MaybeDimIfDisabled(control, foreColor);
            if (control.BackColor != backColor)
            {
                control.BackColor = backColor;
            }

            if (control.ForeColor != foreColor)
            {
                control.ForeColor = foreColor;
            }

            Font font = theme.FormFont ??
                        new Font(control.Font.Name, Model_AppVariables.ThemeFontSize, control.Font.Style);
            if (control.Font == null || control.Font.Size != font.Size || control.Font.Name != font.Name)
            {
                control.Font = font;
            }
        }

        private static void ApplyControlSpecificTheme(Control control)
        {
            if (control == null)
            {
                return;
            }

            Core_AppThemes.AppTheme theme = Core_AppThemes.GetCurrentTheme();
            Model_UserUiColors colors = theme.Colors;
            try
            {
                Type controlType = control.GetType();
                if (ThemeAppliers.TryGetValue(controlType, out ControlThemeApplier? applier))
                {
                    applier(control, colors);
                    return;
                }

                Type? currentType = controlType;
                while (currentType != null && currentType != typeof(object))
                {
                    if (ThemeAppliers.TryGetValue(currentType, out applier))
                    {
                        ThemeAppliers.TryAdd(controlType, applier);
                        applier(control, colors);
                        return;
                    }

                    currentType = currentType.BaseType;
                }

                ThemeAppliersInternal.ApplyCustomControlTheme(control, colors);
            }
            catch (Exception ex)
            {
                LoggingUtility.LogApplicationError(ex);
                ThemeAppliersInternal.ApplyCustomControlTheme(control, colors);
            }
        }

        #endregion

        #region Internal Classes

        private static class ThemeAppliersInternal
        {
            public static void ApplyQuickButtonsTheme(Control control, Model_UserUiColors colors)
            {
                if (control is Control_QuickButtons quickButtons)
                {
                    quickButtons.BackColor = colors.CustomControlBackColor ?? colors.ControlBackColor ?? Color.White;
                    quickButtons.ForeColor = colors.CustomControlForeColor ?? colors.ControlForeColor ?? Color.Black;
                    foreach (Button btn in Control_QuickButtons.quickButtons ?? Enumerable.Empty<Button>())
                    {
                        btn.BackColor = colors.ButtonBackColor ?? Color.White;
                        btn.ForeColor = colors.ButtonForeColor ?? Color.Black;
                        btn.FlatStyle = FlatStyle.Flat;
                        btn.FlatAppearance.BorderSize = 2;
                        btn.FlatAppearance.BorderColor = colors.ButtonBorderColor ?? SystemColors.ControlDark;
                        btn.FlatAppearance.MouseDownBackColor =
                            colors.ButtonPressedBackColor ?? SystemColors.ControlDark;
                        btn.FlatAppearance.MouseOverBackColor =
                            colors.ButtonHoverBackColor ?? SystemColors.ControlLight;
                        btn.Padding = new Padding(2);
                        btn.Paint -= AutoShrinkText_Paint;
                        btn.Paint += AutoShrinkText_Paint;

                        btn.MouseEnter -= Button_MouseEnter;
                        btn.MouseLeave -= Button_MouseLeave;
                        btn.MouseDown -= Button_MouseDown;
                        btn.MouseUp -= Button_MouseUp;

                        btn.MouseEnter += Button_MouseEnter;
                        btn.MouseLeave += Button_MouseLeave;
                        btn.MouseDown += Button_MouseDown;
                        btn.MouseUp += Button_MouseUp;

                        void Button_MouseEnter(object? sender, EventArgs e)
                        {
                            if (sender is Button b)
                            {
                                if (colors.ButtonHoverBackColor.HasValue)
                                {
                                    b.BackColor = colors.ButtonHoverBackColor.Value;
                                }

                                if (colors.ButtonHoverForeColor.HasValue)
                                {
                                    b.ForeColor = colors.ButtonHoverForeColor.Value;
                                }
                            }
                        }

                        void Button_MouseLeave(object? sender, EventArgs e)
                        {
                            if (sender is Button b)
                            {
                                b.BackColor = colors.ButtonBackColor ?? Color.White;
                                b.ForeColor = colors.ButtonForeColor ?? Color.Black;
                            }
                        }

                        void Button_MouseDown(object? sender, MouseEventArgs e)
                        {
                            if (sender is Button b)
                            {
                                if (colors.ButtonPressedBackColor.HasValue)
                                {
                                    b.BackColor = colors.ButtonPressedBackColor.Value;
                                }

                                if (colors.ButtonPressedForeColor.HasValue)
                                {
                                    b.ForeColor = colors.ButtonPressedForeColor.Value;
                                }
                            }
                        }

                        void Button_MouseUp(object? sender, MouseEventArgs e)
                        {
                            if (sender is Button b)
                            {
                                b.BackColor = colors.ButtonBackColor ?? Color.White;
                                b.ForeColor = colors.ButtonForeColor ?? Color.Black;
                            }
                        }
                    }
                }
            }

            public static void ApplyAdvancedInventoryTheme(Control control, Model_UserUiColors colors)
            {
                if (control is Control_AdvancedInventory advInv)
                {
                    advInv.BackColor = colors.CustomControlBackColor ?? colors.ControlBackColor ?? Color.White;
                    advInv.ForeColor = colors.CustomControlForeColor ?? colors.ControlForeColor ?? Color.Black;
                }
            }

            public static void ApplyAdvancedRemoveTheme(Control control, Model_UserUiColors colors)
            {
                if (control is Control_AdvancedRemove advRem)
                {
                    advRem.BackColor = colors.CustomControlBackColor ?? colors.ControlBackColor ?? Color.White;
                    advRem.ForeColor = colors.CustomControlForeColor ?? colors.ControlForeColor ?? Color.Black;
                }
            }

            public static void ApplyConnectionStrengthTheme(Control control, Model_UserUiColors colors)
            {
                if (control is Control_ConnectionStrengthControl conn)
                {
                    conn.BackColor = colors.CustomControlBackColor ?? colors.ControlBackColor ?? Color.White;
                    conn.ForeColor = colors.CustomControlForeColor ?? colors.ControlForeColor ?? Color.Black;
                    conn.Invalidate();
                }
            }

            public static void ApplyInventoryTabTheme(Control control, Model_UserUiColors colors)
            {
                if (control is Control_InventoryTab tab)
                {
                    tab.BackColor = colors.CustomControlBackColor ?? colors.ControlBackColor ?? Color.White;
                    tab.ForeColor = colors.CustomControlForeColor ?? colors.ControlForeColor ?? Color.Black;
                }
            }

            public static void ApplyRemoveTabTheme(Control control, Model_UserUiColors colors)
            {
                if (control is Control_RemoveTab tab)
                {
                    tab.BackColor = colors.CustomControlBackColor ?? colors.ControlBackColor ?? Color.White;
                    tab.ForeColor = colors.CustomControlForeColor ?? colors.ControlForeColor ?? Color.Black;
                }
            }

            public static void ApplyTransferTabTheme(Control control, Model_UserUiColors colors)
            {
                if (control is Control_TransferTab tab)
                {
                    tab.BackColor = colors.CustomControlBackColor ?? colors.ControlBackColor ?? Color.White;
                    tab.ForeColor = colors.CustomControlForeColor ?? colors.ControlForeColor ?? Color.Black;
                }
            }

            public static void ApplyCustomControlTheme(Control control, Model_UserUiColors colors)
            {
                if (colors.CustomControlBackColor.HasValue)
                {
                    control.BackColor = colors.CustomControlBackColor.Value;
                }

                if (colors.CustomControlForeColor.HasValue)
                {
                    control.ForeColor = colors.CustomControlForeColor.Value;
                }
            }

            private static void ApplyOwnerDrawThemes(Control control, Model_UserUiColors colors) =>
                OwnerDrawThemeHelper.ApplyOwnerDrawTheme(control, colors);

            public static void ApplyButtonTheme(Control control, Model_UserUiColors colors)
            {
                if (control is Button btn)
                {
                    // Skip all visual logic if this button is inside Control_QuickButtons
                    if (btn.Parent is Control_QuickButtons)
                    {
                        return;
                    }

                    btn.Margin = new Padding(1);
                    btn.Paint -= AutoShrinkText_Paint;
                    btn.Paint += AutoShrinkText_Paint;
                    Color backColor = colors.ButtonBackColor ?? SystemColors.Control;
                    Color foreColor = colors.ButtonForeColor ?? SystemColors.ControlText;
                    btn.BackColor = backColor;
                    btn.ForeColor = foreColor;
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 2;
                    btn.FlatAppearance.BorderColor = colors.ButtonBorderColor ?? SystemColors.ControlDark;
                    btn.FlatAppearance.MouseDownBackColor = colors.ButtonPressedBackColor ?? SystemColors.ControlDark;
                    btn.FlatAppearance.MouseOverBackColor = colors.ButtonHoverBackColor ?? SystemColors.ControlLight;

                    btn.MouseEnter -= Button_MouseEnter;
                    btn.MouseLeave -= Button_MouseLeave;
                    btn.MouseDown -= Button_MouseDown;
                    btn.MouseUp -= Button_MouseUp;

                    btn.MouseEnter += Button_MouseEnter;
                    btn.MouseLeave += Button_MouseLeave;
                    btn.MouseDown += Button_MouseDown;
                    btn.MouseUp += Button_MouseUp;

                    void Button_MouseEnter(object? sender, EventArgs e)
                    {
                        if (sender is Button b)
                        {
                            if (colors.ButtonHoverBackColor.HasValue)
                            {
                                b.BackColor = colors.ButtonHoverBackColor.Value;
                            }

                            if (colors.ButtonHoverForeColor.HasValue)
                            {
                                b.ForeColor = colors.ButtonHoverForeColor.Value;
                            }
                        }
                    }

                    void Button_MouseLeave(object? sender, EventArgs e)
                    {
                        if (sender is Button b)
                        {
                            b.BackColor = colors.ButtonBackColor ?? SystemColors.Control;
                            b.ForeColor = colors.ButtonForeColor ?? SystemColors.ControlText;
                        }
                    }

                    void Button_MouseDown(object? sender, MouseEventArgs e)
                    {
                        if (sender is Button b)
                        {
                            if (colors.ButtonPressedBackColor.HasValue)
                            {
                                b.BackColor = colors.ButtonPressedBackColor.Value;
                            }

                            if (colors.ButtonPressedForeColor.HasValue)
                            {
                                b.ForeColor = colors.ButtonPressedForeColor.Value;
                            }
                        }
                    }

                    void Button_MouseUp(object? sender, MouseEventArgs e)
                    {
                        if (sender is Button b)
                        {
                            b.BackColor = colors.ButtonBackColor ?? SystemColors.Control;
                            b.ForeColor = colors.ButtonForeColor ?? SystemColors.ControlText;
                        }
                    }
                }
            }

            public static void ApplyTabControlTheme(Control control, Model_UserUiColors colors)
            {
                if (control is TabControl tab)
                {
                    Color backColor = colors.TabControlBackColor ?? colors.FormBackColor ?? Color.White;
                    Color foreColor = colors.TabControlForeColor ?? colors.FormForeColor ?? Color.Black;
                    tab.BackColor = backColor;
                    tab.ForeColor = foreColor;
                    tab.Paint -= AutoShrinkText_Paint;
                    tab.Paint += AutoShrinkText_Paint;
                }
            }

            public static void ApplyTabPageTheme(Control control, Model_UserUiColors colors)
            {
                if (control is TabPage tabPage)
                {
                    if (colors.TabPageBackColor.HasValue)
                    {
                        tabPage.BackColor = colors.TabPageBackColor.Value;
                    }

                    if (colors.TabPageForeColor.HasValue)
                    {
                        tabPage.ForeColor = colors.TabPageForeColor.Value;
                    }

                    tabPage.Paint -= AutoShrinkText_Paint;
                    tabPage.Paint += AutoShrinkText_Paint;

                    ApplyOwnerDrawThemes(tabPage, colors);
                }
            }

            public static void ApplyTextBoxTheme(Control control, Model_UserUiColors colors)
            {
                if (control is TextBox txt)
                {
                    if (colors.TextBoxBackColor.HasValue)
                    {
                        txt.BackColor = colors.TextBoxBackColor.Value;
                    }

                    if (control.Text.Contains("["))
                    {
                        if (colors.TextBoxErrorForeColor.HasValue)
                        {
                            txt.ForeColor = colors.TextBoxErrorForeColor.Value;
                        }
                    }
                    else if (colors.TextBoxForeColor.HasValue)
                    {
                        txt.ForeColor = colors.TextBoxForeColor.Value;
                    }


                    ApplyOwnerDrawThemes(txt, colors);
                }
            }

            public static void ApplyMaskedTextBoxTheme(Control control, Model_UserUiColors colors)
            {
                if (control is MaskedTextBox mtxt)
                {
                    if (colors.MaskedTextBoxBackColor.HasValue)
                    {
                        mtxt.BackColor = colors.MaskedTextBoxBackColor.Value;
                    }

                    if (colors.MaskedTextBoxErrorForeColor.HasValue)
                    {
                        mtxt.ForeColor = colors.MaskedTextBoxErrorForeColor.Value;
                    }

                    ApplyOwnerDrawThemes(mtxt, colors);
                }
            }

            public static void ApplyRichTextBoxTheme(Control control, Model_UserUiColors colors)
            {
                if (control is RichTextBox rtxt)
                {
                    if (colors.RichTextBoxBackColor.HasValue)
                    {
                        rtxt.BackColor = colors.RichTextBoxBackColor.Value;
                    }

                    if (colors.RichTextBoxErrorForeColor.HasValue)
                    {
                        rtxt.ForeColor = colors.RichTextBoxErrorForeColor.Value;
                    }

                    ApplyOwnerDrawThemes(rtxt, colors);
                }
            }

            public static void ApplyComboBoxTheme(Control control, Model_UserUiColors colors)
            {
                if (control is ComboBox cb)
                {
                    // Set theme colors
                    if (colors.ComboBoxBackColor.HasValue)
                    {
                        cb.BackColor = colors.ComboBoxBackColor.Value;
                    }

                    if (colors.ComboBoxForeColor.HasValue)
                    {
                        cb.ForeColor = colors.ComboBoxForeColor.Value;
                    }

                    if (control.Text.Contains("["))
                    {
                        if (colors.ComboBoxErrorForeColor.HasValue)
                        {
                            control.ForeColor = colors.ComboBoxErrorForeColor.Value;
                        }
                    }
                    else if (colors.ComboBoxForeColor.HasValue)
                    {
                        control.ForeColor = colors.ComboBoxForeColor.Value;
                    }

                    // Set standard ComboBox properties to match Control_InventoryTab_ComboBox_Part
                    cb.DropDownStyle = ComboBoxStyle.DropDown;
                    cb.AutoCompleteMode = AutoCompleteMode.Suggest;
                    cb.AutoCompleteSource = AutoCompleteSource.ListItems;
                    cb.MaxDropDownItems = 6;


                    // Remove previous event to avoid multiple subscriptions
                    cb.SelectedIndexChanged -= ComboBox_Theme_SelectedIndexChanged;
                    cb.SelectedIndexChanged += ComboBox_Theme_SelectedIndexChanged;

                    ApplyOwnerDrawThemes(cb, colors);
                }
            }

            // Event handler for ComboBox index change to set error color if needed
            private static void ComboBox_Theme_SelectedIndexChanged(object? sender, EventArgs? e)
            {
                if (sender is ComboBox cb)
                {
                    // If the first item is a placeholder like [ Enter ... ]
                    if (cb.Text.Contains("["))
                    {
                        cb.ForeColor = Model_AppVariables.UserUiColors.ComboBoxErrorForeColor ?? Color.Red;
                    }
                    else
                    {
                        cb.ForeColor = Model_AppVariables.UserUiColors.ComboBoxForeColor ?? Color.Black;
                    }
                }
            }

            public static void ApplyListBoxTheme(Control control, Model_UserUiColors colors)
            {
                if (control is ListBox lb)
                {
                    if (colors.ListBoxBackColor.HasValue)
                    {
                        lb.BackColor = colors.ListBoxBackColor.Value;
                    }

                    if (colors.ListBoxForeColor.HasValue)
                    {
                        lb.ForeColor = colors.ListBoxForeColor.Value;
                    }

                    ApplyOwnerDrawThemes(lb, colors);
                }
            }

            public static void ApplyCheckedListBoxTheme(Control control, Model_UserUiColors colors)
            {
                if (control is CheckedListBox clb)
                {
                    if (colors.CheckedListBoxBackColor.HasValue)
                    {
                        clb.BackColor = colors.CheckedListBoxBackColor.Value;
                    }

                    if (colors.CheckedListBoxForeColor.HasValue)
                    {
                        clb.ForeColor = colors.CheckedListBoxForeColor.Value;
                    }

                    ApplyOwnerDrawThemes(clb, colors);
                }
            }

            public static void ApplyLabelTheme(Control control, Model_UserUiColors colors)
            {
                if (control is Label lbl)
                {
                    if (colors.LabelBackColor.HasValue)
                    {
                        lbl.BackColor = colors.LabelBackColor.Value;
                    }

                    if (colors.LabelForeColor.HasValue)
                    {
                        lbl.ForeColor = colors.LabelForeColor.Value;
                    }

                    lbl.Paint -= AutoShrinkText_Paint;
                    lbl.Paint += AutoShrinkText_Paint;
                }
            }

            public static void ApplyRadioButtonTheme(Control control, Model_UserUiColors colors)
            {
                if (control is RadioButton rb)
                {
                    if (colors.RadioButtonBackColor.HasValue)
                    {
                        rb.BackColor = colors.RadioButtonBackColor.Value;
                    }

                    if (colors.RadioButtonForeColor.HasValue)
                    {
                        rb.ForeColor = colors.RadioButtonForeColor.Value;
                    }
                }
            }

            public static void ApplyCheckBoxTheme(Control control, Model_UserUiColors colors)
            {
                if (control is CheckBox cbx)
                {
                    if (colors.CheckBoxBackColor.HasValue)
                    {
                        cbx.BackColor = colors.CheckBoxBackColor.Value;
                    }

                    if (colors.CheckBoxForeColor.HasValue)
                    {
                        cbx.ForeColor = colors.CheckBoxForeColor.Value;
                    }
                }
            }

            public static void ApplyTreeViewTheme(Control control, Model_UserUiColors colors)
            {
                if (control is TreeView tv)
                {
                    if (colors.TreeViewBackColor.HasValue)
                    {
                        tv.BackColor = colors.TreeViewBackColor.Value;
                    }

                    if (colors.TreeViewForeColor.HasValue)
                    {
                        tv.ForeColor = colors.TreeViewForeColor.Value;
                    }

                    if (colors.TreeViewLineColor.HasValue)
                    {
                        tv.LineColor = colors.TreeViewLineColor.Value;
                    }

                    ApplyOwnerDrawThemes(tv, colors);
                }
            }

            public static void ApplyListViewTheme(Control control, Model_UserUiColors colors)
            {
                if (control is ListView lv)
                {
                    if (colors.ListViewBackColor.HasValue)
                    {
                        lv.BackColor = colors.ListViewBackColor.Value;
                    }

                    if (colors.ListViewForeColor.HasValue)
                    {
                        lv.ForeColor = colors.ListViewForeColor.Value;
                    }

                    ApplyOwnerDrawThemes(lv, colors);
                }
            }

            public static void ApplyMenuStripTheme(Control control, Model_UserUiColors colors)
            {
                if (control is MenuStrip ms)
                {
                    if (colors.MenuStripBackColor.HasValue)
                    {
                        ms.BackColor = colors.MenuStripBackColor.Value;
                    }

                    if (colors.MenuStripForeColor.HasValue)
                    {
                        ms.ForeColor = colors.MenuStripForeColor.Value;
                    }

                    ApplyOwnerDrawThemes(ms, colors);
                }
            }

            public static void ApplyStatusStripTheme(Control control, Model_UserUiColors colors)
            {
                if (control is StatusStrip ss)
                {
                    Color formBackColor = Core_AppThemes.GetCurrentTheme().Colors.FormBackColor ?? Color.White;
                    ss.BackColor = formBackColor;
                    if (colors.StatusStripForeColor.HasValue)
                    {
                        ss.ForeColor = colors.StatusStripForeColor.Value;
                    }

                    ApplyOwnerDrawThemes(ss, colors);
                }
            }

            public static void ApplyToolStripTheme(Control control, Model_UserUiColors colors)
            {
                if (control is ToolStrip ts)
                {
                    if (colors.ToolStripBackColor.HasValue)
                    {
                        ts.BackColor = colors.ToolStripBackColor.Value;
                    }

                    if (colors.ToolStripForeColor.HasValue)
                    {
                        ts.ForeColor = colors.ToolStripForeColor.Value;
                    }

                    ApplyOwnerDrawThemes(ts, colors);
                }
            }

            public static void ApplyGroupBoxTheme(Control control, Model_UserUiColors colors)
            {
                if (control is GroupBox gb)
                {
                    if (colors.GroupBoxBackColor.HasValue)
                    {
                        gb.BackColor = colors.GroupBoxBackColor.Value;
                    }

                    if (colors.GroupBoxForeColor.HasValue)
                    {
                        gb.ForeColor = colors.GroupBoxForeColor.Value;
                    }

                    gb.Paint -= AutoShrinkText_Paint;
                    gb.Paint += AutoShrinkText_Paint;

                    ApplyOwnerDrawThemes(gb, colors);
                }
            }

            public static void ApplyPanelTheme(Control control, Model_UserUiColors colors)
            {
                if (control is Panel pnl)
                {
                    if (colors.PanelForeColor.HasValue)
                    {
                        pnl.ForeColor = colors.PanelForeColor.Value;
                    }

                    ApplyOwnerDrawThemes(pnl, colors);
                }
            }

            public static void ApplySplitContainerTheme(Control control, Model_UserUiColors colors)
            {
                if (control is SplitContainer sc)
                {
                    if (colors.SplitContainerBackColor.HasValue)
                    {
                        sc.BackColor = colors.SplitContainerBackColor.Value;
                    }

                    if (colors.SplitContainerForeColor.HasValue)
                    {
                        sc.ForeColor = colors.SplitContainerForeColor.Value;
                    }

                    ApplyOwnerDrawThemes(sc, colors);
                }
            }

            public static void ApplyFlowLayoutPanelTheme(Control control, Model_UserUiColors colors)
            {
                if (control is FlowLayoutPanel flp)
                {
                    if (colors.FlowLayoutPanelForeColor.HasValue)
                    {
                        flp.ForeColor = colors.FlowLayoutPanelForeColor.Value;
                    }

                    ApplyOwnerDrawThemes(flp, colors);
                }
            }

            public static void ApplyTableLayoutPanelTheme(Control control, Model_UserUiColors colors)
            {
                if (control is TableLayoutPanel tlp)
                {
                    if (colors.TableLayoutPanelForeColor.HasValue)
                    {
                        tlp.ForeColor = colors.TableLayoutPanelForeColor.Value;
                    }

                    ApplyOwnerDrawThemes(tlp, colors);
                }
            }

            public static void ApplyDateTimePickerTheme(Control control, Model_UserUiColors colors)
            {
                if (control is DateTimePicker dtp)
                {
                    if (colors.DateTimePickerBackColor.HasValue)
                    {
                        dtp.BackColor = colors.DateTimePickerBackColor.Value;
                    }

                    if (colors.DateTimePickerForeColor.HasValue)
                    {
                        dtp.ForeColor = colors.DateTimePickerForeColor.Value;
                    }

                    ApplyOwnerDrawThemes(dtp, colors);
                }
            }

            public static void ApplyMonthCalendarTheme(Control control, Model_UserUiColors colors)
            {
                if (control is MonthCalendar mc)
                {
                    if (colors.MonthCalendarBackColor.HasValue)
                    {
                        mc.BackColor = colors.MonthCalendarBackColor.Value;
                    }

                    if (colors.MonthCalendarForeColor.HasValue)
                    {
                        mc.ForeColor = colors.MonthCalendarForeColor.Value;
                    }

                    if (colors.MonthCalendarTitleBackColor.HasValue)
                    {
                        mc.TitleBackColor = colors.MonthCalendarTitleBackColor.Value;
                    }

                    if (colors.MonthCalendarTitleForeColor.HasValue)
                    {
                        mc.TitleForeColor = colors.MonthCalendarTitleForeColor.Value;
                    }

                    if (colors.MonthCalendarTrailingForeColor.HasValue)
                    {
                        mc.TrailingForeColor = colors.MonthCalendarTrailingForeColor.Value;
                    }

                    ApplyOwnerDrawThemes(mc, colors);
                }
            }

            public static void ApplyNumericUpDownTheme(Control control, Model_UserUiColors colors)
            {
                if (control is NumericUpDown nud)
                {
                    if (colors.NumericUpDownBackColor.HasValue)
                    {
                        nud.BackColor = colors.NumericUpDownBackColor.Value;
                    }

                    if (colors.NumericUpDownForeColor.HasValue)
                    {
                        nud.ForeColor = colors.NumericUpDownForeColor.Value;
                    }

                    ApplyOwnerDrawThemes(nud, colors);
                }
            }

            public static void ApplyTrackBarTheme(Control control, Model_UserUiColors colors)
            {
                if (control is TrackBar tb)
                {
                    if (colors.TrackBarBackColor.HasValue)
                    {
                        tb.BackColor = colors.TrackBarBackColor.Value;
                    }

                    if (colors.TrackBarForeColor.HasValue)
                    {
                        tb.ForeColor = colors.TrackBarForeColor.Value;
                    }

                    ApplyOwnerDrawThemes(tb, colors);
                }
            }

            public static void ApplyProgressBarTheme(Control control, Model_UserUiColors colors)
            {
                if (control is ProgressBar pb)
                {
                    if (colors.ProgressBarBackColor.HasValue)
                    {
                        pb.BackColor = colors.ProgressBarBackColor.Value;
                    }

                    if (colors.ProgressBarForeColor.HasValue)
                    {
                        pb.ForeColor = colors.ProgressBarForeColor.Value;
                    }

                    ApplyOwnerDrawThemes(pb, colors);
                }
            }

            public static void ApplyProgressBarUserControlTheme(Control control, Model_UserUiColors colors)
            {
                if (control is Control_ProgressBarUserControl pbuc)
                {
                    if (colors.UserControlBackColor.HasValue)
                    {
                        pbuc.BackColor = colors.UserControlBackColor.Value;
                    }

                    if (colors.UserControlForeColor.HasValue)
                    {
                        pbuc.ForeColor = colors.UserControlForeColor.Value;
                    }

                    ApplyOwnerDrawThemes(pbuc, colors);
                }
            }

            public static void ApplyHScrollBarTheme(Control control, Model_UserUiColors colors)
            {
                if (control is HScrollBar hsb)
                {
                    if (colors.HScrollBarBackColor.HasValue)
                    {
                        hsb.BackColor = colors.HScrollBarBackColor.Value;
                    }

                    if (colors.HScrollBarForeColor.HasValue)
                    {
                        hsb.ForeColor = colors.HScrollBarForeColor.Value;
                    }

                    ApplyOwnerDrawThemes(hsb, colors);
                }
            }

            public static void ApplyVScrollBarTheme(Control control, Model_UserUiColors colors)
            {
                if (control is VScrollBar vsb)
                {
                    if (colors.VScrollBarBackColor.HasValue)
                    {
                        vsb.BackColor = colors.VScrollBarBackColor.Value;
                    }

                    if (colors.VScrollBarForeColor.HasValue)
                    {
                        vsb.ForeColor = colors.VScrollBarForeColor.Value;
                    }

                    ApplyOwnerDrawThemes(vsb, colors);
                }
            }

            public static void ApplyPictureBoxTheme(Control control, Model_UserUiColors colors)
            {
                if (control is PictureBox pic)
                {
                    if (colors.PictureBoxBackColor.HasValue)
                    {
                        pic.BackColor = colors.PictureBoxBackColor.Value;
                    }

                    ApplyOwnerDrawThemes(pic, colors);
                }
            }

            public static void ApplyPropertyGridTheme(Control control, Model_UserUiColors colors)
            {
                if (control is PropertyGrid pg)
                {
                    if (colors.PropertyGridBackColor.HasValue)
                    {
                        pg.BackColor = colors.PropertyGridBackColor.Value;
                    }

                    if (colors.PropertyGridForeColor.HasValue)
                    {
                        pg.ForeColor = colors.PropertyGridForeColor.Value;
                    }

                    ApplyOwnerDrawThemes(pg, colors);
                }
            }

            public static void ApplyDomainUpDownTheme(Control control, Model_UserUiColors colors)
            {
                if (control is DomainUpDown dud)
                {
                    if (colors.DomainUpDownBackColor.HasValue)
                    {
                        dud.BackColor = colors.DomainUpDownBackColor.Value;
                    }

                    if (colors.DomainUpDownForeColor.HasValue)
                    {
                        dud.ForeColor = colors.DomainUpDownForeColor.Value;
                    }

                    ApplyOwnerDrawThemes(dud, colors);
                }
            }

            public static void ApplyWebBrowserTheme(Control control, Model_UserUiColors colors)
            {
                if (control is WebBrowser wb)
                {
                    if (colors.WebBrowserBackColor.HasValue)
                    {
                        wb.BackColor = colors.WebBrowserBackColor.Value;
                    }

                    ApplyOwnerDrawThemes(wb, colors);
                }
            }

            public static void ApplyUserControlTheme(Control control, Model_UserUiColors colors)
            {
                if (control is UserControl uc)
                {
                    Color formBackColor = Core_AppThemes.GetCurrentTheme().Colors.FormBackColor ?? Color.White;
                    uc.BackColor = formBackColor;
                    if (colors.UserControlForeColor.HasValue)
                    {
                        uc.ForeColor = colors.UserControlForeColor.Value;
                    }

                    ApplyOwnerDrawThemes(uc, colors);
                }
            }

            public static void ApplyLinkLabelTheme(Control control, Model_UserUiColors colors)
            {
                if (control is LinkLabel ll)
                {
                    if (colors.LinkLabelLinkColor.HasValue)
                    {
                        ll.LinkColor = colors.LinkLabelLinkColor.Value;
                    }

                    if (colors.LinkLabelActiveLinkColor.HasValue)
                    {
                        ll.ActiveLinkColor = colors.LinkLabelActiveLinkColor.Value;
                    }

                    if (colors.LinkLabelVisitedLinkColor.HasValue)
                    {
                        ll.VisitedLinkColor = colors.LinkLabelVisitedLinkColor.Value;
                    }

                    if (colors.LinkLabelBackColor.HasValue)
                    {
                        ll.BackColor = colors.LinkLabelBackColor.Value;
                    }

                    if (colors.LinkLabelForeColor.HasValue)
                    {
                        ll.ForeColor = colors.LinkLabelForeColor.Value;
                    }

                    if (colors.LinkLabelHoverColor.HasValue)
                    {
                        OwnerDrawThemeHelper.AttachLinkLabelHoverColor(ll, colors.LinkLabelHoverColor.Value);
                    }
                }
            }

            public static void ApplyContextMenuTheme(Control control, Model_UserUiColors colors)
            {
                if (control is ContextMenuStrip cms)
                {
                    if (colors.ContextMenuBackColor.HasValue)
                    {
                        cms.BackColor = colors.ContextMenuBackColor.Value;
                    }

                    if (colors.ContextMenuForeColor.HasValue)
                    {
                        cms.ForeColor = colors.ContextMenuForeColor.Value;
                    }

                    ApplyOwnerDrawThemes(cms, colors);
                }
            }

            public static void ApplyThemeToDataGridView(DataGridView dataGridView)
            {
                if (dataGridView == null)
                {
                    return;
                }

                Model_UserUiColors colors = Core_AppThemes.GetCurrentTheme().Colors;

                if (colors.DataGridBackColor.HasValue)
                {
                    dataGridView.BackgroundColor = colors.DataGridBackColor.Value;
                }

                if (colors.DataGridForeColor.HasValue)
                {
                    dataGridView.ForeColor = colors.DataGridForeColor.Value;
                }

                if (dataGridView.ColumnHeadersDefaultCellStyle != null)
                {
                    if (colors.DataGridHeaderBackColor.HasValue)
                    {
                        dataGridView.ColumnHeadersDefaultCellStyle.BackColor = colors.DataGridHeaderBackColor.Value;
                    }

                    if (colors.DataGridHeaderForeColor.HasValue)
                    {
                        dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = colors.DataGridHeaderForeColor.Value;
                    }
                }

                if (dataGridView.RowsDefaultCellStyle != null)
                {
                    if (colors.DataGridRowBackColor.HasValue)
                    {
                        dataGridView.RowsDefaultCellStyle.BackColor = colors.DataGridRowBackColor.Value;
                    }

                    if (colors.DataGridForeColor.HasValue)
                    {
                        dataGridView.RowsDefaultCellStyle.ForeColor = colors.DataGridForeColor.Value;
                    }
                }

                if (dataGridView.AlternatingRowsDefaultCellStyle != null)
                {
                    if (colors.DataGridAltRowBackColor.HasValue)
                    {
                        dataGridView.AlternatingRowsDefaultCellStyle.BackColor = colors.DataGridAltRowBackColor.Value;
                    }

                    if (colors.DataGridForeColor.HasValue)
                    {
                        dataGridView.AlternatingRowsDefaultCellStyle.ForeColor = colors.DataGridForeColor.Value;
                    }
                }

                if (colors.DataGridGridColor.HasValue)
                {
                    dataGridView.GridColor = colors.DataGridGridColor.Value;
                }

                if (colors.DataGridSelectionBackColor.HasValue)
                {
                    dataGridView.DefaultCellStyle.SelectionBackColor = colors.DataGridSelectionBackColor.Value;
                }

                if (colors.DataGridSelectionForeColor.HasValue)
                {
                    dataGridView.DefaultCellStyle.SelectionForeColor = colors.DataGridSelectionForeColor.Value;
                }

                if (colors.DataGridBorderColor.HasValue)
                {
                    OwnerDrawThemeHelper.ApplyDataGridViewBorderColor(dataGridView, colors.DataGridBorderColor.Value);
                }
            }

            public static void SizeDataGrid(DataGridView dataGridView)
            {
                if (dataGridView == null)
                {
                    throw new ArgumentNullException(nameof(dataGridView));
                }

                dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
                dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                dataGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;

                int[] preferredWidths = new int[dataGridView.Columns.Count];
                int totalPreferredWidth = 0;
                for (int i = 0; i < dataGridView.Columns.Count; i++)
                {
                    preferredWidths[i] = dataGridView.Columns[i].Width;
                    totalPreferredWidth += preferredWidths[i];
                }

                dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                for (int i = 0; i < dataGridView.Columns.Count; i++)
                {
                    if (totalPreferredWidth > 0)
                    {
                        dataGridView.Columns[i].FillWeight = (float)preferredWidths[i] / totalPreferredWidth * 100f;
                    }
                    else
                    {
                        dataGridView.Columns[i].FillWeight = 100f / dataGridView.Columns.Count;
                    }
                }
            }

            private static void AutoShrinkText_Paint(object? sender, PaintEventArgs e)
            {
                if (sender is not Control control)
                {
                    return;
                }

                Color backColor = MaybeDimIfDisabled(control, control.BackColor);
                Color foreColor = MaybeDimIfDisabled(control, control.ForeColor);

                e.Graphics.Clear(backColor);

                string text = control.Text;
                Font font = control.Font;
                Rectangle clientRectangle = control.ClientRectangle;

                StringFormat format = new() { Trimming = StringTrimming.EllipsisCharacter };

                if (control is Label label)
                {
                    ContentAlignment align = label.TextAlign;
                    switch (align)
                    {
                        case ContentAlignment.TopLeft:
                            format.Alignment = StringAlignment.Near;
                            format.LineAlignment = StringAlignment.Near;
                            break;
                        case ContentAlignment.TopCenter:
                            format.Alignment = StringAlignment.Center;
                            format.LineAlignment = StringAlignment.Near;
                            break;
                        case ContentAlignment.TopRight:
                            format.Alignment = StringAlignment.Far;
                            format.LineAlignment = StringAlignment.Near;
                            break;
                        case ContentAlignment.MiddleLeft:
                            format.Alignment = StringAlignment.Near;
                            format.LineAlignment = StringAlignment.Center;
                            break;
                        case ContentAlignment.MiddleCenter:
                            format.Alignment = StringAlignment.Center;
                            format.LineAlignment = StringAlignment.Center;
                            break;
                        case ContentAlignment.MiddleRight:
                            format.Alignment = StringAlignment.Far;
                            format.LineAlignment = StringAlignment.Center;
                            break;
                        case ContentAlignment.BottomLeft:
                            format.Alignment = StringAlignment.Near;
                            format.LineAlignment = StringAlignment.Far;
                            break;
                        case ContentAlignment.BottomCenter:
                            format.Alignment = StringAlignment.Center;
                            format.LineAlignment = StringAlignment.Far;
                            break;
                        case ContentAlignment.BottomRight:
                            format.Alignment = StringAlignment.Far;
                            format.LineAlignment = StringAlignment.Far;
                            break;
                    }
                }
                else if (control is Button btn)
                {
                    // If the button's parent is Control_QuickButtons, force CenterRight alignment
                    if (btn.Parent is Control_QuickButtons)
                    {
                        Debug.Write("QuickButton Set");
                        format.Alignment = StringAlignment.Far;
                        format.LineAlignment = StringAlignment.Center;
                    }
                    else
                    {
                        ContentAlignment align = btn.TextAlign;
                        switch (align)
                        {
                            case ContentAlignment.TopLeft:
                                format.Alignment = StringAlignment.Near;
                                format.LineAlignment = StringAlignment.Near;
                                break;
                            case ContentAlignment.TopCenter:
                                format.Alignment = StringAlignment.Center;
                                format.LineAlignment = StringAlignment.Near;
                                break;
                            case ContentAlignment.TopRight:
                                format.Alignment = StringAlignment.Far;
                                format.LineAlignment = StringAlignment.Near;
                                break;
                            case ContentAlignment.MiddleLeft:
                                format.Alignment = StringAlignment.Near;
                                format.LineAlignment = StringAlignment.Center;
                                break;
                            case ContentAlignment.MiddleCenter:
                                format.Alignment = StringAlignment.Center;
                                format.LineAlignment = StringAlignment.Center;
                                break;
                            case ContentAlignment.MiddleRight:
                                format.Alignment = StringAlignment.Far;
                                format.LineAlignment = StringAlignment.Center;
                                break;
                            case ContentAlignment.BottomLeft:
                                format.Alignment = StringAlignment.Near;
                                format.LineAlignment = StringAlignment.Far;
                                break;
                            case ContentAlignment.BottomCenter:
                                format.Alignment = StringAlignment.Center;
                                format.LineAlignment = StringAlignment.Far;
                                break;
                            case ContentAlignment.BottomRight:
                                format.Alignment = StringAlignment.Far;
                                format.LineAlignment = StringAlignment.Far;
                                break;
                        }
                    }
                }
                else if (control is TabPage)
                {
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;
                }
                else if (control is GroupBox groupBox)
                {
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Near;
                }
                else if (control is CheckBox checkBox)
                {
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Center;
                }
                else if (control is RadioButton radioButton)
                {
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Center;
                }
                else if (control is LinkLabel linkLabel)
                {
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Center;
                }
                else if (control is ToolStrip toolStrip)
                {
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Center;
                }
                else if (control is StatusStrip statusStrip)
                {
                    format.Alignment = StringAlignment.Near;
                    format.LineAlignment = StringAlignment.Center;
                }
                else if (control is TabControl)
                {
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;
                }
                else if (control.Tag is StringFormat tagFormat)
                {
                    format = tagFormat;
                }
                else
                {
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;
                }

                // Shrink font until text fits in a single line (no wrapping)
                SizeF textSize = e.Graphics.MeasureString(text, font, clientRectangle.Size, format);
                Font shrinkFont = font;
                int minFontSize = 6;
                float fontSize = font.Size;
                float singleLineHeight = e.Graphics.MeasureString("A", font).Height;
                while ((textSize.Width > clientRectangle.Width || textSize.Height > singleLineHeight) &&
                       fontSize > minFontSize)
                {
                    fontSize -= 0.5f;
                    shrinkFont = new Font(shrinkFont.FontFamily, fontSize, shrinkFont.Style);
                    singleLineHeight = e.Graphics.MeasureString("A", shrinkFont).Height;
                    textSize = e.Graphics.MeasureString(text, shrinkFont, clientRectangle.Size, format);
                }

                using SolidBrush brush = new(foreColor);
                e.Graphics.DrawString(text, shrinkFont, brush, clientRectangle, format);

                if (control is Button btnBorder)
                {
                    Color borderColor = btnBorder.FlatAppearance != null &&
                                        btnBorder.FlatAppearance.BorderColor != Color.Empty
                        ? btnBorder.FlatAppearance.BorderColor
                        : btnBorder.Parent != null
                            ? btnBorder.Parent.BackColor
                            : SystemColors.ControlDark;
                    borderColor = MaybeDimIfDisabled(control, borderColor);
                    int borderWidth = btnBorder.FlatAppearance?.BorderSize ?? 1;
                    ControlPaint.DrawBorder(e.Graphics, clientRectangle, borderColor, borderWidth,
                        ButtonBorderStyle.Solid,
                        borderColor, borderWidth, ButtonBorderStyle.Solid,
                        borderColor, borderWidth, ButtonBorderStyle.Solid,
                        borderColor, borderWidth, ButtonBorderStyle.Solid);
                }
            }
        }

        private static class OwnerDrawThemeHelper
        {
            public static void ApplyOwnerDrawTheme(Control control, Model_UserUiColors colors)
            {
                if (control is Button btn)
                {
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 1;
                    btn.FlatAppearance.BorderColor = colors.ButtonBorderColor ?? SystemColors.ControlDark;
                    btn.FlatAppearance.MouseDownBackColor = colors.ButtonPressedBackColor ?? SystemColors.ControlDark;
                    btn.FlatAppearance.MouseOverBackColor = colors.ButtonHoverBackColor ?? SystemColors.ControlLight;
                }
                else if (control is GroupBox groupBox)
                {
                    groupBox.Paint -= GroupBox_OwnerDrawBorder;
                    groupBox.Paint += GroupBox_OwnerDrawBorder;

                    void GroupBox_OwnerDrawBorder(object? sender, PaintEventArgs e)
                    {
                        Color borderColor = colors.GroupBoxBorderColor ?? Color.Gray;
                        Color textColor = colors.GroupBoxForeColor ?? groupBox.ForeColor;
                        Color backColor = colors.GroupBoxBackColor ?? groupBox.BackColor;
                        Rectangle rect = groupBox.ClientRectangle;
                        rect.Width -= 1;
                        rect.Height -= 1;
                        using (SolidBrush b = new(backColor))
                        {
                            e.Graphics.FillRectangle(b, rect);
                        }

                        string text = groupBox.Text;
                        Font font = groupBox.Font;
                        SizeF textSize = e.Graphics.MeasureString(text, font);
                        int textPadding = 8;
                        Rectangle textRect = new(textPadding, 0, (int)textSize.Width + 2, (int)textSize.Height);
                        using (Pen p = new(borderColor, 1))
                        {
                            e.Graphics.DrawLine(p, rect.Left, rect.Top + textRect.Height / 2, textRect.Left - 2,
                                rect.Top + textRect.Height / 2);
                            e.Graphics.DrawLine(p, textRect.Right + 2, rect.Top + textRect.Height / 2, rect.Right,
                                rect.Top + textRect.Height / 2);
                            e.Graphics.DrawLine(p, rect.Left, rect.Top + textRect.Height / 2, rect.Left, rect.Bottom);
                            e.Graphics.DrawLine(p, rect.Left, rect.Bottom, rect.Right, rect.Bottom);
                            e.Graphics.DrawLine(p, rect.Right, rect.Top + textRect.Height / 2, rect.Right, rect.Bottom);
                        }

                        using (SolidBrush b = new(backColor))
                        {
                            e.Graphics.FillRectangle(b, textRect);
                        }

                        using (SolidBrush b = new(textColor))
                        {
                            e.Graphics.DrawString(text, font, b, textRect.Left, 0);
                        }
                    }
                }
            }

            public static void AttachLinkLabelHoverColor(LinkLabel ll, Color hoverColor)
            {
                ll.MouseEnter -= LinkLabel_MouseEnter;
                ll.MouseLeave -= LinkLabel_MouseLeave;
                ll.MouseEnter += LinkLabel_MouseEnter;
                ll.MouseLeave += LinkLabel_MouseLeave;

                void LinkLabel_MouseEnter(object? sender, EventArgs e)
                {
                    ll.LinkColor = hoverColor;
                }

                void LinkLabel_MouseLeave(object? sender, EventArgs e)
                {
                    ll.LinkColor = ll.VisitedLinkColor;
                }
            }

            public static void ApplyDataGridViewBorderColor(DataGridView dgv, Color borderColor) =>
                dgv.GridColor = borderColor;
        }

        private static class FocusUtils
        {
            public static void ApplyFocusEventHandling(Control control, Model_UserUiColors colors)
            {
                if (!CanControlReceiveFocus(control))
                {
                    return;
                }

                Apply(control, colors);
            }

            public static void ApplyFocusEventHandlingToControls(Control.ControlCollection controls,
                Model_UserUiColors colors)
            {
                foreach (Control ctrl in controls)
                {
                    ApplyFocusEventHandling(ctrl, colors);
                    if (ctrl.HasChildren)
                    {
                        ApplyFocusEventHandlingToControls(ctrl.Controls, colors);
                    }
                }
            }

            private static void Control_Enter_Handler(object? sender, EventArgs e)
            {
                if (sender is Control ctrl && ctrl.Focused)
                {
                    Model_UserUiColors colors = Core_AppThemes.GetCurrentTheme().Colors;
                    Color focusBackColor = colors.ControlFocusedBackColor ?? Color.LightBlue;
                    ctrl.BackColor = focusBackColor;

                    switch (ctrl)
                    {
                        case TextBox tb: tb.SelectAll(); break;
                        case MaskedTextBox mtb: mtb.SelectAll(); break;
                        case RichTextBox rtb: rtb.SelectAll(); break;
                        case ComboBox cb when cb.DropDownStyle != ComboBoxStyle.DropDownList: cb.SelectAll(); break;
                    }
                }
            }

            private static void Control_Leave_Handler(object? sender, EventArgs e)
            {
                if (sender is Control ctrl)
                {
                    Model_UserUiColors colors = Core_AppThemes.GetCurrentTheme().Colors;
                    Color normalBackColor = GetControlThemeBackColor(ctrl, colors);
                    ctrl.BackColor = normalBackColor;
                }
            }

            private static void TextBox_Click_SelectAll(object? sender, EventArgs e)
            {
                if (sender is TextBox tb)
                {
                    tb.SelectAll();
                }
            }

            private static void ComboBox_DropDown_SelectAll(object? sender, EventArgs e)
            {
                if (sender is ComboBox cb && cb.DropDownStyle != ComboBoxStyle.DropDownList)
                {
                    cb.SelectAll();
                }
            }

            private static void Apply(Control control, Model_UserUiColors colors)
            {
                control.Enter -= Control_Enter_Handler;
                control.Leave -= Control_Leave_Handler;
                if (control is TextBox tb)
                {
                    tb.Click -= TextBox_Click_SelectAll;
                }

                if (control is ComboBox cb)
                {
                    cb.DropDown -= ComboBox_DropDown_SelectAll;
                }

                control.Enter += Control_Enter_Handler;
                control.Leave += Control_Leave_Handler;
                if (control is TextBox tbx)
                {
                    tbx.Click += TextBox_Click_SelectAll;
                }

                if (control is ComboBox cbx)
                {
                    cbx.DropDown += ComboBox_DropDown_SelectAll;
                }
            }

            public static bool CanControlReceiveFocus(Control control)
            {
                if (!control.Enabled || !control.Visible || !control.TabStop)
                {
                    return false;
                }

                return control switch
                {
                    CheckedListBox => false,
                    TextBox => true,
                    ComboBox => true,
                    RichTextBox => true,
                    MaskedTextBox => true,
                    NumericUpDown => true,
                    DateTimePicker => true,
                    ListBox => false,
                    TreeView => false,
                    ListView => false,
                    TrackBar => false,
                    DomainUpDown => false,
                    Button => false,
                    CheckBox => false,
                    RadioButton => false,
                    Label => false,
                    Panel => false,
                    GroupBox => false,
                    PictureBox => false,
                    ProgressBar => false,
                    _ => false
                };
            }

            private static Color GetControlThemeBackColor(Control control, Model_UserUiColors colors) =>
                control switch
                {
                    TextBox => colors.TextBoxBackColor ?? colors.ControlBackColor ?? Color.White,
                    ComboBox => colors.ComboBoxBackColor ?? colors.ControlBackColor ?? Color.White,
                    RichTextBox => colors.RichTextBoxBackColor ?? colors.ControlBackColor ?? Color.White,
                    MaskedTextBox => colors.MaskedTextBoxBackColor ?? colors.ControlBackColor ?? Color.White,
                    NumericUpDown => colors.NumericUpDownBackColor ?? colors.ControlBackColor ?? Color.White,
                    DateTimePicker => colors.DateTimePickerBackColor ?? colors.ControlBackColor ?? Color.White,
                    _ => colors.ControlBackColor ?? Color.White
                };

            private static Color GetControlThemeForeColor(Control control, Model_UserUiColors colors) =>
                control switch
                {
                    TextBox => colors.TextBoxForeColor ?? colors.ControlForeColor ?? Color.Black,
                    ComboBox => colors.ComboBoxForeColor ?? colors.ControlForeColor ?? Color.Black,
                    RichTextBox => colors.RichTextBoxForeColor ?? colors.ControlForeColor ?? Color.Black,
                    MaskedTextBox => colors.MaskedTextBoxForeColor ?? colors.ControlForeColor ?? Color.Black,
                    NumericUpDown => colors.NumericUpDownForeColor ?? colors.ControlForeColor ?? Color.Black,
                    DateTimePicker => colors.DateTimePickerForeColor ?? colors.ControlForeColor ?? Color.Black,
                    _ => colors.ControlForeColor ?? Color.Black
                };
        }

        #endregion

        #region Core_AppThemes

        public static class Core_AppThemes
        {
            #region Theme Definition

            public class AppTheme
            {
                public Model_UserUiColors Colors { get; set; } = new();
                public Font? FormFont { get; set; }
            }

            #endregion

            #region Theme Registry

            private static Dictionary<string, AppTheme> Themes = new();

            private static async Task<string?> LoadAndSetUserThemeNameAsync(string userId)
            {
                try
                {
                    LoggingUtility.Log($"Loaded user theme name for user: {userId} = {Model_AppVariables.ThemeName}");
                    return await Dao_User.GetSettingsJsonAsync("Theme_Name", userId, true);
                }
                catch (Exception ex)
                {
                    LoggingUtility.LogApplicationError(ex);
                    throw;
                }
            }

            public static async Task LoadThemesFromDatabaseAsync()
            {
                try
                {
                    Dictionary<string, AppTheme> themes = new();
                    Helper_Database_Core helper = new(Model_AppVariables.ConnectionString);
                    DataTable dt = await helper.ExecuteDataTable("SELECT * FROM app_themes", null,
                        true,
                        CommandType.Text);
                    foreach (DataRow row in dt.Rows)
                    {
                        string? themeName = row["ThemeName"]?.ToString();
                        string? SettingsJson = row["SettingsJson"]?.ToString();
                        if (!string.IsNullOrWhiteSpace(themeName) && !string.IsNullOrWhiteSpace(SettingsJson))
                        {
                            try
                            {
                                JsonSerializerOptions options = new();
                                options.Converters.Add(new JsonColorConverter());
                                // Try legacy format first
                                Model_UserUiColors? colors = null;
                                try
                                {
                                    colors = JsonSerializer.Deserialize<Model_UserUiColors>(SettingsJson, options);
                                }
                                catch { }
                                if (colors != null)
                                {
                                    themes[themeName] = new AppTheme { Colors = colors, FormFont = null };
                                }
                                else
                                {
                                    // Try new simplified format
                                    try
                                    {
                                        var st = JsonSerializer.Deserialize<SimplifiedTheme>(SettingsJson);
                                        if (st != null)
                                        {
                                            colors = MapSimplifiedThemeToUserUiColors(st);
                                            themes[themeName] = new AppTheme { Colors = colors, FormFont = null };
                                        }
                                    }
                                    catch (Exception ex2)
                                    {
                                        LoggingUtility.LogApplicationError(ex2);
                                    }
                                }
                            }
                            catch (JsonException jsonEx)
                            {
                                LoggingUtility.LogApplicationError(jsonEx);
                            }
                        }
                    }

                    Themes = themes;
                    LoggingUtility.Log("Loaded themes from database.");
                }
                catch (Exception ex)
                {
                    LoggingUtility.LogApplicationError(ex);
                    throw;
                }
            }

            #endregion

            #region Theme Accessors

            public static IEnumerable<string> GetThemeNames()
            {
                try
                {
                    Debug.Assert(Themes != null, "Themes dictionary is not initialized.");
                    return Themes.Keys;
                }
                catch (Exception ex)
                {
                    LoggingUtility.LogApplicationError(ex);
                    throw;
                }
            }

            public static AppTheme GetCurrentTheme()
            {
                try
                {
                    string themeName = Model_AppVariables.ThemeName ?? "Default";
                    if (Themes.TryGetValue(themeName, out AppTheme? theme))
                    {
                        return theme;
                    }

                    Debug.Assert(Themes != null, "Themes dictionary is not initialized.");
                    return Themes.ContainsKey("Default") ? Themes["Default"] : new AppTheme();
                }
                catch (Exception ex)
                {
                    LoggingUtility.LogApplicationError(ex);
                    throw;
                }
            }

            public static AppTheme GetTheme(string themeName)
            {
                try
                {
                    if (Themes.TryGetValue(themeName, out AppTheme? theme))
                    {
                        return theme;
                    }

                    Debug.Assert(Themes != null, "Themes dictionary is not initialized.");
                    return Themes.ContainsKey("Default") ? Themes["Default"] : new AppTheme();
                }
                catch (Exception ex)
                {
                    LoggingUtility.LogApplicationError(ex);
                    throw;
                }
            }

            public static string GetEffectiveThemeName()
            {
                try
                {
                    string themeName = Model_AppVariables.ThemeName ?? "Default";
                    if (!Themes.ContainsKey(themeName))
                    {
                        themeName = "Default";
                    }

                    Debug.Assert(Themes != null, "Themes dictionary is not initialized.");
                    return themeName;
                }
                catch (Exception ex)
                {
                    LoggingUtility.LogApplicationError(ex);
                    throw;
                }
            }

            #endregion

            #region Theme Startup Sequence

            public static async Task InitializeThemeSystemAsync(string userId)
            {
                try
                {
                    await LoadAndSetUserThemeNameAsync(userId);
                    await LoadThemesFromDatabaseAsync();

                    foreach (AppTheme theme in Themes.Values)
                    {
                        if (theme.FormFont == null)
                        {
                            theme.FormFont = new Font("Segoe UI Emoji", Model_AppVariables.ThemeFontSize);
                        }
                        else if (Math.Abs(theme.FormFont.Size - Model_AppVariables.ThemeFontSize) > 0.01f)
                        {
                            theme.FormFont = new Font(theme.FormFont.FontFamily, Model_AppVariables.ThemeFontSize,
                                theme.FormFont.Style);
                        }
                    }

                    Debug.WriteLine(
                        $"Themes count: {Themes.Count}, using font size: {Model_AppVariables.ThemeFontSize}");
                    LoggingUtility.Log($"Theme system initialized with font size: {Model_AppVariables.ThemeFontSize}");
                }
                catch (Exception ex)
                {
                    LoggingUtility.LogApplicationError(ex);
                    throw;
                }
            }

            #endregion
        }

        #endregion

        public static async Task ApplyThemeToDataGridViewWithUserSettingsAsync(DataGridView dataGridView, string gridName)
        {
            // Apply theme as usual
            ApplyThemeToDataGridView(dataGridView);

            // Load and apply saved grid settings (column visibility/order)
            try
            {
                string userId = MTM_Inventory_Application.Models.Model_AppVariables.User;
                string json = await MTM_Inventory_Application.Data.Dao_User.GetGridViewSettingsJsonAsync(userId);
                if (!string.IsNullOrWhiteSpace(json))
                {
                    using var doc = JsonDocument.Parse(json);
                    if (doc.RootElement.TryGetProperty("Columns", out var columnsElem) && columnsElem.ValueKind == JsonValueKind.Array)
                    {
                        var columns = columnsElem.EnumerateArray().ToList();
                        // Set visibility and order for each column
                        foreach (var colElem in columns)
                        {
                            string? name = colElem.GetProperty("Name").GetString();
                            bool visible = colElem.GetProperty("Visible").GetBoolean();
                            int displayIndex = colElem.GetProperty("DisplayIndex").GetInt32();
                            if (!string.IsNullOrEmpty(name) && dataGridView.Columns.Contains(name))
                            {
                                var col = dataGridView.Columns[name];
                                col.Visible = visible;
                                col.DisplayIndex = displayIndex;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Core_Themes] Error loading grid settings: {ex}");
            }
        }

        public class SimplifiedTheme
        {
            public BaseColors? Base { get; set; }
            public AccentColors? Accent { get; set; }
            public StateColors? State { get; set; }
            public ComponentColors? Component { get; set; }

            public class BaseColors
            {
                public string? Background { get; set; }
                public string? Foreground { get; set; }
                public string? Border { get; set; }
            }
            public class AccentColors
            {
                public string? Primary { get; set; }
                public string? Secondary { get; set; }
                public string? Dark { get; set; }
            }
            public class StateColors
            {
                public string? Info { get; set; }
                public string? Success { get; set; }
                public string? Warning { get; set; }
                public string? Error { get; set; }
            }
            public class ComponentColors
            {
                public string? InputBackground { get; set; }
                public string? ButtonBackground { get; set; }
                public string? HeaderBackground { get; set; }
                public string? AltRowBackground { get; set; }
                public string? TabUnselected { get; set; }
                public string? ToolTipBackground { get; set; }
                public string? StatusBackground { get; set; }
            }
        }

        private static Model_UserUiColors MapSimplifiedThemeToUserUiColors(SimplifiedTheme st)
        {
            var colors = new Model_UserUiColors();
            if (st.Base != null)
            {
                colors.FormBackColor = ParseColor(st.Base.Background);
                colors.FormForeColor = ParseColor(st.Base.Foreground);
                colors.ControlBackColor = ParseColor(st.Base.Background);
                colors.ControlForeColor = ParseColor(st.Base.Foreground);
                colors.PanelBackColor = ParseColor(st.Base.Background);
                colors.PanelForeColor = ParseColor(st.Base.Foreground);
                colors.PanelBorderColor = ParseColor(st.Base.Border);
            }
            if (st.Accent != null)
            {
                colors.AccentColor = ParseColor(st.Accent.Primary);
                colors.SecondaryAccentColor = ParseColor(st.Accent.Secondary);
                colors.ButtonHoverBackColor = ParseColor(st.Accent.Primary);
                colors.ButtonHoverForeColor = ParseColor(st.Base?.Foreground);
                colors.ButtonPressedBackColor = ParseColor(st.Accent.Dark);
                colors.ButtonPressedForeColor = ParseColor(st.Base?.Foreground);
            }
            if (st.State != null)
            {
                colors.InfoColor = ParseColor(st.State.Info);
                colors.SuccessColor = ParseColor(st.State.Success);
                colors.WarningColor = ParseColor(st.State.Warning);
                colors.ErrorColor = ParseColor(st.State.Error);
            }
            if (st.Component != null)
            {
                colors.TextBoxBackColor = ParseColor(st.Component.InputBackground);
                colors.TextBoxForeColor = ParseColor(st.Base?.Foreground);
                colors.ButtonBackColor = ParseColor(st.Component.ButtonBackground);
                colors.ButtonForeColor = ParseColor(st.Base?.Foreground);
                colors.DataGridHeaderBackColor = ParseColor(st.Component.HeaderBackground);
                colors.DataGridAltRowBackColor = ParseColor(st.Component.AltRowBackground);
                colors.TabUnselectedBackColor = ParseColor(st.Component.TabUnselected);
                colors.ToolTipBackColor = ParseColor(st.Component.ToolTipBackground);
                colors.StatusStripBackColor = ParseColor(st.Component.StatusBackground);
            }
            return colors;
        }

        private static Color? ParseColor(string? hex)
        {
            if (string.IsNullOrWhiteSpace(hex)) return null;
            try
            {
                return ColorTranslator.FromHtml(hex);
            }
            catch { return null; }
        }
    }
}
