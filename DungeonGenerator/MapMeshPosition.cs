using Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonGenerator
{
    public class MapMeshPosition
    {
        public MapMeshPosition(Vector3 position, Quaternion orientation)
        {
            Position = position;
            Orientation = orientation;
        }

        public Vector3 Position { get; }

        public Quaternion Orientation { get; }
    }
}
