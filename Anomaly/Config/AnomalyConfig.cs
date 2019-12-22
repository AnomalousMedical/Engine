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
    public class AnomalyConfig
    {
        private static ConfigFile configFile;
        private static String docRoot = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Anomalous Software/Anomaly";
        private static ToolsConfig tools;
        private static ConfigSection program;

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
            program = configFile.createOrRetrieveConfigSection("Program");
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

        public static String LastShaderVersion
        {
            get
            {
                return program.getValue("LastShaderVersion", "");
            }
            set
            {
                program.setValue("LastShaderVersion", value);
            }
        }

        public static String LastPublishDirectory
        {
            get
            {
                return program.getValue("LastPublishDirectory", "");
            }
            set
            {
                program.setValue("LastPublishDirectory", value);
            }
        }

        private AnomalyConfig()
        {
            
        }
    }
}
