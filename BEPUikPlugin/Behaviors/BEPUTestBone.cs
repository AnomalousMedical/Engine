using BEPUik;
using Engine;
using Engine.Attributes;
using Engine.Editing;
using Engine.ObjectManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BEPUikPlugin.IKTest
{
    class BEPUTestBone : Interface
    {
        [DoNotCopy]
        [DoNotSave]
        Bone bone;

        [Editable]
        private String ikSolverSimObjectName = "IKSolver";

        [Editable]
        private String ikSolverName = "IKSolver";

        [Editable]
        private bool pinned = false;

        protected override void constructed()
        {
            base.constructed();
            
            bone = new Bone(Owner.Translation.toBepuVec3(), Owner.Rotation.toBepuQuat(), 1, 3);
            bone.Pinned = pinned;
        }

        protected override void link()
        {
            base.link();

            SimObject ikSolverSimObj = Owner.getOtherSimObject(ikSolverSimObjectName);

            BEPUikManager ikSolver = ikSolverSimObj.getElement(ikSolverName) as BEPUikManager;
            ikSolver.addBone(this);
        }

        public void syncSimObject()
        {
            Vector3 trans = bone.Position.toEngineVec3();
            Quaternion rot = bone.Orientation.toEngineQuat();

            updatePosition(ref trans, ref rot);
        }

        public void syncBone()
        {
            bone.Position = Owner.Translation.toBepuVec3();
            bone.Orientation = Owner.Rotation.toBepuQuat();
        }

        public Bone Bone
        {
            get
            {
                return bone;
            }
        }
    }
}
