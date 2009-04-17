using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Renderer;
using Engine.Platform;
using OgreWrapper;

namespace OgrePlugin
{
    abstract class OgreWindow : RendererWindow, IDisposable
    {
        private OSWindow handle;

        public OgreWindow(OSWindow handle)
        {
            this.handle = handle;
        }

        public OSWindow Handle
        {
            get
            {
                return handle;
            }
        }

        public abstract void Dispose();

        public abstract RenderWindow OgreRenderWindow
        {
             get;
        }
    }
}
