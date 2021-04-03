using System;
using System.Collections.Generic;
using System.Text;

namespace RpgMath
{
    class CharacterTimer : ICharacterTimer
    {
        public event Action<ICharacterTimer> TurnReady;

        public const long VTimerUnit = 8192L;
        public const long CTimerUnit = 4096L;
        public const long TurnTimerUnit = 65535L;

        private long vTimer;
        private long cTimer;
        private long turnTimer;

        public long VTimer => vTimer;
        public long CTimer => cTimer;
        public long TurnTimer
        {
            get { return turnTimer; }
            set { turnTimer = value; }
        }

        public float TurnTimerPct => (float)turnTimer / TurnTimerUnit;

        /// <summary>
        /// The speed modifier.
        /// </summary>
        public long Modifier { get; set; } = 1;

        /// <summary>
        /// True to multiply by the modifier, false to divide.
        /// </summary>
        public bool ModifierMultiplies { get; set; }

        /// <summary>
        /// The character's entire dexterity score.
        /// </summary>
        public long TotalDex { get; set; }

        /// <summary>
        /// Set this to true while the character is running or waiting for their turn.
        /// When a turn occurs this becomes false before the <see cref="TurnReady"/> call.
        /// </summary>
        public bool TurnTimerActive { get; set; } = true;

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
            if (TurnTimerActive)
            {
                turnTimer += TotalDex * vTimerIncrease / normalSpeed;
            }
            if (turnTimer > TurnTimerUnit)
            {
                TurnTimerActive = false;
                TurnReady?.Invoke(this);
                turnTimer = 0;
            }
        }
    }
}
