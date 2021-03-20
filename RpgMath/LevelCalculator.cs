using System;
using System.Collections.Generic;
using System.Text;

namespace RpgMath
{
    public class LevelCalculator
    {
        private Random random = new Random();

        private PrimaryStatCurves primaryStatCurves = new PrimaryStatCurves();

        public long ComputePrimaryStatGain(int level, int rank, int current)
        {
            var curve = primaryStatCurves.GetStatCurve(level, rank);
            long baselineStat = curve.Baseline + (curve.Gradient * level / 100);

            long diff = (random.Next(8) + 1) + baselineStat - current;

            if(diff < 0)
            {
                diff = 0;
            }
            else if(diff > 11)
            {
                diff = 11;
            }

            switch (diff)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    return 0;
                case 4:
                case 5:
                case 6:
                    return 1;
                case 7:
                case 8:
                case 9:
                    return 2;
                case 10:
                case 11:
                    return 3;
                default:
                    throw new InvalidOperationException("Should not happen");
            }
        }
    }
}
