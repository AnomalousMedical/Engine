using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Engine;
using Engine.Resources;
using Engine.Platform;
using Logging;
using Medical.GUI;

namespace Medical
{
    public enum UIExtraScale
    {
        Smaller,
        Normal,
        Larger
    }
    
    public class CoreConfig
    {
        private static ConfigFile configFile;
        private static ConfigSection program;

        private static String docRoot;
        private static String recentDocsFile;

        private static String sceneDirectory;

        private static String userAnomalousFolder;
        private static String localDataFileFolder;

        private static float cameraTransitionTime;
        private static float transparencyChangeMultiplier;
        private static bool autoOpenAnatomyFinder;

        public CoreConfig(String programName)
        {
            BuildName = null;
            FolderFinder.setProgramName(programName);

            //Setup directories
            CoreConfig.userAnomalousFolder = FolderFinder.LocalUserDocumentsFolder;
            if (!Directory.Exists(userAnomalousFolder))
            {
                Directory.CreateDirectory(userAnomalousFolder);
            }

            //Fix up paths based on the build name
            String buildUrlExtraPath = "";
            String localDataFileFolder = FolderFinder.LocalDataFolder;
            if (!String.IsNullOrEmpty(BuildName))
            {
                buildUrlExtraPath = "/" + BuildName;
                localDataFileFolder = Path.Combine(localDataFileFolder, BuildName);
            }

            //Setup local private folder
            if(!Directory.Exists(FolderFinder.LocalPrivateDataFolder))
            {
                Directory.CreateDirectory(FolderFinder.LocalPrivateDataFolder);
            }

            //Setup local data folder
            CoreConfig.localDataFileFolder = localDataFileFolder;
            if (!Directory.Exists(localDataFileFolder))
            {
                Directory.CreateDirectory(localDataFileFolder);
            }
            TemporaryFilesPath = Path.Combine(localDataFileFolder, "Temp");
            if (!Directory.Exists(TemporaryFilesPath))
            {
                Directory.CreateDirectory(TemporaryFilesPath);
            }

            //User configuration settings
            configFile = new ConfigFile(ConfigFilePath);
            configFile.loadConfigFile();

            program = configFile.createOrRetrieveConfigSection("Program");
            sceneDirectory = "Scenes";

            cameraTransitionTime = program.getValue("CameraTransitionTime", 0.5f);
            transparencyChangeMultiplier = program.getValue("TransparencyChangeMultiplier", 2.0f);
            //ServerConnection.DefaultTimeout = program.getValue("DefaultTimeout", ServerConnection.DefaultTimeout);
            autoOpenAnatomyFinder = program.getValue("AutoOpenAnatomyFinder", true);

            EngineConfig = new EngineConfig(configFile);

            SafeDownloadFolder = Path.Combine(localDataFileFolder, "Downloads");
            if (!Directory.Exists(SafeDownloadFolder))
            {
                Directory.CreateDirectory(SafeDownloadFolder);
            }
        }

        public static String LogFile
        {
            get
            {
                return Path.Combine(userAnomalousFolder, "Log.log");
            }
        }

        public static String CrashLogDirectory
        {
            get
            {
                return Path.Combine(userAnomalousFolder, "CrashLogs");
            }
        }

        public static string ConfigFilePath
        {
            get
            {
                return Path.Combine(userAnomalousFolder, "config.ini");
            }
        }

        public static String SafeDownloadFolder { get; private set; }

        public static String TemporaryFilesPath { get; private set; }

        public static String UserDocRoot
        {
            get
            {
                return docRoot;
            }
        }

        public static ConfigFile ConfigFile
        {
            get
            {
                return configFile;
            }
        }

        public static bool EnableMultitouch
        {
            get
            {
                return program.getValue("EnableMultitouch", PlatformConfig.DefaultEnableMultitouch);
            }
            set
            {
                program.setValue("EnableMultitouch", value);
            }
        }

        public static UIExtraScale ExtraScaling
        {
            get
            {
                UIExtraScale extraScale;
                if (!Enum.TryParse<UIExtraScale>(program.getValue("ExtraScaling", () => UIExtraScale.Normal.ToString()), out extraScale))
                {
                    extraScale = UIExtraScale.Normal;
                }
                return extraScale;
            }
            set
            {
                program.setValue("ExtraScaling", value.ToString());
            }
        }

        public static String BuildName { get; private set; }

        public static EngineConfig EngineConfig { get; private set; }

        public static void save()
        {
            configFile.writeConfigFile();
        }
    }
}