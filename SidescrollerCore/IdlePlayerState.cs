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
        class IdlePlayerState : PlayerControllerState
        {
            public override void start()
            {
                playerController.idleBase.setEnabled(true);
                playerController.idleTop.setEnabled(true);
            }

            public override void stop()
            {
                playerController.idleBase.setEnabled(false);
                playerController.idleTop.setEnabled(false);
            }

            public override void update(Clock clock, EventManager eventManager)
            {
                playerController.idleBase.addTime(clock.DeltaSeconds);
                playerController.idleTop.addTime(clock.DeltaSeconds);

                if (playerController.Controls.MoveLeftEvent.FirstFrameDown 
                    || playerController.Controls.MoveRightEvent.FirstFrameDown 
                    || playerController.Controls.MoveUpEvent.FirstFrameDown 
                    || playerController.Controls.MoveDownEvent.FirstFrameDown)
                {
                    playerController.CurrentState = playerController.runningState;
                }

                if(playerController.Jump.FirstFrameDown)
                {
                    playerController.CurrentState = playerController.jumpUpState;
                }

                Vector3 linearVelocity = playerController.rigidBody.getLinearVelocity();
                linearVelocity.x = 0;
                playerController.rigidBody.setLinearVelocity(linearVelocity);
            }
        }
    }
}
