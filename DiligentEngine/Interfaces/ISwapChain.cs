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
    /// The swap chain is created by a platform-dependent function
    /// </summary>
    public partial class ISwapChain :  IObject
    {
        public ISwapChain(IntPtr objPtr)
            : base(objPtr)
        {

        }
        /// <summary>
        /// Presents a rendered image to the user
        /// </summary>
        public void Present(Uint32 SyncInterval)
        {
            ISwapChain_Present(
                this.objPtr
                , SyncInterval
            );
        }
        /// <summary>
        /// Changes the swap chain size
        /// \param [in] NewWidth     - New logical swap chain width (not accounting for pre-transform), in pixels.
        /// \param [in] NewHeight    - New logical swap chain height (not accounting for pre-transform), in pixels.
        /// \param [in] NewTransform - New surface transform, see Diligent::SURFACE_TRANSFORM.
        /// 
        /// \note When resizing non-primary swap chains, the engine unbinds the
        /// swap chain buffers from the output.
        /// 
        /// New width and height should not account for surface pre-transform. For example,
        /// if the window size is 1920 x 1080, but the surface is pre-rotated by 90 degrees,
        /// NewWidth should still be 1920, and NewHeight should still be 1080. It is highly
        /// recommended to always use SURFACE_TRANSFORM_OPTIMAL to let the engine select
        /// the most optimal pre-transform. However SURFACE_TRANSFORM_ROTATE_90 will also work in
        /// the scenario above. After the swap chain has been resized, its actual width will be 1080,
        /// actual height will be 1920, and PreTransform will be SURFACE_TRANSFORM_ROTATE_90.
        /// </summary>
        public void Resize(Uint32 NewWidth, Uint32 NewHeight, SURFACE_TRANSFORM NewTransform)
        {
            ISwapChain_Resize(
                this.objPtr
                , NewWidth
                , NewHeight
                , NewTransform
            );
        }
        /// <summary>
        /// Returns render target view of the current back buffer in the swap chain
        /// \note For Direct3D12 and Vulkan backends, the function returns
        /// different pointer for every offscreen buffer in the swap chain
        /// (flipped by every call to ISwapChain::Present()). For Direct3D11
        /// backend it always returns the same pointer. For OpenGL/GLES backends
        /// the method returns null.
        /// 
        /// The method does *NOT* call AddRef() on the returned interface,
        /// so Release() must not be called.
        /// </summary>
        public ITextureView GetCurrentBackBufferRTV()
        {
            var theReturnValue = 
            ISwapChain_GetCurrentBackBufferRTV(
                this.objPtr
            );
            return theReturnValue != IntPtr.Zero ? new ITextureView(theReturnValue) : null;
        }
        /// <summary>
        /// Returns depth-stencil view of the depth buffer
        /// The method does *NOT* call AddRef() on the returned interface,
        /// so Release() must not be called.
        /// </summary>
        public ITextureView GetDepthBufferDSV()
        {
            var theReturnValue = 
            ISwapChain_GetDepthBufferDSV(
                this.objPtr
            );
            return theReturnValue != IntPtr.Zero ? new ITextureView(theReturnValue) : null;
        }


        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ISwapChain_Present(
            IntPtr objPtr
            , Uint32 SyncInterval
        );
        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ISwapChain_Resize(
            IntPtr objPtr
            , Uint32 NewWidth
            , Uint32 NewHeight
            , SURFACE_TRANSFORM NewTransform
        );
        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ISwapChain_GetCurrentBackBufferRTV(
            IntPtr objPtr
        );
        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ISwapChain_GetDepthBufferDSV(
            IntPtr objPtr
        );
    }
}
