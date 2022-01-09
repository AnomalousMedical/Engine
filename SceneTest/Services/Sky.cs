using DiligentEngine.RT;
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
        readonly Color[] DaySky = new Color[6] { Color.FromARGB(0xff2a63cc), Color.FromARGB(0xff2a63cc), Color.FromARGB(0xff2a63cc), Color.FromARGB(0xff2a63cc), Color.FromARGB(0xff2a63cc), Color.FromARGB(0xff2a63cc) };
        readonly Color[] NightSky = new Color[6] { Color.FromARGB(0xff030303), Color.FromARGB(0xff030303), Color.FromARGB(0xff030303), Color.FromARGB(0xff030303), Color.FromARGB(0xff030303), Color.FromARGB(0xff030303) };
        readonly Color[] DawnSky = new Color[6] { Color.FromARGB(0xff1f2b5f), Color.FromARGB(0xff7a5c9c), Color.FromARGB(0xff7a5c9c), Color.FromARGB(0xff7a5c9c), Color.FromARGB(0xff7a5c9c), Color.FromARGB(0xff7a5c9c) };
        readonly Color[] DuskSky = new Color[6] { Color.FromARGB(0xff811d5e), Color.FromARGB(0xff983275), Color.FromARGB(0xfffd2f24), Color.FromARGB(0xffff6f01), Color.FromARGB(0xfffed800), Color.FromARGB(0xfffed800) };
        private readonly ITimeClock timeClock;
        private readonly RTCameraAndLight cameraAndLight;

        //Clear Color
        //private Color[] skyPallet = new Color[6];

        //Light
        private Vector3 sunPosition;
        private Vector3 moonPosition;
        Vector4 lightColor = new Vector4(1, 1, 1, 1);
        float lightIntensity = 3;
        float averageLogLum = 0.3f;

        public Sky(ITimeClock timeClock, RTCameraAndLight cameraAndLight)
        {
            this.timeClock = timeClock;
            this.cameraAndLight = cameraAndLight;
        }

        const float LightDistance = 10000.0f;

        public unsafe void UpdateLight(Clock clock)
        {
            var rotation = new Quaternion(Vector3.UnitZ, timeClock.TimeFactor * 2 * MathF.PI);
            sunPosition = Quaternion.quatRotate(rotation, Vector3.Down) * LightDistance;
            sunPosition += new Vector3(0f, 0f, -LightDistance);

            moonPosition = Quaternion.quatRotate(rotation, Vector3.Up) * LightDistance;
            moonPosition += new Vector3(0f, 0f, -LightDistance);

            if (timeClock.IsDay)
            {
                var dayFactor = (timeClock.DayFactor - 0.5f) * 2.0f;
                var noonFactor = 1.0f - Math.Abs(dayFactor);
                lightIntensity = 5f * noonFactor + 2.0f;

                averageLogLum = 0.3f;

                if (timeClock.CurrentTimeMicro < timeClock.DayStart + OneHour)
                {
                    float timeFactor = (timeClock.CurrentTimeMicro - timeClock.DayStart) / (float)OneHour;
                    BlendSetPallet(timeFactor, DawnSky, DaySky, cameraAndLight.MissPallete);
                }
                else if (timeClock.CurrentTimeMicro > timeClock.DayEnd - OneHour)
                {
                    float timeFactor = (timeClock.CurrentTimeMicro - (timeClock.DayEnd - OneHour)) / (float)OneHour;
                    BlendSetPallet(timeFactor, DaySky, DuskSky, cameraAndLight.MissPallete);
                }
                else
                {
                    SetPallet(DaySky, cameraAndLight.MissPallete);
                }
            }
            else
            {
                var nightFactor = (timeClock.NightFactor - 0.5f) * 2.0f;
                var midnightFactor = 1.0f - Math.Abs(nightFactor);

                lightIntensity = 0.7f * midnightFactor + 2.0f;

                averageLogLum = 0.8f;

                if (timeClock.CurrentTimeMicro > timeClock.DayStart - OneHour && timeClock.CurrentTimeMicro <= timeClock.DayStart)
                {
                    float timeFactor = (timeClock.CurrentTimeMicro - (timeClock.DayStart - OneHour)) / (float)OneHour;
                    BlendSetPallet(timeFactor, NightSky, DawnSky, cameraAndLight.MissPallete);
                }
                else if (timeClock.CurrentTimeMicro >= timeClock.DayEnd && timeClock.CurrentTimeMicro < timeClock.DayEnd + OneHour)
                {
                    float timeFactor = (timeClock.CurrentTimeMicro - timeClock.DayEnd) / (float)OneHour;
                    BlendSetPallet(timeFactor, DuskSky, NightSky, cameraAndLight.MissPallete);
                }
                else
                {
                    SetPallet(NightSky, cameraAndLight.MissPallete);
                }
            }

            cameraAndLight.Light1Pos = new Vector4(sunPosition.x, sunPosition.y, sunPosition.z, 0);
            cameraAndLight.Light2Pos = new Vector4(moonPosition.x, moonPosition.y, moonPosition.z, 0);
        }

        private void SetPallet(Color[] src, Color[] dest)
        {
            var length = src.Length;
            for (var i = 0; i < length; ++i)
            {
                dest[i] = src[i];
            }
        }

        private void BlendSetPallet(float factor, Color[] color1, Color[] color2, Color[] dest)
        {
            var length = color1.Length;
            for(var i = 0; i < length; ++i)
            {
                dest[i] = Color.FadeColors(factor, color1[i], color2[i]);
            }
        }
    }
}
