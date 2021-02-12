using BepuPhysics;
using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BepuDemo
{
    /// <summary>
    /// This class helps keep the position in sync between physics objects and their owner.
    /// </summary>
    class BodyPositionSync
    {
        private readonly BodyReference bodyReference;

        public BodyPositionSync(BodyReference bodyReference)
        {
            this.bodyReference = bodyReference;
        }

        public Matrix4x4 GetWorldPositionMatrix()
        {
            var bodOrientation = bodyReference.Pose.Orientation;
            var bodPos = bodyReference.Pose.Position;
            var rot = new Quaternion(bodOrientation.X, bodOrientation.Y, bodOrientation.Z, bodOrientation.W);
            //rot = rot.inverse(); //Need this if you don't invert x and z below
            var pos = new Vector3(-bodPos.X, bodPos.Y, -bodPos.Z);//This position doesn't set correctly and rots are backward, something messed up with camera
                                                                  //Try this without the pbr renderer and see what happens

            return rot.toRotationMatrix4x4(pos);
        }
    }
}
