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
    public abstract class BEPUikJoint : BEPUikConstraint
    {
        private BEPUikBone connectionA;
        private BEPUikBone connectionB;
        private Vector3 connectionAPositionOffset;

        public BEPUikJoint(BEPUikBone connectionA, BEPUikBone connectionB, String name, SimObject instance)
            :base(name)
        {
            this.connectionA = connectionA;
            this.connectionB = connectionB;
            connectionAPositionOffset = instance.Translation - connectionA.Owner.Translation;
        }

        protected override void Dispose()
        {
            IKJoint.Enabled = false;
        }

        protected void setupJoint(BEPUikJointDefinition definition)
        {
            setupConstraint(definition);
        }

        protected void setupJointDefinition(BEPUikJointDefinition definition)
        {
            definition.ConnectionABoneName = connectionA.Name;
            definition.ConnectionASimObjectName = connectionA.Owner == Owner ? "this" : connectionA.Owner.Name;
            definition.ConnectionBBoneName = connectionB.Name;
            definition.ConnectionBSimObjectName = connectionB.Owner == Owner ? "this" : connectionB.Owner.Name;

            setupConstraintDefinition(definition);
        }

        internal override sealed IKConstraint IKConstraint
        {
            get
            {
                return IKJoint;
            }
        }

        public BEPUikBone ConnectionA
        {
            get
            {
                return connectionA;
            }
        }

        public BEPUikBone ConnectionB
        {
            get
            {
                return connectionB;
            }
        }

        protected Vector3 VisualizationOrigin
        {
            get
            {
                return ConnectionA.Owner.Translation + Quaternion.quatRotate(ConnectionA.Owner.Rotation, connectionAPositionOffset);
            }
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
            IKJoint.Enabled = enabled;
        }

        internal abstract IKJoint IKJoint { get; }
    }
}
