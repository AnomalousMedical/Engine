using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhysXWrapper;
using Engine.Editing;
using Engine.ObjectManagement;
using Engine.Saving;
using Engine.Reflection;

namespace PhysXPlugin
{
    class PhysRevoluteJointDefinition: PhysJointDefinitionBase<PhysRevoluteJointDesc, PhysRevoluteJoint>
    {
        #region Static

        private static MemberScanner memberScanner = new MemberScanner();

        static PhysRevoluteJointDefinition()
        {
            EditableAttributeFilter filter = new EditableAttributeFilter();
            filter.TerminatingType = typeof(PhysJointDefinitionBase<PhysRevoluteJointDesc, PhysRevoluteJoint>);
            memberScanner.Filter = filter;
        }

        /// <summary>
        /// Create function for commands.
        /// </summary>
        /// <param name="name">The name of the definition to create.</param>
        /// <returns>A new definition.</returns>
        internal static PhysRevoluteJointDefinition Create(String name, EditUICallback callback)
        {
            return new PhysRevoluteJointDefinition(name);
        }

        #endregion Static

        [Editable] private PhysJointLimitPairDescription limit;
        [Editable] private PhysMotorDescription motor;
        [Editable] private PhysSpringDescription spring;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the joint.</param>
        public PhysRevoluteJointDefinition(String name)
            :base(new PhysRevoluteJointDesc(), name, "Revolute Joint", null)
        {
            configureSubParts();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the joint.</param>
        /// <param name="joint">An existing PhysJointElement to get values from.</param>
        internal PhysRevoluteJointDefinition(String name, PhysRevoluteJointElement joint)
            :base(new PhysRevoluteJointDesc(), joint, name, "Revolute Joint", null)
        {
            joint.RealJoint.saveToDesc(jointDesc);
            configureSubParts();
        }

        protected void configureSubParts()
        {
            //Create the sub parts
            limit = new PhysJointLimitPairDescription();
            motor = new PhysMotorDescription();
            spring = new PhysSpringDescription();

            //Copy values for sub parts
            limit.copyFromDescription(jointDesc.Limit);
            motor.copyFromDescription(jointDesc.Motor);
            spring.copyFromDescription(jointDesc.Spring);
        }


        /// <summary>
        /// This function is called just before the joint is created. This
        /// allows any subclass configuration of the joint description to take
        /// place.
        /// </summary>
        protected override void configureJoint()
        {
            //copy sub parts values
            limit.copyToDescription(jointDesc.Limit);
            motor.copyToDescription(jointDesc.Motor);
            spring.copyToDescription(jointDesc.Spring);
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
            return new PhysRevoluteJointElement(jointId, (PhysRevoluteJoint)joint, scene, subscription);
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
        public float ProjectionAngle
        {
            get
            {
                return jointDesc.ProjectionAngle;
            }
            set
            {
                jointDesc.ProjectionAngle = value;
            }
        }

        [Editable]
        public RevoluteJointFlag Flags
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

        private const String LIMIT = "PhysRevoluteJointDefinitionlimit";
        private const String MOTOR = "PhysRevoluteJointDefinitionmotor";
        private const String SPRING = "PhysRevoluteJointDefinitionspring";

        private const String PROJECTION_DISTANCE = "PhysRevoluteJointDefinitionProjectionDistance";
        private const String PROJECTION_ANGLE = "PhysRevoluteJointDefinitionProjectionAngle";
        private const String FLAGS = "PhysRevoluteJointDefinitionFlags";
        private const String PROJECTION_MODE = "PhysRevoluteJointDefinitionProjectionMode";

        /// <summary>
        /// Load constructor.
        /// </summary>
        /// <param name="loadInfo"></param>
        private PhysRevoluteJointDefinition(LoadInfo info)
            :base(new PhysRevoluteJointDesc(), info)
        {
            limit = info.GetValue<PhysJointLimitPairDescription>(LIMIT);
            motor = info.GetValue<PhysMotorDescription>(MOTOR);
            spring = info.GetValue<PhysSpringDescription>(SPRING);

            ProjectionDistance = info.GetFloat(PROJECTION_DISTANCE);
            ProjectionAngle = info.GetFloat(PROJECTION_ANGLE);
            Flags = info.GetValue<RevoluteJointFlag>(FLAGS);
            ProjectionMode = info.GetValue<JointProjectionMode>(PROJECTION_MODE);
        }

        /// <summary>
        /// Save function.
        /// </summary>
        /// <param name="info"></param>
        protected override void getJointInfo(SaveInfo info)
        {
            info.AddValue(LIMIT, limit);
            info.AddValue(MOTOR, motor);
            info.AddValue(SPRING, spring);

            info.AddValue(PROJECTION_DISTANCE, ProjectionDistance);
            info.AddValue(PROJECTION_ANGLE, ProjectionAngle);
            info.AddValue(FLAGS, Flags);
            info.AddValue(PROJECTION_MODE, ProjectionMode);
        }

        #endregion Saveable
    }
}
