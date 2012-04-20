using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace libRocketPlugin
{
    class RenderInterfaceOgre3D : RenderInterface
    {
        public RenderInterfaceOgre3D(int width, int height)
        {
            renderInterface = RenderInterfaceOgre3D_Create(width, height);
        }

        public override void Dispose()
        {
            RenderInterfaceOgre3D_Delete(renderInterface);
        }

        #region PInvoke

        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr RenderInterfaceOgre3D_Create(int width, int height);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void RenderInterfaceOgre3D_Delete(IntPtr renderInterface);

        #endregion
    }
}
