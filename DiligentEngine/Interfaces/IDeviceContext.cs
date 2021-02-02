using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Engine;

using Uint8 = System.Byte;
using Int8 = System.SByte;
using Bool = System.Boolean;
using Uint32 = System.UInt32;
using Uint64 = System.UInt64;

namespace DiligentEngine
{
    public partial class IDeviceContext :  IObject
    {
        public IDeviceContext(IntPtr objPtr)
            : base(objPtr)
        {

        }
        public void ClearDepthStencil(ITextureView pView, CLEAR_DEPTH_STENCIL_FLAGS ClearFlags, float fDepth, Uint8 Stencil, RESOURCE_STATE_TRANSITION_MODE StateTransitionMode)
        {
            IDeviceContext_ClearDepthStencil(this.objPtr, pView.objPtr, ClearFlags, fDepth, Stencil, StateTransitionMode);
        }
        public void ClearRenderTarget(ITextureView pView, Color RGBA, RESOURCE_STATE_TRANSITION_MODE StateTransitionMode)
        {
            IDeviceContext_ClearRenderTarget(this.objPtr, pView.objPtr, RGBA, StateTransitionMode);
        }
        public void Flush()
        {
            IDeviceContext_Flush(this.objPtr);
        }


        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void IDeviceContext_ClearDepthStencil(IntPtr objPtr, IntPtr pView, CLEAR_DEPTH_STENCIL_FLAGS ClearFlags, float fDepth, Uint8 Stencil, RESOURCE_STATE_TRANSITION_MODE StateTransitionMode);
        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void IDeviceContext_ClearRenderTarget(IntPtr objPtr, IntPtr pView, Color RGBA, RESOURCE_STATE_TRANSITION_MODE StateTransitionMode);
        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void IDeviceContext_Flush(IntPtr objPtr);
    }
}
