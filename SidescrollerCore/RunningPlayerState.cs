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
        class RunningPlayerState : PlayerControllerState
        {
            public override void start()
            {
                playerController.runBase.setEnabled(true);
                playerController.runTop.setEnabled(true);
            }

            public override void stop()
            {
                playerController.runBase.setEnabled(false);
                playerController.runTop.setEnabled(false);
            }

            public override void update(Clock clock, EventManager eventManager)
            {
                playerController.runBase.addTime(clock.DeltaSeconds);
                playerController.runTop.addTime(clock.DeltaSeconds);

                Vector3 linearVelocity = playerController.rigidBody.getLinearVelocity();

                if (playerController.Controls.MoveRightEvent.FirstFrameUp && !playerController.Controls.MoveLeftEvent.Down ||
                    playerController.Controls.MoveLeftEvent.FirstFrameUp && !playerController.Controls.MoveRightEvent.Down)
                {
                    linearVelocity.x = 0;
                }
                else
                {
                    if ((playerController.Controls.MoveRightEvent.FirstFrameUp && linearVelocity.x > 0.0f) ||
                        (playerController.Controls.MoveLeftEvent.FirstFrameUp && linearVelocity.x < 0.0f))
                    {
                        linearVelocity.x = 0.0f;
                    }
                }

                if (playerController.Controls.MoveUpEvent.FirstFrameUp && !playerController.Controls.MoveDownEvent.Down ||
                    playerController.Controls.MoveDownEvent.FirstFrameUp && !playerController.Controls.MoveUpEvent.Down)
                {
                    linearVelocity.z = 0;
                }
                else
                {
                    if ((playerController.Controls.MoveUpEvent.FirstFrameUp && linearVelocity.z > 0.0f) ||
                        (playerController.Controls.MoveDownEvent.FirstFrameUp && linearVelocity.z < 0.0f))
                    {
                        linearVelocity.z = 0.0f;
                    }
                }

                if(playerController.Controls.MoveLeftEvent.Up && playerController.Controls.MoveRightEvent.Up && playerController.Controls.MoveUpEvent.Up && playerController.Controls.MoveDownEvent.Up)
                {
                    playerController.CurrentState = playerController.idleState;
                }

                playerController.rigidBody.setLinearVelocity(linearVelocity);

                if (playerController.Jump.FirstFrameDown)
                {
                    playerController.CurrentState = playerController.jumpUpState;
                }
                else if(!playerController.isOnGround())
                {
                    playerController.CurrentState = playerController.fallingState;
                }
            }

            public override void physicsTick(float timeSpan)
            {
                playerController.moveDuringPhysics();
            }
        }
    }
}
