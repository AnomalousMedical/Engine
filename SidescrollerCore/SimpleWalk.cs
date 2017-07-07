using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Platform;
using Engine.Attributes;
using Engine.Editing;
using BulletPlugin;

namespace Anomalous.SidescrollerCore
{
    public class SimpleWalk : Behavior
    {
        [Editable]
        public String RigidBodyName { get; set; }

        [DoNotCopy]
        [DoNotSave]
        private RigidBody rigidBody;

        [Editable]
        public float RunAcceleration { get; set; }

        [Editable]
        public float MaxRunSpeed { get; set; }

        public SimpleWalk()
        {
            RunAcceleration = 3.0f;
            MaxRunSpeed = 7.0f;
        }

        protected override void willDestroy()
        {
            rigidBody.Scene.Tick -= Scene_Tick;

            base.willDestroy();
        }

        protected override void constructed()
        {
            base.constructed();

            rigidBody = Owner.getElement(RigidBodyName) as RigidBody;
            if (rigidBody == null)
            {
                blacklist("Cannot find SimpleWalk rigid body '{0}'", RigidBodyName);
            }

            rigidBody.Scene.Tick += Scene_Tick;
        }

        private void Scene_Tick(float timeSpan)
        {
            rigidBody.applyCentralImpulse(new Vector3(-RunAcceleration, 0, 0));
            rigidBody.capLinearVelocityX(MaxRunSpeed);
        }

        public override void update(Clock clock, EventManager eventManager)
        {
            
        }
    }
}
