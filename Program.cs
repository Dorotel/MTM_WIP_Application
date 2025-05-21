using System;
using System.Windows.Forms;
using MTM_WIP_Application.Forms.MainForm;

namespace MTM_WIP_Application;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        Application.Run(new MainForm()); // Replace with MainForm when ready
    }
}