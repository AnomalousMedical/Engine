using System;
using System.Runtime.InteropServices;

namespace DilligentEngine
{
    struct CreateDeviceAndSwapChainResult
    {
        IntPtr m_pDevice;
        IntPtr m_pImmediateContext;
        IntPtr m_pSwapChain;
    }

    public class GenericEngineFactory
    {
        public void CreateDeviceAndSwapChain(IntPtr hwnd)
        {
            var result = GenericEngineFactory_CreateDeviceAndSwapChain(hwnd);
            Console.WriteLine(result);
        }

        public IDeviceContext DeviceContext { get; set; }

        public IRenderDevice RenderDevice { get; set; }

        public ISwapChain SwapChain { get; set; }

        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern CreateDeviceAndSwapChainResult GenericEngineFactory_CreateDeviceAndSwapChain(IntPtr hWnd);
    }
}
