using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logging;
using System.Runtime.InteropServices;
using System.Reflection;

namespace CEGUIPlugin
{
    public enum VerticalAlignment
    {
        /**
         * Window's position specifies an offset of it's top edge from the top edge
         * of it's parent.
         */
        VA_TOP,
        /**
         * Window's position specifies an offset of it's vertical centre from the
         * vertical centre of it's parent.
         */
        VA_CENTRE,
        /**
         * Window's position specifies an offset of it's bottom edge from the
         * bottom edge of it's parent.
         */
        VA_BOTTOM
    };

    public enum HorizontalAlignment
    {
        /**
         * Window's position specifies an offset of it's left edge from the left
         * edge of it's parent.
         */
        HA_LEFT,
        /**
         * Window's position specifies an offset of it's horizontal centre from the
         * horizontal centre of it's parent.
         */
        HA_CENTRE,
        /**
         * Window's position specifies an offset of it's right edge from the right
         * edge of it's parent.
         */
        HA_RIGHT
    };

    public delegate void CEGUIEvent(EventArgs e);

    public class Window : IDisposable
    {
        private static readonly Type[] constructorArgs = { typeof(IntPtr) };

        internal static Window createWrapper(IntPtr nativeObject, object[] args)
        {
            Type windowType = args[0] as Type;
            ConstructorInfo constructor = windowType.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, constructorArgs, null);
            return (Window)constructor.Invoke(new Object[] { nativeObject });
            //return new Window(nativeObject);
        }

        private IntPtr window;

        protected Window(IntPtr window)
        {
            this.window = window;
        }

        public virtual void Dispose()
        {
            window = IntPtr.Zero;
        }

        internal IntPtr CEGUIWindow
        {
            get
            {
                return window;
            }
        }

        public T getChildRecursive<T>(String name)
            where T : Window
        {
            return WindowManager.Singleton.getWindow<T>(Window_getChildRecursive(window, name));
        }

#region PInvoke

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr Window_getChildRecursive(IntPtr window, String name);

#endregion
    }
}
