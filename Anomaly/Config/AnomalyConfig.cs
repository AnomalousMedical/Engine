using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using System.IO;

namespace Anomaly
{
    class AnomalyConfig
    {
        public const String RENDERER_HEADER = "Renderer";

        public const String FSAA_ENTRY = "FSAA";
        public const String FSAA_DEFAULT = "0";

        public const String VSYNC_ENTRY = "VSync";
        public const bool VSYNC_DEFAULT = false;

        public const String CAMERA_HEADER = "Cameras";
        public const String FRONT_CAMERA_POSITION_ENTRY = "FrontPosition";
        public static readonly Vector3 FRONT_CAMERA_POSITION_DEFAULT = new Vector3(0.0f, 0.0f, 150.0f);
        public const String FRONT_CAMERA_LOOKAT_ENTRY = "FrontLookAt";
        public static readonly Vector3 FRONT_CAMERA_LOOKAT_DEFAULT = Vector3.Zero;

        public const String LEFT_CAMERA_POSITION_ENTRY = "LeftPosition";
        public static readonly Vector3 LEFT_CAMERA_POSITION_DEFAULT = new Vector3(150.0f, 0.0f, 0.0f);
        public const String LEFT_CAMERA_LOOKAT_ENTRY = "LeftLookAt";
        public static readonly Vector3 LEFT_CAMERA_LOOKAT_DEFAULT = Vector3.Zero;

        public const String RIGHT_CAMERA_POSITION_ENTRY = "RightPosition";
        public static readonly Vector3 RIGHT_CAMERA_POSITION_DEFAULT = new Vector3(-150.0f, 0.0f, 0.0f);
        public const String RIGHT_CAMERA_LOOKAT_ENTRY = "RightLookAt";
        public static readonly Vector3 RIGHT_CAMERA_LOOKAT_DEFAULT = Vector3.Zero;

        public const String BACK_CAMERA_POSITION_ENTRY = "BackPosition";
        public static readonly Vector3 BACK_CAMERA_POSITION_DEFAULT = new Vector3(0.0f, 0.0f, -150.0f);
        public const String BACK_CAMERA_LOOKAT_ENTRY = "BackLookAt";
        public static readonly Vector3 BACK_CAMERA_LOOKAT_DEFAULT = Vector3.Zero;

        private static ConfigFile configFile;
        private static String docRoot = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Anomaly";

        static AnomalyConfig()
        {
            if (!Directory.Exists(docRoot))
            {
                Directory.CreateDirectory(docRoot);
            }
            configFile = new ConfigFile(docRoot + "/config.ini");
        }

        public static ConfigFile ConfigFile
        {
            get
            {
                return configFile;
            }
        }

        public static String DocRoot
        {
            get
            {
                return docRoot;
            }
        }

        private AnomalyConfig()
        {

        }
    }
}
