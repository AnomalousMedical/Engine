using BulletPlugin;
using Engine;
using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anomalous.SidescrollerCore
{
    partial class PlayerController
    {
        class JumpUpPlayerState : PlayerControllerState
        {
            private float jumpTime;
            private BlendedPhaseAnimation animation;

            public JumpUpPlayerState()
            {
                
            }

            public override void link(PlayerController playerController)
            {
                base.link(playerController);
                animation = new BlendedPhaseAnimation(playerController.jumpStart, playerController.jumpLoop);
            }

            public override void start()
            {
                animation.start();
                jumpTime = 0.0f;
            }

            public override void stop()
            {
                animation.stop();
            }

            public override void update(Clock clock, EventManager eventManager)
            {
                animation.update(clock.DeltaSeconds);

                Vector3 linearVelocity = playerController.rigidBody.getLinearVelocity();
                linearVelocity.y = jumpSpeed - jumpDecel * jumpTime;
                playerController.rigidBody.setLinearVelocity(linearVelocity);

                if(playerController.Jump.FirstFrameUp)
                {
                    playerController.CurrentState = playerController.fallingState;
                }

                if(jumpTime > playerController.JumpMaxDuration)
                {
                    playerController.CurrentState = playerController.fallingState;
                }
                jumpTime += clock.DeltaSeconds;
            }

            public override void physicsTick(float timeSpan)
            {
                playerController.moveDuringPhysics();
            }
        }
    }
}
