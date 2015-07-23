using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HinttechPractice.Core
{
    /// <summary>
    /// Class to represent logger(log4net).
    /// </summary>
    public static class MyLogger
    {
        /// <summary>
        /// Get active logger for config file(App.config).
        /// </summary>
        /// <param name="type">Name of class where we get this logger.</param>
        /// <returns>Our logger that we want to use.</returns>
        public static ILog GetLogger(Type type)
        {
            // If no loggers have been created, load our own.
            if (LogManager.GetCurrentLoggers().Length == 0)
            {
                LoadConfig();
            }
            return LogManager.GetLogger(type);
        }

        /// <summary>
        /// Class for loading configuration file from file system. Using XmlConfigurator.
        /// </summary>
        private static void LoadConfig()
        {
            StringBuilder LOG_CONFIG_FILE = new StringBuilder();
            string activeConfigPath = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile; //Active config file path.

            //Parsing active config path to jump to other project and get that config.
            string[] arrayOfFolders = activeConfigPath.Split(new string[] { "\\" }, StringSplitOptions.None);

            for (int i = 0; i < arrayOfFolders.Length - 2; i++)
            {
                LOG_CONFIG_FILE.Append(arrayOfFolders[i] + "\\");
            }

            LOG_CONFIG_FILE.Append("HinttechPractice.Core\\App.config");

            XmlConfigurator.ConfigureAndWatch(new FileInfo(LOG_CONFIG_FILE.ToString()));
        }

    }
}
