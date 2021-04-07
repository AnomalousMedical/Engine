using Engine.Platform;
using System.Collections.Generic;

namespace RpgMath
{
    public interface ITurnTimer
    {
        void Restart(long battleSpeed, long baseDexSum);
        void AddTimer(ICharacterTimer timer);
        void RemoveTimer(ICharacterTimer timer);
        void Update(Clock clock);
    }
}