using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace Anomaly
{
    public class CameraSection
    {
        private const String Header = "Cameras";
        private const String MainPosition = "MainCameraPosition";
        private static readonly Vector3 MainDefaultPosition = new Vector3(0.0f, 0.0f, 150.0f);
        private const String MainLookAt = "MainCameraLookAt";
        private static readonly Vector3 MainDefaultLookAt = Vector3.Zero;

        private ConfigSection configSection;

        public CameraSection(ConfigFile configFile)
        {
            configSection = configFile.createOrRetrieveConfigSection(Header);
        }

        public Vector3 MainCameraPosition
        {
            get
            {
                return configSection.getValue(MainPosition, MainDefaultPosition);
            }
            set
            {
                configSection.setValue(MainPosition, value);
            }
        }

        public Vector3 MainCameraLookAt
        {
            get
            {
                return configSection.getValue(MainLookAt, MainDefaultLookAt);
            }
            set
            {
                configSection.setValue(MainLookAt, value);
            }
        }
    }
}
