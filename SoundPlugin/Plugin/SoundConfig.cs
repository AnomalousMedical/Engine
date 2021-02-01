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

        private static float masterVolume = 1.0f;

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

                if (MasterVolumeChanged != null)
                {
                    MasterVolumeChanged.Invoke(null, EventArgs.Empty);
                }
            }
        }
    }
}
