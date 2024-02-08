using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using System;
using metadata_extractor;

namespace ConsoleToWebAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            //open connection with database

            //IConfigurationRoot config = new ConfigurationBuilder()
            //    .AddJsonFile("appsettings.json")
            //    .AddEnvironmentVariables()
            //    .Build();

            //var settings = config.GetRequiredSection("ConnString").Get<Dictionary<string, string>>();

            //var connString = $"Host={settings["Host"]};Username={settings["Username"]};" +
            //    $"Password={settings["Password"]};Database={settings["Database"]}";

            Console.WriteLine(Constants.connString);

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}