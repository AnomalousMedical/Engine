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
        class FallingPlayerState : PlayerControllerState
        {
            public override void start()
            {
                playerController.jumpLoop.setEnabled(true);
                Vector3 linearVelocity = playerController.rigidBody.getLinearVelocity();
                if (linearVelocity.y > 0)
                {
                    linearVelocity.y /= 2.0f;
                    playerController.rigidBody.setLinearVelocity(linearVelocity);
                }
            }

            public override void stop()
            {
                playerController.jumpLoop.setEnabled(false);
            }

            public override void update(Clock clock, EventManager eventManager)
            {
                playerController.jumpLoop.addTime(clock.DeltaSeconds);

                if(playerController.isOnGround())
                {
                    if(playerController.Controls.MoveLeftEvent.Down 
                        || playerController.Controls.MoveRightEvent.Down 
                        || playerController.Controls.MoveDownEvent.Down 
                        || playerController.Controls.MoveUpEvent.Down)
                    {
                        playerController.CurrentState = playerController.runningState;
                    }
                    else
                    {
                        playerController.CurrentState = playerController.idleState;
                    }
                }
            }

            public override void physicsTick(float timeSpan)
            {
                playerController.moveDuringPhysics();
            }
        }
    }
}
