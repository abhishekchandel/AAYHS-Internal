using DbUp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AAYHS.Migration
{
    class Program
    {
        public static IConfigurationRoot Configuration;

        static void Main(string[] args)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();

            Console.WriteLine("Updating Database. Please stay tuned...");

            InitDatabase();
        }

        private static int InitDatabase()
        {
            var dbUpgradeEngineBuilder = DeployChanges.To
                .SqlDatabase(Configuration.GetConnectionString("ApplicationContext"))
                .WithScriptsEmbeddedInAssembly(typeof(Program).Assembly)
                .WithTransaction()
                .LogToConsole();

            var dbUpgradeEngine = dbUpgradeEngineBuilder.Build();
            if (dbUpgradeEngine.IsUpgradeRequired())
            {
                Console.WriteLine("Upgrades have been detected. Upgrading database now...");
                var operation = dbUpgradeEngine.PerformUpgrade();
                if (operation.Successful)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Success!");
                    Console.ResetColor();
                    return 0;
                }

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(operation.Error);
                Console.WriteLine("Everyobit Error");
                Console.ResetColor();
#if DEBUG
                Console.ReadLine();
#endif
                return -1;
            }
            Console.WriteLine("No Upgrades have been detected.");
            return 0;
        }
    }
}
