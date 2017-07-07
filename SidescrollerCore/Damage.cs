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
    public class Damage : BehaviorInterface
    {
        [Editable]
        private String RigidBodyName = "RigidBody";

        [Editable]
        public float Amount { get; internal set; }

        [Editable]
        public HealthGroup Attacks { get; internal set; }

        [DoNotCopy]
        [DoNotSave]
        private RigidBody rigidBody;

        public Damage()
        {
            Amount = 10;
        }

        protected override void link()
        {
            base.link();

            rigidBody = Owner.getElement(RigidBodyName) as RigidBody;
            if (rigidBody == null)
            {
                blacklist($"Cannot find rigid body {RigidBodyName}");
            }

            rigidBody.ContactStarted += RigidBody_ContactStarted;
        }

        private void RigidBody_ContactStarted(ContactInfo contact, RigidBody sourceBody, RigidBody otherBody, bool isBodyA)
        {
            ThreadManager.invoke(() =>
            {
                var target = otherBody.Owner.getElement("Health") as Health;
                if (target != null)
                {
                    target.takeDamage(this);
                }
            });
        }
    }
}
