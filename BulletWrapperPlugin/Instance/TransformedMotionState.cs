using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulletPlugin
{
    class TransformedMotionState : MotionState
    {
        private TransformedBulletScene scene;

        public TransformedMotionState(TransformedBulletScene scene, RigidBody rigidBody, float maxContactDistance, ref Vector3 initialTrans, ref Quaternion initialRot)
            :base(rigidBody, maxContactDistance, ref initialTrans, ref initialRot)
        {
            this.scene = scene;
        }

        protected override void motionStateCallback(ref Vector3 trans, ref Quaternion rot)
        {
            Vector3 extraTrans = scene.TransformSimObject.Translation;
            Quaternion extraRot = scene.TransformSimObject.Rotation;

            extraTrans += Quaternion.quatRotate(ref extraRot, ref trans);
            extraRot = extraRot * rot;

            rigidBody.updateObjectPosition(ref extraTrans, ref extraRot);
        }
    }
}
