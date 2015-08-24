using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using System.Collections.Concurrent;

namespace Engine.Performance
{
    class EnabledPerformanceMonitor : PerformanceMonitorState
    {
        private SystemTimer timer;
        private ConcurrentDictionary<String, Timelapse> timelapses = new ConcurrentDictionary<string, Timelapse>();
        private ConcurrentDictionary<String, Timelapse> newTimelapses = new ConcurrentDictionary<string, Timelapse>();

        public EnabledPerformanceMonitor(SystemTimer timer)
        {
            this.timer = timer;
            timer.initialize();
        }

        public void start(string name)
        {
            long time = timer.getCurrentTime() / 1000;
            Timelapse timelapse = newTimelapses.GetOrAdd(name, passName => new Timelapse(passName) { StartTime = time } );
            timelapse.StartTime = time;
        }

        public void stop(string name)
        {
            Timelapse timelapse;
            if (newTimelapses.TryRemove(name, out timelapse))
            {
                timelapse.EndTime = timer.getCurrentTime() / 1000;
                timelapses.AddOrUpdate(name, timelapse, (key, value) =>
                {
                    value.StartTime = timelapse.StartTime;
                    value.EndTime = timelapse.EndTime;
                    return value;
                });
            }
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
    }
}
