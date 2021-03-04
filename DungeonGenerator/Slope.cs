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
    }
}
