using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Platform;
using Engine.Editing;
using BulletPlugin;
using Engine.Attributes;

namespace Anomalous.SidescrollerCore
{
    class LinearFlyingMovement : Behavior
    {
        [Editable]
        public Vector3 Direction { get; set; }

        [Editable]
        public float Acceleration { get; set; }

        [Editable]
        public float Speed { get; set; }

        [Editable]
        public float MaxSpeed { get; set; }

        [Editable]
        public String RigidBodyName { get; set; }

        [DoNotCopy]
        [DoNotSave]
        private RigidBody rigidBody;

        [DoNotCopy]
        [DoNotSave]
        private Vector3 travelDirection;

        public LinearFlyingMovement()
        {
            RigidBodyName = "RigidBody";
        }

        protected override void willDestroy()
        {
            base.willDestroy();
            rigidBody.Scene.Tick -= Scene_Tick;
        }

        protected override void link()
        {
            base.link();
            rigidBody = Owner.getElement(RigidBodyName) as RigidBody;
            if(rigidBody == null)
            {
                blacklist($"Cannot find rigid body '{RigidBodyName}'");
            }

            rigidBody.Scene.Tick += Scene_Tick;

            travelDirection = Quaternion.quatRotate(Owner.Rotation, Direction);
        }

        private void Scene_Tick(float timeSpan)
        {
            rigidBody.setLinearVelocity(travelDirection * Speed);
            rigidBody.capLinearVelocity(MaxSpeed);
        }

        public override void update(Clock clock, EventManager eventManager)
        {
            Speed += Acceleration * clock.DeltaSeconds;
        }
    }
}
