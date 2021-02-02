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

        public IPipelineState CreateGraphicsPipelineState(GraphicsPipelineStateCreateInfo PSOCreateInfo,
            //All this is for testing
            ISwapChain m_pSwapChain, IShader pVS, IShader pPS)
        {
            return new IPipelineState(IRenderDevice_CreateGraphicsPipelineState(objPtr, PSOCreateInfo.ObjPtr
                //All this is for testing
                , m_pSwapChain.objPtr, pVS.objPtr, pPS.objPtr));
        }

        public IShader CreateShader(ShaderCreateInfo ShaderCI)
        {
            return new IShader(IRenderDevice_CreateShader(objPtr, ShaderCI.ObjPtr));
        }

        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr IRenderDevice_CreateGraphicsPipelineState(IntPtr objPtr, IntPtr PSOCreateInfo
            //All this is for testing
            , IntPtr m_pSwapChain, IntPtr pVS, IntPtr pPS);

        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr IRenderDevice_CreateShader(IntPtr objPtr, IntPtr ShaderCI);
    }
}
