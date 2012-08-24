using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;

namespace Engine.Performance
{
    class EnabledPerformanceMonitor : PerformanceMonitorState
    {
        private SystemTimer timer;
        private Dictionary<String, Timelapse> timelapses = new Dictionary<string, Timelapse>();

        public EnabledPerformanceMonitor(SystemTimer timer)
        {
            this.timer = timer;
            timer.initialize();
        }

        public void start(string name)
        {
            Timelapse timelapse = getTimelapse(name);
            timelapse.StartTime = timer.getCurrentTime() / 1000;
        }

        public void stop(string name)
        {
            Timelapse timelapse = getTimelapse(name);
            timelapse.EndTime = timer.getCurrentTime() / 1000;
        }

        public Timelapse this[String name]
        {
            get
            {
                Timelapse timelapse;
                timelapses.TryGetValue(name, out timelapse);
                return timelapse;
            }
        }

        public IEnumerable<Timelapse> Timelapses
        {
            get
            {
                return timelapses.Values;
            }
        }

        private Timelapse getTimelapse(String name)
        {
            Timelapse timelapse;
            if (!timelapses.TryGetValue(name, out timelapse))
            {
                timelapse = new Timelapse(name);
                timelapses.Add(name, timelapse);
            }
            return timelapse;
        }
    }
}
