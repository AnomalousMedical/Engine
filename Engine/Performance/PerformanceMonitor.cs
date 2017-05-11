using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Performance;
using Engine.Platform;
using System.Reflection;
using Engine.Shim;

namespace Engine
{
    public static class PerformanceMonitor
    {
        private static PerformanceMonitorState currentState;
        private static PerformanceMonitorState disabledState;
        private static PerformanceMonitorState enabledState;
        private static bool enabled;
        private static List<PerformanceValueProvider> valueProviders = new List<PerformanceValueProvider>();

        static PerformanceMonitor()
        {
            currentState = disabledState = new DisabledPerformanceMonitor();
            addValueProvider("Private Memory", () => Prettify.GetSizeReadable(NetFrameworkShim.ProcessInfo.PrivateMemorySize64));
            addValueProvider("Working Set", () => Prettify.GetSizeReadable(NetFrameworkShim.ProcessInfo.WorkingSet64));
            addValueProvider("Virtual Memory", () => Prettify.GetSizeReadable(NetFrameworkShim.ProcessInfo.VirtualMemorySize64));
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

        public static void addValueProvider(String name, Func<String> getValueFunc)
        {
            valueProviders.Add(new PerformanceValueProvider(name, getValueFunc));
        }

        public static void removeValueProvider(String name)
        {
            valueProviders.RemoveAll(m => m.Name == name);
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

        /// <summary>
        /// Get an enum over the performance values. These can be any value that should be output, for example the memory usage of a resource manager.
        public static IEnumerable<PerformanceValueProvider> PerformanceValues
        {
            get
            {
                return valueProviders;
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
