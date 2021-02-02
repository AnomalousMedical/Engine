using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DilligentEngine
{
    public class GraphicsPipelineStateCreateInfo : IDisposable
    {
        private IntPtr ptr;

        public IntPtr ObjPtr => ptr;

        public GraphicsPipelineStateCreateInfo()
        {
            this.ptr = GraphicsPipelineStateCreateInfo_Create();
        }

        public void Dispose()
        {
            GraphicsPipelineStateCreateInfo_Delete(this.ptr);
        }

        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GraphicsPipelineStateCreateInfo_Create();

        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void GraphicsPipelineStateCreateInfo_Delete(IntPtr obj);
    }
}
