# 12. Application Startup and Lifecycle

Startup Sequence (Program.cs)
```csharp
Application.ThreadException += GlobalExceptionHandler;
AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;

Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
ApplicationConfiguration.Initialize();

Model_AppVariables.User = Dao_System.System_GetUserName();
await Dao_System.System_UserAccessTypeAsync(true);

Application.Run(new StartupSplashApplicationContext());
```

Splash Tasks
1) Initialize logging  
2) Clean old logs  
3) Wipe app data folders  
4) Setup DataTables  
5) Initialize version checker  
6) Initialize theme system  
7) Load user settings  
8) Create and configure MainForm  
9) Apply theme