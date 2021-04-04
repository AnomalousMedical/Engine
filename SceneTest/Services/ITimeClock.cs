using Engine.Platform;

namespace SceneTest
{
    interface ITimeClock
    {
        long CurrentTimeMicro { get; set; }
        long DayEnd { get; set; }
        float DayFactor { get; }
        long DayStart { get; set; }
        bool IsDay { get; }
        float NightFactor { get; }

        void Update(Clock clock);
    }
}