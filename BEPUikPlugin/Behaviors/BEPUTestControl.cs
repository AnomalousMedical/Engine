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
    class BEPUTestControl : Interface
    {
        [DoNotCopy]
        [DoNotSave]
        DragControl dragControl;

        [Editable]
        private String boneSimObjectName = "this";

        [Editable]
        private String boneName = "Bone";

        [Editable]
        private String ikSolverSimObjectName = "IKSolver";

        [Editable]
        private String ikSolverName = "IKSolver";

        protected override void link()
        {
            base.constructed();

            SimObject boneSimObject = Owner.getOtherSimObject(boneSimObjectName);
            if (boneSimObject == null)
            {
                blacklist("Cannot find drag control bone sim object named '{0}'", boneSimObjectName);
            }

            BEPUTestBone bone = boneSimObject.getElement(boneName) as BEPUTestBone;
            if (bone == null)
            {
                blacklist("Cannot find drag control bone named '{0}'", boneName);
            }

            dragControl = new DragControl();
            dragControl.TargetBone = bone.Bone;

            SimObject ikSolverSimObj = Owner.getOtherSimObject(ikSolverSimObjectName);

            BEPUikManager ikSolver = ikSolverSimObj.getElement(ikSolverName) as BEPUikManager;
            ikSolver.addController(this);
        }

        public Control Control
        {
            get
            {
                return dragControl;
            }
        }

        internal void syncPosition()
        {
            dragControl.LinearMotor.TargetPosition = Owner.Translation.toBepuVec3();
        }
    }
}
