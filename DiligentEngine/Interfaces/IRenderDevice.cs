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
using float4 = Engine.Vector4;
using float3 = Engine.Vector3;
using float2 = Engine.Vector2;
using float4x4 = Engine.Matrix4x4;
using BOOL = System.Boolean;

namespace DiligentEngine
{
    /// <summary>
    /// Render device interface
    /// </summary>
    public partial class IRenderDevice :  IObject
    {
        public IRenderDevice(IntPtr objPtr)
            : base(objPtr)
        {

        }
        /// <summary>
        /// Creates a new buffer object
        /// \param [in] BuffDesc   - Buffer description, see Diligent::BufferDesc for details.
        /// \param [in] pBuffData  - Pointer to Diligent::BufferData structure that describes
        /// initial buffer data or nullptr if no data is provided.
        /// Immutable buffers (USAGE_IMMUTABLE) must be initialized at creation time.
        /// \param [out] ppBuffer  - Address of the memory location where the pointer to the
        /// buffer interface will be stored. The function calls AddRef(),
        /// so that the new buffer will contain one reference and must be
        /// released by a call to Release().
        /// 
        /// \remarks
        /// Size of a uniform buffer (BIND_UNIFORM_BUFFER) must be multiple of 16.\n
        /// Stride of a formatted buffer will be computed automatically from the format if
        /// ElementByteStride member of buffer description is set to default value (0).
        /// </summary>
        public AutoPtr<IBuffer> CreateBuffer(BufferDesc BuffDesc, BufferData pBuffData)
        {
            var theReturnValue = 
            IRenderDevice_CreateBuffer(
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
            );
            return new AutoPtr<IBuffer>(new IBuffer(theReturnValue), false);
        }
        /// <summary>
        /// Creates a new shader object
        /// \param [in] ShaderCI  - Shader create info, see Diligent::ShaderCreateInfo for details.
        /// \param [out] ppShader - Address of the memory location where the pointer to the
        /// shader interface will be stored.
        /// The function calls AddRef(), so that the new object will contain
        /// one reference.
        /// </summary>
        public AutoPtr<IShader> CreateShader(ShaderCreateInfo ShaderCI)
        {
            var theReturnValue = 
            IRenderDevice_CreateShader(
                this.objPtr
                , ShaderCI.FilePath
                , ShaderCI.Source
                , ShaderCI.EntryPoint
                , ShaderCI.UseCombinedTextureSamplers
                , ShaderCI.CombinedSamplerSuffix
                , ShaderCI.Desc.ShaderType
                , ShaderCI.Desc.Name
                , ShaderCI.SourceLanguage
            );
            return new AutoPtr<IShader>(new IShader(theReturnValue), false);
        }
        /// <summary>
        /// Creates a new texture object
        /// \param [in] TexDesc - Texture description, see Diligent::TextureDesc for details.
        /// \param [in] pData   - Pointer to Diligent::TextureData structure that describes
        /// initial texture data or nullptr if no data is provided.
        /// Immutable textures (USAGE_IMMUTABLE) must be initialized at creation time.
        /// 
        /// \param [out] ppTexture - Address of the memory location where the pointer to the
        /// texture interface will be stored.
        /// The function calls AddRef(), so that the new object will contain
        /// one reference.
        /// \remarks
        /// To create all mip levels, set the TexDesc.MipLevels to zero.\n
        /// Multisampled resources cannot be initialzed with data when they are created. \n
        /// If initial data is provided, number of subresources must exactly match the number
        /// of subresources in the texture (which is the number of mip levels times the number of array slices.
        /// For a 3D texture, this is just the number of mip levels).
        /// For example, for a 15 x 6 x 2 2D texture array, the following array of subresources should be
        /// provided: \n
        /// 15x6, 7x3, 3x1, 1x1, 15x6, 7x3, 3x1, 1x1.\n
        /// For a 15 x 6 x 4 3D texture, the following array of subresources should be provided:\n
        /// 15x6x4, 7x3x2, 3x1x1, 1x1x1
        /// </summary>
        public AutoPtr<ITexture> CreateTexture(TextureDesc TexDesc, TextureData pData)
        {
            var theReturnValue = 
            IRenderDevice_CreateTexture(
                this.objPtr
                , TexDesc.Type
                , TexDesc.Width
                , TexDesc.Height
                , TexDesc.Format
                , TexDesc.MipLevels
                , TexDesc.SampleCount
                , TexDesc.Usage
                , TexDesc.BindFlags
                , TexDesc.CPUAccessFlags
                , TexDesc.MiscFlags
                , TexDesc.ClearValue.Format
                , TexDesc.ClearValue.Color_0
                , TexDesc.ClearValue.Color_1
                , TexDesc.ClearValue.Color_2
                , TexDesc.ClearValue.Color_3
                , TexDesc.ClearValue.DepthStencil.Depth
                , TexDesc.ClearValue.DepthStencil.Stencil
                , TexDesc.CommandQueueMask
                , TexDesc.Name
                , TextureSubResDataPassStruct.ToStruct(pData?.pSubResources)
                , pData?.pSubResources != null ? (Uint32)pData.pSubResources.Count : 0
            );
            return new AutoPtr<ITexture>(new ITexture(theReturnValue), false);
        }
        /// <summary>
        /// Creates a new graphics pipeline state object
        /// \param [in]  PSOCreateInfo   - Graphics pipeline state create info, see Diligent::GraphicsPipelineStateCreateInfo for details.
        /// \param [out] ppPipelineState - Address of the memory location where the pointer to the
        /// pipeline state interface will be stored.
        /// The function calls AddRef(), so that the new object will contain
        /// one reference.
        /// </summary>
        public AutoPtr<IPipelineState> CreateGraphicsPipelineState(GraphicsPipelineStateCreateInfo PSOCreateInfo)
        {
            var theReturnValue = 
            IRenderDevice_CreateGraphicsPipelineState(
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
                , LayoutElementPassStruct.ToStruct(PSOCreateInfo.GraphicsPipeline.InputLayout?.LayoutElements)
                , PSOCreateInfo.GraphicsPipeline.InputLayout?.LayoutElements != null ? (Uint32)PSOCreateInfo.GraphicsPipeline.InputLayout.LayoutElements.Count : 0
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
                , PSOCreateInfo.PSODesc.ResourceLayout?.Variables != null ? (Uint32)PSOCreateInfo.PSODesc.ResourceLayout.Variables.Count : 0
                , ShaderResourceVariableDescPassStruct.ToStruct(PSOCreateInfo.PSODesc.ResourceLayout?.Variables)
                , PSOCreateInfo.PSODesc.ResourceLayout?.ImmutableSamplers != null ? (Uint32)PSOCreateInfo.PSODesc.ResourceLayout.ImmutableSamplers.Count : 0
                , ImmutableSamplerDescPassStruct.ToStruct(PSOCreateInfo.PSODesc.ResourceLayout?.ImmutableSamplers)
                , PSOCreateInfo.PSODesc.Name
                , PSOCreateInfo.Flags
            );
            return new AutoPtr<IPipelineState>(new IPipelineState(theReturnValue), false);
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
        private static extern IntPtr IRenderDevice_CreateTexture(
            IntPtr objPtr
            , RESOURCE_DIMENSION TexDesc_Type
            , Uint32 TexDesc_Width
            , Uint32 TexDesc_Height
            , TEXTURE_FORMAT TexDesc_Format
            , Uint32 TexDesc_MipLevels
            , Uint32 TexDesc_SampleCount
            , USAGE TexDesc_Usage
            , BIND_FLAGS TexDesc_BindFlags
            , CPU_ACCESS_FLAGS TexDesc_CPUAccessFlags
            , MISC_TEXTURE_FLAGS TexDesc_MiscFlags
            , TEXTURE_FORMAT TexDesc_ClearValue_Format
            , Float32 TexDesc_ClearValue_Color_0
            , Float32 TexDesc_ClearValue_Color_1
            , Float32 TexDesc_ClearValue_Color_2
            , Float32 TexDesc_ClearValue_Color_3
            , Float32 TexDesc_ClearValue_DepthStencil_Depth
            , Uint8 TexDesc_ClearValue_DepthStencil_Stencil
            , Uint64 TexDesc_CommandQueueMask
            , String TexDesc_Name
            , TextureSubResDataPassStruct[] pData_pSubResources
            , Uint32 pData_NumSubresources
        );
        [DllImport(LibraryInfo.LibraryName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr IRenderDevice_CreateGraphicsPipelineState(
            IntPtr objPtr
            , [MarshalAs(UnmanagedType.I1)]Bool PSOCreateInfo_GraphicsPipeline_BlendDesc_AlphaToCoverageEnable
            , [MarshalAs(UnmanagedType.I1)]Bool PSOCreateInfo_GraphicsPipeline_BlendDesc_IndependentBlendEnable
            , Uint32 PSOCreateInfo_GraphicsPipeline_SampleMask
            , FILL_MODE PSOCreateInfo_GraphicsPipeline_RasterizerDesc_FillMode
            , CULL_MODE PSOCreateInfo_GraphicsPipeline_RasterizerDesc_CullMode
            , [MarshalAs(UnmanagedType.I1)]Bool PSOCreateInfo_GraphicsPipeline_RasterizerDesc_FrontCounterClockwise
            , [MarshalAs(UnmanagedType.I1)]Bool PSOCreateInfo_GraphicsPipeline_RasterizerDesc_DepthClipEnable
            , [MarshalAs(UnmanagedType.I1)]Bool PSOCreateInfo_GraphicsPipeline_RasterizerDesc_ScissorEnable
            , [MarshalAs(UnmanagedType.I1)]Bool PSOCreateInfo_GraphicsPipeline_RasterizerDesc_AntialiasedLineEnable
            , Int32 PSOCreateInfo_GraphicsPipeline_RasterizerDesc_DepthBias
            , Float32 PSOCreateInfo_GraphicsPipeline_RasterizerDesc_DepthBiasClamp
            , Float32 PSOCreateInfo_GraphicsPipeline_RasterizerDesc_SlopeScaledDepthBias
            , [MarshalAs(UnmanagedType.I1)]Bool PSOCreateInfo_GraphicsPipeline_DepthStencilDesc_DepthEnable
            , [MarshalAs(UnmanagedType.I1)]Bool PSOCreateInfo_GraphicsPipeline_DepthStencilDesc_DepthWriteEnable
            , COMPARISON_FUNCTION PSOCreateInfo_GraphicsPipeline_DepthStencilDesc_DepthFunc
            , [MarshalAs(UnmanagedType.I1)]Bool PSOCreateInfo_GraphicsPipeline_DepthStencilDesc_StencilEnable
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
            , ShaderResourceVariableDescPassStruct[] PSOCreateInfo_PSODesc_ResourceLayout_Variables
            , Uint32 PSOCreateInfo_PSODesc_ResourceLayout_NumImmutableSamplers
            , ImmutableSamplerDescPassStruct[] PSOCreateInfo_PSODesc_ResourceLayout_ImmutableSamplers
            , String PSOCreateInfo_PSODesc_Name
            , PSO_CREATE_FLAGS PSOCreateInfo_Flags
        );
    }
}
