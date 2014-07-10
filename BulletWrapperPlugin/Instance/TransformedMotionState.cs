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

        public override Vector3 WorldTranslation
        {
            get
            {
                Vector3 extraTrans = scene.TransformSimObject.Translation;
                Quaternion extraRot = scene.TransformSimObject.Rotation;
                extraTrans += Quaternion.quatRotate(ref extraRot, ref localTranslation);
                return extraTrans;
            }
        }

        public override Quaternion WorldRotation
        {
            get
            {
                return scene.TransformSimObject.Rotation * localRotation;
            }
        }
    }
}
