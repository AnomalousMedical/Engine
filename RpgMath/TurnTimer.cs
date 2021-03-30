using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Text;

namespace RpgMath
{
    class CharacterTimer
    {
        public event Action<CharacterTimer> TurnReady;

        public const long VTimerUnit = 8192L;
        public const long CTimerUnit = 4096L;
        public const long TurnTimerUnit = 65535L;

        private long vTimer;
        private long cTimer;
        private long turnTimer;

        public long VTimer => vTimer;
        public long CTimer => cTimer;
        public long TurnTimer => turnTimer;

        /// <summary>
        /// The speed modifier.
        /// </summary>
        public long Modifier { get; set; }

        /// <summary>
        /// True to multiply by the modifier, false to divide.
        /// </summary>
        public bool ModifierMultiplies { get; set; }

        /// <summary>
        /// The character's entire dexterity score.
        /// </summary>
        public long TotalDex { get; set; }

        public void Tick(long speedValue, long normalSpeed)
        {
            long vTimerIncrease = 0;
            if (Modifier != 0)
            {
                vTimerIncrease = 2 * speedValue;
                if (ModifierMultiplies)
                {
                    vTimerIncrease *= Modifier;
                }
                else
                {
                    vTimerIncrease /= Modifier;
                }
                vTimer += vTimerIncrease;
            }
            cTimer += 68;
            turnTimer += TotalDex * vTimerIncrease / normalSpeed;
            if(turnTimer > TurnTimerUnit)
            {
                TurnReady?.Invoke(this);
                turnTimer = 0;
            }
        }
    }

    class TurnTimer
    {
        public const long TickTime = (long)(1f / 30f * Clock.SecondsToMicro);
        public const long GlobalTimerUnit = 8192L;

        private long speedValue;
        private long accumulator;
        private long globalTimer;
        private long normalSpeed;

        private List<CharacterTimer> characterTimers;

        /// <summary>
        /// Restart the timer.
        /// </summary>
        /// <param name="battleSpeed">The battle speed chosen by the player's config. 0 is fastest 255 is slowest and 128 is middle.</param>
        /// <param name="baseDexSum">The sum of the base dex of all characters in the party. This is</param>
        /// <param name="characterTimers"></param>
        public void Restart(long battleSpeed, long baseDexSum, List<CharacterTimer> characterTimers)
        {
            this.characterTimers = characterTimers;
            speedValue = 32768L / (120L + battleSpeed * 15L / 8L);
            accumulator = 0;
            globalTimer = 0;
            normalSpeed = (long)MathF.Ceiling(baseDexSum / 2f) + 50;
        }

        public void Update(Clock clock)
        {
            accumulator += clock.DeltaTimeMicro;
            if (accumulator > TickTime)
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
