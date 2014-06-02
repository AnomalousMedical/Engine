using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;

namespace Engine.Renderer
{
    public class WindowInfoEventArgs : EventArgs
    {
        public WindowInfoEventArgs(RendererWindow createdWindow)
        {
            CreatedWindow = createdWindow;
        }

        public RendererWindow CreatedWindow { get; private set; }
    }

    /// <summary>
    /// This class controls how the windows are created for a RendererPlugin.
    /// </summary>
    public class WindowInfo
    {
        public event EventHandler WindowCreated;

        private int width = -1;
        private int height = -1;
        private bool fullscreen = false;

        /// <summary>
        /// Constructor to have the renderer auto-create its window with the given title.
        /// </summary>
        /// <param name="windowTitle">The title to give to the renderer's auto-created window.</param>
        public WindowInfo(String windowTitle, int width, int height)
        {
            AutoCreateWindow = true;
            AutoWindowTitle = windowTitle;
            this.width = width;
            this.height = height;
            MonitorIndex = 0;
        }

        /// <summary>
        /// Constructor to have the renderer embed its primary window.
        /// </summary>
        /// <param name="embedWindow">The OSWindow to embed into.</param>
        public WindowInfo(OSWindow embedWindow, String name)
        {
            AutoCreateWindow = false;
            AutoWindowTitle = name;
            EmbedWindow = embedWindow;
            MonitorIndex = 0;
        }

        /// <summary>
        /// Fire the WindowCreated event. This should only be called by whatever
        /// class creates the window described by this class.
        /// </summary>
        public void _fireWindowCreated(WindowInfoEventArgs eventArgs)
        {
            if (WindowCreated != null)
            {
                WindowCreated.Invoke(this, eventArgs);
            }
        }

        /// <summary>
        /// If AutoCreateWindow is set to false set this to provide the OSWindow
        /// to embed the primary window in.
        /// </summary>
        public OSWindow EmbedWindow { get; private set; }

        /// <summary>
        /// Returns true if the renderer plugin should create the window automatically.
        /// </summary>
        public bool AutoCreateWindow { get; private set; }

        /// <summary>
        /// Returns the name to use for the created window if AutoCreateWindow is true.
        /// </summary>
        public String AutoWindowTitle { get; private set; }

        /// <summary>
        /// The index of the monitor to start ogre on.
        /// </summary>
        public int MonitorIndex { get; set; }

        /// <summary>
        /// The content scaling factor for a window, this should be set to what the os reports the scaling factor as
        /// and not the adjusted scaling size that might be applied on top of this. This only does something on osx right
        /// now, and should basically be 1.0 or 2.0 (for retina displays).
        /// </summary>
        public float ContentScalingFactor
        {
            get
            {
                if(EmbedWindow != null)
                {
                    return EmbedWindow.WindowScaling;
                }
                return 1.0f;
            }
        }

        /// <summary>
        /// Get/Set the width of the window. If an EmbedWindow is provided this
        /// can still be set and the value set here will be returned instead of
        /// the EmbedWindow's width.
        /// </summary>
        public int Width
        {
            get
            {
                if (EmbedWindow != null && width == -1)
                {
                    return EmbedWindow.WindowWidth;
                }
                else
                {
                    return width;
                }
            }
            set
            {
                width = value;
            }
        }

        /// <summary>
        /// Get/Set the height of the window. If an EmbedWindow is provided this
        /// can still be set and the value set here will be returned instead of
        /// the EmbedWindow's height.
        /// </summary>
        public int Height
        {
            get
            {
                if (EmbedWindow != null && height == -1)
                {
                    return EmbedWindow.WindowHeight;
                }
                else
                {
                    return height;
                }
            }
            set
            {
                height = value;
            }
        }

        /// <summary>
        /// Set to true to make the window fullscreen.
        /// </summary>
        public bool Fullscreen
        {
            get
            {
                return fullscreen;
            }
            set
            {
                fullscreen = value;
            }
        }
    }
}
