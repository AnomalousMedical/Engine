using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;
using OgreWrapper;

namespace OgrePlugin
{
    /// <summary>
    /// An OSWindow for Ogre's auto-created windows. This class can be enhanced
    /// to fire the window events using WindowEventUtilties in ogre, but this is
    /// not wrapped so it not currently supported.
    /// </summary>
    class OgreOSWindow : OSWindow, IDisposable
    {
        private RenderWindow window;
        private IntPtr handle;

        public OgreOSWindow(RenderWindow window)
        {
            this.window = window;
            uint mHandle;
            unsafe
            {
                window.getCustomAttribute("WINDOW", &mHandle);
            }
            handle = new IntPtr(mHandle);
        }

        public IntPtr WindowHandle
        {
            get
            {
                return handle;
            }
        }

        public int WindowWidth
        {
            get
            {
                return (int)window.getWidth();
            }
        }

        public int WindowHeight
        {
            get
            {
                return (int)window.getHeight();
            }
        }

        public void addListener(OSWindowListener listener)
        {

        }

        public void removeListener(OSWindowListener listener)
        {

        }

        public void Dispose()
        {
            //for now does nothing will do stuff when the callbacks are hooked up
        }

        public RenderWindow RenderWindow
        {
            get
            {
                return window;
            }
        }
    }
}
