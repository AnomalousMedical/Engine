#include "StdAfx.h"
#include "Graphics/GraphicsEngine/interface/PipelineState.h"
using namespace Diligent;
extern "C" _AnomalousExport IShaderResourceVariable* IPipelineState_GetStaticVariableByName(
	IPipelineState* objPtr
, SHADER_TYPE ShaderType, Char* Name)
{
	return objPtr->GetStaticVariableByName(
		ShaderType
		, Name
	);
}
extern "C" _AnomalousExport IShaderResourceBinding* IPipelineState_CreateShaderResourceBinding(
	IPipelineState* objPtr
, bool InitStaticResources)
{
	IShaderResourceBinding* theReturnValue = nullptr;
	objPtr->CreateShaderResourceBinding(
		&theReturnValue
		, InitStaticResources
	);
	return theReturnValue;
}
