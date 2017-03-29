using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Common
{
    public class ConfigurationService
    {
        public string FileStoragePath { get; set; }

        public ConfigurationService()
        {
            FileStoragePath = GetValueOrDefault("FileStoragePath", @"c:\PaskolFiles");
        }

        private string GetValueOrDefault(string key, string defaultValue)
        {
            string res;
            res = ConfigurationManager.AppSettings.Get(key);

            if (res == null)
            {
                res = defaultValue;
            }

            return res;
        }
    }
}
