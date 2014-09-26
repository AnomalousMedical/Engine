using Engine;
using Engine.Attributes;
using Engine.Editing;
using Engine.ObjectManagement;
using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulletPlugin
{
    /// <summary>
    /// This behavior will cause the physics scene that contains the specified rigid body
    /// to delay its synchronize until this behavior's update function is called.
    /// </summary>
    public class DelayPhysicsSyncBehavior : Behavior
    {
        [Editable]
        private String rigidBodySimObjectName;

        [Editable]
        private String rigidBodyName = "Actor";

        [DoNotCopy]
        [DoNotSave]
        private BulletScene bulletScene;

        protected override void link()
        {
            base.link();

            SimObject rigidBodySimObject = Owner.getOtherSimObject(rigidBodySimObjectName);
            if(rigidBodySimObject == null)
            {
                blacklist("Cannot find rigidBodySimObject '{0}'", rigidBodySimObjectName);
            }

            RigidBody rigidBody = rigidBodySimObject.getElement(rigidBodyName) as RigidBody;
            if(rigidBody == null)
            {
                blacklist("Cannot find rigid body '{0}' rigidBodySimObject '{1}'", rigidBodyName, rigidBodySimObjectName);
            }

            bulletScene = rigidBody.Scene;
            bulletScene.AutoSynchronizeAfterUpdate = false;
        }

        protected override void destroy()
        {
            base.destroy();
            bulletScene.AutoSynchronizeAfterUpdate = true;
        }

        public override void update(Clock clock, EventManager eventManager)
        {
            bulletScene.synchronizePhysicsToScene();
        }
    }
}
