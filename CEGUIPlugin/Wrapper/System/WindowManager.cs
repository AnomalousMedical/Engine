using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Engine;
using System.Reflection;

namespace CEGUIPlugin
{
    public class WindowManager : IDisposable
    {
        static WindowManager instance = new WindowManager();
        static WindowTypeManager windowTypeManager;

        static WindowManager()
        {
            //Base window
            windowTypeManager = new WindowTypeManager(typeof(Window));

            //Buttons
            windowTypeManager.addLeafType(typeof(PushButton));
        }

        public static WindowManager Singleton
        {
            get
            {
                return instance;
            }
        }

        private static readonly Type[] constructorArgs = { typeof(IntPtr) };

        private static Window createWrapper(IntPtr nativeObject, object[] args)
        {
            Type windowType = windowTypeManager.searchType(nativeObject);
            ConstructorInfo constructor = windowType.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, constructorArgs, null);
            return (Window)constructor.Invoke(new Object[] { nativeObject });
        }

        private IntPtr windowManager;
        private WrapperCollection<Window> windows = new WrapperCollection<Window>(createWrapper);

        private WindowManager()
        {
            windowManager = WindowManager_getSingletonPtr();
        }

        public void Dispose()
        {
            windows.Dispose();
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
