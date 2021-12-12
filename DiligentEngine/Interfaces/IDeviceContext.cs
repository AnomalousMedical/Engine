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
using float4 = Engine.Vector4;
using float3 = Engine.Vector3;
using float2 = Engine.Vector2;
using float4x4 = Engine.Matrix4x4;
using BOOL = System.Boolean;

namespace DiligentEngine
{
    /// <summary>
    /// \remarks Device context keeps strong references to all objects currently bound to
    /// the pipeline: buffers, states, samplers, shaders, etc.
    /// The context also keeps strong reference to the device and
    /// the swap chain.
    /// </summary>
    public partial class IDeviceContext :  IObject
    {
        public IDeviceContext(IntPtr objPtr)
            : base(objPtr)
        {
            this._ConstructorCalled();
        }
        partial void _ConstructorCalled();
        /// <summary>
        /// Sets the pipeline state.
        /// \param [in] pPipelineState - Pointer to IPipelineState interface to bind to the context.
        /// </summary>
        public void SetPipelineState(IPipelineState pPipelineState)
        {
            IDeviceContext_SetPipelineState(
                this.objPtr
                , pPipelineState.objPtr
            );
        }
        /// <summary>
        /// Commits shader resources to the device context.
        /// \param [in] pShaderResourceBinding - Shader resource binding whose resources will be committed.
        /// If pipeline state contains no shader resources, this parameter
        /// can be null.
        /// \param [in] StateTransitionMode    - State transition mode (see Diligent::RESOURCE_STATE_TRANSITION_MODE).
        /// 
        /// \remarks Pipeline state object that was used to create the shader resource binding must be bound
        /// to the pipeline when CommitShaderResources() is called. If no pipeline state object is bound
        /// or the pipeline state object does not match the shader resource binding, the method will fail.\n
        /// If Diligent::RESOURCE_STATE_TRANSITION_MODE_TRANSITION mode is used,
        /// the engine will also transition all shader resources to required states. If the flag
        /// is not set, it is assumed that all resources are already in correct states.\n
        /// Resources can be explicitly transitioned to required states by calling
        /// IDeviceContext::TransitionShaderResources() or IDeviceContext::TransitionResourceStates().\n
        /// 
        /// \remarks Automatic resource state transitioning is not thread-safe.
        /// 
        /// - If Diligent::RESOURCE_STATE_TRANSITION_MODE_TRANSITION mode is used, the method may alter the states
        /// of resources referenced by the shader resource binding and no other thread is allowed to read or write these states.
        /// 
        /// - If Diligent::RESOURCE_STATE_TRANSITION_MODE_VERIFY mode is used, the method will read the states, so no other thread
        /// should alter the states by calling any of the methods that use Diligent::RESOURCE_STATE_TRANSITION_MODE_TRANSITION mode.
        /// It is safe for other threads to read the states.
        /// 
        /// - If Diligent::RESOURCE_STATE_TRANSITION_MODE_NONE mode is used, the method does not access the states of resources.
        /// 
        /// If the application intends to use the same resources in other threads simultaneously, it should manage the states
        /// manually by setting the state to Diligent::RESOURCE_STATE_UNKNOWN (which will disable automatic state
        /// management) using IBuffer::SetState() or ITexture::SetState() and explicitly transitioning the states with
        /// IDeviceContext::TransitionResourceStates().
        /// Refer to http://diligentgraphics.com/2018/12/09/resource-state-management/ for detailed explanation
        /// of resource state management in Diligent Engine.
        /// 
        /// If an application calls any method that changes the state of any resource after it has been committed, the
        /// application is responsible for transitioning the resource back to correct state using one of the available methods
        /// before issuing the next draw or dispatch command.
        /// </summary>
        public void CommitShaderResources(IShaderResourceBinding pShaderResourceBinding, RESOURCE_STATE_TRANSITION_MODE StateTransitionMode)
        {
            IDeviceContext_CommitShaderResources(
                this.objPtr
                , pShaderResourceBinding.objPtr
                , StateTransitionMode
            );
        }
        /// <summary>
        /// Binds vertex buffers to the pipeline.
        /// \param [in] StartSlot           - The first input slot for binding. The first vertex buffer is
        /// explicitly bound to the start slot each additional vertex buffer
        /// in the array is implicitly bound to each subsequent input slot.
        /// \param [in] NumBuffersSet       - The number of vertex buffers in the array.
        /// \param [in] ppBuffers           - A pointer to an array of vertex buffers.
        /// The buffers must have been created with the Diligent::BIND_VERTEX_BUFFER flag.
        /// \param [in] pOffsets            - Pointer to an array of offset values one offset value for each buffer
        /// in the vertex-buffer array. Each offset is the number of bytes between
        /// the first element of a vertex buffer and the first element that will be
        /// used. If this parameter is nullptr, zero offsets for all buffers will be used.
        /// \param [in] StateTransitionMode - State transition mode for buffers being set (see Diligent::RESOURCE_STATE_TRANSITION_MODE).
        /// \param [in] Flags               - Additional flags. See Diligent::SET_VERTEX_BUFFERS_FLAGS for a list of allowed values.
        /// 
        /// \remarks The device context keeps strong references to all bound vertex buffers.
        /// Thus a buffer cannot be released until it is unbound from the context.\n
        /// It is suggested to specify Diligent::SET_VERTEX_BUFFERS_FLAG_RESET flag
        /// whenever possible. This will assure that no buffers from previous draw calls
        /// are bound to the pipeline.
        /// 
        /// \remarks When StateTransitionMode is Diligent::RESOURCE_STATE_TRANSITION_MODE_TRANSITION, the method will
        /// transition all buffers in known states to Diligent::RESOURCE_STATE_VERTEX_BUFFER. Resource state
        /// transitioning is not thread safe, so no other thread is allowed to read or write the states of
        /// these buffers.
        /// 
        /// If the application intends to use the same resources in other threads simultaneously, it needs to
        /// explicitly manage the states using IDeviceContext::TransitionResourceStates() method.
        /// Refer to http://diligentgraphics.com/2018/12/09/resource-state-management/ for detailed explanation
        /// of resource state management in Diligent Engine.
        /// </summary>
        public void SetVertexBuffers(Uint32 StartSlot, Uint32 NumBuffersSet, IBuffer[] ppBuffers, Uint32[] pOffsets, RESOURCE_STATE_TRANSITION_MODE StateTransitionMode, SET_VERTEX_BUFFERS_FLAGS Flags)
        {
            IDeviceContext_SetVertexBuffers(
                this.objPtr
                , StartSlot
                , NumBuffersSet
                , ppBuffers.Select(i => i.objPtr).ToArray()
                , pOffsets
                , StateTransitionMode
                , Flags
            );
        }
        /// <summary>
        /// Binds an index buffer to the pipeline.
        /// \param [in] pIndexBuffer        - Pointer to the index buffer. The buffer must have been created
        /// with the Diligent::BIND_INDEX_BUFFER flag.
        /// \param [in] ByteOffset          - Offset from the beginning of the buffer to
        /// the start of index data.
        /// \param [in] StateTransitionMode - State transiton mode for the index buffer to bind (see Diligent::RESOURCE_STATE_TRANSITION_MODE).
        /// 
        /// \remarks The device context keeps strong reference to the index buffer.
        /// Thus an index buffer object cannot be released until it is unbound
        /// from the context.
        /// 
        /// \remarks When StateTransitionMode is Diligent::RESOURCE_STATE_TRANSITION_MODE_TRANSITION, the method will
        /// transition the buffer to Diligent::RESOURCE_STATE_INDEX_BUFFER (if its state is not unknown). Resource
        /// state transitioning is not thread safe, so no other thread is allowed to read or write the state of
        /// the buffer.
        /// 
        /// If the application intends to use the same resource in other threads simultaneously, it needs to
        /// explicitly manage the states using IDeviceContext::TransitionResourceStates() method.
        /// Refer to http://diligentgraphics.com/2018/12/09/resource-state-management/ for detailed explanation
        /// of resource state management in Diligent Engine.
        /// </summary>
        public void SetIndexBuffer(IBuffer pIndexBuffer, Uint32 ByteOffset, RESOURCE_STATE_TRANSITION_MODE StateTransitionMode)
        {
            IDeviceContext_SetIndexBuffer(
                this.objPtr
                , pIndexBuffer.objPtr
                , ByteOffset
                , StateTransitionMode
            );
        }
        /// <summary>
        /// Executes a draw command.
        /// \param [in] Attribs - Draw command attributes, see Diligent::DrawAttribs for details.
        /// 
        /// \remarks  If Diligent::DRAW_FLAG_VERIFY_STATES flag is set, the method reads the state of vertex
        /// buffers, so no other threads are allowed to alter the states of the same resources.
        /// It is OK to read these states.
        /// 
        /// If the application intends to use the same resources in other threads simultaneously, it needs to
        /// explicitly manage the states using IDeviceContext::TransitionResourceStates() method.
        /// </summary>
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
        /// <summary>
        /// Executes an indexed draw command.
        /// \param [in] Attribs - Draw command attributes, see Diligent::DrawIndexedAttribs for details.
        /// 
        /// \remarks  If Diligent::DRAW_FLAG_VERIFY_STATES flag is set, the method reads the state of vertex/index
        /// buffers, so no other threads are allowed to alter the states of the same resources.
        /// It is OK to read these states.
        /// 
        /// If the application intends to use the same resources in other threads simultaneously, it needs to
        /// explicitly manage the states using IDeviceContext::TransitionResourceStates() method.
        /// </summary>
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
        /// <summary>
        /// Clears a depth-stencil view.
        /// \param [in] pView               - Pointer to ITextureView interface to clear. The view type must be
        /// Diligent::TEXTURE_VIEW_DEPTH_STENCIL.
        /// \param [in] StateTransitionMode - state transition mode of the depth-stencil buffer to clear.
        /// \param [in] ClearFlags          - Idicates which parts of the buffer to clear, see Diligent::CLEAR_DEPTH_STENCIL_FLAGS.
        /// \param [in] fDepth              - Value to clear depth part of the view with.
        /// \param [in] Stencil             - Value to clear stencil part of the view with.
        /// 
        /// \remarks The full extent of the view is always cleared. Viewport and scissor settings are not applied.
        /// \note The depth-stencil view must be bound to the pipeline for clear operation to be performed.
        /// 
        /// \remarks When StateTransitionMode is Diligent::RESOURCE_STATE_TRANSITION_MODE_TRANSITION, the method will
        /// transition the state of the texture to the state required by clear operation.
        /// In Direct3D12, this satate is always Diligent::RESOURCE_STATE_DEPTH_WRITE, however in Vulkan
        /// the state depends on whether the depth buffer is bound to the pipeline.
        /// 
        /// Resource state transitioning is not thread safe, so no other thread is allowed to read or write
        /// the state of resources used by the command.
        /// Refer to http://diligentgraphics.com/2018/12/09/resource-state-management/ for detailed explanation
        /// of resource state management in Diligent Engine.
        /// </summary>
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
        /// <summary>
        /// Clears a render target view
        /// \param [in] pView               - Pointer to ITextureView interface to clear. The view type must be
        /// Diligent::TEXTURE_VIEW_RENDER_TARGET.
        /// \param [in] RGBA                - A 4-component array that represents the color to fill the render target with.
        /// If nullptr is provided, the default array {0,0,0,0} will be used.
        /// \param [in] StateTransitionMode - Defines required state transitions (see Diligent::RESOURCE_STATE_TRANSITION_MODE)
        /// 
        /// \remarks The full extent of the view is always cleared. Viewport and scissor settings are not applied.
        /// 
        /// The render target view must be bound to the pipeline for clear operation to be performed in OpenGL backend.
        /// 
        /// \remarks When StateTransitionMode is Diligent::RESOURCE_STATE_TRANSITION_MODE_TRANSITION, the method will
        /// transition the texture to the state required by the command. Resource state transitioning is not
        /// thread safe, so no other thread is allowed to read or write the states of the same textures.
        /// 
        /// If the application intends to use the same resource in other threads simultaneously, it needs to
        /// explicitly manage the states using IDeviceContext::TransitionResourceStates() method.
        /// 
        /// \note    In D3D12 backend clearing render targets requires textures to always be transitioned to
        /// Diligent::RESOURCE_STATE_RENDER_TARGET state. In Vulkan backend however this depends on whether a
        /// render pass has been started. To clear render target outside of a render pass, the texture must be transitioned to
        /// Diligent::RESOURCE_STATE_COPY_DEST state. Inside a render pass it must be in Diligent::RESOURCE_STATE_RENDER_TARGET
        /// state. When using Diligent::RESOURCE_STATE_TRANSITION_TRANSITION mode, the engine takes care of proper
        /// resource state transition, otherwise it is the responsibility of the application.
        /// </summary>
        public void ClearRenderTarget(ITextureView pView, Color RGBA, RESOURCE_STATE_TRANSITION_MODE StateTransitionMode)
        {
            IDeviceContext_ClearRenderTarget(
                this.objPtr
                , pView.objPtr
                , RGBA
                , StateTransitionMode
            );
        }
        /// <summary>
        /// Submits all pending commands in the context for execution to the command queue.
        /// \remarks    Only immediate contexts can be flushed.\n
        /// Internally the method resets the state of the current command list/buffer.
        /// When the next draw command is issued, the engine will restore all states
        /// (rebind render targets and depth-stencil buffer as well as index and vertex buffers,
        /// restore viewports and scissor rects, etc.) except for the pipeline state and shader resource
        /// bindings. An application must explicitly reset the PSO and bind all required shader
        /// resources after flushing the context.
        /// </summary>
        public void Flush()
        {
            IDeviceContext_Flush(
                this.objPtr
            );
        }
        /// <summary>
        /// Updates the data in the buffer.
        /// \param [in] pBuffer             - Pointer to the buffer to updates.
        /// \param [in] Offset              - Offset in bytes from the beginning of the buffer to the update region.
        /// \param [in] Size                - Size in bytes of the data region to update.
        /// \param [in] pData               - Pointer to the data to write to the buffer.
        /// \param [in] StateTransitionMode - Buffer state transition mode (see Diligent::RESOURCE_STATE_TRANSITION_MODE)
        /// </summary>
        public void UpdateBuffer(IBuffer pBuffer, Uint32 Offset, Uint32 Size, IntPtr pData, RESOURCE_STATE_TRANSITION_MODE StateTransitionMode)
        {
            IDeviceContext_UpdateBuffer(
                this.objPtr
                , pBuffer.objPtr
                , Offset
                , Size
                , pData
                , StateTransitionMode
            );
        }
        /// <summary>
        /// Maps the buffer.
        /// \param [in] pBuffer      - Pointer to the buffer to map.
        /// \param [in] MapType      - Type of the map operation. See Diligent::MAP_TYPE.
        /// \param [in] MapFlags     - Special map flags. See Diligent::MAP_FLAGS.
        /// \param [out] pMappedData - Reference to the void pointer to store the address of the mapped region.
        /// </summary>
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
        /// <summary>
        /// Unmaps the previously mapped buffer.
        /// \param [in] pBuffer - Pointer to the buffer to unmap.
        /// \param [in] MapType - Type of the map operation. This parameter must match the type that was
        /// provided to the Map() method.
        /// </summary>
        public void UnmapBuffer(IBuffer pBuffer, MAP_TYPE MapType)
        {
            IDeviceContext_UnmapBuffer(
                this.objPtr
                , pBuffer.objPtr
                , MapType
            );
        }
        /// <summary>
        /// Builds a bottom-level acceleration structure with the specified geometries.
        /// \param [in] Attribs - Structure describing build BLAS command attributes, see Diligent::BuildBLASAttribs for details.
        /// 
        /// \note Don't call build or copy operation on the same BLAS in a different contexts, because BLAS has CPU-side data
        /// that will not match with GPU-side, so shader binding were incorrect.
        /// </summary>
        public void BuildBLAS(BuildBLASAttribs Attribs)
        {
            IDeviceContext_BuildBLAS(
                this.objPtr
                , Attribs.pBLAS?.objPtr ?? IntPtr.Zero
                , Attribs.BLASTransitionMode
                , Attribs.GeometryTransitionMode
                , BLASBuildTriangleDataPassStruct.ToStruct(Attribs?.pTriangleData)
                , Attribs?.pTriangleData != null ? (Uint32)Attribs.pTriangleData.Count : 0
                , BLASBuildBoundingBoxDataPassStruct.ToStruct(Attribs?.pBoxData)
                , Attribs?.pBoxData != null ? (Uint32)Attribs.pBoxData.Count : 0
                , Attribs.pScratchBuffer?.objPtr ?? IntPtr.Zero
                , Attribs.ScratchBufferOffset
                , Attribs.ScratchBufferTransitionMode
                , Attribs.Update
            );
        }
        /// <summary>
        /// Builds a top-level acceleration structure with the specified instances.
        /// \param [in] Attribs - Structure describing build TLAS command attributes, see Diligent::BuildTLASAttribs for details.
        /// 
        /// \note Don't call build or copy operation on the same TLAS in a different contexts, because TLAS has CPU-side data
        /// that will not match with GPU-side, so shader binding were incorrect.
        /// </summary>
        public void BuildTLAS(BuildTLASAttribs Attribs)
        {
            IDeviceContext_BuildTLAS(
                this.objPtr
                , Attribs.pTLAS?.objPtr ?? IntPtr.Zero
                , Attribs.TLASTransitionMode
                , Attribs.BLASTransitionMode
                , TLASBuildInstanceDataPassStruct.ToStruct(Attribs?.pInstances)
                , Attribs?.pInstances != null ? (Uint32)Attribs.pInstances.Count : 0
                , Attribs.pInstanceBuffer?.objPtr ?? IntPtr.Zero
                , Attribs.InstanceBufferOffset
                , Attribs.InstanceBufferTransitionMode
                , Attribs.HitGroupStride
                , Attribs.BaseContributionToHitGroupIndex
                , Attribs.BindingMode
                , Attribs.pScratchBuffer?.objPtr ?? IntPtr.Zero
                , Attribs.ScratchBufferOffset
                , Attribs.ScratchBufferTransitionMode
                , Attribs.Update
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
        private static extern void IDeviceContext_UpdateBuffer(
            IntPtr objPtr
            , IntPtr pBuffer
            , Uint32 Offset
            , Uint32 Size
            , IntPtr pData
            , RESOURCE_STATE_TRANSITION_MODE StateTransitionMode
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
        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void IDeviceContext_BuildBLAS(
            IntPtr objPtr
            , IntPtr Attribs_pBLAS
            , RESOURCE_STATE_TRANSITION_MODE Attribs_BLASTransitionMode
            , RESOURCE_STATE_TRANSITION_MODE Attribs_GeometryTransitionMode
            , BLASBuildTriangleDataPassStruct[] Attribs_pTriangleData
            , Uint32 Attribs_TriangleDataCount
            , BLASBuildBoundingBoxDataPassStruct[] Attribs_pBoxData
            , Uint32 Attribs_BoxDataCount
            , IntPtr Attribs_pScratchBuffer
            , Uint32 Attribs_ScratchBufferOffset
            , RESOURCE_STATE_TRANSITION_MODE Attribs_ScratchBufferTransitionMode
            , [MarshalAs(UnmanagedType.I1)]Bool Attribs_Update
        );
        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void IDeviceContext_BuildTLAS(
            IntPtr objPtr
            , IntPtr Attribs_pTLAS
            , RESOURCE_STATE_TRANSITION_MODE Attribs_TLASTransitionMode
            , RESOURCE_STATE_TRANSITION_MODE Attribs_BLASTransitionMode
            , TLASBuildInstanceDataPassStruct[] Attribs_pInstances
            , Uint32 Attribs_InstanceCount
            , IntPtr Attribs_pInstanceBuffer
            , Uint32 Attribs_InstanceBufferOffset
            , RESOURCE_STATE_TRANSITION_MODE Attribs_InstanceBufferTransitionMode
            , Uint32 Attribs_HitGroupStride
            , Uint32 Attribs_BaseContributionToHitGroupIndex
            , HIT_GROUP_BINDING_MODE Attribs_BindingMode
            , IntPtr Attribs_pScratchBuffer
            , Uint32 Attribs_ScratchBufferOffset
            , RESOURCE_STATE_TRANSITION_MODE Attribs_ScratchBufferTransitionMode
            , [MarshalAs(UnmanagedType.I1)]Bool Attribs_Update
        );
    }
}
