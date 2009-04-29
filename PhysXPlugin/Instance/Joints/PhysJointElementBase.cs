﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;
using PhysXWrapper;
using Engine;

namespace PhysXPlugin
{
    public abstract class PhysJointElementBase<JointType> : PhysJointElement
        where JointType : PhysJoint
    {
        protected Identifier jointId;
        protected JointType joint;
        private PhysXSceneManager scene;

        internal PhysJointElementBase(Identifier jointId, JointType joint, PhysXSceneManager scene, Subscription subscription)
            :base(jointId.ElementName, subscription)
        {
            this.jointId = jointId;
            this.joint = joint;
            this.scene = scene;
        }

        protected override void Dispose()
        {
            scene.destroyJoint(jointId);
        }

        protected override void updatePositionImpl(ref Vector3 translation, ref Quaternion rotation)
        {
            
        }

        protected override void updateTranslationImpl(ref Vector3 translation)
        {
            
        }

        protected override void updateRotationImpl(ref Quaternion rotation)
        {
            
        }

        protected override void updateScaleImpl(ref Vector3 scale)
        {
            
        }

        protected override void setEnabled(bool enabled)
        {
            //joints are not enabled or disabled.
        }

        public override PhysJoint Joint
        {
            get
            {
                return joint;
            }
        }

        public JointType RealJoint
        {
            get
            {
                return joint;
            }
        }
    }
}
