using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace libRocketPlugin
{
    class RenderInterfaceOgre3D : IDisposable
    {
        private IntPtr renderInterface;

        public RenderInterfaceOgre3D(int width, int height)
        {
            renderInterface = RenderInterfaceOgre3D_Create(width, height);
        }

        public void Dispose()
        {
            RenderInterfaceOgre3D_Delete(renderInterface);
        }

        internal IntPtr RenderInterface
        {
            get
            {
                return renderInterface;
            }
        }

        #region PInvoke

        [DllImport("libRocketWrapper", CallingConvention=CallingConvention.Cdecl)]
        private static extern IntPtr RenderInterfaceOgre3D_Create(int width, int height);

        [DllImport("libRocketWrapper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void RenderInterfaceOgre3D_Delete(IntPtr renderInterface);

        #endregion
    }
}
