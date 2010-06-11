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
        
        public static WindowManager Singleton
        {
            get
            {
                return instance;
            }
        }

        #region Wrapper
        private static readonly Type[] constructorArgs = { typeof(IntPtr) };
        static WindowTypeManager windowTypeManager;

        static WindowManager()
        {
            //Base window
            windowTypeManager = new WindowTypeManager(typeof(Window));

            //Buttons
            windowTypeManager.addLeafType(typeof(PushButton));
        }

        private static Window createWrapper(IntPtr nativeObject, object[] args)
        {
            Type windowType = windowTypeManager.searchType(nativeObject);
            ConstructorInfo constructor = windowType.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, constructorArgs, null);
            return (Window)constructor.Invoke(new Object[] { nativeObject });
        }
        #endregion Wrapper

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

        internal IntPtr deleteWrapper(IntPtr window)
        {
            return windows.destroyObject(window);
        }

        internal IntPtr deleteWrapperAndChildren(Window window)
        {
            window.eraseAllChildren();
            return deleteWrapper(window.CEGUIWindow);
        }

        public void destroyWindow(Window window)
        {
            WindowManager_destroyWindow(windowManager, deleteWrapperAndChildren(window));
        }

        public Window loadWindowLayout(String layoutName)
        {
            return getWindow(WindowManager_loadWindowLayout(windowManager, layoutName));
        }

#region PInvoke

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr WindowManager_getSingletonPtr();

        [DllImport("CEGUIWrapper")]
        private static extern void WindowManager_destroyWindow(IntPtr windowManager, IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr WindowManager_loadWindowLayout(IntPtr windowManager, String layoutName);

#endregion
    }
}
