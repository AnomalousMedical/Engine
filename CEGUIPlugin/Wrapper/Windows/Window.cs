using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CEGUIPlugin
{
    public class Window : IDisposable
    {
        internal static Window createWrapper(IntPtr nativeObject, object[] args)
        {
            return new Window(nativeObject);
        }

        private IntPtr window;

        private Window(IntPtr window)
        {
            this.window = window;
        }

        public void Dispose()
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
    }
}
