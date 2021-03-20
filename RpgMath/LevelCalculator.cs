using System;
using System.Collections.Generic;
using System.Text;

namespace RpgMath
{
    public class LevelCalculator
    {
        private Random random = new Random();

        private PrimaryStatCurves primaryStatCurves = new PrimaryStatCurves();
        private LuckStatCurves luckStatCurves = new LuckStatCurves();
        private HpStatCurves hpStatCurves = new HpStatCurves();

        public long ComputePrimaryStatGain(long level, int rank, long current)
        {
            var curve = primaryStatCurves.GetStatCurve(level, rank);
            return ComputeStatGain(level, current, curve);
        }

        public long ComputeLuckGain(long level, int rank, long current)
        {
            var curve = luckStatCurves.GetStatCurve(level, rank);
            return ComputeStatGain(level, current, curve);
        }

        private long ComputeStatGain(long level, long current, StatCurve curve)
        {
            long baselineStat = curve.Baseline + (curve.Gradient * level / 100);

            long diff = (random.Next(8) + 1) + baselineStat - current;

            if (diff < 0)
            {
                diff = 0;
            }
            else if (diff > 11)
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

        public long ComputeHpGain(long level, int rank, long current)
        {
            var curve = hpStatCurves.GetStatCurve(level, rank);
            long baselineStat = curve.Baseline + (level - 1) * curve.Gradient;
            long diff = (random.Next(8) + 1) + (100L * baselineStat / current) - 100;

            if (diff < 0)
            {
                diff = 0;
            }
            else if (diff > 11)
            {
                diff = 11;
            }

            switch (diff)
            {
                case 0:
                    return (long)(curve.Gradient * 0.4f);
                case 1:
                case 2:
                    return (long)(curve.Gradient * 0.5f);
                case 3:
                    return (long)(curve.Gradient * 0.6f);
                case 4:
                    return (long)(curve.Gradient * 0.7f);
                case 5:
                    return (long)(curve.Gradient * 0.8f);
                case 6:
                    return (long)(curve.Gradient * 0.9f);
                case 7:
                    return curve.Gradient;
                case 8:
                    return (long)(curve.Gradient * 1.1f);
                case 9:
                    return (long)(curve.Gradient * 1.2f);
                case 10:
                    return (long)(curve.Gradient * 1.3f);
                case 11:
                    return (long)(curve.Gradient * 1.5f);
                default:
                    throw new InvalidOperationException("Should not happen");
            }
        }
    }
}
