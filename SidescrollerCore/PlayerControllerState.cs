using Engine.Attributes;
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
        class PlayerControllerState
        {
            [DoNotCopy]
            [DoNotSave]
            protected PlayerController playerController;

            public virtual void link(PlayerController playerController)
            {
                this.playerController = playerController;
            }

            public virtual void start()
            {

            }

            public virtual void stop()
            {

            }

            public virtual void update(Clock clock, EventManager eventManager)
            {

            }

            public virtual void physicsTick(float timeSpan)
            {

            }
        }
    }
}
