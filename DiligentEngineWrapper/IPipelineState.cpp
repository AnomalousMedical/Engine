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
