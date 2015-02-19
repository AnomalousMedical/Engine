using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;

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
        private OgreWindowListener ogreWindowListener;

        public OgreOSWindow(RenderWindow window)
        {
            this.window = window;
            handle = new IntPtr(long.Parse(window.getWindowHandleStr()));
            ogreWindowListener = new OgreWindowListener(this);
            WindowEventUtilities.addWindowEventListener(window, ogreWindowListener);
            Focused = true;
        }

        public void Dispose()
        {
            WindowEventUtilities.removeWindowEventListener(window, ogreWindowListener);
            ogreWindowListener.Dispose();
            window.destroy();
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

        public float WindowScaling
        {
            get
            {
                return 1.0f;
            }
        }

        public bool Focused { get; private set; }

        public RenderWindow RenderWindow
        {
            get
            {
                return window;
            }
        }

        public event OSWindowEvent Moved;

        public event OSWindowEvent Resized;

        public event OSWindowEvent Closing;

        public event OSWindowEvent Closed;

        public event OSWindowEvent FocusChanged;

        internal void _fireWindowMoved()
        {
            if(Moved != null)
            {
                Moved.Invoke(this);
            }
        }

        internal void _fireWindowResized()
        {
            if(Resized != null)
            {
                Resized.Invoke(this);
            }
        }

        internal void _fireWindowClosing()
        {
            if(Closing != null)
            {
                Closing.Invoke(this);
            }
        }

        internal void _fireWindowClosed()
        {
            if(Closed != null)
            {
                Closed.Invoke(this);
            }
        }

        internal void _fireFocusChanged()
        {
            Focused = !Focused;
            if(FocusChanged != null)
            {
                FocusChanged.Invoke(this);
            }
        }
    }
}
