using Engine;
using System;
using System.Runtime.InteropServices;

namespace DiligentEngine
{
    public class GenericEngineFactory : IDisposable
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

        public void CreateDeviceAndSwapChain(IntPtr hwnd)
        {
            var result = GenericEngineFactory_CreateDeviceAndSwapChain(hwnd);
            this.RenderDevice = new IRenderDevice(result.m_pDevice);
            this.ImmediateContext = new IDeviceContext(result.m_pImmediateContext);
            this.SwapChain = new ISwapChain(result.m_pSwapChain);
        }

        public IRenderDevice RenderDevice { get; private set; }

        public IDeviceContext ImmediateContext { get; private set; }

        public ISwapChain SwapChain { get; private set; }

        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern CreateDeviceAndSwapChainResult GenericEngineFactory_CreateDeviceAndSwapChain(IntPtr hWnd);
    }
}
