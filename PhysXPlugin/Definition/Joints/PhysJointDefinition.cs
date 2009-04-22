using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;
using Engine.ObjectManagement;
using PhysXWrapper;
using EngineMath;
using Logging;
using Engine.Reflection;
using Engine.Saving;

namespace PhysXPlugin
{
    public abstract class PhysJointDefinition<Desc, Joint> : PhysElementDefinition
        where Desc : PhysJointDesc
        where Joint : PhysJoint
    {
        #region Static

        private static MemberScanner memberScanner;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static PhysJointDefinition()
        {
            memberScanner = new MemberScanner();
            memberScanner.ProcessFields = false;
            memberScanner.Filter = EditableAttributeFilter.Instance;
        }

        #endregion Static

        private Desc superJointDesc;
        private EditInterface editInterface = null;
        private String jointTypeName;
        private Validate validateCallback;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name"></param>
        public PhysJointDefinition(String name, String jointTypeName, Validate validateCallback)
            : base(name)
        {
            LocalAxisDefined = false;
            Actor0Name = new Identifier("", "");
            Actor1Name = new Identifier("", "");
            this.jointTypeName = jointTypeName;
            this.validateCallback = validateCallback;
        }

        /// <summary>
        /// Call in subclass to set the joint description.
        /// </summary>
        /// <param name="jointDesc"></param>
        protected void setJointDesc(Desc jointDesc)
        {
            this.superJointDesc = jointDesc;
        }

        /// <summary>
        /// Call in subclass during the save function to save common properties.
        /// </summary>
        protected void saveDescription()
        {
            PhysActor actor = superJointDesc.get_Actor(0);
            if (actor != null)
            {
                throw new NotImplementedException();
                //Actor0Name = new Identifier(actor.Name);
            }
            actor = superJointDesc.get_Actor(1);
            if (actor != null)
            {
                throw new NotImplementedException();
                //Actor1Name = new Identifier(actor.Name);
            }
            LocalAxisDefined = true;
        }

        internal override void createProduct(SimObject instance, PhysXSceneManager scene)
        {
            if (Actor0Name.SimObjectName == "this")
            {
                superJointDesc.set_Actor(0, scene.getPhysActor(new Identifier(instance.Name, Actor0Name.ElementName)));
            }
            else
            {
                superJointDesc.set_Actor(0, scene.getPhysActor(Actor0Name));
            }
            if (Actor1Name.SimObjectName == "this")
            {
                superJointDesc.set_Actor(1, scene.getPhysActor(new Identifier(instance.Name, Actor1Name.ElementName)));
            }
            else
            {
                superJointDesc.set_Actor(1, scene.getPhysActor(Actor1Name));
            }
            if (superJointDesc.get_Actor(0) != null || superJointDesc.get_Actor(1) != null)
            {
                Identifier identifier = new Identifier(instance.Name, this.Name);
                superJointDesc.setGlobalAnchor(instance.Translation);
                superJointDesc.setGlobalAxis(Quaternion.quatRotate(instance.Rotation, Vector3.Forward));
                PhysJoint joint = scene.createJoint(identifier, superJointDesc);
                configureJoint((Joint)joint);
            }
            else
            {
                String message = String.Format("Invalid joint description: {0}, {1}.  Must specify at least one valid actor for this joint.", instance.Name, Name);
                Log.Default.sendMessage(message, LogLevel.Error, "ObjectManagement");
            }
        }

        protected abstract void configureJoint(Joint joint);

        internal override void createStaticProduct(SimObject instance, PhysXSceneManager scene)
        {
            throw new NotImplementedException();
        }

        public override void register(SimSubScene subscene, SimObject instance)
        {
            if (subscene.hasSimElementManagerType(typeof(PhysXSceneManager)))
            {
                PhysXSceneManager sceneManager = subscene.getSimElementManager<PhysXSceneManager>();
                sceneManager.getPhysFactory().addJointDefinition(instance, this);
            }
            else
            {
                Log.Default.sendMessage("Cannot add PhysJoint {0} to SimSubScene {1} because it does not contain a PhysXSceneManager.", LogLevel.Warning, PhysXInterface.PluginName, Name, subscene.Name);
            }
        }

        public override EditInterface getEditInterface()
        {
            if (editInterface == null)
            {
                editInterface = ReflectedEditInterface.createEditInterface(this, typeof(PhysJointDefinition<Desc, Joint>), memberScanner, Name + " " + jointTypeName, validateCallback);
                configureEditInterface(editInterface);
            }
            return editInterface;
        }

        protected abstract void configureEditInterface(EditInterface editInterface);

        [Editable]//(typeof(PhysActor))]
        public Identifier Actor0Name { get; set; }

        [Editable]//(typeof(PhysActor))]
        public Identifier Actor1Name { get; set; }

        [Editable]
        public Vector3 GlobalAxis { get; set; }

        [Editable]
        public Vector3 GlobalAnchor { get; set; }

        [Editable]
        public float MaxForce
        {
            get
            {
                return superJointDesc.MaxForce;
            }
            set
            {
                superJointDesc.MaxForce = value;
            }
        }

        [Editable]
        public float MaxTorque
        {
            get
            {
                return superJointDesc.MaxTorque;
            }
            set
            {
                superJointDesc.MaxTorque = value;
            }
        }

        [Editable]
        public float SolverExtrapolationFactor
        {
            get
            {
                return superJointDesc.SolverExtrapolationFactor;
            }
            set
            {
                superJointDesc.SolverExtrapolationFactor = value;
            }
        }

        [Editable]
        public uint UseAccelerationSpring
        {
            get
            {
                return superJointDesc.UseAccelerationSpring;
            }
            set
            {
                superJointDesc.UseAccelerationSpring = value;
            }
        }

        [Editable]
        public JointFlag JointFlags
        {
            get
            {
                return superJointDesc.JointFlags;
            }
            set
            {
                superJointDesc.JointFlags = value;
            }
        }

        public bool LocalAxisDefined { get; set; }

        public Vector3 LocalNormal0
        {
            get
            {
                return superJointDesc.get_LocalNormal(0);
            }
            set
            {
                superJointDesc.set_LocalNormal(0, value);
            }
        }

        public Vector3 LocalNormal1
        {
            get
            {
                return superJointDesc.get_LocalNormal(1);
            }
            set
            {
                superJointDesc.set_LocalNormal(1, value);
            }
        }

        public Vector3 LocalAxis0
        {
            get
            {
                return superJointDesc.get_LocalAxis(0);
            }
            set
            {
                superJointDesc.set_LocalAxis(0, value);
            }
        }

        public Vector3 LocalAxis1
        {
            get
            {
                return superJointDesc.get_LocalAxis(1);
            }
            set
            {
                superJointDesc.set_LocalAxis(1, value);
            }
        }

        public Vector3 LocalAnchor0
        {
            get
            {
                return superJointDesc.get_LocalAnchor(0);
            }
            set
            {
                superJointDesc.set_LocalAnchor(0, value);
            }
        }

        public Vector3 LocalAnchor1
        {
            get
            {
                return superJointDesc.get_LocalAnchor(1);
            }
            set
            {
                superJointDesc.set_LocalAnchor(1, value);
            }
        }

        #region Saveable

        private const String ACTOR_0_NAME = "PhysJointDefinitionActor0Name";
        private const String ACTOR_1_NAME = "PhysJointDefinitionActor1Name";
        private const String GLOBAL_AXIS = "PhysJointDefinitionGlobalAxis";
        private const String GLOBAL_ANCHOR = "PhysJointDefinitionGlobalAnchor";
        private const String MAX_FORCE = "PhysJointDefinitionMaxForce";
        private const String MAX_TORQUE = "PhysJointDefinitionMaxTorque";
        private const String SOLVER_EXTRAPOLATION_FACTOR = "SPhysJointDefinitionolverExtrapolationFactor";
        private const String USE_ACCELERATION_SPRING = "PhysJointDefinitionUseAccelerationSpring";
        private const String JOINT_FLAGS = "PhysJointDefinitionJointFlags";
        private const String LOCAL_AXIS_DEFINED = "PhysJointDefinitionLocalAxisDefined";
        private const String LOCAL_NORMAL_0 = "PhysJointDefinitionLocalNormal0";
        private const String LOCAL_NORMAL_1 = "PhysJointDefinitionLocalNormal1";
        private const String LOCAL_AXIS_0 = "PhysJointDefinitionLocalAxis0";
        private const String LOCAL_AXIS_1 = "PhysJointDefinitionLocalAxis1";
        private const String LOCAL_ANCHOR_0 = "PhysJointDefinitionLocalAnchor0";
        private const String LOCAL_ANCHOR_1 = "PhysJointDefinitionLocalAnchor1";

        protected PhysJointDefinition(LoadInfo info)
            :base(info)
        {

        }

        /// <summary>
        /// Call in subclass serializer constructor to set the joint description.
        /// </summary>
        /// <param name="jointDesc"></param>
        /// <param name="info"></param>
        protected void setJointDesc(Desc jointDesc, LoadInfo info)
        {
            this.superJointDesc = jointDesc;
            Actor0Name = info.GetIdentifier(ACTOR_0_NAME);
            Actor1Name = info.GetIdentifier(ACTOR_1_NAME);
            GlobalAxis = info.GetVector3(GLOBAL_AXIS);
            GlobalAnchor = info.GetVector3(GLOBAL_ANCHOR);
            MaxForce = info.GetFloat(MAX_FORCE);
            MaxTorque = info.GetFloat(MAX_TORQUE);
            SolverExtrapolationFactor = info.GetFloat(SOLVER_EXTRAPOLATION_FACTOR);
            UseAccelerationSpring = info.GetUInt32(USE_ACCELERATION_SPRING);
            JointFlags = info.GetValue<JointFlag>(JOINT_FLAGS);
            LocalAxisDefined = info.GetBoolean(LOCAL_AXIS_DEFINED);
            LocalNormal0 = info.GetVector3(LOCAL_NORMAL_0);
            LocalNormal1 = info.GetVector3(LOCAL_NORMAL_1);
            LocalAxis0 = info.GetVector3(LOCAL_AXIS_0);
            LocalAxis1 = info.GetVector3(LOCAL_AXIS_1);
            LocalAnchor0 = info.GetVector3(LOCAL_ANCHOR_0);
            LocalAnchor1 = info.GetVector3(LOCAL_ANCHOR_1);
        }

        public override void getInfo(SaveInfo info)
        {
            base.getInfo(info);
            info.AddValue(ACTOR_0_NAME, Actor0Name);
            info.AddValue(ACTOR_1_NAME, Actor1Name);
            info.AddValue(GLOBAL_AXIS, GlobalAxis);
            info.AddValue(GLOBAL_ANCHOR, GlobalAnchor);
            info.AddValue(MAX_FORCE, MaxForce);
            info.AddValue(MAX_TORQUE, MaxTorque);
            info.AddValue(SOLVER_EXTRAPOLATION_FACTOR, SolverExtrapolationFactor);
            info.AddValue(USE_ACCELERATION_SPRING, UseAccelerationSpring);
            info.AddValue(JOINT_FLAGS, JointFlags);
            info.AddValue(LOCAL_AXIS_DEFINED, LocalAxisDefined);
            info.AddValue(LOCAL_NORMAL_0, LocalNormal0);
            info.AddValue(LOCAL_NORMAL_1, LocalNormal1);
            info.AddValue(LOCAL_AXIS_0, LocalAxis0);
            info.AddValue(LOCAL_AXIS_1, LocalAxis1);
            info.AddValue(LOCAL_ANCHOR_0, LocalAnchor0);
            info.AddValue(LOCAL_ANCHOR_1, LocalAnchor1);
            getJointInfo(info);
        }

        protected abstract void getJointInfo(SaveInfo info);

        #endregion Saveable
    }
}
