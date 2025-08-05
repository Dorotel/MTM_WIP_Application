// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MTM_Inventory_Application.Data;
using MTM_Inventory_Application.Models;

namespace MTM_Inventory_Application.Controls.SettingsForm
{
    public partial class Control_Database : UserControl
    {

        public event EventHandler? DatabaseSettingsUpdated;

        public Control_Database()
        {
            InitializeComponent();
        }

        private async Task LoadDatabaseSettings()
        {
            try
            {
                string user = Model_AppVariables.User;

                Control_Database_TextBox_Server.Text =
                    await Dao_User.GetWipServerAddressAsync(user) ?? "172.16.1.104"; //172.16.1.104
                Control_Database_TextBox_Port.Text = await Dao_User.GetWipServerPortAsync(user) ?? "3306";
                Control_Database_TextBox_Database.Text = await Dao_User.GetDatabaseAsync(user) ?? "mtm_wip_application";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading database settings: {ex.Message}");
            }
        }

        private async Task SaveDatabaseSettings()
        {
            try
            {
                string user = Model_AppVariables.User;

                await Dao_User.SetWipServerAddressAsync(user, Control_Database_TextBox_Server.Text);
                await Dao_User.SetDatabaseAsync(user, Control_Database_TextBox_Database.Text);
                await Dao_User.SetWipServerPortAsync(user, Control_Database_TextBox_Port.Text);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to save database settings: {ex.Message}");
            }
        }
    }
}
