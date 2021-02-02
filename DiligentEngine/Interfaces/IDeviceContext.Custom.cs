using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DiligentEngine
{
    partial class IDeviceContext
    {

        public void SetRenderTarget(ITextureView renderTarget, ITextureView depthStencil, RESOURCE_STATE_TRANSITION_MODE StateTransitionMode)
        {
            IDeviceContext_SetRenderTarget(objPtr,
                renderTarget.objPtr,
                depthStencil.objPtr,
                StateTransitionMode);
        }


        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void IDeviceContext_SetRenderTarget(IntPtr objPtr,
            IntPtr ppRenderTarget,
            IntPtr pDepthStencil,
            RESOURCE_STATE_TRANSITION_MODE StateTransitionMode);
    }
}
