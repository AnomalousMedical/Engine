using BulletPlugin;
using Engine;
using Engine.Attributes;
using Engine.Editing;
using Engine.Threads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.SidescrollerCore
{
    class DespawnOnCollision : BehaviorInterface
    {
        [Editable]
        private String RigidBodyName = "RigidBody";

        [DoNotCopy]
        [DoNotSave]
        private RigidBody rigidBody;

        public DespawnOnCollision()
        {
            
        }

        protected override void link()
        {
            rigidBody = Owner.getElement(RigidBodyName) as RigidBody;
            if (rigidBody == null)
            {
                blacklist($"Cannot find rigid body {RigidBodyName}");
            }

            rigidBody.ContactStarted += RigidBody_ContactStarted;

            base.link();
        }

        private void RigidBody_ContactStarted(ContactInfo contact, RigidBody sourceBody, RigidBody otherBody, bool isBodyA)
        {
            ThreadManager.invoke(() =>
            {
                Owner.destroy();
            });
        }
    }
}
