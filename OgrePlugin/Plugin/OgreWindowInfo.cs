using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Renderer;
using Engine.Platform;

namespace OgrePlugin
{
    class OgreWindowInfo : WindowInfo
    {
        private OSWindow primaryWindow;

        public OgreWindowInfo(OSWindow primaryWindow)
        {
            this.primaryWindow = primaryWindow;
        }

        /// <summary>
        /// The primary OSWindow used for rendering.
        /// </summary>
        public OSWindow PrimaryRenderWindow
        {
            get 
            { 
                return primaryWindow; 
            }
        }
    }
}
