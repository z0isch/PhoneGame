using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Configuration;

namespace PhoneGameService.Logging
{
    public class LoggingConfiguration
    {
        public static void Initialize()
        {
            string path = ConfigurationManager.AppSettings["PhoneGameLoggingConfigPath"];
            if (string.IsNullOrEmpty(path))
            {
                log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo("Log4NetConfig.xml"));
            }
            else
            {
                log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(path));
            }
        }

        public static void Initialize(string path)
        {
            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(path));
        }
    }
}
