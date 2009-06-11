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
    class PhysPulleyJointDefinition: PhysJointDefinitionBase<PhysPulleyJointDesc, PhysPulleyJoint>
    {
        #region Static

        private static MemberScanner memberScanner = new MemberScanner();

        static PhysPulleyJointDefinition()
        {
            EditableAttributeFilter filter = new EditableAttributeFilter();
            filter.TerminatingType = typeof(PhysJointDefinitionBase<PhysPulleyJointDesc, PhysPulleyJoint>);
            memberScanner.Filter = filter;
        }

        /// <summary>
        /// Create function for commands.
        /// </summary>
        /// <param name="name">The name of the definition to create.</param>
        /// <returns>A new definition.</returns>
        internal static PhysPulleyJointDefinition Create(String name, EditUICallback callback)
        {
            return new PhysPulleyJointDefinition(name);
        }

        #endregion Static

        [Editable] private PhysMotorDescription motor;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the joint.</param>
        public PhysPulleyJointDefinition(String name)
            :base(new PhysPulleyJointDesc(), name, "Pulley Joint", null)
        {
            configureSubParts();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the joint.</param>
        /// <param name="joint">An existing PhysJointElement to get values from.</param>
        internal PhysPulleyJointDefinition(String name, PhysPulleyJointElement joint)
            :base(new PhysPulleyJointDesc(), joint, name, "Pulley Joint", null)
        {
            joint.RealJoint.saveToDesc(jointDesc);
            configureSubParts();
        }

        protected void configureSubParts()
        {
            //Create the sub parts
            motor = new PhysMotorDescription();

            //Copy values for sub parts
            motor.copyFromDescription(jointDesc.Motor);
        }

        /// <summary>
        /// This function is called just before the joint is created. This
        /// allows any subclass configuration of the joint description to take
        /// place.
        /// </summary>
        protected override void configureJoint()
        {
            //copy sub parts values
            motor.copyToDescription(jointDesc.Motor);
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
        internal override PhysJointElement createElement(PhysJoint joint, PhysXSceneManager scene)
        {
            return new PhysPulleyJointElement(Name, (PhysPulleyJoint)joint, scene, subscription);
        }

        [Editable]
        public Vector3 Pulley0
        {
            get
            {
                return jointDesc.get_Pulley(0);
            }
            set
            {
                jointDesc.set_Pulley(0, value);
            }
        }

        [Editable]
        public Vector3 Pulley1
        {
            get
            {
                return jointDesc.get_Pulley(1);
            }
            set
            {
                jointDesc.set_Pulley(1, value);
            }
        }

        [Editable]
        public float Distance
        {
            get
            {
                return jointDesc.Distance;
            }
            set
            {
                jointDesc.Distance = value;
            }
        }

        [Editable]
        public float Stiffness
        {
            get
            {
                return jointDesc.Stiffness;
            }
            set
            {
                jointDesc.Stiffness = value;
            }
        }

        [Editable]
        public float Ratio
        {
            get
            {
                return jointDesc.Ratio;
            }
            set
            {
                jointDesc.Ratio = value;
            }
        }

        [Editable]
        public PulleyJointFlag Flags
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

        private const String MOTOR = "PhysPulleyJointDefinitionMotor";

        private const String PULLEY_0 = "PhysPulleyJointDefinitionPulley0";
        private const String PULLEY_1 = "PhysPulleyJointDefinitionPulley1";
        private const String DISTANCE = "PhysPulleyJointDefinitionDistance";
        private const String STIFFNESS = "PhysPulleyJointDefinitionStiffness";
        private const String RATIO = "PhysPulleyJointDefinitionRatio";
        private const String FLAGS = "PhysPulleyJointDefinitionFlags";

        /// <summary>
        /// Load constructor.
        /// </summary>
        /// <param name="loadInfo"></param>
        private PhysPulleyJointDefinition(LoadInfo info)
            : base(new PhysPulleyJointDesc(), "Pulley Joint", info)
        {
            motor = info.GetValue<PhysMotorDescription>(MOTOR);
            Pulley0 = info.GetVector3(PULLEY_0);
            Pulley1 = info.GetVector3(PULLEY_1);
            Distance = info.GetFloat(DISTANCE);
            Stiffness = info.GetFloat(STIFFNESS);
            Ratio = info.GetFloat(RATIO);
            Flags = info.GetValue<PulleyJointFlag>(FLAGS);
        }

        /// <summary>
        /// Save function.
        /// </summary>
        /// <param name="info"></param>
        protected override void getJointInfo(SaveInfo info)
        {
            info.AddValue(MOTOR, motor);
            info.AddValue(PULLEY_0, Pulley0);
            info.AddValue(PULLEY_1, Pulley1);
            info.AddValue(DISTANCE, Distance);
            info.AddValue(STIFFNESS, Stiffness);
            info.AddValue(RATIO, Ratio);
            info.AddValue(FLAGS, Flags);
        }

        #endregion Saveable
    }
}
