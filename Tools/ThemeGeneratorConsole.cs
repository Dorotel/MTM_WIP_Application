using System;
using MTM_Inventory_Application.Tools;

namespace MTM_Inventory_Application.Tools
{
    /// <summary>
    /// Console application to generate theme SQL files
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Starting Theme SQL Generation...");
                Console.WriteLine("====================================");
                
                var generator = new ThemeGenerator();
                generator.GenerateAllThemes();
                
                Console.WriteLine("====================================");
                Console.WriteLine("Theme SQL generation completed successfully!");
                Console.WriteLine("Files generated in 'Generated_Themes' directory.");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}