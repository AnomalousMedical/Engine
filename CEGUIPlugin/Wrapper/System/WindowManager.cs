using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine;

namespace CEGUIPlugin
{
    public class WindowManager
    {
        static WindowManager instance = new WindowManager();

        public static WindowManager Singleton
        {
            get
            {
                return instance;
            }
        }

        private IntPtr windowManager;
        private WrapperCollection<Window> windows = new WrapperCollection<Window>(Window.createWrapper);

        private WindowManager()
        {
            windowManager = WindowManager_getSingletonPtr();
        }

        internal Window getWindow(IntPtr nativeWindow)
        {
            if(nativeWindow != IntPtr.Zero)
            {
                return windows.getObject(nativeWindow);
            }
            return null;
        }

        public Window loadWindowLayout(String layoutName)
        {
            return getWindow(WindowManager_loadWindowLayout(windowManager, layoutName));
        }

#region PInvoke

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr WindowManager_getSingletonPtr();

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr WindowManager_loadWindowLayout(IntPtr windowManager, String layoutName);

#endregion
    }
}
