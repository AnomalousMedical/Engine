using Engine;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DilligentEngine
{
    public enum RESOURCE_STATE_TRANSITION_MODE //: UInt8 
    {
        /// Perform no state transitions and no state validation. 
        /// Resource states are not accessed (either read or written) by the command.
        RESOURCE_STATE_TRANSITION_MODE_NONE = 0,

        /// Transition resources to the states required by the specific command.
        /// Resources in unknown state are ignored.
        ///
        /// \note    Any method that uses this mode may alter the state of the resources it works with.
        ///          As automatic state management is not thread-safe, no other thread is allowed to read
        ///          or write the state of the resources being transitioned. 
        ///          If the application intends to use the same resources in other threads simultaneously, it needs to 
        ///          explicitly manage the states using IDeviceContext::TransitionResourceStates() method.
        ///          Refer to http://diligentgraphics.com/2018/12/09/resource-state-management/ for detailed explanation
        ///          of resource state management in Diligent Engine.
        RESOURCE_STATE_TRANSITION_MODE_TRANSITION,

        /// Do not transition, but verify that states are correct.
        /// No validation is performed if the state is unknown to the engine.
        /// This mode only has effect in debug and development builds. No validation 
        /// is performed in release build.
        ///
        /// \note    Any method that uses this mode will read the state of resources it works with.
        ///          As automatic state management is not thread-safe, no other thread is allowed to alter
        ///          the state of resources being used by the command. It is safe to read these states.
        RESOURCE_STATE_TRANSITION_MODE_VERIFY
    };

    public enum CLEAR_DEPTH_STENCIL_FLAGS : UInt32
    {
        /// Perform no clear.
        CLEAR_DEPTH_FLAG_NONE = 0x00,

        /// Clear depth part of the buffer.
        CLEAR_DEPTH_FLAG = 0x01,

        /// Clear stencil part of the buffer.
        CLEAR_STENCIL_FLAG = 0x02
    };

    public class IDeviceContext : DilligentObject
    {
        public IDeviceContext(IntPtr objPtr) : base(objPtr)
        {
        }

        public void Flush()
        {
            IDeviceContext_Flush(objPtr);
        }

        public void SetRenderTarget(ITextureView renderTarget, ITextureView depthStencil, RESOURCE_STATE_TRANSITION_MODE StateTransitionMode)
        {
            IDeviceContext_SetRenderTarget(objPtr,
                renderTarget.objPtr,
                depthStencil.objPtr,
                StateTransitionMode);
        }

        public void ClearRenderTarget(ITextureView view, Color RGBA, RESOURCE_STATE_TRANSITION_MODE StateTransitionMode)
        {
            IDeviceContext_ClearRenderTarget(objPtr,
                view.objPtr,
                RGBA,
                StateTransitionMode);
        }

        public void SetPipelineState(IPipelineState pipelineState)
        {
            IDeviceContext_SetPipelineState(this.objPtr, pipelineState.objPtr);
        }

        public void Draw()
        {
            IDeviceContext_CheaterDraw(this.objPtr);
        }

        public void ClearDepthStencil(
            ITextureView pView,
            CLEAR_DEPTH_STENCIL_FLAGS ClearFlags,
            float fDepth,
            Byte Stencil,
            RESOURCE_STATE_TRANSITION_MODE StateTransitionMode)
        {
            IDeviceContext_ClearDepthStencil(objPtr,
                pView.objPtr,
                ClearFlags,
                fDepth,
                Stencil,
                StateTransitionMode);
        }

        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void IDeviceContext_Flush(IntPtr objPtr);


        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void IDeviceContext_SetRenderTarget(IntPtr objPtr,
            IntPtr ppRenderTarget,
            IntPtr pDepthStencil,
            RESOURCE_STATE_TRANSITION_MODE StateTransitionMode);

        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void IDeviceContext_ClearRenderTarget(IntPtr objPtr,
            IntPtr pView,
            Color RGBA,
            RESOURCE_STATE_TRANSITION_MODE StateTransitionMode);

        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void IDeviceContext_ClearDepthStencil(IntPtr objPtr,
            IntPtr pView,
            CLEAR_DEPTH_STENCIL_FLAGS ClearFlags,
            float fDepth,
            Byte Stencil,
            RESOURCE_STATE_TRANSITION_MODE StateTransitionMode);

        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void IDeviceContext_SetPipelineState(IntPtr objPtr, IntPtr pPipelineState);

        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void IDeviceContext_CheaterDraw(IntPtr objPtr);
    }
}
