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

        public Vector3 LeftFar;

        public Vector3 RightFar;

        public Vector3 RightNear;

        public Vector3 LeftNear;

        public MapMeshSquareInfo(Vector3 center, float halfHeight)
        {
            this.Center = center;
            this.HalfYOffset = halfHeight;
            LeftFar = new Vector3();
            RightFar = new Vector3();
            RightNear = new Vector3();
            LeftNear = new Vector3();
        }
    }
}
