#include "StdAfx.h"
#include "Graphics/GraphicsEngine/interface/ShaderResourceBinding.h"
using namespace Diligent;
extern "C" _AnomalousExport IShaderResourceVariable* IShaderResourceBinding_GetVariableByName(
	IShaderResourceBinding* objPtr
, SHADER_TYPE ShaderType, char* Name)
{
	return objPtr->GetVariableByName(
		ShaderType
		, Name
	);
}
