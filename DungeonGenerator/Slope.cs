using Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonGenerator
{
    struct Slope
    {
        public float YOffset;

        public IntVector2 PreviousPoint { get; set; }

        /// <summary>
        /// The center is calculated as we move through the array
        /// </summary>
        public Vector3 Center { get; set; }
    }
}
