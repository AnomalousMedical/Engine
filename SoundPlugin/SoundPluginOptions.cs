using System;
using System.Collections.Generic;
using System.Text;

namespace SoundPlugin
{
    public class SoundPluginOptions
    {
        /// <summary>
        /// The master volume between 0.0 and 1.0. Default: 1.0.
        /// </summary>
        public float MasterVolume { get; set; } = 1.0f;
    }
}
