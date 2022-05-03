using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Configuration
{
    public static class AppSettings
    {
        private static IConfiguration configuration;

        static AppSettings()
        {
            if (!Environment.UserInteractive) //running as a service
            {
                System.IO.Directory.SetCurrentDirectory(System.AppDomain.CurrentDomain.BaseDirectory);
            }
            string executionDirectory = Directory.GetCurrentDirectory();

            var builder = new ConfigurationBuilder().SetBasePath(executionDirectory)
                .AddJsonFile("smsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            configuration = builder.Build();
        }

        public static string GetSetting(string name)
        {
            string appSettings = configuration[name];
            return appSettings;
        }

        public static SMSettings GetSMSettings()
        {
            return configuration.GetSection("SMSettings").Get<SMSettings>();
        }

    }
}
