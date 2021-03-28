using Engine;
using Engine.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest
{
    class Sky
    {
        const long OneHour = 60L * 60L * Clock.SecondsToMicro;
        readonly Color DaySky = Color.FromARGB(0xff2a63cc);
        readonly Color NightSky = Color.FromARGB(0xff030303);
        readonly Color DawnSky = Color.FromARGB(0xff242148);
        readonly Color DuskSky = Color.FromARGB(0xff242148);
        private readonly TimeClock timeClock;

        //Clear Color
        Color clearColor = Color.FromARGB(0xff2a63cc);

        //Light
        Vector3 lightDirection = Vector3.Up;
        Vector4 lightColor = new Vector4(1, 1, 1, 1);
        float lightIntensity = 3;
        float averageLogLum = 0.3f;

        public Color ClearColor => clearColor;

        public Vector3 LightDirection => lightDirection;
        public Vector4 LightColor => lightColor;
        public float LightIntensity => lightIntensity;

        public float AverageLogLum => averageLogLum;

        public Sky(TimeClock timeClock)
        {
            this.timeClock = timeClock;
        }

        public unsafe void UpdateLight(Clock clock)
        {
            if (timeClock.IsDay)
            {
                var dayFactor = (timeClock.DayFactor - 0.5f) * 2.0f;
                var noonFactor = 1.0f - Math.Abs(dayFactor);
                lightDirection = new Vector3(dayFactor, -0.5f * noonFactor - 0.1f, 1f).normalized();
                lightIntensity = 5f * noonFactor + 2.0f;

                averageLogLum = 0.3f;
                clearColor = DaySky;

                if (timeClock.CurrentTimeMicro < timeClock.DayStart + OneHour)
                {
                    float timeFactor = (timeClock.CurrentTimeMicro - timeClock.DayStart) / (float)OneHour;
                    clearColor = Color.FadeColors(timeFactor, DawnSky, DaySky);
                }

                if (timeClock.CurrentTimeMicro > timeClock.DayEnd - OneHour)
                {
                    float timeFactor = (timeClock.CurrentTimeMicro - (timeClock.DayEnd - OneHour)) / (float)OneHour;
                    clearColor = Color.FadeColors(timeFactor, DaySky, DuskSky);
                }
            }
            else
            {
                var nightFactor = (timeClock.NightFactor - 0.5f) * 2.0f;
                var midnightFactor = 1.0f - Math.Abs(nightFactor);
                lightDirection = new Vector3(nightFactor, -0.5f * midnightFactor - 0.1f, 1f).normalized();

                lightIntensity = 0.7f * midnightFactor + 2.0f;

                averageLogLum = 0.8f;
                clearColor = NightSky;

                if (timeClock.CurrentTimeMicro > timeClock.DayStart - OneHour && timeClock.CurrentTimeMicro <= timeClock.DayStart)
                {
                    float timeFactor = (timeClock.CurrentTimeMicro - (timeClock.DayStart - OneHour)) / (float)OneHour;
                    clearColor = Color.FadeColors(timeFactor, NightSky, DawnSky);
                }

                if (timeClock.CurrentTimeMicro >= timeClock.DayEnd && timeClock.CurrentTimeMicro < timeClock.DayEnd + OneHour)
                {
                    float timeFactor = (timeClock.CurrentTimeMicro - timeClock.DayEnd) / (float)OneHour;
                    clearColor = Color.FadeColors(timeFactor, DuskSky, NightSky);
                }
            }
        }
    }
}
