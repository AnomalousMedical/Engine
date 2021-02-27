using BepuPhysics;
using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepuPlugin
{
    /// <summary>
    /// This class helps keep the position in sync between physics objects and their owner.
    /// </summary>
    public class BodyPositionSync
    {
        private readonly BodyReference bodyReference;

        public BodyPositionSync(BodyReference bodyReference)
        {
            this.bodyReference = bodyReference;
        }

        public Vector3 GetWorldPosition()
        {
            var bodPos = bodyReference.Pose.Position;
            return new Vector3(bodPos.X, bodPos.Y, bodPos.Z);
        }

        public Quaternion GetWorldOrientation()
        {
            var bodOrientation = bodyReference.Pose.Orientation;
            return new Quaternion(bodOrientation.X, bodOrientation.Y, bodOrientation.Z, bodOrientation.W);
        }
    }
}
