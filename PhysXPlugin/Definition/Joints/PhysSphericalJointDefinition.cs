using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhysXWrapper;
using Engine.Editing;
using Engine.ObjectManagement;
using Engine.Saving;
using Engine.Reflection;
using Engine;

namespace PhysXPlugin
{
    class PhysSphericalJointDefinition: PhysJointDefinitionBase<PhysSphericalJointDesc, PhysSphericalJoint>
    {
        #region Static

        private static MemberScanner memberScanner = new MemberScanner();

        static PhysSphericalJointDefinition()
        {
            EditableAttributeFilter filter = new EditableAttributeFilter();
            filter.TerminatingType = typeof(PhysJointDefinitionBase<PhysSphericalJointDesc, PhysSphericalJoint>);
            memberScanner.Filter = filter;
        }

        /// <summary>
        /// Create function for commands.
        /// </summary>
        /// <param name="name">The name of the definition to create.</param>
        /// <returns>A new definition.</returns>
        internal static PhysSphericalJointDefinition Create(String name, EditUICallback callback)
        {
            return new PhysSphericalJointDefinition(name);
        }

        #endregion Static

        [Editable] private PhysJointLimitPairDescription twistLimit;
        [Editable] private PhysJointLimitDescription swingLimit;
        [Editable] private PhysSpringDescription twistSpring;
        [Editable] private PhysSpringDescription swingSpring;
        [Editable] private PhysSpringDescription jointSpring;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the joint.</param>
        public PhysSphericalJointDefinition(String name)
            :base(new PhysSphericalJointDesc(), name, "Spherical Joint", null)
        {
            configureSubParts();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the joint.</param>
        /// <param name="joint">An existing PhysJointElement to get values from.</param>
        internal PhysSphericalJointDefinition(String name, PhysSphericalJointElement joint)
            :base(new PhysSphericalJointDesc(), joint, name, "Spherical Joint", null)
        {
            joint.RealJoint.saveToDesc(jointDesc);
            configureSubParts();
        }

        protected void configureSubParts()
        {
            //Create the sub parts
            twistLimit = new PhysJointLimitPairDescription();
            swingLimit = new PhysJointLimitDescription();
            twistSpring = new PhysSpringDescription();
            swingSpring = new PhysSpringDescription();
            jointSpring = new PhysSpringDescription();

            //Copy values for sub parts
            twistLimit.copyFromDescription(jointDesc.TwistLimit);
            swingLimit.copyFromDescription(jointDesc.SwingLimit);
            twistSpring.copyFromDescription(jointDesc.TwistSpring);
            swingSpring.copyFromDescription(jointDesc.SwingSpring);
            jointSpring.copyFromDescription(jointDesc.JointSpring);
        }

        /// <summary>
        /// This function is called just before the joint is created. This
        /// allows any subclass configuration of the joint description to take
        /// place.
        /// </summary>
        protected override void configureJoint()
        {
            //copy sub parts values
            twistLimit.copyToDescription(jointDesc.TwistLimit);
            swingLimit.copyToDescription(jointDesc.SwingLimit);
            twistSpring.copyToDescription(jointDesc.TwistSpring);
            swingSpring.copyToDescription(jointDesc.SwingSpring);
            jointSpring.copyToDescription(jointDesc.JointSpring);
        }

        /// <summary>
        /// Get any specific information for the EditInterface from a subclass.
        /// </summary>
        /// <param name="editInterface">The EditInterface to populate.</param>
        protected override void configureEditInterface(EditInterface editInterface)
        {
            ReflectedEditInterface.expandEditInterface(this, memberScanner, editInterface);
        }

        /// <summary>
        /// Create the PhysJointElement specific to this Joint.
        /// </summary>
        /// <param name="jointId">The Identifier of the joint being created.</param>
        /// <param name="joint">The joint that was created.</param>
        /// <param name="scene">The scene that contains the joint.</param>
        /// <returns>A new PhysJointElement for this specific joint type.</returns>
        internal override PhysJointElement createElement(Identifier jointId, PhysJoint joint, PhysXSceneManager scene)
        {
            return new PhysSphericalJointElement(jointId, (PhysSphericalJoint)joint, scene, subscription);
        }

        [Editable]
        public Vector3 SwingAxis
        {
            get
            {
                return jointDesc.SwingAxis;
            }
            set
            {
                jointDesc.SwingAxis = value;
            }
        }

        [Editable]
        public float ProjectionDistance
        {
            get
            {
                return jointDesc.ProjectionDistance;
            }
            set
            {
                jointDesc.ProjectionDistance = value;
            }
        }

        [Editable]
        public SphericalJointFlag Flags
        {
            get
            {
                return jointDesc.Flags;
            }
            set
            {
                jointDesc.Flags = value;
            }
        }

        [Editable]
        public JointProjectionMode ProjectionMode
        {
            get
            {
                return jointDesc.ProjectionMode;
            }
            set
            {
                jointDesc.ProjectionMode = value;
            }
        }

        #region Saveable

        private const String TWIST_LIMIT = "PhysSphericalJointDefinitiontwistLimit";
        private const String SWING_LIMIT = "PhysSphericalJointDefinitionswingLimit";
        private const String TWIST_SPRING = "PhysSphericalJointDefinitiontwistSpring";
        private const String SWING_SPRING = "PhysSphericalJointDefinitionswingSpring";
        private const String JOINT_SPRING = "PhysSphericalJointDefinitionjointSpring";

        private const String SWING_AXIS = "PhysSphericalJointDefinitionSwingAxis";
        private const String PROJECTION_DISTANCE = "PhysSphericalJointDefinitionProjectionDistance";
        private const String FLAGS = "PhysSphericalJointDefinitionFlags";
        private const String PROJECTION_MODE = "PhysSphericalJointDefinitionProjectionMode";

        /// <summary>
        /// Load constructor.
        /// </summary>
        /// <param name="loadInfo"></param>
        private PhysSphericalJointDefinition(LoadInfo info)
            : base(new PhysSphericalJointDesc(), info)
        {
            twistLimit = info.GetValue<PhysJointLimitPairDescription>(TWIST_LIMIT);
            swingLimit = info.GetValue<PhysJointLimitDescription>(SWING_LIMIT);
            twistSpring = info.GetValue<PhysSpringDescription>(TWIST_SPRING);
            swingSpring = info.GetValue<PhysSpringDescription>(SWING_SPRING);
            jointSpring = info.GetValue<PhysSpringDescription>(JOINT_SPRING);

            SwingAxis = info.GetVector3(SWING_AXIS);
            ProjectionDistance = info.GetFloat(PROJECTION_DISTANCE);
            Flags = info.GetValue<SphericalJointFlag>(FLAGS);
            ProjectionMode = info.GetValue<JointProjectionMode>(PROJECTION_MODE);
        }

        /// <summary>
        /// Save function.
        /// </summary>
        /// <param name="info"></param>
        protected override void getJointInfo(SaveInfo info)
        {
            info.AddValue(TWIST_LIMIT, twistLimit);
            info.AddValue(SWING_LIMIT, swingLimit);
            info.AddValue(TWIST_SPRING, twistSpring);
            info.AddValue(SWING_SPRING, swingSpring);
            info.AddValue(JOINT_SPRING, jointSpring);

            info.AddValue(SWING_AXIS, SwingAxis);
            info.AddValue(PROJECTION_DISTANCE, ProjectionDistance);
            info.AddValue(FLAGS, Flags);
            info.AddValue(PROJECTION_MODE, ProjectionMode);
        }

        #endregion Saveable
    }
}
