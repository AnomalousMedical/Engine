using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine.Renderer;
using Engine.Platform;
using OgreWrapper;
using EngineMath;

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

        public void createCamera(Vector3 translation, Vector3 lookAt)
        {
            
        }

        public abstract void Dispose();

        public abstract RenderWindow OgreRenderWindow
        {
             get;
        }
    }
}
