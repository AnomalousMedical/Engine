using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace SoundPlugin
{
    public class SoundState
    {
        private float masterVolume = 1.0f;
        public event Action<SoundState> MasterVolumeChanged;

        public SoundState(SoundPluginOptions options)
        {
            this.masterVolume = options.MasterVolume;
        }

        public float MasterVolume
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
                    MasterVolumeChanged.Invoke(this);
                }
            }
        }
    }
}
