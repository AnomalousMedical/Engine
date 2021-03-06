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

        public float LeftFarY;

        public float RightFarY;

        public float RightNearY;

        public float LeftNearY;

        public bool Visited;

        public MapMeshSquareInfo(Vector3 center, float halfHeight)
        {
            this.Center = center;
            this.HalfYOffset = halfHeight;
            LeftFarY = 0;
            RightFarY = 0;
            RightNearY = 0;
            LeftNearY = 0;
            Visited = false;
        }
    }
}
