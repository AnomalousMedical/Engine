using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using System.IO;
using OgrePlugin;

namespace OgreModelEditor
{
    class OgreModelEditorConfig
    {
        private static ConfigFile configFile;
        private static String docRoot = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Anomalous Software/OgreModelEditor";
        private static ConfigSection modelEditorSection;

        static OgreModelEditorConfig()
        {
            if (!Directory.Exists(docRoot))
            {
                Directory.CreateDirectory(docRoot);
            }
            configFile = new ConfigFile(docRoot + "/config.ini");
            configFile.loadConfigFile();
            EngineConfig = new EngineConfig(configFile);
            CameraConfig = new CameraSection(configFile);

            modelEditorSection = configFile.createOrRetrieveConfigSection("ModelEditor");
        }

        public static String DocRoot
        {
            get
            {
                return docRoot;
            }
        }

        public static String VFSRoot
        {
            get
            {
                return modelEditorSection.getValue("VFSRoot", ".");
            }
            set
            {
                modelEditorSection.setValue("VFSRoot", value);
            }
        }

        public static String LastShaderVersion
        {
            get
            {
                return modelEditorSection.getValue("LastShaderVersion", "");
            }
            set
            {
                modelEditorSection.setValue("LastShaderVersion", value);
            }
        }

        public static String OpenGLESEmulatorPath
        {
            get
            {
                return modelEditorSection.getValue("OpenGLESEmulatorPath", default(String));
            }
            set
            {
                modelEditorSection.setValue("OpenGLESEmulatorPath", value);
            }
        }

        public static String LastTextureCompilerSourceDirectory
        {
            get
            {
                return modelEditorSection.getValue("LastTextureCompilerSourceDirectory", default(String));
            }
            set
            {
                modelEditorSection.setValue("LastTextureCompilerSourceDirectory", value);
            }
        }
        public static CompressedTextureSupport CompressedTextureSupport
        {
            get
            {
                CompressedTextureSupport type = CompressedTextureSupport.All;
                Enum.TryParse<CompressedTextureSupport>(modelEditorSection.getValue("CompressedTextureSupport", () => CompressedTextureSupport.All.ToString()), out type);
                return type;
            }
            set
            {
                modelEditorSection.setValue("CompressedTextureSupport", value.ToString());
            }
        }

        public static void save()
        {
            configFile.writeConfigFile();
        }

        public static ConfigFile ConfigFile
        {
            get
            {
                return configFile;
            }
        }

        public static EngineConfig EngineConfig { get; private set; }

        public static CameraSection CameraConfig { get; private set; }
    }
}
