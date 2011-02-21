using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace SoundPlugin
{
    public class SoundConfig
    {
        public static event EventHandler MasterVolumeChanged;

        private static ConfigSection soundSection;
        private static float masterVolume;

        public static void initialize(ConfigFile configFile)
        {
            soundSection = configFile.createOrRetrieveConfigSection("Sound");
            MasterVolume = soundSection.getValue("MasterVolume", 1.0f);
        }

        public static float MasterVolume
        {
            get
            {
                return masterVolume;
            }
            set
            {
                masterVolume = value;
                if (masterVolume < 0.025)
                {
                    masterVolume = 0.0f;
                }
                else if (masterVolume > 1.0f)
                {
                    masterVolume = 1.0f;
                }
                soundSection.setValue("MasterVolume", masterVolume);
                if (MasterVolumeChanged != null)
                {
                    MasterVolumeChanged.Invoke(null, EventArgs.Empty);
                }
            }
        }
    }
}
