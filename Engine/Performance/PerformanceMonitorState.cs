using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Performance
{
    interface PerformanceMonitorState
    {
        void start(string name);

        void stop(string name);

        Timelapse this[String name]
        {
            get;
        }

        IEnumerable<Timelapse> Timelapses
        {
            get;
        }
    }
}
