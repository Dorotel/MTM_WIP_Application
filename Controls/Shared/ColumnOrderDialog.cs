using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MTM_Inventory_Application.Core;

namespace MTM_Inventory_Application.Controls.Shared
{
    /// <summary>
    /// Dialog for reordering DataGridView columns via drag-and-drop
    /// </summary>
    public class ColumnOrderDialog : Form
    {
        #region Fields

        private readonly ListBox listBox;
        private readonly Button btnOK;
        private readonly Button btnCancel;
        private readonly Label infoLabel;
        private readonly Label lblInstructions;
        private readonly List<string> columnNames; // all columns, visible+hidden, in display order
        private readonly List<string> visibleColumnNames; // only visible columns, in display order
        private readonly List<string> hiddenColumnNames; // only hidden columns, in display order
        private int dragIndex = -1;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ColumnOrderDialog
        /// </summary>
        /// <param name="dgv">DataGridView to configure column order for</param>
        public ColumnOrderDialog(DataGridView dgv)
        {
            InitializeDialog();
            InitializeColumnData(dgv);
            InitializeControls(dgv);
            SetupEventHandlers();
            ApplyTheming();
        }

        #endregion

        #region Initialization Methods

        /// <summary>
        /// Initialize dialog properties
        /// </summary>
        private void InitializeDialog()
        {
            Text = "Change Column Order";
            Size = new Size(400, 540);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MinimizeBox = false;
            MaximizeBox = false;
            ShowInTaskbar = false;
        }

        /// <summary>
        /// Initialize column data collections
        /// </summary>
        /// <param name="dgv">DataGridView to analyze</param>
        private void InitializeColumnData(DataGridView dgv)
        {
            columnNames = dgv.Columns.Cast<DataGridViewColumn>()
                .OrderBy(c => c.DisplayIndex)
                .Select(c => c.Name)
                .ToList();
            
            visibleColumnNames = dgv.Columns.Cast<DataGridViewColumn>()
                .Where(c => c.Visible)
                .OrderBy(c => c.DisplayIndex)
                .Select(c => c.Name)
                .ToList();
            
            hiddenColumnNames = dgv.Columns.Cast<DataGridViewColumn>()
                .Where(c => !c.Visible)
                .OrderBy(c => c.DisplayIndex)
                .Select(c => c.Name)
                .ToList();
        }

        /// <summary>
        /// Initialize form controls
        /// </summary>
        /// <param name="dgv">DataGridView to populate from</param>
        private void InitializeControls(DataGridView dgv)
        {
            // Initialize ListBox
            listBox = new ListBox
            {
                Dock = DockStyle.Top,
                Height = 320,
                AllowDrop = true
            };
            
            foreach (var col in visibleColumnNames)
            {
                var gridCol = dgv.Columns[col];
                listBox.Items.Add(gridCol.HeaderText);
            }

            // Initialize Labels
            infoLabel = new Label
            {
                Text = "Drag and drop columns to reorder.\r\nUse Shift+Up/Down to move the selected column.\r\nOnly visible columns are shown. Hidden columns will always appear to the right.",
                Dock = DockStyle.Top,
                Height = 60,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(8),
                Font = new Font(Font.FontFamily, 10, FontStyle.Regular),
                AutoSize = false,
                ForeColor = Color.Black,
                BackColor = Color.Transparent
            };

            lblInstructions = new Label
            {
                Text = "How to use this form:\n\n- Drag and drop column names to change their order.\n- Use Shift+Up/Down to move a selected column.\n- Only visible columns are shown. Hidden columns will always appear to the right.\n- Click OK to save your changes.",
                Dock = DockStyle.Top,
                Height = 90,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(8),
                Font = new Font(Font.FontFamily, 10, FontStyle.Regular),
                AutoSize = false
            };

            // Initialize Buttons
            btnOK = new Button { Text = "OK", DialogResult = DialogResult.OK, Dock = DockStyle.Bottom };
            btnCancel = new Button { Text = "Cancel", DialogResult = DialogResult.Cancel, Dock = DockStyle.Bottom };

            // Add controls to form
            Controls.Add(btnOK);
            Controls.Add(btnCancel);
            Controls.Add(lblInstructions);
            Controls.Add(infoLabel);
            Controls.Add(listBox);

            AcceptButton = btnOK;
            CancelButton = btnCancel;
        }

        /// <summary>
        /// Setup event handlers for drag-and-drop functionality
        /// </summary>
        private void SetupEventHandlers()
        {
            listBox.MouseDown += ListBox_MouseDown;
            listBox.MouseMove += ListBox_MouseMove;
            listBox.DragOver += ListBox_DragOver;
            listBox.DragDrop += ListBox_DragDrop;
            listBox.KeyDown += ListBox_KeyDown;
        }

        /// <summary>
        /// Apply theming and DPI scaling
        /// </summary>
        private void ApplyTheming()
        {
            // DPI scaling and layout adjustments
            Core_Themes.ApplyDpiScaling(this);
            Core_Themes.ApplyRuntimeLayoutAdjustments(this);
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handle mouse down event for drag operation initiation
        /// </summary>
        private void ListBox_MouseDown(object? sender, MouseEventArgs e)
        {
            dragIndex = listBox.IndexFromPoint(e.Location);
        }

        /// <summary>
        /// Handle mouse move event for drag operation
        /// </summary>
        private void ListBox_MouseMove(object? sender, MouseEventArgs e)
        {
            if (dragIndex >= 0 && e.Button == MouseButtons.Left)
            {
                listBox.DoDragDrop(listBox.Items[dragIndex], DragDropEffects.Move);
            }
        }

        /// <summary>
        /// Handle drag over event
        /// </summary>
        private void ListBox_DragOver(object? sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        /// <summary>
        /// Handle drag drop event to reorder items
        /// </summary>
        private void ListBox_DragDrop(object? sender, DragEventArgs e)
        {
            Point point = listBox.PointToClient(new Point(e.X, e.Y));
            int index = listBox.IndexFromPoint(point);
            if (index < 0) index = listBox.Items.Count - 1;
            object data = e.Data?.GetData(typeof(string)) ?? "";
            if (dragIndex >= 0 && dragIndex < listBox.Items.Count)
            {
                listBox.Items.RemoveAt(dragIndex);
                listBox.Items.Insert(index, data);
                listBox.SelectedIndex = index;
                dragIndex = -1;
            }
        }

        /// <summary>
        /// Handle key down events for Shift+Up/Down reordering
        /// </summary>
        private void ListBox_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Shift && listBox.SelectedIndex >= 0)
            {
                int idx = listBox.SelectedIndex;
                if (e.KeyCode == Keys.Up && idx > 0)
                {
                    var item = listBox.Items[idx];
                    listBox.Items.RemoveAt(idx);
                    listBox.Items.Insert(idx - 1, item);
                    listBox.SelectedIndex = idx - 1;
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.Down && idx < listBox.Items.Count - 1)
                {
                    var item = listBox.Items[idx];
                    listBox.Items.RemoveAt(idx);
                    listBox.Items.Insert(idx + 1, item);
                    listBox.SelectedIndex = idx + 1;
                    e.Handled = true;
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get the new column order as configured by the user
        /// </summary>
        /// <returns>List of column names in the new order</returns>
        public List<string> GetColumnOrder()
        {
            // Map header text back to column names for visible columns
            var visibleOrder = new List<string>();
            foreach (string header in listBox.Items)
            {
                string? colName = visibleColumnNames.FirstOrDefault(n => header == n || header == n || header == n);
                // Actually, match by header text to column name
                colName = visibleColumnNames.FirstOrDefault(n => header == n);
                if (colName == null)
                {
                    // fallback: match by header text
                    colName = columnNames.FirstOrDefault(n => header == n);
                }
                if (colName == null)
                {
                    // fallback: try to match by header text in DataGridView
                    colName = visibleColumnNames.FirstOrDefault(n => header == n);
                }
                if (colName == null)
                {
                    // fallback: just skip
                    continue;
                }
                visibleOrder.Add(colName);
            }
            
            // Append hidden columns in their original order
            var finalOrder = new List<string>(visibleOrder);
            finalOrder.AddRange(hiddenColumnNames);
            return finalOrder;
        }

        #endregion
    }
}
