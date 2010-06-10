using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logging;
using System.Runtime.InteropServices;

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
        internal static Window createWrapper(IntPtr nativeObject, object[] args)
        {
            return new Window(nativeObject);
        }

        private IntPtr window;
        private WindowEventTranslator clickedTranslator;

        protected Window(IntPtr window)
        {
            clickedTranslator = new WindowEventTranslator("Clicked", this);
            this.window = window;
        }

        public virtual void Dispose()
        {
            clickedTranslator.Dispose();
            window = IntPtr.Zero;
        }

        public event CEGUIEvent Clicked
        {
            add
            {
                clickedTranslator.BoundEvent += value;
            }
            remove
            {
                clickedTranslator.BoundEvent -= value;
            }
        }

        internal IntPtr CEGUIWindow
        {
            get
            {
                return window;
            }
        }

        public Window getChildRecursive(String name)
        {
            return WindowManager.Singleton.getWindow(Window_getChildRecursive(window, name));
        }

#region PInvoke

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr Window_getChildRecursive(IntPtr window, String name);

#endregion
    }
}
