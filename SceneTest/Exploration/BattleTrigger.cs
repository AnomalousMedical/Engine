using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest
{
    class BattleTrigger : IBattleTrigger
    {
        private int dangerCounter = 0;
        private long dangerCounterAccumulator = 0;
        private const long DangerCounterTick = Clock.SecondsToMicro / 3;
        private Random battleRandom = new Random();

        public bool UpdateRandomEncounter(Clock clock, bool moving)
        {
            bool retVal = false;
            dangerCounterAccumulator += clock.DeltaTimeMicro;
            if (dangerCounterAccumulator > DangerCounterTick)
            {
                dangerCounterAccumulator -= DangerCounterTick;
                if (moving)
                {
                    dangerCounter += 4096 / 64; //This will be encounter value
                    //Console.WriteLine(dangerCounter / 256);
                    int battleChance = battleRandom.Next(256);
                    var check = dangerCounter / 256;
                    retVal = battleChance < check;
                    if (retVal)
                    {
                        dangerCounter = 0;
                    }
                }
            }

            return retVal;
        }
    }
}
