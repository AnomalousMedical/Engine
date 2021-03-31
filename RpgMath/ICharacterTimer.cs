using System;

namespace RpgMath
{
    public interface ICharacterTimer
    {
        long CTimer { get; }
        long Modifier { get; set; }
        bool ModifierMultiplies { get; set; }
        long TotalDex { get; set; }
        bool TurnTimerActive { get; set; }
        long TurnTimer { get; set; }
        long VTimer { get; }

        event Action<ICharacterTimer> TurnReady;

        void Tick(long speedValue, long normalSpeed);
    }
}