using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Platform;
using Engine.Editing;
using Engine.Attributes;

namespace Anomalous.SidescrollerCore
{
    public class RepeatingTrigger : Behavior, Triggerable
    {
        [Editable]
        public float TimeSeconds { get; set; }

        [DoNotCopy]
        [DoNotSave]
        public event Action<Triggerable> Triggered;

        [DoNotCopy]
        [DoNotSave]
        private long remainingTime;

        public RepeatingTrigger()
        {
            TimeSeconds = 1.0f;
        }

        protected override void constructed()
        {
            base.constructed();
            remainingTime = Clock.SecondsToMicroseconds(TimeSeconds);
        }

        public override void update(Clock clock, EventManager eventManager)
        {
            remainingTime -= clock.DeltaTimeMicro;
            if(remainingTime <= 0)
            {
                if (Triggered != null)
                {
                    Triggered.Invoke(this);
                }
                remainingTime = Clock.SecondsToMicroseconds(TimeSeconds);
            }
        }
    }
}
