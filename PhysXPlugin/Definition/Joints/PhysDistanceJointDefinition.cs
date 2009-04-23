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
    class PhysDistanceJointDefinition: PhysJointDefinitionBase<PhysDistanceJointDesc, PhysDistanceJoint>
    {
        #region Static

        private static MemberScanner memberScanner = new MemberScanner();

        static PhysDistanceJointDefinition()
        {
            memberScanner.Filter = EditableAttributeFilter.Instance;
            memberScanner.TerminatingType = typeof(PhysJointDefinitionBase<PhysDistanceJointDesc, PhysDistanceJoint>);
        }

        /// <summary>
        /// Create function for commands.
        /// </summary>
        /// <param name="name">The name of the definition to create.</param>
        /// <returns>A new definition.</returns>
        internal static PhysDistanceJointDefinition Create(String name)
        {
            return new PhysDistanceJointDefinition(name);
        }

        #endregion Static
        
        [Editable] private PhysSpringDescription spring;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the joint.</param>
        public PhysDistanceJointDefinition(String name)
            :base(new PhysDistanceJointDesc(), name, "Distance Joint", null)
        {
            configureSubParts();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The name of the joint.</param>
        /// <param name="joint">An existing PhysJointElement to get values from.</param>
        internal PhysDistanceJointDefinition(String name, PhysDistanceJointElement joint)
            :base(new PhysDistanceJointDesc(), joint, name, "Distance Joint", null)
        {
            joint.RealJoint.saveToDesc(jointDesc);
            configureSubParts();
        }

        private void configureSubParts()
        {
            //Create the sub parts
            spring = new PhysSpringDescription();

            //Copy the values of the sub parts
            spring.copyFromDescription(jointDesc.Spring);
        }

        /// <summary>
        /// This function is called just before the joint is created. This
        /// allows any subclass configuration of the joint description to take
        /// place.
        /// </summary>
        protected override void configureJoint()
        {
            //Copy sub parts values
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

        [Editable]
        public float MaxDistance
        {
            get
            {
                return jointDesc.MaxDistance;
            }
            set
            {
                jointDesc.MaxDistance = value;
            }
        }

        [Editable]
        public float MinDistance
        {
            get
            {
                return jointDesc.MinDistance;
            }
            set
            {
                jointDesc.MinDistance = value;
            }
        }

        [Editable]
        public DistanceJointFlag Flags
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

        /// <summary>
        /// Create the PhysJointElement specific to this Joint.
        /// </summary>
        /// <param name="jointId">The Identifier of the joint being created.</param>
        /// <param name="joint">The joint that was created.</param>
        /// <param name="scene">The scene that contains the joint.</param>
        /// <returns>A new PhysJointElement for this specific joint type.</returns>
        internal override PhysJointElement createElement(Identifier jointId, PhysJoint joint, PhysXSceneManager scene)
        {
            return new PhysDistanceJointElement(jointId, (PhysDistanceJoint)joint, scene, subscription);
        }

        #region Saveable

        private const String SPRING = "PhysDistanceJointDefinitionSpring";
        private const String MAX_DISTANCE = "PhysDistanceJointDefinitionMaxDistance";
        private const String MIN_DISTANCE = "PhysDistanceJointDefinitionMinDistance";
        private const String FLAGS = "PhysDistanceJointDefinitionFlags";

        /// <summary>
        /// Load constructor.
        /// </summary>
        /// <param name="loadInfo"></param>
        private PhysDistanceJointDefinition(LoadInfo info)
            : base(new PhysDistanceJointDesc(), info)
        {
            spring = info.GetValue<PhysSpringDescription>(SPRING);
            MaxDistance = info.GetFloat(MAX_DISTANCE);
            MinDistance = info.GetFloat(MIN_DISTANCE);
            Flags = info.GetValue<DistanceJointFlag>(FLAGS);
        }

        /// <summary>
        /// Save function.
        /// </summary>
        /// <param name="info"></param>
        protected override void getJointInfo(SaveInfo info)
        {
            info.AddValue(SPRING, spring);
            info.AddValue(MAX_DISTANCE, MaxDistance);
            info.AddValue(MIN_DISTANCE, MinDistance);
            info.AddValue(FLAGS, Flags);
        }

        #endregion Saveable
    }
}
