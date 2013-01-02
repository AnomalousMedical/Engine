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

        public void ConfigureRenderSystem(int windowWidth, int windowHeight)
        {
            RenderInterfaceOgre3D_ConfigureRenderSystem(renderInterface, ref windowWidth, ref windowHeight);
        }

        public float PixelsPerInch
        {
            get
            {
                return RenderInterfaceOgre3D_GetPixelsPerInch(renderInterface);
            }
            set
            {
                RenderInterfaceOgre3D_SetPixelsPerInch(renderInterface, value);
            }
        }

        #region PInvoke

        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr RenderInterfaceOgre3D_Create(int width, int height);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void RenderInterfaceOgre3D_Delete(IntPtr renderInterface);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void RenderInterfaceOgre3D_ConfigureRenderSystem(IntPtr renderInterface, ref int renderWidth, ref int renderHeight);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern float RenderInterfaceOgre3D_GetPixelsPerInch(IntPtr renderInterface);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void RenderInterfaceOgre3D_SetPixelsPerInch(IntPtr renderInterface, float ppi);

        #endregion
    }
}
