namespace MTM_Inventory_Application.Forms.Transactions
{
    partial class Transactions
    {
        private System.ComponentModel.IContainer components = null;

        // Controls
        private System.Windows.Forms.ComboBox comboSortBy;
        private System.Windows.Forms.Label lblSortBy;
        private System.Windows.Forms.TextBox txtSearchPartID;
        private System.Windows.Forms.Label lblSearchPartID;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPartEntry;
        private System.Windows.Forms.TabPage tabPartRemoval;
        private System.Windows.Forms.TabPage tabPartTransfer;
        private System.Windows.Forms.DataGridView dataGridTransactions;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Label lblSortByUser;
        private System.Windows.Forms.ComboBox comboUser;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.ComboBox comboUserName;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.ComboBox comboShift;
        private System.Windows.Forms.Label lblShift;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            // Sort by controls
            this.lblSortBy = new System.Windows.Forms.Label();
            this.comboSortBy = new System.Windows.Forms.ComboBox();

            // Search controls
            this.lblSearchPartID = new System.Windows.Forms.Label();
            this.txtSearchPartID = new System.Windows.Forms.TextBox();

            // Reset button
            this.btnReset = new System.Windows.Forms.Button();

            // TabControl and tabs
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPartEntry = new System.Windows.Forms.TabPage();
            this.tabPartRemoval = new System.Windows.Forms.TabPage();
            this.tabPartTransfer = new System.Windows.Forms.TabPage();

            // DataGridView
            this.dataGridTransactions = new System.Windows.Forms.DataGridView();

            // Bottom panel for user filters
            this.panelBottom = new System.Windows.Forms.Panel();
            this.lblSortByUser = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();
            this.comboUser = new System.Windows.Forms.ComboBox();
            this.lblUserName = new System.Windows.Forms.Label();
            this.comboUserName = new System.Windows.Forms.ComboBox();
            this.lblShift = new System.Windows.Forms.Label();
            this.comboShift = new System.Windows.Forms.ComboBox();

            // --- Form size ---
            this.ClientSize = new System.Drawing.Size(830, 450);
            this.Text = "Personal History";

            // --- Sort By ---
            this.lblSortBy.Text = "Sort By :";
            this.lblSortBy.Location = new System.Drawing.Point(15, 32);
            this.lblSortBy.Size = new System.Drawing.Size(60, 23);

            this.comboSortBy.Location = new System.Drawing.Point(75, 30);
            this.comboSortBy.Size = new System.Drawing.Size(130, 23);
            this.comboSortBy.DropDownStyle = ComboBoxStyle.DropDownList;

            // --- Search By Part Number ---
            this.lblSearchPartID.Text = "Search By Part Number :";
            this.lblSearchPartID.Location = new System.Drawing.Point(225, 32);
            this.lblSearchPartID.Size = new System.Drawing.Size(145, 23);

            this.txtSearchPartID.Location = new System.Drawing.Point(370, 30);
            this.txtSearchPartID.Size = new System.Drawing.Size(170, 23);

            // --- Reset button ---
            this.btnReset.Text = "Reset";
            this.btnReset.Location = new System.Drawing.Point(560, 29);
            this.btnReset.Size = new System.Drawing.Size(75, 25);

            // --- TabControl ---
            this.tabControlMain.Location = new System.Drawing.Point(15, 60);
            this.tabControlMain.Size = new System.Drawing.Size(800, 30);
            this.tabControlMain.TabPages.AddRange(new System.Windows.Forms.TabPage[] {
                this.tabPartEntry, this.tabPartRemoval, this.tabPartTransfer
            });

            this.tabPartEntry.Text = "Part Entry";
            this.tabPartRemoval.Text = "Part Removal";
            this.tabPartTransfer.Text = "Part Transfer";

            // --- DataGridView ---
            this.dataGridTransactions.Location = new System.Drawing.Point(15, 90);
            this.dataGridTransactions.Size = new System.Drawing.Size(800, 270);
            this.dataGridTransactions.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom);
            this.dataGridTransactions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dataGridTransactions.ReadOnly = true;
            this.dataGridTransactions.AllowUserToAddRows = false;
            this.dataGridTransactions.AllowUserToDeleteRows = false;
            this.dataGridTransactions.AllowUserToOrderColumns = true;
            this.dataGridTransactions.AllowUserToResizeRows = false;

            // --- Bottom panel ---
            this.panelBottom.Location = new System.Drawing.Point(0, 370);
            this.panelBottom.Size = new System.Drawing.Size(830, 80);
            this.panelBottom.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            this.panelBottom.BorderStyle = BorderStyle.FixedSingle;

            // Sort by User label
            this.lblSortByUser.Text = "Sort by User (Admin Only)";
            this.lblSortByUser.Location = new System.Drawing.Point(8, 8);
            this.lblSortByUser.Size = new System.Drawing.Size(180, 23);

            // User filter
            this.lblUser.Text = "User :";
            this.lblUser.Location = new System.Drawing.Point(8, 42);
            this.lblUser.Size = new System.Drawing.Size(48, 23);
            this.comboUser.Location = new System.Drawing.Point(55, 40);
            this.comboUser.Size = new System.Drawing.Size(150, 23);

            // User Name filter
            this.lblUserName.Text = "User Name :";
            this.lblUserName.Location = new System.Drawing.Point(225, 42);
            this.lblUserName.Size = new System.Drawing.Size(75, 23);
            this.comboUserName.Location = new System.Drawing.Point(300, 40);
            this.comboUserName.Size = new System.Drawing.Size(150, 23);

            // Shift filter
            this.lblShift.Text = "Shift :";
            this.lblShift.Location = new System.Drawing.Point(470, 42);
            this.lblShift.Size = new System.Drawing.Size(40, 23);
            this.comboShift.Location = new System.Drawing.Point(515, 40);
            this.comboShift.Size = new System.Drawing.Size(150, 23);

            // Add controls to panelBottom
            this.panelBottom.Controls.Add(this.lblSortByUser);
            this.panelBottom.Controls.Add(this.lblUser);
            this.panelBottom.Controls.Add(this.comboUser);
            this.panelBottom.Controls.Add(this.lblUserName);
            this.panelBottom.Controls.Add(this.comboUserName);
            this.panelBottom.Controls.Add(this.lblShift);
            this.panelBottom.Controls.Add(this.comboShift);

            // Add controls to form
            this.Controls.Add(this.lblSortBy);
            this.Controls.Add(this.comboSortBy);
            this.Controls.Add(this.lblSearchPartID);
            this.Controls.Add(this.txtSearchPartID);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.tabControlMain);
            this.Controls.Add(this.dataGridTransactions);
            this.Controls.Add(this.panelBottom);
        }
    }
}