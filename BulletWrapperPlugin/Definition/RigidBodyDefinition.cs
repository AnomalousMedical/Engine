using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.ObjectManagement;
using Engine.Editing;
using Engine.Saving;
using System.Runtime.InteropServices;
using Engine.Reflection;
using Engine;
using Logging;

namespace BulletPlugin
{
    public partial class RigidBodyDefinition : BulletElementDefinition
    {
        internal RigidBodyConstructionInfo constructionInfo = new RigidBodyConstructionInfo(0.0f);
        protected EditInterface editInterface;

        public RigidBodyDefinition(String name)
            :base(name)
        {
            LinearVelocity = Vector3.Zero;
            AngularVelocity = Vector3.Zero;
            CurrentActivationState = ActivationState.ActiveTag;
            AnisotropicFriction = new Vector3(1.0f, 1.0f, 1.0f);
            DeactivationTime = 0.0f;
            Flags = 0;
            HitFraction = 1.0f;
            MaxContactDistance = 0.0f;
            CollisionFilterMask = -1;
            CollisionFilterGroup = 1;
        }

        public override void registerScene(SimSubScene subscene, SimObjectBase instance)
        {
            if (subscene.hasSimElementManagerType(typeof(BulletScene)))
            {
                BulletScene sceneManager = subscene.getSimElementManager<BulletScene>();
                sceneManager.getBulletFactory().addRigidBody(this, instance);
            }
            else
            {
		        Log.Default.sendMessage("Cannot add RigidBodyDefinition {0} to SimSubScene {1} because it does not contain a BulletSceneManager.", LogLevel.Warning, BulletInterface.PluginName, Name, subscene.Name);
            }
        }

        internal override void createProduct(SimObjectBase instance, BulletScene scene)
        {
            BulletShapeRepository repository = BulletInterface.Instance.ShapeRepository;
	        if(repository.containsValidCollection(ShapeName))
	        {
		        IntPtr shape = repository.getCollection(ShapeName).CollisionShape;
		        if(constructionInfo.m_mass != 0.0f)
		        {
                    CollisionShapeInterface.CollisionShape_CalculateLocalInertia(shape, constructionInfo.m_mass, ref constructionInfo.m_localInertia);
		        }
		        RigidBody rigidBody = new RigidBody(this, scene, shape, instance.Translation, instance.Rotation);
		        instance.addElement(rigidBody);
	        }
	        else
	        {
		        Log.Default.sendMessage("Could not find collision shape named {0}.", LogLevel.Warning, "BulletPlugin", ShapeName);
	        }
        }

        internal override void createStaticProduct(SimObjectBase instance, BulletScene scene)
        {
            
        }

        internal static SimElementDefinition Create(String name, EditUICallback callback)
        {
            return new RigidBodyDefinition(name);
        }

        #region Properties

        [Editable]
        public String ShapeName { get; set; }

        [Editable]
        public short CollisionFilterMask { get; set; }

	    [Editable]
        public short CollisionFilterGroup { get; set; }

	    [Editable]
        public float HitFraction { get; set; }

	    [Editable]
        public CollisionFlags Flags { get; set; }

	    [Editable]
        public float DeactivationTime { get; set; }

	    [Editable]
        public Vector3 AnisotropicFriction { get; set; }

	    [Editable]
        public ActivationState CurrentActivationState { get; set; }

	    [Editable]
        public Vector3 AngularVelocity { get; set; }

	    [Editable]
        public Vector3 LinearVelocity { get; set; }

	    [Editable]
        public float AngularSleepingThreshold
	    {
		    get
		    {
			    return constructionInfo.m_angularSleepingThreshold;
		    }
		    set
            {
			    constructionInfo.m_angularSleepingThreshold = value;
		    }
	    }

	    [Editable]
        public float LinearSleepingThreshold
	    {
		    get
		    {
			    return constructionInfo.m_linearSleepingThreshold;
		    }
		    set
		    {
			    constructionInfo.m_linearSleepingThreshold = value;
		    }
	    }

	    [Editable]
        public float Restitution
	    {
		    get
		    {
			    return constructionInfo.m_restitution;
		    }
		    set
		    {
			    constructionInfo.m_restitution = value;
		    }
	    }

	    [Editable]
        public float Friction
	    {
		    get
		    {
			    return constructionInfo.m_friction;
		    }
		    set
		    {
			    constructionInfo.m_friction = value;
		    }
	    }

	    [Editable]
        public float AngularDamping
	    {
		    get
		    {
			    return constructionInfo.m_angularDamping;
		    }
		    set
            {
			    constructionInfo.m_angularDamping = value;
		    }
	    }

	    [Editable]
        public float LinearDamping
	    {
		    get
		    {
			    return constructionInfo.m_linearDamping;
		    }
		    set
		    {
			    constructionInfo.m_linearDamping = value;
		    }
	    }

	    [Editable]
        public float MaxContactDistance { get; set; }

	    [Editable]
        public float Mass
	    {
		    get
		    {
			    return constructionInfo.m_mass;
		    }
		    set
		    {
			    constructionInfo.m_mass = value;
		    }
	    }

        #endregion
    }

    partial class RigidBodyDefinition
    {
        protected static MemberScanner memberScanner = new MemberScanner();

        static RigidBodyDefinition()
        {
            memberScanner.ProcessFields = false;
            memberScanner.Filter = new EditableAttributeFilter();
        }

        private static void EditableAttributeFilter()
        {
            throw new NotImplementedException();
        }

        public override EditInterface getEditInterface()
        {
            if(editInterface == null)
	        {
		        editInterface = ReflectedEditInterface.createEditInterface(this, memberScanner, this.Name + " - Rigid Body", null);
		        editInterface.IconReferenceTag = EngineIcons.RigidBody;
	        }
	        return editInterface;
        }
    }

    partial class RigidBodyDefinition
    {
        protected RigidBodyDefinition(LoadInfo info)
            : base(info)
        {
            AngularDamping = info.GetFloat("AngularDamping");
            AngularSleepingThreshold = info.GetFloat("AngularSleepingThreshold");
            AngularVelocity = info.GetVector3("AngularVelocity");
            Friction = info.GetFloat("Friction");
            LinearDamping = info.GetFloat("LinearDamping");
            LinearSleepingThreshold = info.GetFloat("LinearSleepingThreshold");
            LinearVelocity = info.GetVector3("LinearVelocity");
            Mass = info.GetFloat("Mass");
            Restitution = info.GetFloat("Restitution");
            CurrentActivationState = info.GetValue<ActivationState>("CurrentActivationState");
            AnisotropicFriction = info.GetVector3("AnisotropicFriction");
            DeactivationTime = info.GetFloat("DeactivationTime");
            Flags = info.GetValue<CollisionFlags>("Flags");
            HitFraction = info.GetFloat("HitFraction");
            ShapeName = info.GetString("ShapeName");
            MaxContactDistance = info.GetFloat("MaxContactDistance", 0.0f);
            CollisionFilterMask = info.GetInt16("CollisionFilterMask", -1);
            CollisionFilterGroup = info.GetInt16("CollisionFilterGroup", 1);
        }

        public override void getInfo(SaveInfo info)
        {
            base.getInfo(info);
            info.AddValue("AngularDamping", AngularDamping);
            info.AddValue("AngularSleepingThreshold", AngularSleepingThreshold);
            info.AddValue("AngularVelocity", AngularVelocity);
            info.AddValue("Friction", Friction);
            info.AddValue("LinearDamping", LinearDamping);
            info.AddValue("LinearSleepingThreshold", LinearSleepingThreshold);
            info.AddValue("LinearVelocity", LinearVelocity);
            info.AddValue("Mass", Mass);
            info.AddValue("Restitution", Restitution);
            info.AddValue("CurrentActivationState", CurrentActivationState);
            info.AddValue("AnisotropicFriction", AnisotropicFriction);
            info.AddValue("DeactivationTime", DeactivationTime);
            info.AddValue("Flags", Flags);
            info.AddValue("HitFraction", HitFraction);
            info.AddValue("ShapeName", ShapeName);
            info.AddValue("MaxContactDistance", MaxContactDistance);
            info.AddValue("CollisionFilterMask", CollisionFilterMask);
            info.AddValue("CollisionFilterGroup", CollisionFilterGroup);
        }
    }
}
