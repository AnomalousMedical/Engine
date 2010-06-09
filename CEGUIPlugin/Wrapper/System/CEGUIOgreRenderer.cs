using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using OgreWrapper;

namespace CEGUIPlugin
{
    public class CEGUIOgreRenderer : CEGUIRenderer
    {
        private IntPtr ogreRenderer;
        private IntPtr ogreResourceProvider;
        private IntPtr ogreImageCodec;

        public CEGUIOgreRenderer(RenderTarget renderTarget)
        {
            ogreRenderer = CEGUIOgreRenderer_create(renderTarget.OgreRenderTarget);
            ogreResourceProvider = CEGUIOgreRenderer_createOgreResourceProvider();
            ogreImageCodec = CEGUIOgreRenderer_createOgreImageCodec();
        }

        public override void Dispose()
        {
            CEGUIOgreRenderer_destroyOgreImageCodec(ogreImageCodec);
            CEGUIOgreRenderer_destroyOgreResourceProvider(ogreResourceProvider);
            CEGUIOgreRenderer_destroy(ogreRenderer);
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
        private static extern IntPtr CEGUIOgreRenderer_create(IntPtr ogreRenderTarget);

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr CEGUIOgreRenderer_createOgreResourceProvider();

        [DllImport("CEGUIWrapper")]
        private static extern IntPtr CEGUIOgreRenderer_createOgreImageCodec();

        [DllImport("CEGUIWrapper")]
        private static extern void CEGUIOgreRenderer_destroy(IntPtr ogreRenderer);

        [DllImport("CEGUIWrapper")]
        private static extern void CEGUIOgreRenderer_destroyOgreResourceProvider(IntPtr ogreResourceProvider);

        [DllImport("CEGUIWrapper")]
        private static extern void CEGUIOgreRenderer_destroyOgreImageCodec(IntPtr ogreImageCodec);

#endregion
    }
}
