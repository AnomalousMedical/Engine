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
    public partial class ISwapChain :  IObject
    {
        public ISwapChain(IntPtr objPtr)
            : base(objPtr)
        {

        }
        public void Present(Uint32 SyncInterval)
        {
            ISwapChain_Present(
                this.objPtr
                , SyncInterval
            );
        }
        public void Resize(Uint32 NewWidth, Uint32 NewHeight, SURFACE_TRANSFORM NewTransform)
        {
            ISwapChain_Resize(
                this.objPtr
                , NewWidth
                , NewHeight
                , NewTransform
            );
        }
        public ITextureView GetCurrentBackBufferRTV()
        {
            var theReturnValue = 
            ISwapChain_GetCurrentBackBufferRTV(
                this.objPtr
            );
            return new ITextureView(theReturnValue);
        }
        public ITextureView GetDepthBufferDSV()
        {
            var theReturnValue = 
            ISwapChain_GetDepthBufferDSV(
                this.objPtr
            );
            return new ITextureView(theReturnValue);
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
