using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Performance
{
    public class Timelapse
    {
        public Timelapse(String name)
        {
            this.Name = name;
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
    }
}
