#include "StdAfx.h"
#include "Graphics/GraphicsEngine/interface/Texture.h"
using namespace Diligent;

extern "C" _AnomalousExport Uint32 ITexture_GetDesc_MipLevels(
	ITexture * objPtr
)
{
	return objPtr->GetDesc().MipLevels;
}