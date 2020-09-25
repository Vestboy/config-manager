using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConfigManager
{
    public class CommonUtils
    {
        public static IConfigurationRoot GetConfiguration()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));

            var root = builder.Build();
            
            return root;
        }

        public static string GetBaseFolderPath()
        {
            IConfigurationRoot configRoot = GetConfiguration();
            IConfigurationSection appSection = configRoot.GetSection("AppSettings");

            string folderPath = appSection.GetValue<string>("BaseFolder");

            return folderPath;
        }
    }
}
