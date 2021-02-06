using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

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
    partial class IDeviceContext
    {

        public void SetRenderTarget(ITextureView renderTarget, ITextureView depthStencil, RESOURCE_STATE_TRANSITION_MODE StateTransitionMode)
        {
            IDeviceContext_SetRenderTarget(objPtr,
                renderTarget.objPtr,
                depthStencil == null ? IntPtr.Zero : depthStencil.objPtr,
                StateTransitionMode);
        }
        /// <summary>
        /// Transitions resource states.
        /// \param [in] BarrierCount      - Number of barriers in pResourceBarriers array
        /// \param [in] pResourceBarriers - Pointer to the array of resource barriers
        /// \remarks When both old and new states are RESOURCE_STATE_UNORDERED_ACCESS, the engine
        /// executes UAV barrier on the resource. The barrier makes sure that all UAV accesses
        /// (reads or writes) are complete before any future UAV accesses (read or write) can begin.\n
        /// 
        /// There are two main usage scenarios for this method:
        /// 1. An application knows specifics of resource state transitions not available to the engine.
        /// For example, only single mip level needs to be transitioned.
        /// 2. An application manages resource states in multiple threads in parallel.
        /// 
        /// The method always reads the states of all resources to transition. If the state of a resource is managed
        /// by multiple threads in parallel, the resource must first be transitioned to unknown state
        /// (Diligent::RESOURCE_STATE_UNKNOWN) to disable automatic state management in the engine.
        /// 
        /// When StateTransitionDesc::UpdateResourceState is set to true, the method may update the state of the
        /// corresponding resource which is not thread safe. No other threads should read or write the sate of that
        /// resource.
        /// 
        /// \note    Any method that uses Diligent::RESOURCE_STATE_TRANSITION_MODE_TRANSITION mode may alter
        /// the state of resources it works with. Diligent::RESOURCE_STATE_TRANSITION_MODE_VERIFY mode
        /// makes the method read the states, but not write them. When Diligent::RESOURCE_STATE_TRANSITION_MODE_NONE
        /// is used, the method assumes the states are guaranteed to be correct and does not read or write them.
        /// It is the responsibility of the application to make sure this is indeed true.
        /// 
        /// Refer to http://diligentgraphics.com/2018/12/09/resource-state-management/ for detailed explanation
        /// of resource state management in Diligent Engine.
        /// </summary>
        public void TransitionResourceStates(List<StateTransitionDesc> pResourceBarriers)
        {
            IDeviceContext_TransitionResourceStates(
                this.objPtr
                , (Uint32)pResourceBarriers.Count
                , StateTransitionDescPassStruct.ToStruct(pResourceBarriers)
            );
        }


        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void IDeviceContext_SetRenderTarget(IntPtr objPtr,
            IntPtr ppRenderTarget,
            IntPtr pDepthStencil,
            RESOURCE_STATE_TRANSITION_MODE StateTransitionMode);

        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void IDeviceContext_TransitionResourceStates(
            IntPtr objPtr
            , Uint32 BarrierCount
            , StateTransitionDescPassStruct[] pResourceBarriers_pResource
        );
    }
}
