#include "StdAfx.h"
#include "Graphics/GraphicsEngine/interface/RenderDevice.h"
#include "Color.h"
using namespace Diligent;
extern "C" _AnomalousExport IShader* IRenderDevice_CreateShader(
	IRenderDevice* objPtr
	, Char* ShaderCI_FilePath
	, Char* ShaderCI_Source
	, Char* ShaderCI_EntryPoint
	, ShaderDesc ShaderCI_Desc
)
{
	ShaderCreateInfo ShaderCI;
	ShaderCI.FilePath = ShaderCI_FilePath;
	ShaderCI.Source = ShaderCI_Source;
	ShaderCI.EntryPoint = ShaderCI_EntryPoint;
	ShaderCI.Desc = ShaderCI_Desc;
	IShader* ppShader = nullptr;
	objPtr->CreateShader(
		ShaderCI
		, &ppShader
	);
	return ppShader;
}
