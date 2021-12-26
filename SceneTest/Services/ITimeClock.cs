using Engine.Platform;

namespace SceneTest
{
    interface ITimeClock
    {
        long CurrentTimeMicro { get; set; }
        float TimeFactor { get; }
        long DayEnd { get; set; }
        float DayFactor { get; }
        long DayStart { get; set; }
        bool IsDay { get; }
        float NightFactor { get; }

        void Update(Clock clock);
    }
}