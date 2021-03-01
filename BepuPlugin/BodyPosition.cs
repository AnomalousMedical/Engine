using BepuPhysics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace BepuPlugin
{
    class BodyPosition
    {
        public BodyPosition(in BodyHandle bodyHandle, in Vector3 warmupPosition, in Quaternion warmupOrientation)
        {
            BodyHandle = bodyHandle;
            Position = warmupPosition;
            LastPosition = warmupPosition;
            Orientation = warmupOrientation;
            LastOrientation = warmupOrientation;
        }

        public BodyHandle BodyHandle;

        public Vector3 Position;

        public Quaternion Orientation;

        public Vector3 LastPosition;

        public Quaternion LastOrientation;
    }
}
