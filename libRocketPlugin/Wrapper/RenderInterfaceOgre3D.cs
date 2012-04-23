using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace libRocketPlugin
{
    public class RenderInterfaceOgre3D : RenderInterface
    {
        public RenderInterfaceOgre3D(int width, int height)
        {
            renderInterface = RenderInterfaceOgre3D_Create(width, height);
        }

        public override void Dispose()
        {
            RenderInterfaceOgre3D_Delete(renderInterface);
        }

        public void ConfigureRenderSystem()
        {
            RenderInterfaceOgre3D_ConfigureRenderSystem(renderInterface);
        }

        public void RenderWindowResized(uint windowWidth, uint windowHeight)
        {
            RenderInterfaceOgre3D_renderWindowResized(renderInterface, windowWidth, windowHeight);
        }

        #region PInvoke

        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr RenderInterfaceOgre3D_Create(int width, int height);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void RenderInterfaceOgre3D_Delete(IntPtr renderInterface);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void RenderInterfaceOgre3D_ConfigureRenderSystem(IntPtr renderInterface);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void RenderInterfaceOgre3D_renderWindowResized(IntPtr renderInterface, uint window_width, uint window_height);

        #endregion
    }
}
