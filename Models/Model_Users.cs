namespace MTM_Inventory_Application.Models
{
    internal class Model_Users
    {
        #region Properties

        public static string FullName { get; set; } = string.Empty;
        public string HideChangeLog { get; set; } = "false";
        public int Id { get; set; } = 0;
        public string LastShownVersion { get; set; } = "0.0.0.0";
        public string Pin { get; set; } = "0000";
        public string Shift { get; set; } = string.Empty;
        public int ThemeFontSize { get; set; } = 9;
        public string ThemeName { get; set; } = "Default";
        public string User { get; set; } = string.Empty;
        public static string VisualPassword { get; set; } = "Password";
        public static string VisualUserName { get; set; } = "User Name";
        public bool VitsUser { get; set; } = false;
        public static string Database 
        { 
            get
            {
#if DEBUG
                return "mtm_wip_application_test";
#else
                return "mtm_wip_application";
#endif
            }
        }

        public static string WipServerAddress 
        { 
            get
            {
#if DEBUG
                // Debug Mode: Use 172.16.1.104 if current IP matches, otherwise localhost
                return GetLocalIpAddress() == "172.16.1.104" ? "172.16.1.104" : "localhost";
#else
                // Release Mode: Always use 172.16.1.104
                return "172.16.1.104";
#endif
            }
        }

        public static string WipServerPort { get; set; } = "3306";

        /// <summary>
        /// Gets the local IP address to determine server selection in Debug mode
        /// </summary>
        private static string GetLocalIpAddress()
        {
            try
            {
                using var socket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, 
                    System.Net.Sockets.SocketType.Dgram, 0);
                socket.Connect("8.8.8.8", 65530);
                var endPoint = socket.LocalEndPoint as System.Net.IPEndPoint;
                return endPoint?.Address.ToString() ?? "localhost";
            }
            catch
            {
                return "localhost";
            }
        }

        #endregion
    }
}
