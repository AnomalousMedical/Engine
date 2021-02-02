using System;
using System.Collections.Generic;
using System.Text;

namespace DilligentEngine
{
    public class IShader : DilligentObject
    {
        public IShader(IntPtr objPtr) : base(objPtr)
        {
        }

        //public IPipelineState CreateGraphicsPipelineState(GraphicsPipelineStateCreateInfo PSOCreateInfo)
        //{
        //    return new IPipelineState(IRenderDevice_CreateGraphicsPipelineState(objPtr, PSOCreateInfo.ObjPtr));
        //}

        //[DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        //private static extern IntPtr IRenderDevice_CreateGraphicsPipelineState(IntPtr objPtr, IntPtr PSOCreateInfo);
    }
}
