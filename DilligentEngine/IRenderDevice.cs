using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DilligentEngine
{
    public class IRenderDevice : DilligentObject
    {
        public IRenderDevice(IntPtr objPtr) : base(objPtr)
        {
        }

        public IPipelineState CreateGraphicsPipelineState(GraphicsPipelineStateCreateInfo PSOCreateInfo)
        {
            return new IPipelineState(IRenderDevice_CreateGraphicsPipelineState(objPtr, PSOCreateInfo.ObjPtr));
        }

        public IShader CreateShader(ShaderCreateInfo ShaderCI)
        {
            return new IShader(IRenderDevice_CreateShader(objPtr, ShaderCI.ObjPtr));
        }

        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr IRenderDevice_CreateGraphicsPipelineState(IntPtr objPtr, IntPtr PSOCreateInfo);

        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr IRenderDevice_CreateShader(IntPtr objPtr, IntPtr ShaderCI);
    }
}
