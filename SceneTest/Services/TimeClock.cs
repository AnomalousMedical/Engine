using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest
{
    class TimeClock : ITimeClock
    {
        const long HoursToMicro = 60L * 60L * Clock.SecondsToMicro;
        const long HoursPerDay = 24L;

        long currentTime = 6L * HoursToMicro;
        //long timeFactor = 25000L; //Pretty Fast
        //long timeFactor = 10000L;
        long timeFactor = 100L;
        long period = HoursPerDay * HoursToMicro;
        long halfPeriod;
        long dayStart = 6L * HoursToMicro;
        long dayEnd = 18L * HoursToMicro;
        float dayEndFactor;
        float nightEndFactor;

        public TimeClock()
        {
            halfPeriod = period / 2;
            dayEndFactor = (dayEnd - dayStart) * Clock.MicroToSeconds;
            nightEndFactor = (dayStart + period - dayEnd) * Clock.MicroToSeconds;
        }

        public void Update(Clock clock)
        {
            currentTime += clock.DeltaTimeMicro * timeFactor;
            currentTime %= period;
        }

        public bool IsDay => currentTime > dayStart && currentTime <= dayEnd;

        public long CurrentTimeMicro
        {
            get
            {
                return currentTime;
            }
            set
            {
                currentTime = value % period;
            }
        }

        public float TimeFactor => currentTime / (float)period;

        public long DayStart
        {
            get
            {
                return dayStart;
            }
            set
            {
                dayStart = value % period;
            }
        }

        public long DayEnd
        {
            get
            {
                return dayEnd;
            }
            set
            {
                dayEnd = value % period;
            }
        }

        public float DayFactor
        {
            get
            {
                if (!IsDay)
                {
                    return 0.0f;
                }
                return (currentTime - dayStart) * Clock.MicroToSeconds / dayEndFactor;
            }
        }

        public float NightFactor
        {
            get
            {
                if (IsDay)
                {
                    return 0.0f;
                }
                if (currentTime > dayEnd)
                {
                    return (currentTime - dayEnd) * Clock.MicroToSeconds / nightEndFactor;
                }
                //All thats left is (currentTime < dayStart)
                return (currentTime) * Clock.MicroToSeconds / nightEndFactor + 0.5f;
            }
        }
    }
}
