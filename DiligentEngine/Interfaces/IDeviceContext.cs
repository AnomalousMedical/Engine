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
using Float32 = System.Single;
using Uint16 = System.UInt16;

namespace DiligentEngine
{
    public partial class IDeviceContext :  IObject
    {
        public IDeviceContext(IntPtr objPtr)
            : base(objPtr)
        {

        }
        public void Draw(DrawAttribs Attribs)
        {
            IDeviceContext_Draw(
                this.objPtr
                , Attribs.NumVertices
                , Attribs.Flags
                , Attribs.NumInstances
                , Attribs.StartVertexLocation
                , Attribs.FirstInstanceLocation
            );
        }
        public void ClearDepthStencil(ITextureView pView, CLEAR_DEPTH_STENCIL_FLAGS ClearFlags, float fDepth, Uint8 Stencil, RESOURCE_STATE_TRANSITION_MODE StateTransitionMode)
        {
            IDeviceContext_ClearDepthStencil(
                this.objPtr
                , pView.objPtr
                , ClearFlags
                , fDepth
                , Stencil
                , StateTransitionMode
            );
        }
        public void ClearRenderTarget(ITextureView pView, Color RGBA, RESOURCE_STATE_TRANSITION_MODE StateTransitionMode)
        {
            IDeviceContext_ClearRenderTarget(
                this.objPtr
                , pView.objPtr
                , RGBA
                , StateTransitionMode
            );
        }
        public void Flush()
        {
            IDeviceContext_Flush(
                this.objPtr
            );
        }


        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void IDeviceContext_Draw(
            IntPtr objPtr
            , Uint32 Attribs_NumVertices
            , DRAW_FLAGS Attribs_Flags
            , Uint32 Attribs_NumInstances
            , Uint32 Attribs_StartVertexLocation
            , Uint32 Attribs_FirstInstanceLocation
        );
        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void IDeviceContext_ClearDepthStencil(
            IntPtr objPtr
            , IntPtr pView
            , CLEAR_DEPTH_STENCIL_FLAGS ClearFlags
            , float fDepth
            , Uint8 Stencil
            , RESOURCE_STATE_TRANSITION_MODE StateTransitionMode
        );
        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void IDeviceContext_ClearRenderTarget(
            IntPtr objPtr
            , IntPtr pView
            , Color RGBA
            , RESOURCE_STATE_TRANSITION_MODE StateTransitionMode
        );
        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void IDeviceContext_Flush(
            IntPtr objPtr
        );
    }
}
