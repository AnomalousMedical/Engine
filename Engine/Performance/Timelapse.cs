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

        public Timelapse(String name)
        {
            this.Name = name;
        }

        public void resetMinMax()
        {
            min = Int64.MaxValue;
            max = 0;
        }

        public String Name { get; private set; }

        public Int64 StartTime { get; internal set; }

        public Int64 EndTime { get; internal set; }

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
    }
}
