using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using System.IO;

namespace Anomaly
{
    /// <summary>
    /// This is the configuration for the editor.
    /// </summary>
    class AnomalyConfig
    {
        private static ConfigFile configFile;
        private static String docRoot = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Anomaly";

        static AnomalyConfig()
        {
            if (!Directory.Exists(docRoot))
            {
                Directory.CreateDirectory(docRoot);
            }
            configFile = new ConfigFile(docRoot + "/config.ini");
            configFile.loadConfigFile();
            ResourceSection = new ResourceSection(configFile);
            PluginSection = new PluginSection(configFile);
            EngineConfig = new EngineConfig(configFile);
        }

        public static String DocRoot
        {
            get
            {
                return docRoot;
            }
        }

        public static void save()
        {
            configFile.writeConfigFile();
        }

        public static ResourceSection ResourceSection { get; private set; }

        public static PluginSection PluginSection { get; private set; }

        public static EngineConfig EngineConfig { get; private set; }

        public static ConfigFile ConfigFile
        {
            get
            {
                return configFile;
            }
        }

        private AnomalyConfig()
        {
            
        }
    }
}
