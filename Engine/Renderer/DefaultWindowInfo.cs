using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Platform;

namespace Engine.Renderer
{
    /// <summary>
    /// This class controls how the default window is created for a RendererPlugin.
    /// </summary>
    public class DefaultWindowInfo
    {
        private int width = -1;
        private int height = -1;
        private bool fullscreen = false;

        /// <summary>
        /// Constructor to have the renderer auto-create its window with the given title.
        /// </summary>
        /// <param name="windowTitle">The title to give to the renderer's auto-created window.</param>
        public DefaultWindowInfo(String windowTitle, int width, int height)
        {
            AutoCreateWindow = true;
            AutoWindowTitle = windowTitle;
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Constructor to have the renderer embed its primary window.
        /// </summary>
        /// <param name="embedWindow">The OSWindow to embed into.</param>
        public DefaultWindowInfo(OSWindow embedWindow)
        {
            AutoCreateWindow = false;
            AutoWindowTitle = "";
            EmbedWindow = embedWindow;
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
                    return EmbedWindow.Width;
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
                    return EmbedWindow.Height;
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
