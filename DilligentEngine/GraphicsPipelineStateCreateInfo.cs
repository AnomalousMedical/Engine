using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DilligentEngine
{
    public class GraphicsPipelineStateCreateInfo : IDisposable
    {
        private IntPtr objPtr;

        public IntPtr ObjPtr => objPtr;

        public GraphicsPipelineStateCreateInfo()
        {
            this.objPtr = GraphicsPipelineStateCreateInfo_Create();
        }

        public void Dispose()
        {
            GraphicsPipelineStateCreateInfo_Delete(this.objPtr);
        }

        private IShader _pVS;
        public IShader pVS { get => _pVS; set { _pVS = value; GraphicsPipelineStateCreateInfo_Set_pVS(objPtr, value.objPtr); } }

        private IShader _pPS;
        public IShader pPS { get => _pPS; set { _pPS = value; GraphicsPipelineStateCreateInfo_Set_pPS(objPtr, value.objPtr); } }

        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GraphicsPipelineStateCreateInfo_Create();

        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void GraphicsPipelineStateCreateInfo_Delete(IntPtr obj);

        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void GraphicsPipelineStateCreateInfo_Set_pVS(IntPtr obj, IntPtr value);

        //[DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //private static extern IntPtr GraphicsPipelineStateCreateInfo_Get_pVS(IntPtr obj);

        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void GraphicsPipelineStateCreateInfo_Set_pPS(IntPtr obj, IntPtr value);

        //[DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //private static extern IntPtr GraphicsPipelineStateCreateInfo_Get_pPS(IntPtr obj);
    }
}
