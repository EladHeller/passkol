using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;


namespace Infrastructure.Configuration
{
    public static class WebConf
    {
        public static string EmailFromSystem
        {
            get
            {
                return ConfigurationManager.AppSettings["EmailFromSystem"];
            }
        }
        public static string TeamMail
        {
            get
            {
                return ConfigurationManager.AppSettings["TeamMail"];
            }
        }
        public static string FSBaseRoute
        {
            get
            {
                return ConfigurationManager.AppSettings["FSBaseRoute"];
            }
        }
    }
}
