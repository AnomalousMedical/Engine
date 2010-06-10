using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine;

namespace CEGUIPlugin
{
    public class WindowManager : IDisposable
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

        public void Dispose()
        {
            windows.Dispose();
        }

        internal T getWindow<T>(IntPtr nativeWindow)
            where T : Window
        {
            if(nativeWindow != IntPtr.Zero)
            {
                return windows.getObject(nativeWindow, typeof(T)) as T;
            }
            return null;
        }

        public T loadWindowLayout<T>(String layoutName)
            where T : Window
        {
            return getWindow<T>(WindowManager_loadWindowLayout(windowManager, layoutName));
        }

#region PInvoke

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr WindowManager_getSingletonPtr();

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr WindowManager_loadWindowLayout(IntPtr windowManager, String layoutName);

#endregion
    }
}
