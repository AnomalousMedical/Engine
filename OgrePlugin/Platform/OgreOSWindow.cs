﻿using System;
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
        private List<OSWindowListener> windowListeners = new List<OSWindowListener>();
        private OgreWindowListener ogreWindowListener;

        public OgreOSWindow(RenderWindow window)
        {
            this.window = window;
            uint mHandle;
            unsafe
            {
                window.getCustomAttribute("WINDOW", &mHandle);
            }
            handle = new IntPtr(mHandle);
            ogreWindowListener = new OgreWindowListener(this);
            WindowEventUtilities.addWindowEventListener(window, ogreWindowListener);
        }

        public void Dispose()
        {
            WindowEventUtilities.removeWindowEventListener(window, ogreWindowListener);
            ogreWindowListener.Dispose();
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
            windowListeners.Add(listener);
        }

        public void removeListener(OSWindowListener listener)
        {
            windowListeners.Remove(listener);
        }

        public RenderWindow RenderWindow
        {
            get
            {
                return window;
            }
        }

        internal void _fireWindowMoved()
        {
            foreach (OSWindowListener listener in windowListeners)
            {
                listener.moved(this);
            }
        }

        internal void _fireWindowResized()
        {
            foreach (OSWindowListener listener in windowListeners)
            {
                listener.resized(this);
            }
        }

        internal void _fireWindowClosing()
        {
            foreach (OSWindowListener listener in windowListeners)
            {
                listener.closing(this);
            }
        }
    }
}
