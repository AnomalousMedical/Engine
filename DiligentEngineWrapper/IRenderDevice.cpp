#include "StdAfx.h"
#include "Graphics/GraphicsEngine/interface/RenderDevice.h"
#include "Color.h"
using namespace Diligent;
extern "C" _AnomalousExport IShader* IRenderDevice_CreateShader(
	IRenderDevice* objPtr
	, Char* ShaderCI_FilePath
	, Char* ShaderCI_Source
	, Char* ShaderCI_EntryPoint
	, SHADER_TYPE ShaderCI_Desc_ShaderType
	, Char* ShaderCI_Desc_Name
)
{
	ShaderCreateInfo ShaderCI;
	ShaderCI.FilePath = ShaderCI_FilePath;
	ShaderCI.Source = ShaderCI_Source;
	ShaderCI.EntryPoint = ShaderCI_EntryPoint;
	ShaderCI.Desc.ShaderType = ShaderCI_Desc_ShaderType;
	ShaderCI.Desc.Name = ShaderCI_Desc_Name;
	IShader* ppShader = nullptr;
	objPtr->CreateShader(
		ShaderCI
		, &ppShader
	);
	return ppShader;
}
