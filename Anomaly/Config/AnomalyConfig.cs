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
        private static String docRoot = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Anomalous Software/Anomaly";
        private static ToolsConfig tools;

        static AnomalyConfig()
        {
            if (!Directory.Exists(docRoot))
            {
                Directory.CreateDirectory(docRoot);
            }
            configFile = new ConfigFile(docRoot + "/config.ini");
            RecentDocuments = new RecentDocuments(configFile);
            configFile.loadConfigFile();
            EngineConfig = new EngineConfig(configFile);
            CameraConfig = new CameraSection(configFile);
            tools = new ToolsConfig(configFile);
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

        public static RecentDocuments RecentDocuments { get; private set; }

        public static EngineConfig EngineConfig { get; private set; }

        public static CameraSection CameraConfig { get; private set; }

        public static ConfigFile ConfigFile
        {
            get
            {
                return configFile;
            }
        }

        public static String WindowsFile
        {
            get
            {
                return AnomalyConfig.DocRoot + "/WindowConfig.ini";
            }
        }

        public static ToolsConfig Tools
        {
            get
            {
                return tools;
            }
        }

        private AnomalyConfig()
        {
            
        }
    }
}
