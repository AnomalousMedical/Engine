using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Performance
{
    public class Timelapse
    {
        private Int64 min = Int64.MaxValue;
        private Int64 max = 0;

        private Int64 endTime = 0;

        //Averages
        private Int64 totalTimeSpent = 0;
        private Int64 averageTime = 0;
        private Int64 numCalculations = 0;
        private bool recalcAverage = false;

        public Timelapse(String name)
        {
            this.Name = name;
        }

        public void resetStats()
        {
            min = Int64.MaxValue;
            max = 0;
            totalTimeSpent = 0;
            averageTime = 0;
            numCalculations = 0;
            recalcAverage = false;
        }

        public String Name { get; private set; }

        public Int64 StartTime { get; internal set; }

        public Int64 EndTime
        {
            get
            {
                return endTime;
            }
            internal set
            {
                endTime = value;
                recalcAverage = true;
            }
        }

        public Int64 TotalTimeSpent
        {
            get
            {
                return totalTimeSpent;
            }
        }

        public Int64 Duration
        {
            get
            {
                return EndTime - StartTime;
            }
        }

        public Int64 Min
        {
            get
            {
                Int64 duration = Duration;
                if (duration < min)
                {
                    min = duration;
                }
                return min;
            }
        }

        public Int64 Max
        {
            get
            {
                Int64 duration = Duration;
                if (duration > max)
                {
                    max = duration;
                }
                return max;
            }
        }

        /// <summary>
        /// Get the average of this timelapse since it was reset.
        /// </summary>
        public Int64 Average
        {
            get
            {
                computeAverages();
                return averageTime;
            }
        }

        private void computeAverages()
        {
            if (recalcAverage)
            {
                totalTimeSpent += Duration;
                averageTime = TotalTimeSpent / ++numCalculations;
                recalcAverage = false;
            }
        }
    }
}
