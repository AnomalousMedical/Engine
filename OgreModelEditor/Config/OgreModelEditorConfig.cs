using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using System.IO;

namespace OgreModelEditor
{
    class OgreModelEditorConfig
    {
        private static ConfigFile configFile;
        private static String docRoot = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/OgreModelEditor";

        static OgreModelEditorConfig()
        {
            if (!Directory.Exists(docRoot))
            {
                Directory.CreateDirectory(docRoot);
            }
            configFile = new ConfigFile(docRoot + "/config.ini");
            configFile.loadConfigFile();
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

                public static ConfigFile ConfigFile
        {
            get
            {
                return configFile;
            }
        }
    }
}
