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
        private readonly ITimeClock timeClock;

        //Clear Color
        Color clearColor = Color.FromARGB(0xff2a63cc);

        //Light
        private Vector3 sunPosition;
        private Vector3 moonPosition;
        Vector4 lightColor = new Vector4(1, 1, 1, 1);
        float lightIntensity = 3;
        float averageLogLum = 0.3f;

        public Color ClearColor => clearColor;

        public Vector3 SunPosition => sunPosition;

        public Vector3 MoonPosition => moonPosition;
        public Vector4 LightColor => lightColor;
        public float LightIntensity => lightIntensity;

        public float AverageLogLum => averageLogLum;

        public Sky(ITimeClock timeClock)
        {
            this.timeClock = timeClock;
        }

        public unsafe void UpdateLight(Clock clock)
        {
            var rotation = new Quaternion(Vector3.UnitZ, timeClock.TimeFactor * 2 * MathF.PI);
            sunPosition = Quaternion.quatRotate(rotation, Vector3.Down) * 40;
            sunPosition += new Vector3(0f, 0f, -20f);

            moonPosition = Quaternion.quatRotate(rotation, Vector3.Up) * 40;
            moonPosition += new Vector3(0f, 0f, -20f);

            if (timeClock.IsDay)
            {
                var dayFactor = (timeClock.DayFactor - 0.5f) * 2.0f;
                var noonFactor = 1.0f - Math.Abs(dayFactor);
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
