﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace libRocketPlugin
{
    public class Factory
    {
        /// Clears the style sheet cache. This will force style sheets to be reloaded.
        public static void ClearStyleSheetCache()
        {
            Factory_ClearStyleSheetCache();
        }

        /// Registers an instancer for all events.
        /// @param[in] instancer The instancer to be called.
        /// @return The registered instanced on success, NULL on failure.
        public static EventInstancer RegisterEventInstancer(EventInstancer instancer)
        {
            Factory_RegisterEventInstancer(instancer.Ptr);
            return instancer;
        }

        /// Register the instancer to be used for all event listeners.
        /// @return The registered instancer on success, NULL on failure.
        public static EventListenerInstancer RegisterEventListenerInstancer(EventListenerInstancer instancer)
        {
            Factory_RegisterEventListenerInstancer(instancer.Ptr);
            return instancer;
        }

        public static bool InstanceElementString(Element parent, String rml)
        {
            return Factory_InstanceElementString(parent.Ptr, rml);
        }

        #region PInvoke

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Factory_ClearStyleSheetCache();

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Factory_RegisterEventInstancer(IntPtr instancer);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Factory_RegisterEventListenerInstancer(IntPtr instancer);

        [DllImport(RocketInterface.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool Factory_InstanceElementString(IntPtr element, String rml);

        #endregion
    }
}
