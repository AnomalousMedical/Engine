using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DilligentEngine
{
    public class ISwapChain : DilligentObject
    {
        public ISwapChain(IntPtr objPtr) : base(objPtr)
        {
        }

        public void Present()
        {
            ISwapChain_Present(objPtr);
        }

        public void Resize(UInt32 width, UInt32 height)
        {
            ISwapChain_Resize(objPtr, width, height);
        }

        public ITextureView GetCurrentBackBufferRTV()
        {
            //TODO: This is a lot of allocation
            return new ITextureView(ISwapChain_GetCurrentBackBufferRTV(objPtr));
        }

        public ITextureView GetDepthBufferDSV()
        {
            //TODO: This is a lot of allocation
            return new ITextureView(ISwapChain_GetDepthBufferDSV(objPtr));
        }

        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ISwapChain_Present(IntPtr objPtr);

        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void ISwapChain_Resize(IntPtr objPtr, UInt32 NewWidth, UInt32 NewHeight);

        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ISwapChain_GetCurrentBackBufferRTV(IntPtr objPtr);

        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr ISwapChain_GetDepthBufferDSV(IntPtr objPtr);
    }
}
