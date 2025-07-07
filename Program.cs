using MTM_Inventory_Application.Controls.MainForm;
using MTM_Inventory_Application.Core;
using MTM_Inventory_Application.Forms.MainForm;
using MTM_Inventory_Application.Helpers;
using MTM_Inventory_Application.Logging;
using MTM_Inventory_Application.Services;
using MySql.Data.MySqlClient;

namespace MTM_Inventory_Application;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        try
        {
            Service_OnStartup.RunStartupSequenceAsync().GetAwaiter().GetResult();
            var mainForm = new MainForm();
            ControlRemoveTab.MainFormInstance = mainForm;
            ControlInventoryTab.MainFormInstance = mainForm;
            ControlTransferTab.MainFormInstance = mainForm;
            Control_AdvancedInventory.MainFormInstance = mainForm;
            Control_AdvancedRemove.MainFormInstance = mainForm;
            Control_QuickButtons.MainFormInstance = mainForm;
            Helper_UI_ComboBoxes.MainFormInstance = mainForm;
            Service_Timer_VersionChecker.MainFormInstance = mainForm;
            Core_Themes.ApplyTheme(mainForm);

            // Set MainFormInstance properties before running the application


            // Now call RunApplication, passing mainForm if needed
            Application.Run(mainForm);
        }
        catch (MySqlException ex)
        {
            LoggingUtility.LogDatabaseError(ex);
            Service_OnEvent_ExceptionHandler.HandleDatabaseError();
        }
        catch (Exception ex)
        {
            LoggingUtility.LogApplicationError(ex);
            MessageBox.Show(@"An error occurred on Main in Program.cs:\n" + ex.Message, @"Error", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}