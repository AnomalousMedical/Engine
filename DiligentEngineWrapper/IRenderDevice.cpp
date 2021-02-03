#include "StdAfx.h"
#include "Graphics/GraphicsEngine/interface/RenderDevice.h"
#include "Color.h"
using namespace Diligent;
extern "C" _AnomalousExport IShader* IRenderDevice_CreateShader(IRenderDevice* objPtr, ShaderCreateInfo ShaderCI)
{
	IShader* ppShader = nullptr;
	objPtr->CreateShader(
		ShaderCI
		, &ppShader
	);
	return ppShader;
}
