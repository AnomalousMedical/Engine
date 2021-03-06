using Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonGenerator
{
    struct MapMeshTempSquareInfo
    {
        public float LeftFarY;

        public float RightFarY;

        public float RightNearY;

        public float LeftNearY;

        public bool Visited;

        public MapMeshTempSquareInfo(float leftFarY, float rightFarY, float rightNearY, float leftNearY)
        {
            LeftFarY = leftFarY;
            RightFarY = rightFarY;
            RightNearY = rightNearY;
            LeftNearY = leftNearY;
            Visited = false;
        }
    }
}
