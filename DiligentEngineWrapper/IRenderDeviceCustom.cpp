#include "StdAfx.h"
#include "Graphics/GraphicsEngine/interface/RenderDevice.h"
#include "Graphics/GraphicsEngine/interface/Shader.h"
#include "Graphics/GraphicsTools/interface/ShaderMacroHelper.hpp"
#include "Color.h"
#include "MacroPassStruct.h";
#include "NDCAttribs.PassStruct.h"
using namespace Diligent;
extern "C" _AnomalousExport IBuffer * IRenderDevice_CreateBuffer_Null_Data(
	IRenderDevice * objPtr
	, Uint32 BuffDesc_uiSizeInBytes
	, BIND_FLAGS BuffDesc_BindFlags
	, USAGE BuffDesc_Usage
	, CPU_ACCESS_FLAGS BuffDesc_CPUAccessFlags
	, BUFFER_MODE BuffDesc_Mode
	, Uint32 BuffDesc_ElementByteStride
	, Uint64 BuffDesc_CommandQueueMask
	, Char * BuffDesc_Name
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
	IBuffer* ppBuffer = nullptr;
	objPtr->CreateBuffer(
		BuffDesc
		, nullptr
		, &ppBuffer
	);
	return ppBuffer;
}
extern "C" _AnomalousExport IShader * IRenderDevice_CreateShader_Macros(
	IRenderDevice * objPtr
	, Char * ShaderCI_FilePath
	, Char * ShaderCI_Source
	, Char * ShaderCI_EntryPoint
	, bool ShaderCI_UseCombinedTextureSamplers
	, Char * ShaderCI_CombinedSamplerSuffix
	, SHADER_TYPE ShaderCI_Desc_ShaderType
	, Char * ShaderCI_Desc_Name
	, SHADER_SOURCE_LANGUAGE ShaderCI_SourceLanguage
	, MacroPassStruct* macros
	, Uint32 macrosCount
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

	ShaderMacroHelper Macros;
	for (Uint32 i = 0; i < macrosCount; ++i)
	{
		MacroPassStruct& macro = macros[i];
		Macros.AddShaderMacro(macro.name, macro.definition);
	}
	ShaderCI.Macros = Macros;

	IShader* theReturnValue = nullptr;
	objPtr->CreateShader(
		ShaderCI
		, &theReturnValue
	);
	return theReturnValue;
}

extern "C" _AnomalousExport NDCAttribsPassStruct IRenderDevice_GetDeviceCaps_GetNDCAttribs(IRenderDevice * objPtr)
{
	auto attribs = objPtr->GetDeviceCaps().GetNDCAttribs();
	NDCAttribsPassStruct result;
	result.MinZ = attribs.MinZ;
	result.YtoVScale = attribs.YtoVScale;
	result.ZtoDepthScale = attribs.ZtoDepthScale;
	return result;
}

extern "C" _AnomalousExport Uint32 IRenderDevice_DeviceProperties_MaxRayTracingRecursionDepth(IRenderDevice * objPtr)
{
	return objPtr->GetDeviceProperties().MaxRayTracingRecursionDepth;
}