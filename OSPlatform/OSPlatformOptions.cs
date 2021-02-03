using System;
using System.Collections.Generic;
using System.Text;

namespace Anomalous.OSPlatform
{
    public class OSPlatformOptions
    {
        /// <summary>
        /// The type of the class to use to specify event layers.
        /// </summary>
        public Type EventLayersType { get; set; } = typeof(EventLayers);
    }
}
