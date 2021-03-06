using Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonGenerator
{
    struct MapMeshSquareInfo
    {
        public Vector3 Center;

        public float HalfYOffset;

        public MapMeshSquareInfo(Vector3 center, float halfHeight)
        {
            this.Center = center;
            this.HalfYOffset = halfHeight;
        }
    }
}
