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
    public partial class ISwapChain
    {
        private ITextureView[] BackBufferRtvs;
        private int CurrentRtvEntry = 0;

        partial void _ConstructorCalled()
        {
            BackBufferRtvs = new ITextureView[this.GetDesc_BufferCount];
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

            //This just caches per slot, it does not care if we might have the same pointer in another slot.
            //This works really fast as long as we keep getting buffers in the same order. Otherwise it quickly equalizes again in BackBufferRtvs.Length frames.
            //This is an acceptable amount of garbage to avoid a loop and works most of the time.
            //Events like resizing the window or minimizing seem to make it miss.
            var currentBackBuffer = BackBufferRtvs[CurrentRtvEntry];

            if (currentBackBuffer == null || theReturnValue != currentBackBuffer.objPtr)
            {
                BackBufferRtvs[CurrentRtvEntry] = currentBackBuffer = theReturnValue == null ? null : new ITextureView(theReturnValue);
            }

            CurrentRtvEntry = (CurrentRtvEntry + 1) % BackBufferRtvs.Length;

            return currentBackBuffer;
        }

        public TEXTURE_FORMAT GetDesc_ColorBufferFormat => ISwapChain_GetDesc_ColorBufferFormat(objPtr);

        public TEXTURE_FORMAT GetDesc_DepthBufferFormat => ISwapChain_GetDesc_DepthBufferFormat(objPtr);

        public Uint32 GetDesc_BufferCount => ISwapChain_GetDesc_BufferCount(objPtr);

        public SURFACE_TRANSFORM GetDesc_PreTransform => ISwapChain_GetDesc_PreTransform(objPtr);


        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern TEXTURE_FORMAT ISwapChain_GetDesc_ColorBufferFormat(
            IntPtr objPtr
        );
        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern TEXTURE_FORMAT ISwapChain_GetDesc_DepthBufferFormat(
            IntPtr objPtr
        );
        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern Uint32 ISwapChain_GetDesc_BufferCount(
            IntPtr objPtr
        );
        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern SURFACE_TRANSFORM ISwapChain_GetDesc_PreTransform(
            IntPtr objPtr
        );
        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ISwapChain_GetCurrentBackBufferRTV(
            IntPtr objPtr
        );
    }
}
