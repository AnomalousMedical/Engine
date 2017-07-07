using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Platform;
using Engine.Editing;

namespace Anomalous.SidescrollerCore
{
    class DespawnAfterTime : Behavior
    {
        [Editable("The duration in seconds.")]
        public float Duration { get; set; }

        private long remainingTime;

        public DespawnAfterTime()
        {
            Duration = 5;
        }

        protected override void link()
        {
            remainingTime = Clock.SecondsToMicroseconds(Duration);
            base.link();
        }

        public override void update(Clock clock, EventManager eventManager)
        {
            remainingTime -= clock.DeltaTimeMicro;
            if(remainingTime <= 0)
            {
                Owner.destroy();
            }
        }
    }
}
