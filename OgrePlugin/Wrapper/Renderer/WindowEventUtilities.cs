﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace OgreWrapper
{
    class WindowEventUtilities
    {
        public static void addWindowEventListener(RenderWindow window, WindowEventListener listener)
        {
            WindowEventUtilities_addWindowEventListener(window.OgreRenderTarget, listener.OgreListener);
        }

        public static void removeWindowEventListener(RenderWindow window, WindowEventListener listener)
        {
            WindowEventUtilities_removeWindowEventListener(window.OgreRenderTarget, listener.OgreListener);
        }

#region PInvoke

        [DllImport("OgreCWrapper")]
        private static extern void WindowEventUtilities_addWindowEventListener(IntPtr window, IntPtr listener);

        [DllImport("OgreCWrapper")]
        private static extern void WindowEventUtilities_removeWindowEventListener(IntPtr window, IntPtr listener);

#endregion
    }
}
