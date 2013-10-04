using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;
using Engine.Reflection;
using Engine.Saving;
using Engine.ObjectManagement;

namespace BulletPlugin
{
    public abstract partial class TypedConstraintDefinition : BulletElementDefinition
    {
        public TypedConstraintDefinition(String name)
            :base(name)
        {

        }

        internal override void createProduct(Engine.ObjectManagement.SimObjectBase instance, BulletSceneInternal scene)
        {
 	        MTRigidBody rbA = null;
	        MTRigidBody rbB = null;

	        SimObject other = instance.getOtherSimObject(RigidBodyASimObject);
	        if(other != null)
	        {
		         rbA = other.getElement(RigidBodyAElement) as MTRigidBody;
	        }
        	
	        other = instance.getOtherSimObject(RigidBodyBSimObject);
	        if(other != null)
	        {
		        rbB = other.getElement(RigidBodyBElement) as MTRigidBody;
	        }
        	
	        MTTypedConstraintElement element = createConstraint(rbA, rbB, instance, scene);
	        if(element != null)
	        {
		        instance.addElement(element);
	        }
        }

        internal override void createStaticProduct(Engine.ObjectManagement.SimObjectBase instance, BulletSceneInternal scene)
        {

        }

        public override void registerScene(Engine.ObjectManagement.SimSubScene subscene, Engine.ObjectManagement.SimObjectBase instance)
        {
 	        if (subscene.hasSimElementManagerType(typeof(BulletScene)))
            {
                BulletSceneInternal sceneManager = (BulletSceneInternal)subscene.getSimElementManager<BulletScene>();
                sceneManager.getBulletFactory().addTypedConstraint(this, instance);
            }
            else
            {
		        Logging.Log.Default.sendMessage("Cannot add PhysActorDefinition {0} to SimSubScene {1} because it does not contain a BulletSceneManager.", Logging.LogLevel.Warning, BulletInterface.PluginName, Name, subscene.Name);
            }
        }

        internal abstract MTTypedConstraintElement createConstraint(MTRigidBody rbA, MTRigidBody rbB, SimObjectBase instance, BulletSceneInternal scene);

        public abstract String JointType { get; }

        [Editable]
        public String RigidBodyASimObject { get; set; }

        [Editable]
        public String RigidBodyAElement { get; set; }

        [Editable]
        public String RigidBodyBSimObject { get; set; }

        [Editable]
        public String RigidBodyBElement { get; set; }

        static MemberScanner memberScanner = new MemberScanner();

        static TypedConstraintDefinition()
        {
            memberScanner.Filter = new EditableAttributeFilter();
        }

        private EditInterface editInterface;

        protected override EditInterface createEditInterface()
        {
            if(editInterface == null)
	        {
		        editInterface = ReflectedEditInterface.createEditInterface(this, memberScanner, this.Name + " - " + JointType, null);
		        editInterface.IconReferenceTag = EngineIcons.Joint;
	        }
	        return editInterface;
        }

        protected TypedConstraintDefinition(LoadInfo info)
            :base(info)
        {
            RigidBodyASimObject = info.GetString("RigidBodyASimObject");
            RigidBodyAElement = info.GetString("RigidBodyAElement");
            RigidBodyBSimObject = info.GetString("RigidBodyBSimObject");
            RigidBodyBElement = info.GetString("RigidBodyBElement");
        }

        public override void getInfo(SaveInfo info)
        {
            info.AddValue("RigidBodyASimObject", RigidBodyASimObject);
            info.AddValue("RigidBodyAElement", RigidBodyAElement);
            info.AddValue("RigidBodyBSimObject", RigidBodyBSimObject);
            info.AddValue("RigidBodyBElement", RigidBodyBElement);
            base.getInfo(info);
        }
    }
}
