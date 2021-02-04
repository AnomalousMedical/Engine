using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Engine;

using Uint8 = System.Byte;
using Int8 = System.SByte;
using Bool = System.Boolean;
using Uint32 = System.UInt32;
using Uint64 = System.UInt64;
using Float32 = System.Single;
using Uint16 = System.UInt16;

namespace DiligentEngine
{
    public partial class IRenderDevice :  IObject
    {
        public IRenderDevice(IntPtr objPtr)
            : base(objPtr)
        {

        }
        public IBuffer CreateBuffer(BufferDesc BuffDesc, BufferData pBuffData)
        {
            return new IBuffer(IRenderDevice_CreateBuffer(
                this.objPtr
                , BuffDesc.uiSizeInBytes
                , BuffDesc.BindFlags
                , BuffDesc.Usage
                , BuffDesc.CPUAccessFlags
                , BuffDesc.Mode
                , BuffDesc.ElementByteStride
                , BuffDesc.CommandQueueMask
                , BuffDesc.Name
                , pBuffData.pData
                , pBuffData.DataSize
            ));
        }
        public IShader CreateShader(ShaderCreateInfo ShaderCI)
        {
            return new IShader(IRenderDevice_CreateShader(
                this.objPtr
                , ShaderCI.FilePath
                , ShaderCI.Source
                , ShaderCI.EntryPoint
                , ShaderCI.UseCombinedTextureSamplers
                , ShaderCI.CombinedSamplerSuffix
                , ShaderCI.Desc.ShaderType
                , ShaderCI.Desc.Name
                , ShaderCI.SourceLanguage
            ));
        }
        public IPipelineState CreateGraphicsPipelineState(GraphicsPipelineStateCreateInfo PSOCreateInfo)
        {
            return new IPipelineState(IRenderDevice_CreateGraphicsPipelineState(
                this.objPtr
                , PSOCreateInfo.GraphicsPipeline.BlendDesc.AlphaToCoverageEnable
                , PSOCreateInfo.GraphicsPipeline.BlendDesc.IndependentBlendEnable
                , PSOCreateInfo.GraphicsPipeline.SampleMask
                , PSOCreateInfo.GraphicsPipeline.RasterizerDesc.FillMode
                , PSOCreateInfo.GraphicsPipeline.RasterizerDesc.CullMode
                , PSOCreateInfo.GraphicsPipeline.RasterizerDesc.FrontCounterClockwise
                , PSOCreateInfo.GraphicsPipeline.RasterizerDesc.DepthClipEnable
                , PSOCreateInfo.GraphicsPipeline.RasterizerDesc.ScissorEnable
                , PSOCreateInfo.GraphicsPipeline.RasterizerDesc.AntialiasedLineEnable
                , PSOCreateInfo.GraphicsPipeline.RasterizerDesc.DepthBias
                , PSOCreateInfo.GraphicsPipeline.RasterizerDesc.DepthBiasClamp
                , PSOCreateInfo.GraphicsPipeline.RasterizerDesc.SlopeScaledDepthBias
                , PSOCreateInfo.GraphicsPipeline.DepthStencilDesc.DepthEnable
                , PSOCreateInfo.GraphicsPipeline.DepthStencilDesc.DepthWriteEnable
                , PSOCreateInfo.GraphicsPipeline.DepthStencilDesc.DepthFunc
                , PSOCreateInfo.GraphicsPipeline.DepthStencilDesc.StencilEnable
                , PSOCreateInfo.GraphicsPipeline.DepthStencilDesc.StencilReadMask
                , PSOCreateInfo.GraphicsPipeline.DepthStencilDesc.StencilWriteMask
                , PSOCreateInfo.GraphicsPipeline.DepthStencilDesc.FrontFace.StencilFailOp
                , PSOCreateInfo.GraphicsPipeline.DepthStencilDesc.FrontFace.StencilDepthFailOp
                , PSOCreateInfo.GraphicsPipeline.DepthStencilDesc.FrontFace.StencilPassOp
                , PSOCreateInfo.GraphicsPipeline.DepthStencilDesc.FrontFace.StencilFunc
                , PSOCreateInfo.GraphicsPipeline.DepthStencilDesc.BackFace.StencilFailOp
                , PSOCreateInfo.GraphicsPipeline.DepthStencilDesc.BackFace.StencilDepthFailOp
                , PSOCreateInfo.GraphicsPipeline.DepthStencilDesc.BackFace.StencilPassOp
                , PSOCreateInfo.GraphicsPipeline.DepthStencilDesc.BackFace.StencilFunc
                , LayoutElementPassStruct.ToStruct(PSOCreateInfo.GraphicsPipeline.InputLayout.LayoutElements)
                , PSOCreateInfo.GraphicsPipeline.InputLayout.LayoutElements != null ? (Uint32)PSOCreateInfo.GraphicsPipeline.InputLayout.LayoutElements.Length : 0
                , PSOCreateInfo.GraphicsPipeline.PrimitiveTopology
                , PSOCreateInfo.GraphicsPipeline.NumViewports
                , PSOCreateInfo.GraphicsPipeline.NumRenderTargets
                , PSOCreateInfo.GraphicsPipeline.SubpassIndex
                , PSOCreateInfo.GraphicsPipeline.RTVFormats_0
                , PSOCreateInfo.GraphicsPipeline.RTVFormats_1
                , PSOCreateInfo.GraphicsPipeline.RTVFormats_2
                , PSOCreateInfo.GraphicsPipeline.RTVFormats_3
                , PSOCreateInfo.GraphicsPipeline.RTVFormats_4
                , PSOCreateInfo.GraphicsPipeline.RTVFormats_5
                , PSOCreateInfo.GraphicsPipeline.RTVFormats_6
                , PSOCreateInfo.GraphicsPipeline.RTVFormats_7
                , PSOCreateInfo.GraphicsPipeline.DSVFormat
                , PSOCreateInfo.GraphicsPipeline.SmplDesc.Count
                , PSOCreateInfo.GraphicsPipeline.SmplDesc.Quality
                , PSOCreateInfo.GraphicsPipeline.NodeMask
                , PSOCreateInfo.pVS?.objPtr ?? IntPtr.Zero
                , PSOCreateInfo.pPS?.objPtr ?? IntPtr.Zero
                , PSOCreateInfo.pDS?.objPtr ?? IntPtr.Zero
                , PSOCreateInfo.pHS?.objPtr ?? IntPtr.Zero
                , PSOCreateInfo.pGS?.objPtr ?? IntPtr.Zero
                , PSOCreateInfo.pAS?.objPtr ?? IntPtr.Zero
                , PSOCreateInfo.pMS?.objPtr ?? IntPtr.Zero
                , PSOCreateInfo.PSODesc.PipelineType
                , PSOCreateInfo.PSODesc.SRBAllocationGranularity
                , PSOCreateInfo.PSODesc.CommandQueueMask
                , PSOCreateInfo.PSODesc.ResourceLayout.DefaultVariableType
                , PSOCreateInfo.PSODesc.ResourceLayout.NumVariables
                , PSOCreateInfo.PSODesc.ResourceLayout.NumImmutableSamplers
                , PSOCreateInfo.PSODesc.Name
                , PSOCreateInfo.Flags
            ));
        }


        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr IRenderDevice_CreateBuffer(
            IntPtr objPtr
            , Uint32 BuffDesc_uiSizeInBytes
            , BIND_FLAGS BuffDesc_BindFlags
            , USAGE BuffDesc_Usage
            , CPU_ACCESS_FLAGS BuffDesc_CPUAccessFlags
            , BUFFER_MODE BuffDesc_Mode
            , Uint32 BuffDesc_ElementByteStride
            , Uint64 BuffDesc_CommandQueueMask
            , String BuffDesc_Name
            , IntPtr pBuffData_pData
            , Uint32 pBuffData_DataSize
        );
        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr IRenderDevice_CreateShader(
            IntPtr objPtr
            , String ShaderCI_FilePath
            , String ShaderCI_Source
            , String ShaderCI_EntryPoint
            , [MarshalAs(UnmanagedType.I1)]bool ShaderCI_UseCombinedTextureSamplers
            , String ShaderCI_CombinedSamplerSuffix
            , SHADER_TYPE ShaderCI_Desc_ShaderType
            , String ShaderCI_Desc_Name
            , SHADER_SOURCE_LANGUAGE ShaderCI_SourceLanguage
        );
        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr IRenderDevice_CreateGraphicsPipelineState(
            IntPtr objPtr
            , Bool PSOCreateInfo_GraphicsPipeline_BlendDesc_AlphaToCoverageEnable
            , Bool PSOCreateInfo_GraphicsPipeline_BlendDesc_IndependentBlendEnable
            , Uint32 PSOCreateInfo_GraphicsPipeline_SampleMask
            , FILL_MODE PSOCreateInfo_GraphicsPipeline_RasterizerDesc_FillMode
            , CULL_MODE PSOCreateInfo_GraphicsPipeline_RasterizerDesc_CullMode
            , Bool PSOCreateInfo_GraphicsPipeline_RasterizerDesc_FrontCounterClockwise
            , Bool PSOCreateInfo_GraphicsPipeline_RasterizerDesc_DepthClipEnable
            , Bool PSOCreateInfo_GraphicsPipeline_RasterizerDesc_ScissorEnable
            , Bool PSOCreateInfo_GraphicsPipeline_RasterizerDesc_AntialiasedLineEnable
            , Int32 PSOCreateInfo_GraphicsPipeline_RasterizerDesc_DepthBias
            , Float32 PSOCreateInfo_GraphicsPipeline_RasterizerDesc_DepthBiasClamp
            , Float32 PSOCreateInfo_GraphicsPipeline_RasterizerDesc_SlopeScaledDepthBias
            , Bool PSOCreateInfo_GraphicsPipeline_DepthStencilDesc_DepthEnable
            , Bool PSOCreateInfo_GraphicsPipeline_DepthStencilDesc_DepthWriteEnable
            , COMPARISON_FUNCTION PSOCreateInfo_GraphicsPipeline_DepthStencilDesc_DepthFunc
            , Bool PSOCreateInfo_GraphicsPipeline_DepthStencilDesc_StencilEnable
            , Uint8 PSOCreateInfo_GraphicsPipeline_DepthStencilDesc_StencilReadMask
            , Uint8 PSOCreateInfo_GraphicsPipeline_DepthStencilDesc_StencilWriteMask
            , STENCIL_OP PSOCreateInfo_GraphicsPipeline_DepthStencilDesc_FrontFace_StencilFailOp
            , STENCIL_OP PSOCreateInfo_GraphicsPipeline_DepthStencilDesc_FrontFace_StencilDepthFailOp
            , STENCIL_OP PSOCreateInfo_GraphicsPipeline_DepthStencilDesc_FrontFace_StencilPassOp
            , COMPARISON_FUNCTION PSOCreateInfo_GraphicsPipeline_DepthStencilDesc_FrontFace_StencilFunc
            , STENCIL_OP PSOCreateInfo_GraphicsPipeline_DepthStencilDesc_BackFace_StencilFailOp
            , STENCIL_OP PSOCreateInfo_GraphicsPipeline_DepthStencilDesc_BackFace_StencilDepthFailOp
            , STENCIL_OP PSOCreateInfo_GraphicsPipeline_DepthStencilDesc_BackFace_StencilPassOp
            , COMPARISON_FUNCTION PSOCreateInfo_GraphicsPipeline_DepthStencilDesc_BackFace_StencilFunc
            , LayoutElementPassStruct[] PSOCreateInfo_GraphicsPipeline_InputLayout_LayoutElements
            , Uint32 PSOCreateInfo_GraphicsPipeline_InputLayout_NumElements
            , PRIMITIVE_TOPOLOGY PSOCreateInfo_GraphicsPipeline_PrimitiveTopology
            , Uint8 PSOCreateInfo_GraphicsPipeline_NumViewports
            , Uint8 PSOCreateInfo_GraphicsPipeline_NumRenderTargets
            , Uint8 PSOCreateInfo_GraphicsPipeline_SubpassIndex
            , TEXTURE_FORMAT PSOCreateInfo_GraphicsPipeline_RTVFormats_0
            , TEXTURE_FORMAT PSOCreateInfo_GraphicsPipeline_RTVFormats_1
            , TEXTURE_FORMAT PSOCreateInfo_GraphicsPipeline_RTVFormats_2
            , TEXTURE_FORMAT PSOCreateInfo_GraphicsPipeline_RTVFormats_3
            , TEXTURE_FORMAT PSOCreateInfo_GraphicsPipeline_RTVFormats_4
            , TEXTURE_FORMAT PSOCreateInfo_GraphicsPipeline_RTVFormats_5
            , TEXTURE_FORMAT PSOCreateInfo_GraphicsPipeline_RTVFormats_6
            , TEXTURE_FORMAT PSOCreateInfo_GraphicsPipeline_RTVFormats_7
            , TEXTURE_FORMAT PSOCreateInfo_GraphicsPipeline_DSVFormat
            , Uint8 PSOCreateInfo_GraphicsPipeline_SmplDesc_Count
            , Uint8 PSOCreateInfo_GraphicsPipeline_SmplDesc_Quality
            , Uint32 PSOCreateInfo_GraphicsPipeline_NodeMask
            , IntPtr PSOCreateInfo_pVS
            , IntPtr PSOCreateInfo_pPS
            , IntPtr PSOCreateInfo_pDS
            , IntPtr PSOCreateInfo_pHS
            , IntPtr PSOCreateInfo_pGS
            , IntPtr PSOCreateInfo_pAS
            , IntPtr PSOCreateInfo_pMS
            , PIPELINE_TYPE PSOCreateInfo_PSODesc_PipelineType
            , Uint32 PSOCreateInfo_PSODesc_SRBAllocationGranularity
            , Uint64 PSOCreateInfo_PSODesc_CommandQueueMask
            , SHADER_RESOURCE_VARIABLE_TYPE PSOCreateInfo_PSODesc_ResourceLayout_DefaultVariableType
            , Uint32 PSOCreateInfo_PSODesc_ResourceLayout_NumVariables
            , Uint32 PSOCreateInfo_PSODesc_ResourceLayout_NumImmutableSamplers
            , String PSOCreateInfo_PSODesc_Name
            , PSO_CREATE_FLAGS PSOCreateInfo_Flags
        );
    }
}
