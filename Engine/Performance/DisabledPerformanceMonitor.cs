using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Performance
{
    class DisabledPerformanceMonitor : PerformanceMonitorState
    {
        public void start(string name)
        {
            
        }

        public void stop(string name)
        {
            
        }

        public Timelapse this[String name]
        {
            get
            {
                return null;
            }
        }

        public IEnumerable<Timelapse> Timelapses
        {
            get
            {
                yield break;
            }
        }
    }
}
