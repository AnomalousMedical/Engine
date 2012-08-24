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

        public static void start(String name)
        {
            currentState.start(name);
        }

        public static void stop(String name)
        {
            currentState.stop(name);
        }

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
