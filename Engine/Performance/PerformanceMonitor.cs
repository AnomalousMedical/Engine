using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Performance;
using Engine.Platform;

namespace Engine
{
    public static class PerformanceMonitor
    {
        private static PerformanceMonitorState currentState;
        private static PerformanceMonitorState disabledState;
        private static PerformanceMonitorState enabledState;
        private static bool enabled;

        static PerformanceMonitor()
        {
            currentState = disabledState = new DisabledPerformanceMonitor();
        }

        /// <summary>
        /// Set the start time for a counter, this is sort of thread safe, however, start and stop should be called on the same thread.
        /// </summary>
        /// <param name="name">The name of the counter to set the start time for.</param>
        public static void start(String name)
        {
            currentState.start(name);
        }

        /// <summary>
        /// Set the stop time for a counter, this is sort of thread safe, however, start and stop should be called on the same thread.
        /// </summary>
        /// <param name="name">The name of the counter to set the stop time for.</param>
        public static void stop(String name)
        {
            currentState.stop(name);
        }

        /// <summary>
        /// Get an enum over the timelapses. Do not call this any time a background thread could be calling start or stop.
        /// </summary>
        public static IEnumerable<Timelapse> Timelapses
        {
            get
            {
                return currentState.Timelapses;
            }
        }

        public static void setupEnabledState(SystemTimer timer)
        {
            enabledState = new EnabledPerformanceMonitor(timer);
            toggleActuallyEnabled();
        }

        public static void destroyEnabledState()
        {
            enabledState = null;
            toggleActuallyEnabled();
        }

        public static bool Enabled
        {
            get
            {
                return enabledState != null && enabled;
            }
            set
            {
                enabled = value;
                toggleActuallyEnabled();
            }
        }

        private static void toggleActuallyEnabled()
        {
            if (enabled && enabledState != null)
            {
                currentState = enabledState;
            }
            else
            {
                currentState = disabledState;
            }
        }
    }
}
