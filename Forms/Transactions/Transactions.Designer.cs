namespace MTM_Inventory_Application.Forms.Transactions
{
    partial class Transactions
    {
        #region Fields
        

        #region Fields
        
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ComboBox Transactions_ComboBox_SortBy;
        private System.Windows.Forms.Label Transactions_Label_SortBy;
        private System.Windows.Forms.TextBox Transactions_TextBox_SearchPartID;
        private System.Windows.Forms.Label Transactions_Label_SearchPartID;
        private System.Windows.Forms.Button Transactions_Button_Reset;
        private System.Windows.Forms.TabControl Transactions_TabControl_Main;
        private System.Windows.Forms.TabPage Transactions_TabPage_PartEntry;
        private System.Windows.Forms.TabPage Transactions_TabPage_PartRemoval;
        private System.Windows.Forms.TabPage Transactions_TabPage_PartTransfer;
        private System.Windows.Forms.DataGridView Transactions_DataGridView_Transactions;
        private System.Windows.Forms.Panel Transactions_Panel_Bottom;
        private System.Windows.Forms.Label Transactions_Label_SortByUser;
        private System.Windows.Forms.ComboBox Transactions_ComboBox_User;
        private System.Windows.Forms.Label Transactions_Label_User;
        private System.Windows.Forms.ComboBox Transactions_ComboBox_UserName;
        private System.Windows.Forms.Label Transactions_Label_UserName;
        private System.Windows.Forms.ComboBox Transactions_ComboBox_Shift;
        private System.Windows.Forms.Label Transactions_Label_Shift;
        
        #endregion
        
        #region Methods

        
        #endregion
        
        #region Methods

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

            this.Transactions_Label_SortBy = new System.Windows.Forms.Label();
            this.Transactions_ComboBox_SortBy = new System.Windows.Forms.ComboBox();

            this.Transactions_Label_SearchPartID = new System.Windows.Forms.Label();
            this.Transactions_TextBox_SearchPartID = new System.Windows.Forms.TextBox();

            this.Transactions_Button_Reset = new System.Windows.Forms.Button();

            this.Transactions_TabControl_Main = new System.Windows.Forms.TabControl();
            this.Transactions_TabPage_PartEntry = new System.Windows.Forms.TabPage();
            this.Transactions_TabPage_PartRemoval = new System.Windows.Forms.TabPage();
            this.Transactions_TabPage_PartTransfer = new System.Windows.Forms.TabPage();

            this.Transactions_DataGridView_Transactions = new System.Windows.Forms.DataGridView();

            this.Transactions_Panel_Bottom = new System.Windows.Forms.Panel();
            this.Transactions_Label_SortByUser = new System.Windows.Forms.Label();
            this.Transactions_Label_User = new System.Windows.Forms.Label();
            this.Transactions_ComboBox_User = new System.Windows.Forms.ComboBox();
            this.Transactions_Label_UserName = new System.Windows.Forms.Label();
            this.Transactions_ComboBox_UserName = new System.Windows.Forms.ComboBox();
            this.Transactions_Label_Shift = new System.Windows.Forms.Label();
            this.Transactions_ComboBox_Shift = new System.Windows.Forms.ComboBox();

            this.ClientSize = new System.Drawing.Size(830, 450);
            this.Text = "Personal History";

            this.Transactions_Label_SortBy.Text = "Sort By :";
            this.Transactions_Label_SortBy.Location = new System.Drawing.Point(15, 32);
            this.Transactions_Label_SortBy.Size = new System.Drawing.Size(60, 23);

            this.Transactions_ComboBox_SortBy.Location = new System.Drawing.Point(75, 30);
            this.Transactions_ComboBox_SortBy.Size = new System.Drawing.Size(130, 23);
            this.Transactions_ComboBox_SortBy.DropDownStyle = ComboBoxStyle.DropDownList;

            this.Transactions_Label_SearchPartID.Text = "Search By Part Number :";
            this.Transactions_Label_SearchPartID.Location = new System.Drawing.Point(225, 32);
            this.Transactions_Label_SearchPartID.Size = new System.Drawing.Size(145, 23);

            this.Transactions_TextBox_SearchPartID.Location = new System.Drawing.Point(370, 30);
            this.Transactions_TextBox_SearchPartID.Size = new System.Drawing.Size(170, 23);

            this.Transactions_Button_Reset.Text = "Reset";
            this.Transactions_Button_Reset.Location = new System.Drawing.Point(560, 29);
            this.Transactions_Button_Reset.Size = new System.Drawing.Size(75, 25);

            this.Transactions_TabControl_Main.Location = new System.Drawing.Point(15, 60);
            this.Transactions_TabControl_Main.Size = new System.Drawing.Size(800, 30);
            this.Transactions_TabControl_Main.TabPages.AddRange(new System.Windows.Forms.TabPage[] {
                this.Transactions_TabPage_PartEntry, this.Transactions_TabPage_PartRemoval, this.Transactions_TabPage_PartTransfer
            });

            this.Transactions_TabPage_PartEntry.Text = "Part Entry";
            this.Transactions_TabPage_PartRemoval.Text = "Part Removal";
            this.Transactions_TabPage_PartTransfer.Text = "Part Transfer";

            this.Transactions_DataGridView_Transactions.Location = new System.Drawing.Point(15, 90);
            this.Transactions_DataGridView_Transactions.Size = new System.Drawing.Size(800, 270);
            this.Transactions_DataGridView_Transactions.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom);
            this.Transactions_DataGridView_Transactions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.Transactions_DataGridView_Transactions.ReadOnly = true;
            this.Transactions_DataGridView_Transactions.AllowUserToAddRows = false;
            this.Transactions_DataGridView_Transactions.AllowUserToDeleteRows = false;
            this.Transactions_DataGridView_Transactions.AllowUserToOrderColumns = true;
            this.Transactions_DataGridView_Transactions.AllowUserToResizeRows = false;

            this.Transactions_Panel_Bottom.Location = new System.Drawing.Point(0, 370);
            this.Transactions_Panel_Bottom.Size = new System.Drawing.Size(830, 80);
            this.Transactions_Panel_Bottom.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            this.Transactions_Panel_Bottom.BorderStyle = BorderStyle.FixedSingle;

            this.Transactions_Label_SortByUser.Text = "Sort by User (Admin Only)";
            this.Transactions_Label_SortByUser.Location = new System.Drawing.Point(8, 8);
            this.Transactions_Label_SortByUser.Size = new System.Drawing.Size(180, 23);

            this.Transactions_Label_User.Text = "User :";
            this.Transactions_Label_User.Location = new System.Drawing.Point(8, 42);
            this.Transactions_Label_User.Size = new System.Drawing.Size(48, 23);
            this.Transactions_ComboBox_User.Location = new System.Drawing.Point(55, 40);
            this.Transactions_ComboBox_User.Size = new System.Drawing.Size(150, 23);

            this.Transactions_Label_UserName.Text = "User Name :";
            this.Transactions_Label_UserName.Location = new System.Drawing.Point(225, 42);
            this.Transactions_Label_UserName.Size = new System.Drawing.Size(75, 23);
            this.Transactions_ComboBox_UserName.Location = new System.Drawing.Point(300, 40);
            this.Transactions_ComboBox_UserName.Size = new System.Drawing.Size(150, 23);

            this.Transactions_Label_Shift.Text = "Shift :";
            this.Transactions_Label_Shift.Location = new System.Drawing.Point(470, 42);
            this.Transactions_Label_Shift.Size = new System.Drawing.Size(40, 23);
            this.Transactions_ComboBox_Shift.Location = new System.Drawing.Point(515, 40);
            this.Transactions_ComboBox_Shift.Size = new System.Drawing.Size(150, 23);

            this.Transactions_Panel_Bottom.Controls.Add(this.Transactions_Label_SortByUser);
            this.Transactions_Panel_Bottom.Controls.Add(this.Transactions_Label_User);
            this.Transactions_Panel_Bottom.Controls.Add(this.Transactions_ComboBox_User);
            this.Transactions_Panel_Bottom.Controls.Add(this.Transactions_Label_UserName);
            this.Transactions_Panel_Bottom.Controls.Add(this.Transactions_ComboBox_UserName);
            this.Transactions_Panel_Bottom.Controls.Add(this.Transactions_Label_Shift);
            this.Transactions_Panel_Bottom.Controls.Add(this.Transactions_ComboBox_Shift);

            this.Controls.Add(this.Transactions_Label_SortBy);
            this.Controls.Add(this.Transactions_ComboBox_SortBy);
            this.Controls.Add(this.Transactions_Label_SearchPartID);
            this.Controls.Add(this.Transactions_TextBox_SearchPartID);
            this.Controls.Add(this.Transactions_Button_Reset);
            this.Controls.Add(this.Transactions_TabControl_Main);
            this.Controls.Add(this.Transactions_DataGridView_Transactions);
            this.Controls.Add(this.Transactions_Panel_Bottom);
        }
        
        #endregion
    }

        
        #endregion
    }
