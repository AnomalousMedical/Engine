#include "StdAfx.h"
#include "Graphics/GraphicsEngine/interface/RenderDevice.h"
#include "Color.h"
using namespace Diligent;
extern "C" _AnomalousExport IBuffer* IRenderDevice_CreateBuffer(
	IRenderDevice* objPtr
	, Uint32 BuffDesc_uiSizeInBytes
	, BIND_FLAGS BuffDesc_BindFlags
	, USAGE BuffDesc_Usage
	, CPU_ACCESS_FLAGS BuffDesc_CPUAccessFlags
	, BUFFER_MODE BuffDesc_Mode
	, Uint32 BuffDesc_ElementByteStride
	, Uint64 BuffDesc_CommandQueueMask
	, Char* BuffDesc_Name
	, void* pBuffData_pData
	, Uint32 pBuffData_DataSize
)
{
	BufferDesc BuffDesc;
	BuffDesc.uiSizeInBytes = BuffDesc_uiSizeInBytes;
	BuffDesc.BindFlags = BuffDesc_BindFlags;
	BuffDesc.Usage = BuffDesc_Usage;
	BuffDesc.CPUAccessFlags = BuffDesc_CPUAccessFlags;
	BuffDesc.Mode = BuffDesc_Mode;
	BuffDesc.ElementByteStride = BuffDesc_ElementByteStride;
	BuffDesc.CommandQueueMask = BuffDesc_CommandQueueMask;
	BuffDesc.Name = BuffDesc_Name;
	BufferData pBuffData;
	pBuffData.pData = pBuffData_pData;
	pBuffData.DataSize = pBuffData_DataSize;
	IBuffer* ppBuffer = nullptr;
	objPtr->CreateBuffer(
		BuffDesc
		, &pBuffData
		, &ppBuffer
	);
	return ppBuffer;
}
extern "C" _AnomalousExport IShader* IRenderDevice_CreateShader(
	IRenderDevice* objPtr
	, Char* ShaderCI_FilePath
	, Char* ShaderCI_Source
	, Char* ShaderCI_EntryPoint
	, bool ShaderCI_UseCombinedTextureSamplers
	, Char* ShaderCI_CombinedSamplerSuffix
	, SHADER_TYPE ShaderCI_Desc_ShaderType
	, Char* ShaderCI_Desc_Name
	, SHADER_SOURCE_LANGUAGE ShaderCI_SourceLanguage
)
{
	ShaderCreateInfo ShaderCI;
	ShaderCI.FilePath = ShaderCI_FilePath;
	ShaderCI.Source = ShaderCI_Source;
	ShaderCI.EntryPoint = ShaderCI_EntryPoint;
	ShaderCI.UseCombinedTextureSamplers = ShaderCI_UseCombinedTextureSamplers;
	ShaderCI.CombinedSamplerSuffix = ShaderCI_CombinedSamplerSuffix;
	ShaderCI.Desc.ShaderType = ShaderCI_Desc_ShaderType;
	ShaderCI.Desc.Name = ShaderCI_Desc_Name;
	ShaderCI.SourceLanguage = ShaderCI_SourceLanguage;
	IShader* ppShader = nullptr;
	objPtr->CreateShader(
		ShaderCI
		, &ppShader
	);
	return ppShader;
}
extern "C" _AnomalousExport IPipelineState* IRenderDevice_CreateGraphicsPipelineState(
	IRenderDevice* objPtr
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
	, IShader* PSOCreateInfo_pVS
	, IShader* PSOCreateInfo_pPS
	, IShader* PSOCreateInfo_pDS
	, IShader* PSOCreateInfo_pHS
	, IShader* PSOCreateInfo_pGS
	, IShader* PSOCreateInfo_pAS
	, IShader* PSOCreateInfo_pMS
	, PIPELINE_TYPE PSOCreateInfo_PSODesc_PipelineType
	, Uint32 PSOCreateInfo_PSODesc_SRBAllocationGranularity
	, Uint64 PSOCreateInfo_PSODesc_CommandQueueMask
	, SHADER_RESOURCE_VARIABLE_TYPE PSOCreateInfo_PSODesc_ResourceLayout_DefaultVariableType
	, Uint32 PSOCreateInfo_PSODesc_ResourceLayout_NumVariables
	, Uint32 PSOCreateInfo_PSODesc_ResourceLayout_NumImmutableSamplers
	, Char* PSOCreateInfo_PSODesc_Name
	, PSO_CREATE_FLAGS PSOCreateInfo_Flags
)
{
	GraphicsPipelineStateCreateInfo PSOCreateInfo;
	PSOCreateInfo.GraphicsPipeline.BlendDesc.AlphaToCoverageEnable = PSOCreateInfo_GraphicsPipeline_BlendDesc_AlphaToCoverageEnable;
	PSOCreateInfo.GraphicsPipeline.BlendDesc.IndependentBlendEnable = PSOCreateInfo_GraphicsPipeline_BlendDesc_IndependentBlendEnable;
	PSOCreateInfo.GraphicsPipeline.SampleMask = PSOCreateInfo_GraphicsPipeline_SampleMask;
	PSOCreateInfo.GraphicsPipeline.RasterizerDesc.FillMode = PSOCreateInfo_GraphicsPipeline_RasterizerDesc_FillMode;
	PSOCreateInfo.GraphicsPipeline.RasterizerDesc.CullMode = PSOCreateInfo_GraphicsPipeline_RasterizerDesc_CullMode;
	PSOCreateInfo.GraphicsPipeline.RasterizerDesc.FrontCounterClockwise = PSOCreateInfo_GraphicsPipeline_RasterizerDesc_FrontCounterClockwise;
	PSOCreateInfo.GraphicsPipeline.RasterizerDesc.DepthClipEnable = PSOCreateInfo_GraphicsPipeline_RasterizerDesc_DepthClipEnable;
	PSOCreateInfo.GraphicsPipeline.RasterizerDesc.ScissorEnable = PSOCreateInfo_GraphicsPipeline_RasterizerDesc_ScissorEnable;
	PSOCreateInfo.GraphicsPipeline.RasterizerDesc.AntialiasedLineEnable = PSOCreateInfo_GraphicsPipeline_RasterizerDesc_AntialiasedLineEnable;
	PSOCreateInfo.GraphicsPipeline.RasterizerDesc.DepthBias = PSOCreateInfo_GraphicsPipeline_RasterizerDesc_DepthBias;
	PSOCreateInfo.GraphicsPipeline.RasterizerDesc.DepthBiasClamp = PSOCreateInfo_GraphicsPipeline_RasterizerDesc_DepthBiasClamp;
	PSOCreateInfo.GraphicsPipeline.RasterizerDesc.SlopeScaledDepthBias = PSOCreateInfo_GraphicsPipeline_RasterizerDesc_SlopeScaledDepthBias;
	PSOCreateInfo.GraphicsPipeline.DepthStencilDesc.DepthEnable = PSOCreateInfo_GraphicsPipeline_DepthStencilDesc_DepthEnable;
	PSOCreateInfo.GraphicsPipeline.DepthStencilDesc.DepthWriteEnable = PSOCreateInfo_GraphicsPipeline_DepthStencilDesc_DepthWriteEnable;
	PSOCreateInfo.GraphicsPipeline.DepthStencilDesc.DepthFunc = PSOCreateInfo_GraphicsPipeline_DepthStencilDesc_DepthFunc;
	PSOCreateInfo.GraphicsPipeline.DepthStencilDesc.StencilEnable = PSOCreateInfo_GraphicsPipeline_DepthStencilDesc_StencilEnable;
	PSOCreateInfo.GraphicsPipeline.DepthStencilDesc.StencilReadMask = PSOCreateInfo_GraphicsPipeline_DepthStencilDesc_StencilReadMask;
	PSOCreateInfo.GraphicsPipeline.DepthStencilDesc.StencilWriteMask = PSOCreateInfo_GraphicsPipeline_DepthStencilDesc_StencilWriteMask;
	PSOCreateInfo.GraphicsPipeline.DepthStencilDesc.FrontFace.StencilFailOp = PSOCreateInfo_GraphicsPipeline_DepthStencilDesc_FrontFace_StencilFailOp;
	PSOCreateInfo.GraphicsPipeline.DepthStencilDesc.FrontFace.StencilDepthFailOp = PSOCreateInfo_GraphicsPipeline_DepthStencilDesc_FrontFace_StencilDepthFailOp;
	PSOCreateInfo.GraphicsPipeline.DepthStencilDesc.FrontFace.StencilPassOp = PSOCreateInfo_GraphicsPipeline_DepthStencilDesc_FrontFace_StencilPassOp;
	PSOCreateInfo.GraphicsPipeline.DepthStencilDesc.FrontFace.StencilFunc = PSOCreateInfo_GraphicsPipeline_DepthStencilDesc_FrontFace_StencilFunc;
	PSOCreateInfo.GraphicsPipeline.DepthStencilDesc.BackFace.StencilFailOp = PSOCreateInfo_GraphicsPipeline_DepthStencilDesc_BackFace_StencilFailOp;
	PSOCreateInfo.GraphicsPipeline.DepthStencilDesc.BackFace.StencilDepthFailOp = PSOCreateInfo_GraphicsPipeline_DepthStencilDesc_BackFace_StencilDepthFailOp;
	PSOCreateInfo.GraphicsPipeline.DepthStencilDesc.BackFace.StencilPassOp = PSOCreateInfo_GraphicsPipeline_DepthStencilDesc_BackFace_StencilPassOp;
	PSOCreateInfo.GraphicsPipeline.DepthStencilDesc.BackFace.StencilFunc = PSOCreateInfo_GraphicsPipeline_DepthStencilDesc_BackFace_StencilFunc;
	PSOCreateInfo.GraphicsPipeline.PrimitiveTopology = PSOCreateInfo_GraphicsPipeline_PrimitiveTopology;
	PSOCreateInfo.GraphicsPipeline.NumViewports = PSOCreateInfo_GraphicsPipeline_NumViewports;
	PSOCreateInfo.GraphicsPipeline.NumRenderTargets = PSOCreateInfo_GraphicsPipeline_NumRenderTargets;
	PSOCreateInfo.GraphicsPipeline.SubpassIndex = PSOCreateInfo_GraphicsPipeline_SubpassIndex;
	PSOCreateInfo.GraphicsPipeline.RTVFormats[0] = PSOCreateInfo_GraphicsPipeline_RTVFormats_0;
	PSOCreateInfo.GraphicsPipeline.RTVFormats[1] = PSOCreateInfo_GraphicsPipeline_RTVFormats_1;
	PSOCreateInfo.GraphicsPipeline.RTVFormats[2] = PSOCreateInfo_GraphicsPipeline_RTVFormats_2;
	PSOCreateInfo.GraphicsPipeline.RTVFormats[3] = PSOCreateInfo_GraphicsPipeline_RTVFormats_3;
	PSOCreateInfo.GraphicsPipeline.RTVFormats[4] = PSOCreateInfo_GraphicsPipeline_RTVFormats_4;
	PSOCreateInfo.GraphicsPipeline.RTVFormats[5] = PSOCreateInfo_GraphicsPipeline_RTVFormats_5;
	PSOCreateInfo.GraphicsPipeline.RTVFormats[6] = PSOCreateInfo_GraphicsPipeline_RTVFormats_6;
	PSOCreateInfo.GraphicsPipeline.RTVFormats[7] = PSOCreateInfo_GraphicsPipeline_RTVFormats_7;
	PSOCreateInfo.GraphicsPipeline.DSVFormat = PSOCreateInfo_GraphicsPipeline_DSVFormat;
	PSOCreateInfo.GraphicsPipeline.SmplDesc.Count = PSOCreateInfo_GraphicsPipeline_SmplDesc_Count;
	PSOCreateInfo.GraphicsPipeline.SmplDesc.Quality = PSOCreateInfo_GraphicsPipeline_SmplDesc_Quality;
	PSOCreateInfo.GraphicsPipeline.NodeMask = PSOCreateInfo_GraphicsPipeline_NodeMask;
	PSOCreateInfo.pVS = PSOCreateInfo_pVS;
	PSOCreateInfo.pPS = PSOCreateInfo_pPS;
	PSOCreateInfo.pDS = PSOCreateInfo_pDS;
	PSOCreateInfo.pHS = PSOCreateInfo_pHS;
	PSOCreateInfo.pGS = PSOCreateInfo_pGS;
	PSOCreateInfo.pAS = PSOCreateInfo_pAS;
	PSOCreateInfo.pMS = PSOCreateInfo_pMS;
	PSOCreateInfo.PSODesc.PipelineType = PSOCreateInfo_PSODesc_PipelineType;
	PSOCreateInfo.PSODesc.SRBAllocationGranularity = PSOCreateInfo_PSODesc_SRBAllocationGranularity;
	PSOCreateInfo.PSODesc.CommandQueueMask = PSOCreateInfo_PSODesc_CommandQueueMask;
	PSOCreateInfo.PSODesc.ResourceLayout.DefaultVariableType = PSOCreateInfo_PSODesc_ResourceLayout_DefaultVariableType;
	PSOCreateInfo.PSODesc.ResourceLayout.NumVariables = PSOCreateInfo_PSODesc_ResourceLayout_NumVariables;
	PSOCreateInfo.PSODesc.ResourceLayout.NumImmutableSamplers = PSOCreateInfo_PSODesc_ResourceLayout_NumImmutableSamplers;
	PSOCreateInfo.PSODesc.Name = PSOCreateInfo_PSODesc_Name;
	PSOCreateInfo.Flags = PSOCreateInfo_Flags;
	IPipelineState* ppPipelineState = nullptr;
	objPtr->CreateGraphicsPipelineState(
		PSOCreateInfo
		, &ppPipelineState
	);
	return ppPipelineState;
}
