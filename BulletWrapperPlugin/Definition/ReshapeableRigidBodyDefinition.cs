﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Editing;
using Engine.Saving;
using Engine.ObjectManagement;

namespace BulletPlugin
{
    public class ReshapeableRigidBodyDefinition : RigidBodyDefinition
    {
        public ReshapeableRigidBodyDefinition(String name)
            :base(name)
        {

        }

        internal override void createProduct(SimObjectBase instance, BulletScene scene)
        {
            IntPtr shape = CollisionShapeInterface.CompoundShape_Create(0.0f);
	        ReshapeableRigidBody rigidBody = new ReshapeableRigidBody(this, scene, shape, instance.Translation, instance.Rotation);
	        instance.addElement(rigidBody);
        }

        public override Engine.Editing.EditInterface getEditInterface()
        {
            if(editInterface == null)
	        {
		        editInterface = ReflectedEditInterface.createEditInterface(this, memberScanner, this.Name + " - Reshapeable Rigid Body", null);
		        editInterface.IconReferenceTag = EngineIcons.RigidBody;
	        }
	        return editInterface;
        }

        protected ReshapeableRigidBodyDefinition(LoadInfo info)
            :base(info)
        {

        }

        public override void getInfo(SaveInfo info)
        {
            base.getInfo(info);
        }

        internal static ReshapeableRigidBodyDefinition CreateReshapeable(String name, EditUICallback uiCallback)
        {
            return new ReshapeableRigidBodyDefinition(name);
        }
    }
}
