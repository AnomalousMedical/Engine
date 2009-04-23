using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhysXWrapper;
using Engine.Saving;
using Engine.Editing;
using Engine.ObjectManagement;
using EngineMath;
using Engine.Reflection;

namespace PhysXPlugin
{
    public class PhysD6JointDefinition: PhysJointDefinitionBase<PhysD6JointDesc, PhysD6Joint>
    {
        #region Static

        private static MemberScanner memberScanner = new MemberScanner();

        static PhysD6JointDefinition()
        {
            memberScanner.Filter = EditableAttributeFilter.Instance;
            memberScanner.TerminatingType = typeof(PhysJointDefinitionBase<PhysD6JointDesc, PhysD6Joint>);
        }

        /// <summary>
        /// Create function for commands.
        /// </summary>
        /// <param name="name">The name of the definition to create.</param>
        /// <returns>A new definition.</returns>
        internal static PhysD6JointDefinition Create(String name)
        {
            return new PhysD6JointDefinition(name);
        }

        #endregion Static

        [Editable] private PhysJointLimitSoftDescription linearLimit;
        [Editable] private PhysJointLimitSoftDescription swing1Limit;
        [Editable] private PhysJointLimitSoftDescription swing2Limit;
        [Editable] private PhysJointLimitSoftPairDescription twistLimit;
        [Editable] private PhysJointDriveDescription xDrive;
        [Editable] private PhysJointDriveDescription yDrive;
        [Editable] private PhysJointDriveDescription zDrive;
        [Editable] private PhysJointDriveDescription swingDrive;
        [Editable] private PhysJointDriveDescription twistDrive;
        [Editable] private PhysJointDriveDescription slerpDrive;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the joint.</param>
        public PhysD6JointDefinition(String name)
            :base(new PhysD6JointDesc(), name, "D6 Joint", null)
        {
            configureSubParts();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the joint.</param>
        /// <param name="joint">An existing PhysJointElement to get values from.</param>
        internal PhysD6JointDefinition(String name, PhysD6JointElement joint)
            :base(new PhysD6JointDesc(), joint, name, "D6 Joint", null)
        {
            joint.RealJoint.saveToDesc(jointDesc);
            configureSubParts();
        }

        private void configureSubParts()
        {
            //Create the sub parts
            linearLimit = new PhysJointLimitSoftDescription();
            swing1Limit = new PhysJointLimitSoftDescription();
            swing2Limit = new PhysJointLimitSoftDescription();
            twistLimit = new PhysJointLimitSoftPairDescription();
            xDrive = new PhysJointDriveDescription();
            yDrive = new PhysJointDriveDescription();
            zDrive = new PhysJointDriveDescription();
            swingDrive = new PhysJointDriveDescription();
            twistDrive = new PhysJointDriveDescription();
            slerpDrive = new PhysJointDriveDescription();

            //Copy values for sub parts
            linearLimit.copyFromDescription(jointDesc.LinearLimit);
            swing1Limit.copyFromDescription(jointDesc.Swing1Limit);
            swing2Limit.copyFromDescription(jointDesc.Swing2Limit);
            twistLimit.copyFromDescription(jointDesc.TwistLimit);
            xDrive.copyFromDescription(jointDesc.XDrive);
            yDrive.copyFromDescription(jointDesc.YDrive);
            zDrive.copyFromDescription(jointDesc.ZDrive);
            swingDrive.copyFromDescription(jointDesc.SwingDrive);
            twistDrive.copyFromDescription(jointDesc.TwistDrive);
            slerpDrive.copyFromDescription(jointDesc.SlerpDrive);
        }


        /// <summary>
        /// This function is called just before the joint is created. This
        /// allows any subclass configuration of the joint description to take
        /// place.
        /// </summary>
        protected override void configureJoint()
        {
            //Copy the sub parts
            linearLimit.copyToDescription(jointDesc.LinearLimit);
            swing1Limit.copyToDescription(jointDesc.Swing1Limit);
            swing2Limit.copyToDescription(jointDesc.Swing2Limit);
            twistLimit.copyToDescription(jointDesc.TwistLimit);
            xDrive.copyToDescription(jointDesc.XDrive);
            yDrive.copyToDescription(jointDesc.YDrive);
            zDrive.copyToDescription(jointDesc.ZDrive);
            swingDrive.copyToDescription(jointDesc.SwingDrive);
            twistDrive.copyToDescription(jointDesc.TwistDrive);
            slerpDrive.copyToDescription(jointDesc.SlerpDrive);
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
            return new PhysD6JointElement(jointId, (PhysD6Joint)joint, scene, subscription);
        }

        [Editable]
        public D6JointMotion XMotion
        {
            get
            {
                return jointDesc.XMotion;
            }
            set
            {
                jointDesc.XMotion = value;
            }
        }

        [Editable]
        public D6JointMotion YMotion
        {
            get
            {
                return jointDesc.YMotion;
            }
            set
            {
                jointDesc.YMotion = value;
            }
        }

        [Editable]
        public D6JointMotion ZMotion
        {
            get
            {
                return jointDesc.ZMotion;
            }
            set
            {
                jointDesc.ZMotion = value;
            }
        }

        [Editable]
        public D6JointMotion Swing1Motion
        {
            get
            {
                return jointDesc.Swing1Motion;
            }
            set
            {
                jointDesc.Swing1Motion = value;
            }
        }

        [Editable]
        public D6JointMotion Swing2Motion
        {
            get
            {
                return jointDesc.Swing2Motion;
            }
            set
            {
                jointDesc.Swing2Motion = value;
            }
        }

        [Editable]
        public D6JointMotion TwistMotion
        {
            get
            {
                return jointDesc.TwistMotion;
            }
            set
            {
                jointDesc.TwistMotion = value;
            }
        }

        //desc subclasses go here

        [Editable]
        public Vector3 DrivePosition
        {
            get
            {
                return jointDesc.DrivePosition;
            }
            set
            {
                jointDesc.DrivePosition = value;
            }
        }

        [Editable]
        public Quaternion DriveOrientation
        {
            get
            {
                return jointDesc.DriveOrientation;
            }
            set
            {
                jointDesc.DriveOrientation = value;
            }
        }

        [Editable]
        public Vector3 DriveLinearVelocity
        {
            get
            {
                return jointDesc.DriveLinearVelocity;
            }
            set
            {
                jointDesc.DriveLinearVelocity = value;
            }
        }

        [Editable]
        public Vector3 DriveAngularVelocity
        {
            get
            {
                return jointDesc.DriveAngularVelocity;
            }
            set
            {
                jointDesc.DriveAngularVelocity = value;
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
        public float GearRatio
        {
            get
            {
                return jointDesc.GearRatio;
            }
            set
            {
                jointDesc.GearRatio = value;
            }
        }

        [Editable]
        public D6JointFlag Flags
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

        #region Saveable

        public const String LINEAR_LIMIT = "PhysD6JointDefinitionlinearLimit";
        public const String SWING_1_LIMIT = "PhysD6JointDefinitionswing1Limit";
        public const String SWING_2_LIMIT = "PhysD6JointDefinitionswing2Limit";
        public const String TWIST_LIMIT = "PhysD6JointDefinitiontwistLimit";
        public const String X_DRIVE = "PhysD6JointDefinitionxDrive";
        public const String Y_DRIVE = "PhysD6JointDefinitionyDrive";
        public const String Z_DRIVE = "PhysD6JointDefinitionzDrive";
        public const String SWING_DRIVE = "PhysD6JointDefinitionswingDrive";
        public const String TWIST_DRIVE = "PhysD6JointDefinitiontwistDrive";
        public const String SLERP_DRIVE = "PhysD6JointDefinitionslerpDrive";

        public const String X_MOTION = "PhysD6JointDefinitionXMotion";
        public const String Y_MOTION = "PhysD6JointDefinitionYMotion";
        public const String Z_MOTION = "PhysD6JointDefinitionZMotion";
        public const String SWING_1_MOTION = "PhysD6JointDefinitionSwing1Motion";
        public const String SWING_2_MOTION = "PhysD6JointDefinitionSwing2Motion";
        public const String TWIST_MOTION = "PhysD6JointDefinitionTwistMotion";
        public const String DRIVE_POSITION = "PhysD6JointDefinitionDrivePosition";
        public const String DRIVE_ORIENTATION = "PhysD6JointDefinitionDriveOrientation";
        public const String DRIVE_LINEAR_VELOCITY = "PhysD6JointDefinitionDriveLinearVelocity";
        public const String DRIVE_ANGULAR_VELOCITY = "DPhysD6JointDefinitionriveAngularVelocity";
        public const String PROJECTION_MODE = "PhysD6JointDefinitionProjectionMode";
        public const String PROJECTION_DISTANCE = "PhysD6JointDefinitionProjectionDistance";
        public const String PROJECTION_ANGLE = "PhysD6JointDefinitionProjectionAngle";
        public const String GEAR_RATIO = "PhysD6JointDefinitionGearRatio";
        public const String FLAGS = "PhysD6JointDefinitionFlags";

        /// <summary>
        /// Load constructor.
        /// </summary>
        /// <param name="loadInfo"></param>
        private PhysD6JointDefinition(LoadInfo info)
            : base(new PhysD6JointDesc(), info)
        {
            linearLimit = info.GetValue<PhysJointLimitSoftDescription>(LINEAR_LIMIT);
            swing1Limit = info.GetValue<PhysJointLimitSoftDescription>(SWING_1_LIMIT);
            swing2Limit = info.GetValue<PhysJointLimitSoftDescription>(SWING_2_LIMIT);
            twistLimit = info.GetValue<PhysJointLimitSoftPairDescription>(TWIST_LIMIT);
            xDrive = info.GetValue<PhysJointDriveDescription>(X_DRIVE);
            yDrive = info.GetValue<PhysJointDriveDescription>(Y_DRIVE);
            zDrive = info.GetValue<PhysJointDriveDescription>(Z_DRIVE);
            swingDrive = info.GetValue<PhysJointDriveDescription>(SWING_DRIVE);
            twistDrive = info.GetValue<PhysJointDriveDescription>(TWIST_DRIVE);
            slerpDrive = info.GetValue<PhysJointDriveDescription>(SLERP_DRIVE);

            XMotion = info.GetValue<D6JointMotion>(X_MOTION);
            YMotion = info.GetValue<D6JointMotion>(Y_MOTION);
            ZMotion = info.GetValue<D6JointMotion>(Z_MOTION);
            Swing1Motion = info.GetValue<D6JointMotion>(SWING_1_MOTION);
            Swing2Motion = info.GetValue<D6JointMotion>(SWING_2_MOTION);
            TwistMotion = info.GetValue<D6JointMotion>(TWIST_MOTION);
            DrivePosition = info.GetVector3(DRIVE_POSITION);
            DriveOrientation = info.GetQuaternion(DRIVE_ORIENTATION);
            DriveLinearVelocity = info.GetVector3(DRIVE_LINEAR_VELOCITY);
            DriveAngularVelocity = info.GetVector3(DRIVE_ANGULAR_VELOCITY);
            ProjectionMode = info.GetValue<JointProjectionMode>(PROJECTION_MODE);
            ProjectionDistance = info.GetFloat(PROJECTION_DISTANCE);
            ProjectionAngle = info.GetFloat(PROJECTION_ANGLE);
            GearRatio = info.GetFloat(GEAR_RATIO);
            Flags = info.GetValue<D6JointFlag>(FLAGS);
        }

        /// <summary>
        /// Save function.
        /// </summary>
        /// <param name="info"></param>
        protected override void getJointInfo(SaveInfo info)
        {
            info.AddValue(LINEAR_LIMIT, linearLimit);
            info.AddValue(SWING_1_LIMIT, swing1Limit);
            info.AddValue(SWING_2_LIMIT, swing2Limit);
            info.AddValue(TWIST_LIMIT, twistLimit);
            info.AddValue(X_DRIVE, xDrive);
            info.AddValue(Y_DRIVE, yDrive);
            info.AddValue(Z_DRIVE, zDrive);
            info.AddValue(SWING_DRIVE, swingDrive);
            info.AddValue(TWIST_DRIVE, twistDrive);
            info.AddValue(SLERP_DRIVE, slerpDrive);

            info.AddValue(X_MOTION, XMotion);
            info.AddValue(Y_MOTION, YMotion);
            info.AddValue(Z_MOTION, ZMotion);
            info.AddValue(SWING_1_MOTION, Swing1Motion);
            info.AddValue(SWING_2_MOTION, Swing2Motion);
            info.AddValue(TWIST_MOTION, TwistMotion);
            info.AddValue(DRIVE_POSITION, DrivePosition);
            info.AddValue(DRIVE_ORIENTATION, DriveOrientation);
            info.AddValue(DRIVE_LINEAR_VELOCITY, DriveLinearVelocity);
            info.AddValue(DRIVE_ANGULAR_VELOCITY, DriveAngularVelocity);
            info.AddValue(PROJECTION_MODE, ProjectionMode);
            info.AddValue(PROJECTION_DISTANCE, ProjectionDistance);
            info.AddValue(PROJECTION_ANGLE, ProjectionAngle);
            info.AddValue(GEAR_RATIO, GearRatio);
            info.AddValue(FLAGS, Flags);
        }

        #endregion Saveable
    }
}
