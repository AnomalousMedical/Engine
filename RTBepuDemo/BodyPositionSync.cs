using BepuPhysics;
using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTBepuDemo
{
    /// <summary>
    /// This class helps keep the position in sync between physics objects and their owner.
    /// </summary>
    class BodyPositionSync
    {
        private readonly BodyHandle bodyHandle;
        private readonly Simulation simulation;

        public BodyPositionSync(BodyHandle bodyHandle, Simulation simulation)
        {
            this.bodyHandle = bodyHandle;
            this.simulation = simulation;
        }

        public Vector3 GetWorldPosition()
        {
            var bodyReference = simulation.Bodies.GetBodyReference(bodyHandle);
            var bodPos = bodyReference.Pose.Position;
            return new Vector3(bodPos.X, bodPos.Y, bodPos.Z);
        }

        public Quaternion GetWorldOrientation()
        {
            var bodyReference = simulation.Bodies.GetBodyReference(bodyHandle);
            var bodOrientation = bodyReference.Pose.Orientation;
            return new Quaternion(bodOrientation.X, bodOrientation.Y, bodOrientation.Z, bodOrientation.W);
        }
    }
}
