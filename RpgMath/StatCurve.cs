using System;
using System.Collections.Generic;
using System.Text;

namespace RpgMath
{
    class StatCurve
    {
        public StatCurve(long gradient, long baseline)
        {
            Gradient = gradient;
            Baseline = baseline;
        }

        public long Gradient { get; }

        public long Baseline { get; }
    }

    class StatCurveLevelRange
    {
        private readonly List<StatCurve> ranks;

        public StatCurveLevelRange(List<StatCurve> ranks)
        {
            this.ranks = ranks;
        }

        public StatCurve this[int index]
        {
            get
            {
                return ranks[index];
            }
        }
    }
}                       