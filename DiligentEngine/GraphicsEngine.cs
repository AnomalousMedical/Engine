using Engine;
using System;
using System.Runtime.InteropServices;

using Uint8 = System.Byte;
using Int8 = System.SByte;
using Bool = System.Boolean;
using Uint32 = System.UInt32;
using Uint64 = System.UInt64;
using Float32 = System.Single;
using Uint16 = System.UInt16;

namespace DiligentEngine
{
    public class GraphicsEngine : IDisposable
    {
        struct CreateDeviceAndSwapChainResult
        {
            public IntPtr m_pDevice;
            public IntPtr m_pImmediateContext;
            public IntPtr m_pSwapChain;
        }

        public void Dispose()
        {
            this.ImmediateContext.Flush(); //The sample app flushes this out when it shuts down

            this.RenderDevice.Dispose();
            this.ImmediateContext.Dispose();
            this.SwapChain.Dispose();
        }

        public void CreateDeviceAndSwapChain(IntPtr hwnd, SwapChainDesc swapChainDesc)
        {
            var result = GenericEngineFactory_CreateDeviceAndSwapChain(
                hwnd
            , swapChainDesc.Width
            , swapChainDesc.Height
            , swapChainDesc.ColorBufferFormat
            , swapChainDesc.DepthBufferFormat
            , swapChainDesc.Usage
            , swapChainDesc.PreTransform
            , swapChainDesc.BufferCount
            , swapChainDesc.DefaultDepthValue
            , swapChainDesc.DefaultStencilValue
            , swapChainDesc.IsPrimary
            );
            this.RenderDevice = new IRenderDevice(result.m_pDevice);
            this.ImmediateContext = new IDeviceContext(result.m_pImmediateContext);
            this.SwapChain = new ISwapChain(result.m_pSwapChain);
            this.SwapChainDesc = swapChainDesc;
        }

        public IRenderDevice RenderDevice { get; private set; }

        public IDeviceContext ImmediateContext { get; private set; }

        public ISwapChain SwapChain { get; private set; }

        public SwapChainDesc SwapChainDesc { get; private set; }

        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern CreateDeviceAndSwapChainResult GenericEngineFactory_CreateDeviceAndSwapChain(
            IntPtr hWnd
            , Uint32 Width                       
            , Uint32 Height                      
            , TEXTURE_FORMAT ColorBufferFormat   
            , TEXTURE_FORMAT DepthBufferFormat   
            , SWAP_CHAIN_USAGE_FLAGS Usage       
            , SURFACE_TRANSFORM PreTransform     
            , Uint32 BufferCount                 
            , Float32 DefaultDepthValue          
            , Uint8 DefaultStencilValue          
            , [MarshalAs(UnmanagedType.I1)] bool IsPrimary                     
            );
    }
}
