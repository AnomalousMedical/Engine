using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Text;

namespace RpgMath
{
    class TurnTimer : ITurnTimer
    {
        public const long TickTime = (long)(1f / 200f * Clock.SecondsToMicro);
        public const long GlobalTimerUnit = 8192L;

        private long speedValue;
        private long accumulator;
        private long globalTimer;
        private long normalSpeed;

        private List<ICharacterTimer> characterTimers = new List<ICharacterTimer>();

        /// <summary>
        /// Restart the timer.
        /// </summary>
        /// <param name="battleSpeed">The battle speed chosen by the player's config. 0 is fastest 255 is slowest and 128 is middle.</param>
        /// <param name="baseDexSum">The sum of the base dex of all characters in the party. This is</param>
        /// <param name="characterTimers"></param>
        public void Restart(long battleSpeed, long baseDexSum, IEnumerable<ICharacterTimer> characterTimers)
        {
            this.characterTimers.AddRange(characterTimers);
            speedValue = 32768L / (120L + battleSpeed * 15L / 8L);
            accumulator = 0;
            globalTimer = 0;
            normalSpeed = (long)MathF.Ceiling(baseDexSum / 2f) + 50;
        }

        public void End()
        {
            this.characterTimers.Clear();
        }

        public void RemoveTimer(ICharacterTimer timer)
        {
            this.characterTimers.Remove(timer);
        }

        public void Update(Clock clock)
        {
            accumulator += clock.DeltaTimeMicro;
            while (accumulator > TickTime)
            {
                accumulator -= TickTime;
                globalTimer += speedValue;

                //Status conditions here
                foreach (var c in characterTimers)
                {
                    c.Tick(speedValue, normalSpeed);
                }
            }
        }
    }
}
