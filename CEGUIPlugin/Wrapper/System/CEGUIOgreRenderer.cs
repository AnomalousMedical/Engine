using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using OgreWrapper;

namespace CEGUIPlugin
{
    class CEGUIOgreRenderer : CEGUIRenderer
    {
        private IntPtr ogreRenderer;
        private IntPtr ogreResourceProvider;
        private IntPtr ogreImageCodec;

        public CEGUIOgreRenderer(RenderTarget renderTarget)
        {
            ogreRenderer = OgreRenderer_create(renderTarget.OgreRenderTarget);
            ogreResourceProvider = OgreRenderer_createOgreResourceProvider();
            ogreImageCodec = OgreRenderer_createOgreImageCodec();
        }

        public override void Dispose()
        {
            OgreRenderer_destroyOgreImageCodec(ogreImageCodec);
            OgreRenderer_destroyOgreResourceProvider(ogreResourceProvider);
            OgreRenderer_destroy(ogreRenderer);
        }

        internal override IntPtr Renderer
        {
            get { return ogreRenderer; }
        }

        internal override IntPtr ResourceProvider
        {
            get { return ogreResourceProvider; }
        }

        internal override IntPtr ImageCodec
        {
            get { return ogreImageCodec; }
        }

#region PInvoke

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr OgreRenderer_create(IntPtr ogreRenderTarget);

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr OgreRenderer_createOgreResourceProvider();

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr OgreRenderer_createOgreImageCodec();

        [DllImport("CEGUIWrapper")]
        private static extern void OgreRenderer_destroy(IntPtr ogreRenderer);

        [DllImport("CEGUIWrapper")]
        private static extern void OgreRenderer_destroyOgreResourceProvider(IntPtr ogreResourceProvider);

        [DllImport("CEGUIWrapper")]
        private static extern void OgreRenderer_destroyOgreImageCodec(IntPtr ogreImageCodec);

#endregion
    }
}
