using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgMath
{
    public class StandardEnemyCurve
    {
        public long GetHp(int level)
        {
            if(level < 10)
            {
                //1-10
                return (long)NumberFunctions.lerp(40f, 200f, (level) / 10f);
            }
            else if(level < 20)
            {
                //10-20
                return (long)NumberFunctions.lerp(150f, 300f, (level - 10) / 10f);
            }
            else if (level < 30)
            {
                //20-30
                return (long)NumberFunctions.lerp(450f, 1200f, (level - 20) / 10f);
            }
            else if (level < 40)
            {
                //30-40
                return (long)NumberFunctions.lerp(1700f, 4200f, (level - 30) / 10f);
            }
            else if (level < 50)
            {
                //40-50
                return (long)NumberFunctions.lerp(6000f, 11000f, (level - 40) / 10f);
            }
            else if (level < 60)
            {
                //50-60
                return (long)NumberFunctions.lerp(12000f, 16000f, (level - 50) / 10f);
            }
            else if (level < 70)
            {
                //60-70
                return (long)NumberFunctions.lerp(20000f, 22000f, (level - 60) / 10f);
            }
            else if (level < 80)
            {
                //70-80
                return (long)NumberFunctions.lerp(27000f, 38000f, (level - 70) / 10f);
            }
            else if (level < 90)
            {
                //80-90
                return (long)NumberFunctions.lerp(40000f, 51000f, (level - 80) / 10f);
            }
            else if (level < 100)
            {
                //90-99
                return (long)NumberFunctions.lerp(55000f, 65000f, (level - 90) / 10f);
            }

            throw new InvalidOperationException($"Level '{level}' is not supported.");
        }

        public long GetAtt(int level)
        {
            //Pretty linear overall
            if (level < 10)
            {
                //1-10
                return (long)NumberFunctions.lerp(6f, 25f, (level) / 10f);
            }
            else if (level < 20)
            {
                //10-20
                return (long)NumberFunctions.lerp(25f, 47f, (level - 10) / 10f);
            }
            else if (level < 30)
            {
                //20-30
                return (long)NumberFunctions.lerp(47f, 65f, (level - 20) / 10f);
            }
            else if (level < 40)
            {
                //30-40
                return (long)NumberFunctions.lerp(65f, 90f, (level - 30) / 10f);
            }
            else if (level < 50)
            {
                //40-50
                return (long)NumberFunctions.lerp(90f, 115f, (level - 40) / 10f);
            }
            else if (level < 60)
            {
                //50-60
                return (long)NumberFunctions.lerp(115f, 145f, (level - 50) / 10f);
            }
            else if (level < 70)
            {
                //60-70
                return (long)NumberFunctions.lerp(145f, 165f, (level - 60) / 10f);
            }
            else if (level < 80)
            {
                //70-80
                return (long)NumberFunctions.lerp(165f, 200f, (level - 70) / 10f);
            }
            else if (level < 90)
            {
                //80-90
                return (long)NumberFunctions.lerp(200f, 225f, (level - 80) / 10f);
            }
            else if (level < 100)
            {
                //90-99
                return (long)NumberFunctions.lerp(225f, 255f, (level - 90) / 10f);
            }

            throw new InvalidOperationException($"Level '{level}' is not supported.");
        }

        public long GetDef(int level)
        {
            //Defense could go up to 2x for badass enemies
            if (level < 10)
            {
                //1-10
                return (long)NumberFunctions.lerp(8f, 25f, (level) / 10f);
            }
            else if (level < 20)
            {
                //10-20
                return (long)NumberFunctions.lerp(25f, 45f, (level - 10) / 10f);
            }
            else if (level < 30)
            {
                //20-30
                return (long)NumberFunctions.lerp(45f, 55f, (level - 20) / 10f);
            }
            else if (level < 40)
            {
                //30-40
                return (long)NumberFunctions.lerp(55f, 80f, (level - 30) / 10f);
            }
            else if (level < 50)
            {
                //40-50
                return (long)NumberFunctions.lerp(80f, 105f, (level - 40) / 10f);
            }
            else if (level < 60)
            {
                //50-60
                return (long)NumberFunctions.lerp(105f, 135f, (level - 50) / 10f);
            }
            else if (level < 70)
            {
                //60-70
                return (long)NumberFunctions.lerp(135f, 155f, (level - 60) / 10f);
            }
            else if (level < 80)
            {
                //70-80
                return (long)NumberFunctions.lerp(155f, 180f, (level - 70) / 10f);
            }
            else if (level < 90)
            {
                //80-90
                return (long)NumberFunctions.lerp(180f, 215f, (level - 80) / 10f);
            }
            else if (level < 100)
            {
                //90-99
                return (long)NumberFunctions.lerp(215f, 250f, (level - 90) / 10f);
            }

            throw new InvalidOperationException($"Level '{level}' is not supported.");
        }
    }
}
