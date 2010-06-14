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
        static WindowManager instance;
        
        public static WindowManager Singleton
        {
            get
            {
                if(instance == null)
                {
                    instance = new WindowManager();
                }
                return instance;
            }
        }

        #region Wrapper
        private static readonly Type[] constructorArgs = { typeof(IntPtr) };
        static WindowTypeManager windowTypeManager = new WindowTypeManager();

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

        public Window createWindow(String windowType, String name)
        {
            return getWindow(WindowManager_createWindow(windowManager, windowType, name));
        }

        public void destroyWindow(Window window)
        {
            WindowManager_destroyWindow(windowManager, deleteWrapperAndChildren(window));
        }

        public void destroyWindow(String window)
        {
            WindowManager_destroyWindowString(windowManager, window);
        }

        public Window getWindow(String name)
        {
            return getWindow(WindowManager_getWindow(windowManager, name));
        }

        public bool isWindowPresent(String name)
        {
            return WindowManager_isWindowPresent(windowManager, name);
        }

        public void destroyAllWindows()
        {
            windows.clearObjects();
            WindowManager_destroyAllWindows(windowManager);
        }

        public Window loadWindowLayout(String layoutName)
        {
            return getWindow(WindowManager_loadWindowLayout(windowManager, layoutName));
        }

        public bool isDeadPoolEmpty()
        {
            return WindowManager_isDeadPoolEmpty(windowManager);
        }

        public void cleanDeadPool()
        {
            WindowManager_cleanDeadPool(windowManager);
        }

        public void renameWindow(String window, String newName)
        {
            WindowManager_renameWindowString(windowManager, window, newName);
        }

        public void renameWindow(Window window, String newName)
        {
            WindowManager_renameWindow(windowManager, window.CEGUIWindow, newName);
        }

        public void lockChanges()
        {
            WindowManager_lockChanges(windowManager);
        }

        public void unlockChanges()
        {
            WindowManager_unlockChanges(windowManager);
        }

        public bool isLocked()
        {
            return WindowManager_isLocked(windowManager);
        }

        public void DEBUG_dumpWindowNames(String zone)
        {
            WindowManager_DEBUG_dumpWindowNames(windowManager, zone);
        }

#region Internal Window Management

        internal Window getWindow(IntPtr nativeWindow)
        {
            if (nativeWindow != IntPtr.Zero)
            {
                return windows.getObject(nativeWindow);
            }
            return null;
        }

        internal IntPtr deleteWrapper(IntPtr window)
        {
            return windows.destroyObject(window);
        }

        /// <summary>
        /// This function will erase a wrapper and all child wrappers to avoid memory leaks.
        /// </summary>
        /// <param name="window">The window to destroy.</param>
        /// <returns>The pointer of the window that was destroyed.</returns>
        internal IntPtr deleteWrapperAndChildren(Window window)
        {
            window.eraseAllChildren();
            return deleteWrapper(window.CEGUIWindow);
        }

#endregion

#region PInvoke

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr WindowManager_getSingletonPtr();

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr WindowManager_createWindow(IntPtr windowManager, String windowType, String name);

        [DllImport("CEGUIWrapper")]
        private static extern void WindowManager_destroyWindow(IntPtr windowManager, IntPtr window);

        [DllImport("CEGUIWrapper")]
        private static extern void WindowManager_destroyWindowString(IntPtr windowManager, String window);

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr WindowManager_getWindow(IntPtr windowManager, String name);

        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool WindowManager_isWindowPresent(IntPtr windowManager, String name);

        [DllImport("CEGUIWrapper")]
        private static extern void WindowManager_destroyAllWindows(IntPtr windowManager);

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr WindowManager_loadWindowLayout(IntPtr windowManager, String layoutName);

        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool WindowManager_isDeadPoolEmpty(IntPtr windowManager);

        [DllImport("CEGUIWrapper")]
        private static extern void WindowManager_cleanDeadPool(IntPtr windowManager);

        [DllImport("CEGUIWrapper")]
        private static extern void WindowManager_renameWindowString(IntPtr windowManager, String window, String newName);

        [DllImport("CEGUIWrapper")]
        private static extern void WindowManager_renameWindow(IntPtr windowManager, IntPtr window, String newName);

        [DllImport("CEGUIWrapper")]
        private static extern void WindowManager_lockChanges(IntPtr windowManager);

        [DllImport("CEGUIWrapper")]
        private static extern void WindowManager_unlockChanges(IntPtr windowManager);

        [DllImport("CEGUIWrapper")]
        [return: MarshalAs(UnmanagedType.I1)]
        private static extern bool WindowManager_isLocked(IntPtr windowManager);

        [DllImport("CEGUIWrapper")]
        private static extern void WindowManager_DEBUG_dumpWindowNames(IntPtr windowManager, String zone);

#endregion
    }
}
