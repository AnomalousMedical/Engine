using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;
using Engine;

using Uint8 = System.Byte;
using Int8 = System.SByte;
using Bool = System.Boolean;
using Uint32 = System.UInt32;
using Uint64 = System.UInt64;
using Float32 = System.Single;
using Uint16 = System.UInt16;
using PVoid = System.IntPtr;

namespace DiligentEngine
{
    public partial class IDeviceContext :  IObject
    {
        public IDeviceContext(IntPtr objPtr)
            : base(objPtr)
        {

        }
        public void SetPipelineState(IPipelineState pPipelineState)
        {
            IDeviceContext_SetPipelineState(
                this.objPtr
                , pPipelineState.objPtr
            );
        }
        public void CommitShaderResources(IShaderResourceBinding pShaderResourceBinding, RESOURCE_STATE_TRANSITION_MODE StateTransitionMode)
        {
            IDeviceContext_CommitShaderResources(
                this.objPtr
                , pShaderResourceBinding.objPtr
                , StateTransitionMode
            );
        }
        public void SetVertexBuffers(Uint32 StartSlot, Uint32 NumBuffersSet, IBuffer[] ppBuffers, Uint32[] pOffsets, RESOURCE_STATE_TRANSITION_MODE StateTransitionMode, SET_VERTEX_BUFFERS_FLAGS Flags)
        {
            IDeviceContext_SetVertexBuffers(
                this.objPtr
                , StartSlot
                , NumBuffersSet
                , ppBuffers.Select(i => i.objPtr).ToArray() //Not 100% sure on this. When is is gc'd
                , pOffsets
                , StateTransitionMode
                , Flags
            );
        }
        public void SetIndexBuffer(IBuffer pIndexBuffer, Uint32 ByteOffset, RESOURCE_STATE_TRANSITION_MODE StateTransitionMode)
        {
            IDeviceContext_SetIndexBuffer(
                this.objPtr
                , pIndexBuffer.objPtr
                , ByteOffset
                , StateTransitionMode
            );
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
        public void DrawIndexed(DrawIndexedAttribs Attribs)
        {
            IDeviceContext_DrawIndexed(
                this.objPtr
                , Attribs.NumIndices
                , Attribs.IndexType
                , Attribs.Flags
                , Attribs.NumInstances
                , Attribs.FirstIndexLocation
                , Attribs.BaseVertex
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
        public PVoid MapBuffer(IBuffer pBuffer, MAP_TYPE MapType, MAP_FLAGS MapFlags)
        {
            return
            IDeviceContext_MapBuffer(
                this.objPtr
                , pBuffer.objPtr
                , MapType
                , MapFlags
            );
        }
        public void UnmapBuffer(IBuffer pBuffer, MAP_TYPE MapType)
        {
            IDeviceContext_UnmapBuffer(
                this.objPtr
                , pBuffer.objPtr
                , MapType
            );
        }


        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void IDeviceContext_SetPipelineState(
            IntPtr objPtr
            , IntPtr pPipelineState
        );
        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void IDeviceContext_CommitShaderResources(
            IntPtr objPtr
            , IntPtr pShaderResourceBinding
            , RESOURCE_STATE_TRANSITION_MODE StateTransitionMode
        );
        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void IDeviceContext_SetVertexBuffers(
            IntPtr objPtr
            , Uint32 StartSlot
            , Uint32 NumBuffersSet
            , IntPtr[] ppBuffers
            , Uint32[] pOffsets
            , RESOURCE_STATE_TRANSITION_MODE StateTransitionMode
            , SET_VERTEX_BUFFERS_FLAGS Flags
        );
        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void IDeviceContext_SetIndexBuffer(
            IntPtr objPtr
            , IntPtr pIndexBuffer
            , Uint32 ByteOffset
            , RESOURCE_STATE_TRANSITION_MODE StateTransitionMode
        );
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
        private static extern void IDeviceContext_DrawIndexed(
            IntPtr objPtr
            , Uint32 Attribs_NumIndices
            , VALUE_TYPE Attribs_IndexType
            , DRAW_FLAGS Attribs_Flags
            , Uint32 Attribs_NumInstances
            , Uint32 Attribs_FirstIndexLocation
            , Uint32 Attribs_BaseVertex
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
        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern PVoid IDeviceContext_MapBuffer(
            IntPtr objPtr
            , IntPtr pBuffer
            , MAP_TYPE MapType
            , MAP_FLAGS MapFlags
        );
        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void IDeviceContext_UnmapBuffer(
            IntPtr objPtr
            , IntPtr pBuffer
            , MAP_TYPE MapType
        );
    }
}
