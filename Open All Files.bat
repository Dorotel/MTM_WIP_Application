@echo off
echo Opening MTM Inventory Application in Visual Studio 2022...

REM MTM Inventory Application - All Source Files Opener
set "VS_PATH=C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\devenv.exe"
set "PROJECT=MTM_Inventory_Application.csproj"

REM Check if VS path exists
if not exist "%VS_PATH%" (
    set "VS_PATH=devenv.exe"
)

echo Step 1: Opening project...
start "" "%VS_PATH%" "%PROJECT%"

echo Step 2: Waiting for Visual Studio to load...
timeout /t 10 /nobreak >nul

echo Step 3: Opening all application source files...

REM Root Files
"%VS_PATH%" /edit "Program.cs"
"%VS_PATH%" /edit "AssemblyInfo.cs"

REM Core Files
"%VS_PATH%" /edit "Core\Core_Themes.cs"
"%VS_PATH%" /edit "Core\Core_WipAppVariables.cs"
"%VS_PATH%" /edit "Core\Core_DgvPrinter.cs"
"%VS_PATH%" /edit "Core\Core_JsonColorConverter.cs"

REM Data Layer (DAOs)
"%VS_PATH%" /edit "Data\Dao_ErrorLog.cs"
"%VS_PATH%" /edit "Data\Dao_History.cs"
"%VS_PATH%" /edit "Data\Dao_Inventory.cs"
"%VS_PATH%" /edit "Data\Dao_ItemType.cs"
"%VS_PATH%" /edit "Data\Dao_Location.cs"
"%VS_PATH%" /edit "Data\Dao_Operation.cs"
"%VS_PATH%" /edit "Data\Dao_Part.cs"
"%VS_PATH%" /edit "Data\Dao_QuickButtons.cs"
"%VS_PATH%" /edit "Data\Dao_System.cs"
"%VS_PATH%" /edit "Data\Dao_Transactions.cs"
"%VS_PATH%" /edit "Data\Dao_User.cs"

REM Forms
"%VS_PATH%" /edit "Forms\MainForm\MainForm.cs"
"%VS_PATH%" /edit "Forms\MainForm\Classes\MainFormControlHelper.cs"
"%VS_PATH%" /edit "Forms\MainForm\Classes\MainFormTabResetHelper.cs"
"%VS_PATH%" /edit "Forms\MainForm\Classes\MainFormUserSettingsHelper.cs"
"%VS_PATH%" /edit "Forms\Settings\SettingsForm.cs"
"%VS_PATH%" /edit "Forms\Splash\SplashScreenForm.cs"
"%VS_PATH%" /edit "Forms\Transactions\Transactions.cs"
"%VS_PATH%" /edit "Forms\ErrorDialog\EnhancedErrorDialog.cs"
"%VS_PATH%" /edit "Forms\Development\DependencyChartConverter\DependencyChartConverterWinForm.cs"
"%VS_PATH%" /edit "Forms\Development\DependencyChartViewer\DependencyChartViewerForm.cs"

REM Controls - MainForm
"%VS_PATH%" /edit "Controls\MainForm\Control_AdvancedInventory.cs"
"%VS_PATH%" /edit "Controls\MainForm\Control_AdvancedRemove.cs"
"%VS_PATH%" /edit "Controls\MainForm\Control_InventoryTab.cs"
"%VS_PATH%" /edit "Controls\MainForm\Control_QuickButtons.cs"
"%VS_PATH%" /edit "Controls\MainForm\Control_RemoveTab.cs"
"%VS_PATH%" /edit "Controls\MainForm\Control_TransferTab.cs"

REM Controls - SettingsForm
"%VS_PATH%" /edit "Controls\SettingsForm\Control_About.cs"
"%VS_PATH%" /edit "Controls\SettingsForm\Control_Add_ItemType.cs"
"%VS_PATH%" /edit "Controls\SettingsForm\Control_Add_Location.cs"
"%VS_PATH%" /edit "Controls\SettingsForm\Control_Add_Operation.cs"
"%VS_PATH%" /edit "Controls\SettingsForm\Control_Add_PartID.cs"
"%VS_PATH%" /edit "Controls\SettingsForm\Control_Add_User.cs"
"%VS_PATH%" /edit "Controls\SettingsForm\Control_Database.cs"
"%VS_PATH%" /edit "Controls\SettingsForm\Control_Edit_ItemType.cs"
"%VS_PATH%" /edit "Controls\SettingsForm\Control_Edit_Location.cs"
"%VS_PATH%" /edit "Controls\SettingsForm\Control_Edit_Operation.cs"
"%VS_PATH%" /edit "Controls\SettingsForm\Control_Edit_PartID.cs"
"%VS_PATH%" /edit "Controls\SettingsForm\Control_Edit_User.cs"
"%VS_PATH%" /edit "Controls\SettingsForm\Control_Remove_ItemType.cs"
"%VS_PATH%" /edit "Controls\SettingsForm\Control_Remove_Location.cs"
"%VS_PATH%" /edit "Controls\SettingsForm\Control_Remove_Operation.cs"
"%VS_PATH%" /edit "Controls\SettingsForm\Control_Remove_PartID.cs"
"%VS_PATH%" /edit "Controls\SettingsForm\Control_Remove_User.cs"
"%VS_PATH%" /edit "Controls\SettingsForm\Control_Shortcuts.cs"
"%VS_PATH%" /edit "Controls\SettingsForm\Control_Theme.cs"

REM Controls - Shared and Addons
"%VS_PATH%" /edit "Controls\Shared\ColumnOrderDialog.cs"
"%VS_PATH%" /edit "Controls\Shared\Control_ProgressBarUserControl.cs"
"%VS_PATH%" /edit "Controls\Addons\Control_ConnectionStrengthControl.cs"

REM Services
"%VS_PATH%" /edit "Services\Service_ErrorHandler.cs"
"%VS_PATH%" /edit "Services\Service_Timer_VersionChecker.cs"
"%VS_PATH%" /edit "Services\Service_ConnectionRecoveryManager.cs"
"%VS_PATH%" /edit "Services\Service_OnStartup_AppDataCleaner.cs"
"%VS_PATH%" /edit "Services\Service_OnStartup_StartupSplashApplicationContext.cs"

REM Helpers
"%VS_PATH%" /edit "Helpers\Helper_Control_MySqlSignal.cs"
"%VS_PATH%" /edit "Helpers\Helper_Database_StoredProcedure.cs"
"%VS_PATH%" /edit "Helpers\Helper_Database_Variables.cs"
"%VS_PATH%" /edit "Helpers\Helper_StoredProcedureProgress.cs"
"%VS_PATH%" /edit "Helpers\Helper_UI_ComboBoxes.cs"
"%VS_PATH%" /edit "Helpers\Helper_UI_Shortcuts.cs"

REM Models
"%VS_PATH%" /edit "Models\Model_AppVariables.cs"
"%VS_PATH%" /edit "Models\Model_CurrentInventory.cs"
"%VS_PATH%" /edit "Models\Model_DaoResult.cs"
"%VS_PATH%" /edit "Models\Model_HistoryInventory.cs"
"%VS_PATH%" /edit "Models\Model_HistoryRemove.cs"
"%VS_PATH%" /edit "Models\Model_HistoryTransfer.cs"
"%VS_PATH%" /edit "Models\Model_TransactionHistory.cs"
"%VS_PATH%" /edit "Models\Model_Transactions.cs"
"%VS_PATH%" /edit "Models\Model_UserBasedShortcutBar.cs"
"%VS_PATH%" /edit "Models\Model_Users.cs"
"%VS_PATH%" /edit "Models\Model_UserUiColors.cs"
"%VS_PATH%" /edit "Models\Model_VersionHistory.cs"

REM Logging
"%VS_PATH%" /edit "Logging\LoggingUtility.cs"

echo.
echo All MTM Inventory Application source files opened!
echo Total: ~80 application source files (.NET 8)
echo.
echo Files opened include:
echo - Program.cs and AssemblyInfo.cs
echo - All Forms (MainForm, Settings, Splash, Transactions, ErrorDialog, Development)
echo - All Controls (MainForm tabs, Settings controls, Shared controls)
echo - All Data Access Objects (DAOs)
echo - All Services (Error handling, Version checking, Startup services)
echo - All Helpers (Database, UI, Progress helpers)
echo - All Models (Data models and DTOs)
echo - All Core utilities (Themes, Variables, Printing)
echo - Logging utilities
echo.
echo Excluded: Designer.cs files, obj/bin folders, and generated files
echo Use Solution Explorer to navigate between files efficiently.
pause