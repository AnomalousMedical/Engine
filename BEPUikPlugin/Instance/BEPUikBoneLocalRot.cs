using BEPUik;
using Engine;
using Engine.ObjectManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEPUikPlugin
{
    class BEPUikBoneLocalRot : BEPUikBone
    {
        private Quaternion localRot;
        private Quaternion localRotInverse;

        public BEPUikBoneLocalRot(BEPUikBoneDefinition definition, SimObjectBase instance, BEPUikScene scene)
            :base(definition, instance, scene)
        {
            localRot = definition.LocalRotQuat.Value;
            localRotInverse = localRot.inverse();
            IkBone.Orientation *= localRot.toBepuQuat();
        }

        protected override void updatePositionImpl(ref Vector3 translation, ref Quaternion rotation)
        {
            IkBone.Position = translation.toBepuVec3();
            IkBone.Orientation = (rotation * localRot).toBepuQuat();
        }

        protected override void updateRotationImpl(ref Quaternion rotation)
        {
            IkBone.Orientation = (rotation * localRot).toBepuQuat();
        }

        protected override void customizeDefinition(BEPUikBoneDefinition definition)
        {
            definition.LocalRotQuat = localRot;
        }

        internal override void syncSimObject()
        {
            Vector3 trans = IkBone.Position.toEngineVec3();
            Quaternion rot = IkBone.Orientation.toEngineQuat() * localRotInverse;

            updatePosition(ref trans, ref rot);
        }
    }
}
